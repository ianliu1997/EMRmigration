using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.PatientSourceMaster
{
    public class clsGetPatientSourceListByPatientCategoryIdBizActionVO : IBizActionValueObject
    {

        public long? SelectedPatientCategoryIdID { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.PatientSourceMaster.clsGetPatientSourceListByPatientCategoryIdBizAction";
        }

        #endregion

   

        private List<clsPatientSourceVO> _PatientSourceDetails;
        public List<clsPatientSourceVO> PatientSourceDetails
        {
            get { return _PatientSourceDetails; }
            set { _PatientSourceDetails = value; }
        }


        public string ToXml()
        {
            return this.ToString();
        }

    }
}
