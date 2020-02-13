<%@ Page Title="" Language="C#" MasterPageFile="~/CovaiSoft.master" AutoEventWireup="true" CodeFile="TestLoading.aspx.cs" Inherits="TestLoading" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
    <link href="Content/bootstrap.css" rel="stylesheet" />
     <style type="text/css">
        
.modal {
     position: fixed;
    z-index: 999;
    height: 100%;
    width: 100%;
    top: 0;
    background-color: Black;
    filter: alpha(opacity=60);
    opacity: 0.6;
    -moz-opacity: 0.8;
}

.center
{
    z-index: 1000;
    margin: 300px auto;
    padding: 10px;
    width: 130px;
    background-color: White;
    border-radius: 10px;
    filter: alpha(opacity=100);
    opacity: 1;
    -moz-opacity: 1;
}
.center img
{
    height: 128px;
    width: 128px;
}

    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <asp:UpdateProgress ID="Upprocess" runat="server" AssociatedUpdatePanelID="Up">
        <ProgressTemplate>
            <div class="modal">
                        <div class="center">
                             <asp:Label ID="lblUpdateprogress" runat="server" Text="Please wait..."></asp:Label><br />
                            <img alt="Loading...." src="Images/Loader.gif" />
                        </div>
                    </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel runat="server" ID="Up">
        <ContentTemplate>
            <asp:Button ID="button" runat="server" Text="Button" OnClick="button_Click"/>
              <telerik:RadGrid ID="grid" AutoGenerateColumns="true" runat="server" Visible="true">
                                                                <MasterTableView>
                                                                   
                                                                    </MasterTableView>
                  </telerik:RadGrid>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

