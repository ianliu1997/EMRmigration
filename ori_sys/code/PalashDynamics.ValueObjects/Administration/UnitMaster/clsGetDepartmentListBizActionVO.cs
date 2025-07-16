using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.UnitMaster
{
   public class clsGetDepartmentListBizActionVO:IBizActionValueObject
    {
        private List<clsUnitMasterVO> _UnitDetails;
        public List<clsUnitMasterVO> UnitDetails
        {
            get { return _UnitDetails; }
            set { _UnitDetails = value; }
        }
        public bool Status { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.UnitMaster.clsGetDepartmentListBizAction";
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
