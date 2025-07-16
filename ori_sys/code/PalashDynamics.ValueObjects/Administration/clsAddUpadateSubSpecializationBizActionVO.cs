using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PalashDynamics.ValueObjects.Administration
{
    public class clsAddUpadateSubSpecializationBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsAddUpadateSubSpecializationBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsSubSpecializationVO> objItemMaster = new List<clsSubSpecializationVO>();
        public List<clsSubSpecializationVO> ItemMatserDetails
        {
            get
            {
                return objItemMaster;
            }
            set
            {
                objItemMaster = value;

            }
        }


        /// <summary>
        ///  Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        private int _SuccessStatus;
        public int SuccessStatus
        {
            get
            {
                return _SuccessStatus;
            }
            set
            {
                _SuccessStatus = value;
            }
        }



    }

    public class clsSubSpecializationVO
    {
        private List<MasterListItem> _AmtOrPctList = new List<MasterListItem>();
        public List<MasterListItem> AmtOrPctList
        {
            get
            {
                return _AmtOrPctList;
            }
            set
            {
                _AmtOrPctList = value;
            }
        }

        private MasterListItem _SelectedAmtOrPct = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedAmtOrPct
        {
            get
            {
                return _SelectedAmtOrPct;
            }
            set
            {
                _SelectedAmtOrPct = value;
            }
        }


        public long SubSpecializationId { get; set; }
        public string SubSpecializationName { get; set; }
        public string Code { get; set; }
        public long SpecializationId { get; set; }
        public string SpecializationName { get; set; }
        public long ClinicId { get; set; }
        public Boolean Status { get; set; }
        public long? CreatedUnitID { get; set; }
        public long? UpdatedUnitID { get; set; }
        public long? AddedBy { get; set; }
        public string AddedOn { get; set; }
        public DateTime? AddedDateTime { get; set; }
        public long? UpdatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public string AddedWindowsLoginName { get; set; }
        public string UpdateWindowsLoginName { get; set; }

        //public double SharePercentage { get; set; }
        private double _SharePercentage;
        public double SharePercentage
        {
            get { return _SharePercentage; }
            set
            {
                if (_SharePercentage != value)
                {
                    if (_IsPercentageRate)
                    {
                        if (value > 100)
                        {
                            _SharePercentage = 0;
                            throw new ValidationException("Please enter Rate upto 100");
                        }
                        else
                            _SharePercentage = value;
                    }
                    else
                        _SharePercentage = value;
                    OnPropertyChanged("SharePercentage");
                }
            }
        }

        //public double ShareAmount { get; set; }
        private double _ShareAmount;
        public double ShareAmount
        {
            get { return _ShareAmount; }
            set
            {
                if (_ShareAmount != value)
                {
                    _ShareAmount = value;
                    OnPropertyChanged("ShareAmount");
                }
            }
        }

        //public bool IsPercentageRate { get; set; }
        private bool _IsPercentageRate;
        public bool IsPercentageRate
        {
            get { return _IsPercentageRate; }
            set
            {
                if (_IsPercentageRate != value)
                {
                    //if (value)
                    //{
                    //    if (_SharePercentage > 100)
                    //    {
                    //        _IsPercentageRate = value;
                    //        _SharePercentage = 0;
                    //        SharePercentage = 0;
                    //        throw new ValidationException("Please enter Rate upto 100");
                    //    }
                    //    else
                    //        _IsPercentageRate = value;
                    //}
                    //else
                        _IsPercentageRate = value;

                    OnPropertyChanged("IsPercentageRate");
                    //OnPropertyChanged("SharePercentage");
                }
            }
        }

        //public bool IsAmountRate { get; set; }
        private bool _IsAmountRate;
        public bool IsAmountRate
        {
            get { return _IsAmountRate; }
            set
            {
                if (_IsAmountRate != value)
                {
                    _IsAmountRate = value;
                    OnPropertyChanged("IsAmountRate");
                }
            }
        }

        //public bool IsAddition { get; set; }
        private bool _IsAddition;
        public bool IsAddition
        {
            get { return _IsAddition; }
            set
            {
                if (_IsAddition != value)
                {
                    _IsAddition = value;
                    OnPropertyChanged("IsAddition");
                }
            }
        }

        //public bool IsSubtaction { get; set; }
        private bool _IsSubtaction;
        public bool IsSubtaction
        {
            get { return _IsSubtaction; }
            set
            {
                if (_IsSubtaction != value)
                {
                    _IsSubtaction = value;
                    OnPropertyChanged("IsSubtaction");
                }
            }
        }

        private string _stOperationType;
        public string stOperationType
        {
            get { return _stOperationType; }
            set
            {
                if (value != _stOperationType)
                {
                    _stOperationType = value;
                }
            }
        }

        private bool _SelectSubSpecialization;
        public bool SelectSubSpecialization
        {
            get { return _SelectSubSpecialization; }
            set
            {
                if (value != _SelectSubSpecialization)
                {
                    _SelectSubSpecialization = value;
                    OnPropertyChanged("SelectSubSpecialization");
                }
            }
        }


        private int _intOperationType;
        public int intOperationType
        {
            get { return _intOperationType; }
            set
            {
                if (value != _intOperationType)
                {
                    _intOperationType = value;
                }
            }
        }


        private long _BulkRateChangeID;
        public long BulkRateChangeID
        {
            get { return _BulkRateChangeID; }
            set
            {
                if (value != _BulkRateChangeID)
                {
                    _BulkRateChangeID = value;
                }
            }
        }

        private long _BulkRateChangeUnitID;
        public long BulkRateChangeUnitID
        {
            get { return _BulkRateChangeUnitID; }
            set
            {
                if (value != _BulkRateChangeUnitID)
                {
                    _BulkRateChangeUnitID = value;
                }
            }
        }

        private Boolean _IsSetRateForAll;
        public Boolean IsSetRateForAll
        {
            get { return _IsSetRateForAll; }
            set
            {
                if (value != _IsSetRateForAll)
                {
                    _IsSetRateForAll = value;
                }
            }
        }



        private string strSpecSubSpecilization;
        public string SpecSubSpecilization
        {
            get { return strSpecSubSpecilization = SpecializationName + "-" + SubSpecializationName; }
            set { strSpecSubSpecilization = value; }
        }

        public Boolean PrimaryKeyViolationError { get; set; }
        public Boolean GeneralError { get; set; }
        public bool IsReadOnly { get; set; }

        List<MasterListItem> _OpTpe = new List<MasterListItem>();
        public List<MasterListItem> OpTpe
        {
            get
            {
                return _OpTpe;
            }

            set
            {
                if (value != _OpTpe)
                {
                    _OpTpe = value;

                }
            }
        }

        MasterListItem _SelectedOpType = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedOpType
        {
            get
            {
                return _SelectedOpType;

            }
            set
            {
                if (value != _SelectedOpType)
                {
                    _SelectedOpType = value;
                    OnPropertyChanged("SelectedOpType");
                }
            }
        }



        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion

    }
}
