using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Data.SqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Configuration;
using System.Text;
using Telerik.Web.UI;
using System.Threading;
public partial class InvoiceAuditLog : System.Web.UI.Page
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
    string Ref;
    static string strBillNo;
    static string invNo;
    static decimal Amount;
    static decimal Total;
    static decimal CGSTAmt;
    static decimal SGSTAmt;
    static string MailId;
    static string File;
    StringBuilder UpS = new StringBuilder();
    StringBuilder SB1 = new StringBuilder();
    StringBuilder SB = new StringBuilder();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            if (!IsPostBack)
            {
                LoadTitle();
                LoadAccountMaster();
                rdInvoice.DataSource = string.Empty;
                rdInvoice.DataBind();

            }
        }
        catch (Exception ex)
        {
            WebMsgBox.Show(ex.Message);
        }
    }
    private void LoadTitle()
    {
        try
        {
            DataSet dsTitle = sqlobj.ExecuteSP("SP_GetTitleMenus", new SqlParameter() { ParameterName = "@MenuId", SqlDbType = SqlDbType.Int, Value = 160 });


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
    private void LoadAccountMaster()
    {
        try
        {
            DataSet dsLOG = sqlobj.ExecuteSP("SP_AdHocInvoice",
                new SqlParameter() { ParameterName = "@IMode", SqlDbType = SqlDbType.Int, Value = 7 });

            if (dsLOG.Tables[0].Rows.Count > 0)
            {
                gvLog.DataSource = dsLOG;
                gvLog.DataBind();
            }
            else
            {
                gvLog.DataSource = string.Empty;
                gvLog.DataBind();
            }
            dsLOG.Dispose();
        }
        catch (Exception ex)
        {
            WebMsgBox.Show(ex.Message);
        }
    }
    protected void gvLog_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }

    protected void gvLog_Init(object sender, EventArgs e)
    {
        GridFilterMenu menu = gvLog.FilterMenu;
        int i = 0;
        while (i < menu.Items.Count)
        {
            if (menu.Items[i].Text == "NoFilter" || menu.Items[i].Text == "Contains")
            {
                i++;
            }
            else
            {
                menu.Items.RemoveAt(i);
            }
        }
    }
    protected void btnReturn_Click(object sender, EventArgs e)
    {
        Response.Redirect("AdHocInvoice.aspx", false);
    }

    protected void BtnExcelExport_Click1(object sender, EventArgs e)
    {
        try
        {
            SqlProcsNew sqlobj = new SqlProcsNew();
            DataSet dsStatementRPT = new DataSet();
            DataSet dsCategory = sqlobj.ExecuteSP("SP_AdHocInvoice",
                  new SqlParameter() { ParameterName = "@IMode", SqlDbType = SqlDbType.Int, Value = 7 });
            if (dsCategory.Tables[0].Rows.Count > 0)
            {
                DataGrid dg = new DataGrid();
                dg.DataSource = dsCategory.Tables[0];
                dg.DataBind();
                string DT = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss");
                string sFileName = "INVOICEAUDITLOG_" + DT + ".xls";
                // SEND OUTPUT TO THE CLIENT MACHINE USING "RESPONSE OBJECT".
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + sFileName);
                Response.ContentType = "application/vnd.ms-excel";
                EnableViewState = false;

                System.IO.StringWriter objSW = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter objHTW = new System.Web.UI.HtmlTextWriter(objSW);

                dg.HeaderStyle.Font.Bold = true;
                dg.HeaderStyle.BackColor = System.Drawing.Color.GreenYellow; // SET EXCEL HEADERS AS BOLD.
                dg.RenderControl(objHTW);


                Response.Write("<table><tr><td>INVOICE AUDIT LOG</td></tr></table>");



                Response.Write("<style> TABLE { border:dotted 1px #999; } " +
                    "TD { border:dotted 1px #D5D5D5; text-align:center } </style>");
                Response.Write(objSW.ToString());


                Response.End();
                dg = null;


            }
            else
            {
                //WebMsgBox.Show(" From" + dtpfordate.SelectedDate.Value + " To " + dtpuntildate.SelectedDate.Value + " statement does not exist");
            }
        }
        catch (Exception ex)
        {
            WebMsgBox.Show(ex.Message);
        }
    }

    protected void rdInvoice_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void rdInvoice_Init(object sender, EventArgs e)
    {

    }

    protected void lnkInvoiceNo_Click(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        GridDataItem grditm = (GridDataItem)lnk.NamingContainer;
        ViewState["InvNo"] = lnk.Text;
        DataSet dsCategory = sqlobj.ExecuteSP("SP_AdHocInvoice",
                 new SqlParameter() { ParameterName = "@iMode", SqlDbType = SqlDbType.Int, Value = 9 },
                 new SqlParameter() { ParameterName = "@invNo", SqlDbType = SqlDbType.NVarChar, Value = ViewState["InvNo"].ToString() }
                 );
        if (dsCategory.Tables[0].Rows.Count > 0)
        {
            btnGenerate.Visible = true;
            lblName.Text = dsCategory.Tables[0].Rows[0]["Name"].ToString() + " , Printed On " + dsCategory.Tables[0].Rows[0]["C_On"].ToString();
            rdInvoice.DataSource = dsCategory.Tables[0];
            rdInvoice.DataBind();
        }
        else
        {
            btnGenerate.Visible = false;
            lblName.Text = "";
            rdInvoice.DataSource = string.Empty;
            rdInvoice.DataBind();
        }

        //string RSN = grditm.Cells[3].Text.ToString();
    }

    protected void lnkPrint_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsDetails = sqlobj.ExecuteSP("SP_Getdata_ForAdhocInv",
                new SqlParameter() { ParameterName = "@REF", SqlDbType = SqlDbType.NVarChar, Value = ViewState["InvNo"].ToString() }
                );
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
                ReprintInvoice(dsDetails.Tables[0], dsDetails.Tables[1], dsDetails.Tables[3]);                
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            else
            {

            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void ReprintInvoice(DataTable dt, DataTable dtPersonal, DataTable dtGST)
    {
        try
        {

            string outstd = "";
            Amount = 0;
            Total = 0;
            CGSTAmt = 0;
            SGSTAmt = 0;
            DataTable dtNew = new DataTable();

            for (int i = 0; i < dtPersonal.Rows.Count; i++)
            {
                int count = 0;
                count = 0;
                System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;
                string mailserver = string.Empty;
                string user = string.Empty;
                string pwd = string.Empty;
                string sentby = string.Empty;
                string Email = string.Empty;
                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                    {
                        string Villa = dtPersonal.Rows[i]["Villa"].ToString();
                        string Resident = dtPersonal.Rows[i]["Name"].ToString();
                        string Adress = dtPersonal.Rows[i]["Address1"].ToString();
                        string Cty = dtPersonal.Rows[i]["Address3"].ToString();
                        string Accountcode = dtPersonal.Rows[i]["Accountcode"].ToString();
                        outstd = dtPersonal.Rows[i]["outstd"].ToString();
                        string strQuery = "Accountcode = '" + Accountcode + "'";

                        DateTime Odate = DateTime.Now;
                        string printedOn = Odate.ToString("dd-MMM-yyyy hh:mm tt");
                        string date = Odate.ToString("dd-MMM-yyyy");
                        DataSet dsDetails = new DataSet();
                        SqlCommand cmd1 = new SqlCommand("Proc_GetGST", con);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter dap = new SqlDataAdapter(cmd1);
                        dap.Fill(dsDetails);
                        string Bmonth = dsDetails.Tables[0].Rows[0]["BPName"].ToString();
                        string BFROM = dsDetails.Tables[0].Rows[0]["BPFROM"].ToString();
                        string BTILL = dsDetails.Tables[0].Rows[0]["BPTILL"].ToString();
                        int drows = dsDetails.Tables[0].Rows.Count;
                        DataTable dtservice = dt;
                        invNo = dtservice.Rows[0]["InvoiceNo"].ToString();
                        string NARATION = dtservice.Rows[0]["Particulars"].ToString();
                        StringBuilder sb1 = new StringBuilder();
                
                        sb1.Append("<table width='100%' padding='-5PX'  style='font-family:Verdana;'>");
                        sb1.Append("<tr><td align='Right' colspan='4' style='font-size:8px;'>Copy</td></tr>");
                        sb1.Append("<tr><td align='center' colspan='4' style='font-size:10px;'><b> " + commnty + " </b> </td></tr>");
                        sb1.Append("<tr><td align='center' colspan='4' style='font-size:9px;'> " + Address1 + " </td></tr>");
                        sb1.Append("<tr><td align='center' colspan='4' style='font-size:9px;'> " + Address2 + "  </td></tr>");
                        sb1.Append("<tr><td align='center' colspan='4' style='font-size:9px;'> " + Address3 + "  </td></tr>");
                        sb1.Append("<tr><td align='center' colspan='4' style='font-size:9px;'> GSTIN/UIN: " + gstin + " / Pan: " + Pan + "  </td></tr>");
                        sb1.Append("</table>");
                        sb1.Append("<table  width='100%' style='margin-left:15px;opacity:0.5;border:0.1px solid black'>");
                        sb1.Append("<tr >");
                        sb1.Append("<td align='Left' style='font-size:10px;'>");
                        sb1.Append("Invoice No.:" + invNo +"");
                        sb1.Append("</td>");
                        sb1.Append("<td align='RIGHT' style='font-size:10px;'>");
                        sb1.Append("Date: " + date + "");
                        sb1.Append("</td>");
                        sb1.Append("</tr>");
                        sb1.Append("</table>");



                        sb1.Append("<table  width='100%' BORDERCOLOR='#c6ccce'>");
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
                            if (column.ColumnName != "#")
                            {
                                if (column.ColumnName != "RTRSN")
                                {
                                    if (column.ColumnName != "ACCOUNTCODE")
                                    {
                                        if (column.ColumnName != "TXNCODE")
                                        {
                                            if (column.ColumnName != "REF")
                                            {
                                                //if (column.ColumnName == "#")
                                                //{
                                                //    sb1.Append("<th style ='font-size:11px;font-family:Verdana;' align='center'>");
                                                //}
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
                                                else {
                                                    sb1.Append("<b>" + column.ColumnName + "</b>");
                                                    sb1.Append("</td>");
                                                }
                                               
                                            }
                                        }
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
                                if (column.ColumnName != "#")
                                {
                                    if (column.ColumnName != "RTRSN")
                                    {
                                        if (column.ColumnName != "ACCOUNTCODE")
                                        {
                                            if (column.ColumnName != "TXNCODE")
                                            {
                                                if (column.ColumnName != "REF")
                                                {
                                                    //if (column.ToString() == "#")
                                                    //{
                                                    //    sb1.Append("<td style = 'font-size:8px;font-family:Verdana;'  align='center'>" + count + "");
                                                    //}
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
                                                    //else
                                                    //{
                                                    //    sb1.Append("<td style = 'font-size:8px;font-family:Verdana;' align='left'>");
                                                    //}
                                                    if (column.ToString() != "#")
                                                    {
                                                        sb1.Append(row[column]);
                                                    }
                                                    sb1.Append("</td>");
                                                }
                                            }
                                        }
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
                        sb1.Append("<table  width='100%' BORDERCOLOR='#c6ccce'>");
                        sb1.Append("<tr>");
                        sb1.Append("<td align='left' colspan='6' style='font-size:8px;'>Invoice Printed on " + DateTime.Now.ToString("dd-MMM-yyyy HH:mm") + " Hrs.");
                        sb1.Append("</td>");
                        sb1.Append("<td>");
                        sb1.Append("</td>");
                        sb1.Append("</tr>");
                        sb1.Append("</table>");
                       
                        sb1.Append("<table border = '1' width='50%' BORDERCOLOR='#c6ccce'");
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

                        
                        sb1.Append("<table  width='100%' BORDERCOLOR='#c6ccce' >");
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
                        StringReader sr2 = new StringReader(sb1.ToString());
                        string FileName1 ="Copy - "+ invNo + "_" + Villa + "_" + Resident + "_" + NARATION + ".pdf";
                        //Document pdfDoc1 = new Document(PageSize.A4, 60f, 30f, 10f, 5f);
                        Document pdfDoc2 = new Document(PageSize.A4, 60f, 30f, 10f, 5f);
                        HttpContext.Current.Response.Clear();
                        HttpContext.Current.Response.ContentType = "application/pdf";
                        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + FileName1 + "");
                        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        //HTMLWorker htmlparser1 = new HTMLWorker(pdfDoc1);
                        HTMLWorker htmlparser2 = new HTMLWorker(pdfDoc2);
                        using (MemoryStream memoryStream1 = new MemoryStream())
                        {
                            string replace = FileName1.Replace(@"/", "_");
                            //PdfWriter writer2 = PdfWriter.GetInstance(pdfDoc2, new FileStream(@"F:\Dhinesh\Files" + @"\" + replace, FileMode.Create));
                            PdfWriter writer2 = PdfWriter.GetInstance(pdfDoc2, Response.OutputStream);
                            //PdfWriter writer1 = PdfWriter.GetInstance(pdfDoc1, memoryStream1);                                
                            //pdfDoc1.Open();
                            //writer1.PageEvent = new Footer();                               
                            //htmlparser1.Parse(sr1);
                            //pdfDoc1.Close();

                            pdfDoc2.Open();
                            //writer2.PageEvent = new Footer();
                            htmlparser2.Parse(sr2);
                            pdfDoc2.Close();

                            HttpContext.Current.Response.Write(pdfDoc2);
                            HttpContext.Current.Response.Flush();
                            byte[] bytes1 = memoryStream1.ToArray();
                            memoryStream1.Close();
                            File = FileName1;
                            //myMail.Attachments.Add(new Attachment(new MemoryStream(bytes1), FileName1));                             
                        }
                        UpdateTxn(FileName1);
                    }
                    //End
                    //mySmtpClient.Send(myMail);                      
                    //}
                }
                dt.Dispose();
                dtPersonal.Dispose();

            }
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Invoice(s) generated successfully');", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('Something went wrong." + ex.Message + "'');", true);
        }
    }
    protected void UpdateTxn(string FileName)
    {
        try
        {
            DataSet dsUpdate = sqlobj.ExecuteSP("SP_AdHocInvoice",
                 new SqlParameter() { ParameterName = "@iMode", SqlDbType = SqlDbType.Int, Value = 10 },                 
                  new SqlParameter() { ParameterName = "@invNo", SqlDbType = SqlDbType.NVarChar, Value = invNo },
                   new SqlParameter() { ParameterName = "@File", SqlDbType = SqlDbType.NVarChar, Value = FileName },
                    new SqlParameter() { ParameterName = "@C_BY", SqlDbType = SqlDbType.NVarChar, Value = Session["UserID"].ToString() }
                  );
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + ex.Message + "');", true);
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
}