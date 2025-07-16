using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.UserRights
{
    public class clsSetCreditLimitBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //PalashDynamics.BusinessLayer.Administration.UserRights
            return "PalashDynamics.BusinessLayer.Administration.UserRights.clsSetCreditLimitBizAction";
        }
        # endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }
        # endregion

        public clsUserRightsVO objUserRight { get; set; }
        public List<clsUserRightsVO> objListUserRights { get; set; }
        
    }
}
