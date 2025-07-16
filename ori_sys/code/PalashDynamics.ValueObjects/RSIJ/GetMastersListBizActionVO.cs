using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.OutPatientDepartment.QueueManagement;
using PalashDynamics.ValueObjects.EMR;
using PalashDynamics.ValueObjects.Inventory;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.IPD;

namespace PalashDynamics.ValueObjects.RSIJ
{
    /// <summary>
    /// Get the 
    /// </summary>
    public class clsGetRSIJMasterListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.RSIJ.Master.clsGetRSIJMasterListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public clsGetRSIJMasterListBizActionVO()
        {

        }
        private MasterTableNameList _MasterTable = MasterTableNameList.None;
        public MasterTableNameList MasterTable
        {
            get
            {
                return _MasterTable;
            }
            set
            {
                _MasterTable = value;
            }
        }

        public string CodeColumn { get; set; }
        public string DescriptionColumn { get; set; }

        private string _Error = "";
        public string Error
        {
            get { return _Error; }
            set { _Error = value; }
        }
        /// <summary>
        /// Gets or Sets MasterList
        /// </summary>
        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }

        public KeyValue Parent { get; set; }

        /// <summary>
        /// Datatype: Boolean
        /// Gets or Sets active record from list
        /// </summary>
        public bool? IsActive { get; set; }
        /// <summary>
        /// Datatype: Boolean
        /// Gets or Sets IsDate 
        /// </summary>
        public bool? IsDate { get; set; }



    }

    /// <summary>
    /// To Get the Doctors list by Department.
    /// </summary>
    public class clsGetRSIJDoctorDepartmentDetailsBizActionVO : IBizActionValueObject
    {
        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }
        public long DoctorTypeID { get; set; }
        public DateTime? Date { get; set; }
        public long UnitId { get; set; }
        public long DepartmentId { get; set; }
        public string DepartmentCode { get; set; }
        public string SpecialCode { get; set; }
        public string DoctorCode { get; set; }
        public bool FromBill { get; set; }
        public bool FromRoster { get; set; }
        public bool AllRecord { get; set; }
        public long ServiceId { get; set; }
        public bool IsServiceWiseDoctorList { get; set; }
        public bool _IsForReferral = false;
        public bool IsForReferral
        {
            get { return _IsForReferral; }
            set { _IsForReferral = value; }
        }


        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.RSIJ.Master.clsGetRSIJDoctorDepartmentDetailsBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    /// <summary>
    /// This lass gives the OPD Patient List.
    /// </summary>
    public class clsGetRSIJQueueListBizActionVO : IBizActionValueObject
    {

        #region RSIJ Changes

        string _SpecializationCode;
        public string SpecializationCode
        {
            get { return _SpecializationCode; }
            set { _SpecializationCode = value; }
        }
        string _DeptCode;
        public string DeptCode
        {
            get { return _DeptCode; }
            set { _DeptCode = value; }
        }
        private string _DoctorCode;
        public string DoctorCode
        {
            get { return _DoctorCode; }
            set { _DoctorCode = value; }
        }

        string _NoReg;
        public string NoReg
        {
            get { return _NoReg; }
            set { _NoReg = value; }
        }
        #endregion
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        private List<clsQueueVO> _QueueList;
        public List<clsQueueVO> QueueList
        {
            get { return _QueueList; }
            set { _QueueList = value; }
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
        long _TariffID;
        public long TariffID
        {
            get { return _TariffID; }
            set { _TariffID = value; }
        }

        string _MRNo;
        public string MRNo
        {
            get { return _MRNo; }
            set { _MRNo = value; }
        }

        private long? _DoctorID;
        public long? DoctorID
        {
            get { return _DoctorID; }
            set { _DoctorID = value; }
        }

        private long? _DepartmentID;
        public long? DepartmentID
        {
            get { return _DepartmentID; }
            set { _DepartmentID = value; }
        }

        private long? _UnitID;
        public long? UnitID
        {
            get { return _UnitID; }
            set { _UnitID = value; }
        }

        public long CurrentVisit { get; set; }
        private string _SearchExpression;
        public string SearchExpression
        {
            get { return _SearchExpression; }
            set { _SearchExpression = value; }
        }

        public string LinkServer { get; set; }
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string sortExpression { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.RSIJ.Master.clsGetRSIJQueueListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    /// <summary>
    /// Fill Diagnosis type ICD9 and ICD10 from ICOPIM and categori table respectively.
    /// </summary>
    public class clsGetRSIJDiagnosisMasterBizactionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.RSIJ.Master.clsGetRSIJDiagnosisMasterBizaction";
        }

        public string ToXml()
        {
            throw new NotImplementedException();
        }
        private List<clsEMRDiagnosisVO> _DiagnosisDetails;
        public List<clsEMRDiagnosisVO> DiagnosisDetails
        {
            get
            {
                return _DiagnosisDetails;
            }

            set
            {
                _DiagnosisDetails = value;
            }

        }

        public long UnitID { get; set; }
        public string Diagnosis { get; set; }

        public string Code { get; set; }
        public bool IsPagingEnabled { get; set; }
        public bool IsICD9 { get; set; }
        public int StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public string SearchExpression { get; set; }
        public int TotalRows { get; set; }
    }

    /// <summary>
    /// To get the Drug details from the RSIJ HIS.
    /// </summary>
    public class clsGetRSIJItemListBizActionVO : IBizActionValueObject
    {

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.RSIJ.Master.clsGetRSIJItemListBizAction";
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


        private clsRSIJItemMasterVO objItemMater = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsRSIJItemMasterVO ItemDetails
        {
            get { return objItemMater; }
            set { objItemMater = value; }
        }

        public List<clsRSIJItemMasterVO> ItemList { get; set; }

        public int TotalRowCount { get; set; }
        public int MaximumRows { get; set; }
        public Boolean PagingEnabled { get; set; }
        public int StartRowIndex { get; set; }
        public string MoleculeCode { get; set; }
        public string BrandName { get; set; }
        public string CompdName { get; set; }
        public bool IsQtyShow { get; set; }
        public bool IsFromCurrentMed { get; set; }
        public string OPDNO { get; set; }
        public bool IsInsuraceDrug { get; set; }
    }

    public class clsRSIJItemMasterVO : IValueObject, INotifyPropertyChanged
    {
        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }


        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        private Boolean _Status;
        public Boolean Status
        {
            get
            {
                return _Status;
            }
            set
            {
                _Status = value;
                OnPropertyChanged("Status");
            }
        }

        private Boolean _Selected;
        public Boolean Selected
        {
            get
            {
                return _Selected;
            }
            set
            {
                _Selected = value;
                OnPropertyChanged("Selected");
            }
        }

        private string _DrugCode;
        public string DrugCode
        {
            get
            {
                return _DrugCode;
            }
            set
            {
                _DrugCode = value;
                OnPropertyChanged("DrugCode");
            }
        }

        private string _DrugName;
        public string DrugName
        {
            get
            {
                return _DrugName;
            }
            set
            {
                _DrugName = value;
                OnPropertyChanged("DrugName");
            }
        }

        private string _MoleculeCode;
        public string MoleculeCode
        {
            get
            {
                return _MoleculeCode;
            }
            set
            {
                _MoleculeCode = value;
                OnPropertyChanged("MoleculeCode");
            }
        }

        private string _MoleculeName;
        public string MoleculeName
        {
            get
            {
                return _MoleculeName;
            }
            set
            {
                _MoleculeName = value;
                OnPropertyChanged("MoleculeName");
            }
        }

        private double _StockQty;
        public double StockQty
        {
            get
            {
                return _StockQty;
            }
            set
            {
                _StockQty = value;
                OnPropertyChanged("StockQty");
            }
        }
    }
    /// <summary>
    /// The method is used to fetch the Laboratory services from the HIS database.
    /// </summary>
    public class clsGetRSIJLaboratoryServiceBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.RSIJ.Master.clsGetRSIJLaboratoryServiceBizAction";
        }

        public string ToXml()
        {
            throw new NotImplementedException();
        }
        private List<clsServiceMasterVO> _ServiceDetails;
        public List<clsServiceMasterVO> ServiceDetails
        {
            get
            {

                return _ServiceDetails;
            }

            set
            {
                _ServiceDetails = value;
            }
        }

        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }

        public long UnitID { get; set; }
        public string Description { get; set; }
        public string ServiceType { get; set; }
        public string ServiceCode { get; set; }
        public string GroupName { get; set; }
        public string ServiceDepartment { get; set; }
        public bool IsPagingEnabled { get; set; }
        public int StartRowIndex { get; set; }
        public long SpecializationID { get; set; }

        public string SpecializationCode { get; set; }
        public string ModalityID { get; set; }
        public long SubSpecializationID { get; set; }
        public string TariffId { get; set; }
        public int MaximumRows { get; set; }
        public string SearchExpression { get; set; }
        public int TotalRows { get; set; }

    }

    public class clsGetRSIJDoctorScheduleTimeVO : IBizActionValueObject
    {

        private List<clsDoctorScheduleDetailsVO> _DoctorScheduleDetails;
        public List<clsDoctorScheduleDetailsVO> DoctorScheduleDetailsList
        {
            get { return _DoctorScheduleDetails; }
            set { _DoctorScheduleDetails = value; }
        }
        public DateTime? Date { get; set; }
        public string DepartmentCode { get; set; }
        public string SpecializationCode { get; set; }
        public string DoctorCode { get; set; }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public string DayID { get; set; }

        public string LinkServer { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsGetRSIJDoctorScheduleTime";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    /// <summary>
    /// Values Object to fetch all the details of DMS Files.
    /// </summary>
    public class clsDMSViewerVisitAdmissionVO : IValueObject, INotifyPropertyChanged
    {
        public string ToXml()
        {
            return this.ToString();
        }

        List<clsDMSViewerVisitAdmissionVO> PathList = new List<clsDMSViewerVisitAdmissionVO>();

        public long ID { get; set; }

        public DateTime Date { get; set; }

        public string DoctorName { get; set; }

        public long FolderID { get; set; }

        public long NoOfFiles { get; set; }

        public string DepartmentName { get; set; }

        public long PatientID { get; set; }

        public long PatientUnitID { get; set; }

        public long OPD_IPD_External { get; set; }

        public long ParentID { get; set; }

        public string ImgName { get; set; }

        public string ImgPath { get; set; }

        public long PatientVisitId { get; set; }


        #region INotifyPropertyChanged Members

        /// <summary>
        /// Implemts the INotifyPropertyChanged interface.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }

    /// <summary>
    /// This Value object is used to get the DMS Files.
    /// </summary>
    public class ClsGetVisitAdmissionBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.RSIJ.Master.ClsGetVisitAdmissionBizAction";
        }
        #endregion

        #region IValueObject Members
        public string ToXml()
        {
            return this.ToString();
        }
        #endregion

        public long PatientID { get; set; }
        public string MRNO { get; set; }

        private List<clsDMSViewerVisitAdmissionVO> _DMSViewerVisitAdmissionList = new List<clsDMSViewerVisitAdmissionVO>();
        public List<clsDMSViewerVisitAdmissionVO> DMSViewerVisitAdmissionList
        {
            get { return _DMSViewerVisitAdmissionList; }
            set
            {
                if (_DMSViewerVisitAdmissionList != value)
                {
                    _DMSViewerVisitAdmissionList = value;
                }

            }
        }
    }

    public class ClsGetImageBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.RSIJ.Master.ClsGetImageBizAction";
        }
        #endregion

        #region IValueObject Members
        public string ToXml()
        {
            return this.ToString();
        }
        #endregion

        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }

        public string ImagePath { get; set; }

        public byte[] ImageByte { get; set; }
    }

}
