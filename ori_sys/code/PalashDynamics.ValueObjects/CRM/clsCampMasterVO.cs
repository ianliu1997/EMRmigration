using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Administration;

namespace PalashDynamics.ValueObjects.CRM
{
     public class clsCampMasterVO : INotifyPropertyChanged,IValueObject
    {






         private List<CampserviceDetailsVO> _FreeCampServiceList = new List<CampserviceDetailsVO>();
        public List<CampserviceDetailsVO> FreeCampServiceList
        {
            get { return _FreeCampServiceList; }
            set
            {
                if (_FreeCampServiceList != value)
                {
                    _FreeCampServiceList = value;
                    OnPropertyChanged("FreeCampServiceList");
                }
            }
        }


        private List<CampserviceDetailsVO> _ConcessionServiceList = new List<CampserviceDetailsVO>();
        public List<CampserviceDetailsVO> ConcessionServiceList
        {
            get { return _ConcessionServiceList; }
            set
            {
                if (_ConcessionServiceList != value)
                {
                    _ConcessionServiceList = value;
                    OnPropertyChanged("ConcessionServiceList");
                }
            }
        }


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

        private long _CampDetailID;
        public long CampDetailID
        {
            get { return _CampDetailID; }
            set
            {
                if (value != _CampDetailID)
                {
                    _CampDetailID = value;
                    OnPropertyChanged("CampDetailID");
                }
            }
        }

        private long _CampServiceID;
        public long CampServiceID
        {
            get { return _CampServiceID; }
            set
            {
                if (value != _CampServiceID)
                {
                    _CampServiceID = value;
                    OnPropertyChanged("CampServiceID");
                }
            }
        }

        private string _Code ="";
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

        private string _Description ="";
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

        private DateTime? _PatientRegistrationValidTillDate;
        public DateTime? PatientRegistrationValidTillDate
        {
            get { return _PatientRegistrationValidTillDate; }
            set
            {
                if (value != _PatientRegistrationValidTillDate)
                {
                    _PatientRegistrationValidTillDate = value;
                    OnPropertyChanged("PatientRegistrationValidTillDate");
                }
            }
        }
         

       


       

        private long _GroupID;
        public long GroupID
        {
            get { return _GroupID; }
            set
            {
                if (value != _GroupID)
                {
                    _GroupID = value;
                    OnPropertyChanged("GroupID");
                }
            }
        }

        private long _SubGroupID;
        public long SubGroupID
        {
            get { return _SubGroupID; }
            set
            {
                if (value != _SubGroupID)
                {
                    _SubGroupID = value;
                    OnPropertyChanged("SubGroupID");
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

        private long _CampID;
        public long CampID
        {
            get { return _CampID; }
            set
            {
                if (value != _CampID)
                {
                    _CampID = value;
                    OnPropertyChanged("CampID");
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

        private string _City ="";
        public string City
        {
            get { return _City; }
            set
            {
                if (value != _City)
                {
                    _City = value;
                    OnPropertyChanged("City");
                }
            }
        }

        private string _Area ="";
        public string Area
        {
            get { return _Area; }
            set
            {
                if (value != _Area)
                {
                    _Area = value;
                    OnPropertyChanged("Area");
                }
            }
        }

         private double? _ValidDays;
         public double? ValidDays
        {
            get { return _ValidDays; }
            set
            {
                if (value != _ValidDays)
                {
                    _ValidDays = value;
                    OnPropertyChanged("ValidDays");
                }
            }
        }

         private double? _Concession;
         public double? Concession
         {
             get { return _Concession; }
             set
             {
                 if (value != _Concession)
                 {
                     _Concession = value;
                     OnPropertyChanged("Concession");
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

         private string _Tariff ="";
         public string Tariff
         {
             get { return _Tariff; }
             set
             {
                 if (value != _Tariff)
                 {
                     _Tariff = value;
                     OnPropertyChanged("Tariff");
                 }
             }

         }

         private string _Reason ="";
         public string Reason
         {
             get { return _Reason; }
             set
             {
                 if (value != _Reason)
                 {
                     _Reason = value;
                     OnPropertyChanged("Reason");
                 }
             }
         }

         private long _EmailTemplateID;
         public long EmailTemplateID
         {
             get { return _EmailTemplateID; }
             set
             {
                 if (value != _EmailTemplateID)
                 {
                     _EmailTemplateID = value;
                     OnPropertyChanged("EmailTemplateID");
                 }

             }
         }

         private long _SmsTemplateID;
         public long SmsTemplateID
         {
             get { return _SmsTemplateID; }
             set
             {
                 if (value != _SmsTemplateID)
                 {
                     _SmsTemplateID = value;
                     OnPropertyChanged("SmsTemplateID");
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
       
        private string _AddedOn ="";
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

        private string _AddedWindowsLoginName ="";
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

        private string _UpdatedOn ="";
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

        private string _UpdatedWindowsLoginName ="";
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

        private clsEmailTemplateVO _EmailTemplateDetails = new clsEmailTemplateVO();
        public clsEmailTemplateVO EmailTemplateDetails
        {
            get { return _EmailTemplateDetails; }
            set
            {
                if (value != _EmailTemplateDetails)
                {
                    _EmailTemplateDetails = value;
                }
            }
        }

        private clsSMSTemplateVO _SMSTemplateDetails= new clsSMSTemplateVO();
        public clsSMSTemplateVO SMSTemplateDetails
        {
            get { return _SMSTemplateDetails; }
            set 
            {
                if (value != _SMSTemplateDetails)
                {
                    _SMSTemplateDetails = value;
                }
            }
        }


       



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


     public class CampserviceDetailsVO : INotifyPropertyChanged, IValueObject
     {

         private long _ID;
         public long CampServiceID
         {
             get { return _ID; }
             set
             {
                 if (value != _ID)
                 {
                     _ID = value;
                     OnPropertyChanged("CampServiceID");
                 }
             }
         }

         private long _CampDetailsID;
         public long CampDetailsID
         {
             get { return _CampDetailsID; }
             set
             {
                 if (value != _CampDetailsID)
                 {
                     _CampDetailsID = value;
                     OnPropertyChanged("CampDetailsID");
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
                 


         private long _ServiceID;
         public long ServiceID
         {
             get { return _ServiceID; }
             set
             {
                 if (value != _ServiceID)
                 {
                     _ServiceID = value;
                     OnPropertyChanged("ServiceID");
                 }
             }
         }


         private bool _IsFree;
         public bool IsFree
         {
             get { return _IsFree; }
             set
             {
                 if (value != _IsFree)
                 {
                     _IsFree = value;
                     OnPropertyChanged("IsFree");
                 }
             }
         }

         private bool _IsConcession;
         public bool IsConcession
         {
             get { return _IsConcession; }
             set
             {
                 if (value != _IsConcession)
                 {
                     _IsConcession = value;
                     OnPropertyChanged("IsConcession");
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



         private string _ServiceName = "";
         public string ServiceName
         {
             get { return _ServiceName; }
             set
             {
                 if (value != _ServiceName)
                 {
                     _ServiceName = value;
                     OnPropertyChanged("ServiceName");
                 }
             }
         }

         private string _ServiceCode = "";
         public string ServiceCode
         {
             get { return _ServiceCode; }
             set
             {
                 if (value != _ServiceCode)
                 {
                     _ServiceCode = value;
                     OnPropertyChanged("ServiceCode");
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
                     OnPropertyChanged("ConcessionAmount");
                     OnPropertyChanged("NetAmount");
                  }
             }
         }

         //private double _ConcessionPercentage;
         //public double ConcessionPercentage
         //{
         //    get { return _ConcessionPercentage; }
         //    set
         //    {
         //        if (value != _ConcessionPercentage)
         //        {
         //            _ConcessionPercentage = value;
         //            OnPropertyChanged("ConcessionPercentage");
         //            OnPropertyChanged("ConcessionAmount");
         //            OnPropertyChanged("NetAmount");
         //        }
         //    }
         //}

         //private double _ConcessionAmount;
         //public double ConcessionAmount
         //{
         //    get { return _ConcessionAmount; }
         //    set
         //    {
         //        if (value != _ConcessionAmount)
         //        {
         //            _ConcessionAmount = value;
         //            OnPropertyChanged("ConcessionAmount");
         //            OnPropertyChanged("NetAmount");
         //        }
         //    }
         //}

         //private double _NetAmount;
         //public double NetAmount
         //{

         //    get 
         //    {
         //        return _NetAmount;
                
         //    }
         //    set
         //    {
         //        if (_NetAmount != value)
         //        {
         //            _NetAmount = value;
         //            OnPropertyChanged("NetAmount");
         //        }
         //    }
         //}



         private double _ConcessionPercentage;
         public double ConcessionPercentage
         {
             get { return _ConcessionPercentage; }
             set
             {
                 if (_ConcessionPercentage != value)
                 {
                     if (value < 0)
                         value = 0;
                     _ConcessionPercentage = value;

                     OnPropertyChanged("ConcessionPercentage");
                     OnPropertyChanged("ConcessionAmount");
                     OnPropertyChanged("NetAmount");
                 }
             }
         }

         private double _ConcessionAmount;
         public double ConcessionAmount
         {
             get
             {
                 if (_ConcessionPercentage != 0)
                 {
                     return _ConcessionAmount = ((Rate * _ConcessionPercentage) / 100);
                 }
                 else
                     return _ConcessionAmount;
             }
             set
             {
                 if (_ConcessionAmount != value)
                 {
                     if (value < 0)
                         value = 0;
                     _ConcessionAmount = value;
                     if (_ConcessionAmount > 0)
                         _ConcessionPercentage = (_ConcessionAmount * 100) / Rate;
                     else
                         _ConcessionPercentage = 0;

                     OnPropertyChanged("ConcessionPercentage");                
                     OnPropertyChanged("ConcessionAmount");
                     OnPropertyChanged("NetAmount");

                 }
             }
         }

         private double _NetAmount;
         public double NetAmount
         {
             
             get { return _NetAmount = _Rate - _ConcessionAmount; }
             set
             {
                 if (_NetAmount != value)
                 {
                     _NetAmount = value;
                     OnPropertyChanged("NetAmount");
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
     
}
