using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.CRM
{
   public  class clsGetCampMasterListBizActionVO:IBizActionValueObject
    {
       private List<clsCampMasterVO> _CampMasterList;
       public List<clsCampMasterVO> CampMasterList
       {
           get { return _CampMasterList; }
           set { _CampMasterList = value; }
       }

       private string _Description;
       public string Description
       {
           get { return _Description; }
           set { _Description = value; }
       }

       private string _SearchExpression;
       public string SearchExpression
       {
           get { return _SearchExpression; }
           set { _SearchExpression = value; }
       }


       public int TotalRows { get; set; }
       public int StartIndex { get; set; }
       public int MaximumRows { get; set; }
       public bool IsPagingEnabled { get; set; }

       public string sortExpression { get; set; }



        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.CRM.clsGetCampMasterListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
