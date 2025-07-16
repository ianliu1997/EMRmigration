using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Runtime.Remoting;
using PalashDynamics.ValueObjects;



namespace PalashDynamics.BusinessLayer
{
    //This class is used to Process the request made by Client
    //Based  upon Parameters passed by the client Required business
    //action eill be called
    public class clsBusinessConroller
    {
        private static clsBusinessConroller _Instance = null;

        private clsBusinessConroller()
        {

        }

        public static clsBusinessConroller getInstance()
        {
            if (_Instance == null)
                _Instance = new clsBusinessConroller();
            return _Instance;
        }


        //The Process Method

        public IValueObject Process(IBizActionValueObject bizObject, clsUserVO objUserVO)
        {
            IValueObject returnVO = null;
            try
            {
                //Declare a Refernece of BizAction 
                BizAction bizAction = null;
                string FullTypeName = bizObject.GetBizAction();

                MethodInfo GetInstance = Type.GetType(FullTypeName).GetMethod("GetInstance");
                bizAction = (BizAction)GetInstance.Invoke(null, BindingFlags.Public | BindingFlags.Static, null, null, null);

                //returnVO = bizAction.Process(valueObject, objUserVO);
                returnVO = bizAction.Process(bizObject, objUserVO);
            }

            catch (Exception ex)
            {
                //Log Error
            }
            finally
            {

            }
            return returnVO;
        }


    }

}
