using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.IVFPlanTherapy;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
  public  class cls_NewAddUpdateDonorDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.cls_NewAddUpdateDonorDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsFemaleSemenDetailsVO _DonorDetails = new clsFemaleSemenDetailsVO();
        public clsFemaleSemenDetailsVO DonorDetails
        {
            get
            {
                return _DonorDetails;
            }
            set
            {
                _DonorDetails = value;
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

        private bool _IsEdit;
        public bool IsEdit
        {
            get
            {
                return _IsEdit;
            }
            set
            {
                _IsEdit = value;
            }
        }

    }

  public class cls_NewGetDonorDetailsBizActionVO : IBizActionValueObject
  {
      #region  IBizActionValueObject
      public string GetBizAction()
      {
          return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.cls_NewGetDonorDetailsBizAction";
      }

      public string ToXml()
      {
          return this.ToXml();
      }


      #endregion

      private clsFemaleSemenDetailsVO _DonorDetails = new clsFemaleSemenDetailsVO();
      public clsFemaleSemenDetailsVO DonorDetails
      {
          get
          {
              return _DonorDetails;
          }
          set
          {
              _DonorDetails = value;
          }
      }
      private List<clsFemaleSemenDetailsVO> _DonorDetailsList = new List<clsFemaleSemenDetailsVO>();
      public List<clsFemaleSemenDetailsVO> DonorDetailsList
      {
          get
          {
              return _DonorDetailsList;
          }
          set
          {
              _DonorDetailsList = value;
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
}
