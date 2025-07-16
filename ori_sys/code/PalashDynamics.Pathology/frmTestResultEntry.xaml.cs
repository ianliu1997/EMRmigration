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
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Pathology;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Collections;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Pathology.PathologyMasters;
using System.IO;
using System.Windows.Browser;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using DataDrivenApplication.Forms;
using PalashDynamics.ValueObjects.Administration;
using System.Windows.Resources;
using System.Reflection;
using System.Xml.Linq;

namespace PalashDynamics.Pathology
{
    public partial class frmTestResultEntry : UserControl, IInitiateCIMS, IInitiateCIMSIVF
    {
        #region IInitiateCIMS Members

        public void Initiate(string Mode)
        {
            switch (Mode)
            {
                case "NEW":
                    if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW5.Show();
                        IsPatientExist = false;
                        break;
                    }

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW5.Show();

                        IsPatientExist = false;
                        break;
                    }

                    IsPatientExist = true;
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

                    mElement.Text = " : " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).FirstName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).MiddleName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).LastName;
                    break;


            }
        }
        clsMenuVO Menu;
        int flag = 0;
        public void Initiate(clsMenuVO Item)
        {
            Menu = Item;
            switch (Item.Mode)
            {
                case "NEW":
                    if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        flag = 2;
                        msgW5.Show();
                        IsPatientExist = false;
                        break;
                    }

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW5.Show();
                        flag = 2;
                        IsPatientExist = false;
                        break;
                    }

                    IsPatientExist = true;
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

                    mElement.Text = " : " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).FirstName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).MiddleName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).LastName;
                    break;
            }

        }

        #endregion

        #region Properties
        public int PageSize
        {
            get
            {
                return MasterList.PageSize;
            }
            set
            {
                if (value == MasterList.PageSize) return;
                MasterList.PageSize = value;
                MasterListBack.PageSize = value;
                // OnPropertyChanged("PageSize");
            }
        }
        public byte[] AttachedFileContents { get; set; }

        #endregion

        #region Public Variables

        public bool IsPatientExist = false;
        public bool IsPageLoded = false;
        private SwivelAnimation objAnimation;
        string msgTitle = ""; // "PALASHDYNAMICS";
        string msgText = "";
        long CoupleId { get; set; }
        long CoupleUnitId { get; set; }
        private clsCoupleVO _CoupleDetails = new clsCoupleVO();
        public clsCoupleVO CoupleDetails
        {
            get
            {
                return _CoupleDetails;
            }
            set
            {
                _CoupleDetails = value;
            }
        }
        public bool IsCancel = true;
        public long ParamUnitID { get; set; }
        public long GRNReturnParamUnitID { get; set; }
        public string ModuleName { get; set; }
        public string Action { get; set; }
        public long CategoryId = 0;
        public long TestId = 0;
        public long ParamId = 0;
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        public PagedSortableCollectionView<clsPathoResultEntryVO> MasterList { get; private set; }
        public PagedSortableCollectionView<clsPathoResultEntryVO> MasterListBack { get; private set; }
        public bool IsCategory, IsTest, IsParameter, IsLabName, IsValue, IsResult,IsUnit;
        byte[] data1;
        FileInfo Attachment1;
        public string fileName1;
        public bool IsNew = false;
        ObservableCollection<clsPathoResultEntryVO> TestList { get; set; }
        ObservableCollection<clsPathoResultEntryVO> TestListBack { get; set; }


        System.Collections.ObjectModel.ObservableCollection<string> AttachedFileNameList = new System.Collections.ObjectModel.ObservableCollection<string>();

        #endregion

    
        public frmTestResultEntry()
        {
            InitializeComponent();
            this.DataContext = new clsPathoResultEntryVO();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            MasterList = new PagedSortableCollectionView<clsPathoResultEntryVO>();
            MasterListBack = new PagedSortableCollectionView<clsPathoResultEntryVO>();
           MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
           MasterListBack.OnRefresh += new EventHandler<RefreshEventArgs>(MasterListBack_OnRefresh);
           this.dataGrid2PagerBack.DataContext = MasterListBack;
            this.dataGrid2Pager.DataContext = MasterList;
           // grdBackPanel.DataContext = MasterList;

        }

        /// <summary>
        /// Get called when  front panel grid refreshed
        /// </summary>
        /// <param name="sender"> grid</param>
        /// <param name="e">grid refresh</param>
        void MasterListBack_OnRefresh(object sender, RefreshEventArgs e)
        {
           // fillBackGrid();
        }
        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            SetupPage();
        }
        UIElement myData = null;
        void c2_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {


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

                ((IInitiateCIMS)myData).Initiate("VISIT");


                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);



            }
            catch (Exception ex)
            {
                throw;
            }



        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsPatientExist == false)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
              //  ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");  if (Menu != null && Menu.Parent == "Surrogacy")
                if (Menu != null && Menu.Parent == "Surrogacy")
                {
                    ModuleName = "PalashDynamics";
                    Action = "PalashDynamics.Forms.PatientView.PatientListForSurrogacy";
                }
                else
                {
                    ModuleName = "PalashDynamics";
                    Action = "CIMS.Forms.PatientList";
                }
              
                UserControl rootPage = Application.Current.RootVisual as UserControl;

                WebClient c2 = new WebClient();
                c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }

            if (!IsPageLoded && ((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
            {
                SetCommandButtonState("Load");
                fillCoupleDetails();
                FillCategoryMaster();
                //FillTestMaster();
                //FillParameterMaster();
                FillLabMaster();
               FillResultMaster();
                ProcDate.SelectedDate = DateTime.Now.Date.Date;
                ProcTime.Value = DateTime.Now;
                TestList = new ObservableCollection<clsPathoResultEntryVO>();
                TestListBack = new ObservableCollection<clsPathoResultEntryVO>();
                SetupPage();
                cmbLab.SelectedValue = (long)2;
                ParamId = 0;
            }         
        }


        //Added by Saily P on 25.09.12 to get the patient details in case of Donor
        private void LoadPatientHeader()
        {
            clsGetPatientBizActionVO BizAction = new PalashDynamics.ValueObjects.Patient.clsGetPatientBizActionVO();
            BizAction.PatientDetails = new PalashDynamics.ValueObjects.Patient.clsPatientVO();
            BizAction.PatientDetails.GeneralDetails = (clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizAction.PatientDetails.GeneralDetails = ((clsGetPatientBizActionVO)args.Result).PatientDetails.GeneralDetails;
                    BizAction.PatientDetails.SpouseDetails = ((clsGetPatientBizActionVO)args.Result).PatientDetails.SpouseDetails;
                    PatientInfo.Visibility = Visibility.Visible;
                    CoupleInfo.Visibility = Visibility.Collapsed;
                    Patient.DataContext = BizAction.PatientDetails.GeneralDetails;
                    #region Commented by Saily P on 25.09.12
                    //if (BizAction.PatientDetails.GenderID == 1)
                    //{
                    //    Male.DataContext = BizAction.PatientDetails.GeneralDetails;
                    //    if (BizAction.PatientDetails.SpouseDetails != null && BizAction.PatientDetails.SpouseDetails.ID != 0)
                    //    {
                    //        Female.DataContext = BizAction.PatientDetails.SpouseDetails;
                    //        CoupleInfo.Visibility = Visibility.Visible;
                    //        PatientInfo.Visibility = Visibility.Collapsed;
                    //    }
                    //    else
                    //    {
                    //        PatientInfo.Visibility = Visibility.Visible;
                    //        CoupleInfo.Visibility = Visibility.Collapsed;
                    //        Patient.DataContext = BizAction.PatientDetails.GeneralDetails;
                    //    }
                    //}
                    //else
                    //{
                    //    Female.DataContext = BizAction.PatientDetails.GeneralDetails;
                    //    if (BizAction.PatientDetails.SpouseDetails != null && BizAction.PatientDetails.SpouseDetails.ID != 0)
                    //    {
                    //        Male.DataContext = BizAction.PatientDetails.SpouseDetails;
                    //        CoupleInfo.Visibility = Visibility.Visible;
                    //        PatientInfo.Visibility = Visibility.Collapsed;
                    //    }
                    //    else
                    //    {
                    //        Patient.DataContext = BizAction.PatientDetails.GeneralDetails;
                    //        PatientInfo.Visibility = Visibility.Visible;
                    //        CoupleInfo.Visibility = Visibility.Collapsed;
                    //    }
                    //}
                    #endregion
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        #region Couple Details


        private void fillCoupleDetails()
        {
            clsGetCoupleDetailsBizActionVO BizAction = new clsGetCoupleDetailsBizActionVO();
            BizAction.PatientID = ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
            BizAction.PatientUnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;
            BizAction.CoupleDetails = new clsCoupleVO();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails != null && ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.CoupleId > 0)
                    {
                        PatientInfo.Visibility = Visibility.Collapsed;
                        CoupleInfo.Visibility = Visibility.Visible;

                        BizAction.CoupleDetails.MalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient;
                        BizAction.CoupleDetails.FemalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient;
                        CoupleDetails.MalePatient = new clsPatientGeneralVO();
                        CoupleDetails.FemalePatient = new clsPatientGeneralVO();
                        CoupleDetails = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails;
                        //if (CoupleDetails.CoupleId == 0)
                        //{                    
                        //    //MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //    //     new MessageBoxControl.MessageBoxChildWindow("Palash", "Cannot Upload Result. This Functionality is Only For Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        //    //msgW1.Show();

                        //    //((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                        //}
                        //else
                        //{                            
                        CoupleId = CoupleDetails.CoupleId; //((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.CoupleId;
                        CoupleUnitId = CoupleDetails.CoupleUnitId; //((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.CoupleUnitId;
                        GetHeightAndWeight(BizAction.CoupleDetails);
                      //  }
                        if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient.Photo != null)
                        {
                            WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto13.Width, (int)imgPhoto13.Height);   // Fill WriteableBitmap from byte array (format ARGB)
                            bmp.FromByteArray(((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient.Photo);
                            imgPhoto13.Source = bmp;
                            imgPhoto13.Visibility = System.Windows.Visibility.Visible;
                        }
                        else
                        {
                            imgP1.Visibility = System.Windows.Visibility.Visible;
                        }
                        if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient.Photo != null)
                        {
                            WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto12.Width, (int)imgPhoto12.Height);   // Fill WriteableBitmap from byte array (format ARGB)
                            bmp.FromByteArray(((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient.Photo);
                            imgPhoto12.Source = bmp;
                            imgPhoto12.Visibility = System.Windows.Visibility.Visible;
                        }
                        else
                        {
                            imgP2.Visibility = System.Windows.Visibility.Visible;
                        }
                    }
                    else
                    {
                        LoadPatientHeader();
                        //MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //         new MessageBoxControl.MessageBoxChildWindow("Palash", "Cannot Upload Result. This Functionality is Only For Active Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        //msgW1.Show();

                        //((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void GetHeightAndWeight(clsCoupleVO CoupleDetails)
        {
            clsGetGetCoupleHeightAndWeightBizActionVO BizAction = new clsGetGetCoupleHeightAndWeightBizActionVO();
            BizAction.CoupleDetails = new clsCoupleVO();
            BizAction.FemalePatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.MalePatientID = CoupleDetails.MalePatient.PatientID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    if (((clsGetGetCoupleHeightAndWeightBizActionVO)args.Result).CoupleDetails != null)
                        BizAction.CoupleDetails = ((clsGetGetCoupleHeightAndWeightBizActionVO)args.Result).CoupleDetails;
                    if (BizAction.CoupleDetails != null)
                    {
                        clsPatientGeneralVO FemalePatientDetails = new clsPatientGeneralVO();

                        FemalePatientDetails = CoupleDetails.FemalePatient;
                        FemalePatientDetails.Height = BizAction.CoupleDetails.FemalePatient.Height;
                        FemalePatientDetails.Weight = BizAction.CoupleDetails.FemalePatient.Weight;
                        FemalePatientDetails.BMI = Convert.ToDouble(String.Format("{0:0.00}", BizAction.CoupleDetails.FemalePatient.BMI));
                        FemalePatientDetails.Alerts = BizAction.CoupleDetails.FemalePatient.Alerts;
                        Female.DataContext = FemalePatientDetails;

                        clsPatientGeneralVO MalePatientDetails = new clsPatientGeneralVO();
                        MalePatientDetails = CoupleDetails.MalePatient;
                        MalePatientDetails.Height = BizAction.CoupleDetails.MalePatient.Height;
                        MalePatientDetails.Weight = BizAction.CoupleDetails.MalePatient.Weight;
                        MalePatientDetails.BMI = Convert.ToDouble(String.Format("{0:0.00}", BizAction.CoupleDetails.MalePatient.BMI));
                        MalePatientDetails.Alerts = BizAction.CoupleDetails.MalePatient.Alerts;
                        Male.DataContext = MalePatientDetails;


                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();





        }

        private void getEMRDetails(clsPatientGeneralVO PatientDetails, string Gender)
        {
            clsGetEMRDetailsBizActionVO BizAction = new clsGetEMRDetailsBizActionVO();
            BizAction.PatientID = PatientDetails.PatientID;
            BizAction.PatientUnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;
            BizAction.TemplateID = 8;//Using For Getting Height Wight Of Patient 
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Double height = 0;
            Double weight = 0;

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizAction.EMRDetailsList = ((clsGetEMRDetailsBizActionVO)args.Result).EMRDetailsList;

                    if (BizAction.EMRDetailsList != null || BizAction.EMRDetailsList.Count > 0)
                    {
                        for (int i = 0; i < BizAction.EMRDetailsList.Count; i++)
                        {
                            if (BizAction.EMRDetailsList[i].ControlCaption.Equals("Height"))
                            {
                                if (!string.IsNullOrEmpty(BizAction.EMRDetailsList[i].Value))
                                {
                                    height = Convert.ToDouble(BizAction.EMRDetailsList[i].Value);
                                    if (height != 0 && weight != 0)
                                    {
                                        break;
                                    }
                                }
                            }
                            if (BizAction.EMRDetailsList[i].ControlCaption.Equals("Weight"))
                            {
                                if (!string.IsNullOrEmpty(BizAction.EMRDetailsList[i].Value))
                                {
                                    weight = Convert.ToDouble(BizAction.EMRDetailsList[i].Value);
                                    if (height != 0 && weight != 0)
                                    {
                                        break;
                                    }
                                }
                            }
                        }

                        if (height != 0 && weight != 0)
                        {
                            if (Gender.Equals("F"))
                            {
                                PatientDetails.Height = height;
                                PatientDetails.Weight = weight;
                                PatientDetails.BMI = Math.Round(CalculateBMI(height, weight), 2);
                                Female.DataContext = PatientDetails;
                            }
                            else
                            {
                                PatientDetails.Height = height;
                                PatientDetails.Weight = weight;
                                PatientDetails.BMI = Math.Round(CalculateBMI(height, weight), 2);
                                Male.DataContext = PatientDetails;
                            }
                        }
                        else
                        {
                            if (Gender.Equals("F"))
                            {
                                Female.DataContext = PatientDetails;
                            }
                            else
                            {
                                Male.DataContext = PatientDetails;
                            }
                        }
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private double CalculateBMI(Double Height, Double Weight)
        {
            try
            {
                if (Weight == 0)
                {
                    return 0.0;

                }
                else if (Height == 0)
                {

                    return 0.0;
                }
                else
                {
                    double weight = Convert.ToDouble(Weight);
                    double height = Convert.ToDouble(Height);
                    double TotalBMI = weight / height;
                    TotalBMI = TotalBMI / height;
                    TotalBMI = TotalBMI * 10000;
                    return TotalBMI;

                }
            }
            catch (Exception ex)
            {
                return 0.0;
            }
        }

        #endregion

        /// <summary>
        /// Fills front panel grid
        /// </summary>
        public void SetupPage()
        {
            try
            {
                clsGetPathoTestResultEntryBizActionVO bizActionVO = new clsGetPathoTestResultEntryBizActionVO();
                bizActionVO.SearchExpression = ""; //txtSearch.Text;
                bizActionVO.PagingEnabled = true;
                bizActionVO.MaximumRows = MasterList.PageSize;
                bizActionVO.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;
                bizActionVO.PatientID = ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
                // getstoreinfo = new clsIPDBedMasterVO();
                bizActionVO.PathoResultEntry = new List<clsPathoResultEntryVO>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        bizActionVO.PathoResultEntry = (((clsGetPathoTestResultEntryBizActionVO)args.Result).PathoResultEntry);
                      //  grdResultEntry.DataContext = null;
                        if (bizActionVO.PathoResultEntry.Count > 0)
                        {
                           MasterList.Clear();
                            MasterList.TotalItemCount = (int)(((clsGetPathoTestResultEntryBizActionVO)args.Result).TotalRows);
                            foreach (clsPathoResultEntryVO item in bizActionVO.PathoResultEntry)
                            {
                                MasterList.Add(item);
                                TestList.Add(item);
                            }
                            PagedCollectionView collection = new PagedCollectionView(MasterList);
                            collection.GroupDescriptions.Add(new PropertyGroupDescription("Category"));

                            grdResultEntry.DataContext = collection;
                        }
                    }
                };
                client.ProcessAsync(bizActionVO, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ValidateForm()
        {

            //if (string.IsNullOrEmpty(txtValue.Text))
            //{
            //    txtValue.SetValidation("Please Enter Value.");
            //    txtValue.RaiseValidationError();
            //    IsValue = false;
            //}
            //else
            //{
            //    txtValue.ClearValidationError();
            //    IsValue = true;
            //}
            if (((MasterListItem)cmbResultType.SelectedItem).ID > 0)
            {
                cmbResultType.ClearValidationError();
                IsResult = true;
            }
            else
            {
                cmbResultType.SetValidation("Please Select Result Type.");
                cmbResultType.RaiseValidationError();
                IsResult = false;
            }
            if (((MasterListItem)cmbLab.SelectedItem).ID > 0)
            {
                cmbLab.ClearValidationError();
                IsLabName = true;
            }
            else
            {
                cmbLab.SetValidation("Please Select Lab.");
                cmbLab.RaiseValidationError();
                IsLabName = false;
            }
            //if (((MasterListItem)cmbUnit.SelectedItem).ID > 0)
            //{
            //    cmbUnit.ClearValidationError();
            //    IsUnit = true;
            //}
            //else
            //{
            //    cmbUnit.SetValidation("Please Select Unit");
            //    cmbUnit.RaiseValidationError();
            //    IsUnit = false;
            //}
            if (((clsPathoTestParameterVO)cmbParameter.SelectedItem).ID > 0)
            {
                cmbParameter.ClearValidationError();
                IsParameter = true;
            }
            else
            {
                cmbParameter.SetValidation("Please Select Parameter.");
                cmbParameter.RaiseValidationError();
                IsParameter = false;
            }
            if (((MasterListItem)cmbTest.SelectedItem).ID > 0)
            {
                cmbTest.ClearValidationError();
                IsTest = true;
            }
            else
            {
                cmbTest.SetValidation("Please Select Test.");
                cmbTest.RaiseValidationError();
                IsTest = false;
            }
            if (((MasterListItem)cmbCategory.SelectedItem).ID > 0)
            {
                cmbCategory.ClearValidationError();
                IsCategory = true;
            }
            else
            {
                cmbCategory.SetValidation("Please Select Test Category.");
                cmbCategory.RaiseValidationError();
                IsCategory = false;
            }
            //if (ProcDate.SelectedDate == null)
            //{
            //    ProcDate.SetValidation("Please Select Date.");
            //    ProcDate.RaiseValidationError();
            //    IsDate = false;
            //}
            //else
            //{
            //    ProcDate.ClearValidationError();
            //    IsDate = true;
            //}

            //if (ProcTime.Value == null)
            //{
            //    ProcTime.SetValidation("Please Select Time.");

            //}
        }

        private void CmdNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.BackPanel.DataContext = new clsPathoResultEntryVO();
                ClearUI();
              //  FillCategoryMaster();
                //FillTestMaster();
                //FillParameterMaster();
              //  FillLabMaster();
               // FillResultMaster();
                SetCommandButtonState("New");
                IsNew = true;
                objAnimation.Invoke(RotationType.Forward);
                ParamId = 0;
               // fillBackGrid();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidateForm();
                if (IsCategory == true && IsTest == true && IsParameter == true && IsLabName == true )
                //    if (IsCategory == true && IsTest == true && IsParameter == true && IsUnit == true && IsLabName == true && IsValue == true && IsResult == true)
                {
                    string msgTitle = "";
                    string msgText = "Are You Sure \n  You Want To Save ?";

                    MessageBoxControl.MessageBoxChildWindow msgWin =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosed);

                    msgWin.Show();
                }
                else if (IsCategory == false)
                    cmbCategory.Focus();
                else if (IsTest == false)
                    cmbTest.Focus();
                else if (IsParameter == false)
                    cmbParameter.Focus();
                else if (IsLabName == false)
                    cmbLab.Focus();
                //else if (IsValue == false)
                //    txtValue.Focus();
                //else if (IsResult == false)
                //    cmbResultType.Focus();
                //else if (IsUnit == false)
                //    cmbUnit.Focus();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Saves OT Table
        /// </summary>
        /// <param name="result">Message  Box result</param>
        void msgWin_OnMessageBoxClosed(MessageBoxResult result)
        {
            try
            {
                if (result == MessageBoxResult.Yes)
                {
                    clsPathoTestResultEntryBizActionVO bizActionVO = new clsPathoTestResultEntryBizActionVO();
                    clsPathoResultEntryVO addNewTestEntryVO = new clsPathoResultEntryVO();
                    if (cmbUnit.SelectedItem != null)
                    {
                       //bizActionVO.PathoResultEntry.ParameterUnitID = ((MasterListItem)cmbUnit.SelectedItem).ID;
                        addNewTestEntryVO.ParameterUnitID = ((MasterListItem)cmbUnit.SelectedItem).ID;
                    }

                    addNewTestEntryVO = (clsPathoResultEntryVO)this.DataContext;
                    addNewTestEntryVO = CreateFormData();
                    addNewTestEntryVO.ParameterUnitID  = ((MasterListItem)cmbUnit.SelectedItem).ID;
                    addNewTestEntryVO.ParameterUnitName = ((MasterListItem)cmbUnit.SelectedItem).Description;

                    addNewTestEntryVO.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    // addNewBlockVO.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewTestEntryVO.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addNewTestEntryVO.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addNewTestEntryVO.AddedDateTime = System.DateTime.Now;
                    addNewTestEntryVO.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    bizActionVO.PathoResultEntry = addNewTestEntryVO;
                    //bizActionVO.objBedMatserDetails.Add(addNewBlockVO);

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            msgText = "Record Is Successfully Submitted!";

                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                            //After Insertion Back to BackPanel and Setup Page
                           //fillBackGrid();
                           // SetupPage();
                            ClearUI();
                            SetupPage();
                            objAnimation.Invoke(RotationType.Backward);
                            SetCommandButtonState("Save");
                        }
                    };
                    client.ProcessAsync(bizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void fillBackGrid() 
        {
            try
            {
                clsGetPathoTestResultEntryDateWiseBizActionVO bizActionVO = new clsGetPathoTestResultEntryDateWiseBizActionVO();
                //bizActionVO.SearchExpression = "";
                //bizActionVO.PagingEnabled = true;
               bizActionVO.Date = System.DateTime.Now.Date;
                //bizActionVO.MaximumRows = MasterListBack.PageSize;
                //bizActionVO.StartRowIndex = MasterListBack.PageIndex * MasterListBack.PageSize;
                bizActionVO.PatientID = (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
                bizActionVO.UnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;
                bizActionVO.fromform = true;

                // getstoreinfo = new clsIPDBedMasterVO();
                bizActionVO.PathoResultEntry = new List<clsPathoResultEntryVO>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        bizActionVO.PathoResultEntry = (((clsGetPathoTestResultEntryDateWiseBizActionVO)args.Result).PathoResultEntry);
                       // grdResultEntryBack.DataContext = null;
                        if (bizActionVO.PathoResultEntry.Count > 0)
                        {
                            MasterListBack.Clear();
                            MasterListBack.TotalItemCount = (int)(((clsGetPathoTestResultEntryDateWiseBizActionVO)args.Result).TotalRows);
                            foreach (clsPathoResultEntryVO item in bizActionVO.PathoResultEntry)
                            {
                                MasterListBack.Add(item);
                                TestListBack.Add(item);
                            }
                            PagedCollectionView collection = new PagedCollectionView(MasterListBack);
                            collection.GroupDescriptions.Add(new PropertyGroupDescription("Category"));

                            grdResultEntryBack.DataContext = collection;
                        }
                    }
                };
                client.ProcessAsync(bizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void CmdCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsCancel == true)
                {
                    if (Menu != null && Menu.Parent == "Surrogacy")
                    {
                        ModuleName = "PalashDynamics";
                        Action = "PalashDynamics.Forms.PatientView.PatientListForSurrogacy";
                    }
                    else
                    {
                        ModuleName = "PalashDynamics";
                        Action = "CIMS.Forms.PatientList";
                    }
                    UserControl rootPage = Application.Current.RootVisual as UserControl;

                    WebClient c2 = new WebClient();
                    c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                    c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
                }
                else
                {
                    objAnimation.Invoke(RotationType.Backward);
                    IsCancel = true;
                    SetupPage();
                }
                SetCommandButtonState("Cancel");
                #region Commented
                ////SetCommandButtonState("Cancel");
                ////objAnimation.Invoke(RotationType.Backward);
                ////if (IsCancel == true)
                ////{
                ////    ModuleName = "PalashDynamics.Pathology";
                ////    Action = "PalashDynamics.Pathology.frmIPDConfiguration";
                ////    UserControl rootPage = Application.Current.RootVisual as UserControl;

                ////    TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                ////    mElement.Text = "IPD Configuration";

                ////    WebClient c = new WebClient();
                ////    c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
                ////    c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
                ////}
                ////else
                ////{
                ////    IsCancel = true;
                ////}
                #endregion
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// clears UI
        /// </summary>
        public void ClearUI()
        {
            this.grdBackPanel.DataContext = new clsPathoResultEntryVO();
            //CategoryId = 0;
            //TestId = 0;
            cmbCategory.SelectedValue = (long)0;//((clsPathoResultEntryVO)this.DataContext).CategoryID;

            cmbTest.SelectedValue = (long)0; //((clsPathoResultEntryVO)this.DataContext).TestID;

            cmbParameter.SelectedValue = (long)0;// ((clsPathoResultEntryVO)this.DataContext).ParameterID;
            
            cmbTest.ItemsSource = null;
            cmbParameter.ItemsSource = null;
            ProcDate.SelectedDate = DateTime.Now.Date.Date;
            ProcTime.Value = DateTime.Now;
            txtValue.Text = "";
            cmbResultType.SelectedValue = 0;// ((clsPathoResultEntryVO)this.DataContext).ResultType;
            cmbParameter.SelectedValue = (long)0;

            txtNotes.Text = "";
            //hlbEditTemplate.Content = "";
        }

        /// <summary>
        /// set command button set
        /// </summary>
        /// <param name="strFormMode">button content</param>
        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    CmdSave.IsEnabled = false;
                    CmdCancel.IsEnabled = true;
                    CmdNew.IsEnabled = true;
                    IsCancel = true;
                    break;

                case "New":
                    CmdSave.IsEnabled = true;
                    CmdNew.IsEnabled = false;
                    CmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;

                case "Save":
                    CmdNew.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                    CmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;

                //case "Modify":
                //    CmdNew.IsEnabled = true;
                //    CmdCancel.IsEnabled = true;
                // //   IsCancel = true;
                //    break;

                case "Cancel":
                    CmdNew.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                    CmdCancel.IsEnabled = true;
                    break;

                //case "FrontPanel":
                //    CmdNew.IsEnabled = true;
                //    CmdSave.IsEnabled = false;

                //   break;
                case "View":
                    CmdSave.IsEnabled = false;
                    CmdNew.IsEnabled = false;
                    CmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Gets ID of existing OTTable or new OT Table
        /// </summary>
        /// <returns>clsIPDBlockMasterVO object</returns>
        private clsPathoResultEntryVO CreateFormData()
        {
            clsPathoResultEntryVO addNewResult = new clsPathoResultEntryVO();

            addNewResult.ID = 0;
            addNewResult = (clsPathoResultEntryVO)this.grdBackPanel.DataContext;
            addNewResult.Status = true;

            addNewResult.CategoryID = ((MasterListItem)cmbCategory.SelectedItem).ID;
            addNewResult.TestID = ((MasterListItem)cmbTest.SelectedItem).ID;
            addNewResult.ParameterID = ((clsPathoTestParameterVO)cmbParameter.SelectedItem).ParamSTID; //((MasterListItem)cmbParameter.SelectedItem).ID;
            addNewResult.LabID = ((MasterListItem)cmbLab.SelectedItem).ID;
            addNewResult.Date = (DateTime)ProcDate.SelectedDate;
            addNewResult.Time = (DateTime)ProcTime.Value;
           // addNewResult.ResultValue = txtValue.Text;
            if (((clsPathoTestParameterVO)cmbParameter.SelectedItem).IsNumeric == true)
            {
                if (txtValue.Text != null)
                    addNewResult.ResultValue = txtValue.Text;
            }
            else
            {
                if (cmbValueType.SelectedItem != null)
                {
                    addNewResult.ResultValue = ((clsPathoTestParameterVO)cmbValueType.SelectedItem).Description;
                }
            }
            addNewResult.ResultType = ((MasterListItem)cmbResultType.SelectedItem).ID;
            addNewResult.Note = txtNotes.Text;

            addNewResult.PatientID = ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
            addNewResult.PatientUnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;

            addNewResult.AttachmentFileName = fileName1;
            addNewResult.Attachment = AttachedFileContents;

            return addNewResult;
        }

        /// <summary>
        /// Fills Category Master combo
        /// </summary>
        void FillCategoryMaster()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_PathoCategory;
                BizAction.Parent = new KeyValue();
                BizAction.Parent.Key = "1";
                BizAction.Parent.Value = "Status";
                //BizAction.MasterTable.f
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        cmbCategory.ItemsSource = null;
                        cmbCategory.ItemsSource = objList;
                    }
                    if (this.DataContext != null)
                    {
                        if (IsNew == true)
                            cmbCategory.SelectedValue = ((clsPathoResultEntryVO)this.DataContext).CategoryID;
                        //else if (((clsPathoResultEntryVO)grdResultEntry.SelectedItem).CategoryID != null && ((clsPathoResultEntryVO)grdResultEntry.SelectedItem).CategoryID > 0)
                        else if ((clsPathoResultEntryVO)grdResultEntry.SelectedItem != null)
                        {
                            cmbCategory.SelectedValue = ((clsPathoResultEntryVO)grdResultEntry.SelectedItem).CategoryID;
                         }
                        else
                            cmbCategory.SelectedValue = (long)0;
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// Fetches Patho test & fills front panel grid
        /// </summary>
        private void FillTestMaster()
        {
            try
            {
                clsGetPathoTestListForResultEntryBizActionVO BizAction = new clsGetPathoTestListForResultEntryBizActionVO();
                BizAction.TestList = new List<clsPathoTestMasterVO>();
                if ((((IApplicationConfiguration)App.Current).SelectedPatient).Gender == Genders.None.ToString())
                    BizAction.ApplicaleTo = (int)Genders.None;
                else if ((((IApplicationConfiguration)App.Current).SelectedPatient).Gender == Genders.Male.ToString())
                    BizAction.ApplicaleTo = (int)Genders.Male;
                else if ((((IApplicationConfiguration)App.Current).SelectedPatient).Gender == Genders.Female.ToString())
                    BizAction.ApplicaleTo = (int)Genders.Female;
                BizAction.Category = CategoryId;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsGetPathoTestListForResultEntryBizActionVO)arg.Result).TestList != null)
                        {
                            clsGetPathoTestListForResultEntryBizActionVO result = arg.Result as clsGetPathoTestListForResultEntryBizActionVO;
                            List<clsPathoTestMasterVO> objList = new List<clsPathoTestMasterVO>();

                            List<MasterListItem> LstCheck = new List<MasterListItem>();
                            LstCheck.Add(new MasterListItem(0, "-- Select --"));
                            if (result.TestList != null)
                            {
                                objList.Clear();
                                foreach (var item in result.TestList)
                                {
                                    MasterListItem objMaster = new MasterListItem();
                                    objMaster.ID = item.ID;
                                    objMaster.Description = item.Description;
                                    LstCheck.Add(objMaster);
                                    //objList.Add(item);
                                }
                              
                                cmbTest.ItemsSource = null;
                                cmbTest.ItemsSource = LstCheck;
                            }
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }

                    if (IsNew == true)
                        cmbTest.SelectedValue = ((clsPathoResultEntryVO)this.DataContext).TestID;

                    else if ((clsPathoResultEntryVO)grdResultEntry.SelectedItem != null)
                    {
                        cmbTest.SelectedValue = ((clsPathoResultEntryVO)grdResultEntry.SelectedItem).TestID;
                    }

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        void FillParameterMaster()
        {
            try
            {
                clsGetPathoParameterSampleAndItemDetailsByTestIDBizActionVO BizAction = new clsGetPathoParameterSampleAndItemDetailsByTestIDBizActionVO();
                BizAction.ParameterList = new List<clsPathoTestParameterVO>();
                BizAction.ItemList = new List<clsPathoTestItemDetailsVO>();
                BizAction.SampleList = new List<clsPathoTestSampleVO>();
                if (cmbTest.SelectedItem != null && ((MasterListItem)cmbTest.SelectedItem).ID > 0)
                {
                    BizAction.TestID = TestId;
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {
                            clsGetPathoParameterSampleAndItemDetailsByTestIDBizActionVO DetailsVO = new clsGetPathoParameterSampleAndItemDetailsByTestIDBizActionVO();
                            DetailsVO = (clsGetPathoParameterSampleAndItemDetailsByTestIDBizActionVO)arg.Result;

                            if (DetailsVO.ParameterList != null)
                            {
                                //List<MasterListItem> objList = new List<MasterListItem>();
                                //objList.Add(new MasterListItem(0, "-- Select --"));

                                //for (int i = 0; i < DetailsVO.ParameterList.Count; i++)
                                //{
                                //    MasterListItem obj = new MasterListItem();
                                //    obj.ID = DetailsVO.ParameterList[i].ID;
                                //    obj.Description = DetailsVO.ParameterList[i].Description;
                                //    objList.Add(obj);
                                //}

                                List<clsPathoTestParameterVO> objList = new List<clsPathoTestParameterVO>();

                                for (int i = 0; i < DetailsVO.ParameterList.Count; i++)
                                {
                                    clsPathoTestParameterVO obj = new clsPathoTestParameterVO();
                                    if (i == 0)
                                    {
                                        obj.ID = 0;
                                        obj.Description = "-- Select --";
                                        objList.Add(obj);
                                    }

                                    obj = DetailsVO.ParameterList[i];
                                    objList.Add(obj);
                                }
                                cmbParameter.ItemsSource = null;
                                cmbParameter.ItemsSource = objList;
                            }
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("", "Error Occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                            msgW1.Show();
                        }
                        if (this.DataContext != null)
                        {
                            if (IsNew == true)
                                cmbParameter.SelectedValue = (long)0;
                            else
                                cmbParameter.SelectedValue = ((clsPathoResultEntryVO)grdResultEntry.SelectedItem).ParameterID;
                        }
                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
           
            }
            catch (Exception ex)
            {
                throw;
            }
        }

       // By BHUSHAN .....
        public void FillParameUnitMaster(long ParamId)
        {
            try
            {
                 clsGetPathoParameterUnitBizActionVO BizAction = new clsGetPathoParameterUnitBizActionVO();
                 BizAction.MasterList = new List<MasterListItem>();
                 BizAction.ParamID  = ParamId;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
       
                        objList.AddRange(((clsGetPathoParameterUnitBizActionVO)arg.Result).MasterList);

                        cmbUnit.ItemsSource = null;
                            //if (objList.Count == 0)
                            //{
                            //    objList.Add(new MasterListItem(0, "NA"));
                            //    cmbUnit.ItemsSource = objList;
                            //    cmbUnit.SelectedValue = (long)0;
                            //}
                            //else
                            {
                                objList.Add(new MasterListItem(0, "- Select -"));
                                cmbUnit.ItemsSource = objList;
                                if (this.DataContext != null)
                                {
                                    if (IsNew == true)
                                        cmbUnit.SelectedValue = (long)0;
                                    else
                                        cmbUnit.SelectedValue = ((clsPathoResultEntryVO)grdResultEntry.SelectedItem).ParameterUnitID;
                                }
                            }
                    }
                };   
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Fills Lab Master combo
        /// </summary>
        void FillLabMaster()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_LaboratoryMaster;
                BizAction.Parent = new KeyValue();
                BizAction.Parent.Key = "1";
                BizAction.Parent.Value = "Status";
                //BizAction.MasterTable.f
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();

                        //objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        cmbLab.ItemsSource = null;
                        cmbLab.ItemsSource = objList;
                        cmbLab.SelectedValue = (long)2;
                    }
                    if (this.DataContext != null)
                    {
                        cmbLab.SelectedValue = ((clsPathoResultEntryVO)this.DataContext).LabID;
                        cmbLab.SelectedValue = (long)2;
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Fills Result Master combo
        /// </summary>
        void FillResultMaster()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_ResultTypeMaster;
                BizAction.Parent = new KeyValue();
                BizAction.Parent.Key = "1";
                BizAction.Parent.Value = "Status";      
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        cmbResultType.ItemsSource = null;
                        cmbResultType.ItemsSource = objList;
                    }
                    if (this.DataContext != null)
                    {
                        cmbResultType.SelectedValue = ((clsPathoResultEntryVO)this.DataContext).ResultType;
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetCommandButtonState("View");
                if (grdResultEntry.SelectedItem != null)
                {
                    IsNew = false;
                    //OTTableID = ((clsIPDBedMasterVO)grdBed.SelectedItem).ID;
                    grdBackPanel.DataContext = ((clsPathoResultEntryVO)grdResultEntry.SelectedItem);
                   // FillCategoryMaster();
                    if (((clsPathoResultEntryVO)grdResultEntry.SelectedItem).CategoryID != null)
                        cmbCategory.SelectedValue = ((clsPathoResultEntryVO)grdResultEntry.SelectedItem).CategoryID;
                    if (((clsPathoResultEntryVO)grdResultEntry.SelectedItem).TestID != null)
                        cmbTest.SelectedValue = ((clsPathoResultEntryVO)grdResultEntry.SelectedItem).TestID;
                    if (((clsPathoResultEntryVO)grdResultEntry.SelectedItem).ParameterID != null)
                        cmbParameter.SelectedValue = ((clsPathoResultEntryVO)grdResultEntry.SelectedItem).ParameterID;
                    if (((clsPathoResultEntryVO)grdResultEntry.SelectedItem).ParameterUnitID != null)
                        cmbUnit.SelectedValue = ((clsPathoResultEntryVO)grdResultEntry.SelectedItem).ParameterUnitID;
                    if (((clsPathoResultEntryVO)grdResultEntry.SelectedItem).Date != null)
                        ProcDate.SelectedDate = ((clsPathoResultEntryVO)grdResultEntry.SelectedItem).Date;
                    if (((clsPathoResultEntryVO)grdResultEntry.SelectedItem).Time != null)
                        ProcTime.Value = ((clsPathoResultEntryVO)grdResultEntry.SelectedItem).Time;
               
                    // By BHUSHAN
                    cmbLab.SelectedValue = ((clsPathoResultEntryVO)grdResultEntry.SelectedItem).LabID;
                    //  txtValue.Text = ((clsPathoResultEntryVO)grdResultEntry.SelectedItem).ResultValue;
                    cmbResultType.SelectedValue = ((clsPathoResultEntryVO)grdResultEntry.SelectedItem).ResultType;
                    txtNotes.Text = ((clsPathoResultEntryVO)grdResultEntry.SelectedItem).Note;
                    fileName1 = ((clsPathoResultEntryVO)grdResultEntry.SelectedItem).AttachmentFileName;
                    AttachedFileContents = ((clsPathoResultEntryVO)grdResultEntry.SelectedItem).Attachment;
                  
                    if (((clsPathoResultEntryVO)grdResultEntry.SelectedItem).ResultValue != null)
                    {
                        if (((clsPathoResultEntryVO)grdResultEntry.SelectedItem).IsNumeric == true)
                        {
                            txtValue.Text = ((clsPathoResultEntryVO)grdResultEntry.SelectedItem).ResultValue;
                        }
                        else
                        {
                            if (cmbValueType.ItemsSource != null)
                            {
                                foreach (clsPathoTestParameterVO item in ((List<clsPathoTestParameterVO>)cmbValueType.ItemsSource))
                                {
                                    if (item.HelpValue == ((clsPathoResultEntryVO)grdResultEntry.SelectedItem).ResultValue)
                                    {
                                        cmbValueType.SelectedValue = item.ID;
                                        break;
                                    }
                                }
                            }


                        }
                    }
                  
                    //hlbEditTemplate.Content = ((clsPathoResultEntryVO)grdResultEntry.SelectedItem).AttachmentFileName;

                    objAnimation.Invoke(RotationType.Forward);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void cmbCategory_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            cmbParameter.SelectedValue = (long)0;
            cmbTest.SelectedValue = (long)0;
            cmbUnit.SelectedValue = (long)0;
            if (cmbCategory.SelectedItem != null)
            {
                CategoryId = ((MasterListItem)cmbCategory.SelectedItem).ID;
                if (CategoryId > 0)
                    FillTestMaster();
            }
        }

        private void cmbTest_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTest.SelectedItem != null)
            {
                TestId = ((MasterListItem)cmbTest.SelectedItem).ID;
                if (TestId > 0)
                    FillParameterMaster();
            }
        }

        //private void hblAttach_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog OpenFile = new OpenFileDialog();

        //    if (OpenFile.ShowDialog() == true)
        //    {
        //        try
        //        {
        //            //clsPathoResultEntryVO AttachmentDetails = new clsPathoResultEntryVO();

        //            using (Stream stream = OpenFile.File.OpenRead())
        //            {
        //                // Don't allow really big files (more than 5 MB).
        //                if (stream.Length < 5120000)
        //                {
        //                    //long i = FileAttachmentNo;
        //                    data1 = new byte[stream.Length];
        //                    stream.Read(data1, 0, (int)stream.Length);
        //                    Attachment1 = OpenFile.File;
        //                    fileName1 = OpenFile.File.Name;
        //                    hlbEditTemplate.Content = fileName1;
        //                    //AttachmentDetails.Attachment = data1;
        //                    //AttachmentDetails.AttachmentFileName = fileName1;

        //                    //AttachmentList.Add(AttachmentDetails);
        //                    //AddedFiles.Add(AttachmentDetails);
        //                    //AttachmentFiles.Add(AttachmentDetails.AttachmentFileName);
        //                    //dgEmailAttachmentList.ItemsSource = null;
        //                    //dgEmailAttachmentList.ItemsSource = AddedFiles;

        //                    //FileAttachmentNo = FileAttachmentNo + 1;
        //                }
        //                else
        //                {
        //                    MessageBox.Show("File must be less than 5 MB");
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            //throw;
        //        }
        //    }

        //}

        //private void hlbView_Click(object sender, RoutedEventArgs e)
        //{

        //    // if (!string.IsNullOrEmpty(txtFilePath.Text.Trim()))
        //    if (IsNew == true)
        //    {
        //        if (!string.IsNullOrEmpty(fileName1))
        //        {
        //            // HtmlPage.Window.Invoke("openAttachment", new string[] { txtFilePath.Text });
        //            HtmlPage.Window.Invoke("openAttachment", new string[] { fileName1 });
        //        }
        //    }
        //    else
        //    {
        //        fileName1 = ((clsPathoResultEntryVO)grdResultEntry.SelectedItem).AttachmentFileName;
        //        data1 = ((clsPathoResultEntryVO)grdResultEntry.SelectedItem).Attachment;
        //        //if (((clsEmailTemplateVO)dgTest.SelectedItem).IsCompleted == true)
        //        if (!string.IsNullOrEmpty(fileName1))
        //        {
        //            Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
        //            DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
        //            client.UploadETemplateFileCompleted += (s, args) =>
        //            {
        //                if (args.Error == null)
        //                {
        //                    HtmlPage.Window.Invoke("openAttachment", new string[] { fileName1 });
        //                }
        //            };
        //            client.UploadETemplateFileAsync(fileName1, data1); 
        //        }
        //        else
        //        {
        //            //MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "This test's report is not uploaded. Please upload the report then click on preview", MessageBoxButtons.Ok, MessageBoxIcon.Information);
        //            //mgbx.Show();
        //        }
        //    }
        //}
        
        private void cmdDeleteService_Click(object sender, RoutedEventArgs e)
        {
            if (grdResultEntry.SelectedItem != null)
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to Delete the selected Item ?";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        //TestList.RemoveAt(grdResultEntry.SelectedIndex);
                        //grdResultEntry.Focus();
                        //grdResultEntry.UpdateLayout();
                        //grdResultEntry.SelectedIndex = TestList.Count - 1;
                        //grdResultEntry.ItemsSource = TestList;
                        DeleteTheRecord();

                    }
                };
                msgWD.Show();
            }
        }

        private void DeleteTheRecord()
        {
            clsDeletePathoTestResultEntryBizActionVO BizAction = new clsDeletePathoTestResultEntryBizActionVO();
            clsPathoResultEntryVO objBizAction = new clsPathoResultEntryVO();
            objBizAction.ID = ((clsPathoResultEntryVO)this.grdResultEntry.SelectedItem).ID;
            BizAction.PathoResultEntry = objBizAction;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    msgText = "Record is successfully Deleted!";

                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                    //After Insertion Back to BackPanel and Setup Page

                    SetupPage();
                    // objAnimation.Invoke(RotationType.Backward);
                    // SetCommandButtonState("Save");
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        #region File View Event

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFN.Text))
            {
                if (AttachedFileContents != null)
                {
                   // Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                   // DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);

                    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                    client.GlobalUploadFileCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            HtmlPage.Window.Invoke("GlobalOpenFile", new string[] { "UserUploadedFilesByTemplateTool/" + txtFN.Text.Trim() });
                            AttachedFileNameList.Add(txtFN.Text.Trim());

                        }
                    };
                    client.GlobalUploadFileAsync("../UserUploadedFilesByTemplateTool", txtFN.Text.Trim(), AttachedFileContents);
                }
            }
        }

        #endregion

        #region File Browse
        private void CmdBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            if (openDialog.ShowDialog() == true)
            {

                txtFN.Text = openDialog.File.Name;
                try
                {
                    using (Stream stream = openDialog.File.OpenRead())
                    {
                        AttachedFileContents = new byte[stream.Length];
                        stream.Read(AttachedFileContents, 0, (int)stream.Length);
                        fileName1 = openDialog.File.Name;
                    }
                }
                catch (Exception ex)
                {
                    string msgText = "Error while reading file.";

                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow("Palsh", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                }
            }
        }
        #endregion


        private void HyperlinkButton_Click_Male(object sender, RoutedEventArgs e)
        {
            if ((MaleAlert.Text.Trim()).Length > 0)
            {

                frmAttention PatientAlert = new frmAttention();
                PatientAlert.AlertsExpanded = CoupleDetails.MalePatient.Alerts;
                PatientAlert.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Attention Not Entered.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();

            }
        }

        private void HyperlinkButton_Click_Female(object sender, RoutedEventArgs e)
        {
            if ((FemaleAlert.Text.Trim()).Length > 0)
            {
                frmAttention PatientAlert = new frmAttention();
                PatientAlert.AlertsExpanded = CoupleDetails.FemalePatient.Alerts;
                PatientAlert.Show();
            }


            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Attention not entered.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();

            }
        }

        private void ViewImage_ClickBack(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((clsPathoResultEntryVO)grdResultEntry.SelectedItem).AttachmentFileName))
            {
                if (((clsPathoResultEntryVO)grdResultEntry.SelectedItem).Attachment != null)

                //if (AttachedFileContents != null)
                {
                    //Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    //DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);

                    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                    client.GlobalUploadFileCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            HtmlPage.Window.Invoke("GlobalOpenFile", new string[] { "UserUploadedFilesByTemplateTool/" + ((clsPathoResultEntryVO)grdResultEntry.SelectedItem).AttachmentFileName });
                            AttachedFileNameList.Add(((clsPathoResultEntryVO)grdResultEntry.SelectedItem).AttachmentFileName);

                        }
                    };
                    client.GlobalUploadFileAsync("../UserUploadedFilesByTemplateTool", ((clsPathoResultEntryVO)grdResultEntry.SelectedItem).AttachmentFileName, ((clsPathoResultEntryVO)grdResultEntry.SelectedItem).Attachment);
                }
            }
        }

        private void cmbParameter_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ParamId = 0;
            if (cmbParameter.SelectedItem != null)
            {
                ParamId = ((clsPathoTestParameterVO)cmbParameter.SelectedItem).ParamSTID;
                if (ParamId > 0)
                {
                    FillParameUnitMaster(ParamId);
                    // FillParameUnitMaster();
                }

                if (((clsPathoTestParameterVO)cmbParameter.SelectedItem).IsNumeric == true)
                {
                    txtValue.Visibility = Visibility.Visible;
                    cmbValueType.Visibility = Visibility.Collapsed;
                }
                else
                {
                    if (ParamId > 0)
                    {
                        txtValue.Visibility = Visibility.Collapsed;
                        cmbValueType.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        txtValue.Visibility = Visibility.Visible;
                        cmbValueType.Visibility = Visibility.Collapsed;
                    }

                    FillHelpValueData(ParamId);

                }

            }
        }
        private void FillHelpValueData(long ParamSTID)
        {
            try
            {
                clsGetHelpValuesFroResultEntryBizActionVO BizAction = new clsGetHelpValuesFroResultEntryBizActionVO();
                BizAction.HelpValueList = new List<clsPathoTestParameterVO>();
                BizAction.ParameterID = ParamSTID;    //ParameterID;


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {

                        if (((clsGetHelpValuesFroResultEntryBizActionVO)arg.Result).HelpValueList != null)
                        {
                            List<clsPathoTestParameterVO> objList = new List<clsPathoTestParameterVO>();

                            clsPathoTestParameterVO obj = new clsPathoTestParameterVO();

                            obj.ID = 0;
                            obj.Description = "-- Select --";
                            objList.Add(obj);

                            List<clsPathoTestParameterVO> objHelpValueList = new List<clsPathoTestParameterVO>();

                            objHelpValueList.AddRange(((clsGetHelpValuesFroResultEntryBizActionVO)arg.Result).HelpValueList);

                            foreach (clsPathoTestParameterVO item in objHelpValueList)
                            {
                                clsPathoTestParameterVO objHelpItem = new clsPathoTestParameterVO();
                                objHelpItem = item;
                                objHelpItem.Description = item.HelpValue;

                                objList.Add(objHelpItem);
                            }

                            cmbValueType.ItemsSource = objList;

                            cmbValueType.SelectedValue = (long)0;

                            //dgHelpValueList.ItemsSource = ((clsGetHelpValuesFroResultEntryBizActionVO)arg.Result).HelpValueList;
                            //NormalRange = ((clsGetHelpValuesFroResultEntryBizActionVO)arg.Result).HelpValueList;

                        }
                        if (this.DataContext != null && grdResultEntry.SelectedItem != null)
                        {
                            if (((clsPathoResultEntryVO)grdResultEntry.SelectedItem).IsNumeric == true)
                            {
                                txtValue.Text = ((clsPathoResultEntryVO)grdResultEntry.SelectedItem).ResultValue;
                            }
                            else
                            {
                                if (cmbValueType.ItemsSource != null)
                                {
                                    foreach (clsPathoTestParameterVO item in ((List<clsPathoTestParameterVO>)cmbValueType.ItemsSource))
                                    {
                                        if (item.HelpValue == ((clsPathoResultEntryVO)grdResultEntry.SelectedItem).ResultValue)
                                        {
                                            cmbValueType.SelectedValue = item.ID;
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }

                };

                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void txtStoreCode_LostFocus(object sender, RoutedEventArgs e)
        { 
            clsGetResultOnParameterSelectionBizActionVO Bizaction = new clsGetResultOnParameterSelectionBizActionVO();
               
            if (cmbParameter.SelectedItem != null)
            {
                ParamId = ((clsPathoTestParameterVO)cmbParameter.SelectedItem).ParamSTID;
             }
            else 
            {
                msgText = "Please Select Parameter. ";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
            }
            Bizaction.ParamID = ParamId;
            Bizaction.DOB = ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).DateOfBirth;
            Bizaction.Gender = ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).Gender;

            if (txtValue.Text != "" && txtValue.Text != String.Empty)
            {
                Bizaction.resultValue = Convert.ToDouble(txtValue.Text);
            }
            else
            {                
                txtValue.Text = " 0 ";
                Bizaction.resultValue = Convert.ToDouble(txtValue.Text);
            }

               Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
               PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList = cmbResultType.ItemsSource as List<MasterListItem>;

                        Bizaction.PathoResultEntry = new List<clsPathoResultEntryVO>();
                        Bizaction.PathoResultEntry = ((clsGetResultOnParameterSelectionBizActionVO)args.Result).PathoResultEntry;

                        cmbResultType.SelectedItem = objList.Where(z => z.ID == 0).FirstOrDefault();
                        foreach (clsPathoResultEntryVO item in Bizaction.PathoResultEntry)
                        {
                            if ((Bizaction.resultValue <= item.MaxValue && Bizaction.resultValue >= item.MinValue && item.MaxValue != 0 && item.MinValue != 0)  )                         
                            {
                                cmbResultType.SelectedItem = objList.Where(z => z.ID == 1).FirstOrDefault();
                            }
                            else
                            {                               
                                cmbResultType.SelectedItem = objList.Where(z => z.ID == 2).FirstOrDefault();
                            }
                        }                        
                    }
                    };
                    client.ProcessAsync(Bizaction, new clsUserVO());
                    client.CloseAsync();
                }

        private void cmdDeleteServiceBack_Click(object sender, RoutedEventArgs e)
        {

            if (grdResultEntryBack.SelectedItem != null)
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to Delete the selected Item ?";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                       
                        DeleteTheRecordBack();

                    }
                };
                msgWD.Show();
            }
        }
        private void DeleteTheRecordBack() 
        {
            clsDeletePathoTestResultEntryBizActionVO BizAction = new clsDeletePathoTestResultEntryBizActionVO();
            clsPathoResultEntryVO objBizAction = new clsPathoResultEntryVO();
            objBizAction.ID = ((clsPathoResultEntryVO)this.grdResultEntryBack.SelectedItem).ID;
            BizAction.PathoResultEntry = objBizAction;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    msgText = "Record is successfully Deleted!";

                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                    //After Insertion Back to BackPanel and Setup Page

                   // fillBackGrid();
                    SetupPage();
                    // objAnimation.Invoke(RotationType.Backward);
                    // SetCommandButtonState("Save");
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

             
    }
}
