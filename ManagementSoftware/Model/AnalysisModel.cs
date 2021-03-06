﻿using Common.DataTransferObjects;
using Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManagementSoftware.Model
{
    public class AnalysisModel
    {
        public static readonly string FORMAT_PATTERN = "{0} ({1}€)";

        public AnalysisModel(Analysis a)
        {
            Analysis = a;
        }

        public Analysis Analysis { get; private set; }

        public override string ToString()
        {
            return String.Format(FORMAT_PATTERN, Analysis.Name, Util.ToCost(Analysis.PriceInEuro));
        }

        public override bool Equals(object other)
        {
            return Analysis.Name.Equals(((AnalysisModel)other).Analysis.Name);
        }

        public override int GetHashCode()
        {
            return Analysis.Name.GetHashCode();
        }
    }
}
