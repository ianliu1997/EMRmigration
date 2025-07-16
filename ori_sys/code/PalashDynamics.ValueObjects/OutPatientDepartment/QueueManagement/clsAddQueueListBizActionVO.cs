using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.OutPatientDepartment.QueueManagement
{
   public class clsAddQueueListBizActionVO:IBizActionValueObject
    {
        private clsQueueVO _QueueDetails;

        public clsQueueVO QueueDetails
        {
            get { return _QueueDetails; }
            set { _QueueDetails = value; }
        }


       

       private int _SuccessStatus;
       public int SuccessStatus
       {
           get { return _SuccessStatus; }
           set { _SuccessStatus = value; }
       }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.OutPatientDepartment.QueueManagement.clsAddQueueListBizAction";
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
