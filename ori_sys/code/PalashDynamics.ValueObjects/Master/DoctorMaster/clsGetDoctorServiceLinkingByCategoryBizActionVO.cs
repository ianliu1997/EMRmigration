using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Administration;

namespace PalashDynamics.ValueObjects.Master.DoctorMaster
{
    public class clsGetDoctorServiceLinkingByCategoryBizActionVO : IBizActionValueObject, INotifyPropertyChanged
    {
        private clsServiceMasterVO _objServiceMasterDetails = null;
        public clsServiceMasterVO ServiceMasterDetails
        {
            get { return _objServiceMasterDetails; }
            set { _objServiceMasterDetails = value; }
        }

        private List<clsServiceMasterVO> _objServiceMasterDetailsList = null;
        public List<clsServiceMasterVO> ServiceMasterDetailsList
        {
            get { return _objServiceMasterDetailsList; }
            set { _objServiceMasterDetailsList = value; }
        }

        public int TotalRows { get; set; }
        public long StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool PagingEnabled { get; set; }

        private Int64 _SuccessStatus;
        public Int64 SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private long _CategoryID;
        public long CategoryID
        {
            get { return _CategoryID; }
            set { _CategoryID = value; }
        }

        private bool _IsModify;
        public bool IsModify
        {
            get { return _IsModify; }
            set { _IsModify = value; }
        }

        private bool _IsForClinic;
        public bool IsForClinic
        {
            get { return _IsForClinic; }
            set { _IsForClinic = value; }
        }



        private long _ServiceID;
        public long ServiceID
        {
            get { return _ServiceID; }
            set { _ServiceID = value; }
        }
        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set { _UnitID = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsGetDoctorServiceLinkingByCategoryBizAction";
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

    public class clsAddUpdateDoctorServiceLinkingByCategoryBizActionVO : IBizActionValueObject, INotifyPropertyChanged
    {
        private clsServiceMasterVO _objServiceMasterDetails = null;
        public clsServiceMasterVO ServiceMasterDetails
        {
            get { return _objServiceMasterDetails; }
            set { _objServiceMasterDetails = value; }
        }

        private List<clsServiceMasterVO> _objServiceMasterDetailsList = null;
        public List<clsServiceMasterVO> ServiceMasterDetailsList
        {
            get { return _objServiceMasterDetailsList; }
            set { _objServiceMasterDetailsList = value; }
        }

        private List<clsServiceMasterVO> _DeletedServiceList = null;
        public List<clsServiceMasterVO> DeletedServiceList
        {
            get { return _DeletedServiceList; }
            set { _DeletedServiceList = value; }
        }

        public int TotalRows { get; set; }
        public long StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool PagingEnabled { get; set; }

        private Int64 _SuccessStatus;
        public Int64 SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private long _CategoryID;
        public long CategoryID
        {
            get { return _CategoryID; }
            set { _CategoryID = value; }
        }

        private long _DoctorID;
        public long DoctorID
        {
            get { return _DoctorID; }
            set { _DoctorID = value; }
        }

        private bool _IsModify;
        public bool IsModify
        {
            get { return _IsModify; }
            set { _IsModify = value; }
        }


        private long _ServiceID;
        public long ServiceID
        {
            get { return _ServiceID; }
            set { _ServiceID = value; }
        }
        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set { _UnitID = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsAddUpdateDoctorServiceLinkingByCategoryBizAction";
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

    
    // Added BY CDS 
    public class clsGetDoctorServiceLinkingByCategoryAndServiceBizActionVO : IBizActionValueObject, INotifyPropertyChanged
    {
        private clsServiceMasterVO _objServiceMasterDetails = null;
        public clsServiceMasterVO ServiceMasterDetails
        {
            get { return _objServiceMasterDetails; }
            set { _objServiceMasterDetails = value; }
        }

        private List<clsServiceMasterVO> _objServiceMasterDetailsList = null;
        public List<clsServiceMasterVO> ServiceMasterDetailsList
        {
            get { return _objServiceMasterDetailsList; }
            set { _objServiceMasterDetailsList = value; }
        }

        public int TotalRows { get; set; }
        public long StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool PagingEnabled { get; set; }

        private Int64 _SuccessStatus;
        public Int64 SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private long _CategoryID;
        public long CategoryID
        {
            get { return _CategoryID; }
            set { _CategoryID = value; }
        }

        private bool _IsModify;
        public bool IsModify
        {
            get { return _IsModify; }
            set { _IsModify = value; }
        }

        private bool _IsForClinic;
        public bool IsForClinic
        {
            get { return _IsForClinic; }
            set { _IsForClinic = value; }
        }

        private long _DoctorID;
        public long DoctorID
        {
            get { return _DoctorID; }
            set { _DoctorID = value; }
        }

        private long _ServiceID;
        public long ServiceID
        {
            get { return _ServiceID; }
            set { _ServiceID = value; }
        }
        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set { _UnitID = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsGetDoctorServiceLinkingByCategoryAndServiceBizAction";                                                        
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
    // Added BY CDS 

}
