using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.Agency
{
    public class clsGetServiceListBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.Agency.clsGetServiceListBizAction";
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
        public long SpecializationID { get; set; }
        public long SubSpecializationID { get; set; }
        public string ServiceName { get; set; }
        public long UnitID { get; set; }
        public long AgencyID { get; set; }
        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public bool PagingEnabled { get; set; }
        public long TotalRows { get; set; }
        public bool IsAgencyServiceLinkView { get; set; }
        public bool IsStatusChanged { get; set; }
        public bool IsModify { get; set; }
    }

    public class clsAddServiceAgencyLinkBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.Agency.clsAddServiceAgencyLinkBizAction";
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

        private List<clsServiceMasterVO> _ServiceListForLink;
        public List<clsServiceMasterVO> ServiceListForLink
        {
            get { return _ServiceListForLink; }
            set { _ServiceListForLink = value; }
        }
        private clsAgencyMasterVO _objServiceAgencyDetails;
        public clsAgencyMasterVO objServiceAgencyDetails
        {
            get { return _objServiceAgencyDetails; }
            set { _objServiceAgencyDetails = value; }
        }
        private bool _IsDefault;
        public bool IsDefault
        {
            get { return _IsDefault; }
            set { _IsDefault = value; }
        }
        public bool IsStatusChanged { get; set; }
        public bool IsModify { get; set; }
    }


    public class clsGetServiceToAgencyBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.Agency.clsGetServiceToAgencyBizAction";
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
