using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using System.Collections.ObjectModel;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
  public  class cls_NewAddUpdateSpremFreezingBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.cls_NewAddUpdateSpremFreezingBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        public long MalePatientID { get; set; }
        public long MalePatientUnitID { get; set; }
        public long ID { get; set; }
        public long UintID { get; set; }

      
        private clsNew_SpremFreezingVO _SpremFreezing = new clsNew_SpremFreezingVO();
        public clsNew_SpremFreezingVO SpremFreezingVO
        {
            get
            {
                return _SpremFreezing;
            }
            set
            {
                _SpremFreezing = value;
            }
        }

        private cls_NewSpremFreezingMainVO _SpremFreezingMain = new cls_NewSpremFreezingMainVO();
        public cls_NewSpremFreezingMainVO SpremFreezingMainVO
        {
            get
            {
                return _SpremFreezingMain;
            }
            set
            {
                _SpremFreezingMain = value;
            }
        }

        private ObservableCollection<clsNew_SpremFreezingVO> _SpremFreezingDetails = new ObservableCollection<clsNew_SpremFreezingVO>();
        public ObservableCollection<clsNew_SpremFreezingVO> SpremFreezingDetails
        {
            get { return _SpremFreezingDetails; }
            set { _SpremFreezingDetails = value; }
        }

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

  public class cls_NewGetSpremFreezingBizActionVO : IBizActionValueObject
  {
      #region  IBizActionValueObject

      public string GetBizAction()
      {
          return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.cls_NewGetSpremFreezingBizAction";
      }

      public string ToXml()
      {
          return this.ToXml();
      }


      #endregion

      public long MalePatientID { get; set; }
      public long MalePatientUnitID { get; set; }
      public long ID { get; set; }
      public long UintID { get; set; }
      public bool ISModify { get; set; }
      private clsNew_SpremFreezingVO _SpremFreezing = new clsNew_SpremFreezingVO();
      public clsNew_SpremFreezingVO SpremFreezingVO
      {
          get
          {
              return _SpremFreezing;
          }
          set
          {
              _SpremFreezing = value;
          }
      }

      private cls_NewSpremFreezingMainVO _SpremFreezingMain = new cls_NewSpremFreezingMainVO();
      public cls_NewSpremFreezingMainVO SpremFreezingMainVO
      {
          get
          {
              return _SpremFreezingMain;
          }
          set
          {
              _SpremFreezingMain = value;
          }
      }

      private ObservableCollection<clsNew_SpremFreezingVO> _SpremFreezingDetails = new ObservableCollection<clsNew_SpremFreezingVO>();
      public ObservableCollection<clsNew_SpremFreezingVO> SpremFreezingDetails
      {
          get { return _SpremFreezingDetails; }
          set { _SpremFreezingDetails = value; }
      }

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

  public class cls_NewDeleteSpremFreezingBizActionVO : IBizActionValueObject
  {
      #region  IBizActionValueObject

      public string GetBizAction()
      {
          return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.cls_NewDeleteSpremFreezingBizAction";
      }

      public string ToXml()
      {
          return this.ToXml();
      }


      #endregion

      public long MalePatientID { get; set; }
      public long MalePatientUnitID { get; set; }
      public long SpremID { get; set; }
      public long SpremUnitID { get; set; }
      public bool Status { get; set; }

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

      private clsNew_SpremFreezingVO _SpremFreezing = new clsNew_SpremFreezingVO();
      public clsNew_SpremFreezingVO SpremFreezingVO
      {
          get
          {
              return _SpremFreezing;
          }
          set
          {
              _SpremFreezing = value;
          }
      }
  }

    //By Anjali.................................................................
  public class cls_NewGetListSpremFreezingBizActionVO : IBizActionValueObject
  {
      #region  IBizActionValueObject

      public string GetBizAction()
      {
          return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.cls_NewGetListSpremFreezingBizAction";
      }

      public string ToXml()
      {
          return this.ToXml();
      }


      #endregion

      public long MalePatientID { get; set; }
      public long MalePatientUnitID { get; set; }
      public long ID { get; set; }
      public long UintID { get; set; }
      public bool ISModify { get; set; }
      private clsNew_SpremFreezingVO _SpremFreezing = new clsNew_SpremFreezingVO();
      public clsNew_SpremFreezingVO SpremFreezingVO
      {
          get
          {
              return _SpremFreezing;
          }
          set
          {
              _SpremFreezing = value;
          }
      }

      private List<cls_NewSpremFreezingMainVO> _SpremFreezingMainList = new List<cls_NewSpremFreezingMainVO>();
      public List<cls_NewSpremFreezingMainVO> SpremFreezingMainList
      {
          get
          {
              return _SpremFreezingMainList;
          }
          set
          {
              _SpremFreezingMainList = value;
          }
      }
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
    //............................................................................


}
