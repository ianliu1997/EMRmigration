using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace PalashDynamics.ValueObjects.Administration.IPD
{

    public class clsIPDAddAdmissionTypeServiceListBizActionVO : IBizActionValueObject
    {
        private clsIPDAdmissionTypeServiceLinkVO _DoctorServiceDetails;
        public clsIPDAdmissionTypeServiceLinkVO DoctorServiceDetails
        {
            get { return _DoctorServiceDetails; }
            set { _DoctorServiceDetails = value; }
        }


        private ObservableCollection<clsServiceMasterVO> _DoctorServiceList;
        public ObservableCollection<clsServiceMasterVO> DoctorServiceList
        {
            get { return _DoctorServiceList; }
            set { _DoctorServiceList = value; }
        }


        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }
        public bool Modify { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.IPD.clsIPDAddAdmissionTypeServiceListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsIPDAddUpdateAdmissionTypeServiceListBizActionVO : IBizActionValueObject
    {
        private clsIPDAdmissionTypeServiceLinkVO _DoctorServiceDetails;
        public clsIPDAdmissionTypeServiceLinkVO DoctorServiceDetails
        {
            get { return _DoctorServiceDetails; }
            set { _DoctorServiceDetails = value; }
        }


        private ObservableCollection<clsServiceMasterVO> _DoctorServiceList;
        public ObservableCollection<clsServiceMasterVO> DoctorServiceList
        {
            get { return _DoctorServiceList; }
            set { _DoctorServiceList = value; }
        }


        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }
        public bool Modify { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.IPD.clsIPDAddUpdateAdmissionTypeServiceListBizAction";
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
