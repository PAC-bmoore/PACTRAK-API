using System;
using System.Collections.Generic;
using System.Text;

namespace PAC.Common.Tracking
{
    public class DelayItem
    {
        public DateTime FlightDate { get; set; }
        public string AWB { get; set; } = string.Empty;
        public string FlightNum { get; set; } = string.Empty;
        public DateTime ETA { get; set; }
        public DateTime ETD { get; set; }
        public DateTime DelayNoticeDate { get; set; }
        public string DelayReason { get; set; } = string.Empty;
        public bool Delayed { get; set; }
        public string TrackingMessage { get; set; } = string.Empty;
        public bool Departed { get; set; }
    }
}
