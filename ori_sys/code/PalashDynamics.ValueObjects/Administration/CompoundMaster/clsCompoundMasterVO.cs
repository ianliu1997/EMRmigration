using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Administration.CompoundMaster
{
    public class clsCompoundMasterVO : IValueObject, INotifyPropertyChanged    
    {
        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                if(value != _ID)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        private long _CompoundId;
        public long CompoundId
        {
            get { return _CompoundId; }
            set
            {
                if(value != _CompoundId)
                {
                    _CompoundId = value;
                    OnPropertyChanged("CompoundId");
                }
            }
        }

        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set
            {
                if(value != _UnitID)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }

        private string _Code;
        public string Code
        {
            get { return _Code; }
            set
            {
                if(value != _Code)
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
                if(value != _Description)
                {
                    _Description = value;
                    OnPropertyChanged("Description");
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
                if(value != _CreatedUnitID)
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
                if(value != _UpdatedUnitID)
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
                if(value != _AddedBy)
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
                if(value != _AddedOn)
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
                if(value != _AddedDateTime)
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
                if(value != _AddedWindowsLoginName)
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
                if(value != _UpdatedBy)
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
                if(value != _UpdatedOn)
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
                if(value != _UpdatedDateTime)
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
                if(value != _UpdatedWindowsLoginName)
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
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("PropertyName"));
        }

        #endregion
    }
    
    public class clsCompoundMasterDetailsVO : IValueObject, INotifyPropertyChanged
    {
        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                if(value != _ID)
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
                if(value != _RowID)
                {
                    _RowID = value;
                    OnPropertyChanged("_RowID");
                }
            }
        }

        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set
            {
                if(value != _UnitID)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }

        private long _ItemID;
        public long ItemID
        {
            get { return _ItemID; }
            set
            {
                if(_ItemID != value)
                {
                    _ItemID = value;
                    OnPropertyChanged("ItemID");
                }
            }
        }

        private long _CompoundId;
        public long CompoundId
        {
            get { return _CompoundId; }
            set
            {
                if(value != _CompoundId)
                {
                    _CompoundId = value;
                    OnPropertyChanged("CompoundId");
                }
            }
        }

        private long _CompoundUnitID;
        public long CompoundUnitID
        {
            get { return _CompoundUnitID; }
            set
            {
                if(_CompoundUnitID != value)
                {
                    _CompoundUnitID = value;
                    OnPropertyChanged("CompoundUnitID");
                }
            }
        }

        private string _ItemName;
        public string ItemName
        {
            get { return _ItemName; }
            set
            {
                if(value != _ItemName)
                {
                    _ItemName = value;
                    OnPropertyChanged("ItemName");
                }
            }
        }
        
        private string _ItemCode;
        public String ItemCode
        {
            get
            {
                return _ItemCode;
            }
            set
            {
                if (_ItemCode != value)
                {
                    _ItemCode = value;
                    OnPropertyChanged("ItemCode");
                }
            }
        }

        private long _ItemUnitID;
        public long ItemUnitID
        {
            get
            {
                return _ItemUnitID;
            }
            set
            {
                if (_ItemUnitID != value)
                {
                    _ItemUnitID = value;
                    OnPropertyChanged("ItemUnitID");
                }
            }
        }

        private long _CompoundDrugID;
        public long CompoundDrugID
        {
            get
            {
                return _CompoundDrugID;
            }
            set
            {
                if (_CompoundDrugID != value)
                {
                    _CompoundDrugID = value;
                    OnPropertyChanged("CompoundDrugID");
                }
            }
        }

        private long _CompoundDrugUnitID;
        public long CompoundDrugUnitID
        {
            get
            {
                return _CompoundDrugUnitID;
            }
            set
            {
                if (_CompoundDrugUnitID != value)
                {
                    _CompoundDrugUnitID = value;
                    OnPropertyChanged("CompoundDrugUnitID");
                }
            }
        }

        private double _AvailableStock;
        public double AvailableStock
        {
            get
            {
                return _AvailableStock;
            }
            set
            {
                if (_AvailableStock != value)
                {
                    _AvailableStock = value;
                    OnPropertyChanged("AvailableStock");
                }
            }
        }

        private double _StockQty;
        public double StockQty
        {
            get
            {
                return _StockQty;
            }
            set
            {
                if (_StockQty != value)
                {
                    _StockQty = value;
                    OnPropertyChanged("StockQty");
                }
            }
        }

        private string _CompoundDrug;
        public string CompoundDrug
        {
            get
            {
                return _CompoundDrug;
            }
            set
            {
                if (_CompoundDrug != value)
                {
                    _CompoundDrug = value;
                    OnPropertyChanged("CompoundDrug");
                }
            }
        }

        private string _MeasureUnit;
        public string MeasureUnit
        {
            get { return _MeasureUnit; }
            set
            {
                if (value != _MeasureUnit)
                {
                    _MeasureUnit = value;
                    OnPropertyChanged("MeasureUnit");
                }
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                if (value != _description)
                {
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        private string _IsNew;
        public string IsNew
        {
            get { return _IsNew; }
            set
            {
                if(value != _IsNew)
                {
                    _IsNew = value;
                    OnPropertyChanged("IsNew");
                }
            }
        }

        private double _Quantity;
        public double Quantity
        {
            get { return _Quantity; }
            set
            {
                if(value != _Quantity)
                {
                    _Quantity = value;
                    OnPropertyChanged("Quantity");
                }
            }
        }

        private long _CompoundMasterID;
        public long CompoundMasterID
        {
            get { return _CompoundMasterID; }
            set { _CompoundMasterID = value; }
        }

        private long _PrescriptionDetailID;
        public long PrescriptionDetailID
        {
            get { return _PrescriptionDetailID; }
            set
            {
                if (value != _PrescriptionDetailID)
                {
                    _PrescriptionDetailID = value;
                    OnPropertyChanged("PrescriptionDetailID");
                }
            }
        }

        private long _PrescriptionDetailUnitID;
        public long PrescriptionDetailUnitID
        {
            get { return _PrescriptionDetailUnitID; }
            set
            {
                if (value != _PrescriptionDetailUnitID)
                {
                    _PrescriptionDetailUnitID = value;
                    OnPropertyChanged("PrescriptionDetailUnitID");
                }
            }
        }

        #region CommonField

        private bool blnStatus;
        public bool Status
        {
            get { return blnStatus; }
            set
            {
                if(value != blnStatus)
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
                if(value != _CreatedUnitID)
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
                if(value != _UpdatedUnitID)
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
                if(value != _AddedBy)
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
                if(value != _AddedOn)
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
                if(value != _AddedDateTime)
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
                if(value != _AddedWindowsLoginName)
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
                if(value != _UpdatedBy)
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
                if(value != _UpdatedOn)
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
                if(value != _UpdatedDateTime)
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
                if(value != _UpdatedWindowsLoginName)
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
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("PropertyName"));
        }

        #endregion
    }
}
