using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Patient
{

    public class clsGetPatientLabUploadReportDataBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Patient.clsGetPatientLabUploadReportDataBizAction";
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
        public long UnitID { get; set; }
        public long ID { get; set; }
        public byte[] Report { get; set; }
        public string SourceURL { get; set; }
        public Boolean IsPathology { get; set; }
    }
}