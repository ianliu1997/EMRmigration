using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Patient
{
    public class AddBarcodeImageBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Patient.AddBarcodeImageBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private AddBarcodeImageVO _ObjBarcodeImage;
        public AddBarcodeImageVO ObjBarcodeImage
        {
            get { return _ObjBarcodeImage; }
            set { _ObjBarcodeImage = value; }
        }
        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }
    }
}
