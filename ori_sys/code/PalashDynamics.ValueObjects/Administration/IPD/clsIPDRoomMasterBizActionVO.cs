using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.IPD
{
    public class clsIPDGetRoomMasterBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.IPD.clsIPDGetRoomMasterBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsIPDRoomMasterVO> objRoomMaster = new List<clsIPDRoomMasterVO>();
        public List<clsIPDRoomMasterVO> objRoomMasterDetails
        {
            get
            {
                return objRoomMaster;
            }
            set
            {
                objRoomMaster = value;
            }
        }

        private clsIPDRoomMasterVO objRoomMasterAmmDetails = new clsIPDRoomMasterVO();
        public clsIPDRoomMasterVO objRoomMasterAmmenities
        {
            get
            {
                return objRoomMasterAmmDetails;
            }
            set
            {
                objRoomMasterAmmDetails = value;
            }
        }

        public long ID { get; set; }
        public long UnitID { get; set; }
        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string SearchExpression { get; set; }
        /// <summary>
        ///  Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        private int _SuccessStatus;
        public int SuccessStatus
        {
            get
            {
                return _SuccessStatus;
            }
            set
            {
                _SuccessStatus = value;
            }
        }
    }      

    public class clsIPDAddUpdateRoomMasterBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.IPD.clsIPDAddUpdateRoomMasterBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsIPDRoomMasterVO> objRoomMaster = new List<clsIPDRoomMasterVO>();
        public List<clsIPDRoomMasterVO> objRoomMatserDetails
        {
            get
            {
                return objRoomMaster;
            }
            set
            {
                objRoomMaster = value;
            }
        }

        /// <summary>
        ///  Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        private int _SuccessStatus;
        public int SuccessStatus
        {
            get
            {
                return _SuccessStatus;
            }
            set
            {
                _SuccessStatus = value;
            }
        }

    }

    public class clsIPDUpdateRoomStatusBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.IPD.clsIPDUpdateRoomMasterStatusBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
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

        //   public long ID { get; set; }
        private clsIPDRoomMasterVO objRoomStatus = null;

        /// <summary>
        /// Output Property.
        /// This Property Contains Status of the User Which is Added.
        /// </summary>

        public clsIPDRoomMasterVO RoomStatus
        {
            get { return objRoomStatus; }
            set { objRoomStatus = value; }
        }    
    }


}
