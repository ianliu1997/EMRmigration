using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
    public class clsSpermFreezingVO : IValueObject, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        /// <summary>
        /// Implemts the INotifyPropertyChanged interface.
        /// </summary>
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

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public Int64 ID { get; set; }
        public string InvoiceNo { get; set; }
        public string BatchCode { get; set; }
        public string Lab { get; set; }

        public string DonorCode { get; set; }
        public long SpremNo { get; set; }
        public long VtirificationNo { get; set; }
        public DateTime VitrificationDate { get; set; }
        public DateTime VitrificationTime { get; set; }
        public string MRNo { get; set; }
        public string PatientUnitName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string FirstName { get; set; }
        public string GobletColor { get; set; }
        public string CanID { get; set; }
        //public string CanisterNo { get; set; }
        public string StrawId { get; set; }
        public string GobletShapeId { get; set; }
        public string GobletSizeId { get; set; }
        public string CanisterId { get; set; }
        public string TankId { get; set; }
        public string CollectionMethod { get; set; }
        public string Volume { get; set; }
        public string Abstinence { get; set; }
        public string SpermCount { get; set; }
        public string Motility { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public long SpremFreezingID { get; set; }
        public long SpremFreezingUnitID { get; set; }
        public bool LongTerm { get; set; }
        public bool ShortTerm { get; set; }
        public string Type { get; set; }
        private bool _IsDiscard;
        public long PatientID  { get; set; }
        public long PatientUnitID { get; set; }
        public long UnitID { get; set; }
        public bool IsDiscard
        {
            get { return _IsDiscard; }
            set
            {
                if (_IsDiscard != value)
                {
                    _IsDiscard = value;
                    OnPropertyChanged("IsDiscard");
                }
            }
        }


        public string ColorCode { get; set; }
        private System.Windows.Media.Color _SelectesColor = new System.Windows.Media.Color() { A = 255, B = 0, G = 0, R = 128 };
        public System.Windows.Media.Color SelectesColor
        {
            get
            {
                return _SelectesColor;
            }
            set
            {
                _SelectesColor = value;
                ColorCode = _SelectesColor.ToString();

            }
        }

    }

    public class clsGetSpremFreezingDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsSpermFreezingBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion

        public long CoupleUintID { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long ID { get; set; }
        public long UnitID { get; set; }

        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string SearchExpression { get; set; }
        public long Cane { get; set; }

        public string FName { get; set; }
        public string MName { get; set; }
        public string LName { get; set; }
        public string FamilyName { get; set; }
        public string MRNo { get; set; }
        public string DonorCode { get; set; }
        public bool IsDiscard { get; set; }
        private List<clsSpermFreezingVO> _Vitrification = new List<clsSpermFreezingVO>();
        public List<clsSpermFreezingVO> Vitrification
        {
            get
            {
                return _Vitrification;
            }
            set
            {
                _Vitrification = value;
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
