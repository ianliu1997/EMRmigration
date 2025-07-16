using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.EMR;

namespace PalashDynamics.ValueObjects.Administration.OTConfiguration
{
    public class clsGetPatientUnitIDForOtDetailsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetPatientUnitIDForOtDetailsBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public long patientID { get; set; }
        public long patientUnitID { get; set; }

    }

    public class clsGetProceduresForServiceIdBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetProceduresForServiceIdBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public long ServiceID { get; set; }
        public List<clsProcedureMasterVO> procedureList = new List<clsProcedureMasterVO>();

    }

    public class clsGetServicesForProcedureIDBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetServicesForProcedureIDBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public long ProcedureID { get; set; }
        public List<clsServiceMasterVO> serviceList = new List<clsServiceMasterVO>();
        public List<clsOTDetailsItemDetailsVO> ItemList = new List<clsOTDetailsItemDetailsVO>();

    }

    public class clsGetDoctorForOTDetailsBizActionVO : IBizActionValueObject
    {



        private List<clsOTDetailsDocDetailsVO> _DocList = new List<clsOTDetailsDocDetailsVO>();

        public List<clsOTDetailsDocDetailsVO> DocList
        {
            get { return _DocList; }
            set { _DocList = value; }
        }

       
     
        public long? UnitID { get; set; }

        public long? DepartmentID { get; set; }

        public long? DocTypeID { get; set; }

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        public string sortExpression { get; set; }



        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetDoctorForOTDetailsBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsGetOTSheetDetailsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetOTSheetDetailsBizAction";
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

        private clsOTDetailsVO _objOTDetails = new clsOTDetailsVO();
        public clsOTDetailsVO objOTDetails
        {
            get { return _objOTDetails; }
            set { _objOTDetails = value; }
        }
    }

    public class clsAddupdatOtDetailsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddupdatOtDetailsBizAction";
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




        private clsOTDetailsVO _objOTDetails = new clsOTDetailsVO();
        public clsOTDetailsVO objOTDetails
        {
            get { return _objOTDetails; }
            set { _objOTDetails = value; }
        }
    }

    public class clsAddUpdatOtSurgeryDetailsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddUpdatOtSurgeryDetailsBizAction";
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

        private clsOTDetailsVO _objOTDetails = new clsOTDetailsVO();
        public clsOTDetailsVO objOTDetails
        {
            get { return _objOTDetails; }
            set { _objOTDetails = value; }
        }

    }

    public class clsAddUpdateOtItemDetailsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddUpdatOtItemDetailsBizAction";
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

        private clsOTDetailsVO _objOTDetails = new clsOTDetailsVO();
        public clsOTDetailsVO objOTDetails
        {
            get { return _objOTDetails; }
            set { _objOTDetails = value; }
        }
    }

    public class clsAddUpdatOtDocEmpDetailsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddUpdatOtDocEmpDetailsBizAction";
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




        private clsOTDetailsVO _objOTDetails = new clsOTDetailsVO();
        public clsOTDetailsVO objOTDetails
        {
            get { return _objOTDetails; }
            set { _objOTDetails = value; }
        }





    }

    public class clsAddUpdatOtServicesDetailsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddUpdatOtServicesDetailsBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion



        //public List<clsDoctorSuggestedServiceDetailVO> OTServiceList { get; set; }
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


        private List<clsDoctorSuggestedServiceDetailVO> _OTServiceList;
        public List<clsDoctorSuggestedServiceDetailVO> OTServiceList
        {
            get { return _OTServiceList; }
            set { _OTServiceList = value; }
        }


        private clsOTDetailsVO _objOTDetails = new clsOTDetailsVO();
        public clsOTDetailsVO objOTDetails
        {
            get { return _objOTDetails; }
            set { _objOTDetails = value; }
        }





    }

    public class clsAddUpdatOtNotesDetailsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddUpdatOtNotesDetailsBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion



        //public List<clsDoctorSuggestedServiceDetailVO> OTServiceList { get; set; }
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


        private List<clsDoctorSuggestedServiceDetailVO> _OTServiceList;
        public List<clsDoctorSuggestedServiceDetailVO> OTServiceList
        {
            get { return _OTServiceList; }
            set { _OTServiceList = value; }
        }


        private clsOTDetailsVO _objOTDetails = new clsOTDetailsVO();
        public clsOTDetailsVO objOTDetails
        {
            get { return _objOTDetails; }
            set { _objOTDetails = value; }
        }

        private List<clsOTDetailsInstructionListDetailsVO> _SurgeryInstructionList = null;
        public List<clsOTDetailsInstructionListDetailsVO> SurgeryInstructionList
        {
            get
            {
                return _SurgeryInstructionList;
            }
            set
            {
                _SurgeryInstructionList = value;
            }
        }

        private List<clsOTDetailsInstructionListDetailsVO> _AnesthesiaInstructionList = null;
        public List<clsOTDetailsInstructionListDetailsVO> AnesthesiaInstructionList
        {
            get
            {
                return _AnesthesiaInstructionList;
            }
            set
            {
                _AnesthesiaInstructionList = value;
            }
        }

    }

    public class clsAddUpdateOTDoctorNotesDetailsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddUpdateOTDoctorNotesDetailsBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        //public List<clsDoctorSuggestedServiceDetailVO> OTServiceList { get; set; }
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

        private clsOTDetailsVO _objOTDetails = new clsOTDetailsVO();
        public clsOTDetailsVO objOTDetails
        {
            get { return _objOTDetails; }
            set { _objOTDetails = value; }
        }
    }

    public class clsAddUpdatOtPostInstructionDetailsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddUpdatOtPostInstructionDetailsBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion



        //public List<clsDoctorSuggestedServiceDetailVO> OTServiceList { get; set; }
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


        private List<clsDoctorSuggestedServiceDetailVO> _OTServiceList;
        public List<clsDoctorSuggestedServiceDetailVO> OTServiceList
        {
            get { return _OTServiceList; }
            set { _OTServiceList = value; }
        }


        private clsOTDetailsVO _objOTDetails = new clsOTDetailsVO();
        public clsOTDetailsVO objOTDetails
        {
            get { return _objOTDetails; }
            set { _objOTDetails = value; }
        }

    }

    public class clsGetOTDetailsListizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetOTDetailsListizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public long OTID { get; set; }
      
        public long DocID { get; set; }
        public long StaffID { get; set; }

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


        private List<clsOTDetailsVO> _objOTDetails = null;
        public List<clsOTDetailsVO> objOTDetails
        {
            get { return _objOTDetails; }
            set { _objOTDetails = value; }
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

    public class clsGetDetailTablesOfOTDetailsByOTDetailIDBizActionVO : IBizActionValueObject
    {
        public long PatientID { get; set; }
        public long ScheduleId { get; set; }
        private clsOTDetailsOTSheetDetailsVO _OTSheetDetailsObj = null;
        public clsOTDetailsOTSheetDetailsVO OTSheetDetailsObj
        {
            get
            {
                return _OTSheetDetailsObj;
            }
            set 
            {
                _OTSheetDetailsObj = value;
            }
        }

        private List<clsOtDetailsProcedureDetailsVO> _ProcedureList = null;
        public List<clsOtDetailsProcedureDetailsVO> ProcedureList 
        {
            get
            {
                return _ProcedureList;

            }
            set
            {
                _ProcedureList = value;
            }
        }

        private List<clsOTDetailsDocDetailsVO> _DoctorList = null;
        public List<clsOTDetailsDocDetailsVO> DoctorList
        {
            get { return _DoctorList; }
            set { _DoctorList = value; }
        }

        private List<clsOTDetailsStaffDetailsVO> _StaffList = null;
        public List<clsOTDetailsStaffDetailsVO> StaffList
        {
            get { return _StaffList; }
            set { _StaffList = value; }
        }

        private List<clsOTDetailsItemDetailsVO> _ItemList;
        public List<clsOTDetailsItemDetailsVO> ItemList
        {
            get { return _ItemList; }
            set { _ItemList = value; }
        }

        private List<clsOTDetailsInstrumentDetailsVO> _InstrumentList;
        public List<clsOTDetailsInstrumentDetailsVO> InstrumentList
        {
            get { return _InstrumentList; }
            set { _InstrumentList = value; }
        }

        private clsOtDetailsAnesthesiaNotesDetailsVO _AnesthesiaNotesObj = new clsOtDetailsAnesthesiaNotesDetailsVO();
        public clsOtDetailsAnesthesiaNotesDetailsVO AnesthesiaNotesObj
        {
            get { return _AnesthesiaNotesObj; }
            set { _AnesthesiaNotesObj = value; }
        }

        private clsOTDetailsSurgeryDetailsVO _SurgeryNotesObj = new clsOTDetailsSurgeryDetailsVO();
        public clsOTDetailsSurgeryDetailsVO SurgeryNotesObj
        {
            get { return _SurgeryNotesObj; }
            set { _SurgeryNotesObj = value; }
        }

      









        public long OTDetailsID { get; set; }


        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetDetailTablesOfOTDetailsByOTDetailIDBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsGetConsetDetailsForConsentIDBizActionVO : IBizActionValueObject
    {

       
        public long ConsentID { get; set; }
        public clsConsentMasterVO consentmaster = new clsConsentMasterVO();

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetConsetDetailsForConsentIDBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsAddPatientWiseConsentPrintingBizActionVO : IBizActionValueObject
    {



        public clsPatientWiseConsentPrintingVO ConsetPrintingObj = new clsPatientWiseConsentPrintingVO();

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsAddPatientWiseConsentPrintingBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsGetDoctorNotesByOTDetailsIDBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetDoctorNotesByOTDetailsIDBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private clsOTDetailsDoctorNotesDetailsVO _DoctorNotes = null;
        public clsOTDetailsDoctorNotesDetailsVO DoctorNotes
        {
            get { return _DoctorNotes; }
            set { _DoctorNotes = value; }
        }


        public long PatientID { get; set; }
        public long ScheduleId { get; set; }
        public long OTDetailsID { get; set; }

    }

    public class clsGetSurgeryDetailsByOTDetailsIDBizActionVO : IBizActionValueObject
    {
        public long PatientID { get; set; }
        public long ScheduleId { get; set; }

        private List<clsPatientProcedureVO> _ProcedureList = null;
        public List<clsPatientProcedureVO> ProcedureList
        {
            get
            {
                return _ProcedureList;

            }
            set
            {
                _ProcedureList = value;
            }
        }

        public long OTDetailsID { get; set; }


        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetSurgeryDetailsByOTDetailsIDBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsGetServicesByOTDetailsIDBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetServicesByOTDetailsIDBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private List<clsDoctorSuggestedServiceDetailVO> objOtherServiceDetails = null;
        public List<clsDoctorSuggestedServiceDetailVO> ServiceDetails
        {
            get { return objOtherServiceDetails; }
            set { objOtherServiceDetails = value; }
        }

        public long PatientID { get; set; }
        public long ScheduleId { get; set; }
        public long OTDetailsID { get; set; }

    }

    public class clsGetOTNotesByOTDetailsIDBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetOTNotesByOTDetailsIDBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public long PatientID { get; set; }
        public long ScheduleId { get; set; }
        public long OTDetailsID { get; set; }

        private clsOtDetailsAnesthesiaNotesDetailsVO _AnesthesiaNotesObj = new clsOtDetailsAnesthesiaNotesDetailsVO();
        public clsOtDetailsAnesthesiaNotesDetailsVO AnesthesiaNotesObj
        {
            get { return _AnesthesiaNotesObj; }
            set { _AnesthesiaNotesObj = value; }
        }

        private clsOTDetailsSurgeryDetailsVO _SurgeryNotesObj = new clsOTDetailsSurgeryDetailsVO();
        public clsOTDetailsSurgeryDetailsVO SurgeryNotesObj
        {
            get { return _SurgeryNotesObj; }
            set { _SurgeryNotesObj = value; }
        }

        private List<clsOTDetailsInstructionListDetailsVO> _SurgeryInstructionList = null;
        public List<clsOTDetailsInstructionListDetailsVO> SurgeryInstructionList
        {
            get
            {
                return _SurgeryInstructionList;
            }
            set
            {
                _SurgeryInstructionList = value;
            }
        }

        private List<clsOTDetailsInstructionListDetailsVO> _AnesthesiaInstructionList = null;
        public List<clsOTDetailsInstructionListDetailsVO> AnesthesiaInstructionList
        {
            get
            {
                return _AnesthesiaInstructionList;
            }
            set
            {
                _AnesthesiaInstructionList = value;
            }
        }
    }
    public class clsGetItemDetailsByOTDetailsIDBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetItemDetailsByOTDetailsIDBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public long PatientID { get; set; }
        public long ScheduleId { get; set; }

        private List<clsOTDetailsItemDetailsVO> _ItemList = new List<clsOTDetailsItemDetailsVO>();
        public List<clsOTDetailsItemDetailsVO> ItemList
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

        private List<clsProcedureItemDetailsVO> _ItemList1 = new List<clsProcedureItemDetailsVO>();
        public List<clsProcedureItemDetailsVO> ItemList1
        {
            get
            {
                return _ItemList1;
            }
            set
            {
                _ItemList1 = value;
            }
        }
        public long OTDetailsID { get; set; }

    }


    public class clsGetDocEmpDetailsByOTDetailsIDBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetDocEmpDetailsByOTDetailsIDBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public long PatientID { get; set; }
        public long ScheduleId { get; set; }

        private List<clsOTDetailsDocDetailsVO> _DoctorList = null;
        public List<clsOTDetailsDocDetailsVO> DoctorList
        {
            get { return _DoctorList; }
            set { _DoctorList = value; }
        }

        private List<clsOTDetailsStaffDetailsVO> _StaffList = null;
        public List<clsOTDetailsStaffDetailsVO> StaffList
        {
            get { return _StaffList; }
            set { _StaffList = value; }
        }

        List<MasterListItem> _ProcedureList = new List<MasterListItem>();

        public List<MasterListItem> ProcedureList
        {
            get
            {
                return _ProcedureList;
            }
            set
            {
                if (value != _ProcedureList)
                {
                    _ProcedureList = value;
                }
            }

        }

        public long OTDetailsID { get; set; }
    }

}
