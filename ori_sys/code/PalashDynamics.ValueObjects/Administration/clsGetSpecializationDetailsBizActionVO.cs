using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration
{
    public class clsGetSpecializationDetailsBizActionVO : IBizActionValueObject
    {
        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string SerachExpression { get; set; }

        //ROHINEE
        public long AgencyID { get; set; }
        public bool IsFromAgency = false;

        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsGetSpecializationDetailsBizAction";
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
        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }
        public long SpecializationId { get; set; }
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
