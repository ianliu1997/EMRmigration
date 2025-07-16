using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master.DoctorMaster
{
    public class clsGetDoctorTariffServiceListBizActionVO : IBizActionValueObject
    {
        public clsGetDoctorTariffServiceListBizActionVO()
        {

        }
        public clsGetDoctorTariffServiceListBizActionVO(long ID,string description)
        {
            this.TeriffServiceDetail.ServiceID = ID;
            this.TeriffServiceDetail.ServiceName = description;
        }
        private List<clsDoctorWaiverDetailVO> _TeriffServiceDetailList = new List<clsDoctorWaiverDetailVO>();

        public List<clsDoctorWaiverDetailVO> TeriffServiceDetailList
        {
            get { return _TeriffServiceDetailList; }
            set { _TeriffServiceDetailList = value; }
        }

        private clsDoctorWaiverDetailVO _TeriffServiceDetail = new clsDoctorWaiverDetailVO();

        public clsDoctorWaiverDetailVO TeriffServiceDetail
        {
            get { return _TeriffServiceDetail; }
            set { _TeriffServiceDetail = value; }
        }


        public int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public DateTime? AppDate { get; set; }

        public long? UnitId { get; set; }

        public long? DepartmentId { get; set; }

        



        #region IBizAction Members
        /// <summary>
        /// Retuns the bizAction Class Name.
        /// </summary>
        /// <returns></returns>
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsGetDoctorTeriffServiceListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }
}
