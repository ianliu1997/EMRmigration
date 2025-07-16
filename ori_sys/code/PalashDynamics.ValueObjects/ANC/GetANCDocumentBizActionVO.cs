using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.ANC
{
   public class GetANCDocumentBizActionVO:IBizActionValueObject
    {
         #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.ANC.GetANCDocumentBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion
        private List<clsANCDocumentsVO> _ANCDocumentList = new List<clsANCDocumentsVO>();
        public List<clsANCDocumentsVO> ANCDocumentList
        {
            get
            {
                return _ANCDocumentList;
            }
            set
            {
                _ANCDocumentList = value;
            }
        }
        private clsANCDocumentsVO _ANCDocument = new clsANCDocumentsVO();
        public clsANCDocumentsVO ANCDocument
        {
            get
            {
                return _ANCDocument;
            }
            set
            {
                _ANCDocument = value;
            }
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

