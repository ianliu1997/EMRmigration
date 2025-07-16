using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;

namespace PalashDynamics.ValueObjects.User
{
   public class clsChangePasswordFirstTimeBizActionVO:IBizActionValueObject
    {
        ///// <summary>
        ///// It is long Type. Gets or sets the UserId.
        ///// </summary>
        //public long UserId { get; set; }
              
        //public string NewPassword { get; set; }
       
        //public string LoginName { get; set; }
        //public string strQuestion { get; set; }
        //public string strAnswer { get; set; }
        //private List<clsDashBoardVO> _DashBoardList;

        //public List<clsDashBoardVO> DashBoardList
        //{
        //    get
        //    {
        //        if (_DashBoardList == null)
        //            _DashBoardList = new List<clsDashBoardVO>();
        //        return _DashBoardList;
        //    }
        //    set { _DashBoardList = value; }
        //}

        //public Boolean SetPasswordChanged { get; set; }

        //private clsUserGeneralDetailVO _UserGeneralDetailVO = new clsUserGeneralDetailVO();

        //public clsUserGeneralDetailVO UserGeneralDetailVO
        //{
        //    get { return _UserGeneralDetailVO; }
        //    set { _UserGeneralDetailVO = value; }
        //}


        ///// <summary>
        ///// is Integer type. Gets or sets the Success Status of query execution.
        ///// </summary>
        //public int SuccessStatus { get; set; }

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.User.clsChangeFirstPasswordBizAction";
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
        /// 
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private clsUserVO objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains User Details Which is Added.
        /// </summary>

        public clsUserVO Details
        {
            get { return objDetails; }
            set { objDetails = value; }
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
