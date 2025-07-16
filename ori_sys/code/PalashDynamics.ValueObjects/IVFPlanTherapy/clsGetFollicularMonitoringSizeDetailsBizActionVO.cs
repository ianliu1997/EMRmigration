using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
    public class clsGetFollicularMonitoringSizeDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetFollicularMonitoringSizeDetailsBizAction";
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

    

}
