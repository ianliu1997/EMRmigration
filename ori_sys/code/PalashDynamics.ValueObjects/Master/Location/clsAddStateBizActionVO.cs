using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master.Location
{
    public class clsAddStateBizActionVO : IBizActionValueObject
    {
        private List<clsStateVO> objState = null;
        public List<clsStateVO> StateList
        {
            get { return objState; }
            set { objState = value; }
        }
        private clsStateVO _objStateDetail = null;
        public clsStateVO objStateDetail
        {
            get { return _objStateDetail; }
            set { _objStateDetail = value; }
        }

        private List<clsDistVO> objStateDistInfo = null;
        public List<clsDistVO> StateDistList
        {
            get { return objStateDistInfo; }
            set { objStateDistInfo = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {

            return "PalashDynamics.BusinessLayer.Master.Location.clsAddStateBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

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
    }
}
