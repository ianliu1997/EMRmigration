using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.ANC
{
   public class clsGetANCExaminationBizActionVO:IBizActionValueObject
    {
         #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.ANC.clsGetANCExaminationBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion
        private List<clsANCExaminationDetailsVO> _ANCExaminationList = new List<clsANCExaminationDetailsVO>();
        public List<clsANCExaminationDetailsVO> ANCExaminationList
        {
            get
            {
                return _ANCExaminationList;
            }
            set
            {
                _ANCExaminationList = value;
            }
        }
        private clsANCExaminationDetailsVO _ANCExaminationDetails = new clsANCExaminationDetailsVO();
        public clsANCExaminationDetailsVO ANCExaminationDetails
        {
            get
            {
                return _ANCExaminationDetails;
            }
            set
            {
                _ANCExaminationDetails = value;
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

