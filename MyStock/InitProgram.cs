using MyStock.BLL;
using MyStock.Common;
using MyStock.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStock
{
    public class InitProgram
    {
        public InitProgram()
        {
            InitStaticValue();
            //SqlServer.ImportCsvFolder("C:\\");
            //Jobs
            new JobManager().ExecuteAllJobs();
        }
        private void InitStaticValue()
        {
            var dt = SqlServer.GetData("select ID,MaChungKhoan from CoPhieu");
            foreach (DataRow item in dt.Rows)
            {
                var key = long.Parse(item["ID"].ToString());
                var value = item["MaChungKhoan"].ToString();
                StaticValue.dicCoPhieu.Add(key,value);
            }
        }
    }
}
