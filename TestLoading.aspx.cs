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

public partial class TestLoading : System.Web.UI.Page
{
    SqlProcsNew sqlobj = new SqlProcsNew();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void button_Click(object sender, EventArgs e)
    {
        grid.Visible = false;
         DataSet dstxnCode = sqlobj.ExecuteSP("SP_TxnDropDownList",
               new SqlParameter() { ParameterName = "@iMode", SqlDbType = SqlDbType.Int, Value = 10 });
         if (dstxnCode.Tables[0].Rows.Count > 0)
         {
             grid.DataSource = dstxnCode.Tables[0];
             grid.DataBind();
         }
    }
}