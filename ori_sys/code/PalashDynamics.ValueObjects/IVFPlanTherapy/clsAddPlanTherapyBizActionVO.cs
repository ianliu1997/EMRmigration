using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects.EMR;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
    public class clsAddPlanTherapyBizActionVO : IBizActionValueObject
    {

        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsAddPlanTherapyBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsPlanTherapyVO _TherapyDetails = new clsPlanTherapyVO();
        public clsPlanTherapyVO TherapyDetails
        {
            get
            {
                return _TherapyDetails;
            }
            set
            {
                _TherapyDetails = value;
            }
        }

        private clsTherapyDocumentsVO _TherapyDocument = new clsTherapyDocumentsVO();
        public clsTherapyDocumentsVO TherapyDocument
        {
            get
            {
                return _TherapyDocument;
            }
            set
            {
                _TherapyDocument = value;
            }
        }

        private clsTherapyANCVO _TherapyANCVisits = new clsTherapyANCVO();
        public clsTherapyANCVO TherapyANCVisits
        {
            get
            {
                return _TherapyANCVisits;
            }
            set
            {
                _TherapyANCVisits = value;
            }
        }

        private List<clsTherapyANCVO> _List;
        public List<clsTherapyANCVO> ANCList
        {
            get
            {
                if (_List == null)
                    _List = new List<clsTherapyANCVO>();

                return _List;
            }

            set
            {

                _List = value;

            }
        }

        private clsTherapyDeliveryVO _TherapyDelivery = new clsTherapyDeliveryVO();
        public clsTherapyDeliveryVO TherapyDelivery
        {
            get
            {
                return _TherapyDelivery;
            }
            set
            {
                _TherapyDelivery = value;
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
    public class clsPlanTherapyVO
    {

        public long ID { get; set; }
        public long TabID { get; set; }// this is use which tab details is Add or Updated
        public long UnitID { get; set; }
        public long PatientUintId { get; set; }
        public long PatientId { get; set; }
        public bool IsDonorCycle { get; set; }
        public long CoupleId { get; set; }
        public long CoupleUnitId { get; set; }
        public bool IsHalfBilled { get; set; }
        public bool IsFullBilled { get; set; }
        public long EMRProcedureUnitID { get; set; }
        public long EMRProcedureID { get; set; }

        //added by neena for ivf billing
        public bool IsIVFBillingCriteria { get; set; }
        //

        private DateTime? _TherapyStartDate = null;
        public DateTime? TherapyStartDate
        {
            get
            {
                return _TherapyStartDate;
            }
            set
            {
                _TherapyStartDate = value;
            }
        }
        public string CycleDuration { get; set; }
        public string Cyclecode { get; set; }
        public long ProtocolTypeID { get; set; }
        public string Pill { get; set; }

        private DateTime? _PillStartDate = null;
        public DateTime? PillStartDate
        {
            get
            {
                return _PillStartDate;
            }
            set
            {
                _PillStartDate = value;
            }
        }
        private DateTime? _PillEndDate = null;
        public DateTime? PillEndDate
        {
            get
            {
                return _PillEndDate;
            }
            set
            {
                _PillEndDate = value;

            }
        }

        private double _NetAmount;
        public double NetAmount
        {
            get
            {
                return _NetAmount;
            }
            set
            {
                _NetAmount = value;
                OnPropertyChanged("NetAmount");
            }
        }

        private double _PaidAmount;
        public double PaidAmount
        {
            get
            {
                return _PaidAmount;
            }
            set
            {
                _PaidAmount = value;
                OnPropertyChanged("PaidAmount");
            }
        }

        private double _BillAmount;
        public double BillAmount
        {
            get
            {
                return _BillAmount;
            }
            set
            {
                _BillAmount = value;
                OnPropertyChanged("BillAmount");
            }
        }

        private double _BillBalanceAmount;
        public double BillBalanceAmount
        {
            get
            {
                return _BillBalanceAmount;
            }
            set
            {
                _BillBalanceAmount = value;
                OnPropertyChanged("BillBalanceAmount");
            }
        }

        public long PlannedTreatmentID { get; set; }
        public long FinalPlannedTreatmentID { get; set; }

        public long PlannedNoofEmbryos { get; set; }



        public long MainInductionID { get; set; }
        public long MainSubInductionID { get; set; }
        public string MainInduction { get; set; }
        public long PhysicianId { get; set; }
        public long ExternalSimulationID { get; set; }
        public long PlannedSpermCollectionID { get; set; }
        public string TherapyNotes { get; set; }

        public bool PACAnabled { get; set; }
        public bool ConsentCheck { get; set; }
        public long AnesthesistId { get; set; }

        //public long DiagnosisID { get; set; }
        public int Status { get; set; }
        private bool _IsModify = false;
        public bool IsModify
        {
            get
            {
                return _IsModify;
            }
            set
            {
                _IsModify = value;
                OnPropertyChanged("IsModifyLink");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        //
        //By Anjali............
        private DateTime? _OPUtDate;
        public DateTime? OPUtDate
        {
            get
            {
                return _OPUtDate;
            }
            set
            {
                _OPUtDate = value;
            }
        }
        public string OPURemark { get; set; }


        public string LutealSupport { get; set; }
        public string LutealRemarks { get; set; }
        private DateTime? _BHCGAss1Date = null;
        public DateTime? BHCGAss1Date
        {
            get
            {
                return _BHCGAss1Date;
            }
            set
            {
                _BHCGAss1Date = value;

            }
        }

        public bool? BHCGAss1IsBSCG { get; set; }
        public bool? BHCGAss1IsBSCGPositive { get; set; }
        public bool? BHCGAss1IsBSCGNagative { get; set; }
        public string BHCGAss1BSCGValue { get; set; }
        public string BHCGAss1SrProgest { get; set; }
        private DateTime? _BHCGAss2Date = null;
        public DateTime? BHCGAss2Date
        {
            get
            {
                return _BHCGAss2Date;
            }
            set
            {
                _BHCGAss2Date = value;

            }
        }

        public bool? BHCGAss2IsBSCG { get; set; }
        public bool? BHCGAss2IsBSCGPostive { get; set; }
        public bool? BHCGAss2IsBSCGNagative { get; set; }

        public string BHCGAss2BSCGValue { get; set; }
        public string BHCGAss2USG { get; set; }


        public bool? IsPregnancyAchieved { get; set; }
        public bool IsPregnancyAchievedPostive { get; set; }
        public bool IsPregnancyAchievedNegative { get; set; }

        private DateTime? _PregnanacyConfirmDate = null;
        public DateTime? PregnanacyConfirmDate
        {
            get
            {
                return _PregnanacyConfirmDate;
            }
            set
            {
                _PregnanacyConfirmDate = value;

            }
        }
        public bool IsClosed { get; set; }

        //Added By CDs

        private DateTime? _OPUDate1;
        public DateTime? OPUDate1
        {
            get
            {
                return _OPUDate1;
            }
            set
            {
                _OPUDate1 = value;
            }
        }

        public string OPUDONEBY { get; set; }
        private DateTime? _ETDate;
        public DateTime? ETDate
        {
            get
            {
                return _ETDate;
            }
            set
            {
                _ETDate = value;
            }
        }

        private DateTime? _CryoDate;
        public DateTime? CryoDate
        {
            get
            {
                return _CryoDate;
            }
            set
            {
                _CryoDate = value;
            }
        }

        public Int64 CryoNo { get; set; }
        private DateTime? _ThwaDate;
        public DateTime? ThwaDate
        {
            get
            {
                return _ThwaDate;
            }
            set
            {
                _ThwaDate = value;
            }
        }

        public string SourceOfOoctye { get; set; }
        public string SourceOfSemen { get; set; }



        //added later
        public bool BiochemPregnancy { get; set; }
        public bool Ectopic { get; set; }
        public bool Abortion { get; set; }
        public bool Missed { get; set; }
        public bool Incomplete { get; set; }
        public bool IUD { get; set; }
        public bool PretermDelivery { get; set; }
        public bool LiveBirth { get; set; }
        public bool Congenitalabnormality { get; set; }
        public string Count { get; set; }
        public bool FetalHeartSound { get; set; }
        private DateTime? _FetalDate = null;
        public DateTime? FetalDate
        {
            get
            {
                return _FetalDate;
            }
            set
            {
                _FetalDate = value;

            }
        }

        //added later
        public string OutComeRemarks { get; set; }
        public long CreatedUnitID { get; set; }
        public long UpdatedUnitID { get; set; }
        public string AddedBy { get; set; }
        public long AddedOn { get; set; }
        public DateTime? AddedDateTime { get; set; }


        public string ProtocolType { get; set; }
        public string SpermCollection { get; set; }
        public string PlannedTreatment { get; set; }
        public string Physician { get; set; }


        public long ThreapyExecutionId { get; set; }
        public long TherapyExeTypeID { get; set; }
        public string Day { get; set; }
        public string Value { get; set; }

        public long ThearpyTypeDetailId { get; set; }

        public string DrugNotes { get; set; }
        public DateTime? DrugTime { get; set; }

        public bool PCOS { get; set; }
        public bool Hypogonadotropic { get; set; }
        public bool Tuberculosis { get; set; }
        public bool Endometriosis { get; set; }
        public bool UterineFactors { get; set; }
        public bool TubalFactors { get; set; }
        public bool DiminishedOvarian { get; set; }
        public bool PrematureOvarianFailure { get; set; }
        public bool LutealPhasedefect { get; set; }
        public bool HypoThyroid { get; set; }
        public bool HyperThyroid { get; set; }
        public bool MaleFactors { get; set; }
        public bool OtherFactors { get; set; }
        public bool UnknownFactors { get; set; }
        public bool FemaleFactorsOnly { get; set; }
        public bool FemaleandMaleFactors { get; set; }
        // By BHUSHAN
        public bool IsChemicalPregnancy { get; set; }
        public bool IsFullTermDelivery { get; set; }
        public long BabyTypeID { get; set; }
        public bool OHSSEarly { get; set; }
        public bool OHSSLate { get; set; }
        public bool OHSSMild { get; set; }
        public bool OHSSMode { get; set; }
        public bool OHSSSereve { get; set; }
        public string OHSSRemark { get; set; }

        // OUTCOME FOR BABY..   .   .   .   .   
        public long SIXmonthFitnessID { get; set; }
        public string SIXmonthFitnessRemark { get; set; }
        public long ONEyFitnessID { get; set; }
        public string ONEyFitnessRemark { get; set; }
        public long FIVEyFitnessID { get; set; }
        public string FIVEyFitnessRemark { get; set; }
        public long TENyFitnessID { get; set; }
        public string TENyFitnessRemark { get; set; }
        public long TWENTYyFitnessID { get; set; }
        public string TWENTYyFitnessRemark { get; set; }

        public long SIXmonthFitnessID_m { get; set; }
        public string SIXmonthFitnessRemark_m { get; set; }
        public long ONEyFitnessID_m { get; set; }
        public string ONEyFitnessRemark_m { get; set; }
        public long FIVEyFitnessID_m { get; set; }
        public string FIVEyFitnessRemark_m { get; set; }
        public long TENyFitnessID_m { get; set; }
        public string TENyFitnessRemark_m { get; set; }
        public long TWENTYyFitnessID_m { get; set; }
        public string TWENTYyFitnessRemark_m { get; set; }

        // By BHUSHAN For Surrogacy..
        public string SurrogateMRNo { get; set; }
        public long SurrogateID { get; set; }
        public long SurrogateExecutionId { get; set; }
        public bool IsSurrogateCalendar { get; set; }
        public bool IsSurrogateDrug { get; set; }
        public bool IsSurrogate { get; set; }

        //By Anjali..... for DashBoard
        public bool AssistedHatching { get; set; }
        public bool CryoPreservation { get; set; }
        public bool IMSI { get; set; }
        public string LongtermMedication { get; set; }
        public string PlannedEmbryos { get; set; }
        public long SurrogateUnitID { get; set; }
        public bool AttachedSurrogate { get; set; }
        public bool IsEmbryoDonation { get; set; }


        // Added by Anumani 
        // Added as Per the changes in Therapy

        public long SpermSource { get; set; }
        public long PlannedSpermCollection { get; set; }
        public DateTime? StartOvarian { get; set; }
        public DateTime? EndOvarian { get; set; }
        public DateTime? StartTrigger { get; set; }
        public DateTime? TriggerTime { get; set; }
        public DateTime? StartStimulation { get; set; }
        public DateTime? EndStimulation { get; set; }
        public DateTime? SpermCollectionDate { get; set; }
        public string Note { get; set; }
        public string CancellationReason { get; set; }
        public bool IsCancellation { get; set; }
        public DateTime? LutealStartDate { get; set; }
        public DateTime? LutealEndDate { get; set; }
        public string MainIndication { get; set; }
        public string SourceOfSperm { get; set; }
        public long OocyteRetrived { get; set; }
        public long NoOocytesCryo { get; set; }
        public long NoEmbryoCryo { get; set; }
        public long NoEmbryosTransferred { get; set; }
        public long NoFrozenEmbryosTransferred { get; set; }
        public long NoOocytesDonated { get; set; }
        public long NoOfEmbryos { get; set; }
        public long NoEmbryoDonated { get; set; }
        public long NoOfSacs { get; set; }
        public long FetalHeartCount { get; set; }
        public string BHCGAssessment { get; set; }
        public long NoOfEmbExtCulture { get; set; }
        public long NoOfEmbFzOocyte { get; set; }
        public bool IsIsthambul { get; set; }
        public string OPUCycleCancellationReason { get; set; }
        public bool IsOPUCycleCancellation { get; set; }
        public bool OPUFreeze { get; set; }
    }

    public class clsDignosisVO
    {

        //By rohini
        public long ID { get; set; }

        public long UnitID { get; set; }
        public long PatientUintId { get; set; }
        public long PatientId { get; set; }
        public bool Status { get; set; }

        public long DiagnosisID { get; set; }
        public long TransactionID { get; set; }
        public long TransactionUnitID { get; set; }

        public long TransactionTypeID { get; set; }

        #region Common Property Declaration
        private long? _CreatedUnitID;
        public long? CreatedUnitID
        {
            get
            {
                return _CreatedUnitID;
            }
            set
            {
                _CreatedUnitID = value;
                OnPropertyChanged("CreatedUnitID");
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitID
        {
            get
            {
                return _UpdatedUnitID;
            }
            set
            {
                _UpdatedUnitID = value;
                OnPropertyChanged("UpdatedUnitID");
            }
        }

        private long? _AddedBy;
        public long? AddedBy
        {
            get
            {
                return _AddedBy;
            }
            set
            {
                _AddedBy = value;
                OnPropertyChanged("AddedBy");
            }
        }

        private string _AddedOn;
        public string AddedOn
        {
            get
            {
                return _AddedOn;
            }
            set
            {
                _AddedOn = value;
                OnPropertyChanged("AddedOn");
            }
        }

        private DateTime? _AddedDateTime;
        public DateTime? AddedDateTime
        {
            get
            {
                return _AddedDateTime;
            }
            set
            {
                _AddedDateTime = value;
                OnPropertyChanged("AddedDateTime");
            }
        }

        private long? _UpdatedBy;
        public long? UpdatedBy
        {
            get
            {
                return _UpdatedBy;
            }
            set
            {
                _UpdatedBy = value;
                OnPropertyChanged("UpdatedBy");
            }
        }

        private string _UpdatedOn;
        public string UpdatedOn
        {
            get
            {
                return _UpdatedOn;
            }
            set
            {
                _UpdatedOn = value;
                OnPropertyChanged("UpdatedOn");
            }
        }

        private DateTime? _UpdatedDateTime;
        public DateTime? UpdatedDateTime
        {
            get
            {
                return _UpdatedDateTime;
            }
            set
            {
                _UpdatedDateTime = value;
                OnPropertyChanged("UpdatedDateTime");
            }
        }

        private string _AddedWindowsLoginName;
        public string AddedWindowsLoginName
        {
            get
            {
                return _AddedWindowsLoginName;
            }
            set
            {
                _AddedWindowsLoginName = value;
                OnPropertyChanged("AddedWindowsLoginName");
            }
        }

        private string _UpdateWindowsLoginName;
        public string UpdateWindowsLoginName
        {
            get
            {
                return _UpdateWindowsLoginName;
            }
            set
            {
                _UpdateWindowsLoginName = value;
                OnPropertyChanged("UpdateWindowsLoginName");
            }
        }


        #endregion
        private bool _IsModify = false;
        public bool IsModify
        {
            get
            {
                return _IsModify;
            }
            set
            {
                _IsModify = value;
                OnPropertyChanged("IsModify");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        //

    }
    public class clsTherapyDocumentsVO
    {
        public long ID { get; set; }
        public DateTime? Date { get; set; }
        public long TherapyID { get; set; }
        public long ThearpyUnitID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AttachedFileName { get; set; }
        public byte[] AttachedFileContent { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class clsTherapyExecutionVO : NotificationModel
    {
        public string IsForHyprelink { get; set; }
        public string IsForImage { get; set; }

        public long ID { get; set; }
        public long PlanTherapyId { get; set; }
        public DateTime? TherapyStartDate { get; set; }
        public long PhysicianID { get; set; }
        public long PlanTherapyUnitId { get; set; }
        public long TherapyTypeId { get; set; }
        public string TherapyType { get; set; }
        public string TherapyGroup { get; set; }
        public long GroupId { get; set; }
        public bool IsText { get; set; }
        public bool IsBool { get; set; }

        //By Anjali
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long CoupleID { get; set; }
        public long CoupleUnitID { get; set; }
        public bool IsSemenDonorExists { get; set; }
        public bool IsOocyteDonorExists { get; set; }
        public string OoctyDonorMrNo { get; set; }
        public string SemenDonorMrNo { get; set; }
        public bool IsSurrogate { get; set; }
        //........................
        //by vikrant
        public bool IsReadOnly { get; set; }


        public bool IsTriggerDrug { get; set; }
        public string Head { get; set; }
        public long HeadId { get; set; }
        public long ThearpyTypeDetailId { get; set; }

        // By....  BHUSHAN
        public long SurrogateExeID { get; set; }
        public string SurrogateMRNo { get; set; }
        public long SurrogateID { get; set; }
        public bool IsSurrogateCalendar { get; set; }
        private string _Day;
        public string Day
        {
            get
            {
                return _Day;
            }
            set
            {
                if (_Day != value)
                {
                    _Day = value;
                    OnPropertyChanged("Day");
                }
            }
        }

        private string _Day1;
        public string Day1
        {
            get
            {
                return _Day1;
            }
            set
            {
                if (_Day1 != value)
                {
                    _Day1 = value;
                    OnPropertyChanged("Day1");
                }
            }
        }
        //It is string Type. Gets or sets the Day2 value
        private string _Day2;
        public string Day2
        {
            get
            {
                return _Day2;
            }
            set
            {
                if (_Day2 != value)
                {
                    _Day2 = value;
                    OnPropertyChanged("Day2");
                }
            }
        }
        //It is string Type. Gets or sets the Day3 value
        private string _Day3;
        public string Day3
        {
            get
            {
                return _Day3;
            }
            set
            {
                if (_Day3 != value)
                {
                    _Day3 = value;
                    OnPropertyChanged("Day3");
                }
            }
        }
        //It is string Type. Gets or sets the Day4 value
        private string _Day4;
        public string Day4
        {
            get
            {
                return _Day4;
            }
            set
            {
                if (_Day4 != value)
                {
                    _Day4 = value;
                    OnPropertyChanged("Day4");
                }
            }
        }
        //It is string Type. Gets or sets the Day5 value
        private string _Day5;
        public string Day5
        {
            get
            {
                return _Day5;
            }
            set
            {
                if (_Day5 != value)
                {
                    _Day5 = value;
                    OnPropertyChanged("Day5");
                }
            }
        }
        //It is string Type. Gets or sets the Day6 value
        private string _Day6;
        public string Day6
        {
            get
            {
                return _Day6;
            }
            set
            {
                if (_Day6 != value)
                {
                    _Day6 = value;
                    OnPropertyChanged("Day6");
                }
            }
        }
        //It is string Type. Gets or sets the Day7 value
        private string _Day7;
        public string Day7
        {
            get
            {
                return _Day7;
            }
            set
            {
                if (_Day7 != value)
                {
                    _Day7 = value;
                    OnPropertyChanged("Day7");
                }
            }
        }
        //It is string Type. Gets or sets the Day8 value
        private string _Day8;
        public string Day8
        {
            get
            {
                return _Day8;
            }
            set
            {
                if (_Day8 != value)
                {
                    _Day8 = value;
                    OnPropertyChanged("Day8");
                }
            }
        }
        /// <summary>
        /// It is string Type. Gets or sets the Day9 value
        /// </summary>
        private string _Day9;
        public string Day9
        {
            get
            {
                return _Day9;
            }
            set
            {
                if (_Day9 != value)
                {
                    _Day9 = value;
                    OnPropertyChanged("Day9");
                }
            }
        }
        /// <summary>
        /// It is string Type. Gets or sets the Day10 value
        /// </summary>
        private string _Day10;
        public string Day10
        {
            get
            {
                return _Day10;
            }
            set
            {
                if (_Day10 != value)
                {
                    _Day10 = value;
                    OnPropertyChanged("Day10");
                }
            }
        }
        //It is string Type. Gets or sets the Day11 value
        private string _Day11;
        public string Day11
        {
            get
            {
                return _Day11;
            }
            set
            {
                if (_Day11 != value)
                {
                    _Day11 = value;
                    OnPropertyChanged("Day11");
                }
            }
        }
        //It is string Type. Gets or sets the Day12 value
        private string _Day12;
        public string Day12
        {
            get
            {
                return _Day12;
            }
            set
            {
                if (_Day12 != value)
                {
                    _Day12 = value;
                    OnPropertyChanged("Day12");
                }
            }
        }
        //It is string Type. Gets or sets the Day13 value
        private string _Day13;
        public string Day13
        {
            get
            {
                return _Day13;
            }
            set
            {
                if (_Day13 != value)
                {
                    _Day13 = value;
                    OnPropertyChanged("Day13");
                }
            }
        }
        //It is string Type. Gets or sets the Day14 value
        private string _Day14;
        public string Day14
        {
            get
            {
                return _Day14;
            }
            set
            {
                if (_Day14 != value)
                {
                    _Day14 = value;
                    OnPropertyChanged("Day14");
                }
            }
        }
        //It is string Type. Gets or sets the Day15 value
        private string _Day15;
        public string Day15
        {
            get
            {
                return _Day15;
            }
            set
            {
                if (_Day15 != value)
                {
                    _Day15 = value;
                    OnPropertyChanged("Day15");
                }
            }
        }        //It is string Type. Gets or sets the Day16 value
        private string _Day16;
        public string Day16
        {
            get
            {
                return _Day16;
            }
            set
            {
                if (_Day16 != value)
                {
                    _Day16 = value;
                    OnPropertyChanged("Day16");
                }
            }
        }        //It is string Type. Gets or sets the Day17 value
        private string _Day17;
        public string Day17
        {
            get
            {
                return _Day17;
            }
            set
            {
                if (_Day17 != value)
                {
                    _Day17 = value;
                    OnPropertyChanged("Day17");
                }
            }
        }        //It is string Type. Gets or sets the Day18 value
        private string _Day18;
        public string Day18
        {
            get
            {
                return _Day18;
            }
            set
            {
                if (_Day18 != value)
                {
                    _Day18 = value;
                    OnPropertyChanged("Day18");
                }
            }
        }

        //It is string Type. Gets or sets the Day19 value
        private string _Day19;
        public string Day19
        {
            get
            {
                return _Day19;
            }
            set
            {
                if (_Day19 != value)
                {
                    _Day19 = value;
                    OnPropertyChanged("Day19");
                }
            }
        }
        //It is string Type. Gets or sets the Day20 value
        private string _Day20;
        public string Day20
        {
            get
            {
                return _Day20;
            }
            set
            {
                if (_Day20 != value)
                {
                    _Day20 = value;
                    OnPropertyChanged("Day20");
                }
            }
        }

        //It is string Type. Gets or sets the Day21 value
        private string _Day21;
        public string Day21
        {
            get
            {
                return _Day21;
            }
            set
            {
                if (_Day21 != value)
                {
                    _Day21 = value;
                    OnPropertyChanged("Day21");
                }
            }
        }
        //It is string Type. Gets or sets the Day22 value
        private string _Day22;
        public string Day22
        {
            get
            {
                return _Day22;
            }
            set
            {
                if (_Day22 != value)
                {
                    _Day22 = value;
                    OnPropertyChanged("Day22");
                }
            }
        }
        //It is string Type. Gets or sets the Day23 value
        private string _Day23;
        public string Day23
        {
            get
            {
                return _Day23;
            }
            set
            {
                if (_Day23 != value)
                {
                    _Day23 = value;
                    OnPropertyChanged("Day23");
                }
            }
        }

        //It is string Type. Gets or sets the Day24 value
        private string _Day24;
        public string Day24
        {
            get
            {
                return _Day24;
            }
            set
            {
                if (_Day24 != value)
                {
                    _Day24 = value;
                    OnPropertyChanged("Day24");
                }
            }
        }

        //It is string Type. Gets or sets the Day25 value
        private string _Day25;
        public string Day25
        {
            get
            {
                return _Day25;
            }
            set
            {
                if (_Day25 != value)
                {
                    _Day25 = value;
                    OnPropertyChanged("Day25");
                }
            }
        }

        //It is string Type. Gets or sets the Day26 value
        private string _Day26;
        public string Day26
        {
            get
            {
                return _Day26;
            }
            set
            {
                if (_Day26 != value)
                {
                    _Day26 = value;
                    OnPropertyChanged("Day26");
                }
            }
        }

        //It is string Type. Gets or sets the Day27 value
        private string _Day27;
        public string Day27
        {
            get
            {
                return _Day27;
            }
            set
            {
                if (_Day27 != value)
                {
                    _Day27 = value;
                    OnPropertyChanged("Day27");
                }
            }
        }

        //It is string Type. Gets or sets the Day28 value
        private string _Day28;
        public string Day28
        {
            get
            {
                return _Day28;
            }
            set
            {
                if (_Day28 != value)
                {
                    _Day28 = value;
                    OnPropertyChanged("Day28");
                }
            }
        }

        //It is string Type. Gets or sets the Day29 value
        private string _Day29;
        public string Day29
        {
            get
            {
                return _Day29;
            }
            set
            {
                if (_Day29 != value)
                {
                    _Day29 = value;
                    OnPropertyChanged("Day29");
                }
            }
        }

        //It is string Type. Gets or sets the Day30 value
        private string _Day30;
        public string Day30
        {
            get
            {
                return _Day30;
            }
            set
            {
                if (_Day30 != value)
                {
                    _Day30 = value;
                    OnPropertyChanged("Day30");
                }
            }
        }


        //It is string Type. Gets or sets the Day31 value
        private string _Day31;
        public string Day31
        {
            get
            {
                return _Day31;
            }
            set
            {
                if (_Day31 != value)
                {
                    _Day31 = value;
                    OnPropertyChanged("Day31");
                }
            }
        }

        //It is string Type. Gets or sets the Day32 value
        private string _Day32;
        public string Day32
        {
            get
            {
                return _Day32;
            }
            set
            {
                if (_Day32 != value)
                {
                    _Day32 = value;
                    OnPropertyChanged("Day32");
                }
            }
        }

        //It is string Type. Gets or sets the Day33 value
        private string _Day33;
        public string Day33
        {
            get
            {
                return _Day33;
            }
            set
            {
                if (_Day33 != value)
                {
                    _Day33 = value;
                    OnPropertyChanged("Day33");
                }
            }
        }

        //It is string Type. Gets or sets the Day34 value
        private string _Day34;
        public string Day34
        {
            get
            {
                return _Day34;
            }
            set
            {
                if (_Day34 != value)
                {
                    _Day34 = value;
                    OnPropertyChanged("Day34");
                }
            }
        }

        //It is string Type. Gets or sets the Day35 value
        private string _Day35;
        public string Day35
        {
            get
            {
                return _Day35;
            }
            set
            {
                if (_Day35 != value)
                {
                    _Day35 = value;
                    OnPropertyChanged("Day35");
                }
            }
        }

        //It is string Type. Gets or sets the Day36 value
        private string _Day36;
        public string Day36
        {
            get
            {
                return _Day36;
            }
            set
            {
                if (_Day36 != value)
                {
                    _Day36 = value;
                    OnPropertyChanged("Day36");
                }
            }
        }

        //It is string Type. Gets or sets the Day37 value
        private string _Day37;
        public string Day37
        {
            get
            {
                return _Day37;
            }
            set
            {
                if (_Day37 != value)
                {
                    _Day37 = value;
                    OnPropertyChanged("Day37");
                }
            }
        }

        //It is string Type. Gets or sets the Day38 value
        private string _Day38;
        public string Day38
        {
            get
            {
                return _Day38;
            }
            set
            {
                if (_Day38 != value)
                {
                    _Day38 = value;
                    OnPropertyChanged("Day38");
                }
            }
        }

        //It is string Type. Gets or sets the Day39 value
        private string _Day39;
        public string Day39
        {
            get
            {
                return _Day39;
            }
            set
            {
                if (_Day39 != value)
                {
                    _Day39 = value;
                    OnPropertyChanged("Day39");
                }
            }
        }

        //It is string Type. Gets or sets the Day40 value
        private string _Day40;
        public string Day40
        {
            get
            {
                return _Day40;
            }
            set
            {
                if (_Day40 != value)
                {
                    _Day40 = value;
                    OnPropertyChanged("Day40");
                }
            }
        }

        //It is string Type. Gets or sets the Day41 value
        private string _Day41;
        public string Day41
        {
            get
            {
                return _Day41;
            }
            set
            {
                if (_Day41 != value)
                {
                    _Day41 = value;
                    OnPropertyChanged("Day41");
                }
            }
        }

        //It is string Type. Gets or sets the Day42 value
        private string _Day42;
        public string Day42
        {
            get
            {
                return _Day42;
            }
            set
            {
                if (_Day42 != value)
                {
                    _Day42 = value;
                    OnPropertyChanged("Day42");
                }
            }
        }

        //It is string Type. Gets or sets the Day43 value
        private string _Day43;
        public string Day43
        {
            get
            {
                return _Day43;
            }
            set
            {
                if (_Day43 != value)
                {
                    _Day43 = value;
                    OnPropertyChanged("Day43");
                }
            }
        }

        //It is string Type. Gets or sets the Day44 value
        private string _Day44;
        public string Day44
        {
            get
            {
                return _Day44;
            }
            set
            {
                if (_Day44 != value)
                {
                    _Day44 = value;
                    OnPropertyChanged("Day44");
                }
            }
        }

        //It is string Type. Gets or sets the Day45 value
        private string _Day45;
        public string Day45
        {
            get
            {
                return _Day45;
            }
            set
            {
                if (_Day45 != value)
                {
                    _Day45 = value;
                    OnPropertyChanged("Day45");
                }
            }
        }

        //It is string Type. Gets or sets the Day46 value
        private string _Day46;
        public string Day46
        {
            get
            {
                return _Day46;
            }
            set
            {
                if (_Day46 != value)
                {
                    _Day46 = value;
                    OnPropertyChanged("Day46");
                }
            }
        }

        //It is string Type. Gets or sets the Day47 value
        private string _Day47;
        public string Day47
        {
            get
            {
                return _Day47;
            }
            set
            {
                if (_Day47 != value)
                {
                    _Day47 = value;
                    OnPropertyChanged("Day47");
                }
            }
        }

        //It is string Type. Gets or sets the Day48 value
        private string _Day48;
        public string Day48
        {
            get
            {
                return _Day48;
            }
            set
            {
                if (_Day48 != value)
                {
                    _Day48 = value;
                    OnPropertyChanged("Day48");
                }
            }
        }

        //It is string Type. Gets or sets the Day49 value
        private string _Day49;
        public string Day49
        {
            get
            {
                return _Day49;
            }
            set
            {
                if (_Day49 != value)
                {
                    _Day49 = value;
                    OnPropertyChanged("Day49");
                }
            }
        }

        //It is string Type. Gets or sets the Day50 value
        private string _Day50;
        public string Day50
        {
            get
            {
                return _Day50;
            }
            set
            {
                if (_Day50 != value)
                {
                    _Day50 = value;
                    OnPropertyChanged("Day50");
                }
            }
        }

        //It is string Type. Gets or sets the Day51 value
        private string _Day51;
        public string Day51
        {
            get
            {
                return _Day51;
            }
            set
            {
                if (_Day51 != value)
                {
                    _Day51 = value;
                    OnPropertyChanged("Day51");
                }
            }
        }

        //It is string Type. Gets or sets the Day52 value
        private string _Day52;
        public string Day52
        {
            get
            {
                return _Day52;
            }
            set
            {
                if (_Day52 != value)
                {
                    _Day52 = value;
                    OnPropertyChanged("Day52");
                }
            }
        }

        //It is string Type. Gets or sets the Day53 value
        private string _Day53;
        public string Day53
        {
            get
            {
                return _Day53;
            }
            set
            {
                if (_Day53 != value)
                {
                    _Day53 = value;
                    OnPropertyChanged("Day53");
                }
            }
        }

        //It is string Type. Gets or sets the Day54 value
        private string _Day54;
        public string Day54
        {
            get
            {
                return _Day54;
            }
            set
            {
                if (_Day54 != value)
                {
                    _Day54 = value;
                    OnPropertyChanged("Day54");
                }
            }
        }

        //It is string Type. Gets or sets the Day55 value
        private string _Day55;
        public string Day55
        {
            get
            {
                return _Day55;
            }
            set
            {
                if (_Day55 != value)
                {
                    _Day55 = value;
                    OnPropertyChanged("Day55");
                }
            }
        }

        //It is string Type. Gets or sets the Day56 value
        private string _Day56;
        public string Day56
        {
            get
            {
                return _Day56;
            }
            set
            {
                if (_Day56 != value)
                {
                    _Day56 = value;
                    OnPropertyChanged("Day56");
                }
            }
        }

        //It is string Type. Gets or sets the Day57 value
        private string _Day57;
        public string Day57
        {
            get
            {
                return _Day57;
            }
            set
            {
                if (_Day57 != value)
                {
                    _Day57 = value;
                    OnPropertyChanged("Day57");
                }
            }
        }

        //It is string Type. Gets or sets the Day58 value
        private string _Day58;
        public string Day58
        {
            get
            {
                return _Day58;
            }
            set
            {
                if (_Day58 != value)
                {
                    _Day58 = value;
                    OnPropertyChanged("Day58");
                }
            }
        }

        //It is string Type. Gets or sets the Day59 value
        private string _Day59;
        public string Day59
        {
            get
            {
                return _Day59;
            }
            set
            {
                if (_Day59 != value)
                {
                    _Day59 = value;
                    OnPropertyChanged("Day59");
                }
            }
        }

        //It is string Type. Gets or sets the Day60 value
        private string _Day60;
        public string Day60
        {
            get
            {
                return _Day60;
            }
            set
            {
                if (_Day60 != value)
                {
                    _Day60 = value;
                    OnPropertyChanged("Day60");
                }
            }
        }




        // Added by Saily P
        public long EventId { get; set; }
        public string EventDescription { get; set; }
        public bool IsConcurrent { get; set; }
        public string EventCode { get; set; }
    }

    public class TherapyTransactionItem : NotificationModel
    {
        /// <summary>
        /// It is long Type. Gets or sets the TransactionId.
        /// </summary>
        public long TransactionId { get; set; }
        /// <summary>
        /// It is long Type. Gets or sets the TherapyItemId
        /// </summary>
        public long TherapyItemId { get; set; }
        /// <summary>
        /// It is long and nullable Type. Gets or sets the TherapyItemTransactionId
        /// </summary>
        public long? TherapyItemTransactionId { get; set; }
        /// <summary>
        /// It is string Type. Gets or sets the DayNo
        /// </summary>
        public String DayNo { get; set; }
        /// <summary>
        /// It is datetime Type. Gets or sets the Time
        /// </summary>
        private DateTime? _Time;
        public DateTime? Time
        {
            get
            {
                return _Time;
            }
            set
            {
                if (value != _Time)
                {
                    _Time = value;
                    OnPropertyChanged("Time");
                }
            }
        }

        public String DrugNotes { get; set; }
    }

    public class clsFollicularMonitoring
    {
        public long ID { get; set; }
        public long UnitID { get; set; }
        public long TherapyId { get; set; }
        public long TherapyUnitId { get; set; }
        public DateTime? Date { get; set; }
        public long PhysicianID { get; set; }
        public string Physician { get; set; }
        public string AttachmentPath { get; set; }
        public byte[] AttachmentFileContent { get; set; }
        public string EndometriumThickness { get; set; }
        public string FollicularNotes { get; set; }
        public bool Status { get; set; }
        public string FollicularNoList { get; set; }
        public string LeftSizeList { get; set; }
        public string RightSizeList { get; set; }
        public string UserName { get; set; }

        private List<clsFollicularMonitoringSizeDetails> _SizeList;
        public List<clsFollicularMonitoringSizeDetails> SizeList
        {
            get
            {
                return _SizeList;
            }
            set
            {
                _SizeList = value;
            }
        }

    }
    public class clsFollicularMonitoringSizeDetails
    {
        public long ID { get; set; }
        public long FollicularMonitoringId { get; set; }
        public string FollicularNumber { get; set; }
        public string LeftSize { get; set; }
        public string RightSIze { get; set; }
        /* Added  By Sudhir on 16/09/2013 */
        public long OveryID { get; set; }
        public long NoOf_folicule { get; set; }
        public long SizeOf_folicule { get; set; }
        //added by neena
        public long LeftSum { get; set; }
        public long RightSum { get; set; }

    }

    public class clsTherapyDashBoardVO
    {
        public long ID { get; set; }
        public long UnitID { get; set; }
        public long PatientUintId { get; set; }
        public long PatientId { get; set; }
        public long CoupleId { get; set; }
        public long CoupleUnitId { get; set; }
        private DateTime? _TherapyDate = null;
        public DateTime? TherapyDate
        {
            get
            {
                return _TherapyDate;
            }
            set
            {
                _TherapyDate = value;
            }
        }
        //private DateTime? _TherapyTime = null;
        //public DateTime? TherapyTime
        //{
        //    get
        //    {
        //        return _TherapyTime;
        //    }
        //    set
        //    {
        //        _TherapyTime = value;
        //    }
        //}

        public DateTime TherapyTime { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string ContactNo1 { get; set; }
        public string MrNo { get; set; }
        public long PlanTherapyID { get; set; }
        public long EventTypeId { get; set; }
        public string Procedure { get; set; }
    }

    // By BHUSHAN For Surrogacy.....
    public class clsTherapyANCVO
    {
        public long TherapyID { get; set; }
        public long ThearpyUnitID { get; set; }
        private DateTime? _ANCDate;
        public DateTime? ANCDate
        {
            get { return _ANCDate; }
            set { _ANCDate = value; }
        }
        public long ANCID { get; set; }

        public string POG { get; set; }
        public string Findings { get; set; }
        public string USGReproductive { get; set; }
        public string Investigation { get; set; }
        public string Remarks { get; set; }

    }
    // By BHUSHAN For Surrogacy.....
    public class clsTherapyDeliveryVO
    {
        public long ID { get; set; }

        public long TherapyID { get; set; }
        public long ThearpyUnitID { get; set; }
        private Genders _Baby;

        private double _Weight;
        public double Weight
        {
            get { return _Weight; }
            set { _Weight = value; }
        }
        public string Mode { get; set; }
        private DateTime? _DeliveryDate;
        public DateTime? DeliveryDate
        {
            get { return _DeliveryDate; }
            set { _DeliveryDate = value; }
        }
        private DateTime? _TimeofBirth;
        public DateTime? TimeofBirth
        {
            get { return _TimeofBirth; }
            set { _TimeofBirth = value; }
        }
        public string Baby { get; set; }

    }
}
