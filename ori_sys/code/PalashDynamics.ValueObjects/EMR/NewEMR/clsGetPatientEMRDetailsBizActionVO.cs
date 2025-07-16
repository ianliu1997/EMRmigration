//Created Date:10/Sep/2013
//Created By: Nilesh Raut
//Specification: BizAction VO for Get Patient EMR History Details

//Review By:
//Review Date:

//Modified By:
//Modified Date: 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.EMR;
using PalashDynamics.ValueObjects.OutPatientDepartment;

namespace PalashDynamics.ValueObjects.EMR
{
    public class clsGetPatientEMRDetailsBizActionVO : IBizActionValueObject
    {

        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.clsGetPatientEMRDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

        #endregion

        public long VisitID { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long UnitID { get; set; }
        public long TemplateID { get; set; }
        public string DoctorCode { get; set; }
        public string Tab { get; set; }
        //Patient EMR Details Against Patient Id VisitId and (Tempalte ID Optional)
        private List<clsPatientEMRDetailsVO> _EMRDetailsList = new List<clsPatientEMRDetailsVO>();
        public List<clsPatientEMRDetailsVO> EMRDetailsList
        {
            get
            {
                return _EMRDetailsList;
            }
            set
            {
                _EMRDetailsList = value;
            }
        }

        private List<clsPatientEMRDetailsVO> _EMRImgList = new List<clsPatientEMRDetailsVO>();
        public List<clsPatientEMRDetailsVO> EMRImgList
        {
            get
            {
                return _EMRImgList;
            }
            set
            {
                _EMRImgList = value;
            }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get
            {
                return _SuccessStatus;
            }
            set
            {
                _SuccessStatus = value;
            }
        }

        // Added by Saily P on 05.12.13 Purpose - Added new controls.
        //public  clsBPControlVO objBPControl {get;set;}
        private clsBPControlVO _objBPControl;
        public clsBPControlVO objBPControl
        {
            get { return _objBPControl; }
            set { _objBPControl = value; }
        }

        //public clsVisionVO objVisionControl { get; set; }
        private clsVisionVO _objVisionControl;
        public clsVisionVO objVisionControl
        {
            get { return _objVisionControl; }
            set { _objVisionControl = value; }
        }

        //public clsGlassPowerVO objGPControl { get; set; }
        private clsGlassPowerVO _objGPControl;
        public clsGlassPowerVO objGPControl
        {
            get { return _objGPControl; }
            set { _objGPControl = value; }
        }

        public bool isBPControl { get; set; }
        public bool isVisionControl { get; set; }
        public bool isGPControl { get; set; }
        public bool IsOPDIPD { get; set; }
    }

    public class clsGetPatientEMRPhysicalExamDetailsBizActionVO : IBizActionValueObject
    {

        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.clsGetPatientEMRPhysicalExamDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

        #endregion

        public long VisitID { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long UnitID { get; set; }
        public long TemplateID { get; set; }
        public string DoctorCode { get; set; }
        public long DoctorId { get; set; }
        public string Tab { get; set; }
        public Boolean ISFromOTDetails { get; set; }
        //Patient EMR Details Against Patient Id VisitId and (Tempalte ID Optional)
        private List<clsPatientEMRDetailsVO> _EMRDetailsList = new List<clsPatientEMRDetailsVO>();
        public List<clsPatientEMRDetailsVO> EMRDetailsList
        {
            get
            {
                return _EMRDetailsList;
            }
            set
            {
                _EMRDetailsList = value;
            }
        }

        private List<clsPatientEMRDetailsVO> _EMRImgList = new List<clsPatientEMRDetailsVO>();
        public List<clsPatientEMRDetailsVO> EMRImgList
        {
            get
            {
                return _EMRImgList;
            }
            set
            {
                _EMRImgList = value;
            }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get
            {
                return _SuccessStatus;
            }
            set
            {
                _SuccessStatus = value;
            }
        }

        // Added by Saily P on 05.12.13 Purpose - Added new controls.
        //public  clsBPControlVO objBPControl {get;set;}
        private clsBPControlVO _objBPControl;
        public clsBPControlVO objBPControl
        {
            get { return _objBPControl; }
            set { _objBPControl = value; }
        }

        //public clsVisionVO objVisionControl { get; set; }
        private clsVisionVO _objVisionControl;
        public clsVisionVO objVisionControl
        {
            get { return _objVisionControl; }
            set { _objVisionControl = value; }
        }

        //public clsGlassPowerVO objGPControl { get; set; }
        private clsGlassPowerVO _objGPControl;
        public clsGlassPowerVO objGPControl
        {
            get { return _objGPControl; }
            set { _objGPControl = value; }
        }

        public bool isBPControl { get; set; }
        public bool isVisionControl { get; set; }
        public bool isGPControl { get; set; }
        public bool IsOPDIPD { get; set; }
    }

    public class clsGetPatientDiagnosisEMRDetailsBizActionVO : IBizActionValueObject
    {

        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.clsGetPatientDiagnosisEMRDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

        #endregion

        public long VisitID { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long UnitID { get; set; }
        public long TemplateID { get; set; }
        public string Tab { get; set; }
        //Patient EMR Details Against Patient Id VisitId and (Tempalte ID Optional)
        private List<clsPatientEMRDetailsVO> _EMRDetailsList = new List<clsPatientEMRDetailsVO>();
        public List<clsPatientEMRDetailsVO> EMRDetailsList
        {
            get
            {
                return _EMRDetailsList;
            }
            set
            {
                _EMRDetailsList = value;
            }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get
            {
                return _SuccessStatus;
            }
            set
            {
                _SuccessStatus = value;
            }
        }

        // Added by Saily P on 05.12.13 Purpose - Added new controls.
        //public  clsBPControlVO objBPControl {get;set;}
        private clsBPControlVO _objBPControl;
        public clsBPControlVO objBPControl
        {
            get { return _objBPControl; }
            set { _objBPControl = value; }
        }

        //public clsVisionVO objVisionControl { get; set; }
        private clsVisionVO _objVisionControl;
        public clsVisionVO objVisionControl
        {
            get { return _objVisionControl; }
            set { _objVisionControl = value; }
        }

        //public clsGlassPowerVO objGPControl { get; set; }
        private clsGlassPowerVO _objGPControl;
        public clsGlassPowerVO objGPControl
        {
            get { return _objGPControl; }
            set { _objGPControl = value; }
        }

        public bool isBPControl { get; set; }
        public bool isVisionControl { get; set; }
        public bool isGPControl { get; set; }

        private List<clsEyeControlVO> _objEye = new List<clsEyeControlVO>();
        public List<clsEyeControlVO> EyeList
        {
            get { return _objEye; }
            set { _objEye = value; }
        }
    }

    public class clsGetPatientPastPhysicalexamDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.clsGetPatientPastPhysicalexamDetailsBizAction";
        }
        public string ToXml()
        {
            return this.ToXml();
        }
        public long VisitID { get; set; }
        public long PatientID { get; set; }
        public Boolean IsOpdIpd { get; set; }
        public string DoctorCode { get; set; }
        public long DoctorID { get; set; }
        private bool _PagingEnabled;

        public bool PagingEnabled
        {
            get { return _PagingEnabled; }
            set { _PagingEnabled = value; }
        }

        private int _StartRowIndex = 0;

        public int StartRowIndex
        {
            get { return _StartRowIndex; }
            set { _StartRowIndex = value; }
        }

        private int _MaximumRows;

        public int MaximumRows
        {
            get { return _MaximumRows; }
            set { _MaximumRows = value; }
        }
        private int _TotalRows = 0;

        public int TotalRows
        {
            get { return _TotalRows; }
            set { _TotalRows = value; }
        }

        //private string _DateTime;
        //public string DateTime
        //{
        //    get { return _DateTime; }
        //    set { _DateTime = value; }
        //}

        private List<GetPastPhysicalexam> _PatientPastPhysicalexam = null;
        public List<GetPastPhysicalexam> PatientPastPhysicalexam
        {
            get { return _PatientPastPhysicalexam; }
            set { _PatientPastPhysicalexam = value; }
        }
        #endregion
    }

    public class clsGetPatientPastHistroScopyandLaproscopyHistoryBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.clsGetPatientPastHistroScopyandLaproscopyHistoryBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

        public long VisitID { get; set; }
        public long PatientID { get; set; }
        public Boolean IsOpdIpd { get; set; }
        public string DoctorCode { get; set; }
        public long DoctorID { get; set; }
        public long TemplateID { get; set; }
        private bool _PagingEnabled;

        public bool PagingEnabled
        {
            get { return _PagingEnabled; }
            set { _PagingEnabled = value; }
        }

        private int _StartRowIndex = 0;

        public int StartRowIndex
        {
            get { return _StartRowIndex; }
            set { _StartRowIndex = value; }
        }

        private int _MaximumRows;

        public int MaximumRows
        {
            get { return _MaximumRows; }
            set { _MaximumRows = value; }
        }
        private int _TotalRows = 0;

        public int TotalRows
        {
            get { return _TotalRows; }
            set { _TotalRows = value; }
        }

        private List<GetPastHistroandlapro> _PatientPastHistory = null;
        public List<GetPastHistroandlapro> PatientPasthistory
        {
            get { return _PatientPastHistory; }
            set { _PatientPastHistory = value; }
        }

        #endregion
    }

    public class GetPastHistroandlapro
    {
        private string _Template;
        public string Template
        {
            get { return _Template; }
            set { _Template = value; }
        }

        private long _TemplateId;
        public long TemplateId
        {
            get { return _TemplateId; }
            set { _TemplateId = value; }
        }


        private long _EmrId;
        public long EmrId
        {
            get { return _EmrId; }
            set { _EmrId = value; }
        }

        private string _TemplateValue;
        public string TemplateValue
        {
            get { return _TemplateValue; }
            set { _TemplateValue = value; }
        }

        private string _TemplateHeader;
        public string TemplateHeader
        {
            get { return _TemplateHeader; }
            set { _TemplateHeader = value; }
        }

        private string _DoctorName;
        public string DoctorName
        {
            get { return _DoctorName; }
            set { _DoctorName = value; }
        }

        private string _DoctorSpeclization;
        public string DoctorSpeclization
        {
            get { return _DoctorSpeclization; }
            set { _DoctorSpeclization = value; }
        }

        private DateTime _VisitDate;
        public DateTime VisitDate
        {
            get { return _VisitDate; }
            set
            {
                if (_VisitDate != value)
                {
                    _VisitDate = value;
                }
            }
        }


        public string DateTime { get; set; }
    }


    public class GetPastPhysicalexam
    {
        private string _Template;
        public string Template
        {
            get { return _Template; }
            set { _Template = value; }
        }

        private long _TemplateId;
        public long TemplateId
        {
            get { return _TemplateId; }
            set { _TemplateId = value; }
        }

        private string _TemplateValue;
        public string TemplateValue
        {
            get { return _TemplateValue; }
            set { _TemplateValue = value; }
        }

        private string _TemplateHeader;
        public string TemplateHeader
        {
            get { return _TemplateHeader; }
            set { _TemplateHeader = value; }
        }

        private string _DoctorName;
        public string DoctorName
        {
            get { return _DoctorName; }
            set { _DoctorName = value; }
        }

        private string _DoctorSpeclization;
        public string DoctorSpeclization
        {
            get { return _DoctorSpeclization; }
            set { _DoctorSpeclization = value; }
        }

        private DateTime _VisitDate;
        public DateTime VisitDate
        {
            get { return _VisitDate; }
            set
            {
                if (_VisitDate != value)
                {
                    _VisitDate = value;
                }
            }
        }


        public string DateTime { get; set; }
    }

    public class clsGetPatientAllVisitBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.clsGetPatientAllVisitBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

        #endregion
        public long PatientID { get; set; }
        public long VisitID1 { get; set; }
        public long PatientUnitID { get; set; }
        public long UnitID { get; set; }
        public Boolean ISFromNursing { get; set; }
        public Boolean IsFemaleTemplate { get; set; }
        public Boolean IsPhysicalExamination { get; set; }
        public long TemplateID { get; set; }
        public string AllTemplateID { get; set; }
        private List<clsVisitVO> objVisit = new List<clsVisitVO>();
        public List<clsVisitVO> VisitList
        {
            get { return objVisit; }
            set { objVisit = value; }
        }
    }

    public class clsGetPatientIvfIDBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.clsGetPatientIvfIDBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

        #endregion

        public long IvfID { get; set; }
    }

    public class clsGetPatientEMRIvfhistoryDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.clsGetPatientEMRIvfhistoryDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

        #endregion
        public long VisitID { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long UnitID { get; set; }
        public long TemplateID { get; set; }
        public string DoctorCode { get; set; }
        public string Tab { get; set; }
        public long EMRID { get; set; }
        public bool ISFromNursing { get; set; }
        //Patient EMR Details Against Patient Id VisitId and (Tempalte ID Optional)
        private List<clsPatientEMRDetailsVO> _EMRDetailsList = new List<clsPatientEMRDetailsVO>();
        public List<clsPatientEMRDetailsVO> EMRDetailsList
        {
            get
            {
                return _EMRDetailsList;
            }
            set
            {
                _EMRDetailsList = value;
            }
        }

        private List<clsPatientEMRDetailsVO> _EMRImgList = new List<clsPatientEMRDetailsVO>();
        public List<clsPatientEMRDetailsVO> EMRImgList
        {
            get
            {
                return _EMRImgList;
            }
            set
            {
                _EMRImgList = value;
            }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get
            {
                return _SuccessStatus;
            }
            set
            {
                _SuccessStatus = value;
            }
        }
        // Added by Saily P on 05.12.13 Purpose - Added new controls.
        //public  clsBPControlVO objBPControl {get;set;}
        private clsBPControlVO _objBPControl;
        public clsBPControlVO objBPControl
        {
            get { return _objBPControl; }
            set { _objBPControl = value; }
        }
        //public clsVisionVO objVisionControl { get; set; }
        private clsVisionVO _objVisionControl;
        public clsVisionVO objVisionControl
        {
            get { return _objVisionControl; }
            set { _objVisionControl = value; }
        }
        //public clsGlassPowerVO objGPControl { get; set; }
        private clsGlassPowerVO _objGPControl;
        public clsGlassPowerVO objGPControl
        {
            get { return _objGPControl; }
            set { _objGPControl = value; }
        }
        public bool isBPControl { get; set; }
        public bool isVisionControl { get; set; }
        public bool isGPControl { get; set; }
        public bool IsOPDIPD { get; set; }
    }
}
