using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.ValueObjects.Administration.DischargeTemplateMaster
{
    public class clsDischargeTemplateMasterVO : INotifyPropertyChanged, IValueObject
    {
        private List<clsDischargeTemplateDetailsVO> _DischargeTemplateDetailsList;
        public List<clsDischargeTemplateDetailsVO> DischargeTemplateDetailsList
        {
            get { return _DischargeTemplateDetailsList; }
            set { _DischargeTemplateDetailsList = value; }
        }

        private clsDischargeTemplateDetailsVO _DischargeTemplateDetails;
        public clsDischargeTemplateDetailsVO DischargeTemplateDetails
        {
            get { return _DischargeTemplateDetails; }
            set { _DischargeTemplateDetails = value; }
        }

        public long TotalRows { get; set; }

        private long _RowID;
        public long RowID
        {
            get { return _RowID; }
            set
            {
                if (value != _RowID)
                {
                    _RowID = value;
                    OnPropertyChanged("RowID");
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

        private string _Code;
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

        private bool _IsTextTemplate;
        public bool IsTextTemplate
        {
            get { return _IsTextTemplate; }
            set
            {
                if (value != _IsTextTemplate)
                {
                    _IsTextTemplate = value;
                    OnPropertyChanged("IsTextTemplate");
                }
            }
        }

        private long lngUnitId;
        public long UnitID
        {
            get { return lngUnitId; }
            set
            {
                if (value != lngUnitId)
                {
                    lngUnitId = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }

        private string strUnitName;
        public string UnitName
        {
            get { return strUnitName; }
            set
            {
                if (value != strUnitName)
                {
                    strUnitName = value;
                    OnPropertyChanged("UnitName");
                }
            }
        }

        private long _DischargeTemplateID;
        public long DischargeTemplateID
        {
            get { return _DischargeTemplateID; }
            set
            {
                if (value != _DischargeTemplateID)
                {
                    _DischargeTemplateID = value;
                    OnPropertyChanged("DischargeTemplateID");
                }
            }
        }

        #region CommonField

        public bool Status { get; set; }

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
                if (value != _UpdatedWindowsLoginName)
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

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("PropertyName"));
        }

        #endregion
    }

    public class clsDischargeTemplateDetailsVO : INotifyPropertyChanged, IValueObject
    {
        private long _RowID;
        public long RowID
        {
            get { return _RowID; }
            set
            {
                if (value != _RowID)
                {
                    _RowID = value;
                    OnPropertyChanged("RowID");
                }
            }
        }

        private bool _IsTextBoxEnable;
        public bool IsTextBoxEnable
        {
            get { return _IsTextBoxEnable; }
            set
            {
                if (value != _IsTextBoxEnable)
                {
                    _IsTextBoxEnable = value;
                    OnPropertyChanged("IsTextBoxEnable");
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

        private long _BindingControl;
        public long BindingControl
        {
            get { return _BindingControl; }
            set
            {
                if (value != _BindingControl)
                {
                    _BindingControl = value;
                    OnPropertyChanged("BindingControl");

                    if (_BindingControl == (int)PalashDynamics.ValueObjects.BindingControl.CheckBox)
                        BindingControlName = "CheckBox";
                    else if (_BindingControl == (int)PalashDynamics.ValueObjects.BindingControl.DatePicker)
                        BindingControlName = "DatePicker";
                    else if (_BindingControl == (int)PalashDynamics.ValueObjects.BindingControl.Option)
                        BindingControlName = "Option";
                    else if (_BindingControl == (int)PalashDynamics.ValueObjects.BindingControl.Text)
                        BindingControlName = "Text";
                    else if (_BindingControl == (int)PalashDynamics.ValueObjects.BindingControl.Time)
                        BindingControlName = "Time";
                    else
                        BindingControlName = "";

                }
            }
        }

        private string _BindingControlName;
        public string BindingControlName
        {
            get { return _BindingControlName; }
            set
            {
                if (value != _BindingControlName)
                {
                    _BindingControlName = value;
                    OnPropertyChanged("BindingControlName");
                }
            }
        }

        private long _DisChargeTemplateID;
        public long DisChargeTemplateID
        {
            get { return _DisChargeTemplateID; }
            set
            {
                if (value != _DisChargeTemplateID)
                {
                    _DisChargeTemplateID = value;
                    OnPropertyChanged("DisChargeTemplateID");
                }
            }
        }

        private long _DisChargeTemplateUnitId;
        public long DisChargeTemplateUnitID
        {
            get { return _DisChargeTemplateUnitId; }
            set
            {
                if (value != _DisChargeTemplateUnitId)
                {
                    _DisChargeTemplateUnitId = value;
                    OnPropertyChanged("DisChargeTemplateUnitID");
                }
            }
        }

        private long lngUnitId;
        public long UnitID
        {
            get { return lngUnitId; }
            set
            {
                if (value != lngUnitId)
                {
                    lngUnitId = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }

        private string strUnitName;
        public string UnitName
        {
            get { return strUnitName; }
            set
            {
                if (value != strUnitName)
                {
                    strUnitName = value;
                    OnPropertyChanged("UnitName");
                }
            }
        }

        private string _FieldName;
        public string FieldName
        {
            get { return _FieldName; }
            set
            {
                if (value != _FieldName)
                {
                    _FieldName = value;
                    OnPropertyChanged("FieldName");
                }
            }
        }

        private string _ParameterName;
        public string ParameterName
        {
            get { return _ParameterName; }
            set
            {
                if (value != _ParameterName)
                {
                    _ParameterName = value;
                    OnPropertyChanged("ParameterName");
                }
            }
        }

        private string _ApplicableFont;
        public string ApplicableFont
        {
            get { return _ApplicableFont; }
            set
            {
                if (value != _ApplicableFont)
                {
                    _ApplicableFont = value;
                    OnPropertyChanged("ApplicableFont");
                }
            }
        }

        private long _ParameterID;
        public long ParameterID
        {
            get { return _ParameterID; }
            set
            {
                if (value != _ParameterID)
                {
                    _ParameterID = value;
                    OnPropertyChanged("ParameterID");
                }
            }
        }

        private string _TextData;
        public string TextData
        {
            get { return _TextData; }
            set
            {
                if (value != _TextData)
                {
                    _TextData = value;
                    OnPropertyChanged("TextData");
                }
            }
        }

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
}
