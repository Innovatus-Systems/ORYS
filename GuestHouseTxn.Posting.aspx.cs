using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using System.Drawing;
using Telerik.Web.UI;
using System.IO;
using System.Net.Mail;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Text;

public partial class GuestHouseTxn_Posting : System.Web.UI.Page
{
    SqlProcsNew sqlobj = new SqlProcsNew();   
    static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CovaiSoft"].ConnectionString);
    static string contName;
    static string commnty;
    static string Address1;
    static string Address2;
    static string Address3;
    static string Pan;
    static string gstin;
    static string Jurisdiction;
    static string BankName;
    static string BranchName;
    static string AccountNo;
    static string IFSCCode;
    static string payment;
    static string strFooter;
    static string Duration;
    static decimal DI = 0;
    static decimal MI = 0;
    static decimal NC = 0;
    static string invNo;
    static decimal Amount;
    static decimal Total;
    static decimal CGSTAmt;
    static decimal SGSTAmt;
    static string RTRSN;
    static string Villa;
    static string Resident;
    static string Adress;
    static string Cty;
    static string Accountcode;
    static string outstd;
    static string strQuery;
    DateTime Odate;
    static string printedOn;
    static string date;
    DataTable dtservice = null;
    static string whr;
    static string Bmonth;
    static string BFROM;
    static string BTILL;
    static string Email;
    static string FileName1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
        {
            Response.Redirect("Login.aspx");
        }
        if (!IsPostBack)
        {
            LoadTitle();
            LoadResidentDet();
            LoadTxnDrp();
            lblDisable();
            //LoadGrid();
            gvTransactions.DataSource = string.Empty;
            gvTransactions.DataBind();
        }
    }
    private void LoadTitle()
    {
        try
        {
            DataSet dsTitle = sqlobj.ExecuteSP("SP_GetTitleMenus", new SqlParameter() { ParameterName = "@MenuId", SqlDbType = SqlDbType.Int, Value = 146 });
            if (dsTitle.Tables[0].Rows.Count > 0)
            {
                lnktitle.Text = dsTitle.Tables[0].Rows[0]["Title"].ToString();
                lnktitle.ToolTip = dsTitle.Tables[0].Rows[0]["Description"].ToString();
            }

            dsTitle.Dispose();
        }
        catch (Exception ex)
        {
            WebMsgBox.Show(ex.Message);
        }
    }
    private void LoadHelp()
    {
        try
        {

            DataSet dsTxn = sqlobj.ExecuteSP("SP_TxnDropDownList",
                new SqlParameter() { ParameterName = "@iMode", SqlDbType = SqlDbType.Int, Value = 2 },
               new SqlParameter() { ParameterName = "@TxnCode", SqlDbType = SqlDbType.NVarChar, Value = drpTxn.SelectedValue.ToString() });
            if (dsTxn.Tables[0].Rows.Count > 0)
            {
                lblhelp.Visible = true;
                lblhelp1.Visible = true;
                lblMsg.Visible = true;
                lblnarration.Visible = true;
                lblstdnarr.Visible = true;
                lblhelp1.Text = dsTxn.Tables[0].Rows[0]["Help"].ToString();
                lblstdnarr.Text = dsTxn.Tables[0].Rows[0]["StdNarration"].ToString();
                string msg = dsTxn.Tables[0].Rows[0]["StdDescription"].ToString();
                string CGST = dsTxn.Tables[0].Rows[0]["CGST"].ToString();
                string SGST = dsTxn.Tables[0].Rows[0]["SGST"].ToString();
                string code = dsTxn.Tables[0].Rows[0]["TxnCode"].ToString();
                string All = msg + " - " + code + "  <br/> ";
                lblMsg.Text = All;
            }


            dsTxn.Dispose();
        }
        catch (Exception ex)
        {
            WebMsgBox.Show(ex.Message);
        }
    }
    private void LoadTxnDrp()
    {
        try
        {
            DataSet dsTxn = sqlobj.ExecuteSP("SP_TxnDropDownList", new SqlParameter() { ParameterName = "@iMode", SqlDbType = SqlDbType.Int, Value = 7 });
            if (dsTxn.Tables[0].Rows.Count > 0)
            {
                drpTxn.DataSource = dsTxn.Tables[0];
                drpTxn.DataValueField = "TxnCode";
                drpTxn.DataTextField = "StdDescription";
                drpTxn.DataBind();
            }
            drpTxn.Items.Insert(0, "Please Select");
            dsTxn.Dispose();
        }
        catch (Exception ex)
        {
            WebMsgBox.Show(ex.Message);
        }
    }

    protected void LoadResidentDet()
    {
        try
        {
            DataSet dsResident = new DataSet();
            dsResident = sqlobj.ExecuteSP("SP_GenDropDownList",
                 new SqlParameter() { ParameterName = "@iMode", SqlDbType = SqlDbType.Int, Value = 7 }
                );
            cmbResident.DataSource = dsResident.Tables[0];
            cmbResident.DataValueField = "RSN";
            cmbResident.DataTextField = "GName";
            cmbResident.DataBind();
            RadComboBoxItem item2 = new RadComboBoxItem();
            item2.Text = "Please Select";
            item2.Value = "0";
            item2.Selected = true;
            cmbResident.Items.Add(item2);
            dsResident.Dispose();
        }
        catch (Exception ex)
        {
            WebMsgBox.Show(ex.Message.ToString());
        }
    }
    protected void LoadGrid()
    {
        try
        {

            DataSet dsTransactions = sqlobj.ExecuteSP("SP_TxnDropDownList",

                new SqlParameter() { ParameterName = "@IMode", SqlDbType = SqlDbType.Int, Value = 8 },
               new SqlParameter() { ParameterName = "@SelectedValue", SqlDbType = SqlDbType.NVarChar, Value = cmbResident.SelectedValue.ToString() }
               );

            if (dsTransactions.Tables[0].Rows.Count > 0)
            {
                gvTransactions.DataSource = dsTransactions.Tables[0];
                gvTransactions.DataBind();
            }
            else
            {
                gvTransactions.DataSource = string.Empty;
                gvTransactions.DataBind();
            }
            dsTransactions.Dispose();
        }
        catch (Exception ex)
        {
            WebMsgBox.Show(ex.Message.ToString());
        }
    }
    protected void drpTxn_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            LoadHelp();
            DataSet dstxnCode = sqlobj.ExecuteSP("SP_TxnDropDownList",
               new SqlParameter() { ParameterName = "@iMode", SqlDbType = SqlDbType.Int, Value = 2 },
               new SqlParameter() { ParameterName = "@TxnCode", SqlDbType = SqlDbType.NVarChar, Value = drpTxn.SelectedValue.ToString() });
            if (dstxnCode.Tables[0].Rows.Count > 0)
            {
                lblCgst1.Text = dstxnCode.Tables[0].Rows[0]["CGST"].ToString();
                lblSgst1.Text = dstxnCode.Tables[0].Rows[0]["SGST"].ToString();
                if (!string.IsNullOrEmpty(lblCgst1.Text) && !string.IsNullOrEmpty(lblSgst1.Text))
                {
                    lblCgst1.Visible = true;
                    lblSgst1.Visible = true;
                    LabelCGST1.Visible = true;
                    LabelSGST1.Visible = true;
                }
                else
                {
                    lblCgst1.Visible = false;
                    lblSgst1.Visible = false;
                    LabelCGST1.Visible = false;
                    LabelSGST1.Visible = false;
                }
            }

            dstxnCode.Dispose();
        }
        catch (Exception ex)
        {
            WebMsgBox.Show(ex.Message);
        }
    }

    protected void lblDisable()
    {
        try
        {
            lblMsg.Visible = false;
            lblhelp.Visible = false;
            lblhelp1.Visible = false;
            lblSGST2.Visible = false;
            lblCGST2.Visible = false;
            LabelCGST.Visible = false;
            LabelSGST.Visible = false;
            lblCgst1.Visible = false;
            lblSgst1.Visible = false;
            LabelCGST1.Visible = false;
            LabelSGST1.Visible = false;
            lblnarration.Visible = false;
            lblstdnarr.Visible = false;
            LabelOutSt2.Visible = false;
            lbloutstd2.Visible = false;
            LabelNewBal.Visible = false;
            lblNewBal.Visible = false;           
            LabelAccNo.Visible = false;
            lblAccNo.Visible = false;
            LabelOutSt.Visible = false;
            lblOtSt.Visible = false;
            lblChkedOt.Visible = false;
            lblCheckedin.Visible = false;
            lblStatus.Visible = false;
            Label1.Visible = false;
            Label2.Visible = false;
            Label5.Visible = false;
        }
        catch (Exception ex)
        {
            WebMsgBox.Show(ex.Message);
        }
    }
    protected void txtCAmount_TextChanged(object sender, EventArgs e)
    {
        try
        {


            if (string.IsNullOrEmpty(txtCAmount.Text))
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Script", "Sys.Application.add_load(HideUpdateProgress);", true);
                WebMsgBox.Show("Please Enter valid amount.");
                return;
            }

            if (cmbResident.SelectedValue.ToString() == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Script", "Sys.Application.add_load(HideUpdateProgress);", true);
                WebMsgBox.Show("Please select resident.");
                return;
            }
            LabelNewBal.Visible = true;
            lblNewBal.Visible = true;
            DataSet dsDetails = sqlobj.ExecuteSP("SP_TxnDropDownList", new SqlParameter() { ParameterName = "@iMode", SqlDbType = SqlDbType.Int, Value = 9 },
                   new SqlParameter() { ParameterName = "@SelectedValue", SqlDbType = SqlDbType.NVarChar, Value = cmbResident.SelectedValue.ToString() });
            if (dsDetails.Tables[0].Rows.Count > 0)
            {
                lbloutstd2.Text = dsDetails.Tables[0].Rows[0]["Outstanding"].ToString();
                LabelOutSt2.Visible = true;
                lbloutstd2.Visible = true;
            }
            Double Amount = Convert.ToDouble(txtCAmount.Text);
            Double CGST = 0.00;
            Double SGST = 0.00;
            Double CalCGST = 0.00;
            Double CalSGST = 0.00;

            if (drpTxn.SelectedValue == "DP" || drpTxn.SelectedValue == "DR")
            {
                LabelNewBal.Visible = false;
                lblNewBal.Visible = false;
                LabelOutSt2.Visible = false;
                lbloutstd2.Visible = false;

            }
            if (!string.IsNullOrEmpty(lblCgst1.Text) && !string.IsNullOrEmpty(lblSgst1.Text))
            {
                CGST = Convert.ToDouble(lblCgst1.Text);
                SGST = Convert.ToDouble(lblSgst1.Text);
                CalCGST = (Amount * (CGST / 100));
                CalSGST = (Amount * (SGST / 100));
                lblSGST2.Visible = true;
                lblCGST2.Visible = true;
                LabelCGST.Visible = true;
                LabelSGST.Visible = true;
                lblCGST2.Text = CalCGST.ToString("F");
                lblSGST2.Text = CalSGST.ToString("F");
            }
            else
            {

            }

            lblNewBal.Text = (Convert.ToDouble(lbloutstd2.Text) + (Amount + CalCGST + CalSGST)).ToString("F");
        }
        catch (Exception ex)
        {
            WebMsgBox.Show(ex.ToString());
        }

    }
    protected void cmbResident_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {

        //LoadGrid();
        rdbResident_CheckedChanged(sender, e);
        rdbResident.Checked = true;
        DataSet dsDetails = sqlobj.ExecuteSP("SP_TxnDropDownList", new SqlParameter() { ParameterName = "@iMode", SqlDbType = SqlDbType.Int, Value = 9 },
                new SqlParameter() { ParameterName = "@SelectedValue", SqlDbType = SqlDbType.NVarChar, Value = cmbResident.SelectedValue.ToString() },
                 new SqlParameter() { ParameterName = "@TxnCode", SqlDbType = SqlDbType.NVarChar, Value = drpTxn.SelectedValue.ToString() });
        if (dsDetails.Tables[0].Rows.Count > 0)
        {            
            lblAccNo.Text = dsDetails.Tables[0].Rows[0]["AccountCode"].ToString();
            Session["GLAccount"] = dsDetails.Tables[0].Rows[0]["AccountCode"].ToString();
            lblOtSt.Text = dsDetails.Tables[0].Rows[0]["Outstanding"].ToString();
            lblStatus.Text = dsDetails.Tables[0].Rows[0]["Status"].ToString();
            lblCheckedin.Text = dsDetails.Tables[0].Rows[0]["Arrived"].ToString();
            lblChkedOt.Text = dsDetails.Tables[0].Rows[0]["Checkedout"].ToString();
            LabelAccNo.Visible = true;
            lblAccNo.Visible = true;
            LabelOutSt.Visible = true;
            lblOtSt.Visible = true;
            lblChkedOt.Visible = true;
            lblCheckedin.Visible = true;
            lblStatus.Visible = true;
            Label1.Visible = true;
            Label2.Visible = true;
            Label5.Visible = true;
        }
    }
    protected void rdbResident_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cmbResident.SelectedValue != "0")
            {
                LoadGrid();
            }
            else
            {
                gvTransactions.DataSource = string.Empty;
                gvTransactions.DataBind();
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('Please Select Guest, And Try Again.');", true);
                return;
            }


        }
        catch (Exception ex)
        {
            WebMsgBox.Show(ex.Message.ToString());
        }
    }
    protected void rdbAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            DataSet dsTransactions = sqlobj.ExecuteSP("SP_TxnDropDownList",

                new SqlParameter() { ParameterName = "@IMode", SqlDbType = SqlDbType.Int, Value = 11 }
               );

            if (dsTransactions.Tables[0].Rows.Count > 0)
            {
                gvTransactions.DataSource = dsTransactions.Tables[0];
                gvTransactions.DataBind();
            }
            else
            {
                gvTransactions.DataSource = string.Empty;
                gvTransactions.DataBind();
            }
            dsTransactions.Dispose();
        }
        catch (Exception ex)
        {
            WebMsgBox.Show(ex.Message.ToString());
        }
    }
    protected void btnCClear_Click(object sender, EventArgs e)
    {
        txtCAmount.Text = "";
        txtRemarks.Text = "";
        //LoadTxnDrp();
        //LoadResidentDet();      
        rdbResident.Checked = true;
        lblDisable();
        gvTransactions.DataSource = string.Empty;
        gvTransactions.DataBind();
        drpTxn.SelectedIndex = 0;

        cmbResident.SelectedValue = "0";
    }
    protected void btnTransSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (drpTxn.SelectedValue == "0")
            {
                WebMsgBox.Show("Please Select Transaction.");
                return;
            }
            if (cmbResident.SelectedValue == "0")
            {
                WebMsgBox.Show("Please Select Resident.");
                return;
            }
            if (string.IsNullOrEmpty(txtCAmount.Text) || txtCAmount.Text == "0")
            {
                WebMsgBox.Show("Please Enter Valid Amount.");
                return;
            }
            if (string.IsNullOrEmpty(txtRemarks.Text))
            {
                WebMsgBox.Show("Please Enter Remarks.");
                return;
            }
            DateTime bdate = DateTime.Now;

            string strday = bdate.ToString("dd");
            string strmonth = bdate.ToString("MM");
            string stryear = bdate.ToString("yyyy");
            string strhour = bdate.ToString("HH");
            string strmin = bdate.ToString("mm");
            string strsec = bdate.ToString("ss");

            string strBillNo = stryear.ToString() + strmonth.ToString() + strday.ToString() + strhour.ToString() + strmin.ToString() + strsec.ToString();

            //if (!string.IsNullOrEmpty(txtCAmount.Text) || txtCAmount.Text == "0" && !string.IsNullOrEmpty(txtCAmount.Text) || txtCAmount.Text == "0" && txtRemarks.Text != "")
            //{SP_InsertGuestUNBILLEDTxnPosting

            if (drpTxn.SelectedValue == "GC" || drpTxn.SelectedValue == "GP")
            {
                DataSet dsRSN = sqlobj.ExecuteSP("SP_InsertGuestTxnPosting",
                                       new SqlParameter() { ParameterName = "@RTRSN", SqlDbType = SqlDbType.Decimal, Value = 0000 },
                                       new SqlParameter() { ParameterName = "@BGroup", SqlDbType = SqlDbType.NVarChar, Value = drpTxn.SelectedValue.ToString() },
                                       new SqlParameter() { ParameterName = "@BCategory", SqlDbType = SqlDbType.NVarChar, Value = "G" },
                                       new SqlParameter() { ParameterName = "@TAmount", SqlDbType = SqlDbType.Decimal, Value = txtCAmount.Text },
                                       new SqlParameter() { ParameterName = "@TNarration", SqlDbType = SqlDbType.NVarChar, Value = txtRemarks.Text },
                                       new SqlParameter() { ParameterName = "@BillNo", SqlDbType = SqlDbType.NVarChar, Value = strBillNo.ToString() },
                                       new SqlParameter() { ParameterName = "@Status", SqlDbType = SqlDbType.NVarChar, Value = "N" },
                                       new SqlParameter() { ParameterName = "@AccountType", SqlDbType = SqlDbType.NVarChar, Value = "G" },
                                       new SqlParameter() { ParameterName = "@BillingPeriod", SqlDbType = SqlDbType.NVarChar, Value = Session["CurrentBillingPeriod"].ToString() },
                                       new SqlParameter() { ParameterName = "@EntryBy", SqlDbType = SqlDbType.NVarChar, Value = Session["UserID"].ToString() },
                                       new SqlParameter() { ParameterName = "@AccountCode", SqlDbType = SqlDbType.NVarChar, Value = lblAccNo.Text.ToString() },
                    new SqlParameter() { ParameterName = "@CGST", SqlDbType = SqlDbType.Decimal, Value = lblCGST2.Text.ToString() },
                    new SqlParameter() { ParameterName = "@SGST", SqlDbType = SqlDbType.Decimal, Value = lblSGST2.Text.ToString() }
                                         );
            }
            else
            {

                DataSet dsRSN = sqlobj.ExecuteSP("SP_InsertGuestUNBILLEDTxnPosting",
                                      new SqlParameter() { ParameterName = "@RTRSN", SqlDbType = SqlDbType.Decimal, Value = cmbResident.SelectedValue.ToString() },
                                      new SqlParameter() { ParameterName = "@BGroup", SqlDbType = SqlDbType.NVarChar, Value = drpTxn.SelectedValue.ToString() },
                                      new SqlParameter() { ParameterName = "@BCategory", SqlDbType = SqlDbType.NVarChar, Value = "G" },
                                      new SqlParameter() { ParameterName = "@TAmount", SqlDbType = SqlDbType.Decimal, Value = txtCAmount.Text },
                                      new SqlParameter() { ParameterName = "@TNarration", SqlDbType = SqlDbType.NVarChar, Value = txtRemarks.Text },
                                      new SqlParameter() { ParameterName = "@BillNo", SqlDbType = SqlDbType.NVarChar, Value = strBillNo.ToString() },
                                      new SqlParameter() { ParameterName = "@Status", SqlDbType = SqlDbType.NVarChar, Value = "N" },
                                      new SqlParameter() { ParameterName = "@AccountType", SqlDbType = SqlDbType.NVarChar, Value = "G" },
                                      new SqlParameter() { ParameterName = "@BillingPeriod", SqlDbType = SqlDbType.NVarChar, Value = Session["CurrentBillingPeriod"].ToString() },
                                      new SqlParameter() { ParameterName = "@EntryBy", SqlDbType = SqlDbType.NVarChar, Value = Session["UserID"].ToString() },
                                      new SqlParameter() { ParameterName = "@AccountCode", SqlDbType = SqlDbType.NVarChar, Value = lblAccNo.Text.ToString() },
                                    new SqlParameter() { ParameterName = "@CGST", SqlDbType = SqlDbType.Decimal, Value = lblCGST2.Text.ToString() },
                                    new SqlParameter() { ParameterName = "@SGST", SqlDbType = SqlDbType.Decimal, Value = lblSGST2.Text.ToString() }
                                        );
            }
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('Transaction amount posted successfully');", true);
            btnCClear_Click(sender, e);
            //}
        }
        catch (Exception ex)
        {
            WebMsgBox.Show(ex.ToString());
        }
    }

    protected void lnkPrintInvoice_Click(object sender, EventArgs e)
    {
        try
        {
            if (CnfResult.Value == "true")
            {
                string rsn = string.Empty;

                SqlDataAdapter da = new SqlDataAdapter();
                DataSet dsRSN = sqlobj.ExecuteSP("Sp_GetDetailsForInvoice");
                //da.Fill(dsRSN);
                if (dsRSN.Tables[0] != null && dsRSN.Tables[0].Rows.Count > 0)
                {
                    rsn = dsRSN.Tables[0].Rows[0]["RSN"].ToString();
                }
                if (dsRSN.Tables[1] != null && dsRSN.Tables[1].Rows.Count > 0)
                {
                    DI = Convert.ToDecimal(dsRSN.Tables[1].Rows[0]["DI"].ToString());
                }
                if (dsRSN.Tables[2] != null && dsRSN.Tables[2].Rows.Count > 0)
                {
                    MI = Convert.ToDecimal(dsRSN.Tables[2].Rows[0]["MI"].ToString());
                }
                if (dsRSN.Tables[3] != null && dsRSN.Tables[3].Rows.Count > 0)
                {
                    NC = Convert.ToDecimal(dsRSN.Tables[3].Rows[0]["NC"].ToString());
                }
                DataSet dsDetails = sqlobj.ExecuteSP("Proc_GetData_AccountCode");
                if (dsDetails.Tables[0].Rows.Count > 0)
                {
                    commnty = dsDetails.Tables[2].Rows[0]["CommunityName"].ToString();
                    Address1 = dsDetails.Tables[2].Rows[0]["Name"].ToString();
                    Address2 = dsDetails.Tables[2].Rows[0]["Add1"].ToString();
                    Address3 = dsDetails.Tables[2].Rows[0]["City"].ToString() + " , " + dsDetails.Tables[2].Rows[0]["pincode"].ToString();
                    gstin = dsDetails.Tables[2].Rows[0]["gstin_UIN"].ToString();
                    Pan = dsDetails.Tables[2].Rows[0]["pan_NO"].ToString();
                    Jurisdiction = dsDetails.Tables[2].Rows[0]["Jurisdiction"].ToString();
                    BankName = dsDetails.Tables[2].Rows[0]["BankName"].ToString();
                    BranchName = dsDetails.Tables[2].Rows[0]["BranchName"].ToString();
                    AccountNo = dsDetails.Tables[2].Rows[0]["AccountNo"].ToString();
                    IFSCCode = dsDetails.Tables[2].Rows[0]["IFSCCode"].ToString();
                    SendMail(dsDetails.Tables[0], dsDetails.Tables[1]);                    
                }
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + ex.Message.ToString() + "');", true);
        }
    }
    public void SendMail(DataTable dt, DataTable dtPersonal)
    {
        try
        {
            System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;
            HttpContext.Current.Response.Clear();
            string mailserver = string.Empty;
            string user = string.Empty;
            string pwd = string.Empty;
            string sentby = string.Empty;

            SqlCommand cmd = new SqlCommand("Proc_GetEmailCredentials", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet dsCredential = new DataSet();
            da.Fill(dsCredential);
            // Write an informational entry to the event log.
            if (dsCredential != null && dsCredential.Tables.Count > 0)
            {
                foreach (DataRow row in dsCredential.Tables[0].Rows)
                {
                    mailserver = row["mailserver"].ToString();
                    user = row["username"].ToString();
                    pwd = row["password"].ToString();
                    sentby = row["sentbyuser"].ToString();
                    Email = row["Email"].ToString();
                }
            }
            SmtpClient mySmtpClient = new SmtpClient(mailserver, 587);
            MailAddress From = new MailAddress(user, "info@innovatussystems.com");
            MailMessage myMail = new System.Net.Mail.MailMessage();
            myMail.From = From;
            myMail.To.Add(Email);
            myMail.CC.Add("covaiproprarch@gmail.com");
            mySmtpClient.UseDefaultCredentials = false;
            System.Net.NetworkCredential basicauth = new System.Net.NetworkCredential(user, pwd);
            mySmtpClient.Credentials = basicauth;
            mySmtpClient.EnableSsl = false;
            mySmtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            //mySmtpClient.Timeout = 200000;
            myMail.SubjectEncoding = System.Text.Encoding.UTF8;
            myMail.BodyEncoding = System.Text.Encoding.UTF8;

            DataSet dsDetails = new DataSet();
            SqlCommand cmd1 = new SqlCommand("Proc_GetGST", con);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@where", whr);
            SqlDataAdapter dap = new SqlDataAdapter(cmd1);
            dap.Fill(dsDetails, "temp");
            Bmonth = dsDetails.Tables[0].Rows[0]["BPName"].ToString();
            BFROM = dsDetails.Tables[0].Rows[0]["BPFROM"].ToString();
            BTILL = dsDetails.Tables[0].Rows[0]["BPTILL"].ToString();
            DataTable dtNew = new DataTable();
            for (int i = 0; i < dtPersonal.Rows.Count; i++)
            {
                string outstd = "";
                int count = 0;
                outstd = "";
                Amount = 0;
                CGSTAmt = 0;
                SGSTAmt = 0;
                Total = 0;
                count = 0;
                RTRSN = dtPersonal.Rows[i]["RTRSN"].ToString();
                Villa = dtPersonal.Rows[i]["Villa"].ToString();
                Resident = dtPersonal.Rows[i]["Name"].ToString();
                Adress = dtPersonal.Rows[i]["Address1"].ToString();
                Cty = dtPersonal.Rows[i]["Address3"].ToString();
                Accountcode = dtPersonal.Rows[i]["Accountcode"].ToString();
                outstd = dtPersonal.Rows[i]["outstd"].ToString();
                strQuery = "Accountcode = '" + Accountcode + "'";
                Odate = DateTime.Now;
                printedOn = Odate.ToString("dd-MMM-yyyy hh:mm tt");
                date = Odate.ToString("dd-MMM-yyyy");
                whr = strQuery;
                DataRow[] drfilService = dt.Select(whr);
                if (drfilService.Any())
                {
                    dtservice = drfilService.CopyToDataTable();
                    using (StringWriter sw = new StringWriter())
                    {
                        using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                        {
                            invNo = dtservice.Rows[0]["InvoiceNo"].ToString();
                            //string NARATION = dtservice.Rows[0]["Particulars"].ToString();
                            string NARATION = "MEB";
                            // dtservice.DefaultView.Sort = "Date ASC";                            
                            StringBuilder sb1 = new StringBuilder();
                            sb1.Append("<table width='100%' padding='-5PX'  style='font-family:Verdana;margin-left:15px;'>");
                            sb1.Append("<tr><td align='center' colspan='4' style='font-size:10px;'><b> " + commnty + " </b> </td></tr>");
                            sb1.Append("<tr><td align='center' colspan='4' style='font-size:9px;'> " + Address1 + " </td></tr>");
                            sb1.Append("<tr><td align='center' colspan='4' style='font-size:9px;'> " + Address2 + "  </td></tr>");
                            sb1.Append("<tr><td align='center' colspan='4' style='font-size:9px;'> " + Address3 + "  </td></tr>");
                            sb1.Append("<tr><td align='center' colspan='4' style='font-size:9px;'> GSTIN/UIN: " + gstin + " / Pan: " + Pan + "  </td></tr>");
                            sb1.Append("</table>");
                            sb1.Append("<table  width='100%' style='margin-left:15px;opacity:0.5;border:0.1px solid black'>");
                            sb1.Append("<tr >");
                            sb1.Append("<td align='Left' style='font-size:10px;'>");
                            sb1.Append("Invoice No.:" + invNo + "");
                            sb1.Append("</td>");
                            sb1.Append("<td align='RIGHT' style='font-size:10px;'>");
                            sb1.Append("Date: " + date + "");
                            sb1.Append("</td>");
                            sb1.Append("</tr>");
                            sb1.Append("</table>");

                            sb1.Append("<table  width='100%' BORDERCOLOR='#c6ccce' style='margin-left:15px;'>");
                            sb1.Append("<tr >");
                            sb1.Append("<td align='Left' style='font-size:8px;'>");
                            sb1.Append("<b>" + Villa + "</b> - " + Resident + "");
                            sb1.Append("</td>");
                            sb1.Append("</tr>");
                            sb1.Append("<tr >");
                            sb1.Append("<td align='Left' style='font-size:8px;'>");
                            sb1.Append("" + Adress + "");
                            sb1.Append("</td>");
                            sb1.Append("</tr>");
                            sb1.Append("<tr >");
                            sb1.Append("<td align='Left' style='font-size:8px;'>");
                            sb1.Append("" + Cty + " ");
                            sb1.Append("</td>");
                            sb1.Append("</tr>");
                            sb1.Append("</table>");
                            sb1.Append("<table border = '1' width='100%' BORDERCOLOR='#c6ccce' style='margin-left:15px;'>");
                            sb1.Append("<tr >");

                            foreach (DataColumn column in dtservice.Columns)
                            {

                                if (column.ColumnName != "RTRSN")
                                {
                                    if (column.ColumnName != "ACCOUNTCODE")
                                    {
                                        if (column.ColumnName == "Particulars")
                                        {
                                            sb1.Append("<td style ='font-size:11px;font-family:Verdana;' colspan='1'  align='center'>");
                                        }

                                        else if (column.ColumnName == "HSN/SAC")
                                        {
                                            sb1.Append("<td style ='font-size:11px;font-family:Verdana;' align='center'>");
                                        }
                                        else if (column.ColumnName == "GST Rate%")
                                        {
                                            sb1.Append("<td style ='font-size:11px;font-family:Verdana;'align='center'>");
                                        }
                                        else if (column.ColumnName == "TaxableVlu")
                                        {
                                            sb1.Append("<td style ='font-size:11px;font-family:Verdana;width:8%;' align='center'>");
                                        }
                                        else if (column.ColumnName == "CGST Amt")
                                        {
                                            sb1.Append("<td style ='font-size:11px;font-family:Verdana;'align='center'>");
                                        }
                                        else if (column.ColumnName == "SGST Amt")
                                        {
                                            sb1.Append("<td style ='font-size:11px;font-family:Verdana;'align='center'>");
                                        }

                                        else if (column.ColumnName == "Total")
                                        {
                                            sb1.Append("<td style ='font-size:11px;font-family:Verdana;width:8%;' align='center'>");
                                        }
                                        //else
                                        //    {
                                        //        sb1.Append("<th style ='font-size:11px;font-family:Verdana;' align='left'>");
                                        //    }
                                        if (column.ColumnName == "TaxableVlu")
                                        {
                                            sb1.Append("<b>Taxable Value</b>");
                                            sb1.Append("</td>");
                                        }
                                        else if (column.ColumnName == "CGST Amt")
                                        {
                                            sb1.Append("<b>CGST Amount</b>");
                                            sb1.Append("</td>");
                                        }
                                        else if (column.ColumnName == "SGST Amt")
                                        {
                                            sb1.Append("<b>SGST Amount</b>");
                                            sb1.Append("</td>");
                                        }
                                        else
                                        {
                                            sb1.Append("<b>" + column.ColumnName + "</b>");
                                            sb1.Append("</td>");
                                        }
                                    }
                                }
                            }
                            sb1.Append("</tr>");
                            string GST = "";
                            decimal amount = 0;
                            foreach (DataRow row in dtservice.Rows)
                            {
                                count = count + 1;
                                sb1.Append("<tr>");
                                foreach (DataColumn column in dtservice.Columns)
                                {
                                    if (column.ColumnName != "RTRSN")
                                    {
                                        if (column.ColumnName != "ACCOUNTCODE")
                                        {
                                            if (column.ToString() == "Particulars")
                                            {
                                                sb1.Append("<td style = 'font-size:8px;font-family:Verdana;'  align='Left'>");
                                            }
                                            else if (column.ToString() == "HSN/SAC")
                                            {
                                                sb1.Append("<td style = 'font-size:8px;font-family:Verdana;' align='right'>");
                                            }
                                            else if (column.ToString() == "GST Rate%")
                                            {
                                                GST = row[column].ToString();
                                                sb1.Append("<td style = 'font-size:8px;font-family:Verdana;' align='right'>");
                                            }
                                            else if (column.ToString() == "TaxableVlu")
                                            {
                                                Amount = Amount + Convert.ToDecimal(row[column]);
                                                sb1.Append("<td style = 'font-size:8px;font-family:Verdana;' align='right'>");
                                            }
                                            else if (column.ToString() == "CGST Amt")
                                            {
                                                CGSTAmt = CGSTAmt + Convert.ToDecimal(row[column]);
                                                sb1.Append("<td style = 'font-size:8px;font-family:Verdana;' align='right'>");
                                            }
                                            else if (column.ToString() == "SGST Amt")
                                            {
                                                SGSTAmt = SGSTAmt + Convert.ToDecimal(row[column]);
                                                sb1.Append("<td style = 'font-size:8px;font-family:Verdana;' align='right'>");
                                            }

                                            else if (column.ToString() == "Total")
                                            {
                                                Total = Total + Convert.ToDecimal(row[column]);
                                                sb1.Append("<td style = 'font-size:8px;font-family:Verdana;' align='right'>");
                                            }
                                            if (column.ToString() != "#")
                                            {
                                                sb1.Append(row[column]);
                                            }
                                            sb1.Append("</td>");
                                        }
                                    }
                                }
                                sb1.Append("</tr>");
                            }
                            decimal CGST = Convert.ToDecimal(GST) / 2;
                            count = count + 1;
                            sb1.Append("<tr>");
                            sb1.Append("<td  align='left' colspan='3' style='font-size:8px;'>Total");
                            sb1.Append("</td>");
                            sb1.Append("<td  align='Right' style='font-size:8px;'>" + Amount + "");
                            sb1.Append("</td>");
                            sb1.Append("<td  align='Right' style='font-size:8px;'>" + CGSTAmt + "");
                            sb1.Append("</td>");
                            sb1.Append("<td  align='Right' style='font-size:8px;'>" + SGSTAmt + "");
                            sb1.Append("</td>");
                            sb1.Append("<td  align='Right' style='font-size:8px;'>" + Total + "");
                            sb1.Append("</td>");
                            sb1.Append("</tr>");
                            sb1.Append("<tr>");
                            sb1.Append("<td align='left' style='font-size:8px;' colspan='6'>Amount chargeable (in words)");
                            sb1.Append("</td>");
                            sb1.Append("<td style='font-size:8px;'>E&O.E.");
                            sb1.Append("</td>");
                            sb1.Append("</tr>");
                            sb1.Append("<tr>");
                            sb1.Append("<td align='left' style='font-size:8px;' colspan='7'>Rupees " + ConvertToWords(Convert.ToString(Total)) + "");
                            Amount = amount;
                            sb1.Append("</td>");
                            sb1.Append("</tr>");
                            sb1.Append("</table>");
                            sb1.Append("<table  width='100%' BORDERCOLOR='#c6ccce' style='margin-left:15px;'>");
                            sb1.Append("<tr>");
                            sb1.Append("<td align='left' colspan='6' style='font-size:8px;'>Invoice for the period of " + BFROM + " to " + BTILL + " ");
                            sb1.Append("</td>");
                            sb1.Append("<td>");
                            sb1.Append("</td>");
                            sb1.Append("</tr>");
                            sb1.Append("</table>");
                            sb1.Append("<table border = '1' width='50%' BORDERCOLOR='#c6ccce' style='margin-left:15px;'>");
                            sb1.Append("<tr>");
                            sb1.Append("<td align='left' colspan='2' style='font-size:8px;'><b>Company’s bank details</b>");
                            sb1.Append("</td>");
                            sb1.Append("</tr>");
                            sb1.Append("<tr>");
                            sb1.Append("<td align='left' colspan='1' style='font-size:8px;'>Bank name");
                            sb1.Append("</td>");
                            sb1.Append("<td style='font-size:8px;'>" + BankName + "");
                            sb1.Append("</td>");
                            sb1.Append("</tr>");
                            sb1.Append("<tr>");
                            sb1.Append("<td align='left' colspan='1' style='font-size:8px;'>Branch");
                            sb1.Append("</td>");
                            sb1.Append("<td style='font-size:8px;'>" + BranchName + "");
                            sb1.Append("</td>");
                            sb1.Append("</tr>");
                            sb1.Append("<tr>");
                            sb1.Append("<td align='left' colspan='1' style='font-size:8px;'>Account no");
                            sb1.Append("</td>");
                            sb1.Append("<td style='font-size:8px;'>" + AccountNo + "");
                            sb1.Append("</td>");
                            sb1.Append("</tr>");
                            sb1.Append("<tr>");
                            sb1.Append("<td align='left' colspan='1' style='font-size:8px;'>IFSC Code");
                            sb1.Append("</td>");
                            sb1.Append("<td style='font-size:8px;'>" + IFSCCode + "");
                            sb1.Append("</td>");
                            sb1.Append("</tr>");
                            sb1.Append("</table><br/>");
                            sb1.Append("<table  width='100%' BORDERCOLOR='#c6ccce' style='margin-left:15px;'>");
                            sb1.Append("<tr>");
                            sb1.Append("<td align='right' colspan='4' style='font-size:8px;'>" + commnty + "");
                            sb1.Append("</td>");
                            sb1.Append("</tr>");
                            sb1.Append("<br/>");
                            sb1.Append("<tr>");
                            sb1.Append("<td align='right' colspan='4' style='font-size:8px;'>");
                            sb1.Append("</td>");
                            sb1.Append("</tr>");
                            sb1.Append("<br/>");
                            sb1.Append("<tr>");
                            sb1.Append("<td align='right' colspan='4' style='font-size:8px;'>");
                            sb1.Append("</td>");
                            sb1.Append("</tr>");
                            sb1.Append("</table><br/>");
                            sb1.Append("<table  width='100%' BORDERCOLOR='#c6ccce' style='margin-left:15px;'>");
                            sb1.Append("<tr>");
                            sb1.Append("<td align='Center' colspan='4'  style='font-size:10px;'>" + Jurisdiction + "");
                            sb1.Append("</td>");
                            sb1.Append("</tr>");
                            sb1.Append("<tr>");
                            sb1.Append("<td align='Center' colspan='4' style='font-size:8px;'>This is a computer generated invoice");
                            sb1.Append("</td>");
                            sb1.Append("<td>");
                            sb1.Append("</td>");
                            sb1.Append("</tr>");
                            sb1.Append("</table><br/>");
                            StringReader sr1 = new StringReader(sb1.ToString());
                            //sb1.Length = 0;
                            //sb1.Clear();
                            FileName1 = Villa + "_" + Resident + "_" + Bmonth + ".pdf";
                            Document pdfDoc1 = new Document(PageSize.A4, 60f, 30f, 10f, 5f);
                            HTMLWorker htmlparser1 = new HTMLWorker(pdfDoc1);
                            byte[] bytes1 = null;
                            using (MemoryStream memoryStream1 = new MemoryStream())
                            {
                                PdfWriter writer1 = PdfWriter.GetInstance(pdfDoc1, memoryStream1);
                                pdfDoc1.Open();
                                writer1.PageEvent = new Footer();
                                htmlparser1.Parse(sr1);
                                pdfDoc1.Close();
                                bytes1 = memoryStream1.ToArray();
                                memoryStream1.Close();
                                FileName1 = Bmonth + "_" + "_" + Villa + "_" + Resident + "_" + NARATION + ".pdf";
                                myMail.Attachments.Add(new Attachment(new MemoryStream(bytes1), FileName1));
                            }
                        }
                    }
                    UpdateTxn(RTRSN, Email, FileName1, Convert.ToString(Total));
                }

            }
            myMail.IsBodyHtml = true;
            StringBuilder sbBody = new StringBuilder();
            sbBody.Append("<table width='100%' cellspacing='5' cellpadding='5' style='font-family:Verdana;font-size:14px;'>");
            sbBody.Append("<tr><td><b> Greetings,</b></td></tr>");
            sbBody.Append("<tr><td>Month End Billing Invoice(s) for the month of  <b>" + Bmonth + "</b>.");
            sbBody.Append("</td></tr>");
            sbBody.Append("<tr><td>The Invoice(s) are attached.</b>");
            sbBody.Append("</td></tr>");

            sbBody.Append("<tr><td>With Best Regards,");
            sbBody.Append("</td></tr>");
            sbBody.Append("<tr><td><b>" + commnty + "</b></td></tr>");
            sbBody.Append("<tr><td>");
            sbBody.Append("</td></tr>");
            sbBody.Append("<tr><td> ");
            sbBody.Append("</td></tr>");
            sbBody.Append("<tr><td></td></tr>");
            sbBody.Append("<tr><td>The Invoice(s) in PDF Format and can be opened by Adobe ® Acrobat reader by clicking on the attachment.");
            sbBody.Append("</td></tr>");
            sbBody.Append("<tr><td></td></tr>");
            sbBody.Append("<tr style='font-family:Verdana;font-size:13px;'><td><b>This is an automatically generated mail from <a href='http://bincrm.com/oris/' title='Click here to open ORIS'>ORIS</a>, the software for Retirement Communities.</b></td></tr>");
            sbBody.Append("</table>");
            myMail.Subject = "Month End Billing Invoice(s) for the month of " + Bmonth;
            myMail.Body = sbBody.ToString();
            mySmtpClient.Send(myMail);
            dt.Dispose();
            dtPersonal.Dispose();
            //HttpContext.Current.ApplicationInstance.CompleteRequest();
            AdminMail();
        }
        catch (Exception ex)
        {
            WebMsgBox.Show(ex.Message);
        }

    }
    public partial class Footer : PdfPageEventHelper
    {
        //string msg = ViewState["Footer"].ToString();
        public override void OnEndPage(PdfWriter writer, Document doc)
        {
            float cellHeight = doc.TopMargin;
            iTextSharp.text.Rectangle page = doc.PageSize;
            //Paragraph footer = new Paragraph(commnty + " || " + "  Incharge : " + contName + "||" + " Instruction : " + payment, FontFactory.GetFont(FontFactory.TIMES, 12, iTextSharp.text.Font.NORMAL));
            Paragraph footer = new Paragraph(strFooter, FontFactory.GetFont(FontFactory.TIMES, 12, iTextSharp.text.Font.NORMAL));
            footer.Alignment = Element.ALIGN_CENTER;
            PdfPTable footerTbl = new PdfPTable(1);
            footerTbl.TotalWidth = 500;
            footerTbl.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPCell cell = new PdfPCell(footer);
            cell.Border = 0;
            cell.PaddingLeft = 10;
            footerTbl.AddCell(cell);
            //Paragraph header = new Paragraph(commnty, FontFactory.GetFont(FontFactory.TIMES, 14, iTextSharp.text.Font.BOLD));
            //header.Alignment = Element.ALIGN_CENTER;
            //PdfPTable headertbl = new PdfPTable(1);
            //headertbl.TotalWidth = 500;
            //headertbl.HorizontalAlignment = Element.ALIGN_CENTER;
            //PdfPCell hcell = new PdfPCell(header);
            //hcell.Border = 0;
            //hcell.PaddingLeft = 10;
            //headertbl.AddCell(hcell);
            ////footerTbl.WriteSelectedRows(0, -1, 415, 30, writer.DirectContent);
            //footerTbl.WriteSelectedRows(0, -1, 150, 50, writer.DirectContent);
            ////footerTbl.WriteSelectedRows(0, -1, 10f, writer.PageSize.GetBottom(doc.BottomMargin), writer.DirectContent);
            //headertbl.WriteSelectedRows(0, -1, 270, page.Height - cellHeight + headertbl.TotalHeight, writer.DirectContent);
        }
    }
    protected void UpdateTxn(string rtrsn, string mailID, string file, string Amt)
    {
        try
        {
            DataSet dsUpdate = sqlobj.ExecuteSP("CC_UPDATEINVAUDITLOG",
                   new SqlParameter() { ParameterName = "@invNo", SqlDbType = SqlDbType.NVarChar, Value = invNo },
                   new SqlParameter() { ParameterName = "@RTRSN", SqlDbType = SqlDbType.Decimal, Value = rtrsn },
                   new SqlParameter() { ParameterName = "@TXCODE", SqlDbType = SqlDbType.NVarChar, Value = "NO" },
                   new SqlParameter() { ParameterName = "@Amount", SqlDbType = SqlDbType.Decimal, Value = Amt },
                   new SqlParameter() { ParameterName = "@MailId", SqlDbType = SqlDbType.NVarChar, Value = mailID },
                   new SqlParameter() { ParameterName = "@File", SqlDbType = SqlDbType.NVarChar, Value = file },
                   new SqlParameter() { ParameterName = "@C_BY", SqlDbType = SqlDbType.NVarChar, Value = Session["UserID"].ToString() }
                );
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + ex.Message + "');", true);
        }
    }
    protected void AdminMail()
    {
        try
        {
            DataSet ds = sqlobj.ExecuteSP("SP_TotalAccountBilled",
              new SqlParameter() { ParameterName = "@IMode", SqlDbType = SqlDbType.Int, Value = 4 });
            ViewState["InvoiceEnd"] = Convert.ToDecimal(ds.Tables[0].Rows[0]["Invoicecode"].ToString());

            DataSet dsTtlAccount = sqlobj.ExecuteSP("SP_TotalAccountBilled",
                    new SqlParameter() { ParameterName = "@Imode", SqlDbType = SqlDbType.Int, Value = 2 });
            string BillingMonth = dsTtlAccount.Tables[0].Rows[0]["BillingMonth"].ToString();
            string NoOfInvoice = dsTtlAccount.Tables[0].Rows[0]["NoOfInvoice"].ToString();
            string AmountBilled = dsTtlAccount.Tables[0].Rows[0]["AmountBilled"].ToString();
            string NextMonth = dsTtlAccount.Tables[0].Rows[0]["NextMonth"].ToString();
            string Resident = dsTtlAccount.Tables[0].Rows[0]["Resident"].ToString();
            string Tenant = dsTtlAccount.Tables[0].Rows[0]["Tenant"].ToString();
            string Away = dsTtlAccount.Tables[0].Rows[0]["Away"].ToString();
            string NoOfResident = dsTtlAccount.Tables[0].Rows[0]["NoOfResident"].ToString();
            //DataTable dtservice = dsTtlAccount.Tables[1];


            System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;
            string mailserver = string.Empty;
            string user = string.Empty;
            string pwd = string.Empty;
            string sentby = string.Empty;
            string Email = string.Empty;

            SqlCommand cmd = new SqlCommand("Proc_GetEmailCredentials", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet dsCredential = new DataSet();
            da.Fill(dsCredential);
            // Write an informational entry to the event log.  

            if (dsCredential != null && dsCredential.Tables.Count > 0)
            {
                foreach (DataRow row in dsCredential.Tables[0].Rows)
                {
                    mailserver = row["mailserver"].ToString();
                    user = row["username"].ToString();
                    pwd = row["password"].ToString();
                    sentby = row["sentbyuser"].ToString();
                    Email = row["Email"].ToString();
                }
            }
            SmtpClient mySmtpClient = new SmtpClient(mailserver, 587);
            MailAddress From = new MailAddress(user, "info@innovatussystems.com");
            MailMessage myMail = new System.Net.Mail.MailMessage();
            myMail.From = From;
            myMail.To.Add(Email);
            mySmtpClient.UseDefaultCredentials = false;
            System.Net.NetworkCredential basicauth = new System.Net.NetworkCredential(user, pwd);
            mySmtpClient.Credentials = basicauth;
            mySmtpClient.EnableSsl = false;
            mySmtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            //mySmtpClient.Timeout = 200000;
            myMail.SubjectEncoding = System.Text.Encoding.UTF8;
            myMail.BodyEncoding = System.Text.Encoding.UTF8;
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    StringBuilder sbBody = new StringBuilder();
                    sbBody.Append("<table width='100%' cellspacing='5' cellpadding='5' style='font-family:Verdana;font-size:14px;'>");
                    sbBody.Append("<tr><td><b> Greetings,</b></td></tr>");
                    sbBody.Append("<tr><td>Month End Billing control Report for " + BillingMonth + ".</td></tr>");
                    sbBody.Append("<tr><td >1.Invoice No. Start: " + ViewState["InvoiceStart"].ToString() + ".</tr>");
                    sbBody.Append("<tr><td >2.Invoice No. End: " + ViewState["InvoiceEnd"].ToString() + ".</tr>");
                    sbBody.Append("<tr><td >3.No. Of Invoice:" + NoOfInvoice + ".</tr>");
                    sbBody.Append("<tr><td >4.Total Invoice Amount: " + AmountBilled + ".</tr>");
                    sbBody.Append("<tr><td >5.Resident-(OR): " + Resident + ".</tr>");
                    sbBody.Append("<tr><td >6.Tenant-(T): " + Tenant + ".</tr>");
                    sbBody.Append("<tr><td >7.Away-(OA): " + Away + ".</tr>");
                    sbBody.Append("<tr><td >8.Total-(OR,T,OA): " + NoOfResident + ".</tr>");
                    sbBody.Append("<tr><td>The Invoice(s) are archived as PDF files in '<b><u>covaiproprarch@gmail.com</u></b>'.</td></tr>");
                    sbBody.Append("<tr><td>Invoice File Name:- <b>DoorNo_Name_MMMYY.pdf</b>.</td></tr>");
                    sbBody.Append("<tr><td>New Billing Month:- " + NextMonth + ".</td></tr>");
                    sbBody.Append("</table>");
                    myMail.IsBodyHtml = true;
                    myMail.Subject = "Month end Billing Snapshot For " + BillingMonth + ".";
                    myMail.Body = sbBody.ToString();
                    mySmtpClient.Send(myMail);
                }
            }
           
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + ex.Message.ToString() + "');", true);
        }
    }
    private static string ConvertToWords(string numb)
    {
        string val = "", wholeNo = numb, points = "", andStr = "", pointStr = "";
        string endStr = "Only";
        try
        {
            int decimalPlace = numb.IndexOf(".");
            if (decimalPlace > 0)
            {
                wholeNo = numb.Substring(0, decimalPlace);
                points = numb.Substring(decimalPlace + 1);
                if (Convert.ToInt32(points) > 0)
                {
                    andStr = "and";// just to separate whole numbers from points/cents  
                    endStr = "Paisa " + endStr;//Cents  
                    pointStr = ConvertDecimals(points);
                }
            }
            val = string.Format("{0} {1}{2} {3}", ConvertWholeNumber(wholeNo).Trim(), andStr, pointStr, endStr);
        }
        catch { }
        return val;
    }
    private static string ConvertDecimals(string number)
    {
        string cd = "", digit = "", engOne = "", num = "";
        if (number.Equals("0"))
        {
            //engOne = "Zero";
        }
        else
        {
            if (number.Length > 1)
            {
                if (Convert.ToInt32(number) > 9 && Convert.ToInt32(number) < 20)
                {
                    engOne = tens(number);
                    cd += " " + engOne;
                }
                else
                {
                    for (int J = 0; J < number.Length; J++)
                    {
                        digit = number[J].ToString();
                        if (J == 0)
                        {
                            engOne = tens(digit);
                        }
                        else
                        {
                            engOne = ones(digit);
                        }
                        num += " " + engOne;
                    }
                    cd += " " + num;
                    //engOne = ones(digit);
                }
            }
            //cd += " " + engOne;
        }

        return cd;
    }
    private static string ConvertWholeNumber(string Number)
    {
        string word = "";
        try
        {
            bool beginsZero = false;//tests for 0XX  
            bool isDone = false;//test if already translated  
            double dblAmt = (Convert.ToDouble(Number));
            //if ((dblAmt > 0) && number.StartsWith("0"))  
            if (dblAmt > 0)
            {//test for zero or digit zero in a nuemric  
                beginsZero = Number.StartsWith("0");

                int numDigits = Number.Length;
                int pos = 0;//store digit grouping  
                string place = "";//digit grouping name:hundres,thousand,etc...  
                switch (numDigits)
                {
                    case 1://ones' range  

                        word = ones(Number);
                        isDone = true;
                        break;
                    case 2://tens' range  
                        word = tens(Number);
                        isDone = true;
                        break;
                    case 3://hundreds' range  
                        pos = (numDigits % 3) + 1;
                        place = " Hundred ";
                        break;
                    case 4://thousands' range  
                    case 5:
                    case 6:
                        pos = (numDigits % 4) + 1;
                        place = " Thousand ";
                        break;
                    case 7://millions' range  
                    case 8:
                    case 9:
                        pos = (numDigits % 7) + 1;
                        place = " Million ";
                        break;
                    case 10://Billions's range  
                    case 11:
                    case 12:

                        pos = (numDigits % 10) + 1;
                        place = " Billion ";
                        break;
                    //add extra case options for anything above Billion...  
                    default:
                        isDone = true;
                        break;
                }
                if (!isDone)
                {//if transalation is not done, continue...(Recursion comes in now!!)  
                    if (Number.Substring(0, pos) != "0" && Number.Substring(pos) != "0")
                    {
                        try
                        {
                            word = ConvertWholeNumber(Number.Substring(0, pos)) + place + ConvertWholeNumber(Number.Substring(pos));
                        }
                        catch { }
                    }
                    else
                    {
                        word = ConvertWholeNumber(Number.Substring(0, pos)) + ConvertWholeNumber(Number.Substring(pos));
                    }

                    //check for trailing zeros  
                    //if (beginsZero) word = " and " + word.Trim();  
                }
                //ignore digit grouping names  
                if (word.Trim().Equals(place.Trim())) word = "";
            }
        }
        catch { }
        return word.Trim();
    }
    private static string ones(string Number)
    {
        int _Number = Convert.ToInt32(Number);
        string name = "";
        switch (_Number)
        {

            case 1:
                name = "One";
                break;
            case 2:
                name = "Two";
                break;
            case 3:
                name = "Three";
                break;
            case 4:
                name = "Four";
                break;
            case 5:
                name = "Five";
                break;
            case 6:
                name = "Six";
                break;
            case 7:
                name = "Seven";
                break;
            case 8:
                name = "Eight";
                break;
            case 9:
                name = "Nine";
                break;
        }
        return name;
    }
    private static string tens(string Number)
    {
        int _Number = Convert.ToInt32(Number);
        string name = null;
        switch (_Number)
        {
            case 0:
                name = "";
                break;
            case 2:
                name = "Twenty";
                break;
            case 3:
                name = "Thirty";
                break;
            case 4:
                name = "Fourty";
                break;
            case 5:
                name = "Fifty";
                break;
            case 6:
                name = "Sixty";
                break;
            case 7:
                name = "Seventy";
                break;
            case 8:
                name = "Eighty";
                break;
            case 9:
                name = "Ninety";
                break;
            case 10:
                name = "Ten";
                break;
            case 11:
                name = "Eleven";
                break;
            case 12:
                name = "Twelve";
                break;
            case 13:
                name = "Thirteen";
                break;
            case 14:
                name = "Fourteen";
                break;
            case 15:
                name = "Fifteen";
                break;
            case 16:
                name = "Sixteen";
                break;
            case 17:
                name = "Seventeen";
                break;
            case 18:
                name = "Eighteen";
                break;
            case 19:
                name = "Nineteen";
                break;
            case 20:
                name = "Twenty";
                break;
            case 30:
                name = "Thirty";
                break;
            case 40:
                name = "Fourty";
                break;
            case 50:
                name = "Fifty";
                break;
            case 60:
                name = "Sixty";
                break;
            case 70:
                name = "Seventy";
                break;
            case 80:
                name = "Eighty";
                break;
            case 90:
                name = "Ninety";
                break;
            default:
                if (_Number > 0)
                {
                    name = tens(Number.Substring(0, 1) + "0") + " " + ones(Number.Substring(1));
                }
                break;
        }
        return name;
    }
}