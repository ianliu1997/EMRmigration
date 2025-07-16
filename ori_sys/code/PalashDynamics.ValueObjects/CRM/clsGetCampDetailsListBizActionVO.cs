using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.CRM
{
  public class clsGetCampDetailsListBizActionVO:IBizActionValueObject
    {
        private List<clsCampMasterVO> _CampDetailsList;
        public List<clsCampMasterVO> CampDetailsList
        {
            get { return _CampDetailsList; }
            set { _CampDetailsList = value; }
        }

        private long? _Camp;
        public long? Camp
        {
            get { return _Camp; }
            set { _Camp = value; }
        }

        private DateTime? _FromDate;
        public DateTime? FromDate
        {
            get { return _FromDate; }
            set { _FromDate = value; }
        }

        private DateTime? _ToDate;
        public DateTime? ToDate
        {
            get { return _ToDate; }
            set { _ToDate = value; }
        }


        private string _SearchExpression;
        public string SearchExpression
        {
            get { return _SearchExpression; }
            set { _SearchExpression = value; }
        }


        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        public string sortExpression { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.CRM.clsGetCampDetailsListBizAction";

        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }
}
