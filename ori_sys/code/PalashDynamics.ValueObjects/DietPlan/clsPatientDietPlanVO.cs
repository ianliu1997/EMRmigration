using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects
{
   public class clsPatientDietPlanVO :IValueObject, INotifyPropertyChanged
    {
        public string ToXml()
        {
            return this.ToString();
        }

        #region Property Declaration

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

        private long _PatientUnitID;
        public long PatientUnitID
        {
            get { return _PatientUnitID; }
            set
            {
                if (_PatientUnitID != value)
                {
                    _PatientUnitID = value;
                    OnPropertyChanged("PatientUnitID");
                }
            }
        }


        private long _VisitID;
        public long VisitID
        {
            get { return _VisitID; }
            set
            {
                if (_VisitID != value)
                {
                    _VisitID = value;
                    OnPropertyChanged("VisitID");
                }
            }
        }

        private long _VisitDoctorID;
        public long VisitDoctorID
        {
            get { return _VisitDoctorID; }
            set
            {
                if (_VisitDoctorID != value)
                {
                    _VisitDoctorID = value;
                    OnPropertyChanged("VisitDoctorID");
                }
            }
        }

        private string _VisitDoctor;
        public string VisitDoctor
        {
            get { return _VisitDoctor; }
            set
            {
                if (_VisitDoctor != value)
                {
                    _VisitDoctor = value;
                    OnPropertyChanged("VisitDoctor");
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

        private long _PlanID;
        public long PlanID
        {
            get { return _PlanID; }
            set
            {
                if (_PlanID != value)
                {
                    _PlanID = value;
                    OnPropertyChanged("PlanID");
                }
            }
        }


        private string _PlanName;
        public string PlanName
        {
            get { return _PlanName; }
            set
            {
                if (_PlanName != value)
                {
                    _PlanName = value;
                    OnPropertyChanged("PlanName");
                }
            }
        }
        private string _GeneralInformation;
        public string GeneralInformation
        {
            get { return _GeneralInformation; }
            set
            {
                if (_GeneralInformation != value)
                {
                    _GeneralInformation = value;
                    OnPropertyChanged("GeneralInformation");
                }
            }
        }

        private List<clsPatientDietPlanDetailVO> _DietDetails;
        public List<clsPatientDietPlanDetailVO> DietDetails
        {
            get
            {
                if (_DietDetails == null)
                    _DietDetails = new List<clsPatientDietPlanDetailVO>();

                return _DietDetails;
            }
            set
            {
                _DietDetails = value;

            }
        }

        #endregion

        #region Common Properties


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
    
    }

   public class clsPatientDietPlanDetailVO : IValueObject, INotifyPropertyChanged
   {
       public string ToXml()
       {
           return this.ToString();
       }

       #region Property Declaration


       private long _DietPlanID;
       public long DietPlanID
       {
           get { return _DietPlanID; }
           set
           {
               if (_DietPlanID != value)
               {
                   _DietPlanID = value;
                   OnPropertyChanged("DietPlanID");
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


       private long _FoodItemID;
       public long FoodItemID
       {
           get { return _FoodItemID; }
           set
           {
               if (_FoodItemID != value)
               {
                   _FoodItemID = value;
                   OnPropertyChanged("FoodItemID");
               }
           }
       }

       private string _FoodItem;
       public string FoodItem
       {
           get { return _FoodItem; }
           set
           {
               if (_FoodItem != value)
               {
                   _FoodItem = value;
                   OnPropertyChanged("FoodItem");
               }
           }
       }
       
      
       private string _FoodItemCategory;


      
       public string FoodItemCategory
       {
           get { return _FoodItemCategory; }
           set
           {
               if (_FoodItemCategory != value)
               {
                   _FoodItemCategory = value;
                   OnPropertyChanged("FoodItemCategory");
               }
           }
       }


       private long _FoodItemCategoryID;
       public long FoodItemCategoryID
       {
           get { return _FoodItemCategoryID; }
           set
           {
               
               if (_FoodItemCategoryID != value)
               {
                   _FoodItemCategoryID = value;
                   OnPropertyChanged("FoodItemCategoryID");
               }
           }
       }

       private string _Timing;
       public string Timing
       {
           get { return _Timing; }
           set
           {
               if (_Timing != value)
               {
                   _Timing = value;
                   OnPropertyChanged("Timing");
               }
           }
       }

       private string _FoodQty;
       public string FoodQty
       {
           get { return _FoodQty; }
           set
           {
               if (_FoodQty != value)
               {
                   _FoodQty = value;
                   OnPropertyChanged("FoodQty");
               }
           }
       }

       private string _FoodUnit;
       public string FoodUnit
       {
           get { return _FoodUnit; }
           set
           {
               if (_FoodUnit != value)
               {
                   _FoodUnit = value;
                   OnPropertyChanged("FoodUnit");
               }
           }
       }

       private string _FoodCal;
       public string FoodCal
       {
           get { return _FoodCal; }
           set
           {
               if (_FoodCal != value)
               {
                   _FoodCal = value;
                   OnPropertyChanged("FoodCal");
               }
           }
       }

       private string _FoodCH;
       public string FoodCH
       {
           get { return _FoodCH; }
           set
           {
               if (_FoodCH != value)
               {
                   _FoodCH = value;
                   OnPropertyChanged("FoodCH");
               }
           }
       }

       private string _FoodFat;
       public string FoodFat
       {
           get { return _FoodFat; }
           set
           {
               if (_FoodFat != value)
               {
                   _FoodFat = value;
                   OnPropertyChanged("FoodFat");
               }
           }
       }

       private string _FoodExpectedCal;
       public string FoodExpectedCal
       {
           get { return _FoodExpectedCal; }
           set
           {
               if (_FoodExpectedCal != value)
               {
                   _FoodExpectedCal = value;
                   OnPropertyChanged("FoodExpectedCal");
               }
           }
       }

       private string _FoodInstruction;
       public string FoodInstruction
       {
           get { return _FoodInstruction; }
           set
           {
               if (_FoodInstruction != value)
               {
                   _FoodInstruction = value;
                   OnPropertyChanged("FoodInstruction");
               }
           }
       }


      

       #endregion

       #region Common Properties


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

   }
}
