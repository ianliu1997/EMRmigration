using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master
{
    public class clsGetCounterDetailsBizActionVO : IBizActionValueObject
    {
        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }

        public long ClinicID { get; set; }
        //public clsCounterVO CounterDetails
        //{
        //    get { return CounterDetails; }
        //    set { CounterDetails = value; }
        //}

        private List<clsCounterVO> _CounterDetails;

        public List<clsCounterVO> CounterDetails
        {
            get { return _CounterDetails; }
            set { _CounterDetails = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsGetCounterDetailsBizAction";
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
