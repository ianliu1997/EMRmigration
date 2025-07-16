using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Master;

namespace PalashDynamics.ValueObjects
{
   public class clsAddPatientConsentBizActionVO:IBizActionValueObject
   {
       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.clsAddPatientConsentBizAction";
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
       public int SuccessStatus
       {
           get { return _SuccessStatus; }
           set { _SuccessStatus = value; }
       }



       private clsPatientConsentVO _ConsentDetails;
       public clsPatientConsentVO ConsentDetails
       {
           get { return _ConsentDetails; }
           set { _ConsentDetails = value; }

       }

    }


   public class clsGetPatientConsentMasterBizActionVO : IBizActionValueObject
   {
       #region  IBizActionValueObject
       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.clsGetPatientConsentMasterBizAction";
       }

       public string ToXml()
       {
           return this.ToXml();
       }


       #endregion

       /// <summary>
       /// This property contains Item master details.
       /// </summary>
       private List<clsPatientConsentVO> objConsentMaster = new List<clsPatientConsentVO>();
       public List<clsPatientConsentVO> ConsentMatserDetails
       {
           get
           {
               return objConsentMaster;
           }
           set
           {
               objConsentMaster = value;

           }
       }
       public long ID { get; set; }
       public long UnitID { get; set; }
       public long StartRowIndex { get; set; }
       public int MaximumRows { get; set; }
       public int TotalRows { get; set; }
       public bool PagingEnabled { get; set; }
       public string SearchExpression { get; set; }
       public long DepartmentID { get; set; }
       public long Template { get; set; }
       /// <summary>
       ///  Output Property.
       /// This property states the outcome of BizAction Process.
       /// </summary>
       private int _SuccessStatus;
       public int SuccessStatus
       {
           get
           {
               return _SuccessStatus;
           }
           set
           {
               _SuccessStatus = value;
           }
       }

   }

   //public class clsGetMasterListConsentBizActionVO : IBizActionValueObject
   //{
   //    #region IBizActionValueObject Members

   //    public string GetBizAction()
   //    {
   //        return "PalashDynamics.BusinessLayer.clsGetMasterListConsentBizAction";
   //    }

   //    #endregion

   //    #region IValueObject Members

   //    public string ToXml()
   //    {
   //        return this.ToString();
   //    }

   //    #endregion

   //    public clsGetMasterListConsentBizActionVO()
   //    {

   //    }
   //    private MasterTableNameList _MasterTable = MasterTableNameList.None;
   //    public MasterTableNameList MasterTable
   //    {
   //        get
   //        {
   //            return _MasterTable;
   //        }
   //        set
   //        {
   //            _MasterTable = value;
   //        }
   //    }

   //    private string _Error = "";
   //    public string Error
   //    {
   //        get { return _Error; }
   //        set { _Error = value; }
   //    }
   //    private string _FilterExpression;
   //    public string FilterExpression
   //    {
   //        get { return _FilterExpression; }
   //        set { _FilterExpression = value; }
   //    }
   //    /// <summary>
   //    /// Gets or Sets MasterList
   //    /// </summary>
   //    private List<MasterListItem> _MasterList = null;
   //    public List<MasterListItem> MasterList
   //    {
   //        get
   //        { return _MasterList; }

   //        set
   //        { _MasterList = value; }
   //    }

   //    public KeyValue Parent { get; set; }
   //    public bool? IsActive { get; set; }
   //    public bool? IsDate { get; set; }
   //    private bool _IsSubTest = false;
   //    public bool IsSubTest
   //    {
   //        get { return _IsSubTest; }
   //        set { _IsSubTest = value; }
   //    }
   //}

}
