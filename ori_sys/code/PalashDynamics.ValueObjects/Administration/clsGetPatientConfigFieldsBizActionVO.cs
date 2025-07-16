using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration
{
   public class clsGetPatientConfigFieldsBizActionVO : IBizActionValueObject
    {
        #region IBizActionVO Member
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPatientConfigFieldsBizAction";
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
        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsPatientFieldsConfigVO> objPatientConfigFieldsTableMaster = new List<clsPatientFieldsConfigVO>();
        public List<clsPatientFieldsConfigVO> OtPateintConfigFieldsMatserDetails
        {
            get
            {
                return objPatientConfigFieldsTableMaster;
            }
            set
            {
                objPatientConfigFieldsTableMaster = value;

            }
        }
    }
}
