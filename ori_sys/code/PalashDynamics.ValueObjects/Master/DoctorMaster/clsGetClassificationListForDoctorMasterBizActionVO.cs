using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master.DoctorMaster
{
    public class clsGetClassificationListForDoctorMasterBizActionVO : IBizActionValueObject
    {
        private List<clsDoctorVO> _DoctorDetails;
        public List<clsDoctorVO> DoctorDetails
        {
            get { return _DoctorDetails; }
            set { _DoctorDetails = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetClassificationListForDoctorMasterBizAction";
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
