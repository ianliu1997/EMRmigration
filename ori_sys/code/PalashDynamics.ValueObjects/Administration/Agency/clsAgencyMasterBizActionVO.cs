using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.Agency
{
    public class clsAddAgencyMasterBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.Agency.clsAddAgencyMasterBizAction";
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

        //added by neena
        public bool IsAgent { get; set; }
        public bool CheckStatus { get; set; }
        private AgentVO _AgentDetails;
        public AgentVO AgentDetails
        {
            get { return _AgentDetails; }
            set { _AgentDetails = value; }
        }

        private List<YearClinic> _AgentYearList = new List<YearClinic>();
        public List<YearClinic> AgentYearList
        {
            get { return _AgentYearList; }
            set { _AgentYearList = value; }
        }

        private List<YearClinic> _AgentYearListSurrogacy = new List<YearClinic>();
        public List<YearClinic> AgentYearListSurrogacy
        {
            get { return _AgentYearListSurrogacy; }
            set { _AgentYearListSurrogacy = value; }
        }
        //
    }

    public class clsGetAgencyMasterListBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.Agency.clsGetAgencyMasterListBizAction";
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

        //added by neena
        public bool IsAgent { get; set; }
        public bool GetAgentByID { get; set; }
        private AgentVO _AgentDetails;
        public AgentVO AgentDetails
        {
            get { return _AgentDetails; }
            set { _AgentDetails = value; }
        }

        private List<AgentVO> _AgentDetailsList;
        public List<AgentVO> AgentDetailsList
        {
            get { return _AgentDetailsList; }
            set { _AgentDetailsList = value; }
        }

        private List<YearClinic> _AgentYearList = new List<YearClinic>();
        public List<YearClinic> AgentYearList
        {
            get { return _AgentYearList; }
            set { _AgentYearList = value; }
        }

        private List<YearClinic> _AgentYearListSurrogacy = new List<YearClinic>();
        public List<YearClinic> AgentYearListSurrogacy
        {
            get { return _AgentYearListSurrogacy; }
            set { _AgentYearListSurrogacy = value; }
        }
        //
    }
    //rohinee
   
}
