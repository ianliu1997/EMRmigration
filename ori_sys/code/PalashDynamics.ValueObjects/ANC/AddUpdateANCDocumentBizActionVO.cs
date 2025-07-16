using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.ANC
{
    public class AddUpdateANCDocumentBizActionVO:IBizActionValueObject
    {
          #region  IBizActionValueObject
            public string GetBizAction()
            {
                return "PalashDynamics.BusinessLayer.ANC.AddUpdateANCDocumentBizAction";
            }

            public string ToXml()
            {
                return this.ToXml();
            }
            #endregion

            private clsANCDocumentsVO _ANCDocuments = new clsANCDocumentsVO();
            public clsANCDocumentsVO ANCDocuments
            {
                get
                {
                    return _ANCDocuments;
                }
                set
                {
                    _ANCDocuments = value;
                }
            }

            private List<clsANCDocumentsVO> _ANCDocumentsList = new List<clsANCDocumentsVO>();
            public List<clsANCDocumentsVO> ANCDocumentsList
            {
                get
                {
                    return _ANCDocumentsList;
                }
                set
                {
                    _ANCDocumentsList = value;
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

