using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Administration.PackageNew
{
    public class clsPackageServiceMasterVO : IValueObject, INotifyPropertyChanged
    {
        public string ToXml()
        {
            // throw new NotImplementedException();
            return this.ToString();
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("PropertyName"));
        }

        #endregion
    }

    public class clsPackageRelationsVO : IValueObject, INotifyPropertyChanged
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
                if (_UnitID != value)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }

        private long _RelationID;
        public long RelationID
        {
            get { return _RelationID; }
            set
            {
                if (value != _RelationID)
                {
                    _RelationID = value;
                    OnPropertyChanged("RelationID");
                }
            }
        }

        private String _Relation;
        public String Relation
        {
            get { return _Relation; }
            set
            {
                if (_Relation != value)
                {
                    _Relation = value;
                    OnPropertyChanged("Relation");
                }
            }
        }

        private long _PackageServiceID;
        public long PackageServiceID
        {
            get { return _PackageServiceID; }
            set
            {
                if (value != _PackageServiceID)
                {
                    _PackageServiceID = value;
                    OnPropertyChanged("PackageServiceID");
                }
            }
        }

        private long _PackageServiceUnitID;
        public long PackageServiceUnitID
        {
            get { return _PackageServiceUnitID; }
            set
            {
                if (_PackageServiceUnitID != value)
                {
                    _PackageServiceUnitID = value;
                    OnPropertyChanged("PackageServiceUnitID");
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
        
        private bool _IsSetAll;
        public bool IsSetAll
        {
            get { return _IsSetAll; }
            set
            {
                if (value != _IsSetAll)
                {
                    _IsSetAll = value;
                    OnPropertyChanged("IsSetAll");
                }
            }
        }
              
        public string ToXml()
        {
            // throw new NotImplementedException();
            return this.ToString();
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("PropertyName"));
        }

        #endregion
    }

    public class clsPackageServiceConditionsVO : IValueObject, INotifyPropertyChanged
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
                if (_UnitID != value)
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

        private String _ServiceName;
        public String ServiceName
        {
            get { return _ServiceName; }
            set
            {
                if (_ServiceName != value)
                {
                    _ServiceName = value;
                    OnPropertyChanged("ServiceName");
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

        private double _Quantity;
        public double Quantity
        {
            get { return _Quantity; }
            set
            {
                if (_Quantity != value)
                {

                    _Quantity = value;
                    OnPropertyChanged("Quantity");


                }
            }
        }

        private double _Discount;
        public double Discount
        {
            get
            {
                return _Discount;
            }

            set
            {
                if (value != _Discount)
                {
                    if (value < 0)
                        value = 0;

                    if (value > 100)
                        value = 0;
                    _Discount = value;
                    OnPropertyChanged("Discount");
                }
            }

        }

        private long _ConditionTypeID;
        public long ConditionTypeID
        {
            get { return _ConditionTypeID; }
            set
            {
                if (value != _ConditionTypeID)
                {
                    _ConditionTypeID = value;
                    OnPropertyChanged("ConditionTypeID");
                }
            }
        }

        private String _ConditionType;
        public String ConditionType
        {
            get { return _ConditionType; }
            set
            {
                if (_ConditionType != value)
                {
                    _ConditionType = value;
                    OnPropertyChanged("ConditionType");
                }
            }
        }

        private long _SpecilizationID;
        public long SpecilizationID
        {
            get { return _SpecilizationID; }
            set
            {
                if (value != _SpecilizationID)
                {
                    _SpecilizationID = value;
                    OnPropertyChanged("SpecilizationID");
                }
            }
        }

        private bool _IsSpecilizationGroup;
        public bool IsSpecilizationGroup
        {
            get { return _IsSpecilizationGroup; }
            set
            {
                if (value != _IsSpecilizationGroup)
                {
                    _IsSpecilizationGroup = value;
                    OnPropertyChanged("IsSpecilizationGroup");
                }

            }
        }

        List<MasterListItem> _ServiceList = new List<MasterListItem>();
        public List<MasterListItem> ServiceList
        {
            get
            {
                return _ServiceList;
            }
            set
            {
                if (value != _ServiceList)
                {
                    _ServiceList = value;
                }
            }

        }


        MasterListItem _SelectedService = new MasterListItem { ID = 0, Description = "-- Select --" };
        public MasterListItem SelectedService
        {
            get
            {
                return _SelectedService;
            }
            set
            {
                if (value != _SelectedService)
                {
                    _SelectedService = value;
                    OnPropertyChanged("SelectedService");
                }
            }


        }




        List<MasterListItem> _ConditionList = new List<MasterListItem>();
        public List<MasterListItem> ConditionList
        {
            get
            {
                return _ConditionList;
            }
            set
            {
                if (value != _ConditionList)
                {
                    _ConditionList = value;
                }
            }

        }


        MasterListItem _SelectedCondition = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedCondition
        {
            get
            {
                return _SelectedCondition;
            }
            set
            {
                if (value != _SelectedCondition)
                {
                    _SelectedCondition = value;
                    OnPropertyChanged("SelectedCondition");
                }
            }


        }

        private long _PackageID;
        public long PackageID
        {
            get { return _PackageID; }
            set
            {
                if (value != _PackageID)
                {
                    _PackageID = value;
                    OnPropertyChanged("PackageID");
                }
            }
        }

        private long _PackageUnitID;
        public long PackageUnitID
        {
            get { return _PackageUnitID; }
            set
            {
                if (_PackageUnitID != value)
                {
                    _PackageUnitID = value;
                    OnPropertyChanged("PackageUnitID");
                }
            }
        }

        private long _PackageServiceID;
        public long PackageServiceID
        {
            get { return _PackageServiceID; }
            set
            {
                if (value != _PackageServiceID)
                {
                    _PackageServiceID = value;
                    OnPropertyChanged("PackageServiceID");
                }
            }
        }


        //private long _PackageServiceDetailID;
        //public long PackageServiceDetailID
        //{
        //    get { return _PackageServiceDetailID; }
        //    set
        //    {
        //        if (value != _PackageServiceDetailID)
        //        {
        //            _PackageServiceDetailID = value;
        //            OnPropertyChanged("PackageServiceDetailID");
        //        }
        //    }
        //}

        //private long _PackageServiceDetailUnitID;
        //public long PackageServiceDetailUnitID
        //{
        //    get { return _PackageServiceDetailUnitID; }
        //    set
        //    {
        //        if (_PackageServiceDetailUnitID != value)
        //        {
        //            _PackageServiceDetailUnitID = value;
        //            OnPropertyChanged("PackageServiceDetailUnitID");
        //        }
        //    }
        //}

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

        private bool _IsSet;
        public bool IsSet
        {
            get { return _IsSet; }
            set
            {
                if (value != _IsSet)
                {
                    _IsSet = value;
                    OnPropertyChanged("IsSet");
                }

            }
        }

        public string ToXml()
        {
            // throw new NotImplementedException();
            return this.ToString();
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("PropertyName"));
        }

        #endregion
    }

    public class clsPackageServiceRelationsVO : IValueObject, INotifyPropertyChanged
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
                if (_UnitID != value)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }

        private long _PackageID;
        public long PackageID
        {
            get { return _PackageID; }
            set
            {
                if (value != _PackageID)
                {
                    _PackageID = value;
                    OnPropertyChanged("PackageID");
                }
            }
        }

        private long _PackageUnitID;
        public long PackageUnitID
        {
            get { return _PackageUnitID; }
            set
            {
                if (_PackageUnitID != value)
                {
                    _PackageUnitID = value;
                    OnPropertyChanged("PackageUnitID");
                }
            }
        }

        private long _RelationID;
        public long RelationID
        {
            get { return _RelationID; }
            set
            {
                if (value != _RelationID)
                {
                    _RelationID = value;
                    OnPropertyChanged("RelationID");
                }
            }
        }

        private String _Relation;
        public String Relation
        {
            get { return _Relation; }
            set
            {
                if (_Relation != value)
                {
                    _Relation = value;
                    OnPropertyChanged("Relation");
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

        private String _Service;
        public String Service
        {
            get { return _Service; }
            set
            {
                if (_Service != value)
                {
                    _Service = value;
                    OnPropertyChanged("Service");
                }
            }
        }

        private long _SpecilizationID;
        public long SpecilizationID
        {
            get { return _SpecilizationID; }
            set
            {
                if (value != _SpecilizationID)
                {
                    _SpecilizationID = value;
                    OnPropertyChanged("SpecilizationID");
                }
            }
        }

        private bool _IsSpecilizationGroup;
        public bool IsSpecilizationGroup
        {
            get { return _IsSpecilizationGroup; }
            set
            {
                if (value != _IsSpecilizationGroup)
                {
                    _IsSpecilizationGroup = value;
                    OnPropertyChanged("IsSpecilizationGroup");
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



        private bool _IsSetAllRelations;
        public bool IsSetAllRelations
        {
            get { return _IsSetAllRelations; }
            set
            {
                if (value != _IsSetAllRelations)
                {
                    _IsSetAllRelations = value;
                    OnPropertyChanged("IsSetAllRelations");
                }

            }
        }

        //Package New Changes for Process Added on 25042018
        private long _ProcessID;
        public long ProcessID
        {
            get { return _ProcessID; }
            set
            {
                if (_ProcessID != value)
                {
                    _ProcessID = value;
                    OnPropertyChanged("ProcessID");
                }
            }
        }

        public string ToXml()
        {
            // throw new NotImplementedException();
            return this.ToString();
        }

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
