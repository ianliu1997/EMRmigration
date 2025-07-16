using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using CIMS;
using PalashDynamics.Collections;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Administration.OTConfiguration;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Reflection;

namespace PalashDynamics.Administration
{
    public partial class frmProcedureCheckList : UserControl, INotifyPropertyChanged
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
        public PagedSortableCollectionView<clsProcedureCheckListVO> MasterList { get; private set; }
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

        #endregion

        #region Constructor

        public frmProcedureCheckList()
        {
            InitializeComponent();

            objAnimation = new SwivelAnimation(ProcedureCheckListLayout, NewProcedureCheckListLayoutRoot, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            MasterList = new PagedSortableCollectionView<clsProcedureCheckListVO>();
            PageSize = 15;
            this.dpProcedureCheckList.DataContext = MasterList;
            this.grdProcedureCheckList.DataContext = MasterList;
            FillCategory();
            // FillSubCategory();
        }
        #endregion

        #region Loaded
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetCommandButtonCheckList("Load");
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

        #region Fill Combo

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

        //private void FillSubCategory()
        //{
        //    try
        //    {
        //        clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //        BizAction.MasterTable = MasterTableNameList.M_ProcedureSubCategoryMaster;
        //        BizAction.Parent = new KeyValue();
        //        BizAction.Parent.Key = "1";
        //        BizAction.Parent.Value = "Status";
        //        BizAction.MasterList = new List<MasterListItem>();
        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //        client.ProcessCompleted += (s, args) =>
        //        {
        //            if (args.Error == null && args.Result != null)
        //            {
        //                List<MasterListItem> objList = new List<MasterListItem>();

        //                objList.Add(new MasterListItem(0, "-- Select --"));
        //                objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

        //                CmbSubCategory.ItemsSource = null;
        //                CmbSubCategory.ItemsSource = objList;
        //                CmbSubCategory.SelectedItem = objList[0];
        //            }
        //        };
        //        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser); //new clsUserVO());
        //        client.CloseAsync();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        private void FillSubCategory(long SubCatgyID)
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_ProcedureSubCategoryMaster;
                BizAction.Parent = new KeyValue { Value = "ProcedureCategoryID", Key = SubCatgyID.ToString() };
                BizAction.MasterList = new List<MasterListItem>();
                //BizAction.Parent = new KeyValue();
                //BizAction.Parent.Key = "1";
                //BizAction.Parent.Value = "Status";
                //BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                        CmbSubCategory.ItemsSource = null;
                        CmbSubCategory.ItemsSource = objList;
                        CmbSubCategory.SelectedItem = objList[0];

                        //if (this.DataContext != null)
                        //{
                        //    CmbSubCategory.SelectedValue = ((clsProcedureMasterVO)this.DataContext).SubCategoryID;
                        //}
                        if (blView)
                        {
                            CmbSubCategory.SelectedValue = ((clsProcedureCheckListVO)grdProcedureCheckList.SelectedItem).SubCategoryId;
                            blView = false;
                        }
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

        #endregion

        #region SelectionChanged
        private void CmbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.DataContext != null && (MasterListItem)CmbCategory.SelectedItem != null)
            {
                if (((MasterListItem)CmbCategory.SelectedItem).ID > 0)
                    ((clsProcedureCheckListVO)this.DataContext).CategoryId = ((MasterListItem)CmbCategory.SelectedItem).ID;
            }


            try
            {
                if (CmbCategory.SelectedItem != null && ((MasterListItem)CmbCategory.SelectedItem).ID != 0)
                {
                    FillSubCategory(((MasterListItem)CmbCategory.SelectedItem).ID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private void CmbSubCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.DataContext != null && (MasterListItem)CmbSubCategory.SelectedItem != null)
            {
                if (((MasterListItem)CmbSubCategory.SelectedItem).ID > 0)
                    ((clsProcedureCheckListVO)this.DataContext).SubCategoryId = ((MasterListItem)CmbSubCategory.SelectedItem).ID;

            }
        }
        #endregion

        #region Set Command Button State New/Save/Modify/Print

        private void SetCommandButtonCheckList(String strFormMode)
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
                    break;
                case "Modify":
                    cmdNew.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
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
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region ClickEvent
        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonCheckList("New");
            this.DataContext = new clsProcedureCheckListVO();
            ClearUI();
            IsCancel = false;
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock melement = (TextBlock)rootPage.FindName("SampleSubHeader");
            melement.Text = " : " + "New Procedure CheckList Details";
            objAnimation.Invoke(RotationType.Forward);
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonCheckList("Cancel");
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
            MasterList = new PagedSortableCollectionView<clsProcedureCheckListVO>();
            PageSize = 15;
            this.dpProcedureCheckList.DataContext = MasterList;
            this.grdProcedureCheckList.DataContext = MasterList;
            SetupPage();
        }
        #endregion

        #region Save Procedure Check List Master
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
                SaveProcedureCheckList();
            }
        }

        private void SaveProcedureCheckList()
        {
            clsAddUpdateProcedureCheckListBizActionVO BizAction = new clsAddUpdateProcedureCheckListBizActionVO();

            clsProcedureCheckListVO addComptVO = new clsProcedureCheckListVO();
            BizAction.CheckList = new List<clsProcedureCheckListVO>();
            addComptVO.ID = 0;
            addComptVO.Code = txtCode.Text.Trim();
            addComptVO.Description = txtDescription.Text.Trim();
            addComptVO.CategoryId = ((MasterListItem)CmbCategory.SelectedItem).ID;
            addComptVO.SubCategoryId = ((MasterListItem)CmbCategory.SelectedItem).ID;
            addComptVO.Status = true;


            BizAction.CheckList.Add(addComptVO);

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
                    SetCommandButtonCheckList("Save");
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

        #region Validation
        private bool Validation()
        {
            if (string.IsNullOrEmpty(txtCode.Text))
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
                    //if (((MasterListItem)CmbCategory.SelectedItem).ID == 0)
                    //{
                    //msgText = "Please select Category.";
                    //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                    //{
                    //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("CategoryValidation_Msg");
                    //}
                    //else
                    //{
                    msgText = "Please select category.";
                    //}
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                    CmbCategory.Focus();
                    return false;
                }
                
                //}
            //}

            //else if (!string.IsNullOrEmpty(txtDescription.Text))
            //{
            else if (((MasterListItem)CmbSubCategory.SelectedItem) == null || ((MasterListItem)CmbSubCategory.SelectedItem).ID == 0)
                {
                    //if (((MasterListItem)CmbSubCategory.SelectedItem).ID == 0)
                    //{
                    // msgText = "Please select SubCategory.";
                    //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                    //{
                    //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("SubCategoryValidation_Msg");
                    //}
                    //else
                    //{
                    msgText = "Please select sub category.";
                    //}
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                    CmbSubCategory.Focus();
                    return false;
                }
                
                //}
            //}
            else
            {
                txtCode.ClearValidationError();
                txtDescription.ClearValidationError();
                return true;
            }


        }
        #endregion

        #region View  Procedure Check List Master

        bool blView = false;
        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetCommandButtonCheckList("View");
                blView = true;
                cmdModify.IsEnabled = ((clsProcedureCheckListVO)grdProcedureCheckList.SelectedItem).Status;
                IsCancel = false;
                if (grdProcedureCheckList.SelectedItem != null)
                {
                    this.DataContext = ((clsProcedureCheckListVO)grdProcedureCheckList.SelectedItem);
                    txtCode.Text = ((clsProcedureCheckListVO)grdProcedureCheckList.SelectedItem).Code;
                    txtDescription.Text = ((clsProcedureCheckListVO)grdProcedureCheckList.SelectedItem).Description;
                    CmbCategory.SelectedValue = ((clsProcedureCheckListVO)grdProcedureCheckList.SelectedItem).CategoryId;
                    CmbSubCategory.SelectedValue = ((clsProcedureCheckListVO)grdProcedureCheckList.SelectedItem).SubCategoryId;

                    objAnimation.Invoke(RotationType.Forward);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Get Procedure Check List master

        private void SetupPage()
        {
            clsGetProcedureCheckListBizActionVO BizAction = new clsGetProcedureCheckListBizActionVO();
            BizAction.CheckList = new List<clsProcedureCheckListVO>();

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
                        BizAction.CheckList = (((clsGetProcedureCheckListBizActionVO)args.Result).CheckList);

                        MasterList.Clear();
                        MasterList.TotalItemCount = (int)(((clsGetProcedureCheckListBizActionVO)args.Result).TotalRows);
                        foreach (clsProcedureCheckListVO item in BizAction.CheckList)
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

        #endregion

        #region Modify Procedure Check List
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
                    ModifyCheckList();
                    SetCommandButtonCheckList("Modify");
                }

                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        private void ModifyCheckList()
        {
            clsAddUpdateProcedureCheckListBizActionVO BizAction = new clsAddUpdateProcedureCheckListBizActionVO();
            clsProcedureCheckListVO UpdtComptMast = new clsProcedureCheckListVO();
            BizAction.CheckList = new List<clsProcedureCheckListVO>();
            try
            {
                UpdtComptMast.ID = ((clsProcedureCheckListVO)grdProcedureCheckList.SelectedItem).ID;
                UpdtComptMast.Code = txtCode.Text;
                UpdtComptMast.Description = txtDescription.Text;
                UpdtComptMast.CategoryId = ((MasterListItem)CmbCategory.SelectedItem).ID;
                UpdtComptMast.SubCategoryId = ((MasterListItem)CmbSubCategory.SelectedItem).ID;

                UpdtComptMast.Status = ((clsProcedureCheckListVO)grdProcedureCheckList.SelectedItem).Status;
                UpdtComptMast.UnitId = ((clsProcedureCheckListVO)grdProcedureCheckList.SelectedItem).UnitId;

                BizAction.CheckList.Add(UpdtComptMast);

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
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        SetCommandButtonCheckList("Modify");
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
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

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

        #endregion

        #region Status Update
        private void StatusCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (grdProcedureCheckList.SelectedItem != null)
            {
                PalashDynamics.ValueObjects.Administration.OTConfiguration.clsUpdateStatusProcedureCheckListBizActionVO BizAction = new PalashDynamics.ValueObjects.Administration.OTConfiguration.clsUpdateStatusProcedureCheckListBizActionVO();

                BizAction.CheckListDetails = new clsProcedureCheckListVO();
                BizAction.CheckListDetails = ((clsProcedureCheckListVO)grdProcedureCheckList.SelectedItem);

                BizAction.CheckListDetails.Status = true;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {

                        objAnimation.Invoke(RotationType.Backward);
                        //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                        //{
                        //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("lblStatusUpdatedSucessfully");
                        //}
                        //else
                        //{
                            msgText = "Status Updated Successfully.";
                        //}
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        SetupPage();
                    }
                    else
                    {
                        SetCommandButtonCheckList("New");
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

        }

        private void StatusCheckBox_UnChecked(object sender, RoutedEventArgs e)
        {
            PalashDynamics.ValueObjects.Administration.OTConfiguration.clsUpdateStatusProcedureCheckListBizActionVO BizAction = new PalashDynamics.ValueObjects.Administration.OTConfiguration.clsUpdateStatusProcedureCheckListBizActionVO();

            BizAction.CheckListDetails = new clsProcedureCheckListVO();
            BizAction.CheckListDetails = ((clsProcedureCheckListVO)grdProcedureCheckList.SelectedItem);

            BizAction.CheckListDetails.Status = false;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    objAnimation.Invoke(RotationType.Backward);
                    //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                    //{
                    //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("lblStatusUpdatedSucessfully");
                    //}
                    //else
                    //{
                        msgText = "Status Updated Successfully.";
                    //}
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    SetupPage();
                }
                else
                {
                    SetCommandButtonCheckList("New");
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

        # region Clear function
        public void ClearUI()
        {
            txtCode.Text = string.Empty;
            txtDescription.Text = string.Empty;
            CmbCategory.SelectedValue = ((List<MasterListItem>)CmbCategory.ItemsSource)[0].ID;
            if (CmbSubCategory.ItemsSource != null)
                CmbSubCategory.SelectedValue = ((List<MasterListItem>)CmbSubCategory.ItemsSource)[0].ID;
        }
        #endregion

        #region LostFocus And KeyDown
        private void txtDescription_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = CIMS.Comman.HandleAlphnumericAndSpecialChar(sender, e, true);
        }

        private void txtDescription_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)e.OriginalSource).Text))
            {
                if (!((TextBox)e.OriginalSource).Text.AllowSpecialChar())
                {
                    ((TextBox)e.OriginalSource).Text = string.Empty;
                    ((TextBox)e.OriginalSource).BorderBrush = new SolidColorBrush(Colors.Red);
                    //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                    //{
                    //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("SpecCharNotValidation_Msg");
                    //}
                    //else
                    //{
                        msgText = "Only & ,.,- and space special characters are allowed.";
                    //}
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                }
                else
                {
                    ((TextBox)e.OriginalSource).BorderBrush = new SolidColorBrush(Colors.Gray);
                    txtDescription.Text = txtDescription.Text.ToTitleCase();
                }
            }
        }

        private void txtCode_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)e.OriginalSource).Text))
            {
                if (!((TextBox)e.OriginalSource).Text.IsItSpecialChar())
                {
                    ((TextBox)e.OriginalSource).Text = string.Empty;
                    ((TextBox)e.OriginalSource).BorderBrush = new SolidColorBrush(Colors.Red);
                    //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                    //{
                    //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("SpecialCharValidation_Msg");
                    //}
                    //else
                    //{
                        msgText = "Special characters are not allowed.";
                    //}
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
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

        #endregion

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                dpProcedureCheckList.PageIndex = 0;
                SetupPage();
            }
        }

    }
}
