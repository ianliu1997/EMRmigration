using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
  public  class clsIVFDashboard_AddUpdateDay6BizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_AddUpdateDay6BizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsIVFDashboard_LabDaysVO _Day6Details = new clsIVFDashboard_LabDaysVO();
        public clsIVFDashboard_LabDaysVO Day6Details
        {
            get
            {
                return _Day6Details;
            }
            set
            {
                _Day6Details = value;
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

    public class clsIVFDashboard_GetDay6DetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_GetDay6DetailsBizAction";
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
    public class clsIVFDashboard_GetDay6OocyteDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_GetDay6OocyteDetailsBizAction";
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
