using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Reports.SaleReports
{
    public partial class FrmFeedback : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public FrmFeedback()
        {
            InitializeComponent();
        }

        private void FrmMwnuGroupSale_Load(object sender, EventArgs e)
        {




        }

        public void bindreport()
        {

            try
            {
                DataTable dt = new DataTable();


                POSRestaurant.Reports.SaleReports.rptFeedback rptDoc = new rptFeedback();
                POSRestaurant.Reports.SaleReports.dsfeedback dsrpt = new dsfeedback();
                //feereport ds = new feereport(); // .xsd file name


                // Just set the name of data table
                dt.TableName = "Crystal Report";
                dt = getAllOrders();
                string company = "", phone = "", address = "", logo = "";
                try
                {
                    company = dscompany.Tables[0].Rows[0]["Name"].ToString();
                    phone = dscompany.Tables[0].Rows[0]["Phone"].ToString();
                    address = dscompany.Tables[0].Rows[0]["Address"].ToString();
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();
                }
                catch (Exception ex)
                {


                }


                dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);


                rptDoc.SetDataSource(dsrpt);
                rptDoc.SetParameterValue("comp", company);
                rptDoc.SetParameterValue("addrs", address);
                rptDoc.SetParameterValue("phn", phone);
                rptDoc.SetParameterValue("date", "for the period  " + dateTimePicker1.Text + " to " + dateTimePicker2.Text);
                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        DataSet dscompany = new DataSet();
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");

        }
        public DataTable getAllOrders()
        {

            DataTable dtrpt = new DataTable();
            try
            {

                dtrpt.Columns.Add("Question", typeof(string));
                dtrpt.Columns.Add("Option1", typeof(string));
                dtrpt.Columns.Add("Option2", typeof(string));
                dtrpt.Columns.Add("Option3", typeof(string));
                dtrpt.Columns.Add("Option4", typeof(string));
                getcompany();
                string logo = "";
                try
                {
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

                }
                catch (Exception ex)
                {
                }
                DataSet ds = new DataSet();
                string q = "";


                q = "select * from FeedBackQuestions";
                DataSet dsq = new DataSet();
                dsq = objCore.funGetDataSet(q);
                foreach (DataRow dr in dsq.Tables[0].Rows)
                {
                    string questionid = dr["Id"].ToString();
                    string question = dr["Question"].ToString();
                    string Option1 = dr["Option1"].ToString();
                    string Option2 = dr["Option2"].ToString();
                    string Option3 = dr["Option3"].ToString();
                    string Option4 = dr["Option4"].ToString();

                    if (question.ToLower() == "name" || question.ToLower() == "phone")
                    {

                    }
                    else
                    {
                        q = "select count(*) as Count,Answer1, Answer2, Answer3, Answer4 from FeedBackAnswers where QuestionId='" + questionid + "' and date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' group by Answer1, Answer2, Answer3, Answer4";
                        ds = objCore.funGetDataSet(q);

                        try
                        {
                            List<FeedbackAnswers> feedbacks = new List<FeedbackAnswers>();
                            IList<FeedbackAnswers> data = ds.Tables[0].AsEnumerable().Select(row =>
                                new FeedbackAnswers
                                {
                                    Count = row.Field<int>("Count"),
                                    Answer1 = row.Field<string>("Answer1"),
                                    Answer2 = row.Field<string>("Answer2"),
                                    Answer3 = row.Field<string>("Answer3"),
                                    Answer4 = row.Field<string>("Answer4")

                                }).ToList();
                            feedbacks = data.ToList();
                            int ans1 = 0, ans2 = 0, ans3 = 0, ans4 = 0;
                            if (question.ToLower() == "comments")
                            {
                                dtrpt.Rows.Add("Other Comments", "Name", "Phone", "", "Comments");
                            }
                            foreach (var item in feedbacks)
                            {

                                if (question.ToLower() == "comments")
                                {
                                    dtrpt.Rows.Add("Other Comments", item.Answer2, item.Answer3, "", item.Answer1);
                                }

                                if (item.Answer1.Trim().Length > 0)
                                {

                                    ans1 = ans1 + item.Count;
                                }
                                if (item.Answer2.Trim().Length > 0)
                                {

                                    ans2 = ans2 + item.Count;
                                }
                                if (item.Answer3.Trim().Length > 0)
                                {

                                    ans3 = ans3 + item.Count;
                                }
                                if (item.Answer4.Trim().Length > 0)
                                {

                                    ans4 = ans4 + item.Count;
                                }
                            }
                            if (question.ToLower() == "comments")
                            {

                            }
                            else
                            {
                                dtrpt.Rows.Add(question, Option1, Option2, Option3, Option4);
                                dtrpt.Rows.Add(question, ans1, ans2, ans3, ans4);
                            }
                        }
                        catch (Exception ex)
                        {


                        }


                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return dtrpt;
        }
        private void vButton1_Click(object sender, EventArgs e)
        {
            bindreport();
        }
    }
}
