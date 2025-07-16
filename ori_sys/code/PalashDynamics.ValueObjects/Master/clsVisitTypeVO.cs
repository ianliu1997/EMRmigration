using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Master
{
    public class clsVisitTypeVO : IValueObject, INotifyPropertyChanged
    {
        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }


        #endregion

        #region INotifyPropertyChanged Members

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


        private long _ID;
        public long ID
        {
            get
            {
                return _ID;
            }

            set
            {
                if (value != _ID)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        private string _Code;
        public string Code
        {
            get
            {
                return _Code;
            }

            set
            {
                if (value != _Code)
                {
                    _Code = value;
                    OnPropertyChanged("Code");
                }
            }
        }
        
        private string _Description;
        public string Description
        {
            get
            {
                return _Description;
            }

            set
            {
                if (value != _Description)
                {
                    _Description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        private long _ServiceID;
        public long ServiceID
        {
            get
            {
                return _ServiceID;
            }

            set
            {
                if (value != _ServiceID)
                {
                    _ServiceID = value;
                    OnPropertyChanged("ServiceID");
                }
            }
        }

        private bool _IsClinical;
        public bool IsClinical
        {
            get
            {
                return _IsClinical;
            }

            set
            {
                if (value != _IsClinical)
                {
                    _IsClinical = value;
                    OnPropertyChanged("IsClinical");
                }
            }
        }


        private bool _IsPackage;
        public bool IsPackage
        {
            get
            {
                return _IsPackage;
            }

            set
            {
                if (value != _IsPackage)
                {
                    _IsPackage = value;
                    OnPropertyChanged("IsPackage");
                }
            }
        }
                
        private long _UnitID;
        public long UnitID
        {
            get
            {
                return _UnitID;
            }

            set
            {
                if (value != _UnitID)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }

        private bool _Status = true;
        public bool Status
        {
            get
            {
                return _Status;
            }

            set
            {
                if (value != _Status)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
                }
            }
        }


        private bool _IsFree;
        public bool IsFree
        {
            get
            {
                return _IsFree;
            }

            set
            {
                if (value != _IsFree)
                {
                    _IsFree = value;
                    OnPropertyChanged("IsFree");
                }
            }
        }

        private long _FreeDaysDuration;
        public long FreeDaysDuration
        {
            get
            {
                return _FreeDaysDuration;
            }

            set
            {
                if (value != _FreeDaysDuration)
                {
                    _FreeDaysDuration = value;
                    OnPropertyChanged("FreeDaysDuration");
                }
            }
        }

        private long _ConsultationVisitType;
        public long ConsultationVisitType
        {
            get
            {
                return _ConsultationVisitType;
            }

            set
            {
                if (value != _ConsultationVisitType)
                {
                    _ConsultationVisitType = value;
                    OnPropertyChanged("ConsultationVisitType");
                }
            }
        }


    }

    public class clsAddVisitTypeBizActionVO : IBizActionValueObject
    {
        private clsVisitTypeVO _Details;
        public clsVisitTypeVO Details
        {
            get { return _Details; }        
            set { _Details = value; }
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

        private const string _BizActionName = "PalashDynamics.BusinessLayer.Master.clsAddVisitTypeBizAction";
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return _BizActionName;
        }



        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

    }

    public class clsGetVisitTypeBizActionVO : IBizActionValueObject
    {

        //To Identify the log Exceptions Respective to Event.
     

        private const string _BizActionName = "PalashDynamics.BusinessLayer.Master.clsGetVisitTypeBizAction";



        public long ID { get; set; }
        List<clsVisitTypeVO> _List = new List<clsVisitTypeVO>();

        /// <summary>
        /// Output Property.
        /// Get Property To Access And Modify RoleList
        /// </summary> 
        public List<clsVisitTypeVO> List
        {
            get { return _List; }
            set { _List = value; }

        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return _BizActionName;
        }


       
        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }


    public class clsGetAllVisitTypeMasetrBizActionVO : IBizActionValueObject
    {
        private const string _BizActionName = "PalashDynamics.BusinessLayer.Master.clsGetAllVisitTypeMasetrBizAction";
        public long ID { get; set; }

        public long UnitID { get; set; }

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        public string sortExpression { get; set; }
        public string SearchExpression { get; set; }

        List<clsVisitTypeVO> _List = new List<clsVisitTypeVO>();

        /// <summary>
        /// Output Property.
        /// Get Property To Access And Modify RoleList
        /// </summary> 
        /// 
        public List<clsVisitTypeVO> List
        {
            get { return _List; }
            set { _List = value; }

        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return _BizActionName;
        }



        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsCheckVisitTypeMappedWithPackageServiceBizActionVO : IBizActionValueObject
    {
        private const string _BizActionName = "PalashDynamics.BusinessLayer.Master.clsCheckVisitTypeMappedWithPackageServiceBizAction";

        public long VisitTypeID { get; set; }
        public bool IsPackage { get; set; }

        clsVisitTypeVO VisitVO = new clsVisitTypeVO();

        /// <summary>
        /// Output Property.
        /// Get Property To Access And Modify RoleList
        /// </summary> 
        /// 
        public clsVisitTypeVO VisitTypeDetails
        {
            get { return VisitVO; }
            set { VisitVO = value; }

        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return _BizActionName;
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
