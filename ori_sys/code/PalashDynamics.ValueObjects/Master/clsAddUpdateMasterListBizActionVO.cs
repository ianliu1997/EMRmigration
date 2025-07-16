using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master
{
    public class clsAddUpdateMasterListBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsAddUpdateMasterListBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsMasterListVO> objItemMaster = new List<clsMasterListVO>();
        public List<clsMasterListVO> ItemMatserDetails
        {
            get
            {
                return objItemMaster;
            }
            set
            {
                objItemMaster = value;

            }
        }

        public bool IsUnitApplicable { get; set; }
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
    public class clsMasterListVO
    {
        public string MasterTableName { get; set; }
        public long Id { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public long UnitId { get; set; }
        public Boolean Status { get; set; }
        public long? AddUnitID { get; set; }
     
        public long? By { get; set; }
        public string On { get; set; }
        public DateTime? DateTime { get; set; }
        public string WindowsLoginName { get; set; }
        public Boolean IsChecked { get; set; }

        public Boolean PrimaryKeyViolationError { get; set; }
        public Boolean GeneralError { get; set; }


    }
}
