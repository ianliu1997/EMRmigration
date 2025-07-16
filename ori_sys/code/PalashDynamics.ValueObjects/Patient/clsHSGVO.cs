using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects.IVFPlanTherapy;

namespace PalashDynamics.ValueObjects.Patient
{
   public class clsHSGVO : IValueObject, INotifyPropertyChanged
    {
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

        #region Properties

        private long _CoupleID;
        public long CoupleID
        {
            get { return _CoupleID; }
            set
            {
                if (_CoupleID != value)
                {
                    _CoupleID = value;
                    OnPropertyChanged("CoupleID");
                }
            }
        }

        private long _CoupleUnitID;
        public long CoupleUnitID
        {
            get { return _CoupleUnitID; }
            set
            {
                if (_CoupleUnitID != value)
                {
                    _CoupleUnitID = value;
                    OnPropertyChanged("CoupleUnitID");
                }
            }
        }
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

        private long _PatientTypeID;
        public long PatientTypeID
        {
            get { return _PatientTypeID; }
            set
            {
                if (_PatientTypeID != value)
                {
                    _PatientTypeID = value;
                    OnPropertyChanged("PatientTypeID");
                }
            }
        }

        private string _Uterus = "";
        public string Uterus
        {
            get { return _Uterus; }
            set
            {
                if (_Uterus != value)
                {
                    _Uterus = value;
                    OnPropertyChanged("Uterus");
                }
            }
        }

        private DateTime? _HSGDate = DateTime.Now;
        public DateTime? HSGDate
        {
            get { return _HSGDate; }
            set
            {
                if (_HSGDate != value)
                {
                    _HSGDate = value;
                    OnPropertyChanged("HSGDate");
                }
            }
        }

        private DateTime? _HSGTime = DateTime.Now;
        public DateTime? HSGTime
        {
            get { return _HSGTime; }
            set
            {
                if (_HSGTime != value)
                {
                    _HSGTime = value;
                    OnPropertyChanged("HSGTime");
                }
            }
        }


        public byte[] Image { get; set; }

        private bool _cavity;
        public bool cavity
        {
            get { return _cavity; }
            set
            {
                if (_cavity != value)
                {
                    _cavity = value;
                    OnPropertyChanged("cavity");
                }
            }
        }


        private bool _Patent_Tube;
        public bool Patent_Tube
        {
            get { return _Patent_Tube; }
            set
            {
                if (_Patent_Tube != value)
                {
                    _Patent_Tube = value;
                    OnPropertyChanged("Patent_Tube");
                }
            }
        }


        private bool _Blocked_tube;
        public bool Blocked_tube
        {
            get { return _Blocked_tube; }
            set
            {
                if (_Blocked_tube != value)
                {
                    _Blocked_tube = value;
                    OnPropertyChanged("Blocked_tube");
                }
            }
        }


        private bool _Cornul_blockage;
        public bool Cornul_blockage
        {
            get { return _Cornul_blockage; }
            set
            {
                if (_Cornul_blockage != value)
                {
                    _Cornul_blockage = value;
                    OnPropertyChanged("Cornul_blockage");
                }
            }
        }

        private bool _Isthmic_Blockage;
        public bool Isthmic_Blockage
        {
            get { return _Isthmic_Blockage; }
            set
            {
                if (_Isthmic_Blockage != value)
                {
                    _Isthmic_Blockage = value;
                    OnPropertyChanged("Isthmic_Blockage");
                }
            }
        }

        private bool _Ampullary_Blockage;
        public bool Ampullary_Blockage
        {
            get { return _Ampullary_Blockage; }
            set
            {
                if (_Ampullary_Blockage != value)
                {
                    _Ampullary_Blockage = value;
                    OnPropertyChanged("Ampullary_Blockage");
                }
            }
        }

        private bool _Fimbrial_Blockage;
        public bool Fimbrial_Blockage
        {
            get { return _Fimbrial_Blockage; }
            set
            {
                if (_Fimbrial_Blockage != value)
                {
                    _Fimbrial_Blockage = value;
                    OnPropertyChanged("Fimbrial_Blockage");
                }
            }
        }

        private bool _Hydrosalplnx;
        public bool Hydrosalplnx
        {
            get { return _Hydrosalplnx; }
            set
            {
                if (_Hydrosalplnx != value)
                {
                    _Hydrosalplnx = value;
                    OnPropertyChanged("Hydrosalplnx");
                }
            }
        }

        private string _Remark = "";
        public string Remark
        {
            get { return _Remark; }
            set
            {
                if (_Remark != value)
                {
                    _Remark = value;
                    OnPropertyChanged("Remark");
                }
            }
        }

        private string _Other_Patho = "";
        public string Other_Patho
        {
            get { return _Other_Patho; }
            set
            {
                if (_Other_Patho != value)
                {
                    _Other_Patho = value;
                    OnPropertyChanged("Other_Patho");
                }
            }
        }

        private bool _IsFreezed;
        public bool IsFreezed
        {
            get { return _IsFreezed; }
            set
            {
                if (_IsFreezed != value)
                {
                    _IsFreezed = value;
                    OnPropertyChanged("IsFreezed");
                }
            }
        }

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
        public clsCoupleVO CoupleDetail { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string AttachedFileName { get; set; }
        public byte[] AttachedFileContent { get; set; }
        public bool IsDeleted { get; set; }

        #endregion
    }
}
