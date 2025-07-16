using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Radiology
{
    public class clsAddRadTemplateMasterBizActionVO : IBizActionValueObject
    {  
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddRadTemplateMasterBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private clsRadiologyVO objDetails = null;
        public clsRadiologyVO TemplateDetails
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

        private long _SuccessStatus;
        public long SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private List<MasterListItem> _GenderList = new List<MasterListItem>();
        public List<MasterListItem> GenderList
        {
            get
            {
                return _GenderList;
            }
            set
            {
                _GenderList = value;
            }
        }


    }


}
