using System;
using System.Collections.Generic;
using System.Linq;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using System.Text;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
    public class clsIVFDashboard_AddUpdateThawingBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_AddUpdateThawingBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsIVFDashBoard_ThawingVO _Thawing = new clsIVFDashBoard_ThawingVO();
        public clsIVFDashBoard_ThawingVO Thawing
        {
            get
            {
                return _Thawing;
            }
            set
            {
                _Thawing = value;
            }
        }

        private clsIVFDashBoard_ThawingDetailsVO _ThawingObj = new clsIVFDashBoard_ThawingDetailsVO();
        public clsIVFDashBoard_ThawingDetailsVO ThawingObj
        {
            get
            {
                return _ThawingObj;
            }
            set
            {
                _ThawingObj = value;
            }
        }

        private List<clsIVFDashBoard_ThawingDetailsVO> _ThawingDetailList = new List<clsIVFDashBoard_ThawingDetailsVO>();
        public List<clsIVFDashBoard_ThawingDetailsVO> ThawingDetailsList
        {
            get
            {
                return _ThawingDetailList;
            }
            set
            {
                _ThawingDetailList = value;
            }
        }

        // Added By CDS 
        private clsIVFDashBoard_ThawingVO _ThawingForOocyte = new clsIVFDashBoard_ThawingVO();
        public clsIVFDashBoard_ThawingVO ThawingForOocyte
        {
            get
            {
                return _ThawingForOocyte;
            }
            set
            {
                _ThawingForOocyte = value;
            }
        }

        private clsIVFDashBoard_ThawingDetailsVO _ThawingForOocyteObj = new clsIVFDashBoard_ThawingDetailsVO();
        public clsIVFDashBoard_ThawingDetailsVO ThawingForOocyteObj
        {
            get
            {
                return _ThawingForOocyteObj;
            }
            set
            {
                _ThawingForOocyteObj = value;
            }
        }

        private List<clsIVFDashBoard_ThawingDetailsVO> _ThawingDetailForOocyteList = new List<clsIVFDashBoard_ThawingDetailsVO>();
        public List<clsIVFDashBoard_ThawingDetailsVO> ThawingDetailsForOocyteList
        {
            get
            {
                return _ThawingDetailForOocyteList;
            }
            set
            {
                _ThawingDetailForOocyteList = value;
            }
        }

        private bool _IsOnlyForEmbryoThawing;
        public bool IsOnlyForEmbryoThawing
        {
            get { return _IsOnlyForEmbryoThawing; }
            set
            {
                if (_IsOnlyForEmbryoThawing != value)
                {
                    _IsOnlyForEmbryoThawing = value;
                    //OnPropertyChanged("IsOnlyForEmbryoThawing");
                }
            }
        }
        // END
        private string _PostThawingPlan;
        public string PostThawingPlan
        {
            get { return _PostThawingPlan; }
            set
            {
                if (_PostThawingPlan != value)
                {
                    _PostThawingPlan = value;
                    
                }
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

        private bool _IsThawFreezeOocytes;      //Flag use while save Freeze Oocytes under FE ICSI Cycle for Thaw 
        public bool IsThawFreezeOocytes
        {
            get { return _IsThawFreezeOocytes; }
            set
            {
                _IsThawFreezeOocytes = value;
            }
        }


        private bool _IsThawFreezeEmbryos;      //Flag use while save Embryos under freeze/Thaw Transfer Cycle on Thaw Tab
        public bool IsThawFreezeEmbryos
        {
            get { return _IsThawFreezeEmbryos; }
            set
            {
                _IsThawFreezeEmbryos = value;
            }
        }

    }

    public class clsIVFDashboard_GetThawingBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_GetThawingBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsIVFDashBoard_ThawingVO _Thawing = new clsIVFDashBoard_ThawingVO();
        public clsIVFDashBoard_ThawingVO Thawing
        {
            get
            {
                return _Thawing;
            }
            set
            {
                _Thawing = value;
            }
        }

        private List<clsIVFDashBoard_ThawingDetailsVO> _ThawingDetailList = new List<clsIVFDashBoard_ThawingDetailsVO>();
        public List<clsIVFDashBoard_ThawingDetailsVO> ThawingDetailsList
        {
            get
            {
                return _ThawingDetailList;
            }
            set
            {
                _ThawingDetailList = value;
            }
        }

        // Added By CDS 
        private clsIVFDashBoard_ThawingVO _ThawingForOocyte = new clsIVFDashBoard_ThawingVO();
        public clsIVFDashBoard_ThawingVO ThawingForOocyte
        {
            get
            {
                return _ThawingForOocyte;
            }
            set
            {
                _ThawingForOocyte = value;
            }
        }

        private List<clsIVFDashBoard_ThawingDetailsVO> _ThawingDetailForOocyteList = new List<clsIVFDashBoard_ThawingDetailsVO>();
        public List<clsIVFDashBoard_ThawingDetailsVO> ThawingDetailsForOocyteList
        {
            get
            {
                return _ThawingDetailForOocyteList;
            }
            set
            {
                _ThawingDetailForOocyteList = value;
            }
        }

        // Added By CDS 
        private bool _IsOnlyForEmbryoThawing;
        public bool IsOnlyForEmbryoThawing
        {
            get { return _IsOnlyForEmbryoThawing; }
            set
            {
                if (_IsOnlyForEmbryoThawing != value)
                {
                    _IsOnlyForEmbryoThawing = value;
                    //OnPropertyChanged("IsOnlyForEmbryoThawing");
                }
            }
        }
        //END 
        // END

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
    public class clsIVFDashboard_AddUpdateThawingWOCryoBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_AddUpdateThawingWOCryoBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsIVFDashBoard_ThawingVO _Thawing = new clsIVFDashBoard_ThawingVO();
        public clsIVFDashBoard_ThawingVO Thawing
        {
            get
            {
                return _Thawing;
            }
            set
            {
                _Thawing = value;
            }
        }

        private List<clsIVFDashBoard_ThawingDetailsVO> _ThawingDetailList = new List<clsIVFDashBoard_ThawingDetailsVO>();
        public List<clsIVFDashBoard_ThawingDetailsVO> ThawingDetailsList
        {
            get
            {
                return _ThawingDetailList;
            }
            set
            {
                _ThawingDetailList = value;
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

        private bool _IsThawFreezeOocytes;      //Flag use while save Freeze Oocytes under FE ICSI Cycle for Thaw 
        public bool IsThawFreezeOocytes
        {
            get { return _IsThawFreezeOocytes; }
            set
            {
                _IsThawFreezeOocytes = value;
            }
        }

        private bool _IsFreezeOocytes;      //Flag set while retrive Freeze Oocytes under Freeze All Oocytes Cycle 
        public bool IsFreezeOocytes
        {
            get { return _IsFreezeOocytes; }
            set
            {
                _IsFreezeOocytes = value;
            }
        }


    }
    //
}
