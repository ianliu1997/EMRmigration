
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;

namespace PalashDynamics.ValueObjects.Administration.RoleMaster
{
    public class clsGetRoleGeneralDetailsBizActionVO : IBizActionValueObject
    {
      

        public clsGetRoleGeneralDetailsBizActionVO()
        {

        }

        //To Identify the log Exceptions Respective to Event.
        private Guid _Guid;


        private const string _BizActionName = "PalashDynamics.BusinessLayer.clsGetRoleGeneralDetailsBizAction";
        public string BizAction
        {
            get { return _BizActionName; }
        }


        private bool _PagingEnabled;

        public bool InputPagingEnabled
        {
            get { return _PagingEnabled; }
            set { _PagingEnabled = value; }
        }

        private int _StartRowIndex = 0;

        public int InputStartRowIndex
        {
            get { return _StartRowIndex; }
            set { _StartRowIndex = value; }
        }

        private int _MaximumRows = 10;

        public int InputMaximumRows
        {
            get { return _MaximumRows; }
            set { _MaximumRows = value; }
        }

        private string _SortExpression = "Code Desc";

        private string _SearchExpression = "";
        /// <summary>
        /// Search Expression For Filtering Record By Matching Search Expression And Record Description
        /// </summary>
        public string InputSearchExpression { get { return _SearchExpression; } set { _SearchExpression = value; } }

        private bool? _Status = null;
        /// <summary>
        /// For Filtering Result List By Record Status
        /// </summary>
        public bool? Status { get { return _Status; } set { _Status = value; } }

        /// <summary>
        /// This is Input parameter.This Specifies Sort Expression For RoleDetail List
        /// </summary>
        public string InputSortExpression
        {
            get { return _SortExpression; }
            set { _SortExpression = value; }
        }

        private List<clsUserRoleVO> myVar = new List<clsUserRoleVO>();

        public List<clsUserRoleVO> RoleGeneralDetailsList
        {
            get { return myVar; }
            set { myVar = value; }
        }

        private int _TotalRows = 0;

        public int OutputTotalRows
        {
            get { return _TotalRows; }
            set { _TotalRows = value; }
        }



        #region IValueObject Members

        public  string ToXml()
        {
            return this.ToString();
        }

        #endregion

        #region IBizAction Members

        public  string GetBizAction()
        {
            return BizAction;
        }
        #endregion
    }
}
