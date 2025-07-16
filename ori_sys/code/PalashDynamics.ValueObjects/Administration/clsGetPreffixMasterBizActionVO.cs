using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration
{
    public class clsGetPreffixMasterBizActionVO : IBizActionValueObject
    {
        
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsGetPreffixMasterBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsPreffixMasterVO> objPreffixMaster = new List<clsPreffixMasterVO>();
        public List<clsPreffixMasterVO> PreffixMasterDetails
        {
            get
            {
                return objPreffixMaster;
            }
            set
            {
                objPreffixMaster = value;

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

        private clsPreffixMasterVO _ObjpreffixMaster;
        public clsPreffixMasterVO ObjpreffixMaster
        {
            get { return _ObjpreffixMaster; }
            set { _ObjpreffixMaster = value; }
        }
        public string Code { get; set; }
        public string Description { get; set; }
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string sortExpression { get; set; }
        public long ApplicableGender { get; set; }
    }
    }

