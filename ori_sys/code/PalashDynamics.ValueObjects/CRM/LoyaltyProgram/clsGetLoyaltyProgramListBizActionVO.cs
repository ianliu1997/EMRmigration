using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.CRM
{
   public class clsGetLoyaltyProgramListBizActionVO:IBizActionValueObject
    {
       private List<clsLoyaltyProgramVO> _LoyaltyProgramList;
       public List<clsLoyaltyProgramVO> LoyaltyProgramList
        {
            get { return _LoyaltyProgramList; }
            set { _LoyaltyProgramList = value; }
        }

       public string LoyaltyProgramName{ get; set;}

       public DateTime? FromDate { get; set; }
       public DateTime? ToDate { get; set; }

       public int TotalRows { get; set; }
       public int StartIndex { get; set; }
       public int MaximumRows { get; set; }
       public bool IsPagingEnabled { get; set; }

       public string sortExpression { get; set; }


        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.CRM.clsGetLoyaltyProgramListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }
}
