using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.OTConfiguration
{
    public class clsAddUpdateProcedureSubCategoryBizActionVO: IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.OTConfiguration.clsAddUpdateProcedureSubCategoryBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
        private clsProcedureSubCategoryVO myVar = new clsProcedureSubCategoryVO();
        public clsProcedureSubCategoryVO SubCategoryDetails
        {
            get { return myVar; }
            set { myVar = value; }
        }


        private List<clsProcedureSubCategoryVO> _SubCategoryList;
        public List<clsProcedureSubCategoryVO> SubCategoryList
        {
            get { return _SubCategoryList; }
            set { _SubCategoryList = value; }
        }
        
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string sortExpression { get; set; }

       }

    public class clsGetProcedureSubCategoryListBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.OTConfiguration.clsGetProcedureSubCategoryListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
        private clsProcedureSubCategoryVO myVar = new clsProcedureSubCategoryVO();
        public clsProcedureSubCategoryVO SubCategoryDetails
        {
            get { return myVar; }
            set { myVar = value; }
        }


        private List<clsProcedureSubCategoryVO> _SubCategoryList;
        public List<clsProcedureSubCategoryVO> SubCategoryList
        {
            get { return _SubCategoryList; }
            set { _SubCategoryList = value; }
        }

        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string SearchExpression { get; set; }


    }

    public class clsUpdateStatusProcedureSubCategoryBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.OTConfiguration.clsUpdateStatusProcedureSubCategoryBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
        private clsProcedureSubCategoryVO myVar = new clsProcedureSubCategoryVO();
        public clsProcedureSubCategoryVO SubCategoryDetails
        {
            get { return myVar; }
            set { myVar = value; }
        }


        private List<clsProcedureSubCategoryVO> _SubCategoryList;
        public List<clsProcedureSubCategoryVO> SubCategoryList
        {
            get { return _SubCategoryList; }
            set { _SubCategoryList = value; }
        }

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string sortExpression { get; set; }

    }
}
