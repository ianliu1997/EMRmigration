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
using System.ComponentModel;
using PalashDynamics.Animations;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.EMR.EMR_Field_Values;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using System.Windows.Resources;
using System.Reflection;
using System.Xml.Linq;
using System.IO;

namespace PalashDynamics.Administration
{
    public partial class frmEMRFieldValues : UserControl, INotifyPropertyChanged
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

        #region  Variables
        private SwivelAnimation objAnimation;
        string msgTitle = ""; 
        string msgText = "";     
        public bool isModify = false;
        public PagedSortableCollectionView<clsFieldValuesMasterVO> MasterList { get; private set; }
        public bool IsCancel = true;
        public string ModuleName { get; set; }
        public string Action { get; set; }
        long OTTableID;

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

        public frmEMRFieldValues()
        {
            InitializeComponent();
            this.DataContext = new clsFieldValuesMasterVO();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            this.Loaded += new RoutedEventHandler(Bed_Master_Loaded);
            SetCommandButtonState("Load");
          //  FillUsedForMaster();            
            FillUsedForCombo();
            MasterList = new PagedSortableCollectionView<clsFieldValuesMasterVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            this.dataGrid2Pager.DataContext = MasterList;
            grdBed.DataContext = MasterList;
            PageSize = 15;
            SetupPage();
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
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdAdd.IsEnabled = true;
                    IsCancel = true;
                    //cmdClose.Visibility = Visibility.Visible;
                    //cmdCancel.Visibility = Visibility.Collapsed;
                    break;

                case "New":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdAdd.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    //cmdClose.Visibility = Visibility.Collapsed;
                    //cmdCancel.Visibility = Visibility.Visible;
                    break;

                case "Save":
                    cmdAdd.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    //cmdClose.Visibility = Visibility.Visible;
                    //cmdCancel.Visibility = Visibility.Collapsed;
                    break;

                case "Modify":
                    cmdAdd.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    //cmdClose.Visibility = Visibility.Visible;
                    //cmdCancel.Visibility = Visibility.Collapsed; 
                    break;

                case "Cancel":
                    cmdAdd.IsEnabled = true;
                    //cmdCancel.Visibility = Visibility.Collapsed;
                    //cmdClose.Visibility = Visibility.Visible;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;

                case "FrontPanel":

                    cmdAdd.IsEnabled = true;
                    //cmdCancel.Visibility = Visibility.Collapsed;
                    //cmdClose.Visibility = Visibility.Visible;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;

                    break;
                case "View":
                    cmdModify.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdAdd.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    //cmdClose.Visibility = Visibility.Collapsed;
                    //cmdCancel.Visibility = Visibility.Visible; 
                    break;

                default:
                    break;
            }
        }

        void Bed_Master_Loaded(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// Get called when  front panel grid refreshed
        /// </summary>
        /// <param name="sender"> grid</param>
        /// <param name="e">grid refresh</param>
        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            SetupPage();
        }

        /// <summary>
        /// Fills front panel grid
        /// </summary>
        public void SetupPage()
        {
            try
            {
                clsGetFieldValueMasterBizActionVO bizActionVO = new clsGetFieldValueMasterBizActionVO();
                bizActionVO.SearchExpression = txtSearch.Text;
                bizActionVO.PagingEnabled = true;
                bizActionVO.MaximumRows = MasterList.PageSize;
                bizActionVO.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;
                bizActionVO.objFieldMasterDetails = new List<clsFieldValuesMasterVO>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        bizActionVO.objFieldMasterDetails = (((clsGetFieldValueMasterBizActionVO)args.Result).objFieldMasterDetails);
                        if (bizActionVO.objFieldMasterDetails.Count > 0)
                        {
                            MasterList.Clear();
                            MasterList.TotalItemCount = (int)(((clsGetFieldValueMasterBizActionVO)args.Result).TotalRows);
                            foreach (clsFieldValuesMasterVO item in bizActionVO.objFieldMasterDetails)
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


        ///<summary>
        ///Fills the Used For Combo 
        ///</summary>
        public void FillUsedForCombo()
        {
            try
            {
                clsGetUsedForMasterBizActionVO bizActionVO = new clsGetUsedForMasterBizActionVO();
               // clsGetFieldValueMasterBizActionVO bizActionVO = new clsGetFieldValueMasterBizActionVO();
                
                bizActionVO.objFieldMasterDetails = new List<clsFieldValuesMasterVO>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        bizActionVO.objFieldMasterDetails = (((clsGetUsedForMasterBizActionVO)args.Result).objFieldMasterDetails);
                        if (bizActionVO.objFieldMasterDetails.Count > 0)
                        {
                            List<MasterListItem> objList = new List<MasterListItem>();
                            //PagedSortableCollectionView<clsFieldValuesMasterVO> objList = new PagedSortableCollectionView<clsFieldValuesMasterVO>();
                            //objList.Add(new MasterListItem(0, "-- Select --"));
                            //objList.AddRange(((clsGetUsedForMasterBizActionVO)args.Result).objFieldMasterDetails);

                            foreach (clsFieldValuesMasterVO item in bizActionVO.objFieldMasterDetails)
                            {
                                MasterListItem objMaster = new MasterListItem();
                                objMaster.Description = item.UsedFor;
                                objList.Add(objMaster);
                               // objList.Add(item);
                            }
                            cmbUsedFor.ItemsSource = null;
                            cmbUsedFor.ItemsSource = objList;
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



        /// <summary>
        /// Fills Unit Master combo
        /// </summary>
        void FillUsedForMaster()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_EMRFieldValues;
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
                        cmbUsedFor.ItemsSource = null;
                        cmbUsedFor.ItemsSource = objList;
                    }

                    if (this.DataContext != null)
                    {
                        cmbUsedFor.SelectedValue = ((clsFieldValuesMasterVO)this.DataContext).UsedFor;
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
        /// clears UI
        /// </summary>
        public void ClearUI()
        {
            this.grdBackPanel.DataContext = new clsFieldValuesMasterVO();
            cmbUsedFor.SelectedValue = ((clsFieldValuesMasterVO)this.DataContext).UsedFor;
            FillUsedForCombo();
            
        }

        #region Validation
        public bool Validation()
        {
            if (string.IsNullOrEmpty(txtCode.Text))
            {
                txtCode.SetValidation("Please Enter Code");
                txtCode.RaiseValidationError();
                txtCode.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(txtName.Text))
            {
                txtName.SetValidation("Please Enter Bed No.");
                txtName.RaiseValidationError();
                txtName.Focus();
                return false;
            }
            else
            {
                txtCode.ClearValidationError();
                txtName.ClearValidationError();
                return true;
            }
        }

        #endregion

        /// <summary>
        /// Gets ID of existing OTTable or new OT Table
        /// </summary>
        /// <returns>clsIPDBlockMasterVO object</returns>
        private clsFieldValuesMasterVO CreateFormData()
        {
            clsFieldValuesMasterVO addNewBedVO = new clsFieldValuesMasterVO();
            if (isModify == true)
            {
                addNewBedVO.ID = OTTableID;
                addNewBedVO = ((clsFieldValuesMasterVO)grdBackPanel.DataContext);
              //  addNewBedVO.UsedFor  = ((MasterListItem)cmbUsedFor.SelectedItem).Description;
            }
            else
            {
                addNewBedVO.ID = 0;
                addNewBedVO = (clsFieldValuesMasterVO)this.grdBackPanel.DataContext;
                
                addNewBedVO.Status = true;
            }
            // addNewBlockVO.UnitID = ((MasterListItem)cmbUnit.SelectedItem).ID;
            if ((MasterListItem)cmbUsedFor.SelectedItem != null)
                addNewBedVO.UsedFor = ((MasterListItem)cmbUsedFor.SelectedItem).Description;
            else
                addNewBedVO.UsedFor = cmbUsedFor.Text.Trim();
           
            return addNewBedVO;
        }

        /// <summary>
        /// if cancel button clicked on front panel it goes to config screen
        /// </summary>
        /// <param name="sender">Click button</param>
        /// <param name="e">Click Button Click</param>
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

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
           // this.grdBackPanel.DataContext = new clsFieldValuesMasterVO();
            ClearUI();
            isModify = false;
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

                msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosed);

                msgWin.Show();
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
                    clsAddUpdateFieldValueMasterBizActionVO bizActionVO = new clsAddUpdateFieldValueMasterBizActionVO();
                    clsFieldValuesMasterVO addNewBlockVO = new clsFieldValuesMasterVO();

                    isModify = false;
                    addNewBlockVO = (clsFieldValuesMasterVO)this.DataContext;
                    addNewBlockVO = CreateFormData();

                    //addNewBlockVO.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    //addNewBlockVO.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    //addNewBlockVO.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    //addNewBlockVO.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    //addNewBlockVO.AddedDateTime = System.DateTime.Now;
                    //addNewBlockVO.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    bizActionVO.objFieldMatserDetails = addNewBlockVO;
                    //bizActionVO.objBedMatserDetails.Add(addNewBlockVO);

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddUpdateFieldValueMasterBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                msgText = "Record is successfully submitted!";

                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
                                //After Insertion Back to BackPanel and Setup Page
                                OTTableID = 0;
                                SetupPage();

                                objAnimation.Invoke(RotationType.Backward);
                                SetCommandButtonState("Save");
                            }
                            else if (((clsAddUpdateFieldValueMasterBizActionVO)args.Result).SuccessStatus == 2)
                            {
                                msgText = "Record cannot be save because CODE already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                            else if (((clsAddUpdateFieldValueMasterBizActionVO)args.Result).SuccessStatus == 3)
                            {
                                msgText = "Record cannot be save because DESCRIPTION already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                        }
                    };
                    client.ProcessAsync(bizActionVO, new clsUserVO());
                    client.CloseAsync();
                }
            }
            catch (Exception ex)
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
                    Action = "PalashDynamics.Administration.frmClinicConfiguration";
                    UserControl rootPage = Application.Current.RootVisual as UserControl;

                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                    mElement.Text = "Clinical Configuration";

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

                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                    msgW.Show();
                }
            }
            catch (Exception Ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Modifies OT Table Details
        /// </summary>
        /// <param name="result"> MessageBoxResult</param>
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                clsAddUpdateFieldValueMasterBizActionVO bizactionVO = new clsAddUpdateFieldValueMasterBizActionVO();
                clsFieldValuesMasterVO addNewOtTableVO = new clsFieldValuesMasterVO();
                try
                {
                    isModify = true;
                    addNewOtTableVO = CreateFormData();
                    //addNewOtTableVO.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    //addNewOtTableVO.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    //addNewOtTableVO.UpdatedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    //addNewOtTableVO.UpdatedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    //addNewOtTableVO.UpdatedDateTime = System.DateTime.Now;
                    //addNewOtTableVO.UpdateWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    // bizactionVO.objBedMatserDetails.Add(addNewOtTableVO);
                    bizactionVO.objFieldMatserDetails = addNewOtTableVO;
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddUpdateFieldValueMasterBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                msgText = "Record is successfully submitted!";

                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
                                //After Updation Back to BackPanel and Setup Page
                                OTTableID = 0;
                                SetupPage();
                                objAnimation.Invoke(RotationType.Backward);
                                SetCommandButtonState("Modify");
                            }
                            else if (((clsAddUpdateFieldValueMasterBizActionVO)args.Result).SuccessStatus == 2)
                            {
                                msgText = "Record cannot be save because CODE already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                            else if (((clsAddUpdateFieldValueMasterBizActionVO)args.Result).SuccessStatus == 3)
                            {
                                msgText = "Record cannot be save because DESCRIPTION already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                        }
                    };
                    client.ProcessAsync(bizactionVO, new clsUserVO());
                    client.CloseAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
        
        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MasterList = new PagedSortableCollectionView<clsFieldValuesMasterVO>();
                MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
                PageSize = 15;
                this.dataGrid2Pager.DataContext = MasterList;
                this.grdBed.DataContext = MasterList;
                SetupPage();
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
                SetCommandButtonState("View");
                if (grdBed.SelectedItem != null)
                {
                    OTTableID = ((clsFieldValuesMasterVO)grdBed.SelectedItem).ID;
                    grdBackPanel.DataContext = ((clsFieldValuesMasterVO)grdBed.SelectedItem);
                    //FillUsedForMaster();
                    FillUsedForCombo();
                    cmbUsedFor.SelectedItem = ((clsFieldValuesMasterVO)grdBed.SelectedItem).UsedFor;                   
                    objAnimation.Invoke(RotationType.Forward);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void chkStatus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clsUpdateStatusFieldValueMasterBizActionVO objStatus = new clsUpdateStatusFieldValueMasterBizActionVO();
                objStatus.FieldStatus = new clsFieldValuesMasterVO();
                // objStatus.RoomStatus = (clsIPDRoomMasterVO)dgAmmenityList.SelectedItem;
                objStatus.FieldStatus = (clsFieldValuesMasterVO)grdBed.SelectedItem;
                objStatus.FieldStatus.Status = ((clsFieldValuesMasterVO)grdBed.SelectedItem).Status;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (objStatus.FieldStatus.Status == false)
                        {
                            msgText = "Field Status Deactivated Successfully.";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                        }
                        else
                        {
                            msgText = "Field Status Activated Successfully.";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                        }
                    }
                    SetCommandButtonState("View");
                };
                client.ProcessAsync(objStatus, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }        
        }

        private void txtCode_LostFocus(object sender, RoutedEventArgs e)
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

    }
}
