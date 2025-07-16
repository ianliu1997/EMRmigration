using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.IPD
{
    public class clsChangeReferEntityVO : IValueObject, INotifyPropertyChanged
    {


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


        private long _RowID;
        public long RowID
        {
            get { return _RowID; }
            set
            {
                if (_RowID != value)
                {
                    _RowID = value;
                    OnPropertyChanged("RowID");
                }
            }
        }

        private bool _IsAttachWithOPD_IPD;
        public bool IsAttachWithOPD_IPD
        {
            get { return _IsAttachWithOPD_IPD; }
            set
            {
                if (_IsAttachWithOPD_IPD != value)
                {
                    _IsAttachWithOPD_IPD = value;
                    OnPropertyChanged("IsAttachWithOPD_IPD");
                }
            }
        }


        private long _AssignID;
        public long AssignID
        {
            get { return _AssignID; }
            set
            {
                if (_AssignID != value)
                {
                    _AssignID = value;
                    OnPropertyChanged("AssignID");
                }
            }
        }


        private long _AssignUnitID;
        public long AssignUnitID
        {
            get { return _AssignUnitID; }
            set
            {
                if (_AssignUnitID != value)
                {
                    _AssignUnitID = value;
                    OnPropertyChanged("AssignUnitID");
                }
            }
        }

        private long _ReferingEntityTypeId;
        public long ReferingEntityTypeId
        {
            get { return _ReferingEntityTypeId; }
            set
            {
                if (_ReferingEntityTypeId != value)
                {
                    _ReferingEntityTypeId = value;
                    OnPropertyChanged("ReferingEntityTypeId");
                }
            }
        }

        private long _ReferingEntityId;
        public long ReferingEntityId
        {
            get { return _ReferingEntityId; }
            set
            {
                if (_ReferingEntityId != value)
                {
                    _ReferingEntityId = value;
                    OnPropertyChanged("ReferingEntityId");
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
                    OnPropertyChanged("Description");
                }

            }
        }

        private string _RefEntityDescription;
        public string RefEntityDescription
        {
            get { return _RefEntityDescription; }
            set
            {
                if (_RefEntityDescription != value)
                {
                    _RefEntityDescription = value;
                    OnPropertyChanged("RefEntityDescription");
                }

            }
        }

        private long _VisitAdmID;
        public long VisitAdmID
        {
            get { return _VisitAdmID; }
            set
            {
                if (_VisitAdmID != value)
                {
                    _VisitAdmID = value;
                    OnPropertyChanged("VisitAdmID");
                }
            }
        }


        private long _VisitAdmUnitID;
        public long VisitAdmUnitID
        {
            get { return _VisitAdmUnitID; }
            set
            {
                if (_VisitAdmUnitID != value)
                {
                    _VisitAdmUnitID = value;
                    OnPropertyChanged("VisitAdmUnitID");
                }
            }
        }



        private long _OPD_IPD;
        public long OPD_IPD
        {
            get { return _OPD_IPD; }
            set
            {
                if (_OPD_IPD != value)
                {
                    _OPD_IPD = value;
                    OnPropertyChanged("OPD_IPD");
                }
            }
        }
        private long _OPD_IPD_ID;
        public long OPD_IPD_ID
        {
            get { return _OPD_IPD_ID; }
            set
            {
                if (_OPD_IPD_ID != value)
                {
                    _OPD_IPD_ID = value;
                    OnPropertyChanged("OPD_IPD_ID");
                }
            }
        }

        private long _OPD_IPD_UnitID;
        public long OPD_IPD_UnitID
        {
            get { return _OPD_IPD_UnitID; }
            set
            {
                if (_OPD_IPD_UnitID != value)
                {
                    _OPD_IPD_UnitID = value;
                    OnPropertyChanged("OPD_IPD_UnitID");
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


        #region Comman Field
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
