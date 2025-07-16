using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.Agency
{

    public class clsAddAgencyClinicLinkBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.Agency.clsAddAgencyClinicLinkBizAction";
        }

        public string ToXml()
        {

            return this.ToString();
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private clsAgencyClinicLinkVO _AgencyClinicLinkDetails;
        public clsAgencyClinicLinkVO AgencyClinicLinkDetails
        {
            get { return _AgencyClinicLinkDetails; }
            set { _AgencyClinicLinkDetails = value; }
        }
        private List<clsAgencyClinicLinkVO> _AgencyClinicLinkList = new List<clsAgencyClinicLinkVO>();
        public List<clsAgencyClinicLinkVO> AgencyClinicLinkList
        {
            get { return _AgencyClinicLinkList; }
            set { _AgencyClinicLinkList = value; }
        }

        private clsAgencyMasterVO _AgencyMasterDetails;
        public clsAgencyMasterVO AgencyMasterDetails
        {
            get { return _AgencyMasterDetails; }
            set { _AgencyMasterDetails = value; }
        }
        private List<clsAgencyMasterVO> _AgencyMasterList = new List<clsAgencyMasterVO>();
        public List<clsAgencyMasterVO> AgencyMasterList
        {
            get { return _AgencyMasterList; }
            set { _AgencyMasterList = value; }
        }

        public bool IsStatusChanged { get; set; }
        public bool IsModify { get; set; }
       
    }

    public class clsGetAgencyClinicLinkBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.Agency.clsGetAgencyClinicLinkBizAction";
        }

        public string ToXml()
        {

            return this.ToString();
        }
        public int TotalRows { get; set; }
        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public bool PagingEnabled { get; set; }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private clsAgencyClinicLinkVO _AgencyClinicLinkDetails;
        public clsAgencyClinicLinkVO AgencyClinicLinkDetails
        {
            get { return _AgencyClinicLinkDetails; }
            set { _AgencyClinicLinkDetails = value; }
        }
        private List<clsAgencyClinicLinkVO> _AgencyClinicLinkList = new List<clsAgencyClinicLinkVO>();
        public List<clsAgencyClinicLinkVO> AgencyClinicLinkList
        {
            get { return _AgencyClinicLinkList; }
            set { _AgencyClinicLinkList = value; }
        }

        private clsAgencyMasterVO _AgencyMasterDetails;
        public clsAgencyMasterVO AgencyMasterDetails
        {
            get { return _AgencyMasterDetails; }
            set { _AgencyMasterDetails = value; }
        }
        private List<clsAgencyMasterVO> _AgencyMasterList = new List<clsAgencyMasterVO>();
        public List<clsAgencyMasterVO> AgencyMasterList
        {
            get { return _AgencyMasterList; }
            set { _AgencyMasterList = value; }
        }
    }

    public class clsGetClinicMasterListBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.Agency.clsGetClinicMasterListBizAction";
        }

        public string ToXml()
        {

            return this.ToString();
        }

        public int TotalRows { get; set; }
        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public bool PagingEnabled { get; set; }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }
        private clsAgencyMasterVO _clinicDetails;
        public clsAgencyMasterVO ClinicDetails
        {
            get { return _clinicDetails; }
            set { _clinicDetails = value; }
        }
        private List<clsAgencyClinicLinkVO> _AgencyClinicLinkList = new List<clsAgencyClinicLinkVO>();
        public List<clsAgencyClinicLinkVO> AgencyClinicLinkList
        {
            get { return _AgencyClinicLinkList; }
            set { _AgencyClinicLinkList = value; }
        }

        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }


    }

    public class clsGetServiceToAgencyAssignedBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.Agency.clsGetServiceToAgencyAssignedBizAction";
        }

        public string ToXml()
        {

            return this.ToString();
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private List<clsServiceMasterVO> _ServiceList;
        public List<clsServiceMasterVO> ServiceList
        {
            get { return _ServiceList; }
            set { _ServiceList = value; }
        }
        private clsServiceMasterVO _ServiceDetails;
        public clsServiceMasterVO ServiceDetails
        {
            get { return _ServiceDetails; }
            set { _ServiceDetails = value; }
        }

        public string ServiceName { get; set; }
        public long UnitID { get; set; }
        public long AgencyID { get; set; }
        public long ServiceID { get; set; }
        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public bool PagingEnabled { get; set; }
        public long TotalRows { get; set; }
        public bool IsAgencyServiceLinkView { get; set; }
        public bool IsStatusChanged { get; set; }
        public bool IsAssignedService { get; set; }
    }
}
