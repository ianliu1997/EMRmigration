using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
    public class clsGetTherapyListBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetTherapyListBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

        #endregion

        public long CoupleID { get; set; }
        public long TherapyID { get; set; }
        public long TabID { get; set; }
        public long CoupleUintID { get; set; }
        public long TherapyUnitID { get; set; }
        public bool Flag { get; set; }
        public bool IsSurrogate { get; set; }
        //By Anjali............
        public long ProtocolTypeID { get; set; }
        public long PhysicianId { get; set; }
        public long PlannedTreatmentID { get; set; }
        public bool? rdoSuccessful { get; set; }
         public bool? rdoAll { get; set; }
         public bool? rdoUnsuccessful { get; set; }
         public bool? rdoClosed { get; set; }
         public bool? rdoActive { get; set; }
        //..........................
        private List<clsPlanTherapyVO> _TherapyDetailsList = new List<clsPlanTherapyVO>();
        public List<clsPlanTherapyVO> TherapyDetailsList
        {
            get
            {
                return _TherapyDetailsList;
            }
            set
            {
                _TherapyDetailsList = value;
            }
        }

        private clsPlanTherapyVO _TherapyDetails = new clsPlanTherapyVO();
        public clsPlanTherapyVO TherapyDetails
        {
            get
            {
                return _TherapyDetails;
            }
            set
            {
                _TherapyDetails = value;
            }
        }

        private List<clsTherapyDocumentsVO> _TherapyDocument = new List<clsTherapyDocumentsVO>();
        public List<clsTherapyDocumentsVO> TherapyDocument
        {
            get
            {
                return _TherapyDocument;
            }
            set
            {
                _TherapyDocument = value;
            }
        }

        private List<clsTherapyExecutionVO> _TherapyExecutionList = new List<clsTherapyExecutionVO>();
        public List<clsTherapyExecutionVO> TherapyExecutionList
        {
            get
            {
                return _TherapyExecutionList;
            }
            set
            {
                _TherapyExecutionList = value;
            }
        }

        private List<clsFollicularMonitoring> _FollicularMonitoringList = new List<clsFollicularMonitoring>();
        public List<clsFollicularMonitoring> FollicularMonitoringList
        {
            get
            {
                return _FollicularMonitoringList;
            }
            set
            {
                _FollicularMonitoringList = value;
            }
        }

        // bhushan

        private List<clsTherapyExecutionVO> _TherapyExecutionListSurrogate = new List<clsTherapyExecutionVO>();
        public List<clsTherapyExecutionVO> TherapyExecutionListSurrogate
        {
            get
            {
                return _TherapyExecutionListSurrogate;
            }
            set
            {
                _TherapyExecutionListSurrogate = value;
            }
        }

        private clsTherapyDeliveryVO _TherapyDelivery = new clsTherapyDeliveryVO();
        public clsTherapyDeliveryVO TherapyDelivery
        {
            get
            {
                return _TherapyDelivery;
            }
            set
            {
                _TherapyDelivery = value;
            }
        }

        private List<clsTherapyANCVO> _ANCList = new List<clsTherapyANCVO>();
        public List<clsTherapyANCVO> ANCList
        {
            get
            {
                return _ANCList;
            }
            set
            {
                _ANCList = value;
            }
        }

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

    public class clsGetTherapyDetailsForDashBoardBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetTherapyDetailsForDashBoardBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        public string SortExpression { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? FromDate2 { get; set; }
        public DateTime? ToDate2 { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long UnitID { get; set; }


        private List<clsTherapyDashBoardVO> _TherapyDetailsList = new List<clsTherapyDashBoardVO>();
        public List<clsTherapyDashBoardVO> TherapyDetailsList
        {
            get
            {
                return _TherapyDetailsList;
            }
            set
            {
                _TherapyDetailsList = value;
            }
        }

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


   
}
