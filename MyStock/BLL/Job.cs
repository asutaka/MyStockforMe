using HtmlAgilityPack;
using MyStock.DAL;
using MyStock.Models;
using PuppeteerSharp;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace MyStock.BLL
{
    public abstract class Job
    {
        /// <summary>
        /// Execute the Job itself, one ore repeatedly, depending on
        /// the job implementation.
        /// </summary>
        public void ExecuteJob()
        {
            if (IsRepeatable())
            {
                // execute the job in intervals determined by the methd
                // GetRepetionIntervalTime()
                while (true)
                {
                    DoJob().GetAwaiter().GetResult();
                    Thread.Sleep(GetRepetitionIntervalTime());
                }
            }
            // since there is no repetetion, simply execute the job
            else
            {
                DoJob().GetAwaiter().GetResult();
            }
        }

        private LightningModel DataMap(HtmlDocument htmlDoc, long ID, string MaCK)
        {
            var model = new LightningModel { Id = ID, MaCK = MaCK, Update_Time = DateTime.Now };
            var TC = htmlDoc.DocumentNode.SelectSingleNode($"//*[@id=\"{MaCK}ref\"]")?.InnerText.Replace(",", "");
            model.TC = string.IsNullOrWhiteSpace(TC) ? 0 : float.Parse(TC);

            var Tran = htmlDoc.DocumentNode.SelectSingleNode($"//*[@id=\"{MaCK}ceil\"]")?.InnerText.Replace(",", "");
            model.Tran = string.IsNullOrWhiteSpace(Tran) ? 0 : float.Parse(Tran);

            var San = htmlDoc.DocumentNode.SelectSingleNode($"//*[@id=\"{MaCK}floor\"]")?.InnerText.Replace(",", "");
            model.San = string.IsNullOrWhiteSpace(San) ? 0 : float.Parse(San);

            var TongKL = htmlDoc.DocumentNode.SelectSingleNode($"//*[@id=\"{MaCK}tvol\"]")?.InnerText.Replace(",", "");
            model.TongKL = string.IsNullOrWhiteSpace(TongKL) ? 0 : long.Parse(TongKL);

            var TongGT = htmlDoc.DocumentNode.SelectSingleNode($"//*[@id=\"{MaCK}tval\"]")?.InnerText.Replace(",", "");
            model.TongGT = string.IsNullOrWhiteSpace(TongGT) ? 0 : float.Parse(TongGT);

            var Gia3_Mua = htmlDoc.DocumentNode.SelectSingleNode($"//*[@id=\"{MaCK}bP3\"]")?.InnerText.Replace(",", "");
            model.Gia3_Mua = string.IsNullOrWhiteSpace(Gia3_Mua) ? 0 : float.Parse(Gia3_Mua);

            var KL3_Mua = htmlDoc.DocumentNode.SelectSingleNode($"//*[@id=\"{MaCK}bV3\"]")?.InnerText.Replace(",", "");
            model.KL3_Mua = string.IsNullOrWhiteSpace(KL3_Mua) ? 0 : long.Parse(KL3_Mua);

            var Gia2_Mua = htmlDoc.DocumentNode.SelectSingleNode($"//*[@id=\"{MaCK}bP2\"]")?.InnerText.Replace(",", "");
            model.Gia2_Mua = string.IsNullOrWhiteSpace(Gia2_Mua) ? 0 : float.Parse(Gia2_Mua);

            var KL2_Mua = htmlDoc.DocumentNode.SelectSingleNode($"//*[@id=\"{MaCK}bV2\"]")?.InnerText.Replace(",", "");
            model.KL2_Mua = string.IsNullOrWhiteSpace(KL2_Mua) ? 0 : long.Parse(KL2_Mua);

            var Gia1_Mua = htmlDoc.DocumentNode.SelectSingleNode($"//*[@id=\"{MaCK}bP1\"]")?.InnerText.Replace(",", "");
            model.Gia1_Mua = string.IsNullOrWhiteSpace(Gia1_Mua) ? 0 : float.Parse(Gia1_Mua);

            var KL1_Mua = htmlDoc.DocumentNode.SelectSingleNode($"//*[@id=\"{MaCK}bV1\"]")?.InnerText.Replace(",", "");
            model.KL1_Mua = string.IsNullOrWhiteSpace(KL1_Mua) ? 0 : long.Parse(KL1_Mua);

            var Gia = htmlDoc.DocumentNode.SelectSingleNode($"//*[@id=\"{MaCK}matchP\"]")?.InnerText.Replace(",", "");
            model.Gia = string.IsNullOrWhiteSpace(Gia) ? 0 : float.Parse(Gia);

            var KL = htmlDoc.DocumentNode.SelectSingleNode($"//*[@id=\"{MaCK}matchV\"]")?.InnerText.Replace(",", "");
            model.KL = string.IsNullOrWhiteSpace(KL) ? 0 : long.Parse(KL);

            var ChenhLech = htmlDoc.DocumentNode.SelectSingleNode($"//*[@id=\"{MaCK}change\"]")?.InnerText.Replace(",", "");
            model.ChenhLech = string.IsNullOrWhiteSpace(ChenhLech) ? 0 : float.Parse(ChenhLech);

            var ChenhLechPhanTram = htmlDoc.DocumentNode.SelectSingleNode($"//*[@id=\"{MaCK}percent\"]")?.InnerText.Replace(",", "").Replace("%", "");
            model.ChenhLechPhanTram = string.IsNullOrWhiteSpace(ChenhLechPhanTram) ? 0 : float.Parse(ChenhLechPhanTram);

            var Gia1_Ban = htmlDoc.DocumentNode.SelectSingleNode($"//*[@id=\"{MaCK}oP1\"]")?.InnerText.Replace(",", "");
            model.Gia1_Ban = string.IsNullOrWhiteSpace(Gia1_Ban) ? 0 : float.Parse(Gia1_Ban);

            var KL1_Ban = htmlDoc.DocumentNode.SelectSingleNode($"//*[@id=\"{MaCK}oV1\"]")?.InnerText.Replace(",", "");
            model.KL1_Ban = string.IsNullOrWhiteSpace(KL1_Ban) ? 0 : long.Parse(KL1_Ban);

            var Gia2_Ban = htmlDoc.DocumentNode.SelectSingleNode($"//*[@id=\"{MaCK}oP2\"]")?.InnerText.Replace(",", "");
            model.Gia2_Ban = string.IsNullOrWhiteSpace(Gia2_Ban) ? 0 : float.Parse(Gia2_Ban);

            var KL2_Ban = htmlDoc.DocumentNode.SelectSingleNode($"//*[@id=\"{MaCK}oV2\"]")?.InnerText.Replace(",", "");
            model.KL2_Ban = string.IsNullOrWhiteSpace(KL2_Ban) ? 0 : long.Parse(KL2_Ban);

            var Gia3_Ban = htmlDoc.DocumentNode.SelectSingleNode($"//*[@id=\"{MaCK}oP3\"]")?.InnerText.Replace(",", "");
            model.Gia3_Ban = string.IsNullOrWhiteSpace(Gia3_Ban) ? 0 : float.Parse(Gia3_Ban);

            var KL3_Ban = htmlDoc.DocumentNode.SelectSingleNode($"//*[@id=\"{MaCK}oV3\"]")?.InnerText.Replace(",", "");
            model.KL3_Ban = string.IsNullOrWhiteSpace(KL3_Ban) ? 0 : long.Parse(KL3_Ban);

            var Cao = htmlDoc.DocumentNode.SelectSingleNode($"//*[@id=\"{MaCK}highP\"]")?.InnerText.Replace(",", "");
            model.Cao = string.IsNullOrWhiteSpace(Cao) ? 0 : float.Parse(Cao);

            var TB = htmlDoc.DocumentNode.SelectSingleNode($"//*[@id=\"{MaCK}avgP\"]")?.InnerText.Replace(",", "");
            model.TB = string.IsNullOrWhiteSpace(TB) ? 0 : float.Parse(TB);

            var Thap = htmlDoc.DocumentNode.SelectSingleNode($"//*[@id=\"{MaCK}lowP\"]")?.InnerText.Replace(",", "");
            model.Thap = string.IsNullOrWhiteSpace(Thap) ? 0 : float.Parse(Thap);

            var Mua_Du = htmlDoc.DocumentNode.SelectSingleNode($"//*[@id=\"{MaCK}bV4\"]")?.InnerText.Replace(",", "");
            model.Mua_Du = string.IsNullOrWhiteSpace(Mua_Du) ? 0 : long.Parse(Mua_Du);

            var Ban_Du = htmlDoc.DocumentNode.SelectSingleNode($"//*[@id=\"{MaCK}sV4\"]")?.InnerText.Replace(",", "");
            model.Ban_Du = string.IsNullOrWhiteSpace(Ban_Du) ? 0 : long.Parse(Ban_Du);

            var Mua_DTNN = htmlDoc.DocumentNode.SelectSingleNode($"//*[@id=\"{MaCK}foreignB\"]")?.InnerText.Replace(",", "");
            model.Mua_DTNN = string.IsNullOrWhiteSpace(Mua_DTNN) ? 0 : long.Parse(Mua_DTNN);

            var Ban_DTNN = htmlDoc.DocumentNode.SelectSingleNode($"//*[@id=\"{MaCK}foreignS\"]")?.InnerText.Replace(",", "");
            model.Ban_DTNN = string.IsNullOrWhiteSpace(Ban_DTNN) ? 0 : long.Parse(Ban_DTNN);

            var Du_DTNN = htmlDoc.DocumentNode.SelectSingleNode($"//*[@id=\"{MaCK}room\"]")?.InnerText.Replace(",", "");
            model.Du_DTNN = string.IsNullOrWhiteSpace(Du_DTNN) ? 0 : long.Parse(Du_DTNN);
            return model;
        }
        public async Task CallJob(long idSan, string link)
        {
            var dayOfWeek = DateTime.Now.DayOfWeek;
            var hour = DateTime.Now.Hour;
            if (dayOfWeek == DayOfWeek.Sunday
                || dayOfWeek == DayOfWeek.Saturday
                || hour < 9
                || hour > 15
                )
                return;

            Browser browser;
            new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision).GetAwaiter().GetResult();
            browser = Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                Timeout = 0,
                Args = new[] { "--no-sandbox" }
            }).GetAwaiter().GetResult();

            try
            {
                Page _page = await browser.NewPageAsync();
                await _page.SetViewportAsync(ViewPortOptions.Default);
                _page.DefaultTimeout = 0;
                await _page.GoToAsync(link, WaitUntilNavigation.Networkidle2);
                var html = await _page.GetContentAsync();
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                var dtInsert = SqlServer.GetData($"SELECT ID,MACHUNGKHOAN FROM COPHIEU WHERE NOT EXISTS(SELECT TOP 1 1 FROM LIGHTNING WHERE LIGHTNING.MACK = COPHIEU.MACHUNGKHOAN) AND IDSAN = {idSan}");
                foreach (DataRow item in dtInsert.Rows)
                {
                    var ID = long.Parse(item[0].ToString());
                    var MaCK = item[1].ToString();
                    var urlXPath = $"//*[@id=\"{MaCK}\"]";
                    var nodeValue = htmlDoc.DocumentNode.SelectSingleNode(urlXPath);
                    if (nodeValue == null)
                        continue;
                    var model = DataMap(htmlDoc, ID, MaCK);
                    var query = $"INSERT INTO [dbo].[Lightning]				"
                                + "           ([Id]                         "
                                + "           ,[MaCK]                       "
                                + "           ,[TC]                         "
                                + "           ,[Tran]                       "
                                + "           ,[San]                        "
                                + "           ,[TongKL]                     "
                                + "           ,[TongGT]                     "
                                + "           ,[Gia3_Mua]                   "
                                + "           ,[KL3_Mua]                    "
                                + "           ,[Gia2_Mua]                   "
                                + "           ,[KL2_Mua]                    "
                                + "           ,[Gia1_Mua]                   "
                                + "           ,[KL1_Mua]                    "
                                + "           ,[Gia]                        "
                                + "           ,[KL]                         "
                                + "           ,[ChenhLech]                  "
                                + "           ,[ChenhLechPhanTram]          "
                                + "           ,[Gia1_Ban]                   "
                                + "           ,[KL1_Ban]                    "
                                + "           ,[Gia2_Ban]                   "
                                + "           ,[KL2_Ban]                    "
                                + "           ,[Gia3_Ban]                   "
                                + "           ,[KL3_Ban]                    "
                                + "           ,[Cao]                        "
                                + "           ,[TB]                         "
                                + "           ,[Thap]                       "
                                + "           ,[Mua_Du]                     "
                                + "           ,[Ban_Du]                     "
                                + "           ,[Mua_DTNN]                   "
                                + "           ,[Ban_DTNN]                   "
                                + "           ,[Du_DTNN]                    "
                                + "           ,[Update_Time])               "
                                + "     VALUES                              "
                                + $"           ('{model.Id}'                "
                                + $"           ,'{model.MaCK}'              "
                                + $"           ,'{model.TC}'                "
                                + $"           ,'{model.Tran}'              "
                                + $"           ,'{model.San}'               "
                                + $"           ,'{model.TongKL}'            "
                                + $"           ,'{model.TongGT}'            "
                                + $"           ,'{model.Gia3_Mua}'          "
                                + $"           ,'{model.KL3_Mua}'           "
                                + $"           ,'{model.Gia2_Mua}'          "
                                + $"           ,'{model.KL2_Mua}'           "
                                + $"           ,'{model.Gia1_Mua}'          "
                                + $"           ,'{model.KL1_Mua}'           "
                                + $"           ,'{model.Gia}'               "
                                + $"           ,'{model.KL}'                "
                                + $"           ,'{model.ChenhLech}'         "
                                + $"           ,'{model.ChenhLechPhanTram}' "
                                + $"           ,'{model.Gia1_Ban}'          "
                                + $"           ,'{model.KL1_Ban}'           "
                                + $"           ,'{model.Gia2_Ban}'          "
                                + $"           ,'{model.KL2_Ban}'           "
                                + $"           ,'{model.Gia3_Ban}'          "
                                + $"           ,'{model.KL3_Ban}'           "
                                + $"           ,'{model.Cao}'               "
                                + $"           ,'{model.TB}'                "
                                + $"           ,'{model.Thap}'              "
                                + $"           ,'{model.Mua_Du}'            "
                                + $"           ,'{model.Ban_Du}'            "
                                + $"           ,'{model.Mua_DTNN}'          "
                                + $"           ,'{model.Ban_DTNN}'          "
                                + $"           ,'{model.Du_DTNN}'           "
                                + $"           ,CONVERT(DATETIME, '{model.Update_Time}', 103))      ";
                    SqlServer.Execute(query);
                }

                var dtUpdate = SqlServer.GetData($"SELECT A0.ID,A0.MACHUNGKHOAN FROM COPHIEU A0 INNER JOIN LIGHTNING A1 ON A0.MACHUNGKHOAN = A1.MACK WHERE IDSAN = {idSan}");
                foreach (DataRow item in dtUpdate.Rows)
                {
                    var ID = long.Parse(item[0].ToString());
                    var MaCK = item[1].ToString();
                    var urlXPath = $"//*[@id=\"{MaCK}\"]";
                    var nodeValue = htmlDoc.DocumentNode.SelectSingleNode(urlXPath);
                    if (nodeValue == null)
                        continue;
                    var model = DataMap(htmlDoc, ID, MaCK);
                    var query = "UPDATE [dbo].[Lightning]									"
                                + $"   SET [MaCK] = '{model.MaCK}'					        "
                                + $"      ,[TC] = '{model.TC}'								"
                                + $"      ,[Tran] = '{model.Tran}'							"
                                + $"      ,[San] = '{model.San}'							"
                                + $"      ,[TongKL] = '{model.TongKL}'					    "
                                + $"      ,[TongGT] = '{model.TongGT}'						"
                                + $"      ,[Gia3_Mua] = '{model.Gia3_Mua}'					"
                                + $"      ,[KL3_Mua] = '{model.KL3_Mua}'					"
                                + $"      ,[Gia2_Mua] = '{model.Gia2_Mua}'					"
                                + $"      ,[KL2_Mua] = '{model.KL2_Mua}'					"
                                + $"      ,[Gia1_Mua] = '{model.Gia1_Mua}'					"
                                + $"      ,[KL1_Mua] = '{model.KL1_Mua}'					"
                                + $"      ,[Gia] = '{model.Gia}'							"
                                + $"      ,[KL] = '{model.KL}'								"
                                + $"      ,[ChenhLech] = '{model.ChenhLech}'				"
                                + $"      ,[ChenhLechPhanTram] = '{model.ChenhLechPhanTram}'"
                                + $"      ,[Gia1_Ban] = '{model.Gia1_Ban}'				    "
                                + $"      ,[KL1_Ban] = '{model.KL1_Ban}'					"
                                + $"      ,[Gia2_Ban] = '{model.Gia2_Ban}'					"
                                + $"      ,[KL2_Ban] = '{model.KL2_Ban}'					"
                                + $"      ,[Gia3_Ban] = '{model.Gia3_Ban}'				    "
                                + $"      ,[KL3_Ban] = '{model.KL3_Ban}'					"
                                + $"      ,[Cao] = '{model.Cao}'							"
                                + $"      ,[TB] = '{model.TB}'								"
                                + $"      ,[Thap] = '{model.Thap}'							"
                                + $"      ,[Mua_Du] = '{model.Mua_Du}'					    "
                                + $"      ,[Ban_Du] = '{model.Ban_Du}'					    "
                                + $"      ,[Mua_DTNN] = '{model.Mua_DTNN}'					"
                                + $"      ,[Ban_DTNN] = '{model.Ban_DTNN}'					"
                                + $"      ,[Du_DTNN] = '{model.Du_DTNN}'					"
                                + $"      ,[Update_Time] = CONVERT(DATETIME, '{model.Update_Time}', 103)"
                                + $" WHERE Id = '{model.Id}'								";
                    SqlServer.Execute(query);
                }
                await _page.CloseAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                browser.Dispose();
            }
        }

        /// <summary>
        /// If this method is overriden, on can get within the job
        /// parameters set just before the job is started. In this
        /// situation the application is running and the use may have
        /// access to resources which he/she has not during the thread
        /// execution. For instance, in a web application, the user has
        /// no access to the application context, when the thread is running.
        /// Note that this method must not be overriden. It is optional.
        /// </summary>
        /// <returns>Parameters to be used in the job.</returns>
        public virtual Object GetParameters()
        {
            return null;
        }

        /// <summary>
        /// Get the Job´s Name. This name uniquely identifies the Job.
        /// </summary>
        /// <returns>Job´s name.</returns>
        public abstract String GetName();

        /// <summary>
        /// The job to be executed.
        /// </summary>
        public abstract Task DoJob();

        /// <summary>
        /// Determines whether a Job is to be repeated after a
        /// certain amount of time.
        /// </summary>
        /// <returns>True in case the Job is to be repeated, false otherwise.</returns>
        public abstract bool IsRepeatable();

        /// <summary>
        /// The amount of time, in milliseconds, which the Job has to wait until it is started
        /// over. This method is only useful if IJob.IsRepeatable() is true, otherwise
        /// its implementation is ignored.
        /// </summary>
        /// <returns>Interval time between this job executions.</returns>
        public abstract int GetRepetitionIntervalTime();
    }
}
