using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.OutPatientDepartment.QueueManagement
{
    public  class clsUpdatePatientSortOrderBizActionVO:IBizActionValueObject
    {
        public clsQueueVO QueueDetails { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.OutPatientDepartment.QueueManagement.clsUpdatePatientSortOrderBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }


    public class clsUpdateDoctorInQueueBizActionVO : IBizActionValueObject
    {
        private List<clsPackageServiceDetailsVO> objDetails = new List<clsPackageServiceDetailsVO>();

        public List<clsPackageServiceDetailsVO> QueueDetails
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

       
        public long VisitId;
        public long DoctorID;

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.OutPatientDepartment.QueueManagement.clsUpdateDoctorInQueueBizAction";
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
