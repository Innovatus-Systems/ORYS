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
using System.Web.UI.HtmlControls;

public partial class MEBInvoiceLocalSave : System.Web.UI.Page
{
    SqlProcsNew sqlobj = new SqlProcsNew();
    protected void Page_Load(object sender, EventArgs e)
    {       
        if (!IsPostBack)
        {
            LoadDropDown();
            LoadTitle();
            rgInvLocal.DataSource = string.Empty;
            rgInvLocal.DataBind();
        }
    }
    private void LoadDropDown()
    {
        try
        {
            DataSet dsTitle = sqlobj.ExecuteSP("SP_AdHocInvoice", new SqlParameter() { ParameterName = "@iMode", SqlDbType = SqlDbType.Int, Value = 3 });
            if (dsTitle.Tables[0].Rows.Count > 0)
            {
                drpService.DataSource = dsTitle.Tables[0];
                drpService.DataTextField = "STDTEXT";
                drpService.DataValueField = "TXNCODE";
                drpService.DataBind();
                drpService.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Please Select", "0"));
            }
            dsTitle.Dispose();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + ex.Message.ToString() + "');", true);
        }
    }
    private void LoadAccountMaster()
    {
        try
        {
            if(drpService.SelectedValue=="0")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Please select invoice type');", true);
                return;
            }
            DataSet dsLOG = sqlobj.ExecuteSP("SP_AdHocInvoice",
                new SqlParameter() { ParameterName = "@IMode", SqlDbType = SqlDbType.Int, Value = 8 },
                 new SqlParameter() { ParameterName = "@TXCODE", SqlDbType = SqlDbType.NVarChar, Value =drpService.SelectedValue.ToString() });

            if (dsLOG.Tables[0].Rows.Count > 0)
            {
                rgInvLocal.DataSource = dsLOG;
                rgInvLocal.DataBind();
                drpService.Enabled = false;
            }
            else
            {
                rgInvLocal.DataSource = string.Empty;
                rgInvLocal.DataBind();
                drpService.Enabled = true;
            }
            dsLOG.Dispose();
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
            DataSet dsTitle = sqlobj.ExecuteSP("SP_GetTitleMenus", new SqlParameter() { ParameterName = "@MenuId", SqlDbType = SqlDbType.Int, Value = 159 });
            if (dsTitle.Tables[0].Rows.Count > 0)
            {
                lnktitle.Text = dsTitle.Tables[0].Rows[0]["Title"].ToString();
                lnktitle.ToolTip = dsTitle.Tables[0].Rows[0]["Description"].ToString();
            }
            dsTitle.Dispose();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + ex.Message.ToString() + "');", true);
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try{
            LoadAccountMaster();
           }
        catch(Exception ex)
        { }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            rgInvLocal.DataSource = string.Empty;
            rgInvLocal.DataBind();
            drpService.Enabled = true;
            drpService.SelectedValue = "0";
        }
        catch (Exception ex)
        { }
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {

    }

    protected void rgInvLocal_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

    }

    protected void rgInvLocal_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }

    protected void rgInvLocal_Init(object sender, EventArgs e)
    {

    }

    protected void rgInvLocal_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

    }
}