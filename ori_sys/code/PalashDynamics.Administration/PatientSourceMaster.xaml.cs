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
using PalashDynamics.UserControls;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Administration.PatientSourceMaster;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.Collections;
using System.ComponentModel;
using System.Reflection;

namespace PalashDynamics.Administration
{
    public partial class PatientSourceMaster : UserControl, INotifyPropertyChanged
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

        #region Variable Declaration
        private SwivelAnimation objAnimation;
        bool IsPageLoded = false;
        WaitIndicator Indicatior = null;
        public PagedSortableCollectionView<clsPatientSourceVO> DataList { get; private set; }
        bool IsCancel = true;

        #endregion

        public PatientSourceMaster()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));



            DataList = new PagedSortableCollectionView<clsPatientSourceVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            this.dataGrid2Pager.DataContext = DataList;
            this.grdMaster.DataContext = DataList;
            FetchData();

        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchData();
        }
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
                OnPropertyChanged("DataListPageSize");
            }
        }

        private void PatientSourceMaster_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();
                this.DataContext = new clsPatientSourceVO();


                SetCommandButtonState("Load");
                FillTariffList();
                txtcode.Focus();
                Indicatior.Close();
            }
            txtcode.Focus();
            txtcode.UpdateLayout();
            IsPageLoded = true;

        }

        private void FillTariffList()
        {
            clsGetTariffDetailsListBizActionVO BizAction = new clsGetTariffDetailsListBizActionVO();

            BizAction.PatientSourceDetails = new List<clsTariffDetailsVO>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            dgTariffList.ItemsSource = null;
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetTariffDetailsListBizActionVO)arg.Result).PatientSourceDetails != null)
                    {
                        BizAction.PatientSourceDetails = ((clsGetTariffDetailsListBizActionVO)arg.Result).PatientSourceDetails;
                        List<clsTariffDetailsVO> userTariffList = new List<clsTariffDetailsVO>();

                        foreach (var item in BizAction.PatientSourceDetails)
                        {
                            userTariffList.Add(new clsTariffDetailsVO() { TariffID = item.TariffID, TariffCode = item.TariffCode, TariffDescription = item.TariffDescription, Status = false, IsDefaultStatus = false });
                        }

                        dgTariffList.ItemsSource = userTariffList;

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

        private void SavePatientSource()
        {
            clsAddPatientSourceBizActionVO BizAction = new clsAddPatientSourceBizActionVO();
            BizAction.PatientDetails = (clsPatientSourceVO)this.DataContext;

            BizAction.PatientDetails.TariffDetails = (List<clsTariffDetailsVO>)dgTariffList.ItemsSource;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {

                if (arg.Error == null)
                {


                    if (((clsAddPatientSourceBizActionVO)arg.Result).SuccessStatus == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Patient Source Master Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        objAnimation.Invoke(RotationType.Backward);
                        SetCommandButtonState("Save");

                        FetchData();
                        ClearControl();
                    }
                    else if (((clsAddPatientSourceBizActionVO)arg.Result).SuccessStatus == 1)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be save because CODE already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                    else if (((clsAddPatientSourceBizActionVO)arg.Result).SuccessStatus == 2)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be save because DESCRIPTION already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();

                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }



            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void chkTariff_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            this.SetCommandButtonState("New");
            this.DataContext = new clsPatientSourceVO();
            ClearControl();

            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + "New Patient Source Details";
            objAnimation.Invoke(RotationType.Forward);

        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            bool SavePatient = true;
            SavePatient = CheckValidation();
            if (SavePatient == true)
            {

                string msgTitle = "";
                string msgText = "Are you sure you want to save the Patient Source Master?";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                msgW.Show();

            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                SavePatientSource();
        }


        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Cancel");
            this.DataContext = null;

            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = "";

            objAnimation.Invoke(RotationType.Backward);
            if (IsCancel == true)
            {
                mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Patient Configuration";  

                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmPatientConfiguration") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);

            }
            else
            {
                IsCancel = true;
            }
        }

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            bool ModifyPatientSource = true;
            ModifyPatientSource = CheckValidation();
            if (ModifyPatientSource == true)
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to Update the Patient Source Master?";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);

                msgW1.Show();
            }

        }

        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                Modify();


        }

        private void Modify()
        {
            clsAddPatientSourceBizActionVO BizAction = new clsAddPatientSourceBizActionVO();
            BizAction.PatientDetails = (clsPatientSourceVO)this.DataContext;
            BizAction.PatientDetails.ID = ((clsPatientSourceVO)grdMaster.SelectedItem).ID;

            BizAction.PatientDetails.TariffDetails = (List<clsTariffDetailsVO>)dgTariffList.ItemsSource;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {

                if (arg.Error == null)
                {



                    Indicatior.Close();

                    if (((clsAddPatientSourceBizActionVO)arg.Result).SuccessStatus == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Patient Source Master Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        SetCommandButtonState("Modify");
                        objAnimation.Invoke(RotationType.Backward);
                        FetchData();
                        ClearControl();
                    }
                    else if (((clsAddPatientSourceBizActionVO)arg.Result).SuccessStatus == 1)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be Updated because CODE already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                    else if (((clsAddPatientSourceBizActionVO)arg.Result).SuccessStatus == 2)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be Updated because DESCRIPTION already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }

                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }



            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("View");
            ClearControl();
            FillData(((clsPatientSourceVO)grdMaster.SelectedItem).ID);


            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + ((clsPatientSourceVO)grdMaster.SelectedItem).Description;

            objAnimation.Invoke(RotationType.Forward);
        }

        private void FillData(long iID)
        {
            clsGetPatientSourceDetailsByIDBizActionVO BizAction = new clsGetPatientSourceDetailsByIDBizActionVO();
            BizAction.Details = new clsPatientSourceVO();
            BizAction.ID = iID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (grdMaster.SelectedItem != null)
                    {
                        clsGetPatientSourceDetailsByIDBizActionVO ObjTariff = new clsGetPatientSourceDetailsByIDBizActionVO();
                        ObjTariff = (clsGetPatientSourceDetailsByIDBizActionVO)arg.Result;

                        this.DataContext = null;
                        this.DataContext = ObjTariff.Details;

                        //if (ObjTariff != null)
                        //{
                        //    if (ObjTariff.Details.Code != null)
                        //    {
                        //        txtcode.Text = ObjTariff.Details.Code;
                        //    }
                        //    txtDescription.Text = ObjTariff.Details.Description;

                        List<clsTariffDetailsVO> lstTariff = (List<clsTariffDetailsVO>)dgTariffList.ItemsSource;
                        foreach (var item1 in ObjTariff.Details.TariffDetails)
                        {

                            foreach (var item in lstTariff)
                            {
                                if (item.TariffID == item1.TariffID)
                                {
                                    item.IsDefaultStatus = item1.IsDefaultStatus;
                                    item.Status = item1.Status;

                                }
                            }

                        }

                        dgTariffList.ItemsSource = null;
                        dgTariffList.ItemsSource = lstTariff;
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

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

            if (grdMaster.SelectedItem != null)
            {
                clsAddPatientSourceBizActionVO BizAction = new clsAddPatientSourceBizActionVO();
                BizAction.PatientDetails = (clsPatientSourceVO)grdMaster.SelectedItem;
                BizAction.PatientDetails.ID = ((clsPatientSourceVO)grdMaster.SelectedItem).ID;
                BizAction.PatientDetails.Status = Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked);
                BizAction.PatientDetails.TariffDetails = (List<clsTariffDetailsVO>)dgTariffList.ItemsSource;


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {

                    if (arg.Error == null)
                    {
                        string msgTitle = "";
                        string msgText = "Are you sure you want to Update the status of Patient Source Master?";

                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                        msgW5.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            if (res == MessageBoxResult.Yes)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW2 =
                        new MessageBoxControl.MessageBoxChildWindow("", "Status Updated Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW2.Show();

                            }
                        };
                        msgW5.Show();


                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

            }

        }


        private void FetchData()
        {
            clsGetPatientSourceListBizActionVO BizAction = new clsGetPatientSourceListBizActionVO();
            BizAction.PatientSourceDetails = new List<clsPatientSourceVO>();

            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetPatientSourceListBizActionVO)arg.Result).PatientSourceDetails != null)
                    {
                        //grdMaster.ItemsSource = ((clsGetPatientSourceListBizActionVO)arg.Result).PatientSourceDetails;

                        clsGetPatientSourceListBizActionVO result = arg.Result as clsGetPatientSourceListBizActionVO;

                        if (result.PatientSourceDetails != null)
                        {
                            DataList.Clear();
                            DataList.TotalItemCount = ((clsGetPatientSourceListBizActionVO)arg.Result).TotalRows;
                            foreach (clsPatientSourceVO item in result.PatientSourceDetails)
                            {
                                DataList.Add(item);
                            }

                            //grdMaster.ItemsSource = null;
                            //grdMaster.ItemsSource = DataList;

                            //DataPager.Source = null;
                            //DataPager.PageSize = BizAction.MaximumRows;
                            //DataPager.Source = DataList;

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


        #region Set Command Button State New/Save/Modify/Print

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdNew.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "New":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;
                case "Save":
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Modify":
                    cmdNew.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Cancel":
                    cmdNew.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;
                    break;
                case "View":
                    cmdModify.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Reset All Control

        private void ClearControl()
        {
            txtcode.Text = "";
            txtDescription.Text = "";

            if ((List<clsTariffDetailsVO>)dgTariffList.ItemsSource != null)
            {
                List<clsTariffDetailsVO> lList = (List<clsTariffDetailsVO>)dgTariffList.ItemsSource;
                foreach (var item in lList)
                {
                    item.Status = false;
                    item.IsDefaultStatus = false;
                }
                dgTariffList.ItemsSource = null;
                dgTariffList.ItemsSource = lList;
            }


        }

        #endregion

        #region Validation
        private bool CheckValidation()
        {
            bool result = true;
            if (IsPageLoded)
            {

                bool Chk = false;
                List<clsTariffDetailsVO> lList = (List<clsTariffDetailsVO>)dgTariffList.ItemsSource;
                foreach (var item in lList)
                {
                    if (item.Status == false)
                    {
                        Chk = true;
                    }
                    else
                    {
                        Chk = false;
                        break;
                    }

                }
                if (Chk == true)
                {
                    string msgTitle = "";
                    string msgText = "Please Select Tariff";

                    MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW.Show();
                    result = false;
                }
            }
            if (txtDescription.Text == "")
            {
                txtDescription.SetValidation("Please Enter Description");
                txtDescription.RaiseValidationError();
                txtDescription.Focus();

                result = false;
            }
            else
                txtDescription.ClearValidationError();
            if (txtcode.Text == "")
            {
                txtcode.SetValidation("Please Enter Code");
                txtcode.RaiseValidationError();
                txtcode.Focus();

                result = false;
            }
            else
                txtcode.ClearValidationError();

            return result;
        }

        string textBefore = "";
        int selectionStart = 0;
        int selectionLength = 0;

        private void txtDescription_LostFocus(object sender, RoutedEventArgs e)
        {
            txtDescription.Text = txtDescription.Text.ToTitleCase();
        }


        private void txtDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (string.IsNullOrEmpty(((TextBox)sender).Text))
            //{
            //}
            //else if (!string.IsNullOrEmpty(((TextBox)sender).Text))
            //{
            //    if (!((TextBox)sender).Text.IsItCharacter())
            //    {
            //        ((TextBox)sender).Text = textBefore;
            //        ((TextBox)sender).SelectionStart = selectionStart;
            //        ((TextBox)sender).SelectionLength = selectionLength;
            //        textBefore = "";
            //        selectionStart = 0;
            //        selectionLength = 0;
            //    }
            //}

        }

        private void txtDescription_KeyDown(object sender, KeyEventArgs e)
        {
            //textBefore = ((TextBox)sender).Text;
            //selectionStart = ((TextBox)sender).SelectionStart;
            //selectionLength = ((TextBox)sender).SelectionLength;
        }

        #endregion


    }
}
