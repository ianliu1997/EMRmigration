using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.OTConfiguration
{
    public class clsAddUpdateOTTableDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.OTConfiguration.clsAddUpdateOTTableDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

        #endregion

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsOTTableVO> objOTTableMaster = new List<clsOTTableVO>();
        public List<clsOTTableVO> OTTableMasterMatserDetails
        {
            get
            {
                return objOTTableMaster;
            }
            set
            {
                objOTTableMaster = value;

            }
        }

        /// <summary>
        ///  Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
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
    }

    public class clsGetOTTableDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.OTConfiguration.clsGetOTTableDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsOTTableVO> objOtTableMaster = new List<clsOTTableVO>();
        public List<clsOTTableVO> OtTableMatserDetails
        {
            get
            {
                return objOtTableMaster;
            }
            set
            {
                objOtTableMaster = value;

            }
        }
        public long ID { get; set; }
        public long UnitID { get; set; }
        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string SearchExpression { get; set; }
        /// <summary>
        ///  Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
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



    }

    public class clsAddUpdateConsentMasterBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.OTConfiguration.clsAddUpdateConsentMasterBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

        #endregion

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsConsentMasterVO> objConsentMaster = new List<clsConsentMasterVO>();
        public List<clsConsentMasterVO> OTTableMasterMatserDetails
        {
            get
            {
                return objConsentMaster;
            }
            set
            {
                objConsentMaster = value;
            }
        }

        /// <summary>
        ///  Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
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
    }

    public class clsGetConsentMasterBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.OTConfiguration.clsGetConsentMasterBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsConsentMasterVO> objConsentMaster = new List<clsConsentMasterVO>();
        public List<clsConsentMasterVO> ConsentMatserDetails
        {
            get
            {
                return objConsentMaster;
            }
            set
            {
                objConsentMaster = value;

            }
        }
        public long ID { get; set; }
        public long UnitID { get; set; }
        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string SearchExpression { get; set; }
        /// <summary>
        ///  Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
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

    }

    public class clsGetInstructionDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.OTConfiguration.clsGetInstructionDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsInstructionMasterVO> objInstructionMaster = new List<clsInstructionMasterVO>();
        public List<clsInstructionMasterVO> InstructionMasterDetails
        {
            get
            {
                return objInstructionMaster;
            }
            set
            {
                objInstructionMaster = value;

            }
        }

        public long ID { get; set; }
        public long UnitID { get; set; }
        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string SearchExpression { get; set; }
        public long FilterCriteria { get; set; }
        /// <summary>
        ///  Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
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

    }

    public class clsGetInstructionDetailsByIDBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.OTConfiguration.clsGetInstructionDetailsByIDBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

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

        public long ID { get; set; }
        public int UserType { get; set; }

        private clsInstructionMasterVO objInstructionDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsInstructionMasterVO InstructionDetails
        {
            get { return objInstructionDetails; }
            set { objInstructionDetails = value; }
        }

    }

    public class clsAddUpdateInstructionDetailsBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.OTConfiguration.clsAddUpdateInstructionDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

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

        clsInstructionMasterVO objInstructionMaster = new clsInstructionMasterVO();
        /// <summary>
        /// Output Property.
        /// This Property Contains OrderBooking List Which is Added.
        /// </summary>

        public clsInstructionMasterVO InstMaster
        {
            get { return objInstructionMaster; }
            set { objInstructionMaster = value; }
        }


    }

    public class clsUpdateInstructionStatusBizActionVO : IBizActionValueObject
    {
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

        //   public long ID { get; set; }
        private clsInstructionMasterVO objTempStatus = null;

        /// <summary>
        /// Output Property.
        /// This Property Contains Status of the User Which is Added.
        /// </summary>

        public clsInstructionMasterVO InstructionTempStatus
        {
            get { return objTempStatus; }
            set { objTempStatus = value; }
        }

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.OTConfiguration.clsUpdateInstructionStatusBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
    }

    #region For IPD Module

    public class clsGetConsentDetailsBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.OTConfiguration.clsGetConsentDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private clsConsentDetailsVO _ConsentDetails;
        public clsConsentDetailsVO ConsentDetails
        {
            get { return _ConsentDetails; }
            set { _ConsentDetails = value; }

        }

    }

    public class clsGetPatientConsentsBizActionVO : IBizActionValueObject
    {

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.OTConfiguration.clsGetPatientConsentsBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
        }

        public long PatientID { get; set; }


        private List<clsConsentDetailsVO> _ConsentList;
        public List<clsConsentDetailsVO> ConsentList
        {
            get { return _ConsentList; }
            set { _ConsentList = value; }

        }


        private clsConsentDetailsVO objDetails;
        public clsConsentDetailsVO ResultDetails
        {
            get { return objDetails; }
            set { objDetails = value; }
        }


        public long ConsentID { get; set; }
        public long ConsentTypeID { get; set; }
        public long VisitAdmID { get; set; }
        public long VisitAdmUnitID { get; set; }
        public bool OPD_IPD { get; set; }
        

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private long _ScheduleID;
        public long ScheduleID
        {
            get
            {
                return _ScheduleID;
            }
            set
            {
                _ScheduleID = value;
            }
        }
    }

    public class clsGetPatientConsentsDetailsInHTMLBizActionVO : IBizActionValueObject
    {

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.OTConfiguration.clsGetPatientConsentsDetailsInHTMLBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
        }


        private List<clsConsentDetailsVO> _ConsentList;
        public List<clsConsentDetailsVO> ConsentList
        {
            get { return _ConsentList; }
            set { _ConsentList = value; }

        }


        private clsConsentDetailsVO objDetails;
        public clsConsentDetailsVO ResultDetails
        {
            get { return objDetails; }
            set { objDetails = value; }
        }



        public long ConsentTypeID { get; set; }
        public long VisitAdmID { get; set; }
        public long VisitAdmUnitID { get; set; }
        public bool OPD_IPD { get; set; }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }
    }

    public class clsSaveConsentDetailsBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.OTConfiguration.clsSaveConsentDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private clsConsentDetailsVO _ConsentDetails;
        public clsConsentDetailsVO ConsentDetails
        {
            get { return _ConsentDetails; }
            set { _ConsentDetails = value; }

        }

    }

    public class clsGetConsentByConsentTypeBizActionVO : IBizActionValueObject
    {

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.OTConfiguration.clsGetConsentByConsentTypeBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
        }


        private List<clsConsentDetailsVO> _ConsentList;
        public List<clsConsentDetailsVO> ConsentList
        {
            get { return _ConsentList; }
            set { _ConsentList = value; }

        }


        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }




        public long ConsentTypeID { get; set; }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }
    }

    public class clsDeleteConsentBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.OTConfiguration.clsDeleteConsentBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
        }

        private clsConsentDetailsVO _Consent;
        public clsConsentDetailsVO Consent
        {
            get { return _Consent; }
            set { _Consent = value; }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }
    }


    #endregion

}
