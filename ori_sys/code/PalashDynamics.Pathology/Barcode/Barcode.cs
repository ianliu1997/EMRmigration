using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace PalashDynamics.Pathology.Barcode
{
    public class Barcodes
    {
        public enum YesNoEnum
        {
            Yes,
            No
        }
        public enum BarcodeEnum
        {
            Code39
        }
        private string data;
        public string Data
        {
            get { return data; }
            set { data = value; }
        }
       

        private BarcodeEnum barcodeType;
        public BarcodeEnum BarcodeType
        {
            get { return barcodeType; }
            set { barcodeType = value; }
        }

        private YesNoEnum checkDigit;
        public YesNoEnum CheckDigit
        {
            get { return checkDigit; }
            set { checkDigit = value; }
        }

        private string humanText;
        public string HumanText
        {
            get
            {
                return humanText;
            }
            set { humanText = value; }
        }
        private string encodedData;
        public string EncodedData
        {
            get { return encodedData; }
            set { encodedData = value; }
        }

        public void encode()
        {
            int check = 0;
            if (checkDigit == Barcodes.YesNoEnum.Yes)
                check = 1;

            if (barcodeType == BarcodeEnum.Code39)
            {
                Code39 barcode = new Code39();
                encodedData = barcode.encode(data, check);
                humanText = barcode.getHumanText();
            }
        }
        
        
        
    }
}
