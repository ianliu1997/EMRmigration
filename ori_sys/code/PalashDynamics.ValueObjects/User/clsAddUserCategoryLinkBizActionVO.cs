using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.User
{
    public class clsAddUserCategoryLinkBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.User.clsAddUserCategoryLinkBizAction";
        }

        public string ToXml()
        {

            return this.ToString();
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private clsUserCategoryLinkVO _UserCategoryLinkDetails;
        public clsUserCategoryLinkVO UserCategoryLinkDetails
        {
            get { return _UserCategoryLinkDetails; }
            set { _UserCategoryLinkDetails = value; }
        }
        private List<clsUserCategoryLinkVO> _UserCategoryLinkList = new List<clsUserCategoryLinkVO>();
        public List<clsUserCategoryLinkVO> UserCategoryLinkList
        {
            get { return _UserCategoryLinkList; }
            set { _UserCategoryLinkList = value; }
        }
        public bool IsStatusChanged { get; set; }
        public bool IsModify { get; set; }
    }

    public class clsGetCategoryListBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.User.clsGetCategoryListBizAction";
        }

        public string ToXml()
        {

            return this.ToString();
        }
        public int TotalRows { get; set; }
        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public bool PagingEnabled { get; set; }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private clsUserCategoryLinkVO _CategoryListDetails;
        public clsUserCategoryLinkVO CategoryListDetails
        {
            get { return _CategoryListDetails; }
            set { _CategoryListDetails = value; }
        }
        private List<clsUserCategoryLinkVO> _CategoryList = new List<clsUserCategoryLinkVO>();
        public List<clsUserCategoryLinkVO> CategoryList
        {
            get { return _CategoryList; }
            set { _CategoryList = value; }
        }
    }

    public class clsGetExistingCategoryListBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.User.clsGetExistingCategoryListBizAction";
        }

        public string ToXml()
        {

            return this.ToString();
        }
        public int TotalRows { get; set; }
        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public bool PagingEnabled { get; set; }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private clsUserCategoryLinkVO _ExistingCategoryListDetails;
        public clsUserCategoryLinkVO ExistingCategoryListDetails
        {
            get { return _ExistingCategoryListDetails; }
            set { _ExistingCategoryListDetails = value; }
        }
        private List<clsUserCategoryLinkVO> _ExistingCategoryList = new List<clsUserCategoryLinkVO>();
        public List<clsUserCategoryLinkVO> ExistingCategoryList
        {
            get { return _ExistingCategoryList; }
            set { _ExistingCategoryList = value; }
        }
    }
}
