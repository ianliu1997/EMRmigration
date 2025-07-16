using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.OTConfiguration
{
    #region BizActionVO  Added By Kiran for ADDUpdate,GetList,UpdateStatus ProcedureCheckList
    public class clsAddUpdateProcedureCheckListBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.OTConfiguration.clsAddUpdateProcedureCheckListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
       
        private clsProcedureCheckListVO myVar = new clsProcedureCheckListVO();
        public clsProcedureCheckListVO CheckListDetails
        {
            get { return myVar; }
            set { myVar = value; }
        }


        private List<clsProcedureCheckListVO> _CheckList;
        public List<clsProcedureCheckListVO> CheckList
        {
            get { return _CheckList; }
            set { _CheckList = value; }
        }

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string sortExpression { get; set; }

    }

    public class clsGetProcedureCheckListBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.OTConfiguration.clsGetProcedureCheckListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
        private clsProcedureCheckListVO myVar = new clsProcedureCheckListVO();
        public clsProcedureCheckListVO CheckListDetails
        {
            get { return myVar; }
            set { myVar = value; }
        }


        private List<clsProcedureCheckListVO> _CheckList;
        public List<clsProcedureCheckListVO> CheckList
        {
            get { return _CheckList; }
            set { _CheckList = value; }
        }

        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string SearchExpression { get; set; }


    }

    public class clsUpdateStatusProcedureCheckListBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.OTConfiguration.clsUpdateStatusProcedureCheckListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
        private clsProcedureCheckListVO myVar = new clsProcedureCheckListVO();
        public clsProcedureCheckListVO CheckListDetails
        {
            get { return myVar; }
            set { myVar = value; }
        }


        private List<clsProcedureCheckListVO> _CheckList;
        public List<clsProcedureCheckListVO> CheckList
        {
            get { return _CheckList; }
            set { _CheckList = value; }
        }

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string sortExpression { get; set; }

    }

    # endregion
}
