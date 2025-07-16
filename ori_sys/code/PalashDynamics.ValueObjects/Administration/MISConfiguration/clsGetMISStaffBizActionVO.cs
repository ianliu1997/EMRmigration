using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.MISConfiguration
{
    public class clsGetMISStaffBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.MISConfiguration.clsGetStaffBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
        }

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

        public long TypeId { get; set; }
        public long UnitId { get; set; }
        private List<clsGetMISStaffVO> objStaffInfo = new List<clsGetMISStaffVO>();

        public List<clsGetMISStaffVO> GetStaffInfo
        {
            get { return objStaffInfo; }
            set { objStaffInfo = value; }
        }

    }
    public class clsGetMISStaffVO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long StaffTypeId { get; set; }
        public long SelectStaffTypeId { get; set; }
        public bool Status { get; set; }
        public string EmailId { get; set; }
    }
}
