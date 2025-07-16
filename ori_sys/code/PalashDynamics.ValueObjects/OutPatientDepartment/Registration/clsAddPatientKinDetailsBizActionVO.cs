using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster
{
    public class clsAddPatientKinDetailsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsAddPatientKinDetailsBizAction";
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

        private clsPatientKinDetailsVO  objKinDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsPatientKinDetailsVO KinDetails
        {
            get { return objKinDetails; }
            set { objKinDetails = value; }
        }

    }
}
