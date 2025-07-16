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
using PalashDynamics.ValueObjects;
using PalashDynamics.Collections;
using System.Windows.Data;
using PalashDynamics.UserControls;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects.User;

namespace PalashDynamics.Administration
{
    public partial class UserCategoryLink : ChildWindow
    {
        #region Variable AND List declaration
        bool IsValidate = true;
        public event RoutedEventHandler OnRemoveButton_Click;
        public event RoutedEventHandler OnCancelButton_Click;
        public clsUserVO SelectedUser { get; set; }
        public PagedSortableCollectionView<clsUserCategoryLinkVO> DataList { get; private set; }
        public List<clsUserCategoryLinkVO> LstCategory;
        PagedCollectionView collection;
        public List<clsUserCategoryLinkVO> SelectedCategoryList;
        public List<clsUserCategoryLinkVO> ExistingCatList;
        WaitIndicator indicator;
        public int PageSize
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
        #endregion

        #region Constructor

        public UserCategoryLink()
        {
            InitializeComponent();
            DataList = new PagedSortableCollectionView<clsUserCategoryLinkVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            PageSize = 10;
            LstCategory = new List<clsUserCategoryLinkVO>();
            SelectedCategoryList = new List<clsUserCategoryLinkVO>();
            ExistingCatList = new List<clsUserCategoryLinkVO>();
            indicator = new WaitIndicator();
        }

        #endregion

        #region Toggle Button Click Events
        private void CmdLink_Click(object sender, RoutedEventArgs e)
        {
            string strMsg = "DO YOU WANT TO LINK WITH SELECTED CATEGORIES?.";
            MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
            {
                if (res == MessageBoxResult.Yes)
                {
                    if (SelectedCategoryList != null && SelectedUser != null && SelectedCategoryList.Count > 0)
                        SelectedCategoryList.ToList().ForEach(x => x.UserID = SelectedUser.ID);
                    AddUserCategoryLink();
                }
                else
                {
                    FillCategory();
                    SelectedCategoryList = new List<clsUserCategoryLinkVO>();
                }
            };
            msgW1.Show();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            GetExistingLinkedCategory();
        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        #endregion

        #region Loaded

        private void UserCategoryLink_Loaded(object sender, RoutedEventArgs e)
        {
            GetExistingLinkedCategory();
            //FillCategory();
        }
        #endregion

        #region Other Events
        private void chkSelectAll_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            bool check = chk.IsChecked.Value;
            if (chk.IsChecked == true)
            {
                DataList.ToList().ForEach(z => z.IsSelected = true);
                SelectedCategoryList = new List<clsUserCategoryLinkVO>();
                SelectedCategoryList = DataList.ToList();
            }
            else
            {
                DataList.ToList().ForEach(z => z.IsSelected = false);
                SelectedCategoryList = new List<clsUserCategoryLinkVO>();
            }
            dgCategory.ItemsSource = null;
            collection = new PagedCollectionView(DataList);
            collection.GroupDescriptions.Add(new PropertyGroupDescription("CategoryType"));
            dgCategory.ItemsSource = collection;

        }

        private void textBox_KeyUp(object sender, KeyEventArgs e)
        {

        }


        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillCategory();
        }

        private void chkIsSelected_Click(object sender, RoutedEventArgs e)
        {
            if (dgCategory.SelectedItem != null)
            {
                clsUserCategoryLinkVO selectedCategory = (clsUserCategoryLinkVO)dgCategory.SelectedItem;
                CheckBox chkBox = sender as CheckBox;
                if (chkBox.IsChecked == true)
                {
                    if (SelectedCategoryList != null)
                        SelectedCategoryList.Add(selectedCategory);
                    if (SelectedCategoryList.Count == DataList.ToList().Count)
                    {
                        CheckBox chkSelectAll = GetChildControl(dgCategory, "chkSelectAll") as CheckBox;
                        chkSelectAll.IsChecked = true;
                    }
                }
                else
                {
                    clsUserCategoryLinkVO obj = (clsUserCategoryLinkVO)dgCategory.SelectedItem;
                    obj = SelectedCategoryList.Where(z => z.CategoryTypeID == obj.CategoryTypeID && z.ID == obj.ID).FirstOrDefault();
                    if (SelectedCategoryList != null)
                        SelectedCategoryList.Remove(obj);
                    CheckBox chkSelectAll = GetChildControl(dgCategory, "chkSelectAll") as CheckBox;
                    chkSelectAll.IsChecked = false;
                }
            }
        }

        #endregion

        #region Private Methods
        private void FillCategory()
        {
            try
            {
                indicator.Show();
                clsGetCategoryListBizActionVO BizActionObject = new clsGetCategoryListBizActionVO();
                BizActionObject.CategoryList = new List<clsUserCategoryLinkVO>();
                BizActionObject.CategoryListDetails = new clsUserCategoryLinkVO();
                if (!String.IsNullOrEmpty(txtCatDescription.Text))
                    BizActionObject.CategoryListDetails.CategoryName = txtCatDescription.Text;
                BizActionObject.PagingEnabled = false;
                BizActionObject.MaximumRows = DataList.PageSize; ;
                BizActionObject.StartRowIndex = DataList.PageIndex * DataList.PageSize;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Error == null && ea.Result != null)
                    {
                        clsGetCategoryListBizActionVO result = ea.Result as clsGetCategoryListBizActionVO;
                        DataList.TotalItemCount = result.TotalRows;
                        DataList.Clear();
                        foreach (clsUserCategoryLinkVO Cat in result.CategoryList)
                        {
                            DataList.Add(Cat);
                        }
                        CheckBox chkAllSelect = GetChildControl(dgCategory, "chkSelectAll") as CheckBox;
                        if (chkAllSelect.IsChecked == true)
                            DataList.ToList().ForEach(z => z.IsSelected = true);
                        if (ExistingCatList != null && ExistingCatList.Count > 0)
                        {
                            foreach (clsUserCategoryLinkVO item in DataList.ToList())
                            {
                                ExistingCatList.ToList().ForEach(x =>
                                { if (x.CategoryID == item.ID && x.CategoryTypeID == item.CategoryTypeID) { item.IsSelected = true; } });
                            }
                        }

                        var item5 = from r in DataList.ToList()
                                    where r.IsSelected == false
                                    select r;
                        if (item5 != null && item5.ToList().Count > 0)
                        {
                            CheckBox chkSelectAll = GetChildControl(dgCategory, "chkSelectAll") as CheckBox;
                            chkSelectAll.IsChecked = false;
                        }
                        else
                        {
                            CheckBox chkSelectAll = GetChildControl(dgCategory, "chkSelectAll") as CheckBox;
                            chkSelectAll.IsChecked = true;
                        }

                        dgCategory.ItemsSource = null;
                        collection = new PagedCollectionView(DataList);
                        collection.GroupDescriptions.Add(new PropertyGroupDescription("CategoryType"));
                        dgCategory.ItemsSource = collection;
                        SelectedCategoryList = ExistingCatList;
                        BatchDataPager.Source = null;
                        BatchDataPager.PageSize = (Int32)BizActionObject.MaximumRows;
                        BatchDataPager.Source = DataList;
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "ERROR OCCURRED WHILE PROCESSING.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizActionObject, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                indicator.Close();
                throw;
            }
            finally
            {
                indicator.Close();
            }
        }

        private object GetChildControl(DependencyObject parent, string controlName)
        {

            Object tempObj = null;
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int counter = 0; counter < count; counter++)
            {
                //Get The Child Control based on Index
                tempObj = VisualTreeHelper.GetChild(parent, counter);

                //If Control's name Property matches with the argument control
                //name supplied then Return Control
                if ((tempObj as DependencyObject).GetValue(NameProperty).ToString() == controlName)
                    return tempObj;
                else //Else Search Recursively
                {
                    tempObj = GetChildControl(tempObj as DependencyObject, controlName);
                    if (tempObj != null)
                        return tempObj;
                }
            }
            return null;
        }

        private void AddUserCategoryLink()
        {
            clsAddUserCategoryLinkBizActionVO bizAction = new clsAddUserCategoryLinkBizActionVO();
            bizAction.UserCategoryLinkDetails = new clsUserCategoryLinkVO();
            bizAction.UserCategoryLinkDetails.UserID = SelectedUser.ID;
            bizAction.UserCategoryLinkList = new List<clsUserCategoryLinkVO>();
            bizAction.UserCategoryLinkList = SelectedCategoryList;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Error == null && ea.Result != null)
                {
                    string strMsg = SelectedUser.LoginName + " LINKED WITH SELECTED CATEGORIES.";
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.OK)
                        {
                            SelectedCategoryList.Clear();
                            SelectedCategoryList = new List<clsUserCategoryLinkVO>();
                            this.DialogResult = false;
                        }
                    };
                    msgW1.Show();
                }
            };
            client.ProcessAsync(bizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void GetExistingLinkedCategory()
        {
            try
            {
                indicator.Show();
                clsGetExistingCategoryListBizActionVO BizActionObject = new clsGetExistingCategoryListBizActionVO();
                BizActionObject.ExistingCategoryList = new List<clsUserCategoryLinkVO>();
                BizActionObject.ExistingCategoryListDetails = new clsUserCategoryLinkVO();
                BizActionObject.ExistingCategoryListDetails.UserID = SelectedUser.ID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Error == null && ea.Result != null)
                    {
                        clsGetExistingCategoryListBizActionVO result = ea.Result as clsGetExistingCategoryListBizActionVO;
                        ExistingCatList = result.ExistingCategoryList;
                        FillCategory();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "ERROR OCCURRED WHILE PROCESSING.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizActionObject, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                indicator.Close();
                throw;
            }
            finally
            {
                indicator.Close();
            }
        }
        #endregion
    }
}

