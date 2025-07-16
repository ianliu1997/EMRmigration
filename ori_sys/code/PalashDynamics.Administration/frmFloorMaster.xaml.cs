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
    public partial class frmFloorMaster : UserControl
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
        public PagedSortableCollectionView<clsIPDFloorMasterVO> MasterList { get; private set; }
        public bool IsCancel = true;
        public string ModuleName { get; set; }
        public string Action { get; set; }
        clsIPDRoomMasterVO getstoreinfo;
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
        public frmFloorMaster()
        {
            InitializeComponent();

            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            this.Loaded += new RoutedEventHandler(Room_Master_Loaded);
            SetCommandButtonState("Load");
            MasterList = new PagedSortableCollectionView<clsIPDFloorMasterVO>();
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
        void Room_Master_Loaded(object sender, RoutedEventArgs e)
        {
            FillBlock();
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
            if (string.IsNullOrEmpty(txtStoreCode.Text))
            {
                txtStoreCode.SetValidation("Please Enter Code");
                txtStoreCode.RaiseValidationError();
                txtStoreCode.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(txtStoreName.Text))
            {
                txtStoreName.SetValidation("Please Enter Bed No.");
                txtStoreName.RaiseValidationError();
                txtStoreName.Focus();
                return false;
            }
            else
            {
                txtStoreCode.ClearValidationError();
                txtStoreName.ClearValidationError();
                return true;
            }
        }

        #endregion

        #region Private Methods
        public void FillFloorMasterList()
        {
            try
            {
                clsIPDGetFloorMasterBizActionVO bizActionVO = new clsIPDGetFloorMasterBizActionVO();
                bizActionVO.SearchExpression = txtSearch.Text;
                bizActionVO.PagingEnabled = true;
                bizActionVO.MaximumRows = MasterList.PageSize;
                bizActionVO.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;
                getstoreinfo = new clsIPDRoomMasterVO();
                bizActionVO.objFloorMasterDetails = new List<clsIPDFloorMasterVO>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        bizActionVO.objFloorMasterDetails = (((clsIPDGetFloorMasterBizActionVO)args.Result).objFloorMasterDetails);
                        if (bizActionVO.objFloorMasterDetails.Count > 0)
                        {
                            MasterList.Clear();
                            MasterList.TotalItemCount = (int)(((clsIPDGetFloorMasterBizActionVO)args.Result).TotalRows);
                            foreach (clsIPDFloorMasterVO item in bizActionVO.objFloorMasterDetails)
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

        private clsIPDFloorMasterVO CreateFormData()
        {
            clsIPDFloorMasterVO objFloorVO = new clsIPDFloorMasterVO();
            if (isModify == true)
            {
                objFloorVO.ID = OTTableID;
                objFloorVO = ((clsIPDFloorMasterVO)grdFloor.SelectedItem);
            }
            else
            {
                objFloorVO.ID = 0;
                objFloorVO.Status = true;
                objFloorVO.BlockID = ((MasterListItem)cmbBlock.SelectedItem).ID;
                objFloorVO.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }
            return objFloorVO;
        }

        private void SaveFloorMaster()
        {
            try
            {
                clsIPDAddUpdateFloorMasterBizActionVO bizActionVO = new clsIPDAddUpdateFloorMasterBizActionVO();
                bizActionVO.FlooMasterDetails = new clsIPDFloorMasterVO();
                if (isModify == true)
                {
                    bizActionVO.FlooMasterDetails.ID = ((clsIPDFloorMasterVO)grdFloor.SelectedItem).ID;
                }
                bizActionVO.FlooMasterDetails.Code = txtStoreCode.Text;
                bizActionVO.FlooMasterDetails.Description = txtStoreName.Text;
                bizActionVO.FlooMasterDetails.BlockID = ((MasterListItem)cmbBlock.SelectedItem).ID;
                bizActionVO.FlooMasterDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                bizActionVO.FlooMasterDetails.Status = true;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (((clsIPDAddUpdateFloorMasterBizActionVO)args.Result).SuccessStatus == 1)
                        {
                            msgText = "Record is successfully submitted!";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                FillFloorMasterList();
                                objList.Clear();
                                objAnimation.Invoke(RotationType.Backward);
                                SetCommandButtonState("Save");
                            };
                            msgWindow.Show();
                        }
                        else if (((clsIPDAddUpdateFloorMasterBizActionVO)args.Result).SuccessStatus == 2)
                        {
                            msgText = "Record cannot be save because CODE already exist!";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindow.Show();
                        }
                        else if (((clsIPDAddUpdateFloorMasterBizActionVO)args.Result).SuccessStatus == 3)
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
                                clsIPDFloorMasterVO objVO = (clsIPDFloorMasterVO)grdFloor.SelectedItem;
                                cmbBlock.SelectedItem = objList.Where(z => z.ID == objVO.BlockID).FirstOrDefault();
                            }
                        }
                        else
                            cmbBlock.SelectedItem = objList[0];
                    }
                    if (this.DataContext != null)
                    {
                        cmbBlock.SelectedValue = ((clsIPDFloorMasterVO)this.DataContext).BlockID;
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

        #region Button Click Events
        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            ClearUI();
            isModify = false;
            FillBlock();
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
                        if(res == MessageBoxResult.Yes)
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

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MasterList = new PagedSortableCollectionView<clsIPDFloorMasterVO>();
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

        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FillBlock();
                clsIPDFloorMasterVO objVO = (clsIPDFloorMasterVO)grdFloor.SelectedItem;
                SetCommandButtonState("View");
                cmdModify.IsEnabled = ((clsIPDFloorMasterVO)grdFloor.SelectedItem).Status;

                if (grdFloor.SelectedItem != null)
                {
                    txtStoreCode.Text = objVO.Code;
                    txtStoreName.Text = objVO.Description;
                    cmbBlock.SelectedItem =  objList.Where(z => z.ID == objVO.BlockID).FirstOrDefault();
                    cmbBlock.UpdateLayout();
                    objAnimation.Invoke(RotationType.Forward);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (grdFloor.SelectedItem != null)
            {
                try
                {  
                    clsIPDAddUpdateFloorMasterBizActionVO bizActionVO = new clsIPDAddUpdateFloorMasterBizActionVO();
                    bizActionVO.FlooMasterDetails = new clsIPDFloorMasterVO();
                                        
                    bizActionVO.FlooMasterDetails.ID = ((clsIPDFloorMasterVO)grdFloor.SelectedItem).ID;
                    bizActionVO.FlooMasterDetails.Code = txtStoreCode.Text;
                    bizActionVO.FlooMasterDetails.Description = txtStoreName.Text;
                    bizActionVO.FlooMasterDetails.BlockID = ((MasterListItem)cmbBlock.SelectedItem).ID;
                    bizActionVO.FlooMasterDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;                    
                    bizActionVO.FlooMasterDetails.Status = Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked);

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsIPDAddUpdateFloorMasterBizActionVO)args.Result).SuccessStatus == 1)
                            {                               
                                UnitId = 0;
                                FillFloorMasterList();
                                msgText = "Status Updated Successfully.";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
                                objAnimation.Invoke(RotationType.Backward);
                                SetCommandButtonState("Modify");
                            }
                        }
                    };
                    client.ProcessAsync(bizActionVO, new clsUserVO());
                    client.CloseAsync();
                }
                catch (Exception ex)
                {

                }
            }
        }

        #endregion

        #region Lost Focus Event
        private void txtStoreCode_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(((TextBox)e.OriginalSource).Text))
                {
                    ((TextBox)e.OriginalSource).ClearValidationError();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

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

        private void chkStatus_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                clsIPDAddUpdateFloorMasterBizActionVO bizActionVO = new clsIPDAddUpdateFloorMasterBizActionVO();
                bizActionVO.FlooMasterDetails = new clsIPDFloorMasterVO();
                bizActionVO.FlooMasterDetails.ID = ((clsIPDFloorMasterVO)grdFloor.SelectedItem).ID;
                bizActionVO.FlooMasterDetails.Status = ((clsIPDFloorMasterVO)grdFloor.SelectedItem).Status;
                bizActionVO.IsStatus = true;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (((clsIPDAddUpdateFloorMasterBizActionVO)args.Result).SuccessStatus == 1)
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


                    //if (args.Error == null && args.Result != null)
                    //{
                    //    if (bizActionVO.FlooMasterDetails.Status == false)
                    //    {
                    //        msgText = "Floor Status Deactivated Successfully.";
                    //        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    //        msgWindow.Show();
                    //    }
                    //    else
                    //    {
                    //        msgText = "Floor Status Activated Successfully.";
                    //        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    //        msgWindow.Show();
                    //    }

                    //}



                };
                client.ProcessAsync(bizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch
            {

            }
        }
    }
}