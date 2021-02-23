using HtmlAgilityPack;
using MyStock.BLL;
using MyStock.Common;
using MyStock.DAL;
using MyStock.Models;
using PuppeteerSharp;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MyStock.Jobs
{
    public class CloseSession_ExecutionJob : Job
    {
        public override string GetName()
        {
            return this.GetType().Name;
        }

        /// <summary>
        /// Execute the Job itself. Just print a message.
        /// </summary>
        public override async Task DoJob()
        {
            var dayOfWeek = DateTime.Now.DayOfWeek;
            var hour = DateTime.Now.Hour;
            var minute = DateTime.Now.Minute;
            if (dayOfWeek == DayOfWeek.Sunday
                || dayOfWeek == DayOfWeek.Saturday
                || hour != 15
                )
                return;
            var isExists = SqlServer.CheckExists($"SELECT TOP 1 1 FROM ChotGiaDongCua WHERE NgayGiaoDich = CONVERT(DATE, '{DateTime.Now.Date}', 103)");
            if (isExists)
                return;
            var dtInsert = SqlServer.GetData($"SELECT A1.ID,A0.TC,A0.GIA,A0.CAO,A0.THAP,A0.[TRAN],A0.SAN,A0.TONGKL FROM LIGHTNING A0 INNER JOIN COPHIEU A1 ON A1.MACHUNGKHOAN = A0.MACK WHERE CONVERT(DATE, A0.UPDATE_TIME, 103) = CONVERT(DATE, '{DateTime.Now.Date}', 103)");
            foreach (DataRow item in dtInsert.Rows)
            {
                var Id = SqlServer.GetMaxIndex("SELECT MAX(ID) FROM ChotGiaDongCua");
                var model = new ChotGiaDongCuaModel { Id = Id, NgayGiaoDich = DateTime.Now.Date };
                model.IdCoPhieu = long.Parse(item["ID"].ToString());
                model.GiaThamChieu = float.Parse(item["TC"].ToString());
                model.GiaMoCua = 0;
                model.GiaDongCua = float.Parse(item["GIA"].ToString());
                model.GiaCaoNhat = float.Parse(item["CAO"].ToString());
                model.GiaThapNhat = float.Parse(item["THAP"].ToString());
                model.GiaTran = float.Parse(item["TRAN"].ToString());
                model.GiaSan = float.Parse(item["SAN"].ToString());
                model.KhoiLuongGiaoDich = long.Parse(item["TONGKL"].ToString());
                var query = "INSERT INTO [dbo].[ChotGiaDongCua]		"
                                + "           ([Id]                         "
                                + "           ,[IdCoPhieu]                  "
                                + "           ,[NgayGiaoDich]               "
                                + "           ,[GiaThamChieu]               "
                                + "           ,[GiaMoCua]                   "
                                + "           ,[GiaDongCua]                 "
                                + "           ,[GiaCaoNhat]                 "
                                + "           ,[GiaThapNhat]                "
                                + "           ,[GiaTran]                    "
                                + "           ,[GiaSan]                     "
                                + "           ,[KhoiLuongGiaoDich])         "
                                + "     VALUES                              "
                                + $"           ('{model.Id}'                "
                                + $"           ,'{model.IdCoPhieu}'         "
                                + $"           ,CONVERT(DATE, '{model.NgayGiaoDich}', 103) "
                                + $"           ,'{model.GiaThamChieu}'       "
                                + $"           ,'{model.GiaMoCua}'           "
                                + $"           ,'{model.GiaDongCua}'         "
                                + $"           ,'{model.GiaCaoNhat}'         "
                                + $"           ,'{model.GiaThapNhat}'        "
                                + $"           ,'{model.GiaTran}'            "
                                + $"           ,'{model.GiaSan}'             "
                                + $"           ,'{model.KhoiLuongGiaoDich}')";
                SqlServer.Execute(query);
            }
        }

        /// <summary>
        /// Determines this job is not repeatable.
        /// </summary>
        /// <returns>Returns false because this job is not repeatable.</returns>
        public override bool IsRepeatable()
        {
            return true;
        }

        /// <summary>
        /// In case this method is executed NotImplementedException is thrown
        /// because this method is not to to be used. This method is never used
        /// because it serves the purpose of stating the interval of which the job
        /// will be executed repeatedly. Since this job is a single-execution one,
        /// this method is rendered useless.
        /// </summary>
        /// <returns>Returns nothing because this method is not to be used.</returns>
        public override int GetRepetitionIntervalTime()
        {
            return 900000;
        }
    }
}
