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
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Reflection;
using PalashDynamics.UserControls;
using PalashDynamics.Animations;
using PalashDynamics.Collections;
using System.Windows.Media.Imaging;
using DataDrivenApplication.Forms;
namespace PalashDynamics.IVF
{
    public partial class FemaleHistory : UserControl, IInitiateCIMS
    {
        #region Public Variables
        bool IsPatientExist = false;
        public bool IsPageLoded = false;
        private SwivelAnimation objAnimation;
        public long SelectedRecord;
        public bool isModify = false;
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
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        #endregion

        #region Paging

        public PagedSortableCollectionView<clsFemaleHistoryVO> DataList { get; private set; }
        
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
        
        public void Initiate(string Mode)
        {
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
        }

        public FemaleHistory()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            //Paging
            DataList = new PagedSortableCollectionView<clsFemaleHistoryVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;

            this.DataContext = new clsFemaleHistoryVO();
            this.Loaded += new RoutedEventHandler(FemaleHistory_Loaded);
        }

        void FemaleHistory_Loaded(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            if (IsPatientExist == false)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                return;
            }
            else
            {
                //if (!IsPageLoded && ((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
                    this.DataContext = new clsFemaleHistoryVO();
                    FillBloodLoss();
                    FillInfertilityType();
                   // GetFamaleHistory();
                    fillCoupleDetails();
                    FetchData();
                    SetCommandButtonState("New");
            }
            IsPageLoded = true;
        }

        private void FetchData()
        {
            clsGetFemaleHistoryBizActionVO BizAction = new clsGetFemaleHistoryBizActionVO();
            BizAction.Details = new List<clsFemaleHistoryVO>();
            BizAction.PatientID = (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    if (((clsGetFemaleHistoryBizActionVO)args.Result).Details != null)
                    {
                        clsGetFemaleHistoryBizActionVO result = args.Result as clsGetFemaleHistoryBizActionVO;
                        DataList.TotalItemCount = result.TotalRows;
                        if (result.Details != null)
                        {
                            DataList.Clear();
                            foreach (var item in result.Details)
                            {
                                DataList.Add(item);
                            }
                            dgHistory.ItemsSource = null;
                            dgHistory.ItemsSource = DataList;
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
                    BizAction.CoupleDetails.MalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient;
                    BizAction.CoupleDetails.FemalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient;
                    CoupleDetails.MalePatient = new clsPatientGeneralVO();
                    CoupleDetails.FemalePatient = new clsPatientGeneralVO();
                    CoupleDetails = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails;
                    if (CoupleDetails.CoupleId == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Cannot Create Examination, Examination is Only For Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                    }
                    else
                    {
                        //Female.DataContext = BizAction.CoupleDetails.FemalePatient;
                        //Male.DataContext = BizAction.CoupleDetails.MalePatient;    
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
                        //txtBMI.Text = String.Format("{0:0.00}", FemalePatientDetails.BMI);
                        clsPatientGeneralVO MalePatientDetails = new clsPatientGeneralVO();
                        MalePatientDetails = CoupleDetails.MalePatient;
                        MalePatientDetails.Height = BizAction.CoupleDetails.MalePatient.Height;
                        MalePatientDetails.Weight = BizAction.CoupleDetails.MalePatient.Weight;
                        MalePatientDetails.BMI = Convert.ToDouble(String.Format("{0:0.00}", BizAction.CoupleDetails.MalePatient.BMI));
                        MalePatientDetails.Alerts = BizAction.CoupleDetails.MalePatient.Alerts;
                        Male.DataContext = MalePatientDetails;
                        //txtMBMI.Text = String.Format("{0:0.00}", MalePatientDetails.BMI);
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        
        #endregion

        //private void GetFamaleHistory()
        //{

        //    WaitIndicator indicator = new WaitIndicator();

        //    indicator.Show();

        //    clsGetFemaleHistoryBizActionVO BizAction = new clsGetFemaleHistoryBizActionVO();
        //   // BizAction.Details = (clsFemaleHistoryVO)this.DataContext;
        //    BizAction.PatientID = (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
        //    BizAction.UnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;      


        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    client.ProcessCompleted += (s, arg) =>
        //    {
        //        if (arg.Error == null && arg.Result != null)
        //        {
        //            clsFemaleHistoryVO objHistory = new clsFemaleHistoryVO();
        //            objHistory = ((clsGetFemaleHistoryBizActionVO)arg.Result).Details;

        //            if (objHistory != null)
        //            {
        //                    cmbInfertilityType.SelectedValue = (long)objHistory.InfertilityType;
        //                    cmbBloodLoss.SelectedValue = (long)objHistory.BloodLoss;

        //                    if (objHistory.ContraceptionUsed == true)
        //                        rdbContraceptionUsedY.IsChecked = objHistory.ContraceptionUsed;
        //                    else
        //                        rdbContraceptionUsedN.IsChecked = true;

        //                    if (objHistory.FemaleInfertility == true)
        //                        rdbFemaleInfertilityY.IsChecked = objHistory.ContraceptionUsed;
        //                    else
        //                        rdbFemaleInfertilityN.IsChecked = true;

        //                    if (objHistory.MaleInfertility == true)
        //                        rdbMaleInfertilityY.IsChecked = objHistory.MaleInfertility;
        //                    else
        //                        rdbMaleInfertilityN.IsChecked = true;

        //                    if (objHistory.SexualDisfunction == true)
        //                        rdbSexualDisfunctionY.IsChecked = objHistory.SexualDisfunction;
        //                    else
        //                        rdbSexualDisfunctionN.IsChecked = true;

        //                    if (objHistory.Regular == true)
        //                        rdbRegularY.IsChecked = objHistory.Regular;
        //                    else
        //                        rdbRegularN.IsChecked = true;

        //                    if (objHistory.Dymenorhea == true)
        //                        rdbDymenorheaY.IsChecked = objHistory.Dymenorhea;
        //                    else
        //                        rdbDymenorheaN.IsChecked = true;

        //                    if (objHistory.MidCyclePain == true)
        //                        rdbMidCyclePainY.IsChecked = objHistory.MidCyclePain;
        //                    else
        //                        rdbMidCyclePainN.IsChecked = true;

        //                    if (objHistory.InterMenstrualBleeding == true)
        //                        rdbInterMenstrualBleedingY.IsChecked = objHistory.InterMenstrualBleeding;
        //                    else
        //                        rdbInterMenstrualBleedingN.IsChecked = true;

        //                    if (objHistory.PreMenstrualTension == true)
        //                        rdbPreMenstrualTensionY.IsChecked = objHistory.PreMenstrualTension;
        //                    else
        //                        rdbPreMenstrualTensionN.IsChecked = true;
                        
        //                    if (objHistory.Dysparuneia == true)
        //                        rdbDysparuneiaY.IsChecked = objHistory.Dysparuneia;
        //                    else
        //                        rdbDysparuneiaN.IsChecked = true;
                        
        //                    if (objHistory.PostCoitalBleeding == true)
        //                        rdbPostCoitalBleedingY.IsChecked = objHistory.PostCoitalBleeding;
        //                    else
        //                        rdbPostCoitalBleedingN.IsChecked = true;

        //                    if (objHistory.PreviousIUI == true)
        //                        rdbPreviousIUIY.IsChecked = objHistory.PreviousIUI;
        //                    else
        //                        rdbPreviousIUIN.IsChecked = true;

        //                    if (objHistory.IUISuccessfull == true)
        //                        rdbIUISuccessfullY.IsChecked = objHistory.IUISuccessfull;
        //                    else
        //                        rdbIUISuccessfullN.IsChecked = true;

        //                    if (objHistory.PreviousIVF == true)
        //                        rdbPreviousIVFY.IsChecked = objHistory.PreviousIVF;
        //                    else
        //                        rdbPreviousIVFN.IsChecked = true;

        //                    if (objHistory.IVFSuccessfull == true)
        //                        rdbIVFSuccessfullY.IsChecked = objHistory.IVFSuccessfull;
        //                    else
        //                        rdbIVFSuccessfullN.IsChecked = true;

        //                    this.DataContext = objHistory;
        //            }
        //        }
        //        else
        //        {
                  
        //            MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                   new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

        //            msgW1.Show();
        //        }

        //        indicator.Close();

        //    };

        //    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    client.CloseAsync();

        //}

        public static object[] GetValues(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");
            }

            List<object> values = new List<object>();

            var fields = from field in enumType.GetFields()
                         where field.IsLiteral
                         select field;

            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(enumType);
                values.Add(value);
            }
            return values.ToArray();
        }

        public static void EnumToList(Type EnumType, List<MasterListItem> TheMasterList)
        {
            Array Values = GetValues(EnumType);
            foreach (int Value in Values)
            {               
                string Display = Enum.GetName(EnumType, Value);
                MasterListItem Item = new MasterListItem(Value, Display);
                //if(Value !=0)
                    TheMasterList.Add(Item);
            }
        }

        private void FillBloodLoss()
        {
            List<MasterListItem> lList = new List<MasterListItem>();
            //MasterListItem Default = new MasterListItem(0, "- Select -");
            //lList.Insert(0, Default);
            EnumToList(typeof(IVFBloodLoss), lList);
            cmbBloodLoss.ItemsSource = lList;
            // cmbPayMode.SelectedItem = Default;
            cmbBloodLoss.SelectedValue = (long)((clsFemaleHistoryVO)this.DataContext).BloodLoss;
        }

        private void FillInfertilityType()
        {
            List<MasterListItem> lList = new List<MasterListItem>();
            //MasterListItem Default = new MasterListItem(0, "- Select -");
            //lList.Insert(0, Default);            
            EnumToList(typeof(IVFInfertilityTypes), lList);
            cmbInfertilityType.ItemsSource = lList;
            // cmbPayMode.SelectedItem = Default;
            cmbInfertilityType.SelectedValue = (long)((clsFemaleHistoryVO)this.DataContext).InfertilityType;
        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            //((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
            ClearFormData();
            FetchData();
            SetCommandButtonState("New");
            objAnimation.Invoke(RotationType.Backward);
        }

        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            string msgTitle = "Palash";
            string msgText = "Are you sure you want to save the Female History?";
            MessageBoxControl.MessageBoxChildWindow msgW =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
            msgW.Show();
        }
               
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if (((clsFemaleHistoryVO)this.DataContext).ID == 0)
                    Save();
                else
                    Update();
            }
        }

        private void Save()
        {

            clsAddFamaleHistoryBizActionVO BizAction = new clsAddFamaleHistoryBizActionVO();
            BizAction.Details = (clsFemaleHistoryVO)this.DataContext;
            BizAction.Details.PatientID = (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;

            if (cmbInfertilityType.SelectedItem != null)
                BizAction.Details.InfertilityType = (IVFInfertilityTypes)(((MasterListItem)cmbInfertilityType.SelectedItem).ID);

            if (cmbBloodLoss.SelectedItem != null)
                BizAction.Details.BloodLoss = (IVFBloodLoss)(((MasterListItem)cmbBloodLoss.SelectedItem).ID);
                  
            if(rdbContraceptionUsedY.IsChecked == true)
                BizAction.Details.ContraceptionUsed = rdbContraceptionUsedY.IsChecked.Value;

            if (rdbFemaleInfertilityY.IsChecked == true)
                BizAction.Details.FemaleInfertility = rdbFemaleInfertilityY.IsChecked.Value;

            if (rdbMaleInfertilityY.IsChecked == true)
                BizAction.Details.MaleInfertility = rdbMaleInfertilityY.IsChecked.Value;

            if (rdbSexualDisfunctionY.IsChecked == true)
                BizAction.Details.SexualDisfunction = rdbSexualDisfunctionY.IsChecked.Value;

            if (rdbRegularY.IsChecked == true)
                BizAction.Details.Regular = rdbRegularY.IsChecked.Value;

            if (rdbDymenorheaY.IsChecked == true)
                BizAction.Details.Dymenorhea = rdbDymenorheaY.IsChecked.Value;

            if (rdbMidCyclePainY.IsChecked == true)
                BizAction.Details.MidCyclePain = rdbMidCyclePainY.IsChecked.Value;

            if (rdbInterMenstrualBleedingY.IsChecked == true)
                BizAction.Details.InterMenstrualBleeding = rdbInterMenstrualBleedingY.IsChecked.Value;

            if (rdbPreMenstrualTensionY.IsChecked == true)
                BizAction.Details.PreMenstrualTension = rdbPreMenstrualTensionY.IsChecked.Value;

            if (rdbDysparuneiaY.IsChecked == true)
                BizAction.Details.Dysparuneia = rdbDysparuneiaY.IsChecked.Value;

            if (rdbPostCoitalBleedingY.IsChecked == true)
                BizAction.Details.PostCoitalBleeding = rdbPostCoitalBleedingY.IsChecked.Value;

            if (rdbPreviousIUIY.IsChecked == true)
                BizAction.Details.PreviousIUI = rdbPreviousIUIY.IsChecked.Value;

            if (rdbIUISuccessfullY.IsChecked == true)
                BizAction.Details.IUISuccessfull = rdbIUISuccessfullY.IsChecked.Value;

            if (rdbPreviousIVFY.IsChecked == true)
                BizAction.Details.PreviousIVF = rdbPreviousIVFY.IsChecked.Value;
            
            if (rdbIVFSuccessfullY.IsChecked == true)
                BizAction.Details.IVFSuccessfull = rdbIVFSuccessfullY.IsChecked.Value;
            

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    ClearFormData();
                    FetchData();
                    SetCommandButtonState("New");
                    objAnimation.Invoke(RotationType.Backward);
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Female History Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                    this.DataContext = new clsFemaleHistoryVO();
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

        private void Update()
        {
            clsUpdateFemaleHistoryBizActionVO BizAction = new clsUpdateFemaleHistoryBizActionVO();
            BizAction.Details = (clsFemaleHistoryVO)this.DataContext;
            BizAction.Details.PatientID = (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
            BizAction.Details.UnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;

            if (cmbInfertilityType.SelectedItem != null)
                BizAction.Details.InfertilityType = (IVFInfertilityTypes)(((MasterListItem)cmbInfertilityType.SelectedItem).ID);

            if (cmbBloodLoss.SelectedItem != null)
                BizAction.Details.BloodLoss = (IVFBloodLoss)(((MasterListItem)cmbBloodLoss.SelectedItem).ID);

            if (rdbContraceptionUsedY.IsChecked == true)
                BizAction.Details.ContraceptionUsed = rdbContraceptionUsedY.IsChecked.Value;

            if (rdbFemaleInfertilityY.IsChecked == true)
                BizAction.Details.FemaleInfertility = rdbFemaleInfertilityY.IsChecked.Value;

            if (rdbMaleInfertilityY.IsChecked == true)
                BizAction.Details.MaleInfertility = rdbMaleInfertilityY.IsChecked.Value;

            if (rdbSexualDisfunctionY.IsChecked == true)
                BizAction.Details.SexualDisfunction = rdbSexualDisfunctionY.IsChecked.Value;

            if (rdbRegularY.IsChecked == true)
                BizAction.Details.Regular = rdbRegularY.IsChecked.Value;

            if (rdbDymenorheaY.IsChecked == true)
                BizAction.Details.Dymenorhea = rdbDymenorheaY.IsChecked.Value;

            if (rdbMidCyclePainY.IsChecked == true)
                BizAction.Details.MidCyclePain = rdbMidCyclePainY.IsChecked.Value;

            if (rdbInterMenstrualBleedingY.IsChecked == true)
                BizAction.Details.InterMenstrualBleeding = rdbInterMenstrualBleedingY.IsChecked.Value;

            if (rdbPreMenstrualTensionY.IsChecked == true)
                BizAction.Details.PreMenstrualTension = rdbPreMenstrualTensionY.IsChecked.Value;

            if (rdbDysparuneiaY.IsChecked == true)
                BizAction.Details.Dysparuneia = rdbDysparuneiaY.IsChecked.Value;

            if (rdbPostCoitalBleedingY.IsChecked == true)
                BizAction.Details.PostCoitalBleeding = rdbPostCoitalBleedingY.IsChecked.Value;

            if (rdbPreviousIUIY.IsChecked == true)
                BizAction.Details.PreviousIUI = rdbPreviousIUIY.IsChecked.Value;

            if (rdbIUISuccessfullY.IsChecked == true)
                BizAction.Details.IUISuccessfull = rdbIUISuccessfullY.IsChecked.Value;

            if (rdbPreviousIVFY.IsChecked == true)
                BizAction.Details.PreviousIVF = rdbPreviousIVFY.IsChecked.Value;

            if (rdbIVFSuccessfullY.IsChecked == true)
                BizAction.Details.IVFSuccessfull = rdbIVFSuccessfullY.IsChecked.Value;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Female History Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                    this.DataContext = new clsFemaleHistoryVO();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error Occured While Processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void hlbViewExamination_Click(object sender, RoutedEventArgs e)
        {
            if (dgHistory.SelectedItem != null)
            {
                SetCommandButtonState("Save");
                ClearFormData();
                this.DataContext = (clsFemaleHistoryVO)dgHistory.SelectedItem;
                SelectedRecord = ((clsFemaleHistoryVO)dgHistory.SelectedItem).ID;
                clsPatientGeneralVO MalePatientDetails = new clsPatientGeneralVO();
                Male.DataContext = null;
                MalePatientDetails = CoupleDetails.MalePatient;
                //MalePatientDetails.Height = ((clsFemaleHistoryVO)dgHistory.SelectedItem).Height;
                //MalePatientDetails.Weight = ((clsFemaleHistoryVO)dgHistory.SelectedItem).Weight;
                //MalePatientDetails.BMI = ((clsFemaleHistoryVO)dgHistory.SelectedItem).BMI;
                Male.DataContext = MalePatientDetails;
                CmdSave.IsEnabled = false;
            }
            objAnimation.Invoke(RotationType.Forward);
        }
        
        #region Set Command Button State New/Save/Modify/Print

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "New":
                    cmdNew.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                    CmdClose.IsEnabled = false;
                    break;
                case "Save":
                    cmdNew.IsEnabled = false;
                    CmdSave.IsEnabled = true;
                    CmdClose.IsEnabled = true;
                    break;
                case "Modify":
                    cmdNew.IsEnabled = false;
                    CmdSave.IsEnabled = false;
                    CmdClose.IsEnabled = true;
                    break;
                case "Cancel":
                    cmdNew.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                    CmdClose.IsEnabled = false;
                    break;
                default:
                    break;
            }
        }

        #endregion

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {   
            ClearFormData();
            isModify = false;
            SetCommandButtonState("Save");
            try
            {
                objAnimation.Invoke(RotationType.Forward);
            }
            catch (Exception  ex)
            {
                throw;
            }
        }

        private void ClearFormData()
        {
            this.DataContext = new clsFemaleHistoryVO();
        }

        private void WaterMarkTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((WaterMarkTextbox)sender).Text;
            selectionStart = ((WaterMarkTextbox)sender).SelectionStart;
            selectionLength = ((WaterMarkTextbox)sender).SelectionLength;
        }

        private void WaterMarkTextbox_OnTextChanged(object sender, RoutedEventArgs e)
        {
            if (IsPageLoded)
            {
                if (!string.IsNullOrEmpty(((WaterMarkTextbox)sender).Text))
                {                    
                    if (!((WaterMarkTextbox)sender).Text.IsNumberValid() && textBefore != null)
                    {
                        ((WaterMarkTextbox)sender).Text = textBefore;
                        ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                        ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                        textBefore = "";
                        selectionStart = 0;
                        selectionLength = 0;
                    }
                    else if (((WaterMarkTextbox)sender).Text.Length > 10)
                    {
                        ((WaterMarkTextbox)sender).Text = textBefore;
                        ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                        ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                        textBefore = "";
                        selectionStart = 0;
                        selectionLength = 0;
                    }
                }
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
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Attention not entered.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();

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
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Attention not entered.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();

            }
        }
    }
}
