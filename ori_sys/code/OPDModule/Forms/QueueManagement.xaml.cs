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
using PalashDynamics.ValueObjects.OutPatientDepartment.QueueManagement;
using System.Windows.Browser;
using OPDModule;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.TokenDisplay;
using System.Reflection;
using PalashDynamics;
using System.IO;
using System.Windows.Resources;
using System.Xml.Linq;
using System.Reflection;
using OPDModule.Forms;

namespace CIMS.Forms
{
    public partial class QueueManagement : UserControl, IInitiateCIMS
    {

        public bool IsAndrology = false;
        public string ModuleName { get; set; } //***//
        #region IInitiateCIMS Members

        public void Initiate(string Mode)
        {
            //throw new NotImplementedException();
            switch (Mode)
            {
                case "IsAndrology":
                    IsAndrology = true;
                    break;
                default:
                    break;
            }
        }

        #endregion
        /// <summary>
        /// Constructor
        /// </summary>
        public QueueManagement()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(QueueManagement_Loaded);

            //this.Title = null;
            //this.HasCloseButton = false;



            if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
            {
                cmbDoctor.IsEnabled = false;


                this.DataContext = new clsQueueVO()
                {
                    UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId,
                    DepartmentID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DepartmentID,
                    DoctorID = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID

                };

            }
            else
            {
                this.DataContext = new clsQueueVO()
                {
                    UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId,
                    DepartmentID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DepartmentID,
                    DoctorID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DoctorID,

                };
            }

        }

        #region Variable declaration
        public clsQueueVO objQueue = new clsQueueVO();
        bool IsPageLoded = false;
        bool UseAppDoctorID = true;
        bool UseApplicationID = true;
        WaitIndicator Indicatior = null;
        #endregion

        void QueueManagement_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                SetDatToControl(ApplicationDate);

                Indicatior = new WaitIndicator();
                Indicatior.Show();
                FillVisitStatus();


                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                {
                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId
                      && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AllowClinicalTransaction == true)
                    {
                        //do  nothing
                    }
                }

                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                {
                    cmbUnit.IsEnabled = true;
                }
                else
                {
                    cmbUnit.IsEnabled = false;
                    cmbUnit.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }
                FillUnit();
                FillSpecialRegistration();
                SetComboboxValue();
                if (!IsAndrology)
                {
                    FetchData();
                }
                if (IsAndrology)
                {
                    dgQueueList.Columns[0].Visibility = Visibility.Collapsed;
                    dgQueueList.Columns[17].Visibility = Visibility.Collapsed;
                    cmdCallPatient.Visibility = Visibility.Collapsed;
                    TokenDisplay.Visibility = Visibility.Collapsed;
                }
                //cmbDepartment.Focus();
                Indicatior.Close();
            }
            cmbDepartment.Focus();
            cmbDepartment.UpdateLayout();
            IsPageLoded = true;

        }


        #region FillCombobox

        private void FillDepartmentList(long iUnitId)
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_DepartmentUnitView;
            if (iUnitId > 0)
                BizAction.Parent = new KeyValue { Key = iUnitId, Value = "UnitId" };

            BizAction.MasterList = new List<MasterListItem>();
            //PalashServiceClient Client = null;
            //Client = new PalashServiceClient();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

                    if (((MasterListItem)cmbUnit.SelectedItem).ID == 0)
                    {

                        var results = from a in objList
                                      group a by a.ID into grouped
                                      select grouped.First();
                        objList = results.ToList();
                    }

                    cmbDepartment.ItemsSource = null;
                    cmbDepartment.ItemsSource = objList;



                    if (this.DataContext != null)
                    {
                        if (UseApplicationID == true)
                        {
                            cmbDepartment.SelectedValue = ((clsQueueVO)this.DataContext).DepartmentID;
                            UseApplicationID = false;
                        }
                        else
                            cmbDepartment.SelectedValue = objList[0].ID;

                    }

                    if (IsAndrology == true)
                    {
                        ((clsQueueVO)this.DataContext).DepartmentID = 16;//HardCode By Bhushanp For Andrology 08052017 
                        cmbDepartment.SelectedValue = ((clsQueueVO)this.DataContext).DepartmentID;
                        cmbDepartment.UpdateLayout();
                        cmbDepartment.IsEnabled = false;
                        FetchData();
                    }
                }

            };


            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void FillUnit()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);
                    cmbUnit.ItemsSource = null;
                    cmbUnit.ItemsSource = objList;

                }

                if (this.DataContext != null)
                {
                    //cmbUnit.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;


                    cmbUnit.SelectedValue = ((clsQueueVO)this.DataContext).UnitID;
                }

            };
            //client.ProcessAsync(BizAction, new clsUserVO());
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void FillSpecialRegistration()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_SpecialRegistrationMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {

                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbSpRegistration.ItemsSource = null;
                    cmbSpRegistration.ItemsSource = objList;
                    cmbSpRegistration.SelectedItem = objList[0];

                }




            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }

        //private void FillDoctorList(long iDeptId)
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    BizAction.MasterTable = MasterTableNameList.M_DoctorDepartmentView;
        //    if (iDeptId > 0)
        //        BizAction.Parent = new KeyValue { Key = iDeptId, Value = "DepartmentId" };
        //    BizAction.MasterList = new List<MasterListItem>();
        //    //PalashServiceClient Client = null;
        //    //Client = new PalashServiceClient();
        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //    client.ProcessCompleted += (s, arg) =>
        //    {
        //        if (arg.Error == null && arg.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();
        //            objList.Add(new MasterListItem(0, "- Select -"));
        //            objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);


        //            cmbDoctor.ItemsSource = null;
        //            cmbDoctor.ItemsSource = objList;

        //            if (this.DataContext != null)
        //            {

        //                if (UseAppDoctorID == true)
        //                {
        //                    cmbDoctor.SelectedValue = ((clsQueueVO)this.DataContext).DoctorID;
        //                    UseAppDoctorID = false;
        //                }
        //                else
        //                    cmbDoctor.SelectedValue = objList[0].ID;

        //            }

        //        }
        //    };
        //    client.ProcessAsync(BizAction, new clsUserVO());
        //}

        private void FillDoctor(long IUnitId, long iDeptId)
        {
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();


            BizAction.UnitId = IUnitId;

            if ((MasterListItem)cmbDepartment.SelectedItem != null)
            {
                BizAction.DepartmentId = iDeptId;
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);


                    cmbDoctor.ItemsSource = null;
                    cmbDoctor.ItemsSource = objList;

                    if (this.DataContext != null)
                    {

                        if (UseAppDoctorID == true)
                        {
                            cmbDoctor.SelectedValue = ((clsQueueVO)this.DataContext).DoctorID;
                            UseAppDoctorID = false;
                        }
                        else
                            cmbDoctor.SelectedValue = objList[0].ID;

                        if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
                        {
                            cmbDoctor.IsEnabled = false;
                            cmbDoctor.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID;
                        }

                    }

                }
            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        #endregion

        private void SetComboboxValue()
        {
            cmbUnit.SelectedValue = ((clsQueueVO)this.DataContext).UnitID;
            cmbDepartment.SelectedValue = ((clsQueueVO)this.DataContext).DepartmentID;
            cmbDoctor.SelectedValue = ((clsQueueVO)this.DataContext).DoctorID;
            cmbSpRegistration.SelectedValue = ((clsQueueVO)this.DataContext).SpecialRegID;
        }



        #region Validation
        private void txtSearchCriteria_LostFocus(object sender, RoutedEventArgs e)
        {
            txtSearchCriteria.Text = txtSearchCriteria.Text.ToTitleCase();
        }

        private void txtLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtLastName.Text = txtLastName.Text.ToTitleCase();
        }

        #endregion

        #region Get Data
        public long sort { get; set; }
        /// <summary>
        /// Purpose:Search patient using different search criteria.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            if (dtpFromDate.SelectedDate > DateTime.Now)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                   new MessageBoxControl.MessageBoxChildWindow("Palash", "From Date Can Not Be Greater Than Today.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
            }
            else if (dtpToDate.SelectedDate > DateTime.Now)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                   new MessageBoxControl.MessageBoxChildWindow("Palash", "To Date Can Not Be Greater Than Today.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
            }
            else if (dtpFromDate.SelectedDate > dtpToDate.SelectedDate)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                   new MessageBoxControl.MessageBoxChildWindow("Palash", "From Date Can Not Be Greater Than To Date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
            }
            else
            {
                FetchData();
            }
        }
        private void txtFirstName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                cmdSearch_Click(sender, e);
            }
        }

        private void FetchData()
        {
            clsGetQueueListBizActionVO BizAction = new clsGetQueueListBizActionVO();
            BizAction.QueueList = new List<clsQueueVO>();

            if (dtpFromDate.SelectedDate != null)
                BizAction.FromDate = dtpFromDate.SelectedDate;

            if (cmbCurrentVisit.SelectedItem != null)
                BizAction.CurrentVisit = ((MasterListItem)cmbCurrentVisit.SelectedItem).ID;

            if (dtpToDate.SelectedDate != null)
                BizAction.ToDate = dtpToDate.SelectedDate;
            if (UseAppDoctorID != true)
            {

                if (cmbDepartment.SelectedItem != null)
                    BizAction.DepartmentID = ((MasterListItem)cmbDepartment.SelectedItem).ID;

                if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
                {
                    cmbDoctor.IsEnabled = false;
                    BizAction.DoctorID = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID;
                }
                else
                {
                    if (cmbDoctor.SelectedItem != null)
                        BizAction.DoctorID = ((MasterListItem)cmbDoctor.SelectedItem).ID;
                }
                //if (((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId != 0)
                //{
                //    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                //}

                if (cmbUnit.SelectedItem != null)
                {
                    BizAction.UnitID = ((MasterListItem)cmbUnit.SelectedItem).ID;
                }

                #region commented by Saily P to filter by clinic
                ////if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                ////{
                ////    BizAction.UnitID = 0;
                ////}
                ////else
                ////{
                ////    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                ////}
                #endregion
            }
            else
            {
                if (this.DataContext != null)
                {
                    BizAction.DepartmentID = ((clsQueueVO)this.DataContext).DepartmentID;
                    if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
                    {
                        cmbDoctor.IsEnabled = false;
                        BizAction.DoctorID = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID;
                    }
                    else
                    {

                        BizAction.DoctorID = ((clsQueueVO)this.DataContext).DoctorID;
                    }
                    //BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                    if (cmbUnit.SelectedItem != null)
                    {
                        BizAction.UnitID = ((MasterListItem)cmbUnit.SelectedItem).ID;
                    }

                    #region commented by Saily P on 15.05.12 to filter by clinic
                    //if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                    //{
                    //    BizAction.UnitID = 0;
                    //}
                    //else
                    //{
                    //    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    //}
                    #endregion
                }
            }

            //By Anjali....................
            if (cmbSpRegistration.SelectedItem != null)
                BizAction.SpecialRegID = ((MasterListItem)cmbSpRegistration.SelectedItem).ID;
            //...........................

            if (txtSearchCriteria.Text != null)
                BizAction.FirstName = txtSearchCriteria.Text;

            if (txtLastName.Text != null)
                BizAction.LastName = txtLastName.Text;

            if (txtMRNO.Text.Trim() != null)
                BizAction.MRNo = txtMRNO.Text.Trim();

            if (txtContactNo.Text.Trim() != null)
                BizAction.ContactNo = txtContactNo.Text.Trim();

            if (txtTokenNo.Text.Trim() != null)
                BizAction.TokenNo = txtTokenNo.Text.Trim();

            BizAction.IsPagingEnabled = false;
            //BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            //BizAction.MaximumRows = DataList.PageSize;

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == false)
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            dgQueueList.ItemsSource = null;
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {

                    if (((clsGetQueueListBizActionVO)arg.Result).QueueList != null)
                    {
                        BizAction.QueueList = new List<clsQueueVO>();
                        //commented by akshays
                        for (int i = 0; i < ((clsGetQueueListBizActionVO)arg.Result).QueueList.Count; i++)
                        {
                            sort = i + 1;
                            clsQueueVO objQueueVO = new clsQueueVO();
                            objQueueVO = ((clsGetQueueListBizActionVO)arg.Result).QueueList[i];
                            //objQueueVO.SortOrder = sort;

                            BizAction.QueueList.Add(objQueueVO);
                            dgQueueList.ItemsSource = BizAction.QueueList;

                            //    clsUpdatePatientSortOrderBizActionVO BizActionobj = new clsUpdatePatientSortOrderBizActionVO();
                            //    BizActionobj.QueueDetails = new clsQueueVO();
                            //    BizActionobj.QueueDetails.QueueID = objQueueVO.QueueID;
                            //    BizActionobj.QueueDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                            //    BizActionobj.QueueDetails.SortOrder = objQueueVO.SortOrder;
                            //    client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                            //    client.ProcessCompleted += (sa, arg1) =>
                            //    {
                            //        if (arg1.Error == null)
                            //        {

                            //        }
                            //        else
                            //        {
                            //            MessageBoxControl.MessageBoxChildWindow msgW1 =
                            //                new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                            //            msgW1.Show();
                            //        }


                            //    };
                            //    client.ProcessAsync(BizActionobj, new clsUserVO());
                            //    client.CloseAsync();

                        }
                        //commented by akshays

                    }
                }

                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        #endregion

        void dataGrid1_RowsMoved(object sender, RowsMovedEventArgs args)
        {
            //IList<clsQueueVO> list = this.dgQueueList.ItemsSource as IList<clsQueueVO>;
            //this.MoveInList(list, args.StartIndex, args.Count, args.DestinationIndex);
        }

        private void dataGrid1_BeforeMovingRows(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // this.FlattenSelection();
        }

        private void FlattenSelection()
        {
            //int topRowIndex = Int32.MaxValue;
            //int botRowIndex = Int32.MinValue;
            //List<clsQueueVO> list = this.dgQueueList.ItemsSource as List<clsQueueVO>;
            //foreach (clsQueueVO item in this.dgQueueList.SelectedItems)
            //{
            //    int index = list.IndexOf(item);
            //    if (index < topRowIndex)
            //        topRowIndex = index;
            //    if (index > botRowIndex)
            //        botRowIndex = index;
            //}
            //if (this.dgQueueList.SelectedItems.Count < (botRowIndex - topRowIndex + 1))
            //{
            //    for (int i = topRowIndex; i <= botRowIndex; i++)
            //    {
            //        clsQueueVO o = list[i];
            //        if (this.dgQueueList.SelectedItems.Contains(o) == false)
            //            this.dgQueueList.SelectedItems.Add(o);
            //    }
            //}
        }

        #region Move Patient from current positon
        /// <summary>
        /// Purpose:For move patient Up.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void cmdMoveUp_Click(object sender, RoutedEventArgs e)
        {
            if (dgQueueList.SelectedItem != null)
            {
                if (dgQueueList.SelectedIndex != 0)
                {
                    int lstart = dgQueueList.SelectedIndex;
                    IList<clsQueueVO> list = (IList<clsQueueVO>)this.dgQueueList.ItemsSource;
                    IList<clsQueueVO> list1;
                    list1 = list.DeepCopy();

                    list.RemoveAt(lstart);


                    list.Insert(lstart - 1, list1[lstart]);

                    list[lstart].SortOrder = list[lstart].SortOrder + 1;
                    //list[lstart - 1].SortOrder = list[lstart - 1].SortOrder - 1;

                    this.dgQueueList.ItemsSource = null;
                    this.dgQueueList.ItemsSource = list;



                    for (int i = 0; i < list.Count; i++)
                    {
                        clsAddQueueListBizActionVO BizActionobj = new clsAddQueueListBizActionVO();
                        BizActionobj.QueueDetails = new clsQueueVO();
                        BizActionobj.QueueDetails.PatientId = list[i].PatientId;
                        BizActionobj.QueueDetails.DateTime = list[i].DateTime;
                        BizActionobj.QueueDetails.OPDNO = list[i].OPDNO;
                        BizActionobj.QueueDetails.DepartmentID = list[i].DepartmentID;
                        BizActionobj.QueueDetails.DoctorID = list[i].DoctorID;
                        BizActionobj.QueueDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                        BizActionobj.QueueDetails.Status = list[i].Status;
                        BizActionobj.QueueDetails.SortOrder = list[i].SortOrder;
                        BizActionobj.QueueDetails.CurrentVisitStatus = list[i].CurrentVisitStatus;


                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        client.ProcessCompleted += (s, arg) =>
                        {
                            if (arg.Error == null)
                            {

                            }


                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                                msgW1.Show();
                            }

                        };

                        client.ProcessAsync(BizActionobj, new clsUserVO());
                        client.CloseAsync();


                    }
                }
                else
                {
                    string msgTitle = "";
                    string msgText = "Can not move Patient Up";

                    MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW.Show();
                }
            }

            else
            {
                string msgTitle = "";
                string msgText = "Please Select Patient";

                MessageBoxControl.MessageBoxChildWindow msgW =
                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();
            }
        }

        /// <summary>
        /// Purpose:For move patient down.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void cmdMoveDown_Click(object sender, RoutedEventArgs e)
        {

            if (dgQueueList.SelectedItem != null)
            {

                int lstart = dgQueueList.SelectedIndex;
                IList<clsQueueVO> list = (IList<clsQueueVO>)this.dgQueueList.ItemsSource;

                IList<clsQueueVO> list1;
                if (dgQueueList.SelectedIndex != list.Count - 1)
                {

                    list1 = list.DeepCopy();

                    list.RemoveAt(lstart);

                    if (lstart == list.Count)
                        list.Add(list1[lstart]);
                    else
                        list.Insert(lstart + 1, list1[lstart]);

                    //list[lstart].SortOrder = list[lstart].SortOrder - 1;
                    list[lstart + 1].SortOrder = list[lstart + 1].SortOrder + 1;

                    this.dgQueueList.ItemsSource = null;
                    this.dgQueueList.ItemsSource = list;

                    for (int i = 0; i < list.Count; i++)
                    {
                        clsAddQueueListBizActionVO BizActionobj = new clsAddQueueListBizActionVO();
                        BizActionobj.QueueDetails = new clsQueueVO();
                        BizActionobj.QueueDetails.PatientId = list[i].PatientId;
                        BizActionobj.QueueDetails.DateTime = list[i].DateTime;
                        BizActionobj.QueueDetails.OPDNO = list[i].OPDNO;
                        BizActionobj.QueueDetails.DepartmentID = list[i].DepartmentID;
                        BizActionobj.QueueDetails.DoctorID = list[i].DoctorID;
                        BizActionobj.QueueDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                        BizActionobj.QueueDetails.Status = list[i].Status;
                        BizActionobj.QueueDetails.SortOrder = list[i].SortOrder;
                        BizActionobj.QueueDetails.CurrentVisitStatus = list[i].CurrentVisitStatus;


                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        client.ProcessCompleted += (s, arg) =>
                        {
                            if (arg.Error == null)
                            {

                            }

                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                                msgW1.Show();
                            }

                        };

                        client.ProcessAsync(BizActionobj, new clsUserVO());
                        client.CloseAsync();

                    }
                }
                else
                {
                    string msgTitle = "";
                    string msgText = " Cannot move patient down";

                    MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW.Show();
                }
            }

            else
            {
                string msgTitle = "";
                string msgText = "Please Select Patient";

                MessageBoxControl.MessageBoxChildWindow msgW =
                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();
            }

        }
        #endregion


        private void dgQueueList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgQueueList.SelectedItem != null)
            {
                clsQueueVO ObjQueueVO = new clsQueueVO();
                ObjQueueVO = ((clsQueueVO)dgQueueList.SelectedItem);

                clsGetPatientGeneralDetailsListBizActionVO BizAction = new clsGetPatientGeneralDetailsListBizActionVO();
                BizAction.PatientDetailsList = new List<clsPatientGeneralVO>();
                BizAction.MRNo = ObjQueueVO.MRNO;
                BizAction.OPDNo = ObjQueueVO.OPDNO;
                BizAction.ISFromQueeManagment = true;
                BizAction.VisitWise = true;
                BizAction.RegistrationTypeID = 10;
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                {
                    BizAction.UnitID = 0;
                }
                else
                {
                    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsGetPatientGeneralDetailsListBizActionVO)arg.Result).PatientDetailsList.Count > 0)
                        {
                            ((IApplicationConfiguration)App.Current).SelectedPatient = ((clsGetPatientGeneralDetailsListBizActionVO)arg.Result).PatientDetailsList[0];
                            if (((clsQueueVO)dgQueueList.SelectedItem).CrVisitStatus == "Closed")
                            {
                                ((IApplicationConfiguration)App.Current).SelectedPatient.IsClinicVisited = false;
                            }
                            else
                            {
                                ((IApplicationConfiguration)App.Current).SelectedPatient.IsClinicVisited = true;
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
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();

            }

            VisitIndexChange();
        }

        #region Close or Open visit Of patient
        /// <summary>
        /// Purpose:Close or open visit of selected patient
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkVisitClose_Click(object sender, RoutedEventArgs e)
        {
            if ((clsQueueVO)dgQueueList.SelectedItem != null && ((clsQueueVO)dgQueueList.SelectedItem).Status == true)
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to close the Visit ?";
                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                msgW.Show();
            }
            else
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to Open the Visit ?";
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);

                msgW1.Show();
            }

        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                clsUpdateCurrentVisitStatusBizActionVO BizAction = new clsUpdateCurrentVisitStatusBizActionVO();
                BizAction.CurrentVisitStatus = VisitCurrentStatus.Closed;
                BizAction.VisitID = ((clsQueueVO)dgQueueList.SelectedItem).VisitID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        FetchData();
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Visit Closed Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                    }
                };

                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            else
            {
                ((clsQueueVO)dgQueueList.SelectedItem).CloseVisit = false;
            }
        }

        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                clsUpdateCurrentVisitStatusBizActionVO BizAction = new clsUpdateCurrentVisitStatusBizActionVO();
                BizAction.CurrentVisitStatus = VisitCurrentStatus.ReOpen;
                BizAction.VisitID = ((clsQueueVO)dgQueueList.SelectedItem).VisitID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        FetchData();
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Visit Open Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                    }
                };

                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }

        }

        #endregion

        private void cmbUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbUnit.SelectedItem != null)
                FillDepartmentList(((MasterListItem)cmbUnit.SelectedItem).ID);

        }

        private void cmbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if ((MasterListItem)cmbDepartment.SelectedItem != null) ;
            FillDoctor(((MasterListItem)cmbUnit.SelectedItem).ID, ((MasterListItem)cmbDepartment.SelectedItem).ID);


        }

        private void cmdCallPatient_Click(object sender, RoutedEventArgs e)
        {
            if (dgQueueList.SelectedItem != null)
            {
                if (cmdCallPatient.Content.ToString() == "Call Patient")
                {
                    cmdCallPatient.Content = "End Call";
                    CallStart(PatientID, VisitID, DoctorID);
                }
                else if (cmdCallPatient.Content.ToString() == "End Call")
                {
                    cmdCallPatient.Content = "Call Patient";
                    CallEnd(PatientID, VisitID, DoctorID);

                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW5 =
                new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW5.Show();
            }
            //added by akshays
            //string URL = "../Reports/Patient/TokanDisplay.aspx?UnitID=" + '1' + "&TNo=" + TokanNo + "&deptname=" + Department;
            //HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            //closed by akshays
        }

        void CallStart(long PatientId, long VisitID, long DoctorID)
        {
            try
            {
                clsAddUpdateTokenDisplayBizActionVO objBizActionVO = new clsAddUpdateTokenDisplayBizActionVO();
                objBizActionVO.PatientId = PatientId;
                objBizActionVO.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                objBizActionVO.VisitId = VisitID;
                objBizActionVO.DoctorID = DoctorID;
                objBizActionVO.VisitDate = Convert.ToDateTime(ApplicationDate.Date + DateTime.Now.TimeOfDay);
                objBizActionVO.IsDisplay = false;
                objBizActionVO.Id = 0;

                //**********************Update Visit Master***********************************//
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsAddUpdateTokenDisplayBizActionVO)arg.Result).SuccessStatus == 1)
                        {
                            VisitIndexChange();

                        }
                        else
                        {
                            string msgTitle = "";
                            string msgText = "Error occurred while processing.";


                            MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        }


                    }
                    else
                    {
                        string msgTitle = "";
                        string msgText = "Error occurred while updating";


                        MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    }

                };

                client.ProcessAsync(objBizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

            }
            catch (Exception)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 = new MessageBoxControl.MessageBoxChildWindow("", "Error Occured.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();

            }
        }
        DateTime ApplicationDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 00, 00, 00);
        void CallEnd(long PatientId, long VisitID, long DoctorID)
        {
            try
            {
                clsUpdateTokenDisplayStatusBizActionVO objBizActionVO = new clsUpdateTokenDisplayStatusBizActionVO();
                objBizActionVO.PatientId = PatientId;
                objBizActionVO.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                objBizActionVO.VisitId = VisitID;
                objBizActionVO.DoctorID = DoctorID;
                objBizActionVO.VisitDate = Convert.ToDateTime(ApplicationDate.Date + DateTime.Now.TimeOfDay);
                objBizActionVO.IsDisplay = true;
                objBizActionVO.Id = 0;
                //**********************Update Visit Master***********************************//
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsUpdateTokenDisplayStatusBizActionVO)arg.Result).SuccessStatus == 1)
                        {
                            VisitIndexChange();
                        }
                        else
                        {
                            string msgTitle = "";
                            string msgText = "Error occurred while processing.";
                            MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        string msgTitle = "";
                        string msgText = "Error occurred while updating";
                        MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    }
                };
                client.ProcessAsync(objBizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 = new MessageBoxControl.MessageBoxChildWindow("", "Error Occured.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        long PatientID = 0; long DoctorID = 0; long VisitID = 0; long PatientUnitID = 0;
        void VisitIndexChange()
        {
            if (dgQueueList.SelectedItem != null)
            {
                VisitID = ((clsQueueVO)dgQueueList.SelectedItem).VisitID;
                DoctorID = ((clsQueueVO)dgQueueList.SelectedItem).DoctorID;
                PatientID = ((clsQueueVO)dgQueueList.SelectedItem).PatientId;
                DateTime? selDate = ((clsQueueVO)dgQueueList.SelectedItem).Date;
                //IsLoadRec = false;
                ShowCallStatus(PatientID, ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, VisitID);
            }
        }
        void ShowCallStatus(long PatientID, long UnitId, long VisitID)
        {
            try
            {
                bool IsTokenDispled = false;
                clsGetTokenDisplayPatirntDetailsBizActionVO bizActionVO = new clsGetTokenDisplayPatirntDetailsBizActionVO();
                bizActionVO.UnitId = UnitId;
                bizActionVO.PatientId = PatientID;
                bizActionVO.VisitId = VisitID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    Indicatior.Show();
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            string CurrentDateaa = ((clsGetTokenDisplayPatirntDetailsBizActionVO)arg.Result).VisitDate.ToString("dd-MM-yyyy");
                            IsTokenDispled = ((clsGetTokenDisplayPatirntDetailsBizActionVO)arg.Result).IsDisplay;
                            if (IsTokenDispled == true)
                            {
                                cmdCallPatient.Content = "End Call";
                            }
                            else
                            {
                                cmdCallPatient.Content = "Call Patient";

                            }
                        }
                    }
                    Indicatior.Close();
                };
                client.ProcessAsync(bizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                Indicatior.Close();
            }
        }


        //added by akshays 
        private void FillVisitStatus()
        {
            List<MasterListItem> mlPaymode = new List<MasterListItem>();
            MasterListItem Default = new MasterListItem(0, "- Select -");
            mlPaymode.Insert(0, Default);
            EnumToList(typeof(VisitCurrentStatus), mlPaymode);
            cmbCurrentVisit.ItemsSource = mlPaymode;
            cmbCurrentVisit.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CurrentVisit;
        }

        public static void EnumToList(Type EnumType, List<MasterListItem> TheMasterList)
        {
            Array Values = GetValues(EnumType);
            foreach (int Value in Values)
            {
                if (Value > 0)
                {
                    string Display = Enum.GetName(EnumType, Value);
                    MasterListItem Item = new MasterListItem(Value, Display);
                    TheMasterList.Add(Item);
                }
            }
        }

        public static object[] GetValues(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");
            }

            List<object> values = new List<object>();

            var fields = from field in enumType.GetFields()
                         where field.IsLiteral
                         select field;

            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(enumType);
                values.Add(value);
            }

            return values.ToArray();

        }

        void SetDatToControl(DateTime SetDate)
        {
            dtpFromDate.SelectedDate = SetDate;
            dtpToDate.SelectedDate = SetDate;
        }

        string PatientName = ""; string Department = ""; string TokanNo = "";
        private void cmdCallPatientTokan_Click(object sender, RoutedEventArgs e)
        {
            clsQueueVO clsq = new clsQueueVO();
            if (dgQueueList.SelectedItem != null)
            {
                PatientName = ((clsQueueVO)dgQueueList.SelectedItem).PatientName;
                Department = ((clsQueueVO)dgQueueList.SelectedItem).Discription;
                TokanNo = ((clsQueueVO)dgQueueList.SelectedItem).SortOrder;
                string pname = PatientName;
                string deptname = Department;
                string TNo = TokanNo;
                //clsq.PatientName = pname;
                //clsq.Discription = deptname;
                //clsq.SortOrder = TNo;

                //long ID = ((clsPatientVO)this.DataContext).GeneralDetails.PatientID;
                //long UnitID = ((clsPatientVO)this.DataContext).GeneralDetails.UnitId;
                ////long IsDoctorID ;
                ////long IsEmployee;
                ////long DoctorID ;
                ////long EmployeeID;

                if (PatientName != null && TokanNo != null && Department != null)
                {



                    string URL = "../Reports/Patient/TokanDisplay.aspx?UnitID=" + '1' + "&TNo=" + TokanNo + "&deptname=" + Department;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }


            }
        }

        private void TokenDisplay_Click(object sender, RoutedEventArgs e)
        {
            if (((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId != null)
            {
                string dateformat = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DateFormat;
                long lUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                string URL = "../Reports/Patient/TokanDisplay.aspx?UnitID=" + lUnitID + "&TNo=" + TokanNo + "&deptname=" + Department + "&dformat=" + dateformat;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgWtoken =
               new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occurred", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWtoken.Show();
            }
        }
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        private void txtSearchCriteria_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsPersonNameValid())
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtContactNo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsPageLoded)
            {
                if (!string.IsNullOrEmpty(((TextBox)sender).Text))
                {
                    if (!((TextBox)sender).Text.IsNumberValid() && textBefore != null)
                    {
                        ((TextBox)sender).Text = textBefore;
                        ((TextBox)sender).SelectionStart = selectionStart;
                        ((TextBox)sender).SelectionLength = selectionLength;
                        textBefore = "";
                        selectionStart = 0;
                        selectionLength = 0;
                    }
                    else if (((TextBox)sender).Text.Length > 10)
                    {
                        ((TextBox)sender).Text = textBefore;
                        ((TextBox)sender).SelectionStart = selectionStart;
                        ((TextBox)sender).SelectionLength = selectionLength;
                        textBefore = "";
                        selectionStart = 0;
                        selectionLength = 0;
                    }
                }
            }
        }

        private void txtContactNo_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void txtSearchCriteria_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtContactNo_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }



        // closed by akshays


        //***//----------------    


        private void AddFollowup_Click(object sender, RoutedEventArgs e)
        {
            if (dgQueueList.SelectedItem != null)
            {
                if (((clsQueueVO)dgQueueList.SelectedItem).CrVisitStatus != "Closed")
                {
                    PatientFollowup FollowupObj = new PatientFollowup();
                    (((IApplicationConfiguration)App.Current).SelectedPatient).DepartmentID = ((clsQueueVO)dgQueueList.SelectedItem).DepartmentID;
                    FollowupObj.OnSaveButton_Click += new RoutedEventHandler(FollowupWin_OnSaveButton_Click);
                    FollowupObj.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW5 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Patient Visit Closed", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW5.Show();
                }

            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW5 =
                new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW5.Show();
            }
        }

        void FollowupWin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            this.Loaded += new RoutedEventHandler(QueueManagement_Loaded);
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            Nullable<DateTime> dtFT = null;
            Nullable<DateTime> dtTT = null;

            //if (dgQueueList.ItemsSource != null)
            //{
            //    MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                   new MessageBoxControl.MessageBoxChildWindow("", "There are no reports to print", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //    msgW1.Show();

            //}
            //else
            //if {
            if (dtpFromDate.SelectedDate != null)
            {
                dtFT = dtpFromDate.SelectedDate.Value.Date.Date;
            }
            if (dtpToDate.SelectedDate != null)
            {
                dtTT = dtpToDate.SelectedDate.Value.Date.Date;
            }
            long clinic = 0;
            long dept = 0;
            long doc = 0;
            long SpecialRegID = 0;
            long CurrentVisit = 0;
            string MRNO = null;

            //Added by Aniket on 15/10/2018 
            string FirstName = null;
            string LastName = null;

            //Added by Aniket on 16/10/2018 
            string ContactNo = null;
            string TokenNo = null;
            
            //string SpecialReg = null;

            if (cmbUnit.SelectedItem != null)
            {
                clinic = ((MasterListItem)cmbUnit.SelectedItem).ID;
            }
            if (cmbDepartment.SelectedItem != null)
            {
                 dept = ((MasterListItem)cmbDepartment.SelectedItem).ID;
            }
            if (cmbDoctor.SelectedItem != null)
            {
                doc = ((MasterListItem)cmbDoctor.SelectedItem).ID;
            }

            if (txtMRNO.Text.Trim() != null)
                MRNO = txtMRNO.Text.Trim();

            //Added by Aniket on 15/10/2018 
            if (txtSearchCriteria.Text.Trim() != null)
                FirstName = txtSearchCriteria.Text.Trim();

            if (txtLastName.Text.Trim() != null)
                LastName = txtLastName.Text.Trim();

            //Added by Aniket on 16/10/2018 
            if (txtContactNo.Text.Trim() != null)
                ContactNo = txtContactNo.Text.Trim();

            if (txtTokenNo.Text.Trim() != null)
               TokenNo = txtTokenNo.Text.Trim();

            if (cmbCurrentVisit.SelectedItem != null)
            {
                CurrentVisit = ((MasterListItem)cmbCurrentVisit.SelectedItem).ID;
            }

            if (cmbSpRegistration.SelectedItem != null)
            {
                SpecialRegID = ((MasterListItem)cmbSpRegistration.SelectedItem).ID;
            }


            string URL;
            if (dtFT != null && dtTT != null)
            {
                URL = "../Reports/OPD/QueueListReport.aspx?FromDate=" + dtFT.Value.ToString("MM/dd/yyyy") + "&ToDate=" + dtTT.Value.ToString("MM/dd/yyyy") + "&ClinicID=" + clinic + "&DepartmentID=" + dept + "&DoctorID=" + doc + "&VisitStatus=" + CurrentVisit + "&SpecialRegistrationId=" + SpecialRegID + "&MRNO=" + MRNO + "&FirstName=" + FirstName + "&LastName=" + LastName + "&Excel=" + chkExcel.IsChecked + "&ContactNo=" + ContactNo + "&TokenNo=" + TokenNo;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }   
            else
            {
                URL = "../Reports/OPD/QueueListReport.aspx?ClinicID=" + clinic + "&DepartmentID=" + dept + "&DoctorID=" + doc;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
            
           

        }


    }


}


