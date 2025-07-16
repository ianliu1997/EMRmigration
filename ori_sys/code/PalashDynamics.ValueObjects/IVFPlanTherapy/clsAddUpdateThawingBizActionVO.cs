using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
    public class clsAddUpdateThawingBizActionVO : IBizActionValueObject
    {

        #region  IBizActionValueObject

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsAddUpdateThawingBizAction";
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

        private clsThawingVO _Thawing = new clsThawingVO();
        public clsThawingVO Thawing
        {
            get
            {
                return _Thawing;
            }
            set
            {
                _Thawing = value;
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
