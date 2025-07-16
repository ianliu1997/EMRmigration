using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Browser;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Threading;
using DataDrivenApplication;
using System.Reflection;
using System.Xml.Serialization;
using System.Runtime.Serialization;
//using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using PalashDynamics.Service.DataTemplateServiceRef1;
using System.Windows.Data;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Printing;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.EMR;
using CIMS;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;

namespace DataDrivenApplication
{
    public partial class FormDesigner : UserControl
    {
        #region Hard Coded for Print
        //private EditableImage editableImage = null;
        //private int _height;
        //private int _width;
        //private double _xmin = -2.1; // -2.1
        //private double _ymin = -1.3; // -1.3
        //private double _xmax = 1;    // 1
        //private double _ymax = 1.3;  // 1.3
        //private bool _captured = false;
        //private Color[] _map = null;

        //private bool imageGenerated = false;
        #endregion
        System.Windows.Controls.Primitives.Popup p = null;
        System.Windows.Controls.Primitives.Popup pf = null;
        public FormDetail SelectedFormStructure { get; set; }
        public FormDetail PreviewFormStructure { get; set; }
        public FormDesigner(FormDetail SelectedFormStructure)
        {
            InitializeComponent();
            this.SelectedFormStructure = SelectedFormStructure;
            this.Loaded += new RoutedEventHandler(FormDesigner_Loaded);
            this.Unloaded += new RoutedEventHandler(FormDesigner_Unloaded);
        }

        void FormDesigner_Unloaded(object sender, RoutedEventArgs e)
        {
            p.IsOpen = false;
            pf.IsOpen = false;
            p = null;
            pf = null;
        }


        public object SelectedItem { get; set; }

        void FormDesigner_Loaded(object sender, RoutedEventArgs e)
        {
            trvFormStructure.ItemsSource = null;
            List<FormDetail> list = new List<FormDetail>();
            list.Add(SelectedFormStructure);
            trvFormStructure.ItemsSource = list;
            SelectedItem = SelectedFormStructure;
            Thread T = new Thread(m);
            T.Start();

            p = new System.Windows.Controls.Primitives.Popup();
            pf = new System.Windows.Controls.Primitives.Popup();
            Border b = new Border();
            b.Background = (Brush)this.Resources["BackgroundDefault"];
            b.BorderBrush = (Brush)this.Resources["BorderDefault"];
            b.BorderThickness = new Thickness(1);
            b.CornerRadius = new CornerRadius(5);
            b.MouseLeftButtonDown += new MouseButtonEventHandler(b_MouseLeftButtonDown);
            ScrollViewer sv = new ScrollViewer();
            sv.Background = new SolidColorBrush(Colors.Gray);
            sv.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            sv.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            sv.Content = b;
            p.Child = sv;

            Border b1 = new Border();
            b1.Background = (Brush)this.Resources["BackgroundDefault"];
            b1.BorderBrush = (Brush)this.Resources["BorderDefault"];
            b1.BorderThickness = new Thickness(1);
            b1.CornerRadius = new CornerRadius(5);
            b1.MouseLeftButtonDown += new MouseButtonEventHandler(b1_MouseLeftButtonDown);
            ScrollViewer sv1 = new ScrollViewer();
            sv1.Background = new SolidColorBrush(Colors.Gray);
            sv1.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            sv1.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            sv1.Content = b1;
            pf.Child = sv1;
        }

        void b_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //throw new NotImplementedException();

            p.IsOpen = false;
        }

        void b1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //throw new NotImplementedException();

            pf.IsOpen = false;
        }
        private void m()
        {
            Thread.Sleep(600);
            Dispatcher.BeginInvoke(() =>
            {
                trvFormStructure.ExpandAll();
                trvFormStructure.SelectItem(SelectedItem);
                GenratePreview();
                MapRelations();
            });
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (trvFormStructure.SelectedItem != null)
            {
                if (trvFormStructure.SelectedItem is FormDetail)
                {
                    SectionEditor w = new SectionEditor();
                    w.OnSaveButtonClick += new RoutedEventHandler(AddSaveButton_Click);
                    w.Show();
                }
                if (trvFormStructure.SelectedItem is SectionDetail)
                {
                    FieldEditor w = new FieldEditor((BaseDetail)trvFormStructure.SelectedItem);
                    //w.DataContext = trvFormStructure.SelectedItem;
                    w.OnSaveButtonClick += new RoutedEventHandler(AddSaveButton_Click);
                    w.Show();
                }
                if (trvFormStructure.SelectedItem is FieldDetail)
                {
                    FieldEditor w = new FieldEditor((BaseDetail)trvFormStructure.SelectedItem);
                    w.DataContext = trvFormStructure.SelectedItem;
                    w.OnSaveButtonClick += new RoutedEventHandler(AddSaveButton_Click);
                    w.Show();
                }
            }
        }

        private void EditItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (trvFormStructure.SelectedItem != null)
            {
                if (trvFormStructure.SelectedItem is FormDetail)
                {
                    FormEditor w = new FormEditor();
                    w.DataContext = trvFormStructure.SelectedItem;
                    w.OnOkButtonClick += new RoutedEventHandler(EditSaveButton_Click);
                    w.Show();
                }
                if (trvFormStructure.SelectedItem is SectionDetail)
                {
                    SectionEditor w = new SectionEditor();
                    w.DataContext = trvFormStructure.SelectedItem;
                    w.OnSaveButtonClick = new RoutedEventHandler(EditSaveButton_Click);
                    w.Show();
                }
                if (trvFormStructure.SelectedItem is FieldDetail)
                {
                    FieldEditor w = new FieldEditor((BaseDetail)((FieldDetail)trvFormStructure.SelectedItem).Parent);
                    w.DataContext = ((FieldDetail)trvFormStructure.SelectedItem);

                    w.OnSaveButtonClick += new RoutedEventHandler(EditSaveButton_Click);
                    w.Show();
                }
            }
        }

        private void EditSaveButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedItem = sender;
            trvFormStructure.ItemsSource = null;
            List<FormDetail> list = new List<FormDetail>();
            list.Add(SelectedFormStructure);
            trvFormStructure.ItemsSource = list;
            Thread T = new Thread(m);
            T.Start();
        }


        private void AddSaveButton_Click(object sender, RoutedEventArgs e)
        {
            //SelectedItem = sender;
            if (trvFormStructure.SelectedItem is FormDetail)
            {
                if (((FormDetail)trvFormStructure.SelectedItem).SectionList == null)
                    ((FormDetail)trvFormStructure.SelectedItem).SectionList = new List<SectionDetail>();
                ((FormDetail)trvFormStructure.SelectedItem).SectionList.Add((SectionDetail)sender);
            }
            if (trvFormStructure.SelectedItem is SectionDetail)
            {                
                bool tempflag=false;
                foreach (SectionDetail SD in SelectedFormStructure.SectionList)
                {
                    if (SD.FieldList != null)
                    {
                        foreach (FieldDetail FD in SD.FieldList)
                        {
                            if (FD.Name !=null && ((FieldDetail)sender).Name.ToString() == FD.Name.ToString())
                            {
                                tempflag = true;
                                break;
                            }
                        }
                        if (tempflag == true)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgbx = new MessageBoxControl.MessageBoxChildWindow("PALASH", "Field is not saved. Name is already exist.\nPlease enter a unique name.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgbx.Show();
                            break;
                        }
                    }
                }
                
                if(tempflag==false)
                {
                    if (((SectionDetail)trvFormStructure.SelectedItem).FieldList == null)
                        ((SectionDetail)trvFormStructure.SelectedItem).FieldList = new List<FieldDetail>();
                    ((FieldDetail)sender).Parent = ((SectionDetail)trvFormStructure.SelectedItem);
                    ((SectionDetail)trvFormStructure.SelectedItem).FieldList.Add((FieldDetail)sender);
                }
            }
            if (trvFormStructure.SelectedItem is FieldDetail)
            {                
                if (((FieldDetail)trvFormStructure.SelectedItem).DependentFieldDetail == null)
                    ((FieldDetail)trvFormStructure.SelectedItem).DependentFieldDetail = new List<FieldDetail>();
                ((FieldDetail)sender).Parent = ((FieldDetail)trvFormStructure.SelectedItem);
                ((FieldDetail)trvFormStructure.SelectedItem).DependentFieldDetail.Add((FieldDetail)sender);
            }

            trvFormStructure.ItemsSource = null;
            List<FormDetail> list = new List<FormDetail>();
            list.Add(SelectedFormStructure);
            trvFormStructure.ItemsSource = list;
            Thread T = new Thread(m);
            T.Start();
            //GenratePreview();
        }


        private void DeleteItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (trvFormStructure.SelectedItem != null)
            {
                if (trvFormStructure.SelectedItem is FieldDetail)
                {
                    DeletionWindow dw = new DeletionWindow();

                    dw.Message = "Are you sure you want to remove the Field '" + ((FieldDetail)trvFormStructure.SelectedItem).Title + "' ?";
                    dw.ID = 1;
                    dw.FormTitle.Text = "Delete Field";
                    dw.OnOkButtonClick += new RoutedEventHandler(dw_OnOkButtonClick);
                    dw.Show();
                }

                if (trvFormStructure.SelectedItem is SectionDetail)
                {
                    DeletionWindow dw = new DeletionWindow();
                    dw.Message = "Are you sure you want to remove the Section '" + ((SectionDetail)trvFormStructure.SelectedItem).Title + "' ?";
                    dw.ID = 2;
                    dw.FormTitle.Text = "Delete Section";
                    dw.OnOkButtonClick += new RoutedEventHandler(dw_OnOkButtonClick);
                    dw.Show();
                }
                if (trvFormStructure.SelectedItem is FormDetail)
                {
                    DeletionWindow dw = new DeletionWindow();
                    dw.Message = "Are you sure you want to remove the Form (Template) '" + ((FormDetail)trvFormStructure.SelectedItem).Title + "' ?";
                    dw.ID = 3;
                    dw.FormTitle.Text = "Delete Form/Template";
                    dw.OnOkButtonClick += new RoutedEventHandler(dw_OnOkButtonClick);
                    dw.Show();

                }
            }
        }

        void dw_OnOkButtonClick(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            DeletionWindow dw = (DeletionWindow)sender;
            if (dw.ID == 1)
            {
                SectionDetail s = (SectionDetail)trvFormStructure.GetParentItem((FieldDetail)trvFormStructure.SelectedItem);
                s.FieldList.Remove((FieldDetail)trvFormStructure.SelectedItem);

                SelectedItem = s;
                trvFormStructure.ItemsSource = null;
                List<FormDetail> list = new List<FormDetail>();
                list.Add(SelectedFormStructure);
                trvFormStructure.ItemsSource = list;
                Thread T = new Thread(m);
                T.Start();
            }
            if (dw.ID == 2)
            {
                FormDetail f = (FormDetail)trvFormStructure.GetParentItem((SectionDetail)trvFormStructure.SelectedItem);
                f.SectionList.Remove((SectionDetail)trvFormStructure.SelectedItem);

                SelectedItem = f;
                trvFormStructure.ItemsSource = null;
                List<FormDetail> list = new List<FormDetail>();
                list.Add(SelectedFormStructure);
                trvFormStructure.ItemsSource = list;
                Thread T = new Thread(m);
                T.Start();
            }
            if (dw.ID == 3)
            {
                //Used With DataTemplate Service
                //Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                //DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                //client.DeleteTemplateCompleted += (s, args) =>
                //{
                //    App.MainWindow.MainRegion.Content = new TemplateList();
                //};
                //client.DeleteTemplateAsync(App.SelectedTemplate);

                clsUpdateEMRTemplateBizActionVO BizAction = new clsUpdateEMRTemplateBizActionVO();
                BizAction.EMRTemplateDetails = App.SelectedTemplate;
                BizAction.EMRTemplateDetails.Status = false;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, args) =>
                {
                    ((ContentControl)this.Parent).Content = new TemplateList();
                };

                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            }
        }

        private void trvFormStructure_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //SelectedItem = trvFormStructure.SelectedItem;
            if (trvFormStructure.SelectedItem != null)
            {
                //if (trvFormStructure.SelectedItem is FormDetail)
                //{
                //    cmdAdd.Text = "Add Section";
                //    cmdEdit.Text = "Edit Form";
                //    cmdDelete.Text = "Delete Form";
                //}
                //if (trvFormStructure.SelectedItem is SectionDetail)
                //{
                //    cmdAdd.Text = "Add Field";
                //    cmdEdit.Text = "Edit Section";
                //    cmdDelete.Text = "Delete Section";
                //}
                //if (trvFormStructure.SelectedItem is FieldDetail)
                //{
                //    cmdAdd.Text = "Add Field";
                //    cmdEdit.Text = "Edit Field";
                //    cmdDelete.Text = "Delete Field";
                //}
            }
        }

        private void GenratePreview()
        {
            this.PreviewFormStructure = SelectedFormStructure.DeepCopy();
            //this.PreviewFormStructure = SelectedFormStructure;
            Form.RowDefinitions.Clear();
            Form.Children.Clear();
            if (PreviewFormStructure != null && PreviewFormStructure.SectionList != null)
            {
                foreach (var item in PreviewFormStructure.SectionList)
                {
                    AddNodeItems(item, false);
                }
            }




            // Work for Setting Alarm Alerts on Bloodstools,Vomiting and urine OutPut
            #region (Hard Coded)
            ////Work for Setting Alarm Alerts on Bloodstools,Vomiting and urine OutPut

            ////IEnumerator<UIElement> lst = (IEnumerator<UIElement>)Form.Children.GetEnumerator();

            //int index = 0;
            //while (index < Form.Children.Count - 1)
            //{
            //    Grid sec = (Grid)Form.Children[index];
            //    if ((string)sec.Tag == "History")
            //    {
            //        Grid cont = (Grid)((Border)sec.Children[0]).Child;

            //        IEnumerator<UIElement> lst1 = (IEnumerator<UIElement>)cont.Children.GetEnumerator();

            //        while (lst1.MoveNext())
            //        {
            //            if (lst1.Current is StackPanel)
            //            {
            //                StackPanel st = (StackPanel)lst1.Current;
            //                IEnumerator<UIElement> lst2 = (IEnumerator<UIElement>)st.Children.GetEnumerator();

            //                while (lst2.MoveNext())
            //                {
            //                    if (lst2.Current is RadioButton)
            //                    {

            //                        RadioButton r = (RadioButton)lst2.Current;

            //                        if (r.Name == "BloodStoolsYes")
            //                        {
            //                            r.Click += new RoutedEventHandler(r_Click);
            //                            if (r.IsChecked == true)
            //                            {
            //                                r.IsChecked = false;
            //                                r.IsChecked = true;
            //                            }
            //                        }
            //                        if (r.Name == "BloodStoolsNo")
            //                        {
            //                            r.Click += new RoutedEventHandler(r_Click);
            //                            if (r.IsChecked == true)
            //                            {
            //                                r.IsChecked = false;
            //                                r.IsChecked = true;
            //                            }
            //                        }
            //                        if (r.Name == "VomitingYes")
            //                        {
            //                            r.Click += new RoutedEventHandler(r_Click);
            //                            if (r.IsChecked == true)
            //                            {
            //                                r.IsChecked = false;
            //                                r.IsChecked = true;
            //                            }
            //                        }
            //                        if (r.Name == "VomitingNo")
            //                        {
            //                            r.Click += new RoutedEventHandler(r_Click);
            //                            if (r.IsChecked == true)
            //                            {
            //                                r.IsChecked = false;
            //                                r.IsChecked = true;
            //                            }
            //                        }
            //                    }
            //                }

            //            }
            //            if (lst1.Current is ComboBox)
            //            {
            //                ComboBox cb = (ComboBox)lst1.Current;

            //                if (cb.Name == "UrineOutput")
            //                {
            //                    cb.SelectionChanged += new SelectionChangedEventHandler(cb_SelectionChanged);

            //                    ListFieldSetting listSetting = ((ListFieldSetting)((FieldDetail)cb.DataContext).Settings);

            //                    if (listSetting.SelectedItem != null)
            //                        listSetting.SelectedItem = listSetting.ItemSource.Where(i => i.Title == listSetting.SelectedItem.Title).Single();
            //                }
            //            }
            //        }
            //    }
            //    index++;
            //}


            #endregion
            //Grid grid = new Grid();
            //grid = Form;
            //LinearGradientBrush a = new LinearGradientBrush();
            //GradientStop gs1 = new GradientStop();
            //gs1.Offset = 0;
            //gs1.Color = Colors.White;
            //a.GradientStops.Add(gs1);
            //grid.Background = a;

            //WriteableBitmap wb = new WriteableBitmap(grid, null);

            //editableImage = new EditableImage((Int32)grid.ActualWidth, (Int32)grid.ActualHeight);

            //int wbCounter = 0;
            //Converter cl = new Converter();

            //for (int idx = 0; idx < editableImage.Height; idx++)      // Height (y)
            //{
            //    for (int jdx = 0; jdx < editableImage.Width; jdx++) // Width (x)
            //    {

            //        if (wbCounter == wb.Pixels.Length)
            //            break;
            //        byte[] colorBytes = cl.ConvIntegertoByteArray(wb.Pixels[wbCounter], 3);
            //        editableImage.SetPixel(jdx, idx, colorBytes[0], colorBytes[1], colorBytes[2], 255);
            //        wbCounter++;
            //    }
            //}
            //imageGenerated = true;

        }

        #region Hard Coded
        //void cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    //throw new NotImplementedException();


        //    //IEnumerator<UIElement> list = (IEnumerator<UIElement>)Form.Children.GetEnumerator();
        //    ComboBox cb = (ComboBox)sender;
        //    if (cb.SelectedIndex == 2)
        //    {
        //        int i = 0;
        //        while (i < Form.Children.Count)
        //        {
        //            Grid sec = (Grid)Form.Children[i];
        //            if ((string)sec.Tag == "Alarm Features")
        //            {
        //                Grid cont = (Grid)((Border)sec.Children[0]).Child;
        //                IEnumerator<UIElement> lst1 = (IEnumerator<UIElement>)cont.Children.GetEnumerator();

        //                while (lst1.MoveNext())
        //                {
        //                    if (lst1.Current is CheckBox)
        //                    {
        //                        CheckBox chk = (CheckBox)lst1.Current;
        //                        if (chk.Name == "ChkAlarm5")
        //                        {
        //                            chk.IsChecked = true;
        //                        }
        //                    }
        //                    if (lst1.Current is TextBox)
        //                    {
        //                        TextBox txtbx = (TextBox)lst1.Current;
        //                        if (txtbx.Name == "TxtAlarm6")
        //                        {
        //                            txtbx.Text = "Urine Absent Problem is Discovered.";

        //                        }

        //                    }
        //                }
        //            }
        //            i++;
        //        }
        //    }

        //    if (cb.SelectedIndex != 2)
        //    {
        //        int i = 0;
        //        while (i < Form.Children.Count)
        //        {
        //            Grid sec = (Grid)Form.Children[i];
        //            if ((string)sec.Tag == "Alarm Features")
        //            {
        //                Grid cont = (Grid)((Border)sec.Children[0]).Child;
        //                IEnumerator<UIElement> lst1 = (IEnumerator<UIElement>)cont.Children.GetEnumerator();

        //                while (lst1.MoveNext())
        //                {
        //                    if (lst1.Current is CheckBox)
        //                    {
        //                        CheckBox chk = (CheckBox)lst1.Current;
        //                        if (chk.Name == "ChkAlarm5")
        //                        {
        //                            chk.IsChecked = false;
        //                        }
        //                    }
        //                    if (lst1.Current is TextBox)
        //                    {
        //                        TextBox txtbx = (TextBox)lst1.Current;
        //                        if (txtbx.Name == "TxtAlarm6")
        //                        {
        //                            txtbx.Text = "";

        //                        }

        //                    }
        //                }
        //            }
        //            i++;
        //        }
        //    }                            
        //}
        #endregion


        #region Hard Coded
        //void r_Click(object sender, RoutedEventArgs e)
        //{
        //    //throw new NotImplementedException();
        //    if (((RadioButton)e.OriginalSource).Name == "BloodStoolsYes")
        //    {
        //        //IEnumerator<UIElement> list = (IEnumerator<UIElement>)Form.Children.GetEnumerator();
        //        int i = 0;
        //        while (i < Form.Children.Count)
        //        {
        //            Grid sec = (Grid)Form.Children[i];
        //            if ((string)sec.Tag == "Alarm Features")
        //            {
        //                Grid cont = (Grid)((Border)sec.Children[0]).Child;
        //                IEnumerator<UIElement> lst1 = (IEnumerator<UIElement>)cont.Children.GetEnumerator();

        //                while (lst1.MoveNext())
        //                {
        //                    if (lst1.Current is CheckBox)
        //                    {
        //                        CheckBox chk = (CheckBox)lst1.Current;
        //                        if (chk.Name == "ChkAlarm1")
        //                        {
        //                            chk.IsChecked = true;
        //                        }
        //                    }
        //                    if (lst1.Current is TextBox)
        //                    {
        //                        TextBox txtbx = (TextBox)lst1.Current;
        //                        if (txtbx.Name == "TxtAlarm2")
        //                        {
        //                            txtbx.Text = "Blood in stools is Discovered.";

        //                        }

        //                    }
        //                }
        //            }
        //            i++;
        //        }
        //    }
        //    if (((RadioButton)e.OriginalSource).Name == "BloodStoolsNo")
        //    {
        //        //IEnumerator<UIElement> list = (IEnumerator<UIElement>)Form.Children.GetEnumerator();
        //        int i = 0;
        //        while (i < Form.Children.Count)
        //        {
        //            Grid sec = (Grid)Form.Children[i];
        //            if ((string)sec.Tag == "Alarm Features")
        //            {
        //                Grid cont = (Grid)((Border)sec.Children[0]).Child;
        //                IEnumerator<UIElement> lst1 = (IEnumerator<UIElement>)cont.Children.GetEnumerator();

        //                while (lst1.MoveNext())
        //                {
        //                    if (lst1.Current is CheckBox)
        //                    {
        //                        CheckBox chk = (CheckBox)lst1.Current;
        //                        if (chk.Name == "ChkAlarm1")
        //                        {
        //                            chk.IsChecked = false;
        //                        }
        //                    }
        //                    if (lst1.Current is TextBox)
        //                    {
        //                        TextBox txtbx = (TextBox)lst1.Current;
        //                        if (txtbx.Name == "TxtAlarm2")
        //                        {
        //                            txtbx.Text = "";

        //                        }

        //                    }
        //                }
        //            }
        //            i++;
        //        }
        //    }

        //    if (((RadioButton)e.OriginalSource).Name == "VomitingYes")
        //    {
        //        IEnumerator<UIElement> list = (IEnumerator<UIElement>)Form.Children.GetEnumerator();

        //        while (list.MoveNext())
        //        {
        //            Grid sec = (Grid)list.Current;
        //            if ((string)sec.Tag == "Alarm Features")
        //            {
        //                Grid cont = (Grid)((Border)sec.Children[0]).Child;
        //                IEnumerator<UIElement> lst1 = (IEnumerator<UIElement>)cont.Children.GetEnumerator();

        //                while (lst1.MoveNext())
        //                {
        //                    if (lst1.Current is CheckBox)
        //                    {
        //                        CheckBox chk = (CheckBox)lst1.Current;
        //                        if (chk.Name == "ChkAlarm3")
        //                        {
        //                            chk.IsChecked = true;
        //                        }
        //                    }
        //                    if (lst1.Current is TextBox)
        //                    {
        //                        TextBox txtbx = (TextBox)lst1.Current;
        //                        if (txtbx.Name == "TxtAlarm4")
        //                        {
        //                            txtbx.Text = "Nausea & Vomoiting Problem is Discovered.";

        //                        }

        //                    }
        //                }
        //            }

        //        }
        //    }

        //    if (((RadioButton)e.OriginalSource).Name == "VomitingNo")
        //    {
        //        IEnumerator<UIElement> list = (IEnumerator<UIElement>)Form.Children.GetEnumerator();

        //        while (list.MoveNext())
        //        {
        //            Grid sec = (Grid)list.Current;
        //            if ((string)sec.Tag == "Alarm Features")
        //            {
        //                Grid cont = (Grid)((Border)sec.Children[0]).Child;
        //                IEnumerator<UIElement> lst1 = (IEnumerator<UIElement>)cont.Children.GetEnumerator();

        //                while (lst1.MoveNext())
        //                {
        //                    if (lst1.Current is CheckBox)
        //                    {
        //                        CheckBox chk = (CheckBox)lst1.Current;
        //                        if (chk.Name == "ChkAlarm3")
        //                        {
        //                            chk.IsChecked = false;
        //                        }
        //                    }
        //                    if (lst1.Current is TextBox)
        //                    {
        //                        TextBox txtbx = (TextBox)lst1.Current;
        //                        if (txtbx.Name == "TxtAlarm4")
        //                        {
        //                            txtbx.Text = "";

        //                        }

        //                    }
        //                }
        //            }

        //        }
        //    }
        //}
        #endregion

        private void MapRelations()
        {
            if (SelectedFormStructure != null && SelectedFormStructure.SectionList != null && SelectedFormStructure.Relations != null && SelectedFormStructure.Relations.Count > 0)
            {
                foreach (var item in SelectedFormStructure.Relations)
                {
                    FieldDetail source = null;
                    FieldDetail target = null;
                    foreach (var section in SelectedFormStructure.SectionList)
                    {
                        if (section.UniqueId.ToString() == item.SourceSectionId)
                        {
                            foreach (var field in section.FieldList)
                            {
                                if (field.UniqueId.ToString() == item.SourceFieldId)
                                {
                                    source = field;
                                    source.Parent = section;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    foreach (var section in SelectedFormStructure.SectionList)
                    {
                        if (section.UniqueId.ToString() == item.TargetSectionId)
                        {
                            foreach (var field in section.FieldList)
                            {
                                if (field.UniqueId.ToString() == item.TargetFieldId)
                                {
                                    target = field;
                                    target.Parent = section;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    if (source != null && target != null)
                    {
                        item.SourceField = source;
                        item.TargetField = target;
                        item.SourceSection = source.Parent.Title;
                        item.TargetSection = target.Parent.Title;
                        target.RelationCondition = item.ExpCondition;
                        if (source.RelationalFieldList == null)
                            source.RelationalFieldList = new List<FieldDetail>();
                        source.RelationalFieldList.Add(target);
                    }
                }
            }


            if (PreviewFormStructure != null && PreviewFormStructure.SectionList != null && PreviewFormStructure.Relations != null && PreviewFormStructure.Relations.Count > 0)
            {
                foreach (var item in PreviewFormStructure.Relations)
                {
                    FieldDetail source = null;
                    FieldDetail target = null;
                    foreach (var section in PreviewFormStructure.SectionList)
                    {
                        if (section.UniqueId.ToString() == item.SourceSectionId)
                        {
                            foreach (var field in section.FieldList)
                            {
                                if (field.UniqueId.ToString() == item.SourceFieldId)
                                {
                                    source = field;
                                    source.Parent = section;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    foreach (var section in PreviewFormStructure.SectionList)
                    {
                        if (section.UniqueId.ToString() == item.TargetSectionId)
                        {
                            foreach (var field in section.FieldList)
                            {
                                if (field.UniqueId.ToString() == item.TargetFieldId)
                                {
                                    target = field;
                                    target.Parent = section;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    if (source != null && target != null)
                    {
                        item.SourceField = source;
                        item.TargetField = target;
                        item.SourceSection = source.Parent.Title;
                        item.TargetSection = target.Parent.Title;
                        target.RelationCondition = item.ExpCondition;
                        ((FrameworkElement)target.LabelControl).Visibility = ((FrameworkElement)target.Control).Visibility = Visibility.Collapsed;
                        if (source.RelationalFieldList == null)
                            source.RelationalFieldList = new List<FieldDetail>();
                        source.RelationalFieldList.Add(target);
                    }
                }
            }
            MapPCR();
            MapCaseReferral();
        }

        private void MapPCR()
        {
            if (SelectedFormStructure != null && SelectedFormStructure.SectionList != null && SelectedFormStructure.PCRRelations != null && SelectedFormStructure.PCRRelations.Count > 0)
            {
                foreach (var item in SelectedFormStructure.PCRRelations)
                {
                    MasterListItem source = null;
                    FieldDetail target = null;
                    source = item.SourceField;

                    foreach (var section in SelectedFormStructure.SectionList)
                    {
                        if (section.UniqueId.ToString() == item.TargetSectionId)
                        {
                            foreach (var field in section.FieldList)
                            {
                                if (field.UniqueId.ToString() == item.TargetFieldId)
                                {
                                    target = field;
                                    target.Parent = section;
                                    break;
                                }
                            }
                            break;
                        }
                    }

                    if (source != null && target != null)
                    {
                        item.TargetField = target;
                        item.TargetSection = target.Parent.Title;
                    }
                }

            }


            if (PreviewFormStructure != null && PreviewFormStructure.SectionList != null && PreviewFormStructure.PCRRelations != null && PreviewFormStructure.PCRRelations.Count > 0)
            {
                foreach (var item in PreviewFormStructure.PCRRelations)
                {
                    MasterListItem source = null;
                    FieldDetail target = null;
                    source = item.SourceField;

                    foreach (var section in PreviewFormStructure.SectionList)
                    {
                        if (section.UniqueId.ToString() == item.TargetSectionId)
                        {
                            foreach (var field in section.FieldList)
                            {
                                if (field.UniqueId.ToString() == item.TargetFieldId)
                                {
                                    target = field;
                                    target.Parent = section;
                                    break;
                                }
                            }
                            break;
                        }
                    }

                    if (source != null && target != null)
                    {
                        item.TargetField = target;
                        item.TargetSection = target.Parent.Title;
                    }
                }
            }
        }

        private void MapCaseReferral()
        {
            if (SelectedFormStructure != null && SelectedFormStructure.SectionList != null && SelectedFormStructure.CaseReferralRelations != null && SelectedFormStructure.CaseReferralRelations.Count > 0)
            {
                foreach (var item in SelectedFormStructure.CaseReferralRelations)
                {
                    MasterListItem source = null;
                    FieldDetail target = null;
                    source = item.SourceField;

                    foreach (var section in SelectedFormStructure.SectionList)
                    {
                        if (section.UniqueId.ToString() == item.TargetSectionId)
                        {
                            foreach (var field in section.FieldList)
                            {
                                if (field.UniqueId.ToString() == item.TargetFieldId)
                                {
                                    target = field;
                                    target.Parent = section;
                                    break;
                                }
                            }
                            break;
                        }
                    }

                    if (source != null && target != null)
                    {
                        item.TargetField = target;
                        item.TargetSection = target.Parent.Title;
                    }
                }

            }


            if (PreviewFormStructure != null && PreviewFormStructure.SectionList != null && PreviewFormStructure.CaseReferralRelations != null && PreviewFormStructure.CaseReferralRelations.Count > 0)
            {
                foreach (var item in PreviewFormStructure.CaseReferralRelations)
                {
                    MasterListItem source = null;
                    FieldDetail target = null;
                    source = item.SourceField;

                    foreach (var section in PreviewFormStructure.SectionList)
                    {
                        if (section.UniqueId.ToString() == item.TargetSectionId)
                        {
                            foreach (var field in section.FieldList)
                            {
                                if (field.UniqueId.ToString() == item.TargetFieldId)
                                {
                                    target = field;
                                    target.Parent = section;
                                    break;
                                }
                            }
                            break;
                        }
                    }

                    if (source != null && target != null)
                    {
                        item.TargetField = target;
                        item.TargetSection = target.Parent.Title;
                    }
                }
            }
        }
        Grid printGrid = null;

        public void AddNodeItems(SectionDetail PItem, bool status)
        {
            if (status == false)
            {
                RowDefinition Row = new RowDefinition();
                //Row.Height = new GridLength(23, GridUnitType.Auto);
                Form.RowDefinitions.Add(Row);
            }
            Grid section = GetSectionLayout(PItem.Title);
            if (PItem.IsToolTip == true)
            {
                #region new added by harish
                //ToolTip tt = new ToolTip();
                TextBox tbl = new TextBox();
                tbl.IsEnabled = false;
                //tbl.Background = new SolidColorBrush(Colors.Yellow);
                //TextBlock tbl = new TextBlock();                
                tbl.Text = PItem.ToolTipText;
                tbl.TextWrapping = TextWrapping.Wrap;
                //tt.Content = tbl;
                //ToolTipService.SetToolTip((Border)section.Children[1], tt);

                ((Border)section.Children[1]).DataContext = tbl;


                ((Border)section.Children[1]).MouseEnter += new MouseEventHandler(FormDesigner_MouseEnter);

                //tt.Placement = System.Windows.Controls.Primitives.PlacementMode.Mouse;
                #endregion
            }
            Grid container = (Grid)((Border)section.Children[0]).Child;

            ColumnDefinition column1 = new ColumnDefinition();
            column1.Width = new GridLength(200, GridUnitType.Auto);
            ColumnDefinition column2 = new ColumnDefinition();
            //column1.Width = new GridLength(200, GridUnitType.Auto);
            container.ColumnDefinitions.Add(column1);
            container.ColumnDefinitions.Add(column2);

            if (status == false)
            {
                Grid.SetRow(section, Form.RowDefinitions.Count - 1);
                Form.Children.Add(section);
            }

            if (PItem.FieldList != null)
                foreach (var item in PItem.FieldList)
                {
                    AddNodeItems(container, item, false);
                }

            printGrid = section;

        }

        void FormDesigner_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!p.IsOpen)
            {
                ((Border)((ScrollViewer)p.Child).Content).Child = (TextBox)((Border)sender).DataContext;
                GeneralTransform gt = ((Border)sender).TransformToVisual(Application.Current.RootVisual as UIElement);
                Point offset = gt.Transform(new Point(0, 0));
                double controlTop = offset.Y + ((Border)sender).ActualHeight;
                double controlLeft = offset.X;
                ((ScrollViewer)p.Child).MaxWidth = this.ActualWidth - controlLeft + 10;
                //((ScrollViewer)p.Child).MaxHeight = this.ActualHeight - controlTop + 10;
                ((ScrollViewer)p.Child).MaxHeight = (Application.Current.RootVisual as UIElement).DesiredSize.Height - controlTop - 10;
                p.VerticalOffset = controlTop;
                p.HorizontalOffset = controlLeft;
                p.IsOpen = true;
            }
        }



        public void AddNodeItems(Grid Container, FieldDetail PItem, bool IdDependentField)
        {
            RowDefinition Row = new RowDefinition();
            Row.Height = new GridLength(23, GridUnitType.Auto);
            Container.RowDefinitions.Add(Row);

            TextBlock FTitle = new TextBlock();
            #region new added by harish
            FTitle.Tag = PItem.DataType.Id;
            //ToolTip tt = new ToolTip();
            //TextBlock tbl = new TextBlock();
            //tbl.Text = PItem.ToolTipText;
            //tbl.TextWrapping = TextWrapping.Wrap;
            //tt.Content = tbl;


            //tt.Placement = System.Windows.Controls.Primitives.PlacementMode.Mouse;
            #endregion
            FTitle.HorizontalAlignment = HorizontalAlignment.Right;
            FTitle.VerticalAlignment = VerticalAlignment.Center;
            FTitle.Margin = new Thickness(2);
            FTitle.Text = PItem.Title + (string.IsNullOrEmpty(PItem.Title) ? "" : " : ");
            if (PItem.DataType.Id != 7)
            {
                Grid.SetRow(FTitle, Container.RowDefinitions.Count - 1);
                Container.Children.Add(FTitle);
            }            
            PItem.LabelControl = FTitle;
            switch (PItem.DataType.Id)
            {
                case 1:
                    TextBox Field = new TextBox();
                    #region Added By Harish
                    //Field.Text = ((TextFieldSetting)PItem.Settings).DefaultText;                    
                    if (PItem.IsToolTip == true)
                    {
                        //ToolTipService.SetToolTip(Field, tt);
                        #region new added by harish
                        Field.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                        #endregion
                    }
                    #endregion

                    #region Hard Coded
                    //if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features")
                    //{
                    //    Field.Name = "TxtAlarm" + Container.RowDefinitions.Count;
                    //}
                    #endregion
                    Field.LostFocus += new RoutedEventHandler(TextField_LostFocus);
                    Field.GotFocus += new RoutedEventHandler(TextField_GotFocus);
                    Field.DataContext = PItem;
                    Binding binding = new Binding("Settings.Value");
                    binding.Mode = BindingMode.TwoWay;
                    Field.SetBinding(TextBox.TextProperty, binding);
                    Field.Margin = new Thickness(2);
                    Grid.SetRow(Field, Container.RowDefinitions.Count - 1);
                    Grid.SetColumn(Field, 1);
                    if (!(((TextFieldSetting)PItem.Settings).Mode))
                    {
                        Field.AcceptsReturn = true;
                        Field.Height = 60;
                        Field.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                    }
                    if (IdDependentField)
                        FTitle.Visibility = Field.Visibility = Visibility.Collapsed;
                    PItem.Control = Field;
                    Container.Children.Add(Field);
                    break;
                case 2:
                    if ((((BooleanFieldSetting)PItem.Settings).Mode))
                    {
                        CheckBox chk = new CheckBox();
                        if (PItem.IsToolTip == true)
                        {
                            //ToolTipService.SetToolTip(chk, tt);
                            #region new added by harish
                            chk.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                            #endregion
                        }
                        #region Hard Coded
                        //if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features")
                        //{
                        //    chk.Name = "ChkAlarm" + Container.RowDefinitions.Count;
                        //}
                        #endregion
                        chk.Margin = new Thickness(2);
                        chk.Click += new RoutedEventHandler(chk_Click);
                        chk.DataContext = PItem;
                        binding = new Binding("Settings.Value");
                        binding.Mode = BindingMode.TwoWay;
                        chk.SetBinding(CheckBox.IsCheckedProperty, binding);
                        //chk.IsChecked = false;
                        //chk.IsChecked = ((BooleanFieldSetting)PItem.Settings).DefaultValue;
                        Grid.SetRow(chk, Container.RowDefinitions.Count - 1);
                        Grid.SetColumn(chk, 1);
                        PItem.Control = chk;
                        Container.Children.Add(chk);
                        if (IdDependentField)
                            FTitle.Visibility = chk.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        StackPanel panel = new StackPanel();
                        if (PItem.IsToolTip == true)
                        {
                            //ToolTipService.SetToolTip(panel, tt);
                            #region new added by harish
                            panel.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                            #endregion
                        }
                        panel.DataContext = PItem;
                        panel.Orientation = Orientation.Horizontal;
                        RadioButton yes = new RadioButton();
                        Binding byes = new Binding("Settings.Value");
                        #region Hard Coded
                        //if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Blood in Stools")
                        //{
                        //    yes.Name = "BloodStoolsYes";
                        //}
                        //if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Nausea & vomiting")
                        //{
                        //    yes.Name = "VomitingYes";
                        //}
                        #endregion
                        byes.Converter = new BoolToYesNoConverter();
                        byes.ConverterParameter = "true";
                        byes.Mode = BindingMode.TwoWay;
                        yes.SetBinding(RadioButton.IsCheckedProperty, byes);
                        yes.Margin = new Thickness(2);
                        yes.Click += new RoutedEventHandler(chk_Click);
                        //yes.IsChecked = ((BooleanFieldSetting)PItem.Settings).DefaultValue;
                        yes.Content = "Yes";
                        RadioButton No = new RadioButton();
                        #region Hard Coded
                        //if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Blood in Stools")
                        //{
                        //    No.Name = "BloodStoolsNo";
                        //}
                        //if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Nausea & vomiting")
                        //{
                        //    No.Name = "VomitingNo";
                        //}
                        #endregion
                        No.Margin = new Thickness(2);
                        No.Click += new RoutedEventHandler(chk_Click);
                        No.IsChecked = !yes.IsChecked.Value;
                        No.Content = "No";
                        Binding bno = new Binding("Settings.Value");
                        bno.Converter = new BoolToYesNoConverter();
                        bno.ConverterParameter = "false";
                        bno.Mode = BindingMode.TwoWay;
                        No.SetBinding(RadioButton.IsCheckedProperty, bno);
                        panel.Children.Add(yes);
                        panel.Children.Add(No);
                        Grid.SetRow(panel, Container.RowDefinitions.Count - 1);
                        Grid.SetColumn(panel, 1);
                        PItem.Control = panel;
                        Container.Children.Add(panel);
                        if (PItem.IsRequired)
                        {
                            yes.SetValidation(PItem.Title + " is required.");
                            yes.RaiseValidationError();
                            No.SetValidation(PItem.Title + " is required.");
                            No.RaiseValidationError();
                        }
                        if (IdDependentField)
                        {
                            FTitle.Visibility = panel.Visibility = Visibility.Collapsed;
                        }
                    }
                    break;
                case 3:
                    DatePicker dtp = new DatePicker();
                    if (PItem.IsToolTip == true)
                    {
                        //ToolTipService.SetToolTip(dtp, tt);
                        #region new added by harish
                        dtp.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                        #endregion
                    }
                    #region new added by harish
                    dtp.DataContext = PItem;
                    #endregion
                    dtp.Margin = new Thickness(2);
                    binding = new Binding("Settings.Date");
                    binding.Mode = BindingMode.TwoWay;
                    dtp.SetBinding(DatePicker.SelectedDateProperty, binding);
                    //dtp.SelectedDateChanged += new EventHandler<SelectionChangedEventArgs>(dtp_SelectedDateChanged);
                    Grid.SetRow(dtp, Container.RowDefinitions.Count - 1);
                    Grid.SetColumn(dtp, 1);
                    PItem.Control = dtp;
                    if (IdDependentField)
                        FTitle.Visibility = dtp.Visibility = Visibility.Collapsed;
                    Container.Children.Add(dtp);
                    break;
                case 4:
                    ListFieldSetting listSetting = ((ListFieldSetting)PItem.Settings);
                    switch (listSetting.ChoiceMode)
                    {
                        case SelectionMode.Single:
                            switch (listSetting.ControlType)
                            {
                                case ListControlType.ComboBox:
                                    ComboBox cmbList = new ComboBox();
                                    if (PItem.IsToolTip == true)
                                    {
                                        //ToolTipService.SetToolTip(cmbList, tt);
                                        #region new added by harish
                                        cmbList.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                                        #endregion
                                    }
                                    #region Hard Coded
                                    //if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Urine output")
                                    //{
                                    //    cmbList.Name = "UrineOutput";
                                    //}
                                    #endregion
                                    cmbList.SelectionChanged += new SelectionChangedEventHandler(cmbList_SelectionChanged);
                                    if (listSetting.SelectedItem != null)
                                        listSetting.SelectedItem = listSetting.ItemSource.Where(i => i.Title == listSetting.SelectedItem.Title).Single();
                                    else
                                    {
                                        if (listSetting.ItemSource.Count != 0)
                                            listSetting.SelectedItem = listSetting.ItemSource[0];
                                        listSetting.SelectedItem = null;
                                    }
                                    cmbList.DisplayMemberPath = "Title";
                                    cmbList.DataContext = PItem;
                                    Binding Sourcebinding = new Binding("Settings.ItemSource");
                                    Sourcebinding.Mode = BindingMode.OneWay;
                                    cmbList.SetBinding(ComboBox.ItemsSourceProperty, Sourcebinding);
                                    Binding SIbinding = new Binding("Settings.SelectedItem");
                                    SIbinding.Mode = BindingMode.TwoWay;
                                    cmbList.SetBinding(ComboBox.SelectedItemProperty, SIbinding);
                                    cmbList.Margin = new Thickness(2);
                                    Grid.SetRow(cmbList, Container.RowDefinitions.Count - 1);
                                    Grid.SetColumn(cmbList, 1);
                                    //cmbList.ItemsSource = listSetting.ItemSource;
                                    //cmbList.SelectedIndex = listSetting.DefaultSelectedItemIndex;
                                    PItem.Control = cmbList;
                                    if (PItem.IsRequired)
                                    {
                                        cmbList.SetValidation(PItem.Title + " is required.");
                                        cmbList.RaiseValidationError();
                                    }
                                    if (IdDependentField)
                                        FTitle.Visibility = cmbList.Visibility = Visibility.Visible;
                                    Container.Children.Add(cmbList);
                                    break;
                                case ListControlType.RadioButton:
                                    break;
                            }

                            break;
                        case SelectionMode.Multiples:
                            ListBox lbList = new ListBox();
                            if (PItem.IsToolTip == true)
                            {
                                //ToolTipService.SetToolTip(lbList, tt);
                                #region new added by harish
                                lbList.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                                #endregion
                            }
                            lbList.SelectionMode = System.Windows.Controls.SelectionMode.Multiple;
                            lbList.DataContext = PItem;
                            Binding Sourcebinding1 = new Binding("Settings.ItemSource");
                            Sourcebinding1.Mode = BindingMode.OneWay;
                            lbList.SetBinding(ListBox.ItemsSourceProperty, Sourcebinding1);
                            if (listSetting.SelectedItems != null && listSetting.SelectedItems.Count > 0)
                                foreach (var item in listSetting.SelectedItems)
                                {
                                    lbList.SelectedItems.Add(listSetting.ItemSource.Where(i => i.Title == item.Title).Single());
                                }
                            lbList.MaxHeight = 100;
                            lbList.DisplayMemberPath = "Title";
                            lbList.Margin = new Thickness(2);
                            lbList.SelectionChanged += new SelectionChangedEventHandler(lbList_SelectionChanged);
                            Grid.SetRow(lbList, Container.RowDefinitions.Count - 1);
                            Grid.SetColumn(lbList, 1);
                            lbList.ItemsSource = listSetting.ItemSource;
                            PItem.Control = lbList;
                            if (IdDependentField)
                                FTitle.Visibility = lbList.Visibility = Visibility.Visible;
                            Container.Children.Add(lbList);
                            break;
                    }
                    break;
                case 5:
                    StackPanel DecP = new StackPanel();
                    DecP.DataContext = PItem;
                    DecP.Orientation = Orientation.Horizontal;
                    TextBox DecField = new TextBox();
                    if (PItem.IsToolTip == true)
                    {
                        //ToolTipService.SetToolTip(DecField, tt);
                        #region new added by harish
                        DecP.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                        #endregion
                    }
                    DecField.VerticalAlignment = VerticalAlignment.Center;
                    DecField.Margin = new Thickness(2);
                    DecP.Children.Add(DecField);
                    TextBlock decUnit = new TextBlock();
                    decUnit.Margin = new Thickness(2);
                    decUnit.VerticalAlignment = VerticalAlignment.Center;
                    DecP.Children.Add(decUnit);
                    Grid.SetRow(DecP, Container.RowDefinitions.Count - 1);
                    Grid.SetColumn(DecP, 1);
                    if (PItem.Settings != null)
                    {
                        binding = new Binding("Settings.Value");
                        binding.Mode = BindingMode.TwoWay;
                        DecField.SetBinding(TextBox.TextProperty, binding);
                        DecField.Width = 50;
                        if (string.IsNullOrEmpty(DecField.Text))
                            DecField.Text = ((DecimalFieldSetting)PItem.Settings).DefaultValue.HasValue ? ((DecimalFieldSetting)PItem.Settings).DefaultValue.ToString() : "";
                        DecField.LostFocus += new RoutedEventHandler(decUnit_LostFocus);
                        DecField.TextAlignment = TextAlignment.Right;
                        decUnit.Text = string.IsNullOrEmpty(((DecimalFieldSetting)PItem.Settings).Unit) ? "" : ((DecimalFieldSetting)PItem.Settings).Unit;
                        //DecField.Text = ((DecimalFieldSetting)PItem.Settings).DefaultValue.ToString();
                        //DecField.Text = ((DecimalFieldSetting)PItem.Settings).DefaultValue.ToString();
                    }
                    if (PItem.IsRequired)
                    {
                        DecField.SetValidation(PItem.Title + " is required.");
                        DecField.RaiseValidationError();
                    }
                    if (IdDependentField)
                        FTitle.Visibility = DecP.Visibility = Visibility.Collapsed;
                    PItem.Control = DecP;
                    Container.Children.Add(DecP);
                    break;
                case 6:
                    HyperlinkButton HyperBtn = new HyperlinkButton();
                    HyperBtn.VerticalAlignment = VerticalAlignment.Center;
                    HyperBtn.IsTabStop = false;
                    #region Added By Harish
                    if (PItem.IsToolTip == true)
                    {
                        //ToolTipService.SetToolTip(HyperBtn, tt);
                        #region new added by harish
                        HyperBtn.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                        #endregion
                    }
                    #endregion
                    if (((HyperlinkFieldSetting)PItem.Settings).Url != null && ((HyperlinkFieldSetting)PItem.Settings).Url != "")
                    {
                        HyperBtn.Content = ((HyperlinkFieldSetting)PItem.Settings).Url;
                        HyperBtn.TargetName = ((HyperlinkFieldSetting)PItem.Settings).Url;
                        HyperBtn.Click += new RoutedEventHandler(HyperBtn_Click);
                    }
                    HyperBtn.DataContext = PItem;
                    PItem.Control = HyperBtn;
                    Grid.SetRow(HyperBtn, Container.RowDefinitions.Count - 1);
                    Grid.SetColumn(HyperBtn, 1);
                    Container.Children.Add(HyperBtn);
                    break;
                case 7:
                    FTitle.FontFamily = new FontFamily("Portable User Interface");
                    FTitle.Foreground = this.Resources["Heading"] as Brush;
                    FTitle.FontWeight = FontWeights.Bold;
                    FTitle.FontStyle = FontStyles.Italic;
                    FTitle.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    Grid.SetRow(FTitle, Container.RowDefinitions.Count - 1);
                    Grid.SetColumn(FTitle, 0);
                    Grid.SetColumnSpan(FTitle, 2);
                    Container.Children.Add(FTitle);
                    if (PItem.IsToolTip == true)
                    {
                        //ToolTipService.SetToolTip(FTitle, tt);
                        #region new added by harish
                        FTitle.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                        #endregion
                    }
                    FTitle.DataContext = PItem;
                    break;

                case 8:
                    LookUpFieldSetting LookUpSetting = ((LookUpFieldSetting)PItem.Settings);

                    switch (LookUpSetting.ChoiceMode)
                    {
                        case SelectionMode.Single:
                            ComboBox cmbList = new ComboBox();
                            cmbList.DisplayMemberPath = "Title";
                            cmbList.Margin = new Thickness(2);
                            //cmbList.ItemsSource = ((IGetList)(Assembly.GetExecutingAssembly().CreateInstance(LookUpSetting.SelectedSource.Value))).GetList();
                            if (!LookUpSetting.IsAlternateText)
                            {
                                if (PItem.IsToolTip == true)
                                {
                                    //ToolTipService.SetToolTip(cmbList, tt);
                                    cmbList.DataContext = PItem;
                                    #region new added by harish
                                    cmbList.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                                    #endregion
                                }
                                Grid.SetRow(cmbList, Container.RowDefinitions.Count - 1);
                                Grid.SetColumn(cmbList, 1);
                                //cmbList.SelectedIndex = listSetting.DefaultSelectedItemIndex;
                                PItem.Control = cmbList;
                                if (IdDependentField)
                                    cmbList.Visibility = Visibility.Collapsed;
                                Container.Children.Add(cmbList);
                            }
                            else
                            {
                                Grid lookupgrid = new Grid();
                                if (PItem.IsToolTip == true)
                                {
                                    //ToolTipService.SetToolTip(lookupgrid, tt);
                                    lookupgrid.DataContext = PItem;
                                    #region new added by harish
                                    lookupgrid.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                                    #endregion
                                }
                                ColumnDefinition col1 = new ColumnDefinition();
                                ColumnDefinition col2 = new ColumnDefinition();
                                ColumnDefinition col3 = new ColumnDefinition();
                                col2.Width = new GridLength(30, GridUnitType.Auto);
                                lookupgrid.ColumnDefinitions.Add(col1);
                                lookupgrid.ColumnDefinitions.Add(col2);
                                lookupgrid.ColumnDefinitions.Add(col3);

                                Grid.SetColumn(cmbList, 0);

                                TextBlock or = new TextBlock();
                                or.VerticalAlignment = VerticalAlignment.Center;
                                or.Margin = new Thickness(2);
                                or.Text = "Or";
                                Grid.SetColumn(or, 1);
                                TextBox Alt = new TextBox();
                                or.Margin = new Thickness(2);
                                Grid.SetColumn(Alt, 2);
                                //cmbList.SelectedIndex = listSetting.DefaultSelectedItemIndex;
                                Grid.SetRow(lookupgrid, Container.RowDefinitions.Count - 1);
                                Grid.SetColumn(lookupgrid, 1);

                                lookupgrid.Children.Add(cmbList);
                                lookupgrid.Children.Add(or);
                                lookupgrid.Children.Add(Alt);
                                PItem.Control = lookupgrid;
                                if (IdDependentField)
                                    FTitle.Visibility = cmbList.Visibility = Visibility.Collapsed;
                                Container.Children.Add(lookupgrid);
                            }

                            break;
                        case SelectionMode.Multiples:
                            ListBox lbList = new ListBox();
                            if (PItem.IsToolTip == true)
                            {
                                //ToolTipService.SetToolTip(lbList, tt);
                                #region new added by harish
                                lbList.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                                #endregion
                            }
                            lbList.MaxHeight = 100;
                            lbList.DisplayMemberPath = "Title";
                            lbList.SelectionMode = System.Windows.Controls.SelectionMode.Multiple;
                            lbList.Margin = new Thickness(2);
                            lbList.DataContext = PItem;
                            Grid.SetRow(lbList, Container.RowDefinitions.Count - 1);
                            Grid.SetColumn(lbList, 1);
                            lbList.ItemsSource = ((IGetList)(Assembly.GetExecutingAssembly().CreateInstance(LookUpSetting.SelectedSource.Value))).GetList();
                            PItem.Control = lbList;
                            if (IdDependentField)
                                FTitle.Visibility = lbList.Visibility = Visibility.Collapsed;
                            Container.Children.Add(lbList);
                            break;
                    }
                    break;

                case 9:
                    MedicationFieldSetting MedSetting = ((MedicationFieldSetting)PItem.Settings);
                    FTitle.VerticalAlignment = VerticalAlignment.Top;
                    FTitle.Margin = new Thickness(2, 8, 2, 0);
                    ListBox lstBox = new ListBox();
                    if (PItem.IsToolTip == true)
                    {
                        //ToolTipService.SetToolTip(lstBox, tt);
                        #region new added by harish
                        lstBox.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                        #endregion
                    }
                    lstBox.DataContext = PItem;
                    lstBox.Margin = new Thickness(2);
                    //lstBox.ItemTemplate = (DataTemplate)this.Resources["ListItemDataTemplate"];
                    //lstBox.ItemContainerStyle = (Style)this.Resources["ListBoxItemStyle"];

                    // ---------------Commented for Dynamic Drug ist
                    //for (int i = 0; i < MedSetting.ItemsSource.Count; i++)
                    //{
                    //    MedicatioRepeterControlItem mrci = new MedicatioRepeterControlItem();
                    //    mrci.OnAddRemoveClick += new RoutedEventHandler(mrci_OnAddRemoveClick);
                    //    //Medication m = new Medication() { Command = "Add", Index = lstBox.Items.Count, Parent = lstBox, DrugSource = Helpers.GetDrugList(), DosageSource = Helpers.GetDosageList(), RouteSource = Helpers.GetRouteList(), FrequencySource = Helpers.GetFrequencyList() };
                    //    MedSetting.ItemsSource[i].Index = i;
                    //    MedSetting.ItemsSource[i].Command = ((i == MedSetting.ItemsSource.Count - 1) ? "Add" : "Remove");
                    //    MedSetting.ItemsSource[i].Parent = lstBox;
                    //    MedSetting.ItemsSource[i].MedicationSetting = MedSetting;

                    //    mrci.DataContext = MedSetting.ItemsSource[i];
                    //    lstBox.Items.Add(mrci);
                    //}

                    //Grid.SetRow(lstBox, Container.RowDefinitions.Count - 1);
                    //Grid.SetColumn(lstBox, 1);
                    //if (IdDependentField)
                    //    FTitle.Visibility = lstBox.Visibility = Visibility.Collapsed;
                    //PItem.Control = lstBox;
                    //Container.Children.Add(lstBox);

                    if (MedSetting.MedicationDrugType != null)
                    {
                        clsGetDrugListBizActionVO BizAction = new clsGetDrugListBizActionVO();

                        if (MedSetting.MedicationDrugType != null)
                            BizAction.TheraputicID = ((MasterListItem)MedSetting.MedicationDrugType).ID;
                        else
                            BizAction.TheraputicID = 0L;

                        if (MedSetting.MoleculeType != null)
                            BizAction.MoleculeID = ((MasterListItem)MedSetting.MoleculeType).ID;
                        else
                            BizAction.MoleculeID = 0L;

                        if (MedSetting.GroupType != null)
                            BizAction.GroupID = ((MasterListItem)MedSetting.GroupType).ID;
                        else
                            BizAction.GroupID = 0L;

                        if (MedSetting.CategoryType != null)
                            BizAction.CategoryID = ((MasterListItem)MedSetting.CategoryType).ID;
                        else
                            BizAction.CategoryID = 0L;

                        if (MedSetting.PregnancyClass != null)
                            BizAction.PregnancyID = ((MasterListItem)MedSetting.PregnancyClass).ID;
                        else
                            BizAction.PregnancyID = 0L;

                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        client.ProcessCompleted += (s, args) =>
                        {
                            if (args.Error == null && args.Result != null)
                            {
                                List<clsDrugVO> lstSourceDrug = ((clsGetDrugListBizActionVO)args.Result).objDrugList;

                                List<MasterListItem> lstDrug = new List<MasterListItem>();
                                for (int i = 0; i < lstSourceDrug.Count; i++)
                                {
                                    lstDrug.Add(new MasterListItem() { ID = ((clsDrugVO)lstSourceDrug[i]).ID, Description = ((clsDrugVO)lstSourceDrug[i]).DrugName });
                                }
                                // Sort Drug List                
                                lstDrug = (List<MasterListItem>)(lstDrug.OrderBy(i => i.Description).ToList<MasterListItem>());

                                for (int i = 0; i < MedSetting.ItemsSource.Count; i++)
                                {
                                    ((Medication)MedSetting.ItemsSource[i]).DrugSource = lstDrug;

                                    MedicatioRepeterControlItem mrci = new MedicatioRepeterControlItem();
                                    mrci.OnAddRemoveClick += new RoutedEventHandler(mrci_OnAddRemoveClick);
                                    //Medication m = new Medication() { Command = "Add", Index = lstBox.Items.Count, Parent = lstBox, DrugSource = Helpers.GetDrugList(), DosageSource = Helpers.GetDosageList(), RouteSource = Helpers.GetRouteList(), FrequencySource = Helpers.GetFrequencyList() };
                                    MedSetting.ItemsSource[i].Index = i;
                                    MedSetting.ItemsSource[i].Command = ((i == MedSetting.ItemsSource.Count - 1) ? "Add" : "Remove");
                                    MedSetting.ItemsSource[i].Parent = lstBox;
                                    MedSetting.ItemsSource[i].MedicationSetting = MedSetting;

                                    mrci.DataContext = MedSetting.ItemsSource[i];
                                    lstBox.Items.Add(mrci);
                                }

                                //Grid.SetRow(lstBox, Container.RowDefinitions.Count - 1);
                                //Grid.SetColumn(lstBox, 1);
                                //if (IdDependentField)
                                //    FTitle.Visibility = lstBox.Visibility = Visibility.Collapsed;
                                //PItem.Control = lstBox;
                                //Container.Children.Add(lstBox);
                            }
                        };
                        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                        client.CloseAsync();
                    }
                    else
                    {
                        for (int i = 0; i < MedSetting.ItemsSource.Count; i++)
                        {
                            MedicatioRepeterControlItem mrci = new MedicatioRepeterControlItem();
                            mrci.OnAddRemoveClick += new RoutedEventHandler(mrci_OnAddRemoveClick);
                            //Medication m = new Medication() { Command = "Add", Index = lstBox.Items.Count, Parent = lstBox, DrugSource = Helpers.GetDrugList(), DosageSource = Helpers.GetDosageList(), RouteSource = Helpers.GetRouteList(), FrequencySource = Helpers.GetFrequencyList() };
                            MedSetting.ItemsSource[i].Index = i;
                            MedSetting.ItemsSource[i].Command = ((i == MedSetting.ItemsSource.Count - 1) ? "Add" : "Remove");
                            MedSetting.ItemsSource[i].Parent = lstBox;
                            MedSetting.ItemsSource[i].MedicationSetting = MedSetting;

                            mrci.DataContext = MedSetting.ItemsSource[i];
                            lstBox.Items.Add(mrci);
                        }
                        //Grid.SetRow(lstBox, Container.RowDefinitions.Count - 1);
                        //Grid.SetColumn(lstBox, 1);
                        //if (IdDependentField)
                        //    FTitle.Visibility = lstBox.Visibility = Visibility.Collapsed;
                        //PItem.Control = lstBox;
                        //Container.Children.Add(lstBox);
                    }
                    Grid.SetRow(lstBox, Container.RowDefinitions.Count - 1);
                    Grid.SetColumn(lstBox, 1);
                    if (IdDependentField)
                        FTitle.Visibility = lstBox.Visibility = Visibility.Collapsed;
                    PItem.Control = lstBox;
                    Container.Children.Add(lstBox);

                    break;

                case 10:
                    break;
                case 11:
                    OtherInvestigationFieldSetting InvestSetting = ((OtherInvestigationFieldSetting)PItem.Settings);
                    FTitle.VerticalAlignment = VerticalAlignment.Top;
                    FTitle.Margin = new Thickness(2, 8, 2, 0);
                    ListBox lstBoxInvest = new ListBox();
                    if (PItem.IsToolTip == true)
                    {
                        //ToolTipService.SetToolTip(lstBoxInvest, tt);
                        #region new added by harish
                        lstBoxInvest.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                        #endregion
                    }
                    lstBoxInvest.DataContext = PItem;
                    lstBoxInvest.Margin = new Thickness(2);
                    //lstBox.ItemTemplate = (DataTemplate)this.Resources["ListItemDataTemplate"];
                    //lstBox.ItemContainerStyle = (Style)this.Resources["ListBoxItemStyle"];
                    for (int i = 0; i < InvestSetting.ItemsSource.Count; i++)
                    {
                        InvestigationRepetorControlItem irci = new InvestigationRepetorControlItem();
                        irci.OnAddRemoveClick += new RoutedEventHandler(irci_OnAddRemoveClick);
                        //Medication m = new Medication() { Command = "Add", Index = lstBox.Items.Count, Parent = lstBox, DrugSource = Helpers.GetDrugList(), DosageSource = Helpers.GetDosageList(), RouteSource = Helpers.GetRouteList(), FrequencySource = Helpers.GetFrequencyList() };
                        InvestSetting.ItemsSource[i].Index = i;
                        InvestSetting.ItemsSource[i].Command = ((i == InvestSetting.ItemsSource.Count - 1) ? "Add" : "Remove");
                        InvestSetting.ItemsSource[i].Parent = lstBoxInvest;
                        InvestSetting.ItemsSource[i].InvestigationSetting = InvestSetting;

                        irci.DataContext = InvestSetting.ItemsSource[i];
                        lstBoxInvest.Items.Add(irci);
                    }

                    Grid.SetRow(lstBoxInvest, Container.RowDefinitions.Count - 1);
                    Grid.SetColumn(lstBoxInvest, 1);
                    if (IdDependentField)
                        FTitle.Visibility = lstBoxInvest.Visibility = Visibility.Collapsed;
                    PItem.Control = lstBoxInvest;
                    Container.Children.Add(lstBoxInvest);
                    break;

                case 12:
                    ListOfCheckBoxesFieldSetting listSetting1 = ((ListOfCheckBoxesFieldSetting)PItem.Settings);
                    //ListBox lbList1 = new ListBox();
                    ComboBox lbList1 = new ComboBox();
                    lbList1.SelectionChanged += new SelectionChangedEventHandler(lbList1_SelectionChanged);

                    if (PItem.IsToolTip == true)
                    {
                        //ToolTipService.SetToolTip(lbList1, tt);
                        #region new added by harish
                        lbList1.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                        #endregion
                    }
                    //lbList1.SelectionMode = System.Windows.Controls.SelectionMode.Multiple;
                    lbList1.DataContext = PItem;
                    //Binding Sourcebinding2 = new Binding("Settings.ItemSource");
                    //Sourcebinding2.Mode = BindingMode.OneWay;
                    //lbList1.SetBinding(ListBox.ItemsSourceProperty, Sourcebinding2);

                    int j = 0;
                    while (j < listSetting1.ItemsSource.Count)
                    {
                        CheckBox chk = new CheckBox();
                        chk.Content = listSetting1.ItemsSource[j];
                        chk.Margin = new Thickness(2);
                        //chk.Click += new RoutedEventHandler(chk_Click);
                        chk.DataContext = PItem;
                        binding = new Binding("Settings.SelectedItems[j]");
                        binding.Mode = BindingMode.TwoWay;
                        chk.SetBinding(CheckBox.IsCheckedProperty, binding);
                        lbList1.Items.Add(chk);
                        j++;
                    }

                    lbList1.MaxHeight = 100;
                    lbList1.Margin = new Thickness(2);
                    Grid.SetRow(lbList1, Container.RowDefinitions.Count - 1);
                    Grid.SetColumn(lbList1, 1);
                    PItem.Control = lbList1;
                    if (IdDependentField)
                        FTitle.Visibility = lbList1.Visibility = Visibility.Visible;
                    Container.Children.Add(lbList1);

                    #region Code for Other Nutrition TextBox
                    RowDefinition Row2 = new RowDefinition();
                    Row2.Height = new GridLength(23, GridUnitType.Auto);
                    Container.RowDefinitions.Add(Row2);

                    TextBlock FTitle1 = new TextBlock();
                    if (!listSetting1.IsOtherText)
                        FTitle1.Visibility = Visibility.Collapsed;

                    if (listSetting1.ListType == "Nutrition List")
                    {
                        FTitle1.Name = "ONTitle";
                        FTitle1.Text = "Other Nutrition";
                    }
                    if (listSetting1.ListType == "Other Alarms")
                    {
                        FTitle1.Name = "OATitle";
                        FTitle1.Text = "Other Alarms";
                    }
                    FTitle1.HorizontalAlignment = HorizontalAlignment.Right;
                    FTitle1.VerticalAlignment = VerticalAlignment.Center;
                    FTitle1.Margin = new Thickness(2);

                    Grid.SetRow(FTitle1, Container.RowDefinitions.Count - 1);
                    Container.Children.Add(FTitle1);

                    TextBox Field2 = new TextBox();
                    if (!listSetting1.IsOtherText)
                        Field2.Visibility = Visibility.Collapsed;
                    if (listSetting1.ListType == "Nutrition List")
                    {
                        Field2.Name = "ONField";
                    }
                    if (listSetting1.ListType == "Other Alarms")
                    {
                        Field2.Name = "OAField";
                    }

                    Field2.DataContext = PItem;
                    Binding binding2 = new Binding("Settings.OtherText");
                    binding2.Mode = BindingMode.TwoWay;
                    Field2.SetBinding(TextBox.TextProperty, binding2);
                    Field2.Margin = new Thickness(2);
                    Grid.SetRow(Field2, Container.RowDefinitions.Count - 1);
                    Grid.SetColumn(Field2, 1);

                    Field2.AcceptsReturn = true;
                    Field2.Height = 60;
                    Field2.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;

                    Container.Children.Add(Field2);
                    #endregion
                    break;
                case 13:
                    AutomatedListFieldSetting AutolistSetting = ((AutomatedListFieldSetting)PItem.Settings);
                    switch (AutolistSetting.ControlType)
                    {
                        case AutoListControlType.ComboBox:
                            #region Region for Auto Combo
                            ComboBox cmbList = new ComboBox();
                            if (PItem.IsToolTip == true)
                            {
                                //ToolTipService.SetToolTip(cmbList, tt);
                                #region new added by harish
                                cmbList.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                                #endregion
                            }
                            cmbList.SelectionChanged += new SelectionChangedEventHandler(AutoComboList_SelectionChanged);
                            //if (AutolistSetting.SelectedItem != null)
                            //    listSetting.SelectedItem = listSetting.ItemSource.Where(i => i.Title == listSetting.SelectedItem.Title).Single();
                            //else
                            //{
                            //    if (listSetting.ItemSource.Count != 0)
                            //        listSetting.SelectedItem = listSetting.ItemSource[0];
                            //    listSetting.SelectedItem = null;
                            //}
                            cmbList.DisplayMemberPath = "Description";
                            cmbList.SelectedValuePath = "ID";
                            cmbList.DataContext = PItem;
                            if (AutolistSetting.IsDynamic == true)
                            {
                                clsGetMasterListByTableNameAndColumnNameBizActionVO BizAction = new clsGetMasterListByTableNameAndColumnNameBizActionVO();
                                BizAction.MasterTable = ((MasterListItem)AutolistSetting.SelectedTable).Description;
                                BizAction.ColumnName = ((MasterListItem)AutolistSetting.SelectedColumn).Description;

                                BizAction.MasterList = new List<MasterListItem>();
                                Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                                PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                                client1.ProcessCompleted += (s, args) =>
                                {
                                    if (args.Error == null && args.Result != null)
                                    {
                                        List<MasterListItem> SourceList = ((clsGetMasterListByTableNameAndColumnNameBizActionVO)args.Result).MasterList;
                                        ((AutomatedListFieldSetting)PItem.Settings).ItemSource = SourceList;
                                        Binding Sourcebinding = new Binding("Settings.ItemSource");
                                        Sourcebinding.Mode = BindingMode.OneWay;
                                        cmbList.SetBinding(ComboBox.ItemsSourceProperty, Sourcebinding);
                                        Binding SIbinding = new Binding("Settings.SelectedItem");
                                        SIbinding.Mode = BindingMode.TwoWay;
                                        cmbList.SetBinding(ComboBox.SelectedItemProperty, SIbinding);
                                    }
                                };
                                client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                                client1.CloseAsync();
                            }
                            else
                            {
                                Binding Sourcebinding = new Binding("Settings.ItemSource");
                                Sourcebinding.Mode = BindingMode.OneWay;
                                cmbList.SetBinding(ComboBox.ItemsSourceProperty, Sourcebinding);
                                Binding SIbinding = new Binding("Settings.SelectedItem");
                                SIbinding.Mode = BindingMode.TwoWay;
                                cmbList.SetBinding(ComboBox.SelectedItemProperty, SIbinding);
                            }
                            cmbList.Margin = new Thickness(2);
                            Grid.SetRow(cmbList, Container.RowDefinitions.Count - 1);
                            Grid.SetColumn(cmbList, 1);
                            //cmbList.ItemsSource = listSetting.ItemSource;
                            //cmbList.SelectedIndex = listSetting.DefaultSelectedItemIndex;
                            PItem.Control = cmbList;
                            if (PItem.IsRequired)
                            {
                                cmbList.SetValidation(PItem.Title + " is required.");
                                cmbList.RaiseValidationError();
                            }
                            if (IdDependentField)
                                FTitle.Visibility = cmbList.Visibility = Visibility.Visible;
                            Container.Children.Add(cmbList);
                            #endregion
                            break;

                        case AutoListControlType.ListBox:
                            switch (AutolistSetting.ChoiceMode)
                            {
                                case SelectionMode.Single:
                                    //Region for Auto List (SelectionMode-Single)
                                    #region Region for Auto List (SelectionMode-Single)
                                    ListBox lbListSingle = new ListBox();
                                    if (PItem.IsToolTip == true)
                                    {
                                        //ToolTipService.SetToolTip(lbList, tt);
                                        #region new added by harish
                                        lbListSingle.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                                        #endregion
                                    }
                                    lbListSingle.SelectionChanged += new SelectionChangedEventHandler(AutoComboList_SelectionChanged);
                                    lbListSingle.SelectionMode = System.Windows.Controls.SelectionMode.Single;
                                    lbListSingle.DataContext = PItem;
                                    if (AutolistSetting.IsDynamic == true)
                                    {
                                        clsGetMasterListByTableNameAndColumnNameBizActionVO BizAction = new clsGetMasterListByTableNameAndColumnNameBizActionVO();
                                        BizAction.MasterTable = ((MasterListItem)AutolistSetting.SelectedTable).Description;
                                        BizAction.ColumnName = ((MasterListItem)AutolistSetting.SelectedColumn).Description;

                                        BizAction.MasterList = new List<MasterListItem>();
                                        Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                                        PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                                        client1.ProcessCompleted += (s, args) =>
                                        {
                                            if (args.Error == null && args.Result != null)
                                            {
                                                List<MasterListItem> SourceList = ((clsGetMasterListByTableNameAndColumnNameBizActionVO)args.Result).MasterList;
                                                ((AutomatedListFieldSetting)PItem.Settings).ItemSource = SourceList;
                                                Binding Sourcebinding1 = new Binding("Settings.ItemSource");
                                                Sourcebinding1.Mode = BindingMode.OneWay;
                                                lbListSingle.SetBinding(ListBox.ItemsSourceProperty, Sourcebinding1);
                                                Binding SIbinding1 = new Binding("Settings.SelectedItem");
                                                SIbinding1.Mode = BindingMode.TwoWay;
                                                lbListSingle.SetBinding(ListBox.SelectedItemProperty, SIbinding1);
                                            }
                                        };
                                        client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                                        client1.CloseAsync();
                                    }
                                    else
                                    {
                                        Binding Sourcebinding1 = new Binding("Settings.ItemSource");
                                        Sourcebinding1.Mode = BindingMode.OneWay;
                                        lbListSingle.SetBinding(ListBox.ItemsSourceProperty, Sourcebinding1);
                                        Binding SIbinding1 = new Binding("Settings.SelectedItem");
                                        SIbinding1.Mode = BindingMode.TwoWay;
                                        lbListSingle.SetBinding(ListBox.SelectedItemProperty, SIbinding1);
                                    }
                                    lbListSingle.MaxHeight = 100;
                                    lbListSingle.DisplayMemberPath = "Description";
                                    lbListSingle.SelectedValuePath = "ID";
                                    lbListSingle.Margin = new Thickness(2);
                                    //lbList.SelectionChanged += new SelectionChangedEventHandler(lbAutoList_SelectionChanged);
                                    Grid.SetRow(lbListSingle, Container.RowDefinitions.Count - 1);
                                    Grid.SetColumn(lbListSingle, 1);
                                    lbListSingle.ItemsSource = AutolistSetting.ItemSource;
                                    if (AutolistSetting.SelectedItem != null)
                                        lbListSingle.SelectedValue = AutolistSetting.SelectedItem.ID;
                                    PItem.Control = lbListSingle;
                                    if (IdDependentField)
                                        FTitle.Visibility = lbListSingle.Visibility = Visibility.Visible;
                                    Container.Children.Add(lbListSingle);
                                    #endregion
                                    break;
                                case SelectionMode.Multiples:
                                    //Region for Auto List (SelectionMode-Multiple)
                                    #region Region for Auto List (SelectionMode-Multiple)
                                    ListBox lbList = new ListBox();
                                    if (PItem.IsToolTip == true)
                                    {
                                        //ToolTipService.SetToolTip(lbList, tt);
                                        #region new added by harish
                                        lbList.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                                        #endregion
                                    }
                                    lbList.SelectionMode = System.Windows.Controls.SelectionMode.Multiple;
                                    lbList.DataContext = PItem;
                                    if (AutolistSetting.IsDynamic == true)
                                    {
                                        clsGetMasterListByTableNameAndColumnNameBizActionVO BizAction = new clsGetMasterListByTableNameAndColumnNameBizActionVO();
                                        BizAction.MasterTable = ((MasterListItem)AutolistSetting.SelectedTable).Description;
                                        BizAction.ColumnName = ((MasterListItem)AutolistSetting.SelectedColumn).Description;

                                        BizAction.MasterList = new List<MasterListItem>();
                                        Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                                        PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                                        client1.ProcessCompleted += (s, args) =>
                                        {
                                            if (args.Error == null && args.Result != null)
                                            {
                                                List<MasterListItem> SourceList = ((clsGetMasterListByTableNameAndColumnNameBizActionVO)args.Result).MasterList;
                                                ((AutomatedListFieldSetting)PItem.Settings).ItemSource = SourceList;

                                                Binding Sourcebinding2 = new Binding("Settings.ItemSource");
                                                Sourcebinding2.Mode = BindingMode.OneWay;
                                                lbList.SetBinding(ListBox.ItemsSourceProperty, Sourcebinding2);
                                                if (AutolistSetting.SelectedItems != null && AutolistSetting.SelectedItems.Count > 0)
                                                    foreach (var item in AutolistSetting.SelectedItems)
                                                    {
                                                        lbList.SelectedItems.Add(AutolistSetting.ItemSource.Where(i => i.ID == item.ID).Single());
                                                    }
                                            }
                                        };
                                        client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                                        client1.CloseAsync();
                                    }
                                    else
                                    {
                                        Binding Sourcebinding2 = new Binding("Settings.ItemSource");
                                        Sourcebinding2.Mode = BindingMode.OneWay;
                                        lbList.SetBinding(ListBox.ItemsSourceProperty, Sourcebinding2);
                                        if (AutolistSetting.SelectedItems != null && AutolistSetting.SelectedItems.Count > 0)
                                            foreach (var item in AutolistSetting.SelectedItems)
                                            {
                                                lbList.SelectedItems.Add(AutolistSetting.ItemSource.Where(i => i.ID == item.ID).Single());
                                            }
                                    }
                                    lbList.MaxHeight = 100;
                                    lbList.DisplayMemberPath = "Description";
                                    lbList.SelectedValuePath = "ID";
                                    lbList.Margin = new Thickness(2);
                                    lbList.SelectionChanged += new SelectionChangedEventHandler(lbAutoList_SelectionChanged);
                                    Grid.SetRow(lbList, Container.RowDefinitions.Count - 1);
                                    Grid.SetColumn(lbList, 1);
                                    lbList.ItemsSource = AutolistSetting.ItemSource;
                                    PItem.Control = lbList;
                                    if (IdDependentField)
                                        FTitle.Visibility = lbList.Visibility = Visibility.Visible;
                                    Container.Children.Add(lbList);
                                    #endregion
                                    break;
                            }
                            break;
                        case AutoListControlType.CheckListBox:
                            FTitle.VerticalAlignment = VerticalAlignment.Top;
                            FTitle.Margin = new Thickness(2, 8, 2, 0);
                            ListBox CheckListBox = new ListBox();
                            CheckListBox.MaxHeight = 100;
                            if (PItem.IsToolTip == true)
                            {
                                //ToolTipService.SetToolTip(lstBox, tt);
                                #region new added by harish
                                CheckListBox.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                                #endregion
                            }
                            CheckListBox.DataContext = PItem;
                            CheckListBox.Margin = new Thickness(2);
                            if (AutolistSetting.IsDynamic == true)
                            {                                
                                clsGetMasterListByTableNameAndColumnNameBizActionVO BizAction = new clsGetMasterListByTableNameAndColumnNameBizActionVO();
                                BizAction.MasterTable = ((MasterListItem)AutolistSetting.SelectedTable).Description;
                                BizAction.ColumnName = ((MasterListItem)AutolistSetting.SelectedColumn).Description;

                                BizAction.MasterList = new List<MasterListItem>();
                                Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                                PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                                client1.ProcessCompleted += (s, args) =>
                                {
                                    if (args.Error == null && args.Result != null)
                                    {
                                        List<MasterListItem> SourceList = ((clsGetMasterListByTableNameAndColumnNameBizActionVO)args.Result).MasterList;
                                        ((AutomatedListFieldSetting)PItem.Settings).ItemSource = SourceList;
                                        for (int k = 0; k < AutolistSetting.ItemSource.Count; k++)
                                        {
                                            CheckListBoxControlItem CLBCI = new CheckListBoxControlItem();
                                            CLBCI.chkItemClicked += new RoutedEventHandler(CLBCI_chkItemClicked);
                                            CLBCI.DataContext = AutolistSetting.ItemSource[k];
                                            CLBCI.Tag = PItem;
                                            CheckListBox.Items.Add(CLBCI);
                                        }
                                    }
                                };
                                client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                                client1.CloseAsync();                                
                            }
                            else
                            {
                                for (int k = 0; k < AutolistSetting.ItemSource.Count; k++)
                                {
                                    CheckListBoxControlItem CLBCI = new CheckListBoxControlItem();
                                    CLBCI.chkItemClicked += new RoutedEventHandler(CLBCI_chkItemClicked);
                                    CLBCI.DataContext = AutolistSetting.ItemSource[k];
                                    CLBCI.Tag = PItem;
                                    CheckListBox.Items.Add(CLBCI);
                                }
                            }
                            Grid.SetRow(CheckListBox, Container.RowDefinitions.Count - 1);
                            Grid.SetColumn(CheckListBox, 1);
                            if (IdDependentField)
                                FTitle.Visibility = CheckListBox.Visibility = Visibility.Collapsed;
                            PItem.Control = CheckListBox;
                            Container.Children.Add(CheckListBox);
                            break;
                    }                    
                    break;
                case 14:
                    OtherMedicationFieldSetting OtherMedSetting = ((OtherMedicationFieldSetting)PItem.Settings);
                    FTitle.VerticalAlignment = VerticalAlignment.Top;
                    FTitle.Margin = new Thickness(2, 8, 2, 0);
                    ListBox OtherMedlstBox = new ListBox();
                    if (PItem.IsToolTip == true)
                    {
                        //ToolTipService.SetToolTip(lstBox, tt);
                        #region new added by harish
                        OtherMedlstBox.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                        #endregion
                    }
                    OtherMedlstBox.DataContext = PItem;
                    OtherMedlstBox.Margin = new Thickness(2);


                    for (int i = 0; i < OtherMedSetting.ItemsSource.Count; i++)
                    {
                        OtherMedicatioRepeterControlItem Otmrci = new OtherMedicatioRepeterControlItem();
                        Otmrci.OnAddRemoveClick += new RoutedEventHandler(Otmrci_OnAddRemoveClick);
                        //Medication m = new Medication() { Command = "Add", Index = lstBox.Items.Count, Parent = lstBox, DrugSource = Helpers.GetDrugList(), DosageSource = Helpers.GetDosageList(), RouteSource = Helpers.GetRouteList(), FrequencySource = Helpers.GetFrequencyList() };
                        OtherMedSetting.ItemsSource[i].Index = i;
                        OtherMedSetting.ItemsSource[i].Command = ((i == OtherMedSetting.ItemsSource.Count - 1) ? "Add" : "Remove");
                        OtherMedSetting.ItemsSource[i].Parent = OtherMedlstBox;
                        OtherMedSetting.ItemsSource[i].MedicationSetting = OtherMedSetting;

                        Otmrci.DataContext = OtherMedSetting.ItemsSource[i];
                        OtherMedlstBox.Items.Add(Otmrci);
                    }
                    Grid.SetRow(OtherMedlstBox, Container.RowDefinitions.Count - 1);
                    Grid.SetColumn(OtherMedlstBox, 1);
                    if (IdDependentField)
                        FTitle.Visibility = OtherMedlstBox.Visibility = Visibility.Collapsed;
                    PItem.Control = OtherMedlstBox;
                    Container.Children.Add(OtherMedlstBox);
                    break;
                case 15:
                    InvestigationFieldSetting InvestAutolistSetting = ((InvestigationFieldSetting)PItem.Settings);
                    switch (InvestAutolistSetting.ControlType)
                    {
                        case AutoListControlType.ComboBox:
                            #region Region for Auto Combo
                            ComboBox cmbList = new ComboBox();
                            if (PItem.IsToolTip == true)
                            {
                                //ToolTipService.SetToolTip(cmbList, tt);
                                #region new added by harish
                                cmbList.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                                #endregion
                            }
                            cmbList.SelectionChanged += new SelectionChangedEventHandler(InvestAutoComboList_SelectionChanged);

                            cmbList.DisplayMemberPath = "Description";
                            cmbList.SelectedValuePath = "ID";
                            cmbList.DataContext = PItem;
                            Binding Sourcebinding = new Binding("Settings.ItemSource");
                            Sourcebinding.Mode = BindingMode.OneWay;
                            cmbList.SetBinding(ComboBox.ItemsSourceProperty, Sourcebinding);
                            Binding SIbinding = new Binding("Settings.SelectedItem");
                            SIbinding.Mode = BindingMode.TwoWay;
                            cmbList.SetBinding(ComboBox.SelectedItemProperty, SIbinding);
                            cmbList.Margin = new Thickness(2);
                            Grid.SetRow(cmbList, Container.RowDefinitions.Count - 1);
                            Grid.SetColumn(cmbList, 1);
                            //cmbList.ItemsSource = listSetting.ItemSource;
                            //cmbList.SelectedIndex = listSetting.DefaultSelectedItemIndex;
                            PItem.Control = cmbList;
                            if (PItem.IsRequired)
                            {
                                cmbList.SetValidation(PItem.Title + " is required.");
                                cmbList.RaiseValidationError();
                            }
                            if (IdDependentField)
                                FTitle.Visibility = cmbList.Visibility = Visibility.Visible;
                            Container.Children.Add(cmbList);
                            #endregion
                            break;

                        case AutoListControlType.ListBox:
                            switch (InvestAutolistSetting.ChoiceMode)
                            {
                                case SelectionMode.Single:
                                    //Region for Auto List (SelectionMode-Single)
                                    #region Region for Auto List (SelectionMode-Single)
                                    ListBox lbListSingle = new ListBox();
                                    if (PItem.IsToolTip == true)
                                    {
                                        //ToolTipService.SetToolTip(lbList, tt);
                                        #region new added by harish
                                        lbListSingle.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                                        #endregion
                                    }
                                    lbListSingle.SelectionChanged += new SelectionChangedEventHandler(InvestAutoComboList_SelectionChanged);
                                    lbListSingle.SelectionMode = System.Windows.Controls.SelectionMode.Single;
                                    lbListSingle.DataContext = PItem;
                                    Binding Sourcebinding1 = new Binding("Settings.ItemSource");
                                    Sourcebinding1.Mode = BindingMode.OneWay;
                                    lbListSingle.SetBinding(ListBox.ItemsSourceProperty, Sourcebinding1);
                                    Binding SIbinding1 = new Binding("Settings.SelectedItem");
                                    SIbinding1.Mode = BindingMode.TwoWay;
                                    lbListSingle.SetBinding(ListBox.SelectedItemProperty, SIbinding1);

                                    lbListSingle.MaxHeight = 100;
                                    lbListSingle.DisplayMemberPath = "Description";
                                    lbListSingle.SelectedValuePath = "ID";
                                    lbListSingle.Margin = new Thickness(2);
                                    //lbList.SelectionChanged += new SelectionChangedEventHandler(lbAutoList_SelectionChanged);
                                    Grid.SetRow(lbListSingle, Container.RowDefinitions.Count - 1);
                                    Grid.SetColumn(lbListSingle, 1);
                                    lbListSingle.ItemsSource = InvestAutolistSetting.ItemSource;
                                    if (InvestAutolistSetting.SelectedItem != null)
                                        lbListSingle.SelectedValue = InvestAutolistSetting.SelectedItem.ID;
                                    PItem.Control = lbListSingle;
                                    if (IdDependentField)
                                        FTitle.Visibility = lbListSingle.Visibility = Visibility.Visible;
                                    Container.Children.Add(lbListSingle);
                                    #endregion
                                    break;
                                case SelectionMode.Multiples:
                                    //Region for Auto List (SelectionMode-Multiple)
                                    #region Region for Auto List (SelectionMode-Multiple)
                                    ListBox lbList = new ListBox();
                                    if (PItem.IsToolTip == true)
                                    {
                                        //ToolTipService.SetToolTip(lbList, tt);
                                        #region new added by harish
                                        lbList.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                                        #endregion
                                    }
                                    lbList.SelectionMode = System.Windows.Controls.SelectionMode.Multiple;
                                    lbList.DataContext = PItem;
                                    Binding Sourcebinding2 = new Binding("Settings.ItemSource");
                                    Sourcebinding2.Mode = BindingMode.OneWay;
                                    lbList.SetBinding(ListBox.ItemsSourceProperty, Sourcebinding2);
                                    if (InvestAutolistSetting.SelectedItems != null && InvestAutolistSetting.SelectedItems.Count > 0)
                                        foreach (var item in InvestAutolistSetting.SelectedItems)
                                        {
                                            lbList.SelectedItems.Add(InvestAutolistSetting.ItemSource.Where(i => i.ID == item.ID).Single());
                                        }
                                    lbList.MaxHeight = 100;
                                    lbList.DisplayMemberPath = "Description";
                                    lbList.SelectedValuePath = "ID";
                                    lbList.Margin = new Thickness(2);
                                    lbList.SelectionChanged += new SelectionChangedEventHandler(lbInvestAutoList_SelectionChanged);
                                    Grid.SetRow(lbList, Container.RowDefinitions.Count - 1);
                                    Grid.SetColumn(lbList, 1);
                                    lbList.ItemsSource = InvestAutolistSetting.ItemSource;
                                    PItem.Control = lbList;
                                    if (IdDependentField)
                                        FTitle.Visibility = lbList.Visibility = Visibility.Visible;
                                    Container.Children.Add(lbList);
                                    #endregion
                                    break;
                            }
                            break;
                        case AutoListControlType.CheckListBox:
                            FTitle.VerticalAlignment = VerticalAlignment.Top;
                            FTitle.Margin = new Thickness(2, 8, 2, 0);
                            ListBox CheckListBox = new ListBox();
                            CheckListBox.MaxHeight = 100;
                            if (PItem.IsToolTip == true)
                            {
                                //ToolTipService.SetToolTip(lstBox, tt);
                                #region new added by harish
                                CheckListBox.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                                #endregion
                            }
                            CheckListBox.DataContext = PItem;
                            CheckListBox.Margin = new Thickness(2);
                            for (int k = 0; k < InvestAutolistSetting.ItemSource.Count; k++)
                            {
                                CheckListBoxControlItem CLBCI = new CheckListBoxControlItem();
                                CLBCI.chkItemClicked += new RoutedEventHandler(InvestCLBCI_chkItemClicked);
                                CLBCI.DataContext = InvestAutolistSetting.ItemSource[k];
                                CLBCI.Tag = PItem;
                                CheckListBox.Items.Add(CLBCI);
                            }
                            Grid.SetRow(CheckListBox, Container.RowDefinitions.Count - 1);
                            Grid.SetColumn(CheckListBox, 1);
                            if (IdDependentField)
                                FTitle.Visibility = CheckListBox.Visibility = Visibility.Collapsed;
                            PItem.Control = CheckListBox;
                            Container.Children.Add(CheckListBox);
                            break;
                    }
                    break;
                case 16:
                    TimePicker TP = new TimePicker();                    
                    if (PItem.IsToolTip == true)
                    {
                        //ToolTipService.SetToolTip(dtp, tt);
                        #region new added by harish
                        TP.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                        #endregion
                    }
                    #region new added by harish
                    TP.DataContext = PItem;
                    #endregion
                    TP.Margin = new Thickness(2);
                    binding = new Binding("Settings.Time");
                    binding.Mode = BindingMode.TwoWay;
                    TP.SetBinding(TimePicker.ValueProperty, binding);
                    //dtp.SelectedDateChanged += new EventHandler<SelectionChangedEventArgs>(dtp_SelectedDateChanged);
                    Grid.SetRow(TP, Container.RowDefinitions.Count - 1);
                    Grid.SetColumn(TP, 1);
                    PItem.Control = TP;
                    if (IdDependentField)
                        FTitle.Visibility = TP.Visibility = Visibility.Collapsed;
                    Container.Children.Add(TP);
                    break;
                case 17:
                    FileUploadFieldSetting FUSetting = ((FileUploadFieldSetting)PItem.Settings);
                    FTitle.VerticalAlignment = VerticalAlignment.Top;
                    FTitle.Margin = new Thickness(2, 8, 2, 0);
                    ListBox FUlstBox = new ListBox();
                    if (PItem.IsToolTip == true)
                    {
                        //ToolTipService.SetToolTip(lstBox, tt);
                        #region new added by harish
                        FUlstBox.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                        #endregion
                    }
                    FUlstBox.DataContext = PItem;
                    FUlstBox.Margin = new Thickness(2);

                    for (int i = 0; i < FUSetting.ItemsSource.Count; i++)
                    {
                        FileUploadRepeterControlItem FUrci = new FileUploadRepeterControlItem();
                        FUrci.OnAddRemoveClick += new RoutedEventHandler(FUrci_OnAddRemoveClick);

                        //Medication m = new Medication() { Command = "Add", Index = lstBox.Items.Count, Parent = lstBox, DrugSource = Helpers.GetDrugList(), DosageSource = Helpers.GetDosageList(), RouteSource = Helpers.GetRouteList(), FrequencySource = Helpers.GetFrequencyList() };
                        FUSetting.ItemsSource[i].Index = i;
                        FUSetting.ItemsSource[i].Command = ((i == FUSetting.ItemsSource.Count - 1) ? "Add" : "Remove");
                        FUSetting.ItemsSource[i].Parent = FUlstBox;
                        FUSetting.ItemsSource[i].FileUploadSetting = FUSetting;

                        FUrci.DataContext = FUSetting.ItemsSource[i];
                        FUlstBox.Items.Add(FUrci);
                    }
                    Grid.SetRow(FUlstBox, Container.RowDefinitions.Count - 1);
                    Grid.SetColumn(FUlstBox, 1);
                    if (IdDependentField)
                        FTitle.Visibility = FUlstBox.Visibility = Visibility.Collapsed;
                    PItem.Control = FUlstBox;
                    Container.Children.Add(FUlstBox);
                    break;
            }
            if (PItem.DependentFieldDetail != null && PItem.DependentFieldDetail.Count > 0)
                foreach (var item in PItem.DependentFieldDetail)
                {
                    AddNodeItems(Container, item, true);
                }
        }

        void FUrci_OnAddRemoveClick(object sender, RoutedEventArgs e)
        {
            var lstBox = ((FileUpload)((HyperlinkButton)sender).DataContext).Parent;
            var FUSetting = ((FileUpload)((HyperlinkButton)sender).DataContext).FileUploadSetting;
            if (((HyperlinkButton)sender).TargetName.ToString() == "Remove")
            {
                FUSetting.ItemsSource.RemoveAt(((FileUpload)((HyperlinkButton)sender).DataContext).Index);
            }
            if (((HyperlinkButton)sender).TargetName.ToString() == "Add")
            {
                FUSetting.ItemsSource.Add(new FileUpload());
            }
            lstBox.Items.Clear();
            for (int i = 0; i < FUSetting.ItemsSource.Count; i++)
            {
                FileUploadRepeterControlItem FUrci = new FileUploadRepeterControlItem();
                FUrci.OnAddRemoveClick +=new RoutedEventHandler(FUrci_OnAddRemoveClick);

                FUSetting.ItemsSource[i].Index = i;
                FUSetting.ItemsSource[i].Command = ((i == FUSetting.ItemsSource.Count - 1) ? "Add" : "Remove");
                FUSetting.ItemsSource[i].Parent = lstBox;
                FUSetting.ItemsSource[i].FileUploadSetting = FUSetting;
                FUrci.DataContext = FUSetting.ItemsSource[i];
                lstBox.Items.Add(FUrci);
            }
        }

        void DesignAutoList(FieldDetail PItem)
        {
            //AutomatedListFieldSetting AutolistSetting = ((AutomatedListFieldSetting)PItem.Settings);
            //clsGetMasterListByTableNameAndColumnNameBizActionVO BizAction = new clsGetMasterListByTableNameAndColumnNameBizActionVO();
            //BizAction.MasterTable = ((MasterListItem)AutolistSetting.SelectedTable).Description;
            //BizAction.ColumnName = ((MasterListItem)AutolistSetting.SelectedColumn).Description;

            //BizAction.MasterList = new List<MasterListItem>();
            //Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            //PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
            //client1.ProcessCompleted += (s, args) =>
            //{
            //    if (args.Error == null && args.Result != null)
            //    {
            //        List<MasterListItem> SourceList = ((clsGetMasterListByTableNameAndColumnNameBizActionVO)args.Result).MasterList;
            //        ((AutomatedListFieldSetting)PItem.Settings).ItemSource = SourceList;                    
            //    }
            //};
            //client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //client1.CloseAsync();
        }

        void Otmrci_OnAddRemoveClick(object sender, RoutedEventArgs e)
        {
            var lstBox = ((OtherMedication)((HyperlinkButton)sender).DataContext).Parent;
            var MedSetting = ((OtherMedication)((HyperlinkButton)sender).DataContext).MedicationSetting;
            if (((HyperlinkButton)sender).TargetName.ToString() == "Remove")
            {
                MedSetting.ItemsSource.RemoveAt(((OtherMedication)((HyperlinkButton)sender).DataContext).Index);
            }

            if (((HyperlinkButton)sender).TargetName.ToString() == "Add")
            {
                MedSetting.ItemsSource.Add(new OtherMedication() { RouteSource = Helpers.GetRouteList() });
            }
            lstBox.Items.Clear();
            for (int i = 0; i < MedSetting.ItemsSource.Count; i++)
            {
                OtherMedicatioRepeterControlItem mrci = new OtherMedicatioRepeterControlItem();
                mrci.OnAddRemoveClick += new RoutedEventHandler(Otmrci_OnAddRemoveClick);
                //Medication m = new Medication() { Command = "Add", Index = lstBox.Items.Count, Parent = lstBox, DrugSource = Helpers.GetDrugList(), DosageSource = Helpers.GetDosageList(), RouteSource = Helpers.GetRouteList(), FrequencySource = Helpers.GetFrequencyList() };
                MedSetting.ItemsSource[i].Index = i;
                MedSetting.ItemsSource[i].Command = ((i == MedSetting.ItemsSource.Count - 1) ? "Add" : "Remove");
                MedSetting.ItemsSource[i].Parent = lstBox;
                MedSetting.ItemsSource[i].MedicationSetting = MedSetting;
                mrci.DataContext = MedSetting.ItemsSource[i];
                lstBox.Items.Add(mrci);
            }
        }

        void CLBCI_chkItemClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((FrameworkElement)(((FieldDetail)(((Control)sender).Tag))).Control).Visibility == Visibility.Visible)
                {
                    if (((FieldDetail)(((Control)sender).Tag)).RelationalFieldList != null)
                        foreach (var item in ((FieldDetail)(((Control)sender).Tag)).RelationalFieldList)
                        {
                            if (item.Control is FrameworkElement)
                            {
                                switch (((CheckListExpression<bool>)item.RelationCondition).Operation)
                                {
                                    case ComboOperations.EqualTo:
                                        try
                                        {
                                            if (((AutomatedListFieldSetting)((FieldDetail)((Control)sender).Tag).Settings).ItemSource != null && ((AutomatedListFieldSetting)((FieldDetail)((Control)sender).Tag).Settings).ItemSource.First(f => (f.ID == ((CheckListExpression<bool>)item.RelationCondition).SelectedItem.ID && f.Status == true)) != null)
                                            {
                                                ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                        }

                                        break;
                                    case ComboOperations.NotEqualTo:
                                        try
                                        {
                                            if (((AutomatedListFieldSetting)((FieldDetail)((Control)sender).Tag).Settings).ItemSource != null && ((AutomatedListFieldSetting)((FieldDetail)((Control)sender).Tag).Settings).ItemSource.First(f => (f.ID == ((CheckListExpression<bool>)item.RelationCondition).SelectedItem.ID && f.Status == true)) != null)
                                            {
                                                ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                        }

                                        break;
                                }

                                if (item.DependentFieldDetail != null && item.DependentFieldDetail.Count > 0)
                                    CheckChildElements(item, ((FrameworkElement)item.Control).Visibility == Visibility.Visible ? true : false);
                            }
                        }
                }

            }
            catch (Exception ex)
            {

                //throw;
            }
        }

        void InvestCLBCI_chkItemClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((FrameworkElement)(((FieldDetail)(((Control)sender).Tag))).Control).Visibility == Visibility.Visible)
                {
                    if (((FieldDetail)(((Control)sender).Tag)).RelationalFieldList != null)
                        foreach (var item in ((FieldDetail)(((Control)sender).Tag)).RelationalFieldList)
                        {
                            if (item.Control is FrameworkElement)
                            {
                                switch (((CheckListExpression<bool>)item.RelationCondition).Operation)
                                {
                                    case ComboOperations.EqualTo:
                                        try
                                        {
                                            if (((InvestigationFieldSetting)((FieldDetail)((Control)sender).Tag).Settings).ItemSource != null && ((InvestigationFieldSetting)((FieldDetail)((Control)sender).Tag).Settings).ItemSource.First(f => (f.ID == ((CheckListExpression<bool>)item.RelationCondition).SelectedItem.ID && f.Status == true)) != null)
                                            {
                                                ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                        }

                                        break;
                                    case ComboOperations.NotEqualTo:
                                        try
                                        {
                                            if (((InvestigationFieldSetting)((FieldDetail)((Control)sender).Tag).Settings).ItemSource != null && ((InvestigationFieldSetting)((FieldDetail)((Control)sender).Tag).Settings).ItemSource.First(f => (f.ID == ((CheckListExpression<bool>)item.RelationCondition).SelectedItem.ID && f.Status == true)) != null)
                                            {
                                                ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                        }

                                        break;
                                }

                                if (item.DependentFieldDetail != null && item.DependentFieldDetail.Count > 0)
                                    CheckChildElements(item, ((FrameworkElement)item.Control).Visibility == Visibility.Visible ? true : false);
                            }
                        }
                }

            }
            catch (Exception ex)
            {

                //throw;
            }
        }

        void Field_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!pf.IsOpen)
            {
                //TextBlock tbl = new TextBlock();
                TextBox tbl = new TextBox();
                tbl.IsEnabled = false;
                //tbl.Background = new SolidColorBrush(Colors.Yellow);
                tbl.Text = ((FieldDetail)((FrameworkElement)sender).DataContext).ToolTipText;
                tbl.TextWrapping = TextWrapping.Wrap;

                ((Border)((ScrollViewer)pf.Child).Content).Child = null;
                ((Border)((ScrollViewer)pf.Child).Content).Child = tbl;
                GeneralTransform gt = ((FrameworkElement)sender).TransformToVisual(Application.Current.RootVisual as UIElement);
                Point offset = gt.Transform(new Point(0, 0));
                double controlTop = offset.Y + ((FrameworkElement)sender).ActualHeight;
                double controlLeft = offset.X;
                ((ScrollViewer)pf.Child).MaxWidth = this.ActualWidth - controlLeft + 10;
                //((ScrollViewer)pf.Child).MaxHeight = this.ActualHeight - controlTop + 10;
                ((ScrollViewer)pf.Child).MaxHeight = (Application.Current.RootVisual as UIElement).DesiredSize.Height - controlTop;
                pf.VerticalOffset = controlTop;
                pf.HorizontalOffset = controlLeft;
                pf.IsOpen = true;
            }
        }

        void lbList1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //throw new NotImplementedException();
            ((ComboBox)sender).SelectedItem = null;
        }

        void irci_OnAddRemoveClick(object sender, RoutedEventArgs e)
        {
            var lstBoxInvest = ((OtherInvestigation)((HyperlinkButton)sender).DataContext).Parent;
            var InvestSetting = ((OtherInvestigation)((HyperlinkButton)sender).DataContext).InvestigationSetting;
            if (((HyperlinkButton)sender).TargetName.ToString() == "Remove")
            {
                InvestSetting.ItemsSource.RemoveAt(((OtherInvestigation)((HyperlinkButton)sender).DataContext).Index);
            }

            if (((HyperlinkButton)sender).TargetName.ToString() == "Add")
            {
                InvestSetting.ItemsSource.Add(new OtherInvestigation() { InvestigationSource = Helpers.GetInvestigationList() });
            }

            lstBoxInvest.Items.Clear();

            for (int i = 0; i < InvestSetting.ItemsSource.Count; i++)
            {
                InvestigationRepetorControlItem irci = new InvestigationRepetorControlItem();
                irci.OnAddRemoveClick += new RoutedEventHandler(irci_OnAddRemoveClick);
                //Medication m = new Medication() { Command = "Add", Index = lstBox.Items.Count, Parent = lstBox, DrugSource = Helpers.GetDrugList(), DosageSource = Helpers.GetDosageList(), RouteSource = Helpers.GetRouteList(), FrequencySource = Helpers.GetFrequencyList() };
                InvestSetting.ItemsSource[i].Index = i;
                InvestSetting.ItemsSource[i].Command = ((i == InvestSetting.ItemsSource.Count - 1) ? "Add" : "Remove");
                InvestSetting.ItemsSource[i].Parent = lstBoxInvest;
                InvestSetting.ItemsSource[i].InvestigationSetting = InvestSetting;
                irci.DataContext = InvestSetting.ItemsSource[i];
                lstBoxInvest.Items.Add(irci);
            }
        }

        void dtp_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //throw new NotImplementedException();
            string s = ((DatePicker)sender).SelectedDate.ToString();
        }

        void HyperBtn_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();  
            HtmlPopupWindowOptions op = new HtmlPopupWindowOptions();
            op.Toolbar = false;
            op.Status = false;
            op.Directories = false;
            op.Height = 1200;
            op.Width = 1400;
            op.Resizeable = true;
            //http://localhost:1186/Images/Sangita9Sep.xps


            //string file = System.IO.Path.Combine(App.FilePath, ((HyperlinkButton)sender).TargetName);

            if (((HyperlinkButton)sender).TargetName == "Razi_Case Referral Sheet_31_08_2010.doc")
            {

            }
            else
            {
                //HtmlPage.PopupWindow(new Uri(file), "_blank", op);
                HtmlPage.Window.Invoke("alertText", new string[] { ((HyperlinkButton)sender).TargetName });
            }
            //HtmlPage.Window.Invoke("open", new object[] { "../Images/" + ((HyperlinkButton)sender).TargetName});
        }


        void cmbList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //throw new NotImplementedException();

            try
            {
                if (((FrameworkElement)(((FieldDetail)(((Control)sender).DataContext))).Control).Visibility == Visibility.Visible)
                {
                    #region Dependent
                    //foreach (var item in ((FieldDetail)(((Control)sender).DataContext)).DependentFieldDetail)
                    //{
                    //    if (item.Control is FrameworkElement)
                    //    {
                    //        switch (((BooleanExpression<bool>)item.Condition).Operation)
                    //        {
                    //            case BooleanOperations.EqualTo:
                    //                if (((BooleanExpression<bool>)item.Condition).ReferenceValue == ((BooleanFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).Value)
                    //                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                    //                else
                    //                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                    //                break;
                    //            case BooleanOperations.NotEqualTo:
                    //                if (((BooleanExpression<bool>)item.Condition).ReferenceValue != ((BooleanFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).Value)
                    //                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                    //                else
                    //                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                    //                break;
                    //        }

                    //        if (item.DependentFieldDetail != null && item.DependentFieldDetail.Count > 0)
                    //            CheckChildElements(item, ((FrameworkElement)item.Control).Visibility == Visibility.Visible ? true : false);
                    //    }
                    //}
                    #endregion

                    if (((FieldDetail)(((Control)sender).DataContext)).RelationalFieldList != null)
                        foreach (var item in ((FieldDetail)(((Control)sender).DataContext)).RelationalFieldList)
                        {
                            if (item.Control is FrameworkElement)
                            {
                                switch (((ComboExpression<bool>)item.RelationCondition).Operation)
                                {
                                    case ComboOperations.EqualTo:
                                        if (((ComboExpression<bool>)item.RelationCondition).SelectedItem == ((ListFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItem.Title)
                                        {
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                        }
                                        else
                                        {
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                        }
                                        break;
                                    case ComboOperations.NotEqualTo:
                                        if (((ComboExpression<bool>)item.RelationCondition).SelectedItem != ((ListFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItem.Title)
                                        {
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                        }
                                        else
                                        {
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                        }
                                        break;
                                }


                                if (item.DependentFieldDetail != null && item.DependentFieldDetail.Count > 0)
                                    CheckChildElements(item, ((FrameworkElement)item.Control).Visibility == Visibility.Visible ? true : false);
                            }
                        }
                }

            }
            catch (Exception ex)
            {

                //throw;
            }
        }


        //AutoCombo + AutoList(Single Selection Mode) Selection and Relation management
        void AutoComboList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (((FrameworkElement)(((FieldDetail)(((Control)sender).DataContext))).Control).Visibility == Visibility.Visible)
                {
                    if (((FieldDetail)(((Control)sender).DataContext)).RelationalFieldList != null)
                        foreach (var item in ((FieldDetail)(((Control)sender).DataContext)).RelationalFieldList)
                        {
                            if (item.Control is FrameworkElement)
                            {
                                switch (((CheckListExpression<bool>)item.RelationCondition).Operation)
                                {
                                    case ComboOperations.EqualTo:
                                        if (((AutomatedListFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItem != null && ((CheckListExpression<bool>)item.RelationCondition).SelectedItem.ID == ((AutomatedListFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItem.ID)
                                        {
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                        }
                                        else
                                        {
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                        }
                                        break;
                                    case ComboOperations.NotEqualTo:
                                        if (((AutomatedListFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItem != null && ((CheckListExpression<bool>)item.RelationCondition).SelectedItem.ID == ((AutomatedListFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItem.ID)
                                        {
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                        }
                                        else
                                        {
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                        }
                                        break;
                                }

                                if (item.DependentFieldDetail != null && item.DependentFieldDetail.Count > 0)
                                    CheckChildElements(item, ((FrameworkElement)item.Control).Visibility == Visibility.Visible ? true : false);
                            }
                        }
                }

            }
            catch (Exception ex)
            {

                //throw;
            }
        }

        void InvestAutoComboList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (((FrameworkElement)(((FieldDetail)(((Control)sender).DataContext))).Control).Visibility == Visibility.Visible)
                {
                    if (((FieldDetail)(((Control)sender).DataContext)).RelationalFieldList != null)
                        foreach (var item in ((FieldDetail)(((Control)sender).DataContext)).RelationalFieldList)
                        {
                            if (item.Control is FrameworkElement)
                            {
                                switch (((CheckListExpression<bool>)item.RelationCondition).Operation)
                                {
                                    case ComboOperations.EqualTo:
                                        if (((InvestigationFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItem != null && ((CheckListExpression<bool>)item.RelationCondition).SelectedItem.ID == ((InvestigationFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItem.ID)
                                        {
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                        }
                                        else
                                        {
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                        }
                                        break;
                                    case ComboOperations.NotEqualTo:
                                        if (((InvestigationFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItem != null && ((CheckListExpression<bool>)item.RelationCondition).SelectedItem.ID == ((InvestigationFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItem.ID)
                                        {
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                        }
                                        else
                                        {
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                        }
                                        break;
                                }

                                if (item.DependentFieldDetail != null && item.DependentFieldDetail.Count > 0)
                                    CheckChildElements(item, ((FrameworkElement)item.Control).Visibility == Visibility.Visible ? true : false);
                            }
                        }
                }

            }
            catch (Exception ex)
            {

                //throw;
            }
        }

        void lbList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender != null)
            {
                FieldDetail field = (FieldDetail)((ListBox)sender).DataContext;
                if (((ListFieldSetting)field.Settings).SelectedItems == null)
                    ((ListFieldSetting)field.Settings).SelectedItems = new List<DynamicListItem>();
                ((ListFieldSetting)field.Settings).SelectedItems.Clear();
                foreach (var item in ((ListBox)sender).SelectedItems)
                {
                    ((ListFieldSetting)field.Settings).SelectedItems.Add((DynamicListItem)item);
                }
            }
        }

        // Selection and Relation management for AutoList MultiSelect mode
        void lbAutoList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender != null)
            {
                FieldDetail field = (FieldDetail)((ListBox)sender).DataContext;
                if (((AutomatedListFieldSetting)field.Settings).SelectedItems == null)
                    ((AutomatedListFieldSetting)field.Settings).SelectedItems = new List<MasterListItem>();
                ((AutomatedListFieldSetting)field.Settings).SelectedItems.Clear();
                foreach (var item in ((ListBox)sender).SelectedItems)
                {
                    ((AutomatedListFieldSetting)field.Settings).SelectedItems.Add((MasterListItem)item);
                }

                try
                {
                    if (((FrameworkElement)(((FieldDetail)(((Control)sender).DataContext))).Control).Visibility == Visibility.Visible)
                    {
                        if (((FieldDetail)(((Control)sender).DataContext)).RelationalFieldList != null)
                            foreach (var item in ((FieldDetail)(((Control)sender).DataContext)).RelationalFieldList)
                            {
                                if (item.Control is FrameworkElement)
                                {
                                    switch (((CheckListExpression<bool>)item.RelationCondition).Operation)
                                    {
                                        case ComboOperations.EqualTo:
                                            try
                                            {
                                                if (((AutomatedListFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItems != null && ((AutomatedListFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItems.First(f => (f.ID == ((CheckListExpression<bool>)item.RelationCondition).SelectedItem.ID)) != null)
                                                {
                                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                            }
                                            //if(((AutomatedListFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItems.Contains(((CheckListExpression<bool>)item.RelationCondition).SelectedItem))
                                            //{
                                            //    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                            //}
                                            //else
                                            //{
                                            //    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                            //}

                                            break;
                                        case ComboOperations.NotEqualTo:
                                            try
                                            {
                                                if (((AutomatedListFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItems != null && ((AutomatedListFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItems.First(f => (f.ID == ((CheckListExpression<bool>)item.RelationCondition).SelectedItem.ID)) != null)
                                                {
                                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                            }
                                            //if (!(((AutomatedListFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItems.Contains(((CheckListExpression<bool>)item.RelationCondition).SelectedItem)))
                                            //{
                                            //    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                            //}
                                            //else
                                            //{
                                            //    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                            //}                                            
                                            break;
                                    }

                                    if (item.DependentFieldDetail != null && item.DependentFieldDetail.Count > 0)
                                        CheckChildElements(item, ((FrameworkElement)item.Control).Visibility == Visibility.Visible ? true : false);
                                }
                            }
                    }

                }
                catch (Exception ex)
                {

                    //throw;
                }
            }
        }

        void lbInvestAutoList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender != null)
            {
                FieldDetail field = (FieldDetail)((ListBox)sender).DataContext;
                if (((InvestigationFieldSetting)field.Settings).SelectedItems == null)
                    ((InvestigationFieldSetting)field.Settings).SelectedItems = new List<MasterListItem>();
                ((InvestigationFieldSetting)field.Settings).SelectedItems.Clear();
                foreach (var item in ((ListBox)sender).SelectedItems)
                {
                    ((InvestigationFieldSetting)field.Settings).SelectedItems.Add((MasterListItem)item);
                }

                try
                {
                    if (((FrameworkElement)(((FieldDetail)(((Control)sender).DataContext))).Control).Visibility == Visibility.Visible)
                    {
                        if (((FieldDetail)(((Control)sender).DataContext)).RelationalFieldList != null)
                            foreach (var item in ((FieldDetail)(((Control)sender).DataContext)).RelationalFieldList)
                            {
                                if (item.Control is FrameworkElement)
                                {
                                    switch (((CheckListExpression<bool>)item.RelationCondition).Operation)
                                    {
                                        case ComboOperations.EqualTo:
                                            try
                                            {
                                                if (((InvestigationFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItems != null && ((InvestigationFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItems.First(f => (f.ID == ((CheckListExpression<bool>)item.RelationCondition).SelectedItem.ID)) != null)
                                                {
                                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                            }
                                            //if(((AutomatedListFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItems.Contains(((CheckListExpression<bool>)item.RelationCondition).SelectedItem))
                                            //{
                                            //    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                            //}
                                            //else
                                            //{
                                            //    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                            //}

                                            break;
                                        case ComboOperations.NotEqualTo:
                                            try
                                            {
                                                if (((InvestigationFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItems != null && ((InvestigationFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItems.First(f => (f.ID == ((CheckListExpression<bool>)item.RelationCondition).SelectedItem.ID)) != null)
                                                {
                                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                            }
                                            //if (!(((AutomatedListFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItems.Contains(((CheckListExpression<bool>)item.RelationCondition).SelectedItem)))
                                            //{
                                            //    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                            //}
                                            //else
                                            //{
                                            //    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                            //}                                            
                                            break;
                                    }

                                    if (item.DependentFieldDetail != null && item.DependentFieldDetail.Count > 0)
                                        CheckChildElements(item, ((FrameworkElement)item.Control).Visibility == Visibility.Visible ? true : false);
                                }
                            }
                    }

                }
                catch (Exception ex)
                {

                    //throw;
                }
            }
        }

        void TextField_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((TextBox)sender) != null && ((TextBox)sender).DataContext != null)
                {
                    if (((FieldDetail)((TextBox)sender).DataContext).IsRequired)
                    {
                        ((TextBox)sender).SetValidation(((FieldDetail)((TextBox)sender).DataContext).Title + " required.");
                        ((TextBox)sender).RaiseValidationError();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        void TextField_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((TextBox)sender) != null && ((TextBox)sender).DataContext != null)
                {
                    if (((FieldDetail)((TextBox)sender).DataContext).IsRequired && string.IsNullOrEmpty(((TextBox)sender).Text))
                    {
                        ((TextBox)sender).SetValidation(((FieldDetail)((TextBox)sender).DataContext).Title + " required.");
                        ((TextBox)sender).RaiseValidationError();
                    }
                    else
                    {
                        ((TextBox)sender).ClearValidationError();
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        void decUnit_LostFocus(object sender, RoutedEventArgs e)
        {

            log = "Starting....";
            try
            {
                if (sender is TextBox && ((FrameworkElement)(((FieldDetail)(((TextBox)sender).DataContext))).Control).Visibility == Visibility.Visible)
                {
                    log += ("\r" + ((FieldDetail)(((TextBox)sender).DataContext)).Title);
                    foreach (var item in ((FieldDetail)(((TextBox)sender).DataContext)).DependentFieldDetail)
                    {
                        log += ("\r" + item.Title);
                        if (item.Control is FrameworkElement)
                        {
                            if (((TextBox)sender).Text == "")
                            {
                                ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                            }
                            else
                            {
                                switch (((DecimalExpression<decimal>)item.Condition).Operation)
                                {
                                    case DoubleOperations.EqualTo:
                                        if (((DecimalExpression<decimal>)item.Condition).ReferenceValue == decimal.Parse(((TextBox)sender).Text))
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                        else
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                        break;
                                    case DoubleOperations.NotEqualTo:
                                        if (((DecimalExpression<decimal>)item.Condition).ReferenceValue != decimal.Parse(((TextBox)sender).Text))
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                        else
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                        break;
                                    case DoubleOperations.GreterThan:
                                        if (((DecimalExpression<decimal>)item.Condition).ReferenceValue < decimal.Parse(((TextBox)sender).Text))
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                        else
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                        break;

                                    case DoubleOperations.GreterThanEqualTo:
                                        if (((DecimalExpression<decimal>)item.Condition).ReferenceValue <= decimal.Parse(((TextBox)sender).Text))
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                        else
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                        break;
                                    case DoubleOperations.LessThan:
                                        if (((DecimalExpression<decimal>)item.Condition).ReferenceValue > decimal.Parse(((TextBox)sender).Text))
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                        else
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                        break;
                                    case DoubleOperations.LessThanEqualTo:
                                        if (((DecimalExpression<decimal>)item.Condition).ReferenceValue >= decimal.Parse(((TextBox)sender).Text))
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                        else
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                        break;
                                }
                            }


                            log += ((FrameworkElement)item.LabelControl).Visibility == Visibility.Collapsed ? Visibility.Collapsed.ToString() : Visibility.Visible.ToString();

                            if (item.DependentFieldDetail != null && item.DependentFieldDetail.Count > 0)
                                CheckChildElements(item, ((FrameworkElement)item.Control).Visibility == Visibility.Visible ? true : false);
                        }
                    }

                    if (((FieldDetail)(((TextBox)sender).DataContext)).RelationalFieldList != null)
                        foreach (var item in ((FieldDetail)(((TextBox)sender).DataContext)).RelationalFieldList)
                        {
                            log += ("\r" + item.Title);
                            if (item.Control is FrameworkElement)
                            {
                                if (((TextBox)sender).Text == "")
                                {
                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                }
                                else
                                {
                                    switch (((DecimalExpression<decimal>)item.RelationCondition).Operation)
                                    {
                                        case DoubleOperations.EqualTo:
                                            if (((DecimalExpression<decimal>)item.RelationCondition).ReferenceValue == decimal.Parse(((TextBox)sender).Text))
                                                ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                            else
                                                ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                            break;
                                        case DoubleOperations.NotEqualTo:
                                            if (((DecimalExpression<decimal>)item.RelationCondition).ReferenceValue != decimal.Parse(((TextBox)sender).Text))
                                                ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                            else
                                                ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                            break;
                                        case DoubleOperations.GreterThan:
                                            if (((DecimalExpression<decimal>)item.RelationCondition).ReferenceValue < decimal.Parse(((TextBox)sender).Text))
                                                ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                            else
                                                ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                            break;

                                        case DoubleOperations.GreterThanEqualTo:
                                            if (((DecimalExpression<decimal>)item.RelationCondition).ReferenceValue <= decimal.Parse(((TextBox)sender).Text))
                                                ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                            else
                                                ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                            break;
                                        case DoubleOperations.LessThan:
                                            if (((DecimalExpression<decimal>)item.RelationCondition).ReferenceValue > decimal.Parse(((TextBox)sender).Text))
                                                ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                            else
                                                ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                            break;
                                        case DoubleOperations.LessThanEqualTo:
                                            if (((DecimalExpression<decimal>)item.RelationCondition).ReferenceValue >= decimal.Parse(((TextBox)sender).Text))
                                                ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                            else
                                                ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                            break;
                                    }
                                }

                                log += ((FrameworkElement)item.LabelControl).Visibility == Visibility.Collapsed ? Visibility.Collapsed.ToString() : Visibility.Visible.ToString();

                                if (item.DependentFieldDetail != null && item.DependentFieldDetail.Count > 0)
                                    CheckChildElements(item, ((FrameworkElement)item.Control).Visibility == Visibility.Visible ? true : false);
                            }
                        }
                }
            }
            catch (Exception ex)
            {

                //throw;
            }

        }

        private void CheckChildElements(FieldDetail pitem, bool Override)
        {
            if (!Override)
            {
                foreach (var item in pitem.DependentFieldDetail)
                {
                    if (item.Control is FrameworkElement)
                    {
                        ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                    }
                    if (item.DependentFieldDetail != null && item.DependentFieldDetail.Count > 0)
                        CheckChildElements(item, Override);
                }
            }
            else
            {
                CheckChildWithOverRide(pitem, Override);

            }
        }

        private void CheckChildWithOverRide(FieldDetail pitem, bool Override)
        {
            foreach (var item in pitem.DependentFieldDetail)
            {
                if (item.Control is FrameworkElement)
                {
                    if (pitem.Settings is DecimalFieldSetting)
                    {
                        switch (((DecimalExpression<decimal>)item.Condition).Operation)
                        {
                            case DoubleOperations.EqualTo:
                                if (((DecimalExpression<decimal>)item.Condition).ReferenceValue == ((DecimalFieldSetting)pitem.Settings).Value)
                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                else
                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                break;
                            case DoubleOperations.NotEqualTo:
                                if (((DecimalExpression<decimal>)item.Condition).ReferenceValue != ((DecimalFieldSetting)pitem.Settings).Value)
                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                else
                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                break;
                            case DoubleOperations.GreterThan:
                                if (((DecimalExpression<decimal>)item.Condition).ReferenceValue < ((DecimalFieldSetting)pitem.Settings).Value)
                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                else
                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                break;

                            case DoubleOperations.GreterThanEqualTo:
                                if (((DecimalExpression<decimal>)item.Condition).ReferenceValue <= ((DecimalFieldSetting)pitem.Settings).Value)
                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                else
                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                break;
                            case DoubleOperations.LessThan:
                                if (((DecimalExpression<decimal>)item.Condition).ReferenceValue > ((DecimalFieldSetting)pitem.Settings).Value)
                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                else
                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                break;
                            case DoubleOperations.LessThanEqualTo:
                                if (((DecimalExpression<decimal>)item.Condition).ReferenceValue >= ((DecimalFieldSetting)pitem.Settings).Value)
                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                else
                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                break;
                        }
                    }

                    if (pitem.Settings is BooleanFieldSetting)
                    {
                        switch (((BooleanExpression<bool>)item.Condition).Operation)
                        {
                            case BooleanOperations.EqualTo:
                                if ((bool)((BooleanExpression<bool>)item.Condition).ReferenceValue == ((BooleanFieldSetting)pitem.Settings).Mode)
                                    ((TextBox)item.Control).Visibility = Visibility.Visible;
                                else
                                    ((TextBox)item.Control).Visibility = Visibility.Collapsed;
                                break;
                            case BooleanOperations.NotEqualTo:
                                if ((bool)((BooleanExpression<bool>)item.Condition).ReferenceValue != ((BooleanFieldSetting)pitem.Settings).Mode)
                                    ((TextBox)item.Control).Visibility = Visibility.Visible;
                                else
                                    ((TextBox)item.Control).Visibility = Visibility.Collapsed;
                                break;
                        }
                    }

                    if (item.DependentFieldDetail != null && item.DependentFieldDetail.Count > 0)
                        CheckChildElements(item, Override);

                }
            }
        }

        void mrci_OnAddRemoveClick(object sender, RoutedEventArgs e)
        {

            var lstBox = ((Medication)((HyperlinkButton)sender).DataContext).Parent;
            var MedSetting = ((Medication)((HyperlinkButton)sender).DataContext).MedicationSetting;
            if (((HyperlinkButton)sender).TargetName.ToString() == "Remove")
            {
                MedSetting.ItemsSource.RemoveAt(((Medication)((HyperlinkButton)sender).DataContext).Index);
            }

            if (((HyperlinkButton)sender).TargetName.ToString() == "Add")
            {
                MedSetting.ItemsSource.Add(new Medication() { DrugSource = MedSetting.ItemsSource[0].DrugSource, DaySource = Helpers.GetDayList(), QuantitySource = Helpers.GetDayList(), DosageSource = Helpers.GetDosageList(), RouteSource = Helpers.GetRouteList(), FrequencySource = Helpers.GetFrequencyList() });
                //switch ((string)MedSetting.MedicationDrugType)
                //{
                //    case "Antibiotics":
                //        //MedSetting.MedicationDrugType = "Antibiotics";
                //        MedSetting.ItemsSource.Add(new Medication() { DrugSource = Helpers.GetAntibioticsList(), DosageSource = Helpers.GetDosageList(), RouteSource = Helpers.GetRouteList(), FrequencySource = Helpers.GetFrequencyList() });
                //        break;

                //    case "Antiemetics":
                //        //medSet.MedicationDrugType = "Antiemetics";
                //        MedSetting.ItemsSource.Add(new Medication() { DrugSource = Helpers.GetAntiemeticsList(), DosageSource = Helpers.GetDosageList(), RouteSource = Helpers.GetRouteList(), FrequencySource = Helpers.GetFrequencyList() });
                //        break;

                //    case "Antipyretic":
                //        //medSet.MedicationDrugType = "Antipyretic";
                //        MedSetting.ItemsSource.Add(new Medication() { DrugSource = Helpers.GetAntipyreticList(), DosageSource = Helpers.GetDosageList(), RouteSource = Helpers.GetRouteList(), FrequencySource = Helpers.GetFrequencyList() });
                //        break;

                //    case "Antispasmodic":
                //        //medSet.MedicationDrugType = "Antispasmodic";
                //        MedSetting.ItemsSource.Add(new Medication() { DrugSource = Helpers.GetAntispasmodicList(), DosageSource = Helpers.GetDosageList(), RouteSource = Helpers.GetRouteList(), FrequencySource = Helpers.GetFrequencyList() });
                //        break;
                //}
                //MedSetting.ItemsSource.Add(new Medication() { DrugSource = Helpers.GetDrugList(), DosageSource = Helpers.GetDosageList(), RouteSource = Helpers.GetRouteList(), FrequencySource = Helpers.GetFrequencyList() });
            }
            lstBox.Items.Clear();
            for (int i = 0; i < MedSetting.ItemsSource.Count; i++)
            {
                MedicatioRepeterControlItem mrci = new MedicatioRepeterControlItem();
                mrci.OnAddRemoveClick += new RoutedEventHandler(mrci_OnAddRemoveClick);
                //Medication m = new Medication() { Command = "Add", Index = lstBox.Items.Count, Parent = lstBox, DrugSource = Helpers.GetDrugList(), DosageSource = Helpers.GetDosageList(), RouteSource = Helpers.GetRouteList(), FrequencySource = Helpers.GetFrequencyList() };
                MedSetting.ItemsSource[i].Index = i;
                MedSetting.ItemsSource[i].Command = ((i == MedSetting.ItemsSource.Count - 1) ? "Add" : "Remove");
                MedSetting.ItemsSource[i].Parent = lstBox;
                MedSetting.ItemsSource[i].MedicationSetting = MedSetting;
                mrci.DataContext = MedSetting.ItemsSource[i];
                lstBox.Items.Add(mrci);
            }
        }

        void chk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((FrameworkElement)(((FieldDetail)(((Control)sender).DataContext))).Control).Visibility == Visibility.Visible)
                {
                    foreach (var item in ((FieldDetail)(((Control)sender).DataContext)).DependentFieldDetail)
                    {
                        if (item.Control is FrameworkElement)
                        {
                            switch (((BooleanExpression<bool>)item.Condition).Operation)
                            {
                                case BooleanOperations.EqualTo:
                                    if (((BooleanExpression<bool>)item.Condition).ReferenceValue == ((BooleanFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).Value)
                                        ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                    else
                                        ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                    break;
                                case BooleanOperations.NotEqualTo:
                                    if (((BooleanExpression<bool>)item.Condition).ReferenceValue != ((BooleanFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).Value)
                                        ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                    else
                                        ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                    break;
                            }

                            if (item.DependentFieldDetail != null && item.DependentFieldDetail.Count > 0)
                                CheckChildElements(item, ((FrameworkElement)item.Control).Visibility == Visibility.Visible ? true : false);
                        }
                    }

                    if (((FieldDetail)(((Control)sender).DataContext)).RelationalFieldList != null)
                        foreach (var item in ((FieldDetail)(((Control)sender).DataContext)).RelationalFieldList)
                        {
                            if (item.Control is FrameworkElement)
                            {
                                switch (((BooleanExpression<bool>)item.RelationCondition).Operation)
                                {
                                    case BooleanOperations.EqualTo:
                                        if (((BooleanExpression<bool>)item.RelationCondition).ReferenceValue == ((BooleanFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).Value)
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                        else
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                        break;
                                    case BooleanOperations.NotEqualTo:
                                        if (((BooleanExpression<bool>)item.RelationCondition).ReferenceValue != ((BooleanFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).Value)
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                        else
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                        break;
                                }


                                if (item.DependentFieldDetail != null && item.DependentFieldDetail.Count > 0)
                                    CheckChildElements(item, ((FrameworkElement)item.Control).Visibility == Visibility.Visible ? true : false);
                            }
                        }
                }

            }
            catch (Exception ex)
            {

                //throw;
            }
        }

        private Grid GetSectionLayout(string Title)
        {
            Grid OverLayGrid = new Grid();
            OverLayGrid.Tag = Title;
            OverLayGrid.Margin = new Thickness(0, 0, 0, 5);
            Border ContentBorder = new Border();
            ContentBorder.BorderBrush = (Brush)this.Resources["BorderDefault"];
            ContentBorder.BorderThickness = new Thickness(1);
            ContentBorder.CornerRadius = new CornerRadius(5);
            ContentBorder.Padding = new Thickness(5);
            ContentBorder.Margin = new Thickness(0, 8, 0, 0);
            Grid ContentGrid = new Grid();
            ContentGrid.Margin = new Thickness(5, 15, 5, 5);
            ContentBorder.Child = ContentGrid;
            OverLayGrid.Children.Add(ContentBorder);
            Border groupBorder = new Border();
            groupBorder.HorizontalAlignment = HorizontalAlignment.Left;
            groupBorder.Margin = new Thickness(8, 0, 0, 0);
            groupBorder.VerticalAlignment = VerticalAlignment.Top;
            groupBorder.Background = (Brush)this.Resources["BackgroundDefault"];
            groupBorder.BorderBrush = (Brush)this.Resources["BorderDefault"];
            groupBorder.BorderThickness = new Thickness(1);
            groupBorder.CornerRadius = new CornerRadius(5);
            groupBorder.RenderTransformOrigin = new Point(0.5, 0.5);
            TransformGroup groupBorderTransform = new TransformGroup();
            var st = new ScaleTransform();
            st.ScaleY = 0.994;
            var skt = new SkewTransform();
            skt.AngleX = -20;
            var rt = new RotateTransform();
            var tt = new TranslateTransform();
            tt.X = 3.627;
            tt.Y = 0.063;
            groupBorderTransform.Children.Add(st);
            groupBorderTransform.Children.Add(skt);
            groupBorderTransform.Children.Add(rt);
            groupBorderTransform.Children.Add(tt);
            groupBorder.RenderTransform = groupBorderTransform;
            TextBlock tbl = new TextBlock();
            tbl.Text = Title;
            tbl.FontFamily = new FontFamily("Portable User Interface");
            tbl.Margin = new Thickness(10, 1, 10, 1);
            tbl.Foreground = this.Resources["Heading"] as Brush;
            tbl.FontWeight = FontWeights.Bold;
            groupBorder.Child = tbl;
            OverLayGrid.Children.Add(groupBorder);
            return OverLayGrid;

        }

        private void SaveTemplateItemButton_Click(object sender, RoutedEventArgs e)
        {
            string test = this.SelectedFormStructure.XmlSerilze();
            string title = this.SelectedFormStructure.Title;
            string desc = this.SelectedFormStructure.Description;
            bool isphysicalexam = this.SelectedFormStructure.IsPhysicalExam;
            bool ISForOT = this.SelectedFormStructure.IsForOT;
            long TemplateTypeID = this.SelectedFormStructure.TemplateTypeID;
            string TemplateType = this.SelectedFormStructure.TemplateType;
            long TemplateSubtypeID = this.SelectedFormStructure.TemplateSubtypeID;
            string TemplateSubtype = this.SelectedFormStructure.TemplateSubtype;

            if (App.SelectedFormIndex != -1)
            {
                //App.FormTemplateList[App.SelectedFormIndex] = SelectedFormStructure;

                //Used With DataTemplate Service
                //Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                //DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                //client.UpdateTemplateCompleted += (s, args) =>
                //{
                //    //App.MainWindow.MainRegion.Content = new TemplateList();
                //};
                //App.SelectedTemplate.Title = title;
                //App.SelectedTemplate.Description = desc;
                //App.SelectedTemplate.Template = test;
                //client.UpdateTemplateAsync(App.SelectedTemplate);

                clsUpdateEMRTemplateBizActionVO BizAction = new clsUpdateEMRTemplateBizActionVO();
                App.SelectedTemplate.Title = title;
                App.SelectedTemplate.Description = desc;
                App.SelectedTemplate.Template = test;
                App.SelectedTemplate.IsPhysicalExam = isphysicalexam;
                App.SelectedTemplate.IsForOT = ISForOT;
                App.SelectedTemplate.TemplateTypeID = TemplateTypeID;
                App.SelectedTemplate.TemplateType = TemplateType;
                App.SelectedTemplate.TemplateSubtypeID = TemplateSubtypeID;
                App.SelectedTemplate.TemplateSubtype = TemplateSubtype;
                BizAction.EMRTemplateDetails = App.SelectedTemplate;

                //User Related Values
                BizAction.EMRTemplateDetails.Status = true;
                BizAction.EMRTemplateDetails.UpdatedDateTime = Convert.ToDateTime(DateTime.Now);

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null)
                    {
                        MessageBox.Show("EMR Template Updated Successfully");
                    }
                };

                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            else
            {
                //App.FormTemplateList.Add(SelectedFormStructure);

                //Used With DataTemplate Service
                //Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                //DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                //client.SaveTemplateCompleted += (s, args) =>
                //{
                //    //App.MainWindow.MainRegion.Content = new TemplateList();
                //};
                //EMRTemplate t = new EMRTemplate();
                //t.Title = SelectedFormStructure.Title;
                //t.Description = SelectedFormStructure.Description;
                //t.Template = test;

                //client.SaveTemplateAsync(t);


            }
        }

        string log = "";

        private void SerilizeTemplateItemButton_Click(object sender, RoutedEventArgs e)
        {

            ////ChildWindow1 win = new ChildWindow1();
            ////win.Log = log;
            ////win.Show();
            //string test = this.SelectedFormStructure.XmlSerilze();

            //if (App.SelectedFormIndex != -1)
            //{
            //    //App.FormTemplateList[App.SelectedFormIndex] = SelectedFormStructure;
            //    DataTemplateServiceClient client = new DataTemplateServiceClient();
            //    client.UpdateTemplateCompleted += (s, args) =>
            //    {
            //    };
            //    App.SelectedTemplate.XmlTemplate = test;
            //    client.UpdateTemplateAsync(App.SelectedTemplate);
            //}
            //else
            //{
            //    //App.FormTemplateList.Add(SelectedFormStructure);
            //    DataTemplateServiceClient client = new DataTemplateServiceClient();
            //    client.SaveTemplateCompleted += (s, args) =>
            //    {
            //    };
            //    Template t = new Template();
            //    t.Title = SelectedFormStructure.Title;
            //    t.Description = SelectedFormStructure.Description;
            //    t.XmlTemplate = test;
            //    client.SaveTemplateAsync(t);
            //}
        }

        private void RelationManageButton_Click(object sender, RoutedEventArgs e)
        {
            RelationEditor win = new RelationEditor(this.SelectedFormStructure);
            win.OnOkButtonClick += new RoutedEventHandler(win_OnOkButtonClick);
            win.OnDeleteRelClick += new RoutedEventHandler(win_OnDeleteRelClick);
            win.Show();
        }

        void win_OnDeleteRelClick(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            if (sender is IntraTemplateRelation)
            {
                this.SelectedFormStructure.Relations.Remove((IntraTemplateRelation)sender);
                GenratePreview();
                MapRelations();

                RelationEditor win = new RelationEditor(this.SelectedFormStructure);
                win.OnOkButtonClick += new RoutedEventHandler(win_OnOkButtonClick);
                win.OnDeleteRelClick += new RoutedEventHandler(win_OnDeleteRelClick);
                win.Show();
            }
        }

        void win_OnOkButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender is IntraTemplateRelation)
            {
                if (this.SelectedFormStructure.Relations == null)
                    this.SelectedFormStructure.Relations = new List<IntraTemplateRelation>();
                if (this.PreviewFormStructure.Relations == null)
                    this.PreviewFormStructure.Relations = new List<IntraTemplateRelation>();
                this.SelectedFormStructure.Relations.Add((IntraTemplateRelation)sender);
                GenratePreview();
                MapRelations();
            }

        }

        private void CloseItemButton_Click(object sender, RoutedEventArgs e)
        {
            //Used With DataTemplate Service
            //App.MainWindow.MainRegion.Content = new TemplateList();

            ((ContentControl)this.Parent).Content = new TemplateList();
        }






        //private static void _SaveToDisk(byte[] buffer, string fileName)
        //{
        //    //I
        //    //IsolatedStorageFile i = IsolatedStorageFile.GetUserStoreForApplication();

        //    using (IsolatedStorageFile iso =
        //        IsolatedStorageFile.GetUserStoreForApplication())
        //    {
        //        using (
        //            IsolatedStorageFileStream stream =
        //        new IsolatedStorageFileStream(fileName, FileMode.CreateNew,
        //                        iso))
        //        {
        //            stream.Write(buffer, 0, buffer.Length);
        //        }
        //    }
        //}

        //private static byte[] _GetSaveBuffer(WriteableBitmap bitmap)
        //{
        //    long matrixSize = bitmap.PixelWidth * bitmap.PixelHeight;

        //    long byteSize = matrixSize * 4 + 4;

        //    byte[] retVal = new byte[byteSize];

        //    long bufferPos = 0;

        //    retVal[bufferPos++] = (byte)((bitmap.PixelWidth / 256) & 0xff);
        //    retVal[bufferPos++] = (byte)((bitmap.PixelWidth % 256) & 0xff);
        //    retVal[bufferPos++] = (byte)((bitmap.PixelHeight / 256) & 0xff);
        //    retVal[bufferPos++] = (byte)((bitmap.PixelHeight % 256) & 0xff);

        //    for (int matrixPos = 0; matrixPos < matrixSize; matrixPos++)
        //    {
        //        retVal[bufferPos++] = (byte)((bitmap.Pixels[matrixPos] >> 24) & 0xff);
        //        retVal[bufferPos++] = (byte)((bitmap.Pixels[matrixPos] >> 16) & 0xff);
        //        retVal[bufferPos++] = (byte)((bitmap.Pixels[matrixPos] >> 8) & 0xff);
        //        retVal[bufferPos++] = (byte)((bitmap.Pixels[matrixPos]) & 0xff);
        //    }

        //    return retVal;
        //}

        private int itemIndex;
        private int fieldItemIndex;
        private Grid documentBodyItem;
        private bool flag = false;

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {

            itemIndex = 0;
            fieldItemIndex = 0;
            documentBodyItem = Form;

            PrintDocument document = new PrintDocument();

            document.BeginPrint += new EventHandler<BeginPrintEventArgs>(document_BeginPrint);
            document.EndPrint += new EventHandler<EndPrintEventArgs>(document_EndPrint);
            document.PrintPage += new EventHandler<PrintPageEventArgs>(document_PrintPage);
            document.Print("Template");

        }

        void document_PrintPage(object sender, PrintPageEventArgs e)
        {
            //throw new NotImplementedException();

            //GenratePreview();
            documentBodyItem = Form;

            PrintPage page = new PrintPage();
            //page.SetHeaderAndFooterText("Patient : "+(string)this.SelectedPatient.Name,"Template : "+(string)this.SelectedFormStructure.Title);

            int numberOfItemsAdded = 0;

            while (itemIndex < documentBodyItem.Children.Count)
            {
                Grid Section = (Grid)documentBodyItem.Children[itemIndex];
                Grid Container = (Grid)((Border)Section.Children[0]).Child;

                page.SetSectionHeader("Section : " + (string)Section.Tag);
                page.Measure(new Size(e.PrintableArea.Width, double.PositiveInfinity));

                if (page.DesiredSize.Height > e.PrintableArea.Height
                    && numberOfItemsAdded > 0)
                {
                    ((Grid)page.PageBody).Children.RemoveAt(((Grid)page.PageBody).Children.Count - 1);

                    e.HasMorePages = true;
                    break;
                }
                int id = 0;
                while (fieldItemIndex < Container.Children.Count)
                {
                    string field = "";
                    string fieldValue = "";

                    if ((Int32)((FrameworkElement)Container.Children[fieldItemIndex]).GetValue(Grid.ColumnProperty) % 2 == 0)
                    {
                        field = ((TextBlock)Container.Children[fieldItemIndex]).Text;
                        id = (Int32)((TextBlock)Container.Children[fieldItemIndex]).Tag;
                        page.SetFieldText(field);
                    }
                    else
                    {
                        switch (id)
                        {
                            case 1:
                                fieldValue = ((TextBox)Container.Children[fieldItemIndex]).Text;
                                break;

                            case 2:
                                if (Container.Children[fieldItemIndex] is CheckBox)
                                {
                                    fieldValue = ((CheckBox)Container.Children[fieldItemIndex]).IsChecked.ToString();
                                }
                                else if (Container.Children[fieldItemIndex] is StackPanel)
                                {
                                    if ((bool)((RadioButton)((StackPanel)Container.Children[fieldItemIndex]).Children[0]).IsChecked)
                                        fieldValue = ((bool)((RadioButton)((StackPanel)Container.Children[fieldItemIndex]).Children[0]).IsChecked).ToString();
                                    else
                                        fieldValue = bool.FalseString;
                                }
                                break;

                            case 3:
                                if (((DatePicker)Container.Children[fieldItemIndex]).SelectedDate == null)
                                    fieldValue = "Null";
                                else
                                    fieldValue = ((DateTime)((DatePicker)Container.Children[fieldItemIndex]).SelectedDate).ToString();
                                break;

                            case 4:
                                if (Container.Children[fieldItemIndex] is ComboBox)
                                {
                                    if ((DynamicListItem)((ComboBox)Container.Children[fieldItemIndex]).SelectedItem == null)
                                        fieldValue = "Null";
                                    else
                                        fieldValue = ((DynamicListItem)((ComboBox)Container.Children[fieldItemIndex]).SelectedItem).Title;
                                }
                                else if (Container.Children[fieldItemIndex] is ListBox)
                                {
                                    if ((IEnumerator<DynamicListItem>)((ListBox)Container.Children[fieldItemIndex]).SelectedItems == null)
                                        fieldValue = "Null";
                                    else
                                    {
                                        IEnumerator<DynamicListItem> list = (IEnumerator<DynamicListItem>)((ListBox)Container.Children[fieldItemIndex]).SelectedItems.GetEnumerator();
                                        while (list.MoveNext())
                                        {
                                            DynamicListItem li = (DynamicListItem)list.Current;
                                            fieldValue = fieldValue + " , " + li.Title;
                                        }
                                    }
                                }
                                break;

                            case 5:
                                fieldValue = ((TextBox)((StackPanel)Container.Children[fieldItemIndex]).Children[0]).Text + " (" + ((TextBlock)((StackPanel)Container.Children[fieldItemIndex]).Children[1]).Text + ")";
                                break;

                            case 7:

                                break;

                            case 8:
                                if (Container.Children[fieldItemIndex] is ComboBox)
                                {
                                    if ((DynamicListItem)((ComboBox)Container.Children[fieldItemIndex]).SelectedItem == null)
                                        fieldValue = "null";
                                    else
                                        fieldValue = ((DynamicListItem)((ComboBox)Container.Children[fieldItemIndex]).SelectedItem).Title;
                                }
                                else if (Container.Children[fieldItemIndex] is Grid)
                                {
                                    if ((DynamicListItem)((ComboBox)((Grid)Container.Children[fieldItemIndex]).Children[0]).SelectedItem == null)
                                        fieldValue = "Null";
                                    else
                                        fieldValue = ((DynamicListItem)((ComboBox)((Grid)Container.Children[fieldItemIndex]).Children[0]).SelectedItem).Title + "   or  " + ((TextBox)((Grid)Container.Children[fieldItemIndex]).Children[2]).Text;
                                }
                                else if (Container.Children[fieldItemIndex] is ListBox)
                                {
                                    if ((IEnumerator<DynamicListItem>)((ListBox)Container.Children[fieldItemIndex]).SelectedItems == null)
                                        fieldValue = "Null";
                                    else
                                    {
                                        IEnumerator<DynamicListItem> list = (IEnumerator<DynamicListItem>)((ListBox)Container.Children[fieldItemIndex]).SelectedItems.GetEnumerator();
                                        while (list.MoveNext())
                                        {
                                            DynamicListItem li = (DynamicListItem)list.Current;
                                            fieldValue = fieldValue + " , " + li.Title;
                                        }
                                    }
                                }
                                break;

                            case 9:

                                break;
                        }
                        page.SetFieldValue(fieldValue);
                    }
                    page.Measure(new Size(e.PrintableArea.Width, double.PositiveInfinity));
                    if (page.DesiredSize.Height > e.PrintableArea.Height
                    && numberOfItemsAdded > 0)
                    {
                        int i = (Int32)(((FrameworkElement)((Grid)page.PageBody).Children[((Grid)page.PageBody).Children.Count - 1]).GetValue(Grid.RowProperty));
                        int j = (Int32)(((FrameworkElement)((Grid)page.PageBody).Children[((Grid)page.PageBody).Children.Count - 2]).GetValue(Grid.RowProperty));

                        //if ((Int32)(((FrameworkElement)Container.Children[((Grid)page.PageBody).Children.Count - 1]).GetValue(Grid.RowProperty)) == (Int32)(((FrameworkElement)Container.Children[((Grid)page.PageBody).Children.Count - 2]).GetValue(Grid.RowProperty)))
                        //{
                        //    ((Grid)page.PageBody).Children.RemoveAt(((Grid)page.PageBody).Children.Count - 1);
                        //    ((Grid)page.PageBody).Children.RemoveAt(((Grid)page.PageBody).Children.Count - 2);
                        //}
                        if (i == j)
                        {
                            ((Grid)page.PageBody).Children.RemoveAt(((Grid)page.PageBody).Children.Count - 1);
                            ((Grid)page.PageBody).Children.RemoveAt(((Grid)page.PageBody).Children.Count - 2);
                        }
                        else
                            ((Grid)page.PageBody).Children.RemoveAt(((Grid)page.PageBody).Children.Count - 1);

                        e.HasMorePages = true;
                        flag = true;
                        break;
                    }
                    flag = false;
                    fieldItemIndex++;
                    numberOfItemsAdded++;
                }
                if (flag == true)
                {
                    flag = false;
                    break;
                }
                fieldItemIndex = 0;
                itemIndex++;
                #region Comment
                //RowDefinition r = new RowDefinition();
                //page.PageBody.RowDefinitions.Add(r);

                //Grid section = new Grid();


                //section = (Grid)documentBodyItem.Children[itemIndex];

                //documentBodyItem.Children.Remove(section);


                //Grid.SetRow(section, page.PageBody.RowDefinitions.Count - 1);
                //page.PageBody.Children.Add(section);

                //GenratePreview();
                //MapRelations();

                //page.Measure(new Size(e.PrintableArea.Width, double.PositiveInfinity));
                //itemIndex++;
                //numberOfItemsAdded++;

                //if (page.DesiredSize.Height > section.ActualHeight - 1
                //    && numberOfItemsAdded > 0 && itemIndex != documentBodyItem.Children.Count 
                //    && section.Visibility==Visibility.Visible)
                //{

                //    e.HasMorePages = true;
                //    break;
                //}
                #endregion
            }
            e.PageVisual = page;

        }

        void document_EndPrint(object sender, EndPrintEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void document_BeginPrint(object sender, BeginPrintEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void PositionMgmtButton_Click(object sender, RoutedEventArgs e)
        {
            PositionEditor win = new PositionEditor(this.SelectedFormStructure);
            win.OnSaveButtonClick += new RoutedEventHandler(win_OnSaveButtonClick);
            win.Show();
        }

        void win_OnSaveButtonClick(object sender, RoutedEventArgs e)
        {
            SelectedFormStructure = (FormDetail)sender;
            trvFormStructure.ItemsSource = null;
            List<FormDetail> list = new List<FormDetail>();
            list.Add(SelectedFormStructure);
            trvFormStructure.ItemsSource = list;
            Thread T = new Thread(m);
            T.Start();
        }

        private void ManagePCR_Click(object sender, RoutedEventArgs e)
        {
            ManagePatientCaseRecord win = new ManagePatientCaseRecord(this.SelectedFormStructure);
            win.Tag = "PCR";
            win.OnOkButtonClick += new RoutedEventHandler(win_OnPCROkButtonClick);
            win.OnDeleteRelClick += new RoutedEventHandler(win_OnPCRDeleteRelClick);
            win.Show();
        }

        void win_OnPCRDeleteRelClick(object sender, RoutedEventArgs e)
        {
            // For PCR
            if (sender is PatientCaseRecordRelation)
            {
                this.SelectedFormStructure.PCRRelations.Remove((PatientCaseRecordRelation)sender);

                ManagePatientCaseRecord win = new ManagePatientCaseRecord(this.SelectedFormStructure);
                win.Tag = "PCR";
                win.OnOkButtonClick += new RoutedEventHandler(win_OnPCROkButtonClick);
                win.OnDeleteRelClick += new RoutedEventHandler(win_OnPCRDeleteRelClick);
                win.Show();
            }

            // For Case Referral
            if (sender is PatientCaseReferralRelation)
            {
                this.SelectedFormStructure.CaseReferralRelations.Remove((PatientCaseReferralRelation)sender);

                ManagePatientCaseRecord win = new ManagePatientCaseRecord(this.SelectedFormStructure);
                win.Tag = "CaseReferral";
                win.OnOkButtonClick += new RoutedEventHandler(win_OnPCROkButtonClick);
                win.OnDeleteRelClick += new RoutedEventHandler(win_OnPCRDeleteRelClick);
                win.Show();
            }

        }

        void win_OnPCROkButtonClick(object sender, RoutedEventArgs e)
        {
            // For PCR
            if (sender is PatientCaseRecordRelation)
            {
                if (this.SelectedFormStructure.PCRRelations == null)
                    this.SelectedFormStructure.PCRRelations = new List<PatientCaseRecordRelation>();
                if (this.PreviewFormStructure.PCRRelations == null)
                    this.PreviewFormStructure.PCRRelations = new List<PatientCaseRecordRelation>();
                this.SelectedFormStructure.PCRRelations.Add((PatientCaseRecordRelation)sender);
            }

            // For Case Referral
            if (sender is PatientCaseReferralRelation)
            {
                if (this.SelectedFormStructure.CaseReferralRelations == null)
                    this.SelectedFormStructure.CaseReferralRelations = new List<PatientCaseReferralRelation>();
                if (this.PreviewFormStructure.CaseReferralRelations == null)
                    this.PreviewFormStructure.CaseReferralRelations = new List<PatientCaseReferralRelation>();
                this.SelectedFormStructure.CaseReferralRelations.Add((PatientCaseReferralRelation)sender);
            }
        }
        private void ManageReferral_Click(object sender, RoutedEventArgs e)
        {
            ManagePatientCaseRecord win = new ManagePatientCaseRecord(this.SelectedFormStructure);
            win.Tag = "CaseReferral";
            win.OnOkButtonClick += new RoutedEventHandler(win_OnPCROkButtonClick);
            win.OnDeleteRelClick += new RoutedEventHandler(win_OnPCRDeleteRelClick);
            win.Show();
        }
    }

    public class TestClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public object Salary { get; set; }
    }
}
