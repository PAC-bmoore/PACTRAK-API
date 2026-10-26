using System.Data.OleDb;
using PAC.Common.Sage;
using PAC.Common.Tracking;

namespace PAC.DataLayer.Sage
{
    /// <summary>
    /// Tracking reads for the portal: sales order headers (SOMAST01),
    /// manifest lines (somanf01), proof of delivery (SOPODD01) and
    /// invoices (ARMAST01) joined to their orders. Every query takes its
    /// values as OLE DB parameters (the ? placeholders), never as text
    /// spliced into the statement.
    /// </summary>
    public class TrackingDL : BaseDL
    {


    }
}
