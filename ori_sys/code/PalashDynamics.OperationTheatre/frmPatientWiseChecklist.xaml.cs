using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.ValueObjects.Administration.OTConfiguration;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using System.Windows.Data;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.OperationTheatre
{
    public partial class frmPatientWiseChecklist : ChildWindow
    {
        public bool ViewChecklist { get; set; }
        public long ScheduleID { get; set; }
        public long ScheduleUnieID { get; set; }
        string msgText;
        public event RoutedEventHandler OnSaveButton_Click;
        public List<long> procedureIDList = new List<long>();
        PagedCollectionView pcv = null;

        public frmPatientWiseChecklist()
        {
            InitializeComponent();
            FillCategory();
            //FillCheckList();
            //FillSubCategory();
            this.Loaded += new RoutedEventHandler(PatientWiseChecklist_Loaded);
        }

        void PatientWiseChecklist_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ViewChecklist == false)
                    FillChecklistGrid();
                else
                    FillChecklistGrid(ScheduleID, ScheduleUnieID);
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// Fills checklist grid
        /// </summary>
        /// <param name="ScheduleID"></param>
        private void FillChecklistGrid(long ScheduleID, long ScheduleUnieID)
        {
            try
            {
                clsGetCheckListByScheduleIDBizActionVO BizActionVo = new clsGetCheckListByScheduleIDBizActionVO();

                BizActionVo.ScheduleID = ScheduleID;
                BizActionVo.ScheduleUnitID = ScheduleUnieID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        if (((clsGetCheckListByScheduleIDBizActionVO)e.Result).ChecklistDetails != null)
                        {
                            clsGetCheckListByScheduleIDBizActionVO result = e.Result as clsGetCheckListByScheduleIDBizActionVO;


                            if (result.ChecklistDetails != null)
                            {
                                myList.Clear();
                                myList = result.ChecklistDetails;
                                pcv = new PagedCollectionView(myList);

                                pcv.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
                                pcv.GroupDescriptions.Add(new PropertyGroupDescription("SubCategory1"));

                                dgCheckList.ItemsSource = null;
                                dgCheckList.ItemsSource = pcv;
                            }

                        }
                    }
                };

                Client.ProcessAsync(BizActionVo, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();



            }
            catch (Exception ex)
            {
                throw;
            }
        }

        List<clsPatientProcedureChecklistDetailsVO> myList = new List<clsPatientProcedureChecklistDetailsVO>();
        public List<clsPatientProcedureChecklistDetailsVO> resultCheckList = new List<clsPatientProcedureChecklistDetailsVO>();
        /// <summary>
        /// Fills Checklist
        /// </summary>
        private void FillChecklistGrid()
        {
            try
            {
                clsGetCheckListByProcedureIDBizActionVO BizActionVo = new clsGetCheckListByProcedureIDBizActionVO();
                BizActionVo.procedureIDList = this.procedureIDList;

                //for (int i = 0; i < procedureIDList.Count; i++)
                //{
                //    BizActionVo.ProcedureID = procedureIDList[i];
                BizActionVo.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        if (((clsGetCheckListByProcedureIDBizActionVO)e.Result).ChecklistDetails != null)
                        {
                            clsGetCheckListByProcedureIDBizActionVO result = e.Result as clsGetCheckListByProcedureIDBizActionVO;

                            if (result.ChecklistDetails != null)
                            {
                                myList.Clear();
                                myList = result.ChecklistDetails;
                                pcv = new PagedCollectionView(myList);
                                pcv.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
                                pcv.GroupDescriptions.Add(new PropertyGroupDescription("SubCategory"));
                                pcv.GroupDescriptions.Add(new PropertyGroupDescription("Name"));
                                dgCheckList.ItemsSource = pcv;
                            }

                        }
                    }
                };

                Client.ProcessAsync(BizActionVo, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();

                //}
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        /// <summary>
        /// Adds Checklist item
        /// </summary>

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CheckValidation() && ((MasterListItem)CmbCategory.SelectedItem).ID > 0 && ((MasterListItem)CmbSubCategory.SelectedItem).ID > 0 && ((MasterListItem)CmbCheckList.SelectedItem).ID > 0)

                {
                   
                    bool flag = false;
                  // myList.Add(new clsPatientProcedureChecklistDetailsVO() { Category = ((MasterListItem)CmbCategory.SelectedItem).Description, SubCategory1 = ((MasterListItem)CmbSubCategory.SelectedItem).Description, Name = ((MasterListItem)CmbCheckList.SelectedItem).Description, Status = true, Remarks = "", ID = ((MasterListItem)CmbCheckList.SelectedItem).ID });
                   foreach (var item in myList)
                   {
                       if (item.ID == ((MasterListItem)CmbCheckList.SelectedItem).ID || item.Name == ((MasterListItem)CmbCheckList.SelectedItem).Description)
                       {
                           flag = true;
                       }
                      
                   }
                   if (flag == false)// new CheckList item add
                   {
                       myList.Add(new clsPatientProcedureChecklistDetailsVO() { Category = ((MasterListItem)CmbCategory.SelectedItem).Description, SubCategory1 = ((MasterListItem)CmbSubCategory.SelectedItem).Description, Name = ((MasterListItem)CmbCheckList.SelectedItem).Description, Status = true, Remarks = "", ID = ((MasterListItem)CmbCheckList.SelectedItem).ID });

                       pcv = new PagedCollectionView(myList);

                       pcv.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
                       pcv.GroupDescriptions.Add(new PropertyGroupDescription("SubCategory"));
                       pcv.GroupDescriptions.Add(new PropertyGroupDescription("Name"));

                       dgCheckList.ItemsSource = pcv;
                       CmbCategory.SelectedValue = ((List<MasterListItem>)CmbCategory.ItemsSource)[0].ID;
                       CmbSubCategory.SelectedValue = ((List<MasterListItem>)CmbSubCategory.ItemsSource)[0].ID;
                       CmbCheckList.SelectedValue = ((List<MasterListItem>)CmbCheckList.ItemsSource)[0].ID;
                   }
                   else
                   {
                       string msgText = "";
                       //ckSavePhoto = true;
                       msgText = "Checklist already present in the list";
                       MessageBoxControl.MessageBoxChildWindow msgW =
                             new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                       msgW.Show();
                   
                   }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private bool CheckValidation()
        {
            bool result = true;
            if ((MasterListItem)CmbCategory.SelectedItem == null || ((MasterListItem)CmbCategory.SelectedItem).ID == 0)
            {
                //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                //{
                //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("CategoryValidation_Msg");
                //}
                //else
                //{
                    msgText = "Please select category.";
                //}
                CmbCategory.TextBox.SetValidation(msgText);
                CmbCategory.TextBox.RaiseValidationError();
                result = false;
            }
            else if (((MasterListItem)CmbSubCategory.SelectedItem).ID == 0 || ((MasterListItem)CmbSubCategory.SelectedItem).ID == 0)
            {
                //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                //{
                //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("SubCategoryValidation_Msg");
                //}
                //else
                //{
                    msgText = "Please select subcategory.";
                //}
                CmbSubCategory.TextBox.SetValidation(msgText);
                CmbSubCategory.TextBox.RaiseValidationError();
                result = false;
            }
            else if (((MasterListItem)CmbCheckList.SelectedItem).ID == 0 || ((MasterListItem)CmbCheckList.SelectedItem).ID == 0)
            {
                //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                //{
                //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("CheckListValidation_Msg");
                //}
                //else
                //{
                    msgText = "Please select checklist.";
                //}
                CmbCheckList.TextBox.SetValidation(msgText);
                CmbCheckList.TextBox.RaiseValidationError();
                result = false;
            }
            else
            {
                CmbCategory.TextBox.ClearValidationError();
                CmbSubCategory.TextBox.ClearValidationError();
                CmbCheckList.TextBox.ClearValidationError();
                result = true;
            }
            return result;
        }

        private void cmdDeleteCheckListItem_Click(object sender, RoutedEventArgs e)
        {
            myList.RemoveAt(dgCheckList.SelectedIndex);
            dgCheckList.ItemsSource = null;
            pcv = new PagedCollectionView(myList);
            pcv.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
            pcv.GroupDescriptions.Add(new PropertyGroupDescription("SubCategory1"));
            dgCheckList.ItemsSource = pcv;
            dgCheckList.UpdateLayout();
        }

        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgCheckList.ItemsSource != null)
                {
                    foreach (var item in dgCheckList.ItemsSource)
                    {
                        if (((clsPatientProcedureChecklistDetailsVO)item).Status == true)
                        {
                            resultCheckList.Add(((clsPatientProcedureChecklistDetailsVO)item));
                        }
                    }
                }
                if (OnSaveButton_Click != null)
                    OnSaveButton_Click(this, new RoutedEventArgs());
                this.DialogResult = false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

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

        private void FillSubCategory()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_ProcedureSubCategoryMaster;
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

                        CmbSubCategory.ItemsSource = null;
                        CmbSubCategory.ItemsSource = objList;
                        CmbSubCategory.SelectedItem = objList[0];
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

        private void FillCheckList()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_ProcedureCheklistDetails;
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

                        CmbCheckList.ItemsSource = null;
                        CmbCheckList.ItemsSource = objList;
                        CmbCheckList.SelectedItem = objList[0];
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

        private void CmbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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
            try
            {
                if (CmbSubCategory.SelectedItem != null && ((MasterListItem)CmbSubCategory.SelectedItem).ID != 0)
                {
                    FillCheckList(((MasterListItem)CmbSubCategory.SelectedItem).ID, ((MasterListItem)CmbCategory.SelectedItem).ID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

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

                        if (this.DataContext != null)
                        {
                            CmbSubCategory.SelectedValue = ((clsProcedureMasterVO)this.DataContext).SubCategoryID;
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

        private void FillCheckList(long CheckLstID, long CategoryID)
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_ProcedureCheklistDetails;

                BizAction.Parent = new KeyValue { Value = "SubCategoryID", Key = CheckLstID.ToString() };
                BizAction.Category = new KeyValue { Value = "CategoryID", Key = CategoryID.ToString() };
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

                        CmbCheckList.ItemsSource = null;
                        CmbCheckList.ItemsSource = objList;
                        CmbCheckList.SelectedItem = objList[0];

                        if (this.DataContext != null)
                        {
                            CmbCheckList.SelectedValue = ((clsProcedureMasterVO)this.DataContext).CheckListID;
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
    }
}


