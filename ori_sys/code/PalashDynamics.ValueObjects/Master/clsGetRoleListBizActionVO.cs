using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Administration;

namespace PalashDynamics.ValueObjects.Master
{
    public class clsGetRoleListBizActionVO:IBizActionValueObject 
    {
         
        //To Identify the log Exceptions Respective to Event.
        private Guid _Guid = Guid.NewGuid();

        private const string _BizActionName = "PalashDynamics.BusinessLayer.clsGetRoleListBizAction";


        private long lngRoleId;
        /// <summary>
        /// Set And Get Property For  RoleId Of Object
        /// </summary>
        public long RoleId
        {
            get { return lngRoleId; }
            set { lngRoleId = value; }
        }

        private bool? _Status = null;

        /// <summary>
        /// Input Parameters.
        /// Get And Set Property for StatusEnum
        /// </summary>
        public bool? Status { get { return _Status; } set { _Status = value; } }

        private string _SearchExpression = "";
        /// <summary>
        /// Search Expression For Filtering Record By Matching Search Expression And Record Description
        /// </summary>
        public string SearchExpression { get { return _SearchExpression; } set { _SearchExpression = value; } }


        List<MasterListItem> _RoleList = new List<MasterListItem>();

        /// <summary>
        /// Output Property.
        /// Get Property To Access And Modify RoleList
        /// </summary>
        public List<MasterListItem> RoleList { 
            get { return _RoleList; }
            set { _RoleList = value ; }

        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return _BizActionName;
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
