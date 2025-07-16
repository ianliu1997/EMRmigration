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
using PalashDynamics.ValueObjects;
using PalashDynamics.Animations;
using PalashDynamics.Collections;
using System.Windows.Browser;
using PalashDynamics.Converters;
using System.IO;
using System.Windows.Resources;
using System.Xml.Linq;
using System.Reflection;
using System.Windows.Media.Imaging;
using DataDrivenApplication.Forms;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.Administration;
namespace PalashDynamics.IVF
{
    public partial class GeneralExaminationForFemale : UserControl, IInitiateCIMS, IInitiateCIMSIVF
    {
        #region IInitiateCIMS
        public void Initiate(string Mode)
        {
            switch (Mode)
            {
                case "NEWRE":


                    if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                    {
                        //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW5.Show();
                        IsPatientExist = false;
                        return;
                    }

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW5.Show();

                        IsPatientExist = false;
                        return;
                    }


                    if (((IApplicationConfiguration)App.Current).SelectedPatient.Gender == Genders.Male.ToString())
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Female Examination can not be used for Male Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW5.Show();

                        IsPatientExist = false;
                        return;
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
                case "NEWRE":


                    if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                    {
                        //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW5.Show();
                        flag = 2;
                        IsPatientExist = false;
                        return;
                    }

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW5.Show();
                        flag = 2;
                        IsPatientExist = false;
                        return;
                    }


                    if (((IApplicationConfiguration)App.Current).SelectedPatient.Gender == Genders.Male.ToString())
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Female Examination can not be used for Male Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW5.Show();
                        flag = 2;
                        IsPatientExist = false;
                        return;
                    }

                    IsPatientExist = true;
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

                    mElement.Text = " : " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).FirstName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).MiddleName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).LastName;

                    break;
            }

        }
        #endregion

        #region Public variables
        bool IsPatientExist = false;
        public bool IsPageLoded = false;
        private SwivelAnimation objAnimation;
        public long SelectedRecord;
        public bool IsCancel = true;
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
        public clsUserVO loggedinUser { get; set; }
        public string Action { get; set; }
        public string ModuleName { get; set; }
        #endregion

        #region Paging

        public PagedSortableCollectionView<clsGeneralExaminationVO> DataList { get; private set; }


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
                // RaisePropertyChanged("DataListPageSize");
            }
        }


        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchData();

        }
        #endregion
        public GeneralExaminationForFemale(clsPatientGeneralVO SelectedPatient, bool IsPatient)
        {
            InitializeComponent();
            //Paging
            DataList = new PagedSortableCollectionView<clsGeneralExaminationVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            IsPatientExist = IsPatient;
            ((IApplicationConfiguration)App.Current).SelectedPatient = SelectedPatient;
            this.Loaded += new RoutedEventHandler(GeneralExaminationForFemale_Loaded);
        }

        public GeneralExaminationForFemale()
        {
            InitializeComponent();
            //Paging
            DataList = new PagedSortableCollectionView<clsGeneralExaminationVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            this.Loaded += new RoutedEventHandler(GeneralExaminationForFemale_Loaded);
        }

        void GeneralExaminationForFemale_Loaded(object sender, RoutedEventArgs e)
        {
            FillBuilt();
          //  FillEyeColor();
           // FillHairColor();
           // FillSkinColor();
            if (IsPatientExist == false)
            {
                ////((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                ////((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                ////((IInitiateCIMS)App.Current).Initiate("VISIT");

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
                //return;
            }

            if (!IsPageLoded && ((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
            {
                SetCommandButtonState("Load");
                this.DataContext = new clsGeneralExaminationVO();
                fillCoupleDetails();
                FetchData();
                ClearFormData();
            }
            IsPageLoded = true;
        }


        //private void FillEyeColor()
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    BizAction.IsActive = true;
        //    BizAction.MasterTable = MasterTableNameList.M_EyeColor;
        //    BizAction.MasterList = new List<MasterListItem>();

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {
        //            //cmbMaritalStatus.ItemsSource = null;
        //            //cmbMaritalStatus.ItemsSource = ((clsGetMasterListBizActionVO)e.Result).MasterList;
        //            List<MasterListItem> objList = new List<MasterListItem>();

        //           // objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
        //            objList.Add(new MasterListItem(0, "- Select -", true));
        //            objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
        //            cmbEyeColor.ItemsSource = null;
        //            cmbEyeColor.ItemsSource = objList;


        //            cmbEyeColor.SelectedItem = objList[0];
        //        }


        //    };
        //    Client.ProcessAsync(BizAction, loggedinUser);
        //    Client.CloseAsync();

        //}

        //private void FillHairColor()
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    BizAction.IsActive = true;
        //    BizAction.MasterTable = MasterTableNameList.M_HairColor;
        //    BizAction.MasterList = new List<MasterListItem>();

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {
        //            //cmbMaritalStatus.ItemsSource = null;
        //            //cmbMaritalStatus.ItemsSource = ((clsGetMasterListBizActionVO)e.Result).MasterList;
        //            List<MasterListItem> objList = new List<MasterListItem>();

        //           // objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
        //            objList.Add(new MasterListItem(0, "- Select -", true));
        //            objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
        //            cmbHairColor.ItemsSource = null;
        //            cmbHairColor.ItemsSource = objList;


        //            cmbHairColor.SelectedItem = objList[0];
        //        }


        //    };
        //    Client.ProcessAsync(BizAction, loggedinUser);
        //    Client.CloseAsync();
        //}
        //private void FillSkinColor()
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    BizAction.IsActive = true;
        //    BizAction.MasterTable = MasterTableNameList.M_SkinColor;
        //    BizAction.MasterList = new List<MasterListItem>();

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {
        //            //cmbMaritalStatus.ItemsSource = null;
        //            //cmbMaritalStatus.ItemsSource = ((clsGetMasterListBizActionVO)e.Result).MasterList;
        //            List<MasterListItem> objList = new List<MasterListItem>();

        //          //  objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
        //            objList.Add(new MasterListItem(0, "- Select -", true));
        //            objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
        //            cmbSkinColor.ItemsSource = null;
        //            cmbSkinColor.ItemsSource = objList;


        //            cmbSkinColor.SelectedItem = objList[0];
        //        }


        //    };
        //    Client.ProcessAsync(BizAction, loggedinUser);
        //    Client.CloseAsync();
        //}
        private void FillBuilt()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_IvfBuilt;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    //cmbMaritalStatus.ItemsSource = null;
                    //cmbMaritalStatus.ItemsSource = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                    List<MasterListItem> objList = new List<MasterListItem>();

                  //  objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                    objList.Add(new MasterListItem(0, "- Select -", true));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbBuilt.ItemsSource = null;
                    cmbBuilt.ItemsSource = objList;


                    cmbBuilt.SelectedItem = objList[0];
                }


            };
            Client.ProcessAsync(BizAction, loggedinUser);
            Client.CloseAsync();
        }

       

        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to save the General examination?";
                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                msgW.Show();
            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                Save();
        }

        private void Save()
        {
            clsAddGeneralExaminationFemaleBizActionVO BizAction = new clsAddGeneralExaminationFemaleBizActionVO();
            BizAction.Details = (clsGeneralExaminationVO)this.DataContext;
            BizAction.Details.PatientID = (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
            BizAction.Details.PatientUnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;
          
            //By Anjali......
            
            if (AcenNo.IsChecked == true)
            {
                BizAction.Details.Acen1= false;
            }
            else
            {
                BizAction.Details.Acen1 = true;
            }

            if (HirsutismNo.IsChecked == true)
            {
                BizAction.Details.Hirsutism1 = false;
            }
            else
            {
                BizAction.Details.Hirsutism1 = true;
            }
            BizAction.Details.BuiltID = ((MasterListItem)cmbBuilt.SelectedItem).ID;
           // BizAction.Details.EyeColor1 = ((MasterListItem)cmbEyeColor.SelectedItem).ID;

           //BizAction.Details.HairColor1 = ((MasterListItem)cmbHairColor.SelectedItem).ID;
           //BizAction.Details.SkinColor1 = ((MasterListItem)cmbSkinColor.SelectedItem).ID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Examination Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                    SetCommandButtonState("Save");
                    FetchData();
                    objAnimation.Invoke(RotationType.Backward);
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

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            if (IsCancel == true)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                //((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                //((IInitiateCIMS)App.Current).Initiate("VISIT");
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
                FetchData();
            }
            SetCommandButtonState("Cancel");
        }

        #region Added by Priyanka
        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            this.SetCommandButtonState("New");
            ClearFormData();
            clsPatientGeneralVO FemalePatientDetails = new clsPatientGeneralVO();
            FemalePatientDetails = (clsPatientGeneralVO)Female.DataContext;
            clsGeneralExaminationVO obj = new clsGeneralExaminationVO();
            //obj.Height = (float)FemalePatientDetails.Height;
            //obj.Weight = (float)FemalePatientDetails.Weight;
            //obj.BMI = (float)FemalePatientDetails.BMI;
            this.DataContext = obj;// new clsGeneralExaminationVO();
            txtBMI.Text = "";
            txtBPsystolic.Text = "";
            txtBPdystolic.Text = "";
            txtHeight.Text = "";
            txtPulse.Text = "";
            txtWeight.Text = "";
            // By  bhushan.. 
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId
                  && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AllowClinicalTransaction == true)
                {
                    //do nothing
                }
                else
                    CmdSave.IsEnabled = false;
            }

            objAnimation.Invoke(RotationType.Forward);
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
        #region FillCouple Details

        private void fillCoupleDetails()
        {
            clsGetCoupleDetailsBizActionVO BizAction = new clsGetCoupleDetailsBizActionVO();
            BizAction.PatientID = (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
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
                        //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //         new MessageBoxControl.MessageBoxChildWindow("Palash", "Cannot Create Examination, Examination is Only For Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        //    msgW1.Show();
                        //    //((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                        //    //((IInitiateCIMS)App.Current).Initiate("VISIT");
                        //    ModuleName = "PalashDynamics";
                        //    Action = "CIMS.Forms.PatientList";
                        //    UserControl rootPage = Application.Current.RootVisual as UserControl;
                        //    WebClient c2 = new WebClient();
                        //    c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                        //    c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
                        //}
                        //else
                        //{
                            GetHeightAndWeight(BizAction.CoupleDetails);
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
                       // }
                    }
                    else
                    {
                        LoadPatientHeader();
                        #region Commented by Saily P on 260912. purpose, the form is applicable to donor patient
                        //MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //         new MessageBoxControl.MessageBoxChildWindow("Palash", "Cannot Create Examination, Examination is Only For Active Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        //msgW1.Show();
                        ////((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                        ////((IInitiateCIMS)App.Current).Initiate("VISIT");

                        //ModuleName = "PalashDynamics";
                        //Action = "CIMS.Forms.PatientList";
                        //UserControl rootPage = Application.Current.RootVisual as UserControl;

                        //WebClient c2 = new WebClient();
                        //c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                        //c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
                        #endregion
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

        #endregion

        private void hlbViewExamination_Click(object sender, RoutedEventArgs e)
        {
            if (dgExamination.SelectedItem != null)
            {
                SetCommandButtonState("Modify");
                this.DataContext = (clsGeneralExaminationVO)dgExamination.SelectedItem;
                SelectedRecord = ((clsGeneralExaminationVO)dgExamination.SelectedItem).ID;
                clsPatientGeneralVO FemalePatientDetails = new clsPatientGeneralVO();
                Female.DataContext = null;
                FemalePatientDetails = CoupleDetails.FemalePatient;
                FemalePatientDetails.Height = ((clsGeneralExaminationVO)dgExamination.SelectedItem).Height;
                FemalePatientDetails.Weight = ((clsGeneralExaminationVO)dgExamination.SelectedItem).Weight;


                //By Anjali
                cmbBuilt.SelectedValue =((clsGeneralExaminationVO)dgExamination.SelectedItem).BuiltID;
                //cmbEyeColor.SelectedValue = ((clsGeneralExaminationVO)dgExamination.SelectedItem).EyeColor1;
                //cmbSkinColor.SelectedValue = ((clsGeneralExaminationVO)dgExamination.SelectedItem).SkinColor1;
                //cmbHairColor.SelectedValue = ((clsGeneralExaminationVO)dgExamination.SelectedItem).HairColor1;
                if (((clsGeneralExaminationVO)dgExamination.SelectedItem).Acen1 == true)
                {
                    AcenYes.IsChecked = true;
                }
                else 
                {
                    AcenNo.IsChecked = true;
                }
                if (((clsGeneralExaminationVO)dgExamination.SelectedItem).Hirsutism1 == true)
                {
                    HirsutismYes.IsChecked = true;
                }
                else
                {
                    HirsutismNo.IsChecked = true;
                }
                if (((clsGeneralExaminationVO)dgExamination.SelectedItem).HIVPositive == true)
                {
                    chkHIV.IsChecked = true;
                }
                else
                {
                    chkHIV.IsChecked = true;
                }
                if (((clsGeneralExaminationVO)dgExamination.SelectedItem).HBVPositive == true)
                {
                    chkHBV.IsChecked = true;
                }
                else
                {
                    chkHBV.IsChecked = true;
                }
                if (((clsGeneralExaminationVO)dgExamination.SelectedItem).HCVPositive == true)
                {
                    chkHCV.IsChecked = true;
                }
                else
                {
                    chkHCV.IsChecked = true;
                }


                FemalePatientDetails.BMI = Convert.ToDouble(String.Format("{0:0.00}", ((clsGeneralExaminationVO)dgExamination.SelectedItem).BMI));
                Female.DataContext = FemalePatientDetails;
                CmdSave.IsEnabled = false;
                objAnimation.Invoke(RotationType.Forward);
            }
        }

        #region Set Command Button State New/Save/Modify/Print

        private void SetCommandButtonState(String strFormMode)
        {
            GeneralExamination.IsEnabled = true;
            PhysicalExamination.IsEnabled = true;
            Alterts.IsEnabled = true;

            switch (strFormMode)
            {
                case "Load":
                    cmdNew.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                    CmdClose.IsEnabled = true;
                    CmdPrint.IsEnabled = false;
                    IsCancel = true;
                    break;
                case "New":
                    cmdNew.IsEnabled = false;
                    CmdSave.IsEnabled = true;
                    CmdClose.IsEnabled = true;
                    CmdPrint.IsEnabled = false;
                    IsCancel = false;
                    break;
                case "Save":
                    cmdNew.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                    CmdClose.IsEnabled = true;
                    CmdPrint.IsChecked = false;
                    IsCancel = true;
                    break;
                case "Modify":
                    cmdNew.IsEnabled = false;
                    CmdSave.IsEnabled = false;
                    CmdClose.IsEnabled = true;
                    CmdPrint.IsEnabled = true;
                    IsCancel = false;
                    GeneralExamination.IsEnabled = false;
                    PhysicalExamination.IsEnabled = false;
                    Alterts.IsEnabled = false;
                    break;
                case "Cancel":
                    cmdNew.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                    CmdClose.IsEnabled = true;
                    CmdPrint.IsEnabled = false;
                    break;
                default:
                    break;
            }
        }

        #endregion

        #endregion

        private void FetchData()
        {
            clsGetGeneralExaminationFemaleBizActionVO BizAction = new clsGetGeneralExaminationFemaleBizActionVO();
            BizAction.List = new List<clsGeneralExaminationVO>();
            BizAction.PatientID = (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
            BizAction.UnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;
            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    
                    if (((clsGetGeneralExaminationFemaleBizActionVO)args.Result).List != null)
                    {
                        clsGetGeneralExaminationFemaleBizActionVO result = args.Result as clsGetGeneralExaminationFemaleBizActionVO;
                       
                        DataList.TotalItemCount = result.TotalRows;
                        if (result.List != null)
                        {
                            DataList.Clear();

                            foreach (var item in result.List)
                            {
                                
                                DataList.Add(item);
                            }
                                
                           
                            dgExamination.ItemsSource = null;
                            dgExamination.ItemsSource = DataList;
                           
                            dgDataPager.Source = null;
                            dgDataPager.PageSize = BizAction.MaximumRows;
                            dgDataPager.Source = DataList;
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

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        private bool Validation()
        {

            bool result = true;
            if (txtWeight.Text == "")
            {
                txtWeight.SetValidation("Please enter Weight");
                txtWeight.RaiseValidationError();
                txtWeight.Focus();
                result = false;
            }
            else if (txtWeight.Text == "0")
            {
                txtWeight.SetValidation("Please enter Weight");
                txtWeight.RaiseValidationError();
                txtWeight.Focus();
                result = false;
            }
            else
                txtWeight.ClearValidationError();

            if (txtHeight.Text == "")
            {
                txtHeight.SetValidation("Please enter Height");
                txtHeight.RaiseValidationError();
                txtHeight.Focus();
                result = false;
            }
            else if (txtHeight.Text == "0")
            {
                txtHeight.SetValidation("Please enter Height");
                txtHeight.RaiseValidationError();
                txtHeight.Focus();
                result = false;
            }
            else
                txtHeight.ClearValidationError();

            return result;
        }

        private void ClearFormData()
        {
            //txtAcne.Text = "";
            //txtAlerts.Text = "";

            //txtBuilt.Text = "";
            //txtCNS.Text = "";
            //txtComments.Text = "";
            //txtCVS.Text = "";
            //txtEyeColor.Text = "";
            //txtGeneralGenitalExam.Text = "";
            //txtHairColor.Text = "";

            //txtHirsutism.Text = "";
            //txtPA.Text = "";
            //txtPhysicalBuilt.Text = "";

            //txtRS.Text = "";
            //txtSkinColor.Text = "";
            //txtThyroid.Text = "";

            chkHIV.IsChecked = false;
        }

        #region Added by Saily P
        private void CmdPrint_Click(object sender, RoutedEventArgs e)
        {
            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/Patient/PatientHistory.aspx?Type=1&ID=" + SelectedRecord), "_blank");
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
                    txtBMI.Text = String.Format("{0:0.00}", TotalBMI);
                    return TotalBMI;
                }
            }
            catch (Exception ex)
            {
                return 0.0;
            }
        }

        private void txtWeight_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtWeight.Text) && !string.IsNullOrEmpty(txtHeight.Text))
            {
                double Height = Convert.ToDouble(txtHeight.Text.Trim());
                double Weight = Convert.ToDouble(txtWeight.Text.Trim());
                txtBMI.Text = Convert.ToString(CalculateBMI(Height, Weight));
            }
            else if (string.IsNullOrEmpty(txtWeight.Text))
                txtWeight.Focus();
            else if (string.IsNullOrEmpty(txtHeight.Text))
                txtHeight.Focus();
        }

       public long PatientID;
       public long PatientUnitID;
        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            // Modify By BHUSHAN.... 
            SelectedRecord = ((clsGeneralExaminationVO)dgExamination.SelectedItem).ID;
             PatientID = (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
             PatientUnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;

             HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/Patient/PatientHistory.aspx?Type=1&ID=" + SelectedRecord + "&PatientID=" + PatientID + "&PatientUnitID=" + PatientUnitID + "&PrintFomatID=" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID), "_blank");
        }

        #endregion

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

        private void HyperlinkButton_Click_Female(object sender, RoutedEventArgs e)
        {
            if (FemaleAlert.Text.Trim().Length > 0)
            {
                frmAttention PatientAlert = new frmAttention();
                PatientAlert.AlertsExpanded = CoupleDetails.FemalePatient.Alerts;
                PatientAlert.Show();
            }
        }

        private void HyperlinkButton_Click_Male(object sender, RoutedEventArgs e)
        {
            if (MaleAlert.Text.Trim().Length > 0)
            {
                frmAttention PatientAlert = new frmAttention();
                PatientAlert.AlertsExpanded = CoupleDetails.MalePatient.Alerts;
                PatientAlert.Show();
            }
        }

        private void txtWeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsNumberValid() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtWeight_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtHeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsNumberValid() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtHeight_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtBPsystolic_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsNumberValid() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtBPsystolic_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtBPdystolic_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsNumberValid() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtBPdystolic_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }


        private void txtPulse_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsNumberValid() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtPulse_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

    }
}
