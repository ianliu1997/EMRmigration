using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.IVFPlanTherapy;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
    public class clsIVFDashboard_AddUpdateVitrificationBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_AddUpdateVitrificationBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsIVFDashBoard_VitrificationVO _VitrificationDetails = new clsIVFDashBoard_VitrificationVO();
        public clsIVFDashBoard_VitrificationVO VitrificationMain
        {
            get
            {
                return _VitrificationDetails;
            }
            set
            {
                _VitrificationDetails = value;
            }
        }

        private clsIVFDashBoard_VitrificationDetailsVO _VitrificationDetailsObj = new clsIVFDashBoard_VitrificationDetailsVO();
        public clsIVFDashBoard_VitrificationDetailsVO VitrificationDetailsObj
        {
            get
            {
                return _VitrificationDetailsObj;
            }
            set
            {
                _VitrificationDetailsObj = value;
            }
        }

        private List<clsIVFDashBoard_VitrificationDetailsVO> _VitrificationDetailsList = new List<clsIVFDashBoard_VitrificationDetailsVO>();
        public List<clsIVFDashBoard_VitrificationDetailsVO> VitrificationDetailsList
        {
            get
            {
                return _VitrificationDetailsList;
            }
            set
            {
                _VitrificationDetailsList = value;
            }
        }

        //Added By CDS
        private clsIVFDashBoard_VitrificationVO _VitrificationDetailsForOocyte = new clsIVFDashBoard_VitrificationVO();
        public clsIVFDashBoard_VitrificationVO VitrificationMainForOocyte
        {
            get
            {
                return _VitrificationDetailsForOocyte;
            }
            set
            {
                _VitrificationDetailsForOocyte = value;
            }
        }

        private clsIVFDashBoard_VitrificationDetailsVO _VitrificationDetailsForOocyteObj = new clsIVFDashBoard_VitrificationDetailsVO();
        public clsIVFDashBoard_VitrificationDetailsVO VitrificationDetailsForOocyteObj
        {
            get
            {
                return _VitrificationDetailsForOocyteObj;
            }
            set
            {
                _VitrificationDetailsForOocyteObj = value;
            }
        }

        private List<clsIVFDashBoard_VitrificationDetailsVO> _VitrificationDetailsForOocyteList = new List<clsIVFDashBoard_VitrificationDetailsVO>();
        public List<clsIVFDashBoard_VitrificationDetailsVO> VitrificationDetailsForOocyteList
        {
            get
            {
                return _VitrificationDetailsForOocyteList;
            }
            set
            {
                _VitrificationDetailsForOocyteList = value;
            }
        }
        //END

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

        private bool _IsFreezeOocytes;      //Flag set while save Freeze Oocytes under Freeze All Oocytes Cycle for freeze & Thaw
        public bool IsFreezeOocytes
        {
            get { return _IsFreezeOocytes; }
            set
            {
                _IsFreezeOocytes = value;
            }
        }

        private bool _IsRefreeze;
        public bool IsRefreeze
        {
            get { return _IsRefreeze; }
            set
            {
                if (_IsRefreeze != value)
                {
                    _IsRefreeze = value;                  
                }
            }
        }

        private bool _IsRenewal;
        public bool IsRenewal
        {
            get { return _IsRenewal; }
            set
            {
                if (_IsRenewal != value)
                {
                    _IsRenewal = value;
                }
            }
        }

        public bool IsOocyteFreezed { get; set; }
        public bool IsSprem { get; set; }
        public long VitificationID { get; set; }
        public long VitificationUnitID { get; set; }
        public long VitificationDetailsID { get; set; }
        public long VitificationDetailsUnitID { get; set; }
        public long SpremFreezingID { get; set; }
        public long SpremFreezingUnitID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? ExpiryTime { get; set; }
        public bool LongTerm { get; set; }
        public bool ShortTerm { get; set; }
    }

    public class clsIVFDashboard_GetVitrificationBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_GetVitrificationBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsIVFDashBoard_VitrificationVO _VitrificationDetails = new clsIVFDashBoard_VitrificationVO();
        public clsIVFDashBoard_VitrificationVO VitrificationMain
        {
            get
            {
                return _VitrificationDetails;
            }
            set
            {
                _VitrificationDetails = value;
            }
        }

        private List<clsIVFDashBoard_VitrificationDetailsVO> _VitrificationDetailsList = new List<clsIVFDashBoard_VitrificationDetailsVO>();
        public List<clsIVFDashBoard_VitrificationDetailsVO> VitrificationDetailsList
        {
            get
            {
                return _VitrificationDetailsList;
            }
            set
            {
                _VitrificationDetailsList = value;
            }
        }

        private clsIVFDashBoard_VitrificationVO _VitrificationRefeezeDetails = new clsIVFDashBoard_VitrificationVO();
        public clsIVFDashBoard_VitrificationVO VitrificationRefeezeMain
        {
            get
            {
                return _VitrificationRefeezeDetails;
            }
            set
            {
                _VitrificationRefeezeDetails = value;
            }
        }

        private List<clsIVFDashBoard_VitrificationDetailsVO> _VitrificationRefeezeDetailsList = new List<clsIVFDashBoard_VitrificationDetailsVO>();
        public List<clsIVFDashBoard_VitrificationDetailsVO> VitrificationRefeezeDetailsList
        {
            get
            {
                return _VitrificationRefeezeDetailsList;
            }
            set
            {
                _VitrificationRefeezeDetailsList = value;
            }
        }

        //Added By CDS
        private clsIVFDashBoard_VitrificationVO _VitrificationDetailsForOocyte = new clsIVFDashBoard_VitrificationVO();
        public clsIVFDashBoard_VitrificationVO VitrificationMainForOocyte
        {
            get
            {
                return _VitrificationDetailsForOocyte;
            }
            set
            {
                _VitrificationDetailsForOocyte = value;
            }
        }

        private List<clsIVFDashBoard_VitrificationDetailsVO> _VitrificationDetailsForOocyteList = new List<clsIVFDashBoard_VitrificationDetailsVO>();
        public List<clsIVFDashBoard_VitrificationDetailsVO> VitrificationDetailsForOocyteList
        {
            get
            {
                return _VitrificationDetailsForOocyteList;
            }
            set
            {
                _VitrificationDetailsForOocyteList = value;
            }
        }
        //END

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

        private bool _IsFreezeOocytes;      //Flag set while retrive Freeze Oocytes under Freeze All Oocytes Cycle 
        public bool IsFreezeOocytes
        {
            get { return _IsFreezeOocytes; }
            set
            {
                _IsFreezeOocytes = value;
            }
        }

        private bool _IsForThawTab;      //Flag set while retrive Cryo Oocytes under FE ICSI cycle from Freeze All Oocytes Cycle 
        public bool IsForThawTab
        {
            get { return _IsForThawTab; }
            set
            {
                _IsForThawTab = value;
            }
        }
    }

    public class clsIVFDashboard_GetPreviousVitrificationBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_GetPreviousVitrificationBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsIVFDashBoard_VitrificationVO _VitrificationDetails = new clsIVFDashBoard_VitrificationVO();
        public clsIVFDashBoard_VitrificationVO VitrificationMain
        {
            get
            {
                return _VitrificationDetails;
            }
            set
            {
                _VitrificationDetails = value;
            }
        }

        private List<clsIVFDashBoard_VitrificationDetailsVO> _VitrificationDetailsList = new List<clsIVFDashBoard_VitrificationDetailsVO>();
        public List<clsIVFDashBoard_VitrificationDetailsVO> VitrificationDetailsList
        {
            get
            {
                return _VitrificationDetailsList;
            }
            set
            {
                _VitrificationDetailsList = value;
            }
        }

        //Added By CDS
        private clsIVFDashBoard_VitrificationVO _VitrificationDetailsForOocyte = new clsIVFDashBoard_VitrificationVO();
        public clsIVFDashBoard_VitrificationVO VitrificationMainForOocyte
        {
            get
            {
                return _VitrificationDetailsForOocyte;
            }
            set
            {
                _VitrificationDetailsForOocyte = value;
            }
        }

        private List<clsIVFDashBoard_VitrificationDetailsVO> _VitrificationDetailsForOocyteList = new List<clsIVFDashBoard_VitrificationDetailsVO>();
        public List<clsIVFDashBoard_VitrificationDetailsVO> VitrificationDetailsForOocyteList
        {
            get
            {
                return _VitrificationDetailsForOocyteList;
            }
            set
            {
                _VitrificationDetailsForOocyteList = value;
            }
        }
        //END

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

        private bool _IsFreezeOocytes;      //Flag set while retrive Freeze Oocytes under FE ICSI Cycle 
        public bool IsFreezeOocytes
        {
            get { return _IsFreezeOocytes; }
            set
            {
                _IsFreezeOocytes = value;
            }
        }


    }

    public class clsIVFDashboard_UpdateVitrificationDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_UpdateVitrificationDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsIVFDashBoard_VitrificationVO _VitrificationDetails = new clsIVFDashBoard_VitrificationVO();
        public clsIVFDashBoard_VitrificationVO VitrificationMain
        {
            get
            {
                return _VitrificationDetails;
            }
            set
            {
                _VitrificationDetails = value;
            }
        }

        private List<clsIVFDashBoard_VitrificationDetailsVO> _VitrificationDetailsList = new List<clsIVFDashBoard_VitrificationDetailsVO>();
        public List<clsIVFDashBoard_VitrificationDetailsVO> VitrificationDetailsList
        {
            get
            {
                return _VitrificationDetailsList;
            }
            set
            {
                _VitrificationDetailsList = value;
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

    public class clsIVFDashboard_GetUsedEmbryoDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_GetUsedEmbryoDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsIVFDashBoard_VitrificationVO _VitrificationDetails = new clsIVFDashBoard_VitrificationVO();
        public clsIVFDashBoard_VitrificationVO VitrificationMain
        {
            get
            {
                return _VitrificationDetails;
            }
            set
            {
                _VitrificationDetails = value;
            }
        }

        private List<clsIVFDashBoard_VitrificationDetailsVO> _VitrificationDetailsList = new List<clsIVFDashBoard_VitrificationDetailsVO>();
        public List<clsIVFDashBoard_VitrificationDetailsVO> VitrificationDetailsList
        {
            get
            {
                return _VitrificationDetailsList;
            }
            set
            {
                _VitrificationDetailsList = value;
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
