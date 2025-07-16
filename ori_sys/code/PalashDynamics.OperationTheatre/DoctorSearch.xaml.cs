using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects.Administration.OTConfiguration;
using PalashDynamics.Collections;
//using PalashDynamics.ValueObjects.RSIJ;
using PalashDynamics.ValueObjects.OutPatientDepartment.QueueManagement;
using System.Collections.ObjectModel;
using System.Text;
using PalashDynamics.ValueObjects.RSIJ;

namespace PalashDynamics.OperationTheatre
{
    public partial class DoctorSearch : ChildWindow
    {
        #region Variable Declaration
        public event RoutedEventHandler OnAddButton_Click;
        public PagedSortableCollectionView<MasterListItem> DoctorList { get; set; }
        public ObservableCollection<MasterListItem> AddedDoctorList = new ObservableCollection<MasterListItem>();
        //public ObservableObjectCollection<MasterListItem> AddedDoctorList = new PagedSortableCollectionView<MasterListItem>();
        bool UseAppDoctorID = true;
        #endregion

        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion

        #region Constructor and Loaded
        public DoctorSearch()
        {
            InitializeComponent();
            DoctorList = new PagedSortableCollectionView<MasterListItem>();
            DataListPageSize = 15;
            DoctorList.PageSize = DataListPageSize;
        }

        public int DataListPageSize
        {
            get
            {
                return DoctorList.PageSize;
            }
            set
            {
                if (value == DoctorList.PageSize) return;
                DoctorList.PageSize = value;
            }
        }

        /// <summary>
        /// child window loaded
        /// </summary>
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //FillDepartmentList();
            FetchDoctorClassification();
        }
        #endregion

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        #region Button Click Events
        /// <summary>
        /// Ok button click
        /// </summary>
        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {
            string msgText;
            try
            {
                //if (DoctorList.Where(D => D.Status.Equals(true)).ToList().Count > 0)
                if (AddedDoctorList.Where(D => D.Status.Equals(true)).ToList().Count > 0)
                {
                    if (OnAddButton_Click != null)
                    {
                        this.DialogResult = true;
                        //DoctorList.Where(z => z.Status == true);
                        OnAddButton_Click(this, new RoutedEventArgs());
                        this.Close();
                    }
                }
                else
                {
                    //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                    //{
                    //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("DocValidation_Msg");
                    //}
                    //else
                    //{
                        msgText = "Please select doctor.";
                    //}
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        /// <summary>
        /// Search button click
        /// </summary>
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            FillDoctor(((MasterListItem)cmbDepartment.SelectedItem).Code);
            //FetchData();
        }
        #endregion

        #region Fetched Methods

        private void FetchDoctorClassification()
        {
            try
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
                        objList.Add(new MasterListItem("0", "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        objList.ForEach(z => z.Code = z.ID.ToString());
                        cmbDepartment.ItemsSource = null;
                        cmbDepartment.ItemsSource = objList;
                        cmbDepartment.SelectedItem = objList[0];
                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //private void FetchDoctorClassification()
        //{
        //    clsGetRSIJMasterListBizActionVO BizAction = new clsGetRSIJMasterListBizActionVO();
        //    BizAction.DescriptionColumn = "NMSPESIAL";
        //    BizAction.MasterList = new List<MasterListItem>();
        //    BizAction.MasterTable = MasterTableNameList.SPESIAL;
        //    BizAction.CodeColumn = "KDSPESIAL";

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //    client.ProcessCompleted += (s, arg) =>
        //    {
        //        if (arg.Error == null && arg.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();
        //            objList.Add(new MasterListItem("0", "-- Select --"));
        //            objList.AddRange(((clsGetRSIJMasterListBizActionVO)arg.Result).MasterList);
        //            cmbDepartment.ItemsSource = null;
        //            cmbDepartment.ItemsSource = objList;
        //            cmbDepartment.SelectedItem = objList[0];
        //            //if (this.DataContext != null)
        //            //{
        //            //    CmbDoctor.SelectedValue = ((clsProcedureMasterVO)this.DataContext).ID;
        //            //}
        //        }
        //    };
        //    client.ProcessAsync(BizAction, new clsUserVO());
        //}

        /// <summary>
        /// Fills department list
        /// </summary>
        private void FillDepartmentList()
        {
            clsGetRSIJMasterListBizActionVO BizAction = new clsGetRSIJMasterListBizActionVO();
            BizAction.DescriptionColumn = "NamaBagian";
            BizAction.MasterList = new List<MasterListItem>();
            if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID > 0)
            {
                BizAction.MasterTable = MasterTableNameList.BAGIAN_DOC;
                BizAction.CodeColumn = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorCode;
            }
            else
            {
                BizAction.MasterTable = MasterTableNameList.BAGIAN;
                BizAction.CodeColumn = "KodeBagian";
            }


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem("0", "-- Select --"));
                    objList.AddRange(((clsGetRSIJMasterListBizActionVO)arg.Result).MasterList);
                    cmbDepartment.ItemsSource = null;
                    cmbDepartment.ItemsSource = objList;
                    cmbDepartment.SelectedItem = objList[0];
                }
                if (this.DataContext != null)
                {
                    cmbDepartment.SelectedValue = ((clsQueueVO)this.DataContext).DepartmentID;
                }

            };
            client.ProcessAsync(BizAction, new clsUserVO());

        }

        private void FillDoctor(string sDeptCode)
        {
            clsGetRSIJDoctorDepartmentDetailsBizActionVO BizAction = new clsGetRSIJDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();
            if ((MasterListItem)cmbDepartment.SelectedItem != null)
            {
                BizAction.DepartmentCode = ((MasterListItem)cmbDepartment.SelectedItem).Code;
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (DoctorList == null)
                        DoctorList = new PagedSortableCollectionView<MasterListItem>();
                    DoctorList.Clear();
                    List<MasterListItem> lstDoctorList = new List<MasterListItem>();
                    //lstDoctorList.Add(new MasterListItem(0, "-- Select --"));
                    lstDoctorList.AddRange(((clsGetRSIJDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);
                    lstDoctorList.ForEach(z => DoctorList.Add(z));
                    dgDocList.ItemsSource = null;
                    dgDocList.ItemsSource = DoctorList;
                }
            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        ///// <summary>
        ///// Gets doctors from doctor  classification
        ///// </summary>
        ///// <param name="docClassification"></param>
        //private void FetchDoctorByDocSpecialization(string sSpecialization)
        //{
        //    try
        //    {
        //        clsGetRSIJMasterListBizActionVO BizAction = new clsGetRSIJMasterListBizActionVO();
        //        BizAction.MasterList = new List<MasterListItem>();
        //        BizAction.MasterTable = MasterTableNameList.SPESIAL_DOC;
        //        BizAction.CodeColumn = "KODEDOKTER";
        //        BizAction.DescriptionColumn = "NAMADOKTER";
        //        BizAction.Parent = new KeyValue();
        //        BizAction.Parent.Value = "KDSPESIAL";
        //        BizAction.Parent.Key = sSpecialization;
        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //        client.ProcessCompleted += (s, arg) =>
        //        {
        //            if (arg.Error == null && arg.Result != null)
        //            {
        //                if (DoctorList == null)
        //                    DoctorList = new PagedSortableCollectionView<MasterListItem>();
        //                DoctorList.Clear();

        //                List<MasterListItem> objList = new List<MasterListItem>();
        //                //objList.Add(new MasterListItem("0", "-- Select --"));
        //                objList.AddRange(((clsGetRSIJMasterListBizActionVO)arg.Result).MasterList);
        //                objList.ForEach(z => DoctorList.Add(z));
        //                dgDocList.ItemsSource = null;
        //                dgDocList.ItemsSource = DoctorList;
        //                //dgDocList.SelectedItem = objList[0];

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
        /// Gets doctors from doctor  classification
        /// </summary>
        /// <param name="docClassification"></param>
        private void FetchDoctorByDocSpecialization(string sSpecialization)
        {
            try
            {
                clsGetDoctorListBySpecializationBizActionVO BizActionVo = new clsGetDoctorListBySpecializationBizActionVO();
                BizActionVo.SpecializationCode = sSpecialization;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        //objList.Add(new MasterListItem("0", "-- Select --"));
                        objList.AddRange(((clsGetDoctorListBySpecializationBizActionVO)e.Result).DocDetails);
                        objList.ForEach(z => DoctorList.Add(z));
                        dgDocList.ItemsSource = null;
                        dgDocList.ItemsSource = objList;                    
                        
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

        #endregion

        #region Checked-UnChecked Events
        /// <summary>
        /// Grid Check box checked
        /// </summary>
        private void chkStatus_Checked(object sender, RoutedEventArgs e)
        {
            //if (dgDocList.SelectedItem != null)
            //{
            //    ((MasterListItem)dgDocList.SelectedItem).Status = true;
            //    //newDoc.Status = true;
            //    //AddedDoctorList.Add(newDoc);
            //}
        }

        private void chkStatus_Unchecked(object sender, RoutedEventArgs e)
        {
            //if (dgDocList.SelectedItem != null)
            //{
            //    ((MasterListItem)dgDocList.SelectedItem).Status = false;
            //    //AddedDoctorList.RemoveAt(dgDocList.SelectedIndex);
            //}
        }

        #endregion

        private void cmbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FetchDoctorByDocSpecialization(((MasterListItem)cmbDepartment.SelectedItem).Code);
            //FillDoctor(((MasterListItem)cmbDepartment.SelectedItem).Code);
        }

        private void chkStatus_Click(object sender, RoutedEventArgs e)
        {
            bool IsValid = true;
            if (dgDocList.SelectedItem != null)
            {
                try
                {
                    if (AddedDoctorList == null)
                        AddedDoctorList = new ObservableCollection<MasterListItem>();

                    CheckBox chk = (CheckBox)sender;
                    StringBuilder strError = new StringBuilder();
                    if (chk.IsChecked == true)
                    {
                        if (AddedDoctorList.Count > 0)
                        {
                            var item = from r in AddedDoctorList
                                       where r.Code == ((MasterListItem)dgDocList.SelectedItem).Code
                                       select new MasterListItem
                                       {
                                           Status = r.Status,
                                           Code = r.Code
                                       };
                            if (item.ToList().Count > 0)
                            {
                                if (strError.ToString().Length > 0)
                                    strError.Append(",");
                                strError.Append(((MasterListItem)dgDocList.SelectedItem).Code);
                                if (!string.IsNullOrEmpty(strError.ToString()))
                                {
                                    string strMsg = "Doctor already Selected : " + strError.ToString();

                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                               new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.Show();
                                    ((clsRSIJItemMasterVO)dgDocList.SelectedItem).Selected = false;

                                    IsValid = false;
                                }
                                else
                                {
                                    AddedDoctorList.Add((MasterListItem)dgDocList.SelectedItem);
                                }
                            }
                            else
                            {
                                AddedDoctorList.Add((MasterListItem)dgDocList.SelectedItem);
                            }
                        }
                        else
                        {
                            AddedDoctorList.Add((MasterListItem)dgDocList.SelectedItem);
                        }
                    }
                    else
                        AddedDoctorList.Remove((MasterListItem)dgDocList.SelectedItem);

                }
                catch (Exception) { }
            }
        }
    }
}

