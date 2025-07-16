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
using System.Windows.Data;
using PalashDynamics.Collections;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.MachineParameter;
using System.ComponentModel;
using PalashDynamics.Service.PalashTestServiceReference;
using System.IO;
using System.Windows.Resources;
using System.Xml.Linq;
using System.Reflection;
using CIMS;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;

namespace PalashDynamics.Administration
{
    public partial class frmmachineparameterlink : UserControl
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

        #region Variables

        public PagedSortableCollectionView<clsParameterLinkingVO> MasterList { get; private set; }
        private SwivelAnimation objAnimation;
        public bool IsCancel = true;
        public bool IsSearch = false;
        public string ParaDescription = "";
        public long MachineId = 0;
        public bool IsModify = false;
        public long SelectedItemID = 0;
        public string ModuleName { get; set; }
        public string Action { get; set; }
        public bool IsView = false;
        public long MachineParameterIDSearch = 0;
        public long AppParameterIDSearch = 0;
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
        PagedCollectionView collection;
        #endregion

        #region Constructor
        public frmmachineparameterlink()
        {
            InitializeComponent();
            this.DataContext = new clsParameterLinkingVO();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            this.Loaded += new RoutedEventHandler(Machine_ParameterLoaded);
            SetCommandButtonState("Load");
            MasterList = new PagedSortableCollectionView<clsParameterLinkingVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            this.dataGrid2Pager.DataContext = MasterList;
            grdMachineParameters.DataContext = MasterList;
            //FillApplicationParameterListForSearch();
          //  FillMachineParameterListSearch();
        }
        #endregion

        private void Machine_ParameterLoaded(object sender, RoutedEventArgs e)
        {
            PageSize = 15;
            SetupPage();
            //by rohini
            List<MasterListItem> objList = new List<MasterListItem>();
            MasterListItem objM = new MasterListItem(0, "-- Select --");
            objList.Add(objM);
            cmbMachineSearch.ItemsSource = objList;
            cmbMachineSearch.SelectedItem = objM;
            cmbParameterSearch.ItemsSource = objList;
            cmbParameterSearch.SelectedItem = objM;
            cmbMachineParameterSearch.ItemsSource = objList;
            cmbMachineParameterSearch.SelectedItem = objM;
            FillMachineSearch();
        }

        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
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

        /// <summary>
        /// Fills front panel grid
        /// </summary>
        public void SetupPage()
        {
            try
            {
                clsGetParameterLinkingBizActionVO bizActionVO = new clsGetParameterLinkingBizActionVO();

                if (IsSearch == true)
                {
                    if ((MasterListItem)cmbMachineSearch.SelectedItem != null)
                    {

                        bizActionVO.MachineID = ((MasterListItem)cmbMachineSearch.SelectedItem).ID;
                        AppParameterIDSearch = bizActionVO.ParameterID;
                    }
                    if ((MasterListItem)cmbParameterSearch.SelectedItem != null)
                    {

                        bizActionVO.ParameterID = ((MasterListItem)cmbParameterSearch.SelectedItem).ID;
                        AppParameterIDSearch = bizActionVO.ParameterID;
                    }
                    if ((MasterListItem)cmbMachineParameterSearch.SelectedItem != null)
                    {
                        bizActionVO.MachineParaID = ((MasterListItem)cmbMachineParameterSearch.SelectedItem).ID;
                        MachineParameterIDSearch = bizActionVO.MachineParaID;
                    }
                }

                bizActionVO.IsPagingEnabled = true;
                bizActionVO.MaximumRows = MasterList.PageSize;
                bizActionVO.StartIndex = MasterList.PageIndex * MasterList.PageSize;

                bizActionVO.DetailsList = new List<clsParameterLinkingVO>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        bizActionVO.DetailsList = (((clsGetParameterLinkingBizActionVO)args.Result).DetailsList);
                        MasterList.Clear();
                        if (bizActionVO.DetailsList.Count > 0)
                        {
                            MasterList.TotalItemCount = (int)(((clsGetParameterLinkingBizActionVO)args.Result).TotalRows);
                            foreach (clsParameterLinkingVO item in bizActionVO.DetailsList)
                            {
                                MasterList.Add(item);
                            }
                        }
                        grdMachineParameters.ItemsSource = null;
                        collection = new PagedCollectionView(MasterList);
                        collection.GroupDescriptions.Add(new PropertyGroupDescription("MachineName"));
                        grdMachineParameters.ItemsSource = collection;
                        dataGrid2Pager.Source = null;
                        dataGrid2Pager.PageSize = MasterList.PageSize;
                        dataGrid2Pager.Source = MasterList;
                        //FillMachineParameterListSearch();
                        //FillApplicationParameterListForSearch();
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
        #region For Search Criteria
        /// <summary>
        /// Fills Machine Master combo
        /// </summary>
        //void FillApplicationParameterListForSearch()
        //{
        //    try
        //    {
        //        clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //        BizAction.MasterTable = MasterTableNameList.M_PathoParameterMaster;
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
        //                cmbParameterSearch.ItemsSource = null;
        //                cmbParameterSearch.ItemsSource = objList;
        //                cmbParameterSearch.SelectedItem = objList[0];
        //            }
        //            if (IsSearch == true)
        //            {
        //                cmbApplicationParameterSearch.SelectedValue = AppParameterIDSearch;
        //            }
        //        };
        //        client.ProcessAsync(BizAction, new clsUserVO());
        //        client.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
        /// <summary>
        /// Fills Machine Master combo
        /// </summary>
        //void FillMachineParameterListSearch()
        //{
        //    try
        //    {
        //        clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //       // BizAction.MasterTable = MasterTableNameList.M_MachineParameterMaster;
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
        //                cmbMachineParameterSearch.ItemsSource = null;
        //                cmbMachineParameterSearch.ItemsSource = objList;
        //                cmbMachineParameterSearch.SelectedItem = objList[0];
        //            }
        //            if (IsSearch == true)
        //            {
        //                cmbMachineParameterSearch.SelectedValue = MachineParameterIDSearch;
        //            }
        //        };
        //        client.ProcessAsync(BizAction, new clsUserVO());
        //        client.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
        #endregion

#region commented by rohini dated18.1.2016
        //void FillApplicationParameterList()
        //{
        //    try
        //    {
        //        clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //        BizAction.MasterTable = MasterTableNameList.M_PathoParameterMaster;
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
        //                cmbApplicationParameter.ItemsSource = null;
        //                cmbApplicationParameter.ItemsSource = objList;
        //                cmbApplicationParameter.SelectedItem = objList[0];
        //            }
        //            if ((clsParameterLinkingVO)grdMachineParameters.SelectedItem != null && IsView == true)
        //            {
        //                cmbApplicationParameter.SelectedValue = ((clsParameterLinkingVO)grdMachineParameters.SelectedItem).ParameterID;
        //                if (chkIsFreezed.IsChecked == true)
        //                    cmbApplicationParameter.IsEnabled = false;
        //                else
        //                    cmbApplicationParameter.IsEnabled = true;
        //                //IsView = false;
        //                //cmbMachineName.SelectedValue = ((clsMachineParameterMasterVO)this.DataContext).MachineId;
        //            }
        //            else
        //            {
        //                cmbApplicationParameter.IsEnabled = true;
        //            }
        //        };
        //        client.ProcessAsync(BizAction, new clsUserVO());
        //        client.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        //void FillMachineParameterList()
        //{
        //    try
        //    {
        //        clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //       BizAction.MasterTable = MasterTableNameList.M_MachineParameterMaster;
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
        //                cmbMachineParameter.ItemsSource = null;
        //                cmbMachineParameter.ItemsSource = objList;
        //                cmbMachineParameter.SelectedItem = objList[0];
        //            }
        //            if ((clsParameterLinkingVO)grdMachineParameters.SelectedItem != null && IsView == true)
        //            {
        //                cmbMachineParameter.SelectedValue = ((clsParameterLinkingVO)grdMachineParameters.SelectedItem).MachineParameterID;
        //                if (chkIsFreezed.IsChecked == true)
        //                {
        //                    cmbMachineParameter.IsEnabled = false;
        //                }
        //                else
        //                    cmbMachineParameter.IsEnabled = true;
        //                //    IsView = false;
        //                //cmbMachineName.SelectedValue = ((clsMachineParameterMasterVO)this.DataContext).MachineId;
        //            }
        //            else
        //            {
        //                cmbMachineParameter.IsEnabled = true;
        //            }
        //        };
        //        client.ProcessAsync(BizAction, new clsUserVO());
        //        client.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
#endregion
        //added by rohini dated 18.1.16


        private void cmbMachine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //  by rohini dated 18.1.2016
            if ((MasterListItem)cmbMachine.SelectedItem != null)
            {
                if (((MasterListItem)cmbMachine.SelectedItem).ID > 0)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);                    
                    cmbApplicationParameter.ItemsSource = objList;
                    cmbApplicationParameter.SelectedItem = objM;
                    cmbMachineParameter.ItemsSource = objList;
                    cmbMachineParameter.SelectedItem = objM;
                    FillMachineParameter(((MasterListItem)cmbMachine.SelectedItem).ID);
                }
            }
          
        }
        private void cmbMachineSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbMachineSearch.SelectedItem != null)
            {
                if (((MasterListItem)cmbMachineSearch.SelectedItem).ID > 0)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    cmbParameterSearch.ItemsSource = objList;
                    cmbParameterSearch.SelectedItem = objM;
                    FillMachineParameterSearch(((MasterListItem)cmbMachineSearch.SelectedItem).ID);

                    cmbParameterSearch.SelectedValue =0;
                    cmbMachineParameterSearch.SelectedValue=0;
                }
            }
        }
        public long Mchine = 0;
        public long MParameter = 0;
        public long Parameter = 0;
        private void FillMachineSearch()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_MachineMaster;
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
                    cmbMachineSearch.ItemsSource = null;
                    cmbMachineSearch.ItemsSource = objList;  
                    cmbMachineSearch.SelectedValue = objList[0];
                  
                }                            
            
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        private void FillMachine()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_MachineMaster;
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
                    cmbMachine.ItemsSource = null;
                    cmbMachine.ItemsSource = objList;                   

                    if (Mchine != 0)
                    {
                        cmbMachine.SelectedValue = Mchine;
                    }
                    else
                    {
                        cmbMachine.SelectedValue = objList[0];                        
                    }
                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        private void FillMachineParameter(long iSupId)
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_MachineParameterMaster;
            if (iSupId > 0)
                BizAction.Parent = new KeyValue { Key = iSupId, Value = "MachineID" };
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);
                    cmbMachineParameter.ItemsSource = null;
                    cmbMachineParameter.ItemsSource = objList.DeepCopy();                   

                    if (MParameter!= 0)
                    {
                        cmbMachineParameter.SelectedValue = MParameter;                        
                    }                    
                    else
                    {
                        cmbMachineParameter.SelectedValue = objList[0];                       
                    }
                    // by rohini for 3dt combo fill
                    if (cmbMachine.SelectedItem != null && cmbMachine.SelectedValue != null)
                    {
                        if (((MasterListItem)cmbMachine.SelectedItem).ID > 0)
                        {
                            List<MasterListItem> objList1 = new List<MasterListItem>();
                            MasterListItem objM1 = new MasterListItem(0, "-- Select --");
                            objList1.Add(objM1);
                            cmbApplicationParameter.ItemsSource = objList1;
                            cmbApplicationParameter.SelectedItem = objM1;
                            FillParameter(((MasterListItem)cmbMachine.SelectedItem).ID);
                        }
                    }
                }


            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        private void FillMachineParameterSearch(long iSupId)
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_MachineParameterMaster;
            if (iSupId > 0)
                BizAction.Parent = new KeyValue { Key = iSupId, Value = "MachineID" };
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);
                    cmbMachineParameterSearch.ItemsSource = null;
                    cmbMachineParameterSearch.ItemsSource = objList.DeepCopy();
                    cmbMachineParameterSearch.SelectedValue = objList[0];
                    
                    // by rohini for 3dt combo fill
                    if (cmbMachineSearch.SelectedItem != null && cmbMachineSearch.SelectedValue != null)
                    {
                        if (((MasterListItem)cmbMachineSearch.SelectedItem).ID > 0)
                        {
                            List<MasterListItem> objList1 = new List<MasterListItem>();
                            MasterListItem objM1 = new MasterListItem(0, "-- Select --");
                            objList1.Add(objM1);                          
                            FillParameterSearch(((MasterListItem)cmbMachineSearch.SelectedItem).ID);
                        }
                    }
                }


            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        private void FillParameter(long iSupId)
        {
            clsGetParaByParaAndMachineBizActionVO BizAction = new clsGetParaByParaAndMachineBizActionVO();
            BizAction.MachineParaID = ((MasterListItem)cmbMachine.SelectedItem).ID;            
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                  clsGetParaByParaAndMachineBizActionVO BizActionObj = new clsGetParaByParaAndMachineBizActionVO();
                  //cmbApplicationParameter.ItemsSource = null;
                  //cmbApplicationParameter.ItemsSource = BizAction.DetailsList;
                 
                if (arg.Error == null)
                {
                    if (((clsGetParaByParaAndMachineBizActionVO)arg.Result).DetailsList != null)
                    {
                        BizActionObj.DetailsList = ((clsGetParaByParaAndMachineBizActionVO)arg.Result).DetailsList;
                        //List<MasterListItem> userDepartmentList = new List<MasterListItem>();                     
                        //userDepartmentList.Add(new MasterListItem(0, "-- Select --"));
                        //userDepartmentList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);
                        List<MasterListItem> userDepartmentList = new List<MasterListItem>();
                        MasterListItem objM = new MasterListItem(0, "-- Select --");
                        userDepartmentList.Add(objM);
                        cmbMachineParameterSearch.ItemsSource = null;                     
                        foreach (var item in BizActionObj.DetailsList)
                        {
                            userDepartmentList.Add(new MasterListItem() { ID = item.ID, Description = item.ParameterName});
                        } 
                        cmbApplicationParameter.ItemsSource = null;
                        cmbApplicationParameter.ItemsSource = userDepartmentList.DeepCopy();

                        if (Parameter!=0)
                        {
                            cmbApplicationParameter.SelectedValue = Parameter;
                        }                       
                        else
                        {
                            cmbApplicationParameter.SelectedValue = userDepartmentList[0]; 
                        }
                    }
                }

                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        private void FillParameterSearch(long iSupId)
        {
            clsGetParaByParaAndMachineBizActionVO BizAction = new clsGetParaByParaAndMachineBizActionVO();
            BizAction.MachineParaID = ((MasterListItem)cmbMachineSearch.SelectedItem).ID;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                clsGetParaByParaAndMachineBizActionVO BizActionObj = new clsGetParaByParaAndMachineBizActionVO();
                //cmbApplicationParameter.ItemsSource = null;
                //cmbApplicationParameter.ItemsSource = BizAction.DetailsList;

                if (arg.Error == null)
                {
                    if (((clsGetParaByParaAndMachineBizActionVO)arg.Result).DetailsList != null)
                    {
                        BizActionObj.DetailsList = ((clsGetParaByParaAndMachineBizActionVO)arg.Result).DetailsList;
                        List<MasterListItem> userDepartmentList = new List<MasterListItem>();

                        MasterListItem objM = new MasterListItem(0, "-- Select --");
                        userDepartmentList.Add(objM);
                        foreach (var item in BizActionObj.DetailsList)
                        {
                            userDepartmentList.Add(new MasterListItem() { ID = item.ID, Description = item.ParameterName });
                        }
                        cmbParameterSearch.ItemsSource = null;
                        cmbParameterSearch.ItemsSource = userDepartmentList.DeepCopy();                       
                        cmbParameterSearch.SelectedValue = 0;                        
                    }
                }

                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            this.grdBackPanel.DataContext = new clsParameterLinkingVO();
            ClearUI();
            
            IsView = false;
            IsModify = false;
            IsSearch = false;
            //FillMachineParameterList();
            //FillApplicationParameterList();
            //by rohini dated 19.1.2016
            List<MasterListItem> objList = new List<MasterListItem>();
            MasterListItem objM = new MasterListItem(0, "-- Select --");
            objList.Add(objM);
            cmbMachine.ItemsSource = objList;
            cmbMachine.SelectedItem = objM;
            cmbApplicationParameter.ItemsSource = objList;
            cmbApplicationParameter.SelectedItem = objM;
            cmbMachineParameter.ItemsSource = objList;
            cmbMachineParameter.SelectedItem = objM;           
            FillMachine();
            Validation();
            cmbMachine.Focus();
            //
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

        /// <summary>
        /// clears UI
        /// </summary>
        public void ClearUI()
        {
            this.grdBackPanel.DataContext = new clsParameterLinkingVO();
            chkIsFreezed.IsChecked = false;
            chkIsFreezed.IsEnabled = true;
            cmbMachineParameter.IsEnabled = true;
            cmbApplicationParameter.IsEnabled = true;
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                IsModify = false;
                string msgTitle = "";
                string msgText = "Are you sure you want to Save ?";

                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosed);

                msgWin.Show();
            }
        }

        void msgWin_OnMessageBoxClosed(MessageBoxResult result)
        {
            try
            {
                if (result == MessageBoxResult.Yes)
                {
                    SaveDetails();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void SaveDetails()
        {
            clsAddUpdateParameterLinkingBizActionVO objDetails = new clsAddUpdateParameterLinkingBizActionVO();
            try
            {
                clsParameterLinkingVO objParameter = new clsParameterLinkingVO();
                objParameter = (clsParameterLinkingVO)this.DataContext;
                objParameter = CreateFormData();

                objParameter.CreatedUnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                //addNewBlockVO.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                objParameter.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                objParameter.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                objParameter.AddedDateTime = System.DateTime.Now;
                objParameter.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;

                objDetails.Details = objParameter;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (((clsAddUpdateParameterLinkingBizActionVO)args.Result).SuccessStatus == 1)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("", "Record is successfully submitted!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                            //After Insertion Back to BackPanel and Setup Page
                            SelectedItemID = 0;
                            SetupPage();
                            if (IsView == true)
                                IsView = false;
                            objAnimation.Invoke(RotationType.Backward);
                            SetCommandButtonState("Save");
                        }
                        else if (((clsAddUpdateParameterLinkingBizActionVO)args.Result).SuccessStatus == 3)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be save because CODE already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindow.Show();
                        }
                        else if (((clsAddUpdateParameterLinkingBizActionVO)args.Result).SuccessStatus == 2)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("", "Application parameter is already linked with existing Machine parameter  !", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindow.Show();
                        }
                    }
                    else
                    {
                        if (args.Error != null)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("", "Error occured while saving.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindow.Show();
                        }
                        else if (args.Result == null)
                        {
                        }

                    }
                };
                client.ProcessAsync(objDetails, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private clsParameterLinkingVO CreateFormData()
        {
            clsParameterLinkingVO objParameter = new clsParameterLinkingVO();
            if (IsModify == true)
            {
                objParameter.ID = SelectedItemID;
                objParameter.UnitId = (((clsParameterLinkingVO)grdMachineParameters.SelectedItem).UnitId);
            }
            else
            {
                objParameter.ID = 0;
                objParameter.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }

            objParameter.Freezed = chkIsFreezed.IsChecked.Value;
            objParameter.Status = true;
            objParameter.MachineParameterID = ((MasterListItem)cmbMachineParameter.SelectedItem).ID;
            objParameter.ParameterID = ((MasterListItem)cmbApplicationParameter.SelectedItem).ID;
            //by rohini dated 18.1.2016
            objParameter.MachineID = ((MasterListItem)cmbMachine.SelectedItem).ID;
            return objParameter;
        }

        #region Validation
        public bool Validation()
        {
            bool result = true;
            if (((MasterListItem)cmbMachineParameter.SelectedItem).ID <= 0)
            {
                cmbMachineParameter.TextBox.SetValidation("Please Select Machine Parameter");
                cmbMachineParameter.TextBox.RaiseValidationError();
                cmbMachineParameter.TextBox.Focus();
                result = false;
            }
            else
            {
                cmbMachineParameter.TextBox.ClearValidationError();
               
            }
             if (((MasterListItem)cmbApplicationParameter.SelectedItem).ID <= 0)
            {
                cmbApplicationParameter.TextBox.SetValidation("Please Select Parameter");
                cmbApplicationParameter.TextBox.RaiseValidationError();
                cmbApplicationParameter.TextBox.Focus();
                result = false;
            }
            else
            {
                cmbApplicationParameter.ClearValidationError();
                
            }
             if (((MasterListItem)cmbMachine.SelectedItem).ID <= 0)
             {
                 cmbMachine.TextBox.SetValidation("Please Select Machine");
                 cmbMachine.TextBox.RaiseValidationError();
                 cmbMachine.TextBox.Focus();
                 result = false;
             }
             else
             {
                 cmbMachine.TextBox.ClearValidationError();
               
             }
             return result;
        }

        #endregion
        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Validation())
                {
                    IsModify = true;
                    IsSearch = false;
                    string msgTitle = "";
                    string msgText = "Are you sure you want to Update ?";

                    MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosed);
                    msgW.Show();
                }
            }
            catch (Exception Ex)
            {
                throw;
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetCommandButtonState("Cancel");
                IsSearch = false;
                Mchine = 0;
                MParameter = 0;
                Parameter = 0;

                cmbMachine.SelectedValue = 0;
                cmbMachineSearch.SelectedValue = 0;
                cmbParameterSearch.SelectedValue = 0;
                cmbApplicationParameter.SelectedValue = 0;
                cmbMachineParameterSearch.SelectedValue = 0;
                cmbMachineParameter.SelectedValue = 0;
                objAnimation.Invoke(RotationType.Backward);
                if (IsCancel == true)
                {
                    ModuleName = "PalashDynamics.Administration";
                    Action = "PalashDynamics.Administration.frmPathologyConfiguration";
                    UserControl rootPage = Application.Current.RootVisual as UserControl;

                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                    mElement.Text = "Pathology Configuration";

                    WebClient c = new WebClient();
                    c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
                    c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
                }
                else
                {
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                    mElement.Text = "  ";
                    IsCancel = true;
                    SetupPage();
                    if (IsView == true)
                        IsView = false;
                }

              
                
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// When cancel button click on front panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            IsSearch = true;
            SetupPage();
        }

        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetCommandButtonState("View");
               // Validation();
                if (grdMachineParameters.SelectedItem != null)
                {
                    SelectedItemID = ((clsParameterLinkingVO)grdMachineParameters.SelectedItem).ID;
                    grdBackPanel.DataContext = ((clsParameterLinkingVO)grdMachineParameters.SelectedItem).DeepCopy();
                    //--------by rohini dated 19.1.16----
                    Mchine = ((clsParameterLinkingVO)grdMachineParameters.SelectedItem).MachineID;
                    MParameter = ((clsParameterLinkingVO)grdMachineParameters.SelectedItem).MachineParameterID;
                    Parameter = ((clsParameterLinkingVO)grdMachineParameters.SelectedItem).ParameterID;
                   
                    if (Mchine != 0)
                    {
                        FillMachine();
                    }
                    else
                    {
                    cmbMachine.SelectedValue = Mchine;
                    cmbApplicationParameter.SelectedValue = Parameter;
                    cmbMachineParameter.SelectedValue = MParameter;

                    }
                    chkIsFreezed.IsChecked = ((clsParameterLinkingVO)grdMachineParameters.SelectedItem).Freezed;
                    IsView = true;
                    if (chkIsFreezed.IsChecked == true)
                    {                                     
                        chkIsFreezed.IsEnabled = false;
                        cmdModify.IsEnabled = false;
                    }
                    else
                    {
                        chkIsFreezed.IsEnabled = true;
                        cmbMachineParameter.IsEnabled = true;
                        cmbApplicationParameter.IsEnabled = true;                        
                    }                  
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
            clsUpdateStatusParameterLinkingBizActionVO objStatus = new clsUpdateStatusParameterLinkingBizActionVO();
            objStatus.ID = ((clsParameterLinkingVO)grdMachineParameters.SelectedItem).ID;
            objStatus.UnitID = ((clsParameterLinkingVO)grdMachineParameters.SelectedItem).UnitId;
            objStatus.MachineParameterID = ((clsParameterLinkingVO)grdMachineParameters.SelectedItem).MachineParameterID;
            objStatus.AppParameterID = ((clsParameterLinkingVO)grdMachineParameters.SelectedItem).ParameterID;
            objStatus.Status = ((clsParameterLinkingVO)grdMachineParameters.SelectedItem).Status;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    if (objStatus.Status == false)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("", "Status Deactivated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWindow.Show();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("", "Status Activated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWindow.Show();
                    }
                }
                SetCommandButtonState("Cancel");
            };
            client.ProcessAsync(objStatus, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

       

    }
}
