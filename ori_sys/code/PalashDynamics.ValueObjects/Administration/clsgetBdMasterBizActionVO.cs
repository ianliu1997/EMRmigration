using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration
{   

    public class clsgetBdMasterBizActionVO : IBizActionValueObject
    {
        public clsgetBdMasterBizActionVO()
        {
        
        }

        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsGetBdMasterListBizAction";
                    //PalashDynamics.BusinessLayer.Administration.clsGetBdMasterListBizAction
        }

        public string ToXml()
        {
            return this.ToXml();
        }

        public long UnitID { get; set; }
        #endregion
        private List<MasterListItem> _MasterListItem = null;
        public List<MasterListItem> MasterListItem
        {
            get
            { return _MasterListItem; }

            set
            { _MasterListItem = value; }
        }
        //private cls_DashboardMIS_ReferralReportVO _Details = new cls_DashboardMIS_ReferralReportVO();
        //public cls_DashboardMIS_ReferralReportVO Details
        //{
        //    get
        //    {
        //        return _Details;
        //    }
        //    set
        //    {
        //        _Details = value;
        //    }
        //}

        //private List<cls_DashboardMIS_ReferralReportListVO> _ReferralReportList = new List<cls_DashboardMIS_ReferralReportListVO>();
        //public List<cls_DashboardMIS_ReferralReportListVO> ReferralReportList
        //{
        //    get { return _ReferralReportList; }
        //    set { _ReferralReportList = value; }
        //}
    }
}
