using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Administration.StaffMaster;

namespace PalashDynamics.ValueObjects.IPD
{
    
   public class clsGetUnitWiseEmpBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IPD.clsGetUnitWiseEmpBizAction";
        }

        public string ToXml()
        {

            return this.ToString();
        }

        private List<clsStaffMasterVO> myVar = new List<clsStaffMasterVO>();
        public List<clsStaffMasterVO> StaffMasterList
        {
            get { return myVar; }
            set { myVar = value; }
        }

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        /// 

        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }

        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private List<clsIPDVitalSDetailsVO> _GetUnitWiseEmpList;
        public List<clsIPDVitalSDetailsVO> GetUnitWiseEmpList
        {
            get { return _GetUnitWiseEmpList; }
            set { _GetUnitWiseEmpList = value; }
        }

        private clsIPDVitalSDetailsVO _GetEmpDetails;
        public clsIPDVitalSDetailsVO GetEmpDetails
        {
            get { return _GetEmpDetails; }
            set { _GetEmpDetails = value; }
        }

        public long StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public int TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string sortExpression { get; set; }
        public int StartIndex { get; set; }
      

        public long UnitID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

      
    }

   public class clsAddVitalSDetailsBizActionVO : IBizActionValueObject
   {
       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.IPD.clsAddVitalSDetailsBizAction";
       }

       public string ToXml()
       {

           return this.ToString();
       }

     
       private int _SuccessStatus;
       /// <summary>
       /// Output Property.
       /// This property states the outcome of BizAction Process.
       /// </summary>
       /// 

       private List<MasterListItem> _MasterList = null;
       public List<MasterListItem> MasterList
       {
           get
           { return _MasterList; }

           set
           { _MasterList = value; }
       }

       public int SuccessStatus
       {
           get { return _SuccessStatus; }
           set { _SuccessStatus = value; }
       }

       private List<clsIPDVitalSDetailsVO> _AddVitalDetailsList;
       public List<clsIPDVitalSDetailsVO> AddVitalDetailsList
       {
           get { return _AddVitalDetailsList; }
           set { _AddVitalDetailsList = value; }
       }

       private clsIPDVitalSDetailsVO _AddVitalDetails;
       public clsIPDVitalSDetailsVO AddVitalDetails
       {
           get { return _AddVitalDetails; }
           set { _AddVitalDetails = value; }
       }

       public long StartRowIndex { get; set; }
       public int MaximumRows { get; set; }
       public int TotalRows { get; set; }
       public bool PagingEnabled { get; set; }
       public bool IsPagingEnabled { get; set; }
       public string sortExpression { get; set; }
       public int StartIndex { get; set; }

   }

   public class clsGetVitalSDetailsListByAdmIDAdmUnitIDDateBizActionVO : IBizActionValueObject
   {
       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.IPD.clsGetVitalSDetailsListByAdmIDAdmUnitIDDateBizAction";
       }

       public string ToXml()
       {

           return this.ToString();
       }


       private int _SuccessStatus;
       /// <summary>
       /// Output Property.
       /// This property states the outcome of BizAction Process.
       /// </summary>
       /// 

       private List<MasterListItem> _MasterList = null;
       public List<MasterListItem> MasterList
       {
           get
           { return _MasterList; }

           set
           { _MasterList = value; }
       }

       public int SuccessStatus
       {
           get { return _SuccessStatus; }
           set { _SuccessStatus = value; }
       }

       private List<clsIPDVitalSDetailsVO> _GetVitalSDetailsList;
       public List<clsIPDVitalSDetailsVO> GetVitalSDetailsList
       {
           get { return _GetVitalSDetailsList; }
           set { _GetVitalSDetailsList = value; }
       }

       private clsIPDVitalSDetailsVO _GetVitalSDetails;
       public clsIPDVitalSDetailsVO GetVitalSDetails
       {
           get { return _GetVitalSDetails; }
           set { _GetVitalSDetails = value; }
       }

       public long StartRowIndex { get; set; }
       public int MaximumRows { get; set; }
       public int TotalRows { get; set; }
       public bool PagingEnabled { get; set; }
       public bool IsPagingEnabled { get; set; }
       public string sortExpression { get; set; }
       public int StartIndex { get; set; }

   }

   public class clsGetVitalSDetailsListBizActionVO : IBizActionValueObject
   {
       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.IPD.clsGetVitalSDetailsListBizAction";
       }

       public string ToXml()
       {

           return this.ToString();
       }


       private int _SuccessStatus;
       /// <summary>
       /// Output Property.
       /// This property states the outcome of BizAction Process.
       /// </summary>
       /// 

       private List<MasterListItem> _MasterList = null;
       public List<MasterListItem> MasterList
       {
           get
           { return _MasterList; }

           set
           { _MasterList = value; }
       }

       public int SuccessStatus
       {
           get { return _SuccessStatus; }
           set { _SuccessStatus = value; }
       }

       private bool _IsListOfVitalDetails = false;
       public bool IsListOfVitalDetails
       {
           get { return _IsListOfVitalDetails; }
           set { _IsListOfVitalDetails = value; }
       }

       private List<clsIPDVitalSDetailsVO> _GetVitalSDetailsList;
       public List<clsIPDVitalSDetailsVO> GetVitalSDetailsList
       {
           get { return _GetVitalSDetailsList; }
           set { _GetVitalSDetailsList = value; }
       }

       private List<clsIPDVitalSDetailsVO> _vitalList;
       public List<clsIPDVitalSDetailsVO> vitalList
       {
           get { return _vitalList; }
           set { _vitalList = value; }
       }

       private clsIPDVitalSDetailsVO _GetVitalSDetails;
       public clsIPDVitalSDetailsVO GetVitalSDetails
       {
           get { return _GetVitalSDetails; }
           set { _GetVitalSDetails = value; }
       }

       public long StartRowIndex { get; set; }
       public int MaximumRows { get; set; }
       public int TotalRows { get; set; }
       public bool PagingEnabled { get; set; }
       public bool IsPagingEnabled { get; set; }
       public string sortExpression { get; set; }
       public int StartIndex { get; set; }

   }

   public class clsGetListofVitalSDetailsBizActionVO : IBizActionValueObject
   {
       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.IPD.clsGetListofVitalSDetailsBizAction";
       }

       public string ToXml()
       {

           return this.ToString();
       }


       private int _SuccessStatus;
       /// <summary>
       /// Output Property.
       /// This property states the outcome of BizAction Process.
       /// </summary>
       /// 

       private List<MasterListItem> _MasterList = null;
       public List<MasterListItem> MasterList
       {
           get
           { return _MasterList; }

           set
           { _MasterList = value; }
       }

       public int SuccessStatus
       {
           get { return _SuccessStatus; }
           set { _SuccessStatus = value; }
       }

       private List<clsIPDVitalSDetailsVO> _GetListofVitalSDetails;
       public List<clsIPDVitalSDetailsVO> GetListofVitalSDetails
       {
           get { return _GetListofVitalSDetails; }
           set { _GetListofVitalSDetails = value; }
       }

       private clsIPDVitalSDetailsVO _GetVitalSDetails;
       public clsIPDVitalSDetailsVO GetVitalSDetails
       {
           get { return _GetVitalSDetails; }
           set { _GetVitalSDetails = value; }
       }

       public long StartRowIndex { get; set; }
       public int MaximumRows { get; set; }
       public int TotalRows { get; set; }
       public bool PagingEnabled { get; set; }
       public bool IsPagingEnabled { get; set; }
       public string sortExpression { get; set; }
       public int StartIndex { get; set; }

   }

   public class clsUpdateStatusVitalDetailsBizActionVO : IBizActionValueObject
   {
       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.IPD.clsUpdateStatusVitalDetailsBizAction";
       }

       public string ToXml()
       {

           return this.ToString();
       }


       private int _SuccessStatus;
       /// <summary>
       /// Output Property.
       /// This property states the outcome of BizAction Process.
       /// </summary>
       /// 

       private List<MasterListItem> _MasterList = null;
       public List<MasterListItem> MasterList
       {
           get
           { return _MasterList; }

           set
           { _MasterList = value; }
       }

       public int SuccessStatus
       {
           get { return _SuccessStatus; }
           set { _SuccessStatus = value; }
       }

       private List<clsIPDVitalSDetailsVO> _GetListofVitalSDetails;
       public List<clsIPDVitalSDetailsVO> GetListofVitalSDetails
       {
           get { return _GetListofVitalSDetails; }
           set { _GetListofVitalSDetails = value; }
       }

       private clsIPDVitalSDetailsVO _GetVitalSDetails;
       public clsIPDVitalSDetailsVO GetVitalSDetails
       {
           get { return _GetVitalSDetails; }
           set { _GetVitalSDetails = value; }
       }

       public long StartRowIndex { get; set; }
       public int MaximumRows { get; set; }
       public int TotalRows { get; set; }
       public bool PagingEnabled { get; set; }
       public bool IsPagingEnabled { get; set; }
       public string sortExpression { get; set; }
       public int StartIndex { get; set; }

   }

   public class clsGetGraphDetailsBizActionVO : IBizActionValueObject
   {
       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.IPD.clsGetGraphDetailsBizAction";
       }

       public string ToXml()
       {

           return this.ToString();
       }


       private int _SuccessStatus;
       /// <summary>
       /// Output Property.
       /// This property states the outcome of BizAction Process.
       /// </summary>
       /// 

       private List<MasterListItem> _MasterList = null;
       public List<MasterListItem> MasterList
       {
           get
           { return _MasterList; }

           set
           { _MasterList = value; }
       }

       public int SuccessStatus
       {
           get { return _SuccessStatus; }
           set { _SuccessStatus = value; }
       }

       private List<clsIPDVitalSDetailsVO> _GetGraphDetailsList;
       public List<clsIPDVitalSDetailsVO> GetGraphDetailsList
       {
           get { return _GetGraphDetailsList; }
           set { _GetGraphDetailsList = value; }
       }

       private clsIPDVitalSDetailsVO _GetGraphDetails;
       public clsIPDVitalSDetailsVO GetGraphDetails
       {
           get { return _GetGraphDetails; }
           set { _GetGraphDetails = value; }
       }

       public long StartRowIndex { get; set; }
       public int MaximumRows { get; set; }
       public int TotalRows { get; set; }
       public bool PagingEnabled { get; set; }
       public bool IsPagingEnabled { get; set; }
       public string sortExpression { get; set; }
       public int StartIndex { get; set; }

   }
}

