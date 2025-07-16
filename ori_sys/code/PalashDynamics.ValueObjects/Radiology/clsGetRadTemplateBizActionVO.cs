using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Radiology
{
   public class clsGetRadTemplateBizActionVO : IBizActionValueObject
   {
       private List<clsRadiologyVO> objTemplateList = null;
       public List<clsRadiologyVO> TemplateList
       {
           get { return objTemplateList; }
           set { objTemplateList = value; }
       }

       public string Description { get; set; }
       public long Radiologist { get; set; }
       public long GenderID { get; set; }
       public long TemplateResultID { get; set; }

       public string RadiologistName { get; set; }




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
            return "PalashDynamics.BusinessLayer.clsGetRadTemplateBizAction";
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
