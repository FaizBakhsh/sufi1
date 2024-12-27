using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using POSRestaurant.forms;
using System.Xml;
//using POSRestaurant.dbsources;

namespace POSRestaurant.classes
{
    public class Clsdbcon
    {
        public static SqlConnection con;
        public static string Database;
        public static string Password;
        public static string Server;
        public static string User;
        public static string constring = "";
     
        private SqlConnection connection;
        public static POSRestaurant.forms.frmdbconnector Connector = new POSRestaurant.forms.frmdbconnector();

        public Clsdbcon()
        {
            try
            {
                this.connection = new SqlConnection();
                this.connection.ConnectionString = this.getConnectionString();
            }
            catch (Exception ex)
            {
                               
            }
        }

        public string getConnectionString()
        {
            return POSRestaurant.Properties.Settings.Default.ConnectionString;
        }
        public void addcolumns()
        {
            string query = "";
            try
            {
                if (this.connection.State == ConnectionState.Open)
                    this.connection.Close();
                this.connection.Open();
                SqlCommand com = new SqlCommand(query, this.connection);
                int res = 0;
                try
                {
                    query = "CREATE TABLE [dbo].[VoucherTrack](	[Id] [int] IDENTITY(1,1) NOT NULL,	[saleid] [int] NULL,	[VoucherId] [int] NULL,	[amount] [float] NULL, CONSTRAINT [PK_VoucherTrack] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();


                    query = "ALTER TABLE [dbo].[VoucherTrack]  WITH CHECK ADD  CONSTRAINT [FK_VoucherTrack_Sale] FOREIGN KEY([saleid]) REFERENCES [dbo].[Sale] ([Id]) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                    query = "ALTER TABLE [dbo].[VoucherTrack] CHECK CONSTRAINT [FK_VoucherTrack_Sale]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                    query = "ALTER TABLE [dbo].[VoucherTrack]  WITH CHECK ADD  CONSTRAINT [FK_VoucherTrack_VoucherKeys] FOREIGN KEY([VoucherId]) REFERENCES [dbo].[VoucherKeys] ([Id])";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                    query = "ALTER TABLE [dbo].[VoucherTrack] CHECK CONSTRAINT [FK_VoucherTrack_VoucherKeys]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                
                
                try
                {
                    query = "ALTER TABLE PurchasereturnDetails DROP CONSTRAINT FK_PurchasereturnDetails_Purchase ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                   
                try
                {
                    query = "ALTER TABLE PurchasereturnDetails DROP CONSTRAINT FK_PurchasereturnDetails_Purchase ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                
                 try
                {
                    query = "ALTER TABLE InventoryAccount DROP CONSTRAINT FK_InventoryAccount_item ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                 try
                 {
                     query = "ALTER TABLE [dbo].[menuitem]  ADD submenugroupid varchar(50) NULL ";
                     com = new SqlCommand(query, this.connection);
                     res = com.ExecuteNonQuery();
                 }
                 catch (Exception ex)
                 {


                 }
                try
                {
                    query = "ALTER TABLE [dbo].[PurchasereturnDetails]  ADD Date date NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[PurchasereturnDetails]  ADD PDID int NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[Currency]  ADD Rate float NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[Supplier]  ADD CreditLimit float NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[Attachmenu1]  ADD PrintKitchen varchar(50) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[Sale]  ADD CurrencyRate float NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[MenuItem]  ADD Name2 nvarchar(500) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[ModifierFlavour]  ADD Name2 nvarchar(500) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[RuntimeModifier]  ADD Name2 nvarchar(500) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[Modifier]  ADD Name2 nvarchar(500) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[Discard]  ADD userid int NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[CompleteWaste]  ADD type varchar(50) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[Mailsetting]  ADD ssl bit NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[SaleDetails]  ADD GrossPrice varchar(100) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    
                    
                }
                try
                {
                    query = "ALTER TABLE [dbo].[SupplierAccount]  ADD invoiceno varchar(150) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[Purchase]  ADD PaymentStatus varchar(100) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                    query = "ALTER TABLE Purchase ADD CONSTRAINT df_PaymentStatus DEFAULT 'Pending' FOR PaymentStatus";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    
                }
                try
                {
                    query = "ALTER TABLE [dbo].[Sale]  ADD uploadstatusvoucher varchar(50) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                    query = "ALTER TABLE Sale ADD CONSTRAINT df_uploadstatusvoucher DEFAULT 'Pending' FOR uploadstatusvoucher";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                }

                  try
                {
                    query = "CREATE TABLE [dbo].[BankReconciliation](	[Id] [int] IDENTITY(1,1) NOT NULL,	[AccountId] [int] NULL,	[Date] [date] NULL,	[Debit] [float] NULL,	[Credit] [float] NULL,	[Balance] [float] NULL,	[Description] [nvarchar](max) NULL) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                }
                
                try
                {
                    query = "ALTER TABLE [dbo].[BankAccountPaymentSupplier]  ADD ClearanceStatus varchar(50) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                    query = "ALTER TABLE BankAccountPaymentSupplier ADD CONSTRAINT df_ClearanceStatus DEFAULT 'Pending' FOR ClearanceStatus";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                }
                try
                {
                    query = "ALTER TABLE [dbo].[BankAccountReceiptCustomer]  ADD ClearanceStatus varchar(50) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                    query = "ALTER TABLE BankAccountReceiptCustomer ADD CONSTRAINT df_ClearanceStatusRec DEFAULT 'Pending' FOR ClearanceStatus";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                }
                try
                {
                    query = "ALTER TABLE [dbo].[Sale]  ADD uploadstatusdiscountindividual varchar(50) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                    query = "ALTER TABLE Sale ADD CONSTRAINT df_uploadstatusdiscountindividual DEFAULT 'Pending' FOR uploadstatusdiscountindividual";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                }
                try
                {
                    query = "ALTER TABLE [dbo].[Sale]  ADD uploadmainstatusdelivery varchar(50) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                    query = "ALTER TABLE Sale ADD CONSTRAINT df_uploadmainstatusdelivery DEFAULT 'Pending' FOR uploadmainstatusdelivery";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                }
                try
                {
                    query = "ALTER TABLE [dbo].[Sale]  ADD uploadstatustakeaway varchar(50) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                    query = "ALTER TABLE Sale ADD CONSTRAINT df_uploadstatustakeaway DEFAULT 'Pending' FOR uploadstatustakeaway";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                }
                try
                {
                    query = "ALTER TABLE [dbo].[Sale]  ADD uploadstatusdeinein varchar(50) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                    query = "ALTER TABLE Sale ADD CONSTRAINT df_uploadstatusdeinein DEFAULT 'Pending' FOR uploadstatusdeinein";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                }
                try
                {
                    query = "CREATE TABLE [dbo].[Charity](	[ID] [int] IDENTITY(1,1) NOT NULL,	[Text] [nvarchar](max) NULL,	[Perc] [float] NULL) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();


                    query = "ALTER TABLE [dbo].[SaleDetails]  ADD Priority varchar(100) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                    query = "ALTER TABLE SaleDetails ADD CONSTRAINT df_Priority DEFAULT 'Normal' FOR Priority";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[EmployeeAttandance](	[Id] [int] IDENTITY(1,1) NOT NULL,	[EmpID] [int] NULL,	[Days] [float] NULL,	[Month] [nvarchar](500) NULL,	[branchid] [int] NULL, CONSTRAINT [PK_EmployeeSalary] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                    query = "ALTER TABLE [dbo].[EmployeeAttandance]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeSalary_Employees] FOREIGN KEY([EmpID]) REFERENCES [dbo].[Employees] ([Id])";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                    query = "ALTER TABLE [dbo].[EmployeeAttandance] CHECK CONSTRAINT [FK_EmployeeSalary_Employees]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {

                    query = "CREATE TABLE [dbo].[InventoryAllocation](	[Id] [int] IDENTITY(1,1) NOT NULL,	[branchid] [int] NULL,	[Percentage] [float] NULL,	[uploadstatus] [varchar](100) NULL, CONSTRAINT [PK_InventoryAllocation] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                    query = "ALTER TABLE [dbo].[InventoryAllocation]  WITH CHECK ADD  CONSTRAINT [FK_InventoryAllocation_Branch] FOREIGN KEY([branchid]) REFERENCES [dbo].[Branch] ([Id])";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                    query = "ALTER TABLE [dbo].[InventoryAllocation] CHECK CONSTRAINT [FK_InventoryAllocation_Branch]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[CashAccountPaymentSupplier]  ADD Status varchar(100) NULL CONSTRAINT CashAccountPaymentSupplier_default  DEFAULT 'Pending' ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[BankAccountPaymentSupplier]  ADD Status varchar(100) NULL CONSTRAINT bankPaymentSupplier_default  DEFAULT 'Pending' ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[BankAccountPaymentSupplier]  ADD supporting varbinary(MAX) NULL";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[CashAccountPaymentSupplier]  ADD supporting varbinary(MAX) NULL";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[BankAccountPaymentSupplier]  ADD supporting varbinary(MAX) NULL";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[BankAccountReceiptCustomer]  ADD Status varchar(100) NULL CONSTRAINT BankAccountReceiptCustomer_default  DEFAULT 'Pending' ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[JournalAccount]  ADD Status varchar(100) NULL CONSTRAINT JournalAccount_default  DEFAULT 'Pending' ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[JournalAccount]  ADD supporting varbinary(MAX) NULL";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                

                try
                {
                    query = "ALTER TABLE [dbo].[Delivery]  ADD PromisedTime varchar(200) NULL";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                 try
                {
                    query = "ALTER TABLE [dbo].[Sale]  ADD ProcessedTime datetime NULL";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[EmployeesAccount]  ADD Status varchar(100) NULL CONSTRAINT EmployeesAccount_default  DEFAULT 'Pending' ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[EmployeesAccount]  ADD supporting varbinary(MAX) NULL";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[SupplierAccount]  ADD Status varchar(100) NULL CONSTRAINT SupplierAccount_default  DEFAULT 'Pending' ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[SupplierAccount]  ADD supporting varbinary(MAX) NULL";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[CustomerAccount]  ADD Status varchar(100) NULL CONSTRAINT CustomerAccount22_default  DEFAULT 'Pending' ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {

                    query = "CREATE TABLE [dbo].[InventoryTransferApproval]([id] [int] IDENTITY(1,1) NOT NULL,	[Date] [date] NULL,	[Itemid] [int] NULL,	[TransferIn] [float] NULL,	[TransferOut] [float] NULL,	[branchid] [int] NULL,	[uploadstatus] [varchar](50) NULL,	[Onlineid] [int] NULL,	[Remarks] [varchar](max) NULL,	[sourcebranchid] [int] NULL,	[price] [float] NULL,	[total] [float] NULL,	[status] [varchar](50) NULL,	[NetTotal] [float] NULL,	[GST] [float] NULL,	[GSTAmount] [float] NULL,	[Discount] [float] NULL,	[DiscountAmount] [float] NULL,	[datedownload] [varchar](100) NULL,	[invoiceno] [varchar](500) NULL,	[barcode] [varchar](500) NULL, CONSTRAINT [PK_InventoryTransferApproval] PRIMARY KEY CLUSTERED (	[id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY] ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }

                
                try
                {

                    query = "ALTER TABLE [dbo].[Saledetailsrefund]   ADD MakeStatus varchar(200) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {

                    query = "ALTER TABLE [dbo].[CashAccountPaymentSupplier]   ADD Approveduserid int NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {

                    query = "ALTER TABLE [dbo].[SupplierAccount]   ADD Approveduserid int NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {

                    query = "ALTER TABLE [dbo].[EmployeesAccount]   ADD Approveduserid int NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {

                    query = "ALTER TABLE [dbo].[BankAccountPaymentSupplier]   ADD Approveduserid int NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {

                    query = "ALTER TABLE [dbo].[JournalAccount]   ADD Approveduserid int NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }

                try
                {

                    query = "ALTER TABLE AttachRecipe DROP CONSTRAINT FK_AttachRecipe_MenuItem; ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {

                    query = "ALTER TABLE [dbo].[InventoryTransferApproval]   ADD Approveduserid int NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {

                    query = "ALTER TABLE [dbo].[Delivery]   ADD Phone2 varchar(100) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {

                    query = "ALTER TABLE [dbo].[Sale]   ADD DiscountNaration varchar(600) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {

                    query = "ALTER TABLE [dbo].[Sale]   ADD VoucherNaration varchar(600) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {

                    query = "ALTER TABLE [dbo].[SaleDetails]   ADD ComplimentoryNarration varchar(600) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {

                    query = "ALTER TABLE [dbo].[Requisition]   ADD Approveduserid int NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {

                    query = "ALTER TABLE [dbo].[PurchaseOrder]   ADD Approveduserid int NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[Requisition](	[Id] [int] IDENTITY(1,1) NOT NULL,	[ItemId] [int] NULL,	[Quantity] [float] NULL,	[Price] [float] NULL,	[TotalAmount] [float] NULL,	[Remarks] [nvarchar](500) NULL,	[Status] [varchar](50) NULL,	[branchid] [int] NULL,	[Date] [date] NULL, CONSTRAINT [PK_Requisition] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex )
                {
                    
                 
                }
                try
                {

                    query = "ALTER TABLE [dbo].[RawItem]   ADD PrintBarcode varchar(50) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {

                    query = "ALTER TABLE [dbo].[CashSalesAccountsList]   ADD bankid varchar(50) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {

                    query = "ALTER TABLE [dbo].[InventoryTransfer]  ADD demanddate varchar(100) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {

                    query = "ALTER TABLE [dbo].[InventoryTransfer]  ADD Approveduserid int NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {

                    query = "ALTER TABLE [dbo].[Users]  ADD Signature varbinary(MAX) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {

                    query = "ALTER TABLE [dbo].[RawItem]  ADD PackingName varchar(200) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {

                    query = "ALTER TABLE [dbo].[Sale]  ADD serviceid varchar(100) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {

                    query = "ALTER TABLE [dbo].[MenuGroup]  ADD branchid int NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {
                   
                    query = "ALTER TABLE [dbo].[SerivceCharges]  ADD Title varchar(100) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[POSFee](	[Id] [int] IDENTITY(1,1) NOT NULL,	[Name] [varchar](100) NULL,	[Amount] [float] NULL, CONSTRAINT [PK_POSFee] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }

                try
                {
                    query = "CREATE TABLE [dbo].[BankDeposits](	[Id] [int] IDENTITY(1,1) NOT NULL,	[Date] [date] NULL,	[ActualAmount] [float] NULL,	[DepositedAmount] [float] NULL,[Onlineid] [int] NULL,[branchid] [int] NULL,	[Image] [varbinary](max) NULL,	[Status] [varchar](100) NULL, CONSTRAINT [PK_BankDeposits] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY] ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[InventoryTransfer]  WITH CHECK ADD  CONSTRAINT [FK_InventoryTransfer_Branch] FOREIGN KEY([branchid]) REFERENCES [dbo].[Branch] ([Id]) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                    query = "ALTER TABLE [dbo].[InventoryTransfer] CHECK CONSTRAINT [FK_InventoryTransfer_Branch] ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();


                    query = "ALTER TABLE [dbo].[InventoryTransfer]  WITH CHECK ADD  CONSTRAINT [FK_InventoryTransfer_InventoryTransfer] FOREIGN KEY([id]) REFERENCES [dbo].[InventoryTransfer] ([id]) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                    query = "ALTER TABLE [dbo].[InventoryTransfer] CHECK CONSTRAINT [FK_InventoryTransfer_InventoryTransfer] ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();


                    query = "CREATE INDEX idx_Date ON inventorytransfer (Date)";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                    query = "CREATE INDEX idx_Itemid ON inventorytransfer (Itemid) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                    query = "CREATE INDEX idx_branchid ON inventorytransfer (branchid)";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                    query = "CREATE INDEX idx_invoiceno ON inventorytransfer (invoiceno) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                    query = "CREATE INDEX idx_barcode ON inventorytransfer (barcode) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();


                    



                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE INDEX idx_saledate ON Sale (Date) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE INDEX idx_salebranchid ON Sale (branchid) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE INDEX idx_saleshiftid ON Sale (shiftid) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE INDEX idx_saleid ON SaleDetails (saleid) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE INDEX idx_menuitemid ON SaleDetails (menuitemid) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE INDEX idx_flavourid ON SaleDetails (Flavourid) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE INDEX idx_modifierid ON SaleDetails (ModifierId) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE INDEX idx_RunTimeModifierId ON SaleDetails (RunTimeModifierId) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE INDEX idx_saleidbill ON BillType (saleid) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE INDEX idx_saleidbill ON BillType (saleid) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE INDEX idx_MenuItemgroupid ON MenuItem (MenuGroupId) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    

                    query = "CREATE INDEX idx_closingitemid ON Closing (itemid) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE INDEX idx_closingdate ON Closing (date) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {

                    query = "CREATE INDEX idx_closinbranchid ON Closing (branchid) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                    query = "CREATE INDEX idx_closinRemaining ON Closing (Remaining) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                    
                }
                catch (Exception ex)
                {


                }
                try
                {

                    query = "CREATE INDEX idx_closinbranchid ON Discard (branchid) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();


                    query = "CREATE INDEX idx_DiscardDate ON Discard (Date) ";
                    com = new SqlCommand(query, this.connection);

                    query = "CREATE INDEX idx_Discard ON Discard (Discard) ";
                    com = new SqlCommand(query, this.connection);


                    res = com.ExecuteNonQuery();
                    query = "CREATE INDEX idx_staff ON Discard (staff) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                    query = "CREATE INDEX idx_closincompletewaste ON Discard (completewaste) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE INDEX idx_closingdate ON Closing (date) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE INDEX idx_TransferIn ON InventoryTransfer (TransferIn  ) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                  

                    query = "CREATE INDEX idx_Discarditemid ON Discard (itemid) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                   

                    query = "CREATE INDEX idx_InventoryTransferitemid ON InventoryTransfer (itemid) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                   

                    query = "CREATE INDEX idx_UOMID ON RawItem (UOMID) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                   

                    query = "CREATE INDEX idx_InventoryConsumeditemid ON InventoryConsumed (RawItemId) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                    query = "CREATE INDEX idx_InventoryConsumedbranchid ON InventoryConsumed (branchid) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                    query = "CREATE INDEX idx_InventoryConsumedDate ON InventoryConsumed (Date) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {

                    query = "CREATE INDEX idx_InventoryTransferbranchid ON InventoryTransfer (branchid) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }

                try
                {

                    query = "CREATE INDEX idx_InventoryTransfersourcebranchid ON InventoryTransfer (sourcebranchid) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {

                    query = "CREATE INDEX idx_PurchaseSupplierId ON Purchase (SupplierId) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {

                    query = "CREATE INDEX idx_PurchaseBranchCode ON Purchase (BranchCode) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {

                    query = "CREATE INDEX idx_PurchaseStoreCode ON Purchase (StoreCode) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {

                    query = "CREATE INDEX idx_PurchaseDetailsitemid ON PurchaseDetails (RawItemId) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {

                    query = "CREATE INDEX idx_PurchaseDetailsPurchaseId ON PurchaseDetails (PurchaseId) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }

                try
                {
                    query = "ALTER TABLE dbo.SaleDetails  ALTER COLUMN Comments VARCHAR(500)  NULL; ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    
                   
                }

                try
                {
                    query = "ALTER TABLE dbo.MenuGroup  ALTER COLUMN Name VARCHAR(500)  NULL; ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                    query = "CREATE INDEX idx_MenuGroupname ON MenuGroup (Name) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.TakeAway  ALTER COLUMN Saleid int  NULL; ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                    query = "CREATE INDEX indx_TakeAway ON TakeAway (Saleid) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                    query = "CREATE INDEX indx_DinInTables ON DinInTables (Saleid) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }

                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.MenuItem  ALTER COLUMN Name VARCHAR(500)  NULL; ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                    query = "CREATE INDEX idx_MenuItemname ON MenuItem (Name) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.BillType  ALTER COLUMN Type VARCHAR(200)  NULL; ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                    query = "CREATE INDEX idx_BillTypet ON BillType (Type) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }   
                try
                {
                    query = "ALTER TABLE dbo.Sale  ALTER COLUMN BillStatus VARCHAR(200)  NULL; ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                    query = "CREATE INDEX idx_salebille ON Sale (Billstatus) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[RawItem]  ADD LoosQTY varchar(100) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[Supplier]  ADD CatId int NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE DinInTables DROP CONSTRAINT FK_DinInTables_Sale ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[Saledetailsrefund]  ADD Terminal VARCHAR(500) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.CashAccountPurchase  ALTER COLUMN Description VARCHAR(500)  NULL; ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.CashAccountSales  ALTER COLUMN Description VARCHAR(500)  NULL; ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.PurchaseDetails  ADD Expiry VARCHAR(100)  NULL; ";
                  
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.CostSalesAccount  ALTER COLUMN Description VARCHAR(500)  NULL; ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.DiscountAccount  ALTER COLUMN Description VARCHAR(500)  NULL; ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.GSTAccount  ALTER COLUMN Description VARCHAR(500)  NULL; ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.SalesAccount  ALTER COLUMN Description VARCHAR(500)  NULL; ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.SupplierAccount  ALTER COLUMN Description VARCHAR(500)  NULL; ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.InventoryAccount  ALTER COLUMN Description VARCHAR(500)  NULL; ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                 try
                {
                    query = "ALTER TABLE [dbo].[EmployeesAccount]  ADD SalaryMonth varchar(100) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                 try
                 {
                     query = "ALTER TABLE [dbo].[InventoryTransfer]  ADD invoiceno varchar(500) NULL ";
                     com = new SqlCommand(query, this.connection);
                     res = com.ExecuteNonQuery();

                 }
                 catch (Exception ex)
                 {


                 }
                 try
                 {
                     query = "ALTER TABLE [dbo].[InventoryTransfer]  ADD barcode varchar(500) NULL ";
                     com = new SqlCommand(query, this.connection);
                     res = com.ExecuteNonQuery();

                 }
                 catch (Exception ex)
                 {


                 }
                try
                {
                    query = "ALTER TABLE [dbo].[RawItem]  ADD Status varchar(100) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[SaleDetails]  ADD OnlineId int NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                }
                try
                {
                    query = "ALTER TABLE [dbo].[closing]  ADD storeid int NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[Discard]  ADD storeid int NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[Production]  ADD storeid int NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[Demand]  ADD Closing float NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[Demand]  ADD OnlineId int NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[RawItem]  ADD Status varchar(50) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[Sale]  ADD POSFee float NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "EXEC sp_MSforeachtable ' if not exists (select * from sys.columns                where object_id = object_id(''?'')               and name = ''uploadstatus'')  begin    ALTER TABLE ? ADD uploadstatus varchar(100)  NULL DEFAULT ''Pending''; end';";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[Slider](	[Id] [int] IDENTITY(1,1) NOT NULL,	[imageurl] [varchar](max) NULL,	[Status] [varchar](50) NULL, CONSTRAINT [PK_Slider] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY] ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[AppUsers](	[Id] [int] IDENTITY(1,1) NOT NULL,	[Name] [nvarchar](max) NULL,	[Email] [nvarchar](max) NULL,	[Phone] [nvarchar](max) NULL,	[Address] [nvarchar](max) NULL,	[City] [nvarchar](max) NULL,	[DOB] [nvarchar](max) NULL,	[password] [nvarchar](max) NULL, CONSTRAINT [PK_AppUsers] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[DinInTables]  ADD Dsid varchar(100) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }

                try
                {
                    query = "ALTER TABLE [dbo].[InventoryTransfer]  ADD datedownload varchar(100) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[ResturantStaff]  ADD CashierId int NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[MenuVarient](	[Id] [int] IDENTITY(1,1) NOT NULL,	[MenuItemId] [int] NULL,	[VarientId] [int] NULL,	[Status] [varchar](50) NULL, CONSTRAINT [PK_MenuVarient] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                 

                    query = "ALTER TABLE [dbo].[MenuVarient]  WITH CHECK ADD  CONSTRAINT [FK_MenuVarient_MenuItem] FOREIGN KEY([MenuItemId]) REFERENCES [dbo].[MenuItem] ([Id])";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                    query = "ALTER TABLE [dbo].[MenuVarient] CHECK CONSTRAINT [FK_MenuVarient_MenuItem]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[Promotions](	[Id] [int] IDENTITY(1,1) NOT NULL,	[MenuItemId] [int] NULL,	[Date] [date] NULL,	[Price] [float] NULL,	[GrossPrice] [float] NULL,CONSTRAINT [PK_Promotions] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[DiscountKeys]  ADD limit varchar(100) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[ChartofAccounts]  ADD SubAccount varchar(200) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[SBRInfo](	[Id] [int] IDENTITY(1,1) NOT NULL,	[NTN] [varchar](max) NULL,	[Name] [varchar](max) NULL,	[Address] [varchar](max) NULL,	[MachineId] [varchar](max) NULL,	[POSId] [varchar](max) NULL, CONSTRAINT [PK_SBRInfo] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[MenuItem]  ADD OptionalModifier varchar(100) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[SaleDetails]  ADD NotificationStatus varchar(100) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                //try
                //{
                //    query = "ALTER TABLE [dbo].[Customers]  ADD DirectCashout varchar(100) NULL ";
                //    com = new SqlCommand(query, this.connection);
                //    res = com.ExecuteNonQuery();

                //}
                //catch (Exception ex)
                //{


                //}
                try
                {
                    query = "ALTER TABLE [dbo].[DinInTables]  ADD ResId varchar(100) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[Sale]  ADD applyservicecharges varchar(100) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                       
                }
                catch (Exception ex)
                {

                }
                try
                {
                    query = "CREATE TABLE [dbo].[DayBook](	[Id] [int] IDENTITY(1,1) NOT NULL,	[AccountId] [int] NULL,	[Type] [varchar](100) NULL,	[Branchid] [int] NULL, CONSTRAINT [PK_DayBook] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                    query = "ALTER TABLE [dbo].[DayBook]  WITH CHECK ADD  CONSTRAINT [FK_DayBook_ChartofAccounts] FOREIGN KEY([AccountId]) REFERENCES [dbo].[ChartofAccounts] ([Id])";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                    query = "ALTER TABLE [dbo].[DayBook] CHECK CONSTRAINT [FK_DayBook_ChartofAccounts]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[StockEstimation](	[id] [int] IDENTITY(1,1) NOT NULL,	[Flavouid] [varchar](50) NULL,	[Menuitemid] [int] NULL,	[Quantity] [float] NULL,	[Date] [date] NULL,	[Status] [varchar](50) NULL,	[OrderNo] [varchar](100) NULL, CONSTRAINT [PK_StockEstimation] PRIMARY KEY CLUSTERED (	[id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                   
                }
                catch (Exception ex)
                {


                }

                try
                {
                    query = "CREATE TABLE [dbo].[Production](	[Id] [int] IDENTITY(1,1) NOT NULL,	[Date] [date] NULL,	[ItemId] [int] NULL,	[Quantity] [float] NULL,	[Userid] [int] NULL,	[Status] [varchar](50) NULL,	[branchid] [int] NULL, CONSTRAINT [PK_Production] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[RecipeProduction](	[Id] [int] NOT NULL,	[ItemId] [int] NULL,	[RawItemId] [int] NULL,	[UOMCId] [int] NULL,	[Quantity] [float] NULL, CONSTRAINT [PK_RecipeProduction] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE PROCEDURE [dbo].[str_sale] 		@UploadstatusDelivery varchar(100) = '', 	@branchid int = 0, 	@discountid varchar(100) = '',	@date  varchar(100)='',	@time  varchar(100) ='',	@UserId varchar(100) ='', 	 @TotalBill varchar(100) ='', 	@Discount varchar(100) ='',	@NetBill varchar(100) ='',	@BillType varchar(100) ='',	@OrderType varchar(100) ='',	@GST varchar(100) ='',	@BillStatus varchar(100) ='' ,	@Terminal varchar(100) ='',	@uploadstatus varchar(100) ='',	@OrderStatus varchar(100) ='',	@GSTPerc varchar(100) ='',	@Shiftid int =0,	@customer varchar(max) ='',	@servicecharges varchar(100) ='',	@TerminalOrder varchar(max) =''  AS BEGIN		SET NOCOUNT ON;    insert into sale (UploadstatusDelivery,branchid,discountid,date,time,UserId,TotalBill,Discount,NetBill,BillType,OrderType,GST,BillStatus,Terminal,uploadstatus,OrderStatus,GSTPerc,Shiftid,customer,servicecharges,TerminalOrder)										values (@UploadstatusDelivery,@branchid,@discountid,@date,@time,@UserId,@TotalBill,@Discount,@NetBill,@BillType,@OrderType,@GST,@BillStatus,@Terminal,@uploadstatus,@OrderStatus,@GSTPerc,@Shiftid,@customer,@servicecharges,@TerminalOrder) END";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                   
                }
                catch (Exception ex)
                {


                }
                try
                {
                   
                    query = "ALTER PROCEDURE [dbo].[str_sale] @UploadstatusDelivery varchar(100) = '', @branchid int = 0, 	@discountid varchar(100) = '',@date  varchar(100)='',	@time  varchar(100) ='',	@UserId varchar(100) ='', 	 @TotalBill varchar(100) ='', @Discount varchar(100) ='',	@NetBill varchar(100) ='',@BillType varchar(100) ='',	@OrderType varchar(100) ='',	@GST varchar(100) ='',	@BillStatus varchar(100) ='' ,@Terminal varchar(100) ='',	@uploadstatus varchar(100) ='',@OrderStatus varchar(100) ='',	@GSTPerc varchar(100) ='',	@Shiftid int =0,	@customer varchar(max) ='',	@servicecharges varchar(100) ='',@TerminalOrder varchar(max) ='' AS BEGIN	 declare @id int,@autoinc int; set @id  =(select max(id+1) from Sale) set @autoinc=(SELECT COLUMNPROPERTY(OBJECT_ID('sale'),'id','isidentity')) SET NOCOUNT ON;   if(@autoinc=1) BEGIN insert into sale (UploadstatusDelivery,branchid,discountid,date,time,UserId,TotalBill,Discount,NetBill,BillType,OrderType,GST,BillStatus,Terminal,uploadstatus,OrderStatus,GSTPerc,Shiftid,customer,servicecharges,TerminalOrder)										values ( @UploadstatusDelivery,@branchid,@discountid,@date,@time,@UserId,@TotalBill,@Discount,@NetBill,@BillType,@OrderType,@GST,@BillStatus,@Terminal,@uploadstatus,@OrderStatus,@GSTPerc,@Shiftid,@customer,@servicecharges,@TerminalOrder)  END ELSE BEGIN insert into sale (Id,UploadstatusDelivery,branchid,discountid,date,time,UserId,TotalBill,Discount,NetBill,BillType,OrderType,GST,BillStatus,Terminal,uploadstatus,OrderStatus,GSTPerc,Shiftid,customer,servicecharges,TerminalOrder)										values (@id, @UploadstatusDelivery,@branchid,@discountid,@date,@time,@UserId,@TotalBill,@Discount,@NetBill,@BillType,@OrderType,@GST,@BillStatus,@Terminal,@uploadstatus,@OrderStatus,@GSTPerc,@Shiftid,@customer,@servicecharges,@TerminalOrder) END END";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[Closing](	[id] [numeric](18, 0) NOT NULL,	[itemid] [int] NULL,	[date] [date] NULL,	[quantity] [float] NULL,	[Discard] [float] NULL,	[Remaining] [float] NULL,	[staff] [float] NULL,	[completewaste] [float] NULL,	[uploadstatus] [varchar](50) NULL,	[Remarks] [varchar](50) NULL,[Onlineid] [int] NULL,	[branchid] [int] NULL, CONSTRAINT [PK_Closing] PRIMARY KEY CLUSTERED (	[id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE PROCEDURE [dbo].[str_deliverydelete]	@onlineid int = 0  AS BEGIN		SET NOCOUNT ON;				delete from Saledetails where  saleid=(select id from sale where onlineid=@onlineid);		delete from Delivery where SaleId=(select id from sale where onlineid=@onlineid);		delete from Sale where onlineid=@onlineid end";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[FeedBackQuestions](	[Id] [int] IDENTITY(1,1) NOT NULL,	[Question] [nvarchar](max) NULL,	[Option1] [nvarchar](max) NULL,	[Option2] [nvarchar](max) NULL,	[Option3] [nvarchar](max) NULL,	[Option4] [nvarchar](max) NULL,	[Status] [nvarchar](max) NULL, CONSTRAINT [PK_FeedBackQuestions] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[FeedBackAnswers](	[Id] [int] IDENTITY(1,1) NOT NULL,	[terminal] [nvarchar](max) NULL,[saleid] [nvarchar](50) NULL,	[date] [datetime] NULL,	[QuestionId] [nvarchar](max) NULL,	[Answer1] [nvarchar](max) NULL,	[Answer2] [nvarchar](max) NULL, [Answer3] [nvarchar](max) NULL,	[Answer4] [nvarchar](max) NULL, CONSTRAINT [PK_FeedBackAnswers] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[MenuItemBranch](	[Id] [int] IDENTITY(1,1) NOT NULL,	[Branchid] [int] NULL,	[MenuItemId] [int] NULL,	[Status] [varchar](5) NULL, CONSTRAINT [PK_MenuItemBranch] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[Log](	[Id] [int] IDENTITY(1,1) NOT NULL,	[Name] [varchar](max) NULL,	[Time] [datetime] NULL,[Userid] [int] NULL,	[Description] [varchar](max) NULL, CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[BackupPath](	[Id] [int] IDENTITY(1,1) NOT NULL,	[Path] [varchar](50) NULL, CONSTRAINT [PK_BackupPath] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = " CREATE TABLE [dbo].[DemandSheet](	[Id] [int] IDENTITY(1,1) NOT NULL,	[Date] [date] NULL,	[ItemId] [int] NULL,	[Quantity] [float] NULL,	[Userid] [int] NULL, CONSTRAINT [PK_DemandSheet] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                 try
                {
                    query = "CREATE TABLE [dbo].[fbrinfo](	[Id] [int] IDENTITY(1,1) NOT NULL,	[POSID] [varchar](max) NULL,	[PCTCode] [varchar](max) NULL) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE PROCEDURE [dbo].[GetFoodCost]	@date1 date,@date2 date,@branchid int,@type varchar(50) AS if(@type='menu') begin	SELECT        TOP (100) PERCENT SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) AS menuprice, dbo.MenuItem.Price AS mprice,                          dbo.ModifierFlavour.price AS fprice, dbo.MenuItem.Name, dbo.MenuItem.Target, dbo.MenuItem.Id, dbo.MenuGroup.Id AS mid, dbo.Saledetails.Flavourid, dbo.ModifierFlavour.name AS Flavour,                          dbo.MenuGroup.Name AS Groupname, dbo.Modifier.Name AS Modifier, dbo.RuntimeModifier.name AS RModifier, dbo.Saledetails.ModifierId, dbo.Saledetails.RunTimeModifierId, dbo.RuntimeModifier.price AS Rmodifierprice,                          dbo.Modifier.Price AS Modifierprice FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id LEFT OUTER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Sale.Date BETWEEN @date1 AND @date2) AND (dbo.Sale.BillStatus = 'Paid') AND (dbo.Sale.branchid = @branchid) and dbo.Saledetails.RunTimeModifierId=0 and dbo.Saledetails.ModifierId=0 GROUP BY dbo.MenuItem.Name, dbo.ModifierFlavour.name, dbo.MenuItem.Id, dbo.MenuGroup.Id, dbo.Saledetails.Flavourid, dbo.MenuGroup.Name, dbo.MenuItem.Price, dbo.ModifierFlavour.price, dbo.MenuItem.Target,                          dbo.Modifier.Name, dbo.RuntimeModifier.name, dbo.Saledetails.ModifierId, dbo.Saledetails.RunTimeModifierId, dbo.RuntimeModifier.price, dbo.Modifier.Price						 end												if(@type='rmodifier') begin	SELECT        TOP (100) PERCENT SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) AS menuprice, dbo.RuntimeModifier.name AS RModifier,                          dbo.Saledetails.RunTimeModifierId, dbo.RuntimeModifier.price AS Rmodifierprice FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id LEFT OUTER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id WHERE        (dbo.Sale.Date BETWEEN @date1 AND @date2) AND (dbo.Sale.BillStatus = 'Paid') AND (dbo.Sale.branchid = @branchid) AND (dbo.Saledetails.RunTimeModifierId > 0) AND (dbo.Saledetails.ModifierId = 0) AND                          (dbo.Saledetails.Price > 0) GROUP BY dbo.RuntimeModifier.name, dbo.Saledetails.RunTimeModifierId, dbo.RuntimeModifier.price						 end						 if(@type='modifier') begin	SELECT        TOP (100) PERCENT SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) AS menuprice, dbo.Modifier.Name AS Modifier,                          dbo.Saledetails.ModifierId, dbo.Modifier.Price AS Modifierprice FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id LEFT OUTER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id WHERE        (dbo.Sale.Date BETWEEN @date1 AND @date2) AND (dbo.Sale.BillStatus = 'Paid') AND (dbo.Sale.branchid = @branchid) AND (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId > 0) AND                          (dbo.Saledetails.Price > 0) GROUP BY dbo.Modifier.Name, dbo.Saledetails.ModifierId, dbo.Modifier.Price						 end";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE FUNCTION [dbo].[Getprice]( @date1 date,	@date2 date,    @itemid int) RETURNS float  AS  BEGIN     Declare @Itemprice float	 DECLARE @COUNT INT	set @Itemprice= (select top 1 price from InventoryTransfer where Itemid=@itemid order by date desc)	set @Itemprice=(SELECT AVG(price) AS Expr1 FROM            dbo.InventoryTransfer WHERE        (dbo.InventoryTransfer.sourcebranchid IS NOT NULL) and ( dbo.InventoryTransfer.date between @date1 and @date2) and dbo.InventoryTransfer.ItemId = @itemid)    if( @Itemprice<=0 or @Itemprice is null)     begin    set @Itemprice=(SELECT top 1 (price) AS Expr1 FROM            dbo.InventoryTransfer WHERE        (dbo.InventoryTransfer.sourcebranchid IS NOT NULL) and ( dbo.InventoryTransfer.date <= @date1 ) and dbo.InventoryTransfer.ItemId = @itemid order by date desc)     end   if( @Itemprice<=0 or @Itemprice is null)     begin   set @Itemprice=(SELECT     AVG(dbo.PurchaseDetails.priceperitem) AS Expr1 FROM         dbo.PurchaseDetails INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id WHERE ( dbo.Purchase.date between @date1 and @date2) and RawItemId = @itemid)    end	if( @Itemprice<=0 or @Itemprice is null)     begin   set @Itemprice=(SELECT     top 1  (dbo.PurchaseDetails.priceperitem) AS Expr1 FROM         dbo.PurchaseDetails INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id WHERE ( dbo.Purchase.date <= @date1 ) and RawItemId = @itemid order by Date desc)    end   if( @Itemprice<=0 or @Itemprice is null)     begin    SELECT @Itemprice=Price   FROM RawItem WHERE id = @itemid   end   return @Itemprice END;";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "DROP FUNCTION dbo.GetCost3";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();

                    query = "CREATE FUNCTION [dbo].[GetCost3](    @itemid INT,    @date11 Date,    @date12 Date,	@flid1 int ,	@rid1 int,	 @mid1 int,	@type1 varchar(50),	@producttype1 varchar(50)) RETURNS float AS BEGIN    DECLARE @COUNT INT	DECLARE @MAX INT	 DECLARE @Total float	DECLARE @Quantity float	DECLARE @Itemprice float	DECLARE @Type varchar(100)	DECLARE @ProductType varchar(100)	 DECLARE @Name varchar(100)	DECLARE @Price varchar(100)	DECLARE @Rawitemid varchar(100)	DECLARE @Menuitemid int	Declare @date1 date	Declare @date2 date	DECLARE @rid INT	DECLARE @mid INT	 DECLARE @flid INT	declare @rate float	set @Type=@type1	set @ProductType=@producttype1	set @rid=@rid1	set @mid=@mid1	set @flid=@flid1	set @Menuitemid=@itemid	set @date1=@date11	set @date2=@date12 SET @Total =0	if(@rid>0)		begin	set @Count = 1 set @MAX = 1 		end	else  if(@mid>0)		begin	set @Count = 1 set @MAX = 1 	end	 else if(@flid>0) begin	if(@ProductType='Packing') begin	 SELECT @Count = min(dbo.Recipe.id) , @MAX = max(dbo.Recipe.id)  FROM 		dbo.Recipe INNER JOIN                       		dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id		 WHERE  dbo.Recipe.MenuItemId=@Menuitemid  and modifierid=@flid   and dbo.Type.TypeName=@ProductType and (dbo.Recipe.type=@Type or dbo.Recipe.type='Both')  end		 else		 begin	 SELECT @Count = min(dbo.Recipe.id) , @MAX = max(dbo.Recipe.id)  FROM 		dbo.Recipe INNER JOIN                       		dbo.RawItem ON dbo.Recipe.RawItemId  = dbo.RawItem.Id INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id		 WHERE dbo.Recipe.MenuItemId=@Menuitemid  and modifierid=@flid  and dbo.Type.TypeName !='Packing' and  (dbo.Recipe.type=@Type or dbo.Recipe.type='Both') 		 end	end		else	 begin	if(@ProductType='Packing')	begin	SELECT @Count = min(dbo.Recipe.id) , @MAX = max(dbo.Recipe.id)  FROM 		 dbo.Recipe INNER JOIN                       		dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id		  WHERE  dbo.Recipe.MenuItemId=@Menuitemid  and dbo.Type.TypeName=@ProductType and (dbo.Recipe.type=@Type or dbo.Recipe.type='Both') 		 end	 else		 begin	 SELECT @Count = min(dbo.Recipe.id) , @MAX = max(dbo.Recipe.id)  FROM 		dbo.Recipe INNER JOIN                       		dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id		 WHERE dbo.Recipe.MenuItemId=@Menuitemid  and dbo.Type.TypeName !='Packing' and (dbo.Recipe.type=@Type or dbo.Recipe.type='Both') 		 end	 end		 WHILE(@Count IS NOT NULL  AND @Count <= @MAX)		begin	if(@rid>0)	 Begin			 SELECT @Quantity = quantity,@Type=type,@Rawitemid=RawItemId	   FROM RuntimeModifier  WHERE id = @rid		End	 else if(@mid>0)		Begin			SELECT @Quantity = quantity,@Type='Both',@Rawitemid=RawItemId	   FROM Modifier WHERE id = @mid		End	 else 	 Begin	 if(@ProductType='Packing')		 begin	 if(@flid>0) begin SELECT @Rawitemid=dbo.Recipe.RawItemId, @Quantity=dbo.Recipe.Quantity FROM 		dbo.Recipe INNER JOIN                       		 dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id		 WHERE dbo.Recipe.id = @Count and dbo.Recipe.MenuItemId=@Menuitemid and dbo.Type.TypeName=@ProductType and (dbo.Recipe.type=@Type or dbo.Recipe.type='Both')  and dbo.Recipe.modifierid=@flid end else begin SELECT @Rawitemid=dbo.Recipe.RawItemId, @Quantity=dbo.Recipe.Quantity FROM 		dbo.Recipe INNER JOIN                       		 dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id		 WHERE dbo.Recipe.id = @Count and dbo.Recipe.MenuItemId=@Menuitemid and dbo.Type.TypeName=@ProductType and (dbo.Recipe.type=@Type or dbo.Recipe.type='Both') 			 end end		  else	 begin		 if(@flid>0) begin SELECT @Rawitemid=dbo.Recipe.RawItemId, @Quantity=dbo.Recipe.Quantity FROM 		dbo.Recipe INNER JOIN                       		dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                       dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id		 WHERE dbo.Recipe.id = @Count and dbo.Recipe.MenuItemId=@Menuitemid  and dbo.Type.TypeName !='Packing' and (dbo.Recipe.type=@Type or dbo.Recipe.type='Both') and dbo.Recipe.modifierid=@flid end else begin SELECT @Rawitemid=dbo.Recipe.RawItemId, @Quantity=dbo.Recipe.Quantity FROM 		dbo.Recipe INNER JOIN                       		dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                       dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id		 WHERE dbo.Recipe.id = @Count and dbo.Recipe.MenuItemId=@Menuitemid  and dbo.Type.TypeName !='Packing' and (dbo.Recipe.type=@Type or dbo.Recipe.type='Both') end		 end End			 set @Itemprice= (SELECT dbo.Getprice(@date1,@date2,@Rawitemid))     	 SELECT @Name = ItemName FROM RawItem WHERE id = @Rawitemid	    set @rate=(SELECT     dbo.UOMConversion.ConversionRate FROM       dbo.RawItem INNER JOIN                       dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN             dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RawItem.Id= @Rawitemid)	   if(@rate>0)	   begin	   set @rate = @Quantity / @rate	   end		 SET @Total = @Total + @Itemprice * @rate					 if(@rid>0)		Begin			SET @Count  = @Count  + 1  			end 		else if(@mid>0)		begin		 SET @Count  = @Count  + 1  		end		else	 begin		 if(@ProductType='Packing')	 begin	 if(@flid>0) begin set @COUNT=(SELECT top 1  dbo.Recipe.id	   FROM 		dbo.Recipe INNER JOIN    dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id		 WHERE  dbo.Recipe.MenuItemId=@Menuitemid   and dbo.Type.TypeName=@ProductType and (dbo.Recipe.type=@Type or dbo.Recipe.type='Both')  and dbo.Recipe.id > @Count  order by dbo.Recipe.id) end else begin set @COUNT=(SELECT top 1  dbo.Recipe.id	   FROM 		dbo.Recipe INNER JOIN    dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id		 WHERE  dbo.Recipe.MenuItemId=@Menuitemid   and dbo.Type.TypeName=@ProductType and (dbo.Recipe.type=@Type or dbo.Recipe.type='Both')  and dbo.Recipe.id > @Count  order by dbo.Recipe.id) end	 end else if(@flid>0) begin set @COUNT=(SELECT top 1  dbo.Recipe.id	  FROM 		dbo.Recipe INNER JOIN                       		dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id	 WHERE  dbo.Recipe.MenuItemId=@Menuitemid  and dbo.Type.TypeName!='Packing' and (dbo.Recipe.type=@Type or dbo.Recipe.type='Both') and dbo.Recipe.modifierid=@flid and dbo.Recipe.id > @Count  order by dbo.Recipe.id) end else begin set @COUNT=(SELECT top 1  dbo.Recipe.id	 FROM 		dbo.Recipe INNER JOIN                       		dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id	WHERE  dbo.Recipe.MenuItemId=@Menuitemid  and dbo.Type.TypeName!='Packing' and (dbo.Recipe.type=@Type or dbo.Recipe.type='Both')  and dbo.Recipe.id > @Count  order by dbo.Recipe.id)	end		end	end		return @Total END;";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }

                try
                {
                    query = "CREATE TABLE [dbo].[DineInTableDesign](	[Id] [int] IDENTITY(1,1) NOT NULL,	[Floor] [nvarchar](50) NULL,	[TableNo] [nvarchar](50) NULL, CONSTRAINT [PK_DineInTableDesign] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY])";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[BranchAccount](	[Id] [int] NOT NULL,	[Date] [date] NULL,	[CustomersId] [int] NULL,	[PayableAccountId] [int] NULL,	[VoucherNo] [varchar](max) NULL,	[CheckNo] [varchar](max) NULL,	[CheckDate] [date] NULL,	[Description] [varchar](max) NULL,	[Debit] [float] NULL,	[Credit] [float] NULL,	[Balance] [float] NULL,	[EntryType] [varchar](max) NULL,	[branchid] [int] NULL, CONSTRAINT [PK_BranchAccount] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[BranchAccount]  WITH NOCHECK ADD  CONSTRAINT [FK_BranchAccount_ChartofAccounts] FOREIGN KEY([PayableAccountId]) REFERENCES [dbo].[ChartofAccounts] ([Id]) ALTER TABLE [dbo].[BranchAccount] CHECK CONSTRAINT [FK_BranchAccount_ChartofAccounts] ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[SalariesAccount](	[Id] [int] NOT NULL,	[Date] [date] NULL,	[ChartAccountId] [int] NULL,	[VoucherNo] [varchar](max) NULL,	[Description][varchar](max) NULL,	[Credit] [float] NULL,	[Debit] [float] NULL,	[Balance] [float] NULL,	[branchid] [int] NULL, CONSTRAINT [PK_SalariesAccount] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                    query=" ALTER TABLE [dbo].[SalariesAccount]  WITH NOCHECK ADD  CONSTRAINT [FK_SalariesAccount_ChartofAccounts] FOREIGN KEY([ChartAccountId]) REFERENCES [dbo].[ChartofAccounts] ([Id]) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                    query = "ALTER TABLE [dbo].[SalariesAccount] CHECK CONSTRAINT [FK_SalariesAccount_ChartofAccounts]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[StoreDemand](	[id] [int] IDENTITY(1,1) NOT NULL,	[Itemid] [int] NULL,	[Date] [date] NULL,	[Quantity] [float] NULL,	[Status] [varchar](50) NULL,	[branchid] [int] NULL,	[kdsid] [int] NULL, CONSTRAINT [PK_StoreDemand] PRIMARY KEY CLUSTERED (	[id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                

                try
                {
                    query = "ALTER TABLE [dbo].[closing]  ADD kdsid int NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[Discard]  ADD kdsid int NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[StoreDemand]  ADD Invoiceno varchar(100) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[SerivceCharges]  ADD SubType varchar(100) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[Users]  ADD KDSType varchar(50) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[Attachmenu1]  ADD Type varchar(50) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[InventoryTransferStore](	[id] [int] IDENTITY(1,1) NOT NULL,	[Date] [date] NULL,	[Itemid] [int] NULL,	[Quantity] [float] NULL,	[branchid] [int] NULL,	[Onlineid] [int] NULL,	[Remarks] [varchar](max) NULL,	[IssuingStoreId] [int] NULL,	[RecvStoreId] [int] NULL,	[uploadstatus] [varchar](50) NULL, CONSTRAINT [PK_InventoryTransferStore] PRIMARY KEY CLUSTERED (	[id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[InventoryTransferStore] ADD  CONSTRAINT [DF_InventoryTransferStore_uploadstatus]  DEFAULT ('Pending') FOR [uploadstatus]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[InventoryTransferStore]  ADD Invoiceno varchar(100) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                 try
                {
                    query = "CREATE TABLE [dbo].[PurchaseOrder](	[Id] [int] NOT NULL,	[SupplierId] [int] NULL,	[TotalAmount] [float] NULL,	[Date] [date] NULL,	[BranchCode] [varchar](50) NULL,	[StoreCode] [int] NULL,	[InvoiceNo] [varchar](max) NULL,	[Status] [varchar](max) NULL,	[UploadStatus] [varchar](max) NULL,	[branchid] [int] NULL,	[onlineid] [int] NULL, CONSTRAINT [PK_PurchaseOrder] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                 try
                 {
                     query = "ALTER TABLE [dbo].[PurchaseOrder]  WITH NOCHECK ADD  CONSTRAINT [FK_PurchaseOrder_Supplier] FOREIGN KEY([SupplierId]) REFERENCES [dbo].[Supplier] ([Id]) ";
                     com = new SqlCommand(query, this.connection);
                     res = com.ExecuteNonQuery();
                     query = "ALTER TABLE [dbo].[PurchaseOrder] CHECK CONSTRAINT [FK_PurchaseOrder_Supplier]";
                     com = new SqlCommand(query, this.connection);
                     res = com.ExecuteNonQuery();
                 }
                 catch (Exception ex)
                 {


                 }
                 try
                 {
                     query = "CREATE TABLE [dbo].[PurchaseOrderDetails](	[Id] [int] NOT NULL,	[RawItemId] [int] NULL,	[PurchaseOrderId] [int] NULL,	[Package] [varchar](max) NULL,	[PackageItems] [float] NULL,	[TotalItems] [float] NULL,	[Price] [float] NULL,	[PricePerItem] [float] NULL,	[TotalAmount] [float] NULL,	[UploadStatus] [varchar](max) NULL,	[branchid] [int] NULL,	[Description] [varchar](max) NULL,	[onlineid] [int] NULL, CONSTRAINT [PK_PurchaseOrderDetails] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                     com = new SqlCommand(query, this.connection);
                     res = com.ExecuteNonQuery();
                 }
                 catch (Exception ex)
                 {


                 }
                 try
                 {
                     query = "ALTER TABLE [dbo].[PurchaseOrderDetails]  WITH NOCHECK ADD  CONSTRAINT [FK_PurchaseOrderDetails_Purchase] FOREIGN KEY([PurchaseOrderId]) REFERENCES [dbo].[PurchaseOrder] ([Id]) ";
                     com = new SqlCommand(query, this.connection);
                     res = com.ExecuteNonQuery();
                     query = "ALTER TABLE [dbo].[PurchaseOrderDetails] CHECK CONSTRAINT [FK_PurchaseOrderDetails_Purchase]";
                     com = new SqlCommand(query, this.connection);
                     res = com.ExecuteNonQuery();
                 }
                 catch (Exception ex)
                 {


                 }
                 
                 try
                {
                    query = "ALTER TABLE [dbo].[Purchase]  ADD PONo varchar(100) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[employees]  ADD payableaccountid int NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[InventoryConsumed]  ADD kdsid int NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[purchase]  ADD onlineid int NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[purchasedetails]  ADD onlineid int NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[Attachmenu1]  ADD userecipe varchar(50) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[employees]  ADD Status varchar(50) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[EmployeesAccount](	[Id] [int] NOT NULL,	[Date] [date] NULL,	[EmployeeId] [int] NULL,	[PayableAccountId] [int] NULL,	[VoucherNo] [varchar](max) NULL,	[CheckNo] [varchar](max) NULL,	[CheckDate] [date] NULL,	[Description] [varchar](max) NULL,	[Debit] [float] NULL,	[Credit] [float] NULL,	[Balance] [float] NULL,	[EntryType] [varchar](max) NULL,	[branchid] [int] NULL, CONSTRAINT [PK_EmployeesAccount] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY] ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                    query = " ALTER TABLE [dbo].[EmployeesAccount]  WITH NOCHECK ADD  CONSTRAINT [FK_EmployeesAccount_ChartofAccounts] FOREIGN KEY([PayableAccountId]) REFERENCES [dbo].[ChartofAccounts] ([Id])  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                    query = "ALTER TABLE [dbo].[EmployeesAccount] CHECK CONSTRAINT [FK_EmployeesAccount_ChartofAccounts] ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                
                try
                {
                    query = "ALTER TABLE [dbo].[Branch]  ADD ChartaccountId varchar(50) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[AttachRecipe]  ADD type varchar(50) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[ModifierFlavour]  ADD Status varchar(50) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[InventoryTransfer]  ADD NetTotal float NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[InventoryTransfer]  ADD GST float NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[InventoryTransfer]  ADD GSTAmount float NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[InventoryTransfer]  ADD Discount float NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[InventoryTransfer]  ADD DiscountAmount float NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[saledetails]  ADD ParkStatus varchar(50) NULL CONSTRAINT park_default  DEFAULT 'Pending' ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[Users]  ADD ParkStatus varchar(50) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[tabs]  ADD showpendingorsers varchar(50) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[AttachRecipe]  ADD FlavourId int NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[SaleDetails]  ADD completedtime datetime NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[MenuItem]  ADD Price2 float NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[MenuItem]  ADD Price3 float NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                
                try
                {
                    query = "ALTER TABLE [dbo].[CompanyInfo]  ADD PrintInvoiceNo varchar(50) NULL";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[Sale]  ADD cancelservicecharges varchar(50) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                 try
                {
                    query = " CREATE TABLE [dbo].[DSSale]([Id] [int] IDENTITY(1,1) NOT NULL,	[Date] [date] NULL,	[time] [datetime] NULL,	[UserId] [int] NULL,	[TotalBill] [float] NULL,	[Discount] [float] NULL,	[DiscountAmount] [float] NULL,	[NetBill] [float] NULL,	[BillType] [varchar](50) NULL,	[OrderType] [varchar](max) NULL,	[GST] [float] NULL,	[BillStatus] [varchar](max) NULL,	[OrderStatus] [varchar](max) NULL,	[Terminal] [varchar](max) NULL,	[UploadStatus] [varchar](max) NULL,	[branchid] [int] NULL,	[GSTPerc] [float] NULL,	[Shiftid] [int] NULL,	[Customer] [varchar](max) NULL,	[message] [varchar](max) NULL,	[Deliverystatus] [varchar](50) NULL,	[servicecharges] [float] NULL,	[TerminalOrder] [varchar](50) NULL,	[phone] [varchar](50) NULL,	[invoice] [int] NULL,	[discountid] [varchar](50) NULL,	[OnlineId] [int] NULL,	[uploadstatusserver] [varchar](50) NULL,	[DeliveredTime] [datetime] NULL,	[discountkeyid] [varchar](50) NULL,	[uploadstatusbilltype] [varchar](50) NULL,	[uploadstatusrefund] [varchar](50) NULL,	[FBRcode] [varchar](max) NULL,	[RiderId] [varchar](50) NULL,	[UploadstatusDelivery] [varchar](50) NULL,	[cashierid] [int] NULL,	[GSTtype] [varchar](100) NULL,	[Token] [varchar](100) NULL,	[cancelservicecharges] [varchar](50) NULL, CONSTRAINT [PK_DSSale] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                  try
                {
                    query = "CREATE TABLE [dbo].[DSSaledetails](	[Id] [int] IDENTITY(1,1) NOT NULL,	[saleid] [int] NULL,	[MenuItemId] [int] NULL,	[Flavourid] [int] NULL,	[ModifierId] [int] NULL,	[RunTimeModifierId] [int] NULL,	[Quantity] [float] NULL,	[Price] [float] NULL,	[BarnchCode] [varchar](50) NULL,	[Status] [varchar](50) NULL,	[comments] [varchar](50) NULL,	[Orderstatus] [varchar](50) NULL,	[branchid] [int] NULL,	[Itemdiscount] [float] NULL,	[ItemdiscountPerc] [float] NULL,	[ItemGst] [float] NULL,	[ItemGstPerc] [float] NULL,	[OrderStatusmain] [varchar](50) NULL,	[atid] [int] NULL,	[dealid] [int] NULL,	[OnlineId] [int] NULL,	[kdsgroup] [int] NULL,	[time] [varchar](max) NULL,	[kdsid] [int] NULL,	[pointscode] [varchar](max) NULL,	[completedtime] [datetime] NULL, CONSTRAINT [PK_DSSaledetails] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[DSSaledetails]  WITH NOCHECK ADD  CONSTRAINT [FK_DSSaledetails_Sale] FOREIGN KEY([saleid]) REFERENCES [dbo].[DSSale] ([Id]) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[DSSaledetails] CHECK CONSTRAINT [FK_DSSaledetails_Sale] ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                  try
                {
                    query = "CREATE TABLE [dbo].[DSBillType](	[id] [int] IDENTITY(1,1) NOT NULL,	[type] [varchar](max) NULL,	[saleid] [int] NULL,	[Amount] [float] NULL,	[branchid] [int] NULL,	[uploadStatus] [varchar](50) NULL,	[recvid] [int] NULL,	[cashoutime] [datetime] NULL, CONSTRAINT [PK_DSBillType] PRIMARY KEY CLUSTERED (	[id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[DSSaledetails]  WITH NOCHECK ADD  CONSTRAINT [FK_Saledetails_MenuItem] FOREIGN KEY([MenuItemId]) REFERENCES [dbo].[MenuItem] ([Id]) ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[DSSale]  ADD Staffid int NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[menuitem]  ADD kdsid2 varchar(50) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[RUNTimemodifier]  ADD kdsid2 varchar(50) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[Modifier]  ADD kdsid2 varchar(50) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                 try
                {
                    query = "ALTER TABLE [dbo].[Closing] ADD  CONSTRAINT [DF_Closing_uploadstatus]  DEFAULT ('Pending') FOR [uploadstatus]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }

                
                 try
                 {
                     query = "ALTER TABLE [dbo].[SerivceCharges]  ADD OrderType varchar(50) NULL ";
                     com = new SqlCommand(query, this.connection);
                     res = com.ExecuteNonQuery();
                 }
                 catch (Exception ex)
                 {


                 }
                 try
                 {
                     query = "ALTER TABLE [dbo].[SerivceCharges]  ADD GstType varchar(50) NULL ";
                     com = new SqlCommand(query, this.connection);
                     res = com.ExecuteNonQuery();
                 }
                 catch (Exception ex)
                 {


                 }
                 try
                 {
                     query = "ALTER TABLE [dbo].[DiscountCompaign]  ADD type varchar(100) NULL ";
                     com = new SqlCommand(query, this.connection);
                     res = com.ExecuteNonQuery();
                 }
                 catch (Exception ex)
                 {


                 }
                 try
                 {
                     query = "ALTER TABLE [dbo].[Closing]  ADD Userid int NULL ";
                     com = new SqlCommand(query, this.connection);
                     res = com.ExecuteNonQuery();
                 }
                 catch (Exception ex)
                 {


                 }
                 try
                 {
                     query = "CREATE TABLE [dbo].[CashBook](	[Id] [int] IDENTITY(1,1) NOT NULL,	[AccountId] [int] NULL,	[Type] [varchar](100) NULL,	[Branchid] [int] NULL, CONSTRAINT [PK_CashBook] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]";
                     com = new SqlCommand(query, this.connection);
                     res = com.ExecuteNonQuery();
                 }
                 catch (Exception ex)
                 {


                 }
                 try
                 {
                     query = "ALTER TABLE [dbo].[CashBook]  WITH CHECK ADD  CONSTRAINT [FK_CashBook_ChartofAccounts] FOREIGN KEY([AccountId]) REFERENCES [dbo].[ChartofAccounts] ([Id]) ";
                     com = new SqlCommand(query, this.connection);
                     res = com.ExecuteNonQuery();
                 }
                 catch (Exception ex)
                 {


                 }
                 try
                 {
                     query = "ALTER TABLE [dbo].[CashBook] CHECK CONSTRAINT [FK_CashBook_ChartofAccounts] ";
                     com = new SqlCommand(query, this.connection);
                     res = com.ExecuteNonQuery();
                 }
                 catch (Exception ex)
                 {


                 }
                 try
                 {
                     query = "ALTER TABLE [dbo].[RawItem]  ADD critical varchar(100) NULL ";
                     com = new SqlCommand(query, this.connection);
                     res = com.ExecuteNonQuery();
                 }
                 catch (Exception ex)
                 {


                 }
                 try
                 {
                     query = "ALTER TABLE [dbo].[Sale]  ADD GSTtype varchar(100) NULL ";
                     com = new SqlCommand(query, this.connection);
                     res = com.ExecuteNonQuery();
                 }
                 catch (Exception ex)
                 {


                 }
                 try
                 {
                     query = "ALTER TABLE [dbo].[DiscountCompaign]  ADD groupid varchar(100) NULL ";
                     com = new SqlCommand(query, this.connection);
                     res = com.ExecuteNonQuery();
                 }
                 catch (Exception ex)
                 {


                 }
                 try
                 {
                     query = "ALTER TABLE [dbo].[Companyinfo]  ADD printvisa varchar(100) NULL ";
                     com = new SqlCommand(query, this.connection);
                     res = com.ExecuteNonQuery();
                 }
                 catch (Exception ex)
                 {


                 }
                try
                 {
                     query = "ALTER TABLE [dbo].[Customers]  ADD Chartaccountid int NULL ";
                     com = new SqlCommand(query, this.connection);
                     res = com.ExecuteNonQuery();
                 }
                 catch (Exception ex)
                 {


                 }
                
                try
                 {
                     query = "ALTER TABLE [dbo].[fbrinfo]  ADD Terminal varchar(100) NULL ";
                     com = new SqlCommand(query, this.connection);
                     res = com.ExecuteNonQuery();
                 }
                 catch (Exception ex)
                 {


                 }
                try
                {
                    query = "ALTER TABLE [dbo].[MenuGroup]  ADD SubGroup varchar(500) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[MenuItem]  ADD ManualQty varchar(50) NULL ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                 {
                     query = "ALTER TABLE [dbo].[Sale]  ADD UploadstatusDelivery varchar(50) NULL ";
                     com = new SqlCommand(query, this.connection);
                     res = com.ExecuteNonQuery();
                 }
                 catch (Exception ex)
                 {


                 }
                 try
                 {
                     query = "ALTER TABLE [dbo].[ordersource]  ADD amount varchar(50) NULL ";
                     com = new SqlCommand(query, this.connection);
                     res = com.ExecuteNonQuery();
                 }
                 catch (Exception ex)
                 {


                 }
                 try
                 {
                     query = "ALTER TABLE [dbo].[GST]  ADD Type varchar(50) NULL ";
                     com = new SqlCommand(query, this.connection);
                     res = com.ExecuteNonQuery();
                 }
                 catch (Exception ex)
                 {


                 }
                 try
                 {
                     query = "ALTER TABLE [dbo].[Sale]  ADD  UploadstatusDelivery varchar(50) NULL CONSTRAINT [DF_Sale_uploadstatusdelivery] DEFAULT ('Pending')";
                     com = new SqlCommand(query, this.connection);
                     res = com.ExecuteNonQuery();
                 }
                 catch (Exception ex)
                 {


                 }
                 try
                 {
                     query = "ALTER TABLE [dbo].[Sale]  ADD cashierid int NULL ";
                     com = new SqlCommand(query, this.connection);
                     res = com.ExecuteNonQuery();
                 }
                 catch (Exception ex)
                 {


                 }
                
                 try
                 {
                     query = "ALTER TABLE [dbo].[CompanyInfo]  ADD showcardvisa varchar(50) NULL ";
                     com = new SqlCommand(query, this.connection);
                     res = com.ExecuteNonQuery();
                 }
                 catch (Exception ex)
                 {


                 }
                try
                 {
                     query = "ALTER TABLE [dbo].[Sale]  ADD RiderId varchar(50) NULL ";
                     com = new SqlCommand(query, this.connection);
                     res = com.ExecuteNonQuery();
                 }
                 catch (Exception ex)
                 {


                 }
                try
                {
                    
                    query = "ALTER TABLE [dbo].[Sale]  ADD uploadstatusbilltype varchar(50) NULL  CONSTRAINT [DF_Sale_uploadstatusbilltype] DEFAULT ('Pending')";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[Sale]  ADD uploadstatusrefund varchar(50) NULL  CONSTRAINT [DF_Sale_uploadstatusrefund] DEFAULT ('Pending')";
                  
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[Sale]  ADD PrintDeliveryKot varchar(50) NULL  CONSTRAINT [DF_Sale_PrintDeliveryKot] DEFAULT ('Pending')";

                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[VoucherKeys](	[Id] [int] IDENTITY(1,1) NOT NULL,	[Name] [varchar](max) NULL,	[Amount] [float] NULL, CONSTRAINT [PK_VoucherKeys] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[SubItems](	[Id] [int] IDENTITY(1,1) NOT NULL,	[Name] [varchar](max) NULL,	[Status] [varchar](50) NULL, CONSTRAINT [PK_SubItems] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[SubRecipe](	[Id] [int] NOT NULL,	[ItemId] [int] NULL,	[RawItemId] [int] NULL,	[Quantity] [float] NULL,	[uploadstatus] [varchar](max) NULL,	[type] [varchar](50) NULL, CONSTRAINT [PK_SubRecipe] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE PROCEDURE [dbo].[str_Consumed] 		@Id int = 0, 	@RawItemId int = 0, 	@QuantityConsumed float = 0,	@Date varchar(50)='',	@branchid int =0,	@onlineid int =0 AS BEGIN		SET NOCOUNT ON;    insert into InventoryConsumed (Id,RawItemId, QuantityConsumed, Date, branchid,onlineid)	 values (@Id,@RawItemId,@QuantityConsumed,@Date,@branchid,@onlineid) END";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE PROCEDURE [dbo].[str_Consumeddelete] 		@onlineid int = 0, 	@branchid int = 0 AS BEGIN	SET NOCOUNT ON;	delete from InventoryConsumed where OnlineId=@onlineid and branchid=@branchid END";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[Buttons](	[Id] [int] IDENTITY(1,1) NOT NULL,	[Name] [varchar](50) NULL,	[Status] [varchar](50) NULL) ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[AttachRecipe](	[Id] [int] IDENTITY(1,1) NOT NULL,	[Menuitemid] [int] NULL,	[SubItemId] [int] NULL,	[Quantity] [float] NULL, CONSTRAINT [PK_AttachRecipe] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[AttachRecipe]  WITH CHECK ADD  CONSTRAINT [FK_AttachRecipe_MenuItem] FOREIGN KEY([Menuitemid]) REFERENCES [dbo].[MenuItem] ([Id])";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[AttachRecipe]  WITH CHECK ADD  CONSTRAINT [FK_AttachRecipe_SubRecipe] FOREIGN KEY([SubItemId]) REFERENCES [dbo].[SubItems] ([Id])";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                
                try
                {
                    query = "CREATE TABLE [dbo].[PriceSetting](	[Id] [int] IDENTITY(1,1) NOT NULL,	[Type] [varchar](50) NULL) ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[SubRecipe]  WITH NOCHECK ADD  CONSTRAINT [FK_SubRecipe_SubItem] FOREIGN KEY([ItemId])REFERENCES [dbo].[SubItems] ([Id])";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE PROCEDURE [dbo].[str_GetmenuitemByname] 	@name varchar(100) 	AS BEGIN		SET NOCOUNT ON;	select * from Menuitem where name=@name END";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE PROCEDURE [dbo].[str_Getdate] 		@DayStatus varchar(50) 	 AS BEGIN		SET NOCOUNT ON;   select top(1) * from DayEnd  where DayStatus=@DayStatus  order by id desc END";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE PROCEDURE [dbo].[str_Getmenuitem] 	@groupid int = 0, 	@topvalue int = 0,	@sno int = 0	 AS BEGIN	SET NOCOUNT ON;   Select top (@topvalue) * from (SELECT dbo.menuitem.Id, dbo.menuitem.Code, dbo.menuitem.Name, dbo.menuitem.MenuGroupId, dbo.menuitem.BarCode, dbo.menuitem.Price, dbo.menuitem.Status, dbo.menuitem.ColorId,                dbo.menuitem.KDSId, dbo.menuitem.Image, dbo.menuitem.FontColorId, dbo.menuitem.FontSize, dbo.menuitem.Minutes, dbo.menuitem.alarmtime, dbo.menuitem.minuteskdscolor,                dbo.menuitem.alarmkdscolor, dbo.menuitem.uploadstatus, dbo.menuitem.branchid, Color_1.ColorName, dbo.Color.ColorName AS Fontcolor,ROW_NUMBER() OVER (ORDER BY dbo.menuitem.id) as Sno FROM  dbo.menuitem INNER JOIN               dbo.Color AS Color_1 ON dbo.menuitem.ColorId = Color_1.Id INNER JOIN                  dbo.Color ON dbo.menuitem.FontColorId = dbo.Color.Id where dbo.MenuItem.MenuGroupId=@groupid and dbo.MenuItem.status='Active'  ) as t where Sno >@sno End";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE PROCEDURE [dbo].[str_GetModifierflavour] 	@sizechk varchar(50),	@MenuItemId int = 0 AS BEGIN		SET NOCOUNT ON;    	select * from ModifierFlavour where name =@sizechk and MenuItemId=@MenuItemId END";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE PROCEDURE [dbo].[str_Getruntimemodifier] 		@menuItemid int = 0	 AS BEGIN		SET NOCOUNT ON;	select * from RuntimeModifier where menuItemid=@menuItemid and status='Active' END";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE PROCEDURE [dbo].[str_GetRuntimeModifierDetails] 	@top int = 0, 	@branchtype varchar(50), 	@Sno varchar(50), 	@menuItemid int = 0 AS BEGIN		SET NOCOUNT ON;	SELECT    top (@top)  * from  ( select  dbo.MenuItem.Code, dbo.MenuItem.MenuGroupId, dbo.MenuItem.BarCode, dbo.MenuItem.ColorId, dbo.MenuItem.Image, dbo.MenuItem.FontColorId, dbo.MenuItem.FontSize, dbo.MenuItem.Minutes,                          dbo.MenuItem.alarmtime, dbo.MenuItem.minuteskdscolor, dbo.MenuItem.alarmkdscolor, dbo.MenuItem.uploadstatus, dbo.MenuItem.branchid, Color_1.ColorName, dbo.Color.ColorName AS Fontcolor,                          dbo.RuntimeModifier.id, dbo.RuntimeModifier.name, dbo.RuntimeModifier.price, dbo.RuntimeModifier.status, dbo.RuntimeModifier.kdsid,ROW_NUMBER() OVER (ORDER BY dbo.RuntimeModifier.id) as Sno FROM            dbo.MenuItem INNER JOIN                         dbo.Color AS Color_1 ON dbo.MenuItem.ColorId = Color_1.Id INNER JOIN                         dbo.Color ON dbo.MenuItem.FontColorId = dbo.Color.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.MenuItem.Id = dbo.RuntimeModifier.menuItemid where dbo.RuntimeModifier.menuItemid=@menuItemid  and dbo.RuntimeModifier.type=@branchtype  and dbo.RuntimeModifier.status='Active'   or   dbo.RuntimeModifier.menuItemid=@menuItemid  and dbo.RuntimeModifier.type='Both'  and dbo.RuntimeModifier.status='Active' ) as t where Sno >@Sno END";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                }
                try
                {
                    query = "CREATE PROCEDURE [dbo].[str_Getshift] 	@Status varchar(50) ,	@Terminal varchar(50) ,	@date date AS BEGIN		SET NOCOUNT ON;    select top(1) * from ShiftStart where date=@date and status=@Status and Terminal=@Terminal  order by id desc END";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE PROCEDURE [dbo].[str_Consumed] 		@Id int = 0, 	@RawItemId int = 0, 	@QuantityConsumed float = 0,	@Date varchar(50)='',	@branchid int =0,	@onlineid int =0 AS BEGIN		SET NOCOUNT ON;    insert into InventoryConsumed (Id,RawItemId, QuantityConsumed, Date, branchid,onlineid)	 values (@Id,@RawItemId,@QuantityConsumed,@Date,@branchid,@onlineid) END";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE PROCEDURE [dbo].[str_Consumeddelete] 		@onlineid int = 0, 	@branchid int = 0 AS BEGIN	SET NOCOUNT ON;	delete from InventoryConsumed where OnlineId=@onlineid and branchid=@branchid END";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE PROCEDURE [dbo].[str_discard] 		@id int = 0, 	@itemid int = 0,	@date varchar(50) = '', 	@Discard varchar(50) = '', 	@Remaining varchar(50) = '', 	@staff varchar(50) = '', 	@completewaste varchar(50) = '', 	@Remarks varchar(100) = '', 	@branchid int = 0, 	@Onlineid int = 0 AS BEGIN		SET NOCOUNT ON;	insert into Discard(id,itemid,date,Discard,Remaining,staff,completewaste,Remarks,branchid,Onlineid)	values(@id,@itemid,@date,@Discard,@Remaining,@staff,@completewaste,@Remarks,@branchid,@Onlineid) END";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE PROCEDURE [dbo].[str_discarddelete] 		@branchid int = 0, 	@onlineid int = 0 AS BEGIN		SET NOCOUNT ON;	delete from Discard where branchid=@branchid and Onlineid=@onlineid END";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.BillType ADD recvid int NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    
                    
                }
                try
                {
                    query = "ALTER TABLE dbo.Menuitem ADD modifiergroup varchar(50) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    
                    
                }
                try
                {
                    query = "ALTER TABLE dbo.Branch ADD Status varchar(50) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    
                    
                }
                

                try
                {
                    query = "ALTER TABLE dbo.DiscountIndividual ADD discountid int NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.GST ADD Title varchar(50) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.Sale ADD FBRcode varchar(max) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                

                try
                {
                    query = "ALTER TABLE dbo.Modifier ADD imageurl varchar(100) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.Companyinfo ADD logourl varchar(100) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.DiscountKeys ADD Status varchar(50) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.VoucherKeys ADD Status varchar(50) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.RuntimeModifier ADD imageurl varchar(max) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.Sale ADD discountkeyid int NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.Saledetailsrefund ADD time datetime NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.Saledetailsrefund ADD type varchar(50) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[ReopenLog](	[Id] [int] IDENTITY(1,1) NOT NULL,	[Time] [datetime] NULL,	[Saleid] [int] NULL) ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.Menuitem ADD GrossPrice float NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.ModifierFlavour ADD GrossPrice float NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.Modifier ADD GrossPrice float NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.RuntimeModifier ADD GrossPrice float NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.Sale ADD Onlineid int NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.RawItem ADD Supplierid int NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    
                    
                }
                try
                {
                    query = "ALTER TABLE dbo.DinInTables ADD Guests varchar(50) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.Sale ADD Onlineid int NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.recipe ADD attachmenuid int NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.RuntimeModifier ADD stage int NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.sale ADD discountkeyid varchar(50) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.RuntimeModifier ADD quantityallowed int NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[ordersource](	[id] [int] IDENTITY(1,1) NOT NULL,	[name] [varchar](max) NULL,[status] [varchar](max) NULL) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.RuntimeModifier ADD Necessary bit NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.saledetails ADD kdsid int NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }


                try
                {
                    query = "ALTER TABLE dbo.menugroup ADD imageurl varchar(50) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.EmployeesSalary ADD branchid int NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.Expenses ADD uploadstatus varchar(50) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.Expenses ADD onlineid int NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.Employees ADD onlineid int NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.EmployeesSalary ADD onlineid int NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.EmployeesSalary ADD uploadstatus varchar(50) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.Employees ADD uploadstatus varchar(50) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.Menuitem ADD imageurl varchar(500) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }


                try
                {
                    query = "ALTER TABLE dbo.Menuitem ADD startdate varchar(50) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.Menuitem ADD currentmargin float NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.InventoryTransfer ADD sourcebranchid int NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.InventoryTransfer ADD onlineid int NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery(); 
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.InventoryTransfer ADD price float NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.InventoryTransfer ADD total float NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.InventoryTransfer ADD status varchar(50) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.Menuitem ADD proposedprice float NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.Menuitem ADD ProposedMargin float NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }

                try
                {
                    query = "ALTER TABLE dbo.ModifierFlavour ADD currentmargin float NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.ModifierFlavour ADD proposedprice float NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.ModifierFlavour ADD ProposedMargin float NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.ModifierFlavour ADD cost float NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }

                try
                {
                    query = "ALTER TABLE dbo.Menuitem ADD enddate varchar(50) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.Menuitem ADD cost float NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.rawitem ADD maxorder varchar(500) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.ExtraPrice ADD Menuitemid int NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.delivery ADD type varchar(50) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.ExtraPrice ADD flavourid int NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.Saledetails ADD kdsgroup int NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                
                try
                {
                    query = "CREATE TABLE [dbo].[JV](	[id] [int] IDENTITY(1,1) NOT NULL,	[voucherno] [varchar](max) NULL) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.sale ADD DeliveredTime datetime NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.CompanyInfo ADD CustomerMessage varchar(max) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                
                try
                {
                    query = "CREATE TABLE [dbo].[Attachmenu1](	[id] [int] IDENTITY(1,1) NOT NULL,	[menuitemid] [int] NULL,	[Flavourid] [int] NULL,	[attachmenuid] [int] NULL,	[attachFlavourid] [int] NULL,	[Quantity] [float] NULL,	[status] [varchar](50) NULL, CONSTRAINT [PK_Attachmenu1] PRIMARY KEY CLUSTERED (	[id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                 try
                {
                    query = "CREATE TABLE [dbo].[Ordercomplete](	[id] [int] IDENTITY(1,1) NOT NULL,	[saledetailsid] [int] NULL,	[itemid] [int] NULL) ON [PRIMARY]  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.CompanyInfo ADD CustomerMessage2 varchar(max) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.Saledetails ADD time datetime NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = " CREATE TABLE [dbo].[DiscardCompleteWaste](	[id] [numeric](18, 0) NOT NULL,	[itemid] [int] NULL,	[date] [date] NULL,	[quantity] [float] NULL,	[Discard] [float] NULL,	[Remaining] [float] NULL,	[staff] [float] NULL,	[completewaste] [float] NULL,	[uploadstatus] [varchar](50) NULL,	[Onlineid] [int] NULL,	[Remarks] [varchar](50) NULL,	[branchid] [int] NULL, CONSTRAINT [PK_DiscardCompleteWaste] PRIMARY KEY CLUSTERED (	[id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = " CREATE TABLE [dbo].[DiscardStaff](	[id] [numeric](18, 0) NOT NULL,	[itemid] [int] NULL,	[date] [date] NULL,	[quantity] [float] NULL,	[Discard] [float] NULL,	[Remaining] [float] NULL,	[staff] [float] NULL,	[completewaste] [float] NULL,	[uploadstatus] [varchar](50) NULL,	[Onlineid] [int] NULL,	[Remarks] [varchar](50) NULL,	[branchid] [int] NULL, CONSTRAINT [PK_DiscardStaff] PRIMARY KEY CLUSTERED (	[id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[Expenses](	[Id] [int] IDENTITY(1,1) NOT NULL,	[Name] [varchar](max) NULL,	[Amount] [float] NULL,	[Date] [date] NULL,	[branchid] [int] NULL, CONSTRAINT [PK_Expenses] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
               try
                {
                    query = " CREATE TABLE [dbo].[Theme](	[id] [int] IDENTITY(1,1) NOT NULL,	[Red] [int] NULL,	[Green] [int] NULL,	[Blue] [int] NULL,	[type] [varchar](50) NULL,	[subtype] [varchar](50) NULL, CONSTRAINT [PK_Theme] PRIMARY KEY CLUSTERED (	[id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[Employees](	[Id] [int] IDENTITY(1,1) NOT NULL,	[EmpId] [int] NOT NULL,	[Name] [varchar](50) NULL,	[Phone] [varchar](50) NULL,	[Designation] [varchar](50) NULL,	[JoiningDate] [date] NULL, CONSTRAINT [PK_Employees] PRIMARY KEY CLUSTERED (	[EmpId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = " CREATE TABLE [dbo].[DiscountIndividual](	[id] [int] IDENTITY(1,1) NOT NULL,	[DiscountPerc] [float] NULL,	[MenuItemId] [int] NULL,	[Saleid] [int] NULL,[Saledetailsid] [int] NULL,[Runtimemodifierid] [int] NULL,[flavourid] [int] NULL,	[Userid] [int] NULL,	[Date] [datetime] NULL, CONSTRAINT [PK_DiscountIndividual] PRIMARY KEY CLUSTERED (	[id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[EmployeesSalary](	[id] [int] IDENTITY(1,1) NOT NULL,	[Empid] [int] NULL,[Amount] [float] NULL,	[Date] [date] NULL, CONSTRAINT [PK_EmployeesSalary] PRIMARY KEY CLUSTERED (	[id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.RuntimeModifier ADD baseimage varchar(max) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.menuitem ADD baseimage varchar(max) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.menugroup ADD baseimage varchar(max) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.DiscountIndividual ADD discount float NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.discard ADD branchid int NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.Demand ADD supplierid int NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.Employees ADD branchid int NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.discard ADD branchid int NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.InventoryTransfer ADD branchid int NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.Discard ADD Remarks varchar(50) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.menuitem ADD modifiercount varchar(50) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.InventoryTransfer ADD Remarks varchar(50) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }

                try
                {
                    query = "ALTER TABLE dbo.Discard ADD Onlineid int NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.MenuItem ADD Target float NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.SqlServerInfo ADD type varchar(50) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.sale ADD uploadstatusserver varchar(50) NULL  DEFAULT('Pending');  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.Branch ADD type varchar(50) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }


                try
                {
                    query = "ALTER TABLE dbo.RuntimeModifier ADD type varchar(50) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.Recipe ADD type varchar(50) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.DeviceSetting ADD port varchar(50) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.Discard ADD uploadstatus varchar(50) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.kds ADD KDSid int NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                }


                try
                {
                    query = "ALTER TABLE dbo.kds ADD terminal varchar(50) NULL ;";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                }

                try
                {
                    query = "ALTER TABLE dbo.sale ADD invoice int NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                }


                try
                {
                    query = "ALTER TABLE dbo.BillType ADD cashoutime datetime NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                }


                try
                {
                    query = "ALTER TABLE dbo.Saledetails ADD atid int NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                }
                try
                {
                    query = "ALTER TABLE dbo.Saledetails ADD dealid int NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                }
                try
                {
                    query = "ALTER TABLE dbo.RawItem ADD MinOrder float NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                }
                try
                {
                    query = "ALTER TABLE dbo.users ADD role varchar(50) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.MenuGroup ADD role varchar(50) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.sale ADD phone varchar(50) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.sale ADD discountid varchar(50) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.discard  ADD completewaste float NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }

                try
                {
                    query = "ALTER TABLE dbo.discard  ADD staff float NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                } 
                try
                {
                    query = "ALTER TABLE dbo.printers ADD Terminal varchar(50) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.printtype ADD Terminal varchar(50) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.ResturantStaff ADD ChartaccountId int NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                }
                try
                {
                    query = "CREATE TABLE [dbo].[deliverytransfer](	[type] [varchar](50) NULL,	[url] [varchar](max) NULL) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                }
                try
                {
                    query = "CREATE TABLE [dbo].[DiscountCompaign](	[Id] [int] IDENTITY(1,1) NOT NULL,	[Name] [varchar](max) NULL,	[Discount] [float] NULL,	[Monday] [bit] NULL,	[Tuesday] [bit] NULL,	[Wednesday] [bit] NULL,	[Thursday] [bit] NULL,	[Friday] [bit] NULL,	[Saturday] [bit] NULL,	[Sunday] [bit] NULL,	[AllDay] [bit] NULL,	[Datefrom] [date] NULL,	[DateTo] [date] NULL,	[TimeFrom] [time](7) NULL,	[TimeTo] [time](7) NULL,	[Status] [varchar](50) NULL) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                }
                try
                {
                    query = "CREATE TABLE [dbo].[ExtraPrice](	[id] [int] IDENTITY(1,1) NOT NULL,	[Type] [varchar](50) NULL,	[amount] [float] NULL, CONSTRAINT [PK_ExtraPrice] PRIMARY KEY CLUSTERED (	[id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                }
                try
                {
                    query = "CREATE TABLE [dbo].[Cashout](	[id] [int] IDENTITY(1,1) NOT NULL,	[terminal] [varchar](max) NULL,	[status] [varchar](50) NULL, CONSTRAINT [PK_Cashout] PRIMARY KEY CLUSTERED  (	[id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[CompleteWaste](	[id] [int] IDENTITY(1,1) NOT NULL,	[Flavouid] [varchar](50) NULL,	[Menuitemid] [int] NULL,	[Quantity] [float] NULL,	[Date] [date] NULL,	[Status] [varchar](50) NULL, CONSTRAINT [PK_CompleteWaste] PRIMARY KEY CLUSTERED  (	[id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                

                try
                {
                    query = "CREATE TABLE [dbo].[InventoryTransfer](	[id] [int] IDENTITY(1,1) NOT NULL,	[Date] [date] NULL,	[Itemid] [int] NULL,	[TransferIn] [float] NULL,	[TransferOut] [float] NULL,	[branchid] [int] NULL,  CONSTRAINT [PK_InventoryTransfer] PRIMARY KEY CLUSTERED  (	[id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }

                try
                {
                    query = "CREATE TABLE [dbo].[Messages] (	[id] [int] IDENTITY(1,1) NOT NULL,	[type] [varchar](100) NULL,	[message] [varchar](max) NULL,[url] [varchar](max) NULL, CONSTRAINT [PK_Messages] PRIMARY KEY CLUSTERED (	[id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.printtype ADD printer varchar(20) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.RuntimeModifier ADD menuItemid int NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.RuntimeModifier ADD rawitemid int NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.shiftstart ADD useridstart int NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.Mailsetting ADD path varchar(20) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.Threading ADD terminal varchar(50) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.sale ADD servicecharges float NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.sale1 ADD servicecharges float NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.CompanyInfo ADD logoname varchar(20) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.sale ADD TerminalOrder varchar(50) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.CompanyInfo ADD width int NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.shiftstart ADD useridend int NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    
                    
                }
                try
                {
                    query = "ALTER TABLE dbo.shiftstart ADD Terminal varchar(20) ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    
                   
                }
                try
                {
                    query = "CREATE TABLE [dbo].[pterminal](	[id] [int] IDENTITY(1,1) NOT NULL,	[terminal] [varchar](50) NULL) ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[DiscountDetails](	[id] [int] NULL,	[name] [varchar](max) NULL,	[phone] [varchar](max) NULL,	[staff] [varchar](max) NULL,[Reference] [varchar](max) NULL,	[saleid] [int] NULL) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.DiscountDetails ADD Reference varchar(max) NULL;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[DiscountDetailsEnable](	[status] [varchar](50) NULL) ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[LoyalityCards]([id] [int] NOT NULL,	[cardno] [varchar](max) NULL,	[discount] [float] NULL,	[date] [date] NULL,	[name] [varchar](max) NULL,	[phone] [varchar](max) NULL, CONSTRAINT [PK_LoyalityCards] PRIMARY KEY CLUSTERED  (	[id] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] ) ON [PRIMARY] ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[LoyalityCardsales](	[id] [int] IDENTITY(1,1) NOT NULL,	[saleid] [int] NULL,	[cardid] [int] NULL, CONSTRAINT [PK_LoyalityCardsales] PRIMARY KEY CLUSTERED (	[id] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] ) ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[Threading](	[id] [int] IDENTITY(1,1) NOT NULL,	[status] [varchar](50) NULL) ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[EmployeeRecvb] ([id] [int] NOT NULL,	[empid] [int] NULL,	[amount] [float] NULL,	[date] [date] NULL, CONSTRAINT [PK_EmployeeRecvb] PRIMARY KEY CLUSTERED (	[id] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] ) ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[SubMenugroup](	[id] [int] NOT NULL,	[name] [varchar](max) NULL,	[menugroupid] [int] NULL,	[status] [varchar](50) NULL CONSTRAINT [DF_SubMenugroup_status]  DEFAULT ('Active'),CONSTRAINT [PK_SubMenugroup] PRIMARY KEY CLUSTERED (	[id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE [dbo].[SubMenugroup]  WITH CHECK ADD  CONSTRAINT [FK_SubMenugroup_MenuGroup] FOREIGN KEY([menugroupid]) REFERENCES [dbo].[MenuGroup] ([Id])";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[cashdrawer](	[id] [int] IDENTITY(1,1) NOT NULL,	[opendrawer] [varchar](50) NULL ) ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[tabs](	[id] [int] IDENTITY(1,1) NOT NULL,	[tabname] [varchar](50) NULL,	[targetterminal] [varchar](50) NULL, CONSTRAINT [PK_tabs] PRIMARY KEY CLUSTERED (	[id] ASC) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]) ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[SerivceCharges]	([id] [int] IDENTITY(1,1) NOT NULL,	[charges] [float] NULL, CONSTRAINT [PK_SerivceCharges] PRIMARY KEY CLUSTERED ([id] ASC)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]) ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[sortby](	[id] [int] IDENTITY(1,1) NOT NULL,	[name] [varchar](50) NULL,	[sort] [varchar](50) NULL) ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[linelength](	[id] [int] NULL,	[length] [varchar](50) NULL,	[type] [varchar](50) NULL,	[printr] [varchar](50) NULL) ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[selecttype]	([id] [int] IDENTITY(1,1) NOT NULL,	[selecttype] [varchar](50) NULL, CONSTRAINT [PK_selecttype] PRIMARY KEY CLUSTERED ([id] ASC)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]) ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[Mailsetting](	[id] [int] NULL,	[mailfrom] [varchar](50) NULL,	[password] [varchar](50) NULL,	[head] [varchar](50) NULL,	[body] [varchar](50) NULL,	[host] [varchar](50) NULL,	[mailto] [varchar](50) NULL,	[status] [varchar](50) NULL) ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE dbo.Mailsetting ADD port int NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {



                }
                try
                {
                    query = "ALTER TABLE dbo.Mailsetting ADD cc1 varchar(50) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {



                }
                try
                {
                    query = "ALTER TABLE dbo.Mailsetting ADD cc2 varchar(50) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                }
                try
                {
                    query = "ALTER TABLE dbo.Mailsetting ADD cc3 varchar(50) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {



                }
                try
                {
                    query = "ALTER TABLE dbo.Users ADD kdsid varchar(20) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                }
                try
                {
                    query = "ALTER TABLE dbo.Users ADD terminal varchar(20) NULL ;  ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "CREATE TABLE [dbo].[Banks](	[id] [int] NULL,	[Name] [varchar](50) NULL) ON [PRIMARY]";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                try
                {
                    query = "ALTER TABLE Saledetailsrefund DROP CONSTRAINT PK_SaledetailsSaledetailsrefund; ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                   
                }
                catch (Exception ex)
                {


                }
                try
                {
                   
                    query = "ALTER TABLE Saledetailsrefund DROP CONSTRAINT PK_Saledetailsrefund; ";
                    com = new SqlCommand(query, this.connection);
                    res = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {


                }
                query = "ALTER TABLE dbo.shiftcash ADD Terminal varchar(20) ;  ";
                com = new SqlCommand(query, this.connection);
                res = com.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

            }
        }
        public string authentication1(string form, string uid)
        {
            DataSet ds = new DataSet();
            string val = "";
            try
            {
                string user = uid;
                
                string q = "SELECT     dbo.Rights.Status, dbo.Forms.Forms, dbo.Rights.Userid FROM         dbo.Rights INNER JOIN                      dbo.Forms ON dbo.Rights.formid = dbo.Forms.Id where  dbo.Rights.Userid ='" + user + "' and dbo.Forms.Forms='" + form + "'";
                if (this.connection.State == ConnectionState.Open)
                    this.connection.Close();
                this.connection.Open();
                SqlCommand com = new SqlCommand(q, this.connection);
                SqlDataAdapter da = new SqlDataAdapter(com);
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    val = ds.Tables[0].Rows[0]["Status"].ToString(); ;
                }
                else
                {
                    val = "no";
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                this.connection.Close();
            }
            return val;
        }
        public string authenticate(string type)
        {
            string form = POSRestaurant.Properties.Settings.Default.formname;
           
            DataSet ds = new DataSet();
            string val = "yes";
            //try
            //{
            //    string user = "";

            //    user = POSRestaurant.Properties.Settings.Default.UserId.ToString();
                
            //    string q = "SELECT     dbo.Rights.Status,InsertStatus, UpdateStatus, DeleteStaus, dbo.Forms.Forms, dbo.Rights.Userid FROM         dbo.Rights INNER JOIN                      dbo.Forms ON dbo.Rights.formid = dbo.Forms.Id where  dbo.Rights.Userid ='" + user + "' and dbo.Forms.Forms='" + form + "'";
            //    if (this.connection.State == ConnectionState.Open)
            //        this.connection.Close();
            //    this.connection.Open();
            //    SqlCommand com = new SqlCommand(q, this.connection);
            //    SqlDataAdapter da = new SqlDataAdapter(com);
            //    da.Fill(ds);
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        if (type == "insert")
            //        {
            //            val = ds.Tables[0].Rows[0]["InsertStatus"].ToString(); ;
            //        }
            //        if (type == "update")
            //        {
            //            val = ds.Tables[0].Rows[0]["UpdateStatus"].ToString(); ;
            //        }
            //        if (type == "delete")
            //        {
            //            val = ds.Tables[0].Rows[0]["DeleteStaus"].ToString(); ;
            //        }
            //    }
            //    else
            //    {
            //        val = "no";
            //    }
            //}
            //catch (Exception ex)
            //{

            //}
            //finally
            //{
            //    this.connection.Close();
            //}
            return val;
        }
        public static string authenticateEdit(string form, string uid,string type)
        {
            

            DataSet ds = new DataSet();
            string val = "";
            try
            {
                string user = "";

                user = POSRestaurant.Properties.Settings.Default.UserId.ToString();

                string q = "SELECT     dbo.Rights.Status,InsertStatus, UpdateStatus, DeleteStaus, dbo.Forms.Forms, dbo.Rights.Userid FROM         dbo.Rights INNER JOIN                      dbo.Forms ON dbo.Rights.formid = dbo.Forms.Id where  dbo.Rights.Userid ='" + uid + "' and dbo.Forms.Forms='" + form + "'";
                con = new SqlConnection(POSRestaurant.Properties.Settings.Default.ConnectionString);
                if (con.State == ConnectionState.Open)
                    con.Close();
                con.Open();
                SqlCommand com = new SqlCommand(q, con);
                SqlDataAdapter da = new SqlDataAdapter(com);
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (type == "insert")
                    {
                        val = ds.Tables[0].Rows[0]["InsertStatus"].ToString(); ;
                    }
                    if (type == "update")
                    {
                        val = ds.Tables[0].Rows[0]["UpdateStatus"].ToString(); ;
                    }
                    if (type == "delete")
                    {
                        val = ds.Tables[0].Rows[0]["DeleteStaus"].ToString(); ;
                    }
                }
                else
                {
                    val = "no";
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return val;
        }
        public string authentication(string form )
        {
            POSRestaurant.Properties.Settings.Default.formname = form;
            POSRestaurant.Properties.Settings.Default.Save();
            DataSet ds = new DataSet();
            string val = "";
            try
            {
                string user = "";
                //if (uid == string.Empty)
                {
                    user = POSRestaurant.Properties.Settings.Default.UserId.ToString();
                }
                string q = "SELECT     dbo.Rights.Status, dbo.Forms.Forms, dbo.Rights.Userid FROM         dbo.Rights INNER JOIN                      dbo.Forms ON dbo.Rights.formid = dbo.Forms.Id where  dbo.Rights.Userid ='" + user + "' and dbo.Forms.Forms='" + form + "'";
                if (this.connection.State == ConnectionState.Open)
                    this.connection.Close();
                this.connection.Open();
                SqlCommand com = new SqlCommand(q, this.connection);
                SqlDataAdapter da = new SqlDataAdapter(com);
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    val = ds.Tables[0].Rows[0]["Status"].ToString(); ;
                }
                else
                {
                    val = "no";
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                this.connection.Close();
            }
            return val;
        }
        public SqlDataReader funGetDataReader1( string tbl)
        {
            SqlDataReader dr;
            try
            {
                if (this.connection.State == ConnectionState.Open)
                    this.connection.Close();
                this.connection.Open();
                SqlCommand com = new SqlCommand(tbl, this.connection);
                dr = com.ExecuteReader();
                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public SqlDataReader funGetDataReader(string query)
        {
            SqlDataReader dr;

            {
                try
                {
                    if (this.connection.State == ConnectionState.Open)
                        this.connection.Close();
                    this.connection.Open();
                    SqlCommand com = new SqlCommand(query, this.connection);
                    dr = com.ExecuteReader();
                    return dr;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                }
            }
        }
        public DataSet funGetDataSet(string query)
        {
            DataSet ds = new DataSet();
            string Apiexecution = POSRestaurant.Properties.Settings.Default.apiExecution.ToString();
            string baseurl = POSRestaurant.Properties.Settings.Default.BaseUrl.ToString();
            if (Apiexecution == "Enabled" && baseurl.Length>0)
            {
                string url = baseurl + "/Queries/getDataset.asmx/Getresponse?q=" + query;
                try
                {
                    string uri = url;
                    HttpWebRequest request1 = WebRequest.Create(uri) as HttpWebRequest;
                    HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                    Stream stream1 = response1.GetResponseStream();
                    XmlDocument doc = new XmlDocument();
                    using (StreamReader readStream = new StreamReader(stream1, Encoding.UTF8))
                    {
                        // Load into XML document
                        string result = readStream.ReadToEnd();
                        StringReader sr = new StringReader(result);

                        ds.ReadXml(sr);
                       
                    }
                }
                catch (Exception ex)
                {


                }
            }
            else
            {
                try
                {
                    if (this.connection.State == ConnectionState.Open)
                        this.connection.Close();
                    this.connection.Open();
                    SqlCommand com = new SqlCommand(query, this.connection);
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    da.Fill(ds);
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    this.connection.Close();
                    ds.Dispose();
                }
            }
            return ds;
        }
        public int executeQuery(string query)
        {
            int result = 0;
            string status = "";
            if (query.Contains("insert into errors") || query.Contains("insert into log"))
            {
                if (this.connection.State == ConnectionState.Open)
                    this.connection.Close();
                this.connection.Open();
                SqlCommand com = new SqlCommand(query, this.connection);
                com.ExecuteNonQuery();
            }
            else
            {
                try
                {
                    if (query.Substring(0, 8).ToLower().Contains("insert"))
                    {
                        status = authenticate("insert");

                    }
                    if (query.Substring(0, 8).ToLower().Contains("update"))
                    {
                        status = authenticate("update");

                    }
                    if (query.Substring(0, 8).ToLower().Contains("delete"))
                    {
                        status = authenticate("delete");

                    }
                }
                catch (Exception ex)
                {

                }
                if (status == "no")
                {
                    Message obj = new Message();
                    obj.Show();
                    return 0;
                }
              
                string Apiexecution = POSRestaurant.Properties.Settings.Default.apiExecution.ToString();
                string baseurl = POSRestaurant.Properties.Settings.Default.BaseUrl.ToString();
                if (Apiexecution == "Enabled")
                {
                    string url = baseurl + "/Queries/execute.asmx/Getresponse?q=" + query;
                    try
                    {
                        string uri = url;
                        HttpWebRequest request1 = WebRequest.Create(uri) as HttpWebRequest;
                        HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                        Stream stream1 = response1.GetResponseStream();
                        using (StreamReader readStream = new StreamReader(stream1, Encoding.UTF8))
                        {
                            // Load into XML document
                            string resultstream = readStream.ReadToEnd();
                            List<Responseclass> res = (List<Responseclass>)JsonConvert.DeserializeObject(resultstream, typeof(List<Responseclass>));
                            if (res[0].response == "Success")
                            {

                            }
                            else
                            {

                            }

                        }
                    }
                    catch (Exception ex)
                    {


                    }
                }
                else
                {
                    try
                    {
                        if (this.connection.State == ConnectionState.Open)
                            this.connection.Close();
                        this.connection.Open();
                        SqlCommand com = new SqlCommand(query, this.connection);
                        result = com.ExecuteNonQuery();
                        if (result == 0)
                        {
                            if (this.connection.State == ConnectionState.Open)
                                this.connection.Close();
                            this.connection.Open();
                            com = new SqlCommand(query, this.connection);
                            result = com.ExecuteNonQuery();
                            if (result == 0)
                            {
                                if (this.connection.State == ConnectionState.Open)
                                    this.connection.Close();
                                this.connection.Open();
                                query = "insert into errors  (query,date) values('" + query.Replace("'", "") + "','" + DateTime.Now.ToString() + "')";
                                com = new SqlCommand(query, this.connection);
                                com.ExecuteNonQuery();
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            if (this.connection.State == ConnectionState.Open)
                                this.connection.Close();
                            this.connection.Open();
                            query = "insert into errors  (query,date,message) values('" + query.Replace("'", "") + "','" + DateTime.Now.ToString() + "','" + ex.Message.Replace("'", "") + "')";
                            SqlCommand com = new SqlCommand(query, this.connection);
                            com.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                        }
                    }
                    finally
                    {
                        this.connection.Close();
                    }
                }
            }
            return result;
        }
        public int executeQueryint(string query)
        {
           
            string status = "";
            int res = 0;
            if (query.Contains("insert into errors") || query.Contains("insert into log"))
            {
            }
            else
            {
                try
                {
                    if (query.Substring(0, 8).ToLower().Contains("insert"))
                    {
                        status = authenticate("insert");

                    }
                    if (query.Substring(0, 8).ToLower().Contains("update"))
                    {
                        status = authenticate("update");

                    }
                    if (query.Substring(0, 8).ToLower().Contains("delete"))
                    {
                        status = authenticate("delete");

                    }
                }
                catch (Exception ex)
                {

                }
                if (status == "no")
                {
                    Message obj = new Message();
                    obj.Show();
                    return 0;
                }

               
                string Apiexecution = POSRestaurant.Properties.Settings.Default.apiExecution.ToString();
                string baseurl = POSRestaurant.Properties.Settings.Default.BaseUrl.ToString();
                if (Apiexecution == "Enabled")
                {
                    string url = baseurl + "/Queries/execute.asmx/Getresponse?q=" + query;
                    try
                    {
                        string uri = url;
                        HttpWebRequest request1 = WebRequest.Create(uri) as HttpWebRequest;
                        HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                        Stream stream1 = response1.GetResponseStream();
                        using (StreamReader readStream = new StreamReader(stream1, Encoding.UTF8))
                        {
                            // Load into XML document
                            string result = readStream.ReadToEnd();
                            List<POSRestaurant.forms.Responseclass> ress = (List<Responseclass>)JsonConvert.DeserializeObject(result, typeof(List<Responseclass>));
                            if (ress[0].response == "Success")
                            {
                                res = 1;
                            }
                            else
                            {
                                res = 0;
                            }

                        }
                    }
                    catch (Exception ex)
                    {


                    }
                }
                else
                {
                    try
                    {
                        if (this.connection.State == ConnectionState.Open)
                            this.connection.Close();
                        this.connection.Open();
                        SqlCommand com = new SqlCommand(query, this.connection);
                        res = com.ExecuteNonQuery();


                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        this.connection.Close();
                    }
                }
            }

            return res;
        }
        public string getUserId()
        {
            return POSRestaurant.Properties.Settings.Default.UserId;
        }

        public bool getUserRight(int formId, string colName)
        {
            bool right = false;
            try
            {
                SqlDataReader dr = this.funGetDataReader("Select " + colName + " from UserRights Where Id = (Select UserId from Stafflogin Where UserId = '" + this.getUserId() + "') And FormId = " + formId);
                if (dr.HasRows)
                {
                    dr.Read();
                    
                }
                right = true;// Convert.ToBoolean(dr.GetValue(0));
                dr.Close();
                this.closeConnection();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return right;
        }

        public void closeConnection()
        {
            try
            {
                if (this.connection.State == ConnectionState.Open)
                    this.connection.Close();
            }
            catch (Exception ex)
            {
            }
        }
        public static Clsdbcon GetDB()
        {
            if (clsado == null)
            {
                clsado = new Clsdbcon();
                return clsado;
            }

            return clsado;
        }
        public static Clsdbcon clsado;
        //public dsRecord dtset = new dsRecord();
        //public POSRestaurant.dbsources.dsRecordTableAdapters.stockposTableAdapter brgyAdapter = new POSRestaurant.dbsources.dsRecordTableAdapters.stockposTableAdapter();
        ////public Monitoring.Database.VincentDBTableAdapters.Purok_tblTableAdapter purokAdapter = new Monitoring.Database.VincentDBTableAdapters.Purok_tblTableAdapter();
        //public Monitoring.Database.VincentDBTableAdapters.HouseHold_tblTableAdapter houseAdapter = new Monitoring.Database.VincentDBTableAdapters.HouseHold_tblTableAdapter();
        //public Monitoring.Database.VincentDBTableAdapters.Residents_tblTableAdapter residentAdapter = new Monitoring.Database.VincentDBTableAdapters.Residents_tblTableAdapter();
        //public Monitoring.Database.VincentDBTableAdapters.Vehicles_tblTableAdapter vehicleAdapter = new Monitoring.Database.VincentDBTableAdapters.Vehicles_tblTableAdapter();
        //public Monitoring.Database.VincentDBTableAdapters.StaffTableAdapter staffAdapter = new Monitoring.Database.VincentDBTableAdapters.StaffTableAdapter();
        //public Monitoring.Database.VincentDBTableAdapters.TableAdapterManager ad = new Monitoring.Database.VincentDBTableAdapters.TableAdapterManager();

        //public Monitoring.Database.VincentDBTableAdapters.ShowResidentByPurokTableAdapter showResByPurok = new Monitoring.Database.VincentDBTableAdapters.ShowResidentByPurokTableAdapter();
        //public Monitoring.Database.VincentDBTableAdapters.ShowResidentByHH_IDTableAdapter showResByHHID = new Monitoring.Database.VincentDBTableAdapters.ShowResidentByHH_IDTableAdapter();
        //public Monitoring.Database.VincentDBTableAdapters.ShowResidentByHHTableAdapter showByHH = new Monitoring.Database.VincentDBTableAdapters.ShowResidentByHHTableAdapter();
        //public Monitoring.Database.VincentDBTableAdapters.ShowResidentByYearTableAdapter showByYear = new Monitoring.Database.VincentDBTableAdapters.ShowResidentByYearTableAdapter();

        //public Monitoring.Database.VincentDBTableAdapters.GetAgeBracketTableAdapter agebracket = new Monitoring.Database.VincentDBTableAdapters.GetAgeBracketTableAdapter();
        //public Monitoring.Database.VincentDBTableAdapters.GetEmploymentByPurokTableAdapter employmentadapter = new Monitoring.Database.VincentDBTableAdapters.GetEmploymentByPurokTableAdapter();

        //public Monitoring.Database.VincentDBTableAdapters.Appliances_tblTableAdapter appliancesadapter = new Monitoring.Database.VincentDBTableAdapters.Appliances_tblTableAdapter();
        //public Monitoring.Database.VincentDBTableAdapters.Age_tblTableAdapter age = new Monitoring.Database.VincentDBTableAdapters.Age_tblTableAdapter();


        //THIS ARE THE DATASET THAT CAN BE USED WITHIN THIS MODULE;
	}
}
