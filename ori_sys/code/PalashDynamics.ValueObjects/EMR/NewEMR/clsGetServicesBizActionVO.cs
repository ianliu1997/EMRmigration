using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Administration;

namespace PalashDynamics.ValueObjects.EMR.NewEMR
{
    public class clsGetServicesBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.NewEMR.clsGetServicesBizAction";
        }
        #endregion

        #region IValueObject Members
        public string ToXml()
        {
            return this.ToString();
        }

        #endregion


        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long UnitID { get; set; }
        public long VisitID { get; set; }
        public long DoctorID { get; set; }
        public bool IsOPDIPD { get; set; }
        public string DoctorCode { get; set; }

        private int _StartRowIndex = 0;
        public int StartRowIndex
        {
            get { return _StartRowIndex; }
            set { _StartRowIndex = value; }
        }

        private int _MaximumRows;
        public int MaximumRows
        {
            get { return _MaximumRows; }
            set { _MaximumRows = value; }
        }

        private int _TotalRows = 0;
        public int TotalRows
        {
            get { return _TotalRows; }
            set { _TotalRows = value; }
        }

        private List<clsServiceMasterVO> objServiceList = new List<clsServiceMasterVO>();
        
        public List<clsServiceMasterVO> VisitServicesList
        {
            get { return objServiceList; }
            set { objServiceList = value; }
        }
    }
}
