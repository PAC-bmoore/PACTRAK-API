using System;
using System.Collections.Generic;
using System.Text;

namespace PAC.Common.Sage
{
    public class PODItem
    {
        public string signby { get; set; }
        public DateTime pickdate { get; set; }
        public string picktime { get; set; }
        public decimal recpcs { get; set; }
        public decimal trnwt { get; set; }
        public string umeasur { get; set; }
        public string custno { get; set; }
        public string awbno { get; set; }
        public string adduser { get; set; }
        public DateTime adddate { get; set; }
        public string addtime { get; set; }
        public string lckuser { get; set; }
        public string lckstat { get; set; }
        public DateTime lckdate { get; set; }
        public string lcktime { get; set; }
        public decimal tranlineno { get; set; }
        public DateTime emailpu { get; set; }
    }
}
