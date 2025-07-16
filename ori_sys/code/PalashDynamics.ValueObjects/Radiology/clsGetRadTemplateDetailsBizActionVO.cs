using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Radiology
{
  public  class clsGetRadTemplateDetailsBizActionVO: IBizActionValueObject
    {
        private List<clsRadiologyVO> objTemplateList = null;
        public List<clsRadiologyVO> TemplateList
        {
            get { return objTemplateList; }
            set { objTemplateList = value; }
        }
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetRadTemplateDetailsBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

  public class clsGetRadServiceBizActionVO : IBizActionValueObject
  {
      private List<MasterListItem> objList = null;
      public List<MasterListItem> MasterList
      {
          get { return objList; }
          set { objList = value; }
      }
      #region IBizActionValueObject Members

      public string GetBizAction()
      {
          return "PalashDynamics.BusinessLayer.clsGetRadServiceBizAction";
      }

      #endregion

      #region IValueObject Members

      public string ToXml()
      {
          return this.ToString();
      }

      #endregion
  }

  public class clsGetRadItemBizActionVO : IBizActionValueObject
  {
      private List<MasterListItem> objList = null;
      public List<MasterListItem> MasterList
      {
          get { return objList; }
          set { objList = value; }
      }
      #region IBizActionValueObject Members

      public string GetBizAction()
      {
          return "PalashDynamics.BusinessLayer.clsGetRadItemBizAction";
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
