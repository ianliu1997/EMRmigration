using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Globalization;
using System.Windows.Data;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using System.Reflection;
using System.Collections.Generic;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using CIMS;
using PalashDynamics.IVF.TherpyExecution;
using PalashDynamics.ValueObjects.DashBoardVO;


namespace PalashDynamics.IVF
{
    #region  Converters
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value,
                           Type targetType,
                           object parameter,
                           CultureInfo culture)
        {
            bool IsVisible = (bool)value;
            if (IsVisible)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value,
                                  Type targetType,
                                  object parameter,
                                  CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    



    public class UploadButtonVisibilityConverter : IValueConverter
    {
        public object Convert(object value,
                           Type targetType,
                           object parameter,
                           CultureInfo culture)
        {
            if (parameter != null)
            {
                if (String.IsNullOrEmpty((string)value))
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            else
            {
                if (String.IsNullOrEmpty((string)value))
                    return Visibility.Collapsed;
                else
                    return Visibility.Visible;
            }
        }

        public object ConvertBack(object value,
                                  Type targetType,
                                  object parameter,
                                  CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TherapyToVisibilityConverter : IValueConverter
    {
        public object Convert(object value,
                           Type targetType,
                           object parameter,
                           CultureInfo culture)
        {
            bool IsVisible = false;
            if (value != null && value.ToString() != string.Empty)
            {
                IsVisible = true;
            }
            if (IsVisible)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value,
                                  Type targetType,
                                  object parameter,
                                  CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StringToBoolConverter : IValueConverter
    {
        public object Convert(object value,
                           Type targetType,
                           object parameter,
                           CultureInfo culture)
        {
            if (value != null && (value.ToString().ToLower() == "false" || value.ToString().ToLower() == "true"))
            {
                return bool.Parse(value.ToString());
            }
            else
            {
                return false;
            }
        }

        public object ConvertBack(object value,
                                  Type targetType,
                                  object parameter,
                                  CultureInfo culture)
        {
            return value.ToString();
        }
    }

    public class NumberToBrushConverter : FrameworkElement, IValueConverter
    {
        private string GetPropertyValue(string pName, clsTherapyExecutionVO control)
        {

            Type type = control.GetType();

            string propertyName = pName;

            BindingFlags flags = BindingFlags.GetProperty;

            Binder binder = null;

            object[] args = null;

            object value = type.InvokeMember(

            propertyName,

            flags,

            binder,

            control,

            args

            );
            if (value != null)
            {
                return value.ToString();
            }
            else
            {
                return null;
            }

        }



        public long Id
        {
            get { return (long)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Id.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Id", typeof(long), typeof(NumberToBrushConverter), null);



        public object Convert(object value,
                           Type targetType,
                           object parameter,
                           CultureInfo culture)
        {
            if (value is clsTherapyExecutionVO)
            {
                clsTherapyExecutionVO planTherapy = null;

                //foreach (var item in App.TherapyList)
                //{
                //    if (item.Id == ((Therapy)value).ForeignId)
                //    {
                //        planTherapy = item;
                //        break;
                //    }
                //}
                if (planTherapy == null)
                {
                    planTherapy = new clsTherapyExecutionVO();
                }

                if (planTherapy != null)
                {
                    SolidColorBrush brush = null;
                    if (((GetPropertyValue(parameter.ToString(), planTherapy) == null || (GetPropertyValue(parameter.ToString(), planTherapy) == "False" || (GetPropertyValue(parameter.ToString(), planTherapy) == "True")) ? "" : GetPropertyValue(parameter.ToString(), planTherapy))) != (GetPropertyValue(parameter.ToString(), (clsTherapyExecutionVO)value) == null ? "" : GetPropertyValue(parameter.ToString(), (clsTherapyExecutionVO)value)))
                    {
                        brush = new SolidColorBrush(Colors.Orange);
                    }

                    if (brush == null)
                    {
                        brush = new SolidColorBrush(Colors.White);
                    }

                    if (planTherapy.GroupId == 3 && (GetPropertyValue(parameter.ToString(), planTherapy) == "True"))
                    {
                        brush = new SolidColorBrush(Colors.Magenta);
                    }

                    return brush;
                }
                else
                {
                    return new SolidColorBrush(Colors.White);
                }
            }
            else
            {

                return new SolidColorBrush(Colors.White);
            }

        }

        public object ConvertBack(object value,
                                  Type targetType,
                                  object parameter,
                                  CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TherapyToTimeConverter : IValueConverter
    {
        public object Convert(object value,
                           Type targetType,
                           object parameter,
                           CultureInfo culture)
        {
            if (value != null)
            {
                string t = "";

                t = String.Format("{0:hh:mm tt}", new DateTime(((TimeSpan)value).Ticks));



                return t;
            }
            else
            {
                return "";
            }
        }

        public object ConvertBack(object value,
                                  Type targetType,
                                  object parameter,
                                  CultureInfo culture)
        {
            return value.ToString();
        }
    }

    public class ToggleBooleanValueConverter : IValueConverter
    {
        public object Convert(object value,
                           Type targetType,
                           object parameter,
                           CultureInfo culture)
        {
            if (value != null && (value.ToString().ToLower() == "false" || value.ToString().ToLower() == "true"))
            {
                return bool.Parse(value.ToString());
            }
            else
            {
                return false;
            }
        }

        public object ConvertBack(object value,
                                  Type targetType,
                                  object parameter,
                                  CultureInfo culture)
        {
            return value.ToString();
        }

        //public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    if (value != null)
        //    {
        //        bool val = (bool)value;

        //        return !val; // Math.Round(val, 2).ToString();
        //    }
        //    else
        //        return value;
        //}

        //public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    throw new NotImplementedException();
        //}
    }

    public class TherapyToDateTimeConverter : IValueConverter
    {
        public object Convert(object value,
                           Type targetType,
                           object parameter,
                           CultureInfo culture)
        {
            if (value != null)
            {
                DateTime? t = null;
                List<TherapyTransactionItem> TherapyTransactionList = null;//((clsTherapyExecutionVO)value).TherapyTransactionList;
                if (TherapyTransactionList != null)
                    foreach (var item in TherapyTransactionList)
                    {
                        if (item.DayNo == parameter.ToString())
                        {
                            t = item.Time;
                            break;
                        }
                    }
                return t;
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value,
                                  Type targetType,
                                  object parameter,
                                  CultureInfo culture)
        {
            return value.ToString();
        }
    }
    #endregion

    #region Therpy Execution Class
    public class ExeExtendedGrid : Grid
    {
        #region Variables
        public static Control TherapyFocusControl { get; set; }
        public static clsTherapyExecutionVO TherapyExeDetails { get; set; }
        public static string _kEY { get; set; }
        #endregion
        public bool Flag { get; set; }
        #region Constructor
        public ExeExtendedGrid()
            : base()
        {
            this.Loaded += new RoutedEventHandler(this.ChildItem_Loaded);
        }
        #endregion

        #region Loaded Event
        private void ChildItem_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in this.Children)
            {
                if (item is TextBox)
                {
                    ((TextBox)item).MouseEnter += new MouseEventHandler(ExtendedGrid_MouseEnter);
                    ((TextBox)item).MouseLeave += new MouseEventHandler(ExtendedGrid_MouseLeave);
                    ((TextBox)item).GotFocus += new RoutedEventHandler(ExtendedGrid_GotFocus);
                    ((TextBox)item).LostFocus += new RoutedEventHandler(ExtendedGrid_LostFocus);
                    ((TextBox)item).KeyDown += new KeyEventHandler(ExeExtendedGrid_KeyDown);
                    
                }
                if (item is CheckBox)
                {
                    ((CheckBox)item).Click += new RoutedEventHandler(ExtendedGrid_Click);
                }
            
                
            }
        }

        #endregion

        #region Set/Get Propert Value Methods

        private void SetPropertyValue(string pName, clsTherapyExecutionVO control, string value)
        {

            Type type = control.GetType();

            string propertyName = pName;
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            PropertyInfo prop = type.GetProperty(propertyName, flags);
            if (null != prop && prop.CanWrite)
            {
                prop.SetValue(control, value, null);
            }
        }

        private string GetPropertyValue(string pName, clsTherapyExecutionVO control)
        {
            Type type = control.GetType();

            string propertyName = pName;

            BindingFlags flags = BindingFlags.GetProperty;

            Binder binder = null;

            object[] args = null;

            object value = type.InvokeMember(

            propertyName,

            flags,

            binder,

            control,

            args

            );
            if (value != null)
            {
                return value.ToString();
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region TextBox Events

        void ExeExtendedGrid_KeyDown(object sender, KeyEventArgs e)
        {
            #region Drug Cell Deletion (I Handel this By Value to set Value as "Delete" and "DeleteAll" )

            if (e.Key == Key.Delete && e.PlatformKeyCode == 46)
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to Delete Drug Dosage for Specific Date?";
                MessageBoxControl.MessageBoxChildWindow msgWin =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += (result) =>
                {
                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                                 string kEY = ((TextBlock)((ExeExtendedGrid)((TextBox)(sender)).Parent).Children[0]).Text;//
                                string VALUE = "Delete"; //GetPropertyValue(kEY, (clsTherapyExecutionVO)((TextBox)(sender)).DataContext);
                  
                                clsAddPlanTherapyBizActionVO BizAction = new clsAddPlanTherapyBizActionVO();
                                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                                BizAction.TherapyDetails = new clsPlanTherapyVO();
                                BizAction.TherapyDetails.ID = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).PlanTherapyId;
                                BizAction.TherapyDetails.UnitID =((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).PlanTherapyUnitId;
                                BizAction.TherapyDetails.ThreapyExecutionId =((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).ID;
                                BizAction.TherapyDetails.TherapyExeTypeID = (int)TherapyGroup.Drug;
                                BizAction.TherapyDetails.TherapyStartDate = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).TherapyStartDate;
                                BizAction.TherapyDetails.PhysicianId = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).PhysicianID;
                                BizAction.TherapyDetails.TabID = (int)TherapyTabs.Execution;
                                BizAction.TherapyDetails.Day = kEY;
                                BizAction.TherapyDetails.Value = VALUE;
                                BizAction.TherapyDetails.ThearpyTypeDetailId =((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).ThearpyTypeDetailId;
                                #region Service Call (Check Validation)

                                client.ProcessCompleted += (s, args) =>
                                {
                                    if (args.Error == null && args.Result != null)
                                    {
                                        App.TherapyExecution.setupPage(((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).PlanTherapyId, (int)TherapyTabs.Execution);

                                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Drug Updated Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        msgW1.Show();
                        
                                    }
                                };
                                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                                client.CloseAsync();
                                #endregion
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                };
                msgWin.Show();
              
            }
            #endregion

            #region Drug Updation
            else
            {
                try
                {
                    string kEY = ((TextBlock)((ExeExtendedGrid)((TextBox)(sender)).Parent).Children[0]).Text;//
                    string VALUE = GetPropertyValue(kEY, (clsTherapyExecutionVO)((TextBox)(sender)).DataContext);
                    //SetPropertyValue(kEY,(clsTherapyExecutionVO)((TextBox)(sender)).DataContext,VALUE);
                    TherapyExeDetails = new clsTherapyExecutionVO();
                    TherapyExeDetails = (clsTherapyExecutionVO)((TextBox)(sender)).DataContext;
                    _kEY = ((TextBlock)((ExeExtendedGrid)((TextBox)(sender)).Parent).Children[0]).Text;
                    clsgetTherapyDrugDetailsBizActionVO BizAction = new clsgetTherapyDrugDetailsBizActionVO();
                    BizAction.TherapyExeID = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).ID;
                    BizAction.UnitID = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).PlanTherapyUnitId;
                    BizAction.DayNo = kEY;
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            AddDrug addDrug = new AddDrug();
                            addDrug.IsEdit = true;
                            addDrug.TherpayExeId = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).ID;
                            addDrug.TherapyStartDate = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).TherapyStartDate;
                            if (((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugDate == null)
                            {
                                string addDays = BizAction.DayNo.Substring(BizAction.DayNo.IndexOf("y") + 1);
                                
                                ((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugDate = addDrug.TherapyStartDate.Value.AddDays(Convert.ToInt64(addDays)-1);
                                ((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugDate = ((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugDate.Value.Add(System.DateTime.Now.TimeOfDay);
                                addDrug.dtthreapystartdate.SelectedDate = ((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugDate;
                            }
                            else
                            {
                                addDrug.dtthreapystartdate.SelectedDate = ((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugDate;
                            }
                            addDrug.txtForDays.Text = "1";
                            addDrug.txtDosage.Text = VALUE;
                            addDrug.txtTime.Value = ((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugDate;
                            if (!string.IsNullOrEmpty(((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugNotes))
                            {
                                addDrug.txtDrugNotes.Text = ((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugNotes;
                            }
                            addDrug.DrugId = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).ThearpyTypeDetailId; ;
                            addDrug.OnSaveButton_Click += new RoutedEventHandler(App.TherapyExecution.addDrug_OnSaveButton_Click);

                            addDrug.Show();
                        }
                    };

                    client.ProcessAsync(BizAction, new clsUserVO());
                    client.CloseAsync();
                }
                catch (Exception ex)
                {

                }

            }
            #endregion

        }
       

        void ExtendedGrid_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        void ExtendedGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            if (((TextBox)(sender)) != null && ((TextBox)(sender)).Text != null && ((TextBox)(sender)).Text != string.Empty)
            {
                ToolTip tp = (ToolTip)ToolTipService.GetToolTip((DependencyObject)(((TextBox)(sender))));
                if (tp.Content != null && tp.Content.ToString() != string.Empty)
                {
                    tp.Visibility = Visibility.Visible;
                }
                else
                {
                    tp.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                ToolTip tp = (ToolTip)ToolTipService.GetToolTip((DependencyObject)(((TextBox)(sender))));
                tp.Visibility = Visibility.Collapsed;
            }
        }

        string OrgTextBoxValue = "";

        void ExtendedGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            App.TherapyFocusControl = (Control)sender;
            App.kEY = ((TextBlock)((ExeExtendedGrid)((TextBox)(sender)).Parent).Children[0]).Text;
            OrgTextBoxValue = ((TextBox)(sender)).Text;
        }

        void ExtendedGrid_LostFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)(sender)).Text = OrgTextBoxValue;
        }

        int count = 0;
        #endregion

        #region CheckBox Checked and Unchecked

        void ExtendedGrid_Click(object sender, RoutedEventArgs e)
        {
            #region Code for Event (Date of LP)
            if (((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyTypeId == 1)
            {
                string kEY = ((TextBlock)((ExeExtendedGrid)((CheckBox)(sender)).Parent).Children[0]).Text;
                string VALUE = GetPropertyValue(kEY, (clsTherapyExecutionVO)((CheckBox)(sender)).DataContext);
                clsAddPlanTherapyBizActionVO BizAction = new clsAddPlanTherapyBizActionVO();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                BizAction.TherapyDetails = new clsPlanTherapyVO();
                BizAction.TherapyDetails.ID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyId;
                BizAction.TherapyDetails.UnitID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyUnitId;
                BizAction.TherapyDetails.ThreapyExecutionId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).ID;
                BizAction.TherapyDetails.TherapyExeTypeID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyTypeId;
                BizAction.TherapyDetails.TabID = (int)TherapyTabs.Execution;
                BizAction.TherapyDetails.TherapyStartDate = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyStartDate;
                BizAction.TherapyDetails.PhysicianId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PhysicianID;
                BizAction.TherapyDetails.Day = kEY;
                BizAction.TherapyDetails.Value = VALUE;
                BizAction.TherapyDetails.CoupleId = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleId;
                BizAction.TherapyDetails.CoupleUnitId = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleUnitId;
                #region Service Call (Check Validation)

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        App.TherapyExecution.setupPage(((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyId, (int)TherapyTabs.Execution);

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Date of LP Saved Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();    
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                
                #endregion
            }
            #endregion

            #region Code for Follicular Mointoring
            else if (((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyTypeId == 3)
            {
                
                string kEY = ((TextBlock)((ExeExtendedGrid)((CheckBox)(sender)).Parent).Children[0]).Text;
                string VALUE = GetPropertyValue(kEY, (clsTherapyExecutionVO)((CheckBox)(sender)).DataContext);
                
                //if (VALUE=="True")
                //{
                //    //if (Flag==false)
                //    //{
                //    //    //Flag = (((IVFTherapyExecution)sender).Flag);  
                //    //}
                    
                    clsAddPlanTherapyBizActionVO BizAction = new clsAddPlanTherapyBizActionVO();
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    BizAction.TherapyDetails = new clsPlanTherapyVO();
                    BizAction.TherapyDetails.ID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyId;
                    BizAction.TherapyDetails.UnitID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyUnitId;
                    BizAction.TherapyDetails.ThreapyExecutionId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).ID;
                    BizAction.TherapyDetails.TherapyExeTypeID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyTypeId;
                    BizAction.TherapyDetails.TherapyStartDate = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyStartDate;
                    BizAction.TherapyDetails.PhysicianId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PhysicianID;
                    BizAction.TherapyDetails.TabID = (int)TherapyTabs.Execution;
                    BizAction.TherapyDetails.Day = kEY;
                    BizAction.TherapyDetails.Value = VALUE;
                    BizAction.TherapyDetails.CoupleId = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleId;
                    BizAction.TherapyDetails.CoupleUnitId = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleUnitId;
                    #region Service Call (Check Validation)

                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            App.TherapyExecution.setupPage(((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyId, (int)TherapyTabs.Execution);

                            if (((clsAddPlanTherapyBizActionVO)args.Result).SuccessStatus == 10)
                            {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Follicular US Deleted Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            }
                            else if (((clsAddPlanTherapyBizActionVO)args.Result).SuccessStatus == 11)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "As follicular details already added for this day, Follicular US can't be deleted.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                            }
                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Follicular US Saved Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                            }

                            ((CheckBox)(sender)).IsEnabled = false;
                            //((CheckBox)(sender)).IsChecked = false;
                            //((CheckBox)(sender)).IsEnabled = false;
                            //((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).IsBool=false;

                        } 
                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                //}
                //else
                //{

                  
                   
                //    MessageBoxControl.MessageBoxChildWindow msgW1=
                //        new MessageBoxControl.MessageBoxChildWindow("Palash", "Follicular US Cannot be Edited", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //    msgW1.Show();
                   
                    
                //}
                
               
                #endregion


            }
            #endregion

            #region Code for OvumPickup
            else if (((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyTypeId == 4)
            {
                string kEY = ((TextBlock)((ExeExtendedGrid)((CheckBox)(sender)).Parent).Children[0]).Text;
                string VALUE = GetPropertyValue(kEY, (clsTherapyExecutionVO)((CheckBox)(sender)).DataContext);
                clsAddPlanTherapyBizActionVO BizAction = new clsAddPlanTherapyBizActionVO();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                BizAction.TherapyDetails = new clsPlanTherapyVO();
                BizAction.TherapyDetails.ID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyId;
                BizAction.TherapyDetails.UnitID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyUnitId;
                BizAction.TherapyDetails.ThreapyExecutionId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).ID;
                BizAction.TherapyDetails.TherapyExeTypeID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyTypeId;
                BizAction.TherapyDetails.TherapyStartDate = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyStartDate;
                BizAction.TherapyDetails.PhysicianId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PhysicianID;
                BizAction.TherapyDetails.TabID = (int)TherapyTabs.Execution;
                BizAction.TherapyDetails.Day = kEY;
                BizAction.TherapyDetails.Value = VALUE;
                BizAction.TherapyDetails.CoupleId = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleId;
                BizAction.TherapyDetails.CoupleUnitId = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleUnitId;
                #region Service Call (Check Validation)

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        App.TherapyExecution.setupPage(((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyId, (int)TherapyTabs.Execution);
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Ovum Pick UP Saved Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

                #endregion


            }
            #endregion

            #region Code for EmbryoTransfer
            else if (((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyTypeId == 5)
            {
                string kEY = ((TextBlock)((ExeExtendedGrid)((CheckBox)(sender)).Parent).Children[0]).Text;
                string VALUE = GetPropertyValue(kEY, (clsTherapyExecutionVO)((CheckBox)(sender)).DataContext);
                clsAddPlanTherapyBizActionVO BizAction = new clsAddPlanTherapyBizActionVO();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                BizAction.TherapyDetails = new clsPlanTherapyVO();
                BizAction.TherapyDetails.ID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyId;
                BizAction.TherapyDetails.UnitID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyUnitId;
                BizAction.TherapyDetails.ThreapyExecutionId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).ID;
                BizAction.TherapyDetails.TherapyExeTypeID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyTypeId;
                BizAction.TherapyDetails.TherapyStartDate = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyStartDate;
                BizAction.TherapyDetails.PhysicianId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PhysicianID;
                BizAction.TherapyDetails.TabID = (int)TherapyTabs.Execution;
                BizAction.TherapyDetails.Day = kEY;
                BizAction.TherapyDetails.Value = VALUE;
                BizAction.TherapyDetails.CoupleId = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleId;
                BizAction.TherapyDetails.CoupleUnitId = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleUnitId;
                #region Service Call (Check Validation)

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        App.TherapyExecution.setupPage(((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyId, (int)TherapyTabs.Execution);
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Embryo Transfer Saved Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

                #endregion


            }
            #endregion

            #region Commented
            //    count++;
            //    //App.MainWindow.textField.Text = count.ToString();
            //    clsUpdatePlanTherapyDetailItemBizActionVO UBizAction = new clsUpdatePlanTherapyDetailItemBizActionVO();
            //    //UBizAction.CenterId = App.SessionUser.FccId;
            //    UBizAction.CenterId = App.SessionUser.UserCenterId;
            //    UBizAction.CoupleId = App.SelectedCouple.CoupleId;
            //    UBizAction.PatientId = App.SelectedCouple.FemalePatient.Id;
            //    UBizAction.TherapyStartDate = App.TherapyStartDate.Value;
            //    UBizAction.IsExecutionCalander = true;
            //    UBizAction.IsUpdateTransaction = false;


            //    UBizAction.PlanTherapyDetailId = ((Therapy)((CheckBox)(sender)).DataContext).Id;
            //    UBizAction.PlanTherapyId = ((Therapy)((CheckBox)(sender)).DataContext).PlanTherapyId;

            //    UBizAction.PropertyNameAndValueList = new Dictionary<string, string>();
            //    string kEY = ((TextBlock)((ExeExtendedGrid)((CheckBox)(sender)).Parent).Children[0]).Text;
            //    string VALUE = GetPropertyValue(kEY, (Therapy)((CheckBox)(sender)).DataContext);
            //    UBizAction.PropertyNameAndValueList.Add(kEY, VALUE);
            //    List<clsAlertVO> AlertList = new List<clsAlertVO>();
            //    clsAlertVO Alert = new clsAlertVO();
            //    //            Alert.CenterId = App.SessionUser.FccId;
            //    Alert.CenterId = App.SessionUser.UserCenterId;

            //    Alert.CoupleId = App.SelectedCouple.CoupleId;
            //    Alert.PatientId = App.SelectedCouple.FemalePatient.Id;
            //    Alert.Date = App.TherapyStartDate.Value.AddDays((int.Parse(kEY.Substring(3, kEY.Length - 3)) - 1));
            //    Alert.Time = Alert.Date;
            //    AlertList.Add(Alert);
            //    if (((Therapy)((CheckBox)(sender)).DataContext).GroupId == 1 & ((Therapy)((CheckBox)(sender)).DataContext).HeadId == 3)
            //    {
            //        UBizAction.IsUpdateTransaction = true;
            //        Alert.EventTypeId = 2;

            //    }
            //    if (((Therapy)((CheckBox)(sender)).DataContext).GroupId == 2)
            //    {
            //        UBizAction.IsUpdateTransaction = true;
            //        Alert.EventTypeId = 1;
            //    }
            //    if (((Therapy)((CheckBox)(sender)).DataContext).GroupId == 3)
            //    {
            //        UBizAction.IsUpdateTransaction = true;
            //        Alert.EventTypeId = 3;
            //    }
            //    if (((Therapy)((CheckBox)(sender)).DataContext).GroupId == 4 & ((Therapy)((CheckBox)(sender)).DataContext).HeadId == 1)
            //    {
            //        UBizAction.IsUpdateTransaction = true;
            //        Alert.EventTypeId = 4;

            //    }
            //    MorpheusIVFWebClient Client = new MorpheusIVFWebClient();
            //    Client.ProcessCompleted += (se, earg) =>
            //    {
            //        if (UBizAction.IsUpdateTransaction == true)
            //        {
            //            //clsAddUpdateAlertBizActionVO UpdateAlertBizAction = new clsAddUpdateAlertBizActionVO();
            //            //UpdateAlertBizAction.CenterId = App.SessionUser.FccId;
            //            //UpdateAlertBizAction.CoupleId = App.SelectedCouple.CoupleId;
            //            //UpdateAlertBizAction.PatientId = App.SelectedCouple.FemalePatient.Id;
            //            //UpdateAlertBizAction.AlertList = AlertList;
            //            //Client = new MorpheusIVFWebClient();
            //            //Client.ProcessCompleted += (ase, aearg) =>
            //            //{

            //            //};
            //            //Client.ProcessAsync(UpdateAlertBizAction, App.SessionUser);
            //            //Client.CloseAsync();
            //        }

            //        if (((Therapy)((CheckBox)(sender)).DataContext).GroupId == 4 & ((Therapy)((CheckBox)(sender)).DataContext).HeadId == 1)
            //        {
            //            //bind grid when follicular ultra sound item enterd in grid from execution
            //            App.TherapyPlanner.BindPatientReportGrid();
            //        }

            //        if (((Therapy)((CheckBox)(sender)).DataContext).GroupId == 1 & (((Therapy)((CheckBox)(sender)).DataContext).HeadId <= 3))
            //        {
            //            for (int i = 1; i <= 60; i++)
            //            {
            //                if (kEY != "Day" + i.ToString())
            //                {
            //                    SetPropertyValue("Day" + i.ToString(), ((Therapy)((CheckBox)(sender)).DataContext), false.ToString());
            //                }
            //            }
            //            if (((Therapy)((CheckBox)(sender)).DataContext).HeadId == 3)
            //            {
            //                App.TherapyPlanner.SetBhcgDate(App.TherapyStartDate.Value.AddDays((int.Parse(kEY.Substring(3, kEY.Length - 3)) - 1)));
            //            }


            //            //App.TherapyPlanner.BindExecutionGrid();
            //        }
            //    };
            //    Client.ProcessAsync(UBizAction, App.SessionUser);
            //    Client.CloseAsync();
            //}
            #endregion
        }

        #endregion

        #region Coverters

        public class TestToBrushConverter : FrameworkElement, IValueConverter
        {
            public object Convert(object value,
                               Type targetType,
                               object parameter,
                               CultureInfo culture)
            {
                if (value != null)
                {
                    return new SolidColorBrush(Colors.Green);
                }
                else
                    return new SolidColorBrush(Colors.Black);
            }

            public object ConvertBack(object value,
                                      Type targetType,
                                      object parameter,
                                      CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class TestToButtonTitleConverter : FrameworkElement, IValueConverter
        {
            public object Convert(object value,
                                Type targetType,
                                object parameter,
                                CultureInfo culture)
            {
                if (value != null)
                {
                    return "View";
                }
                else
                    return "Enter";
            }

            public object ConvertBack(object value,
                                        Type targetType,
                                        object parameter,
                                        CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class TestToStatusConverter : FrameworkElement, IValueConverter
        {
            public object Convert(object value,
                                Type targetType,
                                object parameter,
                                CultureInfo culture)
            {
                if (value != null)
                {
                    return "Done";
                }
                else
                    return "Not Done";
            }

            public object ConvertBack(object value,
                                        Type targetType,
                                        object parameter,
                                        CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class BoolToYesNoConverter : FrameworkElement, IValueConverter
        {
            public object Convert(object value,
                                Type targetType,
                                object parameter,
                                CultureInfo culture)
            {
                try
                {
                    bool? val = (bool?)value;
                    if (val.HasValue)
                    {
                        if (val.Value == bool.Parse(parameter.ToString()))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return val;
                    }
                }
                catch (Exception ex)
                {

                    //throw;
                }
                return null;
            }

            public object ConvertBack(object value,
                                        Type targetType,
                                        object parameter,
                                        CultureInfo culture)
            {
                try
                {
                    bool? val = (bool?)value;
                    if (val.HasValue)
                    {
                        if (val.Value == bool.Parse(parameter.ToString()))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return val;
                    }
                }
                catch (Exception ex)
                {

                    //throw;
                }
                return null;
            }
        }

        public class EscapeStringToPlainConverter : FrameworkElement, IValueConverter
        {
            #region IValueConverter Members

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value != null)
                {
                    return value.ToString().Replace(Environment.NewLine, " ");
                }
                else
                {
                    return value;
                }
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        public class TestToElapsedTimeConverter : FrameworkElement, IValueConverter
        {
            public object Convert(object value,
                               Type targetType,
                               object parameter,
                               CultureInfo culture)
            {
                if (value != null)
                {
                    return DateTime.Today.Subtract((DateTime)value).Days.ToString() + " Days";
                }
                else
                    return null;
            }

            public object ConvertBack(object value,
                                      Type targetType,
                                      object parameter,
                                      CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class BackPathStackItem
        {

            public BackPathStackItem()
            {

            }

            public BackPathStackItem(string Header, string Path, double Width)
            {
                this.Header = Header;
                this.Path = Path;
                this.SideImageWidth = Width;
            }

            public string Header { get; set; }
            public string Path { get; set; }
            public double SideImageWidth { get; set; }
        }

        public class ToLimitCharacterConverter : FrameworkElement, IValueConverter
        {
            public object Convert(object value,
                                Type targetType,
                                object parameter,
                                CultureInfo culture)
            {
                if (value != null)
                {
                    String txtblk = (String)value;
                    if (txtblk.Length > 50)
                    {
                        char[] charArray = new char[50];
                        txtblk.CopyTo(0, charArray, 0, 50);
                        txtblk = charArray.ToString();

                        for (int i = 0; i < 50; i++)
                        {
                            if (charArray[i] == 13 && i < 49)
                            {
                                charArray[i] = charArray[i + 1];
                            }
                        }
                        txtblk = new String(charArray);
                        return txtblk;
                    }
                    else
                    {
                        return value;
                    }
                }
                else
                    return null;
            }

            public object ConvertBack(object value,
                                        Type targetType,
                                        object parameter,
                                        CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }

    #endregion

    #region Surrogate Therpy Execution Class
    public class ExtendedGridSurrogate : Grid
    {
        #region Variables
        public static Control TherapyFocusControl { get; set; }
        public static clsTherapyExecutionVO TherapyExeDetails { get; set; }
        public static string _kEY { get; set; }
        public bool IsSurrogateGrid { get; set; }

        #endregion
        public bool Flag { get; set; }

        #region Constructor
        public ExtendedGridSurrogate()
            : base()
        {

            this.Loaded += new RoutedEventHandler(this.ChildItemSurrogate_Loaded);
        }
        #endregion

        #region Loaded Event
        private void ChildItemSurrogate_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in this.Children)
            {
                if (item is TextBox)
                {
                    ((TextBox)item).MouseEnter += new MouseEventHandler(ExtendedGrid_MouseEnter);
                    ((TextBox)item).MouseLeave += new MouseEventHandler(ExtendedGrid_MouseLeave);
                    ((TextBox)item).GotFocus += new RoutedEventHandler(ExtendedGrid_GotFocus);
                    ((TextBox)item).LostFocus += new RoutedEventHandler(ExtendedGrid_LostFocus);
                    ((TextBox)item).KeyDown += new KeyEventHandler(ExtendedGridSurrogate_KeyDown);

                }
                if (item is CheckBox)
                {
                    ((CheckBox)item).Click += new RoutedEventHandler(ExtendedGridSurrogate_Click);
                }
            }
        }

        #endregion

        #region Set/Get Propert Value Methods

        private void SetPropertyValue(string pName, clsTherapyExecutionVO control, string value)
        {

            Type type = control.GetType();

            string propertyName = pName;
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            PropertyInfo prop = type.GetProperty(propertyName, flags);
            if (null != prop && prop.CanWrite)
            {
                prop.SetValue(control, value, null);
            }
        }

        private string GetPropertyValue(string pName, clsTherapyExecutionVO control)
        {
            Type type = control.GetType();

            string propertyName = pName;

            BindingFlags flags = BindingFlags.GetProperty;

            Binder binder = null;

            object[] args = null;

            object value = type.InvokeMember(

            propertyName,

            flags,

            binder,

            control,

            args

            );
            if (value != null)
            {
                return value.ToString();
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region TextBox Events

        void ExtendedGridSurrogate_KeyDown(object sender, KeyEventArgs e)
        {
            #region Drug Cell Deletion (I Handel this By Value to set Value as "Delete" and "DeleteAll" )

            if (e.Key == Key.Delete && e.PlatformKeyCode == 46)
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to Delete Drug Dosage for Specific Date?";
                MessageBoxControl.MessageBoxChildWindow msgWin =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += (result) =>
                {
                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            string kEY = ((TextBlock)((ExtendedGridSurrogate)((TextBox)(sender)).Parent).Children[0]).Text;//
                            string VALUE = "Delete"; //GetPropertyValue(kEY, (clsTherapyExecutionVO)((TextBox)(sender)).DataContext);

                            clsAddPlanTherapyBizActionVO BizAction = new clsAddPlanTherapyBizActionVO();
                            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                            BizAction.TherapyDetails = new clsPlanTherapyVO();
                            BizAction.TherapyDetails.ID = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).PlanTherapyId;
                            BizAction.TherapyDetails.UnitID = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).PlanTherapyUnitId;
                            BizAction.TherapyDetails.ThreapyExecutionId = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).ID;
                            BizAction.TherapyDetails.TherapyExeTypeID = (int)TherapyGroup.Drug;
                            BizAction.TherapyDetails.TherapyStartDate = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).TherapyStartDate;
                            BizAction.TherapyDetails.PhysicianId = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).PhysicianID;
                            BizAction.TherapyDetails.TabID = (int)TherapyTabs.Execution;
                            BizAction.TherapyDetails.Day = kEY;
                            BizAction.TherapyDetails.Value = VALUE;
                            BizAction.TherapyDetails.ThearpyTypeDetailId = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).ThearpyTypeDetailId;
                            #region Service Call (Check Validation)

                            client.ProcessCompleted += (s, args) =>
                            {
                                if (args.Error == null && args.Result != null)
                                {
                                    App.TherapyExecutionSurrogate.setupPage(((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).PlanTherapyId, (int)TherapyTabs.Execution);

                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Drug Updated Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.Show();

                                }
                            };
                            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                            client.CloseAsync();
                            #endregion
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                };
                msgWin.Show();

            }
            #endregion

            #region Drug Updation
            else
            {
                try
                {
                    string kEY = ((TextBlock)((ExtendedGridSurrogate)((TextBox)(sender)).Parent).Children[0]).Text;//
                    string VALUE = GetPropertyValue(kEY, (clsTherapyExecutionVO)((TextBox)(sender)).DataContext);
                    //SetPropertyValue(kEY,(clsTherapyExecutionVO)((TextBox)(sender)).DataContext,VALUE);
                    TherapyExeDetails = new clsTherapyExecutionVO();
                    TherapyExeDetails = (clsTherapyExecutionVO)((TextBox)(sender)).DataContext;
                    _kEY = ((TextBlock)((ExtendedGridSurrogate)((TextBox)(sender)).Parent).Children[0]).Text;
                    clsgetTherapyDrugDetailsBizActionVO BizAction = new clsgetTherapyDrugDetailsBizActionVO();
                    BizAction.TherapyExeID = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).ID;
                    BizAction.UnitID = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).PlanTherapyUnitId;
                    BizAction.DayNo = kEY;
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            AddDrug addDrug = new AddDrug();
                            addDrug.IsEdit = true;

                            addDrug.TherpayExeId = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).ID;
                            addDrug.TherapyStartDate = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).TherapyStartDate;
                            if (((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugDate == null)
                            {
                                string addDays = BizAction.DayNo.Substring(BizAction.DayNo.IndexOf("y") + 1);

                                ((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugDate = addDrug.TherapyStartDate.Value.AddDays(Convert.ToInt64(addDays) - 1);
                                ((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugDate = ((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugDate.Value.Add(System.DateTime.Now.TimeOfDay);
                                addDrug.dtthreapystartdate.SelectedDate = ((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugDate;
                            }
                            else
                            {
                                addDrug.dtthreapystartdate.SelectedDate = ((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugDate;
                            }
                            addDrug.txtForDays.Text = "1";
                            addDrug.txtDosage.Text = VALUE;
                            addDrug.txtTime.Value = ((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugDate;
                            if (!string.IsNullOrEmpty(((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugNotes))
                            {
                                addDrug.txtDrugNotes.Text = ((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugNotes;
                            }
                            addDrug.DrugId = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).ThearpyTypeDetailId; ;
                            addDrug.OnSaveButton_Click += new RoutedEventHandler(App.TherapyExecutionSurrogate.addDrug_OnSaveButton_Click);

                            addDrug.Show();
                        }
                    };

                    client.ProcessAsync(BizAction, new clsUserVO());
                    client.CloseAsync();
                }
                catch (Exception ex)
                {

                }

            }
            #endregion

        }


        void ExtendedGrid_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        void ExtendedGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            if (((TextBox)(sender)) != null && ((TextBox)(sender)).Text != null && ((TextBox)(sender)).Text != string.Empty)
            {
                ToolTip tp = (ToolTip)ToolTipService.GetToolTip((DependencyObject)(((TextBox)(sender))));
                if (tp.Content != null && tp.Content.ToString() != string.Empty)
                {
                    tp.Visibility = Visibility.Visible;
                }
                else
                {
                    tp.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                ToolTip tp = (ToolTip)ToolTipService.GetToolTip((DependencyObject)(((TextBox)(sender))));
                tp.Visibility = Visibility.Collapsed;
            }
        }

        string OrgTextBoxValue = "";

        void ExtendedGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            App.TherapyFocusControl = (Control)sender;
            App.kEY = ((TextBlock)((ExtendedGridSurrogate)((TextBox)(sender)).Parent).Children[0]).Text;
            OrgTextBoxValue = ((TextBox)(sender)).Text;
        }

        void ExtendedGrid_LostFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)(sender)).Text = OrgTextBoxValue;
        }

        int count = 0;
        #endregion

        #region CheckBox Checked and Unchecked

        void ExtendedGridSurrogate_Click(object sender, RoutedEventArgs e)
        {
            #region Code for Event (Date of LP)
            if (((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyTypeId == 1)
            {
                string kEY = ((TextBlock)((ExtendedGridSurrogate)((CheckBox)(sender)).Parent).Children[0]).Text;
                string VALUE = GetPropertyValue(kEY, (clsTherapyExecutionVO)((CheckBox)(sender)).DataContext);
                clsAddPlanTherapyBizActionVO BizAction = new clsAddPlanTherapyBizActionVO();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                BizAction.TherapyDetails = new clsPlanTherapyVO();
                BizAction.TherapyDetails.ID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyId;
                BizAction.TherapyDetails.UnitID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyUnitId;
                //BizAction.TherapyDetails.SurrogateExecutionId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).SurrogateExeID;
                BizAction.TherapyDetails.SurrogateExecutionId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).ID;
                BizAction.TherapyDetails.TherapyExeTypeID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyTypeId;
                BizAction.TherapyDetails.TabID = (int)TherapyTabs.Execution;
                BizAction.TherapyDetails.TherapyStartDate = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyStartDate;
                BizAction.TherapyDetails.PhysicianId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PhysicianID;
                BizAction.TherapyDetails.IsSurrogate = true;
                //BizAction.TherapyDetails.SurrogateID = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).SurrogateID; 
                BizAction.TherapyDetails.IsSurrogateCalendar = true;
                BizAction.TherapyDetails.Day = kEY;
                BizAction.TherapyDetails.Value = VALUE;
                BizAction.TherapyDetails.CoupleId = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleId;
                BizAction.TherapyDetails.CoupleUnitId = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleUnitId;
                #region Service Call (Check Validation)

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        App.TherapyExecutionSurrogate.setupPage(((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyId, (int)TherapyTabs.Execution);

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Date of LP For Surrogate Saved Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

                #endregion
            }
            #endregion

            #region Code for EmbryoTransfer
            else if (((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyTypeId == 5)
            {
                string kEY = ((TextBlock)((ExtendedGridSurrogate)((CheckBox)(sender)).Parent).Children[0]).Text;
                string VALUE = GetPropertyValue(kEY, (clsTherapyExecutionVO)((CheckBox)(sender)).DataContext);
                clsAddPlanTherapyBizActionVO BizAction = new clsAddPlanTherapyBizActionVO();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                BizAction.TherapyDetails = new clsPlanTherapyVO();
                BizAction.TherapyDetails.ID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyId;
                BizAction.TherapyDetails.UnitID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyUnitId;
                //BizAction.TherapyDetails.SurrogateExecutionId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).SurrogateExeID;
                BizAction.TherapyDetails.SurrogateExecutionId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).ID;
                BizAction.TherapyDetails.TherapyExeTypeID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyTypeId;
                BizAction.TherapyDetails.TherapyStartDate = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyStartDate;
                BizAction.TherapyDetails.PhysicianId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PhysicianID;
                BizAction.TherapyDetails.IsSurrogate = true;
                //BizAction.TherapyDetails.SurrogateID = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).SurrogateID; 
                BizAction.TherapyDetails.IsSurrogateCalendar = true;
                BizAction.TherapyDetails.TabID = (int)TherapyTabs.Execution;
                BizAction.TherapyDetails.Day = kEY;
                BizAction.TherapyDetails.Value = VALUE;
                BizAction.TherapyDetails.CoupleId = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleId;
                BizAction.TherapyDetails.CoupleUnitId = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleUnitId;
                #region Service Call (Check Validation)

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        App.TherapyExecutionSurrogate.setupPage(((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyId, (int)TherapyTabs.Execution);
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Embryo Transfer for Surrogate Saved Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

                #endregion


            }
            #endregion

            #region Commented
            //    count++;
            //    //App.MainWindow.textField.Text = count.ToString();
            //    clsUpdatePlanTherapyDetailItemBizActionVO UBizAction = new clsUpdatePlanTherapyDetailItemBizActionVO();
            //    //UBizAction.CenterId = App.SessionUser.FccId;
            //    UBizAction.CenterId = App.SessionUser.UserCenterId;
            //    UBizAction.CoupleId = App.SelectedCouple.CoupleId;
            //    UBizAction.PatientId = App.SelectedCouple.FemalePatient.Id;
            //    UBizAction.TherapyStartDate = App.TherapyStartDate.Value;
            //    UBizAction.IsExecutionCalander = true;
            //    UBizAction.IsUpdateTransaction = false;


            //    UBizAction.PlanTherapyDetailId = ((Therapy)((CheckBox)(sender)).DataContext).Id;
            //    UBizAction.PlanTherapyId = ((Therapy)((CheckBox)(sender)).DataContext).PlanTherapyId;

            //    UBizAction.PropertyNameAndValueList = new Dictionary<string, string>();
            //    string kEY = ((TextBlock)((ExeExtendedGrid)((CheckBox)(sender)).Parent).Children[0]).Text;
            //    string VALUE = GetPropertyValue(kEY, (Therapy)((CheckBox)(sender)).DataContext);
            //    UBizAction.PropertyNameAndValueList.Add(kEY, VALUE);
            //    List<clsAlertVO> AlertList = new List<clsAlertVO>();
            //    clsAlertVO Alert = new clsAlertVO();
            //    //            Alert.CenterId = App.SessionUser.FccId;
            //    Alert.CenterId = App.SessionUser.UserCenterId;

            //    Alert.CoupleId = App.SelectedCouple.CoupleId;
            //    Alert.PatientId = App.SelectedCouple.FemalePatient.Id;
            //    Alert.Date = App.TherapyStartDate.Value.AddDays((int.Parse(kEY.Substring(3, kEY.Length - 3)) - 1));
            //    Alert.Time = Alert.Date;
            //    AlertList.Add(Alert);
            //    if (((Therapy)((CheckBox)(sender)).DataContext).GroupId == 1 & ((Therapy)((CheckBox)(sender)).DataContext).HeadId == 3)
            //    {
            //        UBizAction.IsUpdateTransaction = true;
            //        Alert.EventTypeId = 2;

            //    }
            //    if (((Therapy)((CheckBox)(sender)).DataContext).GroupId == 2)
            //    {
            //        UBizAction.IsUpdateTransaction = true;
            //        Alert.EventTypeId = 1;
            //    }
            //    if (((Therapy)((CheckBox)(sender)).DataContext).GroupId == 3)
            //    {
            //        UBizAction.IsUpdateTransaction = true;
            //        Alert.EventTypeId = 3;
            //    }
            //    if (((Therapy)((CheckBox)(sender)).DataContext).GroupId == 4 & ((Therapy)((CheckBox)(sender)).DataContext).HeadId == 1)
            //    {
            //        UBizAction.IsUpdateTransaction = true;
            //        Alert.EventTypeId = 4;

            //    }
            //    MorpheusIVFWebClient Client = new MorpheusIVFWebClient();
            //    Client.ProcessCompleted += (se, earg) =>
            //    {
            //        if (UBizAction.IsUpdateTransaction == true)
            //        {
            //            //clsAddUpdateAlertBizActionVO UpdateAlertBizAction = new clsAddUpdateAlertBizActionVO();
            //            //UpdateAlertBizAction.CenterId = App.SessionUser.FccId;
            //            //UpdateAlertBizAction.CoupleId = App.SelectedCouple.CoupleId;
            //            //UpdateAlertBizAction.PatientId = App.SelectedCouple.FemalePatient.Id;
            //            //UpdateAlertBizAction.AlertList = AlertList;
            //            //Client = new MorpheusIVFWebClient();
            //            //Client.ProcessCompleted += (ase, aearg) =>
            //            //{

            //            //};
            //            //Client.ProcessAsync(UpdateAlertBizAction, App.SessionUser);
            //            //Client.CloseAsync();
            //        }

            //        if (((Therapy)((CheckBox)(sender)).DataContext).GroupId == 4 & ((Therapy)((CheckBox)(sender)).DataContext).HeadId == 1)
            //        {
            //            //bind grid when follicular ultra sound item enterd in grid from execution
            //            App.TherapyPlanner.BindPatientReportGrid();
            //        }

            //        if (((Therapy)((CheckBox)(sender)).DataContext).GroupId == 1 & (((Therapy)((CheckBox)(sender)).DataContext).HeadId <= 3))
            //        {
            //            for (int i = 1; i <= 60; i++)
            //            {
            //                if (kEY != "Day" + i.ToString())
            //                {
            //                    SetPropertyValue("Day" + i.ToString(), ((Therapy)((CheckBox)(sender)).DataContext), false.ToString());
            //                }
            //            }
            //            if (((Therapy)((CheckBox)(sender)).DataContext).HeadId == 3)
            //            {
            //                App.TherapyPlanner.SetBhcgDate(App.TherapyStartDate.Value.AddDays((int.Parse(kEY.Substring(3, kEY.Length - 3)) - 1)));
            //            }


            //            //App.TherapyPlanner.BindExecutionGrid();
            //        }
            //    };
            //    Client.ProcessAsync(UBizAction, App.SessionUser);
            //    Client.CloseAsync();
            //}
            #endregion
        }

        #endregion

        #region Coverters

        public class TestToBrushConverter : FrameworkElement, IValueConverter
        {
            public object Convert(object value,
                               Type targetType,
                               object parameter,
                               CultureInfo culture)
            {
                if (value != null)
                {
                    return new SolidColorBrush(Colors.Green);
                }
                else
                    return new SolidColorBrush(Colors.Black);
            }

            public object ConvertBack(object value,
                                      Type targetType,
                                      object parameter,
                                      CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class TestToButtonTitleConverter : FrameworkElement, IValueConverter
        {
            public object Convert(object value,
                                Type targetType,
                                object parameter,
                                CultureInfo culture)
            {
                if (value != null)
                {
                    return "View";
                }
                else
                    return "Enter";
            }

            public object ConvertBack(object value,
                                        Type targetType,
                                        object parameter,
                                        CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class TestToStatusConverter : FrameworkElement, IValueConverter
        {
            public object Convert(object value,
                                Type targetType,
                                object parameter,
                                CultureInfo culture)
            {
                if (value != null)
                {
                    return "Done";
                }
                else
                    return "Not Done";
            }

            public object ConvertBack(object value,
                                        Type targetType,
                                        object parameter,
                                        CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class BoolToYesNoConverter : FrameworkElement, IValueConverter
        {
            public object Convert(object value,
                                Type targetType,
                                object parameter,
                                CultureInfo culture)
            {
                try
                {
                    bool? val = (bool?)value;
                    if (val.HasValue)
                    {
                        if (val.Value == bool.Parse(parameter.ToString()))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return val;
                    }
                }
                catch (Exception ex)
                {

                    //throw;
                }
                return null;
            }

            public object ConvertBack(object value,
                                        Type targetType,
                                        object parameter,
                                        CultureInfo culture)
            {
                try
                {
                    bool? val = (bool?)value;
                    if (val.HasValue)
                    {
                        if (val.Value == bool.Parse(parameter.ToString()))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return val;
                    }
                }
                catch (Exception ex)
                {

                    //throw;
                }
                return null;
            }
        }

        public class EscapeStringToPlainConverter : FrameworkElement, IValueConverter
        {
            #region IValueConverter Members

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value != null)
                {
                    return value.ToString().Replace(Environment.NewLine, " ");
                }
                else
                {
                    return value;
                }
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        public class TestToElapsedTimeConverter : FrameworkElement, IValueConverter
        {
            public object Convert(object value,
                               Type targetType,
                               object parameter,
                               CultureInfo culture)
            {
                if (value != null)
                {
                    return DateTime.Today.Subtract((DateTime)value).Days.ToString() + " Days";
                }
                else
                    return null;
            }

            public object ConvertBack(object value,
                                      Type targetType,
                                      object parameter,
                                      CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class BackPathStackItem
        {

            public BackPathStackItem()
            {

            }

            public BackPathStackItem(string Header, string Path, double Width)
            {
                this.Header = Header;
                this.Path = Path;
                this.SideImageWidth = Width;
            }

            public string Header { get; set; }
            public string Path { get; set; }
            public double SideImageWidth { get; set; }
        }

        public class ToLimitCharacterConverter : FrameworkElement, IValueConverter
        {
            public object Convert(object value,
                                Type targetType,
                                object parameter,
                                CultureInfo culture)
            {
                if (value != null)
                {
                    String txtblk = (String)value;
                    if (txtblk.Length > 50)
                    {
                        char[] charArray = new char[50];
                        txtblk.CopyTo(0, charArray, 0, 50);
                        txtblk = charArray.ToString();

                        for (int i = 0; i < 50; i++)
                        {
                            if (charArray[i] == 13 && i < 49)
                            {
                                charArray[i] = charArray[i + 1];
                            }
                        }
                        txtblk = new String(charArray);
                        return txtblk;
                    }
                    else
                    {
                        return value;
                    }
                }
                else
                    return null;
            }

            public object ConvertBack(object value,
                                        Type targetType,
                                        object parameter,
                                        CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }



    

    #endregion
    //By Anjali..............
    public class ExeExtendedGridNew : Grid
    {
        #region Variables
        public static Control TherapyFocusControl { get; set; }
        public static clsTherapyExecutionVO TherapyExeDetails { get; set; }
        public static string _kEY { get; set; }
        #endregion
        public bool Flag { get; set; }
        #region Constructor
        public ExeExtendedGridNew()
            : base()
        {
            this.Loaded += new RoutedEventHandler(this.ChildItem_Loaded);
        }
        #endregion

        #region Loaded Event
        private void ChildItem_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in this.Children)
            {
                if (item is TextBox)
                {
                    ((TextBox)item).MouseEnter += new MouseEventHandler(ExtendedGrid_MouseEnter);
                    ((TextBox)item).MouseLeave += new MouseEventHandler(ExtendedGrid_MouseLeave);
                    ((TextBox)item).GotFocus += new RoutedEventHandler(ExtendedGrid_GotFocus);
                    ((TextBox)item).LostFocus += new RoutedEventHandler(ExtendedGrid_LostFocus);
                    ((TextBox)item).KeyDown += new KeyEventHandler(ExeExtendedGrid_KeyDown);

                }
                if (item is CheckBox)
                {
                  //  ((CheckBox)item).Click += new RoutedEventHandler(ExtendedGrid_Click);
                    ((CheckBox)item).IsEnabled = false;
                }


            }
        }

        #endregion

        #region Set/Get Propert Value Methods

        private void SetPropertyValue(string pName, clsTherapyExecutionVO control, string value)
        {

            Type type = control.GetType();

            string propertyName = pName;
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            PropertyInfo prop = type.GetProperty(propertyName, flags);
            if (null != prop && prop.CanWrite)
            {
                prop.SetValue(control, value, null);
            }
        }

        private string GetPropertyValue(string pName, clsTherapyExecutionVO control)
        {
            Type type = control.GetType();

            string propertyName = pName;

            BindingFlags flags = BindingFlags.GetProperty;

            Binder binder = null;

            object[] args = null;

            object value = type.InvokeMember(

            propertyName,

            flags,

            binder,

            control,

            args

            );
            if (value != null)
            {
                return value.ToString();
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region TextBox Events

        void ExeExtendedGrid_KeyDown(object sender, KeyEventArgs e)
        {
            TherapyExeDetails = new clsTherapyExecutionVO();
            TherapyExeDetails = (clsTherapyExecutionVO)((TextBox)(sender)).DataContext;
            if (TherapyExeDetails.Head != "E2" && TherapyExeDetails.Head != "Progesterone" && TherapyExeDetails.Head != "FSH" && TherapyExeDetails.Head != "LH" && TherapyExeDetails.Head != "Prolactin" && TherapyExeDetails.Head != "BHCG")
            {
                #region Drug Cell Deletion (I Handel this By Value to set Value as "Delete" and "DeleteAll" )

                if (e.Key == Key.Delete && e.PlatformKeyCode == 46)
                {
                    string msgTitle = "Palash";
                    string msgText = "Are you sure you want to Delete Drug Dosage for Specific Date?";
                    MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWin.OnMessageBoxClosed += (result) =>
                    {
                        if (result == MessageBoxResult.Yes)
                        {
                            try
                            {
                                string kEY = ((TextBlock)((ExeExtendedGridNew)((TextBox)(sender)).Parent).Children[0]).Text;//
                                string VALUE = "Delete"; //GetPropertyValue(kEY, (clsTherapyExecutionVO)((TextBox)(sender)).DataContext);

                                clsIVFDashboard_AddPlanTherapyBizActionVO BizAction = new clsIVFDashboard_AddPlanTherapyBizActionVO();
                                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                                BizAction.TherapyDetails = new clsPlanTherapyVO();
                                BizAction.TherapyDetails.ID = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).PlanTherapyId;
                                BizAction.TherapyDetails.UnitID = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).PlanTherapyUnitId;
                                BizAction.TherapyDetails.ThreapyExecutionId = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).ID;
                                BizAction.TherapyDetails.TherapyExeTypeID = (int)TherapyGroup.Drug;
                                BizAction.TherapyDetails.TherapyStartDate = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).TherapyStartDate;
                                BizAction.TherapyDetails.PhysicianId = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).PhysicianID;
                                BizAction.TherapyDetails.TabID = (int)TherapyTabs.Execution;
                                BizAction.TherapyDetails.Day = kEY;
                                BizAction.TherapyDetails.Value = VALUE;
                                BizAction.TherapyDetails.ThearpyTypeDetailId = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).ThearpyTypeDetailId;
                                #region Service Call (Check Validation)

                                client.ProcessCompleted += (s, args) =>
                                {
                                    if (args.Error == null && args.Result != null)
                                    {
                                        App.TherapyExecutionNew.setupPage(((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).PlanTherapyId, (int)TherapyTabs.Execution);

                                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Drug Updated Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        msgW1.Show();

                                    }
                                };
                                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                                client.CloseAsync();
                                #endregion
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    };
                    msgWin.Show();

                }
                #endregion

                #region Drug Updation
                else
                {
                    try
                    {
                        string kEY = ((TextBlock)((ExeExtendedGridNew)((TextBox)(sender)).Parent).Children[0]).Text;//
                        string VALUE = GetPropertyValue(kEY, (clsTherapyExecutionVO)((TextBox)(sender)).DataContext);
                        //SetPropertyValue(kEY,(clsTherapyExecutionVO)((TextBox)(sender)).DataContext,VALUE);
                        TherapyExeDetails = new clsTherapyExecutionVO();
                        TherapyExeDetails = (clsTherapyExecutionVO)((TextBox)(sender)).DataContext;

                        if (TherapyExeDetails.Head != "E2" && TherapyExeDetails.Head != "Progesterone" && TherapyExeDetails.Head != "FSH" && TherapyExeDetails.Head != "LH" && TherapyExeDetails.Head != "Prolactin" && TherapyExeDetails.Head != "BHCG")
                        {
                            _kEY = ((TextBlock)((ExeExtendedGridNew)((TextBox)(sender)).Parent).Children[0]).Text;
                            clsgetTherapyDrugDetailsBizActionVO BizAction = new clsgetTherapyDrugDetailsBizActionVO();
                            BizAction.TherapyExeID = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).ID;
                            BizAction.UnitID = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).PlanTherapyUnitId;
                            BizAction.DayNo = kEY;
                            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                            client.ProcessCompleted += (s, args) =>
                            {
                                if (args.Error == null && args.Result != null)
                                {
                                    AddDrug addDrug = new AddDrug();
                                    addDrug.IsEdit = true;
                                    addDrug.TherpayExeId = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).ID;
                                    addDrug.TherapyStartDate = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).TherapyStartDate;
                                    if (((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugDate == null)
                                    {
                                        string addDays = BizAction.DayNo.Substring(BizAction.DayNo.IndexOf("y") + 1);

                                        ((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugDate = addDrug.TherapyStartDate.Value.AddDays(Convert.ToInt64(addDays) - 1);
                                        ((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugDate = ((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugDate.Value.Add(System.DateTime.Now.TimeOfDay);
                                        addDrug.dtthreapystartdate.SelectedDate = ((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugDate;
                                    }
                                    else
                                    {
                                        addDrug.dtthreapystartdate.SelectedDate = ((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugDate;
                                    }
                                    addDrug.txtForDays.Text = "1";
                                    addDrug.txtDosage.Text = VALUE;
                                    addDrug.txtTime.Value = ((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugDate;
                                    if (!string.IsNullOrEmpty(((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugNotes))
                                    {
                                        addDrug.txtDrugNotes.Text = ((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugNotes;
                                    }
                                    addDrug.DrugId = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).ThearpyTypeDetailId; ;
                                    addDrug.OnSaveButton_Click += new RoutedEventHandler(App.TherapyExecutionNew.addDrug_OnSaveButton_Click);

                                    addDrug.Show();
                                }
                            };

                            client.ProcessAsync(BizAction, new clsUserVO());
                            client.CloseAsync();
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }
                #endregion
            }
        }


        void ExtendedGrid_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        void ExtendedGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            if (((TextBox)(sender)) != null && ((TextBox)(sender)).Text != null && ((TextBox)(sender)).Text != string.Empty)
            {
                ToolTip tp = (ToolTip)ToolTipService.GetToolTip((DependencyObject)(((TextBox)(sender))));
                if (tp.Content != null && tp.Content.ToString() != string.Empty)
                {
                    tp.Visibility = Visibility.Visible;
                }
                else
                {
                    tp.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                ToolTip tp = (ToolTip)ToolTipService.GetToolTip((DependencyObject)(((TextBox)(sender))));
                tp.Visibility = Visibility.Collapsed;
            }
        }

        string OrgTextBoxValue = "";

        void ExtendedGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            App.TherapyFocusControl = (Control)sender;
            App.kEY = ((TextBlock)((ExeExtendedGridNew)((TextBox)(sender)).Parent).Children[0]).Text;
            OrgTextBoxValue = ((TextBox)(sender)).Text;
        }

        void ExtendedGrid_LostFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)(sender)).Text = OrgTextBoxValue;
        }

        int count = 0;
        #endregion

        #region CheckBox Checked and Unchecked

        void ExtendedGrid_Click(object sender, RoutedEventArgs e)
        {
            #region Code for Event (Date of LP)
            if (((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyTypeId == 1)
            {
                string kEY = ((TextBlock)((ExeExtendedGridNew)((CheckBox)(sender)).Parent).Children[0]).Text;
                string VALUE = GetPropertyValue(kEY, (clsTherapyExecutionVO)((CheckBox)(sender)).DataContext);
                clsIVFDashboard_AddPlanTherapyBizActionVO BizAction = new clsIVFDashboard_AddPlanTherapyBizActionVO();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                BizAction.TherapyDetails = new clsPlanTherapyVO();
                BizAction.TherapyDetails.ID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyId;
                BizAction.TherapyDetails.UnitID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyUnitId;
                BizAction.TherapyDetails.ThreapyExecutionId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).ID;
                BizAction.TherapyDetails.TherapyExeTypeID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyTypeId;
                BizAction.TherapyDetails.TabID = (int)TherapyTabs.Execution;
                BizAction.TherapyDetails.TherapyStartDate = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyStartDate;
                BizAction.TherapyDetails.PhysicianId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PhysicianID;
                BizAction.TherapyDetails.Day = kEY;
                BizAction.TherapyDetails.Value = VALUE;
                //BizAction.TherapyDetails.CoupleId = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleId;
                //BizAction.TherapyDetails.CoupleUnitId = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleUnitId;
                BizAction.TherapyDetails.PatientId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PatientID;
                BizAction.TherapyDetails.PatientUintId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PatientUnitID;
                #region Service Call (Check Validation)

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        App.TherapyExecutionNew.setupPage(((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyId, (int)TherapyTabs.Execution);

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Date of LP Saved Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

                #endregion
            }
            #endregion

            #region Code for Follicular Mointoring
            else if (((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyTypeId == 3)
            {

                string kEY = ((TextBlock)((ExeExtendedGridNew)((CheckBox)(sender)).Parent).Children[0]).Text;
                string VALUE = GetPropertyValue(kEY, (clsTherapyExecutionVO)((CheckBox)(sender)).DataContext);

                //if (VALUE=="True")
                //{
                //    //if (Flag==false)
                //    //{
                //    //    //Flag = (((IVFTherapyExecution)sender).Flag);  
                //    //}

                clsIVFDashboard_AddPlanTherapyBizActionVO BizAction = new clsIVFDashboard_AddPlanTherapyBizActionVO();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                BizAction.TherapyDetails = new clsPlanTherapyVO();
                BizAction.TherapyDetails.ID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyId;
                BizAction.TherapyDetails.UnitID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyUnitId;
                BizAction.TherapyDetails.ThreapyExecutionId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).ID;
                BizAction.TherapyDetails.TherapyExeTypeID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyTypeId;
                BizAction.TherapyDetails.TherapyStartDate = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyStartDate;
                BizAction.TherapyDetails.PhysicianId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PhysicianID;
                BizAction.TherapyDetails.TabID = (int)TherapyTabs.Execution;
                BizAction.TherapyDetails.Day = kEY;
                BizAction.TherapyDetails.Value = VALUE;
                //BizAction.TherapyDetails.CoupleId = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleId;
                //BizAction.TherapyDetails.CoupleUnitId = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleUnitId;
                BizAction.TherapyDetails.PatientId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PatientID;
                BizAction.TherapyDetails.PatientUintId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PatientUnitID;
                #region Service Call (Check Validation)

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        App.TherapyExecutionNew.setupPage(((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyId, 0);

                        if (((clsIVFDashboard_AddPlanTherapyBizActionVO)args.Result).SuccessStatus == 10)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Follicular US Deleted Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                        else if (((clsIVFDashboard_AddPlanTherapyBizActionVO)args.Result).SuccessStatus == 11)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "As follicular details already added for this day, Follicular US can't be deleted.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Follicular US Saved Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }

                        ((CheckBox)(sender)).IsEnabled = false;
                        //((CheckBox)(sender)).IsChecked = false;
                        //((CheckBox)(sender)).IsEnabled = false;
                        //((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).IsBool=false;

                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                //}
                //else
                //{



                //    MessageBoxControl.MessageBoxChildWindow msgW1=
                //        new MessageBoxControl.MessageBoxChildWindow("Palash", "Follicular US Cannot be Edited", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //    msgW1.Show();


                //}


                #endregion


            }
            #endregion

            #region Code for OvumPickup
            else if (((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyTypeId == 4)
            {
                string kEY = ((TextBlock)((ExeExtendedGridNew)((CheckBox)(sender)).Parent).Children[0]).Text;
                string VALUE = GetPropertyValue(kEY, (clsTherapyExecutionVO)((CheckBox)(sender)).DataContext);
                clsIVFDashboard_AddPlanTherapyBizActionVO BizAction = new clsIVFDashboard_AddPlanTherapyBizActionVO();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                BizAction.TherapyDetails = new clsPlanTherapyVO();
                BizAction.TherapyDetails.ID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyId;
                BizAction.TherapyDetails.UnitID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyUnitId;
                BizAction.TherapyDetails.ThreapyExecutionId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).ID;
                BizAction.TherapyDetails.TherapyExeTypeID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyTypeId;
                BizAction.TherapyDetails.TherapyStartDate = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyStartDate;
                BizAction.TherapyDetails.PhysicianId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PhysicianID;
                BizAction.TherapyDetails.TabID = (int)TherapyTabs.Execution;
                BizAction.TherapyDetails.Day = kEY;
                BizAction.TherapyDetails.Value = VALUE;
                //BizAction.TherapyDetails.CoupleId = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleId;
                //BizAction.TherapyDetails.CoupleUnitId = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleUnitId;
                BizAction.TherapyDetails.PatientId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PatientID;
                BizAction.TherapyDetails.PatientUintId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PatientUnitID;
                #region Service Call (Check Validation)

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        App.TherapyExecutionNew.setupPage(((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyId, (int)TherapyTabs.Execution);
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Ovum Pick UP Saved Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

                #endregion


            }
            #endregion

            #region Code for EmbryoTransfer
            else if (((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyTypeId == 5)
            {
                string kEY = ((TextBlock)((ExeExtendedGridNew)((CheckBox)(sender)).Parent).Children[0]).Text;
                string VALUE = GetPropertyValue(kEY, (clsTherapyExecutionVO)((CheckBox)(sender)).DataContext);
                clsIVFDashboard_AddPlanTherapyBizActionVO BizAction = new clsIVFDashboard_AddPlanTherapyBizActionVO();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                BizAction.TherapyDetails = new clsPlanTherapyVO();
                BizAction.TherapyDetails.ID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyId;
                BizAction.TherapyDetails.UnitID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyUnitId;
                BizAction.TherapyDetails.ThreapyExecutionId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).ID;
                BizAction.TherapyDetails.TherapyExeTypeID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyTypeId;
                BizAction.TherapyDetails.TherapyStartDate = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyStartDate;
                BizAction.TherapyDetails.PhysicianId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PhysicianID;
                BizAction.TherapyDetails.TabID = (int)TherapyTabs.Execution;
                BizAction.TherapyDetails.Day = kEY;
                BizAction.TherapyDetails.Value = VALUE;
                //BizAction.TherapyDetails.CoupleId = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleId;
                //BizAction.TherapyDetails.CoupleUnitId = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleUnitId;
                BizAction.TherapyDetails.PatientId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PatientID;
                BizAction.TherapyDetails.PatientUintId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PatientUnitID;
                #region Service Call (Check Validation)

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        App.TherapyExecutionNew.setupPage(((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyId, (int)TherapyTabs.Execution);
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Embryo Transfer Saved Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

                #endregion


            }
            #endregion

    
        }

        #endregion

        #region Coverters

        public class TestToBrushConverter : FrameworkElement, IValueConverter
        {
            public object Convert(object value,
                               Type targetType,
                               object parameter,
                               CultureInfo culture)
            {
                if (value != null)
                {
                    return new SolidColorBrush(Colors.Green);
                }
                else
                    return new SolidColorBrush(Colors.Black);
            }

            public object ConvertBack(object value,
                                      Type targetType,
                                      object parameter,
                                      CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class TestToButtonTitleConverter : FrameworkElement, IValueConverter
        {
            public object Convert(object value,
                                Type targetType,
                                object parameter,
                                CultureInfo culture)
            {
                if (value != null)
                {
                    return "View";
                }
                else
                    return "Enter";
            }

            public object ConvertBack(object value,
                                        Type targetType,
                                        object parameter,
                                        CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class TestToStatusConverter : FrameworkElement, IValueConverter
        {
            public object Convert(object value,
                                Type targetType,
                                object parameter,
                                CultureInfo culture)
            {
                if (value != null)
                {
                    return "Done";
                }
                else
                    return "Not Done";
            }

            public object ConvertBack(object value,
                                        Type targetType,
                                        object parameter,
                                        CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class BoolToYesNoConverter : FrameworkElement, IValueConverter
        {
            public object Convert(object value,
                                Type targetType,
                                object parameter,
                                CultureInfo culture)
            {
                try
                {
                    bool? val = (bool?)value;
                    if (val.HasValue)
                    {
                        if (val.Value == bool.Parse(parameter.ToString()))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return val;
                    }
                }
                catch (Exception ex)
                {

                    //throw;
                }
                return null;
            }

            public object ConvertBack(object value,
                                        Type targetType,
                                        object parameter,
                                        CultureInfo culture)
            {
                try
                {
                    bool? val = (bool?)value;
                    if (val.HasValue)
                    {
                        if (val.Value == bool.Parse(parameter.ToString()))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return val;
                    }
                }
                catch (Exception ex)
                {

                    //throw;
                }
                return null;
            }
        }

        public class EscapeStringToPlainConverter : FrameworkElement, IValueConverter
        {
            #region IValueConverter Members

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value != null)
                {
                    return value.ToString().Replace(Environment.NewLine, " ");
                }
                else
                {
                    return value;
                }
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        public class TestToElapsedTimeConverter : FrameworkElement, IValueConverter
        {
            public object Convert(object value,
                               Type targetType,
                               object parameter,
                               CultureInfo culture)
            {
                if (value != null)
                {
                    return DateTime.Today.Subtract((DateTime)value).Days.ToString() + " Days";
                }
                else
                    return null;
            }

            public object ConvertBack(object value,
                                      Type targetType,
                                      object parameter,
                                      CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class BackPathStackItem
        {

            public BackPathStackItem()
            {

            }

            public BackPathStackItem(string Header, string Path, double Width)
            {
                this.Header = Header;
                this.Path = Path;
                this.SideImageWidth = Width;
            }

            public string Header { get; set; }
            public string Path { get; set; }
            public double SideImageWidth { get; set; }
        }

        public class ToLimitCharacterConverter : FrameworkElement, IValueConverter
        {
            public object Convert(object value,
                                Type targetType,
                                object parameter,
                                CultureInfo culture)
            {
                if (value != null)
                {
                    String txtblk = (String)value;
                    if (txtblk.Length > 50)
                    {
                        char[] charArray = new char[50];
                        txtblk.CopyTo(0, charArray, 0, 50);
                        txtblk = charArray.ToString();

                        for (int i = 0; i < 50; i++)
                        {
                            if (charArray[i] == 13 && i < 49)
                            {
                                charArray[i] = charArray[i + 1];
                            }
                        }
                        txtblk = new String(charArray);
                        return txtblk;
                    }
                    else
                    {
                        return value;
                    }
                }
                else
                    return null;
            }

            public object ConvertBack(object value,
                                        Type targetType,
                                        object parameter,
                                        CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }

    //For Patient not for Surrogate
    public class ExeExtendedGridForSurrogacyPatient : Grid
    {
        #region Variables
        public static Control TherapyFocusControl { get; set; }
        public static clsTherapyExecutionVO TherapyExeDetails { get; set; }
        public static string _kEY { get; set; }
        #endregion
        public bool Flag { get; set; }
        #region Constructor
        public ExeExtendedGridForSurrogacyPatient()
            : base()
        {
            this.Loaded += new RoutedEventHandler(this.ChildItem_Loaded);
        }
        #endregion

        #region Loaded Event
        private void ChildItem_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in this.Children)
            {
                if (item is TextBox)
                {
                    ((TextBox)item).MouseEnter += new MouseEventHandler(ExtendedGrid_MouseEnter);
                    ((TextBox)item).MouseLeave += new MouseEventHandler(ExtendedGrid_MouseLeave);
                    ((TextBox)item).GotFocus += new RoutedEventHandler(ExtendedGrid_GotFocus);
                    ((TextBox)item).LostFocus += new RoutedEventHandler(ExtendedGrid_LostFocus);
                    ((TextBox)item).KeyDown += new KeyEventHandler(ExeExtendedGrid_KeyDown);

                }
                if (item is CheckBox)
                {
                    //((CheckBox)item).Click += new RoutedEventHandler(ExtendedGrid_Click);
                    ((CheckBox)item).IsEnabled = false;
                }

                // ((CheckBox)item).IsEnabled = false;
            }
        }

        #endregion

        #region Set/Get Propert Value Methods

        private void SetPropertyValue(string pName, clsTherapyExecutionVO control, string value)
        {

            Type type = control.GetType();

            string propertyName = pName;
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            PropertyInfo prop = type.GetProperty(propertyName, flags);
            if (null != prop && prop.CanWrite)
            {
                prop.SetValue(control, value, null);
            }
        }

        private string GetPropertyValue(string pName, clsTherapyExecutionVO control)
        {
            Type type = control.GetType();

            string propertyName = pName;

            BindingFlags flags = BindingFlags.GetProperty;

            Binder binder = null;

            object[] args = null;

            object value = type.InvokeMember(

            propertyName,

            flags,

            binder,

            control,

            args

            );
            if (value != null)
            {
                return value.ToString();
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region TextBox Events

        void ExeExtendedGrid_KeyDown(object sender, KeyEventArgs e)
        {
            #region Drug Cell Deletion (I Handel this By Value to set Value as "Delete" and "DeleteAll" )

            if (e.Key == Key.Delete && e.PlatformKeyCode == 46)
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to Delete Drug Dosage for Specific Date?";
                MessageBoxControl.MessageBoxChildWindow msgWin =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += (result) =>
                {
                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            string kEY = ((TextBlock)((ExeExtendedGridForSurrogacyPatient)((TextBox)(sender)).Parent).Children[0]).Text;//
                            string VALUE = "Delete"; //GetPropertyValue(kEY, (clsTherapyExecutionVO)((TextBox)(sender)).DataContext);

                            clsIVFDashboard_AddPlanTherapyBizActionVO BizAction = new clsIVFDashboard_AddPlanTherapyBizActionVO();
                            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                            BizAction.TherapyDetails = new clsPlanTherapyVO();
                            BizAction.TherapyDetails.ID = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).PlanTherapyId;
                            BizAction.TherapyDetails.UnitID = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).PlanTherapyUnitId;
                            BizAction.TherapyDetails.ThreapyExecutionId = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).ID;
                            BizAction.TherapyDetails.TherapyExeTypeID = (int)TherapyGroup.Drug;
                            BizAction.TherapyDetails.TherapyStartDate = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).TherapyStartDate;
                            BizAction.TherapyDetails.PhysicianId = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).PhysicianID;
                            BizAction.TherapyDetails.TabID = (int)TherapyTabs.Execution;
                            BizAction.TherapyDetails.IsSurrogateDrug = false;
                            BizAction.TherapyDetails.IsSurrogateCalendar = false;
                            BizAction.TherapyDetails.Day = kEY;
                            BizAction.TherapyDetails.Value = VALUE;
                            BizAction.TherapyDetails.ThearpyTypeDetailId = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).ThearpyTypeDetailId;
                            #region Service Call (Check Validation)

                            client.ProcessCompleted += (s, args) =>
                            {
                                if (args.Error == null && args.Result != null)
                                {
                                    App.TherapyExecutionForSurrogacyPatient.setupPage(((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).PlanTherapyId, (int)TherapyTabs.Execution);

                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Drug Updated Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.Show();

                                }
                            };
                            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                            client.CloseAsync();
                            #endregion
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                };
                msgWin.Show();

            }
            #endregion

            #region Drug Updation
            else
            {
                try
                {
                    string kEY = ((TextBlock)((ExeExtendedGridForSurrogacyPatient)((TextBox)(sender)).Parent).Children[0]).Text;//
                    string VALUE = GetPropertyValue(kEY, (clsTherapyExecutionVO)((TextBox)(sender)).DataContext);
                    //SetPropertyValue(kEY,(clsTherapyExecutionVO)((TextBox)(sender)).DataContext,VALUE);
                    TherapyExeDetails = new clsTherapyExecutionVO();
                    TherapyExeDetails = (clsTherapyExecutionVO)((TextBox)(sender)).DataContext;
                    _kEY = ((TextBlock)((ExeExtendedGridForSurrogacyPatient)((TextBox)(sender)).Parent).Children[0]).Text;
                    clsgetTherapyDrugDetailsBizActionVO BizAction = new clsgetTherapyDrugDetailsBizActionVO();
                    BizAction.TherapyExeID = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).ID;
                    BizAction.UnitID = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).PlanTherapyUnitId;
                    BizAction.DayNo = kEY;
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            AddDrug addDrug = new AddDrug();
                            addDrug.IsEdit = true;
                            addDrug.IsSurrogateCalendar = false;
                            addDrug.IsSurrogateDrug = false;
                            addDrug.TherpayExeId = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).ID;
                            addDrug.TherapyStartDate = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).TherapyStartDate;
                            if (((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugDate == null)
                            {
                                string addDays = BizAction.DayNo.Substring(BizAction.DayNo.IndexOf("y") + 1);

                                ((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugDate = addDrug.TherapyStartDate.Value.AddDays(Convert.ToInt64(addDays) - 1);
                                ((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugDate = ((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugDate.Value.Add(System.DateTime.Now.TimeOfDay);
                                addDrug.dtthreapystartdate.SelectedDate = ((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugDate;
                            }
                            else
                            {
                                addDrug.dtthreapystartdate.SelectedDate = ((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugDate;
                            }
                            addDrug.txtForDays.Text = "1";
                            addDrug.txtDosage.Text = VALUE;
                            addDrug.txtTime.Value = ((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugDate;
                            if (!string.IsNullOrEmpty(((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugNotes))
                            {
                                addDrug.txtDrugNotes.Text = ((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugNotes;
                            }
                            addDrug.DrugId = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).ThearpyTypeDetailId; ;
                            addDrug.OnSaveButton_Click += new RoutedEventHandler(App.TherapyExecutionForSurrogacyPatient.addDrug_OnSaveButton_Click);

                            addDrug.Show();
                        }
                    };

                    client.ProcessAsync(BizAction, new clsUserVO());
                    client.CloseAsync();
                }
                catch (Exception ex)
                {

                }

            }
            #endregion

        }


        void ExtendedGrid_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        void ExtendedGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            if (((TextBox)(sender)) != null && ((TextBox)(sender)).Text != null && ((TextBox)(sender)).Text != string.Empty)
            {
                ToolTip tp = (ToolTip)ToolTipService.GetToolTip((DependencyObject)(((TextBox)(sender))));
                if (tp.Content != null && tp.Content.ToString() != string.Empty)
                {
                    tp.Visibility = Visibility.Visible;
                }
                else
                {
                    tp.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                ToolTip tp = (ToolTip)ToolTipService.GetToolTip((DependencyObject)(((TextBox)(sender))));
                tp.Visibility = Visibility.Collapsed;
            }
        }

        string OrgTextBoxValue = "";

        void ExtendedGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            App.TherapyFocusControl = (Control)sender;
            App.kEY = ((TextBlock)((ExeExtendedGridForSurrogacyPatient)((TextBox)(sender)).Parent).Children[0]).Text;
            OrgTextBoxValue = ((TextBox)(sender)).Text;
        }

        void ExtendedGrid_LostFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)(sender)).Text = OrgTextBoxValue;
        }

        int count = 0;
        #endregion

        #region CheckBox Checked and Unchecked

        void ExtendedGrid_Click(object sender, RoutedEventArgs e)
        {
            #region Code for Event (Date of LP)
            if (((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyTypeId == 1)
            {
                string kEY = ((TextBlock)((ExeExtendedGridForSurrogacyPatient)((CheckBox)(sender)).Parent).Children[0]).Text;
                string VALUE = GetPropertyValue(kEY, (clsTherapyExecutionVO)((CheckBox)(sender)).DataContext);
                clsIVFDashboard_AddPlanTherapyBizActionVO BizAction = new clsIVFDashboard_AddPlanTherapyBizActionVO();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                BizAction.TherapyDetails = new clsPlanTherapyVO();
                BizAction.TherapyDetails.ID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyId;
                BizAction.TherapyDetails.UnitID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyUnitId;
                BizAction.TherapyDetails.ThreapyExecutionId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).ID;
                BizAction.TherapyDetails.TherapyExeTypeID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyTypeId;
                BizAction.TherapyDetails.TabID = (int)TherapyTabs.Execution;
                BizAction.TherapyDetails.TherapyStartDate = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyStartDate;
                BizAction.TherapyDetails.PhysicianId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PhysicianID;
                BizAction.TherapyDetails.Day = kEY;
                BizAction.TherapyDetails.Value = VALUE;
                BizAction.TherapyDetails.CoupleId = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleId;
                BizAction.TherapyDetails.CoupleUnitId = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleUnitId;
                #region Service Call (Check Validation)

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        App.TherapyExecutionForSurrogacyPatient.setupPage(((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyId, (int)TherapyTabs.Execution);

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Date of LP Saved Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

                #endregion
            }
            #endregion

            #region Code for Follicular Mointoring
            else if (((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyTypeId == 3)
            {

                string kEY = ((TextBlock)((ExeExtendedGridNew)((CheckBox)(sender)).Parent).Children[0]).Text;
                string VALUE = GetPropertyValue(kEY, (clsTherapyExecutionVO)((CheckBox)(sender)).DataContext);

                //if (VALUE=="True")
                //{
                //    //if (Flag==false)
                //    //{
                //    //    //Flag = (((IVFTherapyExecution)sender).Flag);  
                //    //}

                clsIVFDashboard_AddPlanTherapyBizActionVO BizAction = new clsIVFDashboard_AddPlanTherapyBizActionVO();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                BizAction.TherapyDetails = new clsPlanTherapyVO();
                BizAction.TherapyDetails.ID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyId;
                BizAction.TherapyDetails.UnitID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyUnitId;
                BizAction.TherapyDetails.ThreapyExecutionId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).ID;
                BizAction.TherapyDetails.TherapyExeTypeID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyTypeId;
                BizAction.TherapyDetails.TherapyStartDate = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyStartDate;
                BizAction.TherapyDetails.PhysicianId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PhysicianID;
                BizAction.TherapyDetails.TabID = (int)TherapyTabs.Execution;
                BizAction.TherapyDetails.Day = kEY;
                BizAction.TherapyDetails.Value = VALUE;
                BizAction.TherapyDetails.CoupleId = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleId;
                BizAction.TherapyDetails.CoupleUnitId = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleUnitId;
                #region Service Call (Check Validation)

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        App.TherapyExecutionForSurrogacyPatient.setupPage(((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyId, 0);

                        if (((clsIVFDashboard_AddPlanTherapyBizActionVO)args.Result).SuccessStatus == 10)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Follicular US Deleted Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                        else if (((clsIVFDashboard_AddPlanTherapyBizActionVO)args.Result).SuccessStatus == 11)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "As follicular details already added for this day, Follicular US can't be deleted.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Follicular US Saved Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }

                        ((CheckBox)(sender)).IsEnabled = false;
                        //((CheckBox)(sender)).IsChecked = false;
                        //((CheckBox)(sender)).IsEnabled = false;
                        //((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).IsBool=false;

                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                //}
                //else
                //{



                //    MessageBoxControl.MessageBoxChildWindow msgW1=
                //        new MessageBoxControl.MessageBoxChildWindow("Palash", "Follicular US Cannot be Edited", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //    msgW1.Show();


                //}


                #endregion


            }
            #endregion

            #region Code for OvumPickup
            else if (((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyTypeId == 4)
            {
                string kEY = ((TextBlock)((ExeExtendedGridForSurrogacyPatient)((CheckBox)(sender)).Parent).Children[0]).Text;
                string VALUE = GetPropertyValue(kEY, (clsTherapyExecutionVO)((CheckBox)(sender)).DataContext);
                clsIVFDashboard_AddPlanTherapyBizActionVO BizAction = new clsIVFDashboard_AddPlanTherapyBizActionVO();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                BizAction.TherapyDetails = new clsPlanTherapyVO();
                BizAction.TherapyDetails.ID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyId;
                BizAction.TherapyDetails.UnitID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyUnitId;
                BizAction.TherapyDetails.ThreapyExecutionId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).ID;
                BizAction.TherapyDetails.TherapyExeTypeID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyTypeId;
                BizAction.TherapyDetails.TherapyStartDate = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyStartDate;
                BizAction.TherapyDetails.PhysicianId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PhysicianID;
                BizAction.TherapyDetails.TabID = (int)TherapyTabs.Execution;
                BizAction.TherapyDetails.Day = kEY;
                BizAction.TherapyDetails.Value = VALUE;
                BizAction.TherapyDetails.CoupleId = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleId;
                BizAction.TherapyDetails.CoupleUnitId = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleUnitId;
                #region Service Call (Check Validation)

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        App.TherapyExecutionForSurrogacyPatient.setupPage(((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyId, (int)TherapyTabs.Execution);
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Ovum Pick UP Saved Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

                #endregion


            }
            #endregion

            #region Code for EmbryoTransfer
            else if (((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyTypeId == 5)
            {
                string kEY = ((TextBlock)((ExeExtendedGridForSurrogacyPatient)((CheckBox)(sender)).Parent).Children[0]).Text;
                string VALUE = GetPropertyValue(kEY, (clsTherapyExecutionVO)((CheckBox)(sender)).DataContext);
                clsIVFDashboard_AddPlanTherapyBizActionVO BizAction = new clsIVFDashboard_AddPlanTherapyBizActionVO();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                BizAction.TherapyDetails = new clsPlanTherapyVO();
                BizAction.TherapyDetails.ID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyId;
                BizAction.TherapyDetails.UnitID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyUnitId;
                BizAction.TherapyDetails.ThreapyExecutionId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).ID;
                BizAction.TherapyDetails.TherapyExeTypeID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyTypeId;
                BizAction.TherapyDetails.TherapyStartDate = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyStartDate;
                BizAction.TherapyDetails.PhysicianId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PhysicianID;
                BizAction.TherapyDetails.TabID = (int)TherapyTabs.Execution;
                BizAction.TherapyDetails.Day = kEY;
                BizAction.TherapyDetails.Value = VALUE;
                BizAction.TherapyDetails.CoupleId = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleId;
                BizAction.TherapyDetails.CoupleUnitId = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleUnitId;
                #region Service Call (Check Validation)

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        App.TherapyExecutionNew.setupPage(((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyId, (int)TherapyTabs.Execution);
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Embryo Transfer Saved Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

                #endregion


            }
            #endregion


        }

        #endregion

        #region Coverters

        public class TestToBrushConverter : FrameworkElement, IValueConverter
        {
            public object Convert(object value,
                               Type targetType,
                               object parameter,
                               CultureInfo culture)
            {
                if (value != null)
                {
                    return new SolidColorBrush(Colors.Green);
                }
                else
                    return new SolidColorBrush(Colors.Black);
            }

            public object ConvertBack(object value,
                                      Type targetType,
                                      object parameter,
                                      CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class TestToButtonTitleConverter : FrameworkElement, IValueConverter
        {
            public object Convert(object value,
                                Type targetType,
                                object parameter,
                                CultureInfo culture)
            {
                if (value != null)
                {
                    return "View";
                }
                else
                    return "Enter";
            }

            public object ConvertBack(object value,
                                        Type targetType,
                                        object parameter,
                                        CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class TestToStatusConverter : FrameworkElement, IValueConverter
        {
            public object Convert(object value,
                                Type targetType,
                                object parameter,
                                CultureInfo culture)
            {
                if (value != null)
                {
                    return "Done";
                }
                else
                    return "Not Done";
            }

            public object ConvertBack(object value,
                                        Type targetType,
                                        object parameter,
                                        CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class BoolToYesNoConverter : FrameworkElement, IValueConverter
        {
            public object Convert(object value,
                                Type targetType,
                                object parameter,
                                CultureInfo culture)
            {
                try
                {
                    bool? val = (bool?)value;
                    if (val.HasValue)
                    {
                        if (val.Value == bool.Parse(parameter.ToString()))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return val;
                    }
                }
                catch (Exception ex)
                {

                    //throw;
                }
                return null;
            }

            public object ConvertBack(object value,
                                        Type targetType,
                                        object parameter,
                                        CultureInfo culture)
            {
                try
                {
                    bool? val = (bool?)value;
                    if (val.HasValue)
                    {
                        if (val.Value == bool.Parse(parameter.ToString()))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return val;
                    }
                }
                catch (Exception ex)
                {

                    //throw;
                }
                return null;
            }
        }

        public class EscapeStringToPlainConverter : FrameworkElement, IValueConverter
        {
            #region IValueConverter Members

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value != null)
                {
                    return value.ToString().Replace(Environment.NewLine, " ");
                }
                else
                {
                    return value;
                }
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        public class TestToElapsedTimeConverter : FrameworkElement, IValueConverter
        {
            public object Convert(object value,
                               Type targetType,
                               object parameter,
                               CultureInfo culture)
            {
                if (value != null)
                {
                    return DateTime.Today.Subtract((DateTime)value).Days.ToString() + " Days";
                }
                else
                    return null;
            }

            public object ConvertBack(object value,
                                      Type targetType,
                                      object parameter,
                                      CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class BackPathStackItem
        {

            public BackPathStackItem()
            {

            }

            public BackPathStackItem(string Header, string Path, double Width)
            {
                this.Header = Header;
                this.Path = Path;
                this.SideImageWidth = Width;
            }

            public string Header { get; set; }
            public string Path { get; set; }
            public double SideImageWidth { get; set; }
        }

        public class ToLimitCharacterConverter : FrameworkElement, IValueConverter
        {
            public object Convert(object value,
                                Type targetType,
                                object parameter,
                                CultureInfo culture)
            {
                if (value != null)
                {
                    String txtblk = (String)value;
                    if (txtblk.Length > 50)
                    {
                        char[] charArray = new char[50];
                        txtblk.CopyTo(0, charArray, 0, 50);
                        txtblk = charArray.ToString();

                        for (int i = 0; i < 50; i++)
                        {
                            if (charArray[i] == 13 && i < 49)
                            {
                                charArray[i] = charArray[i + 1];
                            }
                        }
                        txtblk = new String(charArray);
                        return txtblk;
                    }
                    else
                    {
                        return value;
                    }
                }
                else
                    return null;
            }

            public object ConvertBack(object value,
                                        Type targetType,
                                        object parameter,
                                        CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }

    public class ExtendedGridForSurrogate : Grid
    {
        #region Variables
        public static Control TherapyFocusControl { get; set; }
        public static clsTherapyExecutionVO TherapyExeDetails { get; set; }
        public static string _kEY { get; set; }
        public bool IsSurrogateGrid { get; set; }

        #endregion
        public bool Flag { get; set; }

        #region Constructor
        public ExtendedGridForSurrogate()
            : base()
        {

            this.Loaded += new RoutedEventHandler(this.ChildItemSurrogate_Loaded);
        }
        #endregion

        #region Loaded Event
        private void ChildItemSurrogate_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in this.Children)
            {
                if (item is TextBox)
                {
                    ((TextBox)item).MouseEnter += new MouseEventHandler(ExtendedGrid_MouseEnter);
                    ((TextBox)item).MouseLeave += new MouseEventHandler(ExtendedGrid_MouseLeave);
                    ((TextBox)item).GotFocus += new RoutedEventHandler(ExtendedGrid_GotFocus);
                    ((TextBox)item).LostFocus += new RoutedEventHandler(ExtendedGrid_LostFocus);
                    ((TextBox)item).KeyDown += new KeyEventHandler(ExtendedGridSurrogate_KeyDown);

                }
                if (item is CheckBox)
                {
                    //((CheckBox)item).Click += new RoutedEventHandler(ExtendedGridSurrogate_Click);
                    ((CheckBox)item).IsEnabled = false;
                }
            }
        }

        #endregion

        #region Set/Get Propert Value Methods

        private void SetPropertyValue(string pName, clsTherapyExecutionVO control, string value)
        {

            Type type = control.GetType();

            string propertyName = pName;
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            PropertyInfo prop = type.GetProperty(propertyName, flags);
            if (null != prop && prop.CanWrite)
            {
                prop.SetValue(control, value, null);
            }
        }

        private string GetPropertyValue(string pName, clsTherapyExecutionVO control)
        {
            Type type = control.GetType();

            string propertyName = pName;

            BindingFlags flags = BindingFlags.GetProperty;

            Binder binder = null;

            object[] args = null;

            object value = type.InvokeMember(

            propertyName,

            flags,

            binder,

            control,

            args

            );
            if (value != null)
            {
                return value.ToString();
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region TextBox Events

        void ExtendedGridSurrogate_KeyDown(object sender, KeyEventArgs e)
        {
            #region Drug Cell Deletion (I Handel this By Value to set Value as "Delete" and "DeleteAll" )

            if (e.Key == Key.Delete && e.PlatformKeyCode == 46)
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to Delete Drug Dosage for Specific Date?";
                MessageBoxControl.MessageBoxChildWindow msgWin =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += (result) =>
                {
                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            string kEY = ((TextBlock)((ExtendedGridForSurrogate)((TextBox)(sender)).Parent).Children[0]).Text;//
                            string VALUE = "Delete"; //GetPropertyValue(kEY, (clsTherapyExecutionVO)((TextBox)(sender)).DataContext);

                            clsIVFDashboard_AddPlanTherapyBizActionVO BizAction = new clsIVFDashboard_AddPlanTherapyBizActionVO();
                            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                            BizAction.TherapyDetails = new clsPlanTherapyVO();
                            BizAction.TherapyDetails.ID = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).PlanTherapyId;
                            BizAction.TherapyDetails.UnitID = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).PlanTherapyUnitId;
                            BizAction.TherapyDetails.ThreapyExecutionId = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).ID;
                            BizAction.TherapyDetails.TherapyExeTypeID = (int)TherapyGroup.Drug;
                            BizAction.TherapyDetails.TherapyStartDate = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).TherapyStartDate;
                            BizAction.TherapyDetails.PhysicianId = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).PhysicianID;
                            BizAction.TherapyDetails.TabID = (int)TherapyTabs.Execution;
                            BizAction.TherapyDetails.IsSurrogateDrug = true;
                            BizAction.TherapyDetails.IsSurrogateCalendar = true;
                            BizAction.TherapyDetails.Day = kEY;
                            BizAction.TherapyDetails.Value = VALUE;
                            BizAction.TherapyDetails.ThearpyTypeDetailId = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).ThearpyTypeDetailId;
                            #region Service Call (Check Validation)

                            client.ProcessCompleted += (s, args) =>
                            {
                                if (args.Error == null && args.Result != null)
                                {
                                    App.TherapyExecutionForSurrogate.setupPage(((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).PlanTherapyId, (int)TherapyTabs.Execution);

                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Drug Updated Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.Show();

                                }
                            };
                            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                            client.CloseAsync();
                            #endregion
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                };
                msgWin.Show();

            }
            #endregion

            #region Drug Updation
            else
            {
                try
                {
                    string kEY = ((TextBlock)((ExtendedGridForSurrogate)((TextBox)(sender)).Parent).Children[0]).Text;//
                    string VALUE = GetPropertyValue(kEY, (clsTherapyExecutionVO)((TextBox)(sender)).DataContext);
                    //SetPropertyValue(kEY,(clsTherapyExecutionVO)((TextBox)(sender)).DataContext,VALUE);
                    TherapyExeDetails = new clsTherapyExecutionVO();
                    TherapyExeDetails = (clsTherapyExecutionVO)((TextBox)(sender)).DataContext;
                    _kEY = ((TextBlock)((ExtendedGridForSurrogate)((TextBox)(sender)).Parent).Children[0]).Text;
                    clsgetTherapyDrugDetailsBizActionVO BizAction = new clsgetTherapyDrugDetailsBizActionVO();
                    BizAction.TherapyExeID = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).ID;
                    BizAction.UnitID = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).PlanTherapyUnitId;
                    BizAction.DayNo = kEY;
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            AddDrug addDrug = new AddDrug();
                            addDrug.IsEdit = true;
                            addDrug.IsSurrogateCalendar = true;
                            addDrug.IsSurrogateDrug = true;

                            addDrug.TherpayExeId = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).ID;
                            addDrug.TherapyStartDate = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).TherapyStartDate;
                            if (((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugDate == null)
                            {
                                string addDays = BizAction.DayNo.Substring(BizAction.DayNo.IndexOf("y") + 1);

                                ((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugDate = addDrug.TherapyStartDate.Value.AddDays(Convert.ToInt64(addDays) - 1);
                                ((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugDate = ((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugDate.Value.Add(System.DateTime.Now.TimeOfDay);
                                addDrug.dtthreapystartdate.SelectedDate = ((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugDate;
                            }
                            else
                            {
                                addDrug.dtthreapystartdate.SelectedDate = ((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugDate;
                            }
                            addDrug.txtForDays.Text = "1";
                            addDrug.txtDosage.Text = VALUE;
                            addDrug.txtTime.Value = ((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugDate;
                            if (!string.IsNullOrEmpty(((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugNotes))
                            {
                                addDrug.txtDrugNotes.Text = ((clsgetTherapyDrugDetailsBizActionVO)args.Result).TherapyDrugDetails.DrugNotes;
                            }
                            addDrug.DrugId = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).ThearpyTypeDetailId; ;
                            addDrug.OnSaveButton_Click += new RoutedEventHandler(App.TherapyExecutionForSurrogate.addDrug_OnSaveButton_Click);

                            addDrug.Show();
                        }
                    };

                    client.ProcessAsync(BizAction, new clsUserVO());
                    client.CloseAsync();
                }
                catch (Exception ex)
                {

                }

            }
            #endregion

        }


        void ExtendedGrid_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        void ExtendedGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            if (((TextBox)(sender)) != null && ((TextBox)(sender)).Text != null && ((TextBox)(sender)).Text != string.Empty)
            {
                ToolTip tp = (ToolTip)ToolTipService.GetToolTip((DependencyObject)(((TextBox)(sender))));
                if (tp.Content != null && tp.Content.ToString() != string.Empty)
                {
                    tp.Visibility = Visibility.Visible;
                }
                else
                {
                    tp.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                ToolTip tp = (ToolTip)ToolTipService.GetToolTip((DependencyObject)(((TextBox)(sender))));
                tp.Visibility = Visibility.Collapsed;
            }
        }

        string OrgTextBoxValue = "";

        void ExtendedGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            App.TherapyFocusControl = (Control)sender;
            App.kEY = ((TextBlock)((ExtendedGridForSurrogate)((TextBox)(sender)).Parent).Children[0]).Text;
            OrgTextBoxValue = ((TextBox)(sender)).Text;
        }

        void ExtendedGrid_LostFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)(sender)).Text = OrgTextBoxValue;
        }

        int count = 0;
        #endregion

        #region CheckBox Checked and Unchecked

        void ExtendedGridSurrogate_Click(object sender, RoutedEventArgs e)
        {
            #region Code for Event (Date of LP)
            if (((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyTypeId == 1)
            {
                string kEY = ((TextBlock)((ExtendedGridForSurrogate)((CheckBox)(sender)).Parent).Children[0]).Text;
                string VALUE = GetPropertyValue(kEY, (clsTherapyExecutionVO)((CheckBox)(sender)).DataContext);
                clsAddPlanTherapyBizActionVO BizAction = new clsAddPlanTherapyBizActionVO();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                BizAction.TherapyDetails = new clsPlanTherapyVO();
                BizAction.TherapyDetails.ID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyId;
                BizAction.TherapyDetails.UnitID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyUnitId;
                //BizAction.TherapyDetails.SurrogateExecutionId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).SurrogateExeID;
                BizAction.TherapyDetails.SurrogateExecutionId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).ID;
                BizAction.TherapyDetails.TherapyExeTypeID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyTypeId;
                BizAction.TherapyDetails.TabID = (int)TherapyTabs.Execution;
                BizAction.TherapyDetails.TherapyStartDate = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyStartDate;
                BizAction.TherapyDetails.PhysicianId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PhysicianID;
                BizAction.TherapyDetails.IsSurrogate = true;
                //BizAction.TherapyDetails.SurrogateID = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).SurrogateID; 
                BizAction.TherapyDetails.IsSurrogateCalendar = true;
                BizAction.TherapyDetails.Day = kEY;
                BizAction.TherapyDetails.Value = VALUE;
                BizAction.TherapyDetails.CoupleId = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleId;
                BizAction.TherapyDetails.CoupleUnitId = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleUnitId;
                #region Service Call (Check Validation)

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        App.TherapyExecutionForSurrogate.setupPage(((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyId, (int)TherapyTabs.Execution);

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Date of LP For Surrogate Saved Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

                #endregion
            }
            #endregion

            #region Code for EmbryoTransfer
            else if (((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyTypeId == 5)
            {
                string kEY = ((TextBlock)((ExtendedGridForSurrogate)((CheckBox)(sender)).Parent).Children[0]).Text;
                string VALUE = GetPropertyValue(kEY, (clsTherapyExecutionVO)((CheckBox)(sender)).DataContext);
                clsAddPlanTherapyBizActionVO BizAction = new clsAddPlanTherapyBizActionVO();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                BizAction.TherapyDetails = new clsPlanTherapyVO();
                BizAction.TherapyDetails.ID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyId;
                BizAction.TherapyDetails.UnitID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyUnitId;
                //BizAction.TherapyDetails.SurrogateExecutionId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).SurrogateExeID;
                BizAction.TherapyDetails.SurrogateExecutionId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).ID;
                BizAction.TherapyDetails.TherapyExeTypeID = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyTypeId;
                BizAction.TherapyDetails.TherapyStartDate = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).TherapyStartDate;
                BizAction.TherapyDetails.PhysicianId = ((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PhysicianID;
                BizAction.TherapyDetails.IsSurrogate = true;
                //BizAction.TherapyDetails.SurrogateID = ((clsTherapyExecutionVO)((TextBox)(sender)).DataContext).SurrogateID; 
                BizAction.TherapyDetails.IsSurrogateCalendar = true;
                BizAction.TherapyDetails.TabID = (int)TherapyTabs.Execution;
                BizAction.TherapyDetails.Day = kEY;
                BizAction.TherapyDetails.Value = VALUE;
                BizAction.TherapyDetails.CoupleId = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleId;
                BizAction.TherapyDetails.CoupleUnitId = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleUnitId;
                #region Service Call (Check Validation)

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        App.TherapyExecutionForSurrogate.setupPage(((clsTherapyExecutionVO)((CheckBox)(sender)).DataContext).PlanTherapyId, (int)TherapyTabs.Execution);
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Embryo Transfer for Surrogate Saved Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

                #endregion


            }
            #endregion


        }

        #endregion

        #region Coverters

        public class TestToBrushConverter : FrameworkElement, IValueConverter
        {
            public object Convert(object value,
                               Type targetType,
                               object parameter,
                               CultureInfo culture)
            {
                if (value != null)
                {
                    return new SolidColorBrush(Colors.Green);
                }
                else
                    return new SolidColorBrush(Colors.Black);
            }

            public object ConvertBack(object value,
                                      Type targetType,
                                      object parameter,
                                      CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class TestToButtonTitleConverter : FrameworkElement, IValueConverter
        {
            public object Convert(object value,
                                Type targetType,
                                object parameter,
                                CultureInfo culture)
            {
                if (value != null)
                {
                    return "View";
                }
                else
                    return "Enter";
            }

            public object ConvertBack(object value,
                                        Type targetType,
                                        object parameter,
                                        CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class TestToStatusConverter : FrameworkElement, IValueConverter
        {
            public object Convert(object value,
                                Type targetType,
                                object parameter,
                                CultureInfo culture)
            {
                if (value != null)
                {
                    return "Done";
                }
                else
                    return "Not Done";
            }

            public object ConvertBack(object value,
                                        Type targetType,
                                        object parameter,
                                        CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class BoolToYesNoConverter : FrameworkElement, IValueConverter
        {
            public object Convert(object value,
                                Type targetType,
                                object parameter,
                                CultureInfo culture)
            {
                try
                {
                    bool? val = (bool?)value;
                    if (val.HasValue)
                    {
                        if (val.Value == bool.Parse(parameter.ToString()))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return val;
                    }
                }
                catch (Exception ex)
                {

                    //throw;
                }
                return null;
            }

            public object ConvertBack(object value,
                                        Type targetType,
                                        object parameter,
                                        CultureInfo culture)
            {
                try
                {
                    bool? val = (bool?)value;
                    if (val.HasValue)
                    {
                        if (val.Value == bool.Parse(parameter.ToString()))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return val;
                    }
                }
                catch (Exception ex)
                {

                    //throw;
                }
                return null;
            }
        }

        public class EscapeStringToPlainConverter : FrameworkElement, IValueConverter
        {
            #region IValueConverter Members

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value != null)
                {
                    return value.ToString().Replace(Environment.NewLine, " ");
                }
                else
                {
                    return value;
                }
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        public class TestToElapsedTimeConverter : FrameworkElement, IValueConverter
        {
            public object Convert(object value,
                               Type targetType,
                               object parameter,
                               CultureInfo culture)
            {
                if (value != null)
                {
                    return DateTime.Today.Subtract((DateTime)value).Days.ToString() + " Days";
                }
                else
                    return null;
            }

            public object ConvertBack(object value,
                                      Type targetType,
                                      object parameter,
                                      CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class BackPathStackItem
        {

            public BackPathStackItem()
            {

            }

            public BackPathStackItem(string Header, string Path, double Width)
            {
                this.Header = Header;
                this.Path = Path;
                this.SideImageWidth = Width;
            }

            public string Header { get; set; }
            public string Path { get; set; }
            public double SideImageWidth { get; set; }
        }

        public class ToLimitCharacterConverter : FrameworkElement, IValueConverter
        {
            public object Convert(object value,
                                Type targetType,
                                object parameter,
                                CultureInfo culture)
            {
                if (value != null)
                {
                    String txtblk = (String)value;
                    if (txtblk.Length > 50)
                    {
                        char[] charArray = new char[50];
                        txtblk.CopyTo(0, charArray, 0, 50);
                        txtblk = charArray.ToString();

                        for (int i = 0; i < 50; i++)
                        {
                            if (charArray[i] == 13 && i < 49)
                            {
                                charArray[i] = charArray[i + 1];
                            }
                        }
                        txtblk = new String(charArray);
                        return txtblk;
                    }
                    else
                    {
                        return value;
                    }
                }
                else
                    return null;
            }

            public object ConvertBack(object value,
                                        Type targetType,
                                        object parameter,
                                        CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }

    //...........................................................
}

