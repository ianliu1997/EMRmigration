using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration
{
    public class clsGetServiceBySpecializationBizActionVO:IBizActionValueObject
    {
        private List<clsServiceMasterVO> _ServiceList;
        public List<clsServiceMasterVO> ServiceList
        {
            get { return _ServiceList; }
            set { _ServiceList = value; }
        }

        private string _ServiceName;
        public string ServiceName
        {
            get { return _ServiceName; }
            set { _ServiceName = value; }

        }

        private long _Specialization;
        public long Specialization
        {
            get { return _Specialization; }
            set { _Specialization = value; }



        }

        private long _SubSpecialization;
        public long SubSpecialization
        {
            get { return _SubSpecialization; }
            set { _SubSpecialization = value; }



        }
        public clsServiceMasterVO ServiceMaster { get; set; }

        //public bool GetAllServiceListDetails { get; set; }
        //public bool GetTariffServiceMasterID { get; set; }
        //public bool GetAllTariffIDDetails { get; set; }
        //public bool GetAllServiceClassRateDetails { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsGetServiceBySpecializationBizAction";
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
