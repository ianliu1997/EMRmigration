using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.Patient;

namespace PalashDynamics.ValueObjects.Patient
{
    public class clsGetPatientPenPusherDetailListBizActionVO : IBizActionValueObject
    {
        private List<clsPatientPenPusherInfoVO> _PatientPenPusherDetailsList;
        public List<clsPatientPenPusherInfoVO> PatientPenPusherDetailsList
        {
            get { return _PatientPenPusherDetailsList; }
            set { _PatientPenPusherDetailsList = value; }
        }
        private clsPatientPenPusherInfoVO _PatientPenPusherDetailsInfo;
        public clsPatientPenPusherInfoVO PatientPenPusherDetailsInfo
        {
            get { return _PatientPenPusherDetailsInfo; }
            set { _PatientPenPusherDetailsInfo = value; }
        }
       
        public bool IsPagingEnabled { get; set; }

        public int StartRowIndex { get; set; }

        public int MaximumRows { get; set; }

        public string SearchExpression { get; set; }

        public int TotalRows { get; set; }

        public long PatientID { get; set; }
        public long UnitID { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Patient.clsGetPatientPenPusherDetailListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }
}
