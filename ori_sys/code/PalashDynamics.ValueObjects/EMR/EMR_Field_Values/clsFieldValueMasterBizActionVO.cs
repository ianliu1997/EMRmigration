using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.EMR.EMR_Field_Values;

namespace PalashDynamics.ValueObjects.EMR.EMR_Field_Values
{
    public class clsGetFieldValueMasterBizActionVO :IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.EMR_Field_Values.clsGetEMRFieldValueMasterBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsFieldValuesMasterVO> objFieldMaster = new List<clsFieldValuesMasterVO>();
        public List<clsFieldValuesMasterVO> objFieldMasterDetails
        {
            get
            {
                return objFieldMaster;
            }
            set
            {
                objFieldMaster = value;
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

    public class clsAddUpdateFieldValueMasterBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.EMR_Field_Values.clsAddUpdateEMRFieldValueMasterBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private clsFieldValuesMasterVO objFieldMaster = new clsFieldValuesMasterVO();
        public clsFieldValuesMasterVO objFieldMatserDetails
        {
            get
            {
                return objFieldMaster;
            }
            set
            {
                objFieldMaster = value;
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

    public class clsUpdateStatusFieldValueMasterBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.EMR_Field_Values.clsUpdateStatusEMRFieldValueMasterBizAction";
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
        private clsFieldValuesMasterVO objFieldValueStatus = null;

        /// <summary>
        /// Output Property.
        /// This Property Contains Status of the User Which is Added.
        /// </summary>
        public clsFieldValuesMasterVO FieldStatus
        {
            get { return objFieldValueStatus; }
            set { objFieldValueStatus = value; }
        }
    }

    public class clsGetUsedForMasterBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.EMR_Field_Values.clsGetUsedForMasterBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsFieldValuesMasterVO> objFieldMaster = new List<clsFieldValuesMasterVO>();
        public List<clsFieldValuesMasterVO> objFieldMasterDetails
        {
            get
            {
                return objFieldMaster;
            }
            set
            {
                objFieldMaster = value;
            }
        }
        public long ID { get; set; }
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

}
