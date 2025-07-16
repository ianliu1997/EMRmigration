using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration
{
    public class clsAddUpdateSpecializationBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsAddUpdateSpecializationBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsSpecializationVO> objItemMaster = new List<clsSpecializationVO>();
        public List<clsSpecializationVO> ItemMatserDetails
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

    public class clsSpecializationVO
    {
        public long SpecializationId { get; set; }
        public string SpecializationName { get; set; }
        public string Code { get; set; }
        public long ClinicId { get; set; }
        public Boolean Status { get; set; }
        public long? CreatedUnitID { get; set; }
        public long? UpdatedUnitID { get; set; }
        public long? AddedBy { get; set; }
        public string AddedOn { get; set; }
        public DateTime? AddedDateTime { get; set; }
        public long? UpdatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public string AddedWindowsLoginName { get; set; }
        public string UpdateWindowsLoginName { get; set; }
        public long AgencyID { get; set; }
        public Boolean PrimaryKeyViolationError { get; set; }
        public Boolean GeneralError { get; set; }
        public Boolean IsGenerateToken { get; set; }
        public long SubSpeID { get; set; }
    }
}
