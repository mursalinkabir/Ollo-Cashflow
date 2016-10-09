﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace Ollo_Cashflow.Models
{
    public class Report
    {
        public String Source { get; set; }
        public DateTime SourcePeriod { get; set; }
        public Double Amount { get; set; }
        public String DataType { get; set; }
        public DateTime CurrentPeriod { get; set; }
    }
}