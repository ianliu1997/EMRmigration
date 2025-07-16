using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.User
{
  public class clsGetUserGeneralDetailsBizActionVO:IBizActionValueObject
    {

      public clsGetUserGeneralDetailsBizActionVO()
      {

      }

      private Guid _Guid;

      private bool _PagingEnabled;

      public bool InputPagingEnabled
      {
          get { return _PagingEnabled; }
          set { _PagingEnabled = value; }
      }

      private int _StartRowIndex = 0;

      public int InputStartRowIndex
      {
          get { return _StartRowIndex; }
          set { _StartRowIndex = value; }
      }

      private int _MaximumRows = 0;

      public int InputMaximumRows
      {
          get { return _MaximumRows; }
          set { _MaximumRows = value; }
      }

      private string _ShortExpression = "Code Desc";
     // private string _ShortExpression = "";
      /// <summary>
      /// Search Expression For Filtering Record By Matching Search Expression And Record Description
      /// </summary>
      /// 
        public string GetBizAction()
        {
            throw new NotImplementedException();
        }

        #region IValueObject Members.
        public string ToXml()
        {
            return this.ToString();
        }
        #endregion

    }
}
