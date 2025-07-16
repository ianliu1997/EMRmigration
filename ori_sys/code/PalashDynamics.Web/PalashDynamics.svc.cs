using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.Generic;
using System.Text;
using PalashDynamics.BusinessLayer;
using PalashDynamics.ValueObjects;
using WcfExceptionExample.Web.Behavior;
using WcfExceptionExample.Web.DataContracts;

namespace PalashDynamics.Web
{
   
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [WcfErrorBehavior]
    [WcfSilverlightFaultBehavior]
    public class PalashDynamicsWeb:IPalashService 
    {
        //[OperationContract]
        //public void DoWork()
        //{
        //    // Add your operation implementation here
        //    return;
        //}

        // Add more operations here and mark them with [OperationContract]
        #region IPalashService Members
        [FaultContract(typeof(ConcurrencyException))]
        public IBizActionValueObject Process(IBizActionValueObject BizActionObject, clsUserVO UserInfo)
        {
            //throw new NotImplementedException();
            BizActionObject = (IBizActionValueObject)clsBusinessConroller.getInstance().Process(BizActionObject, UserInfo);
            return BizActionObject;
        }

        #endregion


        [FaultContract(typeof(ConcurrencyException))]
        public clsUserVO GetSessionUser(string sessionKey)
        {
            return System.Web.HttpContext.Current.Session[sessionKey] as clsUserVO;
        }
        
        public void SetSessionUser(string sessionKey, clsUserVO User)
        {
            System.Web.HttpContext.Current.Session[sessionKey] = User;
        }


    }
}
