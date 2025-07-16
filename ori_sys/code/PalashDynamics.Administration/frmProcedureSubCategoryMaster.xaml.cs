using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using CIMS;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Administration.OTConfiguration;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Master;
using System.Reflection;
using System.IO;
using System.Xml.Linq;
using System.Windows.Resources;

namespace PalashDynamics.Administration
{
    public partial class frmProcedureSubCategoryMaster : UserControl, INotifyPropertyChanged
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

        #region Properties
        public PagedSortableCollectionView<clsProcedureSubCategoryVO> MasterList { get; private set; }
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

        #region Variable Declaration
        private SwivelAnimation objAnimation;
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        bool IsCancel = true;
        string msgText = "";
        string msgTitle = "PALASH";
        public string ModuleName { get; set; }
        public string Action { get; set; }
        #endregion

        #region Constructor
        public frmProcedureSubCategoryMaster()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(ProcedureSubCategoryListLayout, NewProcedureSubCategoryLayoutRoot, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            MasterList = new PagedSortableCollectionView<clsProcedureSubCategoryVO>();
            PageSize = 15;
            this.dpProcedureSubCategory.DataContext = MasterList;

            this.grdProcedureSubCategory.DataContext = MasterList;

            FillCategory();
        }

        #endregion

        #region FillCombo
        private void FillCategory()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_ProcedureCategoryMaster;
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
                        CmbCategory.ItemsSource = null;
                        CmbCategory.ItemsSource = objList;
                        CmbCategory.SelectedItem = objList[0];
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser); //new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CmbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.DataContext != null && (MasterListItem)CmbCategory.SelectedItem != null)
            {
                if (((MasterListItem)CmbCategory.SelectedItem).ID > 0)
                    ((clsProcedureSubCategoryVO)this.DataContext).CategoryId = ((MasterListItem)CmbCategory.SelectedItem).ID;
            }
        }

        #endregion

        #region Loaded
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetCommandButtonSubCategory("Load");
            txtSearch.Focus();
            SetupPage();
        }
        #endregion

        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion

        #region Get Procedure SubCategory Master
        private void SetupPage()
        {
            clsGetProcedureSubCategoryListBizActionVO BizAction = new clsGetProcedureSubCategoryListBizActionVO();
            BizAction.SubCategoryList = new List<clsProcedureSubCategoryVO>();

            BizAction.SearchExpression = txtSearch.Text;
            BizAction.PagingEnabled = true;
            BizAction.MaximumRows = MasterList.PageSize;
            BizAction.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;
            try
            {
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        BizAction.SubCategoryList = (((clsGetProcedureSubCategoryListBizActionVO)args.Result).SubCategoryList);

                        MasterList.Clear();
                        MasterList.TotalItemCount = (int)(((clsGetProcedureSubCategoryListBizActionVO)args.Result).TotalRows);
                        foreach (clsProcedureSubCategoryVO item in BizAction.SubCategoryList)
                        {
                            MasterList.Add(item);
                        }
                    }
                };
                client.ProcessAsync(BizAction, User);//new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
            }
        }

        # endregion

        #region Set Command Button State New/Save/Modify/Print
        private void SetCommandButtonSubCategory(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdNew.IsEnabled = true;
                    break;

                case "New":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;
                case "Save":
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    //IsCancel = true;
                    break;
                case "Modify":
                    cmdNew.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    //IsCancel = true;
                    break;
                case "Cancel":
                    cmdNew.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;
                case "View":
                    cmdModify.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    //IsCancel = false;
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Validation
        private bool Validation()
        {
            if (string.IsNullOrEmpty(txtCode.Text) || string.IsNullOrEmpty(txtCode.Text.Trim()))
            {

                //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                //{
                //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("CodeValidation_Msg");
                //}
                //else
                //{
                    msgText = "Please enter code.";
                //}
                txtCode.SetValidation(msgText);
                txtCode.RaiseValidationError();
                txtCode.Focus();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {

                //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                //{
                //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("DescValidation_Msg");
                //}
                //else
                //{
                    msgText = "Please enter description.";
                //}
                txtDescription.SetValidation(msgText);
                txtDescription.RaiseValidationError();
                txtDescription.Focus();
                return false;
            }
            //else if (!string.IsNullOrEmpty(txtDescription.Text))
            //{
                else if (((MasterListItem)CmbCategory.SelectedItem) == null || ((MasterListItem)CmbCategory.SelectedItem).ID == 0)
                {
                    //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                    //{
                    //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("CategoryValidation_Msg");
                    //}
                    //else
                    //{
                    msgText = "Please select category.";
                    //}
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    CmbCategory.Focus();
                    return false;
                }
               
            //}
            else
            {
                txtCode.ClearValidationError();
                txtDescription.ClearValidationError();
                return true;
            }

        }
        #endregion

        #region Save Procedure SubCategory Master
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                string msgTitle = "Palash";
                //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                //{
                //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("SaveVerification_Msg");
                //}
                //else
                //{
                    msgText = "Are you sure you want to Save ?";
                //}
                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                msgW.Show();
            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if (CheckDuplicasy())
                {
                    SaveProcedureSubCategory();
                    SetCommandButtonSubCategory("Save");
                }
            }
        }

        private void SaveProcedureSubCategory()
        {
            clsAddUpdateProcedureSubCategoryBizActionVO BizAction = new clsAddUpdateProcedureSubCategoryBizActionVO();

            clsProcedureSubCategoryVO addComptVO = new clsProcedureSubCategoryVO();
            BizAction.SubCategoryList = new List<clsProcedureSubCategoryVO>();
            addComptVO.ID = 0;
            addComptVO.Code = txtCode.Text.Trim();
            addComptVO.Description = txtDescription.Text.Trim();
            addComptVO.CategoryId = ((MasterListItem)CmbCategory.SelectedItem).ID;
            addComptVO.Status = true;
            addComptVO.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            addComptVO.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            addComptVO.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            addComptVO.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
            addComptVO.AddedDateTime = System.DateTime.Now;
            addComptVO.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;

            BizAction.SubCategoryList.Add(addComptVO);

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    SetupPage();
                    ClearUI();
                    objAnimation.Invoke(RotationType.Backward);
                    //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                    //{
                    //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("RecordSaved_Msg");
                    //}
                    //else
                    //{
                        msgText = "Record saved successfully.";
                    //}
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    SetCommandButtonSubCategory("Save");
                }
                else
                {
                    //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                    //{
                    //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ErrorWhileSaveModify_Msg");
                    //}
                    //else
                    //{
                        msgText = "Error occured while processing.";
                    //}
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        #endregion

        #region Modify Procedure SubCategory Master
        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                string msgTitle = "";
                //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                //{
                //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("UpdateVerification_Msg");
                //}
                //else
                //{
                    msgText = "Are you sure you want to Update ?";
                //}
                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosedSubCategory);

                msgW.Show();
            }
        }

        void msgW_OnMessageBoxClosedSubCategory(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    if (CheckModifyDuplicasy())
                    {
                        ModifySubCategory();
                        SetCommandButtonSubCategory("Modify");
                    }
                }

                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        private void ModifySubCategory()
        {
            clsAddUpdateProcedureSubCategoryBizActionVO BizAction = new clsAddUpdateProcedureSubCategoryBizActionVO();
            clsProcedureSubCategoryVO UpdtComptMast = new clsProcedureSubCategoryVO();
            BizAction.SubCategoryList = new List<clsProcedureSubCategoryVO>();
            try
            {
                UpdtComptMast.ID = ((clsProcedureSubCategoryVO)grdProcedureSubCategory.SelectedItem).ID;
                UpdtComptMast.Code = txtCode.Text.Trim();
                UpdtComptMast.Description = txtDescription.Text.Trim();
                UpdtComptMast.CategoryId = ((MasterListItem)CmbCategory.SelectedItem).ID;
                UpdtComptMast.Status = ((clsProcedureSubCategoryVO)grdProcedureSubCategory.SelectedItem).Status;
                UpdtComptMast.UnitId = ((clsProcedureSubCategoryVO)grdProcedureSubCategory.SelectedItem).UnitId;
                UpdtComptMast.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                UpdtComptMast.UpdatedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                UpdtComptMast.UpdatedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                UpdtComptMast.UpdatedDateTime = System.DateTime.Now;
                UpdtComptMast.UpdateWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;

                BizAction.SubCategoryList.Add(UpdtComptMast);

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        SetupPage();
                        ClearUI();
                        objAnimation.Invoke(RotationType.Backward);
                        //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                        //{
                        //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("RecordModify_Msg");
                        //}
                        //else
                        //{
                            msgText = "Record updated successfully.";
                        //}
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        SetCommandButtonSubCategory("Modify");
                    }
                    else
                    {
                        //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                        //{
                        //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ErrorWhileSaveModify_Msg");
                        //}
                        //else
                        //{
                            msgText = "Error occured while processing.";
                        //}
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
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
        #endregion

        private bool CheckDuplicasy()
        {
            bool result = true;
            if (grdProcedureSubCategory.ItemsSource != null)
            {
                clsProcedureSubCategoryVO Item, Item1;
                Item = ((PagedSortableCollectionView<clsProcedureSubCategoryVO>)grdProcedureSubCategory.ItemsSource).FirstOrDefault(p => p.Code.ToUpper().Equals(txtCode.Text.ToUpper()));
                Item1 = ((PagedSortableCollectionView<clsProcedureSubCategoryVO>)grdProcedureSubCategory.ItemsSource).FirstOrDefault(p => p.Description.ToUpper().Equals(txtDescription.Text.ToUpper()));
                if (Item != null)
                {
                    //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                    //{
                    //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("CodeDuplicate_Msg");
                    //}
                    //else
                    //{
                        msgText = "Record cannot be save because CODE already exist!";
                    //}
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                    result = false;
                }
                else if (Item1 != null)
                {
                    //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                    //{
                    //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("DescDuplicate_Msg");
                    //}
                    //else
                    //{
                        msgText = "Record cannot be save because DESCRIPTION already exist!";
                    //}
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    result = false;
                }
            }
            return result;
        }

        private bool CheckModifyDuplicasy()
        {
            bool result = true;
            if (grdProcedureSubCategory.ItemsSource != null)
            {
                if (grdProcedureSubCategory.SelectedItem != null)
                {
                    clsProcedureSubCategoryVO Item, Item1;
                    Item = ((PagedSortableCollectionView<clsProcedureSubCategoryVO>)grdProcedureSubCategory.ItemsSource).Where(S => S.ID != ((clsProcedureSubCategoryVO)grdProcedureSubCategory.SelectedItem).ID).ToList().FirstOrDefault(p => p.Code.ToUpper().Equals(txtCode.Text.ToUpper()));
                    Item1 = ((PagedSortableCollectionView<clsProcedureSubCategoryVO>)grdProcedureSubCategory.ItemsSource).Where(S => S.ID != ((clsProcedureSubCategoryVO)grdProcedureSubCategory.SelectedItem).ID).ToList().FirstOrDefault(p => p.Description.ToUpper().Equals(txtDescription.Text.ToUpper()));
                    if (Item != null)
                    {
                        //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                        //{
                        //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("CodeDuplicate_Msg");
                        //}
                        //else
                        //{
                            msgText = "Record cannot be save because CODE already exist!";
                        //}
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        result = false;
                    }
                    else if (Item1 != null)
                    {
                        //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                        //{
                        //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("DescDuplicate_Msg");
                        //}
                        //else
                        //{
                            msgText = "Record cannot be save because DESCRIPTION already exist!";
                        //}
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        result = false;
                    }
                }
            }
            return result;
        }
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonSubCategory("Cancel");
            this.DataContext = null;
            SetupPage();
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = "";

            objAnimation.Invoke(RotationType.Backward);
            if (IsCancel == true)
            {
                mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "OT Configuration";
                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmOTConfiguration") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
            }
            else
            {
                IsCancel = true;
            }
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            MasterList = new PagedSortableCollectionView<clsProcedureSubCategoryVO>();
            PageSize = 15;
            this.dpProcedureSubCategory.DataContext = MasterList;
            this.grdProcedureSubCategory.DataContext = MasterList;

            SetupPage();
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


        #region View
        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetCommandButtonSubCategory("View");
                cmdModify.IsEnabled = ((clsProcedureSubCategoryVO)grdProcedureSubCategory.SelectedItem).Status;
                IsCancel = false;
                if (grdProcedureSubCategory.SelectedItem != null)
                {
                    NewProcedureSubCategoryLayoutRoot.DataContext = ((clsProcedureSubCategoryVO)grdProcedureSubCategory.SelectedItem);
                    CmbCategory.SelectedValue = ((clsProcedureSubCategoryVO)grdProcedureSubCategory.SelectedItem).CategoryId;
                    objAnimation.Invoke(RotationType.Forward);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonSubCategory("New");
            this.DataContext = new clsProcedureSubCategoryVO();
            ClearUI();
            IsCancel = false;
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + "New Procedure SubCategory Details";
            objAnimation.Invoke(RotationType.Forward);
        }

        #region Status Update
        private void StatusCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (grdProcedureSubCategory.SelectedItem != null)
            {
                clsUpdateStatusProcedureSubCategoryBizActionVO BizAction = new clsUpdateStatusProcedureSubCategoryBizActionVO();

                BizAction.SubCategoryDetails = new clsProcedureSubCategoryVO();
                BizAction.SubCategoryDetails = ((clsProcedureSubCategoryVO)grdProcedureSubCategory.SelectedItem);

                BizAction.SubCategoryDetails.Status = true;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        SetCommandButtonSubCategory("Save");

                        objAnimation.Invoke(RotationType.Backward);
                        if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                        {
                            msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("lblStatusUpdatedSucessfully");
                        }
                        else
                        {
                            msgText = "Status Updated Successfully.";
                        }
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        SetupPage();
                    }
                    else
                    {
                        SetCommandButtonSubCategory("New");
                        if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                        {
                            msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ErrorWhileSaveModify_Msg");
                        }
                        else
                        {
                            msgText = "Error occured while processing.";
                        }
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }

        }

        private void StatusCheckBox_UnChecked(object sender, RoutedEventArgs e)
        {
            if (grdProcedureSubCategory.SelectedItem != null)
            {
                clsUpdateStatusProcedureSubCategoryBizActionVO BizAction = new clsUpdateStatusProcedureSubCategoryBizActionVO();

                BizAction.SubCategoryDetails = new clsProcedureSubCategoryVO();
                BizAction.SubCategoryDetails = ((clsProcedureSubCategoryVO)grdProcedureSubCategory.SelectedItem);

                BizAction.SubCategoryDetails.Status = false;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        SetCommandButtonSubCategory("Save");
                        objAnimation.Invoke(RotationType.Backward);
                        if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                        {
                            msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("lblStatusUpdatedSucessfully");
                        }
                        else
                        {
                            msgText = "Status Updated Successfully.";
                        }
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        SetupPage();
                    }
                    else
                    {
                        SetCommandButtonSubCategory("New");
                        if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                        {
                            msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ErrorWhileSaveModify_Msg");
                        }
                        else
                        {
                            msgText = "Error occured while processing.";
                        }
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }

        }

        #endregion

        # region Clear function
        public void ClearUI()
        {
            txtCode.Text = string.Empty;
            txtDescription.Text = string.Empty;
            CmbCategory.SelectedValue = ((List<MasterListItem>)CmbCategory.ItemsSource)[0].ID;
        }
        #endregion

        private void txtCode_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)e.OriginalSource).Text))
            {
                if (!((TextBox)e.OriginalSource).Text.IsItSpecialChar())
                {
                    ((TextBox)e.OriginalSource).Text = string.Empty;
                    ((TextBox)e.OriginalSource).BorderBrush = new SolidColorBrush(Colors.Red);
                    if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                    {
                        msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("SpecialCharValidation_Msg");
                    }
                    else
                    {
                        msgText = "Special characters are not allowed.";
                    }
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);


                }
                else
                {
                    ((TextBox)e.OriginalSource).BorderBrush = new SolidColorBrush(Colors.Gray);
                }
            }
        }

        private void txtCode_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = CIMS.Comman.HandleAlphnumericAndSpecialChar(sender, e, false);
        }

        private void txtDescription_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)e.OriginalSource).Text))
            {
                if (!((TextBox)e.OriginalSource).Text.AllowSpecialChar())
                {
                    ((TextBox)e.OriginalSource).Text = string.Empty;
                    ((TextBox)e.OriginalSource).BorderBrush = new SolidColorBrush(Colors.Red);
                    if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                    {
                        msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("SpecCharNotValidation_Msg");
                    }
                    else
                    {
                        msgText = "Only & ,.,- and space special characters are allowed.";
                    }
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                }
                else
                {
                    ((TextBox)e.OriginalSource).BorderBrush = new SolidColorBrush(Colors.Gray);
                    txtDescription.Text = txtDescription.Text.ToTitleCase();
                }
            }
        }

        private void txtDescription_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = CIMS.Comman.HandleAlphnumericAndSpecialChar(sender, e, true);
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                dpProcedureSubCategory.PageIndex = 0;
                SetupPage();
            }
        }
    }
}
