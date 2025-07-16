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
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using CIMS;
using System.Windows.Media.Imaging;
using System.Windows.Browser;
using PalashDynamics.ValueObjects.DashBoardVO;
using System.IO;
using System.Reflection;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class frmAddObservation : ChildWindow
    {
        public DateTime date;
        public long OocyteNo;
        public long SerialOocNo;
        public clsCoupleVO CoupleDetails;
        public long PlanTherapyId;
        public long PlanTherapyUnitId;
        public long DecisionID;
        public DateTime? ProcedureDate;
        public DateTime? ProcedureTime;
        public long Day = 0;
        double hours = 0;
        public bool IsSaved = false;

        System.Collections.ObjectModel.ObservableCollection<string> AttachedFileNameList = new System.Collections.ObjectModel.ObservableCollection<string>();
        public cls_IVFDashboard_GraphicalRepresentationVO SelectedDay = new cls_IVFDashboard_GraphicalRepresentationVO();
        public List<cls_IVFDashboard_GraphicalRepresentationVO> GraphicalOocDetailsList = new List<cls_IVFDashboard_GraphicalRepresentationVO>();

        //added by neena dated 18/5/16
        public long addSerialOocNo;
        List<MasterListItem> OocyteList = new List<MasterListItem>();
        List<MasterListItem> SelectedOocyteList = new List<MasterListItem>();
        public bool IsIsthambul;
        public bool IsClosed;
        //

        public bool FrozenPGDPGS = false;

        public bool Day3PGDPGS = false;
        public bool Day5PGDPGS = false;
        public bool Day6PGDPGS = false;
        public bool Day7PGDPGS = false;

        public event RoutedEventHandler OnSaveButton_Click;

        public frmAddObservation()
        {
            InitializeComponent();
        }

        private bool Validate()
        {
            bool result = true;
            if (dtObservationDate.SelectedDate == null)
            {
                dtObservationDate.SetValidation("Please Select Date");
                dtObservationDate.RaiseValidationError();
                dtObservationDate.Focus();
                result = false;
            }
            else
                dtObservationDate.ClearValidationError();

            if (dtObservationTime.Value == null)
            {
                dtObservationTime.SetValidation("Please Select Time");
                dtObservationTime.RaiseValidationError();
                dtObservationTime.Focus();
                result = false;
            }
            else
                dtObservationTime.ClearValidationError();

            if (IsIsthambul == true && Day != 5 && Day != 6 && Day != 7)
            {
                if (txtCellStage.Text.Trim() == string.Empty)
                {
                    txtCellStage.SetValidation("Please Select Cell Stage");
                    txtCellStage.RaiseValidationError();
                    txtCellStage.Focus();
                    result = false;
                }
                else
                    txtCellStage.ClearValidationError();
            }

            if (cmbCellNo.ItemsSource != null)
            {
                if (cmbCellNo.SelectedItem == null)
                {
                    cmbCellNo.TextBox.SetValidation("Please select Cell Number");
                    cmbCellNo.TextBox.RaiseValidationError();
                    cmbCellNo.Focus();
                    result = false;
                }
                else if (((MasterListItem)cmbCellNo.SelectedItem).ID == 0)
                {
                    cmbCellNo.TextBox.SetValidation("Please select Cell Number");
                    cmbCellNo.TextBox.RaiseValidationError();
                    cmbCellNo.Focus();
                    result = false;
                }
                else
                    cmbCellNo.TextBox.ClearValidationError();
            }

            if (chkSendForBiopsy.IsChecked == true)
            {
                if (dtDate.SelectedDate == null)
                {
                    dtDate.SetValidation("Please Select Date");
                    dtDate.RaiseValidationError();
                    dtDate.Focus();
                    result = false;
                }
                else
                    dtDate.ClearValidationError();

                if (dtTime.Value == null)
                {
                    dtTime.SetValidation("Please Select Time");
                    dtTime.RaiseValidationError();
                    dtTime.Focus();
                    result = false;
                }
                else
                    dtTime.ClearValidationError();

                if (txtNoOfCell.Text.Trim() == string.Empty)
                {
                    txtNoOfCell.SetValidation("Please Select Cell Stage");
                    txtNoOfCell.RaiseValidationError();
                    txtNoOfCell.Focus();
                    result = false;
                }
                else
                    txtNoOfCell.ClearValidationError();
            }

            if (dtObservationDate.SelectedDate < ProcedureDate)
            {
                dtObservationDate.SetValidation("Observation Date Cannot Be Less Than Procedure Date");
                dtObservationDate.RaiseValidationError();
                dtObservationDate.Focus();
                dtObservationDate.Text = " ";
                dtObservationDate.Focus();
                result = false;
            }
            else
                dtObservationDate.ClearValidationError();

            if (dtDate.SelectedDate < ProcedureDate)
            {
                dtDate.SetValidation("Biopsy Date Cannot Be Less Than Procedure Date");
                dtDate.RaiseValidationError();
                dtDate.Focus();
                dtDate.Text = " ";
                dtDate.Focus();
                result = false;
            }
            else
                dtDate.ClearValidationError();

            if (Day == 5 || Day == 6 || Day == 7)
            {
                if (cmbStageofDevelopmentGrade.ItemsSource != null)
                {
                    if (cmbStageofDevelopmentGrade.SelectedItem == null)
                    {
                        cmbStageofDevelopmentGrade.TextBox.SetValidation("Please select Stage of Development Grade");
                        cmbStageofDevelopmentGrade.TextBox.RaiseValidationError();
                        cmbStageofDevelopmentGrade.Focus();
                        result = false;
                    }
                    else if (((MasterListItem)cmbStageofDevelopmentGrade.SelectedItem).ID == 0)
                    {
                        cmbStageofDevelopmentGrade.TextBox.SetValidation("Please select Stage of Development Grade");
                        cmbStageofDevelopmentGrade.TextBox.RaiseValidationError();
                        cmbStageofDevelopmentGrade.Focus();
                        result = false;
                    }
                    else
                        cmbStageofDevelopmentGrade.TextBox.ClearValidationError();
                }

                if (cmbInnerCellMassGrade.ItemsSource != null)
                {
                    if (cmbInnerCellMassGrade.SelectedItem == null)
                    {
                        cmbInnerCellMassGrade.TextBox.SetValidation("Please select Inner Cell Mass Grade");
                        cmbInnerCellMassGrade.TextBox.RaiseValidationError();
                        cmbInnerCellMassGrade.Focus();
                        result = false;
                    }
                    else if (((MasterListItem)cmbInnerCellMassGrade.SelectedItem).ID == 0)
                    {
                        cmbInnerCellMassGrade.TextBox.SetValidation("Please select Inner Cell Mass Grade");
                        cmbInnerCellMassGrade.TextBox.RaiseValidationError();
                        cmbInnerCellMassGrade.Focus();
                        result = false;
                    }
                    else
                        cmbStageofDevelopmentGrade.TextBox.ClearValidationError();
                }

                if (cmbTrophoectodermGrade.ItemsSource != null)
                {
                    if (cmbTrophoectodermGrade.SelectedItem == null)
                    {
                        cmbTrophoectodermGrade.TextBox.SetValidation("Please select Trophoectoderm Grade");
                        cmbTrophoectodermGrade.TextBox.RaiseValidationError();
                        cmbTrophoectodermGrade.Focus();
                        result = false;
                    }
                    else if (((MasterListItem)cmbTrophoectodermGrade.SelectedItem).ID == 0)
                    {
                        cmbTrophoectodermGrade.TextBox.SetValidation("Please select Trophoectoderm Grade");
                        cmbTrophoectodermGrade.TextBox.RaiseValidationError();
                        cmbTrophoectodermGrade.Focus();
                        result = false;
                    }
                    else
                        cmbTrophoectodermGrade.TextBox.ClearValidationError();
                }
            }
            else if (Day == 2)
            {
                if (cmbGrade2.ItemsSource != null)
                {
                    if (cmbGrade2.SelectedItem == null)
                    {
                        cmbGrade2.TextBox.SetValidation("Please select Grade");
                        cmbGrade2.TextBox.RaiseValidationError();
                        cmbGrade2.Focus();
                        result = false;
                    }
                    else if (((MasterListItem)cmbGrade2.SelectedItem).ID == 0)
                    {
                        cmbGrade2.TextBox.SetValidation("Please select Grade");
                        cmbGrade2.TextBox.RaiseValidationError();
                        cmbGrade2.Focus();
                        result = false;
                    }
                    else
                        cmbGrade2.TextBox.ClearValidationError();
                }
            }
            else if (Day == 3)
            {
                if (rdoYes.IsChecked == false && rdoNo.IsChecked == false)
                {
                    rdoYes.SetValidation("Please select Embryo Compacted");
                    rdoYes.RaiseValidationError();
                    rdoYes.Focus();
                    result = false;
                }

                if (rdoYes.IsChecked == true)
                {
                    if (cmbGrade3.ItemsSource != null)
                    {
                        if (cmbGrade3.SelectedItem == null)
                        {
                            cmbGrade3.TextBox.SetValidation("Please select Grade");
                            cmbGrade3.TextBox.RaiseValidationError();
                            cmbGrade3.Focus();
                            result = false;
                        }
                        else if (((MasterListItem)cmbGrade3.SelectedItem).ID == 0)
                        {
                            cmbGrade3.TextBox.SetValidation("Please select Grade");
                            cmbGrade3.TextBox.RaiseValidationError();
                            cmbGrade3.Focus();
                            result = false;
                        }
                        else
                            cmbGrade3.TextBox.ClearValidationError();
                    }
                }

                if (rdoNo.IsChecked == true)
                {
                    if (cmbGrade2.ItemsSource != null)
                    {
                        if (cmbGrade2.SelectedItem == null)
                        {
                            cmbGrade2.TextBox.SetValidation("Please select Grade");
                            cmbGrade2.TextBox.RaiseValidationError();
                            cmbGrade2.Focus();
                            result = false;
                        }
                        else if (((MasterListItem)cmbGrade2.SelectedItem).ID == 0)
                        {
                            cmbGrade2.TextBox.SetValidation("Please select Grade");
                            cmbGrade2.TextBox.RaiseValidationError();
                            cmbGrade2.Focus();
                            result = false;
                        }
                        else
                            cmbGrade2.TextBox.ClearValidationError();
                    }
                }
            }
            else if (Day == 4)
            {
                if (cmbGrade4.ItemsSource != null)
                {
                    if (cmbGrade4.SelectedItem == null)
                    {
                        cmbGrade4.TextBox.SetValidation("Please select Grade");
                        cmbGrade4.TextBox.RaiseValidationError();
                        cmbGrade4.Focus();
                        result = false;
                    }
                    else if (((MasterListItem)cmbGrade4.SelectedItem).ID == 0)
                    {
                        cmbGrade4.TextBox.SetValidation("Please select Grade");
                        cmbGrade4.TextBox.RaiseValidationError();
                        cmbGrade4.Focus();
                        result = false;
                    }
                    else
                        cmbGrade4.TextBox.ClearValidationError();
                }
            }

            //if (result == true)
            //{
            //    if (mylistitem.Count == 0)
            //    {
            //        MessageBoxControl.MessageBoxChildWindow msgWindow =
            //                new MessageBoxControl.MessageBoxChildWindow("Palash", "Please add image.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //        msgWindow.Show();
            //        result = false;
            //    }
            //}

            return result;
        }

        //added by neena       

        private void FillDay1OocyteDate()
        {
            clsIVFDashboard_GetDay1OocyteDetailsBizActionVO BizAction = new clsIVFDashboard_GetDay1OocyteDetailsBizActionVO();
            BizAction.IsDay1Oocyte = true;
            BizAction.Details = new clsIVFDashboard_LabDaysVO();
            BizAction.Details.PlanTherapyID = PlanTherapyId;
            BizAction.Details.PlanTherapyUnitID = PlanTherapyUnitId;
            BizAction.Details.SerialOocyteNumber = OocyteNo;
            //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);           

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsIVFDashboard_GetDay1OocyteDetailsBizActionVO)arg.Result).Oocytelist != null)
                    {
                        //  by neena
                        List<clsIVFDashboard_LabDaysVO> mlist = ((clsIVFDashboard_GetDay1OocyteDetailsBizActionVO)arg.Result).Oocytelist;
                        List<MasterListItem> missingVehicle1 = new List<MasterListItem>();
                        List<MasterListItem> TempobjList = new List<MasterListItem>();

                        foreach (var item in objList)
                        {
                            foreach (var item1 in GraphicalOocDetailsList)
                            {
                                if (item.ID == item1.OocyteNumber)
                                {
                                    if (item1.Day2 == true || item1.Day3 == true || item1.Day4 == true || item1.Day5 == true || item1.Day6 == true || item1.Day7 == true || item1.DecisionID>0)
                                    {

                                    }
                                    else
                                        TempobjList.Add(item);
                                }
                            }
                        }

                        objList = TempobjList;
                        var productFirstChars =
                                               from p in objList
                                               select p.ID;
                        var customerFirstChars =
                                                from c in mlist
                                                //where (c.Isfreezed == true)
                                                select c.OocyteNumber
                                                ;

                        //by neena
                        var list = objList.Where(a => !mlist.Any(b => b.OocyteNumber == a.ID)).ToList();

                        //var missingVehicles = productFirstChars.Intersect(customerFirstChars);
                        MasterListItem newItem = new MasterListItem();
                        foreach (var item in list)
                        {
                            string str = "";
                            str = "Oocyte Number " + item.ID;
                            long id = item.ID;
                            newItem = new MasterListItem(item.ID, str);

                            missingVehicle1.Add(newItem);
                        }

                        cmbApplay.ItemsSource = null;
                        cmbApplay.ItemsSource = missingVehicle1;
                        //cmbApplay.SelectedItem = missingVehicles[0];  


                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillDay2OocyteDate()
        {
            clsIVFDashboard_GetDay2OocyteDetailsBizActionVO BizAction = new clsIVFDashboard_GetDay2OocyteDetailsBizActionVO();
            BizAction.Details = new clsIVFDashboard_LabDaysVO();
            BizAction.Details.PlanTherapyID = PlanTherapyId;
            BizAction.Details.PlanTherapyUnitID = PlanTherapyUnitId;
            BizAction.Details.SerialOocyteNumber = OocyteNo;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsIVFDashboard_GetDay2OocyteDetailsBizActionVO)arg.Result).Oocytelist != null)
                    {
                        //  by neena
                        List<clsIVFDashboard_LabDaysVO> mlist = ((clsIVFDashboard_GetDay2OocyteDetailsBizActionVO)arg.Result).Oocytelist;
                        List<MasterListItem> missingVehicle1 = new List<MasterListItem>();

                        List<MasterListItem> TempobjList = new List<MasterListItem>();

                        foreach (var item in objList)
                        {
                            foreach (var item1 in GraphicalOocDetailsList)
                            {
                                if (item.ID == item1.OocyteNumber)
                                {
                                    if (item1.Day3 == true || item1.Day4 == true || item1.Day5 == true || item1.Day6 == true || item1.Day7 == true || item1.DecisionID > 0)
                                    {

                                    }
                                    else
                                        TempobjList.Add(item);
                                }
                            }
                        }

                        objList = TempobjList;

                        var productFirstChars =
                                               from p in objList
                                               select p.ID;
                        var customerFirstChars =
                                                from c in mlist
                                                //where (c.Isfreezed == true)
                                                select c.OocyteNumber
                                                ;

                        //by neena
                        var list = objList.Where(a => !mlist.Any(b => b.OocyteNumber == a.ID)).ToList();

                        //var missingVehicles = productFirstChars.Intersect(customerFirstChars);
                        MasterListItem newItem = new MasterListItem();
                        foreach (var item in list)
                        {
                            string str = "";
                            str = "Oocyte Number " + item.ID;
                            long id = item.ID;
                            newItem = new MasterListItem(item.ID, str);

                            missingVehicle1.Add(newItem);
                        }

                        cmbApplay.ItemsSource = null;
                        cmbApplay.ItemsSource = missingVehicle1;
                        //cmbApplay.SelectedItem = missingVehicles[0]; 


                        ////  by neena
                        //List<clsIVFDashboard_LabDaysVO> mlist = ((clsIVFDashboard_GetDay2OocyteDetailsBizActionVO)arg.Result).Oocytelist;
                        //List<MasterListItem> missingVehicle1 = new List<MasterListItem>();
                        //var productFirstChars =
                        //                       from p in objList
                        //                       select p.ID;
                        //var customerFirstChars =
                        //                        from c in mlist
                        //                        where (c.Isfreezed == false)
                        //                        select c.OocyteNumber
                        //                        ;

                        ////var missingVehicles = productFirstChars.Except(customerFirstChars);
                        ////by neena
                        //var missingVehicles = productFirstChars.Intersect(customerFirstChars);
                        //// var missingVehicles = customerFirstChars.Except(productFirstChars);
                        ////List<MasterListItem> missingVehicles = new List<MasterListItem>(
                        ////var missingVehicles = from uV in objList
                        ////                      from de in ((clsIVFDashboard_GetDay1OocyteDetailsBizActionVO)arg.Result).Oocytelist
                        ////                      where uV.ID != de.OocyteNumber
                        ////                      select uV;

                        //MasterListItem newItem = new MasterListItem();
                        //foreach (var item in missingVehicles)
                        //{
                        //    string str = "";
                        //    str = "Oocyte Number " + item;
                        //    long id = item;
                        //    newItem = new MasterListItem(item, str);

                        //    missingVehicle1.Add(newItem);
                        //}

                        //cmbApplay.ItemsSource = null;
                        //cmbApplay.ItemsSource = missingVehicle1;
                        ////cmbApplay.SelectedItem = missingVehicles[0];  
                      
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillDay3OocyteDate()
        {
            clsIVFDashboard_GetDay3OocyteDetailsBizActionVO BizAction = new clsIVFDashboard_GetDay3OocyteDetailsBizActionVO();
            BizAction.Details = new clsIVFDashboard_LabDaysVO();
            BizAction.Details.PlanTherapyID = PlanTherapyId;
            BizAction.Details.PlanTherapyUnitID = PlanTherapyUnitId;
            BizAction.Details.SerialOocyteNumber = OocyteNo;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsIVFDashboard_GetDay3OocyteDetailsBizActionVO)arg.Result).Oocytelist != null)
                    {
                        //  by neena
                        List<clsIVFDashboard_LabDaysVO> mlist = ((clsIVFDashboard_GetDay3OocyteDetailsBizActionVO)arg.Result).Oocytelist;
                        List<MasterListItem> missingVehicle1 = new List<MasterListItem>();

                        List<MasterListItem> TempobjList = new List<MasterListItem>();

                        foreach (var item in objList)
                        {
                            foreach (var item1 in GraphicalOocDetailsList)
                            {
                                if (item.ID == item1.OocyteNumber)
                                {
                                    if (item1.Day4 == true || item1.Day5 == true || item1.Day6 == true || item1.Day7 == true || item1.DecisionID > 0)
                                    {

                                    }
                                    else
                                        TempobjList.Add(item);
                                }
                            }
                        }

                        objList = TempobjList;

                        var productFirstChars =
                                               from p in objList
                                               select p.ID;
                        var customerFirstChars =
                                                from c in mlist
                                                //where (c.Isfreezed == true)
                                                select c.OocyteNumber
                                                ;

                        //by neena
                        var list = objList.Where(a => !mlist.Any(b => b.OocyteNumber == a.ID)).ToList();

                        //var missingVehicles = productFirstChars.Intersect(customerFirstChars);
                        MasterListItem newItem = new MasterListItem();
                        foreach (var item in list)
                        {
                            string str = "";
                            str = "Oocyte Number " + item.ID;
                            long id = item.ID;
                            newItem = new MasterListItem(item.ID, str);

                            missingVehicle1.Add(newItem);
                        }

                        cmbApplay.ItemsSource = null;
                        cmbApplay.ItemsSource = missingVehicle1;
                        //cmbApplay.SelectedItem = missingVehicles[0]; 

                        ////  by neena
                        //List<clsIVFDashboard_LabDaysVO> mlist = ((clsIVFDashboard_GetDay3OocyteDetailsBizActionVO)arg.Result).Oocytelist;
                        //List<MasterListItem> missingVehicle1 = new List<MasterListItem>();
                        //var productFirstChars =
                        //                       from p in objList
                        //                       select p.ID;
                        //var customerFirstChars =
                        //                        from c in mlist
                        //                        where (c.Isfreezed == false)
                        //                        select c.OocyteNumber
                        //                        ;

                        //// var missingVehicles = productFirstChars.Except(customerFirstChars);
                        ////by neena
                        //var missingVehicles = productFirstChars.Intersect(customerFirstChars);
                        ////List<MasterListItem> missingVehicles = new List<MasterListItem>(
                        ////var missingVehicles = from uV in objList
                        ////                      from de in ((clsIVFDashboard_GetDay1OocyteDetailsBizActionVO)arg.Result).Oocytelist
                        ////                      where uV.ID != de.OocyteNumber
                        ////                      select uV;

                        //MasterListItem newItem = new MasterListItem();
                        //foreach (var item in missingVehicles)
                        //{
                        //    string str = "";
                        //    str = "Oocyte Number " + item;
                        //    long id = item;
                        //    newItem = new MasterListItem(item, str);

                        //    missingVehicle1.Add(newItem);
                        //}

                        //cmbApplay.ItemsSource = null;
                        //cmbApplay.ItemsSource = missingVehicle1;
                        ////cmbApplay.SelectedItem = missingVehicles[0];  


                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillDay4OocyteDate()
        {
            clsIVFDashboard_GetDay4OocyteDetailsBizActionVO BizAction = new clsIVFDashboard_GetDay4OocyteDetailsBizActionVO();
            BizAction.Details = new clsIVFDashboard_LabDaysVO();
            BizAction.Details.PlanTherapyID = PlanTherapyId;
            BizAction.Details.PlanTherapyUnitID = PlanTherapyUnitId;
            BizAction.Details.SerialOocyteNumber = OocyteNo;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsIVFDashboard_GetDay4OocyteDetailsBizActionVO)arg.Result).Oocytelist != null)
                    {
                        //  by neena
                        List<clsIVFDashboard_LabDaysVO> mlist = ((clsIVFDashboard_GetDay4OocyteDetailsBizActionVO)arg.Result).Oocytelist;
                        List<MasterListItem> missingVehicle1 = new List<MasterListItem>();

                        List<MasterListItem> TempobjList = new List<MasterListItem>();

                        foreach (var item in objList)
                        {
                            foreach (var item1 in GraphicalOocDetailsList)
                            {
                                if (item.ID == item1.OocyteNumber)
                                {
                                    if (item1.Day5 == true || item1.Day6 == true || item1.Day7 == true || item1.DecisionID > 0)
                                    {

                                    }
                                    else
                                        TempobjList.Add(item);
                                }
                            }
                        }

                        objList = TempobjList;
                        var productFirstChars =
                                               from p in objList
                                               select p.ID;
                        var customerFirstChars =
                                                from c in mlist
                                                //where (c.Isfreezed == true)
                                                select c.OocyteNumber
                                                ;

                        //by neena
                        var list = objList.Where(a => !mlist.Any(b => b.OocyteNumber == a.ID)).ToList();

                        //var missingVehicles = productFirstChars.Intersect(customerFirstChars);
                        MasterListItem newItem = new MasterListItem();
                        foreach (var item in list)
                        {
                            string str = "";
                            str = "Oocyte Number " + item.ID;
                            long id = item.ID;
                            newItem = new MasterListItem(item.ID, str);

                            missingVehicle1.Add(newItem);
                        }

                        cmbApplay.ItemsSource = null;
                        cmbApplay.ItemsSource = missingVehicle1;
                        //cmbApplay.SelectedItem = missingVehicles[0]; 

                        ////  by neena
                        //List<clsIVFDashboard_LabDaysVO> mlist = ((clsIVFDashboard_GetDay4OocyteDetailsBizActionVO)arg.Result).Oocytelist;
                        //List<MasterListItem> missingVehicle1 = new List<MasterListItem>();
                        //var productFirstChars =
                        //                       from p in objList
                        //                       select p.ID;
                        //var customerFirstChars =
                        //                        from c in mlist
                        //                        where (c.Isfreezed == false)
                        //                        select c.OocyteNumber
                        //                        ;

                        ////var missingVehicles = productFirstChars.Except(customerFirstChars);
                        ////by neena
                        //var missingVehicles = productFirstChars.Intersect(customerFirstChars);
                        //// var missingVehicles = customerFirstChars.Except(productFirstChars);
                        ////List<MasterListItem> missingVehicles = new List<MasterListItem>(
                        ////var missingVehicles = from uV in objList
                        ////                      from de in ((clsIVFDashboard_GetDay1OocyteDetailsBizActionVO)arg.Result).Oocytelist
                        ////                      where uV.ID != de.OocyteNumber
                        ////                      select uV;

                        //MasterListItem newItem = new MasterListItem();
                        //foreach (var item in missingVehicles)
                        //{
                        //    string str = "";
                        //    str = "Oocyte Number " + item;
                        //    long id = item;
                        //    newItem = new MasterListItem(item, str);

                        //    missingVehicle1.Add(newItem);
                        //}

                        //cmbApplay.ItemsSource = null;
                        //cmbApplay.ItemsSource = missingVehicle1;
                        ////cmbApplay.SelectedItem = missingVehicles[0];  


                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillDay5OocyteDate()
        {
            clsIVFDashboard_GetDay5OocyteDetailsBizActionVO BizAction = new clsIVFDashboard_GetDay5OocyteDetailsBizActionVO();
            BizAction.Details = new clsIVFDashboard_LabDaysVO();
            BizAction.Details.PlanTherapyID = PlanTherapyId;
            BizAction.Details.PlanTherapyUnitID = PlanTherapyUnitId;
            BizAction.Details.SerialOocyteNumber = OocyteNo;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsIVFDashboard_GetDay5OocyteDetailsBizActionVO)arg.Result).Oocytelist != null)
                    {
                        //  by neena
                        List<clsIVFDashboard_LabDaysVO> mlist = ((clsIVFDashboard_GetDay5OocyteDetailsBizActionVO)arg.Result).Oocytelist;
                        List<MasterListItem> missingVehicle1 = new List<MasterListItem>();

                        List<MasterListItem> TempobjList = new List<MasterListItem>();

                        foreach (var item in objList)
                        {
                            foreach (var item1 in GraphicalOocDetailsList)
                            {
                                if (item.ID == item1.OocyteNumber)
                                {
                                    if (item1.Day6 == true || item1.Day7 == true || item1.DecisionID > 0)
                                    {

                                    }
                                    else
                                        TempobjList.Add(item);
                                }
                            }
                        }

                        objList = TempobjList;
                        var productFirstChars =
                                               from p in objList
                                               select p.ID;
                        var customerFirstChars =
                                                from c in mlist
                                                //where (c.Isfreezed == true)
                                                select c.OocyteNumber
                                                ;

                        //by neena
                        var list = objList.Where(a => !mlist.Any(b => b.OocyteNumber == a.ID)).ToList();

                        //var missingVehicles = productFirstChars.Intersect(customerFirstChars);
                        MasterListItem newItem = new MasterListItem();
                        foreach (var item in list)
                        {
                            string str = "";
                            str = "Oocyte Number " + item.ID;
                            long id = item.ID;
                            newItem = new MasterListItem(item.ID, str);

                            missingVehicle1.Add(newItem);
                        }

                        cmbApplay.ItemsSource = null;
                        cmbApplay.ItemsSource = missingVehicle1;
                        //cmbApplay.SelectedItem = missingVehicles[0]; 

                        ////  by neena
                        //List<clsIVFDashboard_LabDaysVO> mlist = ((clsIVFDashboard_GetDay5OocyteDetailsBizActionVO)arg.Result).Oocytelist;
                        //List<MasterListItem> missingVehicle1 = new List<MasterListItem>();
                        //var productFirstChars =
                        //                       from p in objList
                        //                       select p.ID;
                        //var customerFirstChars =
                        //                        from c in mlist
                        //                        where (c.Isfreezed == false)
                        //                        select c.OocyteNumber
                        //                        ;

                        //// var missingVehicles = productFirstChars.Except(customerFirstChars);
                        ////by neena
                        //var missingVehicles = productFirstChars.Intersect(customerFirstChars);
                        //// var missingVehicles = customerFirstChars.Except(productFirstChars);
                        ////List<MasterListItem> missingVehicles = new List<MasterListItem>(
                        ////var missingVehicles = from uV in objList
                        ////                      from de in ((clsIVFDashboard_GetDay1OocyteDetailsBizActionVO)arg.Result).Oocytelist
                        ////                      where uV.ID != de.OocyteNumber
                        ////                      select uV;

                        //MasterListItem newItem = new MasterListItem();
                        //foreach (var item in missingVehicles)
                        //{
                        //    string str = "";
                        //    str = "Oocyte Number " + item;
                        //    long id = item;
                        //    newItem = new MasterListItem(item, str);

                        //    missingVehicle1.Add(newItem);
                        //}

                        //cmbApplay.ItemsSource = null;
                        //cmbApplay.ItemsSource = missingVehicle1;
                        ////cmbApplay.SelectedItem = missingVehicles[0];  


                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillDay6OocyteDate()
        {
            clsIVFDashboard_GetDay6OocyteDetailsBizActionVO BizAction = new clsIVFDashboard_GetDay6OocyteDetailsBizActionVO();
            BizAction.Details = new clsIVFDashboard_LabDaysVO();
            BizAction.Details.PlanTherapyID = PlanTherapyId;
            BizAction.Details.PlanTherapyUnitID = PlanTherapyUnitId;
            BizAction.Details.SerialOocyteNumber = OocyteNo;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsIVFDashboard_GetDay6OocyteDetailsBizActionVO)arg.Result).Oocytelist != null)
                    {
                        //  by neena
                        List<clsIVFDashboard_LabDaysVO> mlist = ((clsIVFDashboard_GetDay6OocyteDetailsBizActionVO)arg.Result).Oocytelist;
                        List<MasterListItem> missingVehicle1 = new List<MasterListItem>();

                        List<MasterListItem> TempobjList = new List<MasterListItem>();

                        foreach (var item in objList)
                        {
                            foreach (var item1 in GraphicalOocDetailsList)
                            {
                                if (item.ID == item1.OocyteNumber)
                                {
                                    if (item1.Day7 == true || item1.DecisionID > 0)
                                    {

                                    }
                                    else
                                        TempobjList.Add(item);
                                }
                            }
                        }

                        objList = TempobjList;
                        var productFirstChars =
                                               from p in objList
                                               select p.ID;
                        var customerFirstChars =
                                                from c in mlist
                                                //where (c.Isfreezed == true)
                                                select c.OocyteNumber
                                                ;

                        //by neena
                        var list = objList.Where(a => !mlist.Any(b => b.OocyteNumber == a.ID)).ToList();

                        //var missingVehicles = productFirstChars.Intersect(customerFirstChars);
                        MasterListItem newItem = new MasterListItem();
                        foreach (var item in list)
                        {
                            string str = "";
                            str = "Oocyte Number " + item.ID;
                            long id = item.ID;
                            newItem = new MasterListItem(item.ID, str);

                            missingVehicle1.Add(newItem);
                        }

                        cmbApplay.ItemsSource = null;
                        cmbApplay.ItemsSource = missingVehicle1;
                        //cmbApplay.SelectedItem = missingVehicles[0]; 

                        ////  by neena
                        //List<clsIVFDashboard_LabDaysVO> mlist = ((clsIVFDashboard_GetDay6OocyteDetailsBizActionVO)arg.Result).Oocytelist;
                        //List<MasterListItem> missingVehicle1 = new List<MasterListItem>();
                        //var productFirstChars =
                        //                       from p in objList
                        //                       select p.ID;
                        //var customerFirstChars =
                        //                        from c in mlist
                        //                        where (c.Isfreezed == false)
                        //                        select c.OocyteNumber
                        //                        ;

                        ////var missingVehicles = customerFirstChars.Except(productFirstChars);
                        ////by neena
                        //var missingVehicles = productFirstChars.Intersect(customerFirstChars);
                        ////List<MasterListItem> missingVehicles = new List<MasterListItem>(
                        ////var missingVehicles = from uV in objList
                        ////                      from de in ((clsIVFDashboard_GetDay1OocyteDetailsBizActionVO)arg.Result).Oocytelist
                        ////                      where uV.ID != de.OocyteNumber
                        ////                      select uV;

                        //MasterListItem newItem = new MasterListItem();
                        //foreach (var item in missingVehicles)
                        //{
                        //    string str = "";
                        //    str = "Oocyte Number " + item;
                        //    long id = item;
                        //    newItem = new MasterListItem(item, str);
                        //    missingVehicle1.Add(newItem);
                        //}

                        //cmbApplay.ItemsSource = null;
                        //cmbApplay.ItemsSource = missingVehicle1;
                        ////cmbApplay.SelectedItem = missingVehicles[0];  


                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillDay7OocyteDate()
        {
            clsIVFDashboard_GetDay7OocyteDetailsBizActionVO BizAction = new clsIVFDashboard_GetDay7OocyteDetailsBizActionVO();
            BizAction.Details = new clsIVFDashboard_LabDaysVO();
            BizAction.Details.PlanTherapyID = PlanTherapyId;
            BizAction.Details.PlanTherapyUnitID = PlanTherapyUnitId;
            BizAction.Details.SerialOocyteNumber = OocyteNo;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsIVFDashboard_GetDay7OocyteDetailsBizActionVO)arg.Result).Oocytelist != null)
                    {
                        //  by neena
                        List<clsIVFDashboard_LabDaysVO> mlist = ((clsIVFDashboard_GetDay7OocyteDetailsBizActionVO)arg.Result).Oocytelist;
                        List<MasterListItem> missingVehicle1 = new List<MasterListItem>();
                        var productFirstChars =
                                               from p in objList
                                               select p.ID;
                        var customerFirstChars =
                                                from c in mlist
                                                //where (c.Isfreezed == true)
                                                select c.OocyteNumber
                                                ;

                        //by neena
                        var list = objList.Where(a => !mlist.Any(b => b.OocyteNumber == a.ID)).ToList();

                        //var missingVehicles = productFirstChars.Intersect(customerFirstChars);
                        MasterListItem newItem = new MasterListItem();
                        foreach (var item in list)
                        {
                            string str = "";
                            str = "Oocyte Number " + item.ID;
                            long id = item.ID;
                            newItem = new MasterListItem(item.ID, str);

                            missingVehicle1.Add(newItem);
                        }

                        cmbApplay.ItemsSource = null;
                        cmbApplay.ItemsSource = missingVehicle1;
                        //cmbApplay.SelectedItem = missingVehicles[0]; 

                        ////  by neena
                        //List<clsIVFDashboard_LabDaysVO> mlist = ((clsIVFDashboard_GetDay7OocyteDetailsBizActionVO)arg.Result).Oocytelist;
                        //List<MasterListItem> missingVehicle1 = new List<MasterListItem>();
                        //var productFirstChars =
                        //                       from p in objList
                        //                       select p.ID;
                        //var customerFirstChars =
                        //                        from c in mlist
                        //                        where (c.Isfreezed == false)
                        //                        select c.OocyteNumber
                        //                        ;

                        ////var missingVehicles = customerFirstChars.Except(productFirstChars);
                        ////by neena
                        //var missingVehicles = productFirstChars.Intersect(customerFirstChars);
                        ////List<MasterListItem> missingVehicles = new List<MasterListItem>(
                        ////var missingVehicles = from uV in objList
                        ////                      from de in ((clsIVFDashboard_GetDay1OocyteDetailsBizActionVO)arg.Result).Oocytelist
                        ////                      where uV.ID != de.OocyteNumber
                        ////                      select uV;

                        //MasterListItem newItem = new MasterListItem();
                        //foreach (var item in missingVehicles)
                        //{
                        //    string str = "";
                        //    str = "Oocyte Number " + item;
                        //    long id = item;
                        //    newItem = new MasterListItem(item, str);
                        //    missingVehicle1.Add(newItem);
                        //}

                        //cmbApplay.ItemsSource = null;
                        //cmbApplay.ItemsSource = missingVehicle1;
                        ////cmbApplay.SelectedItem = missingVehicles[0];  


                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        List<MasterListItem> objList = new List<MasterListItem>();
        private void fillOocyteDetails()
        {
            try
            {

                clsIVFDashboard_GetOPUDetailsBizActionVO BizAction = new clsIVFDashboard_GetOPUDetailsBizActionVO();
                BizAction.Details = new clsIVFDashboard_OPUVO();
                BizAction.Details.PlanTherapyID = PlanTherapyId;
                BizAction.Details.PlanTherapyUnitID = PlanTherapyUnitId;
                if (CoupleDetails != null)
                {
                    BizAction.Details.PatientID = CoupleDetails.FemalePatient.PatientID;
                    BizAction.Details.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
                }

                //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc"); // this url will work both in dev and after deploy
                //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details != null)
                        {

                            if (((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.ID > 0)
                            {

                                //objList.Add(new MasterListItem(0, "-- Select --"));
                                //objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                                for (int i = 1; i <= ((clsIVFDashboard_GetOPUDetailsBizActionVO)arg.Result).Details.OocyteRetrived; i++)
                                {
                                    if (i != OocyteNo && OocyteNo != 0)
                                    {

                                        objList.Add(new MasterListItem(i, "Oocyte No " + i));
                                    }
                                }
                                // filldata();
                                //added by neena 

                            }



                        }
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }


        private void filldata()
        {
            clsIVFDashboard_GetDay1OocyteDetailsBizActionVO BizAction = new clsIVFDashboard_GetDay1OocyteDetailsBizActionVO();
            BizAction.Details = new clsIVFDashboard_LabDaysVO();
            BizAction.Details.PlanTherapyID = PlanTherapyId;
            BizAction.Details.PlanTherapyUnitID = PlanTherapyUnitId;
            BizAction.Details.SerialOocyteNumber = OocyteNo;
            //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);           

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsIVFDashboard_GetDay1OocyteDetailsBizActionVO)arg.Result).Oocytelist != null)
                    {
                        //  by neena

                        List<clsIVFDashboard_LabDaysVO> mlist = ((clsIVFDashboard_GetDay1OocyteDetailsBizActionVO)arg.Result).Oocytelist;
                        objList = new List<MasterListItem>();
                        foreach (var item in mlist)
                        {
                            if (item.OocyteNumber != OocyteNo && OocyteNo != 0)
                            {
                                objList.Add(new MasterListItem(item.OocyteNumber, "Oocyte No " + item.OocyteNumber));
                            }
                        }
                       


                        //List<clsIVFDashboard_LabDaysVO> mlist = ((clsIVFDashboard_GetDay1OocyteDetailsBizActionVO)arg.Result).Oocytelist;
                        //List<MasterListItem> missingVehicle1 = new List<MasterListItem>();
                        //var productFirstChars =
                        //                       from p in objList
                        //                       select p.ID;
                        //var customerFirstChars =
                        //                        from c in mlist
                        //                        where (c.Isfreezed == true)
                        //                        select c.OocyteNumber
                        //                        ;

                        ////by neena
                        //var missingVehicles = productFirstChars.Intersect(customerFirstChars);
                        //MasterListItem newItem = new MasterListItem();
                        //foreach (var item in missingVehicles)
                        //{
                        //    string str = "";
                        //    str = "Oocyte Number " + item;
                        //    long id = item;
                        //    newItem = new MasterListItem(item, str);

                        //    missingVehicle1.Add(newItem);
                        //}

                        //cmbApplay.ItemsSource = null;
                        //cmbApplay.ItemsSource = missingVehicle1;
                        ////cmbApplay.SelectedItem = missingVehicles[0];  


                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillCellNo()
        {
            List<MasterListItem> CellNoList = new List<MasterListItem>();
            CellNoList.Add(new MasterListItem(0, "--Select--"));
            CellNoList.Add(new MasterListItem(1, "1"));
            CellNoList.Add(new MasterListItem(2, "2"));
            CellNoList.Add(new MasterListItem(3, "3"));
            CellNoList.Add(new MasterListItem(4, "4"));
            CellNoList.Add(new MasterListItem(5, "5"));
            CellNoList.Add(new MasterListItem(6, "6"));
            CellNoList.Add(new MasterListItem(7, "7"));
            CellNoList.Add(new MasterListItem(8, "8"));
            CellNoList.Add(new MasterListItem(9, ">8"));
            cmbCellNo.ItemsSource = null;
            cmbCellNo.ItemsSource = CellNoList;
            cmbCellNo.SelectedItem = CellNoList[0];

            FillDay();

        }

        private void FillDay()
        {
            List<MasterListItem> DayList = new List<MasterListItem>();
            DayList.Add(new MasterListItem(0, "--Select--"));
            DayList.Add(new MasterListItem(1, "Day 1"));
            DayList.Add(new MasterListItem(2, "Day 2"));
            DayList.Add(new MasterListItem(3, "Day 3"));
            DayList.Add(new MasterListItem(4, "Day 4"));
            DayList.Add(new MasterListItem(5, "Day 5"));
            DayList.Add(new MasterListItem(6, "Day 6"));
            DayList.Add(new MasterListItem(7, "Day 7 And Above"));
            cmbObservationDay.ItemsSource = null;
            cmbObservationDay.ItemsSource = DayList;
            cmbObservationDay.SelectedItem = DayList[0];

            FillFragmentation();

        }

        private void FillFragmentation()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVF_FragmentationMaster;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            BizAction.MasterList = new List<MasterListItem>();
            //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc");
            //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                    cmbFragmentation.ItemsSource = null;
                    cmbFragmentation.ItemsSource = objList;
                    cmbFragmentation.SelectedItem = objList[0];
                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            blEmbError = false;
            blPreCleavage = false;
            blCleavage = false;
            blDay4Embryo = false;
            blBlastocyst = false;

            if (IsIsthambul)
                CheckTimeAndReturnLabDay();
            else
                CalculateLabDay();
            ShowComboLabDay5();

            if (Validate())
            {

                if (blEmbError)
                {
                    blEmbError = false;
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("", "You can not save Observation details before fertilization..", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }

                if (IsIsthambul)
                {



                    if (Day == 1)
                    {
                        if (blLabDay5 || blLabDay4 || blLabDay2)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                      new MessageBoxControl.MessageBoxChildWindow("", "You can not save previous observation", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                        }
                        else if (blLabDay1)
                        {
                            blLabDay1 = false;
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                       new MessageBoxControl.MessageBoxChildWindow("", "You can not save Pre Cleavage Stage details because it is already present", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save Pre Cleavage Stage details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                            msgW.Show();
                        }
                    }

                    if (Day == 2)
                    {
                        if (blLabDay5 || blLabDay4)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                      new MessageBoxControl.MessageBoxChildWindow("", "You can not save previous observation", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                        }
                        else if (blLabDay2)
                        {
                            blLabDay2 = false;
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                       new MessageBoxControl.MessageBoxChildWindow("", "You can not save Cleavage Stage details because it is already present", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Pre Cleavage Stage is over.. Are you sure you want to save Cleavage Stage details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                            msgW.Show();
                        }
                    }

                    if (Day == 4)
                    {
                        if (blLabDay5)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                      new MessageBoxControl.MessageBoxChildWindow("", "You can not save previous observation", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                        }
                        else if (blLabDay4)
                        {
                            blLabDay4 = false;
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                       new MessageBoxControl.MessageBoxChildWindow("", "You can not save Day 4 Embryo Stage details because it is already present", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Cleavage Stage is over.. Are you sure you want to save Day 4 Embryo Stage details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                            msgW.Show();
                        }
                    }

                    if (Day == 5)
                    {
                        if (blLabDay5)
                        {
                            blLabDay5 = false;
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                       new MessageBoxControl.MessageBoxChildWindow("", "You can not save Blastocyst Stage details because it is already present", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Day 4 Embryo Stage is over.. Are you sure you want to save Blastocyst Stage details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                            msgW.Show();
                        }
                    }

                }
                else
                {
                    if (Day == 1)
                    {
                        if (blLabDay7 || blLabDay6 || blLabDay5 || blLabDay4 || blLabDay3 || blLabDay2)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                      new MessageBoxControl.MessageBoxChildWindow("", "You can not save previous day observation", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                        }
                        else if (blLabDay1)
                        {
                            blLabDay1 = false;
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                       new MessageBoxControl.MessageBoxChildWindow("", "You can not save lab day 1 details because it is already present", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save lab day 1 details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                            msgW.Show();
                        }
                    }

                    if (Day == 2)
                    {
                        if (blLabDay7 || blLabDay6 || blLabDay5 || blLabDay4 || blLabDay3)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                      new MessageBoxControl.MessageBoxChildWindow("", "You can not save previous observation", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                        }
                        else if (blLabDay2)
                        {
                            blLabDay2 = false;
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                       new MessageBoxControl.MessageBoxChildWindow("", "You can not save lab day 2 details because it is already present", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save lab day 2 details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                            msgW.Show();
                        }
                    }

                    if (Day == 3)
                    {
                        if (blLabDay7 || blLabDay6 || blLabDay5 || blLabDay4)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                      new MessageBoxControl.MessageBoxChildWindow("", "You can not save previous observation", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                        }
                        else if (blLabDay3)
                        {
                            blLabDay3 = false;
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                       new MessageBoxControl.MessageBoxChildWindow("", "You can not save lab day 3 details because it is already present", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save lab day 3 details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                            msgW.Show();
                        }
                    }

                    if (Day == 4)
                    {
                        if (blLabDay7 || blLabDay6 || blLabDay5)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                      new MessageBoxControl.MessageBoxChildWindow("", "You can not save previous observation", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                        }
                        else if (blLabDay4)
                        {
                            blLabDay4 = false;
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                       new MessageBoxControl.MessageBoxChildWindow("", "You can not save lab day 4 details because it is already present", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save lab day 4 details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                            msgW.Show();
                        }
                    }

                    if (Day == 5)
                    {
                        if (blLabDay7 || blLabDay6)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                      new MessageBoxControl.MessageBoxChildWindow("", "You can not save previous observation", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                        }
                        else if (blLabDay5)
                        {
                            blLabDay5 = false;
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                       new MessageBoxControl.MessageBoxChildWindow("", "You can not save lab day 5 details because it is already present", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save lab day 5 details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                            msgW.Show();
                        }
                    }

                    if (Day == 6)
                    {
                        if (blLabDay7)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                     new MessageBoxControl.MessageBoxChildWindow("", "You can not save previous observation", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                        }
                        else if (blLabDay6)
                        {
                            blLabDay6 = false;
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                       new MessageBoxControl.MessageBoxChildWindow("", "You can not save lab day 6 details because it is already present", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save lab day 6 details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                            msgW.Show();
                        }
                    }

                    if (Day == 7)
                    {
                        if (blLabDay7)
                        {
                            blLabDay7 = false;
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                       new MessageBoxControl.MessageBoxChildWindow("", "You can not save lab day 7 details because it is already present", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save lab day 7 details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                            msgW.Show();
                        }
                    }

                }



                #region  emb stage validation
                //if (Validate())
                //{
                //    blError = false;
                //    blSaveData = false;
                //    blAlert = false;
                //    blDayCrossed = false;

                //    CheckTimeAndReturnLabDay();

                //    if (Day == 1)
                //    {
                //        if (blLabDay1)
                //        {
                //            blLabDay1 = false;
                //            MessageBoxControl.MessageBoxChildWindow msgW1 =
                //                       new MessageBoxControl.MessageBoxChildWindow("", "You can not save lab day 1 details because it is already present", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //            msgW1.Show();
                //        }
                //        else if (blError)
                //        {
                //            blError = false;
                //            MessageBoxControl.MessageBoxChildWindow msgW1 =
                //                       new MessageBoxControl.MessageBoxChildWindow("", "You can not save lab day details because lab day 1 is not complited yet..", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //            msgW1.Show();
                //        }
                //        else if (blAlert)
                //        {
                //            blAlert = false;
                //            MessageBoxControl.MessageBoxChildWindow msgW1 =
                //                      new MessageBoxControl.MessageBoxChildWindow("", "lab day 1 hours limit is crossed.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //            msgW1.Show();
                //            msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                //            {
                //                if (res == MessageBoxResult.OK)
                //                {
                //                    MessageBoxControl.MessageBoxChildWindow msgW =
                //                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save Day 1 details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                //                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                //                    msgW.Show();
                //                }
                //            };

                //        }
                //        else
                //        {
                //            MessageBoxControl.MessageBoxChildWindow msgW =
                //                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save lab Day 1 details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                //            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                //            msgW.Show();
                //        }
                //    }

                //    if (Day == 2)
                //    {
                //        if (blLabDay2)
                //        {
                //            blLabDay2 = false;
                //            MessageBoxControl.MessageBoxChildWindow msgW1 =
                //                       new MessageBoxControl.MessageBoxChildWindow("", "You can not save lab day 2 details because it is already present", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //            msgW1.Show();
                //        }
                //        else if (blAlert)
                //        {
                //            blAlert = false;
                //            MessageBoxControl.MessageBoxChildWindow msgW1 =
                //                      new MessageBoxControl.MessageBoxChildWindow("", "lab day 1 hours limit is crossed.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //            msgW1.Show();
                //            msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                //            {
                //                if (res == MessageBoxResult.OK)
                //                {
                //                    MessageBoxControl.MessageBoxChildWindow msgW =
                //                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save Day 2 details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                //                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                //                    msgW.Show();
                //                }
                //            };
                //        }
                //        else
                //        {
                //            MessageBoxControl.MessageBoxChildWindow msgW =
                //                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save lab Day 2 details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                //            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                //            msgW.Show();
                //        }
                //    }

                //    if (Day == 3)
                //    {
                //        if (blLabDay3)
                //        {
                //            blLabDay3 = false;
                //            MessageBoxControl.MessageBoxChildWindow msgW1 =
                //                       new MessageBoxControl.MessageBoxChildWindow("", "You can not save lab day 3 details because it is already present", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //            msgW1.Show();
                //        }
                //        else if (blAlert)
                //        {
                //            blAlert = false;
                //            MessageBoxControl.MessageBoxChildWindow msgW1 =
                //                      new MessageBoxControl.MessageBoxChildWindow("", "lab day 2 hours limit is crossed.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //            msgW1.Show();
                //            msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                //            {
                //                if (res == MessageBoxResult.OK)
                //                {
                //                    MessageBoxControl.MessageBoxChildWindow msgW =
                //                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save Day 3 details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                //                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                //                    msgW.Show();
                //                }
                //            };
                //        }
                //        else
                //        {
                //            MessageBoxControl.MessageBoxChildWindow msgW =
                //                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save lab Day 3 details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                //            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                //            msgW.Show();
                //        }
                //    }

                //    if (Day == 4)
                //    {
                //        if (blLabDay4)
                //        {
                //            blLabDay4 = false;
                //            MessageBoxControl.MessageBoxChildWindow msgW1 =
                //                       new MessageBoxControl.MessageBoxChildWindow("", "You can not save lab day 4 details because it is already present", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //            msgW1.Show();
                //        }
                //        else if (blAlert)
                //        {
                //            blAlert = false;
                //            MessageBoxControl.MessageBoxChildWindow msgW1 =
                //                      new MessageBoxControl.MessageBoxChildWindow("", "lab day 3 hours limit is crossed.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //            msgW1.Show();
                //            msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                //            {
                //                if (res == MessageBoxResult.OK)
                //                {
                //                    MessageBoxControl.MessageBoxChildWindow msgW =
                //                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save Day 4 details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                //                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                //                    msgW.Show();
                //                }
                //            };
                //        }
                //        else
                //        {
                //            MessageBoxControl.MessageBoxChildWindow msgW =
                //                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save lab Day 4 details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                //            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                //            msgW.Show();
                //        }
                //    }

                //    if (Day == 5)
                //    {
                //        if (blLabDay5)
                //        {
                //            blLabDay5 = false;
                //            MessageBoxControl.MessageBoxChildWindow msgW1 =
                //                       new MessageBoxControl.MessageBoxChildWindow("", "You can not save lab day 5 details because it is already present", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //            msgW1.Show();
                //        }
                //        else if (blAlert)
                //        {
                //            blAlert = false;
                //            MessageBoxControl.MessageBoxChildWindow msgW1 =
                //                      new MessageBoxControl.MessageBoxChildWindow("", "lab day 4 hours limit is crossed.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //            msgW1.Show();
                //            msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                //            {
                //                if (res == MessageBoxResult.OK)
                //                {
                //                    MessageBoxControl.MessageBoxChildWindow msgW =
                //                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save Day 5 details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                //                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                //                    msgW.Show();
                //                }
                //            };
                //        }
                //        else
                //        {
                //            MessageBoxControl.MessageBoxChildWindow msgW =
                //                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save lab Day 5 details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                //            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                //            msgW.Show();
                //        }
                //    }

                //    if (Day == 6)
                //    {
                //        if (blLabDay6)
                //        {
                //            blLabDay6 = false;
                //            MessageBoxControl.MessageBoxChildWindow msgW1 =
                //                       new MessageBoxControl.MessageBoxChildWindow("", "You can not save lab day 6 details because it is already present", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //            msgW1.Show();
                //        }
                //        else if (blAlert)
                //        {
                //            blAlert = false;
                //            MessageBoxControl.MessageBoxChildWindow msgW1 =
                //                      new MessageBoxControl.MessageBoxChildWindow("", "lab day 5 hours limit is crossed.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //            msgW1.Show();
                //            msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                //            {
                //                if (res == MessageBoxResult.OK)
                //                {
                //                    MessageBoxControl.MessageBoxChildWindow msgW =
                //                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save Day 6 details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                //                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                //                    msgW.Show();
                //                }
                //            };
                //        }
                //        else
                //        {
                //            MessageBoxControl.MessageBoxChildWindow msgW =
                //                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save lab Day 6 details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                //            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                //            msgW.Show();
                //        }
                //    }

                #endregion


                //-----added by neena dated 18/5/16

                OocyteList = (List<MasterListItem>)cmbApplay.ItemsSource;
                MasterListItem newItem1 = new MasterListItem();

                foreach (var item in OocyteList)
                {
                    if (item.Status == true)
                    {
                        long id1 = 0;
                        string str = "";
                        id1 = item.ID;
                        str = item.Description;
                        addSerialOocNo = item.ID - OocyteNo;
                        if (addSerialOocNo == 0 ) //|| addSerialOocNo < 0)
                        {
                            addSerialOocNo = 1;
                        }


                        newItem1 = new MasterListItem(id1, str, addSerialOocNo);
                        SelectedOocyteList.Add(newItem1);
                    }
                }
                //-------------
            }

        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if (Day == 1)
                    SaveLabDay1();
                if (Day == 2)
                    SaveLabDay2();
                if (Day == 3)
                    SaveLabDay3();
                if (Day == 4)
                    SaveLabDay4();
                if (Day == 5)
                    SaveLabDay5();
                if (Day == 6)
                    SaveLabDay6();
                if (Day == 7)
                    SaveLabDay7();

            }
        }

        List<clsAddImageVO> ImageList = new List<clsAddImageVO>();
        public void SaveLabDay1()
        {
            clsIVFDashboard_AddUpdateDay1BizActionVO BizAction = new clsIVFDashboard_AddUpdateDay1BizActionVO();
            BizAction.Day1Details = new clsIVFDashboard_LabDaysVO();

            //added by neena
            BizAction.Day1Details.DecisionID = DecisionID;
            BizAction.Day1Details.OcyteListList = SelectedOocyteList;
            BizAction.Day1Details.SerialOocyteNumber = SerialOocNo;


            DateTime? ObservationDate = null;
            if (dtObservationDate.SelectedDate != null)
                ObservationDate = dtObservationDate.SelectedDate.Value.Date;
            if (dtObservationTime.Value != null)
                ObservationDate = ObservationDate.Value.Add(dtObservationTime.Value.Value.TimeOfDay);

            BizAction.Day1Details.CellObservationDate = ObservationDate;
            BizAction.Day1Details.CellObservationTime = ObservationDate;

            //BizAction.Day1Details.CellObservationDate = dtObservationDate.SelectedDate.Value.Date;
            //BizAction.Day1Details.CellObservationTime = Convert.ToDateTime(dtObservationTime.Value);

            if ((MasterListItem)cmbCellNo.SelectedItem != null)
                BizAction.Day1Details.CellNo = ((MasterListItem)cmbCellNo.SelectedItem).ID;

            //

            //BizAction.Day1Details.ID = ((clsIVFDashboard_LabDaysVO)this.DataContext).ID;
            BizAction.Day1Details.ID = 0;
            BizAction.Day1Details.PatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.Day1Details.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            BizAction.Day1Details.PlanTherapyID = PlanTherapyId;
            BizAction.Day1Details.PlanTherapyUnitID = PlanTherapyUnitId;
            BizAction.Day1Details.OocyteNumber = OocyteNo;
            BizAction.Day1Details.SerialOocyteNumber = SerialOocNo;

            BizAction.Day1Details.Impression = Convert.ToString(txtImpression.Text);
            BizAction.Day1Details.CellStage = txtCellStage.Text.Trim();

            if (chkSendForBiopsy.IsChecked == true)
                BizAction.Day1Details.IsBiopsy = true;
            else
                BizAction.Day1Details.IsBiopsy = false;

            //if ((MasterListItem)cmbGrade.SelectedItem != null)
            //    BizAction.Day1Details.GradeID = ((MasterListItem)cmbGrade.SelectedItem).ID;

            if (dtDate.SelectedDate != null)
                BizAction.Day1Details.BiopsyDate = dtDate.SelectedDate.Value.Date;
            //else
            //    BizAction.Day1Details.BiopsyDate = null;

            if (dtTime.Value != null)
                BizAction.Day1Details.BiopsyTime = Convert.ToDateTime(dtTime.Value);
            //else
            //    BizAction.Day1Details.BiopsyTime = null;

            if (txtNoOfCell.Text.Trim() != string.Empty)
                BizAction.Day1Details.NoOfCell = Convert.ToInt64(txtNoOfCell.Text.Trim());


            if (chkFreeze.IsChecked == true)
                BizAction.Day1Details.Isfreezed = true;
            else
                BizAction.Day1Details.Isfreezed = false;
            // BizAction.Day1Details.Photo = MyPhoto;
            // BizAction.Day1Details.FileName = txtFN.Text;
            BizAction.Day1Details.OocyteDonorID = OocyteDonorID;
            BizAction.Day1Details.OocyteDonorUnitID = OocyteDonorUnitID;

            if (cmbEmbroyologist.SelectedItem != null)
                BizAction.Day1Details.EmbryologistID = ((MasterListItem)cmbEmbroyologist.SelectedItem).ID;
            if (cmbWitnessedEmbroyologist.SelectedItem != null)
                BizAction.Day1Details.AssitantEmbryologistID = ((MasterListItem)cmbWitnessedEmbroyologist.SelectedItem).ID;

            foreach (var item in mylistitem)
            {
                ListItems obj = (ListItems)item;
                clsAddImageVO ObjImg = new clsAddImageVO();
                ObjImg.ID = obj.ID;
                ObjImg.Photo = obj.Photo;
                ObjImg.ImagePath = obj.ImagePath;
                ObjImg.SeqNo = obj.SeqNo;
                ObjImg.Day = obj.Day;
                ObjImg.ServerImageName = obj.OriginalImagePath;
                ImageList.Add(ObjImg);

            }

            BizAction.Day1Details.ImgList = ImageList;
            BizAction.Day1Details.DetailList = DocList;


            //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.OK)
                        {
                            this.DialogResult = true;
                            if (OnSaveButton_Click != null)
                                OnSaveButton_Click(this, new RoutedEventArgs());
                        }
                    };
                    msgW1.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        public void SaveLabDay2()
        {
            clsIVFDashboard_AddUpdateDay2BizActionVO BizAction = new clsIVFDashboard_AddUpdateDay2BizActionVO();
            BizAction.Day2Details = new clsIVFDashboard_LabDaysVO();

            //added by neena
            BizAction.Day2Details.DecisionID = DecisionID;
            BizAction.Day2Details.OcyteListList = SelectedOocyteList;
            BizAction.Day2Details.SerialOocyteNumber = SerialOocNo;

            DateTime? ObservationDate = null;
            if (dtObservationDate.SelectedDate != null)
                ObservationDate = dtObservationDate.SelectedDate.Value.Date;
            if (dtObservationTime.Value != null)
                ObservationDate = ObservationDate.Value.Add(dtObservationTime.Value.Value.TimeOfDay);

            BizAction.Day2Details.CellObservationDate = ObservationDate;
            BizAction.Day2Details.CellObservationTime = ObservationDate;

            //BizAction.Day2Details.CellObservationDate = dtObservationDate.SelectedDate.Value.Date;
            //BizAction.Day2Details.CellObservationTime = Convert.ToDateTime(dtObservationTime.Value);

            if ((MasterListItem)cmbCellNo.SelectedItem != null)
                BizAction.Day2Details.CellNo = ((MasterListItem)cmbCellNo.SelectedItem).ID;

            //

            //BizAction.Day1Details.ID = ((clsIVFDashboard_LabDaysVO)this.DataContext).ID;
            BizAction.Day2Details.ID = 0;
            BizAction.Day2Details.PatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.Day2Details.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            BizAction.Day2Details.PlanTherapyID = PlanTherapyId;
            BizAction.Day2Details.PlanTherapyUnitID = PlanTherapyUnitId;
            BizAction.Day2Details.OocyteNumber = OocyteNo;
            BizAction.Day2Details.SerialOocyteNumber = SerialOocNo;

            BizAction.Day2Details.Impression = Convert.ToString(txtImpression.Text);
            BizAction.Day2Details.CellStage = txtCellStage.Text.Trim();

            if (chkSendForBiopsy.IsChecked == true)
                BizAction.Day2Details.IsBiopsy = true;
            else
                BizAction.Day2Details.IsBiopsy = false;
            if ((MasterListItem)cmbGrade2.SelectedItem != null)
                BizAction.Day2Details.GradeID = ((MasterListItem)cmbGrade2.SelectedItem).ID;
            if ((MasterListItem)cmbFragmentation.SelectedItem != null)
                BizAction.Day2Details.FrgmentationID = ((MasterListItem)cmbFragmentation.SelectedItem).ID;
            if (dtDate.SelectedDate != null)
                BizAction.Day2Details.BiopsyDate = dtDate.SelectedDate.Value.Date;
            if (dtTime.Value != null)
                BizAction.Day2Details.BiopsyTime = Convert.ToDateTime(dtTime.Value);
            if (txtNoOfCell.Text.Trim() != string.Empty)
                BizAction.Day2Details.NoOfCell = Convert.ToInt64(txtNoOfCell.Text.Trim());


            if (chkFreeze.IsChecked == true)
                BizAction.Day2Details.Isfreezed = true;
            else
                BizAction.Day2Details.Isfreezed = false;
            // BizAction.Day1Details.Photo = MyPhoto;
            // BizAction.Day1Details.FileName = txtFN.Text;
            BizAction.Day2Details.OocyteDonorID = OocyteDonorID;
            BizAction.Day2Details.OocyteDonorUnitID = OocyteDonorUnitID;

            if (cmbEmbroyologist.SelectedItem != null)
                BizAction.Day2Details.EmbryologistID = ((MasterListItem)cmbEmbroyologist.SelectedItem).ID;
            if (cmbWitnessedEmbroyologist.SelectedItem != null)
                BizAction.Day2Details.AssitantEmbryologistID = ((MasterListItem)cmbWitnessedEmbroyologist.SelectedItem).ID;

            foreach (var item in mylistitem)
            {
                ListItems obj = (ListItems)item;
                clsAddImageVO ObjImg = new clsAddImageVO();
                ObjImg.ID = obj.ID;
                ObjImg.Photo = obj.Photo;
                ObjImg.ImagePath = obj.ImagePath;
                ObjImg.SeqNo = obj.SeqNo;
                ObjImg.Day = obj.Day;
                ObjImg.ServerImageName = obj.OriginalImagePath;
                ImageList.Add(ObjImg);

            }

            BizAction.Day2Details.ImgList = ImageList;
            BizAction.Day2Details.DetailList = DocList;


            //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.OK)
                        {
                            this.DialogResult = true;
                            if (OnSaveButton_Click != null)
                                OnSaveButton_Click(this, new RoutedEventArgs());
                        }
                    };
                    msgW1.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        public void SaveLabDay3()
        {
            clsIVFDashboard_AddUpdateDay3BizActionVO BizAction = new clsIVFDashboard_AddUpdateDay3BizActionVO();
            BizAction.Day3Details = new clsIVFDashboard_LabDaysVO();

            //added by neena
            BizAction.Day3Details.DecisionID = DecisionID;
            BizAction.Day3Details.OcyteListList = SelectedOocyteList;
            BizAction.Day3Details.SerialOocyteNumber = SerialOocNo;

            DateTime? ObservationDate = null;
            if (dtObservationDate.SelectedDate != null)
                ObservationDate = dtObservationDate.SelectedDate.Value.Date;
            if (dtObservationTime.Value != null)
                ObservationDate = ObservationDate.Value.Add(dtObservationTime.Value.Value.TimeOfDay);

            BizAction.Day3Details.CellObservationDate = ObservationDate;
            BizAction.Day3Details.CellObservationTime = ObservationDate;

            //BizAction.Day3Details.CellObservationDate = dtObservationDate.SelectedDate.Value.Date;
            //BizAction.Day3Details.CellObservationTime = Convert.ToDateTime(dtObservationTime.Value);

            if ((MasterListItem)cmbCellNo.SelectedItem != null)
                BizAction.Day3Details.CellNo = ((MasterListItem)cmbCellNo.SelectedItem).ID;

            //

            if (rdoYes.IsChecked != null)
            {
                if (rdoYes.IsChecked == true)
                {
                    BizAction.Day3Details.IsEmbryoCompacted = true;
                }
            }
            if (rdoNo.IsChecked != null)
            {
                if (rdoNo.IsChecked == true)
                {
                    BizAction.Day3Details.IsEmbryoCompacted = false;
                }
            }

            if (rdoYes.IsChecked == true)
            {
                if ((MasterListItem)cmbGrade3.SelectedItem != null)
                    BizAction.Day3Details.GradeID = ((MasterListItem)cmbGrade3.SelectedItem).ID;
            }
            if (rdoNo.IsChecked == true)
            {
                if ((MasterListItem)cmbGrade2.SelectedItem != null)
                    BizAction.Day3Details.GradeID = ((MasterListItem)cmbGrade2.SelectedItem).ID;
                if ((MasterListItem)cmbFragmentation.SelectedItem != null)
                    BizAction.Day3Details.FrgmentationID = ((MasterListItem)cmbFragmentation.SelectedItem).ID;
            }

            //BizAction.Day1Details.ID = ((clsIVFDashboard_LabDaysVO)this.DataContext).ID;
            BizAction.Day3Details.ID = 0;
            BizAction.Day3Details.PatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.Day3Details.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            BizAction.Day3Details.PlanTherapyID = PlanTherapyId;
            BizAction.Day3Details.PlanTherapyUnitID = PlanTherapyUnitId;
            BizAction.Day3Details.OocyteNumber = OocyteNo;
            BizAction.Day3Details.SerialOocyteNumber = SerialOocNo;

            BizAction.Day3Details.Impression = Convert.ToString(txtImpression.Text);
            BizAction.Day3Details.CellStage = txtCellStage.Text.Trim();

            if (chkSendForBiopsy.IsChecked == true)
                BizAction.Day3Details.IsBiopsy = true;
            else
                BizAction.Day3Details.IsBiopsy = false;
            //if ((MasterListItem)cmbGrade3.SelectedItem != null)
            //    BizAction.Day3Details.GradeID = ((MasterListItem)cmbGrade3.SelectedItem).ID;

            if (dtDate.SelectedDate != null)
                BizAction.Day3Details.BiopsyDate = dtDate.SelectedDate.Value.Date;
            if (dtTime.Value != null)
                BizAction.Day3Details.BiopsyTime = Convert.ToDateTime(dtTime.Value);
            if (txtNoOfCell.Text.Trim() != string.Empty)
                BizAction.Day3Details.NoOfCell = Convert.ToInt64(txtNoOfCell.Text.Trim());


            if (chkFreeze.IsChecked == true)
                BizAction.Day3Details.Isfreezed = true;
            else
                BizAction.Day3Details.Isfreezed = false;
            // BizAction.Day1Details.Photo = MyPhoto;
            // BizAction.Day1Details.FileName = txtFN.Text;
            BizAction.Day3Details.OocyteDonorID = OocyteDonorID;
            BizAction.Day3Details.OocyteDonorUnitID = OocyteDonorUnitID;

            if (cmbEmbroyologist.SelectedItem != null)
                BizAction.Day3Details.EmbryologistID = ((MasterListItem)cmbEmbroyologist.SelectedItem).ID;
            if (cmbWitnessedEmbroyologist.SelectedItem != null)
                BizAction.Day3Details.AssitantEmbryologistID = ((MasterListItem)cmbWitnessedEmbroyologist.SelectedItem).ID;

            foreach (var item in mylistitem)
            {
                ListItems obj = (ListItems)item;
                clsAddImageVO ObjImg = new clsAddImageVO();
                ObjImg.ID = obj.ID;
                ObjImg.Photo = obj.Photo;
                ObjImg.ImagePath = obj.ImagePath;
                ObjImg.SeqNo = obj.SeqNo;
                ObjImg.Day = obj.Day;
                ObjImg.ServerImageName = obj.OriginalImagePath;
                ImageList.Add(ObjImg);

            }

            BizAction.Day3Details.ImgList = ImageList;
            BizAction.Day3Details.DetailList = DocList;


            //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.OK)
                        {
                            this.DialogResult = true;
                            if (OnSaveButton_Click != null)
                                OnSaveButton_Click(this, new RoutedEventArgs());
                        }
                    };
                    msgW1.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        public void SaveLabDay4()
        {
            clsIVFDashboard_AddUpdateDay4BizActionVO BizAction = new clsIVFDashboard_AddUpdateDay4BizActionVO();
            BizAction.Day4Details = new clsIVFDashboard_LabDaysVO();

            //added by neena
            BizAction.Day4Details.DecisionID = DecisionID;
            BizAction.Day4Details.OcyteListList = SelectedOocyteList;
            BizAction.Day4Details.SerialOocyteNumber = SerialOocNo;

            DateTime? ObservationDate = null;
            if (dtObservationDate.SelectedDate != null)
                ObservationDate = dtObservationDate.SelectedDate.Value.Date;
            if (dtObservationTime.Value != null)
                ObservationDate = ObservationDate.Value.Add(dtObservationTime.Value.Value.TimeOfDay);

            BizAction.Day4Details.CellObservationDate = ObservationDate;
            BizAction.Day4Details.CellObservationTime = ObservationDate;

            //BizAction.Day4Details.CellObservationDate = dtObservationDate.SelectedDate.Value.Date;
            //BizAction.Day4Details.CellObservationTime = Convert.ToDateTime(dtObservationTime.Value);

            if ((MasterListItem)cmbCellNo.SelectedItem != null)
                BizAction.Day4Details.CellNo = ((MasterListItem)cmbCellNo.SelectedItem).ID;

            //

            //BizAction.Day1Details.ID = ((clsIVFDashboard_LabDaysVO)this.DataContext).ID;
            BizAction.Day4Details.ID = 0;
            BizAction.Day4Details.PatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.Day4Details.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            BizAction.Day4Details.PlanTherapyID = PlanTherapyId;
            BizAction.Day4Details.PlanTherapyUnitID = PlanTherapyUnitId;
            BizAction.Day4Details.OocyteNumber = OocyteNo;
            BizAction.Day4Details.SerialOocyteNumber = SerialOocNo;

            BizAction.Day4Details.Impression = Convert.ToString(txtImpression.Text);
            BizAction.Day4Details.CellStage = txtCellStage.Text.Trim();

            if (chkSendForBiopsy.IsChecked == true)
                BizAction.Day4Details.IsBiopsy = true;
            else
                BizAction.Day4Details.IsBiopsy = false;
            if ((MasterListItem)cmbGrade4.SelectedItem != null)
                BizAction.Day4Details.GradeID = ((MasterListItem)cmbGrade4.SelectedItem).ID;
            if (dtDate.SelectedDate != null)
                BizAction.Day4Details.BiopsyDate = dtDate.SelectedDate.Value.Date;
            if (dtTime.Value != null)
                BizAction.Day4Details.BiopsyTime = Convert.ToDateTime(dtTime.Value);
            if (txtNoOfCell.Text.Trim() != string.Empty)
                BizAction.Day4Details.NoOfCell = Convert.ToInt64(txtNoOfCell.Text.Trim());


            if (chkFreeze.IsChecked == true)
                BizAction.Day4Details.Isfreezed = true;
            else
                BizAction.Day4Details.Isfreezed = false;
            // BizAction.Day1Details.Photo = MyPhoto;
            // BizAction.Day1Details.FileName = txtFN.Text;
            BizAction.Day4Details.OocyteDonorID = OocyteDonorID;
            BizAction.Day4Details.OocyteDonorUnitID = OocyteDonorUnitID;

            if (cmbEmbroyologist.SelectedItem != null)
                BizAction.Day4Details.EmbryologistID = ((MasterListItem)cmbEmbroyologist.SelectedItem).ID;
            if (cmbWitnessedEmbroyologist.SelectedItem != null)
                BizAction.Day4Details.AssitantEmbryologistID = ((MasterListItem)cmbWitnessedEmbroyologist.SelectedItem).ID;

            foreach (var item in mylistitem)
            {
                ListItems obj = (ListItems)item;
                clsAddImageVO ObjImg = new clsAddImageVO();
                ObjImg.ID = obj.ID;
                ObjImg.Photo = obj.Photo;
                ObjImg.ImagePath = obj.ImagePath;
                ObjImg.SeqNo = obj.SeqNo;
                ObjImg.Day = obj.Day;
                ObjImg.ServerImageName = obj.OriginalImagePath;
                ImageList.Add(ObjImg);

            }

            BizAction.Day4Details.ImgList = ImageList;
            BizAction.Day4Details.DetailList = DocList;
            if (chkAssistedHatching.IsChecked == true)
                BizAction.Day4Details.AssistedHatching = true;
            else
                BizAction.Day4Details.AssistedHatching = false;


            //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.OK)
                        {
                            this.DialogResult = true;
                            if (OnSaveButton_Click != null)
                                OnSaveButton_Click(this, new RoutedEventArgs());
                        }
                    };
                    msgW1.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        public void SaveLabDay5()
        {
            clsIVFDashboard_AddUpdateDay5BizActionVO BizAction = new clsIVFDashboard_AddUpdateDay5BizActionVO();
            BizAction.Day5Details = new clsIVFDashboard_LabDaysVO();

            //added by neena
            BizAction.Day5Details.DecisionID = DecisionID;
            BizAction.Day5Details.OcyteListList = SelectedOocyteList;
            BizAction.Day5Details.SerialOocyteNumber = SerialOocNo;

            DateTime? ObservationDate = null;
            if (dtObservationDate.SelectedDate != null)
                ObservationDate = dtObservationDate.SelectedDate.Value.Date;
            if (dtObservationTime.Value != null)
                ObservationDate = ObservationDate.Value.Add(dtObservationTime.Value.Value.TimeOfDay);

            BizAction.Day5Details.CellObservationDate = ObservationDate;
            BizAction.Day5Details.CellObservationTime = ObservationDate;

            //BizAction.Day5Details.CellObservationDate = dtObservationDate.SelectedDate.Value.Date;
            //BizAction.Day5Details.CellObservationTime = Convert.ToDateTime(dtObservationTime.Value);

            if ((MasterListItem)cmbCellNo.SelectedItem != null)
                BizAction.Day5Details.CellNo = ((MasterListItem)cmbCellNo.SelectedItem).ID;

            //

            //BizAction.Day1Details.ID = ((clsIVFDashboard_LabDaysVO)this.DataContext).ID;
            BizAction.Day5Details.ID = 0;
            BizAction.Day5Details.PatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.Day5Details.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            BizAction.Day5Details.PlanTherapyID = PlanTherapyId;
            BizAction.Day5Details.PlanTherapyUnitID = PlanTherapyUnitId;
            BizAction.Day5Details.OocyteNumber = OocyteNo;
            BizAction.Day5Details.SerialOocyteNumber = SerialOocNo;

            BizAction.Day5Details.Impression = Convert.ToString(txtImpression.Text);
            BizAction.Day5Details.CellStage = txtCellStage.Text.Trim();

            if (chkSendForBiopsy.IsChecked == true)
                BizAction.Day5Details.IsBiopsy = true;
            else
                BizAction.Day5Details.IsBiopsy = false;
            //if ((MasterListItem)cmbGrade.SelectedItem != null)
            //    BizAction.Day5Details.GradeID = ((MasterListItem)cmbGrade.SelectedItem).ID;
            if (dtDate.SelectedDate != null)
                BizAction.Day5Details.BiopsyDate = dtDate.SelectedDate.Value.Date;
            if (dtTime.Value != null)
                BizAction.Day5Details.BiopsyTime = Convert.ToDateTime(dtTime.Value);
            if (txtNoOfCell.Text.Trim() != string.Empty)
                BizAction.Day5Details.NoOfCell = Convert.ToInt64(txtNoOfCell.Text.Trim());


            if (chkFreeze.IsChecked == true)
                BizAction.Day5Details.Isfreezed = true;
            else
                BizAction.Day5Details.Isfreezed = false;
            // BizAction.Day1Details.Photo = MyPhoto;
            // BizAction.Day1Details.FileName = txtFN.Text;
            BizAction.Day5Details.OocyteDonorID = OocyteDonorID;
            BizAction.Day5Details.OocyteDonorUnitID = OocyteDonorUnitID;

            if ((MasterListItem)cmbStageofDevelopmentGrade.SelectedItem != null)
                BizAction.Day5Details.StageofDevelopmentGrade = ((MasterListItem)cmbStageofDevelopmentGrade.SelectedItem).ID;
            if ((MasterListItem)cmbInnerCellMassGrade.SelectedItem != null)
                BizAction.Day5Details.InnerCellMassGrade = ((MasterListItem)cmbInnerCellMassGrade.SelectedItem).ID;
            if ((MasterListItem)cmbTrophoectodermGrade.SelectedItem != null)
                BizAction.Day5Details.TrophoectodermGrade = ((MasterListItem)cmbTrophoectodermGrade.SelectedItem).ID;
            if (chkAssistedHatching.IsChecked == true)
                BizAction.Day5Details.AssistedHatching = true;
            else
                BizAction.Day5Details.AssistedHatching = false;

            if (cmbEmbroyologist.SelectedItem != null)
                BizAction.Day5Details.EmbryologistID = ((MasterListItem)cmbEmbroyologist.SelectedItem).ID;
            if (cmbWitnessedEmbroyologist.SelectedItem != null)
                BizAction.Day5Details.AssitantEmbryologistID = ((MasterListItem)cmbWitnessedEmbroyologist.SelectedItem).ID;

            foreach (var item in mylistitem)
            {
                ListItems obj = (ListItems)item;
                clsAddImageVO ObjImg = new clsAddImageVO();
                ObjImg.ID = obj.ID;
                ObjImg.Photo = obj.Photo;
                ObjImg.ImagePath = obj.ImagePath;
                ObjImg.SeqNo = obj.SeqNo;
                ObjImg.Day = obj.Day;
                ObjImg.ServerImageName = obj.OriginalImagePath;
                ImageList.Add(ObjImg);

            }

            BizAction.Day5Details.ImgList = ImageList;
            BizAction.Day5Details.DetailList = DocList;


            //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.OK)
                        {
                            this.DialogResult = true;
                            if (OnSaveButton_Click != null)
                                OnSaveButton_Click(this, new RoutedEventArgs());
                        }
                    };
                    msgW1.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        public void SaveLabDay6()
        {
            clsIVFDashboard_AddUpdateDay6BizActionVO BizAction = new clsIVFDashboard_AddUpdateDay6BizActionVO();
            BizAction.Day6Details = new clsIVFDashboard_LabDaysVO();

            //added by neena
            BizAction.Day6Details.DecisionID = DecisionID;
            BizAction.Day6Details.OcyteListList = SelectedOocyteList;
            BizAction.Day6Details.SerialOocyteNumber = SerialOocNo;

            DateTime? ObservationDate = null;
            if (dtObservationDate.SelectedDate != null)
                ObservationDate = dtObservationDate.SelectedDate.Value.Date;
            if (dtObservationTime.Value != null)
                ObservationDate = ObservationDate.Value.Add(dtObservationTime.Value.Value.TimeOfDay);

            BizAction.Day6Details.CellObservationDate = ObservationDate;
            BizAction.Day6Details.CellObservationTime = ObservationDate;

            //BizAction.Day6Details.CellObservationDate = dtObservationDate.SelectedDate.Value.Date;
            //BizAction.Day6Details.CellObservationTime = Convert.ToDateTime(dtObservationTime.Value);

            if ((MasterListItem)cmbCellNo.SelectedItem != null)
                BizAction.Day6Details.CellNo = ((MasterListItem)cmbCellNo.SelectedItem).ID;

            //

            //BizAction.Day1Details.ID = ((clsIVFDashboard_LabDaysVO)this.DataContext).ID;
            BizAction.Day6Details.ID = 0;
            BizAction.Day6Details.PatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.Day6Details.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            BizAction.Day6Details.PlanTherapyID = PlanTherapyId;
            BizAction.Day6Details.PlanTherapyUnitID = PlanTherapyUnitId;
            BizAction.Day6Details.OocyteNumber = OocyteNo;
            BizAction.Day6Details.SerialOocyteNumber = SerialOocNo;

            BizAction.Day6Details.Impression = Convert.ToString(txtImpression.Text);
            BizAction.Day6Details.CellStage = txtCellStage.Text.Trim();

            if (chkSendForBiopsy.IsChecked == true)
                BizAction.Day6Details.IsBiopsy = true;
            else
                BizAction.Day6Details.IsBiopsy = false;
            //if ((MasterListItem)cmbGrade.SelectedItem != null)
            //    BizAction.Day6Details.GradeID = ((MasterListItem)cmbGrade.SelectedItem).ID;
            if (dtDate.SelectedDate != null)
                BizAction.Day6Details.BiopsyDate = dtDate.SelectedDate.Value.Date;
            if (dtTime.Value != null)
                BizAction.Day6Details.BiopsyTime = Convert.ToDateTime(dtTime.Value);
            if (txtNoOfCell.Text.Trim() != string.Empty)
                BizAction.Day6Details.NoOfCell = Convert.ToInt64(txtNoOfCell.Text.Trim());


            if (chkFreeze.IsChecked == true)
                BizAction.Day6Details.Isfreezed = true;
            else
                BizAction.Day6Details.Isfreezed = false;
            // BizAction.Day1Details.Photo = MyPhoto;
            // BizAction.Day1Details.FileName = txtFN.Text;
            BizAction.Day6Details.OocyteDonorID = OocyteDonorID;
            BizAction.Day6Details.OocyteDonorUnitID = OocyteDonorUnitID;

            if ((MasterListItem)cmbStageofDevelopmentGrade.SelectedItem != null)
                BizAction.Day6Details.StageofDevelopmentGrade = ((MasterListItem)cmbStageofDevelopmentGrade.SelectedItem).ID;
            if ((MasterListItem)cmbInnerCellMassGrade.SelectedItem != null)
                BizAction.Day6Details.InnerCellMassGrade = ((MasterListItem)cmbInnerCellMassGrade.SelectedItem).ID;
            if ((MasterListItem)cmbTrophoectodermGrade.SelectedItem != null)
                BizAction.Day6Details.TrophoectodermGrade = ((MasterListItem)cmbTrophoectodermGrade.SelectedItem).ID;
            if (chkAssistedHatching.IsChecked == true)
                BizAction.Day6Details.AssistedHatching = true;
            else
                BizAction.Day6Details.AssistedHatching = false;

            if (cmbEmbroyologist.SelectedItem != null)
                BizAction.Day6Details.EmbryologistID = ((MasterListItem)cmbEmbroyologist.SelectedItem).ID;
            if (cmbWitnessedEmbroyologist.SelectedItem != null)
                BizAction.Day6Details.AssitantEmbryologistID = ((MasterListItem)cmbWitnessedEmbroyologist.SelectedItem).ID;

            foreach (var item in mylistitem)
            {
                ListItems obj = (ListItems)item;
                clsAddImageVO ObjImg = new clsAddImageVO();
                ObjImg.ID = obj.ID;
                ObjImg.Photo = obj.Photo;
                ObjImg.ImagePath = obj.ImagePath;
                ObjImg.SeqNo = obj.SeqNo;
                ObjImg.Day = obj.Day;
                ObjImg.ServerImageName = obj.OriginalImagePath;
                ImageList.Add(ObjImg);

            }

            BizAction.Day6Details.ImgList = ImageList;
            BizAction.Day6Details.DetailList = DocList;


            //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.OK)
                        {
                            this.DialogResult = true;
                            if (OnSaveButton_Click != null)
                                OnSaveButton_Click(this, new RoutedEventArgs());
                        }
                    };
                    msgW1.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        public void SaveLabDay7()
        {
            clsIVFDashboard_AddUpdateDay7BizActionVO BizAction = new clsIVFDashboard_AddUpdateDay7BizActionVO();
            BizAction.Day7Details = new clsIVFDashboard_LabDaysVO();

            //added by neena
            BizAction.Day7Details.DecisionID = DecisionID;
            BizAction.Day7Details.OcyteListList = SelectedOocyteList;
            BizAction.Day7Details.SerialOocyteNumber = SerialOocNo;

            DateTime? ObservationDate = null;
            if (dtObservationDate.SelectedDate != null)
                ObservationDate = dtObservationDate.SelectedDate.Value.Date;
            if (dtObservationTime.Value != null)
                ObservationDate = ObservationDate.Value.Add(dtObservationTime.Value.Value.TimeOfDay);

            BizAction.Day7Details.CellObservationDate = ObservationDate;
            BizAction.Day7Details.CellObservationTime = ObservationDate;

            //BizAction.Day7Details.CellObservationDate = dtObservationDate.SelectedDate.Value.Date;
            //BizAction.Day7Details.CellObservationTime = Convert.ToDateTime(dtObservationTime.Value);

            if ((MasterListItem)cmbCellNo.SelectedItem != null)
                BizAction.Day7Details.CellNo = ((MasterListItem)cmbCellNo.SelectedItem).ID;

            //

            //BizAction.Day1Details.ID = ((clsIVFDashboard_LabDaysVO)this.DataContext).ID;
            BizAction.Day7Details.ID = 0;
            BizAction.Day7Details.PatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.Day7Details.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            BizAction.Day7Details.PlanTherapyID = PlanTherapyId;
            BizAction.Day7Details.PlanTherapyUnitID = PlanTherapyUnitId;
            BizAction.Day7Details.OocyteNumber = OocyteNo;
            BizAction.Day7Details.SerialOocyteNumber = SerialOocNo;

            BizAction.Day7Details.Impression = Convert.ToString(txtImpression.Text);
            BizAction.Day7Details.CellStage = txtCellStage.Text.Trim();

            if (chkSendForBiopsy.IsChecked == true)
                BizAction.Day7Details.IsBiopsy = true;
            else
                BizAction.Day7Details.IsBiopsy = false;
            //if ((MasterListItem)cmbGrade.SelectedItem != null)
            //    BizAction.Day6Details.GradeID = ((MasterListItem)cmbGrade.SelectedItem).ID;
            if (dtDate.SelectedDate != null)
                BizAction.Day7Details.BiopsyDate = dtDate.SelectedDate.Value.Date;
            if (dtTime.Value != null)
                BizAction.Day7Details.BiopsyTime = Convert.ToDateTime(dtTime.Value);
            if (txtNoOfCell.Text.Trim() != string.Empty)
                BizAction.Day7Details.NoOfCell = Convert.ToInt64(txtNoOfCell.Text.Trim());


            if (chkFreeze.IsChecked == true)
                BizAction.Day7Details.Isfreezed = true;
            else
                BizAction.Day7Details.Isfreezed = false;
            // BizAction.Day1Details.Photo = MyPhoto;
            // BizAction.Day1Details.FileName = txtFN.Text;
            BizAction.Day7Details.OocyteDonorID = OocyteDonorID;
            BizAction.Day7Details.OocyteDonorUnitID = OocyteDonorUnitID;

            if ((MasterListItem)cmbStageofDevelopmentGrade.SelectedItem != null)
                BizAction.Day7Details.StageofDevelopmentGrade = ((MasterListItem)cmbStageofDevelopmentGrade.SelectedItem).ID;
            if ((MasterListItem)cmbInnerCellMassGrade.SelectedItem != null)
                BizAction.Day7Details.InnerCellMassGrade = ((MasterListItem)cmbInnerCellMassGrade.SelectedItem).ID;
            if ((MasterListItem)cmbTrophoectodermGrade.SelectedItem != null)
                BizAction.Day7Details.TrophoectodermGrade = ((MasterListItem)cmbTrophoectodermGrade.SelectedItem).ID;
            if (chkAssistedHatching.IsChecked == true)
                BizAction.Day7Details.AssistedHatching = true;
            else
                BizAction.Day7Details.AssistedHatching = false;

            if (cmbEmbroyologist.SelectedItem != null)
                BizAction.Day7Details.EmbryologistID = ((MasterListItem)cmbEmbroyologist.SelectedItem).ID;
            if (cmbWitnessedEmbroyologist.SelectedItem != null)
                BizAction.Day7Details.AssitantEmbryologistID = ((MasterListItem)cmbWitnessedEmbroyologist.SelectedItem).ID;

            foreach (var item in mylistitem)
            {
                ListItems obj = (ListItems)item;
                clsAddImageVO ObjImg = new clsAddImageVO();
                ObjImg.ID = obj.ID;
                ObjImg.Photo = obj.Photo;
                ObjImg.ImagePath = obj.ImagePath;
                ObjImg.SeqNo = obj.SeqNo;
                ObjImg.Day = obj.Day;
                ObjImg.ServerImageName = obj.OriginalImagePath;
                ImageList.Add(ObjImg);

            }

            BizAction.Day7Details.ImgList = ImageList;
            BizAction.Day7Details.DetailList = DocList;


            //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.OK)
                        {
                            this.DialogResult = true;
                            if (OnSaveButton_Click != null)
                                OnSaveButton_Click(this, new RoutedEventArgs());
                        }
                    };
                    msgW1.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void GetObservationDate()
        {
            clsIVFDashboard_GetDay1DetailsBizActionVO BizAction = new clsIVFDashboard_GetDay1DetailsBizActionVO();
            BizAction.IsGetDate = true;
            BizAction.Details = new clsIVFDashboard_LabDaysVO();
            BizAction.Details.PlanTherapyID = PlanTherapyId;
            BizAction.Details.PlanTherapyUnitID = PlanTherapyUnitId;
            //if (CoupleDetails != null)
            //{
            //    BizAction.Details.PatientID = CoupleDetails.FemalePatient.PatientID;
            //    BizAction.Details.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            //}
            //BizAction.Details.SerialOocyteNumber = SerialOocNo;
            //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details != null)
                    {
                        if (((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.CellObservationDate != null)
                        {
                            dtObservationDate.SelectedDate = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.CellObservationDate;
                        }

                        if (((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.CellObservationTime != null)
                        {
                            dtObservationTime.Value = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.CellObservationTime;
                        }
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        long OocyteDonorID = 0;
        long OocyteDonorUnitID = 0;
        public void fillDetailsLabDay1()
        {
            clsIVFDashboard_GetDay1DetailsBizActionVO BizAction = new clsIVFDashboard_GetDay1DetailsBizActionVO();
            BizAction.Details = new clsIVFDashboard_LabDaysVO();
            BizAction.Details.PlanTherapyID = PlanTherapyId;
            BizAction.Details.PlanTherapyUnitID = PlanTherapyUnitId;
            //if (CoupleDetails != null)
            //{
            //    BizAction.Details.PatientID = CoupleDetails.FemalePatient.PatientID;
            //    BizAction.Details.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            //}
            BizAction.Details.SerialOocyteNumber = SerialOocNo;
            //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details != null)
                    {
                        this.DataContext = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details;
                        OocyteDonorID = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.OocyteDonorID;
                        OocyteDonorUnitID = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.OocyteDonorUnitID;


                        txtImpression.Text = Convert.ToString(((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.Impression);// BY BHUSHAN

                        //if (((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.GradeID != null)
                        //{
                        //    cmbGrade.SelectedValue = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.GradeID;
                        //}

                        if (((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.CellNo != null)
                        {
                            cmbCellNo.SelectedValue = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.CellNo;
                        }

                        if (((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.FileName != null)
                        {
                            txtFN.Text = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.FileName;
                        }
                        if (((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.Isfreezed == true)
                        {
                            //if (((IApplicationConfiguration)App.Current).CurrentUser.RoleName == "Administrator")
                            //{
                            //    cmdNew.IsEnabled = true;
                            //    cmbnextplan.IsEnabled = false;
                            //}
                            //else
                            //{
                            cmdNew.IsEnabled = false;
                            //}
                        }
                        if (((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.CellObservationDate != null)
                        {
                            dtObservationDate.SelectedDate = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.CellObservationDate;
                        }
                        //else
                        //{
                        //    dtObservationDate.SelectedDate = date;
                        //}

                        if (((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.CellObservationTime != null)
                        {
                            dtObservationTime.Value = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.CellObservationTime;
                        }
                        //else
                        //{
                        //    dtObservationTime.Value = DateTime.Now;
                        //}

                        if (((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.CellStage != null)
                        {
                            txtCellStage.Text = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.CellStage;
                        }

                        if (((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.IsBiopsy == true)
                        {
                            chkSendForBiopsy.IsChecked = true;
                            dtDate.IsEnabled = true;
                            dtTime.IsEnabled = true;
                            txtNoOfCell.IsEnabled = true;
                        }

                        if (((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.BiopsyDate != null)
                        {
                            dtDate.SelectedDate = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.BiopsyDate;
                        }

                        if (((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.BiopsyTime != null)
                        {
                            dtTime.Value = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.BiopsyTime;
                        }

                        if (((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.IsBiopsy == false)
                        {
                            dtDate.Text = null;
                            dtTime.Value = null;
                        }

                        if (((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.NoOfCell != null)
                        {
                            txtNoOfCell.Text = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.NoOfCell.ToString();
                        }

                        if (((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.EmbryologistID != null)
                        {
                            cmbEmbroyologist.SelectedValue = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.EmbryologistID;
                        }

                        if (((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.AssitantEmbryologistID != null)
                        {
                            cmbWitnessedEmbroyologist.SelectedValue = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.AssitantEmbryologistID;
                        }

                        cmbObservationDay.SelectedValue = (long)1;

                        if (((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.ImgList != null && ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.ImgList.Count > 0)
                        {
                            ImageList = new List<clsAddImageVO>();
                            mylistitem = new List<ListItems>();
                            foreach (var item in ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.ImgList)
                            {
                                //BitmapImage img = new BitmapImage();
                                //img.SetSource(new MemoryStream(item.Photo, false));

                                //clsAddImageVO ObjNew = new clsAddImageVO();
                                //ObjNew.Photo = item.Photo;
                                //ObjNew.ImagePath = item.ImagePath;
                                ////ObjNew.BitImagePath = img;

                                //ImageList.Add(ObjNew);

                                ListItems obj = new ListItems();
                                obj.Image1 = new BitmapImage(new Uri(item.ServerImageName, UriKind.Relative));
                                obj.ImagePath = item.ImagePath;
                                obj.ID = item.ID;
                                obj.SeqNo = item.SeqNo;
                                obj.Photo = item.Photo;
                                obj.Day = item.Day;
                                obj.UnitID = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.UnitID;
                                obj.SerialOocyteNumber = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.SerialOocyteNumber;
                                obj.OocyteNumber = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.OocyteNumber;
                                obj.PatientID = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.PatientID;
                                obj.PatientUnitID = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.PatientUnitID;
                                obj.PlanTherapyID = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.PlanTherapyID;
                                obj.PlanTherapyUnitID = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.PlanTherapyUnitID;
                                obj.OriginalImagePath = item.OriginalImagePath;
                                if (((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.Isfreezed == true)
                                    obj.IsDelete = false;
                                else
                                    obj.IsDelete = true;
                                mylistitem.Add(obj);
                                GetMylistitem.Add(obj);

                                //mylistitem.Add(
                                //    new ListItems
                                //    {
                                //        Image1 = new BitmapImage(new Uri(item.ServerImageName, UriKind.Absolute)),
                                //        ImagePath = item.ImagePath,
                                //        ID = item.ID,
                                //        SeqNo = item.SeqNo,
                                //        Photo = item.Photo,
                                //        Day = item.Day,
                                //        UnitID = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.UnitID,
                                //        SerialOocyteNumber = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.SerialOocyteNumber,
                                //        OocyteNumber = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.OocyteNumber,
                                //        PatientID = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.PatientID,
                                //        PatientUnitID = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.PatientUnitID,
                                //        PlanTherapyID = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.PlanTherapyID,
                                //        PlanTherapyUnitID = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.PlanTherapyUnitID,
                                //        OriginalImagePath = item.OriginalImagePath
                                //    });

                            }

                            dgImgList.ItemsSource = null;
                            dgImgList.ItemsSource = mylistitem;
                            //GetMylistitem = mylistitem;
                            txtFN.Text = mylistitem.Count.ToString();
                        }

                        if (((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.DetailList != null && ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.DetailList.Count > 0)
                        {
                            DocList = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.DetailList;
                            foreach (var item in DocList)
                            {
                                if (((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.Isfreezed == true)
                                    item.IsDelete = false;
                                else
                                    item.IsDelete = true;
                            }
                            dgDocumentGrid.ItemsSource = null;
                            dgDocumentGrid.ItemsSource = DocList;
                            GetDocList = ((clsIVFDashboard_GetDay1DetailsBizActionVO)arg.Result).Details.DetailList;
                            txtFileName.Text = DocList.Count.ToString();
                        }

                    }
                    FillDay1OocyteDate();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        public void fillDetailsLabDay2()
        {
            clsIVFDashboard_GetDay2DetailsBizActionVO BizAction = new clsIVFDashboard_GetDay2DetailsBizActionVO();
            BizAction.Details = new clsIVFDashboard_LabDaysVO();
            BizAction.Details.PlanTherapyID = PlanTherapyId;
            BizAction.Details.PlanTherapyUnitID = PlanTherapyUnitId;
            //if (CoupleDetails != null)
            //{
            //    BizAction.Details.PatientID = CoupleDetails.FemalePatient.PatientID;
            //    BizAction.Details.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            //}
            BizAction.Details.SerialOocyteNumber = SerialOocNo;
            //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details != null)
                    {
                        this.DataContext = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details;
                        OocyteDonorID = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.OocyteDonorID;
                        OocyteDonorUnitID = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.OocyteDonorUnitID;


                        txtImpression.Text = Convert.ToString(((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.Impression);// BY BHUSHAN

                        if (((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.GradeID != null)
                        {
                            cmbGrade2.SelectedValue = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.GradeID;
                        }

                        if (((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.CellNo != null)
                        {
                            cmbCellNo.SelectedValue = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.CellNo;
                        }

                        if (((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.FrgmentationID != null)
                        {
                            cmbFragmentation.SelectedValue = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.FrgmentationID;
                        }

                        if (((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.FileName != null)
                        {
                            txtFN.Text = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.FileName;
                        }
                        if (((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.Isfreezed == true)
                        {
                            //if (((IApplicationConfiguration)App.Current).CurrentUser.RoleName == "Administrator")
                            //{
                            //    cmdNew.IsEnabled = true;
                            //    cmbnextplan.IsEnabled = false;
                            //}
                            //else
                            //{
                            cmdNew.IsEnabled = false;
                            //}
                        }
                        if (((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.CellObservationDate != null)
                        {
                            dtObservationDate.SelectedDate = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.CellObservationDate;
                        }
                        else
                        {
                            dtObservationDate.SelectedDate = date;
                        }

                        if (((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.CellObservationTime != null)
                        {
                            dtObservationTime.Value = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.CellObservationTime;
                        }
                        else
                        {
                            dtObservationTime.Value = DateTime.Now;
                        }

                        if (((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.CellStage != null)
                        {
                            txtCellStage.Text = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.CellStage;
                        }

                        if (((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.IsBiopsy == true)
                        {
                            chkSendForBiopsy.IsChecked = true;
                            dtDate.IsEnabled = true;
                            dtTime.IsEnabled = true;
                            txtNoOfCell.IsEnabled = true;
                        }

                        if (((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.BiopsyDate != null)
                        {
                            dtDate.SelectedDate = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.BiopsyDate;
                        }

                        if (((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.BiopsyTime != null)
                        {
                            dtTime.Value = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.BiopsyTime;
                        }

                        if (((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.IsBiopsy == false)
                        {
                            dtDate.Text = null;
                            dtTime.Value = null;
                        }

                        if (((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.NoOfCell != null)
                        {
                            txtNoOfCell.Text = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.NoOfCell.ToString();
                        }

                        if (((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.EmbryologistID != null)
                        {
                            cmbEmbroyologist.SelectedValue = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.EmbryologistID;
                        }

                        if (((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.AssitantEmbryologistID != null)
                        {
                            cmbWitnessedEmbroyologist.SelectedValue = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.AssitantEmbryologistID;
                        }

                        cmbObservationDay.SelectedValue = (long)2;

                        if (((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.ImgList != null && ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.ImgList.Count > 0)
                        {
                            ImageList = new List<clsAddImageVO>();
                            mylistitem = new List<ListItems>();
                            foreach (var item in ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.ImgList)
                            {
                                //BitmapImage img = new BitmapImage();
                                //img.SetSource(new MemoryStream(item.Photo, false));

                                //clsAddImageVO ObjNew = new clsAddImageVO();
                                //ObjNew.Photo = item.Photo;
                                //ObjNew.ImagePath = item.ImagePath;
                                ////ObjNew.BitImagePath = img;

                                //ImageList.Add(ObjNew);

                                ListItems obj = new ListItems();
                                obj.Image1 = new BitmapImage(new Uri(item.ServerImageName, UriKind.Relative));
                                obj.ImagePath = item.ImagePath;
                                obj.ID = item.ID;
                                obj.SeqNo = item.SeqNo;
                                obj.Photo = item.Photo;
                                obj.Day = item.Day;
                                obj.UnitID = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.UnitID;
                                obj.SerialOocyteNumber = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.SerialOocyteNumber;
                                obj.OocyteNumber = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.OocyteNumber;
                                obj.PatientID = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.PatientID;
                                obj.PatientUnitID = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.PatientUnitID;
                                obj.PlanTherapyID = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.PlanTherapyID;
                                obj.PlanTherapyUnitID = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.PlanTherapyUnitID;
                                obj.OriginalImagePath = item.OriginalImagePath;
                                if (((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.Isfreezed == true)
                                    obj.IsDelete = false;
                                else
                                    obj.IsDelete = true;
                                mylistitem.Add(obj);
                                GetMylistitem.Add(obj);
                                //mylistitem.Add(
                                //    new ListItems
                                //    {
                                //        Image1 = new BitmapImage(new Uri(item.ServerImageName, UriKind.Absolute)),
                                //        ImagePath = item.ImagePath,
                                //        ID = item.ID,
                                //        SeqNo = item.SeqNo,
                                //        Photo = item.Photo,
                                //        Day = item.Day,
                                //        UnitID = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.UnitID,
                                //        SerialOocyteNumber = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.SerialOocyteNumber,
                                //        OocyteNumber = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.OocyteNumber,
                                //        PatientID = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.PatientID,
                                //        PatientUnitID = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.PatientUnitID,
                                //        PlanTherapyID = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.PlanTherapyID,
                                //        PlanTherapyUnitID = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.PlanTherapyUnitID,
                                //        OriginalImagePath = item.OriginalImagePath
                                //    });

                            }

                            dgImgList.ItemsSource = null;
                            dgImgList.ItemsSource = mylistitem;
                            //GetMylistitem = mylistitem;
                            txtFN.Text = mylistitem.Count.ToString();
                        }

                        if (((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.DetailList != null && ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.DetailList.Count > 0)
                        {
                            DocList = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.DetailList;
                            foreach (var item in DocList)
                            {
                                if (((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.Isfreezed == true)
                                    item.IsDelete = false;
                                else
                                    item.IsDelete = true;
                            }
                            dgDocumentGrid.ItemsSource = null;
                            dgDocumentGrid.ItemsSource = DocList;
                            GetDocList = ((clsIVFDashboard_GetDay2DetailsBizActionVO)arg.Result).Details.DetailList;
                            txtFileName.Text = DocList.Count.ToString();
                        }

                    }
                    FillDay2OocyteDate();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        public void fillDetailsLabDay3()
        {
            clsIVFDashboard_GetDay3DetailsBizActionVO BizAction = new clsIVFDashboard_GetDay3DetailsBizActionVO();
            BizAction.Details = new clsIVFDashboard_LabDaysVO();
            BizAction.Details.PlanTherapyID = PlanTherapyId;
            BizAction.Details.PlanTherapyUnitID = PlanTherapyUnitId;
            //if (CoupleDetails != null)
            //{
            //    BizAction.Details.PatientID = CoupleDetails.FemalePatient.PatientID;
            //    BizAction.Details.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            //}
            BizAction.Details.SerialOocyteNumber = SerialOocNo;
            //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details != null)
                    {
                        this.DataContext = ((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details;
                        OocyteDonorID = ((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.OocyteDonorID;
                        OocyteDonorUnitID = ((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.OocyteDonorUnitID;


                        txtImpression.Text = Convert.ToString(((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.Impression);// BY BHUSHAN



                        if (((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.CellNo != null)
                        {
                            cmbCellNo.SelectedValue = ((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.CellNo;
                        }

                        if (((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.FileName != null)
                        {
                            txtFN.Text = ((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.FileName;
                        }
                        if (((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.Isfreezed == true)
                        {
                            //if (((IApplicationConfiguration)App.Current).CurrentUser.RoleName == "Administrator")
                            //{
                            //    cmdNew.IsEnabled = true;
                            //    cmbnextplan.IsEnabled = false;
                            //}
                            //else
                            //{
                            cmdNew.IsEnabled = false;
                            //}
                            cmdPGD.Visibility = Visibility.Visible;
                            if (Day3PGDPGS)
                                cmdPGD.IsEnabled = false;
                            else
                                cmdPGD.IsEnabled = true;
                        }
                        else
                        {
                            cmdPGD.Visibility = Visibility.Visible;
                            cmdPGD.IsEnabled = false;
                        }


                        if (((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.Isfreezed == true)
                            hlbBiopsySave.Visibility = Visibility.Visible;
                        else
                            hlbBiopsySave.Visibility = Visibility.Collapsed;

                        if (((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.CellObservationDate != null)
                        {
                            dtObservationDate.SelectedDate = ((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.CellObservationDate;
                        }
                        else
                        {
                            dtObservationDate.SelectedDate = date;
                        }

                        if (((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.CellObservationTime != null)
                        {
                            dtObservationTime.Value = ((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.CellObservationTime;
                        }
                        else
                        {
                            dtObservationTime.Value = DateTime.Now;
                        }

                        if (((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.CellStage != null)
                        {
                            txtCellStage.Text = ((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.CellStage;
                        }

                        if (((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.IsBiopsy == true)
                        {
                            chkSendForBiopsy.IsChecked = true;
                            dtDate.IsEnabled = true;
                            dtTime.IsEnabled = true;
                            txtNoOfCell.IsEnabled = true;
                        }

                        if (((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.BiopsyDate != null)
                        {
                            dtDate.SelectedDate = ((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.BiopsyDate;
                        }

                        if (((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.BiopsyTime != null)
                        {
                            dtTime.Value = ((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.BiopsyTime;
                        }

                        if (((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.IsBiopsy == false)
                        {
                            dtDate.Text = null;
                            dtTime.Value = null;
                        }

                        if (((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.NoOfCell != null)
                        {
                            txtNoOfCell.Text = ((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.NoOfCell.ToString();
                        }

                        if (((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.EmbryologistID != null)
                        {
                            cmbEmbroyologist.SelectedValue = ((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.EmbryologistID;
                        }

                        if (((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.AssitantEmbryologistID != null)
                        {
                            cmbWitnessedEmbroyologist.SelectedValue = ((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.AssitantEmbryologistID;
                        }

                        if (((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.IsEmbryoCompacted == true)
                        {
                            rdoYes.IsChecked = true;
                        }
                        if (((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.IsEmbryoCompacted == false)
                        {
                            rdoNo.IsChecked = true;
                        }

                        if (rdoYes.IsChecked == true)
                        {
                            if (((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.GradeID != null)
                            {
                                cmbGrade3.SelectedValue = ((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.GradeID;
                            }
                        }

                        if (rdoNo.IsChecked == true)
                        {
                            if (((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.GradeID != null)
                            {
                                cmbGrade2.SelectedValue = ((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.GradeID;
                            }

                            if (((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.FrgmentationID != null)
                            {
                                cmbFragmentation.SelectedValue = ((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.FrgmentationID;
                            }
                        }

                        cmbObservationDay.SelectedValue = (long)3;

                        if (((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.ImgList != null && ((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.ImgList.Count > 0)
                        {
                            ImageList = new List<clsAddImageVO>();
                            mylistitem = new List<ListItems>();
                            foreach (var item in ((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.ImgList)
                            {
                                //BitmapImage img = new BitmapImage();
                                //img.SetSource(new MemoryStream(item.Photo, false));

                                //clsAddImageVO ObjNew = new clsAddImageVO();
                                //ObjNew.Photo = item.Photo;
                                //ObjNew.ImagePath = item.ImagePath;
                                ////ObjNew.BitImagePath = img;

                                //ImageList.Add(ObjNew);

                                ListItems obj = new ListItems();
                                obj.Image1 = new BitmapImage(new Uri(item.ServerImageName, UriKind.Relative));
                                obj.ImagePath = item.ImagePath;
                                obj.ID = item.ID;
                                obj.SeqNo = item.SeqNo;
                                obj.Photo = item.Photo;
                                obj.Day = item.Day;
                                obj.UnitID = ((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.UnitID;
                                obj.SerialOocyteNumber = ((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.SerialOocyteNumber;
                                obj.OocyteNumber = ((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.OocyteNumber;
                                obj.PatientID = ((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.PatientID;
                                obj.PatientUnitID = ((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.PatientUnitID;
                                obj.PlanTherapyID = ((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.PlanTherapyID;
                                obj.PlanTherapyUnitID = ((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.PlanTherapyUnitID;
                                obj.OriginalImagePath = item.OriginalImagePath;
                                if (((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.Isfreezed == true)
                                    obj.IsDelete = false;
                                else
                                    obj.IsDelete = true;
                                mylistitem.Add(obj);
                                GetMylistitem.Add(obj);


                                //mylistitem.Add(
                                //    new ListItems
                                //    {
                                //        Image1 = new BitmapImage(new Uri(item.ServerImageName, UriKind.Absolute)),
                                //        ImagePath = item.ImagePath,
                                //        ID = item.ID,
                                //        SeqNo = item.SeqNo,
                                //        Photo = item.Photo,
                                //        Day = item.Day,
                                //        UnitID = ((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.UnitID,
                                //        SerialOocyteNumber = ((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.SerialOocyteNumber,
                                //        OocyteNumber = ((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.OocyteNumber,
                                //        PatientID = ((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.PatientID,
                                //        PatientUnitID = ((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.PatientUnitID,
                                //        PlanTherapyID = ((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.PlanTherapyID,
                                //        PlanTherapyUnitID = ((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.PlanTherapyUnitID,
                                //        OriginalImagePath = item.OriginalImagePath
                                //    });

                            }

                            dgImgList.ItemsSource = null;
                            dgImgList.ItemsSource = mylistitem;
                            //GetMylistitem = mylistitem;
                            txtFN.Text = mylistitem.Count.ToString();
                        }

                        if (((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.DetailList != null && ((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.DetailList.Count > 0)
                        {
                            DocList = ((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.DetailList;
                            foreach (var item in DocList)
                            {
                                if (((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.Isfreezed == true)
                                    item.IsDelete = false;
                                else
                                    item.IsDelete = true;
                            }
                            dgDocumentGrid.ItemsSource = null;
                            dgDocumentGrid.ItemsSource = DocList;
                            GetDocList = ((clsIVFDashboard_GetDay3DetailsBizActionVO)arg.Result).Details.DetailList;
                            txtFileName.Text = DocList.Count.ToString();
                        }

                    }
                    FillDay3OocyteDate();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        public void fillDetailsLabDay4()
        {
            clsIVFDashboard_GetDay4DetailsBizActionVO BizAction = new clsIVFDashboard_GetDay4DetailsBizActionVO();
            BizAction.Details = new clsIVFDashboard_LabDaysVO();
            BizAction.Details.PlanTherapyID = PlanTherapyId;
            BizAction.Details.PlanTherapyUnitID = PlanTherapyUnitId;
            //if (CoupleDetails != null)
            //{
            //    BizAction.Details.PatientID = CoupleDetails.FemalePatient.PatientID;
            //    BizAction.Details.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            //}
            BizAction.Details.SerialOocyteNumber = SerialOocNo;
            //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details != null)
                    {
                        this.DataContext = ((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details;
                        OocyteDonorID = ((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.OocyteDonorID;
                        OocyteDonorUnitID = ((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.OocyteDonorUnitID;


                        txtImpression.Text = Convert.ToString(((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.Impression);// BY BHUSHAN

                        if (((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.GradeID != null)
                        {
                            cmbGrade4.SelectedValue = ((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.GradeID;
                        }

                        if (((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.CellNo != null)
                        {
                            cmbCellNo.SelectedValue = ((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.CellNo;
                        }

                        if (((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.FileName != null)
                        {
                            txtFN.Text = ((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.FileName;
                        }
                        if (((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.Isfreezed == true)
                        {
                            //if (((IApplicationConfiguration)App.Current).CurrentUser.RoleName == "Administrator")
                            //{
                            //    cmdNew.IsEnabled = true;
                            //    cmbnextplan.IsEnabled = false;
                            //}
                            //else
                            //{
                            cmdNew.IsEnabled = false;
                            //}
                        }
                        if (((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.CellObservationDate != null)
                        {
                            dtObservationDate.SelectedDate = ((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.CellObservationDate;
                        }
                        else
                        {
                            dtObservationDate.SelectedDate = date;
                        }

                        if (((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.CellObservationTime != null)
                        {
                            dtObservationTime.Value = ((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.CellObservationTime;
                        }
                        else
                        {
                            dtObservationTime.Value = DateTime.Now;
                        }

                        if (((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.CellStage != null)
                        {
                            txtCellStage.Text = ((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.CellStage;
                        }

                        if (((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.IsBiopsy == true)
                        {
                            chkSendForBiopsy.IsChecked = true;
                            dtDate.IsEnabled = true;
                            dtTime.IsEnabled = true;
                            txtNoOfCell.IsEnabled = true;
                        }

                        if (((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.BiopsyDate != null)
                        {
                            dtDate.SelectedDate = ((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.BiopsyDate;
                        }

                        if (((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.BiopsyTime != null)
                        {
                            dtTime.Value = ((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.BiopsyTime;
                        }

                        if (((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.IsBiopsy == false)
                        {
                            dtDate.Text = null;
                            dtTime.Value = null;
                        }

                        if (((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.NoOfCell != null)
                        {
                            txtNoOfCell.Text = ((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.NoOfCell.ToString();
                        }

                        if (((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.AssistedHatching == true)
                        {
                            chkAssistedHatching.IsChecked = true;
                        }

                        if (((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.EmbryologistID != null)
                        {
                            cmbEmbroyologist.SelectedValue = ((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.EmbryologistID;
                        }

                        if (((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.AssitantEmbryologistID != null)
                        {
                            cmbWitnessedEmbroyologist.SelectedValue = ((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.AssitantEmbryologistID;
                        }

                        cmbObservationDay.SelectedValue = (long)4;

                        if (((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.ImgList != null && ((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.ImgList.Count > 0)
                        {
                            ImageList = new List<clsAddImageVO>();
                            mylistitem = new List<ListItems>();
                            foreach (var item in ((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.ImgList)
                            {
                                //BitmapImage img = new BitmapImage();
                                //img.SetSource(new MemoryStream(item.Photo, false));

                                //clsAddImageVO ObjNew = new clsAddImageVO();
                                //ObjNew.Photo = item.Photo;
                                //ObjNew.ImagePath = item.ImagePath;
                                ////ObjNew.BitImagePath = img;

                                //ImageList.Add(ObjNew);
                                ListItems obj = new ListItems();
                                obj.Image1 = new BitmapImage(new Uri(item.ServerImageName, UriKind.Relative));
                                obj.ImagePath = item.ImagePath;
                                obj.ID = item.ID;
                                obj.SeqNo = item.SeqNo;
                                obj.Photo = item.Photo;
                                obj.Day = item.Day;
                                obj.UnitID = ((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.UnitID;
                                obj.SerialOocyteNumber = ((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.SerialOocyteNumber;
                                obj.OocyteNumber = ((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.OocyteNumber;
                                obj.PatientID = ((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.PatientID;
                                obj.PatientUnitID = ((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.PatientUnitID;
                                obj.PlanTherapyID = ((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.PlanTherapyID;
                                obj.PlanTherapyUnitID = ((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.PlanTherapyUnitID;
                                obj.OriginalImagePath = item.OriginalImagePath;
                                if (((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.Isfreezed == true)
                                    obj.IsDelete = false;
                                else
                                    obj.IsDelete = true;
                                mylistitem.Add(obj);
                                GetMylistitem.Add(obj);

                                //mylistitem.Add(
                                //    new ListItems
                                //    {
                                //        Image1 = new BitmapImage(new Uri(item.ServerImageName, UriKind.Absolute)),
                                //        ImagePath = item.ImagePath,
                                //        ID = item.ID,
                                //        SeqNo = item.SeqNo,
                                //        Photo = item.Photo,
                                //        Day = item.Day,
                                //        UnitID = ((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.UnitID,
                                //        SerialOocyteNumber = ((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.SerialOocyteNumber,
                                //        OocyteNumber = ((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.OocyteNumber,
                                //        PatientID = ((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.PatientID,
                                //        PatientUnitID = ((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.PatientUnitID,
                                //        PlanTherapyID = ((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.PlanTherapyID,
                                //        PlanTherapyUnitID = ((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.PlanTherapyUnitID,
                                //        OriginalImagePath = item.OriginalImagePath
                                //    });

                            }

                            dgImgList.ItemsSource = null;
                            dgImgList.ItemsSource = mylistitem;
                            //GetMylistitem = mylistitem;
                            txtFN.Text = mylistitem.Count.ToString();
                        }

                        if (((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.DetailList != null && ((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.DetailList.Count > 0)
                        {
                            DocList = ((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.DetailList;
                            foreach (var item in DocList)
                            {
                                if (((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.Isfreezed == true)
                                    item.IsDelete = false;
                                else
                                    item.IsDelete = true;
                            }
                            dgDocumentGrid.ItemsSource = null;
                            dgDocumentGrid.ItemsSource = DocList;
                            GetDocList = ((clsIVFDashboard_GetDay4DetailsBizActionVO)arg.Result).Details.DetailList;
                            txtFileName.Text = DocList.Count.ToString();
                        }

                    }
                    FillDay4OocyteDate();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        public void fillDetailsLabDay5()
        {
            clsIVFDashboard_GetDay5DetailsBizActionVO BizAction = new clsIVFDashboard_GetDay5DetailsBizActionVO();
            BizAction.Details = new clsIVFDashboard_LabDaysVO();
            BizAction.Details.PlanTherapyID = PlanTherapyId;
            BizAction.Details.PlanTherapyUnitID = PlanTherapyUnitId;
            //if (CoupleDetails != null)
            //{
            //    BizAction.Details.PatientID = CoupleDetails.FemalePatient.PatientID;
            //    BizAction.Details.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            //}
            BizAction.Details.SerialOocyteNumber = SerialOocNo;
            //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details != null)
                    {
                        this.DataContext = ((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details;
                        OocyteDonorID = ((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.OocyteDonorID;
                        OocyteDonorUnitID = ((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.OocyteDonorUnitID;


                        txtImpression.Text = Convert.ToString(((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.Impression);// BY BHUSHAN

                        //if (((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.GradeID != null)
                        //{
                        //    cmbGrade.SelectedValue = ((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.GradeID;
                        //}

                        if (((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.CellNo != null)
                        {
                            cmbCellNo.SelectedValue = ((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.CellNo;
                        }

                        if (((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.FileName != null)
                        {
                            txtFN.Text = ((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.FileName;
                        }
                        if (((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.Isfreezed == true)
                        {
                            //if (((IApplicationConfiguration)App.Current).CurrentUser.RoleName == "Administrator")
                            //{
                            //    cmdNew.IsEnabled = true;
                            //    cmbnextplan.IsEnabled = false;
                            //}
                            //else
                            //{
                            cmdNew.IsEnabled = false;
                            //}
                            cmdPGD.Visibility = Visibility.Visible;
                            if (Day5PGDPGS)
                                cmdPGD.IsEnabled = false;
                            else
                                cmdPGD.IsEnabled = true;
                        }
                        else
                        {
                            cmdPGD.Visibility = Visibility.Visible;
                            cmdPGD.IsEnabled = false;
                        }

                        if (((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.Isfreezed == true)
                            hlbBiopsySave.Visibility = Visibility.Visible;
                        else
                            hlbBiopsySave.Visibility = Visibility.Collapsed;

                        if (((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.CellObservationDate != null)
                        {
                            dtObservationDate.SelectedDate = ((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.CellObservationDate;
                        }
                        else
                        {
                            dtObservationDate.SelectedDate = date;
                        }

                        if (((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.CellObservationTime != null)
                        {
                            dtObservationTime.Value = ((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.CellObservationTime;
                        }
                        else
                        {
                            dtObservationTime.Value = DateTime.Now;
                        }

                        if (((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.CellStage != null)
                        {
                            txtCellStage.Text = ((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.CellStage;
                        }

                        if (((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.IsBiopsy == true)
                        {
                            chkSendForBiopsy.IsChecked = true;
                            dtDate.IsEnabled = true;
                            dtTime.IsEnabled = true;
                            txtNoOfCell.IsEnabled = true;
                        }

                        if (((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.BiopsyDate != null)
                        {
                            dtDate.SelectedDate = ((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.BiopsyDate;
                        }

                        if (((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.BiopsyTime != null)
                        {
                            dtTime.Value = ((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.BiopsyTime;
                        }

                        if (((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.IsBiopsy == false)
                        {
                            dtDate.Text = null;
                            dtTime.Value = null;
                        }

                        if (((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.NoOfCell != null)
                        {
                            txtNoOfCell.Text = ((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.NoOfCell.ToString();
                        }

                        if (((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.EmbryologistID != null)
                        {
                            cmbEmbroyologist.SelectedValue = ((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.EmbryologistID;
                        }

                        if (((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.AssitantEmbryologistID != null)
                        {
                            cmbWitnessedEmbroyologist.SelectedValue = ((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.AssitantEmbryologistID;
                        }

                        cmbObservationDay.SelectedValue = (long)5;

                        if (((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.ImgList != null && ((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.ImgList.Count > 0)
                        {
                            ImageList = new List<clsAddImageVO>();
                            mylistitem = new List<ListItems>();
                            foreach (var item in ((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.ImgList)
                            {
                                //BitmapImage img = new BitmapImage();
                                //img.SetSource(new MemoryStream(item.Photo, false));

                                //clsAddImageVO ObjNew = new clsAddImageVO();
                                //ObjNew.Photo = item.Photo;
                                //ObjNew.ImagePath = item.ImagePath;
                                ////ObjNew.BitImagePath = img;

                                //ImageList.Add(ObjNew);
                                ListItems obj = new ListItems();
                                obj.Image1 = new BitmapImage(new Uri(item.ServerImageName, UriKind.Relative));
                                obj.ImagePath = item.ImagePath;
                                obj.ID = item.ID;
                                obj.SeqNo = item.SeqNo;
                                obj.Photo = item.Photo;
                                obj.Day = item.Day;
                                obj.UnitID = ((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.UnitID;
                                obj.SerialOocyteNumber = ((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.SerialOocyteNumber;
                                obj.OocyteNumber = ((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.OocyteNumber;
                                obj.PatientID = ((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.PatientID;
                                obj.PatientUnitID = ((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.PatientUnitID;
                                obj.PlanTherapyID = ((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.PlanTherapyID;
                                obj.PlanTherapyUnitID = ((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.PlanTherapyUnitID;
                                obj.OriginalImagePath = item.OriginalImagePath;
                                if (((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.Isfreezed == true)
                                    obj.IsDelete = false;
                                else
                                    obj.IsDelete = true;
                                mylistitem.Add(obj);
                                GetMylistitem.Add(obj);

                                //mylistitem.Add(
                                //    new ListItems
                                //    {
                                //        Image1 = new BitmapImage(new Uri(item.ServerImageName, UriKind.Absolute)),
                                //        ImagePath = item.ImagePath,
                                //        ID = item.ID,
                                //        SeqNo = item.SeqNo,
                                //        Photo = item.Photo,
                                //        Day = item.Day,
                                //        UnitID = ((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.UnitID,
                                //        SerialOocyteNumber = ((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.SerialOocyteNumber,
                                //        OocyteNumber = ((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.OocyteNumber,
                                //        PatientID = ((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.PatientID,
                                //        PatientUnitID = ((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.PatientUnitID,
                                //        PlanTherapyID = ((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.PlanTherapyID,
                                //        PlanTherapyUnitID = ((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.PlanTherapyUnitID,
                                //        OriginalImagePath = item.OriginalImagePath
                                //    });

                            }

                            dgImgList.ItemsSource = null;
                            dgImgList.ItemsSource = mylistitem;
                            //GetMylistitem = mylistitem;
                            txtFN.Text = mylistitem.Count.ToString();
                        }

                        if (((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.DetailList != null && ((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.DetailList.Count > 0)
                        {
                            DocList = ((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.DetailList;
                            foreach (var item in DocList)
                            {
                                if (((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.Isfreezed == true)
                                    item.IsDelete = false;
                                else
                                    item.IsDelete = true;
                            }
                            dgDocumentGrid.ItemsSource = null;
                            dgDocumentGrid.ItemsSource = DocList;
                            GetDocList = ((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.DetailList;
                            txtFileName.Text = DocList.Count.ToString();
                        }

                        if (((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.StageofDevelopmentGrade != null)
                        {
                            cmbStageofDevelopmentGrade.SelectedValue = ((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.StageofDevelopmentGrade;
                        }

                        if (((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.InnerCellMassGrade != null)
                        {
                            cmbInnerCellMassGrade.SelectedValue = ((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.InnerCellMassGrade;
                        }

                        if (((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.TrophoectodermGrade != null)
                        {
                            cmbTrophoectodermGrade.SelectedValue = ((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.TrophoectodermGrade;
                        }

                        if (((clsIVFDashboard_GetDay5DetailsBizActionVO)arg.Result).Details.AssistedHatching == true)
                        {
                            chkAssistedHatching.IsChecked = true;
                        }
                    }
                    FillDay5OocyteDate();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        public void fillDetailsLabDay6()
        {
            clsIVFDashboard_GetDay6DetailsBizActionVO BizAction = new clsIVFDashboard_GetDay6DetailsBizActionVO();
            BizAction.Details = new clsIVFDashboard_LabDaysVO();
            BizAction.Details.PlanTherapyID = PlanTherapyId;
            BizAction.Details.PlanTherapyUnitID = PlanTherapyUnitId;
            //if (CoupleDetails != null)
            //{
            //    BizAction.Details.PatientID = CoupleDetails.FemalePatient.PatientID;
            //    BizAction.Details.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            //}
            BizAction.Details.SerialOocyteNumber = SerialOocNo;
            //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details != null)
                    {
                        this.DataContext = ((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details;
                        OocyteDonorID = ((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.OocyteDonorID;
                        OocyteDonorUnitID = ((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.OocyteDonorUnitID;


                        txtImpression.Text = Convert.ToString(((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.Impression);// BY BHUSHAN

                        //if (((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.GradeID != null)
                        //{
                        //    cmbGrade.SelectedValue = ((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.GradeID;
                        //}

                        if (((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.CellNo != null)
                        {
                            cmbCellNo.SelectedValue = ((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.CellNo;
                        }

                        if (((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.FileName != null)
                        {
                            txtFN.Text = ((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.FileName;
                        }
                        if (((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.Isfreezed == true)
                        {
                            //if (((IApplicationConfiguration)App.Current).CurrentUser.RoleName == "Administrator")
                            //{
                            //    cmdNew.IsEnabled = true;
                            //    cmbnextplan.IsEnabled = false;
                            //}
                            //else
                            //{
                            cmdNew.IsEnabled = false;
                            //}
                            cmdPGD.Visibility = Visibility.Visible;
                            if (Day6PGDPGS)
                                cmdPGD.IsEnabled = false;
                            else
                                cmdPGD.IsEnabled = true;
                        }
                        else
                        {
                            cmdPGD.Visibility = Visibility.Visible;
                            cmdPGD.IsEnabled = false;
                        }

                        if (((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.Isfreezed == true)
                            hlbBiopsySave.Visibility = Visibility.Visible;
                        else
                            hlbBiopsySave.Visibility = Visibility.Collapsed;

                        if (((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.CellObservationDate != null)
                        {
                            dtObservationDate.SelectedDate = ((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.CellObservationDate;
                        }
                        else
                        {
                            dtObservationDate.SelectedDate = date;
                        }

                        if (((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.CellObservationTime != null)
                        {
                            dtObservationTime.Value = ((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.CellObservationTime;
                        }
                        else
                        {
                            dtObservationTime.Value = DateTime.Now;
                        }

                        if (((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.CellStage != null)
                        {
                            txtCellStage.Text = ((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.CellStage;
                        }

                        if (((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.IsBiopsy == true)
                        {
                            chkSendForBiopsy.IsChecked = true;
                            dtDate.IsEnabled = true;
                            dtTime.IsEnabled = true;
                            txtNoOfCell.IsEnabled = true;
                        }

                        if (((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.BiopsyDate != null)
                        {
                            dtDate.SelectedDate = ((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.BiopsyDate;
                        }

                        if (((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.BiopsyTime != null)
                        {
                            dtTime.Value = ((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.BiopsyTime;
                        }

                        if (((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.IsBiopsy == false)
                        {
                            dtDate.Text = null;
                            dtTime.Value = null;
                        }

                        if (((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.NoOfCell != null)
                        {
                            txtNoOfCell.Text = ((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.NoOfCell.ToString();
                        }

                        if (((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.EmbryologistID != null)
                        {
                            cmbEmbroyologist.SelectedValue = ((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.EmbryologistID;
                        }

                        if (((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.AssitantEmbryologistID != null)
                        {
                            cmbWitnessedEmbroyologist.SelectedValue = ((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.AssitantEmbryologistID;
                        }

                        cmbObservationDay.SelectedValue = (long)6;

                        if (((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.ImgList != null && ((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.ImgList.Count > 0)
                        {
                            ImageList = new List<clsAddImageVO>();
                            mylistitem = new List<ListItems>();
                            foreach (var item in ((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.ImgList)
                            {
                                //BitmapImage img = new BitmapImage();
                                //img.SetSource(new MemoryStream(item.Photo, false));

                                //clsAddImageVO ObjNew = new clsAddImageVO();
                                //ObjNew.Photo = item.Photo;
                                //ObjNew.ImagePath = item.ImagePath;
                                ////ObjNew.BitImagePath = img;

                                //ImageList.Add(ObjNew);
                                ListItems obj = new ListItems();
                                obj.Image1 = new BitmapImage(new Uri(item.ServerImageName, UriKind.Relative));
                                obj.ImagePath = item.ImagePath;
                                obj.ID = item.ID;
                                obj.SeqNo = item.SeqNo;
                                obj.Photo = item.Photo;
                                obj.Day = item.Day;
                                obj.UnitID = ((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.UnitID;
                                obj.SerialOocyteNumber = ((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.SerialOocyteNumber;
                                obj.OocyteNumber = ((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.OocyteNumber;
                                obj.PatientID = ((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.PatientID;
                                obj.PatientUnitID = ((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.PatientUnitID;
                                obj.PlanTherapyID = ((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.PlanTherapyID;
                                obj.PlanTherapyUnitID = ((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.PlanTherapyUnitID;
                                obj.OriginalImagePath = item.OriginalImagePath;
                                if (((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.Isfreezed == true)
                                    obj.IsDelete = false;
                                else
                                    obj.IsDelete = true;
                                mylistitem.Add(obj);
                                GetMylistitem.Add(obj);

                                //mylistitem.Add(
                                //    new ListItems
                                //    {
                                //        Image1 = new BitmapImage(new Uri(item.ServerImageName, UriKind.Absolute)),
                                //        ImagePath = item.ImagePath,
                                //        ID = item.ID,
                                //        SeqNo = item.SeqNo,
                                //        Photo = item.Photo,
                                //        Day = item.Day,
                                //        UnitID = ((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.UnitID,
                                //        SerialOocyteNumber = ((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.SerialOocyteNumber,
                                //        OocyteNumber = ((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.OocyteNumber,
                                //        PatientID = ((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.PatientID,
                                //        PatientUnitID = ((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.PatientUnitID,
                                //        PlanTherapyID = ((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.PlanTherapyID,
                                //        PlanTherapyUnitID = ((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.PlanTherapyUnitID,
                                //        OriginalImagePath = item.OriginalImagePath
                                //    });

                            }

                            dgImgList.ItemsSource = null;
                            dgImgList.ItemsSource = mylistitem;
                            //GetMylistitem = mylistitem;
                            txtFN.Text = mylistitem.Count.ToString();
                        }

                        if (((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.DetailList != null && ((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.DetailList.Count > 0)
                        {
                            DocList = ((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.DetailList;
                            foreach (var item in DocList)
                            {
                                if (((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.Isfreezed == true)
                                    item.IsDelete = false;
                                else
                                    item.IsDelete = true;
                            }
                            dgDocumentGrid.ItemsSource = null;
                            dgDocumentGrid.ItemsSource = DocList;
                            GetDocList = ((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.DetailList;
                            txtFileName.Text = DocList.Count.ToString();
                        }

                        if (((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.StageofDevelopmentGrade != null)
                        {
                            cmbStageofDevelopmentGrade.SelectedValue = ((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.StageofDevelopmentGrade;
                        }

                        if (((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.InnerCellMassGrade != null)
                        {
                            cmbInnerCellMassGrade.SelectedValue = ((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.InnerCellMassGrade;
                        }

                        if (((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.TrophoectodermGrade != null)
                        {
                            cmbTrophoectodermGrade.SelectedValue = ((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.TrophoectodermGrade;
                        }

                        if (((clsIVFDashboard_GetDay6DetailsBizActionVO)arg.Result).Details.AssistedHatching == true)
                        {
                            chkAssistedHatching.IsChecked = true;
                        }
                    }
                    FillDay6OocyteDate();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        public void fillDetailsLabDay7()
        {
            clsIVFDashboard_GetDay7DetailsBizActionVO BizAction = new clsIVFDashboard_GetDay7DetailsBizActionVO();
            BizAction.Details = new clsIVFDashboard_LabDaysVO();
            BizAction.Details.PlanTherapyID = PlanTherapyId;
            BizAction.Details.PlanTherapyUnitID = PlanTherapyUnitId;
            //if (CoupleDetails != null)
            //{
            //    BizAction.Details.PatientID = CoupleDetails.FemalePatient.PatientID;
            //    BizAction.Details.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            //}
            BizAction.Details.SerialOocyteNumber = SerialOocNo;
            //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details != null)
                    {
                        this.DataContext = ((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details;
                        OocyteDonorID = ((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.OocyteDonorID;
                        OocyteDonorUnitID = ((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.OocyteDonorUnitID;


                        txtImpression.Text = Convert.ToString(((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.Impression);// BY BHUSHAN

                        //if (((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.GradeID != null)
                        //{
                        //    cmbGrade.SelectedValue = ((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.GradeID;
                        //}

                        if (((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.CellNo != null)
                        {
                            cmbCellNo.SelectedValue = ((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.CellNo;
                        }

                        if (((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.FileName != null)
                        {
                            txtFN.Text = ((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.FileName;
                        }
                        if (((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.Isfreezed == true)
                        {
                            //if (((IApplicationConfiguration)App.Current).CurrentUser.RoleName == "Administrator")
                            //{
                            //    cmdNew.IsEnabled = true;
                            //    cmbnextplan.IsEnabled = false;
                            //}
                            //else
                            //{
                            cmdNew.IsEnabled = false;
                            //}
                            cmdPGD.Visibility = Visibility.Visible;
                            if (Day7PGDPGS)
                                cmdPGD.IsEnabled = false;
                            else
                                cmdPGD.IsEnabled = true;
                        }
                        else
                        {
                            cmdPGD.Visibility = Visibility.Visible;
                            cmdPGD.IsEnabled = false;
                        }

                        if (((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.Isfreezed == true)
                            hlbBiopsySave.Visibility = Visibility.Visible;
                        else
                            hlbBiopsySave.Visibility = Visibility.Collapsed;

                        if (((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.CellObservationDate != null)
                        {
                            dtObservationDate.SelectedDate = ((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.CellObservationDate;
                        }
                        else
                        {
                            dtObservationDate.SelectedDate = date;
                        }

                        if (((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.CellObservationTime != null)
                        {
                            dtObservationTime.Value = ((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.CellObservationTime;
                        }
                        else
                        {
                            dtObservationTime.Value = DateTime.Now;
                        }

                        if (((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.CellStage != null)
                        {
                            txtCellStage.Text = ((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.CellStage;
                        }

                        if (((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.IsBiopsy == true)
                        {
                            chkSendForBiopsy.IsChecked = true;
                            dtDate.IsEnabled = true;
                            dtTime.IsEnabled = true;
                            txtNoOfCell.IsEnabled = true;
                        }

                        if (((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.BiopsyDate != null)
                        {
                            dtDate.SelectedDate = ((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.BiopsyDate;
                        }

                        if (((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.BiopsyTime != null)
                        {
                            dtTime.Value = ((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.BiopsyTime;
                        }

                        if (((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.IsBiopsy == false)
                        {
                            dtDate.Text = null;
                            dtTime.Value = null;
                        }

                        if (((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.NoOfCell != null)
                        {
                            txtNoOfCell.Text = ((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.NoOfCell.ToString();
                        }

                        if (((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.EmbryologistID != null)
                        {
                            cmbEmbroyologist.SelectedValue = ((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.EmbryologistID;
                        }

                        if (((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.AssitantEmbryologistID != null)
                        {
                            cmbWitnessedEmbroyologist.SelectedValue = ((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.AssitantEmbryologistID;
                        }

                        cmbObservationDay.SelectedValue = (long)7;

                        if (((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.ImgList != null && ((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.ImgList.Count > 0)
                        {
                            ImageList = new List<clsAddImageVO>();
                            mylistitem = new List<ListItems>();
                            foreach (var item in ((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.ImgList)
                            {
                                //BitmapImage img = new BitmapImage();
                                //img.SetSource(new MemoryStream(item.Photo, false));

                                //clsAddImageVO ObjNew = new clsAddImageVO();
                                //ObjNew.Photo = item.Photo;
                                //ObjNew.ImagePath = item.ImagePath;
                                ////ObjNew.BitImagePath = img;

                                //ImageList.Add(ObjNew);
                                ListItems obj = new ListItems();
                                obj.Image1 = new BitmapImage(new Uri(item.ServerImageName, UriKind.Relative));
                                obj.ImagePath = item.ImagePath;
                                obj.ID = item.ID;
                                obj.SeqNo = item.SeqNo;
                                obj.Photo = item.Photo;
                                obj.Day = item.Day;
                                obj.UnitID = ((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.UnitID;
                                obj.SerialOocyteNumber = ((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.SerialOocyteNumber;
                                obj.OocyteNumber = ((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.OocyteNumber;
                                obj.PatientID = ((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.PatientID;
                                obj.PatientUnitID = ((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.PatientUnitID;
                                obj.PlanTherapyID = ((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.PlanTherapyID;
                                obj.PlanTherapyUnitID = ((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.PlanTherapyUnitID;
                                obj.OriginalImagePath = item.OriginalImagePath;
                                if (((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.Isfreezed == true)
                                    obj.IsDelete = false;
                                else
                                    obj.IsDelete = true;
                                mylistitem.Add(obj);
                                GetMylistitem.Add(obj);

                                //mylistitem.Add(
                                //    new ListItems
                                //    {
                                //        Image1 = new BitmapImage(new Uri(item.ServerImageName, UriKind.Absolute)),
                                //        ImagePath = item.ImagePath,
                                //        ID = item.ID,
                                //        SeqNo = item.SeqNo,
                                //        Photo = item.Photo,
                                //        Day = item.Day,
                                //        UnitID = ((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.UnitID,
                                //        SerialOocyteNumber = ((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.SerialOocyteNumber,
                                //        OocyteNumber = ((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.OocyteNumber,
                                //        PatientID = ((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.PatientID,
                                //        PatientUnitID = ((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.PatientUnitID,
                                //        PlanTherapyID = ((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.PlanTherapyID,
                                //        PlanTherapyUnitID = ((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.PlanTherapyUnitID,
                                //        OriginalImagePath = item.OriginalImagePath
                                //    });

                            }

                            dgImgList.ItemsSource = null;
                            dgImgList.ItemsSource = mylistitem;
                            //GetMylistitem = mylistitem;
                            txtFN.Text = mylistitem.Count.ToString();
                        }

                        if (((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.DetailList != null && ((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.DetailList.Count > 0)
                        {
                            DocList = ((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.DetailList;
                            foreach (var item in DocList)
                            {
                                if (((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.Isfreezed == true)
                                    item.IsDelete = false;
                                else
                                    item.IsDelete = true;
                            }
                            dgDocumentGrid.ItemsSource = null;
                            dgDocumentGrid.ItemsSource = DocList;
                            GetDocList = ((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.DetailList;
                            txtFileName.Text = DocList.Count.ToString();
                        }

                        if (((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.StageofDevelopmentGrade != null)
                        {
                            cmbStageofDevelopmentGrade.SelectedValue = ((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.StageofDevelopmentGrade;
                        }

                        if (((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.InnerCellMassGrade != null)
                        {
                            cmbInnerCellMassGrade.SelectedValue = ((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.InnerCellMassGrade;
                        }

                        if (((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.TrophoectodermGrade != null)
                        {
                            cmbTrophoectodermGrade.SelectedValue = ((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.TrophoectodermGrade;
                        }

                        if (((clsIVFDashboard_GetDay7DetailsBizActionVO)arg.Result).Details.AssistedHatching == true)
                        {
                            chkAssistedHatching.IsChecked = true;
                        }
                    }
                    FillDay7OocyteDate();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void chkSendForBiopsy_Click(object sender, RoutedEventArgs e)
        {
            if (chkSendForBiopsy.IsChecked == true)
            {
                dtDate.SelectedDate = DateTime.Now;
                dtTime.Value = DateTime.Now;
                dtDate.IsEnabled = true;
                dtTime.IsEnabled = true;
                txtNoOfCell.IsEnabled = true;
            }
            else
            {
                dtDate.SelectedDate = null;
                dtTime.Value = null;
                dtDate.IsEnabled = false;
                dtTime.IsEnabled = false;
                txtNoOfCell.IsEnabled = false;
            }
        }

        byte[] AttachedFileContents;
        string AttachedFileName;

        private void cmdAttachedDoc_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            if (openDialog.ShowDialog() == true)
            {
                AttachedFileName = openDialog.File.Name;
                txtFileName.Text = openDialog.File.Name;
                try
                {
                    using (Stream stream = openDialog.File.OpenRead())
                    {
                        AttachedFileContents = new byte[stream.Length];
                        stream.Read(AttachedFileContents, 0, (int)stream.Length);
                        ShowDoc(AttachedFileContents, txtFileName.Text);
                    }
                }
                catch (Exception ex)
                {
                    string msgText = "Error while reading file.";

                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow("Palsh", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                    throw ex;
                }
            }
        }

        List<clsIVFDashboard_TherapyDocumentVO> DocList = new List<clsIVFDashboard_TherapyDocumentVO>();
        List<clsIVFDashboard_TherapyDocumentVO> GetDocList = new List<clsIVFDashboard_TherapyDocumentVO>();
        private void ShowDoc(byte[] FileContents, string text)
        {
            clsIVFDashboard_TherapyDocumentVO obj = new clsIVFDashboard_TherapyDocumentVO();
            obj.PatientID = CoupleDetails.FemalePatient.PatientID;
            obj.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            obj.PlanTherapyID = PlanTherapyId;
            obj.PlanTherapyUnitID = PlanTherapyUnitId;
            obj.Date = DateTime.Now;
            obj.Title = txtTitle.Text;
            obj.Description = "";
            obj.AttachedFileName = txtFileName.Text;
            obj.AttachedFileContent = AttachedFileContents;
            obj.OocyteNumber = OocyteNo;
            obj.SerialOocyteNumber = SerialOocNo;
            obj.Day = Day;
            obj.IsDelete = true;

            var item1 = from r in DocList.ToList()
                        where r.AttachedFileName == obj.AttachedFileName
                        select r;

            if (item1.ToList().Count == 0)
            {
                DocList.Add(obj);
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                             new MessageBoxControl.MessageBoxChildWindow("Palash", "'" + obj.AttachedFileName + "'" + " File is already Added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }


            dgDocumentGrid.ItemsSource = null;
            dgDocumentGrid.ItemsSource = DocList;

            txtFileName.Text = DocList.Count.ToString();
        }

        private void CmdAddDocument_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FillCellNo();
            if (IsSaved)
            {
                //ShowComboLabDay5();
                //FillCleavageGrade();
                FillEmbryologist();
               // fillOocyteDetails();
                filldata();
            }
            else
            {
                //fillOocyteDetails();
                filldata();
                //GetObservationDate();
                dtObservationDate.SelectedDate = DateTime.Now;
                dtObservationTime.Value = DateTime.Now;
                if (IsIsthambul)
                    CheckTimeAndReturnLabDay();
                else
                    CalculateLabDay();
                //FillCleavageGrade();
                FillEmbryologist();
                //ShowComboLabDay5();

            }

            if (IsIsthambul)
            {
                lblObservationDay.Visibility = Visibility.Collapsed;
                cmbObservationDay.Visibility = Visibility.Collapsed;
                lblCellStage.Visibility = Visibility.Visible;
                txtCellStage.Visibility = Visibility.Visible;
            }
            else
            {
                lblObservationDay.Visibility = Visibility.Visible;
                cmbObservationDay.Visibility = Visibility.Visible;
                lblCellStage.Visibility = Visibility.Collapsed;
                txtCellStage.Visibility = Visibility.Collapsed;
            }

            if (IsClosed)
                cmdNew.IsEnabled = false;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        bool blError = false;
        bool blSaveData = false;
        bool blAlert = false;
        bool blDayCrossed = false;

        bool blLabDay1 = false;
        bool blLabDay2 = false;
        bool blLabDay3 = false;
        bool blLabDay4 = false;
        bool blLabDay5 = false;
        bool blLabDay6 = false;
        bool blLabDay7 = false;

        DateTime ProcudureDateTime;
        DateTime CurrentObervationDateTime;

        bool blEmbError = false;
        bool blPreCleavage = false;
        bool blCleavage = false;
        bool blDay4Embryo = false;
        bool blBlastocyst = false;

        private void CheckTimeAndReturnLabDay()
        {
            Day = 0;
            ProcudureDateTime = ((DateTime)ProcedureDate).Date.Add(((DateTime)ProcedureTime).TimeOfDay);
            CurrentObervationDateTime = ((DateTime)dtObservationDate.SelectedDate).Date.Add(((DateTime)dtObservationTime.Value).TimeOfDay);

            TimeSpan diff = CurrentObervationDateTime - ProcudureDateTime;
            hours = diff.TotalHours;

            if (hours <= 18)
            {
                //Day = 1;
                blEmbError = true;
            }

            if (hours > 18 && hours < 25)
            {
                Day = 1;
                blPreCleavage = true;
            }

            if (hours >= 25 && hours <= 90)
            {
                Day = 2;
                blCleavage = true;
            }

            if (hours > 90 && hours <= 114)
            {
                Day = 4;
                blDay4Embryo = true;
            }

            if (hours > 114)
            {
                Day = 5;
                blBlastocyst = true;
            }

            if (SelectedDay.Day1 == true && SelectedDay.IsLabDay1Freezed == true)
                blLabDay1 = true;
            if (SelectedDay.Day2 == true && SelectedDay.IsLabDay2Freezed == true)
                blLabDay2 = true;
            if (SelectedDay.Day3 == true && SelectedDay.IsLabDay3Freezed == true)
                blLabDay3 = true;
            if (SelectedDay.Day4 == true && SelectedDay.IsLabDay4Freezed == true)
                blLabDay4 = true;
            if (SelectedDay.Day5 == true && SelectedDay.IsLabDay5Freezed == true)
                blLabDay5 = true;
            if (SelectedDay.Day6 == true && SelectedDay.IsLabDay6Freezed == true)
                blLabDay6 = true;

            # region hours calculation on days based commented
            //if (SelectedDay.Plan0.Equals("ICSI"))
            //{
            //    if (hours < 25)
            //    {
            //        blError = true;
            //        Day = 1;
            //    }
            //    if (hours >= 25 && hours <= 27)
            //    {
            //        blSaveData = true;
            //        Day = 1;
            //    }
            //    if (hours > 27 && hours < 43)
            //    {
            //        blAlert = true;
            //        Day = 2;
            //    }
            //}
            //else if (SelectedDay.Plan0.Equals("IVF"))
            //{
            //    if (hours < 27)
            //    {
            //        blError = true;
            //        Day = 1;
            //    }
            //    if (hours >= 27 && hours <= 29)
            //    {
            //        blSaveData = true;
            //        Day = 1;
            //    }
            //    if (hours > 29 && hours < 43)
            //    {
            //        blAlert = true;
            //        Day = 2;
            //    }
            //}

            ////if (hours < 43)
            ////{
            ////    blAlert = true;
            ////    Day = 2;
            ////}
            //if (hours >= 43 && hours <= 45)
            //{
            //    blSaveData = true;
            //    Day = 2;
            //}
            //if (hours > 45 && hours < 67)
            //{
            //    blAlert = true;
            //    Day = 3;
            //}

            ////if (hours < 67)
            ////{
            ////    blAlert = true;
            ////    Day = 3;
            ////}
            //if (hours >= 67 && hours <= 69)
            //{
            //    blSaveData = true;
            //    Day = 3;
            //}
            //if (hours > 69 && hours < 90)
            //{
            //    blAlert = true;
            //    Day = 4;
            //}

            ////if (hours < 90)
            ////{
            ////    blAlert = true;
            ////    Day = 4;
            ////}
            //if (hours >= 90 && hours <= 94)
            //{
            //    blSaveData = true;
            //    Day = 4;
            //}
            //if (hours > 94 && hours < 114)
            //{
            //    blAlert = true;
            //    Day = 5;
            //}

            ////if (hours < 114)
            ////{
            ////    blAlert = true;
            ////    Day = 5;
            ////}
            //if (hours >= 114 && hours <= 118)
            //{
            //    blSaveData = true;
            //    Day = 5;
            //}
            //if (hours > 118)
            //{
            //    blAlert = true;
            //    blSaveData = true;
            //    Day = 6;
            //}


            ////if (hours < 43)
            ////    Day = 1;
            ////if (hours >= 43 && hours < 67)
            ////    Day = 2;
            ////if (hours >= 67 && hours < 90)
            ////    Day = 3;
            ////if (hours >= 90 && hours < 114)
            ////    Day = 4;
            ////if (hours >= 114 && hours < 118)
            ////    Day = 5;
            ////if (hours >= 118)
            ////    Day = 6;

            #endregion


        }

        private void CalculateLabDay()
        {
            Day = 0;
            ProcudureDateTime = ((DateTime)ProcedureDate).Date.Add(((DateTime)ProcedureTime).TimeOfDay);
            CurrentObervationDateTime = ((DateTime)dtObservationDate.SelectedDate).Date.Add(((DateTime)dtObservationTime.Value).TimeOfDay);

            TimeSpan diff = CurrentObervationDateTime - ProcudureDateTime;
            hours = diff.TotalHours;

            if (hours <= 18)
            {
                blEmbError = true;
                cmbObservationDay.SelectedValue = (long)0;
            }

            if (hours > 18 && hours <= 24)
            {
                Day = 1;
                cmbObservationDay.SelectedValue = (long)1;
            }

            if (hours > 24 && hours <= 48)
            {
                Day = 2;
                cmbObservationDay.SelectedValue = (long)2;
            }

            if (hours > 48 && hours <= 72)
            {
                Day = 3;
                cmbObservationDay.SelectedValue = (long)3;
            }

            if (hours > 72 && hours <= 96)
            {
                Day = 4;
                cmbObservationDay.SelectedValue = (long)4;
            }

            if (hours > 96 && hours <= 120)
            {
                Day = 5;
                cmbObservationDay.SelectedValue = (long)5;
            }

            if (hours > 120 && hours <= 144)
            {
                Day = 6;
                cmbObservationDay.SelectedValue = (long)6;
            }

            if (hours > 144)
            {
                Day = 7;
                cmbObservationDay.SelectedValue = (long)7;
            }



            if (SelectedDay.Day1 == true && SelectedDay.IsLabDay1Freezed == true)
                blLabDay1 = true;
            if (SelectedDay.Day2 == true && SelectedDay.IsLabDay2Freezed == true)
                blLabDay2 = true;
            if (SelectedDay.Day3 == true && SelectedDay.IsLabDay3Freezed == true)
                blLabDay3 = true;
            if (SelectedDay.Day4 == true && SelectedDay.IsLabDay4Freezed == true)
                blLabDay4 = true;
            if (SelectedDay.Day5 == true && SelectedDay.IsLabDay5Freezed == true)
                blLabDay5 = true;
            if (SelectedDay.Day6 == true && SelectedDay.IsLabDay6Freezed == true)
                blLabDay6 = true;
            if (SelectedDay.Day7 == true && SelectedDay.IsLabDay7Freezed == true)
                blLabDay7 = true;



            if (Day == 1)
                FillDay1OocyteDate();
            else if (Day == 2)
                FillDay2OocyteDate();
            else if (Day == 3)
                FillDay3OocyteDate();
            else if (Day == 4)
                FillDay4OocyteDate();
            else if (Day == 5)
                FillDay5OocyteDate();
            else if (Day == 6)
                FillDay6OocyteDate();
            else if (Day == 7)
                FillDay7OocyteDate();


        }



        private void dtObservationDate_LostFocus(object sender, RoutedEventArgs e)
        {


            //if (hours < 90)
            //{
            //    cmbGrade.ItemsSource = null;
            //    Day = 2;
            //}
            //else if (hours < 114)
            //{
            //    cmbGrade.ItemsSource = null;
            //    Day = 4;
            //}
            //else
            //{
            //    cmbGrade.ItemsSource = null;
            //    Day = 5;
            //}
            if (IsIsthambul)
                CheckTimeAndReturnLabDay();
            else
                CalculateLabDay();
            ShowComboLabDay5();


        }

        private void ShowComboLabDay5()
        {
            if (Day == 0 || Day == 1)
            {
                //FillCleavageGrade();
                //cmbGrade.ItemsSource = null;
                //cmbGrade.ItemsSource = CleavageGrade2and3;
                //cmbGrade.SelectedItem = CleavageGrade2and3[0]; 
                lblGrade4.Visibility = Visibility.Collapsed;
                cmbGrade4.Visibility = Visibility.Collapsed;
                lblGrade2.Visibility = Visibility.Collapsed;
                cmbGrade2.Visibility = Visibility.Collapsed;
                lblGrade3.Visibility = Visibility.Collapsed;
                cmbGrade3.Visibility = Visibility.Collapsed;
                lblFragmentation.Visibility = Visibility.Collapsed;
                cmbFragmentation.Visibility = Visibility.Collapsed;
                lblEmbryocompacted.Visibility = Visibility.Collapsed;
                spEmbryoCompacted.Visibility = Visibility.Collapsed;
                lblStageofDevelopmentGrade.Visibility = Visibility.Collapsed;
                lblInnerCellMassGrade.Visibility = Visibility.Collapsed;
                lblTrophoectodermGrade.Visibility = Visibility.Collapsed;
                cmbStageofDevelopmentGrade.Visibility = Visibility.Collapsed;
                cmbInnerCellMassGrade.Visibility = Visibility.Collapsed;
                cmbTrophoectodermGrade.Visibility = Visibility.Collapsed;
                chkAssistedHatching.Visibility = Visibility.Collapsed;
                cmdPGD.Visibility = Visibility.Collapsed;
                GrpBiopsy.Visibility = Visibility.Collapsed;
            }
            else if (Day == 4)
            {
                //FillCleavageGrade();
                //cmbGrade.ItemsSource = null;
                //cmbGrade.ItemsSource = CleavageGrade4;
                //cmbGrade.SelectedItem = CleavageGrade4[0];
                lblGrade4.Visibility = Visibility.Visible;
                cmbGrade4.Visibility = Visibility.Visible;
                lblGrade2.Visibility = Visibility.Collapsed;
                cmbGrade2.Visibility = Visibility.Collapsed;
                lblGrade3.Visibility = Visibility.Collapsed;
                cmbGrade3.Visibility = Visibility.Collapsed;
                lblFragmentation.Visibility = Visibility.Collapsed;
                cmbFragmentation.Visibility = Visibility.Collapsed;
                lblEmbryocompacted.Visibility = Visibility.Collapsed;
                spEmbryoCompacted.Visibility = Visibility.Collapsed;
                lblStageofDevelopmentGrade.Visibility = Visibility.Collapsed;
                lblInnerCellMassGrade.Visibility = Visibility.Collapsed;
                lblTrophoectodermGrade.Visibility = Visibility.Collapsed;
                cmbStageofDevelopmentGrade.Visibility = Visibility.Collapsed;
                cmbInnerCellMassGrade.Visibility = Visibility.Collapsed;
                cmbTrophoectodermGrade.Visibility = Visibility.Collapsed;
                chkAssistedHatching.Visibility = Visibility.Visible;
                cmdPGD.Visibility = Visibility.Collapsed;
                GrpBiopsy.Visibility = Visibility.Collapsed;
            }
            else if (Day == 5 || Day == 6 || Day == 7)
            {
                //StageofDevelopmentGrade(); 
                lblGrade4.Visibility = Visibility.Collapsed;
                cmbGrade4.Visibility = Visibility.Collapsed;
                lblGrade2.Visibility = Visibility.Collapsed;
                cmbGrade2.Visibility = Visibility.Collapsed;
                lblGrade3.Visibility = Visibility.Collapsed;
                cmbGrade3.Visibility = Visibility.Collapsed;
                lblFragmentation.Visibility = Visibility.Collapsed;
                cmbFragmentation.Visibility = Visibility.Collapsed;
                lblEmbryocompacted.Visibility = Visibility.Collapsed;
                spEmbryoCompacted.Visibility = Visibility.Collapsed;
                lblStageofDevelopmentGrade.Visibility = Visibility.Visible;
                lblInnerCellMassGrade.Visibility = Visibility.Visible;
                lblTrophoectodermGrade.Visibility = Visibility.Visible;
                cmbStageofDevelopmentGrade.Visibility = Visibility.Visible;
                cmbInnerCellMassGrade.Visibility = Visibility.Visible;
                cmbTrophoectodermGrade.Visibility = Visibility.Visible;
                chkAssistedHatching.Visibility = Visibility.Visible;
                cmdPGD.Visibility = Visibility.Visible;
                GrpBiopsy.Visibility = Visibility.Visible;
                //if (Day5PGDPGS || Day6PGDPGS || Day7PGDPGS)
                cmdPGD.IsEnabled = false;
            }
            else if (Day == 3)
            {
                //FillCleavageGrade();
                //cmbGrade.ItemsSource = null;
                //cmbGrade.ItemsSource = CleavageGrade4;
                //cmbGrade.SelectedItem = CleavageGrade4[0];
                if (rdoYes.IsChecked == true)
                {
                    lblGrade3.Visibility = Visibility.Visible;
                    cmbGrade3.Visibility = Visibility.Visible;
                    lblGrade2.Visibility = Visibility.Collapsed;
                    cmbGrade2.Visibility = Visibility.Collapsed;
                    lblFragmentation.Visibility = Visibility.Collapsed;
                    cmbFragmentation.Visibility = Visibility.Collapsed;
                }

                if (rdoNo.IsChecked == true)
                {
                    lblGrade2.Visibility = Visibility.Visible;
                    cmbGrade2.Visibility = Visibility.Visible;
                    lblFragmentation.Visibility = Visibility.Visible;
                    cmbFragmentation.Visibility = Visibility.Visible;
                    lblGrade3.Visibility = Visibility.Collapsed;
                    cmbGrade3.Visibility = Visibility.Collapsed;
                }

                lblGrade4.Visibility = Visibility.Collapsed;
                cmbGrade4.Visibility = Visibility.Collapsed;
                if (rdoYes.IsChecked == false && rdoNo.IsChecked == false)
                {
                    lblGrade2.Visibility = Visibility.Collapsed;
                    cmbGrade2.Visibility = Visibility.Collapsed;
                    lblGrade3.Visibility = Visibility.Collapsed;
                    cmbGrade3.Visibility = Visibility.Collapsed;
                    lblFragmentation.Visibility = Visibility.Collapsed;
                    cmbFragmentation.Visibility = Visibility.Collapsed;
                }
                lblEmbryocompacted.Visibility = Visibility.Visible;
                spEmbryoCompacted.Visibility = Visibility.Visible;
                lblStageofDevelopmentGrade.Visibility = Visibility.Collapsed;
                lblInnerCellMassGrade.Visibility = Visibility.Collapsed;
                lblTrophoectodermGrade.Visibility = Visibility.Collapsed;
                cmbStageofDevelopmentGrade.Visibility = Visibility.Collapsed;
                cmbInnerCellMassGrade.Visibility = Visibility.Collapsed;
                cmbTrophoectodermGrade.Visibility = Visibility.Collapsed;
                chkAssistedHatching.Visibility = Visibility.Collapsed;
                cmdPGD.Visibility = Visibility.Visible;
                GrpBiopsy.Visibility = Visibility.Visible;
                //if (Day3PGDPGS)
                cmdPGD.IsEnabled = false;

            }
            else
            {
                //FillCleavageGrade();
                //cmbGrade.ItemsSource = null;
                //cmbGrade.ItemsSource = CleavageGrade2and3;
                //cmbGrade.SelectedItem = CleavageGrade2and3[0]; 
                lblGrade4.Visibility = Visibility.Collapsed;
                cmbGrade4.Visibility = Visibility.Collapsed;
                lblGrade3.Visibility = Visibility.Collapsed;
                cmbGrade3.Visibility = Visibility.Collapsed;
                lblGrade2.Visibility = Visibility.Visible;
                cmbGrade2.Visibility = Visibility.Visible;
                lblFragmentation.Visibility = Visibility.Visible;
                cmbFragmentation.Visibility = Visibility.Visible;
                lblEmbryocompacted.Visibility = Visibility.Collapsed;
                spEmbryoCompacted.Visibility = Visibility.Collapsed;
                lblStageofDevelopmentGrade.Visibility = Visibility.Collapsed;
                lblInnerCellMassGrade.Visibility = Visibility.Collapsed;
                lblTrophoectodermGrade.Visibility = Visibility.Collapsed;
                cmbStageofDevelopmentGrade.Visibility = Visibility.Collapsed;
                cmbInnerCellMassGrade.Visibility = Visibility.Collapsed;
                cmbTrophoectodermGrade.Visibility = Visibility.Collapsed;
                chkAssistedHatching.Visibility = Visibility.Collapsed;
                cmdPGD.Visibility = Visibility.Collapsed;
                GrpBiopsy.Visibility = Visibility.Collapsed;
            }

        }

        private void FillEmbryologist()
        {
            clsGetEmbryologistBizActionVO BizAction = new clsGetEmbryologistBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();
            BizAction.UnitId = 0;
            BizAction.DepartmentId = 0;
            //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestDynamics.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetEmbryologistBizActionVO)arg.Result).MasterList);

                    cmbEmbroyologist.ItemsSource = null;
                    cmbEmbroyologist.ItemsSource = objList;
                    cmbEmbroyologist.SelectedItem = objList[0];

                    cmbWitnessedEmbroyologist.ItemsSource = null;
                    cmbWitnessedEmbroyologist.ItemsSource = objList;
                    cmbWitnessedEmbroyologist.SelectedItem = objList[0];

                    FillCleavageGrade();

                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }


        public void FillCleavageGrade()
        {
            clsGetCleavageGradeMasterListBizActionVO BizAction = new clsGetCleavageGradeMasterListBizActionVO();
            BizAction.CleavageGrade = new clsCleavageGradeMasterVO();
            if (Day == 1)
                BizAction.CleavageGrade.ApplyTo = 2;
            else
                BizAction.CleavageGrade.ApplyTo = Day;

            //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc");
            //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null && ((clsGetCleavageGradeMasterListBizActionVO)args.Result).CleavageGradeList != null)
                {
                    List<MasterListItem> list = new List<MasterListItem>();
                    list.Add(new MasterListItem(0, "-- Select --"));
                    list.AddRange(((clsGetCleavageGradeMasterListBizActionVO)args.Result).CleavageGradeList);

                    List<MasterListItem> CleavageGrade2 = new List<MasterListItem>();
                    List<MasterListItem> CleavageGrade3 = new List<MasterListItem>();
                    List<MasterListItem> CleavageGrade4 = new List<MasterListItem>();
                    CleavageGrade2.Add(new MasterListItem(0, "-- Select --"));
                    CleavageGrade3.Add(new MasterListItem(0, "-- Select --"));
                    CleavageGrade4.Add(new MasterListItem(0, "-- Select --"));
                    CleavageGrade2.AddRange(list.Where(x => x.ApplyTo == 2).ToList());
                    CleavageGrade3.AddRange(list.Where(x => x.ApplyTo == 3).ToList());
                    CleavageGrade4.AddRange(list.Where(x => x.ApplyTo == 4).ToList());

                    cmbGrade2.ItemsSource = null;
                    cmbGrade2.ItemsSource = CleavageGrade2;
                    cmbGrade2.SelectedItem = CleavageGrade2[0];

                    cmbGrade3.ItemsSource = null;
                    cmbGrade3.ItemsSource = CleavageGrade3;
                    cmbGrade3.SelectedItem = CleavageGrade3[0];

                    cmbGrade4.ItemsSource = null;
                    cmbGrade4.ItemsSource = CleavageGrade4;
                    cmbGrade4.SelectedItem = CleavageGrade4[0];



                    //cmbGrade.ItemsSource = null;
                    //cmbGrade.ItemsSource = list;
                    //cmbGrade.SelectedItem = list[0];

                    //cmbGrade.ItemsSource = null;
                    //cmbGrade.ItemsSource = CleavageGrade2and3;
                    //cmbGrade.SelectedItem = CleavageGrade2and3[0];
                    ShowComboLabDay5();

                    if (IsSaved)
                    {
                        if (Day == 1)
                            fillDetailsLabDay1();
                        else if (Day == 2)
                            fillDetailsLabDay2();
                        else if (Day == 3)
                            fillDetailsLabDay3();
                        else if (Day == 4)
                            fillDetailsLabDay4();
                    }
                    StageofDevelopmentGrade();

                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        public void StageofDevelopmentGrade()
        {
            clsGetLab5_6MasterListBizActionVO BizAction = new clsGetLab5_6MasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_StageofDevelopmentGrade;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            BizAction.MasterList = new List<MasterListItem>();
            //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc");
            //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetLab5_6MasterListBizActionVO)args.Result).MasterList);
                    cmbStageofDevelopmentGrade.ItemsSource = null;
                    cmbStageofDevelopmentGrade.ItemsSource = objList;
                    cmbStageofDevelopmentGrade.SelectedItem = objList[0];
                    InnerCellMassGrade();
                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void InnerCellMassGrade()
        {
            clsGetLab5_6MasterListBizActionVO BizAction = new clsGetLab5_6MasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_InnerCellMassGrade;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            BizAction.MasterList = new List<MasterListItem>();
            //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc");
            //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetLab5_6MasterListBizActionVO)args.Result).MasterList);
                    cmbInnerCellMassGrade.ItemsSource = null;
                    cmbInnerCellMassGrade.ItemsSource = objList;
                    cmbInnerCellMassGrade.SelectedItem = objList[0];
                    TrophoectodermGrade();
                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void TrophoectodermGrade()
        {
            clsGetLab5_6MasterListBizActionVO BizAction = new clsGetLab5_6MasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_TrophoectodermGrade;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            BizAction.MasterList = new List<MasterListItem>();
            //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc");
            //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetLab5_6MasterListBizActionVO)args.Result).MasterList);
                    cmbTrophoectodermGrade.ItemsSource = null;
                    cmbTrophoectodermGrade.ItemsSource = objList;
                    cmbTrophoectodermGrade.SelectedItem = objList[0];
                    if (IsSaved)
                    {
                        if (Day == 5)
                            fillDetailsLabDay5();
                        if (Day == 6)
                            fillDetailsLabDay6();
                        if (Day == 7)
                            fillDetailsLabDay7();
                    }
                }

            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }


        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        byte[] MyPhoto { get; set; }
        private void CmdBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Multiselect = false;
            openDialog.Filter = "(*.jpg, *.png)|*.jpg;*.png;";
            openDialog.FilterIndex = 1;
            if (openDialog.ShowDialog() == true)
            {
                txtFN.Text = openDialog.File.Name;
                WriteableBitmap imageSource = new WriteableBitmap(200, 100);
                try
                {
                    //imageSource.SetSource(openDialog.File.OpenRead());
                    //ImageList.Add(imageSource);
                    //dgImgList.ItemsSource = ImageList;

                    using (Stream stream = openDialog.File.OpenRead())
                    {
                        MyPhoto = new byte[stream.Length];
                        stream.Read(MyPhoto, 0, (int)stream.Length);
                        ShowPhoto(MyPhoto, txtFN.Text);


                    }

                }
                catch (Exception ex)
                {
                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Error while reading file.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                }
            }
        }

        //ObservableObjectCollection mylistitem = new ObservableObjectCollection();

        List<ListItems> mylistitem = new List<ListItems>();
        List<ListItems> GetMylistitem = new List<ListItems>();
        private void ShowPhoto(byte[] MyPhoto, string ImgName)
        {
            BitmapImage img = new BitmapImage();
            img.SetSource(new MemoryStream(MyPhoto, false));

            ListItems obj = new ListItems();
            obj.Image1 = img;
            obj.Photo = MyPhoto;
            obj.ImagePath = ImgName;
            obj.IsDelete = true;

            var item1 = from r in mylistitem.ToList()
                        where r.ImagePath == obj.ImagePath
                        select r;

            if (item1.ToList().Count == 0)
            {
                mylistitem.Add(obj);
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                             new MessageBoxControl.MessageBoxChildWindow("Palash", "'" + obj.ImagePath + "'" + " File is already Added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }

            //mylistitem.Add(new ListItems { Image1 = img, Photo = MyPhoto, ImagePath = ImgName });

            dgImgList.ItemsSource = null;
            dgImgList.ItemsSource = mylistitem;

            txtFN.Text = mylistitem.Count.ToString();
            //clsAddImageVO ObjNew = new clsAddImageVO();
            //ObjNew.Photo = MyPhoto;
            //ObjNew.ImagePath = ImgName;
            //ObjNew.BitImagePath = img;

            //ImageList.Add(ObjNew);
            //dgImgList.ItemsSource = null;
            //dgImgList.ItemsSource = ImageList;
        }

        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to delete image", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);
            msgW1.Show();
        }

        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if (mylistitem.Count > GetMylistitem.Count)
                {
                    mylistitem.Remove((ListItems)dgImgList.SelectedItem);
                    dgImgList.ItemsSource = null;
                    dgImgList.ItemsSource = mylistitem;
                    txtFN.Text = mylistitem.Count.ToString();
                }
                else
                {
                    clsIVFDashboard_DeleteAndGetLabDayImagesBizActionVO BizAction = new clsIVFDashboard_DeleteAndGetLabDayImagesBizActionVO();
                    BizAction.ImageObj = new clsAddImageVO();
                    ListItems obj = (ListItems)dgImgList.SelectedItem;
                    BizAction.ImageObj.PatientID = obj.PatientID;
                    BizAction.ImageObj.PatientUnitID = obj.PatientUnitID;
                    BizAction.ImageObj.PlanTherapyID = obj.PlanTherapyID;
                    BizAction.ImageObj.PlanTherapyUnitID = obj.PlanTherapyUnitID;
                    BizAction.ImageObj.OocyteNumber = obj.OocyteNumber;
                    BizAction.ImageObj.SerialOocyteNumber = obj.SerialOocyteNumber;
                    BizAction.ImageObj.Day = obj.Day;
                    //BizAction.ImageObj.ServerImageName = obj.ServerImageName;
                    BizAction.ImageObj.OriginalImagePath = obj.OriginalImagePath;


                    //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc"); // this url will work both in dev and after deploy
                    //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {
                            if (((clsIVFDashboard_DeleteAndGetLabDayImagesBizActionVO)arg.Result).ImageList != null && ((clsIVFDashboard_DeleteAndGetLabDayImagesBizActionVO)arg.Result).ImageList.Count > 0)
                            {
                                ImageList = new List<clsAddImageVO>();
                                foreach (var item in ((clsIVFDashboard_DeleteAndGetLabDayImagesBizActionVO)arg.Result).ImageList)
                                {
                                    mylistitem = new List<ListItems>();
                                    mylistitem.Add(
                                        new ListItems
                                        {
                                            Image1 = new BitmapImage(new Uri(item.ServerImageName, UriKind.Relative)),
                                            ImagePath = item.ImagePath,
                                            ID = item.ID,
                                            SeqNo = item.SeqNo,
                                            Photo = item.Photo,
                                            Day = item.Day,
                                            UnitID = item.UnitID,
                                            SerialOocyteNumber = item.SerialOocyteNumber,
                                            OocyteNumber = item.OocyteNumber,
                                            PatientID = item.PatientID,
                                            PatientUnitID = item.PatientUnitID,
                                            PlanTherapyID = item.PlanTherapyID,
                                            PlanTherapyUnitID = item.PlanTherapyUnitID,
                                            OriginalImagePath = item.OriginalImagePath,
                                            IsDelete = true
                                        });

                                }
                                txtFN.Text = "";
                                dgImgList.ItemsSource = null;
                                dgImgList.ItemsSource = mylistitem;
                                txtFN.Text = mylistitem.Count.ToString();
                            }
                            else
                            {
                                mylistitem = new List<ListItems>();
                                dgImgList.ItemsSource = null;
                                dgImgList.ItemsSource = mylistitem;
                                txtFN.Text = mylistitem.Count.ToString();
                            }
                            ////MessageBoxControl.MessageBoxChildWindow msgW1 =
                            ////       new MessageBoxControl.MessageBoxChildWindow("", "Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            ////msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                            ////{
                            ////    if (res == MessageBoxResult.OK)
                            ////    {

                            ////    }
                            ////};
                            ////msgW1.Show();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                        }
                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
            }
        }


        private void hpyrlinkFileView_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((clsIVFDashboard_TherapyDocumentVO)dgDocumentGrid.SelectedItem).AttachedFileName))
            {
                if (((clsIVFDashboard_TherapyDocumentVO)dgDocumentGrid.SelectedItem).AttachedFileContent != null)
                {
                    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                    client.GlobalUploadFileCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            HtmlPage.Window.Invoke("GlobalOpenFile", new string[] { "UserUploadedFilesByTemplateTool/" + ((clsIVFDashboard_TherapyDocumentVO)dgDocumentGrid.SelectedItem).AttachedFileName });
                            AttachedFileNameList.Add(((clsIVFDashboard_TherapyDocumentVO)dgDocumentGrid.SelectedItem).AttachedFileName);
                        }
                    };
                    client.GlobalUploadFileAsync("../UserUploadedFilesByTemplateTool", ((clsIVFDashboard_TherapyDocumentVO)dgDocumentGrid.SelectedItem).AttachedFileName, ((clsIVFDashboard_TherapyDocumentVO)dgDocumentGrid.SelectedItem).AttachedFileContent);
                }
            }
        }

        private void cmdDeleteDoc_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxControl.MessageBoxChildWindow msgW2 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to delete Document", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            msgW2.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW2_OnMessageBoxClosed);
            msgW2.Show();
        }

        void msgW2_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if (DocList.Count > GetDocList.Count)
                {
                    DocList.Remove((clsIVFDashboard_TherapyDocumentVO)dgDocumentGrid.SelectedItem);
                    dgDocumentGrid.ItemsSource = null;
                    dgDocumentGrid.ItemsSource = DocList;
                    txtFileName.Text = DocList.Count.ToString();
                }
                else
                {
                    clsIVFDashboard_DeleteTherapyDocumentBizActionVO BizAction = new clsIVFDashboard_DeleteTherapyDocumentBizActionVO();
                    BizAction.Details = new clsIVFDashboard_TherapyDocumentVO();
                    BizAction.Details.ID = ((clsIVFDashboard_TherapyDocumentVO)dgDocumentGrid.SelectedItem).ID;
                    BizAction.Details.PatientID = ((clsIVFDashboard_TherapyDocumentVO)dgDocumentGrid.SelectedItem).PatientID;
                    BizAction.Details.PatientUnitID = ((clsIVFDashboard_TherapyDocumentVO)dgDocumentGrid.SelectedItem).PatientUnitID;
                    BizAction.Details.PlanTherapyID = ((clsIVFDashboard_TherapyDocumentVO)dgDocumentGrid.SelectedItem).PlanTherapyID;
                    BizAction.Details.PlanTherapyUnitID = ((clsIVFDashboard_TherapyDocumentVO)dgDocumentGrid.SelectedItem).PlanTherapyUnitID;
                    BizAction.Details.OocyteNumber = ((clsIVFDashboard_TherapyDocumentVO)dgDocumentGrid.SelectedItem).OocyteNumber;
                    BizAction.Details.SerialOocyteNumber = ((clsIVFDashboard_TherapyDocumentVO)dgDocumentGrid.SelectedItem).SerialOocyteNumber;
                    BizAction.Details.DocNo = ((clsIVFDashboard_TherapyDocumentVO)dgDocumentGrid.SelectedItem).DocNo;
                    BizAction.Details.Day = ((clsIVFDashboard_TherapyDocumentVO)dgDocumentGrid.SelectedItem).Day;

                    //Uri address = new Uri(Application.Current.Host.Source, "../PalashDynamics.svc"); // this url will work both in dev and after deploy
                    //PalashServiceClient client = new PalashServiceClient("CustomBinding_IPalashService", address.AbsoluteUri);

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {
                            if (((clsIVFDashboard_DeleteTherapyDocumentBizActionVO)arg.Result).DetailsList != null && ((clsIVFDashboard_DeleteTherapyDocumentBizActionVO)arg.Result).DetailsList.Count > 0)
                            {
                                foreach (var item in ((clsIVFDashboard_DeleteTherapyDocumentBizActionVO)arg.Result).DetailsList)
                                {
                                    item.IsDelete = true;
                                }

                                dgDocumentGrid.ItemsSource = null;
                                dgDocumentGrid.ItemsSource = ((clsIVFDashboard_DeleteTherapyDocumentBizActionVO)arg.Result).DetailsList;
                                txtFileName.Text = DocList.Count.ToString();
                            }
                            else
                            {
                                DocList = new List<clsIVFDashboard_TherapyDocumentVO>();
                                dgDocumentGrid.ItemsSource = null;
                                dgDocumentGrid.ItemsSource = DocList;
                                txtFileName.Text = DocList.Count.ToString();
                            }


                            ////MessageBoxControl.MessageBoxChildWindow msgW1 =
                            ////       new MessageBoxControl.MessageBoxChildWindow("", "Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            ////msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                            ////{
                            ////    if (res == MessageBoxResult.OK)
                            ////    {

                            ////    }
                            ////};
                            ////msgW1.Show();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                        }
                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
            }
            //throw new NotImplementedException();
        }

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        private void txtNoOfCell_TextChanged(object sender, TextChangedEventArgs e)
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
        }

        private void txtNoOfCell_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void chkAssistedHatching_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmbGrade2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            long FragmentationID = ((MasterListItem)cmbGrade2.SelectedItem).FragmentationID;
            cmbFragmentation.SelectedValue = FragmentationID;
        }

        private void rdoYes_Click(object sender, RoutedEventArgs e)
        {
            if (rdoYes.IsChecked == true)
            {
                lblGrade3.Visibility = Visibility.Visible;
                cmbGrade3.Visibility = Visibility.Visible;
                lblGrade2.Visibility = Visibility.Collapsed;
                cmbGrade2.Visibility = Visibility.Collapsed;
                lblFragmentation.Visibility = Visibility.Collapsed;
                cmbFragmentation.Visibility = Visibility.Collapsed;
            }

        }

        private void rdoNo_Click(object sender, RoutedEventArgs e)
        {
            if (rdoNo.IsChecked == true)
            {
                lblGrade2.Visibility = Visibility.Visible;
                cmbGrade2.Visibility = Visibility.Visible;
                lblFragmentation.Visibility = Visibility.Visible;
                cmbFragmentation.Visibility = Visibility.Visible;
                lblGrade3.Visibility = Visibility.Collapsed;
                cmbGrade3.Visibility = Visibility.Collapsed;
            }
        }

        private void cmdPGD_Click(object sender, RoutedEventArgs e)
        {
            frmPGD_New win = new frmPGD_New();
            win.FrozenPGDPGS = FrozenPGDPGS;
            win.Day = Day;
            win.LabDayID = ((clsIVFDashboard_LabDaysVO)this.DataContext).ID;
            win.LabDayUnitID = ((clsIVFDashboard_LabDaysVO)this.DataContext).UnitID;
            win.LabDayNo = Day;
            win.Title = "PGD/PGS - Embryo No. : " + OocyteNo;
            win.PatientID = CoupleDetails.FemalePatient.PatientID;
            win.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            win.PlanTherapyID = PlanTherapyId;
            win.PlanTherapyUnitID = PlanTherapyUnitId;
            win.OocyteNumber = OocyteNo;
            win.SerialOocyteNumber = SerialOocNo;
            win.Show();
        }

        private void hlbBiopsySave_Click(object sender, RoutedEventArgs e)
        {
            clsIVFDashboard_AddUpdateDay3BizActionVO BizAction = new clsIVFDashboard_AddUpdateDay3BizActionVO();
            BizAction.BiopsyFileAfterFreeze = true;
            BizAction.Day3Details = new clsIVFDashboard_LabDaysVO();

            //BizAction.Day3Details.ID = 0;
            //BizAction.Day3Details.PatientID = CoupleDetails.FemalePatient.PatientID;
            //BizAction.Day3Details.PatientUnitID = CoupleDetails.FemalePatient.UnitId;
            //BizAction.Day3Details.PlanTherapyID = PlanTherapyId;
            //BizAction.Day3Details.PlanTherapyUnitID = PlanTherapyUnitId;
            //BizAction.Day3Details.OocyteNumber = OocyteNo;
            //BizAction.Day3Details.SerialOocyteNumber = SerialOocNo;


            BizAction.Day3Details.DetailList = DocList;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.OK)
                        {
                            this.DialogResult = true;
                            if (OnSaveButton_Click != null)
                                OnSaveButton_Click(this, new RoutedEventArgs());
                        }
                    };
                    msgW1.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }



    }
}

