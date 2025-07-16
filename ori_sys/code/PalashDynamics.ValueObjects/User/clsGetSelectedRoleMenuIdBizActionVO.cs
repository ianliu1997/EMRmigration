using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using PalashDynamics.ValueObjects.Administration.RoleMaster;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Administration.Menu;
using PalashDynamics.ValueObjects.Master;

namespace PalashDynamics.ValueObjects.Administration.RoleMaster
{
    
    public class clsGetSelectedRoleMenuIdBizActionVO : IBizActionValueObject
    {
        //public clsGetSelectedRoleMenuIdBizActionVO(long RoleId)
        //{
        //    this._RoleId = RoleId;
        //}

        public clsGetSelectedRoleMenuIdBizActionVO()
        {

        }

        private const string _BizActionName = "PalashDynamics.BusinessLayer.clsGetSelectedRoleMenuIdBizAction";
        public string BizAction
        {
            get { return _BizActionName; }
        }


        private long _RoleId = 0;
        public long RoleId { get { return _RoleId; } set { _RoleId = value; } }

        private List<clsMenuVO> _MenuList;

        public List<clsMenuVO> MenuList
        {
            get
            {
                if (_MenuList == null)
                    _MenuList = new List<clsMenuVO>();
                return _MenuList;
            }
            set { _MenuList = value; }
        }


        private List<clsDashBoardVO> _DashBoardList;

        public List<clsDashBoardVO> DashBoardList
        {
            get
            {
                if (_DashBoardList == null)
                    _DashBoardList = new List<clsDashBoardVO>();
                return _DashBoardList;
            }
            set { _DashBoardList = value; }
        }

    
        #region IValueObject Members

        public string ToXml()
        {
            //string messge = "";
            //XmlSerializer Xs = new XmlSerializer(typeof(clsGetSelectedRoleMenuIdBizActionVO));
            //StringWriter op = new StringWriter(new StringBuilder());
            //Xs.Serialize(op, this);
            //messge = op.ToString();
            //return messge;
            return this.ToString();
        }

        #endregion

        #region IBizAction Members

        public string GetBizAction()
        {
            return BizAction;
        }

         //To Identify the Logs respective to bizAction
        private Guid _Guid = Guid.NewGuid();
        public Guid GetBizActionGuid()
        {
            return _Guid;
        }

        #endregion
    }
}
