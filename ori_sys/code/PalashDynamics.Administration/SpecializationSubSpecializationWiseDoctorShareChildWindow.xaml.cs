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
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Administration;

namespace PalashDynamics.Administration
{
    public partial class SpecializationSubSpecializationWiseDoctorShareChildWindow : ChildWindow
    {
        public List<bool> check = new List<bool>();

        public List<clsSubSpecializationVO> SelectedSubSpecializationList = new List<clsSubSpecializationVO>();

        public List<clsSubSpecializationVO> DeepCopySubSpecializationList { get; set; }

        public bool IsValidate = true;

        public List<clsSubSpecializationVO> SubSpecializationList = new List<clsSubSpecializationVO>();
        public event RoutedEventHandler OnSaveButton_Click;
        public event RoutedEventHandler OnCloseButton_Click;
        public SpecializationSubSpecializationWiseDoctorShareChildWindow()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void SpecializationSearch_Loaded(object sender, RoutedEventArgs e)
        {
            ClearData();
            if (DeepCopySubSpecializationList !=null)
            {
                DeepCopySubSpecializationList.Clear();
            }
            dgSelectedSubSpecializationList.UpdateLayout();
            cmdAdd.IsEnabled = false;
            //chkApplyToSubSp.Visibility = System.Windows.Visibility.Collapsed;
            //txtShareToAllSubSp.Visibility = System.Windows.Visibility.Collapsed;



            SelectedSubSpecializationList = new List<clsSubSpecializationVO>();
            if (DeepCopySubSpecializationList != null)
            {
                if (DeepCopySubSpecializationList.Count > 0)
                {
                    SelectedSubSpecializationList = DeepCopySubSpecializationList.DeepCopy();
                    cmdAdd.IsEnabled = true;
                }
            }
            FillSpecialization();
        }

        private void FillSpecialization()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_Specialization;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    dgServiceList.ItemsSource = null;
                    dgServiceList.ItemsSource = objList;
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        //clsSubSpecializationVO ObjSubspe = new clsSubSpecializationVO();
        private void FillSubSpecialization(long iSupId)
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_SubSpecialization;
            if (iSupId > 0)
                BizAction.Parent = new KeyValue { Key = iSupId, Value = "fkSpecializationID" };
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);
                    dgSelectedSubSpecializationList.ItemsSource = null;
                    foreach (var item in objList)
                    {
                        item.Status = false;
                        //item.SharePercentage = "0.0";
                        //item.IsReadOnly = false;

                    }

                    SubSpecializationList = new List<clsSubSpecializationVO>();
                    foreach (var item in objList)
                    {
                        clsSubSpecializationVO ObjSubspe = new clsSubSpecializationVO();
                        ObjSubspe.SubSpecializationId = item.ID;
                        ObjSubspe.Status = false;
                        ObjSubspe.SubSpecializationName = item.Description;
                        ObjSubspe.SpecializationId = iSupId;
                        SubSpecializationList.Add(ObjSubspe);
                    }

                    dgSelectedSubSpecializationList.ItemsSource = SubSpecializationList;

                    if (SelectedSubSpecializationList.Count > 0)
                    {
                        foreach (var item in SelectedSubSpecializationList)
                        {
                            foreach (clsSubSpecializationVO item1 in dgSelectedSubSpecializationList.ItemsSource)
                            {
                                if (item.SubSpecializationId == item1.SubSpecializationId)
                                {
                                    item1.Status = true;
                                    item1.SharePercentage = item.SharePercentage;
                                    item1.IsReadOnly = true;
                                }
                            }
                        }
                    }

                    if (chkApplyToSubSp.IsChecked == true)
                    {
                        txtShareToAllSubSp.IsEnabled = true;
                        foreach (clsSubSpecializationVO ObjSubSpVO in SubSpecializationList)
                        {
                            ObjSubSpVO.Status = true;
                            ObjSubSpVO.SharePercentage = Convert.ToDouble(txtShareToAllSubSp.Text);
                        }
                    }

                    dgSelectedSubSpecializationList.UpdateLayout();
                    dgSelectedSubSpecializationList.Focus();
                    lblShare.Visibility = Visibility.Visible;
                    txtShareToAllSubSp.Visibility = System.Windows.Visibility.Visible;
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private FrameworkElement GetParent(FrameworkElement child, Type targetType)
        {
            object parent = null;
            if (child != null)
            {
                parent = child.Parent;
            }
            if (parent != null)
            {
                if (parent.GetType() == targetType)
                {
                    return (FrameworkElement)parent;
                }
                else
                {
                    return GetParent((FrameworkElement)parent, targetType);
                }
            }
            return null;

        }
        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            IsValidate = true;
            var dgsubSpItemSource = dgSelectedSubSpecializationList.ItemsSource;
            if (SelectedSubSpecializationList.Count > 0)
            {
                if (dgsubSpItemSource != null)
                {
                    foreach (var item3 in dgsubSpItemSource)
                    {
                        foreach (clsSubSpecializationVO objVO in SelectedSubSpecializationList)
                        {
                            clsSubSpecializationVO item1 = SubSpecializationList.Where(z => z.SubSpecializationId == objVO.SubSpecializationId).FirstOrDefault();
                            if (item1 != null)
                            {
                                if (objVO.SharePercentage == 0)
                               // if (item1.SharePercentage == 0)
                                {
                                    FrameworkElement fe = dgSelectedSubSpecializationList.Columns[2].GetCellContent(item1);
                                    FrameworkElement parent = GetParent(fe, typeof(DataGridCell));
                                    var thisCell = (DataGridCell)parent;
                                    if (thisCell != null)
                                    {
                                        TextBox txt = thisCell.Content as TextBox;
                                        txt.SetValidation("Please Enter Share Percentage more than ZERO");
                                        txt.RaiseValidationError();
                                        txt.Focus();
                                        IsValidate = false;
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (IsValidate == true)
            {
                this.DialogResult = true;
                if (OnSaveButton_Click != null)
                    OnSaveButton_Click(this, new RoutedEventArgs());
            }
        }

        public void ClearData()
        {
            SelectedSubSpecializationList.Clear();
            SubSpecializationList.Clear();
            dgSelectedSubSpecializationList.ItemsSource = null;
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            ClearData();
            if (OnCloseButton_Click != null)
                OnCloseButton_Click(this, new RoutedEventArgs());
        }
        long SubSPId = 0;
        private void dgServiceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          //  ClearData();
            chkApplyToSubSp.IsChecked = false;
         //   chkApplyToSubSp.Visibility = Visibility.Collapsed;
            txtShareToAllSubSp.Text = "";
            var dgsubSpItemSource = dgSelectedSubSpecializationList.ItemsSource;
            #region Validation For Share Percentage
            if (SelectedSubSpecializationList.Count > 0)
            {
                if (dgsubSpItemSource != null)
                {
                    foreach (var item3 in dgsubSpItemSource)
                    {
                        foreach (clsSubSpecializationVO objVO in SelectedSubSpecializationList)
                        {
                            clsSubSpecializationVO item1 = SubSpecializationList.Where(z => z.SubSpecializationId == objVO.SubSpecializationId).FirstOrDefault();
                            if (item1 != null)
                            {
                                if (objVO.SharePercentage == 0 )
                                {
                                    FrameworkElement fe = dgSelectedSubSpecializationList.Columns[2].GetCellContent(item1);
                                    FrameworkElement parent = GetParent(fe, typeof(DataGridCell));
                                    var thisCell = (DataGridCell)parent;
                                    TextBox txt = thisCell.Content as TextBox;
                                    txt.SetValidation("Please Enter Share Percentage more than ZERO");
                                    txt.RaiseValidationError();
                                    txt.Focus();
                                    IsValidate = false;
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            #endregion
            SubSPId = ((MasterListItem)dgServiceList.SelectedItem).ID;
            FillSubSpecialization(SubSPId);
        }

        private void ChildWindow_Closed(object sender, EventArgs e)
        {
        }
        FrameworkElement element;
        DataGridRow row;
        TextBox TxtDoctorShare;
        private void chkSubSpecialization_Click(object sender, RoutedEventArgs e)
        {

            if (dgSelectedSubSpecializationList.SelectedItems != null)
            {
                element = dgSelectedSubSpecializationList.Columns.Last().GetCellContent(dgSelectedSubSpecializationList.SelectedItem);
                row = DataGridRow.GetRowContainingElement(element.Parent as FrameworkElement);
                TxtDoctorShare = FindVisualChild<TextBox>(row, "txtShare");

                List<clsSubSpecializationVO> CancelSubSp = new List<clsSubSpecializationVO>();
                CancelSubSp = ((List<clsSubSpecializationVO>)dgSelectedSubSpecializationList.ItemsSource).ToList();

                var item = from r in CancelSubSp
                           where r.Status == true
                           select r;

                if (item != null && item.ToList().Count > 0)
                {
                    cmdAdd.IsEnabled = true;
                }
                else
                {
                    cmdAdd.IsEnabled = false;
                }
                if (((CheckBox)sender).IsChecked == true)
                {
                    TxtDoctorShare.IsEnabled = true;
                  //  TxtDoctorShare.Text = "";
                    clsSubSpecializationVO Obj = new clsSubSpecializationVO();
                    Obj.SpecializationId = SubSPId;
                    Obj.SubSpecializationId = ((clsSubSpecializationVO)dgSelectedSubSpecializationList.SelectedItem).SubSpecializationId;
                    Obj.SharePercentage = 0;
                    SelectedSubSpecializationList.Add(Obj);
                }
                else if (chkApplyToSubSp.IsChecked == true && txtShareToAllSubSp.Text != null)
                {
                    //chkApplyToSubSp.Visibility = Visibility.Collapsed;
                    //txtShareToAllSubSp.Visibility = Visibility.Collapsed;
                    //lblShare.Visibility = Visibility.Collapsed;
                    TxtDoctorShare.Text = "0.0";
                    TxtDoctorShare.IsEnabled = false;
                    clsSubSpecializationVO obj = (clsSubSpecializationVO)dgSelectedSubSpecializationList.SelectedItem;
                    obj = SelectedSubSpecializationList.Where(z => z.SpecializationId == obj.SpecializationId && z.SubSpecializationId == obj.SubSpecializationId).FirstOrDefault();
                    SelectedSubSpecializationList.Remove(obj);
                }
                else
                {
                    //chkApplyToSubSp.Visibility = Visibility.Collapsed;
                    //txtShareToAllSubSp.Visibility = Visibility.Collapsed;
                    //lblShare.Visibility = Visibility.Collapsed;
                    TxtDoctorShare.Text = "0.0";
                    TxtDoctorShare.IsEnabled = false;
                    clsSubSpecializationVO obj = (clsSubSpecializationVO)dgSelectedSubSpecializationList.SelectedItem;
                    obj = SelectedSubSpecializationList.Where(z => z.SpecializationId == obj.SpecializationId && z.SubSpecializationId == obj.SubSpecializationId).FirstOrDefault();
                    SelectedSubSpecializationList.Remove(obj);
                }
                DeepCopySubSpecializationList = SelectedSubSpecializationList.DeepCopy();
            }
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
        private void txtShare_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (dgSelectedSubSpecializationList.SelectedItem != null)
            {
                foreach (var item in SelectedSubSpecializationList)
                {
                    string strText = ((TextBox)sender).Text;
                    if ((!String.IsNullOrEmpty(strText)) && (!strText.IsValueDouble()))
                    {
                        TxtDoctorShare.SetValidation("Input Format is Incorrect ");
                        TxtDoctorShare.RaiseValidationError();
                        TxtDoctorShare.Focus();
                    }
                    else if (item.SubSpecializationId == ((clsSubSpecializationVO)dgSelectedSubSpecializationList.SelectedItem).SubSpecializationId)
                    {
                        item.SharePercentage = Convert.ToDouble(((TextBox)sender).Text);
                    }
                }
            }
        }

        private void chkApplyToSubSp_Click(object sender, RoutedEventArgs e)
        {
            if (chkApplyToSubSp.IsChecked == true)
            {
                txtShareToAllSubSp.IsEnabled = true;
                foreach (clsSubSpecializationVO ObjSubSpVO in SubSpecializationList)
                {
                    ObjSubSpVO.Status = true;
                    ObjSubSpVO.SharePercentage = Convert.ToDouble(txtShareToAllSubSp.Text);


                    ObjSubSpVO.SpecializationId = SubSPId;
                    SelectedSubSpecializationList.Add(ObjSubSpVO);
                }
                //SelectedSubSpecializationList = SubSpecializationList;
                dgSelectedSubSpecializationList.ItemsSource = null;
                dgSelectedSubSpecializationList.ItemsSource = SubSpecializationList.DeepCopy();
                dgSelectedSubSpecializationList.UpdateLayout();
                dgSelectedSubSpecializationList.Focus();
                cmdAdd.IsEnabled = true;
            }
            else
            {
                foreach (clsSubSpecializationVO ObjSubSpVO in SubSpecializationList)
                {
                    ObjSubSpVO.Status = false;
                    ObjSubSpVO.SharePercentage = 0;
                }
                //By Umesh.............
                SelectedSubSpecializationList = new List<clsSubSpecializationVO>();
                SelectedSubSpecializationList = DeepCopySubSpecializationList.DeepCopy();
                //......................
                SelectedSubSpecializationList = SubSpecializationList.DeepCopy();
                dgSelectedSubSpecializationList.ItemsSource = null;
                dgSelectedSubSpecializationList.ItemsSource = SubSpecializationList.DeepCopy();
                dgSelectedSubSpecializationList.UpdateLayout();
                dgSelectedSubSpecializationList.Focus();
                cmdAdd.IsEnabled = false;
                txtShareToAllSubSp.Text = "0.0";
            }
        }

        private void txtShareToAllSubSp_TextChanged(object sender, TextChangedEventArgs e)
        {
            string strText1;
            strText1 = txtShareToAllSubSp.Text;
            if ((!String.IsNullOrEmpty(strText1)))
            {
                chkApplyToSubSp.Visibility = Visibility.Visible;
            }
        }

        private void dgSelectedSubSpecializationList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
