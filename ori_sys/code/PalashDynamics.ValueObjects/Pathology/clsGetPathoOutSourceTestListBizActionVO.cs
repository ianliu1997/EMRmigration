using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Pathology
{
    public class clsGetPathoOutSourceTestListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Pathology.clsGetPathoOutSourceTestListBizAction";
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

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        #region Paging
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string sortExpression { get; set; }
        #endregion

        private List<clsPathoTestOutSourceDetailsVO> _PathoOutSourceTestList = new List<clsPathoTestOutSourceDetailsVO>();
        public List<clsPathoTestOutSourceDetailsVO> PathoOutSourceTestList
        {
            get { return _PathoOutSourceTestList; }
            set { _PathoOutSourceTestList = value; }
        }

        private List<clsPathoTestOutSourceDetailsVO> _UnAssignedAgnecyTestList = new List<clsPathoTestOutSourceDetailsVO>();
        public List<clsPathoTestOutSourceDetailsVO> UnAssignedAgnecyTestList
        {
            get { return _UnAssignedAgnecyTestList; }
            set { _UnAssignedAgnecyTestList = value; }
        }

        public clsPathoTestOutSourceDetailsVO _PathoOutSourceTestDetails = new clsPathoTestOutSourceDetailsVO();
        public clsPathoTestOutSourceDetailsVO PathoOutSourceTestDetails
        {
            get { return _PathoOutSourceTestDetails; }
            set { _PathoOutSourceTestDetails = value; }
        }
    }

    public class clsChangePathoTestAgencyBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Pathology.clsChangePathoTestAgencyBizAction";
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

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        #region Paging
        #endregion

        private List<clsPathoTestOutSourceDetailsVO> _PathoOutSourceTestList = new List<clsPathoTestOutSourceDetailsVO>();
        public List<clsPathoTestOutSourceDetailsVO> PathoOutSourceTestList
        {
            get { return _PathoOutSourceTestList; }
            set { _PathoOutSourceTestList = value; }
        }

        private List<clsPathoTestOutSourceDetailsVO> _AssigneAgnecyTestList = new List<clsPathoTestOutSourceDetailsVO>();
        public List<clsPathoTestOutSourceDetailsVO> AssignedAgnecyTestList
        {
            get { return _AssigneAgnecyTestList; }
            set { _AssigneAgnecyTestList = value; }
        }

        public clsPathoTestOutSourceDetailsVO _PathoOutSourceTestDetails = new clsPathoTestOutSourceDetailsVO();
        public clsPathoTestOutSourceDetailsVO PathoOutSourceTestDetails
        {
            get { return _PathoOutSourceTestDetails; }
            set { _PathoOutSourceTestDetails = value; }
        }

        public bool IsOutsource { get; set; }
        public String OutSourceID { get; set; }

           

    }

    //Added By Anumani
    public class clsGetAssignedAgencyBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Pathology.clsGetAssignedAgencyBizAction";
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

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }


        private long _ServiceId;
        public long ServiceID
        {
            get { return _ServiceId; }
            set { _ServiceId = value; }
        }

        private long _AgencyId;
        public long AgencyID
        {
            get { return _AgencyId; }
            set { _AgencyId = value; }
        }

        private long _DefaultAgencyId;
        public long DefaultAgencyID
        {
            get { return _DefaultAgencyId; }
            set { _DefaultAgencyId = value; }
        }

        private string _DefaultAgencyName;
        public string DefaultAgencyName
        {
            get { return _DefaultAgencyName; }
            set { _DefaultAgencyName = value; }
        }


      private List<clsGetAssignedAgencyBizActionVO> _AgencyList = new List<clsGetAssignedAgencyBizActionVO>();
      public List<clsGetAssignedAgencyBizActionVO> AgencyList
      {
          get { return _AgencyList; }
          set { _AgencyList = value; }
      }
    }
}
