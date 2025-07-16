using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
    public class clsAddUpdateVitrificationBizActionVO : IBizActionValueObject
    {

        #region  IBizActionValueObject

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsAddUpdateVitrificationBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        public long CoupleID { get; set; }
        public long CoupleUintID { get; set; }
        public long ID { get; set; }
        public long UintID { get; set; }
        public long PatientID { get; set; }

        private clsGetVitrificationVO _Vitrification = new clsGetVitrificationVO();
        public clsGetVitrificationVO Vitrification
        {
            get
            {
                return _Vitrification;
            }
            set
            {
                _Vitrification = value;
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
