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
using PalashDynamics.ValueObjects.OutPatientDepartment.QueueManagement;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Windows.Browser;
using OPDModule;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.TokenDisplay;
using System.Reflection;
using System.Windows.Controls.Data;
using CIMS;
using OPDModule.Forms;
using MessageBoxControl;
using PalashDynamics.Collections;

namespace PalashDynamics.Forms.Home
{
    public partial class HomeQueue : UserControl
    {
      //  bool UseAppDoctorID = true;
        public long sort { get; set; }
        public PagedSortableCollectionView<clsQueueVO> DataList = new PagedSortableCollectionView<clsQueueVO>();
        List<clsQueueVO> DataList1 = new List<clsQueueVO>();
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

        public HomeQueue()
        {
            InitializeComponent();
            mElement = (TextBlock)rootPage.FindName("SampleHeader");
            mElement.Text = "Queue List";
            this.Loaded += new RoutedEventHandler(frmHomeQueue_Loaded);
            DataList = new PagedSortableCollectionView<clsQueueVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
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
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {

            FetchData();

        }
        void SetDatToControl(DateTime SetDate)
        {
            dtpFromDate.SelectedDate = SetDate;
            dtpToDate.SelectedDate = SetDate;
        }
        #region Variable declaration
        public clsQueueVO objQueue = new clsQueueVO();
        bool IsPageLoded = false;
        bool UseAppDoctorID = true;
       // bool UseApplicationID = true;
        WaitIndicator Indicatior = null;
        DateTime ApplicationDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 00, 00, 00);
        
        #endregion
        UserControl rootPage = Application.Current.RootVisual as UserControl;
        TextBlock mElement;
        private void frmHomeQueue_Loaded(object sender, RoutedEventArgs e)
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
                SetComboboxValue();

                FetchData();
                //cmbDepartment.Focus();
                Indicatior.Close();
            }
            dtpFromDate.Focus();
         
            cmbDepartment.UpdateLayout();
            IsPageLoded = true;
        }
        private void SetComboboxValue()
        {
            cmbUnit.SelectedValue = ((clsQueueVO)this.DataContext).UnitID;
            //cmbDepartment.SelectedValue = ((clsQueueVO)this.DataContext).DepartmentID;
            //cmbDoctor.SelectedValue = ((clsQueueVO)this.DataContext).DoctorID;
            cmbDepartment.SelectedValue =(long)0;
            cmbDoctor.SelectedValue = (long)0;
        }

      
        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FetchData();
        }
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

                if (((CIMS.IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
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

         //   BizAction.IsPagingEnabled = false;

            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == false)
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            grdPatientQ.ItemsSource = null;
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {

                    //if (((clsGetQueueListBizActionVO)arg.Result).QueueList != null)
                    //{
                    //    BizAction.QueueList = new List<clsQueueVO>();
                    
                    //    for (int i = 0; i < ((clsGetQueueListBizActionVO)arg.Result).QueueList.Count; i++)
                    //    {
                    //        sort = i + 1;
                    //        clsQueueVO objQueueVO = new clsQueueVO();
                    //        objQueueVO = ((clsGetQueueListBizActionVO)arg.Result).QueueList[i];                         

                    //        BizAction.QueueList.Add(objQueueVO);
                    //        grdPatientQ.ItemsSource = BizAction.QueueList;


                    //    }

               // }

                        //clsGetQueueListBizActionVO result = arg.Result as clsGetQueueListBizActionVO;
                        DataList.TotalItemCount = ((clsGetQueueListBizActionVO)arg.Result).TotalRows;
                        BizAction.QueueList = new List<clsQueueVO>();                  
                        DataList1 = new List<clsQueueVO>();
                            foreach (var item in ((clsGetQueueListBizActionVO)arg.Result).QueueList)
                            {
                                DataList1.Add(item);
                                DataList.Add(item);
                            }
             
                            grdPatientQ.ItemsSource = null;
                            grdPatientQ.ItemsSource = DataList1;
                            grdPgrPatientQ.Source = DataList; 
                 

                    
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
      

        private void cmbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
               if((MasterListItem)cmbDepartment.SelectedItem !=null);
              FillDoctor(((MasterListItem)cmbUnit.SelectedItem).ID, ((MasterListItem)cmbDepartment.SelectedItem).ID);
        }
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

        private void txtFirstName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                cmdSearch_Click(sender, e);
            }
        }

        private void txtLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtLastName.Text = txtLastName.Text.ToTitleCase();
        }

        private void txtSearchCriteria_LostFocus(object sender, RoutedEventArgs e)
        {
            txtSearchCriteria.Text = txtSearchCriteria.Text.ToTitleCase();
        }

        private void cmbUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbUnit.SelectedItem != null)
                FillDepartmentList(((MasterListItem)cmbUnit.SelectedItem).ID);
        }
        bool UseApplicationID = true;
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



                }

            };


            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void grdPatientQ_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grdPatientQ.SelectedItem != null)
            {
                clsQueueVO ObjQueueVO = new clsQueueVO();
                ObjQueueVO = ((clsQueueVO)grdPatientQ.SelectedItem);

                clsGetPatientGeneralDetailsListBizActionVO BizAction = new clsGetPatientGeneralDetailsListBizActionVO();
                BizAction.PatientDetailsList = new List<clsPatientGeneralVO>();
                BizAction.MRNo = ObjQueueVO.MRNO;
                BizAction.OPDNo = ObjQueueVO.OPDNO;
                BizAction.VisitWise = true;

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

            //VisitIndexChange();
        }

      

        private void btnSearchCriteria_Click(object sender, RoutedEventArgs e)
        {
            FetchData();
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

        private void txtSearchCriteria_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;

            if (e.Key == Key.Enter)
            {
                FetchData();
            }
        }

        private void txtContactNo_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
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

        private void txtLastName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FetchData();
            }
        }

        private void txtMRNO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FetchData();
            }
        }
    
        
    }
}
