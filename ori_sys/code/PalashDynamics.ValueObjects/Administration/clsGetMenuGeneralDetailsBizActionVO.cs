using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.Menu
{
    public class clsGetMenuGeneralDetailsBizActionVO:IBizActionValueObject 
    {
        //To Identify the log Exceptions Respective to Event.
        private Guid _Guid;


        private const string _BizActionName = "PalashDynamics.BusinessLayer.clsGetMenuGeneralDetailsBizAction";
        public string BizAction
        {
            get { return _BizActionName; }
        }
        private List<clsMenuVO> myVar = null;

        public List<clsMenuVO> MenuList
        {
            get { return myVar; }
            set { myVar = value; }
        }
        private bool? _Status = null;
        /// <summary>
        /// For Filtering Result List By Record Status
        /// </summary>
        public bool? Status { get { return _Status; } set { _Status = value; } }
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
        private string _SortExpression = "MenuId Asc";
        private string _SearchExpression = "";
        /// <summary>
        /// Search Expression For Filtering Record By Matching Search Expression And Record Description
        /// </summary>
        public string InputSearchExpression { get { return _SearchExpression; } set { _SearchExpression = value; } }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return BizAction;
        }
        public Guid GetBizActionGuid()
        {
            return _Guid;
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
