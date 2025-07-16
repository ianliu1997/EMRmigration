using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration
{
   public class clsDeleteTariffServiceAndServiceTariffBizActionVO:IBizActionValueObject
    {

       public clsDeleteTariffServiceAndServiceTariffBizActionVO()
        {

        }

        private clsServiceMasterVO _objServiceMasterDetails = null;
        public clsServiceMasterVO ServiceMasterDetails
        {
            get { return _objServiceMasterDetails; }
            set { _objServiceMasterDetails = value; }

        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }


        private long _ServiceID;
        public long ServiceID
        {
            get { return _ServiceID; }
            set { _ServiceID = value; }
        }
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsDeleteTariffServiceAndServiceTariffBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }


        public List<long> TariffList { get; set; }

        public List<MasterListItem> Tariffs { get; set; }
     
        public string Query { get; set; }
        #endregion
    }
}
