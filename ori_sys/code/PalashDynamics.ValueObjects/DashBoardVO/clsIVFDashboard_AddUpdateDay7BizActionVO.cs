using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
    public class clsIVFDashboard_AddUpdateDay7BizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_AddUpdateDay7BizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsIVFDashboard_LabDaysVO _Day7Details = new clsIVFDashboard_LabDaysVO();
        public clsIVFDashboard_LabDaysVO Day7Details
        {
            get
            {
                return _Day7Details;
            }
            set
            {
                _Day7Details = value;
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

    public class clsIVFDashboard_GetDay7DetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_GetDay7DetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsIVFDashboard_LabDaysVO _Details = new clsIVFDashboard_LabDaysVO();
        public clsIVFDashboard_LabDaysVO Details
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
    }

    //added by neena
    public class clsIVFDashboard_GetDay7OocyteDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_GetDay7OocyteDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private List<clsIVFDashboard_LabDaysVO> _Oocytelist = new List<clsIVFDashboard_LabDaysVO>();
        public List<clsIVFDashboard_LabDaysVO> Oocytelist
        {
            get
            {
                return _Oocytelist;
            }
            set
            {
                _Oocytelist = value;
            }
        }
        private clsIVFDashboard_LabDaysVO _Details = new clsIVFDashboard_LabDaysVO();
        public clsIVFDashboard_LabDaysVO Details
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
    }
}
