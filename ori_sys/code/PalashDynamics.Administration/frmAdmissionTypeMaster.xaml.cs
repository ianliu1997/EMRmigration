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
using PalashDynamics.ValueObjects.Administration.IPD;
using PalashDynamics.Animations;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using System.IO;
using System.Windows.Resources;
using System.Xml.Linq;
using System.Reflection;
using PalashDynamics.ValueObjects.Master;
using System.ComponentModel;

namespace PalashDynamics.Administration
{
    public partial class frmAdmissionTypeMaster : UserControl
    {
        #region INotifyPropertyChanged Members
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

        #region  Variables
        private SwivelAnimation objAnimation;
        string msgTitle = "";
        string msgText = "";
        long OTTableID;
        private long UnitId;
        public bool isModify = false;
        public PagedSortableCollectionView<clsIPDAdmissionTypeVO> MasterList { get; private set; }
        public bool IsCancel = true;
        public string ModuleName { get; set; }
        public string Action { get; set; }
        clsIPDAdmissionTypeVO getstoreinfo;
        public bool Ammenities = false;
        List<MasterListItem> objList = new List<MasterListItem>();
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
                OnPropertyChanged("PageSize");
            }
        }
        #endregion

        #region Constructor
        public frmAdmissionTypeMaster()
        {
            InitializeComponent();

            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            this.Loaded += new RoutedEventHandler(AdmissionType_Master_Loaded);
            SetCommandButtonState("Load");
            MasterList = new PagedSortableCollectionView<clsIPDAdmissionTypeVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            this.dataGrid2Pager.DataContext = MasterList;
            grdFloor.DataContext = MasterList;
            PageSize = 15;
            FillFloorMasterList();
        }
        #endregion

        #region Set Button State Command

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdAdd.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "New":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdAdd.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;
                case "Save":
                    cmdAdd.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    cmdModify.IsEnabled = false;  //Added by CDS
                    break;
                case "Modify":
                    cmdAdd.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Cancel":
                    cmdAdd.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdAdmissionTypeLinking.Visibility = Visibility.Visible;
                    break;
                case "FrontPanel":
                    cmdAdd.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    break;
                case "View":
                    cmdModify.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdAdd.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    isModify = true;
                    IsCancel = false;
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Loaded Event
        void AdmissionType_Master_Loaded(object sender, RoutedEventArgs e)
        {
            //FillBlock();
        }
        #endregion

        #region on Refresh Event
        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillFloorMasterList();
        }
        #endregion

        #region Validation
        public bool Validation()
        {
            bool result = true;
            if (string.IsNullOrEmpty(txtStoreCode.Text))
            {
                txtStoreCode.SetValidation("Please Enter Code");
                txtStoreCode.RaiseValidationError();
                txtStoreCode.Focus();
            }
            else
                txtStoreCode.ClearValidationError();
            if (string.IsNullOrEmpty(txtStoreName.Text))
            {
                txtStoreName.SetValidation("Please Enter Description");
                txtStoreName.RaiseValidationError();
                txtStoreName.Focus();
                result = false;
            }
            else

                txtStoreName.ClearValidationError();
            return result;
        }

        #endregion

        #region Private Methods

        public void FillFloorMasterList()
        {
            try
            {
                clsIPDGetAdmissionTypeMasterBizActionVO bizActionVO = new clsIPDGetAdmissionTypeMasterBizActionVO();

                bizActionVO.SearchExpression = txtSearch.Text;
                bizActionVO.PagingEnabled = true;
                bizActionVO.MaximumRows = MasterList.PageSize;
                bizActionVO.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;
                getstoreinfo = new clsIPDAdmissionTypeVO();
                bizActionVO.objAdmissionTypeMasterDetails = new List<clsIPDAdmissionTypeVO>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        bizActionVO.objAdmissionTypeMasterDetails = (((clsIPDGetAdmissionTypeMasterBizActionVO)args.Result).objAdmissionTypeMasterDetails);
                        if (bizActionVO.objAdmissionTypeMasterDetails.Count > 0)
                        {
                            MasterList.Clear();
                            MasterList.TotalItemCount = (int)(((clsIPDGetAdmissionTypeMasterBizActionVO)args.Result).TotalRows);
                            foreach (clsIPDAdmissionTypeVO item in bizActionVO.objAdmissionTypeMasterDetails)
                            {
                                MasterList.Add(item);
                            }
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

        public void ClearUI()
        {
            txtStoreCode.Text = String.Empty;
            txtStoreName.Text = String.Empty;
            cmbBlock.SelectedItem = objList.Where(z => z.ID == 0).FirstOrDefault();
        }

        private clsIPDAdmissionTypeVO CreateFormData()
        {
            clsIPDAdmissionTypeVO objFloorVO = new clsIPDAdmissionTypeVO();
            if (isModify == true)
            {
                objFloorVO.ID = OTTableID;
                objFloorVO = ((clsIPDAdmissionTypeVO)grdFloor.SelectedItem);
            }
            else
            {
                objFloorVO.ID = 0;
                objFloorVO.Status = true;
                //objFloorVO.BlockID = ((MasterListItem)cmbBlock.SelectedItem).ID;
                objFloorVO.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }
            return objFloorVO;
        }

        private void SaveFloorMaster()
        {
            try
            {
                clsIPDAddUpdateAdmissionTypeMasterBizActionVO bizActionVO = new clsIPDAddUpdateAdmissionTypeMasterBizActionVO();
                bizActionVO.AdmissionTypeMasterDetails = new clsIPDAdmissionTypeVO();
                if (isModify == true)
                {
                    bizActionVO.AdmissionTypeMasterDetails.ID = ((clsIPDAdmissionTypeVO)grdFloor.SelectedItem).ID;
                }
                bizActionVO.AdmissionTypeMasterDetails.Code = txtStoreCode.Text;
                bizActionVO.AdmissionTypeMasterDetails.Description = txtStoreName.Text;
                //bizActionVO.AdmissionTypeMasterDetails.BlockID = ((MasterListItem)cmbBlock.SelectedItem).ID;
                bizActionVO.AdmissionTypeMasterDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                bizActionVO.AdmissionTypeMasterDetails.Status = true;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (((clsIPDAddUpdateAdmissionTypeMasterBizActionVO)args.Result).SuccessStatus == 1)
                        {
                            msgText = "Record is successfully submitted!";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                FillFloorMasterList();
                                objList.Clear();
                                objAnimation.Invoke(RotationType.Backward);
                                cmdAdmissionTypeLinking.Visibility = Visibility.Visible;
                                SetCommandButtonState("Save");
                            };
                            msgWindow.Show();
                        }
                        else if (((clsIPDAddUpdateAdmissionTypeMasterBizActionVO)args.Result).SuccessStatus == 2)
                        {
                            msgText = "Record cannot be save because CODE already exist!";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindow.Show();
                        }
                        else if (((clsIPDAddUpdateAdmissionTypeMasterBizActionVO)args.Result).SuccessStatus == 3)
                        {
                            msgText = "Record cannot be save because DESCRIPTION already exist!";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindow.Show();
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

        private void ModifyFloorMaster()
        {

        }

        #endregion

        #region Fill Combo boxes

        private void FillBlock()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_BlockMaster;
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
                        cmbBlock.ItemsSource = null;
                        cmbBlock.ItemsSource = objList;
                        if (isModify)
                        {
                            if (grdFloor.SelectedItem != null)
                            {
                                clsIPDAdmissionTypeVO objVO = (clsIPDAdmissionTypeVO)grdFloor.SelectedItem;
                                cmbBlock.SelectedItem = objList.Where(z => z.ID == objVO.BlockID).FirstOrDefault();
                            }
                        }
                        else
                            cmbBlock.SelectedItem = objList[0];
                    }
                    if (this.DataContext != null)
                    {
                        cmbBlock.SelectedValue = ((clsIPDAdmissionTypeVO)this.DataContext).BlockID;
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

        #endregion

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to Save ?";
                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWin.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        SaveFloorMaster();
                    }
                };
                msgWin.Show();
            }
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            ClearUI();
            Validation();
            txtStoreCode.Focus();
            isModify = false;
            FillBlock();
            cmdAdmissionTypeLinking.Visibility = Visibility.Collapsed;
            SetCommandButtonState("New");
            try
            {
                objAnimation.Invoke(RotationType.Forward);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Validation())
                {
                    string msgTitle = "";
                    string msgText = "Are you sure you want to Update ?";

                    MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgW.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            SaveFloorMaster();
                        }
                    };
                    msgW.Show();
                }
            }
            catch (Exception Ex)
            {
                throw;
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetCommandButtonState("Cancel");
                objAnimation.Invoke(RotationType.Backward);
                if (IsCancel == true)
                {
                    ModuleName = "PalashDynamics.Administration";
                    Action = "PalashDynamics.Administration.frmIPDAdmissionConfiguration";
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                    mElement.Text = "Admission Configuration";
                    WebClient c = new WebClient();
                    c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
                    c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
                }
                else
                {
                    IsCancel = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
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

                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdAdmissionTypeLinking_Click(object sender, RoutedEventArgs e)
        {
            if (grdFloor.SelectedItem != null)
            {
                //DoctorServiceLinkChildWindow Win = new DoctorServiceLinkChildWindow();
                AdmissionTypeServiceLinkChildWindow Win = new AdmissionTypeServiceLinkChildWindow();
                Win.AdmissionTypeID = ((clsIPDAdmissionTypeVO)grdFloor.SelectedItem).ID;
                Win.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Select the Admission Type to link with Service.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        private void txtStoreCode_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(((TextBox)e.OriginalSource).Text))
                {
                    ((TextBox)e.OriginalSource).Text = ((TextBox)e.OriginalSource).Text.ToTitleCase();
                    ((TextBox)e.OriginalSource).ClearValidationError();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //FillBlock();
                cmdAdmissionTypeLinking.Visibility = Visibility.Collapsed;
                clsIPDAdmissionTypeVO objVO = (clsIPDAdmissionTypeVO)grdFloor.SelectedItem;
                SetCommandButtonState("View");
                Validation();
                cmdModify.IsEnabled = ((clsIPDAdmissionTypeVO)grdFloor.SelectedItem).Status;

                if (grdFloor.SelectedItem != null)
                {
                    txtStoreCode.Text = objVO.Code;
                    txtStoreName.Text = objVO.Description;
                    objAnimation.Invoke(RotationType.Forward);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MasterList = new PagedSortableCollectionView<clsIPDAdmissionTypeVO>();
                MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
                PageSize = 15;
                this.dataGrid2Pager.DataContext = MasterList;
                this.grdFloor.DataContext = MasterList;
                FillFloorMasterList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void chkStatus_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                clsIPDAddUpdateAdmissionTypeMasterBizActionVO bizActionVO = new clsIPDAddUpdateAdmissionTypeMasterBizActionVO();
                bizActionVO.AdmissionTypeMasterDetails = new clsIPDAdmissionTypeVO();
                bizActionVO.AdmissionTypeMasterDetails.ID = ((clsIPDAdmissionTypeVO)grdFloor.SelectedItem).ID;
                bizActionVO.AdmissionTypeMasterDetails.Status = ((clsIPDAdmissionTypeVO)grdFloor.SelectedItem).Status;
                bizActionVO.IsStatus = true;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (((clsIPDAddUpdateAdmissionTypeMasterBizActionVO)args.Result).SuccessStatus == 1)
                        {
                            msgText = "Status updated successfully!";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                FillFloorMasterList();
                            };
                            msgWindow.Show();
                        }
                    }
                };
                client.ProcessAsync(bizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch
            {

            }
        }

        private void grdFloor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((clsIPDAdmissionTypeVO)grdFloor.SelectedItem != null)
            {
                if (((clsIPDAdmissionTypeVO)grdFloor.SelectedItem).Status == false)
                {
                    cmdAdmissionTypeLinking.Visibility = Visibility.Collapsed;
                }
                else
                {
                    cmdAdmissionTypeLinking.Visibility = Visibility.Visible;
                }
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MasterList = new PagedSortableCollectionView<clsIPDAdmissionTypeVO>();
                MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
                PageSize = 15;
                this.dataGrid2Pager.DataContext = MasterList;
                this.grdFloor.DataContext = MasterList;
                FillFloorMasterList();
            }
        }
    }
}
