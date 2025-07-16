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
using DataDrivenApplication;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using PalashDynamics.ValueObjects.EMR;

namespace DataDrivenApplication
{
    public partial class App : Application
    {
        public static RaziStartPage MainWindow { get; set; }
        public static clsEMRTemplateVO SelectedTemplate { get; set; }
        public static int SelectedFormIndex { get; set; }
        public static List<FormDetail> FormTemplateList { get; set; }
        FormDetail FormStructure = null;
        public static PatientCaseRecord pcr;
        public static string strpcr { get; set; }

        public static string FilePath;
        public App()
        {
            
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();
            FormTemplateList = new List<FormDetail>();
            SelectedFormIndex = -1;
            SelectedTemplate = null;
            //pcr = new PatientCaseRecord();
        }


        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
            DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
            client.GetFilePathCompleted += (s, args) =>
            {
                App.FilePath = (string)args.Result;
            };
            client.GetFilePathAsync();
            //FormStructure = new FormDetail();
            //FormStructure.Title = "Emr Template1";
            //FormStructure.Description = "This case is prepared for dierrhoea";
            //FormStructure.SectionList = new List<SectionDetail>();
            //SectionDetail f = new SectionDetail();
            //f.Title = "Observation";
            //f.FieldList = new List<FieldDetail>();
            //FieldDetail f1 = new FieldDetail();
            //f1.Title = "Temprature";
            //f1.DataType = new FieldType() { Id = 5, Title = "Decimal", DataType = "System.String" };
            //f1.Settings = new DecimalFieldSetting() { Unit = "Degree Celcius", DefaultValue = 36 };
            //f.FieldList.Add(f1);

            //FieldDetail pulse = new FieldDetail();
            //pulse.Title = "Pulse";
            //pulse.DataType = new FieldType() { Id = 5, Title = "Decimal", DataType = "System.String" };
            //pulse.Settings = new DecimalFieldSetting();
            //f.FieldList.Add(pulse);


            //FieldDetail rpr = new FieldDetail();
            //rpr.Title = "Respiratory Rate";
            //rpr.DataType = new FieldType() { Id = 5, Title = "Decimal", DataType = "System.String" };
            //rpr.Settings = new DecimalFieldSetting();
            //f.FieldList.Add(rpr);
            //FormStructure.SectionList.Add(f);

            ////History Section
            //SectionDetail history = new SectionDetail();
            //history.Title = "History";
            //history.FieldList = new List<FieldDetail>();

            //FieldDetail pc = new FieldDetail();
            //pc.Title = "Patient complaint as reported";
            //pc.DataType = new FieldType() { Id = 1, Title = "Text", DataType = "System.String" };
            //pc.Settings = new TextFieldSetting() { Mode = true };
            //history.FieldList.Add(pc);

            //FieldDetail stoolHeader = new FieldDetail();
            //stoolHeader.Title = "Stool Details";
            //stoolHeader.DataType = new FieldType() { Id = 7 };
            //history.FieldList.Add(stoolHeader);

            //FieldDetail dursy = new FieldDetail();
            //dursy.Title = "Duration of symptoms";
            //dursy.DataType = new FieldType() { Id = 5 };
            //dursy.Settings = new DecimalFieldSetting() { Unit = "Hours" };
            //history.FieldList.Add(dursy);

            //FieldDetail dursyf = new FieldDetail();
            //dursyf.Title = "Frequency";
            //dursyf.DataType = new FieldType() { Id = 5 };
            //dursyf.Settings = new DecimalFieldSetting() { Unit = "in last 24 hrs." };
            //history.FieldList.Add(dursyf);

            //FieldDetail colc = new FieldDetail();
            //colc.Title = @"Color\Consistency";
            //colc.DataType = new FieldType() { Id = 1 };
            //colc.Settings = new TextFieldSetting() { Mode = true };
            //history.FieldList.Add(colc);

            //FieldDetail bloods = new FieldDetail();
            //bloods.Title = @"Blood is stools";
            //bloods.DataType = new FieldType() { Id = 2 };
            //bloods.Settings = new BooleanFieldSetting() { Mode = false };
            //bloods.DependentFieldDetail = new List<FieldDetail>();

            //FieldDetail bloodsr = new FieldDetail();
            //bloodsr.DataType = new FieldType() { Id = 1 };
            //bloodsr.Settings = new TextFieldSetting() { Mode = true };
            //bloodsr.Parent = bloods;
            //bloodsr.Condition = new BooleanExpression<bool>(){ Operation = BooleanOperations.EqualTo, ReferenceValue = true };
            //bloods.DependentFieldDetail.Add(bloodsr);
            //history.FieldList.Add(bloods);

            //FormStructure.SectionList.Add(history);

            ////History Section
            //SectionDetail phistory = new SectionDetail();
            //phistory.Title = "Past History";
            //phistory.FieldList = new List<FieldDetail>();

            //FieldDetail remph = new FieldDetail();
            //remph.Title = @"Remarks";
            //remph.DataType = new FieldType() { Id = 1 };
            //remph.Settings = new TextFieldSetting() { Mode = false };
            //phistory.FieldList.Add(remph);

            //FormStructure.SectionList.Add(phistory);


            ////History Section
            //SectionDetail PHE = new SectionDetail();
            //PHE.Title = "Physical Examination";
            //PHE.FieldList = new List<FieldDetail>();

            //FieldDetail Gec = new FieldDetail();
            //Gec.Title = "General Conditions";
            //Gec.DataType = new FieldType() { Id = 7 };
            //PHE.FieldList.Add(Gec);


            //FieldDetail wea = new FieldDetail();
            //wea.Title = @"Well Alert";
            //wea.DataType = new FieldType() { Id = 2 };
            //wea.Settings = new BooleanFieldSetting() { Mode = true };
            //wea.DependentFieldDetail = new List<FieldDetail>();

            //FieldDetail wear = new FieldDetail();
            //wear.DataType = new FieldType() { Id = 1 };
            //wear.Settings = new TextFieldSetting() { Mode = true };
            //wear.Parent = wea;
            //wear.Condition = new BooleanExpression<bool>() { Operation = BooleanOperations.EqualTo, ReferenceValue = true };
            //wea.DependentFieldDetail.Add(bloodsr);
            //PHE.FieldList.Add(wea);

            //FieldDetail wri = new FieldDetail();
            //wri.Title = @"Restless,irritable";
            //wri.DataType = new FieldType() { Id = 2 };
            //wri.Settings = new BooleanFieldSetting() { Mode = true };
            //wri.DependentFieldDetail = new List<FieldDetail>();

            //FieldDetail wrir = new FieldDetail();
            //wrir.DataType = new FieldType() { Id = 1 };
            //wrir.Settings = new TextFieldSetting() { Mode = true };
            //wrir.Parent = wri;
            //wrir.Condition = new BooleanExpression<bool>() { Operation = BooleanOperations.EqualTo, ReferenceValue = true };
            //wri.DependentFieldDetail.Add(wrir);
            //PHE.FieldList.Add(wri);

            //FieldDetail nuris = new FieldDetail();
            //nuris.Title = @"Nutritional Status";
            //nuris.DataType = new FieldType() { Id = 4 };
            //List<DynamicListItem> listn = new List<DynamicListItem>();
            //listn.Add(new DynamicListItem() { Title = "Normal" });
            //listn.Add(new DynamicListItem() { Title = "Mildly Impared" });
            //listn.Add(new DynamicListItem() { Title = "Moderately Impared" });
            //ListFieldSetting listStg = new ListFieldSetting();
            //listStg.ItemSource = listn;
            //listStg.ChoiceMode = SelectionMode.Single;
            //listStg.ControlType = ListControlType.ComboBox;
            ////listStg.DefaultSelectedItemIndex = listdefaultValue.SelectedIndex;
            //nuris.Settings = listStg;
            //PHE.FieldList.Add(nuris);
            //FormStructure.SectionList.Add(PHE);

            ////History Section
            //SectionDetail mgmt = new SectionDetail();
            //mgmt.Title = "Management";
            //mgmt.FieldList = new List<FieldDetail>();

            //FieldDetail antm = new FieldDetail();
            //antm.Title = "Antiemetics";
            //antm.DataType = new FieldType() { Id = 9 };
            //MedicationFieldSetting medSet = new MedicationFieldSetting();
            //medSet.ItemsSource.Add(new Medication() { DrugSource = Helpers.GetDrugList(), DosageSource = Helpers.GetDosageList(), RouteSource = Helpers.GetRouteList(), FrequencySource = Helpers.GetFrequencyList() });
            //antm.Settings = medSet;

            //mgmt.FieldList.Add(antm);

            //FormStructure.SectionList.Add(mgmt);


            // App.FormTemplateList.Add(FormStructure);

            this.RootVisual = new RaziStartPage();
        }

        private void Application_Exit(object sender, EventArgs e)
        {

        }
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // the browser's exception mechanism. On IE this will display it a yellow alert 
            // icon in the status bar and Firefox will display a script error.
            if (!System.Diagnostics.Debugger.IsAttached)
            {

                // NOTE: This will allow the application to continue running after an exception has been thrown
                // but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.
                e.Handled = true;
                Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
            }
        }
        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }
    }
}
