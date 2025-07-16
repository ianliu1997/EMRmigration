using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
    public class clsUpdateFollicularMonitoringBizActionVO : IBizActionValueObject
    {

        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsUpdateFollicularMonitoringBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion


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

    public class clsGetFollicularModifiedDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetFollicularModifiedDetailsBizAction";
        }
        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion

        public long FollicularID { get; set; }
        public long TherapyID { get; set; }
        public bool IsModified { get; set; }

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
