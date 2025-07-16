using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.EMR;

namespace PalashDynamics.ValueObjects.Administration.OTConfiguration
{
    
    public class clsGetOTForProcedureBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetOTForProcedureBizAction";
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




        private List<MasterListItem> objOTDetails = new List<MasterListItem>();
        public List<MasterListItem> OTDetails
        {
            get { return objOTDetails; }
            set { objOTDetails = value; }
        }

        private List<MasterListItem> objDocTypeDetails = new List<MasterListItem>();
        public List<MasterListItem> DocDetails
        {
            get { return objDocTypeDetails; }
            set { objDocTypeDetails = value; }
        }

        private List<MasterListItem> objDesignationDetails = new List<MasterListItem>();
        public List<MasterListItem> DesignationDetails
        {
            get { return objDesignationDetails; }
            set { objDesignationDetails = value; }
        }
        public long noOfStaff { get; set; }
        public long? procedureID { get; set; }

        public List<long> procedureIDList { get; set; }


    }

    public class clsGetDoctorForDoctorTypeBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetDoctorForDoctorTypeBizAction";
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


        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public long PriorityID { get; set; }
        public int Opd_Ipd { get; set; }
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        public bool IsCalledForDoctorOrder { get; set; }

        private List<clsPatientProcedureScheduleVO> _DoctorOrderList = new List<clsPatientProcedureScheduleVO>();
        public List<clsPatientProcedureScheduleVO> DoctorOrderList
        {
            get { return _DoctorOrderList; }
            set { _DoctorOrderList = value; }
        }


        private List<MasterListItem> objDocDetails = new List<MasterListItem>();
        public List<MasterListItem> DocDetails
        {
            get { return objDocDetails; }
            set { objDocDetails = value; }
        }
        public long? docTypeID { get; set; }


        public string SpecializationCode { get; set; }

    }

    public class clsGetDoctorListBySpecializationBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetDoctorListBySpecializationBizAction";
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

        private List<MasterListItem> objDocDetails = new List<MasterListItem>();
        public List<MasterListItem> DocDetails
        {
            get { return objDocDetails; }
            set { objDocDetails = value; }
        }

        public string SpecializationCode { get; set; }
    }

    public class clsStaffByDesignationIDBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsStaffByDesignationIDBizAction";
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




        private List<MasterListItem> objStaffDetails = new List<MasterListItem>();
        public List<MasterListItem> StaffDetails
        {
            get { return objStaffDetails; }
            set { objStaffDetails = value; }
        }

        public long? DesignationID { get; set; }
        public long? ProcedureID { get; set; }
        public long staffQuantity { get; set; }




    }

    public class clsAddupdatePatientProcedureSchedulebizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddupdatePatientProcedureSchedulebizAction";
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




        private clsPatientProcedureScheduleVO objPatientProcSchedule = new clsPatientProcedureScheduleVO();
        public clsPatientProcedureScheduleVO patientProcScheduleDetails
        {
            get { return objPatientProcSchedule; }
            set { objPatientProcSchedule = value; }
        }

       



    }

    public class clsGetPatientProcedureScheduleBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetPatientProcedureScheduleBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public long OTID { get; set; }
        public long OTUnitID { get; set; }
        public long OTTableID { get; set; }
        public long DocID { get; set; }
        public DateTime? OTBookingDate { get; set; }
        public DateTime? OTTODate { get; set; }
        public long StaffID { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public bool IsEmergency { get; set; }
        public bool IsCancelled { get; set; }
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

        private List<clsPatientProcedureScheduleVO> objPatientProcSchedule = null;
        public List<clsPatientProcedureScheduleVO> patientProcScheduleDetails
        {
            get { return objPatientProcSchedule; }
            set { objPatientProcSchedule = value; }
        }

        //public long? PatientProcedureScheduleID { get; set; }

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }


        public string SortExpression { get; set; }


        string _MRNo;
        public string MRNo
        {
            get { return _MRNo; }
            set { _MRNo = value; }
        }

        private string _PatientName;
        public string PatientName
        {
            get { return _PatientName; }
            set { _PatientName = value; }
        }

        private string _FirstName;
        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; }
        }

        private string _LastName;
        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; }
        }
    }

    public class clsGetProcScheduleDetailsByProcScheduleIDBizActionVO : IBizActionValueObject
    {
        private List<clsPatientProcedureVO> _AddedPatientProcList = null;
        public List<clsPatientProcedureVO> AddedPatientProcList
        {
            get
            {
                return _AddedPatientProcList;

            }
            set
            {
                _AddedPatientProcList = value;
            }
        }

        private List<clsPatientProcedureVO> _PatientProcList = null;
        public List<clsPatientProcedureVO> PatientProcList
        {
            get
            {
                return _PatientProcList;

            }
            set
            {
                _PatientProcList = value;
            }
        }

        private List<clsPatientProcDocScheduleDetailsVO> _DocScheduleDetails;
        public List<clsPatientProcDocScheduleDetailsVO> DocScheduleDetails
        {
            get { return _DocScheduleDetails; }
            set { _DocScheduleDetails = value; }
        }

        private List<clsPatientProcStaffDetailsVO> _StaffDetailList;
        public List<clsPatientProcStaffDetailsVO> StaffDetailList
        {
            get { return _StaffDetailList; }
            set { _StaffDetailList = value; }
        }

        private List<clsPatientProcedureScheduleVO> _OTScheduleList;
        public List<clsPatientProcedureScheduleVO> OTScheduleList
        {
            get { return _OTScheduleList; }
            set { _OTScheduleList = value; }
        }

        private clsPatientInfo _patientInfoObject = new clsPatientInfo();
        public clsPatientInfo patientInfoObject
        {
            get { return _patientInfoObject; }
            set { _patientInfoObject = value; }
        }

        private List<clsPatientProcedureChecklistDetailsVO> _CheckList;
        public List<clsPatientProcedureChecklistDetailsVO> CheckList
        {
            get { return _CheckList; }
            set { _CheckList = value; }
        }

        private clsOtDetailsAnesthesiaNotesDetailsVO _AnesthesiaNotesObj = new clsOtDetailsAnesthesiaNotesDetailsVO();
        public clsOtDetailsAnesthesiaNotesDetailsVO AnesthesiaNotesObj
        {
            get { return _AnesthesiaNotesObj; }
            set { _AnesthesiaNotesObj = value; }
        }

        private List<clsDoctorSuggestedServiceDetailVO> _ServiceList = null;
        public List<clsDoctorSuggestedServiceDetailVO> ServiceList
        {
            get
            {
                return _ServiceList;
            }
            set
            {
                _ServiceList = value;
            }
        }

        private List<string> _PreOperativeInstructionList;
        public List<string> PreOperativeInstructionList
        {
            get
            {
                return _PreOperativeInstructionList;
            }
            set
            {
                _PreOperativeInstructionList = value;
            }
        }

        private List<string> _IntraOperativeInstructionList;
        public List<string> IntraOperativeInstructionList
        {
            get
            {
                return _IntraOperativeInstructionList;
            }
            set
            {
                _IntraOperativeInstructionList = value;
            }
        }

        public List<string> _PostOperativeInstructionList;
        public List<string> PostOperativeInstructionList
        {
            get
            {
                return _PostOperativeInstructionList;
            }
            set
            {
                _PostOperativeInstructionList = value;
            }
        }

        private List<clsOTDetailsPostInstructionDetailsVO> _PostInstructionList = null;
        public List<clsOTDetailsPostInstructionDetailsVO> PostInstructionList
        {
            get
            {
                return _PostInstructionList;
            }
            set
            {
                _PostInstructionList = value;
            }
        }

        private List<clsOTDetailsPreInstructionDetailsVO> _PreInstructionList = null;
        public List<clsOTDetailsPreInstructionDetailsVO> PreInstructionList
        {
            get
            {
                return _PreInstructionList;
            }
            set
            {
                _PreInstructionList = value;
            }
        }

        private List<clsOTDetailsIntraInstructionDetailsVO> _IntraInstructionList = null;
        public List<clsOTDetailsIntraInstructionDetailsVO> IntraInstructionList
        {
            get
            {
                return _IntraInstructionList;
            }
            set
            {
                _IntraInstructionList = value;
            }
        }

        private List<clsOTDetailsInstructionListDetailsVO> _InstructionList = null;
        public List<clsOTDetailsInstructionListDetailsVO> InstructionList
        {
            get
            {
                return _InstructionList;
            }
            set
            {
                _InstructionList = value;
            }
        }

        private List<string> _PostInstruction = null;
        public List<string> PostInstruction
        {
            get
            {
                return _PostInstruction;
            }
            set
            {
                _PostInstruction = value;
            }
        }

        private List<clsProcedureItemDetailsVO> _ItemList = null;
        public List<clsProcedureItemDetailsVO> ItemList
        {
            get
            {
                return _ItemList;
            }
            set
            {
                _ItemList = value;
            }
        }

        public long SelfCompanyID { get; set; }

        public long ScheduleUnitID { get; set; }

        public long ScheduleID { get; set; }

        private string _AnesthesiaNotes;
        public string AnesthesiaNotes
        {
            get
            {
                return _AnesthesiaNotes;
            }
            set
            {
                _AnesthesiaNotes = value;
            }
        }

        public long detailsID { get; set; }

        private long? _AnesthesiaNotesID;
        public long? AnesthesiaNotesID
        {
            get
            {
                return _AnesthesiaNotesID;
            }
            set
            {
                _AnesthesiaNotesID = value;
            }
        }

        private bool _Status;
        public bool Status
        {
            get
            {
                return _Status;
            }
            set
            {
                _Status = value;
            }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetProcScheduleDetailsByProcScheduleIDBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsGetProcDetailsByGetEMRTemplateBizactionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetProcDetailsByGetEMRTemplateBizaction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public long ScheduleID { get; set; }

        private List<clsPatientProcedureVO> _PatientProcList = null;
        public List<clsPatientProcedureVO> PatientProcList
        {
            get
            {
                return _PatientProcList;

            }
            set
            {
                _PatientProcList = value;
            }
        }
    }

    public class clsPatientInfo
    {
        public long pateintID;
        public string pateintFName;
        public string PatientName;
        public string pateintLname;
        public string MRNO;
        public long GenderID;
        public DateTime DOB;
        public string patientMName;
        public long patientUnitID;
        public long patientSourceID;
        public long tariffID;
        public long DoctorID;
        public DateTime? VisitDate;
        public string Gender;
    }


    public class clsCheckTimeForOTScheduleExistanceBizActionVO : IBizActionValueObject
    {
        private List<clsOTScheduleDetailsVO> _Details;
        public List<clsOTScheduleDetailsVO> Details
        {
            get { return _Details; }
            set { _Details = value; }
        }
        //public string DayID { get; set; }
        public long DayID { get; set; }
        public string Schedule1_StartTime { get; set; }
        public string Schedule1_EndTime { get; set; }

        public string Schedule2_EndTime { get; set; }
        public string Schedule2_StartTime { get; set; }
        public long OTID { get; set; }
        public long UnitID { get; set; }
        public long OTTableID { get; set; }

        private DateTime? _StartTime;
        public DateTime? StartTime
        {
            get { return _StartTime; }
            set
            {
                if (value != _StartTime)
                {
                    _StartTime = value;
                }
            }
        }

        private DateTime? _EndTime;
        public DateTime? EndTime
        {
            get { return _EndTime; }
            set
            {
                if (value != _EndTime)
                {
                    _EndTime = value;
                }
            }
        }

        public bool? IsSchedulePresent { get; set; }

        public bool SuccessStatus { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsCheckTimeForOTScheduleExistanceBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsAddOTScheduleMasterBizActionVO : IBizActionValueObject
    {

        public clsAddOTScheduleMasterBizActionVO()
        {

        }

        private clsOTScheduleVO objOTSchedule = null;
        public clsOTScheduleVO OTScheduleDetails
        {
            get { return objOTSchedule; }
            set { objOTSchedule = value; }

        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsAddOTScheduleMasterBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsGetOTScheduleMasterListBizActionVO : IBizActionValueObject
    {
        private List<clsOTScheduleVO> myVar = new List<clsOTScheduleVO>();

        public List<clsOTScheduleVO> OTScheduleList
        {
            get { return myVar; }
            set { myVar = value; }
        }

        private string _SearchExpression = "";
        public string InputSearchExpression
        {
            get { return _SearchExpression; }
            set { _SearchExpression = value; }
        }
        public long? UnitID { get; set; }

        public long? OTID { get; set; }

        public long? OTTableID { get; set; }

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        public string sortExpression { get; set; }

        public string LinkServer { get; set; }



        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetOTScheduleMasterListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsGetOTScheduleListBizActionVO : IBizActionValueObject
    {



        private List<clsOTScheduleDetailsVO> myVar = new List<clsOTScheduleDetailsVO>();

        public List<clsOTScheduleDetailsVO> OTScheduleList
        {
            get { return myVar; }
            set { myVar = value; }
        }
        public long OTScheduleID { get; set; }


        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetOTScheduleListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsUpdateOTScheduleMasterBizActionVO : IBizActionValueObject
    {

        public clsUpdateOTScheduleMasterBizActionVO()
        {

        }

        private clsOTScheduleVO objOTSchedule = null;
        public clsOTScheduleVO OTScheduleDetails
        {
            get { return objOTSchedule; }
            set { objOTSchedule = value; }

        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsUpdateOTScheduleMasterBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsGetOTScheduleTimeVO : IBizActionValueObject
    {

        private List<clsOTScheduleDetailsVO> _OTScheduleDetails;
        public List<clsOTScheduleDetailsVO> OTScheduleDetailsList
        {
            get { return _OTScheduleDetails; }
            set { _OTScheduleDetails = value; }
        }



        public long UnitId { get; set; }
        public long OTID { get; set; }
        public long OTTabelID { get; set; }

        private string _SearchExpression;
        public string InputSearchExpression
        {
            get { return _SearchExpression; }
            set { _SearchExpression = value; }
        }


        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public string DayID { get; set; }



        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetOTScheduleTime";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsGetProcOTScheduleBizActionVO : IBizActionValueObject
    {

        private List<clsPatientProcOTScheduleDetailsVO> _OTScheduleDetails;
        public List<clsPatientProcOTScheduleDetailsVO> OTScheduleDetailsList
        {
            get { return _OTScheduleDetails; }
            set { _OTScheduleDetails = value; }
        }

        public long OTID { get; set; }

        public long OTTableID { get; set; }



        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetProcOTScheduleBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsGetCheckListByProcedureIDBizActionVO : IBizActionValueObject
    {



        public List<long> procedureIDList { get; set; }

        private List<clsPatientProcedureChecklistDetailsVO> _ChecklistDetails;
        public List<clsPatientProcedureChecklistDetailsVO> ChecklistDetails
        {
            get { return _ChecklistDetails; }
            set { _ChecklistDetails = value; }
        }

        public long UnitID { get; set; }
        public long ProcedureID { get; set; }



        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetCheckListByProcedureIDBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsGetCheckListByScheduleIDBizActionVO : IBizActionValueObject
    {



        private List<clsPatientProcedureChecklistDetailsVO> _ChecklistDetails;
        public List<clsPatientProcedureChecklistDetailsVO> ChecklistDetails
        {
            get { return _ChecklistDetails; }
            set { _ChecklistDetails = value; }
        }
        public long ScheduleUnitID { get; set; }
        public long ScheduleID { get; set; }



        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetCheckListByScheduleIDBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsGetDocScheduleByDocIDBizActionVO : IBizActionValueObject
    {



        private List<clsPatientProcDocScheduleDetailsVO> _DocScheduleListDetails;
        public List<clsPatientProcDocScheduleDetailsVO> DocScheduleListDetails
        {
            get { return _DocScheduleListDetails; }
            set { _DocScheduleListDetails = value; }
        }

        public long DocID { get; set; }
        public long DocTableID { get; set; }
        public DateTime procDate { get; set; }




        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetDocScheduleByDocIDBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsCancelOTBookingBizActionVO : IBizActionValueObject
    {
        public long patientProcScheduleID { get; set; }
        public long CancelledBy { get; set; }
        public string CancelledReason { get; set; }
        public DateTime Cancelleddate { get; set; }


        #region IBizAction Members
        /// <summary>
        /// Retuns the bizAction Class Name.
        /// </summary>
        /// <returns></returns>
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsCancelOTBookingBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion


    }

    public class clsGetStaffForOTSchedulingBizActionVO : IBizActionValueObject
    {
        public List<MasterListItem> staffList = new List<MasterListItem>();




        #region IBizAction Members
        /// <summary>
        /// Retuns the bizAction Class Name.
        /// </summary>
        /// <returns></returns>
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetStaffForOTSchedulingBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            throw new NotImplementedException();
        }

        #endregion


    }

    public class clsGetProceduresForOTBookingIDBizActionVO : IBizActionValueObject
    {
        public List<MasterListItem> procedureList = new List<MasterListItem>();
        public long OTBokingID { get; set; }



        #region IBizAction Members
        /// <summary>
        /// Retuns the bizAction Class Name.
        /// </summary>
        /// <returns></returns>
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetProceduresForOTBookingIDBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            throw new NotImplementedException();
        }

        #endregion



    }

    public class clsGetConsentForProcedureIDBizActionVO : IBizActionValueObject
    {
        public List<MasterListItem> ConsentList = new List<MasterListItem>();
        public long ProcedureID { get; set; }



        #region IBizAction Members
        /// <summary>
        /// Retuns the bizAction Class Name.
        /// </summary>
        /// <returns></returns>
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetConsentForProcedureIDBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            throw new NotImplementedException();
        }

        #endregion



    }

    public class clsGetOTBookingByOTTablebookingDateBizActionVO : IBizActionValueObject
    {
        private List<clsPatientProcedureScheduleVO> myVar = new List<clsPatientProcedureScheduleVO>();

        public List<clsPatientProcedureScheduleVO> bookingDetailsList
        {
            get { return myVar; }
            set { myVar = value; }
        }

        public long? OTTableID { get; set; }
        public long? OTID { get; set; }
        public long? UnitId { get; set; }

        public bool SuccessStatus { get; set; }

        public DateTime? OTDate { get; set; }

        // public DateTime? NextAppointmentDate { get; set; }


        public string LinkServer { get; set; }


        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetOTBookingByOTTablebookingDateBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsUpdateOTScheduleStatusBizActionVO : IBizActionValueObject
    {

        #region IBizAction Members
        /// <summary>
        /// Retuns the bizAction Class Name.
        /// </summary>
        /// <returns></returns>
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsUpdateOTScheduleStatusBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            throw new NotImplementedException();
        }

        #endregion

        private clsPatientProcedureScheduleVO _UpdateStatusField = null;
        public clsPatientProcedureScheduleVO UpdateStatusField
        {
            get { return _UpdateStatusField; }
            set { _UpdateStatusField = value; }

        }

        private bool _IsCalledForStatus = false;
        public bool IsCalledForStatus
        {
            get { return _IsCalledForStatus; }
            set { _IsCalledForStatus = value; }

        }

        private bool _IsCalledForPAC = false;
        public bool IsCalledForPAC
        {
            get { return _IsCalledForPAC; }
            set { _IsCalledForPAC = value; }

        }
    }





}
