using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Administration.UnitMaster
{
    public class clsUnitMasterVO : IValueObject, INotifyPropertyChanged
    {

        #region DepartmentDetails
        private List<clsDepartmentDetailsVO> _DepartmentDetails;
        public List<clsDepartmentDetailsVO> DepartmentDetails
        {
            get
            {
                if (_DepartmentDetails == null)
                    _DepartmentDetails = new List<clsDepartmentDetailsVO>();

                return _DepartmentDetails;
            }

            set
            {

                _DepartmentDetails = value;

            }
        }

        private long _DeptID;
        public long DepartmentID
        {
            get { return _DeptID; }
            set
            {
                if (value != _DeptID)
                {
                    _DeptID = value;
                    OnPropertyChanged("DepartmentID");
                }
            }
        }

        private string  _Code;
        public string Code
        {
            get { return _Code; }
            set
            {
                if (value != _Code)
                {
                    _Code = value;
                    OnPropertyChanged("Code");
                }
            }
        }

        private string _Department;
        public string Department
        {
            get { return _Department; }
            set
            {
                if (value != _Department)
                {
                    _Department = value;
                    OnPropertyChanged("Department");
                }
            }
        }
        private bool _IsActive;
        public bool IsActive
        {
            get { return _IsActive; }
            set
            {
                if (value != _IsActive)
                {
                    _IsActive = value;
                    OnPropertyChanged("IsActive");
                }
            }
        }

        #endregion

        #region UnitDetails
        private long _ID;
        public long UnitID
        {
            get { return _ID; }
            set
            {
                if (value != _ID)
                {
                    _ID = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }

        private bool _IsClinic;

        public bool IsClinic
        {
            get { return _IsClinic; }
            set
            {
                if (value != _IsClinic)
                {
                    _IsClinic = value;
                    OnPropertyChanged("IsClinic");
                }
            }
        }


        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                if (value != _Name)
                {
                    _Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        private string _UnitCode;
        public string UnitCode
        {
            get { return _UnitCode; }
            set
            {
                if (value != _UnitCode)
                {
                    _UnitCode = value;
                    OnPropertyChanged("UnitCode");
                }
            }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set
            {
                if (value != _Description)
                {
                    _Description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        private string _AddressLine1 = "";
        public string AddressLine1
        {
            get { return _AddressLine1; }
            set
            {
                if (_AddressLine1 != value)
                {
                    _AddressLine1 = value;
                    OnPropertyChanged("AddressLine1");
                }
            }
        }

        private string _AddressLine2 = "";
        public string AddressLine2
        {
            get { return _AddressLine2; }
            set
            {
                if (_AddressLine2 != value)
                {
                    _AddressLine2 = value;
                    OnPropertyChanged("AddressLine2");
                }
            }
        }

        private string _AddressLine3 = "";
        public string AddressLine3
        {
            get { return _AddressLine3; }
            set
            {
                if (_AddressLine3 != value)
                {
                    _AddressLine3 = value;
                    OnPropertyChanged("AddressLine3");
                }
            }
        }

        private string _Country = "";
        public string Country
        {
            get { return _Country; }
            set
            {
                if (_Country != value)
                {
                    _Country = value;
                    OnPropertyChanged("Country");
                }
            }
        }

        private string _State = "";
        public string State
        {
            get
            {
                if (_State == null)
                    return "";

                return _State;
            }
            set
            {
                if (_State != value)
                {
                    _State = value;
                    OnPropertyChanged("State");
                }
            }
        }

        private string _District = "";
        public string District
        {
            get
            {
                if (_District == null)
                    return "";
                return _District;
            }
            set
            {
                if (_District != value)
                {
                    _District = value;
                    OnPropertyChanged("District");
                }
            }
        }

        private string _Taluka = "";
        public string Taluka
        {
            get
            {
                if (_Taluka == null)
                    return "";
                return _Taluka;
            }
            set
            {
                if (_Taluka != value)
                {
                    _Taluka = value;
                    OnPropertyChanged("Taluka");
                }
            }
        }

        private string _City = "";
        public string City
        {
            get
            {
                if (_City == null)
                    return "";

                return _City;
            }
            set
            {
                if (_City != value)
                {
                    _City = value;
                    OnPropertyChanged("City");
                }
            }
        }

        private string _Area = "";
        public string Area
        {
            get
            {
                if (_Area == null)
                    return "";

                return _Area;
            }
            set
            {
                if (_Area != value)
                {
                    _Area = value;
                    OnPropertyChanged("Area");
                }
            }
        }

        private string _Pincode = "";
        public string Pincode
        {
            get
            {
                if (_Pincode == null)
                    return "";
                return _Pincode;
            }
            set
            {
                if (_Pincode != value)
                {
                    _Pincode = value;
                    OnPropertyChanged("Pincode");
                }
            }
        }

        private int _ResiNoCountryCode;
        public int ResiNoCountryCode
        {
            get { return _ResiNoCountryCode; }
            set
            {
                if (_ResiNoCountryCode != value)
                {
                    _ResiNoCountryCode = value;
                    //OnPropertyChanged("ResiNoCountryCode");
                }
            }
        }

        private int _MobileCountryCode;
        public int MobileCountryCode
        {
            get { return _MobileCountryCode; }
            set
            {
                if (_MobileCountryCode != value)
                {
                    _MobileCountryCode = value;
                    //OnPropertyChanged("ResiNoCountryCode");
                }
            }
        }

        private int _ResiSTDCode;
        public int ResiSTDCode
        {
            get { return _ResiSTDCode; }
            set
            {
                if (_ResiSTDCode != value)
                {
                    _ResiSTDCode = value;
                   // OnPropertyChanged("ResiSTDCode");
                }
            }
        }


        private long _ClusterID;
        public long ClusterID
        {
            get { return _ClusterID; }
            set
            {
                if (_ClusterID != value)
                {
                    _ClusterID = value;
                    // OnPropertyChanged("ResiSTDCode");
                }
            }
        }



        private string _ContactNo = "";
        public string ContactNo
        {
            get { return _ContactNo; }
            set
            {
                if (_ContactNo != value)
                {
                    _ContactNo = value;
                    //OnPropertyChanged("ContactNo");
                }
            }
        }

        private string _ContactNo1 = "";
        public string ContactNo1
        {
            get { return _ContactNo1; }
            set
            {
                if (_ContactNo1 != value)
                {
                    _ContactNo1 = value;
                    //OnPropertyChanged("ContactNo");
                }
            }
        }

        private string _MobileNO = "";
        public string MobileNO
        {
            get { return _MobileNO; }
            set
            {
                if (_MobileNO != value)
                {
                    _MobileNO = value;
                    //OnPropertyChanged("ContactNo");
                }
            }
        }

        private string _FaxNo = "";
        public string FaxNo
        {
            get { return _FaxNo; }
            set
            {
                if (_FaxNo != value)
                {
                    _FaxNo = value;
                    OnPropertyChanged("FaxNo");
                }
            }
        }

        //added by rohini datred 5.2.16 as per client requiremet 

        private bool _IsProcessingUnit;
        public bool IsProcessingUnit
        {
            get { return _IsProcessingUnit; }
            set
            {
                if (_IsProcessingUnit != value)
                {
                    _IsProcessingUnit = value;
                    OnPropertyChanged("IsProcessingUnit");
                }
            }
        }
        private bool _IsCollectionUnit ;
        public bool IsCollectionUnit
        {
            get { return _IsCollectionUnit; }
            set
            {
                if (_IsCollectionUnit != value)
                {
                    _IsCollectionUnit = value;
                    OnPropertyChanged("IsCollectionUnit");
                }
            }
        }

        //

        private string _Email = "";
        public string Email
        {
            get { return _Email; }
            set
            {
                if (_Email != value)
                {
                    _Email = value;
                    OnPropertyChanged("Email");
                }
            }
        }

        private string _ServerName = "";
        public string ServerName
        {
            get { return _ServerName; }
            set
            {
                if (_ServerName != value)
                {
                    _ServerName = value;
                    OnPropertyChanged("ServerName");
                }
            }
        }

        private string _DatabaseName = "";
        public string DatabaseName
        {
            get { return _DatabaseName; }
            set
            {
                if (_DatabaseName != value)
                {
                    _DatabaseName = value;
                    OnPropertyChanged("DatabaseName");
                }
            }
        }

        private string _PharmacyLNo = "";
        public string PharmacyLicenseNo
        {
            get { return _PharmacyLNo; }
            set
            {
                if (_PharmacyLNo != value)
                {
                    _PharmacyLNo = value;
                    OnPropertyChanged("PharmacyLicenseNo");
                }
            }
        }

        private string _ClinicRegNo;
        public string ClinicRegNo
        {
            get { return _ClinicRegNo; }
            set
            {
                if (_ClinicRegNo != value)
                {
                    _ClinicRegNo = value;
                    OnPropertyChanged("ClinicRegNo");
                }
            }
        }

        private string _TINNo;
        public string TINNo
        {
            get { return _TINNo; }
            set
            {
                if (_TINNo != value)
                {
                    _TINNo = value;
                    OnPropertyChanged("TINNo");
                }
            }
        }

        //added by neena
        private string _GSTNNo;
        public string GSTNNo
        {
            get { return _GSTNNo; }
            set
            {
                if (_GSTNNo != value)
                {
                    _GSTNNo = value;
                    OnPropertyChanged("GSTNNo");
                }
            }
        }
        //

        private string _ShopNo;
        public string ShopNo
        {
            get { return _ShopNo; }
            set
            {
                if (_ShopNo != value)
                {
                    _ShopNo = value;
                    OnPropertyChanged("ShopNo");
                }
            }
        }

        private string _TradeNo;
        public string TradeNo
        {
            get { return _TradeNo; }
            set
            {
                if (_TradeNo != value)
                {
                    _TradeNo = value;
                    OnPropertyChanged("TradeNo");
                }
            }
        }

       // Added by Akshays
        private long _Cityid;
        public long Cityid
        {
            get { return _Cityid; }
            set
            {
                if (value != _Cityid)
                {
                    _Cityid = value;
                    OnPropertyChanged("Cityid");
                }
            }
        }

        private long _Stateid;
        public long Stateid
        {
            get { return _Stateid; }
            set
            {
                if (value != _Stateid)
                {
                    _Stateid = value;
                    OnPropertyChanged("Stateid");
                }
            }
        }

        private long _Countryid;
        public long Countryid
        {
            get { return _Countryid; }
            set
            {
                if (value != _Countryid)
                {
                    _Countryid = value;
                    OnPropertyChanged("Countryid");
                }
            }
        }

        private long _Areaid;
        public long Areaid
        {
            get { return _Areaid; }
            set
            {
                if (value != _Areaid)
                {
                    _Areaid = value;
                    OnPropertyChanged("Areaid");
                }
            }
        }
        #endregion

        #region CommonField

        private bool blnStatus;
        public bool Status
        {
            get { return blnStatus; }
            set
            {
                if (value != blnStatus)
                {
                    blnStatus = value;
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


        private string _AddedOn;
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

        private DateTime? _AddedDateTime = DateTime.Now;
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

        private string _AddedWindowsLoginName;
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


        private string _UpdatedBy;
        public string UpdatedBy
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

        private string _UpdatedOn;
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

        private DateTime? _UpdatedDateTime = DateTime.Now;
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

        private string _UpdatedWindowsLoginName;
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

        private string _Remark;

        public string Remark
        {
            get { return _Remark; }
            set { _Remark = value; }
        }

        private bool _ChkSelect;

        public bool ChkSelect
        {
            get { return _ChkSelect; }
            set { _ChkSelect = value; }
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

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("PropertyName"));
        }

        #endregion
    }

    public class clsDepartmentDetailsVO
    {
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

                }
            }
        }


        private long _DepartmentID;
        public long DepartmentID
        {
            get
            {
                return _DepartmentID;
            }

            set
            {
                if (value != _DepartmentID)
                {
                    _DepartmentID = value;

                }
            }
        }

        public string Department { get; set; }

        public string Code { get; set; }

        private long _UnitID;
        public long UnitID
        {
            get
            {
                return _UnitID;
            }

            set
            {
                if (value != _UnitID)
                {
                    _UnitID = value;

                }
            }
        }

        public string UnitName { get; set; }

        private bool _Status;
        public bool Status
        {
            get
            {
                return _Status;
            }

            set
            {
                if (value != _Status)
                {
                    _Status = value;

                }
            }
        }

        private bool _IsDefault;
        public bool IsDefault
        {
            get
            {
                return _IsDefault;
            }

            set
            {
                if (value != _IsDefault)
                {
                    _IsDefault = value;

                }
            }
        }

        private bool _IsActive;
        public bool IsActive
        {
            get { return _IsActive; }
            set
            {
                if (value != _IsActive)
                {
                    _IsActive = value;
              
                }
            }
        }
    }
}
