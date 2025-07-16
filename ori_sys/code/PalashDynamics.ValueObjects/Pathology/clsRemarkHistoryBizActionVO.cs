using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Pathology
{
    //get datta
    public class clsRemarkHistoryBizActionVO : IBizActionValueObject //BY ROHINI DATED 19/1/17 ADD HISTORY
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Pathology.clsRemarkHistoryBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion


        private int _SuccessStatus;     
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }


        public long UnitID { get; set; }
        public string UserName { get; set; }        
        public long OrderID { get; set; }
        public long UserID { get; set; }
        public long OrderUnitID { get; set; }
        public string Remark { get; set; }

        private List<clsPathOrderBookingDetailVO> ObjRemarkHistory = new List<clsPathOrderBookingDetailVO>();
        public List<clsPathOrderBookingDetailVO> RemarkHistory
        {
            get { return ObjRemarkHistory; }
            set { ObjRemarkHistory = value; }
        }

        public long ID { get; set; }
    }
   
}
