using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Patient;

namespace PalashDynamics.ValueObjects.OutPatientDepartment 
{
    public class clsPatientSponsorVO :  IValueObject, INotifyPropertyChanged
    {
        //***//
        private clsBankDetailsInfoVO objBankDetails = null;
        public clsBankDetailsInfoVO BankDetails
        {
            get { return objBankDetails; }
            set { objBankDetails = value; }
        }

        #region Property Declaration Section

        private long _ID;
        public long ID
        {
            get
            {
                return _ID;
            }
            set
            {
                if (value != _ID)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        private long _PatientId;
        public long PatientId
        {
            get
            {
                return _PatientId;
            }
            set
            {
                if (value != _PatientId)
                {
                    _PatientId = value;
                    OnPropertyChanged("PatientId");
                }
            }
        }

        private long _PatientUnitId;
        public long PatientUnitId
        {
            get
            {
                return _PatientUnitId;
            }
            set
            {
                if (value != _PatientUnitId)
                {
                    _PatientUnitId = value;
                    OnPropertyChanged("PatientUnitId");
                }
            }
        }
        


        private long _PatientSourceID;
        public long PatientSourceID
        {
            get
            {
                return _PatientSourceID;
            }
            set
            {
                if (value != _PatientSourceID)
                {
                    _PatientSourceID = value;
                    OnPropertyChanged("PatientSourceID");
                }
            }
        }

        public string PatientSource { get; set; }
        private long _CompanyID;
        public long CompanyID
        {
            get
            {
                return _CompanyID;
            }
            set
            {
                if (value != _CompanyID)
                {
                    _CompanyID = value;
                    OnPropertyChanged("CompanyID");
                }
            }
        }

        public string ComapnyName { get; set; }
       
        private long _AssociatedCompanyID;
        public long AssociatedCompanyID
        {
            get
            {
                return _AssociatedCompanyID;
            }
            set
            {
                if (value != _AssociatedCompanyID)
                {
                    _AssociatedCompanyID= value;
                    OnPropertyChanged("AssociatedCompanyID");
                }
            }
        }

        public string AssociateComapnyName { get; set; }

        private string _ReferenceNo="";
        public string ReferenceNo
        {
            get
            {
                return _ReferenceNo;
            }
            set
            {
                if (value != _ReferenceNo)
                {
                    _ReferenceNo = value;
                    OnPropertyChanged("ReferenceNo");
                }
            }
        }

        private double _CreditLimit;
        public double CreditLimit
        {
            get
            {
                return _CreditLimit;
            }
            set
            {
                if (value != _CreditLimit)
                {
                    _CreditLimit = value;
                    OnPropertyChanged("CreditLimit");
                }
            }
        }

        private DateTime? _EffectiveDate;
        public DateTime? EffectiveDate
        {
            get
            {
                return _EffectiveDate;
            }
            set
            {
                if (_EffectiveDate != value)
                {
                    _EffectiveDate = value;
                    OnPropertyChanged("EffectiveDate");
                }

            }
        }

        private DateTime? _ExpiryDate;
        public DateTime? ExpiryDate
        {
            get
            {
                return _ExpiryDate;
            }
            set
            {
                if (_ExpiryDate != value)
                {
                    _ExpiryDate = value;
                    OnPropertyChanged("ExpiryDate");
                }

            }
        }

        private long? _TariffID;
        public long? TariffID
        {
            get
            {
                return _TariffID;
            }
            set
            {
                if (value != _TariffID)
                {
                    _TariffID = value;
                    OnPropertyChanged("TariffID");
                }
            }
        }

        public string TariffName { get; set; }

        private string _EmployeeNo="";
        public string EmployeeNo
        {
            get
            {
                return _EmployeeNo;
            }
            set
            {
                if (value != _EmployeeNo)
                {
                    _EmployeeNo = value;
                    OnPropertyChanged("EmployeeNo");
                }
            }
        }

        private long _DesignationID;
        public long DesignationID
        {
            get
            {
                return _DesignationID;
            }
            set
            {
                if (value != _DesignationID)
                {
                    _DesignationID = value;
                    OnPropertyChanged("DesignationID");
                }
            }
        }

        public string Designation { get; set; }

        private string _Remark="";
        public string Remark
        {
            get
            {
                return _Remark;
            }
            set
            {
                if (value != _Remark)
                {
                    _Remark= value;
                    OnPropertyChanged("Remark");
                }
            }
        }

        public short PatientSourceType { get; set; }
        
        public long PatientSourceTypeID { get; set; }

        
        private long _MemberRelationID;
        public long MemberRelationID
        {
            get
            {
                return _MemberRelationID;
            }
            set
            {
                if (value != _MemberRelationID)
                {
                    _MemberRelationID = value;
                    OnPropertyChanged("MemberRelationID");
                }
            }
        }

        private long? _PatientCategoryID;
        public long? PatientCategoryID
        {
            get
            {
                return _PatientCategoryID;
            }
            set
            {
                if (value != _PatientCategoryID)
                {
                    _PatientCategoryID = value;
                    OnPropertyChanged("PatientCategoryID");
                }
            }
        }

        private string _PatientCategoryName;
        public string PatientCategoryName
        {
            get
            {
                return _PatientCategoryName;
            }
            set
            {
                if (value != _PatientCategoryName)
                {
                    _PatientCategoryName = value;
                    OnPropertyChanged("PatientCategoryName");
                }
            }
        }


        private bool _IsDupliSponser;
        public bool IsDupliSponser
        {
            get
            {
                return _IsDupliSponser;
            }
            set
            {
                if (value != _IsDupliSponser)
                {
                    _IsDupliSponser = value;
                    OnPropertyChanged("IsDupliSponser");
                }
            }
        }

         

        ////***//  Bank Details-----------------------------------
        //private long _BankID;
        //public long BankID
        //{
        //    get { return _BankID; }
        //    set
        //    {
        //        if (value != _BankID)
        //        {
        //            _BankID = value;
        //            OnPropertyChanged("BankID");
        //        }
        //    }

        //}

        //private long _BranchID;
        //public long BranchID
        //{
        //    get { return _BranchID; }
        //    set
        //    {
        //        if (value != _BranchID)
        //        {
        //            _BranchID = value;
        //            OnPropertyChanged("BranchID");
        //        }
        //    }

        //}

        //public string IFSCCode { get; set; }
        //public bool   AccountType { get; set; }
        //public string AccountNo { get; set; }
        //public string AccountHolderName {get;set;}
 
        ////-------------------------------


  
        #endregion

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

        private long _UnitId;
        public long UnitId
        {
            get { return _UnitId; }
            set
            {
                if (_UnitId != value)
                {
                    _UnitId = value;
                    OnPropertyChanged("UnitId");
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

        private string _AddedOn="";
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

        private string _AddedWindowsLoginName="";
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

        private string _UpdatedOn="";
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

        private string _UpdatedWindowsLoginName="";
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

    }

    public class clsPatientSponsorGroupDetailsVO : IValueObject, INotifyPropertyChanged
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

        #region Property Declaration Section

        private long _ID;
        public long ID
        {
            get
            {
                return _ID;
            }
            set
            {
                if (value != _ID)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                   
                }
            }
        }

        private long _SponsorID;
        public long SponsorID
        {
            get
            {
                return _SponsorID;
            }
            set
            {
                if (value != _SponsorID)
                {
                    _SponsorID = value;
                    OnPropertyChanged("SponsorID");
                   
                }
            }
        }

        private long _GroupID;
        public long GroupID
        {
            get
            {
                return _GroupID;
            }
            set
            {
                if (value != _GroupID)
                {
                    _GroupID= value;
                    OnPropertyChanged("GroupID");

                }
            }
        }

        private string _GroupName="";
        public string GroupName
        {
            get
            {
                return _GroupName;
            }
            set
            {
                if (value != _GroupName)
                {
                    _GroupName = value;
                    OnPropertyChanged("GroupName");

                }
            }
        }

        private decimal? _DeductibleAmount;
        public decimal? DeductibleAmount
        {
            get
            {
                return _DeductibleAmount;
            }
            set
            {
                if (value != _DeductibleAmount)
                {
                    _DeductibleAmount = value;
                    OnPropertyChanged("DeductibleAmount");

                }
            }
        }

        private double? _DeductionPercentage;
        public double? DeductionPercentage
        {
            get
            {
                return _DeductionPercentage;
            }
            set
            {
                if (value != _DeductionPercentage)
                {
                    _DeductionPercentage = value;
                    OnPropertyChanged("DeductionPercentage");

                }
            }
        }

        private bool _Status = true;
        public bool Status
        {
            get
            { return _Status; }

            set
            {
                if (value != _Status)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        #endregion

    }
    
    public class clsPatientSponsorServiceDetailsVO : IValueObject, INotifyPropertyChanged
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

        #region Property Declaration Section

        private long _ID;
        public long ID
        {
            get
            {
                return _ID;
            }
            set
            {
                if (value != _ID)
                {
                    _ID = value;
                    OnPropertyChanged("ID");

                }
            }
        }

        private long _SponsorID;
        public long SponsorID
        {
            get
            {
                return _SponsorID;
            }
            set
            {
                if (value != _SponsorID)
                {
                    _SponsorID = value;
                    OnPropertyChanged("SponsorID");

                }
            }
        }

        private long _ServiceID;
        public long ServiceID
        {
            get
            {
                return _ServiceID;
            }
            set
            {
                if (value != _ServiceID)
                {
                    _ServiceID = value;
                    OnPropertyChanged("ServiceID");

                }
            }
        }

        private string _ServiceName="";
        public string ServiceName
        {
            get
            {
                return _ServiceName;
            }
            set
            {
                if (value != _ServiceName)
                {
                    _ServiceName = value;
                    OnPropertyChanged("ServiceName");

                }
            }
        }

        private decimal? _DeductibleAmount;
        public decimal? DeductibleAmount
        {
            get
            {
                return _DeductibleAmount;
            }
            set
            {
                if (value != _DeductibleAmount)
                {
                    _DeductibleAmount = value;
                    OnPropertyChanged("DeductibleAmount");

                }
            }
        }

        private double? _DeductionPercentage;
        public double? DeductionPercentage
        {
            get
            {
                return _DeductionPercentage;
            }
            set
            {
                if (value != _DeductionPercentage)
                {
                    _DeductionPercentage = value;
                    OnPropertyChanged("DeductionPercentage");

                }
            }
        }

        private bool _Status = true;
        public bool Status
        {
            get
            { return _Status; }

            set
            {
                if (value != _Status)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
                }
            }
        }


        #endregion

    }

    public class clsPatientSponsorCardDetailsVO : IValueObject, INotifyPropertyChanged
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

        #region Property Declaration Section

        private long _ID;
        public long ID { get; set; }

        private long _SponsorID;
        public long SponsorID { get; set; }

        private string _Title="";
        public string  Title { get; set; }

        private byte[] _Image;
        public byte[] Image { get; set; }

        private bool _Status = true;
        public bool Status
        {
            get
            { return _Status; }

            set
            {
                if (value != _Status)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        #endregion

    }

}
