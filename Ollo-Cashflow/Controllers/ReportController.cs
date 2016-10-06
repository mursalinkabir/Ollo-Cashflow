using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ollo_Cashflow.Core.BLL;

namespace Ollo_Cashflow.Controllers
{
    public class ReportController : Controller
    {
        ReportManager reportManager = new ReportManager();
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Report()
        {
            ViewBag.isReportDataProcessed = reportManager.ProcessReportData();
            if (reportManager.ProcessReportData())
            {
                ViewBag.GetAllReportData = reportManager.GetallReportdata();
            }
            
            return View();
        }
    }
}