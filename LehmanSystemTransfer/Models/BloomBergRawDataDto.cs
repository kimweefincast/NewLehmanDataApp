using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LehmanSystemTransfer.Models
{
    public class BloomBergRawDataDto
    {
        public Guid FileRecordID { get; set; }
        public string PrcDate { get; set; }
        public string FullName { get; set; }
        public string idIndex { get; set; }
        public string NumIssuesB { get; set; }
        public string NumIssuesF { get; set; }
        public string Currency { get; set; }
        public string RetType { get; set; }
        public string Coupon { get; set; }
        public string Maturity { get; set; }
        public string Quality { get; set; }
        public string Price { get; set; }
        public string MarketValue { get; set; }
        public string YieldWorst { get; set; }
        public string YieldMaturity { get; set; }
        public string DurationModF { get; set; }
        public string DurationModWorst { get; set; }
        public string DurationWorstF { get; set; }
        public string Convexity { get; set; }
        public string OAS { get; set; }
        public string DurationModB { get; set; }
        public string RetDaily { get; set; }
        public string RetMTD { get; set; }
        public string Ret3M { get; set; }
        public string Ret6M { get; set; }
        public string Ret12M { get; set; }
        public string RetYTD { get; set; }
        public string RetMTDPrice { get; set; }
        public string RetMTDCoupon { get; set; }
        public string RetMTDPaydown { get; set; }
        public string RetMTDCurrency { get; set; }
        public string ExcessRet { get; set; }
        public string ExcessRet3M { get; set; }
        public string ExcessRet6M { get; set; }
        public string ExcessRet12M { get; set; }
        public string ExcessRetYTD { get; set; }
        public string RetInception { get; set; }
        public string IndexValue { get; set; }
    }
}
