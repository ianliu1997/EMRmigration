using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
    public class clsIVFDashboard_AddUpdateOutcomeBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_AddUpdateOutcomeBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsIVFDashboard_OutcomeVO _OutcomeDetails = new clsIVFDashboard_OutcomeVO();
        public clsIVFDashboard_OutcomeVO OutcomeDetails
        {
            get
            {
                return _OutcomeDetails;
            }
            set
            {
                _OutcomeDetails = value;
            }
        }

        //added by neena
        private List<clsPregnancySacsDetailsVO> _PregnancySacsList = new List<clsPregnancySacsDetailsVO>();
        public List<clsPregnancySacsDetailsVO> PregnancySacsList
        {
            get
            {
                return _PregnancySacsList;
            }
            set
            {
                _PregnancySacsList = value;
            }
        }

        private List<clsIVFDashboard_OutcomeVO> _OutcomeDetailsList = new List<clsIVFDashboard_OutcomeVO>();
        public List<clsIVFDashboard_OutcomeVO> OutcomeDetailsList
        {
            get
            {
                return _OutcomeDetailsList;
            }
            set
            {
                _OutcomeDetailsList = value;
            }
        }

        private List<clsIVFDashboard_OutcomeVO> _OutcomePregnancySacList = new List<clsIVFDashboard_OutcomeVO>();
        public List<clsIVFDashboard_OutcomeVO> OutcomePregnancySacList
        {
            get
            {
                return _OutcomePregnancySacList;
            }
            set
            {
                _OutcomePregnancySacList = value;
            }
        }
        //

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

    public class clsIVFDashboard_GetOutcomeBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_GetOutcomeBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsIVFDashboard_OutcomeVO _Details = new clsIVFDashboard_OutcomeVO();
        public clsIVFDashboard_OutcomeVO Details
        {
            get
            {
                return _Details;
            }
            set
            {
                _Details = value;
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

        //added by neena
        private List<clsPregnancySacsDetailsVO> _PregnancySacsList = new List<clsPregnancySacsDetailsVO>();
        public List<clsPregnancySacsDetailsVO> PregnancySacsList
        {
            get
            {
                return _PregnancySacsList;
            }
            set
            {
                _PregnancySacsList = value;
            }
        }

        private List<clsIVFDashboard_OutcomeVO> _OutcomeDetailsList = new List<clsIVFDashboard_OutcomeVO>();
        public List<clsIVFDashboard_OutcomeVO> OutcomeDetailsList
        {
            get
            {
                return _OutcomeDetailsList;
            }
            set
            {
                _OutcomeDetailsList = value;
            }
        }

        private List<clsIVFDashboard_OutcomeVO> _OutcomePregnancySacList = new List<clsIVFDashboard_OutcomeVO>();
        public List<clsIVFDashboard_OutcomeVO> OutcomePregnancySacList
        {
            get
            {
                return _OutcomePregnancySacList;
            }
            set
            {
                _OutcomePregnancySacList = value;
            }
        }

        private clsIVFDashboard_OutcomeVO _ObjOutcomePregnancySacList = new clsIVFDashboard_OutcomeVO();
        public clsIVFDashboard_OutcomeVO ObjOutcomePregnancySacList
        {
            get
            {
                return _ObjOutcomePregnancySacList;
            }
            set
            {
                _ObjOutcomePregnancySacList = value;
            }
        }
        //
    }
}
