using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster
{
    public class clsAddPatientSponsorBizActionVO : IBizActionValueObject
    {
        
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsAddPatientSponsorBizAction";
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

        private clsPatientSponsorVO objPatientSponsor = new clsPatientSponsorVO();
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsPatientSponsorVO PatientSponsorDetails
        {
            get { return objPatientSponsor; }
            set { objPatientSponsor = value; }
        }

    }


    public class clsDeletePatientSponsorBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsDeletePatientSponsorBizAction";
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

        private long _SponsorID;
        public long SponsorID
        {
            get { return _SponsorID; }
            set { _SponsorID = value; }
        }



        //private clsPatientSponsorVO objPatientSponsor = new clsPatientSponsorVO();
        ///// <summary>
        ///// Output Property.
        ///// This Property Contains OPDPatient Details Which is Added.
        ///// </summary>
        //public clsPatientSponsorVO PatientSponsorDetails
        //{
        //    get { return objPatientSponsor; }
        //    set { objPatientSponsor = value; }
        //}

    }


    public class clsAddPatientSponsorForPathologyBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsAddPatientSponsorForPathologyBizAction";
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

        private clsPatientSponsorVO objPatientSponsor = new clsPatientSponsorVO();
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsPatientSponsorVO PatientSponsorDetails
        {
            get { return objPatientSponsor; }
            set { objPatientSponsor = value; }
        }

    }


}
