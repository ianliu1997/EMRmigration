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
using System.ComponentModel;
using PalashDynamics.Animations;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Administration.Agency;
using CIMS;
using System.Reflection;
using System.Windows.Data;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.UserControls;

namespace PalashDynamics.Administration.Agency
{
    public partial class frmAgencyClinicLinking : UserControl
    {

        #region Variable And List Declaration
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public PagedSortableCollectionView<clsAgencyClinicLinkVO> MasterList { get; private set; }
        private List<clsAgencyMasterVO> AgencyList;
        private List<clsAgencyMasterVO> SelectedAgencyList;
        private List<clsAgencyMasterVO> ModifyAgencyList;
        private List<clsAgencyMasterVO> ChkDefaultAgencyList;
        private List<clsAgencyClinicLinkVO> AgencyClinicLinkList;
        private SwivelAnimation objAnimation;
     
        PagedCollectionView collection;
        private bool IsCancel = true;
        private string msgText = String.Empty;
        private string msgTitle = String.Empty;
        private bool IsModify = false;
        private long SelectedClinicID = 0;
           MasterListItem obj;
        public int DataListPageSize
        {
            get
            {
                return MasterList.PageSize;
            }
            set
            {
                if (value == MasterList.PageSize) return;
                MasterList.PageSize = value;
            }
        }
        public bool IsNewLinking = false;
        #endregion
        public frmAgencyClinicLinking()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            SetCommandButtonState("Load");
            FillClinic();
          //  FillAgency();
            /*==========================================================*/
            MasterList = new PagedSortableCollectionView<clsAgencyClinicLinkVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            DataListPageSize = 15;

            //dgDataPager.PageSize = DataListPageSize;
            //dgDataPager.Source = MasterList;

            //this.dgDataPager.DataContext = MasterList;
            //dgAgencyClinicList.DataContext = MasterList;
            dgAgencyClinicList1.DataContext = MasterList;
            /*==========================================================*/
            AgencyList = new List<clsAgencyMasterVO>();
            SelectedAgencyList = new List<clsAgencyMasterVO>();
            ModifyAgencyList = new List<clsAgencyMasterVO>();
           // objMasterList = new List<MasterListItem>();
        }


        private void frmAgencyClinicLink_Loaded(object sender, RoutedEventArgs e)
        {
           // FillAgencyClinicLinkList();
            FillAgencyClinicLinkList1();
        }

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("New");
            AgencyList = new List<clsAgencyMasterVO>();
            SelectedAgencyList.Clear();
            ModifyAgencyList = new List<clsAgencyMasterVO>();
            FillAgency();

           
            ClearUI();
            try
            {
                objAnimation.Invoke(RotationType.Forward);
                IsModify = false;
            }
            catch (Exception)
            {
                throw;
            }

        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            var item3 = from r1 in MasterList
                        where r1.UnitID == ((MasterListItem)cmbClinic1.SelectedItem).ID
                        select r1;
            //}
            if (CheckValidation())
            {
                if (item3 != null && item3.ToList().Count > 0)
                {
                    string msgTitle = "Palash";
                    string msgText = "Linking with " + "'" + ((MasterListItem)cmbClinic1.SelectedItem).Description + "'" + "is already done.";
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        try
                        {
                            objAnimation.Invoke(RotationType.Forward);
                            
                            FillAgencyClinicLinkList();

                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    };
                    msgW1.Show();
                }
                else if (item3 != null && item3.ToList().Count == 0)
                {
                    string msgTitle = "Palash";
                    string msgText = "Are you sure you want to Save the Agency Clinic Linking.?";
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            SaveAgencyClinicLink();
                        }
                    };
                    msgW1.Show();
                }
            }
        }

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            if (CheckValidation())
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to Modify Agency Clinic Linking.?";
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        SaveAgencyClinicLink();
                        SetCommandButtonState("Load");
                    }
                };
                msgW1.Show();
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Cancel");
            objAnimation.Invoke(RotationType.Backward);
            ClearUI();
            if (IsCancel == true)
            {
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Clinic Configuration";
                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.Agency.frmAgencyConfiguration") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
            }
            else
            {
                //
                ModifyAgencyList = new List<clsAgencyMasterVO>();
                SelectedAgencyList.Clear();
                FillAgencyClinicLinkList1();
                IsCancel = true;
            }
            chkDefault = 0;
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FindClinic();
        
        }
      
        private void cmbClinic1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (dgAgencyClinicList1.SelectedItem != null)
            //{
            //    try
            //    {
            //        IsModify = true;
            //        FillClinic();
            //        SetCommandButtonState("View");
            //        FillAgency();
            //        ModifyAgencyList = new List<clsAgencyMasterVO>();
            //        clsAgencyClinicLinkVO obj = (clsAgencyClinicLinkVO)cmbClinic1.SelectedItem;
            //        objAnimation.Invoke(RotationType.Forward);
            //    }
            //    catch (Exception)
            //    {
            //        throw;
            //    }
            //}
        }
        private void cmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           // FillAgencyClinicLinkList();
           // FillClinic();

        }

        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            if (dgAgencyClinicList1.SelectedItem != null)
            {
                try
                {
                    IsModify = true;
                    FillClinic();
                    SetCommandButtonState("View");
                    FillAgency();
                    ModifyAgencyList = new List<clsAgencyMasterVO>();
                    SelectedAgencyList.Clear();
                    clsAgencyClinicLinkVO obj = (clsAgencyClinicLinkVO)dgAgencyClinicList1.SelectedItem;
                    objAnimation.Invoke(RotationType.Forward);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private void chkStatus_Click(object sender, RoutedEventArgs e)
        {

        }
         private List<clsServiceMasterVO> _OtherServiceSelected;
        public List<clsServiceMasterVO> SelectedOtherServices { get { return _OtherServiceSelected; } }
        private void chkAgency_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            if (dgAgency.SelectedItem != null)
             {
                 List<MasterListItem> List = (List<MasterListItem>)dgAgency.ItemsSource;
                 clsAgencyMasterVO objVO = new clsAgencyMasterVO { ID = ((MasterListItem)dgAgency.SelectedItem).ID };
                 //CheckBox chk = sender as CheckBox;
                 if (chk.IsChecked == true)
                 {   
                     if (IsModify == true && dgAgency.SelectedItem != null)
                     {                      
                        objVO.Status = true;
                        ModifyAgencyList.Add(objVO);

                     }
                     else
                     {
                        AgencyList.Add(objVO);
                     }
                    if (List != null && List.Count > 0)
                    {
                        foreach (MasterListItem item in List)
                        {
                            if (item.ID == ((MasterListItem)dgAgency.SelectedItem).ID && item.Status == true)
                            {
                                item.isChecked = true;
                            }
                        }
                        dgAgency.ItemsSource = null;
                        dgAgency.ItemsSource = List;
                    }
                  }
                  else
                  {
                      WaitIndicator Indicatior = new WaitIndicator();
                      Indicatior.Show();
                    clsGetServiceToAgencyAssignedBizActionVO bizAction = new clsGetServiceToAgencyAssignedBizActionVO();
                    bizAction.ServiceDetails = new clsServiceMasterVO();
                    bizAction.ServiceList = new List<clsServiceMasterVO>();
                    bizAction.IsAgencyServiceLinkView = true;
                    if (dgAgency.SelectedItem != null)
                    {
                        bizAction.UnitID = ((MasterListItem)dgAgencyClinicList1.SelectedItem).ID;
                        bizAction.AgencyID = ((MasterListItem)dgAgency.SelectedItem).ID;
                    }
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        List<clsServiceMasterVO> result = ((clsGetServiceToAgencyAssignedBizActionVO)args.Result).ServiceList;
                        if (((clsGetServiceToAgencyAssignedBizActionVO)args.Result).ServiceDetails.AssignedAgencyID != 0 || ((clsGetServiceToAgencyAssignedBizActionVO)args.Result).ServiceDetails.AssignedAgencyID > 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Service is already assigned to Agency.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();

                            chk.IsChecked = true;
                            Indicatior.Close();
                        }
                        else
                        {

                            clsAgencyMasterVO obj;
                            if (IsModify == true && dgAgency.SelectedItem != null)
                            {
                                //obj = SelectedAgencyList.Where(z => z.ID == objVO.ID).FirstOrDefault();
                                obj = ModifyAgencyList.Where(z => z.ID == objVO.ID).FirstOrDefault();
                                ModifyAgencyList.Remove(obj);
                            }
                            else
                            {
                                obj = AgencyList.Where(z => z.ID == objVO.ID).FirstOrDefault();
                                AgencyList.Remove(obj);
                            }
                            if (List != null && List.Count > 0)
                            {
                                foreach (MasterListItem item in List)
                                {
                                    if (item.ID == ((MasterListItem)dgAgency.SelectedItem).ID && item.Status == false)
                                    {
                                        item.isChecked = false;
                                        item.IsDefault = false;
                                        chkDefault = 0;
                                    }
                                }
                                dgAgency.ItemsSource = null;
                                dgAgency.ItemsSource = List;
                            }
                            Indicatior.Close();
                        }
                    };
                    client.ProcessAsync(bizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }                                 
             }
               
        }
        int chkDefault=0;
        bool chkFlag = false;
        private void chkDefault_Click(object sender, RoutedEventArgs e)
        {
           // FillAgency();
            //FillAgencyClinicLinkList();

            //if (dgItemBatches.SelectedItem != null)
            //{
            //    CheckBox chk = (CheckBox)sender;

            //    if (chk.IsChecked == true)
            //    {
            //        _BatchSelected.Add((clsItemStockVO)dgItemBatches.SelectedItem);
            //        dgItemBatches.CurrentColumn.IsReadOnly = true;
            //        foreach (var item in BatchList.ToList())
            //        {
            //            if (item.BatchID != ((clsItemStockVO)dgItemBatches.SelectedItem).BatchID)
            //            {
            //                item.IsBatchEnabled = false;
            //            }
            //            else
            //            {
            //                item.IsBatchEnabled = true;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        _BatchSelected.Remove((clsItemStockVO)dgItemBatches.SelectedItem);
            //        foreach (var item in BatchList.ToList())
            //        {
            //            item.IsBatchEnabled = true;
            //        }
            //    }
            //    dgItemBatches.ItemsSource = null;
            //    dgAgency.ItemsSource = BatchList;
            //}
            if (((CheckBox)sender).IsChecked == true)
            {

                if (IsModify == true && dgAgency.SelectedItem != null)
                {
                  
                    if (ModifyAgencyList != null && ModifyAgencyList.Count > 0)
                    {
                        foreach (var item in ModifyAgencyList)
                        {

                            if (item.IsDefault == true && chkDefault == 0)
                            {                             
                                item.IsDefault = false;
                            
                            }
                            else if (item.ID == ((MasterListItem)dgAgency.SelectedItem).ID && chkDefault == 0)
                            {
                                item.IsDefault = true;                               
                            }                           
                        }
                        chkDefault = chkDefault + 1;

                    }
                  
                }

                else
                {
                    if (AgencyList != null && AgencyList.Count > 0)
                    {
                        foreach (var item in AgencyList)
                        {
                            if (item.IsDefault == true)
                            {
                                chkFlag = true;
                                item.IsDefault = false;
                                chkDefault = 0;
                            }
                            else if (item.ID == ((MasterListItem)dgAgency.SelectedItem).ID && chkDefault == 0)
                            {                        
                                item.IsDefault = true;
                                chkFlag = false;
                                chkDefault = 0;
                            }
                        }
                    }
                }
                if (chkDefault > 1)
                {
                    ((CheckBox)sender).IsChecked = false;
                    //chkDefault = 0;
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select only one default agency.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                   
                    
                }
            }
            else
            {
                if (IsModify == true && dgAgency.SelectedItem != null)
                {
                    if (ModifyAgencyList != null )//&& AgencyList.Count > 0)
                    {
                        foreach (var item in ModifyAgencyList)
                        {
                            if (item.ID == ((MasterListItem)dgAgency.SelectedItem).ID)
                            {
                                item.IsDefault = false;
                                chkDefault = 0;
                            }
                        }
                    }
                }
                else
                {
                    if (AgencyList != null && AgencyList.Count > 0)
                    {
                        foreach (var item in AgencyList)
                        {
                            if (item.ID == ((MasterListItem)dgAgency.SelectedItem).ID)
                            {
                                item.IsDefault = false;
                                chkDefault = 0;
                            }
                        }
                    }
                }
            }

            

        }

       
        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillAgencyClinicLinkList();
        }

        private void FillClinic()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
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
                        cmbClinic.ItemsSource = null;
                        cmbClinic1.ItemsSource = null;
                        cmbClinic.ItemsSource = objList;
                        cmbClinic.SelectedItem = objList[0];
                        if (dgAgencyClinicList1.SelectedItem != null && IsModify == true)
                        {
                            cmbClinic1.SelectedItem = objList.Where(z => z.ID == ((MasterListItem)dgAgencyClinicList1.SelectedItem).ID).FirstOrDefault();
                        }
                        else
                        {
                            cmbClinic1.SelectedItem = objList[0];
                        }
                        cmbClinic1.ItemsSource = objList;
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

       private void FillAgency()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_PathAgencyMaster;
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

                        //clsGetAgencyClinicLinkBizActionVO bizActionObj = new clsGetAgencyClinicLinkBizActionVO();
                        //bizActionObj.AgencyClinicLinkDetails = new clsAgencyClinicLinkVO();

                        dgAgency.ItemsSource = null;
                        if (dgAgencyClinicList1.SelectedItem != null && IsModify == true)
                        {
                           
                                foreach (clsAgencyMasterVO item1 in SelectedAgencyList)
                                { 
                                    foreach (var item in objList)
                                    {
                                    //if (item.ID == item1.ID &&)
                                   // {
                                    if (item1.ApplicableUnitID ==((MasterListItem)dgAgencyClinicList1.SelectedItem).ID && item1.ID == item.ID)
                                    {
                                        item.Status = true;
                                        item.isChecked = true;
                                        ModifyAgencyList.Add(item1);
                                    }
                                    if (item1.IsDefault == true && item1.ApplicableUnitID == ((MasterListItem)dgAgencyClinicList1.SelectedItem).ID && item.ID == item1.ID)
                                    {
                                        item.IsDefault = true;
                                        chkDefault = 1;

                                    }
                                }
                            }
                           
                           
                           // objMasterList = objList;
                        }
                        dgAgency.ItemsSource = objList;
                        dgAgency.UpdateLayout();
                        //else
                        //{
                        //    foreach (MasterListItem item in objList)
                        //    {
                        //        item.isChecked = false;
                        //    }
                        //    dgAgency.ItemsSource = objList;
                        //    objMasterList = objList;
                        //}
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

        private void ClearUI()
        {
            AgencyList = new List<clsAgencyMasterVO>();
            cmbClinic1.SelectedValue = (long)0;
            chkDefault = 0;
        }

        private bool CheckValidation()
        {
            bool IsValidate = true;
            ChkDefaultAgencyList=new List<clsAgencyMasterVO>();
            if (cmbClinic1.SelectedItem != null && ((MasterListItem)cmbClinic1.SelectedItem).ID == 0)
            {
                cmbClinic1.TextBox.SetValidation("Please, Select Clinic.");
                cmbClinic1.TextBox.RaiseValidationError();
                cmbClinic1.TextBox.Focus();
                IsValidate = false;
            }
            if (IsModify == true)
            {
                if (ModifyAgencyList != null && ModifyAgencyList.ToList().Count == 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select At Least One Agency To Link With Clinic.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                  
                    IsValidate = false;
                }
                //added by rohini dated 17.3.16 as per disscuss with dr priyanka
                if (ModifyAgencyList != null )
                {
                    foreach (var item in ModifyAgencyList)
                    {
                        if (item.IsDefault == true)
                        {
                            ChkDefaultAgencyList.Add(item);
                        }                       
                    }
                    if (ChkDefaultAgencyList.Count == 0)
                    {
                        //first clic save without default check then check default gives error 
                        chkDefault = 0;
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select At Least One Default Agency To Link With Clinic.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        IsValidate = false;                       
                        ChkDefaultAgencyList = null;
                    }
                }
                //
            }
            else
            {
                if (AgencyList != null && AgencyList.ToList().Count == 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select At Least One Agency To Link With Clinic.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                    IsValidate = false;
                }

                //added by rohini dated 17.3.16 as per disscuss with dr priyanka
                if (AgencyList != null)
                {
                    foreach (var item in AgencyList)
                    {
                        if (item.IsDefault == true)
                        {
                            ChkDefaultAgencyList.Add(item);
                        }
                    }
                    if (ChkDefaultAgencyList.Count == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select At Least One Default Agency To Link With Clinic.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        IsValidate = false;
                        ChkDefaultAgencyList = null;
                    }
                }
                //
            }
          
            return IsValidate;
        }

        private void SaveAgencyClinicLink()
        {
            WaitIndicator Indicatior = new WaitIndicator();
          
            try
            {
             //   ModifyAgencyList.Clear();
                Indicatior.Show();
                clsAddAgencyClinicLinkBizActionVO BizAction = new clsAddAgencyClinicLinkBizActionVO();
                BizAction.AgencyClinicLinkDetails = new clsAgencyClinicLinkVO();
                if (cmbClinic1.SelectedItem != null && ((MasterListItem)cmbClinic1.SelectedItem).ID != 0)
                {
                    BizAction.AgencyClinicLinkDetails.UnitID= ((MasterListItem)cmbClinic1.SelectedItem).ID;
                }
                if (dgAgencyClinicList1.SelectedItem != null && IsModify == true)
                {
                    BizAction.AgencyMasterList = ModifyAgencyList;
                    BizAction.AgencyClinicLinkDetails.IsModifyLink = true;
                }
                else if (AgencyList != null && AgencyList.Count > 0)
                {
                    BizAction.AgencyMasterList = AgencyList;
                }
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("PALASH", "Record is updated successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWindow.Show();
                        msgWindow.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            objAnimation.Invoke(RotationType.Backward);
                            FillAgencyClinicLinkList1();
                            SetCommandButtonState("Save");
                            Indicatior.Close();
                        };
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
                Indicatior.Close();
            }
            return;
        }

          private void FindClinic()
        {
            try
            {

                clsGetClinicMasterListBizActionVO bizActionObj = new clsGetClinicMasterListBizActionVO();
                bizActionObj.ClinicDetails = new clsAgencyMasterVO(); ;
              

                //bizActionObj.ClinicDetails = new List<clsAgencyMasterVO>();
                if (cmbClinic.SelectedItem != null && ((MasterListItem)cmbClinic.SelectedItem).ID > 0)
                {

                    bizActionObj.ClinicDetails.ID = ((MasterListItem)cmbClinic.SelectedItem).ID;
                    bizActionObj.ClinicDetails.Description = ((MasterListItem)cmbClinic.SelectedItem).Description;

                }
                else
                {
                    bizActionObj.ClinicDetails.ID = 0;
                }

                bizActionObj.PagingEnabled = true;
              
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        clsGetClinicMasterListBizActionVO result = args.Result as clsGetClinicMasterListBizActionVO;
                    
                        dgAgencyClinicList1.ItemsSource = null;
                        dgAgencyClinicList1.ItemsSource = ((clsGetClinicMasterListBizActionVO)args.Result).MasterList;
                     
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occurred While Processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(bizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {

            }
            return;

        }
        private void FillAgencyClinicLinkList()
        {
            WaitIndicator Indicatior = new WaitIndicator();
            try
            {
                Indicatior.Show();
                clsGetAgencyClinicLinkBizActionVO bizActionObj = new clsGetAgencyClinicLinkBizActionVO();
                bizActionObj.AgencyClinicLinkDetails = new clsAgencyClinicLinkVO();
                bizActionObj.AgencyMasterList = new List<clsAgencyMasterVO>();
                if (cmbClinic.SelectedItem != null && ((MasterListItem)cmbClinic.SelectedItem).ID > 0)
                {
              
                    bizActionObj.AgencyClinicLinkDetails.UnitID = ((MasterListItem)cmbClinic.SelectedItem).ID;
                }
                else
                {
                    bizActionObj.AgencyClinicLinkDetails.UnitID = 0;
                }

                if (dgAgencyClinicList1.SelectedItem != null)
                {
                    bizActionObj.AgencyClinicLinkDetails.UnitID = ((MasterListItem)dgAgencyClinicList1.SelectedItem).ID;
                }
                bizActionObj.PagingEnabled = true;
                bizActionObj.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;
                bizActionObj.MaximumRows = MasterList.PageSize;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        clsGetAgencyClinicLinkBizActionVO result = args.Result as clsGetAgencyClinicLinkBizActionVO;
                        MasterList.Clear();
                        SelectedAgencyList.Clear();
                        SelectedAgencyList = result.AgencyMasterList;
                        //if (result.AgencyMasterList != null)
                        //{
                        //    MasterList.TotalItemCount = (int)((clsGetAgencyClinicLinkBizActionVO)args.Result).TotalRows;
                        //    foreach (clsAgencyClinicLinkVO item in result.AgencyClinicLinkList)
                        //    {
                        //        MasterList.Add(item);
                        //    }
                        //    //dgAgencyClinicList.ItemsSource = null;
                        //    //collection = new PagedCollectionView(MasterList);
                        //    //collection.GroupDescriptions.Add(new PropertyGroupDescription("ClinicName"));
                        //    //dgAgencyClinicList.ItemsSource = collection;
                        //    //dgDataPager.Source = null;
                        //    //dgDataPager.PageSize = MasterList.PageSize;
                        //    //dgDataPager.Source = MasterList;
                        //    if (MasterList.Count == 0)
                        //    {
                        //        IsNewLinking = true;
                        //    }
                        //}
                        Indicatior.Close();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured While Processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                        Indicatior.Close();
                    }
                    FillAgency();
                };
                client.ProcessAsync(bizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {

            }
            return;
        }

        private void FillAgencyClinicLinkList1()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
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

                        //objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                      
                        dgAgencyClinicList1.ItemsSource = objList;
                        //dgAgencyClinicList1.SelectedItem = objList[0];
                        //if (dgAgencyClinicList.SelectedItem != null && IsModify == true)
                        //{
                        //    dgAgencyClinicList1.SelectedItem = objList.Where(z => z.ID == ((clsAgencyClinicLinkVO)dgAgencyClinicList.SelectedItem).UnitID).FirstOrDefault();
                        //}
                        //else
                        //{
                        //    dgAgencyClinicList1.SelectedItem = objList[0];
                        //}
                        //dgAgencyClinicList1.ItemsSource = objList;
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
                    IsModify = false;
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
                case "View":
                    cmdModify.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    IsModify = true;
                    break;
                case "View1":
                    cmdModify.IsEnabled = false;
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

        private void cmdViewClinic_Click(object sender, RoutedEventArgs e)
        {
            

            //if (dgAgencyClinicList1.SelectedItem != null)
            //{
            //    try
            //    {
            //        IsModify = true;
            //     //   FillClinic();
            //        SetCommandButtonState("View");
                   
            //        ModifyAgencyList = new List<clsAgencyMasterVO>();
            //        FillAgencyClinicLinkList1();
            //        FillAgency();
            //        //clsAgencyClinicLinkVO obj = (clsAgencyClinicLinkVO)dgAgencyClinicList1.SelectedItem;
            //        clsAgencyClinicLinkVO obj = ((MasterListItem)dgAgencyClinicList1.SelectedItem).ID;
            //        objAnimation.Invoke(RotationType.Forward);
            //    }
            //    catch (Exception)
            //    {
            //        throw;
            //    }
            //}

      
            cmbClinic1.IsEnabled = false;
            if (dgAgencyClinicList1.SelectedItem != null)
            {
                try
                {
                    IsModify = true;
                    ModifyAgencyList = new List<clsAgencyMasterVO>();
                    SetCommandButtonState("View");
                    FillClinic();
                    FillAgencyClinicLinkList();

                 
                   
                    //clsAgencyClinicLinkVO obj = (clsAgencyClinicLinkVO)dgAgencyClinicList1.SelectedItem;
                    objAnimation.Invoke(RotationType.Forward);
                }
                catch (Exception)
                {
                    throw;
                }
            }

        }

       

    }
}
