using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsGetItemConversionFactorListBizActionVO : IBizActionValueObject
    {

        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetItemConversionFactorListBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion

        public long ItemID {get;set;}

        public List<clsConversionsVO> UOMConversionList {get;set;}
        public List<MasterListItem> UOMConvertList {get;set;}
        public bool GetSavedData { get; set; }
    }


    public class clsAddUpdateItemConversionFactorListBizActionVO : IBizActionValueObject
    {

        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsAddUpdateItemConversionFactorListBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        public long ItemID {get;set;}

        public clsConversionsVO UOMConversionVO { get; set; }

        public List<clsConversionsVO> UOMConversionList {get;set;}
        public List<MasterListItem> UOMConvertList {get;set;}
        public Boolean IsForDelete { get; set; }
        public Boolean IsModify { get; set; }

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
