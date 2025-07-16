using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.OutPatientDepartment.QueueManagement
{
   public  class clsGetQueueListBizActionVO:IBizActionValueObject
    {

       private List<clsQueueVO> _QueueList;
       public DateTime? FromDate { get; set; }
       public DateTime? ToDate { get; set; }

       public List<clsQueueVO> QueueList
       {
           get { return _QueueList; }
           set { _QueueList = value; }
       }
       public long CurrentVisit { get; set; }
       string _MRNo;
       public string MRNo
       {
           get { return _MRNo; }
           set { _MRNo = value; }
       }
       string _ContactNo;
       public string ContactNo
       {
           get { return _ContactNo; }
           set { _ContactNo = value; }
       }
        string _TokenNo;
       public string TokenNo
       {
           get { return _TokenNo; }
           set { _TokenNo = value; }
       }
       private string _PatientName;
       public string PatientName
       {
           get { return _PatientName; }
           set { _PatientName = value; }
       }

       private string _FirstName;
       public string FirstName
       {
           get { return _FirstName; }
           set { _FirstName = value; }
       }

       private string _LastName;
       public string LastName
       {
           get { return _LastName; }
           set { _LastName = value; }
       }
       private long _SpecialRegID;
       public long SpecialRegID
       {
           get { return _SpecialRegID; }
           set { _SpecialRegID = value; }
       }
       private string _SpecialReg;
       public string SpecialReg
       {
           get { return _SpecialReg; }
           set { _SpecialReg = value; }
       }
       private long? _DoctorID;
       public long? DoctorID
       {
           get { return _DoctorID; }
           set { _DoctorID = value; }
       }

       private long? _DepartmentID;
       public long? DepartmentID
       {
           get { return _DepartmentID; }
           set { _DepartmentID = value; }
       }



       private long? _UnitID;
       public long? UnitID
       {
           get { return _UnitID; }
           set { _UnitID = value; }
       }


       private string _SearchExpression;
       public string SearchExpression
       {
           get { return _SearchExpression; }
           set { _SearchExpression = value; }
       }

       public string LinkServer { get; set; }
       public int TotalRows { get; set; }
       public int StartIndex { get; set; }
       public int MaximumRows { get; set; }
       public bool IsPagingEnabled { get; set; }
       public string sortExpression { get; set; }


       private clsQueueVO _QueueDetails;
       public clsQueueVO QueueDetails
       {
           get { return _QueueDetails; }
           set { _QueueDetails = value; }
       }
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.OutPatientDepartment.QueueManagement.clsGetQueueListBizAction";
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
