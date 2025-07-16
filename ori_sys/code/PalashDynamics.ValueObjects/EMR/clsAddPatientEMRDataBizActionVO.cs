using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.EMR
{
    public class clsAddPatientEMRDataBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddPatientEMRDataBizAction";
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

        private clsPatientEMRDataVO objPatientEMRData = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsPatientEMRDataVO PatientEMRDataDetails
        {
            get { return objPatientEMRData; }
            set { objPatientEMRData = value; }
        }
     
    }

    public class clsAddPatientFeedbackBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddPatientFeedbackBizAction";
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

        private clsPatientFeedbackVO objPatientFeedback = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsPatientFeedbackVO PatientFeedbackDetails
        {
            get { return objPatientFeedback; }
            set { objPatientFeedback = value; }
        }
    }


    //// To save Template Data FieldWise in Table-T_PatientEMRDetails
    // Author- Harish Kirnani , Date:22July2011
    public class clsAddUpdatePatientEMRDetailBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddUpdatePatientEMRDetailBizAction";
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

        // Flag to define Add or Update transaction
        // For Add = 0
        // For Update = 1
        public int FalgForAddUpdate { get; set; }
        public bool IsDonor { get; set; }

        // This Varianve object is speciefied 
        // for only getting Basic details 
        // like UnitID, VisitID, TemplateID, DoctorID
        public clsVarianceVO TempVariance { get; set; }

        private List<clsPatientEMRDetailsVO> objPatientEMRDetail = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsPatientEMRDetailsVO> PatientEMRDataDetails
        {
            get { return objPatientEMRDetail; }
            set { objPatientEMRDetail = value; }
        }
        public long TemplateDataId { get; set; }
    }

    public class clsAddUpdatePatientEMRUploadedFilesBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddUpdatePatientEMRUploadedFilesBizAction";
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

        // Flag to define Add or Update transaction
        // For Add = 0
        // For Update = 1
        public int FalgForAddUpdate { get; set; }
        public int IsivfID { get; set; }

        // This Varianve object is speciefied 
        // for only getting Basic details 
        // like UnitID, VisitID, TemplateID, DoctorID
        public clsVarianceVO TempVariance { get; set; }

        private List<clsPatientEMRUploadedFilesVO> objPatientEMRUploadedFiles = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsPatientEMRUploadedFilesVO> PatientEMRUploadedFiles
        {
            get { return objPatientEMRUploadedFiles; }
            set { objPatientEMRUploadedFiles = value; }
        }
    }

    public class clsGetPatientEMRUploadedFilesBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPatientEMRUploadedFilesBizAction";
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

        private long _PatientID;
        public long PatientID
        {
            get { return _PatientID; }
            set
            {

                _PatientID = value;
            }
        }

        private long _PatientUnitID;
        public long PatientUnitID
        {
            get { return _PatientUnitID; }
            set
            {

                _PatientUnitID = value;
            }
        }


        private long _TemplateID;
        public long TemplateID
        {
            get { return _TemplateID; }
            set
            {

                _TemplateID = value;


            }
        }

        private long _VisitID;
        public long VisitID
        {
            get { return _VisitID; }
            set
            {

                _VisitID = value;


            }
        }

        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set
            {

                _UnitID = value;


            }
        }

        private string _ControlName;
        public string ControlName
        {
            get { return _ControlName; }
            set
            {

                _ControlName = value;

            }
        }

        private int _ControlIndex;
        public int ControlIndex
        {
            get { return _ControlIndex; }
            set
            {
                _ControlIndex = value;
            }
        }

        private List<clsPatientEMRUploadedFilesVO> PatientEMRUploadedFiles;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsPatientEMRUploadedFilesVO> objPatientEMRUploadedFiles
        {
            get { return PatientEMRUploadedFiles; }
            set { PatientEMRUploadedFiles = value; }
        }
    }
}
