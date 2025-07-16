using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Administration.PatientSourceMaster
{
   public  class clsPatientSourceVO : INotifyPropertyChanged, IValueObject
    {

       private List<clsTariffDetailsVO> _TariffDetails;
       public List<clsTariffDetailsVO> TariffDetails
        {
            get
            {
                if (_TariffDetails == null)
                    _TariffDetails = new List<clsTariffDetailsVO>();

                return _TariffDetails;
            }

            set
            {

                _TariffDetails = value;

            }
        }


       //ROHINEE FOR PATIENT CATEGORY MASTER
       private long _PatientCatagoryID;
       public long PatientCatagoryID
       {
           get { return _PatientCatagoryID; }
           set
           {
               if (value != _PatientCatagoryID)
               {
                   _PatientCatagoryID = value;
                   OnPropertyChanged("PatientCatagoryID");
               }
           }
       }

       private string _PatientCategoryName;
       public string PatientCategoryName
       {
           get { return _PatientCategoryName; }
           set
           {
               if (value != _PatientCategoryName)
               {
                   _PatientCategoryName = value;
                   OnPropertyChanged("PatientCategoryName");

               }
           }
       }

       //
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

        private string _Code = "";
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

        private string _Description = "";
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

        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set
            {
                if (value != _UnitID)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }

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

        private DateTime? _FromDate;
        public DateTime? FromDate
        {
            get { return _FromDate; }
            set
            {
                if (value != _FromDate)
                {
                    _FromDate = value;
                    OnPropertyChanged("FromDate");
                }
            }

        }

        private DateTime? _ToDate;
        public DateTime? ToDate
        {
            get { return _ToDate; }
            set
            {
                if (value != _ToDate)
                {
                    _ToDate = value;
                    OnPropertyChanged("ToDate");
                }
            }
        }

        public short PatientSourceType { get; set; }
        public long PatientSourceTypeID { get; set; }

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
        public override string ToString()
        {
            return Description; ;
        }

        #endregion
    }


    public class clsTariffDetailsVO: INotifyPropertyChanged, IValueObject
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

         private bool _IsFamily;
         public bool IsFamily
         {
             get { return _IsFamily; }
             set
             {
                 if (value != _IsFamily)
                 {
                     _IsFamily = value;
                     OnPropertyChanged("IsFamily");
                 }
             }
         }
         private long _PatientSourceID;
         public long PatientSourceID
         {
             get { return _PatientSourceID; }
             set
             {
                 if (value != _PatientSourceID)
                 {
                     _PatientSourceID = value;
                     OnPropertyChanged("PatientSourceID");
                 }
             }
         }

         private long _TariffID;
         public long TariffID
         {
             get { return _TariffID; }
             set
             {
                 if (value != _TariffID)
                 {
                     _TariffID = value;
                     OnPropertyChanged("TariffID");
                 }
             }
         }

         private string _TariffCode = "";
         public string TariffCode
         {
             get { return _TariffCode; }
             set
             {
                 if (value != _TariffCode)
                 {
                     _TariffCode = value;
                     OnPropertyChanged("TariffCode");
                 }
             }
         }

         private string _TariffDescription = "";
         public string TariffDescription
         {
             get { return _TariffDescription; }
             set
             {
                 if (value != _TariffDescription)
                 {
                     _TariffDescription = value;
                     OnPropertyChanged("TariffDescription");
                 }
             }
         }

         private long _UnitID;
         public long UnitID
         {
             get { return _UnitID; }
             set
             {
                 if (value != _UnitID)
                 {
                     _UnitID = value;
                     OnPropertyChanged("UnitID");
                 }
             }
         }

         private bool _Status ;
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

         private bool _IsDefaultStatus;
         public bool IsDefaultStatus
         {
             get { return _IsDefaultStatus; }
             set
             {
                 if (value != _IsDefaultStatus)
                 {
                     _IsDefaultStatus = value;
                     OnPropertyChanged("IsDefaultStatus");
                 }
             }
         }


         private long _CompanyID;
         public long CompanyID
         {
             get { return _CompanyID; }
             set
             {
                 if (value != _CompanyID)
                 {
                     _CompanyID = value;
                     OnPropertyChanged("CompanyID");
                 }
             }
         }
         private bool _SelectedTariff = false;
         public bool SelectedTariff
         {
             get { return _SelectedTariff; }
             set
             {
                 if (value != _SelectedTariff)
                 {
                     _SelectedTariff = value;
                     OnPropertyChanged("SelectedTariff");
                 }
             }
         }

         private bool _IsSelectedTariff = true;
         public bool IsSelectedTariff
         {
             get { return _IsSelectedTariff; }
             set
             {
                 if (value != _IsSelectedTariff)
                 {
                     _IsSelectedTariff = value;
                     OnPropertyChanged("IsSelectedTariff");
                 }
             }
         }
         private bool _SelectTariff;
         public bool SelectTariff
         {
             get { return _SelectTariff; }
             set
             {
                 if (value != _SelectTariff)
                 {
                     _SelectTariff = value;
                     OnPropertyChanged("SelectTariff");
                 }
             }
         }

         #region CommonFileds

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

    //by rohinee for patient registration  
    public class clsRegistrationChargesVO : INotifyPropertyChanged, IValueObject
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


        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set
            {
                if (value != _UnitID)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }
        //from master list patient id
        private long _PatientCategoryID;
        public long PatientCategoryID
        {
            get { return _PatientCategoryID; }
            set
            {
                if (value != _PatientCategoryID)
                {
                    _PatientCategoryID = value;
                    OnPropertyChanged("PatientCategoryID");
                }
            }
        }
        private long _PatientTypeId;
        public long PatientTypeId
        {
            get { return _PatientTypeId; }
            set
            {
                if (value != _PatientTypeId)
                {
                    _PatientTypeId = value;
                    OnPropertyChanged("PatientTypeId");
                }
            }
        }


        private string _PatientName;
        public string PatientName
        {
            get { return _PatientName; }
            set
            {
                if (value != _PatientName)
                {
                    _PatientName = value;
                    OnPropertyChanged("PatientName");

                }
            }
        }


        private long _PatientServiceId;
        public long PatientServiceId
        {
            get { return _PatientServiceId; }
            set
            {
                if (value != _PatientServiceId)
                {
                    _PatientServiceId = value;
                    OnPropertyChanged("PatientServiceId");
                }
            }
        }

        private string _PatientServiceName;
        public string PatientServiceName
        {
            get { return _PatientServiceName; }
            set
            {
                if (value != _PatientServiceName)
                {
                    _PatientServiceName = value;
                    OnPropertyChanged("PatientServiceName");

                }
            }
        }

        private double _Rate;
        public double Rate
        {
            get { return _Rate; }
            set
            {
                if (value != _Rate)
                {
                    _Rate = value;
                    OnPropertyChanged("Rate");
                }
            }
        }

     

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

        private DateTime? _FromDate;
        public DateTime? FromDate
        {
            get { return _FromDate; }
            set
            {
                if (value != _FromDate)
                {
                    _FromDate = value;
                    OnPropertyChanged("FromDate");
                }
            }

        }

        private DateTime? _ToDate;
        public DateTime? ToDate
        {
            get { return _ToDate; }
            set
            {
                if (value != _ToDate)
                {
                    _ToDate = value;
                    OnPropertyChanged("ToDate");
                }
            }
        }

        public short PatientSourceType { get; set; }
        public long PatientSourceTypeID { get; set; }

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
}
