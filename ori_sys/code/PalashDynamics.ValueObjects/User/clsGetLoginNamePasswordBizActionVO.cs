using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;

namespace PalashDynamics.ValueObjects.User
{
   public class clsGetLoginNamePasswordBizActionVO:IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.User.clsGetLoginPasswordBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
        }

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

        public long ID { get; set; }
       //public string LoginNAme { get; set; }
        //public bool isPasswordChanged { get; set; }
        private clsUserVO objLoginName = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsUserVO LoginDetails
        {
            get { return objLoginName; }
            set { objLoginName = value; }
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
    }
}
