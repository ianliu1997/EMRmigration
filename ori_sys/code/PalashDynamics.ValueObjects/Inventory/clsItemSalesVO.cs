using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects.CompoundDrug;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsItemSalesVO : IValueObject, INotifyPropertyChanged
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

        #region Common Properties

        private string _LinkServer;
        public string LinkServer
        {
            get { return _LinkServer; }
            set
            {
                if (_LinkServer != value)
                {
                    _LinkServer = value;
                    OnPropertyChanged("LinkServer");
                }
            }
        }

        private long _UnitId;
        public long UnitId
        {
            get { return _UnitId; }
            set
            {
                if (_UnitId != value)
                {
                    _UnitId = value;
                    OnPropertyChanged("UnitId");
                }
            }
        }

        private long _CostingDivisionID;
        public long CostingDivisionID
        {
            get { return _CostingDivisionID; }
            set
            {
                if (_CostingDivisionID != value)
                {
                    _CostingDivisionID = value;
                    OnPropertyChanged("CostingDivisionID");
                }
            }
        }

        private long _CreatedUnitID;
        public long CreatedUnitId
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitId");
                }
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitId
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitId");
                }
            }
        }

        private bool _Status = true;
        public bool Status
        {
            get { return _Status; }
            set
            {
                if (_Status != value)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        private long? _AddedBy;
        public long? AddedBy
        {
            get { return _AddedBy; }
            set
            {
                if (_AddedBy != value)
                {
                    _AddedBy = value;
                    OnPropertyChanged("AddedBy");
                }
            }
        }

        private string _AddedOn = "";
        public string AddedOn
        {
            get { return _AddedOn; }
            set
            {
                if (_AddedOn != value)
                {
                    _AddedOn = value;
                    OnPropertyChanged("AddedOn");
                }
            }
        }

        private DateTime? _AddedDateTime = DateTime.Now;
        public DateTime? AddedDateTime
        {
            get { return _AddedDateTime; }
            set
            {
                if (_AddedDateTime != value)
                {
                    _AddedDateTime = value;
                    OnPropertyChanged("AddedDateTime");
                }
            }
        }

        private string _AddedWindowsLoginName = "";
        public string AddedWindowsLoginName
        {
            get { return _AddedWindowsLoginName; }
            set
            {
                if (_AddedWindowsLoginName != value)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }

        private long? _UpdatedBy;
        public long? UpdatedBy
        {
            get { return _UpdatedBy; }
            set
            {
                if (_UpdatedBy != value)
                {
                    _UpdatedBy = value;
                    OnPropertyChanged("UpdatedBy");
                }
            }
        }

        private string _UpdatedOn = "";
        public string UpdatedOn
        {
            get { return _UpdatedOn; }
            set
            {
                if (_UpdatedOn != value)
                {
                    _UpdatedOn = value;
                    OnPropertyChanged("UpdatedOn");
                }
            }
        }

        private DateTime? _UpdatedDateTime = DateTime.Now;
        public DateTime? UpdatedDateTime
        {
            get { return _UpdatedDateTime; }
            set
            {
                if (_UpdatedDateTime != value)
                {
                    _UpdatedDateTime = value;
                    OnPropertyChanged("UpdatedDateTime");
                }
            }
        }

        private string _UpdatedWindowsLoginName = "";
        public string UpdatedWindowsLoginName
        {
            get { return _UpdatedWindowsLoginName; }
            set
            {
                if (_UpdatedWindowsLoginName != value)
                {
                    _UpdatedWindowsLoginName = value;
                    OnPropertyChanged("UpdatedWindowsLoginName");
                }
            }
        }



        #endregion


        #region Property Declaration Section

        private List<clsItemSalesDetailsVO> _Items = new List<clsItemSalesDetailsVO>();
        public List<clsItemSalesDetailsVO> Items
        {
            get { return _Items; }
            set
            {
                if (_Items != value)
                {
                    _Items = value;
                    OnPropertyChanged("Items");
                }
            }
        }


        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                if (_ID != value)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        private long _StoreID;
        public long StoreID
        {
            get { return _StoreID; }
            set
            {
                if (_StoreID != value)
                {
                    _StoreID = value;
                    OnPropertyChanged("StoreID");
                }
            }
        }

        private long _BillID;
        public long BillID
        {
            get { return _BillID; }
            set
            {
                if (_BillID != value)
                {
                    _BillID = value;
                    OnPropertyChanged("BillID");
                }
            }
        }

        private string _DeliveryAddress;
        public string DeliveryAddress
        {
            get { return _DeliveryAddress; }
            set
            {
                if (_DeliveryAddress != value)
                {
                    _DeliveryAddress = value;
                    OnPropertyChanged("DeliveryAddress");
                }
            }
        }



        private long _DeliveryBY;
        public long DeliveryBY
        {
            get { return _DeliveryBY; }
            set
            {
                if (_DeliveryBY != value)
                {
                    _DeliveryBY = value;
                    OnPropertyChanged("DeliveryBY");
                }
            }
        }


        private long _PurchaseFrequency;
        public long PurchaseFrequency
        {
            get { return _PurchaseFrequency; }
            set
            {
                if (_PurchaseFrequency != value)
                {
                    _PurchaseFrequency = value;
                    OnPropertyChanged("PurchaseFrequency");
                }
            }
        }

        private long _PurchaseFrequencyUnit;
        public long PurchaseFrequencyUnit
        {
            get { return _PurchaseFrequencyUnit; }
            set
            {
                if (_PurchaseFrequencyUnit != value)
                {
                    _PurchaseFrequencyUnit = value;
                    OnPropertyChanged("PurchaseFrequencyUnit");
                }
            }
        }


        private bool _IsBilled;
        public bool IsBilled
        {
            get { return _IsBilled; }
            set
            {
                if (_IsBilled != value)
                {
                    _IsBilled = value;
                    OnPropertyChanged("IsBilled");
                }
            }
        }


        private DateTime _Date = DateTime.Now;
        public DateTime Date
        {
            get { return _Date; }
            set
            {
                if (_Date != value)
                {
                    _Date = value;
                    OnPropertyChanged("Date");
                }
            }
        }

        private DateTime _Time = DateTime.Now;
        public DateTime Time
        {
            get { return _Time; }
            set
            {
                if (_Time != value)
                {
                    _Time = value;
                    OnPropertyChanged("Time");
                }
            }
        }


        private long _PatientID;
        public long PatientID
        {
            get { return _PatientID; }
            set
            {
                if (_PatientID != value)
                {
                    _PatientID = value;
                    OnPropertyChanged("PatientID");
                }
            }
        }

        private long _PatientUnitID;
        public long PatientUnitID
        {
            get { return _PatientUnitID; }
            set
            {
                if (_PatientUnitID != value)
                {
                    _PatientUnitID = value;
                    OnPropertyChanged("PatientUnitID");
                }
            }
        }

        private long _VisitID;
        public long VisitID
        {
            get { return _VisitID; }
            set
            {
                if (_VisitID != value)
                {
                    _VisitID = value;
                    OnPropertyChanged("VisitID");
                }
            }
        }
        private long _ReferenceDoctorID;
        public long ReferenceDoctorID
        {
            get { return _ReferenceDoctorID; }
            set
            {
                if (_ReferenceDoctorID != value)
                {
                    _ReferenceDoctorID = value;
                    OnPropertyChanged("ReferenceDoctorID");
                }
            }
        }
        private string _ReferenceDoctor;
        public string ReferenceDoctor
        {
            get { return _ReferenceDoctor; }
            set
            {
                if (_ReferenceDoctor != value)
                {
                    _ReferenceDoctor = value;
                    OnPropertyChanged("ReferenceDoctor");
                }
            }
        }
        private string _ReasonForVariance;
        public string ReasonForVariance
        {
            get { return _ReasonForVariance; }
            set
            {
                if (_ReasonForVariance != value)
                {
                    _ReasonForVariance = value;
                    OnPropertyChanged("ReasonForVariance");
                }
            }
        }

        private string _ItemSaleNo;
        public string ItemSaleNo
        {
            get { return _ItemSaleNo; }
            set
            {
                if (_ItemSaleNo != value)
                {
                    _ItemSaleNo = value;
                    OnPropertyChanged("ItemSaleNo");
                }
            }
        }

        private string _Remarks;
        public string Remarks
        {
            get { return _Remarks; }
            set
            {
                if (_Remarks != value)
                {
                    _Remarks = value;
                    OnPropertyChanged("Remarks");
                }
            }
        }

        private double _ConcessionPercentage;
        public double ConcessionPercentage
        {
            get { return _ConcessionPercentage; }
            set
            {
                if (_ConcessionPercentage != value)
                {
                    _ConcessionPercentage = value;
                    OnPropertyChanged("ConcessionPercentage");
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _ConcessionAmount;
        public double ConcessionAmount
        {
            get { return _ConcessionAmount; }
            set
            {
                if (_ConcessionAmount != value)
                {
                    _ConcessionAmount = value;
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _VATPercentage;
        public double VATPercentage
        {
            get { return _VATPercentage; }
            set
            {
                if (_VATPercentage != value)
                {
                    _VATPercentage = value;
                    OnPropertyChanged("VATPercentage");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("NetAmount");

                }
            }
        }

        private double _VATAmount;
        public double VATAmount
        {
            get { return _VATAmount; }
            set
            {
                if (_VATAmount != value)
                {
                    _VATAmount = value;
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _TotalAmount;
        public double TotalAmount
        {
            get { return _TotalAmount; }
            set
            {
                if (_TotalAmount != value)
                {
                    _TotalAmount = value;
                    OnPropertyChanged("TotalAmount");
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _NetAmount;
        public double NetAmount
        {
            get { return _NetAmount; }// = (_TotalAmount - _ConcessionAmount) + _VATAmount; }
            set
            {
                if (_NetAmount != value)
                {
                    _NetAmount = value;
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private Int64 _OPDNo;
        public Int64 OPDNo
        {
            get { return _OPDNo; }
            set
            {
                if (_OPDNo != value)
                {
                    _OPDNo = value;
                    OnPropertyChanged("OPDNo");
                }
            }
        }

        private double _NetBillAmount;
        public double NetBillAmount
        {
            get { return _NetBillAmount; }
            set
            {
                if (_NetBillAmount != value)
                {
                    _NetBillAmount = value;
                    OnPropertyChanged("NetBillAmount");
                }
            }
        }

        private double _BalanceAmountSelf;
        public double BalanceAmountSelf
        {
            get { return _BalanceAmountSelf; }
            set
            {
                if (_BalanceAmountSelf != value)
                {
                    _BalanceAmountSelf = value;
                    OnPropertyChanged("BalanceAmountSelf");
                }
            }
        }

        private double _PaidAmount;
        public double PaidAmount
        {
            get { return _PaidAmount; }
            set
            {
                if (_PaidAmount != value)
                {
                    _PaidAmount = value;
                    OnPropertyChanged("PaidAmount");
                }
            }
        }

        // Added By Changdeo Sase


        private double _BalAmount;
        public double BalAmount
        {
            get { return _BalAmount; } // = (_TotalAmount - _ConcessionAmount) + _VATAmount; }
            set
            {
                if (_BalAmount != value)
                {
                    _BalAmount = value;
                    OnPropertyChanged("BalAmount");
                }
            }
        }


        private string _FirstName = "";
        public string FirstName
        {
            get
            {
                return _FirstName;
            }
            set
            {
                if (_FirstName != value)
                {
                    _FirstName = value;
                    OnPropertyChanged("FirstName");
                }
            }
        }

        private string _MiddleName = "";
        public string MiddleName
        {
            get
            {
                return _MiddleName;
            }
            set
            {
                if (_MiddleName != value)
                {
                    _MiddleName = value;
                    OnPropertyChanged("MiddleName");
                }
            }
        }

        private string _LastName = "";
        public string LastName
        {
            get { return _LastName; }
            set
            {
                if (_LastName != value)
                {
                    _LastName = value;
                    OnPropertyChanged("LastName");
                }
            }
        }

        private string strPatientName = "";
        public string PatientName
        {
            get { return strPatientName = _FirstName + " " + _LastName; }
            set
            {
                if (value != strPatientName)
                {
                    strPatientName = value;
                    OnPropertyChanged("PatientName");
                }
            }
        }

        private string _BillNo;
        public string BillNo
        {
            get { return _BillNo; }
            set
            {
                if (_BillNo != value)
                {
                    _BillNo = value;
                    OnPropertyChanged("BillNo");
                }
            }
        }

        private long _PackageID;
        public long PackageID
        {
            get { return _PackageID; }
            set
            {
                if (_PackageID != value)
                {
                    _PackageID = value;
                    OnPropertyChanged("PackageID");
                }
            }
        }

        public bool ISForMaterialConsumption { get; set; } //Added by AJ Date 30/1/2017
        private long _Opd_Ipd_External_Id;  //Date 18/2/2017
        public long Opd_Ipd_External_Id
        {
            get { return _Opd_Ipd_External_Id; }
            set
            {
                if (_Opd_Ipd_External_Id != value)
                {
                    _Opd_Ipd_External_Id = value;
                    OnPropertyChanged("Opd_Ipd_External_Id");
                }
            }
        }

        private long _Opd_Ipd_External_UnitId;
        public long Opd_Ipd_External_UnitId
        {
            get { return _Opd_Ipd_External_UnitId; }
            set
            {
                if (_Opd_Ipd_External_UnitId != value)
                {
                    _Opd_Ipd_External_UnitId = value;
                    OnPropertyChanged("Opd_Ipd_External_UnitId");
                }
            }
        }

        #endregion
    }



    public class clsItemSalesDetailsVO : IValueObject, INotifyPropertyChanged
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

        #region Common Properties

        private string _LinkServer;
        public string LinkServer
        {
            get { return _LinkServer; }
            set
            {
                if (_LinkServer != value)
                {
                    _LinkServer = value;
                    OnPropertyChanged("LinkServer");
                }
            }
        }

        private long _UnitId;
        public long UnitId
        {
            get { return _UnitId; }
            set
            {
                if (_UnitId != value)
                {
                    _UnitId = value;
                    OnPropertyChanged("UnitId");
                }
            }
        }



        private long _CreatedUnitID;
        public long CreatedUnitId
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitId");
                }
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitId
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitId");
                }
            }
        }

        private bool _Status = true;
        public bool Status
        {
            get { return _Status; }
            set
            {
                if (_Status != value)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        private long? _AddedBy;
        public long? AddedBy
        {
            get { return _AddedBy; }
            set
            {
                if (_AddedBy != value)
                {
                    _AddedBy = value;
                    OnPropertyChanged("AddedBy");
                }
            }
        }

        private string _AddedOn = "";
        public string AddedOn
        {
            get { return _AddedOn; }
            set
            {
                if (_AddedOn != value)
                {
                    _AddedOn = value;
                    OnPropertyChanged("AddedOn");
                }
            }
        }

        private DateTime? _AddedDateTime = DateTime.Now;
        public DateTime? AddedDateTime
        {
            get { return _AddedDateTime; }
            set
            {
                if (_AddedDateTime != value)
                {
                    _AddedDateTime = value;
                    OnPropertyChanged("AddedDateTime");
                }
            }
        }

        private string _AddedWindowsLoginName = "";
        public string AddedWindowsLoginName
        {
            get { return _AddedWindowsLoginName; }
            set
            {
                if (_AddedWindowsLoginName != value)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }

        private long? _UpdatedBy;
        public long? UpdatedBy
        {
            get { return _UpdatedBy; }
            set
            {
                if (_UpdatedBy != value)
                {
                    _UpdatedBy = value;
                    OnPropertyChanged("UpdatedBy");
                }
            }
        }

        private string _UpdatedOn = "";
        public string UpdatedOn
        {
            get { return _UpdatedOn; }
            set
            {
                if (_UpdatedOn != value)
                {
                    _UpdatedOn = value;
                    OnPropertyChanged("UpdatedOn");
                }
            }
        }

        private DateTime? _UpdatedDateTime = DateTime.Now;
        public DateTime? UpdatedDateTime
        {
            get { return _UpdatedDateTime; }
            set
            {
                if (_UpdatedDateTime != value)
                {
                    _UpdatedDateTime = value;
                    OnPropertyChanged("UpdatedDateTime");
                }
            }
        }

        private string _UpdatedWindowsLoginName = "";
        public string UpdatedWindowsLoginName
        {
            get { return _UpdatedWindowsLoginName; }
            set
            {
                if (_UpdatedWindowsLoginName != value)
                {
                    _UpdatedWindowsLoginName = value;
                    OnPropertyChanged("UpdatedWindowsLoginName");
                }
            }
        }



        #endregion


        //By Anjali...........................

        private long _PrescriptionDetailsID;
        public long PrescriptionDetailsID
        {
            get { return _PrescriptionDetailsID; }
            set
            {
                if (_PrescriptionDetailsID != value)
                {
                    _PrescriptionDetailsID = value;
                    OnPropertyChanged("PrescriptionDetailsID");
                }
            }
        }
        private long _PrescriptionDetailsUnitID;
        public long PrescriptionDetailsUnitID
        {
            get { return _PrescriptionDetailsUnitID; }
            set
            {
                if (_PrescriptionDetailsUnitID != value)
                {
                    _PrescriptionDetailsUnitID = value;
                    OnPropertyChanged("PrescriptionDetailsUnitID");
                }
            }
        }
        private double _NewPendingQuantity;
        public double NewPendingQuantity
        {
            get { return _NewPendingQuantity; }
            set
            {
                if (_NewPendingQuantity != value)
                {

                    _NewPendingQuantity = value;
                    OnPropertyChanged("NewPendingQuantity");
                }
            }
        }
        private double _PrescribeQuantity;
        public double PrescribeQuantity
        {
            get { return _PrescribeQuantity; }
            set
            {
                if (_PrescribeQuantity != value)
                {

                    _PrescribeQuantity = value;
                    OnPropertyChanged("PrescribeQuantity");
                }
            }
        }
        //.....................................

        private long _Id;
        public long ID
        {
            get { return _Id; }
            set
            {
                if (_Id != value)
                {
                    _Id = value;
                    OnPropertyChanged("ID");
                }
            }
        }


        public bool IsBilled { get; set; }

        private bool? _IsPrescribedMedicine = false;
        public bool? IsPrescribedMedicine
        {
            get { return _IsPrescribedMedicine; }
            set
            {
                if (_IsPrescribedMedicine != value)
                {
                    _IsPrescribedMedicine = value;
                    OnPropertyChanged("IsPrescribedMedicine");
                }
            }
        }

        private long _ItemSaleId;
        public long ItemSaleId
        {
            get { return _ItemSaleId; }
            set
            {
                if (_ItemSaleId != value)
                {
                    _ItemSaleId = value;
                    OnPropertyChanged("ItemSaleId");
                }
            }
        }

        private long _ItemID;
        public long ItemID
        {
            get { return _ItemID; }
            set
            {
                if (_ItemID != value)
                {
                    _ItemID = value;
                    OnPropertyChanged("ItemID");
                }
            }
        }

        private string _ItemName;
        public string ItemName
        {
            get { return _ItemName; }
            set
            {
                if (_ItemName != value)
                {
                    _ItemName = value;
                    OnPropertyChanged("ItemName");
                }
            }
        }

        private long _BatchID;
        public long BatchID
        {
            get { return _BatchID; }
            set
            {
                if (_BatchID != value)
                {
                    _BatchID = value;
                    OnPropertyChanged("BatchID");
                }
            }
        }

        private string _BatchCode;
        public string BatchCode
        {
            get { return _BatchCode; }
            set
            {
                if (_BatchCode != value)
                {
                    _BatchCode = value;
                    OnPropertyChanged("BatchCode");
                }
            }
        }



        private string _ItemCode;
        public string ItemCode
        {
            get { return _ItemCode; }
            set
            {
                if (_ItemCode != value)
                {
                    _ItemCode = value;
                    OnPropertyChanged("ItemCode");
                }
            }
        }


        public DateTime? ExpiryDate { get; set; }

        private double _Quantity;
        public double Quantity
        {
            get { return _Quantity; }
            set
            {
                if (_Quantity != value)
                {
                    //if (value <= 0)
                    //    value = 1;

                    _Quantity = value;
                    OnPropertyChanged("Quantity");
                    //////
                    OnPropertyChanged("PendingQuantity");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("SGSTPercent");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTPercent");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("IGSTPercent");
                    OnPropertyChanged("NetAmount");
                    OnPropertyChanged("ConcessionPercentage");
                    OnPropertyChanged("VATPercent");
                    OnPropertyChanged("VATAmount");
                    /*
                        //Added By Pallavi
                    OnPropertyChanged("PendingQuantity");
                    OnPropertyChanged("Amount");
                    //Added By Pallavi
                    OnPropertyChanged("ConcessionPercentage");
                    OnPropertyChanged("ConcessionAmount");
                     OnPropertyChanged("VATPercent");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("NetAmount");
                     */
                }
            }
        }

        private double _PendingQuantity;
        public double PendingQuantity
        {
            get { return _PendingQuantity; }
            set
            {
                if (_PendingQuantity != value)
                {
                    if (value <= 0)
                        value = 1;

                    _PendingQuantity = value;
                    OnPropertyChanged("PendingQuantity");
                    /*
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("ConcessionPercentage");
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("VATPercent");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("NetAmount");*/
                }
            }
        }

        private double _MRP;
        public double MRP
        {
            get { return _MRP; }
            set
            {
                if (_MRP != value)
                {
                    if (value < 0)
                        value = 0;

                    _MRP = value;
                    OnPropertyChanged("MRP");
                    ////
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("NetAmount");

                    /* OnPropertyChanged("Amount");
                     #region Added By pallavi
                     OnPropertyChanged("ConcessionPercentage");
                     OnPropertyChanged("ConcessionAmount");
                     OnPropertyChanged("VATPercent");
                     OnPropertyChanged("VATAmount");
                     #endregion
                     OnPropertyChanged("NetAmount");*/

                }
            }
        }

        private double _Amount;
        public double Amount
        {
            get { return _Amount = _MRP * _Quantity; }
            set
            {
                if (_Amount != value)
                {
                    _Amount = value;
                    OnPropertyChanged("Amount");
                    ////
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("NetAmount");

                    /* #region Added By Pallavi
                     OnPropertyChanged("ConcessionPercentage");
                     OnPropertyChanged("ConcessionAmount");
                     OnPropertyChanged("VATPercent");
                     OnPropertyChanged("VATAmount");
                     #endregion
                     OnPropertyChanged("NetAmount");*/
                }
            }
        }

        private double _VATPercent;
        public double VATPercent
        {
            get { return _VATPercent; }
            set
            {
                if (_VATPercent != value)
                {
                    if (value < 0)
                        value = 0;
                    if (value > 100)
                        value = 100;

                    _VATPercent = value;
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("VATPercent");
                    OnPropertyChanged("ConcessionPercentage");
                    OnPropertyChanged("SGSTPercent");
                    OnPropertyChanged("CGSTPercent");
                    OnPropertyChanged("IGSTPercent");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("NetAmount");
                    OnPropertyChanged("ActualNetAmt");

                }
            }
        }



        private double _VATAmount;
        public double VATAmount
        {
            get
            {
                if (_VATPercent > 0)
                {
                    if (ItemVatType == 2)
                    {
                        //return _VATAmount = ((_Amount - _ConcessionAmount) * _VATPercent / 100);
                        return _VATAmount = ((_Amount) * _VATPercent / 100);
                    }
                    else
                    {
                        //double TotAmount = _Amount - _ConcessionAmount;
                        //_VATAmount = ((_Amount - _ConcessionAmount) / (100 + _VATPercent) * 100);
                        //return _VATAmount = TotAmount - _VATAmount;

                        if (_ConcessionPercentage > 0)
                        {
                            //return _VATAmount = (((((MRP / (100 + (_VATPercent))) * 100)) - (((MRP / (100 + (_VATPercent))) * 100) * (_ConcessionPercentage / 100))) * (_VATPercent / 100)) * _Quantity;
                            //return _VATAmount = (((((MRP / (100 + (_VATPercent))) * 100)) - (((MRP / (100 + (_VATPercent))) * 100) )) * (_VATPercent / 100)) * _Quantity;
                            return _VATAmount = ((MRP * _Quantity) - ((((MRP / (100 + (_VATPercent))) * 100)) * _Quantity));

                        }
                        else
                        {
                            return _VATAmount = ((MRP * _Quantity) - ((((MRP / (100 + (_VATPercent))) * 100)) * _Quantity));
                            // return _VATAmount = (((((MRP / (100 + (_VATPercent))) * 100)) - (((MRP / (100 + (_VATPercent))) * 100))) * (_VATPercent / 100)) * _Quantity;

                            //return _VATAmount = (((((MRP / (100 + (_VATPercent))) * 100)) - (((MRP / (100 + (_VATPercent))) * 100) * _ConcessionPercentage)) * (_VATPercent / 100)) * _Quantity;
                        }
                    }
                }
                else
                {
                    return _VATAmount = 0;
                }

            }
            set
            {
                if (_VATAmount != value)
                {
                    if (value < 0)
                        value = 0;
                    _VATAmount = value;
                    if (_VATAmount > 0 && (_Amount - ConcessionAmount) > 0)
                        if (ItemVatType == 2)
                        {
                            if (IsItemSaleReturn == false)
                            {
                                _VATPercent = (_VATAmount * 100) / (_Amount - ConcessionAmount);
                            }
                        }
                        else
                        {
                        }
                    else
                        _VATPercent = 0;

                    OnPropertyChanged("ConcessionPercentage");
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("VATPercent");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("SGSTPercent");
                    OnPropertyChanged("CGSTPercent");
                    OnPropertyChanged("IGSTPercent");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }
        //.....................................
        private double _ConcessionPercentage;
        public double ConcessionPercentage
        {
            get { return _ConcessionPercentage; }
            set
            {
                if (_ConcessionPercentage != value)
                {
                    if (value < 0)
                        value = 0;

                    if (value > 100)
                        value = 100;
                    _ConcessionPercentage = value;

                    OnPropertyChanged("ConcessionPercentage");
                    OnPropertyChanged("ConcessionAmount");
                    //By Anjali
                    OnPropertyChanged("VATPercent");
                    //....................
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("SGSTPercent");
                    OnPropertyChanged("CGSTPercent");
                    OnPropertyChanged("IGSTPercent");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("NetAmount");
                    //OnPropertyChanged("ActualNetAmt");
                }
            }
        }

        private double _ConcessionAmount;
        public double ConcessionAmount
        {
            get
            {
                if (_ConcessionPercentage > 0)
                {
                    if (_SGSTtaxtype == 2 || _CGSTtaxtype == 2 || _IGSTtaxtype == 2)
                    {
                        _ConcessionAmount = (_MRP / ((_SGSTPercent + _CGSTPercent + 100)) * 100) * (_ConcessionPercentage / 100) * _Quantity;
                    }
                    else
                    {
                        //_ConcessionAmount = (_MRP / ((_SGSTPercent + _CGSTPercent + 100)) * 100) * (_ConcessionPercentage / 100) * _Quantity;     // For Package New Changes commented on 14052018

                        if (_PackageID > 0)     // For Package New Changes added on 14052018
                            _ConcessionAmount = (_MRP) * (_ConcessionPercentage / 100) * _Quantity;     // For Package New Changes added on 14052018
                        else
                            _ConcessionAmount = (_MRP / ((_SGSTPercent + _CGSTPercent + 100)) * 100) * (_ConcessionPercentage / 100) * _Quantity;
                    }
                }
                else
                    _ConcessionAmount = 0;

                _ConcessionAmount = Math.Round(_ConcessionAmount, 2);

                return _ConcessionAmount;

                #region Commented on 05042018
                //if (_ConcessionPercentage > 0)
                //{
                //    if (_VATPercent != 0)
                //    {
                //        if (ItemVatType == 2)
                //        {
                //            _ConcessionAmount = (((_MRP * _Quantity) * _ConcessionPercentage) / 100);
                //            _ConcessionAmount = _ConcessionAmount * _Quantity;
                //        }
                //        else
                //        {
                //            _ConcessionAmount = ((MRP / (100 + (_VATPercent))) * 100) * (_ConcessionPercentage / 100);
                //            _ConcessionAmount = _ConcessionAmount * _Quantity;
                //        }
                //    }
                //    else if (_SGSTPercent != 0 || _CGSTPercent != 0 || _IGSTPercent != 0)
                //    {
                //        if (_SGSTtaxtype == 1 || _CGSTtaxtype == 1 || _IGSTtaxtype == 1)
                //        {
                //            _ConcessionAmount = (((MRP * _Quantity) / (1 + (_SGSTPercent / 100) + (_CGSTPercent / 100))) * (_ConcessionPercentage / 100));
                //        }
                //        else
                //        {
                //            _ConcessionAmount = (((_MRP * _Quantity) * _ConcessionPercentage) / 100);
                //        }
                //    }
                //}
                //else
                //    _ConcessionAmount = 0;

                //_ConcessionAmount = Math.Round(_ConcessionAmount, 2);

                //return _ConcessionAmount;
                #endregion
            }
            set
            {
                if (_ConcessionAmount != value)
                {
                    if (value < 0)
                        value = 0;
                    _ConcessionAmount = Math.Round(value, 2);
                    if (_ConcessionAmount > 0)
                    {
                        if (IsItemSaleReturn == false)
                        {
                            if (ItemVatType == 2)
                            {
                                _ConcessionPercentage = ((_ConcessionAmount / _Quantity) * 100) / _MRP;
                            }
                            else
                            {
                                _ConcessionPercentage = (((_ConcessionAmount / ((_MRP / (100 + (_VATPercent))) * 100)) * 100)) / _Quantity;
                            }
                        }
                    }
                    else
                        _ConcessionPercentage = 0;

                    _ConcessionPercentage = Math.Round(_ConcessionPercentage, 2);

                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("ConcessionPercentage");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }


        private double _NetAmount;
        public double NetAmount
        {
            get
            {
                if (_ItemVatType == 2)
                {
                    _NetAmount = _Amount - _ConcessionAmount + _VATAmount;
                }
                else
                {
                    if (_ConcessionPercentage > 0)
                    {
                        //double BP = ((((MRP * _Quantity) / (1 + (_SGSTPercent / 100) + (_CGSTPercent / 100))) - _ConcessionAmount));      //Nettxn Val  //Math.Round(((MRP / (100 + (_VATPercent)) * 100) * _Quantity), 2);       // Package Change 19042017
                        //double DBP = _ConcessionAmount;
                        //double VA = (_SGSTAmount + _CGSTAmount); //_VATAmount;
                        //_NetAmount = BP + VA;//BP - DBP + VA;

                        // For Package New Changes commented on 14052018
                        //double BP = ((MRP / (100 + (_SGSTPercent + _CGSTPercent)) * 100) * _Quantity);
                        //double DBP = _ConcessionAmount;
                        //double VA = _VATAmount;
                        //double GST = _SGSTAmount + _CGSTAmount;
                        //_NetAmount = BP - DBP + VA + GST;

                        double BP = 0;
                        if (_PackageID == 0)
                            BP = ((MRP / (100 + (_SGSTPercent + _CGSTPercent)) * 100) * _Quantity);
                        else
                            BP = ((MRP) * _Quantity);
                        double DBP = _ConcessionAmount;
                        double VA = _VATAmount;
                        double GST = _SGSTAmount + _CGSTAmount;
                        _NetAmount = BP - DBP + VA + GST;
                    }
                    else
                    {
                        //double BP = ((((MRP * _Quantity) / (1 + (_SGSTPercent / 100) + (_CGSTPercent / 100))) - _ConcessionAmount));  //Math.Round(((MRP / (100 + (_VATPercent)) * 100) * _Quantity), 2);      // Package Change 19042017
                        //double DBP = _ConcessionAmount;
                        //double VA = (_SGSTAmount + _CGSTAmount);//_VATAmount;
                        //_NetAmount = BP + VA;// BP - DBP + VA;

                        double BP = ((MRP / (100 + (_SGSTPercent + _CGSTPercent)) * 100) * _Quantity);
                        double DBP = _ConcessionAmount;
                        double VA = _VATAmount;
                        double GST = _SGSTAmount + _CGSTAmount;
                        _NetAmount = BP - DBP + VA + GST;
                    }
                }
                if (_NetAmount > 0)
                    return _NetAmount;
                else
                    return _NetAmount = 0;
            }
            set
            {
                if (_NetAmount != value)
                {
                    _NetAmount = value;
                    // _NetAmount = Math.Round(_NetAmount, 2); 
                    OnPropertyChanged("NetAmount");
                    OnPropertyChanged("ActualNetAmt");
                }
            }
        }

        private int _ItemVatType;
        public int ItemVatType
        {
            get
            {
                return _ItemVatType;
            }
            set
            {
                if (value != _ItemVatType)
                {
                    _ItemVatType = value;
                    OnPropertyChanged("ItemVatType");
                }
            }
        }

        private bool _CheckStatus;
        public bool CheckStatus
        {
            get { return _CheckStatus; }
            set
            {
                if (_CheckStatus != value)
                {
                    _CheckStatus = value;
                    OnPropertyChanged("CheckStatus");
                }
            }
        }

        public double AvailableQuantity { get; set; }

        public double OriginalMRP { get; set; }
        public string Manufacture { get; set; }

        public string PregnancyClass { get; set; }

        public string DrugSchedule { get; set; }

        public double PurchaseRate { get; set; }

        public bool InclusiveOfTax { get; set; }

        private float _ConversionFactor;
        public float ConversionFactor
        {
            get
            {
                return _ConversionFactor;
            }
            set
            {
                if (value != _ConversionFactor)
                {
                    _ConversionFactor = value;
                    OnPropertyChanged("ConversionFactor");
                }
            }
        }

        private float _StockingQuantity;
        public float StockingQuantity
        {
            get
            {
                return _StockingQuantity;
            }
            set
            {
                if (value != _StockingQuantity)
                {
                    _StockingQuantity = value;
                    OnPropertyChanged("StockingQuantity");
                }
            }
        }
        private double _ActualNetAmt;
        public double ActualNetAmt
        {

            get
            {
                if (_ItemVatType == 2)
                {
                    //return _ActualNetAmt = _Amount - _ConcessionAmount + _VATAmount;
                    return _ActualNetAmt = Math.Round(_Amount, 2) - _ConcessionAmount + _VATAmount;     // Package Change 19042017     
                }
                else
                {
                    //return _ActualNetAmt = _Amount - _ConcessionAmount;
                    return _ActualNetAmt = Math.Round(_Amount, 2) - _ConcessionAmount;                  // Package Change 19042017
                }
            }
            set
            {
                if (_ActualNetAmt != value)
                {

                    _ActualNetAmt = value;
                    OnPropertyChanged("ActualNetAmt");
                }
            }
        }

        public bool IsItemSaleReturn { get; set; }
        //SalePriceExclVAT

        public long CategoryId { get; set; }
        public long GroupId { get; set; }


        // Added By Changdeo Sase


        private DateTime? _Date;
        public DateTime? Date
        {
            get { return _Date; }
            set
            {
                if (_Date != value)
                {
                    _Date = value;
                    OnPropertyChanged("Date");
                }
            }
        }

        private bool _IsCashTariff;
        public bool IsCashTariff
        {
            get { return _IsCashTariff; }
            set
            {
                if (_IsCashTariff != value)
                {
                    _IsCashTariff = value;
                    OnPropertyChanged("IsCashTariff");
                }
            }
        }

        private bool _IsBeforeDiscount;
        public bool IsBeforeDiscount
        {

            get { return _IsBeforeDiscount; }
            set
            {
                if (value != _IsBeforeDiscount)
                {
                    _IsBeforeDiscount = value;
                    OnPropertyChanged("IsBeforeDiscount");
                }
            }
        }

        private double _Gross;
        public double Gross
        {
            get
            {
                return _Gross = (_MRP * _Quantity) - _DiscountAmt;
            }
            set
            {
                if (_Gross != value)
                {
                    _Gross = value;
                    OnPropertyChanged("Gross");
                    OnPropertyChanged("Concession");
                    OnPropertyChanged("NetGross");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _SettleNetAmount;
        public double SettleNetAmount
        {
            get { return _SettleNetAmount; }
            set
            {
                if (_SettleNetAmount != value)
                {
                    _SettleNetAmount = value;
                    OnPropertyChanged("SettleNetAmount");
                }
            }
        }


        private double _DiscountPerc;
        public double DiscountPerc
        {
            get
            {
                if (_DiscountPerc > 0)
                {
                    if (_IsCashTariff == true)
                        _DiscountAmt = ((_MRP * _DiscountPerc) / 100);
                    else
                        _DiscountAmt = ((_MRP * _Quantity * _DiscountPerc) / 100);
                    _Gross = _Amount - _DiscountAmt;
                }
                else
                {
                    _DiscountAmt = 0;
                    _DiscountPerc = 0;
                }
                return _DiscountPerc;
            }
            set
            {
                if (_DiscountPerc != value)
                {
                    if (value < 0)
                        value = 0;

                    _DiscountPerc = value;
                    OnPropertyChanged("DiscountPerc");
                    OnPropertyChanged("DiscountAmt");
                    OnPropertyChanged("NetGross");
                    OnPropertyChanged("Gross");
                    OnPropertyChanged("DeductiblePerc");
                    OnPropertyChanged("DeductibleAmt");
                    OnPropertyChanged("NetAmt");
                    OnPropertyChanged("NetAmount");
                    OnPropertyChanged("ActualNetAmt");

                }
            }
        }


        private double _DiscountAmt;
        public double DiscountAmt
        {
            get
            {
                if (_DiscountAmt > 0)
                {
                    if (IsCashTariff == true)
                        _DiscountPerc = (_DiscountAmt * 100) / _MRP;
                    else
                        _DiscountPerc = (_DiscountAmt * 100) / (_MRP * _Quantity);
                    _Gross = _Amount - _DiscountAmt;
                }
                else
                {
                    _DiscountAmt = 0;
                    _DiscountPerc = 0;
                }
                //_DiscountPerc = Math.Round(_DiscountPerc, 2);
                return _DiscountAmt;
            }
            set
            {
                if (_DiscountAmt != value)
                {
                    if (value < 0)
                        value = 0;
                    //if(_DiscountAmt !=)
                    // _DiscountAmt = Math.Round(value, 2);
                    _DiscountAmt = value;
                    if (_DiscountAmt > 0)
                    {
                        _DiscountPerc = (_DiscountAmt * 100) / _Amount;
                        //_DiscountPerc = (_DiscountAmt * 100) / _CompRate;
                        _NetAmount = _Amount - DiscountAmt;
                        _NetAmount = _Amount - DiscountAmt - _DeductibleAmt;
                        _Gross = _Amount - _NetAmount;
                    }
                    else
                        _DiscountPerc = 0;
                    OnPropertyChanged("DiscountAmt");
                    OnPropertyChanged("DiscountPerc");
                    OnPropertyChanged("Gross");
                    OnPropertyChanged("NetGross");
                    OnPropertyChanged("DeductiblePerc");
                    OnPropertyChanged("DeductibleAmt");
                    OnPropertyChanged("NetAmt");
                    OnPropertyChanged("NetAmount");
                    OnPropertyChanged("ActualNetAmt");

                }
            }
        }



        private double _DeductionPerc;
        public double DeductionPerc
        {
            get { return _DeductionPerc; }
            set
            {
                if (_DeductionPerc != value)
                {
                    if (value < 0)
                        value = 0;
                    _DeductionPerc = value;
                    OnPropertyChanged("DeductionPerc");
                }
            }
        }



        private double _DeductiblePerc;
        public double DeductiblePerc
        {
            get
            {
                //_DeductiblePerc = Math.Round(_DeductiblePerc, 2);

                if (_DeductiblePerc > 0)
                {
                    _DeductibleAmt = (_Gross * _DeductiblePerc) / 100;
                }
                else
                {
                    _DeductibleAmt = 0;
                    _DeductiblePerc = 0;
                }
                return _DeductiblePerc;
            }
            set
            {
                if (_DeductiblePerc != value)
                {
                    if (value < 0)
                        value = 0;
                    _DeductiblePerc = value;
                    OnPropertyChanged("DeductiblePerc");
                    OnPropertyChanged("DeductibleAmt");
                    OnPropertyChanged("NetAmt");
                    OnPropertyChanged("NetAmount");
                    OnPropertyChanged("ActualNetAmt");

                }
            }

        }



        private double _DeductibleAmt;
        public double DeductibleAmt
        {
            get
            {
                if (_DeductibleAmt > 0)
                    if (IsBeforeDiscount == true)
                    {
                        _DeductiblePerc = (_DeductibleAmt * 100) / _Gross;
                    }

                    else
                    {
                        _DeductiblePerc = (_DeductibleAmt * 100) / _Gross;
                    }
                else
                {
                    _DeductiblePerc = 0;
                    _DeductibleAmt = 0;
                }
                return _DeductibleAmt;
            }
            set
            {
                if (_DeductibleAmt != value)
                {
                    if (value < 0)
                        value = 0;
                    _DeductibleAmt = value;
                    OnPropertyChanged("DeductibleAmt");
                    OnPropertyChanged("DeductiblePerc");
                    OnPropertyChanged("NetAmt");
                    OnPropertyChanged("NetAmount");
                    OnPropertyChanged("ActualNetAmt");
                }
            }
        }

        private double _CompanyDiscountPerc;
        public double CompanyDiscountPerc
        {
            get { return _CompanyDiscountPerc; }
            set
            {
                if (_CompanyDiscountPerc != value)
                {
                    if (value < 0)
                        value = 0;
                    _CompanyDiscountPerc = value;
                    OnPropertyChanged("CompanyDiscountPerc");
                }
            }

        }


        private double _CompanyDiscountAmt;
        public double CompanyDiscountAmt
        {
            get { return _CompanyDiscountAmt; }
            set
            {
                if (_CompanyDiscountAmt != value)
                {
                    if (value < 0)
                        value = 0;
                    _CompanyDiscountAmt = value;
                    OnPropertyChanged("CompanyDiscountAmt");
                }
            }


        }

        private double _ItemPaidAmount;
        public double ItemPaidAmount
        {
            get { return _ItemPaidAmount; }
            set
            {
                if (_ItemPaidAmount != value)
                {
                    _ItemPaidAmount = value;
                    OnPropertyChanged("ItemPaidAmount");
                }
            }
        }

        private double _CompanyPaidAmt;
        public double CompanyPaidAmt
        {
            get { return _CompanyPaidAmt; }
            set
            {
                if (_CompanyPaidAmt != value)
                {
                    if (value < 0)
                        value = 0;
                    _CompanyPaidAmt = value;
                    OnPropertyChanged("CompanyPaidAmt");
                }
            }
        }


        //By Anjali.................
        private float _Budget;
        public float Budget
        {
            get { return _Budget; }
            set
            {
                if (_Budget != value)
                {
                    if (value < 0)
                        value = 0;
                    _Budget = value;
                    OnPropertyChanged("Budget");
                }
            }
        }
        private long _PackageID;
        public long PackageID
        {
            get { return _PackageID; }
            set
            {
                if (_PackageID != value)
                {
                    _PackageID = value;
                    OnPropertyChanged("PackageID");
                }
            }
        }
        private float _CalculatedBudget;
        public float CalculatedBudget
        {
            get { return _CalculatedBudget; }
            set
            {
                if (_CalculatedBudget != value)
                {
                    if (value < 0)
                        value = 0;
                    _CalculatedBudget = value;
                    OnPropertyChanged("CalculatedBudget");
                }
            }
        }




        public bool IsPackageForItem { get; set; }
        public bool IsPackageForCategory { get; set; }
        public bool IsPackageForGroup { get; set; }


        private double _NetAmtCalculation;
        public double NetAmtCalculation
        {
            get { return _NetAmtCalculation; }
            set
            {
                if (_NetAmtCalculation != value)
                {
                    _NetAmtCalculation = value;
                    OnPropertyChanged("NetAmtCalculation");
                    OnPropertyChanged("NetAmount");
                    OnPropertyChanged("ActualNetAmt");
                }
            }
        }

        //..........................
        // For Conversion Factor
        private float _MainRate;
        public float MainRate
        {
            get { return _MainRate; }
            set
            {
                if (_MainRate != value)
                {
                    _MainRate = value;
                    OnPropertyChanged("MainRate");
                }
            }
        }
        private string _PurchaseUOM;
        public string PurchaseUOM
        {
            get { return _PurchaseUOM; }
            set
            {
                if (_PurchaseUOM != value)
                {
                    _PurchaseUOM = value;
                    OnPropertyChanged("PurchaseUOM");
                }
            }
        }

        private String _StockUOM;
        public string StockUOM
        {
            get { return _StockUOM; }
            set
            {
                if (_StockUOM != value)
                {
                    _StockUOM = value;
                    OnPropertyChanged("StockUOM");
                }
            }
        }
        private float _MainMRP;
        public float MainMRP
        {
            get
            {
                _MainMRP = (float)Math.Round(_MainMRP, 2);
                return _MainMRP;
            }
            set
            {
                if (value != _MainMRP)
                {
                    _MainMRP = value;
                    OnPropertyChanged("MainMRP");
                }
            }
        }
        // For Getting Values The Defined For Conversion Factor
        private long _SaleUOMID;
        public long SaleUOMID
        {
            get { return _SaleUOMID; }
            set
            {
                if (_SaleUOMID != value)
                {
                    _SaleUOMID = value;
                    OnPropertyChanged("SaleUOMID");
                }
            }
        }
        private string _SaleUOM;
        public string SaleUOM
        {
            get { return _SaleUOM; }
            set
            {
                if (_SaleUOM != value)
                {
                    _SaleUOM = value;
                    OnPropertyChanged("SaleUOM");
                }
            }
        }
        private long _TransactionUOMID;
        public long TransactionUOMID
        {
            get { return _TransactionUOMID; }
            set
            {
                if (_TransactionUOMID != value)
                {
                    _TransactionUOMID = value;
                    OnPropertyChanged("TransactionUOMID");
                }
            }
        }

        private string _MainPUOM;
        public string MainPUOM
        {
            get
            {
                return _MainPUOM;
            }
            set
            {
                if (value != _MainPUOM)
                {
                    _MainPUOM = value;
                    OnPropertyChanged("MainPUOM");
                }
            }
        }

        private string _PUOM;
        public string PUOM
        {
            get { return _PUOM; }
            set
            {
                if (_PUOM != value)
                {
                    _PUOM = value;
                    OnPropertyChanged("PUOM");
                }
            }
        }
        private string _Rackname;
        public string Rackname
        {
            get
            {
                return _Rackname;
            }
            set
            {
                if (value != _Rackname)
                {
                    _Rackname = value;
                    OnPropertyChanged("Rackname");
                }
            }
        }
        private string _Shelfname;
        public string Shelfname
        {
            get
            {
                return _Shelfname;
            }
            set
            {
                if (value != _Shelfname)
                {
                    _Shelfname = value;
                    OnPropertyChanged("Shelfname");
                }
            }
        }
        private string _Containername;
        public string Containername
        {
            get
            {
                return _Containername;
            }
            set
            {
                if (value != _Containername)
                {
                    _Containername = value;
                    OnPropertyChanged("Containername");
                }
            }
        }
        private string _SUOM;
        public string SUOM
        {
            get { return _SUOM; }
            set
            {
                if (_SUOM != value)
                {
                    _SUOM = value;
                    OnPropertyChanged("SUOM");
                }
            }
        }

        private float _SingleQuantity;
        public float SingleQuantity
        {
            get
            {

                return _SingleQuantity;
            }
            set
            {
                if (_SingleQuantity != value)
                {

                    _SingleQuantity = value;
                    OnPropertyChanged("SingleQuantity");
                    OnPropertyChanged("Quantity");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("NetAmount");



                }
            }
        }

        //***//-----------
        private double _StaffDiscount;
        public double StaffDiscount
        {
            get
            {
                return _StaffDiscount;
            }
            set
            {
                if (value != _StaffDiscount)
                {
                    _StaffDiscount = value;
                    OnPropertyChanged("StaffDiscount");
                }
            }
        }

        private double _WalkinDiscount;
        public double WalkinDiscount
        {
            get
            {
                return _WalkinDiscount;
            }
            set
            {
                if (value != _WalkinDiscount)
                {
                    _WalkinDiscount = value;
                    OnPropertyChanged("WalkinDiscount");
                }
            }
        }

        private double _RegisteredPatientsDiscount;
        public double RegisteredPatientsDiscount
        {
            get
            {
                return _RegisteredPatientsDiscount;
            }
            set
            {
                if (value != _RegisteredPatientsDiscount)
                {
                    _RegisteredPatientsDiscount = value;
                    OnPropertyChanged("RegisteredPatientsDiscount");
                }
            }
        }      

        //---------------------

        MasterListItem _SelectedUOM = new MasterListItem();
        public MasterListItem SelectedUOM
        {
            get
            {
                return _SelectedUOM;
            }
            set
            {
                if (value != _SelectedUOM)
                {
                    _SelectedUOM = value;
                    OnPropertyChanged("SelectedUOM");
                }
            }


        }

        List<MasterListItem> _UOMList = new List<MasterListItem>();
        public List<MasterListItem> UOMList
        {
            get
            {
                return _UOMList;
            }
            set
            {
                if (value != _UOMList)
                {
                    _UOMList = value;

                }
            }

        }

        List<clsConversionsVO> _UOMConversionList = new List<clsConversionsVO>();
        public List<clsConversionsVO> UOMConversionList
        {
            get
            {
                return _UOMConversionList;
            }
            set
            {
                if (value != _UOMConversionList)
                {
                    _UOMConversionList = value;
                    OnPropertyChanged("UOMConversionList");
                }
            }

        }

        private long _PUOMID;
        public long PUOMID
        {
            get { return _PUOMID; }
            set
            {
                if (_PUOMID != value)
                {
                    _PUOMID = value;
                    OnPropertyChanged("PUOMID");
                }
            }
        }

        private long _SUOMID;
        public long SUOMID
        {
            get { return _SUOMID; }
            set
            {
                if (_SUOMID != value)
                {
                    _SUOMID = value;
                    OnPropertyChanged("SUOMID");
                }
            }
        }

        private long _BaseUOMID;
        public long BaseUOMID
        {
            get { return _BaseUOMID; }
            set
            {
                if (_BaseUOMID != value)
                {
                    _BaseUOMID = value;
                    OnPropertyChanged("BaseUOMID");
                }
            }
        }

        private string _BaseUOM;
        public string BaseUOM
        {
            get { return _BaseUOM; }
            set
            {
                if (_BaseUOM != value)
                {
                    _BaseUOM = value;
                    OnPropertyChanged("BaseUOM");
                }
            }
        }

        private long _SellingUOMID;
        public long SellingUOMID
        {
            get { return _SellingUOMID; }
            set
            {
                if (_SellingUOMID != value)
                {
                    _SellingUOMID = value;
                    OnPropertyChanged("SellingUOMID");
                }
            }
        }

        private string _SellingUOM;
        public string SellingUOM
        {
            get { return _SellingUOM; }
            set
            {
                if (_SellingUOM != value)
                {
                    _SellingUOM = value;
                    OnPropertyChanged("SellingUOM");
                }
            }
        }

        private float _BaseConversionFactor;
        public float BaseConversionFactor
        {
            get { return _BaseConversionFactor; }
            set
            {
                if (_BaseConversionFactor != value)
                {
                    _BaseConversionFactor = value;
                    OnPropertyChanged("BaseConversionFactor");
                }
            }
        }

        private float _BaseQuantity;
        public float BaseQuantity
        {
            get
            {

                return _BaseQuantity;
            }
            set
            {
                if (_BaseQuantity != value)
                {

                    _BaseQuantity = value;
                    OnPropertyChanged("BaseQuantity");
                    OnPropertyChanged("SingleQuantity");
                    OnPropertyChanged("Quantity");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        //by neena
        private double _TotalNewPendingQuantity;
        public double TotalNewPendingQuantity
        {
            get { return _TotalNewPendingQuantity; }
            set
            {
                if (_TotalNewPendingQuantity != value)
                {
                    _TotalNewPendingQuantity = value;
                    OnPropertyChanged("TotalNewPendingQuantity");
                }
            }
        }

        private long _PrescriptionID;
        public long PrescriptionID
        {
            get { return _PrescriptionID; }
            set
            {
                if (_PrescriptionID != value)
                {
                    _PrescriptionID = value;
                    OnPropertyChanged("PrescriptionID");
                }
            }
        }

        private float _ActualPrescribedBaseQuantity;
        public float ActualPrescribedBaseQuantity
        {
            get
            {

                return _ActualPrescribedBaseQuantity;
            }
            set
            {
                if (_ActualPrescribedBaseQuantity != value)
                {

                    _ActualPrescribedBaseQuantity = value;
                    OnPropertyChanged("ActualPrescribedBaseQuantity");
                }
            }
        }

        private float _BaseRate;
        public float BaseRate
        {
            get { return _BaseRate; }
            set
            {
                if (_BaseRate != value)
                {
                    _BaseRate = value;
                    OnPropertyChanged("BaseRate");
                }
            }
        }

        private float _AvailableStockInBase;
        public float AvailableStockInBase
        {
            get { return _AvailableStockInBase; }
            set
            {
                if (_AvailableStockInBase != value)
                {
                    _AvailableStockInBase = value;
                    OnPropertyChanged("AvailableStockInBase");
                }
            }
        }
        private float _BaseMRP;
        public float BaseMRP
        {
            get
            {
                _BaseMRP = (float)Math.Round((decimal)_BaseMRP, 2);
                return _BaseMRP;
            }
            set
            {
                if (value != _BaseMRP)
                {
                    _BaseMRP = value;
                    OnPropertyChanged("BaseMRP");
                }
            }
        }

        private float _ItemMRP;
        public float ItemMRP
        {
            get { return _ItemMRP; }
            set
            {
                if (_ItemMRP != value)
                {
                    _ItemMRP = value;
                    OnPropertyChanged("ItemMRP");
                }
            }
        }
        private float _ItemPurchaseRate;
        public float ItemPurchaseRate
        {
            get { return _ItemPurchaseRate; }
            set
            {
                if (_ItemPurchaseRate != value)
                {
                    _ItemPurchaseRate = value;
                    OnPropertyChanged("ItemPurchaseRate");
                }
            }
        }
        private float _StockCF;
        public float StockCF
        {
            get { return _StockCF; }
            set
            {
                if (_StockCF != value)
                {
                    _StockCF = value;
                    OnPropertyChanged("StockCF");
                }
            }
        }
        // END

        //For MultiSponser...............
        public long CompanyID { get; set; }
        public long PatientSourceID { get; set; }
        public long TariffID { get; set; }
        public long PatientCategoryID { get; set; }
        //.......................

        //Added by Ashish Z. on Dated 140616
        private float _PendingBudget;
        public float PendingBudget
        {
            get
            {
                return _PendingBudget = _Budget - _CalculatedBudget;
            }
            set
            {
                if (_PendingBudget != value)
                {
                    if (value < 0)
                        value = 0;
                    _PendingBudget = value;
                    OnPropertyChanged("PendingBudget");
                }
            }
        }
        //End

        //Added by AJ Date 1/2/2017 use only for Pharmacy Bill 
        public double MRPBill { get; set; }
        public double ConcessionPercentageBill { get; set; }
        public double ConcessionAmountBill { get; set; }
        public double AmountBill { get; set; }
        public double VATPercentBill { get; set; }
        public double VATAmountBill { get; set; }
        public double NetAmountBill { get; set; }
        public double NetBillAmount { get; set; }

        public double SGSTPercentBill { get; set; }
        public double CGSTPercentBill { get; set; }
        public double IGSTPercentBill { get; set; }
        public double SGSTAmountBill { get; set; }
        public double CGSTAmountBill { get; set; }
        public double IGSTAmountBill { get; set; }

        private double _SGSTPercent;
        public double SGSTPercent
        {
            get { return _SGSTPercent; }
            set
            {
                if (_SGSTPercent != value)
                {
                    if (value < 0)
                        value = 0;
                    if (value > 100)
                        value = 100;
                    _SGSTPercent = value;
                    OnPropertyChanged("SGSTPercent");
                    OnPropertyChanged("CGSTPercent");
                    OnPropertyChanged("IGSTPercent");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("VATPercent");
                    OnPropertyChanged("ConcessionPercentage");
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("NetAmount");
                    OnPropertyChanged("ActualNetAmt");
                }
            }
        }


        private double _CGSTPercent;
        public double CGSTPercent
        {
            get { return _CGSTPercent; }
            set
            {
                if (_CGSTPercent != value)
                {
                    if (value < 0)
                        value = 0;
                    if (value > 100)
                        value = 100;
                    _CGSTPercent = value;
                    OnPropertyChanged("SGSTPercent");
                    OnPropertyChanged("CGSTPercent");
                    OnPropertyChanged("IGSTPercent");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("VATPercent");
                    OnPropertyChanged("ConcessionPercentage");
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("NetAmount");
                    OnPropertyChanged("ActualNetAmt");
                }
            }
        }

        private double _IGSTPercent;
        public double IGSTPercent
        {
            get { return _IGSTPercent; }
            set
            {
                if (_IGSTPercent != value)
                {
                    if (value < 0)
                        value = 0;
                    if (value > 100)
                        value = 100;
                    _IGSTPercent = value;
                    OnPropertyChanged("SGSTPercent");
                    OnPropertyChanged("CGSTPercent");
                    OnPropertyChanged("IGSTPercent");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("VATPercent");
                    OnPropertyChanged("ConcessionPercentage");
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("NetAmount");
                    OnPropertyChanged("ActualNetAmt");
                }
            }
        }

        private double _SGSTAmount;
        public double SGSTAmount
        {
            get
            {
                if (_SGSTPercent > 0)
                {
                    if (_SGSTtaxtype == 2)
                    {
                        return _SGSTAmount = ((_Amount) * _SGSTPercent / 100);
                    }
                    else
                    {
                        if (_ConcessionPercentage > 0)
                        {
                            //return _SGSTAmount = (((((MRP * _Quantity) / (1 + (_SGSTPercent / 100) + (_CGSTPercent / 100))) - _ConcessionAmount)) * (_SGSTPercent / 100));

                            // For Package New Changes commented on 14052018
                            //double AbatedAmt = _MRP * _Quantity / ((_SGSTPercent + _CGSTPercent + 100)) * 100;
                            //return _SGSTAmount = (AbatedAmt - _ConcessionAmount) * (_SGSTPercent / 100);

                            if (_PackageID == 0)    // For Package New Changes Added on 14052018
                            {
                                double AbatedAmt = _MRP * _Quantity / ((_SGSTPercent + _CGSTPercent + 100)) * 100;
                                return _SGSTAmount = (AbatedAmt - _ConcessionAmount) * (_SGSTPercent / 100);
                            }
                            else
                                return _SGSTAmount = 0;
                        }
                        else
                        {
                            //return _SGSTAmount = (((((MRP * _Quantity) / (1 + (_SGSTPercent / 100) + (_CGSTPercent / 100))) - _ConcessionAmount)) * (_SGSTPercent / 100));
                            double AbatedAmt = _MRP * _Quantity / ((_SGSTPercent + _CGSTPercent + 100)) * 100;
                            return _SGSTAmount = (AbatedAmt - _ConcessionAmount) * (_SGSTPercent / 100);


                        }
                    }
                }
                else
                {
                    return _SGSTAmount = 0;
                }
            }
            set
            {
                if (_SGSTAmount != value)
                {
                    if (value < 0)
                        value = 0;
                    _SGSTAmount = value;
                    if (_SGSTAmount > 0 && (_Amount - _ConcessionAmount) > 0)
                        if (_SGSTtaxtype == 2)
                        {
                            if (IsItemSaleReturn == false)
                            {
                                _SGSTPercent = (_SGSTAmount * 100) / (_Amount - _ConcessionAmount);
                            }
                        }
                        else
                        {
                        }
                    else
                        _SGSTPercent = 0;

                    OnPropertyChanged("ConcessionPercentage");
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("VATPercent");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("SGSTPercent");
                    OnPropertyChanged("CGSTPercent");
                    OnPropertyChanged("IGSTPercent");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _CGSTAmount;
        public double CGSTAmount
        {
            get
            {
                if (_CGSTPercent > 0)
                {
                    if (_CGSTtaxtype == 2)
                    {
                        return _CGSTAmount = ((_Amount) * _CGSTPercent / 100);
                    }
                    else
                    {
                        if (_ConcessionPercentage > 0)
                        {
                            //return _CGSTAmount = (((((MRP * _Quantity) / (1 + (_SGSTPercent / 100) + (_CGSTPercent / 100))))) * (_CGSTPercent / 100));

                            // For Package New Changes Commented on 14052018
                            //double AbatedAmt = _MRP * _Quantity / (_SGSTPercent + _CGSTPercent + 100) * 100;
                            //return _CGSTAmount = (AbatedAmt - _ConcessionAmount) * (_CGSTPercent / 100);

                            if (_PackageID == 0)    // For Package New Changes Added on 14052018
                            {
                                double AbatedAmt = _MRP * _Quantity / (_SGSTPercent + _CGSTPercent + 100) * 100;
                                return _CGSTAmount = (AbatedAmt - _ConcessionAmount) * (_CGSTPercent / 100);
                            }
                            else
                                return _CGSTAmount = 0;
                        }
                        else
                        {
                            //return _CGSTAmount = (((((MRP * _Quantity) / (1 + (_SGSTPercent / 100) + (_CGSTPercent / 100))))) * (_CGSTPercent / 100));
                            double AbatedAmt = _MRP * _Quantity / ((_SGSTPercent + _CGSTPercent + 100)) * 100;
                            return _CGSTAmount = (AbatedAmt - _ConcessionAmount) * (_CGSTPercent / 100);
                        }
                    }
                }
                else
                {
                    return _CGSTAmount = 0;
                }
            }
            set
            {
                if (_CGSTAmount != value)
                {
                    if (value < 0)
                        value = 0;
                    _CGSTAmount = value;
                    if (_CGSTAmount > 0 && (_Amount - _ConcessionAmount) > 0)
                        if (_CGSTtaxtype == 2)
                        {
                            if (IsItemSaleReturn == false)
                            {
                                _CGSTPercent = (_CGSTAmount * 100) / (_Amount - _ConcessionAmount);
                            }
                        }
                        else
                        {
                        }
                    else
                        _CGSTPercent = 0;

                    OnPropertyChanged("ConcessionPercentage");
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("VATPercent");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("SGSTPercent");
                    OnPropertyChanged("CGSTPercent");
                    OnPropertyChanged("IGSTPercent");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _IGSTAmount;
        public double IGSTAmount
        {
            get
            {
                if (_IGSTPercent > 0)
                {
                    if (IGSTtaxtype == 2)
                    {
                        return _IGSTAmount = ((_Amount) * _IGSTPercent / 100);
                    }
                    else
                    {
                        if (_ConcessionPercentage > 0)
                        {
                            return _IGSTAmount = ((MRP * _Quantity) - ((((MRP / (100 + (_IGSTPercent))) * 100)) * _Quantity));
                        }
                        else
                        {
                            return _IGSTAmount = ((MRP * _Quantity) - ((((MRP / (100 + (_IGSTPercent))) * 100)) * _Quantity));
                        }
                    }
                }
                else
                {
                    return _IGSTAmount = 0;
                }
            }
            set
            {
                if (_IGSTAmount != value)
                {
                    if (value < 0)
                        value = 0;
                    _IGSTAmount = value;
                    if (_IGSTAmount > 0 && (_Amount - _ConcessionAmount) > 0)
                        if (_IGSTtaxtype == 2)
                        {
                            if (IsItemSaleReturn == false)
                            {
                                _IGSTPercent = (_IGSTAmount * 100) / (_Amount - _ConcessionAmount);
                            }
                        }
                        else
                        {
                        }
                    else
                        _IGSTPercent = 0;

                    OnPropertyChanged("ConcessionPercentage");
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("VATPercent");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("SGSTPercent");
                    OnPropertyChanged("CGSTPercent");
                    OnPropertyChanged("IGSTPercent");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private int _SGSTtaxtype;
        public int SGSTtaxtype
        {
            get
            {
                return _SGSTtaxtype;
            }
            set
            {
                if (value != _SGSTtaxtype)
                {
                    _SGSTtaxtype = value;
                    //OnPropertyChanged("SGSTtaxtype");
                }
            }
        }
        private int _SGSTapplicableon;
        public int SGSTapplicableon
        {
            get
            {
                return _SGSTapplicableon;
            }
            set
            {
                if (value != _SGSTapplicableon)
                {
                    _SGSTapplicableon = value;
                    //OnPropertyChanged("SGSTapplicableon");
                }
            }
        }

        ///////////////////////////////////////////////////

        private int _CGSTtaxtype;
        public int CGSTtaxtype
        {
            get
            {
                return _CGSTtaxtype;
            }
            set
            {
                if (value != _CGSTtaxtype)
                {
                    _CGSTtaxtype = value;
                    //OnPropertyChanged("CGSTtaxtype");
                }
            }
        }
        private int _CGSTapplicableon;
        public int CGSTapplicableon
        {
            get
            {
                return _CGSTapplicableon;
            }
            set
            {
                if (value != _CGSTapplicableon)
                {
                    _CGSTapplicableon = value;
                    //OnPropertyChanged("CGSTapplicableon");
                }
            }
        }

        ///////////////////////////////////////////////////

        private int _IGSTtaxtype;
        public int IGSTtaxtype
        {
            get
            {
                return _IGSTtaxtype;
            }
            set
            {
                if (value != _IGSTtaxtype)
                {
                    _IGSTtaxtype = value;
                    //OnPropertyChanged("IGSTtaxtype");
                }
            }
        }
        private int _IGSTapplicableon;
        public int IGSTapplicableon
        {
            get
            {
                return _IGSTapplicableon;
            }
            set
            {
                if (value != _IGSTapplicableon)
                {
                    _IGSTapplicableon = value;
                    //OnPropertyChanged("IGSTapplicableon");
                }
            }
        }

        //***//-----------------------------------------------

        private double _DiscountOnPackageItem;

        public double DiscountOnPackageItem
        {
            get { return _DiscountOnPackageItem; }
            set { _DiscountOnPackageItem = value; }
        }
        
        #region Package Change 18042017

        private long _PackageBillID;
        public long PackageBillID
        {
            get { return _PackageBillID; }
            set
            {
                if (_PackageBillID != value)
                {
                    _PackageBillID = value;
                    OnPropertyChanged("PackageBillID");
                }
            }
        }

        private long _PackageBillUnitID;
        public long PackageBillUnitID
        {
            get { return _PackageBillUnitID; }
            set
            {
                if (_PackageBillUnitID != value)
                {
                    _PackageBillUnitID = value;
                    OnPropertyChanged("PackageBillUnitID");
                }
            }
        }

        ////////////////////////////////////////////////////////////

        private double _PackageConcessionPercentage;
        public double PackageConcessionPercentage
        {
            get { return _PackageConcessionPercentage; }
            set
            {
                if (_PackageConcessionPercentage != value)
                {
                    if (value < 0)
                        value = 0;

                    if (value > 100)
                        value = 100;
                    _PackageConcessionPercentage = value;

                    OnPropertyChanged("PackageConcessionPercentage");
                    OnPropertyChanged("PackageConcessionAmount");
                    ////By Anjali
                    //OnPropertyChanged("VATPercent");
                    ////....................
                    //OnPropertyChanged("VATAmount");
                    //OnPropertyChanged("SGSTPercent");
                    //OnPropertyChanged("CGSTPercent");
                    //OnPropertyChanged("IGSTPercent");
                    //OnPropertyChanged("SGSTAmount");
                    //OnPropertyChanged("CGSTAmount");
                    //OnPropertyChanged("IGSTAmount");
                    //OnPropertyChanged("Amount");
                    //OnPropertyChanged("NetAmount");
                    ////OnPropertyChanged("ActualNetAmt");
                }
            }
        }

        ////////////////////////////////////////////////////////////////

        private double _PackageConcessionAmount;
        public double PackageConcessionAmount
        {
            get
            {
                if (_PackageConcessionPercentage > 0)
                {
                    if (_SGSTtaxtype == 2 || _CGSTtaxtype == 2 || _IGSTtaxtype == 2)
                    {
                        _PackageConcessionAmount = (_MRP / ((_SGSTPercent + _CGSTPercent + 100)) * 100) * (_PackageConcessionPercentage / 100) * _Quantity;
                    }
                    else
                    {
                        //_ConcessionAmount = (_MRP / ((_SGSTPercent + _CGSTPercent + 100)) * 100) * (_ConcessionPercentage / 100) * _Quantity;     // For Package New Changes commented on 14052018

                        if (_PackageID > 0)     // For Package New Changes added on 14052018
                            _PackageConcessionAmount = (_MRP) * (_PackageConcessionPercentage / 100) * _Quantity;     // For Package New Changes added on 14052018
                        else
                            _PackageConcessionAmount = (_MRP / ((_SGSTPercent + _CGSTPercent + 100)) * 100) * (_PackageConcessionPercentage / 100) * _Quantity;
                    }
                }
                else
                    _PackageConcessionAmount = 0;

                _PackageConcessionAmount = Math.Round(_PackageConcessionAmount, 2);

                return _PackageConcessionAmount;

                
            }
            set
            {
                if (_PackageConcessionAmount != value)
                {
                    if (value < 0)
                        value = 0;
                    _PackageConcessionAmount = Math.Round(value, 2);
                    if (_PackageConcessionAmount > 0)
                    {
                        if (IsItemSaleReturn == false)
                        {
                            if (ItemVatType == 2)
                            {
                                _PackageConcessionPercentage = ((_PackageConcessionAmount / _Quantity) * 100) / _MRP;
                            }
                            else
                            {
                                _PackageConcessionPercentage = (((_PackageConcessionAmount / ((_MRP / (100 + (_VATPercent))) * 100)) * 100)) / _Quantity;
                            }
                        }
                    }
                    else
                        _PackageConcessionPercentage = 0;

                    _PackageConcessionPercentage = Math.Round(_PackageConcessionPercentage, 2);

                    OnPropertyChanged("PackageConcessionAmount");
                    OnPropertyChanged("PackageConcessionPercentage");
                    //OnPropertyChanged("VATAmount");
                    //OnPropertyChanged("SGSTAmount");
                    //OnPropertyChanged("CGSTAmount");
                    //OnPropertyChanged("IGSTAmount");
                    //OnPropertyChanged("Amount");
                    //OnPropertyChanged("NetAmount");
                }
            }
        }

        #endregion
    }
}
