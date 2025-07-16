
//Created Date:19/July/2013
//Created By: Nilesh Raut
//Specification: BizAction VO For Add and Update the Patient EMR History

//Review By:
//Review Date:

//Modified By:
//Modified Date: 
using System.Collections.Generic;


namespace PalashDynamics.ValueObjects.EMR
{
    public class clsAddUpdatePatientHistoryDataBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsAddUpdatePatientHistoryDataBizAction";
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
        public bool FalgForAddUpdate { get; set; }

        private List<clsPatientEMRDataVO> objPatientEMRData = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsPatientEMRDataVO> PatientHistoryDataList
        {
            get { return objPatientEMRData; }
            set { objPatientEMRData = value; }
        }

        private clsPatientEMRDataVO _ObjPatientEMRData;
        public clsPatientEMRDataVO PatientEMRData
        {
            get { return _ObjPatientEMRData; }
            set { _ObjPatientEMRData = value; }
        }
    }

    public class clsAddUpdatePatientHistoryDataAndDetailsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddUpdatePatientHistoryDataAndDetailsBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
        public long PatientEmrdata { get; set; }
        public long VisitID { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long UnitID { get; set; }
        public long TemplateID { get; set; }
        public long DoctorID { get; set; }
        public string LinkServer { get; set; }
        public bool Status { get; set; }
        public bool IsOPDIPD { get; set; }
        public string DoctorCode { get; set; }
        public long Takenby { get; set; }
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
        public clsPatientEMRDataVO PatientEMRData
        {
            get { return _ObjPatientEMRData; }
            set { _ObjPatientEMRData = value; }
        }

        private List<clsPatientEMRDetailsVO> objPatientEMRDetail = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsPatientEMRDetailsVO> PatientHistoryDetailsList
        {
            get { return objPatientEMRDetail; }
            set { objPatientEMRDetail = value; }
        }

        private List<clsPatientPrescriptionDetailVO> objPatientCurrentMedicationDetail = null;
        public List<clsPatientPrescriptionDetailVO> PatientCurrentMedicationDetail
        {
            get { return objPatientCurrentMedicationDetail; }
            set { objPatientCurrentMedicationDetail = value; }
        }
    }

    public class clsAddUpdatePatientIVFHistoryDataAndDetailsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddUpdatePatienIVFtHistoryDataAndDetailsBizAction";
        }
        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }
        #endregion
        public long PatientEmrdata { get; set; }
        public long VisitID { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long UnitID { get; set; }
        public long TemplateID { get; set; }
        public long DoctorID { get; set; }
        public string LinkServer { get; set; }
        public bool Status { get; set; }
        public bool IsOPDIPD { get; set; }
        public string DoctorCode { get; set; }
        public long SaveIvfID { get; set; }
        public string Tab { get; set; }
        public bool ISIvfhistory { get; set; }
        private int _SuccessStatus;
        public long  TakenBy { get; set; }
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
        public clsPatientEMRDataVO PatientEMRData
        {
            get { return _ObjPatientEMRData; }
            set { _ObjPatientEMRData = value; }
        }

        private List<clsPatientEMRDetailsVO> objPatientEMRDetail = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsPatientEMRDetailsVO> PatientHistoryDetailsList
        {
            get { return objPatientEMRDetail; }
            set { objPatientEMRDetail = value; }
        }

    }

    public class clsAddUpdatePatientHistoryForIPDLapAndHistroBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddUpdatePatientHistoryForIPDLapandHistroscopyBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
        public long PatientEmrdata { get; set; }
        public long VisitID { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long UnitID { get; set; }
        public long TemplateID { get; set; }
        public long DoctorID { get; set; }
        public string LinkServer { get; set; }
        public bool Status { get; set; }
        public bool IsOPDIPD { get; set; }
        public string DoctorCode { get; set; }
        public long Takenby { get; set; }
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
        public clsPatientEMRDataVO PatientEMRData
        {
            get { return _ObjPatientEMRData; }
            set { _ObjPatientEMRData = value; }
        }

        private List<clsPatientEMRDetailsVO> objPatientEMRDetail = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsPatientEMRDetailsVO> PatientHistoryDetailsList
        {
            get { return objPatientEMRDetail; }
            set { objPatientEMRDetail = value; }
        }

        private List<ListItems2> _HystroLaproImg = null;
        public List<ListItems2> HystroLaproImg
        {
            get { return _HystroLaproImg; }
            set { _HystroLaproImg = value; }
        }

        private List<clsPatientPrescriptionDetailVO> objPatientCurrentMedicationDetail = null;
        public List<clsPatientPrescriptionDetailVO> PatientCurrentMedicationDetail
        {
            get { return objPatientCurrentMedicationDetail; }
            set { objPatientCurrentMedicationDetail = value; }
        }
    }

    public class ListItems1
    {
        public byte[] ImageName1 { get; set; }
       // public System.Windows.Media.Imaging.BitmapImage ImageName { get; set; }
        public byte[] Photo { get; set; }
        public string ImagePath { get; set; }
        public string OriginalImagePath { get; set; }
        private string _ISIPD = "Collapsed";
        public string ISIPD
        {
            get { return _ISIPD; }
            set { _ISIPD = value; }
        }
    }
      public class ListItems2
      {
          public byte[] Photo { get; set; }
      }
   
}
