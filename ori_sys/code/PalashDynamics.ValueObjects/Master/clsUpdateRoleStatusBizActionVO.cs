using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.User;

namespace PalashDynamics.ValueObjects.Master
{
   public class clsUpdateRoleStatusBizActionVO:IBizActionValueObject
    {
       #region IBizActionVO Member
       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.Master.clsUpdateRoleStatusBizAction";
           //throw new NotImplementedException();
       }
       #endregion

       #region IValueObject Members
       public string ToXml()
       {
           return this.ToString();
           // throw new NotImplementedException();
       }
       #endregion

       private int _SuccessStatus;
       /// <summary>
       /// Output Property.
       /// This property states the outcome of BizAction Process.
       /// </summary>

       public int SuccessStatus
       {
           get { return _SuccessStatus; }
           set { _SuccessStatus = value; }

       }

       private clsUserRoleVO objRoleStatus = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains Status Details of a User Role.
        /// </summary>

       public clsUserRoleVO RoleStatus
       {
           get { return objRoleStatus; }
           set { objRoleStatus = value; }
       }
    }
}
