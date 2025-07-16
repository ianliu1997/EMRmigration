using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Administration;

namespace PalashDynamics.ValueObjects.EMR.NewEMR
{
    public class clsGetEMRMenuBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.NewEMR.clsGetEMRMenuBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private clsMenuVO _MenuDetail;

        public List<clsMenuVO> MenuDetails { get; set; }

        public long UserID { get; set; }

        public long UserRoleID { get; set; }
    }
}
