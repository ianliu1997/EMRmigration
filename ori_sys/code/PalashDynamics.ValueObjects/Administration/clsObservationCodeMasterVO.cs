//Created Date:15/August/2012
//Created By: Nilesh Raut
//Specification: To Value Object For M_ObservationCodeMaster

//Review By:
//Review Date:

//Modified By:
//Modified Date: 

using System;
using System.ComponentModel;
using System.Collections.Generic;


namespace PalashDynamics.ValueObjects
{
   public class clsObservationCodeMasterVO : IValueObject, INotifyPropertyChanged
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

        private long _CodeTypeID;
        public long CodeTypeID
        {
            get { return _CodeTypeID; }
            set
            {
                if (value != _CodeTypeID)
                {
                    _CodeTypeID = value;
                    OnPropertyChanged("CodeTypeID");
                }
            }
        }
        private string _CodeType = "";
        public string CodeType
        {
            get { return _CodeType; }
            set
            {
                if (value != _CodeType)
                {
                    _CodeType = value;
                    OnPropertyChanged("CodeType");
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

        private long _ResultStatus;
        public long ResultStatus
        {
            get { return _ResultStatus; }
            set
            {
                if (value != _ResultStatus)
                {
                    _ResultStatus = value;
                    OnPropertyChanged("ResultStatus");
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

   public class clsAddUpdateObservationCodeBizActionVO : IBizActionValueObject
   {
       private clsObservationCodeMasterVO _objObservationCodeMasterDetails = null;
       public clsObservationCodeMasterVO ObservationCodeMasterDetails
       {
           get { return _objObservationCodeMasterDetails; }
           set { _objObservationCodeMasterDetails = value; }

       }

       private int _SuccessStatus;
       public int SuccessStatus
       {
           get { return _SuccessStatus; }
           set { _SuccessStatus = value; }
       }
       private long _ResultStatus;
       public long ResultStatus
       {
           get { return _ResultStatus; }
           set { _ResultStatus = value; }
       }

       #region  IBizActionValueObject
       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.clsAddUpdateObservationCodeBizAction";
       }

       public string ToXml()
       {
           return this.ToXml();
       }
       #endregion
   }

   public class clsGetObservationCodeMasterBizActionVO : IBizActionValueObject
   {

       #region  IBizActionValueObject
       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.clsGetObservationCodeMasterBizAction";
       }

       public string ToXml()
       {
           return this.ToXml();
       }

       public long StartRowIndex { get; set; }
       public long MaximumRows { get; set; }
       public long TotalRows { get; set; }
       public bool PagingEnabled { get; set; }      
       public string Code { get; set; }
       public string Description { get; set; }
       public long ObservationCodeID { get; set; }
       public long CodeTypeId { get; set; }
       public long UnitID { get; set; }     

       #endregion

       /// <summary>
       /// This property contains Item master details.
       /// </summary>
       private List<clsObservationCodeMasterVO> objObservationCodeMasterDetails = new List<clsObservationCodeMasterVO>();
       public List<clsObservationCodeMasterVO> ObservationCodeMasterDetails
       {
           get
           {
               return objObservationCodeMasterDetails;
           }
           set
           {
               objObservationCodeMasterDetails = value;

           }
       }

       private int _SuccessStatus;
       public int SuccessStatus
       {
           get { return _SuccessStatus; }
           set { _SuccessStatus = value; }
       }
       private long _ResultStatus;
       public long ResultStatus
       {
           get { return _ResultStatus; }
           set { _ResultStatus = value; }
       }
   }

   public class clsUpdateObservationCodeStatusBizActionVO : IBizActionValueObject
   {
       
       public long ObservationCodeID { get; set; }
       public long UnitID { get; set; }
       public bool Status { get; set; }

       private long _SuccessStatus;
       public long SuccessStatus
       {
           get { return _SuccessStatus; }
           set { _SuccessStatus = value; }
       }      

       #region  IBizActionValueObject
       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.clsUpdateObservationCodeStatusBizAction";
       }

       public string ToXml()
       {
           return this.ToXml();
       }
       #endregion
   }

}
