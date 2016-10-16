using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
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
        [HttpPost]
        public ActionResult Report(string test)
        {
            ViewBag.isReportDataProcessed = reportManager.ProcessReportData();
            if (reportManager.ProcessReportData())
            {
                ViewBag.GetAllReportData = reportManager.GetallReportdata();
            }
            
            return View();
        }

        public ActionResult Export()
        {
            ViewBag.GetAllReportData = reportManager.GetallReportdata();
           Response.AddHeader("Content-Type", "application/vnd.ms-excel");
           Response.AddHeader("Content-Disposition", "attachment; filename=TreasuryReport.xls");
            Response.ContentEncoding = Encoding.UTF8;
            //Response.ContentType = "application/ms-excel";
           
            return View();
        }
    }
}