using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using CIMS;
using System.IO;
using System.Windows.Resources;
using System.Xml.Linq;
using System.Reflection;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.IPD;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.Service.PalashTestServiceReference;    
using PalashDynamics.ValueObjects.NursingStation;
using PalashDynamics.ValueObjects.NursingStation.EMR;
using System.Windows.Data;
using PalashDynamics.Collections;
using System.Collections.ObjectModel;
using System.Windows.Browser;

namespace PalashDynamics.IPD.Forms
{
    public partial class frmDrugAdministrationChart : UserControl, IPreInitiateCIMS
    {
        // Localization done by Ashutosh Gupte

        #region IPreInitiateCIMS Members

        PalashDynamics.ValueObjects.Administration.clsMenuVO _SelfMenuDetails = null;

        public void PreInitiate(PalashDynamics.ValueObjects.Administration.clsMenuVO _MenuDetails)
        {
            _SelfMenuDetails = _MenuDetails;
        }

        #endregion

        public string ModuleName { get; set; }
        public string Action { get; set; }
        bool IsViewClick;
        UIElement objPatientSearch = null;
        long AdmUnitID, AdmID, ID, UnitID;
        bool IsQtyValid = true;
        bool IsQtyValidZero = true;

        private clsIPDAdmissionVO obj = null;
        private List<clsPrescriptionDetailsVO> FeedingDetails = null;


        private ObservableCollection<clsPrescriptionDetailsVO> _NewFeedingDetails = new ObservableCollection<clsPrescriptionDetailsVO>();
        public ObservableCollection<clsPrescriptionDetailsVO> NewFeedingDetails
        {
            get
            {
                return _NewFeedingDetails;
            }
            set
            {
                _NewFeedingDetails = value;
            }
        }

        private ObservableCollection<clsPrescriptionDetailsVO> _NewFeedingDetailsHistory = new ObservableCollection<clsPrescriptionDetailsVO>();
        public ObservableCollection<clsPrescriptionDetailsVO> NewFeedingDetailsHistory
        {
            get
            {
                return _NewFeedingDetailsHistory;
            }
            set
            {
                _NewFeedingDetailsHistory = value;
            }
        }

        private List<clsPrescriptionDetailsVO> ParentDrugList = null;
        private List<clsPrescriptionDetailsVO> ParentDrugListHistory = null;
        string msgText;

        //Added By CDS 
        clsPatientGeneralVO patientDetails = null;
        public string PatientName = null;
        string strMRNO = null;

        List<MasterListItem> TakenByList = new List<MasterListItem>();

        private ObservableCollection<clsPrescriptionDetailsVO> _ocSelectedItemList = new ObservableCollection<clsPrescriptionDetailsVO>();
        public ObservableCollection<clsPrescriptionDetailsVO> ocSelectedItemList
        {
            get
            {
                return _ocSelectedItemList;
            }
            set
            {
                _ocSelectedItemList = value;
            }
        }


        public PagedSortableCollectionView<clsPrescriptionDetailsVO> DataList { get; private set; }

        public int DataListPageSize
        {
            get
            {
                return DataList.PageSize;
            }
            set
            {
                if (value == DataList.PageSize) return;
                DataList.PageSize = value;
            }
        }

        PagedCollectionView _List = null;

        //public PagedCollectionView <clsPrescriptionDetailsVO> DataList { get; private set; }
        //END

        public frmDrugAdministrationChart()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(frmDrugAdministrationChart_Loaded);
        }

        #region Added By Arati For Call From Nursing Console

        long NursingConsolePatientID = 0, NursingConsolePatientUnitID = 0,
        NursingConsoleAdmissionID = 0, NursingConsoleAdmissionUnitID = 0;
        bool IsFromNursingConsole = false;
        public frmDrugAdministrationChart(long PatientID, long PatientUnitID, long AdmissionID, long AdmissionUnitID, bool IsNursingConsole)
        {
            NursingConsolePatientID = PatientID;
            NursingConsolePatientUnitID = PatientUnitID;
            NursingConsoleAdmissionID = AdmissionID;
            NursingConsoleAdmissionUnitID = AdmissionUnitID;
            IsFromNursingConsole = IsNursingConsole;
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(frmDrugAdministrationChart_Loaded);
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
            mElement.Text = "Drug Administration Chart";
        }
        #endregion

        void frmDrugAdministrationChart_Loaded(object sender, RoutedEventArgs e)
        {
            FeedingDetails = new List<clsPrescriptionDetailsVO>();
            dtpFeedingDate.SelectedDate = DateTime.Now;
            FillTakenByComboBox();
            CmdTopPatientSearch.IsEnabled = true;
            cmdPatientSearch.IsEnabled = true;

            if (IsFromNursingConsole && NursingConsolePatientID > 0) // Added By Arati For Call From Nursing Console
            {
                obj = new clsIPDAdmissionVO();
                obj.AdmID = NursingConsoleAdmissionID;
                obj.AdmUnitID = NursingConsoleAdmissionUnitID;
                AdmID = NursingConsoleAdmissionID;
                AdmUnitID = NursingConsoleAdmissionUnitID;
                FindPatient(NursingConsolePatientID, NursingConsolePatientUnitID, "");
                FillCurrentPrescription(obj.AdmID, obj.AdmUnitID);
                CmdTopPatientSearch.IsEnabled = false;
                cmdPatientSearch.IsEnabled = false;
            }


            #region dgDrugList
            //if (dgDrugsList != null)
            //{
            //    if (dgDrugsList.Columns.Count > 0)
            //    {
            //        if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
            //        {
            //            dgDrugsList.Columns[0].Header = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("grdSrNo");
            //            dgDrugsList.Columns[1].Header = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("grdDrugName");
            //            dgDrugsList.Columns[2].Header = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("grdMoleculeName");
            //            dgDrugsList.Columns[3].Header = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("grdForm");
            //            dgDrugsList.Columns[4].Header = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("grdDose");
            //            dgDrugsList.Columns[5].Header = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("grdDuration");
            //            dgDrugsList.Columns[6].Header = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("grdQuantity");
            //            dgDrugsList.Columns[7].Header = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("grdInstruction");
            //        }
            //        else
            //        {
            //            dgDrugsList.Columns[0].Header = "Sr.No.";
            //            dgDrugsList.Columns[1].Header = "Drug Name";
            //            dgDrugsList.Columns[2].Header = "Molecule Name";
            //            dgDrugsList.Columns[3].Header = "Form";
            //            dgDrugsList.Columns[4].Header = "Dose";
            //            dgDrugsList.Columns[5].Header = "Duration";
            //            dgDrugsList.Columns[6].Header = "Quantity";
            //            dgDrugsList.Columns[7].Header = "Instruction";
            //        }

            //    }
            //}
            #endregion


            #region dgFeedingDetails
            //if (dgFeedingDetails != null)
            //{
            //    if (dgFeedingDetails.Columns.Count > 0)
            //    {
            //        if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
            //        {
            //            dgFeedingDetails.Columns[0].Header = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("grdDrugName");
            //            dgFeedingDetails.Columns[1].Header = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("grdDateandTime");
            //            dgFeedingDetails.Columns[2].Header = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("grdQuantity");
            //            dgFeedingDetails.Columns[3].Header = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("grdGivenBy");
            //            dgFeedingDetails.Columns[4].Header = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("grdRemark");
            //            dgFeedingDetails.Columns[5].Header = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("grdIsFreeze");
            //        }
            //        else
            //        {
            //            dgFeedingDetails.Columns[0].Header = "Drug Name";
            //            dgFeedingDetails.Columns[1].Header = "Date and Time";
            //            dgFeedingDetails.Columns[2].Header = "Quantity";
            //            dgFeedingDetails.Columns[3].Header = "Given By";
            //            dgFeedingDetails.Columns[4].Header = "Remark";
            //            dgFeedingDetails.Columns[5].Header = "Freeze";
            //        }
            //    }
            //}
            #endregion


            #region grdCurrentPrescription
            //if (grdCurrentPrecription != null)
            //{
            //    if (grdCurrentPrecription.Columns.Count > 0)
            //    {
            //        if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
            //        {
            //            grdCurrentPrecription.Columns[0].Header = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("grdDate");
            //            grdCurrentPrecription.Columns[1].Header = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("grdPrescribedBy");
            //        }
            //        else
            //        {
            //            grdCurrentPrecription.Columns[0].Header = "Date";
            //            grdCurrentPrecription.Columns[1].Header = "Prescribed By";

            //        }

            //    }
            //}
            #endregion
            //    FillFeedingHistory();

        }

        #region Patient Search

        private void PatientSearch_Click(object sender, RoutedEventArgs e)
        {
            IsViewClick = false;
            //ModuleName = "OPDModule";
            //Action = "OPDModule.Forms.PatientSearch";
            //UserControl rootPage = Application.Current.RootVisual as UserControl;

            //WebClient c = new WebClient();
            //c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
            //c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));

            IPDPatientSearch frm = new IPDPatientSearch();
            frm.IsFromDischarge = true;
            if (IsViewClick == false)
            {
                frm.Closed += new EventHandler(cw_Closed);
            }
            frm.Show();
        }

        void c_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                UIElement myData = null;
                string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();

                XElement deploy = XDocument.Parse(appManifest).Root;
                List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                        where (assemblyParts.Attribute("Source").Value == ModuleName + ".dll")
                                        select assemblyParts).ToList();
                Assembly asm = null;
                AssemblyPart asmPart = new AssemblyPart();
                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                asm = asmPart.Load(streamInfo.Stream);

                myData = asm.CreateInstance(Action) as UIElement;
                if (myData is IInitiateCIMS)
                {
                    ((IInitiateCIMS)myData).Initiate(PatientTypes.IPD.ToString());
                    objPatientSearch = myData;

                }
                ChildWindow cw = new ChildWindow();
                cw = (ChildWindow)myData;
                cw.Loaded += new RoutedEventHandler(cw_Loaded);

                if (IsViewClick == false)
                {
                    cw.Closed += new EventHandler(cw_Closed);
                }
                cw.Show();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        void cw_Loaded(object sender, RoutedEventArgs e)
        {
            //ramesh   ((OPDModule.Forms.PatientSearch)objPatientSearch).GetDataForEmergencyPatient();
        }

        void cw_Closed(object sender, EventArgs e)
        {

            if ((bool)((PalashDynamics.IPD.IPDPatientSearch)sender).DialogResult)     //if ((bool)((OPDModule.Forms.PatientSearch)sender).DialogResult)
            {
                GetSelectedPatientDetails();
            }
            else
            {
                //txtMRNo.Text = "";
                //Comman.SetDefaultHeader(_SelfMenuDetails);
            }
        }



        private void GetSelectedPatientDetails()
        {
            //Comment
            if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
            {
                long PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                long UnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                AdmID = ((IApplicationConfiguration)App.Current).SelectedPatient.IPDAdmissionID;
                AdmUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.AdmissionUnitID;

                FindPatient(PatientID, UnitId, null);
                obj = new clsIPDAdmissionVO();
                obj.AdmID = ((IApplicationConfiguration)App.Current).SelectedPatient.IPDAdmissionID;
                obj.AdmUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.AdmissionUnitID;
            }

            //ramesh
            //if (((IApplicationConfiguration)App.Current).SelectedPatientDetails != null)
            //{
            //    long PatientID = ((IApplicationConfiguration)App.Current).SelectedPatientDetails.PatientID;
            //    long UnitId = ((IApplicationConfiguration)App.Current).SelectedPatientDetails.UnitId;

            //    FindPatient(PatientID, UnitId, null);
            //}
            //ramesh
        }

        private void FindPatient(long PatientID, long PatientUnitId, string MRNO)
        {
            WaitIndicator Indicatior = new WaitIndicator();
            Indicatior.Show();
            //clsGetPatientBizActionVO BizAction = new clsGetPatientBizActionVO();
            clsGetIPDPatientBizActionVO BizAction = new clsGetIPDPatientBizActionVO();
            BizAction.PatientDetails = new clsGetIPDPatientVO();
            BizAction.PatientDetails.GeneralDetails.PatientID = PatientID;
            BizAction.PatientDetails.GeneralDetails.UnitId = PatientUnitId;
            BizAction.PatientDetails.GeneralDetails.MRNo = MRNO;
            BizAction.PatientDetails.GeneralDetails.IsIPD = true;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        patientDetails = new clsPatientGeneralVO();
                        patientDetails = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails;

                        if (!((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.PatientID.Equals(0))
                        {
                            //BindSelectedPatientDetails((clsGetPatientBizActionVO)arg.Result, Indicatior);

                            PatientName = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.IPDPatientName.ToString();
                            UnitID = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.PatientUnitID;
                            PatientName = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.PatientName;
                            AdmID = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.IPDAdmissionID;
                            AdmUnitID = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.PatientUnitID;

                            //IPDAdmissionNo = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.IPDAdmissionNo;
                            PatientName = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.IPDPatientName.ToString();
                            PatientID = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.PatientID;
                            //_PatientID = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.PatientID;
                            //_PatientUnitID = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.PatientUnitID;
                            if (((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.MRNo != null)
                            {
                                strMRNO = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.MRNo;
                                txtMRNumber.Text = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.MRNo;
                                CmdSave.IsEnabled = true;
                            }
                            if (((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.IPDAdmissionNo != null)
                            {
                                txtIPDNo.Text = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.IPDAdmissionNo;
                            }
                            if (patientDetails.PatientID != null)
                                patientDetails.PatientID = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.PatientID;
                            if (patientDetails.PatientUnitID != null)
                                patientDetails.PatientUnitID = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.PatientUnitID;
                            if (patientDetails.GenderID != null)
                                patientDetails.GenderID = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.GenderID;

                            if (patientDetails.ClassName != null)
                            {
                                patientDetails.ClassName = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.ClassName;
                                ////txtBedCategory.Text = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.ClassName;
                                lblbedName.Text = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.BedName;
                     
                            }
                            if (patientDetails.WardName != null)
                            {
                                ////txtWard.Text = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.WardName;
                                patientDetails.WardName = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.WardName;
                                lblwardName.Text = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.WardName;
                            }
                            if (patientDetails.BedID != null && patientDetails.BedName != null)
                            {
                                int BedId = Convert.ToInt32(((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.BedID);
                                ////txtBed.Text = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.BedName;
                            }
                            if (((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.RegistrationDate != null)
                            {
                                //   txtAdmDate.Text = Convert.ToString(((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.RegistrationDate);
                                txtAdmDate.Text = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.RegistrationDate.Value.Date.ToString("dd/MM/yyy");

                            }
                            if (PatientName != null)
                            {
                                lblPatientName.Text = PatientName;
                            }
                            if (patientDetails.DoctorName != null)
                                txtDoctor.Text = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.DoctorName;
                            if (patientDetails.Unit != null)
                                txtUnit.Text = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.Unit;


                            //checkBedReleasd((clsGetPatientBizActionVO)arg.Result, Indicatior);
                            checkBedReleasd((clsGetIPDPatientBizActionVO)arg.Result, Indicatior);
                        }
                        else
                        {
                            Indicatior.Close();
                            if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                            {
                                msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("MRChkValidation_Msg");
                            }
                            else
                            {
                                msgText = "Please check MR Number.";
                            }
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                      new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            txtMRNumber.Focus();
                            msgW1.Show();
                            Comman.SetDefaultHeader(_SelfMenuDetails);
                        }
                    }
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        //private void checkBedReleasd(clsGetPatientBizActionVO PatientVO, WaitIndicator Indicatior)

        private void checkBedReleasd(clsGetIPDPatientBizActionVO PatientVO, WaitIndicator Indicatior)
        {
            clsGetBillAndBedByAdmIDAndAdmUnitIDBizActionVO BizAction = new clsGetBillAndBedByAdmIDAndAdmUnitIDBizActionVO();
            BizAction.DischargeList = new List<clsIPDDischargeVO>();
            BizAction.DischargeDetails = new clsIPDDischargeVO();
            BizAction.DischargeDetails.AdmID = ((IApplicationConfiguration)App.Current).SelectedPatient.IPDAdmissionID;  //PatientVO.PatientDetails.VisitAdmID;
            BizAction.DischargeDetails.AdmUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.AdmissionUnitID;  //PatientVO.PatientDetails.VisitAdmUnitID;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    clsGetBillAndBedByAdmIDAndAdmUnitIDBizActionVO result = arg.Result as clsGetBillAndBedByAdmIDAndAdmUnitIDBizActionVO;
                    if (result != null)
                    {
                        string msgTitle = "";
                        string msgText = string.Empty;
                        if (result.IsBedRelease.Equals(true))
                        {
                            //msgText = "Bed is already released.";
                            if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                            {
                                msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("BedAlreadyRelease_Msg");
                            }
                            else
                            {
                                msgText = "Bed is already released.";
                            }
                        }
                        else
                        {
                            BindSelectedPatientDetails(PatientVO, Indicatior);
                        }

                        if (!string.IsNullOrEmpty(msgText))
                        {
                            Comman.SetDefaultHeader(_SelfMenuDetails);
                            Indicatior.Close();
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW.Show();
                        }
                    }
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        //private void BindSelectedPatientDetails(clsGetPatientBizActionVO PatientVO, WaitIndicator Indicatior)
        private void BindSelectedPatientDetails(clsGetIPDPatientBizActionVO PatientVO, WaitIndicator Indicatior)
        {
            txtMRNumber.Text = PatientVO.PatientDetails.GeneralDetails.MRNo;
            //txtMRNoSearch.Text = PatientVO.PatientDetails.GeneralDetails.MRNo;

            clsPatientGeneralVO patientDetails = new clsPatientGeneralVO();
            patientDetails.PatientID = PatientVO.PatientDetails.GeneralDetails.PatientID;
            patientDetails.PatientUnitID = PatientVO.PatientDetails.GeneralDetails.UnitId;

            //Comman.SetPatientDetailHeader(PatientVO.PatientDetails);

            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

            mElement.Text = " : " + PatientName;

            mElement.Text += " - " + PatientVO.PatientDetails.GeneralDetails.MRNo + " : " + PatientVO.PatientDetails.GeneralDetails.Gender;

            //mElement.Text = " : " + _Patient.FirstName + " " + _Patient.MiddleName + " " + _Patient.LastName;

            //mElement.Text += " - " + _Patient.GeneralDetails.MRNo + " : " + _Patient.Gender;

            Indicatior.Close();
            if (IsViewClick == true)
            {
                ((IApplicationConfiguration)App.Current).SelectedIPDPatient = new clsIPDAdmissionVO();                  //((IApplicationConfiguration)App.Current).SelectedPatientDetails = new clsPatientGeneralVO();
                ((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientId = patientDetails.PatientID;       //((IApplicationConfiguration)App.Current).SelectedPatientDetails.PatientID = patientDetails.PatientID;
                ((IApplicationConfiguration)App.Current).SelectedIPDPatient.UnitId = patientDetails.PatientUnitID;      //((IApplicationConfiguration)App.Current).SelectedPatientDetails.UnitId = patientDetails.PatientUnitID;


                ModuleName = "OPDModule";
                Action = "OPDModule.PatientAndVisitDetails";
                ////////////UserControl rootPage = Application.Current.RootVisual as UserControl;

                //TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                //mElement.Text = "Inventory Configuration";

                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                obj = new clsIPDAdmissionVO();

                //obj.AdmID = ((IApplicationConfiguration)App.Current).SelectedPatient.IPDAdmissionID; //PatientVO.PatientDetails.VisitAdmID;
                //obj.AdmUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.AdmissionUnitID; //PatientVO.PatientDetails.VisitAdmUnitID;

                obj.AdmID = AdmID;              //((IApplicationConfiguration)App.Current).SelectedPatient.IPDAdmissionID;
                obj.AdmUnitID = AdmUnitID;      //((IApplicationConfiguration)App.Current).SelectedPatient.AdmissionUnitID;

                FillCurrentPrescription(obj.AdmID, obj.AdmUnitID);
            }
        }

        private void CmdFindPatient_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMRNumber.Text))
            {
                FindPatient(0, ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID, txtMRNumber.Text.Trim());
            }
            else
            {
                FeedingDetails = new List<clsPrescriptionDetailsVO>();
                ParentDrugList = new List<clsPrescriptionDetailsVO>();
                ParentDrugListHistory = new List<clsPrescriptionDetailsVO>();
                grdCurrentPrecription.ItemsSource = null;
                dgDrugsList.ItemsSource = null;
                dgFeedingDetails.ItemsSource = null;
                dgFeedingDetailsHistory.ItemsSource = null;
                cmbDrugName.ItemsSource = null;
                if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                {
                    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("MRChkValidation_Msg");
                }
                else
                {
                    msgText = "Please check MR Number.";
                }
                MessageBoxControl.MessageBoxChildWindow msgW1 = new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                txtMRNumber.Focus();
                msgW1.Show();
                Comman.SetDefaultHeader(_SelfMenuDetails);
            }
        }
        #endregion

        private void FillCurrentPrescription(long AdmissionID, long AdmissionUnitID)
        {
            FillFeedingHistory(AdmissionUnitID, AdmissionID);
            clsDrugAdministrationChartBizActionVO BizAction = new clsDrugAdministrationChartBizActionVO();
            BizAction.DrugAdministrationChart = new clsDrugAdministrationChartVO();
            BizAction.DrugAdministrationChart.AdmissionID = AdmissionID;
            BizAction.DrugAdministrationChart.AdmissionUnitID = AdmissionUnitID;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        clsDrugAdministrationChartBizActionVO Objresult = new clsDrugAdministrationChartBizActionVO();
                        Objresult = (clsDrugAdministrationChartBizActionVO)arg.Result;

                        grdCurrentPrecription.ItemsSource = null;
                        grdCurrentPrecription.ItemsSource = Objresult.PrescriptionMasterList;
                        grdCurrentPrecription.SelectedItem = null;
                        FeedingDetails = new List<clsPrescriptionDetailsVO>();

                        ClearDatagrid();

                    }
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }

        //private void cmdPastDetails_Click(object sender, RoutedEventArgs e)
        //{

        //}

        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (CheckValidation())
            {
                string msgTitle = "";
                // string msgText = "Are you sure you want to save the Feeding Details?";
                if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                {
                    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("SaveVerification_Msg");
                }
                else
                {
                    msgText = "Are you sure you want to save ?";
                }
                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                msgW.Show();
            }
        }

        private bool CheckValidation()
        {
            var Result = from r in ParentDrugList
                         where (r.IsChecked == true)
                         select r;
            bool IsBreak = false;
            bool result = true;
            double getQty = 0;
            double totalConsumeQuantity = 0;
            foreach (var it in ParentDrugList)
            {
                foreach (var item in NewFeedingDetails)
                {
                    totalConsumeQuantity = it.ConsumeQuantity;
                    if (item.Quantity <= 0)
                    {
                        IsQtyValidZero = false;
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Quantity Should Be Greater Than Zero", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                        msgW1.Show();
                        IsBreak = true;
                        result = false;
                        break;
                    }
                    //if (item.IsFreeze == false)
                    //{
                    //    IsQtyValidZero = false;
                    //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                    //                              new MessageBoxControl.MessageBoxChildWindow("Palash", " Please Freeze All Druges ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                    //    msgW1.Show();
                    //    IsBreak = true;
                    //    result = false;
                    //    break;
                    //}

                    //if (item.DrugId == it.DrugId) // 
                    //{
                    //    if (item.Quantity == it.ConsumeQuantity)
                    //    {
                    //        //if (item.Quantity)
                    //        //{
                    //        MessageBoxControl.MessageBoxChildWindow msgW1 =
                    //                                      new MessageBoxControl.MessageBoxChildWindow("Palash", item.ItemName + " Drugs  Quantity  Already Consumed:" + item.DrugName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                    //        msgW1.Show();
                    //        IsBreak = true;
                    //        result = false;
                    //        break;

                    //    }
                    //}

                    if (item.DrugId == it.DrugId) // 
                    {
                        // if (item.Quantity != 0 && item.Quantity > 0)
                        //  getQty = getQty + item.Quantity;

                        if (it.Quantity == it.ConsumeQuantity)
                        {

                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Quantity Already Consumed For Drug:" + item.DrugName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                            msgW1.Show();
                            IsBreak = true;
                            result = false;
                            break;


                        }
                    }

                    if (item.DrugId == it.DrugId) // 
                    {
                        // if (item.Quantity != 0 && item.Quantity > 0)
                        getQty = getQty + item.Quantity + it.ConsumeQuantity;

                        if (getQty > it.Quantity)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                              new MessageBoxControl.MessageBoxChildWindow("Palash", item.DrugName + "  " + it.Quantity + " Quantity Greater Than The pending  " + (it.Quantity - it.ConsumeQuantity) + "  Quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                            msgW1.Show();
                            IsBreak = true;
                            result = false;
                            break;


                        }
                    }



                    if (item.Quantity <= 0)
                    {
                        IsQtyValidZero = false;
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Quantity Should Be Less Than The Drug Quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                        msgW1.Show();
                        result = false;
                        break;
                    }
                    else
                    {
                        IsQtyValidZero = true;
                    }
                    if (getQty > it.Quantity)
                    {
                        //MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Quantity Should Be Less Than Selected Drug Quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                        //msgW1.Show();
                        IsQtyValid = false;

                        //  getQty = 0;


                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Quantity Should Be Less Than The Drug Quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                        msgW1.Show();
                        result = false;
                        IsBreak = true;

                        break;
                        //((CheckBox)sender).IsChecked = false;
                    }

                    //else
                    //{
                    //    IsQtyValid = true;
                    //   // getQty = 0;
                    //}
                }
                getQty = 0;
                if (IsBreak)
                {
                    result = false;
                    break;
                }


            }

            return result;
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (CheckValidation())
            {
                if (result == MessageBoxResult.Yes)
                {
                    SaveFeedingDetails();
                }
            }
        }

        private void SaveFeedingDetails()
        {
            clsSaveDrugFeedingDetailsBizActionVO BizAction = new clsSaveDrugFeedingDetailsBizActionVO();
            //BizAction.DrugFeedingList = new List<clsPrescriptionDetailsVO>();

            //BizAction.DrugFeedingList = FeedingDetails;
            //FeedingDetails = ((List<clsPrescriptionDetailsVO>)(dgFeedingDetails.ItemsSource));

            BizAction.DrugFeedingListObserv = new ObservableCollection<clsPrescriptionDetailsVO>();
            NewFeedingDetails = ((ObservableCollection<clsPrescriptionDetailsVO>)(dgFeedingDetails.ItemsSource));
            //BizAction.DrugFeedingList = FeedingDetails;
            BizAction.DrugFeedingListObserv = NewFeedingDetails;

            BizAction.PrescriptionID = ((clsPrescriptionMasterVO)grdCurrentPrecription.SelectedItem).ID;
            BizAction.PrescriptionUnitID = ((clsPrescriptionMasterVO)grdCurrentPrecription.SelectedItem).UnitID;

            BizAction.OPD_IPD = ((clsPrescriptionMasterVO)grdCurrentPrecription.SelectedItem).OPD_IPD;
            BizAction.Opd_Ipd_Id = ((clsPrescriptionMasterVO)grdCurrentPrecription.SelectedItem).Opd_Ipd_Id;
            BizAction.Opd_Ipd_UnitID = ((clsPrescriptionMasterVO)grdCurrentPrecription.SelectedItem).Opd_Ipd_UnitID;
            // BizAction.=((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.Name
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {

                        if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                        {
                            msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("RecordSaved_Msg");
                        }
                        else
                        {
                            msgText = "Record saved successfully.";
                            // FillFeedingDetails();
                            FillDrugList();
                            NewFeedingDetails.Clear();
                            dgFeedingDetails.ItemsSource = null;
                            FillFeedingHistory(((clsPrescriptionMasterVO)grdCurrentPrecription.SelectedItem).UnitID, ((clsPrescriptionMasterVO)grdCurrentPrecription.SelectedItem).Opd_Ipd_Id);

                        }
                        MessageBoxControl.MessageBoxChildWindow msg = null;
                        msg = new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msg.Show();

                        ////FeedingDetails = new List<clsPrescriptionDetailsVO>();
                        ////dgFeedingDetails.ItemsSource = null;
                        ////grdCurrentPrecription.SelectedItem = null;

                        // Added By CDS For Filling The Data
                        //FillDrugList();
                    }
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            if (IsFromNursingConsole)//Added By Arati for Call From Nursing Console
            {
                //Ramesh
                //frmNursingConsole NursingConsole = new frmNursingConsole(true, NursingConsolePatientID, NursingConsolePatientUnitID);
                //((IApplicationConfiguration)App.Current).OpenMainContent(NursingConsole);
                //UserControl rootPage = Application.Current.RootVisual as UserControl;
                //TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                //mElement.Text = "Nursing Console";
                //Ramesh
            }
            else
            {
                frmAdmissionList objAdm = new frmAdmissionList();
                UIElement myData = objAdm;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Admission List";
            }
        }

        private void CmdAdd_Click(object sender, RoutedEventArgs e)
        {

            if (ValidateFeeding())
            {

                clsPrescriptionDetailsVO item = new clsPrescriptionDetailsVO();
                item.RowID = FeedingDetails.Count + 1;
                item.DrugId = ((MasterListItem)cmbDrugName.SelectedItem).ID;
                item.DrugName = ((MasterListItem)cmbDrugName.SelectedItem).Description;

                item.TakenID = ((MasterListItem)cmbTakenBy.SelectedItem).ID;
                item.TakenBy = ((MasterListItem)cmbTakenBy.SelectedItem).Description;

                item.Quantity = Convert.ToDouble(txtQuantity.Text);
                item.Remark = txtRemark.Text;
                item.IsDelete = true;
                DateTime dt = new DateTime(dtpFeedingDate.SelectedDate.Value.Year, dtpFeedingDate.SelectedDate.Value.Month, dtpFeedingDate.SelectedDate.Value.Day, tpTime.Value.Value.Hour, tpTime.Value.Value.Minute, tpTime.Value.Value.Second);
                item.Date = dt;

                FeedingDetails.Add(item);

                if (!ValidateDrugDetails())
                {
                    FeedingDetails.RemoveAt(FeedingDetails.Count - 1);

                    if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                    {
                        msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("QtyNotExceedForSelectedDrug_Msg");
                    }
                    else
                    {
                        msgText = "Quantity should not exceed for selected drug.";
                    }
                    MessageBoxControl.MessageBoxChildWindow msg = null;
                    msg = new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);

                    msg.Show();
                }
                else
                {
                    dgFeedingDetails.ItemsSource = null;
                    dgFeedingDetails.ItemsSource = FeedingDetails;
                    UpdateDrugList();
                    ClearFeedingDetails();
                }

            }
            else
            {
                if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                {
                    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("RemarkCompulsory_Msg");
                }
                else
                {
                    msgText = "Values other than remark are compulsory.";
                }
                MessageBoxControl.MessageBoxChildWindow msg = null;
                msg = new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                msg.Show();
            }
        }

        private void grdCurrentPrecription_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grdCurrentPrecription.SelectedItem != null)
            {
                FillDrugList();
            }
        }

        private void FillDrugList()
        {
            clsGetDrugListForDrugChartBizActionVO BizAction = new clsGetDrugListForDrugChartBizActionVO();
            BizAction.DrugList = new List<clsPrescriptionDetailsVO>();
            BizAction.PrescriptionID = ((clsPrescriptionMasterVO)grdCurrentPrecription.SelectedItem).ID;
            BizAction.PrescriptionUnitID = ((clsPrescriptionMasterVO)grdCurrentPrecription.SelectedItem).UnitID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        //PagedCollectionView _List = null;
                        List<MasterListItem> DrugList = new List<MasterListItem>();
                        clsGetDrugListForDrugChartBizActionVO Objresult = new clsGetDrugListForDrugChartBizActionVO();
                        Objresult = (clsGetDrugListForDrugChartBizActionVO)arg.Result;

                        DrugList.Add(new MasterListItem(0, "--Select--"));
                        foreach (clsPrescriptionDetailsVO item in Objresult.DrugList)
                        {
                            MasterListItem obj = new MasterListItem(item.DrugId, item.DrugName);
                            DrugList.Add(obj);
                        }

                        ParentDrugList = new List<clsPrescriptionDetailsVO>();
                        ParentDrugList = Objresult.DrugList;
                        ParentDrugListHistory = new List<clsPrescriptionDetailsVO>();
                        ParentDrugListHistory = Objresult.DrugList;
                        _List = new PagedCollectionView(Objresult.DrugList);
                        _List.PageSize = 5;
                        //_List.PageSize = 10;

                        dgDrugsList.ItemsSource = null;
                        dgDrugsList.ItemsSource = _List;
                        dgDrugsList.SelectedItem = null;

                        dataGridDrugsListPager.Source = null;
                        dataGridDrugsListPager.Source = _List;

                        cmbDrugName.ItemsSource = null;
                        cmbDrugName.ItemsSource = DrugList;
                        cmbDrugName.SelectedItem = DrugList[0];

                        //  FillFeedingDetails();
                        //  FillFeedingDetailsHistory();

                        //FillDrugCombo(DrugList);
                    }
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillTakenByComboBox()
        {
            clsGetUnitWiseEmpBizActionVO BizAction = new clsGetUnitWiseEmpBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();


            BizAction.UnitID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetUnitWiseEmpBizActionVO)arg.Result).MasterList);

                    //cmbTakenBy.ItemsSource = null;
                    //cmbTakenBy.ItemsSource = objList;

                    TakenByList = objList;


                    if (objList.SingleOrDefault(S => S.ID.Equals(((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.EmployeeID)) != null)
                        cmbTakenBy.SelectedItem = objList.SingleOrDefault(S => S.ID.Equals(((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.EmployeeID));

                }
            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillDrugCombo(List<MasterListItem> DrugList)
        {
            cmbDrugName.ItemsSource = null;
            cmbDrugName.ItemsSource = DrugList;
            cmbDrugName.SelectedItem = DrugList[0];
        }

        private void UpdateDrugList()
        {
            List<MasterListItem> ListofDrug = new List<MasterListItem>();
            ListofDrug = ((List<MasterListItem>)cmbDrugName.ItemsSource);
            foreach (var drug in ListofDrug)
            {
                double sum = 0;
                var ObjList = (from item in ParentDrugList
                               where item.DrugId == drug.ID//((MasterListItem)cmbDrugName.SelectedItem).ID
                               select item).ToList();

                if (ObjList.Count > 0)
                {
                    sum = (from obj in FeedingDetails
                           where obj.DrugId == ObjList[0].DrugId
                           select obj).Sum(S => S.Quantity);

                    if (sum >= ObjList[0].Quantity)
                    {
                        List<MasterListItem> List = (from list in ((List<MasterListItem>)cmbDrugName.ItemsSource)
                                                     where list.ID != ObjList[0].DrugId
                                                     select list).ToList();
                        FillDrugCombo(List);
                    }
                }
            }

        }

        private bool ValidateFeeding()
        {
            if (string.IsNullOrEmpty(txtQuantity.Text))//string.IsNullOrEmpty(txtRemark.Text) ||
                return false;

            if ((MasterListItem)cmbDrugName.SelectedItem == null || ((MasterListItem)cmbDrugName.SelectedItem).ID < 1)
                return false;

            if ((MasterListItem)cmbTakenBy.SelectedItem == null || ((MasterListItem)cmbTakenBy.SelectedItem).ID < 1)
                return false;

            if (tpTime.Value == null)
                return false;



            return true;
        }

        private bool ValidateDrugDetails()
        {
            double sum = 0;
            var Obj = (from item in ParentDrugList
                       where item.DrugId == ((MasterListItem)cmbDrugName.SelectedItem).ID
                       select item).ToList();

            sum = (from obj in FeedingDetails
                   where obj.DrugId == ((MasterListItem)cmbDrugName.SelectedItem).ID
                   select obj).Sum(S => S.Quantity);

            if (sum > Obj[0].Quantity)
                return false;

            return true;
        }

        private void ClearFeedingDetails()
        {
            tpTime.Value = null;
            txtQuantity.Text = string.Empty;
            txtRemark.Text = string.Empty;
            cmbDrugName.SelectedItem = ((List<MasterListItem>)cmbDrugName.ItemsSource).SingleOrDefault(S => S.ID == 0);
            cmbTakenBy.SelectedItem = ((List<MasterListItem>)cmbTakenBy.ItemsSource).SingleOrDefault(S => S.ID == 0);
        }

        private void ClearDatagrid()
        {
            dgDrugsList.ItemsSource = null;
            dataGridDrugsListPager.Source = null;

            dgFeedingDetails.ItemsSource = null;
            cmbDrugName.ItemsSource = null;

            //  dgFeedingDetailsHistory.ItemsSource = null;

        }
        private void FillFeedingHistory(long Opd_Ipd_UnitID, long Opd_Ipd_Id)
        {
            //if(grdCurrentPrecription.

            clsGetFeedingDetailsBizActionVO BizAction = new clsGetFeedingDetailsBizActionVO();
            BizAction.DrugFeedingList = new List<clsPrescriptionDetailsVO>();

            BizAction.PrescriptionID = 0;
            BizAction.PrescriptionUnitID = Opd_Ipd_UnitID;

            BizAction.OPD_IPD = 1;
            BizAction.Opd_Ipd_Id = Opd_Ipd_Id;
            BizAction.Opd_Ipd_UnitID = Opd_Ipd_UnitID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        clsGetFeedingDetailsBizActionVO Objresult = new clsGetFeedingDetailsBizActionVO();
                        Objresult = (clsGetFeedingDetailsBizActionVO)arg.Result;

                        NewFeedingDetailsHistory.Clear();

                        if (((clsGetFeedingDetailsBizActionVO)arg.Result).DrugFeedingList.Count > 0)
                        {

                            for (int i = 0; i < ((clsGetFeedingDetailsBizActionVO)arg.Result).DrugFeedingList.Count; i++)
                            {
                                ((clsGetFeedingDetailsBizActionVO)arg.Result).DrugFeedingList[i].IsChecked = true;

                                ((clsGetFeedingDetailsBizActionVO)arg.Result).DrugFeedingList[i].ListTakenBy = TakenByList;
                                if (Convert.ToInt64(((clsGetFeedingDetailsBizActionVO)arg.Result).DrugFeedingList[i].TakenID) > 0)
                                {
                                    ((clsGetFeedingDetailsBizActionVO)arg.Result).DrugFeedingList[i].SelectedTakenBy = TakenByList.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetFeedingDetailsBizActionVO)arg.Result).DrugFeedingList[i].TakenID));
                                }
                                else
                                {
                                    ((clsGetFeedingDetailsBizActionVO)arg.Result).DrugFeedingList[i].SelectedTakenBy = TakenByList.FirstOrDefault(p => p.ID == 0);
                                }

                                NewFeedingDetailsHistory.Add(((clsGetFeedingDetailsBizActionVO)arg.Result).DrugFeedingList[i]);


                            }


                            var Result = from r in NewFeedingDetailsHistory
                                         where (r.IsFreeze == true)
                                         select r;

                            dgFeedingDetailsHistory.ItemsSource = null;
                            dgFeedingDetailsHistory.ItemsSource = Result;


                        }
                        else
                        {

                            dgFeedingDetailsHistory.ItemsSource = null;
                        }
                    }
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();


        }

        private void FillFeedingDetails()
        {
            clsGetFeedingDetailsBizActionVO BizAction = new clsGetFeedingDetailsBizActionVO();
            BizAction.DrugFeedingList = new List<clsPrescriptionDetailsVO>();

            BizAction.PrescriptionID = ((clsPrescriptionMasterVO)grdCurrentPrecription.SelectedItem).ID;
            BizAction.PrescriptionUnitID = ((clsPrescriptionMasterVO)grdCurrentPrecription.SelectedItem).UnitID;

            BizAction.OPD_IPD = ((clsPrescriptionMasterVO)grdCurrentPrecription.SelectedItem).OPD_IPD;
            BizAction.Opd_Ipd_Id = ((clsPrescriptionMasterVO)grdCurrentPrecription.SelectedItem).Opd_Ipd_Id;
            BizAction.Opd_Ipd_UnitID = ((clsPrescriptionMasterVO)grdCurrentPrecription.SelectedItem).Opd_Ipd_UnitID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        clsGetFeedingDetailsBizActionVO Objresult = new clsGetFeedingDetailsBizActionVO();
                        Objresult = (clsGetFeedingDetailsBizActionVO)arg.Result;

                        NewFeedingDetails.Clear();

                        if (((clsGetFeedingDetailsBizActionVO)arg.Result).DrugFeedingList.Count > 0)
                        {

                            for (int i = 0; i < ((clsGetFeedingDetailsBizActionVO)arg.Result).DrugFeedingList.Count; i++)
                            {
                                ((clsGetFeedingDetailsBizActionVO)arg.Result).DrugFeedingList[i].IsChecked = true;

                                ((clsGetFeedingDetailsBizActionVO)arg.Result).DrugFeedingList[i].ListTakenBy = TakenByList;
                                if (Convert.ToInt64(((clsGetFeedingDetailsBizActionVO)arg.Result).DrugFeedingList[i].TakenID) > 0)
                                {
                                    ((clsGetFeedingDetailsBizActionVO)arg.Result).DrugFeedingList[i].SelectedTakenBy = TakenByList.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetFeedingDetailsBizActionVO)arg.Result).DrugFeedingList[i].TakenID));
                                }
                                else
                                {
                                    ((clsGetFeedingDetailsBizActionVO)arg.Result).DrugFeedingList[i].SelectedTakenBy = TakenByList.FirstOrDefault(p => p.ID == 0);
                                }

                                //   NewFeedingDetails.Add(((clsGetFeedingDetailsBizActionVO)arg.Result).DrugFeedingList[i]);

                                //for (int j = 0; j < ParentDrugList.Count; j++)
                                //{
                                //    if (NewFeedingDetails[i].DrugId == ParentDrugList[j].DrugId)
                                //    {
                                //        ParentDrugList[j].IsChecked = true;
                                //    //}
                                //} 
                            }

                            //NewFeedingDetailsHistory = NewFeedingDetails.DeepCopy();

                            //var Result = from r in NewFeedingDetailsHistory
                            //             where (r.IsFreeze ==true)
                            //             select r;

                            //FeedingDetails = (List<clsPrescriptionDetailsVO>)(NewFeedingDetails).ToList();
                            //dgFeedingDetails.ItemsSource = null;
                            //dgFeedingDetails.ItemsSource = NewFeedingDetails;


                            //dgFeedingDetailsHistory.ItemsSource = null;
                            //dgFeedingDetailsHistory.ItemsSource = Result;

                            UpdateDrugList();

                            //ParentDrugList = new List<clsPrescriptionDetailsVO>();
                            //ParentDrugList = Objresult.DrugList;

                            _List = new PagedCollectionView(ParentDrugList);
                            _List.PageSize = 5;
                            //_List.PageSize = 10;

                            dgDrugsList.ItemsSource = null;
                            dgDrugsList.ItemsSource = _List;
                            dgDrugsList.SelectedItem = null;

                            dataGridDrugsListPager.Source = null;
                            dataGridDrugsListPager.Source = _List;

                            //FeedingDetails = Objresult.DrugFeedingList;
                            //dgFeedingDetails.ItemsSource = null;
                            //dgFeedingDetails.ItemsSource = FeedingDetails;
                            //UpdateDrugList();
                        }
                        else
                        {
                            FeedingDetails = new List<clsPrescriptionDetailsVO>();
                            dgFeedingDetails.ItemsSource = null;
                            dgFeedingDetailsHistory.ItemsSource = null;
                        }
                    }
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();


        }

        //private void FillFeedingDetailsHistory()
        //{
        //    clsGetFeedingDetailsBizActionVO BizAction = new clsGetFeedingDetailsBizActionVO();
        //    BizAction.DrugFeedingList = new List<clsPrescriptionDetailsVO>();

        //    BizAction.PrescriptionID = ((clsPrescriptionMasterVO)grdCurrentPrecription.SelectedItem).ID;
        //    BizAction.PrescriptionUnitID = ((clsPrescriptionMasterVO)grdCurrentPrecription.SelectedItem).UnitID;

        //    BizAction.OPD_IPD = ((clsPrescriptionMasterVO)grdCurrentPrecription.SelectedItem).OPD_IPD;
        //    BizAction.Opd_Ipd_Id = ((clsPrescriptionMasterVO)grdCurrentPrecription.SelectedItem).Opd_Ipd_Id;
        //    BizAction.Opd_Ipd_UnitID = ((clsPrescriptionMasterVO)grdCurrentPrecription.SelectedItem).Opd_Ipd_UnitID;

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, arg) =>
        //    {
        //        if (arg.Error == null)
        //        {
        //            if (arg.Result != null)
        //            {
        //                clsGetFeedingDetailsBizActionVO Objresult = new clsGetFeedingDetailsBizActionVO();
        //                Objresult = (clsGetFeedingDetailsBizActionVO)arg.Result;

        //                NewFeedingDetails.Clear();

        //                if (((clsGetFeedingDetailsBizActionVO)arg.Result).DrugFeedingList.Count > 0 )
        //                {

        //                    for (int i = 0; i < ((clsGetFeedingDetailsBizActionVO)arg.Result).DrugFeedingList.Count; i++)
        //                    {
        //                        ((clsGetFeedingDetailsBizActionVO)arg.Result).DrugFeedingList[i].IsChecked = true;

        //                        ((clsGetFeedingDetailsBizActionVO)arg.Result).DrugFeedingList[i].ListTakenBy = TakenByList;
        //                        if (Convert.ToInt64(((clsGetFeedingDetailsBizActionVO)arg.Result).DrugFeedingList[i].TakenID) > 0)
        //                        {
        //                            ((clsGetFeedingDetailsBizActionVO)arg.Result).DrugFeedingList[i].SelectedTakenBy = TakenByList.FirstOrDefault(p => p.ID == Convert.ToInt64(((clsGetFeedingDetailsBizActionVO)arg.Result).DrugFeedingList[i].TakenID));
        //                        }
        //                        else
        //                        {
        //                            ((clsGetFeedingDetailsBizActionVO)arg.Result).DrugFeedingList[i].SelectedTakenBy = TakenByList.FirstOrDefault(p => p.ID == 0);
        //                        }

        //                        if (((clsGetFeedingDetailsBizActionVO)arg.Result).DrugFeedingList[i].IsFreeze)
        //                        {
        //                            NewFeedingDetails.Add(((clsGetFeedingDetailsBizActionVO)arg.Result).DrugFeedingList[i]);
        //                        }

        //                        for (int j = 0; j < ParentDrugList.Count; j++)
        //                        {
        //                            if (NewFeedingDetails[i].DrugId == ParentDrugList[j].DrugId)
        //                            {
        //                                ParentDrugList[j].IsChecked = true;
        //                            }
        //                        }
        //                    }


        //                    FeedingDetails = (List<clsPrescriptionDetailsVO>)(NewFeedingDetails).ToList();
        //                    dgFeedingDetailsHistory.ItemsSource = null;
        //                    dgFeedingDetailsHistory.ItemsSource = NewFeedingDetails;
        //                    UpdateDrugList();

        //                    //ParentDrugList = new List<clsPrescriptionDetailsVO>();
        //                    //ParentDrugList = Objresult.DrugList;

        //                    _List = new PagedCollectionView(ParentDrugList);
        //                    _List.PageSize = 5;
        //                    //_List.PageSize = 10;

        //                    dgDrugsList.ItemsSource = null;
        //                    dgDrugsList.ItemsSource = _List;
        //                    dgDrugsList.SelectedItem = null;

        //                    dataGridDrugsListPager.Source = null;
        //                    dataGridDrugsListPager.Source = _List;

        //                    //FeedingDetails = Objresult.DrugFeedingList;
        //                    //dgFeedingDetails.ItemsSource = null;
        //                    //dgFeedingDetails.ItemsSource = FeedingDetails;
        //                    //UpdateDrugList();
        //                }
        //                else
        //                {
        //                    FeedingDetails = new List<clsPrescriptionDetailsVO>();
        //                    dgFeedingDetailsHistory.ItemsSource = null;
        //                }
        //            }
        //        }
        //    };
        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    Client.CloseAsync();
        //}

        //Added By Santosh Patil 25/03/2013
        private void hlbDeleteFeedingDetails_Click(object sender, RoutedEventArgs e)
        {
            string msgTitle = "";
            // string msgText = "Are you sure you want to Delete the Feeding Details?";
            if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
            {
                msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("DeleteValidation_Msg");
            }
            else
            {
                msgText = "Are you sure you want to delete the record ?";
            }
            MessageBoxControl.MessageBoxChildWindow msgW =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnDeleteMessageBoxClosed);
            msgW.Show();
        }

        void msgW_OnDeleteMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                Delete();
            }
        }

        private void Delete()
        {
            if (dgFeedingDetails.ItemsSource != null)
            {
                if (dgFeedingDetails.SelectedItem != null)
                {
                    //FeedingDetails.Remove(((clsPrescriptionDetailsVO)dgFeedingDetails.SelectedItem));

                    //dgFeedingDetails.ItemsSource = null;
                    //dgFeedingDetails.ItemsSource = FeedingDetails;


                    clsPrescriptionDetailsVO objItemByIndentId = (clsPrescriptionDetailsVO)dgFeedingDetails.SelectedItem;
                    ObservableCollection<clsPrescriptionDetailsVO> ocItemsByIndentId = new ObservableCollection<clsPrescriptionDetailsVO>();

                    ocItemsByIndentId = NewFeedingDetails;
                    objItemByIndentId = this.NewFeedingDetails.Where(z => z.DrugId == objItemByIndentId.DrugId).First();
                    NewFeedingDetails.Remove(objItemByIndentId);

                    foreach (var item in ParentDrugList)
                    {
                        if (item.DrugId == objItemByIndentId.DrugId)
                        {
                            item.IsChecked = false;
                        }
                    }

                    dgDrugsList.ItemsSource = null;
                    PagedCollectionView collection = new PagedCollectionView(ParentDrugList);
                    collection.PageSize = 5;
                    dgDrugsList.ItemsSource = collection;

                    dataGridDrugsListPager.Source = null;
                    dataGridDrugsListPager.Source = collection;

                    dgFeedingDetails.ItemsSource = null;
                    dgFeedingDetails.ItemsSource = NewFeedingDetails;

                    if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                    {
                        msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("DeletVerify_Msg");
                    }
                    else
                    {
                        msgText = "Record deleted successfully.";
                    }
                    MessageBoxControl.MessageBoxChildWindow msgW =
                new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW.Show();
                    UpdateDrugList();
                }
            }
        }

        private void txtMRNumber_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && txtMRNumber.Text.Length != 0)
            {
                string mrno = txtMRNumber.Text;
                FindPatient(0, ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID, txtMRNumber.Text.Trim());
            }
            else
            {

            }
        }

        private void chkStatus_Click(object sender, RoutedEventArgs e)
        {

        }

        string msgTitle = "PALASHDYNAMICS";

        bool? IsFreezeCheck = false;
        private void ChkIsFreeze_Click(object sender, RoutedEventArgs e)
        {
            IsFreezeCheck = ((CheckBox)sender).IsChecked;
            if (dgFeedingDetails.SelectedItem != null && ((clsPrescriptionDetailsVO)dgFeedingDetails.SelectedItem).ID > 0)   //&& ((clsPrescriptionDetailsVO)dgFeedingDetails.SelectedItem).ID>0
            {

                string msgTitle = "";
                if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                {
                    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("Freeze_Msg");
                }
                else
                {
                    msgText = "Are you sure you want to Freeze.?";
                }
                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(FreezemsgW_OnMessageBoxClosed);
                msgW.Show();
            }
        }

        void FreezemsgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                Freeze();
            }
            else
            {
                if (FeedingDetails.SingleOrDefault(S => S.RowID.Equals(((clsPrescriptionDetailsVO)dgFeedingDetails.SelectedItem).RowID)) != null)
                {
                    FeedingDetails.SingleOrDefault(S => S.RowID.Equals(((clsPrescriptionDetailsVO)dgFeedingDetails.SelectedItem).RowID)).IsFreeze = (bool)IsFreezeCheck;
                }
                FillFeedingDetails();
                //  FillFeedingDetailsHistory();

            }
        }

        private void Freeze()
        {
            if (dgFeedingDetails.ItemsSource != null)
            {
                if (dgFeedingDetails.SelectedItem != null)
                {
                    try
                    {
                        clsUpdateFeedingDetailsIsFreezeBizActionVO bizactionVO = new clsUpdateFeedingDetailsIsFreezeBizActionVO();
                        clsPrescriptionDetailsVO addVO = new clsPrescriptionDetailsVO();
                        addVO.ID = ((clsPrescriptionDetailsVO)dgFeedingDetails.SelectedItem).ID;

                        ((clsPrescriptionDetailsVO)dgFeedingDetails.SelectedItem).IsFreeze = (bool)IsFreezeCheck;
                        addVO.IsFreeze = ((clsPrescriptionDetailsVO)dgFeedingDetails.SelectedItem).IsFreeze;
                        bizactionVO.PrescriptionDetails = new clsPrescriptionDetailsVO();
                        bizactionVO.PrescriptionDetails = addVO;

                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        client.ProcessCompleted += (s, args) =>
                        {
                            if (args.Error == null && args.Result != null)
                            {
                                if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                                {
                                    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("FreezeUpdation_Msg");
                                }
                                else
                                {
                                    msgText = "Freeze updated successfully.";
                                    FillFeedingDetails();
                                }

                                //FeedingDetails = ((List<clsPrescriptionDetailsVO>)dgFeedingDetails.ItemsSource);
                                //if ((bool)IsFreezeCheck)
                                //{
                                //    if (FeedingDetails.SingleOrDefault(S => S.RowID.Equals(((clsPrescriptionDetailsVO)dgFeedingDetails.SelectedItem).RowID)) != null)
                                //    {
                                //        FeedingDetails.SingleOrDefault(S => S.RowID.Equals(((clsPrescriptionDetailsVO)dgFeedingDetails.SelectedItem).RowID)).IsDelete = false;
                                //    }
                                //    //if(FeedingDetails.SingleOrDefault(S=>S.Re
                                //    // ((clsPrescriptionDetailsVO)dgFeedingDetails.SelectedItem).IsDelete = false;
                                //}
                                //else
                                //{
                                //    if (FeedingDetails.SingleOrDefault(S => S.RowID.Equals(((clsPrescriptionDetailsVO)dgFeedingDetails.SelectedItem).RowID)) != null)
                                //    {
                                //        FeedingDetails.SingleOrDefault(S => S.RowID.Equals(((clsPrescriptionDetailsVO)dgFeedingDetails.SelectedItem).RowID)).IsDelete = true;
                                //    }
                                //    //((clsPrescriptionDetailsVO)dgFeedingDetails.SelectedItem).IsDelete = true;
                                //}
                                //dgFeedingDetails.ItemsSource = null;
                                //dgFeedingDetails.ItemsSource = FeedingDetails;
                                //MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                //msgWindow.Show();


                                NewFeedingDetails = ((ObservableCollection<clsPrescriptionDetailsVO>)(dgFeedingDetails.ItemsSource));
                                if ((bool)IsFreezeCheck)
                                {
                                    if (NewFeedingDetails.SingleOrDefault(S => S.RowID.Equals(((clsPrescriptionDetailsVO)dgFeedingDetails.SelectedItem).RowID)) != null)
                                    {
                                        NewFeedingDetails.SingleOrDefault(S => S.RowID.Equals(((clsPrescriptionDetailsVO)dgFeedingDetails.SelectedItem).RowID)).IsDelete = false;
                                    }
                                    //if(FeedingDetails.SingleOrDefault(S=>S.Re
                                    // ((clsPrescriptionDetailsVO)dgFeedingDetails.SelectedItem).IsDelete = false;
                                }
                                else
                                {
                                    if (NewFeedingDetails.SingleOrDefault(S => S.RowID.Equals(((clsPrescriptionDetailsVO)dgFeedingDetails.SelectedItem).RowID)) != null)
                                    {
                                        NewFeedingDetails.SingleOrDefault(S => S.RowID.Equals(((clsPrescriptionDetailsVO)dgFeedingDetails.SelectedItem).RowID)).IsDelete = true;
                                    }
                                    //((clsPrescriptionDetailsVO)dgFeedingDetails.SelectedItem).IsDelete = true;
                                }
                                dgFeedingDetails.ItemsSource = null;
                                dgFeedingDetails.ItemsSource = NewFeedingDetails;




                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
                            }
                        };
                        client.ProcessAsync(bizactionVO, new clsUserVO());
                        client.CloseAsync();

                    }
                    catch (Exception ex)
                    {
                    }

                }
            }
        }

        long drugsID = 0;

        double DrugTotalQty = 0;
        private void chkDrugSelect_Click(object sender, RoutedEventArgs e)
        {
            if (((clsPrescriptionDetailsVO)dgDrugsList.SelectedItem).DrugId > 0)
            {
                if (((CheckBox)sender).IsChecked == true)
                {
                    //var Result = from r in ocSelectedItemList
                    //             where (r.DrugId == ((clsPrescriptionDetailsVO)dgDrugsList.SelectedItem).DrugId)
                    //             select r;

                    var Result = from r in NewFeedingDetails
                                 where (r.DrugId == ((clsPrescriptionDetailsVO)dgDrugsList.SelectedItem).DrugId)
                                 select r;

                    //var Result = from r in NewFeedingDetails
                    //             where (r.DrugId == ((clsPrescriptionDetailsVO)dgDrugsList.SelectedItem).DrugId)
                    //             select r;



                    //foreach (var mc in Result.Where(x => x.Name == "height"))
                    //    mc.Value = 30;
                    //var found = ParentDrugList.FirstOrDefault(c => c.DrugId == ((clsPrescriptionDetailsVO)dgDrugsList.SelectedItem).DrugId);
                    //if (found != null)
                    //{
                    //    found.IsChecked = true;
                    //}



                    // if (Result.ToList().Count == 0)
                    long maxsrno = 0;
                    foreach (clsPrescriptionDetailsVO l in NewFeedingDetails)
                    {
                        if (l.SrNo > maxsrno)
                        {
                            maxsrno = l.SrNo;

                        }

                    }

                    bool result = true;
                    if (((clsPrescriptionDetailsVO)dgDrugsList.SelectedItem).ConsumeQuantity > ((clsPrescriptionDetailsVO)dgDrugsList.SelectedItem).Quantity ||

                        ((clsPrescriptionDetailsVO)dgDrugsList.SelectedItem).ConsumeQuantity == ((clsPrescriptionDetailsVO)dgDrugsList.SelectedItem).Quantity)
                    {
                        ((CheckBox)sender).IsChecked = false;

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Quantity Already Been Consumed", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                        msgW1.Show();
                        result = false;

                    }
                    if (result)
                    {
                        //this.ocSelectedItemList.Add((clsPrescriptionDetailsVO)dgDrugsList.SelectedItem);
                        clsPrescriptionDetailsVO ob = new clsPrescriptionDetailsVO();
                        ob.RowID = NewFeedingDetails.Count + 1;

                        ob.Date = DateTime.Now;
                        ob.DrugId = ((clsPrescriptionDetailsVO)dgDrugsList.SelectedItem).DrugId;
                        ob.DrugName = ((clsPrescriptionDetailsVO)dgDrugsList.SelectedItem).DrugName;
                        ob.MoleculeName = ((clsPrescriptionDetailsVO)dgDrugsList.SelectedItem).MoleculeName;
                        ob.Despense = ((clsPrescriptionDetailsVO)dgDrugsList.SelectedItem).Despense;
                        ob.Duration = ((clsPrescriptionDetailsVO)dgDrugsList.SelectedItem).Duration;
                        ob.Frequency = ((clsPrescriptionDetailsVO)dgDrugsList.SelectedItem).Frequency;
                        ob.Remark = ((clsPrescriptionDetailsVO)dgDrugsList.SelectedItem).Remark;
                        ob.UOM = ((clsPrescriptionDetailsVO)dgDrugsList.SelectedItem).Despense;
                        ob.DoctorName = ((clsPrescriptionMasterVO)grdCurrentPrecription.SelectedItem).DoctorName;
                        ob.Quantity = 0;
                        ob.DoctorID = ((clsPrescriptionDetailsVO)dgDrugsList.SelectedItem).DoctorID;
                        ob.Instruction = ((clsPrescriptionDetailsVO)dgDrugsList.SelectedItem).Instruction;
                        ob.IsChecked = true;
                        ob.SrNo = maxsrno + 1;
                        ob.IsFreeze = true;
                        ob.ListTakenBy = TakenByList;
                        ob.Description = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.Name;
                        ob.DepartmentID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                        ob.DoctorID = ((clsPrescriptionMasterVO)grdCurrentPrecription.SelectedItem).DoctorID;
                        ob.Route = ((clsPrescriptionDetailsVO)dgDrugsList.SelectedItem).Route; //added on 08032017
                        this.NewFeedingDetails.Add(ob.DeepCopy());
                        //string s=(IApplicationConfiguration)App.Current).   

                    }
                    else
                    {
                        //MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //                         new MessageBoxControl.MessageBoxChildWindow("Palash", "Drug Already Exist", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                        //msgW1.Show();
                        //((CheckBox)sender).IsChecked = false;
                    }


                    //////////////////////Added By YK 06042017/////////////////////////////////////////
                    drugsID = ((clsPrescriptionDetailsVO)dgDrugsList.SelectedItem).DrugId;
                    DrugTotalQty = ((clsPrescriptionDetailsVO)dgDrugsList.SelectedItem).Quantity;
                    //var Item = from r in NewFeedingDetails
                    //             where (r.DrugId == ((clsPrescriptionDetailsVO)dgDrugsList.SelectedItem).DrugId && r.Quantity>)
                    //             select r;
                    // checkQty(drugsID);
                    ///////////////////////////END//////////////////////////////////



                    // checkQty(drugsID, DrugTotalQty);
                }
                else
                {
                    clsPrescriptionDetailsVO objItemByIndentId = (clsPrescriptionDetailsVO)dgDrugsList.SelectedItem;
                    objItemByIndentId = this.NewFeedingDetails.Where(z => z.DrugId == objItemByIndentId.DrugId).First();
                    this.NewFeedingDetails.Remove(objItemByIndentId);
                }

                dgDrugsList.UpdateLayout();
                dgDrugsList.Focus();
                FillSelectedDrugList();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Drug '" + ((clsPrescriptionDetailsVO)dgDrugsList.SelectedItem).ItemName + "' Item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                msgW1.Show();
                ((CheckBox)sender).IsChecked = false;
            }
        }

        //private void checkQty(long drugsids,double DrugsTotalQty)
        //{
        //    foreach (var item in NewFeedingDetails)
        //    {
        //        // if(item.Quantity>Result)
        //        if (item.DrugId == drugsids)
        //        {
        //            getQty = getQty + item.Quantity;
        //        }

        //        if (item.Quantity <= 0)
        //        {
        //            IsQtyValidZero = false;
        //            break;
        //        }
        //        else
        //        {
        //            IsQtyValidZero = true;
        //        }
        //        if (getQty > DrugsTotalQty)
        //        {
        //            //MessageBoxControl.MessageBoxChildWindow msgW1 =
        //            //                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Quantity Should Be Less Than Selected Drug Quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
        //            //msgW1.Show();
        //            IsQtyValid = false;
        //            getQty = 0;
        //            IsBreak = true;
        //            break;
        //            //((CheckBox)sender).IsChecked = false;
        //        }

        //        else
        //        {
        //            IsQtyValid = true;
        //            getQty = 0;
        //        }
        //    }



        //}

        private void FillSelectedDrugList()
        {
            //PagedCollectionView pcvItemsByIndentId = new PagedCollectionView(ocSelectedItemList);
            //pcvItemsByIndentId.GroupDescriptions.Add(new PropertyGroupDescription("PurchaseRequisitionNumber"));
            //dgFeedingDetails.ItemsSource = pcvItemsByIndentId;

            dgFeedingDetails.ItemsSource = NewFeedingDetails;
            dgFeedingDetails.UpdateLayout();
        }

        private void chkSelectedItem_UnCheck(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == false)
            {
                clsPrescriptionDetailsVO objItemByIndentId = (clsPrescriptionDetailsVO)dgFeedingDetails.SelectedItem;
                ObservableCollection<clsPrescriptionDetailsVO> ocItemsByIndentId = new ObservableCollection<clsPrescriptionDetailsVO>();
                ////List<clsPrescriptionDetailsVO> ocItemsByIndentId = new List<clsPrescriptionDetailsVO>();
                //long DrugId = (dgFeedingDetails.SelectedItem as clsPrescriptionDetailsVO).DrugId;

                //ocItemsByIndentId = NewFeedingDetails;
                //objItemByIndentId = this.NewFeedingDetails.Where(z => z.DrugId == objItemByIndentId.DrugId).First();
                //NewFeedingDetails.Remove(objItemByIndentId);

                //foreach (var item in ParentDrugList)
                //{
                //    if (item.DrugId == objItemByIndentId.DrugId)
                //    {
                //        item.IsChecked = false;
                //    }
                //}
                for (int i = NewFeedingDetails.Count - 1; i >= 0; i--)
                {
                    if (NewFeedingDetails[i].SrNo == objItemByIndentId.SrNo)

                    //&& NewFeedingDetails[i].DoctorName == objItemByIndentId.DoctorName)
                    {
                        NewFeedingDetails.RemoveAt(i);
                    }
                }

                foreach (var item in ParentDrugList)
                {
                    if (item.DrugId == objItemByIndentId.DrugId && item.Frequency == objItemByIndentId.Frequency)
                    {
                        item.IsChecked = false;

                    }
                }

                dgFeedingDetails.ItemsSource = null;
                dgFeedingDetails.ItemsSource = NewFeedingDetails;
                // dgDrugsList.ItemsSource = null;
                // FillFeedingDetails();
                PagedCollectionView collection = new PagedCollectionView(ParentDrugList);
                collection.PageSize = 5;
                dgDrugsList.ItemsSource = collection;

                dataGridDrugsListPager.Source = null;
                dataGridDrugsListPager.Source = collection;

                //_List = new PagedCollectionView(Objresult.DrugList);
                //_List.PageSize = 5;
                ////_List.PageSize = 10;

                //dgDrugsList.ItemsSource = null;
                //dgDrugsList.ItemsSource = _List;
                //dgDrugsList.SelectedItem = null;


            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (grdCurrentPrecription.SelectedItem != null)
            {
                long PrescriptionID = ((clsPrescriptionMasterVO)grdCurrentPrecription.SelectedItem).ID;
                long PrescriptionUnitID = ((clsPrescriptionMasterVO)grdCurrentPrecription.SelectedItem).UnitID;

                long OPD_IPD = ((clsPrescriptionMasterVO)grdCurrentPrecription.SelectedItem).OPD_IPD;
                long Opd_Ipd_Id = ((clsPrescriptionMasterVO)grdCurrentPrecription.SelectedItem).Opd_Ipd_Id;
                long Opd_Ipd_UnitID = ((clsPrescriptionMasterVO)grdCurrentPrecription.SelectedItem).Opd_Ipd_UnitID;

                PrintDrugAdministartion(PrescriptionID, PrescriptionUnitID, OPD_IPD, Opd_Ipd_Id, Opd_Ipd_UnitID);
            }

        }

        private void PrintDrugAdministartion(long iPrescriptionID, long iPrescriptionUnitID, long iOPD_IPD, long iOpd_Ipd_Id, long iOpd_Ipd_UnitID)
        {
            
                long UnitID = iPrescriptionUnitID;
                string URL = "../Reports/Nursing/DrugAdministrationChart.aspx?PrescriptionID=" + iPrescriptionID + "&UnitID=" + UnitID + "&OPD_IPD=" + iOPD_IPD + "&Opd_Ipd_Id=" + iOpd_Ipd_Id + "&Opd_Ipd_UnitID=" + iOpd_Ipd_UnitID;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
           
        }



        private void txtQuantity_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtQuantity_KeyDown(object sender, RoutedEventArgs e)
        {

        }

        private void txtQuantity_SelectionChanged(object sender, RoutedEventArgs e)
        {
            // checkQty(((clsPrescriptionDetailsVO)dgFeedingDetails.SelectedItem).DrugId);
        }

        private void CmdPrint_Click(object sender, RoutedEventArgs e)
        {
            //if (grdCurrentPrecription.SelectedItem != null)
            //{
            //    long PrescriptionID = ((clsPrescriptionMasterVO)grdCurrentPrecription.SelectedItem).ID;
            //    long PrescriptionUnitID = ((clsPrescriptionMasterVO)grdCurrentPrecription.SelectedItem).UnitID;

            //    long OPD_IPD = ((clsPrescriptionMasterVO)grdCurrentPrecription.SelectedItem).OPD_IPD;
            //    long Opd_Ipd_Id = ((clsPrescriptionMasterVO)grdCurrentPrecription.SelectedItem).Opd_Ipd_Id;
            //    long Opd_Ipd_UnitID = ((clsPrescriptionMasterVO)grdCurrentPrecription.SelectedItem).Opd_Ipd_UnitID;

            //    PrintDrugAdministartion(PrescriptionID, PrescriptionUnitID, OPD_IPD, Opd_Ipd_Id, Opd_Ipd_UnitID);
            //}
            long PrescriptionID = 0;
            long PrescriptionUnitID = 0;

            long OPD_IPD = 0;
            long Opd_Ipd_Id = 0;
            long Opd_Ipd_UnitID = 0;

            if (grdCurrentPrecription.ItemsSource != null)
            {
                foreach (clsPrescriptionMasterVO it in grdCurrentPrecription.ItemsSource)
                {
                    PrescriptionID = it.ID;
                    PrescriptionUnitID = it.UnitID;

                    OPD_IPD = it.OPD_IPD;
                    Opd_Ipd_Id = it.Opd_Ipd_Id;
                    Opd_Ipd_UnitID = it.Opd_Ipd_UnitID;


                    break;
                }

                PrintDrugAdministartion(0, PrescriptionUnitID, OPD_IPD, Opd_Ipd_Id, Opd_Ipd_UnitID);
            }
            else
            {

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                   new MessageBoxControl.MessageBoxChildWindow("Palash", " Patient History  Not Available", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                msgW1.Show();


            }


        }
        private FrameworkElement GetParent(FrameworkElement child, Type targetType)
        {
            object parent = null;
            if (child != null)
            {
                parent = child.Parent;
            }
            if (parent != null)
            {
                if (parent.GetType() == targetType)
                {
                    return (FrameworkElement)parent;
                }
                else
                {
                    return GetParent((FrameworkElement)parent, targetType);
                }
            }
            return null;

        }
        private void dgFeedingDetailsHistory_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.IsEnabled = false;


        }

        private void dgDrugsList_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (((clsPrescriptionDetailsVO)e.Row.DataContext).ConsumeQuantity == ((clsPrescriptionDetailsVO)e.Row.DataContext).Quantity)
            {

                e.Row.IsEnabled = false;
            }
        }
    }
}
