using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.IVFPlanTherapy;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
   public class clsIVFDashboard_GetFollicularMonitoringSizeDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_GetFollicularMonitoringSizeDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        public long FollicularID { get; set; }
        public long TherapyID { get; set; }
        public long UnitID { get; set; }

        private List<clsFollicularMonitoringSizeDetails> _FollicularMonitoringSizeList = new List<clsFollicularMonitoringSizeDetails>();
        public List<clsFollicularMonitoringSizeDetails> FollicularMonitoringSizeList
        {
            get
            {
                return _FollicularMonitoringSizeList;
            }
            set
            {
                _FollicularMonitoringSizeList = value;
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


   public class clsIVFDashboard_UpdateFollicularMonitoringBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_UpdateFollicularMonitoringBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion


        public long UnitID { get; set; }

        public long FollicularID { get; set; }
        public long TherapyID { get; set; }

        private clsFollicularMonitoring _FollicularMonitoringDetial = new clsFollicularMonitoring();
        public clsFollicularMonitoring FollicularMonitoringDetial
        {
            get
            {
                return _FollicularMonitoringDetial;
            }
            set
            {
                _FollicularMonitoringDetial = value;
            }
        }

        private clsFollicularMonitoringSizeDetails _FollicularMonitoringSizeDetials = new clsFollicularMonitoringSizeDetails();
        public clsFollicularMonitoringSizeDetails FollicularMonitoringSizeDetials
        {
            get
            {
                return _FollicularMonitoringSizeDetials;
            }
            set
            {
                _FollicularMonitoringSizeDetials = value;
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

   public class clsIVFDashboard_GetFolliculeLRSumBizActionVO : IBizActionValueObject
   {
       #region  IBizActionValueObject
       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_GetFolliculeLRSumBizAction";
       }

       public string ToXml()
       {
           return this.ToXml();
       }


       #endregion


       public long UnitID { get; set; }

       public long FollicularID { get; set; }
       public long TherapyID { get; set; }

       private clsFollicularMonitoring _FollicularMonitoringDetial = new clsFollicularMonitoring();
       public clsFollicularMonitoring FollicularMonitoringDetial
       {
           get
           {
               return _FollicularMonitoringDetial;
           }
           set
           {
               _FollicularMonitoringDetial = value;
           }
       }

       private clsFollicularMonitoringSizeDetails _FollicularMonitoringSizeDetials = new clsFollicularMonitoringSizeDetails();
       public clsFollicularMonitoringSizeDetails FollicularMonitoringSizeDetials
       {
           get
           {
               return _FollicularMonitoringSizeDetials;
           }
           set
           {
               _FollicularMonitoringSizeDetials = value;
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
