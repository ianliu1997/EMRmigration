//Created Date:24/August/2012
//Created By: Nilesh Raut
//Specification: Biz Action for Item Select by Molucule ID

//Review By:
//Review Date:

//Modified By:
//Modified Date: 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsGetItemListByMoluculeIdBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetItemListByMoluculeIdBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion

        public long MoluculeId { get; set; }
        public long UnitId { get; set; }

        private List<MasterListItem> _MasterList;
        public List<MasterListItem> MasterList
        {
            get { return _MasterList; }
            set { _MasterList = value; }
        }
        //private int _SuccessStatus;
        //public int SuccessStatus
        //{
        //    get
        //    {
        //        return _SuccessStatus;
        //    }
        //    set
        //    {
        //        _SuccessStatus = value;
        //    }
        //}
    }
}
