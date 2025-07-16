using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.IVFPlanTherapy;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
    public class clsIVFDashboard_GetTherapyListBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_GetTherapyListBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

        #endregion

        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }

        public long CoupleID { get; set; }
        public long TherapyID { get; set; }
        public long TabID { get; set; }
        public long CoupleUintID { get; set; }
        public long TherapyUnitID { get; set; }
        public bool Flag { get; set; }
        public bool IsSurrogate { get; set; }
        public long ProtocolTypeID { get; set; }
        public long PhysicianId { get; set; }
        public long PlannedTreatmentID { get; set; }
        public bool? rdoSuccessful { get; set; }
        public bool? rdoAll { get; set; }
        public bool? rdoUnsuccessful { get; set; }
        public bool? rdoClosed { get; set; }
        public bool? rdoActive { get; set; }
        public bool? AttachedSurrogate { get; set; }
        public bool? NewButtonVisibility { get; set; }

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

    public class clsIVFDashboard_GetVisitForARTPrescriptionBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_GetVisitForARTPrescriptionBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

        #endregion

        public long ID { get; set; }
        public long UnitID { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }

        public long CoupleID { get; set; }
        public long TherapyID { get; set; }
        public long TabID { get; set; }
        public long CoupleUintID { get; set; }
        public long TherapyUnitID { get; set; }
        public bool Flag { get; set; }


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