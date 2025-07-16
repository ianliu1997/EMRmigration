using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
   public class cls_IVFDashboar_AddUpdateGraphicalRepBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.cls_IVFDashboar_AddUpdateGraphicalRepBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private List<cls_IVFDashboard_GraphicalRepresentationVO> _GraphicalOocList = new List<cls_IVFDashboard_GraphicalRepresentationVO>();
        public List<cls_IVFDashboard_GraphicalRepresentationVO> GraphicalOocList
        {
            get
            {
                return _GraphicalOocList;
            }
            set
            {
                _GraphicalOocList = value;
            }
        }
        private cls_IVFDashboard_GraphicalRepresentationVO _Details = new cls_IVFDashboard_GraphicalRepresentationVO();
        public cls_IVFDashboard_GraphicalRepresentationVO Details
        {
            get
            {
                return _Details;
            }
            set
            {
                _Details = value;
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

   public class cls_IVFDashboar_GetGraphicalRepBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.cls_IVFDashboar_GetGraphicalRepBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private List<cls_IVFDashboard_GraphicalRepresentationVO> _GraphicalOocList = new List<cls_IVFDashboard_GraphicalRepresentationVO>();
        public List<cls_IVFDashboard_GraphicalRepresentationVO> GraphicalOocList
        {
            get
            {
                return _GraphicalOocList;
            }
            set
            {
                _GraphicalOocList = value;
            }
        }
        private cls_IVFDashboard_GraphicalRepresentationVO _Details = new cls_IVFDashboard_GraphicalRepresentationVO();
        public cls_IVFDashboard_GraphicalRepresentationVO Details
        {
            get
            {
                return _Details;
            }
            set
            {
                _Details = value;
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

   public class cls_IVFDashboard_AddUpdateDecisionBizActionVO : IBizActionValueObject
   {
       #region  IBizActionValueObject
       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.cls_IVFDashboard_AddUpdateDecisionBizAction";
       }

       public string ToXml()
       {
           return this.ToXml();
       }


       #endregion

       private List<cls_IVFDashboard_GraphicalRepresentationVO> _GraphicalOocList = new List<cls_IVFDashboard_GraphicalRepresentationVO>();
       public List<cls_IVFDashboard_GraphicalRepresentationVO> GraphicalOocList
       {
           get
           {
               return _GraphicalOocList;
           }
           set
           {
               _GraphicalOocList = value;
           }
       }
       private cls_IVFDashboard_GraphicalRepresentationVO _Details = new cls_IVFDashboard_GraphicalRepresentationVO();
       public cls_IVFDashboard_GraphicalRepresentationVO Details
       {
           get
           {
               return _Details;
           }
           set
           {
               _Details = value;
           }
       }

       private clsIVFDashboard_LabDaysVO _ETDetails = new clsIVFDashboard_LabDaysVO();
       public clsIVFDashboard_LabDaysVO ETDetails
       {
           get
           {
               return _ETDetails;
           }
           set
           {
               _ETDetails = value;
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

   public class cls_IVFDashboard_AddUpdatePlanDecisionBizActionVO : IBizActionValueObject
   {
       #region  IBizActionValueObject
       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.cls_IVFDashboard_AddUpdatePlanDecisionBizAction";
       }

       public string ToXml()
       {
           return this.ToXml();
       }


       #endregion
       private cls_IVFDashboard_GraphicalRepresentationVO _Details = new cls_IVFDashboard_GraphicalRepresentationVO();
       public cls_IVFDashboard_GraphicalRepresentationVO Details
       {
           get
           {
               return _Details;
           }
           set
           {
               _Details = value;
           }
       }


       private clsIVFDashboard_EmbryoTransferVO _ETDetails = new clsIVFDashboard_EmbryoTransferVO();
       public clsIVFDashboard_EmbryoTransferVO ETDetails
       {
           get
           {
               return _ETDetails;
           }
           set
           {
               _ETDetails = value;
           }
       }

       private List<clsIVFDashboard_EmbryoTransferDetailsVO> _ETDetailsList = new List<clsIVFDashboard_EmbryoTransferDetailsVO>();
       public List<clsIVFDashboard_EmbryoTransferDetailsVO> ETDetailsList
       {
           get
           {
               return _ETDetailsList;
           }
           set
           {
               _ETDetailsList = value;
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
