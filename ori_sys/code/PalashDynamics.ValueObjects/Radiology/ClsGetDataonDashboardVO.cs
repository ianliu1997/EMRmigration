using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Radiology
{
    public class ClsGetDataonDashboardVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Radiology.clsGetDashBoardDataBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion


        public long OrderID { get; set; }
        public long UnitID { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? ToDate { get; set; }
        public bool DeliveryStatus { get; set; }
        public bool CheckDeliveryStatus { get; set; }
        public bool IsPagingEnabled { get; set; }
        public bool IsfromMarketing = false;
        public bool IsFollowUp = false;
        public int StartRowIndex { get; set; }
        public string SpecializationID { get; set; }
        public long SplID { get; set; }
        public long SubSplID { get; set; }
        public string SubSpecializationID { get; set; }
        public long DoctorTypeID { get; set; }
        public int MaximumRows { get; set; }

        public string SearchExpression { get; set; }

       
        public bool ResultEntryStatus { get; set; }
        public bool CheckResultEntryStatus { get; set; }

        public int TotalRows { get; set; }
        public string SortExpression { get; set; }

        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public bool xray { get; set; }
        public bool mri { get; set; }
        public bool ct { get; set; }
        public bool theiroid { get; set;}
        public bool usg  { get; set; }
        public bool samplecollected { get; set; }
        public long OpdNo { get; set; }
        public string MRNO { get; set; }


        private List<ClsGetDataonDashboardVO> objList1 = null;
        public List<ClsGetDataonDashboardVO> DataList1
        {
            get { return objList1; }
            set { objList1 = value; }
        }


       

    }
}
