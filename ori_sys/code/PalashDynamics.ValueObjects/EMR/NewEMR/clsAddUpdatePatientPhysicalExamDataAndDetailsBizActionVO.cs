
//Created Date:21/July/2013
//Created By: Nilesh Raut
//Specification: BizAction VO For Add and Update the Patient EMR Physical Exam

//Review By:
//Review Date:

//Modified By:
//Modified Date: 
using System.Collections.Generic;

namespace PalashDynamics.ValueObjects.EMR
{
    public class clsAddUpdatePatientPhysicalExamDataAndDetailsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.EMR.clsAddUpdatePatientPhysicalExamDataAndDetailsBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
        public long VisitID { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long UnitID { get; set; }
        public long TemplateID { get; set; }
        public string LinkServer { get; set; }
        public bool Status { get; set; }
        public bool IsOPDIPD { get; set; }
        public string DoctorCode { get; set; }
        public long DoctorId { get; set; }
        public long TakenBy { get; set; }
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
        public bool FalgForAddUpdate { get; set; }

        private clsPatientEMRDataVO _ObjPatientEMRData;
        public clsPatientEMRDataVO PatientPhysicalExamData
        {
            get { return _ObjPatientEMRData; }
            set { _ObjPatientEMRData = value; }
        }

        private List<clsPatientEMRDetailsVO> objPatientEMRDetail = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsPatientEMRDetailsVO> PatientPhysicalExamDetailsList
        {
            get { return objPatientEMRDetail; }
            set { objPatientEMRDetail = value; }
        }

        //Added by Saily P on 02.12.13 Purpose - For New control.
        private clsBPControlVO _objBPDetails = null;
        public clsBPControlVO objBPDetails
        {
            get { return _objBPDetails; }
            set { _objBPDetails = value; }
        }

        private bool _IsBPControl;
        public bool IsBPControl
        {
            get { return _IsBPControl; }
            set { _IsBPControl = value; }
        }

        private bool _IsVisionControl;
        public bool IsVisionControl
        {
            get { return _IsVisionControl; }
            set { _IsVisionControl = value; }
        }

        private bool _IsGPControl;
        public bool IsGPControl
        {
            get { return _IsGPControl; }
            set { _IsGPControl = value; }
        }

        private clsVisionVO _objVisionDetails = null;
        public clsVisionVO objVisionDetails
        {
            get { return _objVisionDetails; }
            set { _objVisionDetails = value; }
        }

        private clsGlassPowerVO _objGPDetails = null;
        public clsGlassPowerVO objGPDetails
        {
            get { return _objGPDetails; }
            set { _objGPDetails = value; }
        }
       
    }

    public class clsAddUpdatePatientOTDetailsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.EMR.clsAddUpdatePatientOTDetailsBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
        public long VisitID { get; set; }
        public long ScheduleID { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long UnitID { get; set; }
        public long TemplateID { get; set; }
        public string LinkServer { get; set; }
        public bool Status { get; set; }
        public bool IsOPDIPD { get; set; }
        public string DoctorCode { get; set; }
        public long DoctorId { get; set; }
        public long TakenBy { get; set; }
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
        public bool FalgForAddUpdate { get; set; }

        private clsPatientEMRDataVO _ObjPatientEMRData;
        public clsPatientEMRDataVO PatientPhysicalExamData
        {
            get { return _ObjPatientEMRData; }
            set { _ObjPatientEMRData = value; }
        }

        private List<clsPatientEMRDetailsVO> objPatientEMRDetail = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsPatientEMRDetailsVO> PatientPhysicalExamDetailsList
        {
            get { return objPatientEMRDetail; }
            set { objPatientEMRDetail = value; }
        }

        private List<ListItems2> _UploadImg = null;
        public List<ListItems2> UploadImg
        {
            get { return _UploadImg; }
            set { _UploadImg = value; }
        }

        //Added by Saily P on 02.12.13 Purpose - For New control.
        private clsBPControlVO _objBPDetails = null;
        public clsBPControlVO objBPDetails
        {
            get { return _objBPDetails; }
            set { _objBPDetails = value; }
        }

        private bool _IsBPControl;
        public bool IsBPControl
        {
            get { return _IsBPControl; }
            set { _IsBPControl = value; }
        }

        private bool _IsVisionControl;
        public bool IsVisionControl
        {
            get { return _IsVisionControl; }
            set { _IsVisionControl = value; }
        }

        private bool _IsGPControl;
        public bool IsGPControl
        {
            get { return _IsGPControl; }
            set { _IsGPControl = value; }
        }

        private clsVisionVO _objVisionDetails = null;
        public clsVisionVO objVisionDetails
        {
            get { return _objVisionDetails; }
            set { _objVisionDetails = value; }
        }

        private clsGlassPowerVO _objGPDetails = null;
        public clsGlassPowerVO objGPDetails
        {
            get { return _objGPDetails; }
            set { _objGPDetails = value; }
        }

    }
}
