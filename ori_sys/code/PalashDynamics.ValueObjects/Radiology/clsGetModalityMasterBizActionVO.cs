using System.Collections.Generic;
namespace PalashDynamics.ValueObjects.Radiology
{
    public class clsGetModalityMasterBizActionVO : IBizActionValueObject
    {

        private List<clsModalityMasterVO> objTemplateList = null;
        public List<clsModalityMasterVO> ModalityMasterList
        {
            get { return objTemplateList; }
            set { objTemplateList = value; }
        }
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Radiology.clsGetModalityMasterBizAction";  //BL
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public clsModalityMasterVO modalityObj { get; set; }

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
        public string SortExpression { get; set; }
    }
    public class clsGetModalityMasterByIDBizActionVO : IBizActionValueObject
    {
        private clsModalityMasterVO objDetails = null;
        public clsModalityMasterVO ModalityMasterDetails
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

        private long _ID;
        public long ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private long _Status;
        public long Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        public int TotalRows { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Radiology.clsGetModalityMasterByIDBizAction";
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
