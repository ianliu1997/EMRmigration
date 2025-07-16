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
using PalashDynamics.ValueObjects.Administration.OTConfiguration;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using System.Windows.Data;

namespace PalashDynamics.OperationTheatre
{
    public partial class PatientWiseChecklist : ChildWindow
    {
        public bool ViewChecklist { get; set; }
        public long ScheduleID { get; set; }
        public event RoutedEventHandler OnSaveButton_Click;
        public List<long> procedureIDList = new List<long>();
        PagedCollectionView pcv = null;
        public PatientWiseChecklist()
        {
            InitializeComponent();
            this.Loaded +=new RoutedEventHandler(PatientWiseChecklist_Loaded);
        }

        void PatientWiseChecklist_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ViewChecklist == false)
                    FillChecklistGrid();
                else
                    FillChecklistGrid(ScheduleID);

            }
            catch (Exception ex)
            {
 
            }
        }
        /// <summary>
        /// Fills chechlist grid
        /// </summary>
        /// <param name="ScheduleID"></param>
        private void FillChecklistGrid(long ScheduleID)
        {
            try
            {
                clsGetCheckListByScheduleIDBizActionVO BizActionVo = new clsGetCheckListByScheduleIDBizActionVO();

                BizActionVo.ScheduleID = ScheduleID;
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    Client.ProcessCompleted += (s, e) =>
                    {
                        if (e.Error == null && e.Result != null)
                        {
                            if (((clsGetCheckListByScheduleIDBizActionVO)e.Result).ChecklistDetails != null)
                            {
                                clsGetCheckListByScheduleIDBizActionVO result = e.Result as clsGetCheckListByScheduleIDBizActionVO;


                                if (result.ChecklistDetails != null)
                                {
                                    myList.Clear();
                                    myList = result.ChecklistDetails;
                                    //foreach (var item in result.ChecklistDetails)
                                    //{
                                    //    if (!myList.Contains(item))
                                    //        myList.Add(item);
                                    //}
                                    pcv = new PagedCollectionView(myList);

                                    pcv.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
                                    pcv.GroupDescriptions.Add(new PropertyGroupDescription("SubCategory1"));

                                    dgCheckList.ItemsSource = null;
                                    dgCheckList.ItemsSource = pcv;
                                }

                            }
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

        List<clsPatientProcedureChecklistDetailsVO> myList = new List<clsPatientProcedureChecklistDetailsVO>();
        public List<clsPatientProcedureChecklistDetailsVO> resultCheckList = new List<clsPatientProcedureChecklistDetailsVO>();
        /// <summary>
        /// Fills Checklist
        /// </summary>
        private void FillChecklistGrid()
        {

            try
            {
                clsGetCheckListByProcedureIDBizActionVO BizActionVo = new clsGetCheckListByProcedureIDBizActionVO();
                for (int i = 0; i < procedureIDList.Count; i++)
                {
                    BizActionVo.ProcedureID = procedureIDList[i];
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    Client.ProcessCompleted += (s, e) =>
                    {
                        if (e.Error == null && e.Result != null)
                        {
                            if (((clsGetCheckListByProcedureIDBizActionVO)e.Result).ChecklistDetails != null)
                            {
                                clsGetCheckListByProcedureIDBizActionVO result = e.Result as clsGetCheckListByProcedureIDBizActionVO;


                                if (result.ChecklistDetails != null)
                                {
                                    //myList.Clear();
                                   // myList = result.ChecklistDetails;
                                    foreach (var item in result.ChecklistDetails)
                                    {
                                        if(!myList.Contains(item))
                                        myList.Add(item);
                                    }
                                    pcv = new PagedCollectionView(myList);

                                    pcv.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
                                    pcv.GroupDescriptions.Add(new PropertyGroupDescription("SubCategory1"));

                                    dgCheckList.ItemsSource = null;
                                    dgCheckList.ItemsSource = pcv;
                                }

                            }
                        }
                    };

                    Client.ProcessAsync(BizActionVo, ((IApplicationConfiguration)App.Current).CurrentUser);
                    Client.CloseAsync();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        /// <summary>
        /// Adds Checklist item
        /// </summary>
    
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtCheckList.Text))
                {


                    myList.Add(new clsPatientProcedureChecklistDetailsVO() { Category = txtCategory.Text, SubCategory1 = txtSubCategory.Text, Name = txtCheckList.Text });

                    pcv = new PagedCollectionView(myList);

                    pcv.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
                    pcv.GroupDescriptions.Add(new PropertyGroupDescription("SubCategory1"));

                    dgCheckList.ItemsSource = pcv;
                    txtCategory.Text = "";
                    txtSubCategory.Text = "";
                    txtCheckList.Text = "";

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void dgCheckList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                if (((clsPatientProcedureChecklistDetailsVO)dgCheckList.SelectedItem) != null)
                {
                    txtCategory.Text = ((clsPatientProcedureChecklistDetailsVO)dgCheckList.SelectedItem).Category;
                    txtSubCategory.Text = ((clsPatientProcedureChecklistDetailsVO)dgCheckList.SelectedItem).SubCategory1;
                    txtCheckList.Text = ((clsPatientProcedureChecklistDetailsVO)dgCheckList.SelectedItem).Name;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdDeleteCheckListItem_Click(object sender, RoutedEventArgs e)
        {
            myList.RemoveAt(dgCheckList.SelectedIndex);
            dgCheckList.ItemsSource = null;
            pcv = new PagedCollectionView(myList);
            pcv.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
            pcv.GroupDescriptions.Add(new PropertyGroupDescription("SubCategory1"));
            dgCheckList.ItemsSource = pcv;
            dgCheckList.UpdateLayout();
        }

        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var item in dgCheckList.ItemsSource)
                {
                    if (((clsPatientProcedureChecklistDetailsVO)item).Status == true)
                    {
                        resultCheckList.Add(((clsPatientProcedureChecklistDetailsVO)item));
                    }
                }
                if (OnSaveButton_Click != null)
                    OnSaveButton_Click(this, new RoutedEventArgs());
                this.DialogResult = false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

    }
}

