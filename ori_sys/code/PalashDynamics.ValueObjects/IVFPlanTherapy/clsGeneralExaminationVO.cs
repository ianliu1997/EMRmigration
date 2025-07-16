using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
  public class clsGeneralExaminationVO:IValueObject,INotifyPropertyChanged
    {
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


        private float _Weight;
        public float Weight
        {
            get { return _Weight; }
            set
            {
                if (_Weight != value)
                {
                    _Weight = value;
                    OnPropertyChanged("Weight");
                }
            }
        }

        private float _Height;
        public float Height
        {
            get { return _Height; }
            set
            {
                if (_Height != value)
                {
                    _Height = value;
                    OnPropertyChanged("Height");
                }
            }
        }

        private float _BMI;
        public float BMI
        {
            get { return _BMI; }
            set
            {
                if (_BMI != value)
                {
                    _BMI = value;
                    OnPropertyChanged("BMI");
                }
            }
        }

        private float _BpInMm;
        public float BpInMm
        {
            get { return _BpInMm; }
            set
            {
                if (_BpInMm != value)
                {
                    _BpInMm = value;
                    OnPropertyChanged("BpInMm");
                }
            }
        }

        private float _BpInHg;
        public float BpInHg
        {
            get { return _BpInHg; }
            set
            {
                if (_BpInHg != value)
                {
                    _BpInHg = value;
                    OnPropertyChanged("BpInHg");
                }
            }
        }

        private string _Built;
        public string Built
        {
            get { return _Built; }
            set
            {
                if (_Built != value)
                {
                    _Built = value;
                    OnPropertyChanged("Built");
                }
            }
        }

        private float _Pulse;
        public float Pulse
        {
            get { return _Pulse; }
            set
            {
                if (_Pulse != value)
                {
                    _Pulse = value;
                    OnPropertyChanged("Pulse");
                }
            }
        }

        private string _Fat;
        public string Fat
        {
            get { return _Fat; }
            set
            {
                if (_Fat != value)
                {
                    _Fat = value;
                    OnPropertyChanged("Fat");
                }
            }
        }

        private string _PA;
        public string PA
        {
            get { return _PA; }
            set
            {
                if (_PA != value)
                {
                    _PA = value;
                    OnPropertyChanged("PA");
                }
            }
        }

        private string _RS;
        public string RS
        {
            get { return _RS; }
            set
            {
                if (_RS != value)
                {
                    _RS = value;
                    OnPropertyChanged("RS");
                }
            }
        }

        private string _CVS;
        public string CVS
        {
            get { return _CVS; }
            set
            {
                if (_CVS != value)
                {
                    _CVS = value;
                    OnPropertyChanged("CVS");
                }
            }
        }


        private string _CNS;
        public string CNS
        {
            get { return _CNS; }
            set
            {
                if (_CNS != value)
                {
                    _CNS = value;
                    OnPropertyChanged("CNS");
                }
            }
        }


        private string _Thyroid;
        public string Thyroid
        {
            get { return _Thyroid; }
            set
            {
                if (_Thyroid != value)
                {
                    _Thyroid = value;
                    OnPropertyChanged("Thyroid");
                }
            }
        }

        private string _Gynaecomastia;
        public string Gynaecomastia
        {
            get { return _Gynaecomastia; }
            set
            {
                if (_Gynaecomastia != value)
                {
                    _Gynaecomastia = value;
                    OnPropertyChanged("Gynaecomastia");
                }
            }
        }

        private string _SecondarySexCharacters;
        public string SecondarySexCharacters
        {
            get { return _SecondarySexCharacters; }
            set
            {
                if (_SecondarySexCharacters != value)
                {
                    _SecondarySexCharacters = value;
                    OnPropertyChanged("SecondarySexCharacters");
                }
            }
        }

        private string _Acne;
        public string Acne
        {
            get { return _Acne; }
            set
            {
                if (_Acne != value)
                {
                    _Acne = value;
                    OnPropertyChanged("Acne");
                }
            }
        }

        private string _Hirsutism;
        public string Hirsutism
        {
            get { return _Hirsutism; }
            set
            {
                if (_Hirsutism != value)
                {
                    _Hirsutism = value;
                    OnPropertyChanged("Hirsutism");
                }
            }
        }


        private bool _HIVPositive;
        public bool HIVPositive
        {
            get { return _HIVPositive; }
            set
            {
                if (_HIVPositive != value)
                {
                    _HIVPositive = value;
                    OnPropertyChanged("HIVPositive");
                }
            }
        }

        private string _EyeColor;
        public string EyeColor
        {
            get { return _EyeColor; }
            set
            {
                if (_EyeColor != value)
                {
                    _EyeColor = value;
                    OnPropertyChanged("EyeColor");
                }
            }
        }

        private string _HairColor;
        public string HairColor
        {
            get { return _HairColor; }
            set
            {
                if (_HairColor != value)
                {
                    _HairColor = value;
                    OnPropertyChanged("HairColor");
                }
            }
        }

        private string _SkinColor;
        public string SkinColor
        {
            get { return _SkinColor; }
            set
            {
                if (_SkinColor != value)
                {
                    _SkinColor = value;
                    OnPropertyChanged("SkinColor");
                }
            }
        }

        private string _PhysicalBuilt;
        public string PhysicalBuilt
        {
            get { return _PhysicalBuilt; }
            set
            {
                if (_PhysicalBuilt != value)
                {
                    _PhysicalBuilt = value;
                    OnPropertyChanged("PhysicalBuilt");
                }
            }
        }

        private string _ExternalGenitalExam;
        public string ExternalGenitalExam
        {
            get { return _ExternalGenitalExam; }
            set
            {
                if (_ExternalGenitalExam != value)
                {
                    _ExternalGenitalExam = value;
                    OnPropertyChanged("ExternalGenitalExam");
                }
            }
        }
        private string _Comments;
        public string Comments
        {
            get { return _Comments; }
            set
            {
                if (_Comments != value)
                {
                    _Comments = value;
                    OnPropertyChanged("Comments");
                }
            }
        }

        private string _Alterts;
        public string Alterts
        {
            get { return _Alterts; }
            set
            {
                if (_Alterts != value)
                {
                    _Alterts = value;
                    OnPropertyChanged("Alterts");
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
                    OnPropertyChanged("_Date");
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


      //By Anjali

        private long _EyeColor1;
        public long EyeColor1
        {
            get { return _EyeColor1; }
            set
            {
                if (_EyeColor1 != value)
                {
                    _EyeColor1 = value;
                    OnPropertyChanged("EyeColor1");
                }
            }
        }

        private long _HairColor1;
        public long HairColor1
        {
            get { return _HairColor1; }
            set
            {
                if (_HairColor1 != value)
                {
                    _HairColor1 = value;
                    OnPropertyChanged("HairColor1");
                }
            }
        }

        private long _SkinColor1;
        public long SkinColor1
        {
            get { return _SkinColor1; }
            set
            {
                if (_SkinColor1 != value)
                {
                    _SkinColor1 = value;
                    OnPropertyChanged("SkinColor1");
                }
            }
        }
        private long _BuiltID;
        public long BuiltID
        {
            get { return _BuiltID; }
            set
            {
                if (_BuiltID != value)
                {
                    _BuiltID = value;
                    OnPropertyChanged("BuiltID");
                }
            }
        }
        private bool _HBVPositive;
        public bool HBVPositive
        {
            get { return _HBVPositive; }
            set
            {
                if (_HBVPositive != value)
                {
                    _HBVPositive = value;
                    OnPropertyChanged("HBVPositive");
                }
            }
        }
        private bool _HCVPositive;
        public bool HCVPositive
        {
            get { return _HCVPositive; }
            set
            {
                if (_HCVPositive != value)
                {
                    _HCVPositive = value;
                    OnPropertyChanged("HCVPositive");
                }
            }
        }
      //------------------------------------------------------------------------------
        private string _ThyroidExam;
        public string ThyroidExam
        {
            get { return _ThyroidExam; }
            set
            {
                if (_ThyroidExam != value)
                {
                    _ThyroidExam = value;
                    OnPropertyChanged("ThyroidExam");
                }
            }
        }

        private string _BreastExam;
        public string BreastExam
        {
            get { return _BreastExam; }
            set
            {
                if (_BreastExam != value)
                {
                    _BreastExam = value;
                    OnPropertyChanged("BreastExam");
                }
            }
        }

        private string _AbdominalExam;
        public string AbdominalExam
        {
            get { return _AbdominalExam; }
            set
            {
                if (_AbdominalExam != value)
                {
                    _AbdominalExam = value;
                    OnPropertyChanged("AbdominalExam");
                }
            }
        }


        private string _PelvicExam;
        public string PelvicExam
        {
            get { return _PelvicExam; }
            set
            {
                if (_PelvicExam != value)
                {
                    _PelvicExam = value;
                    OnPropertyChanged("PelvicExam");
                }
            }
        }
      //------------------------------------------------------------------------------
        public bool Acen1 { get; set; }
        public bool AcenYes { get; set; }
        public bool AcenNo { get; set; }
        public bool Hirsutism1 { get; set; }
        public bool HirsutismYes { get; set; }
        public bool HirsutismNo { get; set; }

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

        #region Common Properties

        
       
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

       



        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitID");
                }
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitID
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitID");
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

        private string _AddedOn;
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

        private string _AddedWindowsLoginName;
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

        private string _UpdatedOn;
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

        private string _UpdatedWindowsLoginName;
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

        private bool _Synchronized;
        public bool Synchronized
        {
            get { return _Synchronized; }
            set
            {
                if (_Synchronized != value)
                {
                    _Synchronized = value;
                    OnPropertyChanged("Synchronized");
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
