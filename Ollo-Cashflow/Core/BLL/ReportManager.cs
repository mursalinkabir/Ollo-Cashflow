using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ollo_Cashflow.Core.Gateway;

namespace Ollo_Cashflow.Core.BLL
{
    public class ReportManager
    {
        ReportGateway reportGateway = new ReportGateway();
        public bool ProcessReportData()
        {
            bool isReportProcessed = reportGateway.ProcessReportData();
            return isReportProcessed;
        }
    }
}