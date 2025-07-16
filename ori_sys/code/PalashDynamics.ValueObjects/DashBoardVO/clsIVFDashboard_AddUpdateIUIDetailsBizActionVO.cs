using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
 public  class clsIVFDashboard_AddUpdateIUIDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_AddUpdateIUIDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsIVFDashboard_IUIVO _IUIDetails = new clsIVFDashboard_IUIVO();
        public clsIVFDashboard_IUIVO IUIDetails
        {
            get
            {
                return _IUIDetails;
            }
            set
            {
                _IUIDetails = value;
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

 public class clsIVFDashboard_GetIUIDetailsBizActionVO : IBizActionValueObject
 {
     #region  IBizActionValueObject
     public string GetBizAction()
     {
         return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_GetIUIDetailsBizAction";
     }

     public string ToXml()
     {
         return this.ToXml();
     }


     #endregion

     private clsIVFDashboard_IUIVO _Details = new clsIVFDashboard_IUIVO();
     public clsIVFDashboard_IUIVO Details
     {
         get
         {
             return _Details;
         }
         set
         {
             _Details = value;
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
