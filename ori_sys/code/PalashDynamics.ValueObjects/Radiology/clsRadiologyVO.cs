using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace PalashDynamics.ValueObjects.Radiology
{
    public class clsRadiologyVO : IValueObject, INotifyPropertyChanged
    {
        //private List<MasterListItem> _MasterList = null;
        //public List<MasterListItem> MasterList
        //{
        //    get
        //    { return _MasterList; }

        //    set
        //    { _MasterList = value; }
        //}

        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                if (_ID != value)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }
        private long _Radiologist1;
        public long Radiologist1
        {
            get { return _Radiologist1; }
            set
            {
                if (_Radiologist1 != value)
                {
                    _Radiologist1 = value;
                    OnPropertyChanged("Radiologist1");
                }
            }
        }

        private long _RadSpecilizationID;
        public long RadSpecilizationID
        {
            get { return _RadSpecilizationID; }
            set
            {
                if (_RadSpecilizationID != value)
                {
                    _RadSpecilizationID = value;
                    OnPropertyChanged("RadSpecilizationID");
                }
            }
        }

        private long _TemplateID;
        public long TemplateID
        {
            get { return _TemplateID; }
            set
            {
                if (_TemplateID != value)
                {
                    _TemplateID = value;
                    OnPropertyChanged("TemplateID");
                }
            }
        }


        private long _TestID;
        public long TestID
        {
            get { return _TestID; }
            set
            {
                if (_TestID != value)
                {
                    _TestID = value;
                    OnPropertyChanged("TestID");
                }
            }
        }


        private string _Code;
        public string Code
        {
            get { return _Code; }
            set
            {
                if (_Code != value)
                {
                    _Code = value;
                    //OnPropertyChanged("Code");
                }
            }
        }


        private string _Description;
        public string Description
        {
            get { return _Description; }
            set
            {
                if (_Description != value)
                {
                    _Description = value;
                    //OnPropertyChanged("Description");
                }
            }
        }

        private string _TemplateName;
        public string TemplateName
        {
            get { return _TemplateName; }
            set
            {
                if (_TemplateName != value)
                {
                    _TemplateName = value;
                    OnPropertyChanged("TemplateName");
                }
            }
        }

        private string _TemplateDesign = "";
        public string TemplateDesign
        {
            get { return _TemplateDesign; }
            set
            {
                if (_TemplateDesign != value)
                {
                    _TemplateDesign = value;
                    OnPropertyChanged("TemplateDesign");
                }
            }
        }

        public byte[] Template { get; set; }


        private string _PrintTestName = "";
        public string PrintTestName
        {
            get { return _PrintTestName; }
            set
            {
                if (_PrintTestName != value)
                {
                    _PrintTestName = value;
                    OnPropertyChanged("PrintTestName");
                }
            }
        }

        private long _Radiologist;
        public long Radiologist
        {
            get { return _Radiologist; }
            set
            {
                if (_Radiologist != value)
                {
                    _Radiologist = value;
                    OnPropertyChanged("Radiologist");
                }
            }
        }

        private long _GenderID;
        public long GenderID
        {
            get { return _GenderID; }
            set
            {
                if (_GenderID != value)
                {
                    _GenderID = value;
                    OnPropertyChanged("GenderID");
                }
            }
        }


        private long _TemplateResultID;
        public long TemplateResultID
        {
            get { return _TemplateResultID; }
            set
            {
                if (_TemplateResultID != value)
                {
                    _TemplateResultID = value;
                    OnPropertyChanged("TemplateResultID");
                }
            }
        }


        private DateTime? _Date;
        public DateTime? Date
        {
            get { return _Date; }
            set
            {
                if (_Date != value)
                {
                    _Date = value;
                    OnPropertyChanged("Date");
                }
            }
        }

        private long _CategoryID;
        public long CategoryID
        {
            get { return _CategoryID; }
            set
            {
                if (_CategoryID != value)
                {
                    _CategoryID = value;
                    OnPropertyChanged("CategoryID");
                }
            }
        }


        private long _ServiceID;
        public long ServiceID
        {
            get { return _ServiceID; }
            set
            {
                if (_ServiceID != value)
                {
                    _ServiceID = value;
                    OnPropertyChanged("ServiceID");
                }
            }
        }


        private string _TurnAroundTime = "";
        public string TurnAroundTime
        {
            get { return _TurnAroundTime; }
            set
            {
                if (_TurnAroundTime != value)
                {
                    _TurnAroundTime = value;
                    OnPropertyChanged("TurnAroundTime");
                }
            }
        }



        private string _RadiologistName = "";
        public string RadiologistName
        {
            get { return _RadiologistName; }
            set
            {
                if (_RadiologistName != value)
                {
                    _RadiologistName = value;
                    OnPropertyChanged("RadiologistName");
                }
            }
        }



        private List<clsRadTemplateDetailMasterVO> _TestTemplateList;
        public List<clsRadTemplateDetailMasterVO> TestTemplateList
        {
            get
            {
                if (_TestTemplateList == null)
                    _TestTemplateList = new List<clsRadTemplateDetailMasterVO>();

                return _TestTemplateList;
            }

            set
            {

                _TestTemplateList = value;

            }
        }

        private List<clsRadItemDetailMasterVO> _TestItemList;
        public List<clsRadItemDetailMasterVO> TestItemList
        {
            get
            {
                if (_TestItemList == null)
                    _TestItemList = new List<clsRadItemDetailMasterVO>();

                return _TestItemList;
            }

            set
            {

                _TestItemList = value;

            }
        }

        private List<MasterListItem> _GenderList = new List<MasterListItem>();
        public List<MasterListItem> GenderList
        {
            get
            {
                return _GenderList;
            }
            set
            {
                _GenderList = value;
            }
        }

        #region For Radiology Additions

        private long _ROBDID;
        public long ROBDID
        {
            get { return _ROBDID; }
            set
            {
                if (_ROBDID != value)
                {
                    _ROBDID = value;
                    OnPropertyChanged("ROBDID");
                }
            }
        }
        #endregion

        #region Common Properties


        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set
            {
                if (_UnitID != value)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }

        private string _Unit;
        public string Unit
        {
            get { return _Unit; }
            set
            {
                if (_Unit != value)
                {
                    _Unit = value;
                    OnPropertyChanged("Unit");
                }
            }
        }

        private long _CreatedUnitID;
        public long CreatedUnitId
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitId");
                }
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitId
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitId");
                }
            }
        }

        private bool _Status = true;
        public bool Status
        {
            get { return _Status; }
            set
            {
                if (_Status != value)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        private long? _AddedBy;
        public long? AddedBy
        {
            get { return _AddedBy; }
            set
            {
                if (_AddedBy != value)
                {
                    _AddedBy = value;
                    OnPropertyChanged("AddedBy");
                }
            }
        }

        private string _AddedOn = "";
        public string AddedOn
        {
            get { return _AddedOn; }
            set
            {
                if (_AddedOn != value)
                {
                    _AddedOn = value;
                    OnPropertyChanged("AddedOn");
                }
            }
        }

        private DateTime? _AddedDateTime = DateTime.Now;
        public DateTime? AddedDateTime
        {
            get { return _AddedDateTime; }
            set
            {
                if (_AddedDateTime != value)
                {
                    _AddedDateTime = value;
                    OnPropertyChanged("AddedDateTime");
                }
            }
        }

        private string _AddedWindowsLoginName = "";
        public string AddedWindowsLoginName
        {
            get { return _AddedWindowsLoginName; }
            set
            {
                if (_AddedWindowsLoginName != value)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }

        private long? _UpdatedBy;
        public long? UpdatedBy
        {
            get { return _UpdatedBy; }
            set
            {
                if (_UpdatedBy != value)
                {
                    _UpdatedBy = value;
                    OnPropertyChanged("UpdatedBy");
                }
            }
        }

        private string _UpdatedOn = "";
        public string UpdatedOn
        {
            get { return _UpdatedOn; }
            set
            {
                if (_UpdatedOn != value)
                {
                    _UpdatedOn = value;
                    OnPropertyChanged("UpdatedOn");
                }
            }
        }

        private DateTime? _UpdatedDateTime = DateTime.Now;
        public DateTime? UpdatedDateTime
        {
            get { return _UpdatedDateTime; }
            set
            {
                if (_UpdatedDateTime != value)
                {
                    _UpdatedDateTime = value;
                    OnPropertyChanged("UpdatedDateTime");
                }
            }
        }

        private string _UpdatedWindowsLoginName = "";
        public string UpdatedWindowsLoginName
        {
            get { return _UpdatedWindowsLoginName; }
            set
            {
                if (_UpdatedWindowsLoginName != value)
                {
                    _UpdatedWindowsLoginName = value;
                    OnPropertyChanged("UpdatedWindowsLoginName");
                }
            }
        }

        public List<clsRadiologyVO> RadiologyList { get; set; }//Added By Yogesh K 19 5 16 

        #endregion

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

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }


    public class clsRadTemplateDetailMasterVO : INotifyPropertyChanged, IValueObject
    {

        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                if (value != _ID)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }
        private long? _RadOrderDetailID;
        public long? RadOrderDetailID
        {
            get { return _RadOrderDetailID; }
            set
            {
                if (_RadOrderDetailID != value)
                {
                    _RadOrderDetailID = value;
                    OnPropertyChanged("RadOrderDetailID");
                }
            }
        }
        private string _Template;
        public string Template
        {
            get { return _Template; }
            set
            {
                if (_Template != value)
                {
                    _Template = value;
                    OnPropertyChanged("Template");
                }
            }
        }

        private long _RadOrderID;
        public long RadOrderID
        {
            get { return _RadOrderID; }
            set
            {
                if (_RadOrderID != value)
                {
                    _RadOrderID = value;
                    OnPropertyChanged("RadOrderID");
                }
            }
        }

        private long _RadPatientReportID;
        public long RadPatientReportID
        {
            get { return _RadPatientReportID; }
            set
            {
                if (_RadPatientReportID != value)
                {
                    _RadPatientReportID = value;
                    OnPropertyChanged("RadPatientReportID");
                }
            }
        }


        private long _TestID;
        public long TestID
        {
            get { return _TestID; }
            set
            {
                if (_TestID != value)
                {
                    _TestID = value;
                    OnPropertyChanged("TestID");
                }
            }
        }

        private long _TestTemplateID;
        public long TestTemplateID
        {
            get { return _TestTemplateID; }
            set
            {
                if (value != _TestTemplateID)
                {
                    _TestTemplateID = value;
                    OnPropertyChanged("TestTemplateID");
                }
            }
        }


        private string _TemplateName;
        public string TemplateName
        {
            get { return _TemplateName; }
            set
            {
                if (_TemplateName != value)
                {
                    _TemplateName = value;
                    OnPropertyChanged("TemplateName");
                }
            }
        }

        private string _TemplateCode;
        public string TemplateCode
        {
            get { return _TemplateCode; }
            set
            {
                if (_TemplateCode != value)
                {
                    _TemplateCode = value;
                    OnPropertyChanged("TemplateCode");
                }
            }
        }

        private long _TemplateTestID;
        public long TemplateTestID
        {
            get { return _TemplateTestID; }
            set
            {
                if (_TemplateTestID != value)
                {
                    _TemplateTestID = value;
                    OnPropertyChanged("TemplateTestID");
                }
            }
        }

        #region CommonFileds



        private bool _Status;
        public bool Status
        {
            get { return _Status; }
            set
            {
                if (value != _Status)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
                }

            }
        }

        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get { return _CreatedUnitID; }
            set
            {
                if (value != _CreatedUnitID)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitID");
                }

            }
        }

        private long _UpdatedUnitID;
        public long UpdatedUnitID
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (value != _UpdatedUnitID)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitID");
                }

            }
        }

        private long _AddedBy;
        public long AddedBy
        {
            get { return _AddedBy; }
            set
            {
                if (value != _AddedBy)
                {
                    _AddedBy = value;
                    OnPropertyChanged("AddedBy");
                }

            }

        }

        private string _AddedOn = "";
        public string AddedOn
        {
            get { return _AddedOn; }
            set
            {
                if (value != _AddedOn)
                {
                    _AddedOn = value;
                    OnPropertyChanged("AddedOn");
                }
            }
        }

        private DateTime? _AddedDateTime;
        public DateTime? AddedDateTime
        {
            get { return _AddedDateTime; }
            set
            {
                if (value != _AddedDateTime)
                {
                    _AddedDateTime = value;
                    OnPropertyChanged("AddedDateTime");
                }
            }

        }

        private string _AddedWindowsLoginName = "";
        public string AddedWindowsLoginName
        {
            get { return _AddedWindowsLoginName; }
            set
            {
                if (value != _AddedWindowsLoginName)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }

        private long _UpdatedBy;
        public long UpdatedBy
        {
            get { return _UpdatedBy; }
            set
            {
                if (value != _UpdatedBy)
                {
                    _UpdatedBy = value;
                    OnPropertyChanged("UpdatedBy");
                }

            }

        }

        private string _UpdatedOn = "";
        public string UpdatedOn
        {
            get { return _UpdatedOn; }
            set
            {
                if (value != _UpdatedOn)
                {
                    _UpdatedOn = value;
                    OnPropertyChanged("UpdatedOn");
                }
            }
        }

        private DateTime? _UpdatedDateTime;
        public DateTime? UpdatedDateTime
        {
            get { return _UpdatedDateTime; }
            set
            {
                if (value != _UpdatedDateTime)
                {
                    _UpdatedDateTime = value;
                    OnPropertyChanged("UpdatedDateTime");
                }
            }

        }

        private string _UpdatedWindowsLoginName = "";
        public string UpdatedWindowsLoginName
        {
            get { return _UpdatedWindowsLoginName; }
            set
            {
                if (value != _UpdatedWindowsLoginName)
                {
                    _UpdatedWindowsLoginName = value;
                    OnPropertyChanged("UpdatedWindowsLoginName");
                }
            }
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

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }


    public class clsRadItemDetailMasterVO : INotifyPropertyChanged, IValueObject
    {
        //---- Properties for M_RadItemDetailsMaster
        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                if (value != _ID)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        private long _ItemID;
        public long ItemID
        {
            get { return _ItemID; }
            set
            {
                if (value != _ItemID)
                {
                    _ItemID = value;
                    OnPropertyChanged("ItemID");
                }
            }
        }
        private string _ItemCategoryString;
        public string ItemCategoryString
        {
            get { return _ItemCategoryString; }
            set
            {
                if (value != _ItemCategoryString)
                {
                    _ItemCategoryString = value;
                    OnPropertyChanged("ItemCategoryString");
                }
            }
        }

        private bool _IsFinalized;
        public bool IsFinalized
        {
            get { return _IsFinalized; }
            set
            {
                if (value != _IsFinalized)
                {
                    _IsFinalized = value;
                    OnPropertyChanged("IsFinalized");
                }
            }
        }

        private long _ItemCategoryID;
        public long ItemCategoryID
        {
            get { return _ItemCategoryID; }
            set
            {
                if (value != _ItemCategoryID)
                {
                    _ItemCategoryID = value;
                    OnPropertyChanged("ItemCategoryID");
                }
            }
        }


        List<MasterListItem> _ItemCategory = new List<MasterListItem>();
        public List<MasterListItem> ItemCategory
        {
            get
            {
                return _ItemCategory;
            }
            set
            {
                if (value != _ItemCategory)
                {
                    _ItemCategory = value;
                }
            }

        }

        MasterListItem _SelectedItemCategory = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedItemCategory
        {
            get
            {
                return _SelectedItemCategory;
            }
            set
            {
                if (value != _SelectedItemCategory)
                {
                    _SelectedItemCategory = value;
                    OnPropertyChanged("SelectedItemCategory");
                }
            }


        }

        private string _ItemName;
        public string ItemName
        {
            get { return _ItemName; }
            set
            {
                if (value != _ItemName)
                {
                    _ItemName = value;
                    OnPropertyChanged("ItemName");
                }
            }
        }


        private double _Quantity;
        public double Quantity
        {
            get { return _Quantity; }
            set
            {
                if (value != _Quantity)
                {
                    _Quantity = value;
                    OnPropertyChanged("Quantity");
                }
            }
        }

        private long _ItemTestID;
        public long ItemTestID
        {
            get { return _ItemTestID; }
            set
            {
                if (_ItemTestID != value)
                {
                    _ItemTestID = value;
                    OnPropertyChanged("ItemTestID");
                }
            }
        }


        //---- Properties for T_RadiologyTestResultItemDetails
        private long _RadOrderID;
        public long RadOrderID
        {
            get { return _RadOrderID; }
            set
            {
                if (value != _RadOrderID)
                {
                    _RadOrderID = value;
                    OnPropertyChanged("RadOrderID");
                }
            }
        }

        private long _TestID;
        public long TestID
        {
            get { return _TestID; }
            set
            {
                if (value != _TestID)
                {
                    _TestID = value;
                    OnPropertyChanged("TestID");
                }
            }
        }


        private long _StoreID;
        public long StoreID
        {
            get { return _StoreID; }
            set
            {
                if (value != _StoreID)
                {
                    _StoreID = value;
                    OnPropertyChanged("StoreID");
                }
            }
        }

        private string _ItemCode;
        public string ItemCode
        {
            get { return _ItemCode; }
            set
            {
                if (value != _ItemCode)
                {
                    _ItemCode = value;
                    OnPropertyChanged("ItemCode");
                }
            }
        }

        private long _BatchID;
        public long BatchID
        {
            get { return _BatchID; }
            set
            {
                if (value != _BatchID)
                {
                    _BatchID = value;
                    OnPropertyChanged("BatchID");
                }
            }
        }

        private string _BatchCode;
        public string BatchCode
        {
            get { return _BatchCode; }
            set
            {
                if (value != _BatchCode)
                {
                    _BatchCode = value;
                    OnPropertyChanged("BatchCode");
                }
            }
        }

        private double _BalanceQuantity;
        public double BalanceQuantity
        {
            get { return _BalanceQuantity; }
            set
            {
                if (value != _BalanceQuantity)
                {
                    _BalanceQuantity = value;
                    OnPropertyChanged("BalanceQuantity");
                }
            }
        }


        private double _IdealQuantity;
        public double IdealQuantity
        {
            get { return _IdealQuantity; }
            set
            {
                if (value != _IdealQuantity)
                {
                    _IdealQuantity = value;
                    OnPropertyChanged("IdealQuantity");
                }
            }
        }


        private double _ActualQantity;
        public double ActualQantity
        {
            get { return _ActualQantity; }
            set
            {
                if (_ActualQantity != value)
                {
                    _ActualQantity = value;
                    OnPropertyChanged("ActualQantity");
                }
            }
        }

        private double _WastageQantity;
        public double WastageQantity
        {
            get { return _WastageQantity; }
            set
            {
                if (_WastageQantity != value)
                {
                    _WastageQantity = value;
                    OnPropertyChanged("WastageQantity");
                }
            }
        }


        private string _Remarks;
        public string Remarks
        {
            get { return _Remarks; }
            set
            {
                if (value != _Remarks)
                {
                    _Remarks = value;
                    OnPropertyChanged("Remarks");
                }
            }
        }

        private DateTime? _ExpiryDate;
        public DateTime? ExpiryDate
        {
            get { return _ExpiryDate; }
            set
            {
                if (value != _ExpiryDate)
                {
                    _ExpiryDate = value;
                    OnPropertyChanged("ExpiryDate");
                }
            }
        }


        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        #region CommonFileds


        private bool _Status;
        public bool Status
        {
            get { return _Status; }
            set
            {
                if (value != _Status)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
                }

            }
        }


        private bool _BatchesRequired;
        public bool BatchesRequired
        {
            get { return _BatchesRequired; }
            set
            {
                if (value != _BatchesRequired)
                {
                    _BatchesRequired = value;
                    OnPropertyChanged("BatchesRequired");
                }

            }
        }

        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get { return _CreatedUnitID; }
            set
            {
                if (value != _CreatedUnitID)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitID");
                }

            }
        }

        private long _UpdatedUnitID;
        public long UpdatedUnitID
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (value != _UpdatedUnitID)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitID");
                }

            }
        }

        private long _AddedBy;
        public long AddedBy
        {
            get { return _AddedBy; }
            set
            {
                if (value != _AddedBy)
                {
                    _AddedBy = value;
                    OnPropertyChanged("AddedBy");
                }

            }

        }

        private string _AddedOn = "";
        public string AddedOn
        {
            get { return _AddedOn; }
            set
            {
                if (value != _AddedOn)
                {
                    _AddedOn = value;
                    OnPropertyChanged("AddedOn");
                }
            }
        }

        private DateTime? _AddedDateTime;
        public DateTime? AddedDateTime
        {
            get { return _AddedDateTime; }
            set
            {
                if (value != _AddedDateTime)
                {
                    _AddedDateTime = value;
                    OnPropertyChanged("AddedDateTime");
                }
            }

        }

        private string _AddedWindowsLoginName = "";
        public string AddedWindowsLoginName
        {
            get { return _AddedWindowsLoginName; }
            set
            {
                if (value != _AddedWindowsLoginName)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }

        private long _UpdatedBy;
        public long UpdatedBy
        {
            get { return _UpdatedBy; }
            set
            {
                if (value != _UpdatedBy)
                {
                    _UpdatedBy = value;
                    OnPropertyChanged("UpdatedBy");
                }

            }

        }

        private string _UpdatedOn = "";
        public string UpdatedOn
        {
            get { return _UpdatedOn; }
            set
            {
                if (value != _UpdatedOn)
                {
                    _UpdatedOn = value;
                    OnPropertyChanged("UpdatedOn");
                }
            }
        }

        private DateTime? _UpdatedDateTime;
        public DateTime? UpdatedDateTime
        {
            get { return _UpdatedDateTime; }
            set
            {
                if (value != _UpdatedDateTime)
                {
                    _UpdatedDateTime = value;
                    OnPropertyChanged("UpdatedDateTime");
                }
            }

        }

        private string _UpdatedWindowsLoginName = "";
        public string UpdatedWindowsLoginName
        {
            get { return _UpdatedWindowsLoginName; }
            set
            {
                if (value != _UpdatedWindowsLoginName)
                {
                    _UpdatedWindowsLoginName = value;
                    OnPropertyChanged("UpdatedWindowsLoginName");
                }
            }
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

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsRadOrderBoRadSpecilizationIDokingVO : INotifyPropertyChanged, IValueObject
    {


        private double _CompanyAmount;
        public double CompanyAmount
        {
            get { return _CompanyAmount; }
            set
            {
                if (_CompanyAmount != value)
                {
                    _CompanyAmount = value;
                    OnPropertyChanged("CompanyAmount");
                }
            }
        }
        private double _PatientAmount;
        public double PatientAmount
        {
            get { return _PatientAmount; }
            set
            {
                if (_PatientAmount != value)
                {
                    _PatientAmount = value;
                    OnPropertyChanged("PatientAmount");
                }
            }
        }
        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                if (_ID != value)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        private long _RadSpecilizationID;
             public long RadSpecilizationID
        {
            get { return _RadSpecilizationID; }
            set
            {
                if (_RadSpecilizationID != value)
                {
                    _RadSpecilizationID = value;
                    OnPropertyChanged("RadSpecilizationID");
                }
            }
        }

        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set
            {
                if (_UnitID != value)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }

        private long _OrderDetailID;
        public long OrderDetailID
        {
            get { return _OrderDetailID; }
            set
            {
                if (_OrderDetailID != value)
                {
                    _OrderDetailID = value;
                    OnPropertyChanged("OrderDetailID");
                }
            }
        }

        private string _OrderNo;
        public string OrderNo
        {
            get { return _OrderNo; }
            set
            {
                if (_OrderNo != value)
                {
                    _OrderNo = value;
                    OnPropertyChanged("OrderNo");
                }
            }
        }

        private string _ContactNO;
        public string ContactNO
        {
            get { return _ContactNO; }
            set
            {
                if (_ContactNO != value)
                {
                    _ContactNO = value;
                    OnPropertyChanged("ContactNO");
                }
            }
        }

        private string _PatientEmailId;
        public string PatientEmailId
        {
            get { return _PatientEmailId; }
            set
            {
                if (_PatientEmailId != value)
                {
                    _PatientEmailId = value;
                    OnPropertyChanged("PatientEmailId");
                }
            }
        }
        private string _DoctorEmailID;
        public string DoctorEmailID
        {
            get { return _DoctorEmailID; }
            set
            {
                if (_DoctorEmailID != value)
                {
                    _DoctorEmailID = value;
                    OnPropertyChanged("DoctorEmailID");
                }
            }
        }

        private string _BillNo;
        public string BillNo
        {
            get { return _BillNo; }
            set
            {
                if (_BillNo != value)
                {
                    _BillNo = value;
                    OnPropertyChanged("BillNo");
                }
            }
        }

        private double _TotalAmount;
        public double TotalAmount
        {
            get { return _TotalAmount; }
            set
            {
                if (_TotalAmount != value)
                {
                    _TotalAmount = value;
                    OnPropertyChanged("TotalAmount");
                }
            }
        }

        private double _PaidAmount;
        public double PaidAmount
        {
            get { return _PaidAmount; }
            set
            {
                if (_PaidAmount != value)
                {
                    _PaidAmount = value;
                    OnPropertyChanged("PaidAmount");
                }
            }
        }

        private double _Balance;
        public double Balance
        {
            get { return _Balance; }
            set
            {
                if (_Balance != value)
                {
                    _Balance = value;
                    OnPropertyChanged("Balance");
                }
            }
        }

        private long _Gender;
        public long Gender
        {
            get { return _Gender; }
            set
            {
                if (_Gender != value)
                {
                    _Gender = value;
                    OnPropertyChanged("Gender");
                }
            }
        }

        private string _ReferredDoctor;
        public string ReferredDoctor
        {
            get { return _ReferredDoctor; }
            set
            {
                if (_ReferredDoctor != value)
                {
                    _ReferredDoctor = value;
                    OnPropertyChanged("ReferredDoctor");
                }
            }
        }


        private long _ReferredDoctorID;
        public long ReferredDoctorID
        {
            get { return _ReferredDoctorID; }
            set
            {
                if (_ReferredDoctorID != value)
                {
                    _ReferredDoctorID = value;
                    OnPropertyChanged("ReferredDoctorID");
                }
            }
        }

        private string _VisitNotes;
        public string VisitNotes
        {
            get { return _VisitNotes; }
            set
            {
                if (_VisitNotes != value)
                {
                    _VisitNotes = value;
                    OnPropertyChanged("VisitNotes");
                }
            }
        }

        private DateTime? _Date;
        public DateTime? Date
        {
            get { return _Date; }
            set
            {
                if (_Date != value)
                {
                    _Date = value;
                    OnPropertyChanged("Date");
                }
            }
        }
        private DateTime? _BillDate;
        public DateTime? BillDate
        {
            get { return _BillDate; }
            set
            {
                if (_BillDate != value)
                {
                    _BillDate = value;
                    OnPropertyChanged("BillDate");
                }
            }
        }
        private long _Opd_Ipd_External_ID;
        public long Opd_Ipd_External_ID
        {
            get { return _Opd_Ipd_External_ID; }
            set
            {
                if (_Opd_Ipd_External_ID != value)
                {
                    _Opd_Ipd_External_ID = value;
                    OnPropertyChanged("Opd_Ipd_External_ID");
                }
            }
        }


        private long _Opd_Ipd_External_UnitID;
        public long Opd_Ipd_External_UnitID
        {
            get { return _Opd_Ipd_External_UnitID; }
            set
            {
                if (_Opd_Ipd_External_UnitID != value)
                {
                    _Opd_Ipd_External_UnitID = value;
                    OnPropertyChanged("Opd_Ipd_External_UnitID");
                }
            }
        }



        private long? _Opd_Ipd_External;
        public long? Opd_Ipd_External
        {
            get { return _Opd_Ipd_External; }
            set
            {
                if (_Opd_Ipd_External != value)
                {
                    _Opd_Ipd_External = value;
                    OnPropertyChanged("Opd_Ipd_External");
                }
            }
        }


        private bool _IsApproved;
        public bool IsApproved
        {
            get { return _IsApproved; }
            set
            {
                if (_IsApproved != value)
                {
                    _IsApproved = value;
                    OnPropertyChanged("IsApproved");
                }
            }
        }

        private bool _IsDelivered;
        public bool IsDelivered
        {
            get { return _IsDelivered; }
            set
            {
                if (_IsDelivered != value)
                {
                    _IsDelivered = value;
                    OnPropertyChanged("IsDelivered");
                }
            }
        }


        private long? _TestType;
        public long? TestType
        {
            get { return _TestType; }
            set
            {
                if (_TestType != value)
                {
                    _TestType = value;
                    OnPropertyChanged("TestType");
                }
            }
        }

        private long _TestID;
        public long TestID
        {
            get { return _TestID; }
            set
            {
                if (_TestID != value)
                {
                    _TestID = value;
                    OnPropertyChanged("TestID");
                }
            }
        }
        private long _ModalityID;
        public long ModalityID
        {
            get { return _ModalityID; }
            set
            {
                if (_ModalityID != value)
                {
                    _ModalityID = value;
                    OnPropertyChanged("ModalityID");
                }
            }
        }

        private string _Modality;
        public string Modality
        {
            get { return _Modality; }
            set
            {
                if (_Modality != value)
                {
                    _Modality = value;
                    OnPropertyChanged("Modality");
                }
            }
        }
        private bool _IsCancelled;
        public bool IsCancelled
        {
            get { return _IsCancelled; }
            set
            {
                if (_IsCancelled != value)
                {
                    _IsCancelled = value;
                    OnPropertyChanged("IsCancelled");
                }
            }
        }
        private bool _IsTechnicianEntry;
        public bool IsTechnicianEntry
        {
            get { return _IsTechnicianEntry; }
            set
            {
                if (_IsTechnicianEntry != value)
                {
                    _IsTechnicianEntry = value;
                    OnPropertyChanged("IsTechnicianEntry");
                }
            }
        }
        private bool _IsTechnicianEntryFinalized;
        public bool IsTechnicianEntryFinalized
        {
            get { return _IsTechnicianEntryFinalized; }
            set
            {
                if (_IsTechnicianEntryFinalized != value)
                {
                    _IsTechnicianEntryFinalized = value;
                    OnPropertyChanged("IsTechnicianEntryFinalized");
                }
            }
        }
        private bool _Sedation;
        public bool Sedation
        {
            get { return _Sedation; }
            set
            {
                if (_Sedation != value)
                {
                    _Sedation = value;
                    OnPropertyChanged("Sedation");
                }
            }
        }

        private bool _Contrast;
        public bool Contrast
        {
            get { return _Contrast; }
            set
            {
                if (_Contrast != value)
                {
                    _Contrast = value;
                    OnPropertyChanged("Contrast");
                }
            }
        }

        private string _ContrastDetails;
        public string ContrastDetails
        {
            get { return _ContrastDetails; }
            set
            {
                if (_ContrastDetails != value)
                {
                    _ContrastDetails = value;
                    OnPropertyChanged("ContrastDetails");
                }
            }
        }

        private string _FirstName;
        public string FirstName
        {
            get { return _FirstName; }
            set
            {
                if (_FirstName != value)
                {
                    _FirstName = value;
                    OnPropertyChanged("FirstName");
                }
            }
        }

        private string _MiddleName;
        public string MiddleName
        {
            get { return _MiddleName; }
            set
            {
                if (_MiddleName != value)
                {
                    _MiddleName = value;
                    OnPropertyChanged("MiddleName");
                }
            }
        }

        private string _EmailID;
        public string EmailID
        {
            get { return _EmailID; }
            set
            {
                if (_EmailID != value)
                {
                    _EmailID = value;
                    OnPropertyChanged("EmailID");
                }
            }
        }

        private string _LastName;
        public string LastName
        {
            get { return _LastName; }
            set
            {
                if (_LastName != value)
                {
                    _LastName = value;
                    OnPropertyChanged("LastName");
                }
            }
        }
        private bool _FilmWastage;
        public bool FilmWastage
        {
            get { return _FilmWastage; }
            set
            {
                if (_FilmWastage != value)
                {
                    _FilmWastage = value;
                    OnPropertyChanged("FilmWastage");
                }
            }
        }
        private string _FilmWastageDetails;
        public string FilmWastageDetails
        {
            get { return _FilmWastageDetails; }
            set
            {
                if (_FilmWastageDetails != value)
                {
                    _FilmWastageDetails = value;
                    OnPropertyChanged("FilmWastageDetails");
                }
            }
        }

        private string _GenderName;
        public string GenderName
        {
            get { return _GenderName; }
            set
            {
                if (_GenderName != value)
                {
                    _GenderName = value;
                    OnPropertyChanged("GenderName");
                }
            }
        }

        private long _NoOfFilms;
        public long NoOfFilms
        {
            get { return _NoOfFilms; }
            set
            {
                if (_NoOfFilms != value)
                {
                    _NoOfFilms = value;
                    OnPropertyChanged("NoOfFilms");
                }
            }
        }
        //Added By Somanath on 28/01/2012 as per Priyanka mam

        private long _PaymentModeID;
        public long PaymentModeID
        {
            get { return _PaymentModeID; }
            set
            {
                if (_PaymentModeID != value)
                {
                    _PaymentModeID = value;
                    OnPropertyChanged("PaymentModeID");
                }
            }
        }
        private long _ServiceID;
        public long ServiceID
        {
            get { return _ServiceID; }
            set
            {
                if (_ServiceID != value)
                {
                    _ServiceID = value;
                    OnPropertyChanged("ServiceID");
                }
            }
        }
        private long _BillID;
        public long BillID
        {
            get { return _BillID; }
            set
            {
                if (_BillID != value)
                {
                    _BillID = value;
                    OnPropertyChanged("BillID");
                }
            }
        }
        private long _ChargeID;
        public long ChargeID
        {
            get { return _ChargeID; }
            set
            {
                if (_ChargeID != value)
                {
                    _ChargeID = value;
                    OnPropertyChanged("ChargeID");
                }
            }
        }

        private int _Age;
        public int Age
        {
            get { return _Age; }
            set
            {
                if (_Age != value)
                {
                    _Age = value;
                    OnPropertyChanged("Age");
                }
            }
        }

        private string _AgeINDDMMYYYY;
        public string AgeINDDMMYYYY
        {
            get { return _AgeINDDMMYYYY; }
            set
            {
                if (_AgeINDDMMYYYY != value)
                {
                    _AgeINDDMMYYYY = value;
                    OnPropertyChanged("AgeINDDMMYYYY");
                }
            }
        }
        private string _PaymentMode;
        public string PaymentMode
        {
            get { return _PaymentMode; }
            set
            {
                if (_PaymentMode != value)
                {
                    _PaymentMode = value;
                    OnPropertyChanged("PaymentMode");
                }
            }
        }

        private DateTime _DateOfBirth;
        public DateTime DateOfBirth
        {
            get { return _DateOfBirth; }
            set
            {
                if (_DateOfBirth != value)
                {
                    _DateOfBirth = value;
                    OnPropertyChanged("DateOfBirth");
                }
            }
        }


        private long _PatientWeight;
        public long PatientWeight
        {
            get { return _PatientWeight; }
            set
            {
                if (_PatientWeight != value)
                {
                    _PatientWeight = value;
                    OnPropertyChanged("PatientWeight");
                }
            }
        }

        private long _AgeInYears;
        public long AgeInYears
        {
            get { return _AgeInYears; }
            set
            {
                if (_AgeInYears != value)
                {
                    _AgeInYears = value;
                    OnPropertyChanged("AgeInYears");
                }
            }
        }

        private long _PatientHeight;
        public long PatientHeight
        {
            get { return _PatientHeight; }
            set
            {
                if (_PatientHeight != value)
                {
                    _PatientHeight = value;
                    OnPropertyChanged("PatientHeight");
                }
            }
        }

        private string _ContactNo1;
        public string ContactNo1
        {
            get { return _ContactNo1; }
            set
            {
                if (_ContactNo1 != value)
                {
                    _ContactNo1 = value;
                    OnPropertyChanged("ContactNo1");
                }
            }
        }

        private string _Service;
        public string Service
        {
            get { return _Service; }
            set
            {
                if (_Service != value)
                {
                    _Service = value;
                    OnPropertyChanged("Service");
                }
            }
        }
        //End
        private string _MRNo;
        public string MRNo
        {
            get { return _MRNo; }
            set
            {
                if (_MRNo != value)
                {
                    _MRNo = value;
                    OnPropertyChanged("MRNo");
                }

            }
        }
        private string _OPDNO;
        public string OPDNO
        {
            get { return _OPDNO; }
            set
            {
                if (_OPDNO != value)
                {
                    _OPDNO = value;
                    OnPropertyChanged("OPDNO");
                }

            }
        }
        private string _ScanNo;
        public string ScanNo
        {
            get { return _ScanNo; }
            set
            {
                if (_ScanNo != value)
                {
                    _ScanNo = value;
                    OnPropertyChanged("ScanNo");
                }

            }
        }
        private long _PatientID;
        public long PatientID
        {
            get { return _PatientID; }
            set
            {
                if (_PatientID != value)
                {
                    _PatientID = value;
                    OnPropertyChanged("PatientID");
                }
            }
        }


        private string _PatientName;
        public string PatientName
        {
            get { return _PatientName; }
            set
            {
                if (_PatientName != value)
                {
                    _PatientName = value;
                    OnPropertyChanged("PatientName");
                }

            }
        }


        private long _PatientUnitId;
        public long PatientUnitId
        {
            get { return _PatientUnitId; }
            set
            {
                if (_PatientUnitId != value)
                {
                    _PatientUnitId = value;
                    OnPropertyChanged("PatientUnitId");
                }

            }
        }
        private bool _IsResultEntry;
        public bool IsResultEntry
        {
            get { return _IsResultEntry; }
            set
            {
                if (_IsResultEntry != value)
                {
                    _IsResultEntry = value;
                    OnPropertyChanged("IsResultEntry");
                }
            }
        }
        private bool _IsFinalized;
        public bool IsFinalized
        {
            get { return _IsFinalized; }
            set
            {
                if (_IsFinalized != value)
                {
                    _IsFinalized = value;
                    OnPropertyChanged("IsFinalized");
                }
            }
        }

        private List<clsRadOrderBookingDetailsVO> _ObjDetails = new List<clsRadOrderBookingDetailsVO>();
        public List<clsRadOrderBookingDetailsVO> OrderBookingDetails
        {
            get { return _ObjDetails; }
            set
            {

                _ObjDetails = value;
                OnPropertyChanged("OrderBookingDetails");

            }
        }



        #region Common Properties

        private string _LinkServer;
        public string LinkServer
        {
            get { return _LinkServer; }
            set
            {
                if (_LinkServer != value)
                {
                    _LinkServer = value;
                    OnPropertyChanged("LinkServer");
                }
            }
        }


        private string _UnitName;
        public string UnitName
        {
            get { return _UnitName; }
            set
            {
                if (_UnitName != value)
                {
                    _UnitName = value;
                    OnPropertyChanged("UnitName");
                }
            }
        }

        private long _CreatedUnitID;
        public long CreatedUnitId
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitId");
                }
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitId
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitId");
                }
            }
        }

        private bool _Status = true;
        public bool Status
        {
            get { return _Status; }
            set
            {
                if (_Status != value)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        private long? _AddedBy;
        public long? AddedBy
        {
            get { return _AddedBy; }
            set
            {
                if (_AddedBy != value)
                {
                    _AddedBy = value;
                    OnPropertyChanged("AddedBy");
                }
            }
        }

        private string _AddedOn = "";
        public string AddedOn
        {
            get { return _AddedOn; }
            set
            {
                if (_AddedOn != value)
                {
                    _AddedOn = value;
                    OnPropertyChanged("AddedOn");
                }
            }
        }

        private DateTime? _AddedDateTime = DateTime.Now;
        public DateTime? AddedDateTime
        {
            get { return _AddedDateTime; }
            set
            {
                if (_AddedDateTime != value)
                {
                    _AddedDateTime = value;
                    OnPropertyChanged("AddedDateTime");
                }
            }
        }

        private string _AddedWindowsLoginName = "";
        public string AddedWindowsLoginName
        {
            get { return _AddedWindowsLoginName; }
            set
            {
                if (_AddedWindowsLoginName != value)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }

        private long? _UpdatedBy;
        public long? UpdatedBy
        {
            get { return _UpdatedBy; }
            set
            {
                if (_UpdatedBy != value)
                {
                    _UpdatedBy = value;
                    OnPropertyChanged("UpdatedBy");
                }
            }
        }

        private string _UpdatedOn = "";
        public string UpdatedOn
        {
            get { return _UpdatedOn; }
            set
            {
                if (_UpdatedOn != value)
                {
                    _UpdatedOn = value;
                    OnPropertyChanged("UpdatedOn");
                }
            }
        }

        private DateTime? _UpdatedDateTime = DateTime.Now;
        public DateTime? UpdatedDateTime
        {
            get { return _UpdatedDateTime; }
            set
            {
                if (_UpdatedDateTime != value)
                {
                    _UpdatedDateTime = value;
                    OnPropertyChanged("UpdatedDateTime");
                }
            }
        }

        private string _UpdatedWindowsLoginName = "";
        public string UpdatedWindowsLoginName
        {
            get { return _UpdatedWindowsLoginName; }
            set
            {
                if (_UpdatedWindowsLoginName != value)
                {
                    _UpdatedWindowsLoginName = value;
                    OnPropertyChanged("UpdatedWindowsLoginName");
                }
            }
        }



        #endregion

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

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsRadOrderBookingDetailsVO : INotifyPropertyChanged, IValueObject
    {
        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }               //Added By Yogesh K 18 5 16




        private string _SourceURL;
        public string SourceURL
        {
            get { return _SourceURL; }
            set
            {
                if (_SourceURL != value)
                {
                    _SourceURL = value;
                    OnPropertyChanged("SourceURL");
                }
            }
        }
        private bool _IsUpload;
        public bool IsUpload
        {
            get { return _IsUpload; }
            set
            {
                _IsUpload = value;

            }
        }

        private string _ResultEntryImage;
        public string ResultEntryImage
        {
            get { return _ResultEntryImage; }
            set
            {
                if (_ResultEntryImage != value)
                {
                    _ResultEntryImage = value;
                    OnPropertyChanged("ResultEntryImage");
                }
            }
        }

        private string _TechnicianEntryImage;
        public string TechnicianEntryImage
        {
            get { return _TechnicianEntryImage; }
            set
            {
                if (_TechnicianEntryImage != value)
                {
                    _TechnicianEntryImage = value;
                    OnPropertyChanged("TechnicianEntryImage");
                }
            }
        }

        private string _TechnicianEntryFinalImage;
        public string TechnicianEntryFinalImage
        {
            get { return _TechnicianEntryFinalImage; }
            set
            {
                if (_TechnicianEntryFinalImage != value)
                {
                    _TechnicianEntryFinalImage = value;
                    OnPropertyChanged("TechnicianEntryFinalImage");
                }
            }
        }

        private string _ReportDeliveredImage;
        public string ReportDeliveredImage
        {
            get { return _ReportDeliveredImage; }
            set
            {
                if (_ReportDeliveredImage != value)
                {
                    _ReportDeliveredImage = value;
                    OnPropertyChanged("ReportDeliveredImage");
                }
            }
        }


        private string _TestCancelImage;
        public string TestCancelImage
        {
            get { return _TestCancelImage; }
            set
            {
                if (_TestCancelImage != value)
                {
                    _TestCancelImage = value;
                    OnPropertyChanged("TestCancelImage");
                }
            }
        }

        private string _ContrastGivenImage;
        public string ContrastGivenImage
        {
            get { return _ContrastGivenImage; }
            set
            {
                if (_ContrastGivenImage != value)
                {
                    _ContrastGivenImage = value;
                    OnPropertyChanged("ContrastGivenImage");
                }
            }
        }
        private string _SedationGivenImage;
        public string SedationGivenImage
        {
            get { return _SedationGivenImage; }
            set
            {
                if (_SedationGivenImage != value)
                {
                    _SedationGivenImage = value;
                    OnPropertyChanged("SedationGivenImage");
                }
            }
        }

        private string _ResultEntryFinalImage;
        public string ResultEntryFinalImage
        {
            get { return _ResultEntryFinalImage; }
            set
            {
                if (_ResultEntryFinalImage != value)
                {
                    _ResultEntryFinalImage = value;
                    OnPropertyChanged("ResultEntryFinalImage");
                }
            }
        }

        private long _RadTechnicianEntryID;
        public long RadTechnicianEntryID
        {
            get { return _RadTechnicianEntryID; }
            set
            {
                if (_RadTechnicianEntryID != value)
                {
                    _RadOrderID = value;
                    OnPropertyChanged("RadTechnicianEntryID");
                }
            }
        }

        private string _FinalizedImage;
        public string FinalizedImage
        {
            get { return _FinalizedImage; }
            set
            {
                if (_FinalizedImage != value)
                {
                    _FinalizedImage = value;
                    OnPropertyChanged("FinalizedImage");
                }
            }
        }


        private string _ReportUploadImage;// Added By Yogesh 31 5 2016
        public string ReportUploadImage
        {
            get { return _ReportUploadImage; }
            set
            {
                if (_ReportUploadImage != value)
                {
                    _ReportUploadImage = value;
                    OnPropertyChanged("ReportUploadImage");
                }
            }
        }

        private string _ReportUploadPath;// Added By Yogesh 9 6 2016
        public string ReportUploadPath
        {
            get { return _ReportUploadPath; }
            set
            {
                if (_ReportUploadPath != value)
                {
                    _ReportUploadPath = value;
                    OnPropertyChanged("ReportUploadPath");
                }
            }
        }

       


        private DateTime? _VisitDate;
        public DateTime? VisitDate
        {
            get { return _VisitDate; }
            set
            {
                if (_VisitDate != value)
                {
                    _VisitDate = value;
                    OnPropertyChanged("VisitDate");
                }
            }
        }
        public string FileName { get; set; }

        private byte[] _Report;
        public byte[] Report
        {
            get { return _Report; }
            set
            {
                if (_Report != value)
                {
                    _Report = value;
                    OnPropertyChanged("Report");
                }
            }
        }

        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                if (_ID != value)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }
        private bool _IsOutSourced;
        public bool IsOutSourced
        {
            get { return _IsOutSourced; }
            set
            {
                if (_IsOutSourced != value)
                {
                    _IsOutSourced = value;
                    OnPropertyChanged("IsOutSourced");
                }
            }
        }

        private bool _IsOutSourcedEnabled;
        public bool IsOutSourcedEnabled
        {
            get { return _IsOutSourcedEnabled; }
            set
            {
                if (_IsOutSourcedEnabled != value)
                {
                    _IsOutSourcedEnabled = value;
                    OnPropertyChanged("IsOutSourcedEnabled");
                }
            }
        }

        private long? _AgencyID;
        public long? AgencyID
        {
            get { return _AgencyID; }
            set
            {
                if (_AgencyID != value)
                {
                    _AgencyID = value;
                    OnPropertyChanged("AgencyID");
                }
            }
        }

        private string _AgencyName;
        public string AgencyName
        {
            get { return _AgencyName; }
            set
            {
                if (_AgencyName != value)
                {
                    _AgencyName = value;
                    OnPropertyChanged("AgencyName");
                }
            }
        }

        private bool _IsReportCollected;
        public bool IsReportCollected
        {
            get { return _IsReportCollected; }
            set
            {
                if (_IsReportCollected != value)
                {
                    _IsReportCollected = value;
                    OnPropertyChanged("IsReportCollected");
                }
                IsChecked = !_IsReportCollected;
            }
        }

        private bool _IsChecked;
        public bool IsChecked
        {
            get { return _IsChecked; }
            set
            {
                if (_IsChecked != value)
                {
                    _IsChecked = value;
                    OnPropertyChanged("IsChecked");
                }
            }
        }

        private DateTime? _ReportCollected;
        public DateTime? ReportCollected
        {
            get { return _ReportCollected; }
            set
            {
                if (_ReportCollected != value)
                {
                    _ReportCollected = value;
                    OnPropertyChanged("ReportCollected");
                }
            }
        }

        private DateTime _ReportCollectedDateTime;
        public DateTime ReportCollectedDateTime
        {
            get { return _ReportCollectedDateTime; }
            set
            {
                if (_ReportCollectedDateTime != value)
                {
                    _ReportCollectedDateTime = value;
                    OnPropertyChanged("ReportCollectedDateTime");
                }
            }
        }

        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set
            {
                if (_UnitID != value)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }
        private long _ResultEntryID;
        public long ResultEntryID
        {
            get { return _ResultEntryID; }
            set
            {
                if (_ResultEntryID != value)
                {
                    _ResultEntryID = value;
                    OnPropertyChanged("ResultEntryID");
                }
            }
        }

        private string _FirstName = "";
        //[Required(ErrorMessage = "First Name Required")]
        //[StringLength(50, ErrorMessage = "First Name must be in between 1 to 50 Characters")]
        public string FirstName
        {
            get
            {
                return _FirstName;
            }
            set
            {
                if (_FirstName != value)
                {
                    _FirstName = value;
                    OnPropertyChanged("FirstName");
                }
            }
        }

        private string _MiddleName = "";
        public string MiddleName
        {
            get
            {
                return _MiddleName;
            }
            set
            {
                if (_MiddleName != value)
                {
                    _MiddleName = value;
                    OnPropertyChanged("MiddleName");
                }
            }
        }

        private string strPatientName = "";
        public string PatientName
        {
            get { return strPatientName = _FirstName + " " + _MiddleName + " " + _LastName; }
            set
            {
                if (value != strPatientName)
                {
                    strPatientName = value;
                    OnPropertyChanged("PatientName");
                }
            }
        }

        private string _PatientFullName = string.Empty;
        public string PatientFullName
        {
            get { return _PatientFullName; }
            set
            {
                if (value != _PatientFullName)
                {
                    _PatientFullName = value;
                    OnPropertyChanged("PatientFullName");
                }
            }
        }

        private string _BillNo;
        public string BillNo
        {
            get { return _BillNo; }
            set
            {
                if (_BillNo != value)
                {
                    _BillNo = value;
                    OnPropertyChanged("BillNo");
                }
            }
        }
        private string _LastName = "";
        public string LastName
        {
            get { return _LastName; }
            set
            {
                if (_LastName != value)
                {
                    _LastName = value;
                    OnPropertyChanged("LastName");
                }
            }
        }

        private long _RadOrderID;
        public long RadOrderID
        {
            get { return _RadOrderID; }
            set
            {
                if (_RadOrderID != value)
                {
                    _RadOrderID = value;
                    OnPropertyChanged("RadOrderID");
                }
            }
        }

        private long _RadOrderDetailID;
        public long RadOrderDetailID
        {
            get { return _RadOrderDetailID; }
            set
            {
                if (_RadOrderDetailID != value)
                {
                    _RadOrderDetailID = value;
                    OnPropertyChanged("RadOrderDetailID");
                }
            }
        }

        private long _RadPatientReportID;
        public long RadPatientReportID
        {
            get { return _RadPatientReportID; }
            set
            {
                if (_RadPatientReportID != value)
                {
                    _RadPatientReportID = value;
                    OnPropertyChanged("RadPatientReportID");
                }
            }
        }


        private long _TestID;
        public long TestID
        {
            get { return _TestID; }
            set
            {
                if (_TestID != value)
                {
                    _TestID = value;
                    OnPropertyChanged("TestID");
                }
            }
        }

        private string _TestCode;
        public string TestCode
        {
            get { return _TestCode; }
            set
            {
                if (_TestCode != value)
                {
                    _TestCode = value;
                    OnPropertyChanged("TestCode");
                }
            }
        }

        private bool _IsDigitalSignatureRequired;
        public bool IsDigitalSignatureRequired
        {
            get { return _IsDigitalSignatureRequired; }
            set
            {
                if (_IsDigitalSignatureRequired != value)
                {
                    _IsDigitalSignatureRequired = value;
                    //OnPropertyChanged("IsDigitalSignatureRequired");
                }
            }
        }

        private string _TestName;
        public string TestName
        {
            get { return _TestName; }
            set
            {
                if (_TestName != value)
                {
                    _TestName = value;
                    OnPropertyChanged("TestName");
                }
            }
        }

        private string _ShortDescription;
        public string ShortDescription
        {
            get { return _ShortDescription; }
            set
            {
                if (_ShortDescription != value)
                {
                    _ShortDescription = value;
                    OnPropertyChanged("ShortDescription");
                }
            }
        }

        private string _LongDescription;
        public string LongDescription
        {
            get { return _LongDescription; }
            set
            {
                if (_LongDescription != value)
                {
                    _LongDescription = value;
                    OnPropertyChanged("LongDescription");
                }
            }
        }

        private long _ChargeID;
        public long ChargeID
        {
            get { return _ChargeID; }
            set
            {
                if (_ChargeID != value)
                {
                    _ChargeID = value;
                    OnPropertyChanged("ChargeID");
                }
            }
        }

        private long _TariffServiceID;
        public long TariffServiceID
        {
            get { return _TariffServiceID; }
            set
            {
                if (_TariffServiceID != value)
                {
                    _TariffServiceID = value;
                    OnPropertyChanged("TariffServiceID");
                }
            }
        }
        public long RadiologySpecilizationID { get; set; }
        private long _ServiceID;
        public long ServiceID
        {
            get { return _ServiceID; }
            set
            {
                if (_ServiceID != value)
                {
                    _ServiceID = value;
                    OnPropertyChanged("ServiceID");
                }
            }
        }

        private string _ServiceName;
        public string ServiceName
        {
            get { return _ServiceName; }
            set
            {
                if (_ServiceName != value)
                {
                    _ServiceName = value;
                    OnPropertyChanged("ServiceName");
                }
            }
        }


        private string _Number = "";
        public string Number
        {
            get { return _Number; }
            set
            {
                if (_Number != value)
                {
                    _Number = value;
                    OnPropertyChanged("Number");
                }
            }
        }

        private bool _IsEmergency;
        public bool IsEmergency
        {
            get { return _IsEmergency; }
            set
            {
                if (_IsEmergency != value)
                {
                    _IsEmergency = value;
                    OnPropertyChanged("IsEmergency");
                }
            }
        }


        private long? _DoctorID;
        public long? DoctorID
        {
            get { return _DoctorID; }
            set
            {
                if (_DoctorID != value)
                {
                    _DoctorID = value;
                    OnPropertyChanged("DoctorID");
                }
            }
        }

        private bool _IsApproved;
        public bool IsApproved
        {
            get { return _IsApproved; }
            set
            {
                if (_IsApproved != value)
                {
                    _IsApproved = value;
                    OnPropertyChanged("IsApproved");
                }
            }
        }

        private bool _IsCompleted;
        public bool IsCompleted
        {
            get { return _IsCompleted; }
            set
            {
                if (_IsCompleted != value)
                {
                    _IsCompleted = value;
                    OnPropertyChanged("IsCompleted");
                }
            }
        }

        private bool _IsDelivered;
        public bool IsDelivered
        {
            get { return _IsDelivered; }
            set
            {
                if (_IsDelivered != value)
                {
                    _IsDelivered = value;
                    OnPropertyChanged("IsDelivered");
                }
            }
        }

        private bool _IsCancelled;
        public bool IsCancelled
        {
            get { return _IsCancelled; }
            set
            {
                if (_IsCancelled != value)
                {
                    _IsCancelled = value;
                    OnPropertyChanged("IsCancelled");
                }
            }
        }


        private bool _IsFinalized;
        public bool IsFinalized
        {
            get { return _IsFinalized; }
            set
            {
                if (_IsFinalized != value)
                {
                    _IsFinalized = value;
                    OnPropertyChanged("IsFinalized");
                }
            }
        }

        private bool _IsResultEntry;
        public bool IsResultEntry
        {
            get { return _IsResultEntry; }
            set
            {
                if (_IsResultEntry != value)
                {
                    _IsResultEntry = value;
                    OnPropertyChanged("IsResultEntry");
                }
            }
        }
        private bool _IsPrinted;
        public bool IsPrinted
        {
            get { return _IsPrinted; }
            set
            {
                if (_IsPrinted != value)
                {
                    _IsPrinted = value;
                    OnPropertyChanged("IsPrinted");
                }
            }
        }

        private long? _MicrobiologistID;
        public long? MicrobiologistID
        {
            get { return _MicrobiologistID; }
            set
            {
                if (_MicrobiologistID != value)
                {
                    _MicrobiologistID = value;
                    OnPropertyChanged("MicrobiologistID");
                }
            }
        }

        private long _TemplateID;
        public long TemplateID
        {
            get { return _TemplateID; }
            set
            {
                if (_TemplateID != value)
                {
                    _TemplateID = value;
                    OnPropertyChanged("TemplateID");
                }
            }
        }

        private string _ReferredBy;
        public string ReferredBy
        {
            get { return _ReferredBy; }
            set
            {
                if (_ReferredBy != value)
                {
                    _ReferredBy = value;
                    OnPropertyChanged("ReferredBy");
                }
            }
        }

        private long _ModalityID;
        public long ModalityID
        {
            get { return _ModalityID; }
            set
            {
                if (_ModalityID != value)
                {
                    _ModalityID = value;
                    OnPropertyChanged("ModalityID");
                }
            }
        }

        private string _Modality;
        public string Modality
        {
            get { return _Modality; }
            set
            {
                if (_Modality != value)
                {
                    _Modality = value;
                    OnPropertyChanged("Modality"); //Added By Yogesh k 18 5 16
                }
            }
        }

        private string _PrescribingDr;
        public string PrescribingDr
        {
            get { return _PrescribingDr; }
            set
            {
                if (_PrescribingDr != value)
                {
                    _PrescribingDr = value;
                    OnPropertyChanged("PrescribingDr"); //Added By Yogesh k 26 9 16
                }
            }
        }

        private string _Radiologist;
        public string Radiologist
        {
            get { return _Radiologist; }
            set
            {
                if (_Radiologist != value)
                {
                    _Radiologist = value;
                    OnPropertyChanged("Radiologist");//Added By Yogesh k 18 5 16
                }
            }
        }




        private long _RadiologistID1;
        public long RadiologistID1
        {
            get { return _RadiologistID1; }
            set
            {
                if (_RadiologistID1 != value)
                {
                    _RadiologistID1 = value;
                    OnPropertyChanged("RadiologistID1");//Added By Yogesh k 18 5 16
                }
            }
        }

        private DateTime? _ReportUploadDateTime = DateTime.Now;
        public DateTime? ReportUploadDateTime
        {
            get { return _ReportUploadDateTime; }
            set
            {
                if (_ReportUploadDateTime != value)
                {
                    _ReportUploadDateTime = value;
                    OnPropertyChanged("ReportUploadDateTime");
                }
            }
        }


        private DateTime? _DeliveryReportDateTime = DateTime.Now;
        public DateTime? DeliveryReportDateTime
        {
            get { return _DeliveryReportDateTime; }
            set
            {
                if (_DeliveryReportDateTime != value)
                {
                    _DeliveryReportDateTime = value;
                    OnPropertyChanged("DeliveryReportDateTime");
                }
            }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private string _ContrastDetails;
        public string ContrastDetails
        {
            get { return _ContrastDetails; }
            set
            {
                if (_ContrastDetails != value)
                {
                    _ContrastDetails = value;
                    OnPropertyChanged("ContrastDetails");
                }
            }
        }
        private long _NoOfFilms;
        public long NoOfFilms
        {
            get { return _NoOfFilms; }
            set
            {
                if (_NoOfFilms != value)
                {
                    _NoOfFilms = value;
                    OnPropertyChanged("NoOfFilms");
                }
            }
        }


        private bool _Sedation;
        public bool Sedation
        {
            get { return _Sedation; }
            set
            {
                if (_Sedation != value)
                {
                    _Sedation = value;
                    OnPropertyChanged("Sedation");
                }
            }
        }
        private string _FilmWastageDetails;
        public string FilmWastageDetails
        {
            get { return _FilmWastageDetails; }
            set
            {
                if (_FilmWastageDetails != value)
                {
                    _FilmWastageDetails = value;
                    OnPropertyChanged("FilmWastageDetails");
                }
            }
        }
        private bool _Contrast;
        public bool Contrast
        {
            get { return _Contrast; }
            set
            {
                if (_Contrast != value)
                {
                    _Contrast = value;
                    OnPropertyChanged("Contrast");
                }
            }
        }

        private bool _IsTechnicianEntry;
        public bool IsTechnicianEntry
        {
            get { return _IsTechnicianEntry; }
            set
            {
                if (_IsTechnicianEntry != value)
                {
                    _IsTechnicianEntry = value;
                    OnPropertyChanged("IsTechnicianEntry");
                }
            }
        }

        private bool _FilmWastage;
        public bool FilmWastage
        {
            get { return _FilmWastage; }
            set
            {
                if (_FilmWastage != value)
                {
                    _FilmWastage = value;
                    OnPropertyChanged("FilmWastage");
                }
            }
        }

        private bool _IsTechnicianEntryFinalized;
        public bool IsTechnicianEntryFinalized
        {
            get { return _IsTechnicianEntryFinalized; }
            set
            {
                if (_IsTechnicianEntryFinalized != value)
                {
                    _IsTechnicianEntryFinalized = value;
                    OnPropertyChanged("IsTechnicianEntryFinalized");
                }
            }
        }
        //Addded By Bhushanp 26052017
        private long _Opd_Ipd_External;

        public long Opd_Ipd_External
        {
            get { return _Opd_Ipd_External; }
            set
            {
                if (_Opd_Ipd_External != value)
                {
                    _Opd_Ipd_External = value;
                    OnPropertyChanged("Opd_Ipd_External");
                }
            }
        }

        #region Common Properties



        private string _UnitName;
        public string UnitName
        {
            get { return _UnitName; }
            set
            {
                if (_UnitName != value)
                {
                    _UnitName = value;
                    OnPropertyChanged("UnitName");
                }
            }
        }

        private long _CreatedUnitID;
        public long CreatedUnitId
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitId");
                }
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitId
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitId");
                }
            }
        }

        private bool _Status = true;
        public bool Status
        {
            get { return _Status; }
            set
            {
                if (_Status != value)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        private long? _AddedBy;
        public long? AddedBy
        {
            get { return _AddedBy; }
            set
            {
                if (_AddedBy != value)
                {
                    _AddedBy = value;
                    OnPropertyChanged("AddedBy");
                }
            }
        }

        private string _AddedOn = "";
        public string AddedOn
        {
            get { return _AddedOn; }
            set
            {
                if (_AddedOn != value)
                {
                    _AddedOn = value;
                    OnPropertyChanged("AddedOn");
                }
            }
        }

        private DateTime? _AddedDateTime = DateTime.Now;
        public DateTime? AddedDateTime
        {
            get { return _AddedDateTime; }
            set
            {
                if (_AddedDateTime != value)
                {
                    _AddedDateTime = value;
                    OnPropertyChanged("AddedDateTime");
                }
            }
        }

        private string _AddedWindowsLoginName = "";
        public string AddedWindowsLoginName
        {
            get { return _AddedWindowsLoginName; }
            set
            {
                if (_AddedWindowsLoginName != value)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }

        private long? _UpdatedBy;
        public long? UpdatedBy
        {
            get { return _UpdatedBy; }
            set
            {
                if (_UpdatedBy != value)
                {
                    _UpdatedBy = value;
                    OnPropertyChanged("UpdatedBy");
                }
            }
        }

        private string _UpdatedOn = "";
        public string UpdatedOn
        {
            get { return _UpdatedOn; }
            set
            {
                if (_UpdatedOn != value)
                {
                    _UpdatedOn = value;
                    OnPropertyChanged("UpdatedOn");
                }
            }
        }

        private DateTime? _UpdatedDateTime = DateTime.Now;
        public DateTime? UpdatedDateTime
        {
            get { return _UpdatedDateTime; }
            set
            {
                if (_UpdatedDateTime != value)
                {
                    _UpdatedDateTime = value;
                    OnPropertyChanged("UpdatedDateTime");
                }
            }
        }

        private string _UpdatedWindowsLoginName = "";
        public string UpdatedWindowsLoginName
        {
            get { return _UpdatedWindowsLoginName; }
            set
            {
                if (_UpdatedWindowsLoginName != value)
                {
                    _UpdatedWindowsLoginName = value;
                    OnPropertyChanged("UpdatedWindowsLoginName");
                }
            }
        }

        private DateTime? _RadiologistEntryDate;
        public DateTime? RadiologistEntryDate
        {
            get { return _RadiologistEntryDate; }
            set
            {
                if (_RadiologistEntryDate != value)
                {
                    _RadiologistEntryDate = value;
                    OnPropertyChanged("RadiologistEntryDate");
                }
            }
        }
        private DateTime? _FinalizedByDoctorTime;  //Added By Yogesh K 2 6 16
        public DateTime? FinalizedByDoctorTime
        {
            get { return _FinalizedByDoctorTime; }
            set
            {
                if (_FinalizedByDoctorTime != value)
                {
                    _FinalizedByDoctorTime = value;
                    OnPropertyChanged("FinalizedByDoctorTime");

                }
            }
        }

        private DateTime? _CancelledDateTime;  //Added By Yogesh K 2 6 16
        public DateTime? CancelledDateTime
        {
            get { return _CancelledDateTime; }
            set
            {
                if (_CancelledDateTime != value)
                {
                    _CancelledDateTime = value;
                    OnPropertyChanged("CancelledDateTime");

                }
            }
        }

        #endregion

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

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsRadResultEntryVO : INotifyPropertyChanged, IValueObject
    {
        private bool _Sedation;
        public bool Sedation
        {
            get { return _Sedation; }
            set
            {
                if (_Sedation != value)
                {
                    _Sedation = value;
                    OnPropertyChanged("Sedation");
                }
            }
        }

        private bool _Contrast;
        public bool Contrast
        {
            get { return _Contrast; }
            set
            {
                if (_Contrast != value)
                {
                    _Contrast = value;
                    OnPropertyChanged("Contrast");
                }
            }
        }

        private string _ContrastDetails;
        public string ContrastDetails
        {
            get { return _ContrastDetails; }
            set
            {
                if (_ContrastDetails != value)
                {
                    _ContrastDetails = value;
                    OnPropertyChanged("ContrastDetails");
                }
            }
        }

        private bool _FilmWastage;
        public bool FilmWastage
        {
            get { return _FilmWastage; }
            set
            {
                if (_FilmWastage != value)
                {
                    _FilmWastage = value;
                    OnPropertyChanged("FilmWastage");
                }
            }
        }

        private string _FilmWastageDetails;
        public string FilmWastageDetails
        {
            get { return _FilmWastageDetails; }
            set
            {
                if (_FilmWastageDetails != value)
                {
                    _FilmWastageDetails = value;
                    OnPropertyChanged("FilmWastageDetails");
                }
            }
        }
        private bool _IsTechnicianEntry;
        public bool IsTechnicianEntry
        {
            get { return _IsTechnicianEntry; }
            set
            {
                if (_IsTechnicianEntry != value)
                {
                    _IsTechnicianEntry = value;
                    OnPropertyChanged("IsTechnicianEntry");
                }
            }
        }
        private bool _IsTechnicianEntryFinalized;
        public bool IsTechnicianEntryFinalized
        {
            get { return _IsTechnicianEntryFinalized; }
            set
            {
                if (_IsTechnicianEntryFinalized != value)
                {
                    _IsTechnicianEntryFinalized = value;
                    OnPropertyChanged("IsTechnicianEntryFinalized");
                }
            }
        }


        private string _SourceURL;
        public string SourceURL
        {
            get { return _SourceURL; }
            set
            {
                if (_SourceURL != value)
                {
                    _SourceURL = value;
                    OnPropertyChanged("SourceURL");

                }

            }
        }


        private string _Notes;
        public string Notes
        {
            get { return _Notes; }
            set
            {
                if (_Notes != value)
                {
                    _Notes = value;
                    OnPropertyChanged("Notes");

                }

            }
        }

        private string _Remarks;
        public string Remarks
        {
            get { return _Remarks; }
            set
            {
                if (_Remarks != value)
                {
                    _Remarks = value;
                    OnPropertyChanged("Remarks");

                }

            }
        }

        private byte[] _Report;
        public byte[] Report
        {
            get { return _Report; }
            set
            {
                if (_Report != value)
                {
                    _Report = value;
                    OnPropertyChanged("Report");

                }

            }

        }
        private DateTime? _Time;
        public DateTime? Time
        {
            get { return _Time; }
            set
            {
                if (_Time != value)
                {
                    _Time = value;
                    OnPropertyChanged("Time");

                }
            }
        }
        private bool _IsDelivered;
        public bool IsDelivered
        {
            get { return _IsDelivered; }
            set
            {
                if (_IsDelivered != value)
                {
                    _IsDelivered = value;
                    OnPropertyChanged("IsDelivered");
                }

            }

        }
        private bool _IsCompleted;
        public bool IsCompleted
        {
            get { return _IsCompleted; }
            set
            {
                if (_IsCompleted != value)
                {
                    _IsCompleted = value;
                    OnPropertyChanged("IsCompleted");
                }

            }

        }
        private bool _IsDigitalSignatureRequired;
        public bool IsDigitalSignatureRequired
        {
            get { return _IsDigitalSignatureRequired; }
            set
            {
                if (_IsDigitalSignatureRequired != value)
                {
                    _IsDigitalSignatureRequired = value;
                    OnPropertyChanged("IsDigitalSignatureRequired");
                }

            }

        }
        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                if (_ID != value)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set
            {
                if (_UnitID != value)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }


        private long _BillID;
        public long BillID
        {
            get { return _BillID; }
            set
            {
                if (_BillID != value)
                {
                    _BillID = value;
                    OnPropertyChanged("BillID");
                }
            }
        }

        private long _RadOrderID;
        public long RadOrderID
        {
            get { return _RadOrderID; }
            set
            {
                if (_RadOrderID != value)
                {
                    _RadOrderID = value;
                    OnPropertyChanged("RadOrderID");
                }
            }
        }

        private long _RadTechnicianEntryID;
        public long RadTechnicianEntryID
        {
            get { return _RadTechnicianEntryID; }
            set
            {
                if (_RadTechnicianEntryID != value)
                {
                    _RadTechnicianEntryID = value;
                    OnPropertyChanged("RadTechnicianEntryID");
                }
            }
        }
        private bool _IsRefDoctorSigniture;
        public bool IsRefDoctorSigniture
        {
            get { return _IsRefDoctorSigniture; }
            set
            {
                if (_IsRefDoctorSigniture != value)
                {
                    _IsRefDoctorSigniture = value;
                    OnPropertyChanged("IsRefDoctorSigniture");
                }
            }
        }

        private long _BookingDetailsID;
        public long BookingDetailsID
        {
            get { return _BookingDetailsID; }
            set
            {
                if (_BookingDetailsID != value)
                {
                    _BookingDetailsID = value;
                    OnPropertyChanged("BookingDetailsID");
                }
            }
        }



        private long _TestID;
        public long TestID
        {
            get { return _TestID; }
            set
            {
                if (_TestID != value)
                {
                    _TestID = value;
                    OnPropertyChanged("TestID");
                }
            }
        }

        private long _FilmID;
        public long FilmID
        {
            get { return _FilmID; }
            set
            {
                if (_FilmID != value)
                {
                    _FilmID = value;
                    OnPropertyChanged("FilmID");
                }
            }
        }

        private long _NoOfFilms;
        public long NoOfFilms
        {
            get { return _NoOfFilms; }
            set
            {
                if (_NoOfFilms != value)
                {
                    _NoOfFilms = value;
                    OnPropertyChanged("NoOfFilms");
                }
            }
        }

        private long _RadiologistID1;
        public long RadiologistID1
        {
            get { return _RadiologistID1; }
            set
            {
                if (_RadiologistID1 != value)
                {
                    _RadiologistID1 = value;
                    OnPropertyChanged("RadiologistID1");
                }
            }
        }


        private long _TemplateResultID;
        public long TemplateResultID
        {
            get { return _TemplateResultID; }
            set
            {
                if (_TemplateResultID != value)
                {
                    _TemplateResultID = value;
                    OnPropertyChanged("TemplateResultID");
                }
            }
        }
        private string _FirstLevelDescription;
        public string FirstLevelDescription
        {
            get { return _FirstLevelDescription; }
            set
            {
                if (_FirstLevelDescription != value)
                {
                    _FirstLevelDescription = value;
                    OnPropertyChanged("FirstLevelDescription");
                }
            }
        }

        public String TestTemplateRTF { get; set; }

        private long _RadiologistID2;
        public long RadiologistID2
        {
            get { return _RadiologistID2; }
            set
            {
                if (_RadiologistID2 != value)
                {
                    _RadiologistID2 = value;
                    OnPropertyChanged("RadiologistID2");
                }
            }
        }

        private long _RadiologistID3;
        public long RadiologistID3
        {
            get { return _RadiologistID3; }
            set
            {
                if (_RadiologistID3 != value)
                {
                    _RadiologistID3 = value;
                    OnPropertyChanged("RadiologistID3");
                }
            }
        }

        private string _SecondLevelDescription;
        public string SecondLevelDescription
        {
            get { return _SecondLevelDescription; }
            set
            {
                if (_SecondLevelDescription != value)
                {
                    _SecondLevelDescription = value;
                    OnPropertyChanged("SecondLevelDescription");
                }
            }
        }


        private string _ThirdLevelDescription;
        public string ThirdLevelDescription
        {
            get { return _ThirdLevelDescription; }
            set
            {
                if (_ThirdLevelDescription != value)
                {
                    _ThirdLevelDescription = value;
                    OnPropertyChanged("ThirdLevelDescription");
                }
            }
        }

        private long _FirstLevelId;
        public long FirstLevelId
        {
            get { return _FirstLevelId; }
            set
            {
                if (_FirstLevelId != value)
                {
                    _FirstLevelId = value;
                    OnPropertyChanged("FirstLevelId");
                }
            }
        }


        private long _SecondLevelId;
        public long SecondLevelId
        {
            get { return _SecondLevelId; }
            set
            {
                if (_SecondLevelId != value)
                {
                    _SecondLevelId = value;
                    OnPropertyChanged("SecondLevelId");
                }
            }
        }


        private long _ThirdLevelId;
        public long ThirdLevelId
        {
            get { return _ThirdLevelId; }
            set
            {
                if (_ThirdLevelId != value)
                {
                    _ThirdLevelId = value;
                    OnPropertyChanged("ThirdLevelId");
                }
            }
        }


        private long? _TemplateID;
        public long? TemplateID
        {
            get { return _TemplateID; }
            set
            {
                if (_TemplateID != value)
                {
                    _TemplateID = value;
                    OnPropertyChanged("TemplateID");
                }
            }
        }

        private string _TemplateName;
        public string TemplateName
        {
            get { return _TemplateName; }
            set
            {
                if (_TemplateName != value)
                {
                    _TemplateName = value;
                    OnPropertyChanged("TemplateName");
                }
            }
        }



        private bool _IsOutSourced;
        public bool IsOutSourced
        {
            get { return _IsOutSourced; }
            set
            {
                if (_IsOutSourced != value)
                {
                    _IsOutSourced = value;
                    OnPropertyChanged("IsOutSourced");
                }
            }
        }

        private long _AgencyId;
        public long AgencyId
        {
            get { return _AgencyId; }
            set
            {
                if (_AgencyId != value)
                {
                    _AgencyId = value;
                    OnPropertyChanged("AgencyId");
                }
            }
        }

        private long _ServiceID;
        public long ServiceID
        {
            get { return _ServiceID; }
            set
            {
                if (_ServiceID != value)
                {
                    _ServiceID = value;
                    OnPropertyChanged("ServiceID");
                }
            }
        }




        private string _ReferredBy = "";
        public string ReferredBy
        {
            get { return _ReferredBy; }
            set
            {
                if (_ReferredBy != value)
                {
                    _ReferredBy = value;
                    OnPropertyChanged("ReferredBy");
                }
            }
        }

        private string _PatientName = "";
        public string PatientName
        {
            get { return _PatientName; }
            set
            {
                if (_PatientName != value)
                {
                    _PatientName = value;
                    OnPropertyChanged("PatientName");//Added By Yogesh K
                }
            }
        }


        private DateTime? _FinalizedByDoctorTime;
        public DateTime? FinalizedByDoctorTime
        {
            get { return _FinalizedByDoctorTime; }
            set
            {
                if (_FinalizedByDoctorTime != value)
                {
                    _FinalizedByDoctorTime = value;
                    OnPropertyChanged("FinalizedByDoctorTime");

                }
            }
        }

        private bool _IsFinalized;
        public bool IsFinalized
        {
            get { return _IsFinalized; }
            set
            {
                if (_IsFinalized != value)
                {
                    _IsFinalized = value;
                    OnPropertyChanged("IsFinalized");
                }
            }
        }

        private bool _IsResultEntry = false;
        public bool IsResultEntry
        {
            get { return _IsResultEntry; }
            set
            {
                if (_IsResultEntry != value)
                {
                    _IsResultEntry = value;
                    OnPropertyChanged("IsResultEntry");
                }
            }
        }



        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }


        private long _StoreID;
        public long StoreID
        {
            get { return _StoreID; }
            set { _StoreID = value; }
        }

        private List<clsRadItemDetailMasterVO> _TestItemList;
        public List<clsRadItemDetailMasterVO> TestItemList
        {
            get
            {
                if (_TestItemList == null)
                    _TestItemList = new List<clsRadItemDetailMasterVO>();

                return _TestItemList;
            }

            set
            {

                _TestItemList = value;

            }
        }

        #region Common Properties



        private string _UnitName;
        public string UnitName
        {
            get { return _UnitName; }
            set
            {
                if (_UnitName != value)
                {
                    _UnitName = value;
                    OnPropertyChanged("UnitName");
                }
            }
        }

        private long _CreatedUnitID;
        public long CreatedUnitId
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitId");
                }
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitId
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitId");
                }
            }
        }

        private bool _Status = true;
        public bool Status
        {
            get { return _Status; }
            set
            {
                if (_Status != value)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        private long? _AddedBy;
        public long? AddedBy
        {
            get { return _AddedBy; }
            set
            {
                if (_AddedBy != value)
                {
                    _AddedBy = value;
                    OnPropertyChanged("AddedBy");
                }
            }
        }

        private string _AddedOn = "";
        public string AddedOn
        {
            get { return _AddedOn; }
            set
            {
                if (_AddedOn != value)
                {
                    _AddedOn = value;
                    OnPropertyChanged("AddedOn");
                }
            }
        }

        private DateTime? _AddedDateTime = DateTime.Now;
        public DateTime? AddedDateTime
        {
            get { return _AddedDateTime; }
            set
            {
                if (_AddedDateTime != value)
                {
                    _AddedDateTime = value;
                    OnPropertyChanged("AddedDateTime");
                }
            }
        }

        private string _AddedWindowsLoginName = "";
        public string AddedWindowsLoginName
        {
            get { return _AddedWindowsLoginName; }
            set
            {
                if (_AddedWindowsLoginName != value)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }

        private long? _UpdatedBy;
        public long? UpdatedBy
        {
            get { return _UpdatedBy; }
            set
            {
                if (_UpdatedBy != value)
                {
                    _UpdatedBy = value;
                    OnPropertyChanged("UpdatedBy");
                }
            }
        }

        private string _UpdatedOn = "";
        public string UpdatedOn
        {
            get { return _UpdatedOn; }
            set
            {
                if (_UpdatedOn != value)
                {
                    _UpdatedOn = value;
                    OnPropertyChanged("UpdatedOn");
                }
            }
        }

        private DateTime? _UpdatedDateTime = DateTime.Now;
        public DateTime? UpdatedDateTime
        {
            get { return _UpdatedDateTime; }
            set
            {
                if (_UpdatedDateTime != value)
                {
                    _UpdatedDateTime = value;
                    OnPropertyChanged("UpdatedDateTime");
                }
            }
        }

        private string _UpdatedWindowsLoginName = "";
        public string UpdatedWindowsLoginName
        {
            get { return _UpdatedWindowsLoginName; }
            set
            {
                if (_UpdatedWindowsLoginName != value)
                {
                    _UpdatedWindowsLoginName = value;
                    OnPropertyChanged("UpdatedWindowsLoginName");
                }
            }
        }



        #endregion

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

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsRadPrintResultEntryVO : INotifyPropertyChanged, IValueObject
    {


        private string _PatientName;
        public string PatientName
        {
            get { return _PatientName; }
            set
            {
                if (_PatientName != value)
                {
                    _PatientName = value;
                    OnPropertyChanged("PatientName");
                }
            }
        }

        private string _TestNAme;
        public string TestNAme
        {
            get { return _TestNAme; }
            set
            {
                if (_TestNAme != value)
                {
                    _TestNAme = value;
                    OnPropertyChanged("TestNAme");
                }
            }
        }

        private string _MRno;
        public string MRno
        {
            get { return _MRno; }
            set
            {
                if (_MRno != value)
                {
                    _MRno = value;
                    OnPropertyChanged("MRno");
                }
            }
        }



        private string _BillNo;
        public string BillNo
        {
            get { return _BillNo; }
            set
            {
                if (_BillNo != value)
                {
                    _BillNo = value;
                    OnPropertyChanged("BillNo");
                }
            }
        }

        private DateTime _OrderDate;
        public DateTime OrderDate
        {
            get { return _OrderDate; }
            set
            {
                if (_OrderDate != value)
                {
                    _OrderDate = value;
                    OnPropertyChanged("OrderDate");
                }
            }
        }



        private string _description;
        public string description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged("description");
                }
            }
        }

        private string _Template;
        public string Template
        {
            get { return _Template; }
            set
            {
                if (_Template != value)
                {
                    _Template = value;
                    OnPropertyChanged("Template");
                }
            }
        }

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

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

    }
    public class clsRadOrderBookingVO : INotifyPropertyChanged, IValueObject
    {

        //property

        private bool _InProcess;
        public bool InProcess
        {
            get { return _InProcess; }
            set
            {
                if (_InProcess != value)
                {
                    _InProcess = value;
                    OnPropertyChanged("InProcess");
                }
            }
        
        
        }

        private bool _New;
        public bool New
        {
            get { return _New; }
            set
            {
                if (_New != value)
                {
                    _New = value;
                    OnPropertyChanged("New");
                }
            }


        }


        private long _Completed;
        public long Completed
        {
            get { return _Completed; }
            set
            {
                if (_Completed != value)
                {
                    _Completed = value;
                    OnPropertyChanged("Completed");
                }
            }


        }

        private long _RadSpecilizationID;
            public long RadSpecilizationID
        {
            get { return _RadSpecilizationID; }
            set
            {
                if (_RadSpecilizationID != value)
                {
                    _RadSpecilizationID = value;
                    OnPropertyChanged("RadSpecilizationID");
                }
            }
        }

        private double _CompanyAmount;
        public double CompanyAmount
        {
            get { return _CompanyAmount; }
            set
            {
                if (_CompanyAmount != value)
                {
                    _CompanyAmount = value;
                    OnPropertyChanged("CompanyAmount");
                }
            }
        }
        private double _PatientAmount;
        public double PatientAmount
        {
            get { return _PatientAmount; }
            set
            {
                if (_PatientAmount != value)
                {
                    _PatientAmount = value;
                    OnPropertyChanged("PatientAmount");
                }
            }
        }
        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                if (_ID != value)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set
            {
                if (_UnitID != value)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }

        private long _OrderDetailID;
        public long OrderDetailID
        {
            get { return _OrderDetailID; }
            set
            {
                if (_OrderDetailID != value)
                {
                    _OrderDetailID = value;
                    OnPropertyChanged("OrderDetailID");
                }
            }
        }

        private string _OrderNo;
        public string OrderNo
        {
            get { return _OrderNo; }
            set
            {
                if (_OrderNo != value)
                {
                    _OrderNo = value;
                    OnPropertyChanged("OrderNo");
                }
            }
        }

        private string _ContactNO;
        public string ContactNO
        {
            get { return _ContactNO; }
            set
            {
                if (_ContactNO != value)
                {
                    _ContactNO = value;
                    OnPropertyChanged("ContactNO");
                }
            }
        }

        private string _PatientEmailId;
        public string PatientEmailId
        {
            get { return _PatientEmailId; }
            set
            {
                if (_PatientEmailId != value)
                {
                    _PatientEmailId = value;
                    OnPropertyChanged("PatientEmailId");
                }
            }
        }


     //   public bool DeliveryStatus { get; set; }
        private bool _DeliveryStatus;
        public bool DeliveryStatus
        {
            get { return _DeliveryStatus; }
            set
            {
                if (_DeliveryStatus != value)
                {
                    _DeliveryStatus = value;
                    OnPropertyChanged("DeliveryStatus");
                }
            }
        }





        private string _DoctorEmailID;
        public string DoctorEmailID
        {
            get { return _DoctorEmailID; }
            set
            {
                if (_DoctorEmailID != value)
                {
                    _DoctorEmailID = value;
                    OnPropertyChanged("DoctorEmailID");
                }
            }
        }

        private string _BillNo;
        public string BillNo
        {
            get { return _BillNo; }
            set
            {
                if (_BillNo != value)
                {
                    _BillNo = value;
                    OnPropertyChanged("BillNo");
                }
            }
        }

        private double _TotalAmount;
        public double TotalAmount
        {
            get { return _TotalAmount; }
            set
            {
                if (_TotalAmount != value)
                {
                    _TotalAmount = value;
                    OnPropertyChanged("TotalAmount");
                }
            }
        }

        private double _PaidAmount;
        public double PaidAmount
        {
            get { return _PaidAmount; }
            set
            {
                if (_PaidAmount != value)
                {
                    _PaidAmount = value;
                    OnPropertyChanged("PaidAmount");
                }
            }
        }

        private double _Balance;
        public double Balance
        {
            get { return _Balance; }
            set
            {
                if (_Balance != value)
                {
                    _Balance = value;
                    OnPropertyChanged("Balance");
                }
            }
        }

        private bool _Freezed;
        public bool Freezed
        {
            get { return _Freezed; }
            set
            {
                if (_Freezed != value)
                {
                    _Freezed = value;
                    OnPropertyChanged("Freezed");
                }
            }
        }

        private long _Gender;
        public long Gender
        {
            get { return _Gender; }
            set
            {
                if (_Gender != value)
                {
                    _Gender = value;
                    OnPropertyChanged("Gender");
                }
            }
        }

        private string _ReferredDoctor;
        public string ReferredDoctor
        {
            get { return _ReferredDoctor; }
            set
            {
                if (_ReferredDoctor != value)
                {
                    _ReferredDoctor = value;
                    OnPropertyChanged("ReferredDoctor");
                }
            }
        }


        private long _ReferredDoctorID;
        public long ReferredDoctorID
        {
            get { return _ReferredDoctorID; }
            set
            {
                if (_ReferredDoctorID != value)
                {
                    _ReferredDoctorID = value;
                    OnPropertyChanged("ReferredDoctorID");
                }
            }
        }

        private string _VisitNotes;
        public string VisitNotes
        {
            get { return _VisitNotes; }
            set
            {
                if (_VisitNotes != value)
                {
                    _VisitNotes = value;
                    OnPropertyChanged("VisitNotes");
                }
            }
        }

        private DateTime? _Date;
        public DateTime? Date
        {
            get { return _Date; }
            set
            {
                if (_Date != value)
                {
                    _Date = value;
                    OnPropertyChanged("Date");
                }
            }
        }
        private DateTime? _BillDate;
        public DateTime? BillDate
        {
            get { return _BillDate; }
            set
            {
                if (_BillDate != value)
                {
                    _BillDate = value;
                    OnPropertyChanged("BillDate");
                }
            }
        }
        private long _Opd_Ipd_External_ID;
        public long Opd_Ipd_External_ID
        {
            get { return _Opd_Ipd_External_ID; }
            set
            {
                if (_Opd_Ipd_External_ID != value)
                {
                    _Opd_Ipd_External_ID = value;
                    OnPropertyChanged("Opd_Ipd_External_ID");
                }
            }
        }


        private long _Opd_Ipd_External_UnitID;
        public long Opd_Ipd_External_UnitID
        {
            get { return _Opd_Ipd_External_UnitID; }
            set
            {
                if (_Opd_Ipd_External_UnitID != value)
                {
                    _Opd_Ipd_External_UnitID = value;
                    OnPropertyChanged("Opd_Ipd_External_UnitID");
                }
            }
        }



        private long? _Opd_Ipd_External;
        public long? Opd_Ipd_External
        {
            get { return _Opd_Ipd_External; }
            set
            {
                if (_Opd_Ipd_External != value)
                {
                    _Opd_Ipd_External = value;
                    OnPropertyChanged("Opd_Ipd_External");
                }
            }
        }


        private bool _IsApproved;
        public bool IsApproved
        {
            get { return _IsApproved; }
            set
            {
                if (_IsApproved != value)
                {
                    _IsApproved = value;
                    OnPropertyChanged("IsApproved");
                }
            }
        }

        private bool _IsDelivered;
        public bool IsDelivered
        {
            get { return _IsDelivered; }
            set
            {
                if (_IsDelivered != value)
                {
                    _IsDelivered = value;
                    OnPropertyChanged("IsDelivered");
                }
            }
        }


        private long? _TestType;
        public long? TestType
        {
            get { return _TestType; }
            set
            {
                if (_TestType != value)
                {
                    _TestType = value;
                    OnPropertyChanged("TestType");
                }
            }
        }

        private long _TestID;
        public long TestID
        {
            get { return _TestID; }
            set
            {
                if (_TestID != value)
                {
                    _TestID = value;
                    OnPropertyChanged("TestID");
                }
            }
        }
        private long _ModalityID;
        public long ModalityID
        {
            get { return _ModalityID; }
            set
            {
                if (_ModalityID != value)
                {
                    _ModalityID = value;
                    OnPropertyChanged("ModalityID");
                }
            }
        }

        private string _Modality;
        public string Modality
        {
            get { return _Modality; }
            set
            {
                if (_Modality != value)
                {
                    _Modality = value;
                    OnPropertyChanged("Modality");
                }
            }
        }
        private bool _IsCancelled;
        public bool IsCancelled
        {
            get { return _IsCancelled; }
            set
            {
                if (_IsCancelled != value)
                {
                    _IsCancelled = value;
                    OnPropertyChanged("IsCancelled");
                }
            }
        }
        private bool _IsTechnicianEntry;
        public bool IsTechnicianEntry
        {
            get { return _IsTechnicianEntry; }
            set
            {
                if (_IsTechnicianEntry != value)
                {
                    _IsTechnicianEntry = value;
                    OnPropertyChanged("IsTechnicianEntry");
                }
            }
        }
        private bool _IsTechnicianEntryFinalized;
        public bool IsTechnicianEntryFinalized
        {
            get { return _IsTechnicianEntryFinalized; }
            set
            {
                if (_IsTechnicianEntryFinalized != value)
                {
                    _IsTechnicianEntryFinalized = value;
                    OnPropertyChanged("IsTechnicianEntryFinalized");
                }
            }
        }
        private bool _Sedation;
        public bool Sedation
        {
            get { return _Sedation; }
            set
            {
                if (_Sedation != value)
                {
                    _Sedation = value;
                    OnPropertyChanged("Sedation");
                }
            }
        }

        private bool _Contrast;
        public bool Contrast
        {
            get { return _Contrast; }
            set
            {
                if (_Contrast != value)
                {
                    _Contrast = value;
                    OnPropertyChanged("Contrast");
                }
            }
        }

        private string _ContrastDetails;
        public string ContrastDetails
        {
            get { return _ContrastDetails; }
            set
            {
                if (_ContrastDetails != value)
                {
                    _ContrastDetails = value;
                    OnPropertyChanged("ContrastDetails");
                }
            }
        }

        private string _FirstName;
        public string FirstName
        {
            get { return _FirstName; }
            set
            {
                if (_FirstName != value)
                {
                    _FirstName = value;
                    OnPropertyChanged("FirstName");
                }
            }
        }

        private string _MiddleName;
        public string MiddleName
        {
            get { return _MiddleName; }
            set
            {
                if (_MiddleName != value)
                {
                    _MiddleName = value;
                    OnPropertyChanged("MiddleName");
                }
            }
        }

        private string _EmailID;
        public string EmailID
        {
            get { return _EmailID; }
            set
            {
                if (_EmailID != value)
                {
                    _EmailID = value;
                    OnPropertyChanged("EmailID");
                }
            }
        }

        private string _LastName;
        public string LastName
        {
            get { return _LastName; }
            set
            {
                if (_LastName != value)
                {
                    _LastName = value;
                    OnPropertyChanged("LastName");
                }
            }
        }
        private bool _FilmWastage;
        public bool FilmWastage
        {
            get { return _FilmWastage; }
            set
            {
                if (_FilmWastage != value)
                {
                    _FilmWastage = value;
                    OnPropertyChanged("FilmWastage");
                }
            }
        }
        private string _FilmWastageDetails;
        public string FilmWastageDetails
        {
            get { return _FilmWastageDetails; }
            set
            {
                if (_FilmWastageDetails != value)
                {
                    _FilmWastageDetails = value;
                    OnPropertyChanged("FilmWastageDetails");
                }
            }
        }

        private string _GenderName;
        public string GenderName
        {
            get { return _GenderName; }
            set
            {
                if (_GenderName != value)
                {
                    _GenderName = value;
                    OnPropertyChanged("GenderName");
                }
            }
        }

        private long _NoOfFilms;
        public long NoOfFilms
        {
            get { return _NoOfFilms; }
            set
            {
                if (_NoOfFilms != value)
                {
                    _NoOfFilms = value;
                    OnPropertyChanged("NoOfFilms");
                }
            }
        }
        //Added By Somanath on 28/01/2012 as per Priyanka mam

        private long _PaymentModeID;
        public long PaymentModeID
        {
            get { return _PaymentModeID; }
            set
            {
                if (_PaymentModeID != value)
                {
                    _PaymentModeID = value;
                    OnPropertyChanged("PaymentModeID");
                }
            }
        }
        private long _ServiceID;
        public long ServiceID
        {
            get { return _ServiceID; }
            set
            {
                if (_ServiceID != value)
                {
                    _ServiceID = value;
                    OnPropertyChanged("ServiceID");
                }
            }
        }
        private long _BillID;
        public long BillID
        {
            get { return _BillID; }
            set
            {
                if (_BillID != value)
                {
                    _BillID = value;
                    OnPropertyChanged("BillID");
                }
            }
        }
        private long _ChargeID;
        public long ChargeID
        {
            get { return _ChargeID; }
            set
            {
                if (_ChargeID != value)
                {
                    _ChargeID = value;
                    OnPropertyChanged("ChargeID");
                }
            }
        }

        private int _Age;
        public int Age
        {
            get { return _Age; }
            set
            {
                if (_Age != value)
                {
                    _Age = value;
                    OnPropertyChanged("Age");
                }
            }
        }

        private string _AgeINDDMMYYYY;
        public string AgeINDDMMYYYY
        {
            get { return _AgeINDDMMYYYY; }
            set
            {
                if (_AgeINDDMMYYYY != value)
                {
                    _AgeINDDMMYYYY = value;
                    OnPropertyChanged("AgeINDDMMYYYY");
                }
            }
        }
        private string _PaymentMode;
        public string PaymentMode
        {
            get { return _PaymentMode; }
            set
            {
                if (_PaymentMode != value)
                {
                    _PaymentMode = value;
                    OnPropertyChanged("PaymentMode");
                }
            }
        }

        private DateTime _DateOfBirth;
        public DateTime DateOfBirth
        {
            get { return _DateOfBirth; }
            set
            {
                if (_DateOfBirth != value)
                {
                    _DateOfBirth = value;
                    OnPropertyChanged("DateOfBirth");
                }
            }
        }


        private long _PatientWeight;
        public long PatientWeight
        {
            get { return _PatientWeight; }
            set
            {
                if (_PatientWeight != value)
                {
                    _PatientWeight = value;
                    OnPropertyChanged("PatientWeight");
                }
            }
        }

        private long _AgeInYears;
        public long AgeInYears
        {
            get { return _AgeInYears; }
            set
            {
                if (_AgeInYears != value)
                {
                    _AgeInYears = value;
                    OnPropertyChanged("AgeInYears");
                }
            }
        }

        private long _PatientHeight;
        public long PatientHeight
        {
            get { return _PatientHeight; }
            set
            {
                if (_PatientHeight != value)
                {
                    _PatientHeight = value;
                    OnPropertyChanged("PatientHeight");
                }
            }
        }

        private string _ContactNo1;
        public string ContactNo1
        {
            get { return _ContactNo1; }
            set
            {
                if (_ContactNo1 != value)
                {
                    _ContactNo1 = value;
                    OnPropertyChanged("ContactNo1");
                }
            }
        }

        private string _Service;
        public string Service
        {
            get { return _Service; }
            set
            {
                if (_Service != value)
                {
                    _Service = value;
                    OnPropertyChanged("Service");
                }
            }
        }
        //End
        private string _MRNo;
        public string MRNo
        {
            get { return _MRNo; }
            set
            {
                if (_MRNo != value)
                {
                    _MRNo = value;
                    OnPropertyChanged("MRNo");
                }

            }
        }
        private string _OPDNO;
        public string OPDNO
        {
            get { return _OPDNO; }
            set
            {
                if (_OPDNO != value)
                {
                    _OPDNO = value;
                    OnPropertyChanged("OPDNO");
                }

            }
        }
        private string _ScanNo;
        public string ScanNo
        {
            get { return _ScanNo; }
            set
            {
                if (_ScanNo != value)
                {
                    _ScanNo = value;
                    OnPropertyChanged("ScanNo");
                }

            }
        }
        private long _PatientID;
        public long PatientID
        {
            get { return _PatientID; }
            set
            {
                if (_PatientID != value)
                {
                    _PatientID = value;
                    OnPropertyChanged("PatientID");
                }
            }
        }


        private string _PatientName;
        public string PatientName
        {
            get { return _PatientName; }
            set
            {
                if (_PatientName != value)
                {
                    _PatientName = value;
                    OnPropertyChanged("PatientName");
                }

            }
        }


        private long _PatientUnitId;
        public long PatientUnitId
        {
            get { return _PatientUnitId; }
            set
            {
                if (_PatientUnitId != value)
                {
                    _PatientUnitId = value;
                    OnPropertyChanged("PatientUnitId");
                }

            }
        }
        private bool _IsResultEntry;
        public bool IsResultEntry
        {
            get { return _IsResultEntry; }
            set
            {
                if (_IsResultEntry != value)
                {
                    _IsResultEntry = value;
                    OnPropertyChanged("IsResultEntry");
                }
            }
        }
        private bool _IsFinalized;
        public bool IsFinalized
        {
            get { return _IsFinalized; }
            set
            {
                if (_IsFinalized != value)
                {
                    _IsFinalized = value;
                    OnPropertyChanged("IsFinalized");
                }
            }
        }

        private string _Radiologist;
        public string Radiologist
        {
            get { return _Radiologist; }
            set
            {
                if (_Radiologist != value)
                {
                    _Radiologist = value;
                    OnPropertyChanged("Radiologist");
                }

            }
        }

        private List<clsRadOrderBookingDetailsVO> _ObjDetails = new List<clsRadOrderBookingDetailsVO>();
        public List<clsRadOrderBookingDetailsVO> OrderBookingDetails
        {
            get { return _ObjDetails; }
            set
            {

                _ObjDetails = value;
                OnPropertyChanged("OrderBookingDetails");

            }
        }

        private long _TotalCount;
        public long TotalCount
        {
            get { return _TotalCount; }
            set
            {
                if (_TotalCount != value)
                {
                    _TotalCount = value;
                    OnPropertyChanged("TotalCount");
                }
            }


        }

        private long _ResultEntryCount;
        public long ResultEntryCount
        {
            get { return _ResultEntryCount; }
            set
            {
                if (_ResultEntryCount != value)
                {
                    _ResultEntryCount = value;
                    OnPropertyChanged("ResultEntryCount");
                }
            }


        }

        private long _CompletedTest;
        public long CompletedTest
        {
            get { return _CompletedTest; }
            set
            {
                if (_CompletedTest != value)
                {
                    _CompletedTest = value;
                    OnPropertyChanged("CompletedTest");
                }
            }


        }

        private long _UploadedCount;
        public long UploadedCount
        {
            get { return _UploadedCount; }
            set
            {
                if (_UploadedCount != value)
                {
                    _UploadedCount = value;
                    OnPropertyChanged("UploadedCount");
                }
            }


        }



        #region Common Properties

        private string _LinkServer;
        public string LinkServer
        {
            get { return _LinkServer; }
            set
            {
                if (_LinkServer != value)
                {
                    _LinkServer = value;
                    OnPropertyChanged("LinkServer");
                }
            }
        }


        private string _UnitName;
        public string UnitName
        {
            get { return _UnitName; }
            set
            {
                if (_UnitName != value)
                {
                    _UnitName = value;
                    OnPropertyChanged("UnitName");
                }
            }
        }

        private long _CreatedUnitID;
        public long CreatedUnitId
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitId");
                }
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitId
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitId");
                }
            }
        }

        private bool _Status = true;
        public bool Status
        {
            get { return _Status; }
            set
            {
                if (_Status != value)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        private long? _AddedBy;
        public long? AddedBy
        {
            get { return _AddedBy; }
            set
            {
                if (_AddedBy != value)
                {
                    _AddedBy = value;
                    OnPropertyChanged("AddedBy");
                }
            }
        }

        private string _AddedOn = "";
        public string AddedOn
        {
            get { return _AddedOn; }
            set
            {
                if (_AddedOn != value)
                {
                    _AddedOn = value;
                    OnPropertyChanged("AddedOn");
                }
            }
        }

        private DateTime? _AddedDateTime = DateTime.Now;
        public DateTime? AddedDateTime
        {
            get { return _AddedDateTime; }
            set
            {
                if (_AddedDateTime != value)
                {
                    _AddedDateTime = value;
                    OnPropertyChanged("AddedDateTime");
                }
            }
        }

        private string _AddedWindowsLoginName = "";
        public string AddedWindowsLoginName
        {
            get { return _AddedWindowsLoginName; }
            set
            {
                if (_AddedWindowsLoginName != value)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }

        private long? _UpdatedBy;
        public long? UpdatedBy
        {
            get { return _UpdatedBy; }
            set
            {
                if (_UpdatedBy != value)
                {
                    _UpdatedBy = value;
                    OnPropertyChanged("UpdatedBy");
                }
            }
        }

        private string _UpdatedOn = "";
        public string UpdatedOn
        {
            get { return _UpdatedOn; }
            set
            {
                if (_UpdatedOn != value)
                {
                    _UpdatedOn = value;
                    OnPropertyChanged("UpdatedOn");
                }
            }
        }

        private DateTime? _UpdatedDateTime = DateTime.Now;
        public DateTime? UpdatedDateTime
        {
            get { return _UpdatedDateTime; }
            set
            {
                if (_UpdatedDateTime != value)
                {
                    _UpdatedDateTime = value;
                    OnPropertyChanged("UpdatedDateTime");
                }
            }
        }

        private string _UpdatedWindowsLoginName = "";
        public string UpdatedWindowsLoginName
        {
            get { return _UpdatedWindowsLoginName; }
            set
            {
                if (_UpdatedWindowsLoginName != value)
                {
                    _UpdatedWindowsLoginName = value;
                    OnPropertyChanged("UpdatedWindowsLoginName");
                }
            }
        }



        #endregion

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

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }
}
