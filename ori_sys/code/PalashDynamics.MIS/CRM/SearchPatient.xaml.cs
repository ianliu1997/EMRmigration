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
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.EMR;
using System.Windows.Browser;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using CIMS;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.CRM;
using System.Reflection;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;

namespace PalashDynamics.MIS.CRM
{
    public partial class SearchPatient : UserControl
    {
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        public SearchPatient()
        {
            InitializeComponent();
        }

        private void SearchPatient_Loaded(object sender, RoutedEventArgs e)
        {

            dtpFromDate.SelectedDate = DateTime.Now.Date;
            dtpToDate.SelectedDate = DateTime.Now.Date;
            FillUnitList();
            FillStateList();
            FillGender();
            FillMaritalStatus();
            FillLoyaltyProgram();
            FillComplaint();
            cmbAge.ItemsSource = FillAge();
            cmbAge.SelectedValue = (long)0;
            dtpFromDate.Focus();
        }

        #region FillCombobox
        private void FillStateList()
        {
            clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();
            BizAction.TableName = "T_Registration";
            BizAction.ColumnName = "State";
            BizAction.IsDecode = true;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    txtState.ItemsSource = null;
                    txtState.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List;
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillCityList(string pState)
        {
            clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();
            BizAction.TableName = "T_Registration";
            BizAction.ColumnName = "City";
            BizAction.IsDecode = true;

            if (!string.IsNullOrEmpty(pState))
            {
                pState = pState.Trim();
                BizAction.Parent = new KeyValue { Key = "State", Value = pState };
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    txtCity.ItemsSource = null;
                    txtCity.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List;
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillAreaList(string pCity)
        {
            clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();

            BizAction.TableName = "T_Registration";
            BizAction.ColumnName = "Area";
            BizAction.IsDecode = false;

            if (!string.IsNullOrEmpty(pCity))
            {
                pCity = pCity.Trim();
                BizAction.Parent = new KeyValue { Key = "City", Value = pCity };
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    txtArea.ItemsSource = null;
                    txtArea.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List;
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillGender()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_GenderMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "--Select--"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbGender.ItemsSource = null;
                    cmbGender.ItemsSource = objList;
                    cmbGender.SelectedItem = objList[0];
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillMaritalStatus()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_MaritalStatusMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "--Select--"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbMaritalStatus.ItemsSource = null;
                    cmbMaritalStatus.ItemsSource = objList;
                    cmbMaritalStatus.SelectedItem = objList[0];
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillLoyaltyProgram()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_LoyaltyProgramMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "--Select--"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbLoyaltyProgram.ItemsSource = null;
                    cmbLoyaltyProgram.ItemsSource = objList;
                    cmbLoyaltyProgram.SelectedItem = objList[0];
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillComplaint()
        {
            clsGetEMRTemplateListBizActionVO BizAction = new clsGetEMRTemplateListBizActionVO();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    BizAction.objEMRTemplateList = ((clsGetEMRTemplateListBizActionVO)arg.Result).objEMRTemplateList;

                    BizAction.objEMRTemplateList.Insert(0, new clsEMRTemplateVO()
                    {
                        Title = "--Select--"
                    }
                    );
                    cmbComplaint.ItemsSource = BizAction.objEMRTemplateList;
                    cmbComplaint.SelectedItem = BizAction.objEMRTemplateList[0];
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        public static List<MasterListItem> FillAge()
        {
            List<MasterListItem> objAge = new List<MasterListItem>();
            objAge.Add(new MasterListItem(0, "Select"));
            objAge.Add(new MasterListItem(1, "="));
            objAge.Add(new MasterListItem(2, "<"));
            objAge.Add(new MasterListItem(3, ">"));

            return objAge;
        }

        private void FillUnitList()
        {
            try
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
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

                        cmbClinic.ItemsSource = null;
                        cmbClinic.ItemsSource = objList;

                        if (!((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                        {

                            var res = from r in objList
                                      where r.ID == User.UserLoginInfo.UnitId
                                      select r;
                            cmbClinic.SelectedItem = ((MasterListItem)res.First());
                            cmbClinic.IsEnabled = false;
                        }
                        else
                            cmbClinic.SelectedItem = objList[0];

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

        private void FillDepartmentList(long iUnitId)
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_DepartmentUnitView;

            if (iUnitId > 0)
                BizAction.Parent = new KeyValue { Key = iUnitId, Value = "UnitId" };
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



                    if (((MasterListItem)cmbClinic.SelectedItem).ID == 0)
                    {

                        var results = from a in objList
                                      group a by a.ID into grouped
                                      select grouped.First();
                        objList = results.ToList();
                    }

                    
                    cmbDepartment.ItemsSource = null;
                    cmbDepartment.ItemsSource = objList;
                    cmbDepartment.SelectedItem = objList[0];
                }
            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
         
        private void FillDoctor(long IUnitId, long iDeptId)
        {
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();

            if ((MasterListItem)cmbClinic.SelectedItem != null)
            {
                BizAction.UnitId = IUnitId;
            }
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
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);

                    cmbDoctor.ItemsSource = null;
                    cmbDoctor.ItemsSource = objList;
                    cmbDoctor.SelectedItem = objList[0];
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        #endregion

        private void cmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if ((MasterListItem)cmbClinic.SelectedItem != null)
                    FillDepartmentList(((MasterListItem)cmbClinic.SelectedItem).ID);                
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if ((MasterListItem)cmbDepartment.SelectedItem != null)
                    FillDoctor(((MasterListItem)cmbClinic.SelectedItem).ID, ((MasterListItem)cmbDepartment.SelectedItem).ID);                
            }
            catch (Exception ex)
            {
                throw;
            }
        }

   

        #region  Validation
        private void txtState_LostFocus(object sender, RoutedEventArgs e)
        {
            txtState.Text = txtState.Text.ToTitleCase();
            FillCityList(txtState.Text);
        }

        private void txtCity_LostFocus(object sender, RoutedEventArgs e)
        {
            txtCity.Text = txtCity.Text.ToTitleCase();
            FillAreaList(txtState.Text);
        }

        private void txtFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFirstName.Text = txtFirstName.Text.ToTitleCase();
        }

        private void txtMiddleName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtMiddleName.Text = txtMiddleName.Text.ToTitleCase();
        }

        private void txtLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtLastName.Text = txtLastName.Text.ToTitleCase();
        }

        private void txtArea_LostFocus(object sender, RoutedEventArgs e)
        {
            txtArea.Text = txtArea.Text.ToTitleCase();
        }

        #endregion

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            //DateTime? dtFT = null;
            //DateTime? dtTT = null;
            //DateTime? dtTP = null;

            Nullable<DateTime> dtFT = null;
            Nullable<DateTime> dtTT = null;
            Nullable<DateTime> dtTP = null;

            string MRNO = null;
            string OPDNo = null;
            string FirstName = null;
            string MiddleName = null;
            string LastName = null;
            string AgeFilter = null;
            string State = null;
            string City = null;
            string Area = null;
            string ContactNo = null;
            long UnitID = 0;
            long DepartmentID = 0;
            long DoctorID = 0;
            long GenderID = 0;
            long MaritalStatusID = 0;
            long LoyaltyCardID = 0;
            long ComplaintID = 0;
            int Age = 0;
            bool IsExporttoExcel = false;
            bool chkToDate = true;
            string msgTitle = "";
            if (dtpFromDate.SelectedDate != null)
            {
                dtFT = dtpFromDate.SelectedDate.Value.Date.Date;
            }

            if (dtpToDate.SelectedDate != null)
            {
                dtTT = dtpToDate.SelectedDate.Value.Date.Date;
                if (dtFT.Value > dtTT.Value)
                {
                    dtpToDate.SelectedDate = dtpFromDate.SelectedDate;
                    dtTP = dtFT;
                    chkToDate = false;
                }
                else
                {
                    dtTP = dtTT;
                    //dtTT = dtTP.Value.Date.AddDays(1);
                    dtTT = dtTP.Value.AddDays(1);
                }

            }

            if (dtTT != null)
            {
                if (dtFT != null)
                {
                    dtFT = dtpFromDate.SelectedDate.Value.Date.Date;

                    //if (dtpF.Equals(dtpT))
                    //    dtpT = dtpF.Value.Date.AddDays(1);
                }
            }
            
            if (txtMrno.Text != "")
            {
                MRNO = txtMrno.Text;
            }
            if (txtOPDNo.Text != "")
            {
                OPDNo = txtOPDNo.Text;
            }
            if (txtFirstName.Text != null)
            {
                FirstName = txtFirstName.Text;
            }
            if (txtMiddleName.Text != null)
            {

                MiddleName = txtMiddleName.Text;
            }
            if (txtLastName.Text != null)
            {
                LastName = txtLastName.Text;
            }

            if (cmbClinic.SelectedItem != null)
                UnitID = ((MasterListItem)cmbClinic.SelectedItem).ID;
            if (cmbDepartment.SelectedItem != null)
                DepartmentID = ((MasterListItem)cmbDepartment.SelectedItem).ID;
            if (cmbDoctor.SelectedItem != null)
                DoctorID = ((MasterListItem)cmbDoctor.SelectedItem).ID;

            if (txtState.Text != null)
                State = txtState.Text;
            if (txtCity.Text != null)
                City = txtCity.Text;
            if (txtArea.Text != null)
                Area = txtArea.Text;
            if (cmbGender.SelectedItem != null)
                GenderID = ((MasterListItem)cmbGender.SelectedItem).ID;
            if (cmbMaritalStatus.SelectedItem != null)
                MaritalStatusID = ((MasterListItem)cmbMaritalStatus.SelectedItem).ID;
            if (txtContactNo.Text != null)
                ContactNo = txtContactNo.Text;
            if (cmbLoyaltyProgram.SelectedItem != null)
                LoyaltyCardID = ((MasterListItem)cmbLoyaltyProgram.SelectedItem).ID;

            if (cmbComplaint.SelectedItem != null)
                ComplaintID = ((clsEMRTemplateVO)cmbComplaint.SelectedItem).TemplateID;

            if (cmbAge.SelectedItem != null)
                AgeFilter = ((MasterListItem)cmbAge.SelectedItem).Description;

            if (txtAge.Text.Trim() != "")
            {
                Age = int.Parse(txtAge.Text);
            }
            IsExporttoExcel = (bool)chkExporttoExcel.IsChecked;

            if (chkToDate == true)
            {
                string URL;
                if (dtFT != null && dtTT != null && dtTP != null)
                {
                    URL = "../Reports/CRM/SearchPatientReport.aspx?FromDate=" + dtFT.Value.ToString("dd/MMM/yyyy") + "&ToDate=" + dtTT.Value.ToString("dd/MMM/yyyy") + "&ClinicID=" + UnitID + "&DepartmentID=" + DepartmentID + "&DoctorID=" + DoctorID + "&MRNO=" + MRNO + "&OPDNO=" + OPDNo + "&FirstName=" + FirstName + "&MiddleName=" + MiddleName + "&LastName=" + LastName + "&LoyaltyCardID=" + LoyaltyCardID + "&State=" + State + "&City=" + City + "&Area=" + Area + "&ContactNo=" + ContactNo + "&AgeFilter=" + AgeFilter + "&Age=" + Age + "&GenderID=" + GenderID + "&MaritalStatusID=" + MaritalStatusID + "&ComplaintID=" + ComplaintID + "&ToDatePrint=" + dtTP.Value.ToString("dd/MMM/yyyy") + "&IsExporttoExcel=" + IsExporttoExcel;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
                else
                {
                    URL = "../Reports/CRM/SearchPatientReport.aspx?ClinicID=" + UnitID + "&DepartmentID=" + DepartmentID + "&DoctorID=" + DoctorID + "&MRNO=" + MRNO + "&OPDNO=" + OPDNo + "&FirstName=" + FirstName + "&MiddleName=" + MiddleName + "&LastName=" + LastName + "&LoyaltyCardID=" + LoyaltyCardID + "&State=" + State + "&City=" + City + "&Area=" + Area + "&ContactNo=" + ContactNo + "&AgeFilter=" + AgeFilter + "&Age=" + Age + "&GenderID=" + GenderID + "&MaritalStatusID=" + MaritalStatusID + "&ComplaintID=" + ComplaintID + dtTP + "&IsExporttoExcel=" + IsExporttoExcel;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");

                }
            }
            else
            {
                string msgText = "Incorrect Date Range. From Date cannot be greater than To Date.";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
            } 
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.MIS.CRM.CRMReport") as UIElement;
            ((IApplicationConfiguration)App.Current).OpenMainContent(myData);

        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if ((bool)chkExporttoExcel.IsChecked)
            {
                Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                client.DeleteMISReportFileCompleted += (s1, args1) =>
                {
                    if (args1.Error == null)
                    {
                    }
                };
                client.DeleteMISReportFileAsync("/Reports/CRM");
            }
        }

        private void cmbAge_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbAge.SelectedItem != null && ((MasterListItem)cmbAge.SelectedItem).ID != 0)
            {
                txtAge.IsReadOnly = false;
            }
            else
            {
                txtAge.IsReadOnly = true;
                txtAge.Text = string.Empty;
            }

        }

        private void cmbClinic_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((MasterListItem)cmbClinic.SelectedItem == null)
                cmbClinic.SelectedValue = (long)0;

        }

        private void cmbDepartment_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((MasterListItem)cmbDepartment.SelectedItem == null)
                cmbDepartment.SelectedValue = (long)0;
        }

        private void cmbDoctor_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((MasterListItem)cmbDoctor.SelectedItem == null)
                cmbDoctor.SelectedValue = (long)0;
        }

        private void cmbLoyaltyProgram_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((MasterListItem)cmbLoyaltyProgram.SelectedItem == null)
                cmbLoyaltyProgram.SelectedValue = (long)0;
        }

        private void cmbGender_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((MasterListItem)cmbGender.SelectedItem == null)
                cmbGender.SelectedValue = (long)0;
        }

        private void cmbMaritalStatus_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((MasterListItem)cmbMaritalStatus.SelectedItem == null)
                cmbMaritalStatus.SelectedValue = (long)0;
        }

        //private void cmbComplaint_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    if ((clsEMRTemplateVO)cmbComplaint.SelectedItem == null)
        //        cmbComplaint.SelectedValue = (long)0;
        //}

        
      
    }
}
