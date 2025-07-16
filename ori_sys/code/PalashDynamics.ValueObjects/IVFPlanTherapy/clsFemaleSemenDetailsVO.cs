using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
    public class clsFemaleSemenDetailsVO : IValueObject, INotifyPropertyChanged
    {

        #region Common Properties

        private bool _Status;
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
        
        // By BHUSHAN For IVF DashBoard
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

        private long _PlanTherapyID;
        public long PlanTherapyID
        {
            get { return _PlanTherapyID; }
            set
            {
                if (_PlanTherapyID != value)
                {
                    _PlanTherapyID = value;
                    OnPropertyChanged("PlanTherapyID");
                }
            }
        }

        private long _PlanTherapyUnitID;
        public long PlanTherapyUnitID
        {
            get { return _PlanTherapyUnitID; }
            set
            {
                if (_PlanTherapyUnitID != value)
                {
                    _PlanTherapyUnitID = value;
                    OnPropertyChanged("PlanTherapyUnitID");
                }
            }
        }

        private long _DonorID;
        public long DonorID
        {
            get { return _DonorID; }
            set
            {
                if (_DonorID != value)
                {
                    _DonorID = value;
                    OnPropertyChanged("DonorID");
                }
            }
        }

        private long _DonorUnitID;
        public long DonorUnitID
        {
            get { return _DonorUnitID; }
            set
            {
                if (_DonorUnitID != value)
                {
                    _DonorUnitID = value;
                    OnPropertyChanged("DonorUnitID");
                }
            }
        }

        private long _SourceOfNeedle;
        public long SourceOfNeedle
        {
            get { return _SourceOfNeedle; }
            set
            {
                if (_SourceOfNeedle != value)
                {
                    _SourceOfNeedle = value;
                    OnPropertyChanged("SourceOfNeedle");
                }
            }
        }

        private string _SourceNeedle;
        public string SourceNeedle
        {
            get { return _SourceNeedle; }
            set
            {
                if (_SourceNeedle != value)
                {
                    _SourceNeedle = value;
                    OnPropertyChanged("SourceNeedle");
                }
            }
        }

        private string _SourceSemen;
        public string SourceSemen
        {
            get { return _SourceSemen; }
            set
            {
                if (_SourceSemen != value)
                {
                    _SourceSemen = value;
                    OnPropertyChanged("SourceSemen");
                }
            }
        }

        private string _SourceOocyte;
        public string SourceOocyte
        {
            get { return _SourceOocyte; }
            set
            {
                if (_SourceOocyte != value)
                {
                    _SourceOocyte = value;
                    OnPropertyChanged("SourceOocyte");
                }
            }
        }

        private long _OoctyDonorUnitID;
        public long OoctyDonorUnitID
        {
            get { return _OoctyDonorUnitID; }
            set
            {
                if (_OoctyDonorUnitID != value)
                {
                    _OoctyDonorUnitID = value;
                    OnPropertyChanged("OoctyDonorUnitID");
                }
            }
        }

        private long _OoctyDonorID;
        public long OoctyDonorID
        {
            get { return _OoctyDonorID; }
            set
            {
                if (_OoctyDonorID != value)
                {
                    _OoctyDonorID = value;
                    OnPropertyChanged("OoctyDonorID");
                }
            }
        }

        private string _OoctyDonorMrNo;
        public string OoctyDonorMrNo
        {
            get { return _OoctyDonorMrNo; }
            set
            {
                if (_OoctyDonorMrNo != value)
                {
                    _OoctyDonorMrNo = value;
                    OnPropertyChanged("OoctyDonorMrNo");
                }
            }
        }

        private long _SourceOfOoctye;
        public long SourceOfOoctye
        {
            get { return _SourceOfOoctye; }
            set
            {
                if (_SourceOfOoctye != value)
                {
                    _SourceOfOoctye = value;
                    OnPropertyChanged("SourceOfOoctye");
                }
            }
        }

        private long _SemenDonorID;
        public long SemenDonorID
        {
            get { return _SemenDonorID; }
            set
            {
                if (_SemenDonorID != value)
                {
                    _SemenDonorID = value;
                    OnPropertyChanged("SemenDonorID");
                }
            }
        }

        private long _SemenDonorUnitID;
        public long SemenDonorUnitID
        {
            get { return _SemenDonorUnitID; }
            set
            {
                if (_SemenDonorUnitID != value)
                {
                    _SemenDonorUnitID = value;
                    OnPropertyChanged("SemenDonorUnitID");
                }
            }
        }
        
        private string _SemenDonorMrNo;
        public string SemenDonorMrNo
        {
            get { return _SemenDonorMrNo; }
            set
            {
                if (_SemenDonorMrNo != value)
                {
                    _SemenDonorMrNo = value;
                    OnPropertyChanged("SemenDonorMrNo");
                }
            }
        }

        private string _SemenSampleCode;
        public string SemenSampleCode
        {
            get { return _SemenSampleCode; }
            set
            {
                if (_SemenSampleCode != value)
                {
                    _SemenSampleCode = value;
                    OnPropertyChanged("SemenSampleCode");
                }
            }
        }

        private bool _IsDonorFromModuleDonor;
        public bool IsDonorFromModuleDonor
        {
            get { return _IsDonorFromModuleDonor; }
            set
            {
                if (_IsDonorFromModuleDonor != value)
                {
                    _IsDonorFromModuleDonor = value;
                    OnPropertyChanged("IsDonorFromModuleDonor");
                }
            }
        }

        private bool _IsSampleUsedInDay0;
        public bool IsSampleUsedInDay0
        {
            get { return _IsSampleUsedInDay0; }
            set
            {
                if (_IsSampleUsedInDay0 != value)
                {
                    _IsSampleUsedInDay0 = value;
                    OnPropertyChanged("IsSampleUsedInDay0");
                }
            }
        }

        private string _SemenSampleCodeSelf;
        public string SemenSampleCodeSelf
        {
            get { return _SemenSampleCodeSelf; }
            set
            {
                if (_SemenSampleCodeSelf != value)
                {
                    _SemenSampleCodeSelf = value;
                    OnPropertyChanged("SemenSampleCodeSelf");
                }
            }
        }

        private long _SourceOfSemen_new;
        public long SourceOfSemen_new
        {
            get { return _SourceOfSemen_new; }
            set
            {
                if (_SourceOfSemen_new != value)
                {
                    _SourceOfSemen_new = value;
                    OnPropertyChanged("SourceOfSemen");
                }
            }
        }

        private long _SourceOfNeedle_1;
        public long SourceOfNeedle_1
        {
            get { return _SourceOfNeedle_1; }
            set
            {
                if (_SourceOfNeedle_1 != value)
                {
                    _SourceOfNeedle_1 = value;
                    OnPropertyChanged("SourceOfNeedle");
                }
            }
        }

       private long _SourceOfOoctye_1;
       public long SourceOfOoctye_1
        {
            get { return _SourceOfOoctye_1; }
            set
            {
                if (_SourceOfOoctye_1 != value)
                {
                    _SourceOfOoctye_1 = value;
                    OnPropertyChanged("SourceOfOoctye");
                }
            }
        }

       private long _PreSelfVolume_1;
       public long PreSelfVolume_1
        {
            get { return _PreSelfVolume_1; }
            set
            {
                if (_PreSelfVolume_1 != value)
                {
                    _PreSelfVolume_1 = value;
                    OnPropertyChanged("PreSelfVolume");
                }
            }
        }
       private long _PreSelfConcentration_1;
       public long PreSelfConcentration_1
        {
            get { return _PreSelfConcentration_1; }
            set
            {
                if (_PreSelfConcentration_1 != value)
                {
                    _PreSelfConcentration_1 = value;
                    OnPropertyChanged("PreSelfConcentration");
                }
            }
        }

       private long _PreSelfMotality_1;
       public long PreSelfMotality_1
        {
            get { return _PreSelfMotality_1; }
            set
            {
                if (_PreSelfMotality_1 != value)
                {
                    _PreSelfMotality_1 = value;
                    OnPropertyChanged("PreSelfMotality");
                }
            }
        }

       private long _PreSelfWBC_1;
       public long PreSelfWBC_1
        {
            get { return _PreSelfWBC_1; }
            set
            {
                if (_PreSelfWBC_1 != value)
                {
                    _PreSelfWBC_1 = value;
                    OnPropertyChanged("PreSelfWBC");
                }
            }
        }

       private long _PreDonorVolume_1;
       public long PreDonorVolume_1
        {
            get { return _PreDonorVolume_1; }
            set
            {
                if (_PreDonorVolume_1 != value)
                {
                    _PreDonorVolume_1 = value;
                    OnPropertyChanged("PreDonorVolume");
                }
            }
        }

       private long _PreDonorConcentration_1;
       public long PreDonorConcentration_1
        {
            get { return _PreDonorConcentration_1; }
            set
            {
                if (_PreDonorConcentration_1 != value)
                {
                    _PreDonorConcentration_1 = value;
                    OnPropertyChanged("PreDonorConcentration");
                }
            }
        }

       private long _PreDonorMotality_1;
       public long PreDonorMotality_1
        {
            get { return _PreDonorMotality_1; }
            set
            {
                if (_PreDonorMotality_1 != value)
                {
                    _PreDonorMotality_1 = value;
                    OnPropertyChanged("PreDonorMotality");
                }
            }
        }

       private long _PreDonorWBC_1;
       public long PreDonorWBC_1
        {
            get { return _PreDonorWBC_1; }
            set
            {
                if (_PreDonorWBC_1 != value)
                {
                    _PreDonorWBC_1 = value;
                    OnPropertyChanged("PreDonorWBC");
                }
            }
        }

       private long _PostSelfVolume_1;
       public long PostSelfVolume_1
        {
            get { return _PostSelfVolume_1; }
            set
            {
                if (_PostSelfVolume_1 != value)
                {
                    _PostSelfVolume_1 = value;
                    OnPropertyChanged("PostSelfVolume");
                }
            }
        }

       private long _PostSelfConcentration_1;
       public long PostSelfConcentration_1
        {
            get { return _PostSelfConcentration_1; }
            set
            {
                if (_PostSelfConcentration_1 != value)
                {
                    _PostSelfConcentration_1 = value;
                    OnPropertyChanged("PostSelfConcentration");
                }
            }
        }

       private long _PostSelfMotality_1;
       public long PostSelfMotality_1
        {
            get { return _PostSelfMotality_1; }
            set
            {
                if (_PostSelfMotality_1 != value)
                {
                    _PostSelfMotality_1 = value;
                    OnPropertyChanged("PostSelfMotality");
                }
            }
        }
      
        private long _PostSelfWBC_1;
       public long PostSelfWBC_1
        {
            get { return _PostSelfWBC_1; }
            set
            {
                if (_PostSelfWBC_1 != value)
                {
                    _PostSelfWBC_1 = value;
                    OnPropertyChanged("PostSelfWBC");
                }
            }
        }

       private long _PostDonorVolume_1;
       public long PostDonorVolume_1
        {
            get { return _PostDonorVolume_1; }
            set
            {
                if (_PostDonorVolume_1 != value)
                {
                    _PostDonorVolume_1 = value;
                    OnPropertyChanged("PostDonorVolume");
                }
            }
        }

       private long _PostDonorConcentration_1;
       public long PostDonorConcentration_1
        {
            get { return _PostDonorConcentration_1; }
            set
            {
                if (_PostDonorConcentration_1 != value)
                {
                    _PostDonorConcentration_1 = value;
                    OnPropertyChanged("PostDonorConcentration");
                }
            }
        }

       private long _PostDonorMotality_1;
       public long PostDonorMotality_1
        {
            get { return _PostDonorMotality_1; }
            set
            {
                if (_PostDonorMotality_1 != value)
                {
                    _PostDonorMotality_1 = value;
                    OnPropertyChanged("PostDonorMotality");
                }
            }
        }
       private long _PostDonorWBC_1;
       public long PostDonorWBC_1
        {
            get { return _PostDonorWBC_1; }
            set
            {
                if (_PostDonorWBC_1 != value)
                {
                    _PostDonorWBC_1 = value;
                    OnPropertyChanged("PostDonorWBC");
                }
            }
        }

       private string _Company;
       public string Company
       {
           get { return _Company; }
           set
           {
               if (_Company != value)
               {
                   _Company = value;
                   OnPropertyChanged("Company");
               }
           }
       }
        //....................






        #region Previous parameter
        private long _MethodOfSpermPreparation;
        public long MethodOfSpermPreparation
        {
            get { return _MethodOfSpermPreparation; }
            set
            {
                if (_MethodOfSpermPreparation != value)
                {
                    _MethodOfSpermPreparation = value;
                    OnPropertyChanged("MethodOfSpermPreparation");
                }
            }
        }

        private string _MOSP;
        public string MOSP
        {
            get { return _MOSP; }
            set
            {
                if (_MOSP != value)
                {
                    _MOSP = value;
                    OnPropertyChanged("MOSP");
                }
            }
        }

        private string _SOS;
        public string SOS
        {
            get { return _SOS; }
            set
            {
                if (_SOS != value)
                {
                    _SOS = value;
                    OnPropertyChanged("SOS");
                }
            }
        }

        private long _SourceOfSemen;
        public long SourceOfSemen
        {
            get { return _SourceOfSemen; }
            set
            {
                if (_SourceOfSemen != value)
                {
                    _SourceOfSemen = value;
                    OnPropertyChanged("SourceOfSemen");
                }
            }
        }
        
        private string _PreSelfVolume;
        public string PreSelfVolume
        {
            get { return _PreSelfVolume; }
            set
            {
                if (_PreSelfVolume != value)
                {
                    _PreSelfVolume = value;
                    OnPropertyChanged("PreSelfVolume");
                }
            }
        }

        private string _PreSelfConcentration;
        public string PreSelfConcentration
        {
            get { return _PreSelfConcentration; }
            set
            {
                if (_PreSelfConcentration != value)
                {
                    _PreSelfConcentration = value;
                    OnPropertyChanged("PreSelfConcentration");
                }
            }
        }

        private string _PreSelfMotality;
        public string PreSelfMotality
        {
            get { return _PreSelfMotality; }
            set
            {
                if (_PreSelfMotality != value)
                {
                    _PreSelfMotality = value;
                    OnPropertyChanged("PreSelfMotality");
                }
            }
        }

        private string _PreSelfWBC;
        public string PreSelfWBC
        {
            get { return _PreSelfWBC; }
            set
            {
                if (_PreSelfWBC != value)
                {
                    _PreSelfWBC = value;
                    OnPropertyChanged("PreSelfWBC");
                }
            }
        }

        private string _PreDonorVolume;
        public string PreDonorVolume
        {
            get { return _PreDonorVolume; }
            set
            {
                if (_PreDonorVolume != value)
                {
                    _PreDonorVolume = value;
                    OnPropertyChanged("PreDonorVolume");
                }
            }
        }

        private string _PreDonorConcentration;
        public string PreDonorConcentration
        {
            get { return _PreDonorConcentration; }
            set
            {
                if (_PreDonorConcentration != value)
                {
                    _PreDonorConcentration = value;
                    OnPropertyChanged("PreDonorConcentration");
                }
            }
        }

        private string _PreDonorMotality;
        public string PreDonorMotality
        {
            get { return _PreDonorMotality; }
            set
            {
                if (_PreDonorMotality != value)
                {
                    _PreDonorMotality = value;
                    OnPropertyChanged("PreDonorMotality");
                }
            }
        }

        private string _PreDonorWBC;
        public string PreDonorWBC
        {
            get { return _PreDonorWBC; }
            set
            {
                if (_PreDonorWBC != value)
                {
                    _PreDonorWBC = value;
                    OnPropertyChanged("PreDonorWBC");
                }
            }
        }

        private string _PostSelfVolume;
        public string PostSelfVolume
        {
            get { return _PostSelfVolume; }
            set
            {
                if (_PostSelfVolume != value)
                {
                    _PostSelfVolume = value;
                    OnPropertyChanged("PostSelfVolume");
                }
            }
        }

        private string _PostSelfConcentration;
        public string PostSelfConcentration
        {
            get { return _PostSelfConcentration; }
            set
            {
                if (_PostSelfConcentration != value)
                {
                    _PostSelfConcentration = value;
                    OnPropertyChanged("PostSelfConcentration");
                }
            }
        }

        private string _PostSelfMotality;
        public string PostSelfMotality
        {
            get { return _PostSelfMotality; }
            set
            {
                if (_PostSelfMotality != value)
                {
                    _PostSelfMotality = value;
                    OnPropertyChanged("PostSelfMotality");
                }
            }
        }

        private string _PostSelfWBC;
        public string PostSelfWBC
        {
            get { return _PostSelfWBC; }
            set
            {
                if (_PostSelfWBC != value)
                {
                    _PostSelfWBC = value;
                    OnPropertyChanged("PostSelfWBC");
                }
            }
        }

        private string _PostDonorVolume;
        public string PostDonorVolume
        {
            get { return _PostDonorVolume; }
            set
            {
                if (_PostDonorVolume != value)
                {
                    _PostDonorVolume = value;
                    OnPropertyChanged("PostDonorVolume");
                }
            }
        }

        private string _PostDonorConcentration;
        public string PostDonorConcentration
        {
            get { return _PostDonorConcentration; }
            set
            {
                if (_PostDonorConcentration != value)
                {
                    _PostDonorConcentration = value;
                    OnPropertyChanged("PostDonorConcentration");
                }
            }
        }

        private string _PostDonorMotality;
        public string PostDonorMotality
        {
            get { return _PostDonorMotality; }
            set
            {
                if (_PostDonorMotality != value)
                {
                    _PostDonorMotality = value;
                    OnPropertyChanged("PostDonorMotality");
                }
            }
        }


        private string _PostDonorWBC;
        public string PostDonorWBC
        {
            get { return _PostDonorWBC; }
            set
            {
                if (_PostDonorWBC != value)
                {
                    _PostDonorWBC = value;
                    OnPropertyChanged("PostDonorWBC");
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
