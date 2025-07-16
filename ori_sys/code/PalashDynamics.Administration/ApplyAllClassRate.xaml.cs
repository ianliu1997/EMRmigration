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
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using CIMS;

namespace PalashDynamics.Administration
{
    public partial class ApplyAllClassRate : ChildWindow
    {
        public ApplyAllClassRate()
        {
            InitializeComponent();
        }
        public event RoutedEventHandler OnSaveButton_Click;
        public event RoutedEventHandler OnCancel_Click;
        public long ServiceID { get; set; }
        public List<clsServiceMasterVO> DeepCopyServiceClassList { get; set; }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            if (OnSaveButton_Click != null)
                OnSaveButton_Click(this, new RoutedEventArgs());
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            if (OnCancel_Click != null)
                OnCancel_Click(this, new RoutedEventArgs());
        }
        FrameworkElement element;
        DataGridRow row;
        TextBox TxtServiceClassRate;
        private void chkSelectClass_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            foreach (clsServiceMasterVO item in grdClass.ItemsSource)
            {
                if (grdClass.SelectedItem != null)
                {
                    element = grdClass.Columns.Last().GetCellContent(grdClass.SelectedItem);
                    row = DataGridRow.GetRowContainingElement(element.Parent as FrameworkElement);
                    TxtServiceClassRate = FindVisualChild<TextBox>(row, "txtServiceClassRate");

                    if (chk.IsChecked == true && item.ClassID == ((clsServiceMasterVO)grdClass.SelectedItem).ClassID && item.ClassName == ((clsServiceMasterVO)grdClass.SelectedItem).ClassName)
                    {
                        item.IsSelected = true;
                        item.IsClassRateReadonly = true;
                        TxtServiceClassRate.IsEnabled = true;
                    }
                }
            }
        }

        private void ApplyBaseRate_Click(object sender, RoutedEventArgs e)
        {
            FetchClassRate();
        }
        private ChildControl FindVisualChild<ChildControl>(DependencyObject DependencyObj, String TextBoxName)
         where ChildControl : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(DependencyObj); i++)
            {
                DependencyObject Child = VisualTreeHelper.GetChild(DependencyObj, i);

                if (Child != null && Child is ChildControl)
                {
                    if (Child is TextBox)
                    {
                        if (((TextBox)Child).Name.Equals(TextBoxName))
                        {
                            return (ChildControl)Child;
                        }
                    }
                    else if (Child is DataGrid)
                    {
                        if (((DataGrid)Child).Name.Equals(TextBoxName))
                        {
                            return (ChildControl)Child;
                        }
                    }
                    else
                    {
                        ChildControl ChildOfChild = FindVisualChild<ChildControl>(Child, TextBoxName);
                        if (ChildOfChild != null)
                        {
                            return ChildOfChild;
                        }
                    }
                }
                else
                {
                    ChildControl ChildOfChild = FindVisualChild<ChildControl>(Child, TextBoxName);

                    if (ChildOfChild != null)
                    {
                        return ChildOfChild;
                    }
                }
            }
            return null;
        }
        
        private void FetchClassRate()
        {
            clsGetServiceWiseClassRateBizActionVO BizAction=new clsGetServiceWiseClassRateBizActionVO();
            BizAction.ServiceID = ServiceID;
            BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<clsServiceMasterVO> objList = new List<clsServiceMasterVO>();
                    objList = ((clsGetServiceWiseClassRateBizActionVO)args.Result).ServiceClassList;
                    foreach (var item in objList)
                    {
                        if (chkApplyBaseRate.IsChecked == true)
                        {
                            foreach (var item1 in ClassList)
                            {
                                if (item.ClassID == item1.ClassID)
                                {
                                    item1.Rate = item.Rate;
                                    item1.IsSelected = true;
                                    item1.IsEnabled = false;
                                    //..............
                                    item1.IsClassRateReadonly = true;
                                    //.......................
                                }
                            }
                        }
                        else
                        {
                            foreach (var item1 in ClassList)
                            {
                                if (item.ClassID == item1.ClassID)
                                {
                                    item1.Rate = Convert.ToDecimal("0.00");
                                    item1.IsSelected = false;
                                    item1.IsClassRateReadonly = false;
                                    item1.IsEnabled = true;
                                }
                            }
                        }
                    }
                    grdClass.ItemsSource = null;
                    grdClass.ItemsSource = ClassList;
                    DeepCopyServiceClassList = ClassList;
                    grdClass.UpdateLayout();
                    grdClass.Focus();
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();

        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FillClassGrid();
        }

        List<clsServiceMasterVO> ClassList = new List<clsServiceMasterVO>();
        private void FillClassGrid()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_ClassMaster;
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
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        foreach (var item in objList)
                        {
                            ClassList.Add(new clsServiceMasterVO() { ClassID = item.ID, ClassName = item.Description, UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId });
                        }
                        grdClass.ItemsSource = null;
                        grdClass.ItemsSource = ClassList;
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

