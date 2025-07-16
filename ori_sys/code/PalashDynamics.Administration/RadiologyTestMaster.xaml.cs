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
using PalashDynamics.ValueObjects.Radiology;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using System.Collections.ObjectModel;
using PalashDynamics.Collections;
using System.Reflection;
  

namespace PalashDynamics.Administration
{
    public partial class RadiologyTestMaster : UserControl
    {
        #region Variable Declaration
        private SwivelAnimation objAnimation;
        bool IsPageLoded = false;
        bool IsCancel = true;
        bool IsNew = false;
        int ClickedFlag = 0;

        public ObservableCollection<clsRadItemDetailMasterVO> ItemList { get; set; }

        #endregion
        
        #region Paging

        public PagedSortableCollectionView<clsRadiologyVO> DataList { get; private set; }

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



        #endregion
        
        public RadiologyTestMaster()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
           
            DataList = new PagedSortableCollectionView<clsRadiologyVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
        }
       
        /// <summary>
        /// Purpose:Used when refresh event get called.
        /// Creation Date:04/07/2011
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchData();

        }
        
        private void RadiologyTestMaster_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                SetCommandButtonState("Load");

                this.DataContext = new clsRadiologyVO();
                ItemList = new ObservableCollection<clsRadItemDetailMasterVO>();
                FillTemplateDetails();
                FillCategory();
                FillService();
                FillItem();
                FetchData();
                txtCode.Focus();
                CheckValidations();
            }
            IsPageLoded = true;
            txtCode.Focus();
        }
       
        /// <summary>
        /// Purpose:For add new test
        /// Creation Date:04/07/2011
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            this.SetCommandButtonState("New");

            ClearData();
            IsNew = true;
            TabControlMain.SelectedIndex = 0;
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + "New Test Details";
            objAnimation.Invoke(RotationType.Forward);
            cmdItemModify.IsEnabled = false;
          
        }

        #region FillComboBox
        private void FillTemplateDetails()
        {
            clsGetRadTemplateDetailsBizActionVO BizAction = new clsGetRadTemplateDetailsBizActionVO();
            BizAction.TemplateList = new List<clsRadiologyVO>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
          
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetRadTemplateDetailsBizActionVO)arg.Result).TemplateList != null)
                    {
                        BizAction.TemplateList = ((clsGetRadTemplateDetailsBizActionVO)arg.Result).TemplateList;

                        List<clsRadTemplateDetailMasterVO> UserTemplateList = new List<clsRadTemplateDetailMasterVO>();

                        foreach (var item in BizAction.TemplateList)
                        {
                            UserTemplateList.Add(new clsRadTemplateDetailMasterVO() { TestTemplateID = item.TemplateID, TemplateName = item.Description, TemplateCode = item.Code, Status = false });
                        }
                        
                        dgTemplateList.ItemsSource = null;
                        dgTemplateList.ItemsSource = UserTemplateList;

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

        private void FillService()
        {
            clsGetRadServiceBizActionVO BizAction = new clsGetRadServiceBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetRadServiceBizActionVO)arg.Result).MasterList);

                    cmbService.ItemsSource = null;
                    cmbService.ItemsSource = objList;
                    cmbService.SelectedItem = objList[0];

                    if (this.DataContext != null)
                    {
                        cmbService.SelectedValue = ((clsRadiologyVO)this.DataContext).ServiceID;
                    }
                   

                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillCategory()
        {

            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_RadTestCategory;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {

                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbCategory.ItemsSource = null;
                    cmbCategory.ItemsSource = objList;
                    cmbCategory.SelectedItem = objList[0];

                    cmbCategoryFront.ItemsSource = null;
                    cmbCategoryFront.ItemsSource = objList;
                    cmbCategoryFront.SelectedItem = objList[0];

                  

                    if (this.DataContext != null)
                    {
                        cmbCategory.SelectedValue = ((clsRadiologyVO)this.DataContext).CategoryID;
                        cmbCategoryFront.SelectedValue = ((clsRadiologyVO)this.DataContext).CategoryID;
                        
                    }
                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillItem()
        {
            clsGetRadItemBizActionVO BizAction = new clsGetRadItemBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetRadItemBizActionVO)arg.Result).MasterList);

                    cmbItem.ItemsSource = null;
                    cmbItem.ItemsSource = objList;
                    cmbItem.SelectedItem = objList[0];

                    if (this.DataContext != null)
                    {
                        //cmbItem.SelectedItem = ((clsRadiologyVO)this.DataContext).item;
                    }


                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        #endregion

        # region Search Data
        /// <summary>
        /// Purpose:Get data using different sarach criteria.
        /// Creation Date:04/07/2011
        /// </summary>
        private void FetchData()
        {
            clsGetRadTestDetailsBizActionVO BizAction = new clsGetRadTestDetailsBizActionVO();
            BizAction.TestList = new List<clsRadiologyVO>();

            if (txtDescriptionFront.Text != "")
            {
                BizAction.Description = txtDescriptionFront.Text;
            }
            
            if (cmbCategoryFront.SelectedItem != null)
            {
                BizAction.Category = ((MasterListItem)cmbCategoryFront.SelectedItem).ID;
            }


            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

   
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {

                    if (((clsGetRadTestDetailsBizActionVO)arg.Result).TestList != null)
                    {
                        clsGetRadTestDetailsBizActionVO result = arg.Result as clsGetRadTestDetailsBizActionVO;
                        DataList.TotalItemCount = result.TotalRows;

                        if (result.TestList != null)
                        {
                            DataList.Clear();

                            foreach (var item in result.TestList)
                            {
                                DataList.Add(item);
                            }

                            dgTestList.ItemsSource = null;
                            dgTestList.ItemsSource = DataList;

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

        /// <summary>
        /// Purpose:Get existing data using different search criteria.
        /// Creation Date:05/07/2011
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FetchData();
            dgDataPager.PageIndex = 0;
        }

#endregion

        #region Save data
        /// <summary>
        /// Purpose:Save test in database
        /// Creation Date:05/07/2011
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;
            if (ClickedFlag == 1)
            {
                bool SaveTest = true;

                SaveTest = CheckValidations();

                if (SaveTest == true)
                {
                    string msgTitle = "Palash";
                    string msgText = "Are you sure you want to save the Test Master?";

                    MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                    msgW.Show();
                }
                else
                    ClickedFlag = 0;

            }

        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if (CheckDuplicasy())
                {

                    Save();
                }
                else
                {
                    ClickedFlag = 0;
                }

            }

            else
            {
                ClickedFlag = 0;
            }
        }

        private void Save()
        {
            try
            {
                clsAddRadTestMasterBizActionVO BizAction = new clsAddRadTestMasterBizActionVO();
                BizAction.TestDetails =(clsRadiologyVO)grdTest.DataContext;

                if (cmbCategory.SelectedItem != null)
                    BizAction.TestDetails.CategoryID = ((MasterListItem)cmbCategory.SelectedItem).ID;

                if (cmbService.SelectedItem != null)
                    BizAction.TestDetails.ServiceID = ((MasterListItem)cmbService.SelectedItem).ID;

                BizAction.TestDetails.TestTemplateList = (List<clsRadTemplateDetailMasterVO>)dgTemplateList.ItemsSource;
                
                BizAction.TestDetails.TestItemList = ItemList.ToList();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    ClickedFlag = 0;
                    SetCommandButtonState("Save");
                    if (arg.Error == null)
                    {
                        FetchData();
                        ClearData();
                        objAnimation.Invoke(RotationType.Backward);
                        //Indicatior.Close();
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Test Master Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding Test Master .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

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

        #region Items details
        /// <summary>
        /// Purpose:Add selected item in item grid
        /// Creation Date:04/07/2011
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            
            if(ItemValidation())
            {
            
                clsRadItemDetailMasterVO tempItem = new clsRadItemDetailMasterVO();
                var item1 = from r in ItemList
                            where (r.ItemID == ((MasterListItem)cmbItem.SelectedItem).ID)
                            select new clsRadItemDetailMasterVO
                            {
                                Status = r.Status,
                                ItemID = r.ItemID,
                                ItemName = r.ItemName,
                                Quantity = r.Quantity
                            };

                if (item1.ToList().Count == 0)
                {

                    tempItem.ItemID = ((MasterListItem)cmbItem.SelectedItem).ID;
                    tempItem.ItemName = ((MasterListItem)cmbItem.SelectedItem).Description;
                    tempItem.Quantity = Convert.ToDouble(txtQuantity.Text);
                    tempItem.Status = true;
                    ItemList.Add(tempItem);

                    dgItemDetailsList.ItemsSource = null;
                    dgItemDetailsList.ItemsSource = ItemList;

                    cmbItem.SelectedValue=(long)0;
                    txtQuantity.Text="";

                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Items already added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }

            }
            
        }

         /// <summary>
       /// Purpose:Cecks & assigns validations for the controls.
       /// Creation Date:05/07/2011
       /// </summary>
       /// <returns></returns>
         private bool ItemValidation()
        {
             bool result = true;


             if (txtQuantity.Text == "")
             {
                 txtQuantity.SetValidation("Please Select Quantity");
                 txtQuantity.RaiseValidationError();
                 txtQuantity.Focus();
                 result = false;
             }
             else
                 txtQuantity.ClearValidationError();

            if ((MasterListItem)cmbItem.SelectedItem == null)
            {
                cmbItem.TextBox.SetValidation("Please Select Item");
                cmbItem.TextBox.RaiseValidationError();
                cmbItem.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbItem.SelectedItem).ID == 0)
            {
                cmbItem.TextBox.SetValidation("Please Select Item");
                cmbItem.TextBox.RaiseValidationError();
                cmbItem.Focus();
                result = false;
            }
            else
                cmbItem.TextBox.ClearValidationError();




            return result;
        }
        
        /// <summary>
        /// Purpose:Modify selected items and quantity
        /// Creation Date:05/07/2011
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdItemModify_Click(object sender, RoutedEventArgs e)
        {
            if (ItemList.Count > 0)
            {
                if (((MasterListItem)cmbItem.SelectedItem).ID != 0 && txtQuantity.Text != "")
                {
                    int var = dgItemDetailsList.SelectedIndex;
                    ItemList.RemoveAt(dgItemDetailsList.SelectedIndex);

                    ItemList.Insert(var, new clsRadItemDetailMasterVO
                    {
                        ItemID = ((MasterListItem)cmbItem.SelectedItem).ID,
                        ItemName = ((MasterListItem)cmbItem.SelectedItem).Description,
                        Quantity = Convert.ToDouble(txtQuantity.Text),
                        Status = true

                    }
                    );

                    dgItemDetailsList.ItemsSource = ItemList;
                    dgItemDetailsList.Focus();
                    dgItemDetailsList.UpdateLayout();
                    dgItemDetailsList.SelectedIndex = ItemList.Count - 1;

                    cmbItem.SelectedValue = (long)0;
                    txtQuantity.Text = "";
                }
               
                                    

            }
            cmdAdd.IsEnabled = true;
        }


        /// <summary>
        /// Purpose:Delete selected items from grid.
        /// Creation Date:05/07/2011
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgItemDetailsList.SelectedItem != null)
                {
                    string msgTitle = "";
                    string msgText = "Are you sure you want to Delete the selected Item ?";

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            ItemList.RemoveAt(dgItemDetailsList.SelectedIndex);

                        }
                    };

                    msgWD.Show();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Purpose:Get details for selected items
        /// Creation Date:07/07/2011
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hlbEditItems_Click(object sender, RoutedEventArgs e)
        {
            cmbItem.SelectedValue = ((clsRadItemDetailMasterVO)dgItemDetailsList.SelectedItem).ItemID;
            txtQuantity.Text = ((clsRadItemDetailMasterVO)dgItemDetailsList.SelectedItem).Quantity.ToString();
            cmdAdd.IsEnabled = false;
            cmdItemModify.IsEnabled = true;
        }


        #endregion

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Cancel");
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = "";

            objAnimation.Invoke(RotationType.Backward);

            if (IsCancel == true)
            {
                mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Radiology Configuration";

                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmRadiologyConfiguration") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);

            }
            else
            {
                IsCancel = true;
            }
        }

        #region Modify data
        /// <summary>
        /// Purpose:Modify existing test
        /// Creation Date:06/07/2011
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;
            if (ClickedFlag == 1)
            {

                bool ModifyTemplate = true;
                ModifyTemplate = CheckValidations();
                if (ModifyTemplate == true)
                {
                    string msgTitle = "Palash";
                    string msgText = "Are you sure you want to Update the Test Master?";

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);

                    msgW1.Show();
                }
                else
                    ClickedFlag = 0;
            }

        }

        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if (CheckDuplicasy())
                {
                    Modify();
                }
                else
                {
                    ClickedFlag = 0;
                }
            }
            else
            {
                ClickedFlag = 0;
            }
        }

        private void Modify()
        {
            try
            {
                clsAddRadTestMasterBizActionVO BizAction = new clsAddRadTestMasterBizActionVO();
                BizAction.TestDetails = (clsRadiologyVO)grdTest.DataContext;

                if (cmbCategory.SelectedItem != null)
                    BizAction.TestDetails.CategoryID = ((MasterListItem)cmbCategory.SelectedItem).ID;

                if (cmbService.SelectedItem != null)
                    BizAction.TestDetails.ServiceID = ((MasterListItem)cmbService.SelectedItem).ID;

                BizAction.TestDetails.TestTemplateList = (List<clsRadTemplateDetailMasterVO>)dgTemplateList.ItemsSource;

                BizAction.TestDetails.TestItemList = ItemList.ToList();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    ClickedFlag = 0;
                    SetCommandButtonState("Save");
                    if (arg.Error == null)
                    {
                        FetchData();
                        ClearData();
                        objAnimation.Invoke(RotationType.Backward);
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Test Master Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding Test Master .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

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

       
        /// <summary>
        /// Purpose:View selected template description
        /// Creation Date:06/07/2011
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       
        private void ViewTemplate_Click(object sender, RoutedEventArgs e)
        {
            if (((clsRadTemplateDetailMasterVO)dgTemplateList.SelectedItem).TestTemplateID != 0)
            {
                ViewRadiologyTemplate Win = new ViewRadiologyTemplate();
                Win.TemplateID = ((clsRadTemplateDetailMasterVO)dgTemplateList.SelectedItem).TestTemplateID;
                Win.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Please select Template.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                msgW1.Show();
            }

        }

        #region View Test details
        /// <summary>
        /// Purpose:View existing test details
        /// Creation Date:05/07/2011
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void View_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("View");
            ClearData();
            
            if (dgTestList.SelectedItem != null)
            {
                IsNew = false;
                grdTest.DataContext = ((clsRadiologyVO)dgTestList.SelectedItem).DeepCopy();
                cmbCategory.SelectedValue = ((clsRadiologyVO)dgTestList.SelectedItem).CategoryID;
                cmbService.SelectedValue = ((clsRadiologyVO)dgTestList.SelectedItem).ServiceID;
                if (((clsRadiologyVO)dgTestList.SelectedItem).TestID >0)
                {
                    FillTemplateAndITemList(((clsRadiologyVO)dgTestList.SelectedItem).TestID);
                }
                objAnimation.Invoke(RotationType.Forward);
                TabControlMain.SelectedIndex = 0;
            }

            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + ((clsRadiologyVO)dgTestList.SelectedItem).Description;

            cmdItemModify.IsEnabled =false;
        }

        /// <summary>
        /// Purpose:View template and item details in resp. datagrid.
        /// Creation Date:06/07/2011
        /// </summary>
        /// <param name="iTestID"></param>
        private void FillTemplateAndITemList(long iTestID)
        {
            clsGetRadTemplateAndItemByTestIDBizActionVO BizAction=new clsGetRadTemplateAndItemByTestIDBizActionVO();
            BizAction.TestList = new List<clsRadTemplateDetailMasterVO>();
            BizAction.ItemList = new List<clsRadItemDetailMasterVO>();

            BizAction.TestID = iTestID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    clsGetRadTemplateAndItemByTestIDBizActionVO DetailsVO = new clsGetRadTemplateAndItemByTestIDBizActionVO();
                    DetailsVO = (clsGetRadTemplateAndItemByTestIDBizActionVO)arg.Result;

                    if (DetailsVO.TestList != null)
                    {
                        List<clsRadTemplateDetailMasterVO> lstTemplate = (List<clsRadTemplateDetailMasterVO>)dgTemplateList.ItemsSource;
                        foreach (var item1 in DetailsVO.TestList)
                        {
                            foreach (var item in lstTemplate)
                            {
                                if (item.TestTemplateID == item1.TestTemplateID)
                                {
                                   item.Status = item1.Status;
                                }
                            }
                            dgTemplateList.ItemsSource = null;
                            dgTemplateList.ItemsSource=lstTemplate;
                        }
                       
                    }

                    if (DetailsVO.ItemList != null)
                    {

                        List<clsRadItemDetailMasterVO> ObjItem;
                        ObjItem = DetailsVO.ItemList;
                        foreach (var item4 in ObjItem)
                        { 
                            ItemList.Add(item4);
                        }                            
                        dgItemDetailsList.ItemsSource = ItemList;
                        dgItemDetailsList.Focus();
                        dgItemDetailsList.UpdateLayout();
                        
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

        #endregion
       
        #region Clear controls
        /// <summary>
        /// Purpose:Clear controls on form
        /// Creation Date:07/07/2011
        /// </summary>
        private void ClearData()
        {
            grdTest.DataContext = new clsRadiologyVO();
            txtCode.Text = string.Empty;
            txtDescriptionFront.Text = string.Empty;
            cmbService.SelectedValue = (long)0;
            cmbCategory.SelectedValue = (long)0;
            cmbItem.SelectedValue = (long)0;
            txtQuantity.Text = string.Empty;
            txtPrintTestName.Text = string.Empty;
            dgItemDetailsList.ItemsSource = null;
            ItemList = new ObservableCollection<clsRadItemDetailMasterVO>();

            if ((List<clsRadTemplateDetailMasterVO>)dgTemplateList.ItemsSource != null)
            {
                List<clsRadTemplateDetailMasterVO> lList = (List<clsRadTemplateDetailMasterVO>)dgTemplateList.ItemsSource;
                foreach (var item in lList)
                {
                    item.Status = false;
                }
                dgTemplateList.ItemsSource = null;
                dgTemplateList.ItemsSource = lList;
            }

        }

        #endregion
       
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

        #region Validation
        /// <summary>
        /// Purpose:Check & assigns validations for the controls.
        /// Creation Date:05/07/2011
            /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
         private void txtDescription_LostFocus(object sender, RoutedEventArgs e)
        {
            txtDescription.Text = txtDescription.Text.ToTitleCase();
        }


        private bool CheckValidations()
        {
            bool result = true;




            if (txtDescription.Text == "")
            {

                txtDescription.SetValidation("Please Enter Description");
                txtDescription.RaiseValidationError();
                txtDescription.Focus();

                result = false;
            }
            else
                txtDescription.ClearValidationError();


            if (txtCode.Text == "")
            {

                txtCode.SetValidation("Please Enter Code");
                txtCode.RaiseValidationError();
                txtCode.Focus();

                result = false;
            }
            else
                txtCode.ClearValidationError();

            if (txtPrintTestName.Text == "")
            {
                txtPrintTestName.SetValidation("Please Enter Print Test Name");
                txtPrintTestName.RaiseValidationError();
                txtPrintTestName.Focus();
                result = false;
            }
            else
                txtPrintTestName.ClearValidationError();


            if (IsPageLoded)
            {
                if ((MasterListItem)cmbService.SelectedItem == null)
                {
                    cmbService.TextBox.SetValidation("Please Select Service");
                    cmbService.TextBox.RaiseValidationError();
                    cmbService.Focus();
                    result = false;
                }
                else if (((MasterListItem)cmbService.SelectedItem).ID == 0)
                {
                    cmbService.TextBox.SetValidation("Please Select Service");
                    cmbService.TextBox.RaiseValidationError();
                    cmbService.Focus();
                    result = false;
                }
                else
                    cmbService.TextBox.ClearValidationError();


                if ((MasterListItem)cmbCategory.SelectedItem == null)
                {
                    cmbCategory.TextBox.SetValidation("Please Select Category");
                    cmbCategory.TextBox.RaiseValidationError();
                    cmbCategory.Focus();
                    result = false;
                }
                else if (((MasterListItem)cmbCategory.SelectedItem).ID == 0)
                {
                    cmbCategory.TextBox.SetValidation("Please Select Category");
                    cmbCategory.TextBox.RaiseValidationError();
                    cmbCategory.Focus();
                    result = false;
                }
                else
                    cmbCategory.TextBox.ClearValidationError();


                clsRadTemplateDetailMasterVO Temp = ((List<clsRadTemplateDetailMasterVO>)dgTemplateList.ItemsSource).FirstOrDefault(p => p.Status == true);
                if (Temp == null)
                {
                    string msgTitle = "Palash";
                    string msgText = "Please Select Template";

                    MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW.Show();
                    TabControlMain.SelectedIndex = 0;
                    result = false;
                    return result;
                }

                //if (ItemList.Count == 0)
                //{
                //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                //               new MessageBoxControl.MessageBoxChildWindow("Palash", "You Can not save Test Master without Items.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                //    msgW1.Show();
                //    result = false;
                //    return result;
                //    TabControlMain.SelectedIndex = 1;
                //}




            }

            return result;
        }


        private bool CheckDuplicasy()
        {
            clsRadiologyVO Item;
            clsRadiologyVO Item1;
            clsRadiologyVO Item3;
            if (IsNew)
            {
                Item = ((PagedSortableCollectionView<clsRadiologyVO>)dgTestList.ItemsSource).FirstOrDefault(p => p.Code.ToUpper().Equals(txtCode.Text.Trim().ToUpper()));
                Item1 = ((PagedSortableCollectionView<clsRadiologyVO>)dgTestList.ItemsSource).FirstOrDefault(p => p.Description.ToUpper().Equals(txtDescription.Text.ToUpper()));
                Item3 = ((PagedSortableCollectionView<clsRadiologyVO>)dgTestList.ItemsSource).FirstOrDefault(p => p.ServiceID.Equals(((MasterListItem)cmbService.SelectedItem).ID));
            }
            else
            {
                Item = ((PagedSortableCollectionView<clsRadiologyVO>)dgTestList.ItemsSource).FirstOrDefault(p => p.Code.ToUpper().Equals(txtCode.Text.Trim().ToUpper()) && p.TestID != ((clsRadiologyVO)dgTestList.SelectedItem).TestID);
                Item1 = ((PagedSortableCollectionView<clsRadiologyVO>)dgTestList.ItemsSource).FirstOrDefault(p => p.Description.ToUpper().Equals(txtDescription.Text.ToUpper()) && p.TestID != ((clsRadiologyVO)dgTestList.SelectedItem).TestID);
                Item3 = ((PagedSortableCollectionView<clsRadiologyVO>)dgTestList.ItemsSource).FirstOrDefault(p => p.ServiceID.Equals(((MasterListItem)cmbService.SelectedItem).ID) && p.TestID != ((clsRadiologyVO)dgTestList.SelectedItem).TestID);
            }

            if (Item != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be save because CODE already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                return false;
            }
            else if (Item1 != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be save because Description already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                return false;
            }
            else if (Item3 != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be save because Service already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion
    }
}
