using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.IVFPlanTherapy;

namespace PalashDynamics.ValueObjects.Patient
{
    public class clsAddUpdateHSGBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Patient.clsAddUpdateHSGBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private int _SuccessStatus;
       
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private clsHSGVO objHSG = null;
       
        public clsHSGVO HSGDetails
        {
            get { return objHSG; }
            set { objHSG = value; }
        }
    //    private clsTherapyDocumentsVO _HSGDocument = new clsTherapyDocumentsVO();
    //    public clsTherapyDocumentsVO HSGDocument
    //    {
    //        get
    //        {
    //            return _HSGDocument;
    //        }
    //        set
    //        {
    //            _HSGDocument = value;
    //        }
    //    }
    }
        public class clsGetHSGBizActionVO : IBizActionValueObject
        {
            #region  IBizActionValueObject
            public string GetBizAction()
            {
                return "PalashDynamics.BusinessLayer.Patient.clsGetHSGBizAction";
            }

            public string ToXml()
            {
                return this.ToXml();
            }


            #endregion

            private clsHSGVO objHSG = null;

            public clsHSGVO HSGDetails
            {
                get { return objHSG; }
                set { objHSG = value; }
            }

            private List<clsHSGVO> objHSGList = null;
            public List<clsHSGVO> HSGList
            {
                get { return objHSGList; }
                set { objHSGList = value; }
            }



            private int _SuccessStatus;
            public int SuccessStatus
            {
                get { return _SuccessStatus; }
                set { _SuccessStatus = value; }
            }


        
    }
}
