using System;
using System.Collections.Generic;
using System.Text;

namespace PAC.Common.Tracking
{
    public class FlightStatusItem
    {
        public string FlightNumber { get; set; } = string.Empty;
        public DateTime FlightDate { get; set; }
        public string FlightStatus { get; set; } = string.Empty;
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string StatusMessage { get; set; } = string.Empty;
        public string DepartureAirportCode { get; set; } = string.Empty;
        public string ArrivalAirportCode { get; set; } = string.Empty;
    }
}
