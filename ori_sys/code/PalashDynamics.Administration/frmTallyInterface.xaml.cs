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
using System.Windows.Browser;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using CIMS;
using PalashDynamics.ValueObjects.Billing;
using System.Windows.Data;
using System.Xml;
using System.IO;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Text;
using System.Xml.Linq;
using PalashDynamics.UserControls;

namespace PalashDynamics.Administration
{
    public partial class frmTallyInterface : UserControl
    {
        public clsAccountsVO myAccounts { get; set; }

        private SaveFileDialog dialog = null;
        private bool? showDialog = null;

        public frmTallyInterface()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(frmTallyInterface_Loaded);

            myAccounts = new clsAccountsVO();
        }

        void frmTallyInterface_Loaded(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            try
            {
                UserControl rootPage1 = Application.Current.RootVisual as UserControl;
                TextBlock mElement1 = (TextBlock)rootPage1.FindName("SampleSubHeader");
                mElement1.Text = "Export Day Book";
                
                //dgOPDSelfBillLedgers.GroupDescriptions.Add(new PropertyGroupDescription("Department"));


                FillUnit();

                this.DataContext = myAccounts;
                //DisplayLedgers();

            }
            catch (Exception)
            {
                
                //throw;
            }
        }


        private void DisplayLedgers()
        {
            WaitIndicator indicator = new WaitIndicator();

            try
            {
              

                indicator.Show();

                clsGetTotalBillAccountsLedgersVO BizAction = new clsGetTotalBillAccountsLedgersVO();
                clsAccountsVO tempAccounts = new clsAccountsVO();
                if(dtpExportForDate.SelectedDate.HasValue)
                    tempAccounts.ExportDate = dtpExportForDate.SelectedDate.Value.Date;

                if (cmbUnit.SelectedItem != null && ((MasterListItem)cmbUnit.SelectedItem).ID > 0)
                    tempAccounts.UnitID = ((MasterListItem)cmbUnit.SelectedItem).ID;

                BizAction.Details = tempAccounts;
                              

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        clsAccountsVO objAcc = ((clsGetTotalBillAccountsLedgersVO)e.Result).Details;

                        if (objAcc.OPDSelfBillLedgers != null)
                        {
                            foreach (var item in objAcc.OPDSelfBillLedgers)
                            {
                                switch (item.LedgerName.ToUpper())
                                {
                                    case "CASH":
                                        item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CashLedgerName;
                                        break;
                                    case "CHQDD":
                                        item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.ChequeDDBankName;
                                        break;
                                    case "CRDB":
                                        item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CrDbBankName;
                                        break;
                                    case "CC":
                                        item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.ConsultationLedgerName;
                                        break;
                                    case "DC":
                                        item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.DiagnosticLedgerName;
                                        break;
                                    case "OC":
                                        item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.OtherServicesLedgerName;
                                        break;
                                    default:
                                        break;
                                }
                                cmdSave.IsEnabled = true;
                            }

                            if (objAcc.OPDRefundOfBillLedgers != null)
                            {
                                foreach (var item in objAcc.OPDRefundOfBillLedgers)
                                {
                                    switch (item.LedgerName.ToUpper())
                                    {
                                        case "CASH":
                                            item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CashLedgerName;
                                            break;
                                        case "CHQDD":
                                            item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.ChequeDDBankName;
                                            break;
                                        case "CRDB":
                                            item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CrDbBankName;
                                            break;
                                        case "CC":
                                            item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.ConsultationLedgerName;
                                            break;
                                        case "DC":
                                            item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.DiagnosticLedgerName;
                                            break;
                                        case "OC":
                                            item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.OtherServicesLedgerName;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                cmdSave.IsEnabled = true;
                            }


                            if (objAcc.ItemSaleLedgers != null)
                            {
                                foreach (var item in objAcc.ItemSaleLedgers)
                                {
                                    switch (item.LedgerName.ToUpper())
                                    {
                                        case "CASH":
                                            item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CashLedgerName;
                                            break;
                                        case "CHQDD":
                                            item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.ChequeDDBankName;
                                            break;
                                        case "CRDB":
                                            item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CrDbBankName;
                                            break;
                                        case "COGS":
                                            item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.COGSLedgerName;
                                            break;
                                        case "PROFIT":
                                            item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.ProfitLedgerName;
                                            break;
                                      
                                        default:
                                            break;
                                    }
                                }
                                cmdSave.IsEnabled = true;
                            }

                            if (objAcc.ItemSaleReturnLedgers != null)
                            {
                                foreach (var item in objAcc.ItemSaleReturnLedgers)
                                {
                                    switch (item.LedgerName.ToUpper())
                                    {
                                        case "CASH":
                                            item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CashLedgerName;
                                            break;
                                        case "CHQDD":
                                            item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.ChequeDDBankName;
                                            break;
                                        case "CRDB":
                                            item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CrDbBankName;
                                            break;
                                        case "COGS":
                                            item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.COGSLedgerName;
                                            break;
                                        case "PROFIT":
                                            item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.ProfitLedgerName;
                                            break;

                                        default:
                                            break;
                                    }
                                }
                                cmdSave.IsEnabled = true;
                            }

                            if (objAcc.PurchaseCashLedgers != null)
                            {
                                foreach (var item in objAcc.PurchaseCashLedgers)
                                {
                                   if(item.CR.HasValue)
                                       item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CashLedgerName;
                                   else if(item.DR.HasValue)
                                       item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.PurchaseLedgerName;
                                       
                                }
                                cmdSave.IsEnabled = true;
                            }

                            if (objAcc.PurchaseCreditLedgers != null)
                            {
                                foreach (var item in objAcc.PurchaseCreditLedgers)
                                {
                                    if (item.DR.HasValue)
                                        item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.PurchaseLedgerName;
                                       
                                }
                                cmdSave.IsEnabled = true;
                            }

                            if (objAcc.PurchaseReturnCashLedgers != null)
                            {
                                foreach (var item in objAcc.PurchaseReturnCashLedgers)
                                {
                                    if (item.DR.HasValue)
                                        item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CashLedgerName;
                                    else if (item.CR.HasValue)
                                        item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.PurchaseLedgerName;

                                }
                                cmdSave.IsEnabled = true;
                            }

                            if (objAcc.PurchaseReturnCreditLedgers != null)
                            {
                                foreach (var item in objAcc.PurchaseReturnCreditLedgers)
                                {
                                    if (item.CR.HasValue)
                                        item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.PurchaseLedgerName;

                                }
                                cmdSave.IsEnabled = true;
                            }


                            if (objAcc.ConsumptionLedgers != null)
                            {
                                foreach (var item in objAcc.ConsumptionLedgers)
                                {
                                    if (item.CR.HasValue)
                                        item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CurrentAssetLedgerName;
                                    else if (item.DR.HasValue)
                                        item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.ExpenseLedgerName;
                                }
                                cmdSave.IsEnabled = true;
                            }


                            if (objAcc.ScrapLedgers != null)
                            {
                                foreach (var item in objAcc.ScrapLedgers)
                                {
                                    if (item.DR.HasValue)
                                    {

                                        switch (item.LedgerName.ToUpper())
                                        {
                                            case "CS":
                                                item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CashLedgerName;
                                                break;
                                            case "CHQS":
                                                item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.ChequeDDBankName;
                                                break;
                                         
                                            default:
                                                break;
                                        }
                                    }
                                    
                                    else if (item.CR.HasValue)
                                        item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.ScrapIncomeLedgerName;
                                   
                                }
                                cmdSave.IsEnabled = true;
                            }

                            myAccounts = objAcc;
                            
                            PagedCollectionView collection1 = new PagedCollectionView(myAccounts.OPDRefundOfBillLedgers);
                            collection1.GroupDescriptions.Add(new PropertyGroupDescription("Clinic"));
                            dgOPDRefundOfBillLedgers.ItemsSource = null;
                            dgOPDRefundOfBillLedgers.ItemsSource = collection1;
                          
                            PagedCollectionView collection = new PagedCollectionView(myAccounts.OPDSelfBillLedgers);
                            collection.GroupDescriptions.Add(new PropertyGroupDescription("Clinic"));
                            dgOPDSelfBillLedgers.ItemsSource = null;
                            dgOPDSelfBillLedgers.ItemsSource = collection;


                            PagedCollectionView collection2 = new PagedCollectionView(myAccounts.ItemSaleLedgers);
                            collection2.GroupDescriptions.Add(new PropertyGroupDescription("Clinic"));
                            dgSaleCashLedgers.ItemsSource = null;
                            dgSaleCashLedgers.ItemsSource = collection2;

                            PagedCollectionView collection3 = new PagedCollectionView(myAccounts.ItemSaleReturnLedgers);
                            collection3.GroupDescriptions.Add(new PropertyGroupDescription("Clinic"));
                            dgSaleReturnCashLedgers.ItemsSource = null;
                            dgSaleReturnCashLedgers.ItemsSource = collection3;

                            PagedCollectionView collection4 = new PagedCollectionView(myAccounts.PurchaseCashLedgers);
                            collection4.GroupDescriptions.Add(new PropertyGroupDescription("Clinic"));
                            dgPurchaseCashLedgers.ItemsSource = null;
                            dgPurchaseCashLedgers.ItemsSource = collection4;

                            PagedCollectionView collection5 = new PagedCollectionView(myAccounts.PurchaseCreditLedgers);
                            collection5.GroupDescriptions.Add(new PropertyGroupDescription("Clinic"));
                            dgPurchaseCreditLedgers.ItemsSource = null;
                            dgPurchaseCreditLedgers.ItemsSource = collection5;

                            PagedCollectionView collection6 = new PagedCollectionView(myAccounts.PurchaseReturnCashLedgers);
                            collection6.GroupDescriptions.Add(new PropertyGroupDescription("Clinic"));
                            dgPurchaseReturnCashLedgers.ItemsSource = null;
                            dgPurchaseReturnCashLedgers.ItemsSource = collection6;

                            PagedCollectionView collection7 = new PagedCollectionView(myAccounts.PurchaseReturnCreditLedgers);
                            collection7.GroupDescriptions.Add(new PropertyGroupDescription("Clinic"));
                            dgPurchaseReturnCreditLedgers.ItemsSource = null;
                            dgPurchaseReturnCreditLedgers.ItemsSource = collection7;

                            PagedCollectionView collection8 = new PagedCollectionView(myAccounts.ConsumptionLedgers);
                            collection8.GroupDescriptions.Add(new PropertyGroupDescription("Clinic"));
                            dgConsumptionLedgers.ItemsSource = null;
                            dgConsumptionLedgers.ItemsSource = collection8;

                            PagedCollectionView collection9 = new PagedCollectionView(myAccounts.ScrapLedgers);
                            collection9.GroupDescriptions.Add(new PropertyGroupDescription("Clinic"));
                            dgScrapLedgers.ItemsSource = null;
                            dgScrapLedgers.ItemsSource = collection9;
                            
                            this.DataContext = null;
                            this.DataContext = myAccounts;
                        }
                    }
                    else
                    {
                        //Indicatior.Close();

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                    }
                    indicator.Close();
                };
                // Indicatior.Close();

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                indicator.Close();
                // throw;
            }
            
           

            


            
        }

        private void FillUnit()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- All -"));

                    if (e.Error == null && e.Result != null)
                    {
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    }
                    cmbUnit.ItemsSource = null;
                    cmbUnit.ItemsSource = objList;
                    
                    cmbUnit.SelectedValue = objList[0].ID;
                      
                };
                // Indicatior.Close();

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                ///Indicatior.Close();
               // throw;
            }
        }
        //SaveFileDialog sfd;
        
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                //sfd = new SaveFileDialog();
                //sfd.Filter = "Video Files (.wmv) | *.wmv";

                //bool? result = sfd.ShowDialog();
                //if (result == true)
                //    MessageBox.Show("File saved to" + sfd.SafeFileName);

                //SaveFileDialog saveFileDialog = new SaveFileDialog();

                //saveFileDialog.DefaultExt = "xml";
                //saveFileDialog.Filter = "XML Files (*.xml)|*.xml|All files (*.*)|*.*";
                //saveFileDialog.FilterIndex = 1;
                // Dim result As System.Nullable(Of Boolean) = sfd.ShowDialog()

                //if (saveFileDialog.ShowDialog() == true)
                //{
                //    using (Stream stream = saveFileDialog.OpenFile())
                //    {
                //        StreamWriter sw = new StreamWriter(stream, System.Text.Encoding.UTF8);
                //        sw.Write(GetGeneratedXML().ToString());
                //        sw.Close();

                //        stream.Close();
                //    }
                //}

                this.cmdSave.IsEnabled = false;

                dialog = new SaveFileDialog { Filter = "XML files|*.xml", DefaultExt = "xml" };
                showDialog = dialog.ShowDialog();

                         

                this.SaveExport();
                //this.Finalizitaion();

             }
            catch (Exception)
            {

                throw;
            }
        }

        private void SaveExport()
        {
            if (showDialog == true)
            {
                using (Stream exportStream = dialog.OpenFile())
                {

                    StreamWriter sw = new StreamWriter(exportStream);
                    GetGeneratedXML(sw);
                    sw.Close();

                     exportStream.Close();
                   
                }

                this.Finalizitaion();
            }
        }

        private void Finalizitaion()
        {
            MessageBoxResult result = MessageBox.Show("Export Completed!");

            if (result == MessageBoxResult.OK)
            {
                this.cmdSave.IsEnabled = true;

            }
            

        }

        private void cmdGenerateData_Click(object sender, RoutedEventArgs e)
        {
            DisplayLedgers();
        }

        string qt = @"""";

        private void GetGeneratedXML(StreamWriter m_streamWriter)
        {

            //Start Writing
          

            m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
            //Header Part
            m_streamWriter.WriteLine("<ENVELOPE>");
            m_streamWriter.WriteLine("<HEADER>");
            m_streamWriter.WriteLine("<TALLYREQUEST>Import Data</TALLYREQUEST>");
            m_streamWriter.WriteLine("</HEADER>");
            m_streamWriter.WriteLine("<BODY>");
            m_streamWriter.WriteLine("<IMPORTDATA>");
            m_streamWriter.WriteLine("<REQUESTDESC>");
            m_streamWriter.WriteLine("<REPORTNAME>All Masters</REPORTNAME>");
            m_streamWriter.WriteLine("<STATICVARIABLES>");
            m_streamWriter.WriteLine("<SVCURRENTCOMPANY></SVCURRENTCOMPANY>");
            m_streamWriter.WriteLine("</STATICVARIABLES>");
            m_streamWriter.WriteLine("</REQUESTDESC>");
            m_streamWriter.WriteLine("<REQUESTDATA>");

            if (myAccounts.OPDSelfBillLedgers != null && myAccounts.OPDSelfBillLedgers.Count > 0)
                m_streamWriter = TransferSelfBills(m_streamWriter);

            if (myAccounts.OPDRefundOfBillLedgers != null && myAccounts.OPDRefundOfBillLedgers.Count > 0)
                m_streamWriter = TransferRefundOfBills(m_streamWriter);

            if (myAccounts.ItemSaleLedgers != null && myAccounts.ItemSaleLedgers.Count > 0)
                m_streamWriter = TransferItemSale(m_streamWriter);

            if (myAccounts.ItemSaleReturnLedgers != null && myAccounts.ItemSaleReturnLedgers.Count > 0)
                m_streamWriter = TransferItemSaleReturn(m_streamWriter);

            if (myAccounts.PurchaseCashLedgers != null && myAccounts.PurchaseCashLedgers.Count > 0)
                m_streamWriter = TransferCashPurchase(m_streamWriter);

            if (myAccounts.PurchaseReturnCashLedgers != null && myAccounts.PurchaseReturnCashLedgers.Count > 0)
                m_streamWriter = TransferCashPurchaseReturn(m_streamWriter);


            if (myAccounts.PurchaseCreditLedgers != null && myAccounts.PurchaseCreditLedgers.Count > 0)
                m_streamWriter = TransferCreditPurchase(m_streamWriter);

            if (myAccounts.PurchaseReturnCreditLedgers != null && myAccounts.PurchaseReturnCreditLedgers.Count > 0)
                m_streamWriter = TransferCreditPurchaseReturn(m_streamWriter);

            if (myAccounts.ConsumptionLedgers != null && myAccounts.ConsumptionLedgers.Count > 0)
                m_streamWriter = TransferConsuptionLedgers(m_streamWriter);

            if (myAccounts.ScrapLedgers != null && myAccounts.ScrapLedgers.Count > 0)
                m_streamWriter = TransferScrapLedgers(m_streamWriter);

            //=============================
            //Footer

            m_streamWriter.WriteLine("</REQUESTDATA>");
            m_streamWriter.WriteLine("</IMPORTDATA>");
            m_streamWriter.WriteLine("</BODY>");
            m_streamWriter.WriteLine("</ENVELOPE>");

            //End 
            m_streamWriter.Flush();
            m_streamWriter.Close();
        

           
        }


        private StreamWriter TransferConsuptionLedgers(StreamWriter m_streamWriter)
        {

            //===============================
            //'    Transfer Scrap Ledgers


            List<KeyValue> IDList = new List<KeyValue>();

            var resIDList = from a in myAccounts.ConsumptionLedgers
                            group a by a.Narration into grouped
                            select new KeyValue { Key = grouped.First().UnitID, Value = grouped.First().Narration };


            IDList = resIDList.ToList();

            if (IDList != null && IDList.Count > 0)
            {

                foreach (var mID in IDList)
                {
                    m_streamWriter.WriteLine(@"<TALLYMESSAGE xmlns:UDF=" + qt + "TallyUDF" + qt + ">"); // + ">");
                    m_streamWriter.WriteLine(@"<Voucher REMOTEID=".ToUpper() + qt + qt + " VCHTYPE=" + qt + "Journal" + qt + " ACTION=" + qt + "Create" + qt + ">");
                    m_streamWriter.WriteLine("<Date>".ToUpper() + myAccounts.ExportDate.ToString("yyyyMMdd") + "</Date>".ToUpper());
                    m_streamWriter.WriteLine("<GUID></GUID>");
                    m_streamWriter.WriteLine("<NARRATION>" + mID.Value + "</NARRATION>");
                    m_streamWriter.WriteLine("<VOUCHERTYPENAME>" + "Journal" + "</VOUCHERTYPENAME>");
                    m_streamWriter.WriteLine("<PARTYLEDGERNAME>" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CashLedgerName + "</PARTYLEDGERNAME>");
                    m_streamWriter.WriteLine("<CSTFORMISSUETYPE/>");
                    m_streamWriter.WriteLine("<CSTFORMRECVTYPE/>");
                    m_streamWriter.WriteLine("<FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE>");
                    m_streamWriter.WriteLine("<VCHGSTCLASS/>");
                    m_streamWriter.WriteLine("<DIFFACTUALQTY>No</DIFFACTUALQTY>");
                    m_streamWriter.WriteLine("<AUDITED>No</AUDITED>");
                    m_streamWriter.WriteLine("<FORJOBCOSTING>No</FORJOBCOSTING>");
                    m_streamWriter.WriteLine("<ISOPTIONAL>No</ISOPTIONAL>");
                    m_streamWriter.WriteLine("<EFFECTIVEDATE>" + myAccounts.ExportDate.ToString("yyyyMMdd") + "</EFFECTIVEDATE>");
                    m_streamWriter.WriteLine("<USEFORINTEREST>No</USEFORINTEREST>");
                    m_streamWriter.WriteLine("<USEFORGAINLOSS>No</USEFORGAINLOSS>");
                    m_streamWriter.WriteLine("<USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER>");
                    m_streamWriter.WriteLine("<USEFORCOMPOUND>No</USEFORCOMPOUND>");
                    m_streamWriter.WriteLine("<ALTERID> 2666</ALTERID>");
                    m_streamWriter.WriteLine("<EXCISEOPENING>No</EXCISEOPENING>");
                    m_streamWriter.WriteLine("<USEFORFINALPRODUCTION>No</USEFORFINALPRODUCTION>");
                    m_streamWriter.WriteLine("<ISCANCELLED>No</ISCANCELLED>");
                    m_streamWriter.WriteLine("<HASCASHFLOW>Yes</HASCASHFLOW>");
                    m_streamWriter.WriteLine("<ISPOSTDATED>No</ISPOSTDATED>");
                    m_streamWriter.WriteLine("<USETRACKINGNUMBER>No</USETRACKINGNUMBER>");
                    m_streamWriter.WriteLine("<ISINVOICE>No</ISINVOICE>");
                    m_streamWriter.WriteLine("<MFGJOURNAL>No</MFGJOURNAL>");
                    m_streamWriter.WriteLine("<HASDISCOUNTS>No</HASDISCOUNTS>");
                    m_streamWriter.WriteLine("<ASPAYSLIP>No</ASPAYSLIP>");
                    m_streamWriter.WriteLine("<ISCOSTCENTRE>No</ISCOSTCENTRE>");
                    m_streamWriter.WriteLine("<ISDELETED>No</ISDELETED>");
                    m_streamWriter.WriteLine("<ASORIGINAL>No</ASORIGINAL>");

                    var results = from a in myAccounts.ConsumptionLedgers
                                  where a.UnitID == (long)mID.Key && a.Narration == mID.Value
                                  orderby a.DR descending
                                  select a;

                    List<clsLedgerVO> objList = new List<clsLedgerVO>();
                    objList = results.ToList();

                    foreach (var item in objList)
                    {
                        string str = "";
                        string strPriVch = "";
                        double amt = 0;

                        if (item.DR.HasValue && item.DR > 0)
                        {
                            str = "Yes";
                            strPriVch = "No";
                            amt = -item.DR.Value;
                        }
                        else
                        {
                            str = "No";
                            strPriVch = "No";
                            amt = item.CR.Value;
                        }

                        string strAmt = String.Format("{0:0.00}", amt);

                        m_streamWriter.WriteLine("<ALLLEDGERENTRIES.LIST>");
                        m_streamWriter.WriteLine("<LEDGERNAME>" + item.LedgerName + "</LEDGERNAME>");
                        m_streamWriter.WriteLine("<GSTCLASS/>");
                        m_streamWriter.WriteLine("<ISDEEMEDPOSITIVE>" + str + "</ISDEEMEDPOSITIVE>");
                        m_streamWriter.WriteLine("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        m_streamWriter.WriteLine("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        m_streamWriter.WriteLine("<ISPARTYLEDGER>" + str + "</ISPARTYLEDGER>");
                        m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
                        if (item.CR.HasValue && item.CR > 0)
                        {
                            m_streamWriter.WriteLine("<CATEGORYALLOCATIONS.LIST>");
                            m_streamWriter.WriteLine("<CATEGORY>Primary Cost Category</CATEGORY>");
                            m_streamWriter.WriteLine("<COSTCENTREALLOCATIONS.LIST>");
                            m_streamWriter.WriteLine("<NAME>" + item.Clinic + "</NAME>");
                            m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
                            m_streamWriter.WriteLine("</COSTCENTREALLOCATIONS.LIST>");
                            m_streamWriter.WriteLine("</CATEGORYALLOCATIONS.LIST>");
                        }
                        m_streamWriter.WriteLine("</ALLLEDGERENTRIES.LIST>");

                    }


                    m_streamWriter.WriteLine("</Voucher>".ToUpper());
                    m_streamWriter.WriteLine("</TALLYMESSAGE>");

                }


            }



            return m_streamWriter;
        }     
        private StreamWriter TransferScrapLedgers(StreamWriter m_streamWriter)
        {

            //===============================
            //'    Transfer Scrap Ledgers


            List<KeyValue> IDList = new List<KeyValue>();

            var resIDList = from a in myAccounts.ScrapLedgers
                            group a by a.Narration into grouped
                            select new KeyValue { Key = grouped.First().UnitID, Value = grouped.First().Narration };


            IDList = resIDList.ToList();

            if (IDList != null && IDList.Count > 0)
            {

                foreach (var mID in IDList)
                {
                        m_streamWriter.WriteLine(@"<TALLYMESSAGE xmlns:UDF=" + qt + "TallyUDF" + qt + ">"); // + ">");
                        m_streamWriter.WriteLine(@"<Voucher REMOTEID=".ToUpper() + qt + qt + " VCHTYPE=" + qt + "Receipt" + qt + " ACTION=" + qt + "Create" + qt + ">");
                        m_streamWriter.WriteLine("<Date>".ToUpper() + myAccounts.ExportDate.ToString("yyyyMMdd") + "</Date>".ToUpper());
                        m_streamWriter.WriteLine("<GUID></GUID>");
                        m_streamWriter.WriteLine("<NARRATION>" + mID.Value + "</NARRATION>");
                        m_streamWriter.WriteLine("<VOUCHERTYPENAME>" + "Receipt" + "</VOUCHERTYPENAME>");
                        m_streamWriter.WriteLine("<PARTYLEDGERNAME>" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CashLedgerName + "</PARTYLEDGERNAME>");
                        m_streamWriter.WriteLine("<CSTFORMISSUETYPE/>");
                        m_streamWriter.WriteLine("<CSTFORMRECVTYPE/>");
                        m_streamWriter.WriteLine("<FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE>");
                        m_streamWriter.WriteLine("<VCHGSTCLASS/>");
                        m_streamWriter.WriteLine("<DIFFACTUALQTY>No</DIFFACTUALQTY>");
                        m_streamWriter.WriteLine("<AUDITED>No</AUDITED>");
                        m_streamWriter.WriteLine("<FORJOBCOSTING>No</FORJOBCOSTING>");
                        m_streamWriter.WriteLine("<ISOPTIONAL>No</ISOPTIONAL>");
                        m_streamWriter.WriteLine("<EFFECTIVEDATE>" + myAccounts.ExportDate.ToString("yyyyMMdd") + "</EFFECTIVEDATE>");
                        m_streamWriter.WriteLine("<USEFORINTEREST>No</USEFORINTEREST>");
                        m_streamWriter.WriteLine("<USEFORGAINLOSS>No</USEFORGAINLOSS>");
                        m_streamWriter.WriteLine("<USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER>");
                        m_streamWriter.WriteLine("<USEFORCOMPOUND>No</USEFORCOMPOUND>");
                        m_streamWriter.WriteLine("<ALTERID> 2666</ALTERID>");
                        m_streamWriter.WriteLine("<EXCISEOPENING>No</EXCISEOPENING>");
                        m_streamWriter.WriteLine("<USEFORFINALPRODUCTION>No</USEFORFINALPRODUCTION>");
                        m_streamWriter.WriteLine("<ISCANCELLED>No</ISCANCELLED>");
                        m_streamWriter.WriteLine("<HASCASHFLOW>Yes</HASCASHFLOW>");
                        m_streamWriter.WriteLine("<ISPOSTDATED>No</ISPOSTDATED>");
                        m_streamWriter.WriteLine("<USETRACKINGNUMBER>No</USETRACKINGNUMBER>");
                        m_streamWriter.WriteLine("<ISINVOICE>No</ISINVOICE>");
                        m_streamWriter.WriteLine("<MFGJOURNAL>No</MFGJOURNAL>");
                        m_streamWriter.WriteLine("<HASDISCOUNTS>No</HASDISCOUNTS>");
                        m_streamWriter.WriteLine("<ASPAYSLIP>No</ASPAYSLIP>");
                        m_streamWriter.WriteLine("<ISCOSTCENTRE>No</ISCOSTCENTRE>");
                        m_streamWriter.WriteLine("<ISDELETED>No</ISDELETED>");
                        m_streamWriter.WriteLine("<ASORIGINAL>No</ASORIGINAL>");

                        var results = from a in myAccounts.ScrapLedgers
                                      where a.UnitID == (long)mID.Key && a.Narration == mID.Value
                                      orderby a.DR descending
                                      select a;

                        List<clsLedgerVO> objList = new List<clsLedgerVO>();
                        objList = results.ToList();

                        foreach (var item in objList)
                        {
                            string str = "";
                            string strPriVch = "";
                            double amt = 0;

                            if (item.DR.HasValue && item.DR > 0)
                            {
                                str = "Yes";
                                strPriVch = "No";
                                amt = -item.DR.Value;
                            }
                            else
                            {
                                str = "No";
                                strPriVch = "No";
                                amt = item.CR.Value;
                            }

                        string strAmt = String.Format("{0:0.00}", amt);

                        m_streamWriter.WriteLine("<ALLLEDGERENTRIES.LIST>");
                        m_streamWriter.WriteLine("<LEDGERNAME>" + item.LedgerName + "</LEDGERNAME>");
                        m_streamWriter.WriteLine("<GSTCLASS/>");
                        m_streamWriter.WriteLine("<ISDEEMEDPOSITIVE>" + str + "</ISDEEMEDPOSITIVE>");
                        m_streamWriter.WriteLine("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        m_streamWriter.WriteLine("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        m_streamWriter.WriteLine("<ISPARTYLEDGER>" + str + "</ISPARTYLEDGER>");
                        m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
                        if (item.CR.HasValue && item.CR > 0)
                        {
                            m_streamWriter.WriteLine("<CATEGORYALLOCATIONS.LIST>");
                            m_streamWriter.WriteLine("<CATEGORY>Primary Cost Category</CATEGORY>");
                            m_streamWriter.WriteLine("<COSTCENTREALLOCATIONS.LIST>");
                            m_streamWriter.WriteLine("<NAME>" + item.Clinic + "</NAME>");
                            m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
                            m_streamWriter.WriteLine("</COSTCENTREALLOCATIONS.LIST>");
                            m_streamWriter.WriteLine("</CATEGORYALLOCATIONS.LIST>");
                        }
                        m_streamWriter.WriteLine("</ALLLEDGERENTRIES.LIST>");

                    }


                        m_streamWriter.WriteLine("</Voucher>".ToUpper());
                        m_streamWriter.WriteLine("</TALLYMESSAGE>");

                }


            }



            return m_streamWriter;
        }     

        private StreamWriter TransferCreditPurchaseReturn(StreamWriter m_streamWriter)
        {

            //===============================
            //'    Transfer Purchase Return Credit Ledgers


            List<KeyValue> IDList = new List<KeyValue>();

            var resIDList = from a in myAccounts.PurchaseReturnCreditLedgers
                            group a by a.Narration into grouped
                            select new KeyValue { Key = grouped.First().UnitID, Value = grouped.First().Narration };


            IDList = resIDList.ToList();

            if (IDList != null && IDList.Count > 0)
            {

                foreach (var mID in IDList)
                {

                        m_streamWriter.WriteLine(@"<TALLYMESSAGE xmlns:UDF=" + qt + "TallyUDF" + qt + ">"); // + ">");
                        m_streamWriter.WriteLine(@"<Voucher REMOTEID=".ToUpper() + qt + qt + " VCHTYPE=" + qt + "Debit Note" + qt + " ACTION=" + qt + "Create" + qt + ">");
                        m_streamWriter.WriteLine("<Date>".ToUpper() + myAccounts.ExportDate.ToString("yyyyMMdd") + "</Date>".ToUpper());
                        m_streamWriter.WriteLine("<GUID></GUID>");
                        m_streamWriter.WriteLine("<NARRATION>" + mID.Value + "</NARRATION>");
                        m_streamWriter.WriteLine("<VOUCHERTYPENAME>" + "Debit Note" + "</VOUCHERTYPENAME>");
                        m_streamWriter.WriteLine("<PARTYLEDGERNAME>" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CashLedgerName + "</PARTYLEDGERNAME>");
                        m_streamWriter.WriteLine("<STATADJUSTMENTTYPE>Others</STATADJUSTMENTTYPE>");
                        m_streamWriter.WriteLine("<CSTFORMISSUETYPE/>");
                        m_streamWriter.WriteLine("<CSTFORMRECVTYPE/>");
                        m_streamWriter.WriteLine("<FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE>");
                        m_streamWriter.WriteLine("<VCHGSTCLASS/>");
                        m_streamWriter.WriteLine("<DIFFACTUALQTY>No</DIFFACTUALQTY>");
                        m_streamWriter.WriteLine("<AUDITED>No</AUDITED>");
                        m_streamWriter.WriteLine("<FORJOBCOSTING>No</FORJOBCOSTING>");
                        m_streamWriter.WriteLine("<ISOPTIONAL>No</ISOPTIONAL>");
                        m_streamWriter.WriteLine("<EFFECTIVEDATE>" + myAccounts.ExportDate.ToString("yyyyMMdd") + "</EFFECTIVEDATE>");
                        m_streamWriter.WriteLine("<USEFORINTEREST>No</USEFORINTEREST>");
                        m_streamWriter.WriteLine("<USEFORGAINLOSS>No</USEFORGAINLOSS>");
                        m_streamWriter.WriteLine("<USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER>");
                        m_streamWriter.WriteLine("<USEFORCOMPOUND>No</USEFORCOMPOUND>");
                        m_streamWriter.WriteLine("<ALTERID> 2666</ALTERID>");
                        m_streamWriter.WriteLine("<EXCISEOPENING>No</EXCISEOPENING>");
                        m_streamWriter.WriteLine("<USEFORFINALPRODUCTION>No</USEFORFINALPRODUCTION>");
                        m_streamWriter.WriteLine("<ISCANCELLED>No</ISCANCELLED>");
                        m_streamWriter.WriteLine("<HASCASHFLOW>Yes</HASCASHFLOW>");
                        m_streamWriter.WriteLine("<ISPOSTDATED>No</ISPOSTDATED>");
                        m_streamWriter.WriteLine("<USETRACKINGNUMBER>No</USETRACKINGNUMBER>");
                        m_streamWriter.WriteLine("<ISINVOICE>No</ISINVOICE>");
                        m_streamWriter.WriteLine("<MFGJOURNAL>No</MFGJOURNAL>");
                        m_streamWriter.WriteLine("<HASDISCOUNTS>No</HASDISCOUNTS>");
                        m_streamWriter.WriteLine("<ASPAYSLIP>No</ASPAYSLIP>");
                        m_streamWriter.WriteLine("<ISCOSTCENTRE>No</ISCOSTCENTRE>");
                        m_streamWriter.WriteLine("<ISDELETED>No</ISDELETED>");
                        m_streamWriter.WriteLine("<ASORIGINAL>No</ASORIGINAL>");

                        var results = from a in myAccounts.PurchaseReturnCreditLedgers
                                      where a.UnitID == (long)mID.Key && a.Narration == mID.Value
                                      orderby a.DR descending
                                      select a;

                        List<clsLedgerVO> objList = new List<clsLedgerVO>();
                        objList = results.ToList();

                        foreach (var item in objList)
                        {

                        string str = "";
                        double amt = 0;

                        if (item.DR.HasValue && item.DR > 0)
                        {
                            str = "Yes";
                            amt = -item.DR.Value;
                        }
                        else
                        {
                            str = "No";
                            amt = item.CR.Value;
                        }

                        string strAmt = String.Format("{0:0.00}", amt);

                        m_streamWriter.WriteLine("<ALLLEDGERENTRIES.LIST>");
                        m_streamWriter.WriteLine("<LEDGERNAME>" + item.LedgerName + "</LEDGERNAME>");
                        m_streamWriter.WriteLine("<GSTCLASS/>");
                        m_streamWriter.WriteLine("<ISDEEMEDPOSITIVE>" + str + "</ISDEEMEDPOSITIVE>");
                        m_streamWriter.WriteLine("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        m_streamWriter.WriteLine("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        m_streamWriter.WriteLine("<ISPARTYLEDGER>" + str + "</ISPARTYLEDGER>");
                        m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
                        if (item.CR.HasValue && item.CR > 0)
                        {
                            m_streamWriter.WriteLine("<CATEGORYALLOCATIONS.LIST>");
                            m_streamWriter.WriteLine("<CATEGORY>Primary Cost Category</CATEGORY>");
                            m_streamWriter.WriteLine("<COSTCENTREALLOCATIONS.LIST>");
                            m_streamWriter.WriteLine("<NAME>" + item.Clinic + "</NAME>");
                            m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
                            m_streamWriter.WriteLine("</COSTCENTREALLOCATIONS.LIST>");
                            m_streamWriter.WriteLine("</CATEGORYALLOCATIONS.LIST>");
                        }
                        else if (item.DR.HasValue && item.DR > 0)
                        {
                            m_streamWriter.WriteLine("<BILLALLOCATIONS.LIST>");
                            m_streamWriter.WriteLine("<NAME>" + item.Narration + "</NAME>");
                            m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
                            m_streamWriter.WriteLine("<BILLCREDITPERIOD></BILLCREDITPERIOD>");
                            m_streamWriter.WriteLine("<BILLTYPE>Agst Ref</BILLTYPE>");
                            m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
                            m_streamWriter.WriteLine("</BILLALLOCATIONS.LIST>");
                        }

                        m_streamWriter.WriteLine("</ALLLEDGERENTRIES.LIST>");

                     
                    }

                        m_streamWriter.WriteLine("</Voucher>".ToUpper());
                        m_streamWriter.WriteLine("</TALLYMESSAGE>");

                }


            }



            return m_streamWriter;
        }     
        private StreamWriter TransferCreditPurchase(StreamWriter m_streamWriter)
        {

            //===============================
            //'    Transfer Credit Purchase


            List<KeyValue> IDList = new List<KeyValue>();

            var resIDList = from a in myAccounts.PurchaseCreditLedgers
                            group a by a.Narration into grouped
                            select new KeyValue { Key = grouped.First().UnitID, Value = grouped.First().Narration };


            IDList = resIDList.ToList();

            if (IDList != null && IDList.Count > 0)
            {

                foreach (var mID in IDList)
                {


                    m_streamWriter.WriteLine(@"<TALLYMESSAGE xmlns:UDF=" + qt + "TallyUDF" + qt + ">"); // + ">");
                    m_streamWriter.WriteLine(@"<Voucher REMOTEID=".ToUpper() + qt + qt + " VCHTYPE=" + qt + "Purchase" + qt + " ACTION=" + qt + "Create" + qt + ">");
                    m_streamWriter.WriteLine("<Date>".ToUpper() + myAccounts.ExportDate.ToString("yyyyMMdd") + "</Date>".ToUpper());
                    m_streamWriter.WriteLine("<GUID></GUID>");
                    m_streamWriter.WriteLine("<NARRATION>" + mID.Value + "</NARRATION>");
                    m_streamWriter.WriteLine("<VOUCHERTYPENAME>" + "Purchase" + "</VOUCHERTYPENAME>");
                    m_streamWriter.WriteLine("<PARTYLEDGERNAME>" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CashLedgerName + "</PARTYLEDGERNAME>");
                    m_streamWriter.WriteLine("<CSTFORMISSUETYPE/>");
                    m_streamWriter.WriteLine("<CSTFORMRECVTYPE/>");
                    m_streamWriter.WriteLine("<FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE>");
                    m_streamWriter.WriteLine("<VCHGSTCLASS/>");
                    m_streamWriter.WriteLine("<DIFFACTUALQTY>No</DIFFACTUALQTY>");
                    m_streamWriter.WriteLine("<AUDITED>No</AUDITED>");
                    m_streamWriter.WriteLine("<FORJOBCOSTING>No</FORJOBCOSTING>");
                    m_streamWriter.WriteLine("<ISOPTIONAL>No</ISOPTIONAL>");
                    m_streamWriter.WriteLine("<EFFECTIVEDATE>" + myAccounts.ExportDate.ToString("yyyyMMdd") + "</EFFECTIVEDATE>");
                    m_streamWriter.WriteLine("<USEFORINTEREST>No</USEFORINTEREST>");
                    m_streamWriter.WriteLine("<USEFORGAINLOSS>No</USEFORGAINLOSS>");
                    m_streamWriter.WriteLine("<USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER>");
                    m_streamWriter.WriteLine("<USEFORCOMPOUND>No</USEFORCOMPOUND>");
                    m_streamWriter.WriteLine("<ALTERID> 2666</ALTERID>");
                    m_streamWriter.WriteLine("<EXCISEOPENING>No</EXCISEOPENING>");
                    m_streamWriter.WriteLine("<USEFORFINALPRODUCTION>No</USEFORFINALPRODUCTION>");
                    m_streamWriter.WriteLine("<ISCANCELLED>No</ISCANCELLED>");
                    m_streamWriter.WriteLine("<HASCASHFLOW>Yes</HASCASHFLOW>");
                    m_streamWriter.WriteLine("<ISPOSTDATED>No</ISPOSTDATED>");
                    m_streamWriter.WriteLine("<USETRACKINGNUMBER>No</USETRACKINGNUMBER>");
                    m_streamWriter.WriteLine("<ISINVOICE>No</ISINVOICE>");
                    m_streamWriter.WriteLine("<MFGJOURNAL>No</MFGJOURNAL>");
                    m_streamWriter.WriteLine("<HASDISCOUNTS>No</HASDISCOUNTS>");
                    m_streamWriter.WriteLine("<ASPAYSLIP>No</ASPAYSLIP>");
                    m_streamWriter.WriteLine("<ISCOSTCENTRE>No</ISCOSTCENTRE>");
                    m_streamWriter.WriteLine("<ISDELETED>No</ISDELETED>");
                    m_streamWriter.WriteLine("<ASORIGINAL>No</ASORIGINAL>");

                    var results = from a in myAccounts.PurchaseCreditLedgers
                                  where a.UnitID == (long)mID.Key && a.Narration == mID.Value
                                  orderby a.DR descending
                                  select a;

                    List<clsLedgerVO> objList = new List<clsLedgerVO>();
                    objList = results.ToList();

                    foreach (var item in objList)
                    {

                        string str = "";
                        string strPriVch = "";
                        double amt = 0;

                        if (item.DR.HasValue && item.DR > 0)
                        {
                            str = "Yes";
                            strPriVch = "No";
                            amt = -item.DR.Value;
                        }
                        else
                        {
                            str = "No";
                            strPriVch = "Yes";
                            amt = item.CR.Value;
                        }
                        string strAmt = String.Format("{0:0.00}", amt);

                        m_streamWriter.WriteLine("<ALLLEDGERENTRIES.LIST>");
                        m_streamWriter.WriteLine("<LEDGERNAME>" + item.LedgerName + "</LEDGERNAME>");
                        m_streamWriter.WriteLine("<GSTCLASS/>");
                        m_streamWriter.WriteLine("<ISDEEMEDPOSITIVE>" + str + "</ISDEEMEDPOSITIVE>");
                        m_streamWriter.WriteLine("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        m_streamWriter.WriteLine("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        m_streamWriter.WriteLine("<ISPARTYLEDGER>" + strPriVch + "</ISPARTYLEDGER>");
                        m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");

                        if (item.DR.HasValue && item.DR > 0)
                        {
                            m_streamWriter.WriteLine("<CATEGORYALLOCATIONS.LIST>");
                            m_streamWriter.WriteLine("<CATEGORY>Primary Cost Category</CATEGORY>");
                            m_streamWriter.WriteLine("<COSTCENTREALLOCATIONS.LIST>");
                            m_streamWriter.WriteLine("<NAME>" + item.Clinic + "</NAME>");
                            m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
                            m_streamWriter.WriteLine("</COSTCENTREALLOCATIONS.LIST>");
                            m_streamWriter.WriteLine("</CATEGORYALLOCATIONS.LIST>");
                        }
                        else if (item.CR.HasValue && item.CR > 0)
                        {
                            m_streamWriter.WriteLine("<BILLALLOCATIONS.LIST>");                          
                            m_streamWriter.WriteLine("<NAME>" + item.Narration + "</NAME>");
                            m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
                            m_streamWriter.WriteLine("<BILLCREDITPERIOD></BILLCREDITPERIOD>"); 
                            m_streamWriter.WriteLine("<BILLTYPE>New Ref</BILLTYPE>");
                            m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
                            m_streamWriter.WriteLine("</BILLALLOCATIONS.LIST>");
                        }
                        m_streamWriter.WriteLine("</ALLLEDGERENTRIES.LIST>");
                    }

                    m_streamWriter.WriteLine("</Voucher>".ToUpper());
                    m_streamWriter.WriteLine("</TALLYMESSAGE>");

                }
            }

            return m_streamWriter;
        }

        private StreamWriter TransferCashPurchaseReturn(StreamWriter m_streamWriter)
        {

            //===============================
            //'    Transfer Purchase Return Cash Ledgers


            List<KeyValue> IDList = new List<KeyValue>();

            var resIDList = from a in myAccounts.PurchaseReturnCashLedgers
                            group a by a.Narration into grouped
                            select new KeyValue { Key = grouped.First().UnitID, Value = grouped.First().Narration };


            IDList = resIDList.ToList();

            if (IDList != null && IDList.Count > 0)
            {

                foreach (var mID in IDList)
                {
                  

                    m_streamWriter.WriteLine(@"<TALLYMESSAGE xmlns:UDF=" + qt + "TallyUDF" + qt + ">"); // + ">");
                    m_streamWriter.WriteLine(@"<Voucher REMOTEID=".ToUpper() + qt + qt + " VCHTYPE=" + qt + "Receipt" + qt + " ACTION=" + qt + "Create" + qt + ">");
                    m_streamWriter.WriteLine("<Date>".ToUpper() + myAccounts.ExportDate.ToString("yyyyMMdd") + "</Date>".ToUpper());
                    m_streamWriter.WriteLine("<GUID></GUID>");
                    m_streamWriter.WriteLine("<NARRATION>" + mID.Value + "</NARRATION>");
                    m_streamWriter.WriteLine("<VOUCHERTYPENAME>" + "Receipt" + "</VOUCHERTYPENAME>");
                    m_streamWriter.WriteLine("<PARTYLEDGERNAME>" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CashLedgerName + "</PARTYLEDGERNAME>");
                    m_streamWriter.WriteLine("<CSTFORMISSUETYPE/>");
                    m_streamWriter.WriteLine("<CSTFORMRECVTYPE/>");
                    m_streamWriter.WriteLine("<FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE>");
                    m_streamWriter.WriteLine("<VCHGSTCLASS/>");
                    m_streamWriter.WriteLine("<DIFFACTUALQTY>No</DIFFACTUALQTY>");
                    m_streamWriter.WriteLine("<AUDITED>No</AUDITED>");
                    m_streamWriter.WriteLine("<FORJOBCOSTING>No</FORJOBCOSTING>");
                    m_streamWriter.WriteLine("<ISOPTIONAL>No</ISOPTIONAL>");
                    m_streamWriter.WriteLine("<EFFECTIVEDATE>" + myAccounts.ExportDate.ToString("yyyyMMdd") + "</EFFECTIVEDATE>");
                    m_streamWriter.WriteLine("<USEFORINTEREST>No</USEFORINTEREST>");
                    m_streamWriter.WriteLine("<USEFORGAINLOSS>No</USEFORGAINLOSS>");
                    m_streamWriter.WriteLine("<USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER>");
                    m_streamWriter.WriteLine("<USEFORCOMPOUND>No</USEFORCOMPOUND>");
                    m_streamWriter.WriteLine("<ALTERID> 2666</ALTERID>");
                    m_streamWriter.WriteLine("<EXCISEOPENING>No</EXCISEOPENING>");
                    m_streamWriter.WriteLine("<USEFORFINALPRODUCTION>No</USEFORFINALPRODUCTION>");
                    m_streamWriter.WriteLine("<ISCANCELLED>No</ISCANCELLED>");
                    m_streamWriter.WriteLine("<HASCASHFLOW>Yes</HASCASHFLOW>");
                    m_streamWriter.WriteLine("<ISPOSTDATED>No</ISPOSTDATED>");
                    m_streamWriter.WriteLine("<USETRACKINGNUMBER>No</USETRACKINGNUMBER>");
                    m_streamWriter.WriteLine("<ISINVOICE>No</ISINVOICE>");
                    m_streamWriter.WriteLine("<MFGJOURNAL>No</MFGJOURNAL>");
                    m_streamWriter.WriteLine("<HASDISCOUNTS>No</HASDISCOUNTS>");
                    m_streamWriter.WriteLine("<ASPAYSLIP>No</ASPAYSLIP>");
                    m_streamWriter.WriteLine("<ISCOSTCENTRE>No</ISCOSTCENTRE>");
                    m_streamWriter.WriteLine("<ISDELETED>No</ISDELETED>");
                    m_streamWriter.WriteLine("<ASORIGINAL>No</ASORIGINAL>");


                    var results = from a in myAccounts.PurchaseReturnCashLedgers
                                  where a.UnitID == (long)mID.Key && a.Narration == mID.Value
                                  orderby a.DR descending
                                  select a;

                    List<clsLedgerVO> objList = new List<clsLedgerVO>();
                    objList = results.ToList();

                    foreach (var item in objList)
                    {

                        string str = "";
                        double amt = 0;

                        if (item.DR.HasValue && item.DR > 0)
                        {
                            str = "Yes";
                            amt = -item.DR.Value;
                        }
                        else
                        {
                            str = "No";
                            amt = item.CR.Value;
                        }

                        string strAmt = String.Format("{0:0.00}", amt);

                        m_streamWriter.WriteLine("<ALLLEDGERENTRIES.LIST>");
                        m_streamWriter.WriteLine("<LEDGERNAME>" + item.LedgerName + "</LEDGERNAME>");
                        m_streamWriter.WriteLine("<GSTCLASS/>");
                        m_streamWriter.WriteLine("<ISDEEMEDPOSITIVE>" + str + "</ISDEEMEDPOSITIVE>");
                        m_streamWriter.WriteLine("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        m_streamWriter.WriteLine("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        m_streamWriter.WriteLine("<ISPARTYLEDGER>" + str + "</ISPARTYLEDGER>");
                        m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
                        if (item.CR.HasValue && item.CR > 0)
                        {
                            m_streamWriter.WriteLine("<CATEGORYALLOCATIONS.LIST>");
                            m_streamWriter.WriteLine("<CATEGORY>Primary Cost Category</CATEGORY>");
                            m_streamWriter.WriteLine("<COSTCENTREALLOCATIONS.LIST>");
                            m_streamWriter.WriteLine("<NAME>" + item.Clinic + "</NAME>");
                            m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
                            m_streamWriter.WriteLine("</COSTCENTREALLOCATIONS.LIST>");
                            m_streamWriter.WriteLine("</CATEGORYALLOCATIONS.LIST>");
                        }
                        m_streamWriter.WriteLine("</ALLLEDGERENTRIES.LIST>");

                      
                    }

                    m_streamWriter.WriteLine("</Voucher>".ToUpper());
                    m_streamWriter.WriteLine("</TALLYMESSAGE>");
                  

                }


            }



            return m_streamWriter;
        }     
        private StreamWriter TransferCashPurchase(StreamWriter m_streamWriter)
        {

            //===============================
            //'    Transfer Cash Purchase


            List<KeyValue> IDList = new List<KeyValue>();

            var resIDList = from a in myAccounts.PurchaseCashLedgers
                            group a by a.Narration into grouped
                            select new KeyValue { Key = grouped.First().UnitID, Value = grouped.First().Narration };


            IDList = resIDList.ToList();

            if (IDList != null && IDList.Count > 0)
            {

                foreach (var mID in IDList)
                {
                   

                    m_streamWriter.WriteLine(@"<TALLYMESSAGE xmlns:UDF=" + qt + "TallyUDF" + qt + ">"); // + ">");
                    m_streamWriter.WriteLine(@"<Voucher REMOTEID=".ToUpper() + qt + qt + " VCHTYPE=" + qt + "Payment" + qt + " ACTION=" + qt + "Create" + qt + ">");
                    m_streamWriter.WriteLine("<Date>".ToUpper() + myAccounts.ExportDate.ToString("yyyyMMdd") + "</Date>".ToUpper());
                    m_streamWriter.WriteLine("<GUID></GUID>");
                    m_streamWriter.WriteLine("<NARRATION>" + mID.Value + "</NARRATION>");
                    m_streamWriter.WriteLine("<VOUCHERTYPENAME>" + "Payment" + "</VOUCHERTYPENAME>");
                    m_streamWriter.WriteLine("<PARTYLEDGERNAME>" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CashLedgerName + "</PARTYLEDGERNAME>");
                    m_streamWriter.WriteLine("<CSTFORMISSUETYPE/>");
                    m_streamWriter.WriteLine("<CSTFORMRECVTYPE/>");
                    m_streamWriter.WriteLine("<FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE>");
                    m_streamWriter.WriteLine("<VCHGSTCLASS/>");
                    m_streamWriter.WriteLine("<DIFFACTUALQTY>No</DIFFACTUALQTY>");
                    m_streamWriter.WriteLine("<AUDITED>No</AUDITED>");
                    m_streamWriter.WriteLine("<FORJOBCOSTING>No</FORJOBCOSTING>");
                    m_streamWriter.WriteLine("<ISOPTIONAL>No</ISOPTIONAL>");
                    m_streamWriter.WriteLine("<EFFECTIVEDATE>" + myAccounts.ExportDate.ToString("yyyyMMdd") + "</EFFECTIVEDATE>");
                    m_streamWriter.WriteLine("<USEFORINTEREST>No</USEFORINTEREST>");
                    m_streamWriter.WriteLine("<USEFORGAINLOSS>No</USEFORGAINLOSS>");
                    m_streamWriter.WriteLine("<USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER>");
                    m_streamWriter.WriteLine("<USEFORCOMPOUND>No</USEFORCOMPOUND>");
                    m_streamWriter.WriteLine("<ALTERID> 2666</ALTERID>");
                    m_streamWriter.WriteLine("<EXCISEOPENING>No</EXCISEOPENING>");
                    m_streamWriter.WriteLine("<USEFORFINALPRODUCTION>No</USEFORFINALPRODUCTION>");
                    m_streamWriter.WriteLine("<ISCANCELLED>No</ISCANCELLED>");
                    m_streamWriter.WriteLine("<HASCASHFLOW>Yes</HASCASHFLOW>");
                    m_streamWriter.WriteLine("<ISPOSTDATED>No</ISPOSTDATED>");
                    m_streamWriter.WriteLine("<USETRACKINGNUMBER>No</USETRACKINGNUMBER>");
                    m_streamWriter.WriteLine("<ISINVOICE>No</ISINVOICE>");
                    m_streamWriter.WriteLine("<MFGJOURNAL>No</MFGJOURNAL>");
                    m_streamWriter.WriteLine("<HASDISCOUNTS>No</HASDISCOUNTS>");
                    m_streamWriter.WriteLine("<ASPAYSLIP>No</ASPAYSLIP>");
                    m_streamWriter.WriteLine("<ISCOSTCENTRE>No</ISCOSTCENTRE>");
                    m_streamWriter.WriteLine("<ISDELETED>No</ISDELETED>");
                    m_streamWriter.WriteLine("<ASORIGINAL>No</ASORIGINAL>");

                    var results = from a in myAccounts.PurchaseCashLedgers
                                  where a.UnitID == (long)mID.Key && a.Narration == mID.Value
                                  orderby a.DR descending
                                  select a;

                    List<clsLedgerVO> objList = new List<clsLedgerVO>();
                    objList = results.ToList();

                    foreach (var item in objList)
                    {
                 
                        string str = "";
                        string strPriVch = "";
                        double amt = 0;

                        if (item.DR.HasValue && item.DR > 0)
                        {
                            str = "Yes";
                            strPriVch = "No";
                            amt = -item.DR.Value;
                        }
                        else
                        {
                            str = "No";
                            strPriVch = "Yes";
                            amt = item.CR.Value;
                        }
                        string strAmt = String.Format("{0:0.00}", amt);

                        m_streamWriter.WriteLine("<ALLLEDGERENTRIES.LIST>");
                        m_streamWriter.WriteLine("<LEDGERNAME>" + item.LedgerName + "</LEDGERNAME>");
                        m_streamWriter.WriteLine("<GSTCLASS/>");
                        m_streamWriter.WriteLine("<ISDEEMEDPOSITIVE>" + str + "</ISDEEMEDPOSITIVE>");
                        m_streamWriter.WriteLine("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        m_streamWriter.WriteLine("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        m_streamWriter.WriteLine("<ISPARTYLEDGER>" + strPriVch + "</ISPARTYLEDGER>");
                        m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");

                        if (item.DR.HasValue && item.DR > 0)
                        {
                            m_streamWriter.WriteLine("<CATEGORYALLOCATIONS.LIST>");
                            m_streamWriter.WriteLine("<CATEGORY>Primary Cost Category</CATEGORY>");
                            m_streamWriter.WriteLine("<COSTCENTREALLOCATIONS.LIST>");
                            m_streamWriter.WriteLine("<NAME>" + item.Clinic + "</NAME>");
                            m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
                            m_streamWriter.WriteLine("</COSTCENTREALLOCATIONS.LIST>");
                            m_streamWriter.WriteLine("</CATEGORYALLOCATIONS.LIST>");
                        }
                        m_streamWriter.WriteLine("</ALLLEDGERENTRIES.LIST>");
                    }

                    m_streamWriter.WriteLine("</Voucher>".ToUpper());
                    m_streamWriter.WriteLine("</TALLYMESSAGE>");

                }
            }

            return m_streamWriter;
        }

        private StreamWriter TransferItemSale(StreamWriter m_streamWriter)
        {

            //===============================
            //'    Transfer Item sales


            List<KeyValue> IDList = new List<KeyValue>();

            var resIDList = from a in myAccounts.ItemSaleLedgers
                            group a by a.UnitID into grouped
                            select new KeyValue { Key = grouped.First().UnitID, Value = grouped.First().Narration };


            IDList = resIDList.ToList();

            if (IDList != null && IDList.Count > 0)
            {

                foreach (var mID in IDList)
                {
                    m_streamWriter.WriteLine(@"<TALLYMESSAGE xmlns:UDF=" + qt + "TallyUDF" + qt + ">"); // + ">");
                    m_streamWriter.WriteLine(@"<Voucher REMOTEID=".ToUpper() + qt + qt + " VCHTYPE=" + qt + "Receipt" + qt + " ACTION=" + qt + "Create" + qt + ">");
                    m_streamWriter.WriteLine("<Date>".ToUpper() + myAccounts.ExportDate.ToString("yyyyMMdd") + "</Date>".ToUpper());
                    m_streamWriter.WriteLine("<GUID></GUID>");
                    m_streamWriter.WriteLine("<NARRATION>" + mID.Value + "</NARRATION>");
                    m_streamWriter.WriteLine("<VOUCHERTYPENAME>" + "Receipt" + "</VOUCHERTYPENAME>");
                    m_streamWriter.WriteLine("<PARTYLEDGERNAME>" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CashLedgerName + "</PARTYLEDGERNAME>");
                    m_streamWriter.WriteLine("<CSTFORMISSUETYPE/>");
                    m_streamWriter.WriteLine("<CSTFORMRECVTYPE/>");
                    m_streamWriter.WriteLine("<FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE>");
                    m_streamWriter.WriteLine("<VCHGSTCLASS/>");
                    m_streamWriter.WriteLine("<DIFFACTUALQTY>No</DIFFACTUALQTY>");
                    m_streamWriter.WriteLine("<AUDITED>No</AUDITED>");
                    m_streamWriter.WriteLine("<FORJOBCOSTING>No</FORJOBCOSTING>");
                    m_streamWriter.WriteLine("<ISOPTIONAL>No</ISOPTIONAL>");
                    m_streamWriter.WriteLine("<EFFECTIVEDATE>" + myAccounts.ExportDate.ToString("yyyyMMdd") + "</EFFECTIVEDATE>");
                    m_streamWriter.WriteLine("<USEFORINTEREST>No</USEFORINTEREST>");
                    m_streamWriter.WriteLine("<USEFORGAINLOSS>No</USEFORGAINLOSS>");
                    m_streamWriter.WriteLine("<USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER>");
                    m_streamWriter.WriteLine("<USEFORCOMPOUND>No</USEFORCOMPOUND>");
                    m_streamWriter.WriteLine("<ALTERID> 2666</ALTERID>");
                    m_streamWriter.WriteLine("<EXCISEOPENING>No</EXCISEOPENING>");
                    m_streamWriter.WriteLine("<USEFORFINALPRODUCTION>No</USEFORFINALPRODUCTION>");
                    m_streamWriter.WriteLine("<ISCANCELLED>No</ISCANCELLED>");
                    m_streamWriter.WriteLine("<HASCASHFLOW>Yes</HASCASHFLOW>");
                    m_streamWriter.WriteLine("<ISPOSTDATED>No</ISPOSTDATED>");
                    m_streamWriter.WriteLine("<USETRACKINGNUMBER>No</USETRACKINGNUMBER>");
                    m_streamWriter.WriteLine("<ISINVOICE>No</ISINVOICE>");
                    m_streamWriter.WriteLine("<MFGJOURNAL>No</MFGJOURNAL>");
                    m_streamWriter.WriteLine("<HASDISCOUNTS>No</HASDISCOUNTS>");
                    m_streamWriter.WriteLine("<ASPAYSLIP>No</ASPAYSLIP>");
                    m_streamWriter.WriteLine("<ISCOSTCENTRE>No</ISCOSTCENTRE>");
                    m_streamWriter.WriteLine("<ISDELETED>No</ISDELETED>");
                    m_streamWriter.WriteLine("<ASORIGINAL>No</ASORIGINAL>");

                    var results = from a in myAccounts.ItemSaleLedgers
                                  where a.UnitID == (long)mID.Key
                                  orderby a.DR descending
                                  select a;

                    List<clsLedgerVO> objList = new List<clsLedgerVO>();
                    objList = results.ToList();

                    foreach (var item in objList)
                    {
                        string str = "";
                        double amt = 0;

                        if (item.DR.HasValue && item.DR > 0)
                        {
                            str = "Yes";
                            amt = -item.DR.Value;
                        }
                        else
                        {
                            str = "No";
                            amt = item.CR.Value;
                        }

                        string strAmt = String.Format("{0:0.00}", amt);

                        m_streamWriter.WriteLine("<ALLLEDGERENTRIES.LIST>");
                        m_streamWriter.WriteLine("<LEDGERNAME>" + item.LedgerName + "</LEDGERNAME>");
                        m_streamWriter.WriteLine("<GSTCLASS/>");
                        m_streamWriter.WriteLine("<ISDEEMEDPOSITIVE>" + str + "</ISDEEMEDPOSITIVE>");
                        m_streamWriter.WriteLine("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        m_streamWriter.WriteLine("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        m_streamWriter.WriteLine("<ISPARTYLEDGER>" + str + "</ISPARTYLEDGER>");
                        m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
                        if (item.CR.HasValue && item.CR > 0)
                        {
                            m_streamWriter.WriteLine("<CATEGORYALLOCATIONS.LIST>");
                            m_streamWriter.WriteLine("<CATEGORY>Primary Cost Category</CATEGORY>");
                            m_streamWriter.WriteLine("<COSTCENTREALLOCATIONS.LIST>");
                            m_streamWriter.WriteLine("<NAME>" + item.Clinic + "</NAME>");
                            m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
                            m_streamWriter.WriteLine("</COSTCENTREALLOCATIONS.LIST>");
                            m_streamWriter.WriteLine("</CATEGORYALLOCATIONS.LIST>");
                        }
                        m_streamWriter.WriteLine("</ALLLEDGERENTRIES.LIST>");
                    }

                    m_streamWriter.WriteLine("</Voucher>".ToUpper());
                    m_streamWriter.WriteLine("</TALLYMESSAGE>");

                }


            }



            return m_streamWriter;
        }
        private StreamWriter TransferItemSaleReturn(StreamWriter m_streamWriter)
        {

            //===============================
            //'    Transfer Item sales Return


            List<KeyValue> IDList = new List<KeyValue>();

            var resIDList = from a in myAccounts.ItemSaleReturnLedgers
                            group a by a.UnitID into grouped
                            select new KeyValue { Key = grouped.First().UnitID, Value = grouped.First().Narration };


            IDList = resIDList.ToList();

            if (IDList != null && IDList.Count > 0)
            {

                foreach (var mID in IDList)
                {
                    m_streamWriter.WriteLine(@"<TALLYMESSAGE xmlns:UDF=" + qt + "TallyUDF" + qt + ">"); // + ">");
                    m_streamWriter.WriteLine(@"<Voucher REMOTEID=".ToUpper() + qt + qt + " VCHTYPE=" + qt + "Payment" + qt + " ACTION=" + qt + "Create" + qt + ">");
                    m_streamWriter.WriteLine("<Date>".ToUpper() + myAccounts.ExportDate.ToString("yyyyMMdd") + "</Date>".ToUpper());
                    m_streamWriter.WriteLine("<GUID></GUID>");
                    m_streamWriter.WriteLine("<NARRATION>" + mID.Value + "</NARRATION>");
                    m_streamWriter.WriteLine("<VOUCHERTYPENAME>" + "Payment" + "</VOUCHERTYPENAME>");
                    m_streamWriter.WriteLine("<PARTYLEDGERNAME>" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CashLedgerName + "</PARTYLEDGERNAME>");
                    m_streamWriter.WriteLine("<CSTFORMISSUETYPE/>");
                    m_streamWriter.WriteLine("<CSTFORMRECVTYPE/>");
                    m_streamWriter.WriteLine("<FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE>");
                    m_streamWriter.WriteLine("<VCHGSTCLASS/>");
                    m_streamWriter.WriteLine("<DIFFACTUALQTY>No</DIFFACTUALQTY>");
                    m_streamWriter.WriteLine("<AUDITED>No</AUDITED>");
                    m_streamWriter.WriteLine("<FORJOBCOSTING>No</FORJOBCOSTING>");
                    m_streamWriter.WriteLine("<ISOPTIONAL>No</ISOPTIONAL>");
                    m_streamWriter.WriteLine("<EFFECTIVEDATE>" + myAccounts.ExportDate.ToString("yyyyMMdd") + "</EFFECTIVEDATE>");
                    m_streamWriter.WriteLine("<USEFORINTEREST>No</USEFORINTEREST>");
                    m_streamWriter.WriteLine("<USEFORGAINLOSS>No</USEFORGAINLOSS>");
                    m_streamWriter.WriteLine("<USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER>");
                    m_streamWriter.WriteLine("<USEFORCOMPOUND>No</USEFORCOMPOUND>");
                    m_streamWriter.WriteLine("<ALTERID> 2666</ALTERID>");
                    m_streamWriter.WriteLine("<EXCISEOPENING>No</EXCISEOPENING>");
                    m_streamWriter.WriteLine("<USEFORFINALPRODUCTION>No</USEFORFINALPRODUCTION>");
                    m_streamWriter.WriteLine("<ISCANCELLED>No</ISCANCELLED>");
                    m_streamWriter.WriteLine("<HASCASHFLOW>Yes</HASCASHFLOW>");
                    m_streamWriter.WriteLine("<ISPOSTDATED>No</ISPOSTDATED>");
                    m_streamWriter.WriteLine("<USETRACKINGNUMBER>No</USETRACKINGNUMBER>");
                    m_streamWriter.WriteLine("<ISINVOICE>No</ISINVOICE>");
                    m_streamWriter.WriteLine("<MFGJOURNAL>No</MFGJOURNAL>");
                    m_streamWriter.WriteLine("<HASDISCOUNTS>No</HASDISCOUNTS>");
                    m_streamWriter.WriteLine("<ASPAYSLIP>No</ASPAYSLIP>");
                    m_streamWriter.WriteLine("<ISCOSTCENTRE>No</ISCOSTCENTRE>");
                    m_streamWriter.WriteLine("<ISDELETED>No</ISDELETED>");
                    m_streamWriter.WriteLine("<ASORIGINAL>No</ASORIGINAL>");

                    var results = from a in myAccounts.ItemSaleReturnLedgers
                                  where a.UnitID == (long)mID.Key
                                  orderby a.DR descending
                                  select a;

                    List<clsLedgerVO> objList = new List<clsLedgerVO>();
                    objList = results.ToList();

                    foreach (var item in objList)
                    {
                        string str = "";
                        string strPriVch = "";
                        double amt = 0;

                        if (item.DR.HasValue && item.DR > 0)
                        {
                            str = "Yes";
                            strPriVch = "No";
                            amt = -item.DR.Value;
                        }
                        else
                        {
                            str = "No";
                            strPriVch = "Yes";
                            amt = item.CR.Value;
                        }
                        string strAmt = String.Format("{0:0.00}", amt);

                        m_streamWriter.WriteLine("<ALLLEDGERENTRIES.LIST>");
                        m_streamWriter.WriteLine("<LEDGERNAME>" + item.LedgerName + "</LEDGERNAME>");
                        m_streamWriter.WriteLine("<GSTCLASS/>");
                        m_streamWriter.WriteLine("<ISDEEMEDPOSITIVE>" + str + "</ISDEEMEDPOSITIVE>");
                        m_streamWriter.WriteLine("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        m_streamWriter.WriteLine("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        m_streamWriter.WriteLine("<ISPARTYLEDGER>" + strPriVch + "</ISPARTYLEDGER>");
                        m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");

                        if (item.DR.HasValue && item.DR > 0)
                        {
                            m_streamWriter.WriteLine("<CATEGORYALLOCATIONS.LIST>");
                            m_streamWriter.WriteLine("<CATEGORY>Primary Cost Category</CATEGORY>");
                            m_streamWriter.WriteLine("<COSTCENTREALLOCATIONS.LIST>");
                            m_streamWriter.WriteLine("<NAME>" + item.Clinic + "</NAME>");
                            m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
                            m_streamWriter.WriteLine("</COSTCENTREALLOCATIONS.LIST>");
                            m_streamWriter.WriteLine("</CATEGORYALLOCATIONS.LIST>");
                        }
                        m_streamWriter.WriteLine("</ALLLEDGERENTRIES.LIST>");
                    }

                    m_streamWriter.WriteLine("</Voucher>".ToUpper());
                    m_streamWriter.WriteLine("</TALLYMESSAGE>");

                }


            }



            return m_streamWriter;
        }
        
        private StreamWriter TransferSelfBills(StreamWriter m_streamWriter)
        {

            //===============================
            //'    Transfer Self Bills
           

                List<KeyValue> IDList = new List<KeyValue>();

                var resIDList = from a in myAccounts.OPDSelfBillLedgers
                                group a by a.UnitID into grouped
                                select new KeyValue { Key = grouped.First().UnitID, Value = grouped.First().Narration };


                IDList = resIDList.ToList();

                if (IDList != null && IDList.Count > 0)
                {

                    foreach (var mID in IDList)
                    {
                        m_streamWriter.WriteLine(@"<TALLYMESSAGE xmlns:UDF=" + qt + "TallyUDF" + qt + ">"); // + ">");
                        m_streamWriter.WriteLine(@"<Voucher REMOTEID=".ToUpper() + qt + qt + " VCHTYPE=" + qt + "Journal" + qt + " ACTION=" + qt + "Create" + qt + ">");
                        m_streamWriter.WriteLine("<Date>".ToUpper() + myAccounts.ExportDate.ToString("yyyyMMdd") + "</Date>".ToUpper());
                        m_streamWriter.WriteLine("<GUID></GUID>");
                        m_streamWriter.WriteLine("<NARRATION>" + mID.Value + "</NARRATION>");
                        m_streamWriter.WriteLine("<VOUCHERTYPENAME>" + "Journal" + "</VOUCHERTYPENAME>");
                        m_streamWriter.WriteLine("<PARTYLEDGERNAME>" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CashLedgerName + "</PARTYLEDGERNAME>");
                        m_streamWriter.WriteLine("<CSTFORMISSUETYPE/>");
                        m_streamWriter.WriteLine("<CSTFORMRECVTYPE/>");
                        m_streamWriter.WriteLine("<FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE>");
                        m_streamWriter.WriteLine("<VCHGSTCLASS/>");
                        m_streamWriter.WriteLine("<DIFFACTUALQTY>No</DIFFACTUALQTY>");
                        m_streamWriter.WriteLine("<AUDITED>No</AUDITED>");
                        m_streamWriter.WriteLine("<FORJOBCOSTING>No</FORJOBCOSTING>");
                        m_streamWriter.WriteLine("<ISOPTIONAL>No</ISOPTIONAL>");
                        m_streamWriter.WriteLine("<EFFECTIVEDATE>" + myAccounts.ExportDate.ToString("yyyyMMdd") + "</EFFECTIVEDATE>");
                        m_streamWriter.WriteLine("<USEFORINTEREST>No</USEFORINTEREST>");
                        m_streamWriter.WriteLine("<USEFORGAINLOSS>No</USEFORGAINLOSS>");
                        m_streamWriter.WriteLine("<USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER>");
                        m_streamWriter.WriteLine("<USEFORCOMPOUND>No</USEFORCOMPOUND>");
                        m_streamWriter.WriteLine("<ALTERID> 2666</ALTERID>");
                        m_streamWriter.WriteLine("<EXCISEOPENING>No</EXCISEOPENING>");
                        m_streamWriter.WriteLine("<USEFORFINALPRODUCTION>No</USEFORFINALPRODUCTION>");
                        m_streamWriter.WriteLine("<ISCANCELLED>No</ISCANCELLED>");
                        m_streamWriter.WriteLine("<HASCASHFLOW>Yes</HASCASHFLOW>");
                        m_streamWriter.WriteLine("<ISPOSTDATED>No</ISPOSTDATED>");
                        m_streamWriter.WriteLine("<USETRACKINGNUMBER>No</USETRACKINGNUMBER>");
                        m_streamWriter.WriteLine("<ISINVOICE>No</ISINVOICE>");
                        m_streamWriter.WriteLine("<MFGJOURNAL>No</MFGJOURNAL>");
                        m_streamWriter.WriteLine("<HASDISCOUNTS>No</HASDISCOUNTS>");
                        m_streamWriter.WriteLine("<ASPAYSLIP>No</ASPAYSLIP>");
                        m_streamWriter.WriteLine("<ISCOSTCENTRE>No</ISCOSTCENTRE>");
                        m_streamWriter.WriteLine("<ISDELETED>No</ISDELETED>");
                        m_streamWriter.WriteLine("<ASORIGINAL>No</ASORIGINAL>");

                        var results = from a in myAccounts.OPDSelfBillLedgers
                                      where a.UnitID == (long)mID.Key
                                      orderby a.DR descending
                                      select a;

                        List<clsLedgerVO> objList = new List<clsLedgerVO>();
                        objList = results.ToList();

                        foreach (var item in objList)
                        {
                            string str = "";
                            double amt = 0;

                            if (item.DR.HasValue && item.DR > 0)
                            {
                                str = "Yes";
                                amt = -item.DR.Value;
                            }
                            else
                            {
                                str = "No";
                                amt = item.CR.Value;
                            }

                            string strAmt = String.Format("{0:0.00}", amt);

                            m_streamWriter.WriteLine("<ALLLEDGERENTRIES.LIST>");
                            m_streamWriter.WriteLine("<LEDGERNAME>" + item.LedgerName + "</LEDGERNAME>");
                            m_streamWriter.WriteLine("<GSTCLASS/>");
                            m_streamWriter.WriteLine("<ISDEEMEDPOSITIVE>" + str + "</ISDEEMEDPOSITIVE>");
                            m_streamWriter.WriteLine("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                            m_streamWriter.WriteLine("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                            m_streamWriter.WriteLine("<ISPARTYLEDGER>" + str + "</ISPARTYLEDGER>");
                            m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
                            if (item.CR.HasValue && item.CR > 0)
                            {
                                m_streamWriter.WriteLine("<CATEGORYALLOCATIONS.LIST>");
                                m_streamWriter.WriteLine("<CATEGORY>Primary Cost Category</CATEGORY>");
                                m_streamWriter.WriteLine("<COSTCENTREALLOCATIONS.LIST>");
                                m_streamWriter.WriteLine("<NAME>" + item.Clinic + "</NAME>");
                                m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
                                m_streamWriter.WriteLine("</COSTCENTREALLOCATIONS.LIST>");
                                m_streamWriter.WriteLine("</CATEGORYALLOCATIONS.LIST>");
                            }
                            m_streamWriter.WriteLine("</ALLLEDGERENTRIES.LIST>");
                        }

                        m_streamWriter.WriteLine("</Voucher>".ToUpper());
                        m_streamWriter.WriteLine("</TALLYMESSAGE>");

                    }


                }
            

            return m_streamWriter;
        
        }
        private StreamWriter TransferRefundOfBills(StreamWriter m_streamWriter)
        {
            //===============================
            //'    Transfer Refund of Bills

           

                List<KeyValue> IDList = new List<KeyValue>();

                var resIDList = from a in myAccounts.OPDRefundOfBillLedgers
                                group a by a.UnitID into grouped
                                select new KeyValue { Key = grouped.First().UnitID, Value = grouped.First().Narration };


                IDList = resIDList.ToList();

                if (IDList != null && IDList.Count > 0)
                {

                    foreach (var mID in IDList)
                    {
                        m_streamWriter.WriteLine(@"<TALLYMESSAGE xmlns:UDF=" + qt + "TallyUDF" + qt + ">"); // + ">");
                        m_streamWriter.WriteLine(@"<Voucher REMOTEID=".ToUpper() + qt + qt + " VCHTYPE=" + qt + "Payment" + qt + " ACTION=" + qt + "Create" + qt + ">");
                        m_streamWriter.WriteLine("<Date>".ToUpper() + myAccounts.ExportDate.ToString("yyyyMMdd") + "</Date>".ToUpper());
                        m_streamWriter.WriteLine("<GUID></GUID>");
                        m_streamWriter.WriteLine("<NARRATION>" + mID.Value + "</NARRATION>");
                        m_streamWriter.WriteLine("<VOUCHERTYPENAME>" + "Payment" + "</VOUCHERTYPENAME>");
                        m_streamWriter.WriteLine("<PARTYLEDGERNAME>" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CashLedgerName + "</PARTYLEDGERNAME>");
                        m_streamWriter.WriteLine("<CSTFORMISSUETYPE/>");
                        m_streamWriter.WriteLine("<CSTFORMRECVTYPE/>");
                        m_streamWriter.WriteLine("<FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE>");
                        m_streamWriter.WriteLine("<VCHGSTCLASS/>");
                        m_streamWriter.WriteLine("<DIFFACTUALQTY>No</DIFFACTUALQTY>");
                        m_streamWriter.WriteLine("<AUDITED>No</AUDITED>");
                        m_streamWriter.WriteLine("<FORJOBCOSTING>No</FORJOBCOSTING>");
                        m_streamWriter.WriteLine("<ISOPTIONAL>No</ISOPTIONAL>");
                        m_streamWriter.WriteLine("<EFFECTIVEDATE>" + myAccounts.ExportDate.ToString("yyyyMMdd") + "</EFFECTIVEDATE>");
                        m_streamWriter.WriteLine("<USEFORINTEREST>No</USEFORINTEREST>");
                        m_streamWriter.WriteLine("<USEFORGAINLOSS>No</USEFORGAINLOSS>");
                        m_streamWriter.WriteLine("<USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER>");
                        m_streamWriter.WriteLine("<USEFORCOMPOUND>No</USEFORCOMPOUND>");
                        m_streamWriter.WriteLine("<ALTERID> 2666</ALTERID>");
                        m_streamWriter.WriteLine("<EXCISEOPENING>No</EXCISEOPENING>");
                        m_streamWriter.WriteLine("<USEFORFINALPRODUCTION>No</USEFORFINALPRODUCTION>");
                        m_streamWriter.WriteLine("<ISCANCELLED>No</ISCANCELLED>");
                        m_streamWriter.WriteLine("<HASCASHFLOW>Yes</HASCASHFLOW>");
                        m_streamWriter.WriteLine("<ISPOSTDATED>No</ISPOSTDATED>");
                        m_streamWriter.WriteLine("<USETRACKINGNUMBER>No</USETRACKINGNUMBER>");
                        m_streamWriter.WriteLine("<ISINVOICE>No</ISINVOICE>");
                        m_streamWriter.WriteLine("<MFGJOURNAL>No</MFGJOURNAL>");
                        m_streamWriter.WriteLine("<HASDISCOUNTS>No</HASDISCOUNTS>");
                        m_streamWriter.WriteLine("<ASPAYSLIP>No</ASPAYSLIP>");
                        m_streamWriter.WriteLine("<ISCOSTCENTRE>No</ISCOSTCENTRE>");
                        m_streamWriter.WriteLine("<ISDELETED>No</ISDELETED>");
                        m_streamWriter.WriteLine("<ASORIGINAL>No</ASORIGINAL>");

                        var results = from a in myAccounts.OPDRefundOfBillLedgers
                                      where a.UnitID == (long)mID.Key
                                      orderby a.DR descending
                                      select a;

                        List<clsLedgerVO> objList = new List<clsLedgerVO>();
                        objList = results.ToList();

                        foreach (var item in objList)
                        {
                            string str = "";
                            string strPriVch = "";
                            double amt = 0;

                            if (item.DR.HasValue && item.DR > 0)
                            {
                                str = "Yes";
                                strPriVch = "No";
                                amt = -item.DR.Value;
                            }
                            else
                            {
                                str = "No";
                                strPriVch = "Yes";
                                amt = item.CR.Value;
                            }
                            string strAmt = String.Format("{0:0.00}", amt);

                            m_streamWriter.WriteLine("<ALLLEDGERENTRIES.LIST>");
                            m_streamWriter.WriteLine("<LEDGERNAME>" + item.LedgerName + "</LEDGERNAME>");
                            m_streamWriter.WriteLine("<GSTCLASS/>");
                            m_streamWriter.WriteLine("<ISDEEMEDPOSITIVE>" + str + "</ISDEEMEDPOSITIVE>");
                            m_streamWriter.WriteLine("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                            m_streamWriter.WriteLine("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                            m_streamWriter.WriteLine("<ISPARTYLEDGER>" + strPriVch + "</ISPARTYLEDGER>");
                            m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");

                            if (item.DR.HasValue && item.DR > 0)
                            {
                                m_streamWriter.WriteLine("<CATEGORYALLOCATIONS.LIST>");
                                m_streamWriter.WriteLine("<CATEGORY>Primary Cost Category</CATEGORY>");
                                m_streamWriter.WriteLine("<COSTCENTREALLOCATIONS.LIST>");
                                m_streamWriter.WriteLine("<NAME>" + item.Clinic + "</NAME>");
                                m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
                                m_streamWriter.WriteLine("</COSTCENTREALLOCATIONS.LIST>");
                                m_streamWriter.WriteLine("</CATEGORYALLOCATIONS.LIST>");
                            }
                            m_streamWriter.WriteLine("</ALLLEDGERENTRIES.LIST>");
                        }

                        m_streamWriter.WriteLine("</Voucher>".ToUpper());
                        m_streamWriter.WriteLine("</TALLYMESSAGE>");

                    }


                }
            

            return m_streamWriter;
        }
    }
}
