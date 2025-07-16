/* Added by SUDHIR PATIL */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.IPD
{

    public class clsIPDGetBedCensusAndNonCensusListBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IPD.clsIPDGetBedCensusAndNonCensusListBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsIPDBedMasterVO> objBedMaster = new List<clsIPDBedMasterVO>();
        public List<clsIPDBedMasterVO> objBedMasterDetails
        {
            get
            {
                return objBedMaster;
            }
            set
            {
                objBedMaster = value;
            }
        }

        private clsIPDBedMasterVO _objBedDetails = new clsIPDBedMasterVO();
        public clsIPDBedMasterVO BedDetails
        {
            get
            {
                return _objBedDetails;
            }
            set
            {
                _objBedDetails = value;
            }
        }
        public long ID { get; set; }
        public bool Status { get; set; }
        public long UnitID { get; set; }
        public long ClassID { get; set; }
        public long BedID { get; set; }
        public string BedName { get; set; }
        public string WardName { get; set; }
        public string ClassName { get; set; }
        public long WardID { get; set; }
        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string SearchExpression { get; set; }
        public bool IsNonCensus { get; set; }
        public bool IsBedDetails { get; set; }
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
