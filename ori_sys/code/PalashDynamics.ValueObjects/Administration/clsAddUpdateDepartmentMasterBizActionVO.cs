using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration
{
    public class clsAddUpdateDepartmentMasterBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsAddUpdateDepartmentMasterBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsDepartmentVO> objItemMaster = new List<clsDepartmentVO>();
        public List<clsDepartmentVO> ItemMatserDetails
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

        private clsDepartmentVO _objSpecializationDetails = new clsDepartmentVO();
        public clsDepartmentVO objSpecializationDetails
        {
            get
            {
                return _objSpecializationDetails;
            }
            set
            {
                _objSpecializationDetails = value;

            }
        }

        public bool IsUpdate { get; set; }

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
    public class clsDepartmentVO
    {
        public string MasterTableName { get; set; }
        public long Id { get; set; }
        public Boolean IsClinical { get; set; }
        public string ClinicalStatus { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public long UnitId { get; set; }
        public Boolean Status { get; set; }
        public long? AddUnitID { get; set; }

        public long? By { get; set; }
        public string On { get; set; }
        public DateTime? DateTime { get; set; }
        public string WindowsLoginName { get; set; }


        public Boolean PrimaryKeyViolationError { get; set; }
        public Boolean GeneralError { get; set; }


        private List<clsDeptSpecilizationLinkingVO> ObjList = new List<clsDeptSpecilizationLinkingVO>();
        public List<clsDeptSpecilizationLinkingVO> SpecList
        {
            get
            {
                return ObjList;
            }
            set
            {
                ObjList = value;

            }
        }

        private List<clsSubSpecializationVO> _SpecilizationList = new List<clsSubSpecializationVO>();
        public List<clsSubSpecializationVO> SpecilizationList
        {
            get
            {
                return _SpecilizationList;
            }
            set
            {
                _SpecilizationList = value;

            }
        }

    }

    // Added For IPD by CDS

    public class clsDeptSpecilizationLinkingVO
    {
        public long Id { get; set; }
        public long UnitId { get; set; }
        public long DeptId { get; set; }
        public long SpecilizationId { get; set; }
        public bool Status { get; set; }
        public string Description { get; set; }

    }
}
