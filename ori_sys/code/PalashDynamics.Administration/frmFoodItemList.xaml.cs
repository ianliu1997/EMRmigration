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
using PalashDynamics.ValueObjects;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Administration.IPD;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Master;

namespace PalashDynamics.Administration
{
    public partial class frmFoodItemList : ChildWindow
    {
        #region Variable Declaration
        public event RoutedEventHandler OnAddButton_Click;
        public List<bool> check = new List<bool>();

        public List<clsIPDDietNutritionMasterVO> FoodItemSource { get; set; }
        public PagedSortableCollectionView<clsIPDDietNutritionMasterVO> MasterList { get; private set; }

        bool IsPageLoded = false;
        WaitIndicator Indicatior = null;
//        public long TariffID { get; set; }

        bool IsSearch = false;
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
                //OnPropertyChanged("PageSize");
            }
        }
        #endregion



        public frmFoodItemList()
        {
            InitializeComponent();

            MasterList = new PagedSortableCollectionView<clsIPDDietNutritionMasterVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            FillFoodCategoryMaster();
        }           

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new clsIPDDietNutritionMasterVO();
            //this.DataContext = new clsIPDDietNutritionMasterVO();
            //FetchData();
            IsSearch = false;
            FillDataGrid();
            txtStoreName.Focus();
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            if (OnAddButton_Click != null)
            {
                this.DialogResult = true;
                OnAddButton_Click(this, new RoutedEventArgs());

                this.Close();
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void txtStoreCode_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                //this.dataGrid2Pager.DataContext = MasterList;
                //this.grdBed.DataContext = MasterList;
                //SetupPage();
                IsSearch = true;
                FillDataGrid();
            }
            catch (Exception ex)
            {
                throw;
            }
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
                clsIPDGetDietNutritionBizActionVO bizActionVO = new clsIPDGetDietNutritionBizActionVO();
                bizActionVO.SearchExpression = txtStoreName.Text;
                bizActionVO.PagingEnabled = true;
                bizActionVO.MaximumRows = MasterList.PageSize;
                bizActionVO.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;
                //getstoreinfo = new clsIPDDietNutritionMasterVO();
                bizActionVO.objDietMasterDetails = new List<clsIPDDietNutritionMasterVO>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        bizActionVO.objDietMasterDetails = (((clsIPDGetDietNutritionBizActionVO)args.Result).objDietMasterDetails);
                        if (bizActionVO.objDietMasterDetails.Count > 0)
                        {
                            MasterList.Clear();
                            MasterList.TotalItemCount = (int)(((clsIPDGetDietNutritionBizActionVO)args.Result).TotalRows);
                            foreach (clsIPDDietNutritionMasterVO item in bizActionVO.objDietMasterDetails)
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

        private void chkService_Click(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
            {
                check[dgFoodItemsList.SelectedIndex] = true;
            }
            else
            {
                check[dgFoodItemsList.SelectedIndex] = false;
            }
        }

        #region Reset All controls
        private void ClearControl()
        {
         //   this.DataContext = new clsIPDDietNutritionMasterVO();
            this.DataContext = new clsIPDDietNutritionMasterVO();
            txtStoreName.Text = string.Empty;
            dgFoodItemsList.SelectedItem = null;
        }

        #endregion

        #region Commented
        //private void FetchData()
        //{
        //    clsGetServiceMasterListBizActionVO BizAction = new clsGetServiceMasterListBizActionVO();
        //    BizAction.ServiceList = new List<clsServiceMasterVO>();

        //    if (txtServiceName.Text != null)
        //        BizAction.ServiceName = txtServiceName.Text;
        //    if (cmbSpecialization.SelectedItem != null)
        //        BizAction.Specialization = ((MasterListItem)cmbSpecialization.SelectedItem).ID;
        //    if (cmbSubSpecialization.SelectedItem != null)
        //        BizAction.SubSpecialization = ((MasterListItem)cmbSubSpecialization.SelectedItem).ID;

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    dgFoodItemsList.ItemsSource = null;
        //    client.ProcessCompleted += (s, arg) =>
        //    {
        //        if (arg.Error == null)
        //        {
        //            if (((clsGetServiceMasterListBizActionVO)arg.Result).ServiceList != null)
        //            {
        //                dgFoodItemsList.ItemsSource = ((clsGetServiceMasterListBizActionVO)arg.Result).ServiceList;
        //                ServiceItemSource = ((clsGetServiceMasterListBizActionVO)arg.Result).ServiceList;

        //                for (int i = 0; i < ((clsGetServiceMasterListBizActionVO)arg.Result).ServiceList.Count; i++)
        //                {
        //                    bool b = false;
        //                    check.Add(b);
        //                }

        //            }
        //        }

        //        else
        //        {
        //            MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                   new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

        //            msgW1.Show();
        //        }

        //    };
        //    client.ProcessAsync(BizAction, new clsUserVO());
        //    client.CloseAsync();
        //}

        #endregion
        private void FillDataGrid()
        {
            try
            {
                long lngItemCategory = 0;
                //clsIPDGetDietNutritionBizActionVO BizAction = new clsIPDGetDietNutritionBizActionVO();
                clsIPDGetDietNutritionBizActionVO BizAction = new clsIPDGetDietNutritionBizActionVO();
                if (!string.IsNullOrEmpty(txtStoreName.Text))
                    BizAction.SearchExpression = txtStoreName.Text;
                else
                    BizAction.SearchExpression = null;
                if (IsSearch == true)
                {

                    lngItemCategory = ((MasterListItem)cmdItemCategory.SelectedItem).ID;
                    if (lngItemCategory > 0)
                    {
                        BizAction.SearchCategory = lngItemCategory;
                    }
                }
                else
                {
                    lngItemCategory = ((clsIPDDietNutritionMasterVO)this.DataContext).ItemID;
                    if (lngItemCategory > 0)
                    {
                        BizAction.SearchCategory = lngItemCategory;
                    }
                }


                BizAction.PagingEnabled = true;
                BizAction.MaximumRows = MasterList.PageSize;
                BizAction.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;
                //getstoreinfo = new clsIPDDietNutritionMasterVO();
                BizAction.objDietMasterDetails = new List<clsIPDDietNutritionMasterVO>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
               // dgFoodItemsList.ItemsSource = null;
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result !=null)
                    {
                        if (((clsIPDGetDietNutritionBizActionVO)arg.Result).objDietMasterDetails != null)
                        {
                            dgFoodItemsList.ItemsSource = null;
                            dgFoodItemsList.ItemsSource = ((clsIPDGetDietNutritionBizActionVO)arg.Result).objDietMasterDetails;
                            FoodItemSource = ((clsIPDGetDietNutritionBizActionVO)arg.Result).objDietMasterDetails;

                            for (int i = 0; i < ((clsIPDGetDietNutritionBizActionVO)arg.Result).objDietMasterDetails.Count; i++)
                            {
                                bool b = false;
                                check.Add(b);
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
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
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

        /// <summary>
        /// Fills Unit Master combo
        /// </summary>
        void FillFoodCategoryMaster()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_FoodItemCategoryMaster;
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
                        cmdItemCategory.ItemsSource = null;
                        cmdItemCategory.ItemsSource = objList;
                    }

                    if (this.DataContext != null)
                    {
                        cmdItemCategory.SelectedValue = ((clsIPDDietNutritionMasterVO)this.DataContext).ItemID;
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


    }
}
