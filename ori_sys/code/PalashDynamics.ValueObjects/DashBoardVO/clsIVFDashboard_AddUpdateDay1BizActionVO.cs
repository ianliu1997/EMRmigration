using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
  public  class clsIVFDashboard_AddUpdateDay1BizActionVO: IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_AddUpdateDay1BizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsIVFDashboard_LabDaysVO _Day1Details = new clsIVFDashboard_LabDaysVO();
        public clsIVFDashboard_LabDaysVO Day1Details
        {
            get
            {
                return _Day1Details;
            }
            set
            {
                _Day1Details = value;
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

    public class clsIVFDashboard_GetDay1DetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_GetDay1DetailsBizAction";
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

        private bool _IsGetDate;
        public bool IsGetDate
        {
            get
            {
                return _IsGetDate;
            }
            set
            {
                _IsGetDate = value;
            }
        }
    }

    //added by neena 
    public class clsIVFDashboard_GetDay1OocyteDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_GetDay1OocyteDetailsBizAction";
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

        private bool _IsDay1Oocyte;
        public bool IsDay1Oocyte
        {
            get
            {
                return _IsDay1Oocyte;
            }
            set
            {
                _IsDay1Oocyte = value;
            }
        }
    }
}


