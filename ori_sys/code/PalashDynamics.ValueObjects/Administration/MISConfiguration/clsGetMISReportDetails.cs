using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Administration.MISConfiguration
{
   public class clsGetMISReportDetailsBiZActionVO:IBizActionValueObject
    {

       public long MISID { get; set; }
       private List<clsMISReportVO> objMISConfig = null;
       public List<clsMISReportVO> MISReportDetails
        {
            get { return objMISConfig; }
            set { objMISConfig = value; }
        }

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.MISConfiguration.clsGetMISReportDetailsBiZAction";
        }

        public string ToXml()
        {
            return this.ToString();
        }
    }


   public class clsMISReportVO : IValueObject,INotifyPropertyChanged
   {
       public string ToXml()
       {
           return this.ToString();
       }

       #region INotifyPropertyChanged
       public event PropertyChangedEventHandler PropertyChanged;

       protected void OnPropertyChanged(string propertyName)
       {
           PropertyChangedEventHandler handler = PropertyChanged;

           if (null != handler)
           {
               handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
           }
       }

       #endregion

       private long _id;
       public long MISID
       {
           get { return _id; }
           set { _id = value; }
       }


       private long _Sys_MISReportId;
       public long Sys_MISReportId
       {
           get { return _Sys_MISReportId; }
           set { _Sys_MISReportId = value; }
       }


       private string _rptFileName;
       public string rptFileName
       {
           get { return _rptFileName; }
           set { _rptFileName = value; }
       }


       private string _ReportName;
       public string ReportName
       {
           get { return _ReportName; }
           set { _ReportName = value; }
       }

       public long MISReportFormat { get; set; }
      
       private long _StaffTypeID;
       public long StaffTypeID
       {
           get { return _StaffTypeID; }
           set { _StaffTypeID = value; }
       }

       private long _StaffID;
       public long StaffID
       {
           get { return _StaffID; }
           set { _StaffID = value; }
       }

       private string _EmailID;
       public string EmailID
       {
           get { return _EmailID; }
           set { _EmailID = value; }
       }



   }
}
