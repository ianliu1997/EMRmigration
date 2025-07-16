using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.UnitMaster
{
   public class clsGetUnitMasterListBizActionVO:IBizActionValueObject
    {
        private List<clsUnitMasterVO> _UnitDetails;
        public List<clsUnitMasterVO> UnitDetails
        {
            get { return _UnitDetails; }
            set { _UnitDetails = value; }
        }

        public string Description { get; set; }
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        public string sortExpression { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.UnitMaster.clsGetUnitMasterListBizAction";
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
