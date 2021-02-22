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
    public class Upcom_ExecutionJob : Job
    {
        private readonly string link = "https://trade.vndirect.com.vn/chung-khoan/upcom";
        private readonly long idSan = (int)enumSanGD.UPCOM;
        public override string GetName()
        {
            return this.GetType().Name;
        }

        /// <summary>
        /// Execute the Job itself. Just print a message.
        /// </summary>
        public override async Task DoJob()
        {
            await CallJob(idSan, link);
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
