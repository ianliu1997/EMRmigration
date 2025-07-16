using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using PalashDynamics.ValueObjects.Administration.UnitMaster;

namespace PalashDynamics.ValueObjects
{

    public enum PatientsKind
    {
        OPD = 0,
        IPD = 1,
        Registration = 2
    }

    public enum TaxApplicableOn
    {
        GrossAmt = 1,
        NetAmt = 2,
        TaxOnTax = 3
    }
    public enum FollowUpFrom
    {
        None = 0,
        Billing = 1,
        EMR = 2,
        Followup = 3,
        Prospect = 4

    }

    public enum InventoryIndentType
    {
        None = 0,
        Indent = 1,
        PurchaseRequisition = 2,
        QuarantineExpired = 3,
        QuarantineDOS = 4,
        QuarantineGRNReturn = 5,
        Direct = 6

    }

    public enum IVFLabDay
    {
        Day0 = 0,
        Day1 = 1,
        Day2 = 2,
        Day3 = 3,
        Day4 = 4,
        Day5 = 5,
        Day6 = 6,
        EmbryoTransfer = 7,

    }

    // Added For Only For Package Discount Items by CDS
    public enum PurchaseFrequencyUnit
    {
        Days = 1,
        Week = 2,
        Month = 3,
    }

    // Added For Only For IPD by CDS

    public enum Controls
    {
        TextBox = 1,
        AutoCompleteComboBox = 2,
        TimePicker = 3,
        DatePicker = 4,
        WaterMarkTextbox = 5
    }

    public enum IVFInfertilityTypes
    {
        None = 0,
        Primary = 1,
        Secondary = 2

    }

    public enum ReportType
    {
        OPDReport = 1,
        IPDReport = 2,
        EMRReport = 3,
        MISReport = 4,
        AppointmentList = 5
    }
    public enum BindingControl
    {
        DatePicker = 1,
        Time = 2,
        Text = 3,
        CheckBox = 4,
        Option = 5
    }
    public enum DischargeParameter
    {
        None = 0,
        Clinical_Findings = 1,
        Diagnosis = 2,
        Medication = 3,
        Advise = 4,
        Operating_Notes = 5,
        Investigation = 6,
        Note = 7,
        FollowUp = 8,
        Remark = 9
    }
    public enum IVFBloodLoss
    {
        None = 0,
        Heavy = 1,
        Moderate = 2,
        Scanty = 3

    }

    // Bhushan For Surrogacy
    public enum TherapyGroupSurrogate
    {
        Event = 1,
        Drug = 2,

        EmbryoTransfer = 5
    }

    public enum IVFLabWorkForm
    {
        FemaleLabDay0 = 0,
        FemaleLabDay1 = 1,
        FemaleLabDay2 = 2,
        FemaleLabDay3 = 3,
        FemaleLabDay4 = 4,
        FemaleLabDay5 = 5,
        FemaleLabDay6 = 6,
        EmbryoTransfer = 7,
        Vitrification = 8,
        OnlyVitrification = 9,
        Thawing = 10,
        ET = 11,
        OnlyET = 12,
        MediaCosting = 13

    }

    public enum VisitCurrentStatus
    {
        None = 0,
        Started = 1,
        Consulation = 2,
        Billing = 3,
        Closed = 4,
        ReOpen = 5

    }

    public enum TariffOperationType
    {
        New = 0,
        Modify = 1,
        Remove = 2
    }


    public enum BillPaymentTypes
    {
        None = 0,
        AgainstBill = 1,
        AgainstServices = 2
    }
    public enum Genders
    {
        None = 0,
        Male = 1,
        Female = 2
    }

    public enum PatientTypes
    {
        OPD = 0,
        IPD = 1,
        All = 2,
        None = 3,
        OPD_IPD = 4,
        IPDAll = 5
    }

    public enum PrescriptionFrom
    {
        Unknown = 0,
        Consultation = 1,
        IPDEMR = 2,
        OPDEMR = 3
    }

    public enum MasterTableNameList
    {
        None = 0,
        T_EmailTemplate,
        T_SMSTemplate,
        AppointmentTypeMaster,
        AreaMaster,
        M_BloodGroupMaster,
        M_BankMaster,
        M_BankBranchMaster,
        M_CabinMaster,
        M_CampMaster,
        CityMaster,
        M_CompanyAssociateMaster,
        M_CompanyMaster,
        CompanyTypeMaster,
        CountryMaster,
        M_DepartmentMaster,
        M_DepartmentUnitView,
        DependentTypeMaster,
        M_DesignationMaster,
        DoctorMaster,
        M_DoctorDepartmentView,
        M_GenderMaster,
        M_ApplicableToGenderMaster,
        M_ConditionsMaster,
        GroupMaster,
        IncomeCategoryMaster,
        LanguageMaster,
        M_LoyaltyProgramMaster,
        M_MaritalStatusMaster,
        M_DischargeType,
        M_DischargeDestination,
        M_DispensingType,
        M_DoctorTypeMaster,
        M_Molecule,
        M_ItemGroup,
        M_ItemCategory,
        M_StoreageType,
        M_Supplier,
        M_PreganancyClass,
        M_AppointmentReasonMaster,
        M_TherapeuticClass,
        M_TariffMaster,
        M_ItemCompany,
        M_Route,
        M_UnitOfMeasure,
        M_TaxMaster,
        NationalityMaster,
        M_OccupationMaster,
        RaceMaster,
        M_StaffMaster,
        M_AnesthesiaNotes,
        M_SurgeryNotes,
        //rohinee for source master l1 master
        M_CategoryL1Master,
        //rohinee dated 28/12/2015
        M_DoctorMaster
            ,
        M_MachineParameterMaster,
        M_ReligionMaster,
        ServiceMaster,
        StateMaster,
        M_UnitMaster,
        M_UnitDepartmentDetails,
        M_VisitTypeMaster,
        M_PatientCategoryMaster,
        M_PatientSourceMaster,
        M_PathAgencyMaster,
        M_AdvanceAgainst,
        M_CodeType,
        M_Specialization,
        M_ClusterMaster,
        M_SubSpecialization,
        M_ScheduleMaster,
        M_TaxNature,
        M_TermsofPayment,
        M_ModeOfPayment,
        M_Currency,
        M_Store,
        M_CompanyTypeMaster,
        M_TermAndCondition,
        M_Delivery,
        M_Schedule,
        M_PrimarySymptoms,
        T_UserRoleMaster,
        M_ItemMaster,
        Secret_Qtn,
        M_EMRTemplateConfigurationMaster,
        M_ServiceMaster,
        M_RelationMaster,
        Sys_MISType,
        M_PatientConsentMaster,

        M_EMR_PCR_SectionMaster,
        M_EMR_PCR_FieldMaster,
        M_EMR_REFERRAL_SectionMaster,
        M_EMR_REFERRAL_FieldMaster,

        M_RadFilmSize,
        M_RadTemplateResult,
        M_RadTestCategory,
        M_RadTemplateMaster,

        M_PathoCategory,
        M_PathoParameterUnits,
        M_PathoSampleMaster,
        M_PathoParameterMaster,
        M_PathoParameterCategoryMaster,
        M_MachineMaster,
        M_PathoTestMaster,

        M_OTTheatreMaster,
        M_PreOperativeInstructionsMaster,
        M_ConsentMaster,
        M_ProcedureMaster,
        M_ProcedureTypeMaster,
        M_AnesthesiaTypeMaster,
        M_OTTableMaster,
        M_OperationResultMaster,
        M_OperationStatusMaster,
        // By BHUSHAN
        M_IVF_BabyType,
        M_IVF_ETPattern,
        M_IVF_BabyFitness,
        M_TESETissueSide,
        M_ReferralTypeMaster,
        M_IVFMainIndication,
        M_IVFPlannedSpermCollection,
        M_IVFPlannedTreatment,
        M_IVFProtocolType,
        M_IVF_OveryDetails,
        M_ProcedurePerformed,
        M_Classification,
        M_IVF_Viscosity,
        M_IVF_TypeOfNeedle,
        M_IVF_Anesthesia,
        M_IVF_SpermSource,
        M_IVf_MainIndication,


        M_IVF_SourceNeedleMaster,
        M_IVF_SourceDenudingNeedleMaster,
        M_IVF_SourceOocyteMaster,
        M_IVF_SourceSemenMaster,
        M_IVF_CumulusMaster,
        M_IVF_DOSMaster,
        M_IVF_GradeMaster,
        M_IVF_MOIMaster,
        M_IVF_PICMaster,
        M_IVF_PlanMaster,
        M_IVF_MediaMaster,
        M_IVF_OPSTypeMaster,
        M_IVF_FertilizationStageMaster,
        M_IVF_MethodOfSpermPreparationMaster,
        M_IVF_CatheterTypeMaster,
        M_IVF_DifficultyTypeMaster,
        M_IVF_FragmentationMaster,
        M_IVF_BlastomereSymmetryMaster,
        M_IVF_TrophectodermMaster,
        M_IVF_BlastocytsGradeMaster,
        M_IVF_ExpansionMaster,
        M_IVF_ICMMaster,
        M_DoctorAddressMaster,

        M_IVFCanMaster,
        M_IVFTankMaster,
        M_IVFCanisterMaster,
        M_IVFStrawMaster,
        M_IVFGobletSizeMaster,
        M_IVFGobletShapeMaster,
        M_IVFGobletColor,
        M_IvfBuilt,
        M_HairColor,
        M_SkinColor,
        M_EyeColor,
        M_WardTypeMaster,

        M_WardMaster,
        M_FloorMaster,
        M_RoomMaster,
        M_ClassMaster,
        M_RoomAmmenitiesMaster,
        M_BedMaster,
        M_AdmissionType,
        M_BedAmmenitiesMaster,
        M_DietPlanMaster,
        M_FoodItemCategoryMaster,

        M_LaboratoryMaster,
        M_ResultTypeMaster,
        M_EMRFieldValues,
        M_PostThawingPlan,
        M_NoOfPlannedEmbroys,
        M_ANC_FHS,
        M_ANC_Investigation,
        M_ANC_PresentationPosition,
        M_ANC_Consult,
        M_ExpenseMaster,
        M_IVF_ClinicalSummary,

        Config_EmailSMSEventType,

        //By Anjali..................
        view_ListOfOPUNeedles,
        M_IVF_OocyteQualityMaster,
        M_IVF_ELevelOnDayMaster,
        M_IVF_InseminationLocationMaster,
        M_IVF_Incubator,
        M_IVF_Condition,
        M_IVF_Child,
        M_IVF_DealthPastPortMan,
        M_IVF_DeliveryMethod,
        M_IVF_DiedParinatallyOn,
        M_TreatmentForOocyteMaster,
        M_ANC_Hirsutism,
        M_ANC_Oedema,
        M_CountryMaster,
        M_StateMaster,
        M_CashCounterMaster,
        M_CityMaster,
        M_RegionMaster,
        M_RadModalityMaster,
        M_SurrogateAgencyMaster,
        M_ReasonForIssue,

        #region IPD
        M_BlockMaster,
        M_RefEntityMaster,
        M_IdentityMaster,
        M_IPDConsentMaster,
        #endregion


        //.....................
        M_IVFPGDCultureType,
        M_IVFBiopsy,
        M_IVFPGDBindingTechnique,
        M_IVF_ChromosomeTestOrdered,
        M_IVF_Chromosome,
        M_IVFPGDSpecimanUsed,

        //..................... 

        M_SemenColorMaster,
        M_Abstinence,

        //..................... Added By Changdeo
        M_ReportPrintFormat,
        M_ViscousRange,
        M_SemenRemarks,


        //---------------- Inventory Merge
        M_StrengthUnitMaster,
        M_ContainerMaster,
        M_ShelfMaster,
        M_RackMaster,
        M_SupplierCategory,
        M_DonorSource,
        M_IVF_InseminationmethodMaster,

        //Costing Divisions for Clinical & Pharmacy Billing
        M_IVFCostingDivision,
        //rohini
        M_IVF_DiagnosisMaster,
        M_TubeMaster,
        M_PathoFastingStatus,
        //
        LabInvestigationDepartment,
        DiagnostikBAGIAN,
        M_EMRInstructionMaster,
        GENERIK,
        M_EMRChiefComplaints,
        M_ReferenceType,

        M_ProcedureSubCategoryMaster,
        M_ProcedureCategoryMaster,
        M_ProcedureCheklistDetails,
        M_PostOperativeInstructionsMaster,
        M_IntraOperativeInstructionsMaster,
        M_DoctorNotes,
        BAGIAN_DOC,
        SPESIAL,
        SPESIAL_DOC,
        M_SpecialRegistrationMaster,
        BAGIAN,
        M_DiagnosisMaster,
        M_DoctorCategoryMaster
        ,
        M_Preffixmaster
            ,
        M_RequestApprovalMaster

            ,
        M_NationalityMaster
            ,
        M_PrefLanguageMaster
            ,
        M_TreatRequiredMaster
            ,
        M_EducationDetailsMaster
            ,
        M_ServiceL1
            ,
        M_ServiceL2
            ,
        M_ServiceL3
            ,
        M_ServiceL4
            ,
        M_CostCenterCodes //By Umesh
            //By Yogesh
            , M_ReasonMaster,

        //by neena
        M_StageofDevelopmentGrade,
        M_InnerCellMassGrade,
        M_TrophoectodermGrade,
        M_IVF_OutcomeResultMaster,
        M_IVF_OutcomePregnancyMaster,
        M_IVF_OutcomePregnancyAchievedMaster,
        M_IVF_PostThawingPlanMaster,
        M_IVf_MainSubIndication,
        M_IVf_BirthActivity,
        M_IVf_BirthPulse,
        M_IVf_BirthGrimace,
        M_IVf_BirthAppearance,
        M_IVf_BirthRespiration,
        M_IVf_BirthAPGARScore,
        M_IVf_BirthDeliveryType,
        M_IVF_NeedleMaster,
        M_IVF_TypeOfSperm,
        M_IVF_RefferingFacility,
        M_IVF_PGDIndication,
        M_IVF_AffectedPartner,
        M_IVF_FamilyHistory,
        M_IVF_BiopsyResult,
        M_IVF_ExcutionCalenderParamenterLinkingMaster,
        M_EMRTemplate, // Added by Ashish Z. on dated 10012017

        //Added by Ajit
        M_BdMaster,
        M_AgentMaster,

        M_MenuMaster,
        M_HSNCodes, //Added By Bhushanp For GST 19062017
        M_SACCodes //Added By Ashish Z. For GST 27062017 
        ,
        M_ReasonOfRefundMaster

            ,
        M_DayMaster           // added on 22032018 for New Doctor Schedule
            ,
        M_WeekDayNoMaster     // added on 22032018 for New Doctor Schedule
            , M_ProcessMaster       // Package New Changes for Procedure Added on 18042018
        ,M_GeneralLedgerMaster
    }

    public enum BillTypes
    {
        Clinical_Pharmacy = 0,
        Clinical = 1,
        Pharmacy = 2

    }
    public enum MaterPayModeList
    {


        Cash = 1,
        Cheque = 2,
        DD = 3,
        CreditCard = 4,
        DebitCard = 5,
        StaffFree = 6,
        CompanyAdvance = 7,
        PatientAdvance = 8,
        Credit = 9,
        NEFTRTGS = 10,
        FinanceCharge = 13,      //(Used for MPesa For FertilityPoint, comment by Prashant Channe on 27thNov2019)
        InsuranceCorporate = 14,  //Added by Prashant Channe for FertilityPoint by Prashant Channe on 3rdDec2019
        AdjustmentDiscount = 15, //Added by AniketK on 28Feb2019
        AdjustmentInterest = 16  //Added by AniketK on 28Feb2019

    }

    public enum MasterAddressType
    {


        ResidentalAddress = 1,
        PermanateAddress = 2,
        PolyClinic = 3,
        Nursing = 4,
        clinic = 5,
        Hospital = 6

    }

    public enum PatientSourceTypeType
    {
        Default = 0,
        Loyalty = 1,
        Camp = 2
    }

    public enum AppointmentType
    {
        Default = 0,
        Doctor = 1,
        Department = 2
    }

    public enum BlockTypes
    {
        Default = 0,
        Rack = 1,
        Shelf = 2,
        Bin = 3,
        Item = 4
    }

    //added by neena
    public enum SourceOfSperm
    {
        Donar = 1,
        Partner = 2,
        Crossover = 3
    }

    public enum FertCheck
    {
        Yes = 1,
        No = 2
    }

    public enum OocyteMaturity
    {
        GV = 1,
        MI = 2,
        MII = 3
    }

    public enum OocyteCytoplasmDysmorphisim
    {
        Present = 1,
        Absent = 2
    }

    public enum OocyteCoronaCumulusComplex
    {
        Normal = 1,
        Abnormal = 2
    }

    public enum PlanDesicion
    {
        Discard = 1,
        Cryo = 2,
        Transfer = 3,
        Donate = 4,
        DonateCryo = 5
    }

    public enum DrugSource
    {
        OvarianSuppression = 1,
        Stimulation = 2,
        Trigger = 3,
        PrescribedDrugs = 4,
        LutealPhase = 5
    }

    //

    #region For OT
    public enum PriorityType
    {
        Stat = 1,
        Regular = 2,
        TimeStamped = 3
    }

    #endregion

    #region Enums (Use for MIS Config)

    public enum StaffType
    {
        All = 1,
        Doctor = 2,
        Staff = 3
    }
    public enum ReportFormat
    {
        Pdf = 1,
        Excel = 2,

    }
    public enum ScheduleOn
    {
        Daily = 1,
        Weekly = 2,
        Monthly = 3
    }

    #endregion


    #region Therapy for Combo box
    public enum ExternalStimulation
    {
        Yes = 1,
        No = 2
    }
    public enum TherapyGroup
    {
        Event = 1,
        Drug = 2,
        UltraSound = 3,
        OvumPickUp = 4,
        EmbryoTransfer = 5,

        AMH = 6,
        E2 = 7,
        Progesterone = 8,
        LH = 9,
        FSH = 10,
        Prolactin = 11,
        TSH = 12,
        Testesterone = 13,
        BHCG = 14,

        HIV = 15,
        HCV = 16,
        HbSAg = 17

        //E2=6,
        //Progesterone=7,
        //FSH=8,
        //LH=9,
        //Prolactin=10,
        //BHCG=11
    }
    public enum EMR_Template_Applicable_Criteria
    {
        Both = 0,
        Female = 1,
        Male = 2,
        NotApplicable = 3
    }

    public enum TherapyTabs
    {
        General = 1,
        Execution = 2,
        FollicularMonitoring = 3,
        Documents = 4,
        LutealComments = 5,
        Outcome = 6,
        // By BHUSHAN..... For Surrogacy...
        ANCVisit = 7,
        Deliverydetails = 8
    }

    // By BHUSHAN

    public enum ModeofDelivery
    {
        Find = 0,
        LSIS = 1
    }


    //By Anjali............. on Date 21/12/2013
    public enum ANCTabs
    {
        GeneralDetails = 1,
        History = 2,
        Investigation = 3,
        Examination = 4,
        Documents = 5,
        Suggestion = 6

    }
    public enum IVFDashBoardTab
    {
        TabOverview = 1,
        TabIUI = 2,
        TableRepresentation = 3,
        GraphicalRepresentation = 4,
        SummedRepresentation = 5,
        Tabtransfer = 6,
        TabLutualPhase = 7,
        Taboutcome = 8,
        Tabbirthdetails = 9
    }
    public enum LevelOfDifficulty
    {
        Difficult = 1,
        Easy = 2
    }

    #endregion



    public class MasterListItem : NotificationModel
    {
        public MasterListItem()
        {

        }

        public MasterListItem(string Code, string Description)
        {
            this.Code = Code;
            this.Description = Description;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id">MasterId</param>
        /// <param name="Description">Field Description</param>
        public MasterListItem(long Id, string Description)
        {
            this.ID = Id;
            this.Description = Description;
        }

        /// <summary>
        /// MasterListItem
        /// </summary>
        /// <param name="Id">MasterId</param>
        /// <param name="Description">Field Description</param>
        public MasterListItem(long Id, string Description, DateTime? Date)
        {
            this.ID = Id;
            this.Description = Description;
            this.Date = Date;
        }


        public MasterListItem(long Id, string Code, string Description, DateTime? Date)
        {
            this.ID = Id;
            this.Code = Code;
            this.Description = Description;
            this.Date = Date;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id">MasterId</param>
        /// <param name="Description">Field Description</param>
        public MasterListItem(long Id, string Description, bool Status)
        {
            this.ID = Id;
            this.Description = Description;
            this.Status = Status;
        }

        public MasterListItem(long Id, string Description, long FilterID, double Value)
        {
            this.ID = Id;
            this.Description = Description;
            this.FilterID = FilterID;
            this.Value = Value;
        }

        public MasterListItem(long Id, string Description, double Value)
        {
            this.ID = Id;
            this.Description = Description;
            this.Value = Value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id">MasterId</param>
        /// <param name="Code">Code</param>
        /// <param name="Description">Field Description</param>
        /// <param name="Status">Record Status</param>
        public MasterListItem(long Id, string Code, string Description, bool Status)
        {
            this.ID = Id;
            this.Code = Code;
            this.Description = Description;
            this.Status = Status;
        }

        private bool _Isfree;
        public bool Isfree { get { return _Isfree; } set { _Isfree = value; } }

        private long _FreeDaysDuration;
        public long FreeDaysDuration { get { return _FreeDaysDuration; } set { _FreeDaysDuration = value; } }

        private long _ConsultationVisitTypeID;
        public long ConsultationVisitTypeID { get { return _ConsultationVisitTypeID; } set { _ConsultationVisitTypeID = value; } }






        public MasterListItem(long Id, string Description, bool Isfree, long FreeDaysDuration, long ConsultationVisitTypeID)
        {
            this.ID = Id;

            this.Description = Description;
            this.Isfree = Isfree;
            this.FreeDaysDuration = FreeDaysDuration;
            this.ConsultationVisitTypeID = ConsultationVisitTypeID;
        }


        //Added by AJ Date 2/2/2017 from get Pharmacy Package Detalils
        private double _ApplicableToAllDiscount;
        public double ApplicableToAllDiscount { get { return _ApplicableToAllDiscount; } set { _ApplicableToAllDiscount = value; } }
        private double _PharmacyFixedRate;
        public double PharmacyFixedRate { get { return _PharmacyFixedRate; } set { _PharmacyFixedRate = value; } }
        public bool _ApplicableToAll;
        public bool ApplicableToAll { get { return _ApplicableToAll; } set { _ApplicableToAll = value; } }
        private long _PackageBillID;
        public long PackageBillID { get { return _PackageBillID; } set { _PackageBillID = value; } }
        private long _PackageBillUnitID;
        public long PackageBillUnitID { get { return _PackageBillUnitID; } set { _PackageBillUnitID = value; } }
        private double _PackageConsumptionAmount;
        public double PackageConsumptionAmount { get { return _PackageConsumptionAmount; } set { _PackageConsumptionAmount = value; } }
        private long _ChargeID;
        public long ChargeID { get { return _ChargeID; } set { _ChargeID = value; } }
        //Added By Bhushanp For New Package Percentage Flow 11092017
        private double _OPDConsumption;
        public double OPDConsumption { get { return _OPDConsumption; } set { _OPDConsumption = value; } }
        private double _OpdExcludeServiceConsumption;
        public double OpdExcludeServiceConsumption { get { return _OpdExcludeServiceConsumption; } set { _OpdExcludeServiceConsumption = value; } }

        //***//
        private double _TotalPackageAdvance;
        public double TotalPackageAdvance { get { return _TotalPackageAdvance; } set { _TotalPackageAdvance = value; } }


        private double _PharmacyConsumeAmount;                      // Package New Changes Added on 20042018
        public double PharmacyConsumeAmount { get { return _PharmacyConsumeAmount; } set { _PharmacyConsumeAmount = value; } }


        private double _PackageConsumableLimit;                      // Package New Changes Added on 30042018
        public double PackageConsumableLimit { get { return _PackageConsumableLimit; } set { _PackageConsumableLimit = value; } }

        private double _ConsumableServicesBilled;                      // Package New Changes Added on 17052018
        public double ConsumableServicesBilled { get { return _ConsumableServicesBilled; } set { _ConsumableServicesBilled = value; } }

        private double _PackageClinicalTotal;                      // Package New Changes Added on 19062018
        public double PackageClinicalTotal { get { return _PackageClinicalTotal; } set { _PackageClinicalTotal = value; } }

        //Added by AJ Date 2/2/2017 from get Pharmacy Package Detalils
        public MasterListItem(long Id, string Description, long Diffrence, bool _ApplicableToAll, double ApplicableToAllDiscount, double PharmacyFixedRate, long PackageBillID, long PackageBillUnitID, long ChargeID, double PackageConsumptionAmount, double OPDConsumption, double OpdExcludeServiceConsumption, double TotalPackageAdvance, double PharmacyConsumeAmount, double PackageConsumableLimit, double ConsumableServicesBilled,double PackageClinicalTotal)
        {
            this.ID = Id;
            this.Description = Description;
            this.Diffrence = Diffrence;
            this._ApplicableToAll = _ApplicableToAll;
            this.ApplicableToAllDiscount = ApplicableToAllDiscount;
            this.PharmacyFixedRate = PharmacyFixedRate;
            this.PackageBillID = PackageBillID;
            this.PackageBillUnitID = PackageBillUnitID;
            this.ChargeID = ChargeID;
            this.PackageConsumptionAmount = PackageConsumptionAmount;
            this.OPDConsumption = OPDConsumption;
            this.OpdExcludeServiceConsumption = OpdExcludeServiceConsumption;
            this.TotalPackageAdvance = TotalPackageAdvance;
            this.PharmacyConsumeAmount = PharmacyConsumeAmount;         // Package New Changes Added on 27042018
            this.PackageConsumableLimit = PackageConsumableLimit;       // Package New Changes Added on 30042018
            this.ConsumableServicesBilled = ConsumableServicesBilled;   // Package New Changes Added on 17052018
            this.PackageClinicalTotal = PackageClinicalTotal;           // Package New Changes Added on 19062018
        }

        private long _Diffrence;
        public long Diffrence { get { return _Diffrence; } set { _Diffrence = value; } }


        //Added BY CDS 22/02/2016
        //public MasterListItem(long Id, string Code, string Description, bool Status)
        //{
        //    this.ID = Id;
        //    this.Code = Code;
        //    this.Description = Description;
        //    this.Status = Status;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id">MasterId</param>
        /// <param name="Code">Code</param>
        /// <param name="Description">Field Description</param>
        /// <param name="Status">Record Status</param>
        public string NonBatchItemBarcode { get; set; }
        public long BaseUOMID { get; set; }
        public MasterListItem(long Id, string Code, string Description, bool Status, decimal MRP, decimal PurchaseRate, decimal VatPer, string NonBatchItemBarcode, long BaseUOMID)
        {
            this.ID = Id;
            this.Code = Code;
            this.Description = Description;
            this.Status = Status;
            this.MRP = MRP;
            this.PurchaseRate = PurchaseRate;
            this.VatPer = VatPer;
            this.NonBatchItemBarcode = NonBatchItemBarcode;
            this.BaseUOMID = BaseUOMID;
        }
        //By Anjali.........

        public MasterListItem(long Id, string Code, string Description, bool Status, string column0, string column1, string column2, decimal PurchaseRate, long Column3)
        {
            this.ID = Id;
            this.Code = Code;
            this.Description = Description;
            this.Status = Status;
            this.column0 = column0;
            this.column1 = column1;
            this.column2 = column2;

            this.PurchaseRate = PurchaseRate;
            this.column3 = column3;

        }
        //

        #region Compound Drug
        private string _PrintName = string.Empty;
        public string PrintName { get { return _PrintName; } set { _PrintName = value; } }
        public long SelectedID { get; set; }
        public long SelectedID1 { get; set; }

        #endregion

        //rohinee
        public MasterListItem(long Id, string Code, string Description, bool Status, bool IsDefault)
        {
            this.ID = Id;
            this.Code = Code;
            this.Description = Description;
            this.Status = Status;
            this.IsDefault = IsDefault;

        }

        //added by neena
        public MasterListItem(long PatientID, long PatientUnitID, string Mrno, string Description)
        {
            this.PatientID = PatientID;
            this.PatientUnitID = PatientUnitID;
            this.MrNo = Mrno;
            this.Description = Description;
        }


        public MasterListItem(long Id, string Code, string Description, bool Status, long MinPoint, long MaxPoint)
        {
            this.ID = Id;
            this.Code = Code;
            this.Description = Description;
            this.Status = Status;
            this.MinPoint = MinPoint;
            this.MaxPoint = MaxPoint;
        }

        public MasterListItem(long Id, string Code, string Description, long point, bool Status)
        {
            this.ID = Id;
            this.Code = Code;
            this.Description = Description;
            this.Point = point;
            this.Status = Status;
            //this.MinPoint = MinPoint;
            //this.MaxPoint = MaxPoint;
        }

        public MasterListItem(long Id, string Code, string Name, string Description, string Flag, bool Status)
        {
            this.ID = Id;
            this.Code = Code;
            this.Name = Name;
            this.Description = Description;
            this.Flag = Flag;
            this.Status = Status;
        }

        public MasterListItem(long Id, string Code, string Name, string Description, string Flag, bool Status, long ApplyTo)
        {
            this.ID = Id;
            this.Code = Code;
            this.Name = Name;
            this.Description = Description;
            this.Flag = Flag;
            this.Status = Status;
            this.ApplyTo = ApplyTo;
        }

        public MasterListItem(long Id, string Description, bool Status, bool IsHrs)
        {
            this.ID = Id;
            this.Description = Description;
            this.Status = Status;
            this.IsHrs = IsHrs;
        }
        //
        public MasterListItem(long Id, string Code, string Description, bool Status, double Value)
        {
            this.ID = Id;
            this.Code = Code;
            this.Description = Description;
            this.Status = Status;
            this.Value = Value;
        }
        //Added By Bhushanp For GST
        public MasterListItem(long Id, string Code, string Description, bool Status, long StateID)
        {
            this.ID = Id;
            this.Code = Code;
            this.Description = Description;
            this.Status = Status;
            this.StateID = StateID;
        }

        private long _Id;
        private string _Code;
        private long _Point;
        private long _MinPoint;
        private long _MaxPoint;
        private string _Description = string.Empty;
        private string _PrintDescription = string.Empty;
        private bool _Status = false;
        public bool isChecked { get; set; }
        public long StateID { get; set; }

        // Addded by CDS
        private bool _IsEnable = true;
        public bool IsEnable
        {
            get { return _IsEnable; }
            set
            {
                if (value != _IsEnable)
                {
                    _IsEnable = value;
                    OnPropertyChanged("IsEnable");
                }
            }
        }

        private double _StockQty;

        public double StockQty
        {
            get { return _StockQty; }
            set { _StockQty = value; }
        }

        private string _Name = string.Empty;
        public string Name { get { return _Name; } set { _Name = value; } }
        private long _FilterID;
        public long FilterID { get { return _FilterID; } set { _FilterID = value; } }
        private double _Value;
        public double Value { get { return _Value; } set { _Value = value; } }

        private long _PatientID;
        public long PatientID { get { return _PatientID; } set { _PatientID = value; } }
        private long _PatientUnitID;
        public long PatientUnitID { get { return _PatientUnitID; } set { _PatientUnitID = value; } }
        private long _UnitID;
        public long UnitID { get { return _UnitID; } set { _UnitID = value; } }
        private string _MrNo;
        public string MrNo { get { return _MrNo; } set { _MrNo = value; } }

        //public string Description { get { return _Description; } set { _Description = value; } }
        private decimal _MRP;
        public decimal MRP { get { return _MRP; } set { _MRP = value; } }
        private decimal _PurchaseRate;
        public decimal PurchaseRate { get { return _PurchaseRate; } set { _PurchaseRate = value; } }
        private decimal _VatPer;
        public decimal VatPer { get { return _VatPer; } set { _VatPer = value; } }

        //By Anjali.....
        private string _column0 = string.Empty;
        private string _column1 = string.Empty;
        private string _column2 = string.Empty;
        private long _column3 = 0;
        public string column0 { get { return _column0; } set { _column0 = value; } }
        public string column1 { get { return _column1; } set { _column1 = value; } }
        public string column2 { get { return _column2; } set { _column2 = value; } }

        public long column3 { get { return _column3; } set { _column3 = value; } }

        //

        public long Point { get { return _Point; } set { _Point = value; } }

        public long MinPoint { get { return _MinPoint; } set { _MinPoint = value; } }
        public long MaxPoint { get { return _MaxPoint; } set { _MaxPoint = value; } }

        /// <summary>
        /// ParameterDirection:
        ///         Input/Output Parameter.
        /// Summary:
        ///         Id To Represent Record.
        /// </summary>
        public long ID { get { return _Id; } set { _Id = value; } }

        private long _ApplyTo;
        public long ApplyTo { get { return _ApplyTo; } set { _ApplyTo = value; } }

        private long _FragmentationID;
        public long FragmentationID { get { return _FragmentationID; } set { _FragmentationID = value; } }

        /// <summary>
        /// ParameterDirection:
        ///         Input/Output Parameter.
        /// Summary:
        ///         Code Of Record.
        /// </summary>
        public string Code { get { return _Code; } set { _Code = value; } }

        /// <summary>
        /// ParameterDirection:
        ///         Input/Output Parameter.
        /// Summary:
        ///         Description.
        /// </summary>
        public string Description { get { return _Description; } set { _Description = value; } }

        /// <summary>
        /// ParameterDirection:
        ///         Input/Output Parameter.
        /// Summary:
        ///         Description.
        /// </summary>
        public string PrintDescription { get { return _PrintDescription; } set { _PrintDescription = value; } }

        private string _UOM;
        public string UOM { get { return _UOM; } set { _UOM = value; } }

        private long _UOMID;
        public long UOMID { get { return _UOMID; } set { _UOMID = value; } }

        private long _SpecializationID;
        public long SpecializationID { get { return _SpecializationID; } set { _SpecializationID = value; } }

        private string _Route;
        public string Route
        {
            get
            {
                return _Route;
            }
            set
            {
                if (value != _Route)
                {
                    _Route = value;
                    OnPropertyChanged("Route");
                }
            }
        }


        private double _AvailableStock;
        public double AvailableStock
        {
            get { return _AvailableStock; }
            set
            {
                if (_AvailableStock != value)
                {
                    _AvailableStock = value;
                    OnPropertyChanged("AvailableStock");
                }
            }
        }

        /// <summary>
        /// ParameterDirection:
        ///         Input/Output Parameter.
        /// Summary:
        ///         Status Of Record.
        /// </summary>
        public bool Status
        {
            get { return _Status; }
            set
            {
                if (value != _Status)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
                }
            }
        }
        private bool _IsHrs;
        public bool IsHrs
        {
            get { return _IsHrs; }
            set
            {
                if (value != _IsHrs)
                {
                    _IsHrs = value;
                    OnPropertyChanged("IsHrs");
                }
            }
        }
        private bool _IsDefault;
        public bool IsDefault
        {
            get { return _IsDefault; }
            set
            {
                if (value != _IsDefault)
                {
                    _IsDefault = value;
                    OnPropertyChanged("IsDefault");
                }
            }
        }
        public DateTime? Date { get; set; }

        # region For IPD

        private string _TemplateName = string.Empty;
        public string TemplateName { get { return _TemplateName; } set { _TemplateName = value; } }

        public MasterListItem(long Id, string Description, long FilterID)
        {
            this.ID = Id;
            this.Description = Description;
            this.FilterID = FilterID;
        }

        //Added By Anjali........
        public MasterListItem(long Id, string Description, string TemplateName)
        {
            this.ID = Id;
            this.Description = Description;
            this.TemplateName = TemplateName;
        }

        #endregion

        public bool Selected { get; set; }

        public List<clsPackageServiceDetailsVO> PackageInPackageItemList = new List<clsPackageServiceDetailsVO>();

        public bool FromPackage { get; set; }

        public override string ToString()
        {
            return this.Description;
        }

        public long SponsorID { get; set; }
        public long SponsorUnitID { get; set; }

        public string Flag { get; set; }

        # region For Item Selection Control

        public bool IsItemBlock { get; set; }

        #endregion

    }

    public enum InventoryTransactionType
    {
        None = 0,
        OpeningBalance = 1,
        Indent = 2,
        Issue = 3,
        ItemReturn = 4,
        GoodsReceivedNote = 5,
        GRNReturn = 6,
        ItemsSale = 7,
        ItemSaleReturn = 8,
        ExpiryItemReturn = 9,
        ReceiveIssue = 10,
        ReceiveItemReturn = 11,
        ReceivedItemAgainstIssue = 12,
        ReceivedItemAgainstReturn = 13,
        StockAdujustment = 14,
        ScrapSale = 15,
        MaterialConsumption = 16,
        RadiologyTestConsumption = 17,
        PathologyTestConsumption = 18,

        OTDetails = 19,// for Clinical Purpose only


    }

    public enum InventoryIndentStatus
    {
        None = 0,
        New = 1,
        Cancelled = 2,
        Pending = 3,
        Completed = 4,
        ForceFullyCompleted = 5,
        Rejected = 6,
        BulkClose = 7


    }

    public enum InventoryGRNType
    {
        None = 0,
        Direct = 1,
        AgainstPO = 2
    }

    public enum InventoryStockOperationType
    {
        None = 0, Addition = 1, Subtraction = 2
    }

    public enum AmountOrPercentOperation
    {
        None = 0, Amount = 1, Percent = 2
    }

    public enum InventoryTaxApplicaleOn
    {
        None = 0,
        PurchaseRate = 1,
        MRP = 2
    }

    #region GST Details added by Ashish Z. on dated 24062017
    public enum TaxType
    {
        None = 0,
        Inclusive = 1,
        Exclusive = 2
    }
    #endregion
    public enum InventoryGoodReturnType
    {
        None = 0,
        Direct = 1,
        AgainstGRN = 2
    }

    //By Anjali................................
    public enum PatientRegistrationType
    {
        OPD = 0,
        IPD = 1,
        Pharmacy = 2,
        Tablet = 3,
        Mobile = 4,
        Pathology = 5

    }
    //.........................................

    public class clsGetLicenseDetailsBizActionVO : IBizActionValueObject
    {
        private clsUnitMasterVO _UnitDetails;
        public clsUnitMasterVO UnitDetails
        {
            get { return _UnitDetails; }
            set { _UnitDetails = value; }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private DateTime _DateToStatic;
        public DateTime DateToStatic
        {
            get { return _DateToStatic; }
            set { _DateToStatic = value; }
        }

        private string _sDate;
        public string sDate
        {
            get { return _sDate; }
            set { _sDate = value; }
        }

        private string _sTime;
        public string sTime
        {
            get { return _sTime; }
            set { _sTime = value; }
        }

        private bool _Activated;
        public bool Activated
        {
            get { return _Activated; }
            set { _Activated = value; }
        }

        private string _K1;
        public string K1
        {
            get { return _K1; }
            set { _K1 = value; }
        }

        private string _K2;
        public string K2
        {
            get { return _K2; }
            set { _K2 = value; }
        }

        private string _Key;
        public string Key
        {
            get { return _Key; }
            set { _Key = value; }
        }

        private long _Id;
        public long Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.UnitMaster.clsGetLicenseDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
        }
    }

    public class clsAddLicenseToBizActionVO : IBizActionValueObject
    {
        private clsUnitMasterVO _UnitDetails;
        public clsUnitMasterVO UnitDetails
        {
            get { return _UnitDetails; }
            set { _UnitDetails = value; }
        }

        private string _sDate;
        public string sDate
        {
            get { return _sDate; }
            set { _sDate = value; }
        }

        private string _sTime;
        public string sTime
        {
            get { return _sTime; }
            set { _sTime = value; }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private string _K2;
        public string K2
        {
            get { return _K2; }
            set { _K2 = value; }
        }

        private DateTime _RegDateTime;
        public DateTime RegDateTime
        {
            get { return _RegDateTime; }
            set { _RegDateTime = value; }
        }

        private string _K1;
        public string K1
        {
            get { return _K1; }
            set { _K1 = value; }
        }

        private string _Key;
        public string Key
        {
            get { return _Key; }
            set { _Key = value; }
        }

        private long _Id;
        public long Id
        {
            get { return _Id; }
            set { _Id = value; }
        }


        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.UnitMaster.clsAddLicenseToBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
        }
    }

    public class clsUpdateLicenseToBizActionVO : IBizActionValueObject
    {
        private clsUnitMasterVO _UnitDetails;
        public clsUnitMasterVO UnitDetails
        {
            get { return _UnitDetails; }
            set { _UnitDetails = value; }
        }

        private long _Id;
        public long Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        private string _sDate;
        public string sDate
        {
            get { return _sDate; }
            set { _sDate = value; }
        }

        private string _sTime;
        public string sTime
        {
            get { return _sTime; }
            set { _sTime = value; }
        }

        private string _sNo;
        public string sNo
        {
            get { return _sNo; }
            set { _sNo = value; }
        }

        private string _sId;
        public string sId
        {
            get { return _sId; }
            set { _sId = value; }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.UnitMaster.clsUpdateLicenseToBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
        }
    }

    public class clsdbDetailsBizActionVO : IBizActionValueObject
    {
        private long _Id;
        public long Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        private string _Value;
        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.UnitMaster.clsdbDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
        }
    }
}