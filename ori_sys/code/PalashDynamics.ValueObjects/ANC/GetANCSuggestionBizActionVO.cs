using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.ANC
{
 public   class GetANCSuggestionBizActionVO:IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.ANC.GetANCSuggestionBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion
       
        private clsANCSuggestionVO _ANCSuggestion = new clsANCSuggestionVO();
        public clsANCSuggestionVO ANCSuggestion
        {
            get
            {
                return _ANCSuggestion;
            }
            set
            {
                _ANCSuggestion = value;
            }
        }
        private List<clsANCSuggestionVO> _ANCSuggestionList = null;
        public List<clsANCSuggestionVO> ANCSuggestionList
        {
            get
            {
                return _ANCSuggestionList;
            }
            set
            {
                _ANCSuggestionList = value;
            }
        }
      
        private List<MasterListItem> objConsultList = null;
        public List<MasterListItem> ConsultList
        {
            get { return objConsultList; }
            set { objConsultList = value; }
        }

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
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

    }
}

