using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.ANC
{
   public class AddUpdateANCSuggestionBizActionVO:IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.ANC.AddUpdateANCSuggestionBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion
        public List<MasterListItem> ConsultList { get; set; }
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

        public long TabID { get; set; }

        public bool IsUpdate { get; set; }
    }
}

