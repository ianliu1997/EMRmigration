using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.Master.DoctorMaster;
using PalashDynamics.ValueObjects;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using System.Reflection;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Administration;
using System.Windows.Data;
using PalashDynamics.ValueObjects.Master.DoctorMaster;

namespace PalashDynamics.Administration
{
    public partial class PendingDoctorList : ChildWindow
    {
        public PendingDoctorList()
        {
            InitializeComponent();

            DataList = new PagedSortableCollectionView<clsDoctorVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;



            this.Loaded += new RoutedEventHandler(frmShareSelectionList_Loaded);
            this.DataContext = strListID;
        }
        public event RoutedEventHandler OnSaveButton_Click;

        public long DoctorID { get; set; }
        public string DoctorName { get; set; }
        public bool IsForPendingDoctor { get; set; }
        //private void OKButton_Click(object sender, RoutedEventArgs e)
        //{
        //    this.DialogResult = true;
        //}

        //private void CancelButton_Click(object sender, RoutedEventArgs e)
        //{
        //    this.DialogResult = false;
        //}




        #region Public Event
        public event RoutedEventHandler OnSelectButtonClick;
        public event RoutedEventHandler OnCancelButtonClick;
        #endregion

        #region Public Variable
        public bool isServiceRecord = false;
        public long TariffId;
        public long SpecId;
        public string strListID = string.Empty;
        public PagedSortableCollectionView<clsDoctorVO> DataList { get; private set; }


        #endregion

        #region Paging



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


        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {

            FillPendingDoctor();


        }


        #endregion

        void frmShareSelectionList_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = "Doctor List";
            if (IsForPendingDoctor == true)
            {
                FillPendingDoctor();
            }

        }


        private void FillPendingDoctor()
        {

            clsGetPendingDoctorDetails BizAction = new clsGetPendingDoctorDetails();

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == false)
            {
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }
            else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
            {
                BizAction.UnitID = 0;
            }

            if (txtFirstName1.Text != null)
            {
                BizAction.FirstName = txtFirstName1.Text;
            }
            if (txtLastName2.Text != null)
            {
                BizAction.LastName = txtLastName2.Text;
            }

            //   BizAction.DoctorTypeID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.ReferalDoctorTypeID;
            BizAction.IsPagingEnabled = true;
            BizAction.StartRowIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetPendingDoctorDetails)arg.Result).DoctorDetails != null)
                    {
                        clsGetPendingDoctorDetails result = arg.Result as clsGetPendingDoctorDetails;
                        List<MasterListItem> objDoctorList = new List<MasterListItem>();
                        if (result.DoctorDetails != null)
                        {
                            DataList.TotalItemCount = result.TotalRows;
                            DataList.Clear();
                            foreach (var item in result.DoctorDetails)
                            {
                                //MasterListItem Obj = new MasterListItem();
                                //Obj.ID = item.DoctorId;
                                //Obj.Description = item.DoctorName;
                                //objDoctorList.Add(item);
                                //((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder = item;
                                DataList.Add(item);
                            }

                            dgDoctorList.ItemsSource = null;
                            dgDoctorList.ItemsSource = DataList;
                            //dgDoctorList.Columns[1].Header = "Doctor Name";

                            DataPagerDoc.Source = null;
                            DataPagerDoc.PageSize = BizAction.MaximumRows;
                            DataPagerDoc.Source = DataList;
                        }
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            //if (OnSelectButtonClick != null)
            //{
            //var objDrList = dgDoctorList.ItemsSource;
            //strListID = string.Empty;
            //foreach (MasterListItem aa in objDrList)
            //{
            //    if (aa.Status == true)
            //    {
            //        if (strListID == string.Empty)
            //            strListID = aa.ID.ToString();
            //        else
            //            strListID = strListID + "," + aa.ID.ToString();
            //    }
            //}
            ////OnSaveButtonClick((clsPathOrderBookingDetailVO)(this.DataContext), e);
            //OnSelectButtonClick((this.strListID), e);

            DoctorID = ((clsDoctorVO)dgDoctorList.SelectedItem).DoctorId;
            DoctorName = ((clsDoctorVO)dgDoctorList.SelectedItem).DoctorName;
            if (OnSaveButton_Click != null)
                OnSaveButton_Click(this, new RoutedEventArgs());
            // }
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (OnCancelButtonClick != null)
            {
                //OnCancelButtonClick((clsPathOrderBookingDetailVO)(this.DataContext), e);
                OnCancelButtonClick((this.DataContext), e);
            }
            this.DialogResult = false;
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FillPendingDoctor();
        }

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsPersonNameValid())
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void TextName_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        //  List<clsDoctorVO> DoctorShareSelectList = new List<clsDoctorVO>();
        public List<clsDoctorVO> DoctorShareSelectList = new List<clsDoctorVO>();
        private void chkService_Click(object sender, RoutedEventArgs e)
        {
            //DoctorShareSelectList = new List<clsDoctorVO>();
            //if (DoctorShareSelectList == null)
            //{
            //    DoctorShareSelectList = new List<clsDoctorVO>();
            //}
            if (((CheckBox)sender).IsChecked == true)
            {
                foreach (var item in DataList)
                {
                    if (item.DoctorId == ((clsDoctorVO)dgDoctorList.SelectedItem).DoctorId)
                    {
                        DoctorShareSelectList.Add(item);
                    }
                }
            }
        }

        private void txtFirstName1_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            FillPendingDoctor();
        }

        private void txtLastName2_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            FillPendingDoctor();
        }
    }
}

