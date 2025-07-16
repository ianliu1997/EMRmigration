using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.IVFPlanTherapy;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
    public class clsIVFDashboard_AddUpdateDay0BizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_AddUpdateDay0BizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsIVFDashboard_LabDaysVO _Day0Details = new clsIVFDashboard_LabDaysVO();
        public clsIVFDashboard_LabDaysVO Day0Details
        {
            get
            {
                return _Day0Details;
            }
            set
            {
                _Day0Details = value;
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

    public class clsIVFDashboard_GetDay0DetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_GetDay0DetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private bool _IsSemenSample;
        public bool IsSemenSample
        {
            get
            {
                return _IsSemenSample;
            }
            set
            {
                _IsSemenSample = value;
            }
        }

        private List<MasterListItem> _SemenSampleList = new List<MasterListItem>();
        public List<MasterListItem> SemenSampleList
        {
            get 
            {
                return _SemenSampleList;
            }
            set
            {
                _SemenSampleList = value;
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

    public class clsIVFDashboard_AddDay0OocyteListBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_AddDay0OocyteListBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private List<clsIVFDashboard_LabDaysVO> _Day0OocList = new List<clsIVFDashboard_LabDaysVO>();
        public List<clsIVFDashboard_LabDaysVO> Day0OocList
        {
            get
            {
                return _Day0OocList;
            }
            set
            {
                _Day0OocList = value;
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

    public class clsIVFDashboard_GetDay0OocyteListBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_GetDay0OocyteListBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private List<clsIVFDashboard_LabDaysVO> _Day0OocList = new List<clsIVFDashboard_LabDaysVO>();
        public List<clsIVFDashboard_LabDaysVO> Day0OocList
        {
            get
            {
                return _Day0OocList;
            }
            set
            {
                _Day0OocList = value;
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

    //added by neena 
    public class clsIVFDashboard_GetDay0OocyteDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_GetDay0OocyteDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        public bool IsOocyteRecipient { get; set; }
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


    public class clsIVFDashboard_AddUpdateFertCheckBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_AddUpdateFertCheckBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsIVFDashboard_FertCheck _FertCheckDetails = new clsIVFDashboard_FertCheck();
        public clsIVFDashboard_FertCheck FertCheckDetails
        {
            get
            {
                return _FertCheckDetails;
            }
            set
            {
                _FertCheckDetails = value;
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

    public class clsIVFDashboard_GetFertCheckBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_GetFertCheckBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsIVFDashboard_FertCheck _FertCheckDetails = new clsIVFDashboard_FertCheck();
        public clsIVFDashboard_FertCheck FertCheckDetails
        {
            get
            {
                return _FertCheckDetails;
            }
            set
            {
                _FertCheckDetails = value;
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

        private bool _IsApply;
        public bool IsApply
        {
            get
            {
                return _IsApply;
            }
            set
            {
                _IsApply = value;
            }
        }

        private List<clsIVFDashboard_FertCheck> _Oocytelist = new List<clsIVFDashboard_FertCheck>();
        public List<clsIVFDashboard_FertCheck> Oocytelist
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

    public class clsIVFDashboard_DeleteAndGetLabDayImagesBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_DeleteAndGetLabDayImagesBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion        

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

        private clsAddImageVO _ImageObj = new clsAddImageVO();
        public clsAddImageVO ImageObj
        {
            get
            {
                return _ImageObj;
            }
            set
            {
                _ImageObj = value;
            }
        }

        private List<clsAddImageVO> _ImageList = new List<clsAddImageVO>();
        public List<clsAddImageVO> ImageList
        {
            get
            {
                return _ImageList;
            }
            set
            {
                _ImageList = value;
            }
        }
    }

    //


}
