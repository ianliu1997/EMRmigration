using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
    public class cls_IVFDashboard_GetAgencyListOfSurrogateBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.cls_IVFDashboard_GetAgencyListOfSurrogateBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion
        private cls_AgencyInfoVO _AgencyDetails = new cls_AgencyInfoVO();
        public cls_AgencyInfoVO AgencyDetails
        {
            get
            {
                return _AgencyDetails;
            }
            set
            {
                _AgencyDetails = value;
            }
        }
        private List<cls_AgencyInfoVO> _AgencyList = new List<cls_AgencyInfoVO>();
        public List<cls_AgencyInfoVO> AgencyList
        {
            get
            {
                return _AgencyList;
            }
            set
            {
                _AgencyList = value;
            }
        }
    }
    public class cls_AgencyInfoVO
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

        private string _Agencyname;
        public string Agencyname
        {
            get { return _Agencyname; }
            set
            {
                if (_Agencyname != value)
                {
                    _Agencyname = value;
                    OnPropertyChanged("Agencyname");
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
        private string _AgencyContactNo;
        public string AgencyContactNo
        {
            get { return _AgencyContactNo; }
            set
            {
                if (_AgencyContactNo != value)
                {
                    _AgencyContactNo = value;
                    OnPropertyChanged("AgencyContactNo");
                }
            }
        }
        private string _AgencyEmail;
        public string AgencyEmail
        {
            get { return _AgencyEmail; }
            set
            {
                if (_AgencyEmail != value)
                {
                    _AgencyEmail = value;
                    OnPropertyChanged("AgencyEmail");
                }
            }
        }
        private string _AgencyAddress;
        public string AgencyAddress
        {
            get { return _AgencyAddress; }
            set
            {
                if (_AgencyAddress != value)
                {
                    _AgencyAddress = value;
                    OnPropertyChanged("AgencyAddress");
                }
            }
        }
        private string _AgencyInfo;
        public string AgencyInfo
        {
            get { return _AgencyInfo; }
            set
            {
                if (_AgencyInfo != value)
                {
                    _AgencyInfo = value;
                    OnPropertyChanged("AgencyInfo");
                }
            }
        }
    }
}
