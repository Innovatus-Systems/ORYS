<%@ Page Title="" Language="C#" MasterPageFile="~/CovaiSoft.master" AutoEventWireup="true" CodeFile="MEBInvoiceLocalSave.aspx.cs" Inherits="MEBInvoiceLocalSave" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <link href="CSS/bootstrap.css" rel="stylesheet" type="text/css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.2/jquery.min.js"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {


            $(".allow_numeric").on("input", function (evt) {
                var self = $(this);
                self.val(self.val().replace(/[^\d].+/, ""));
                if ((evt.which < 48 || evt.which > 57)) {
                    evt.preventDefault();
                }
            });

            $(".allow_decimal").on("input", function (evt) {
                var self = $(this);
                self.val(self.val().replace(/[^0-9\.]/g, ''));
                if ((evt.which != 46 || self.val().indexOf('.') != -1) && (evt.which < 48 || evt.which > 57)) {
                    evt.preventDefault();
                }
            });

        });
        function ConfirmMsg() {
            var result = confirm('Are you sure, you want to Genenrate Invoice for selected Transaction(s)?');
            if (result) {
                return true;
            }
            else {
                return false;
            }
        }
        function Validation() {
            debugger;            
            //var Ser = document.getElementById("drpService").value;
            var Ser = document.getElementById("drpService").value;
           if (Ser == "0" || Ser == "" || Ser == null) {
                alert('Oops! Plese select Invoice Type.');
                return false;
            }
            else {
                return true;
            }            
        }
    </script>
      <style type="text/css">
        
.Loadingdiv {
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

.centerdiv
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
.centerdiv img
{
    height: 128px;
    width: 128px;
}

    </style>
    

    <style>
        .ddlstyle {
            /*display: block;*/
            padding: 6px 12px;
            font-size: 14px;
            line-height: 1.42857143;
            color: #555;
            background-color: #fff;
            background-image: none;
            border: 1px solid #ccc;
            border-radius: 4px;
            -webkit-box-shadow: inset 0 1px 1px rgba(0, 0, 0, .075);
                    box-shadow: inset 0 1px 1px rgba(0, 0, 0, .075);
            -webkit-transition: border-color ease-in-out .15s, -webkit-box-shadow ease-in-out .15s;
                -o-transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
                    transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
            }
       
    </style>
     <style type="text/css">
        .RadGrid th.rgHeader {
            background-image: none;
            background-color: #196F3D;
            color: white;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="main_cnt">
        <div class="first_cnt">
             <div>
   
    </div>
              <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpPanel">
                <ProgressTemplate>
                    <div class="Loadingdiv">
                        <div class="centerdiv">
                              <asp:Label ID="lblUpdateprogress" runat="server" Text="Please wait..."></asp:Label>
                            <img alt="Loading...." src="loading3.gif" />
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:UpdatePanel ID="UpPanel" runat="server">
                <ContentTemplate>
                    <div runat="server" style="width: 100%">
                        <table style="width: 100%;">
                            <tr>
                                <td align="center">
                                    <asp:LinkButton ID="lnktitle" runat="server" Font-Names="verdana" Font-Size="Medium" ForeColor="Green" Font-Bold="true"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                              <td>
                                <asp:Label ID="Label1" runat="server" Text="Select Inv. Type:" CssClass="style3" Font-Names="verdana" Font-Size="Small"></asp:Label>
                                    </td>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                <asp:DropDownList ID="drpService" CssClass="ddlstyle"  runat="server" ></asp:DropDownList>&nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="btnSearch" runat="Server" Text="Search" CssClass="btn btn-success" OnClientClick="javascript:return Validation()"
                                            ToolTip="Click here to Search." ForeColor="White"  OnClick="btnSearch_Click"  />&nbsp;&nbsp;&nbsp;
                                                    <asp:Button ID="btnClear" runat="Server" Text="Refresh" CssClass="btn btn-primary"
                                            ToolTip="Click here to Clear selected data."  OnClick="btnClear_Click"  />
                                                </td>
                                                <td align="Right">
                                                        <asp:Button ID="btnGenerate" runat="Server" Text="Generate Invoice" CssClass="btn btn-danger"
                                            ToolTip="Click here to Generate Invoice." ForeColor="White"  OnClick="btnGenerate_Click" OnClientClick="javascript:return ConfirmMsg()" />
                                                </td>
                                            </tr>
                                        </table>                                 
                                    </td>                                           
                                </tr>
                            </table>

                        <table>
                            <tr>
                                 <td vertical-align: top;" align="Center" >
                                    <telerik:RadGrid ID="rgInvLocal" runat="server" AutoGenerateColumns="False" Skin="WebBlue"
                                        Height="300px" Width="95%" AllowSorting="true" OnItemCreated="rgInvLocal_ItemCreated" OnItemDataBound="rgInvLocal_ItemDataBound" 
                                        OnItemCommand="rgInvLocal_ItemCommand" AllowMultiRowSelection="true" AllowFilteringByColumn="true" OnInit="rgInvLocal_Init">
                                        <%-- OnItemDataBound="RdGrd_TaskTrack_ItemDataBound"--%>

                                        <ClientSettings>
                                            <%--<Resizing AllowColumnResize="True" AllowResizeToFit="true" AllowRowResize="false" ClipCellContentOnResize="true" EnableRealTimeResize="false" ResizeGridOnColumnResize="false" />--%>
                                            <Scrolling AllowScroll="True" SaveScrollPosition="true" UseStaticHeaders="True" />
                                        </ClientSettings>
                                        <MasterTableView  AllowSorting="true" AllowFilteringByColumn="true">
                                            <NoRecordsTemplate>
                                                No Record Found..
                                            </NoRecordsTemplate>

                                            <Columns>
                                                 <telerik:GridBoundColumn HeaderText="Name" HeaderStyle-Width="150px" DataField="Name" ReadOnly="true" FilterControlWidth="90px"></telerik:GridBoundColumn>
                                                 <telerik:GridBoundColumn HeaderText="Date" HeaderStyle-Width="110px" DataField="Date" ReadOnly="true" FilterControlWidth="90px"></telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderText="Type" HeaderStyle-Width="110px" DataField="Type" ReadOnly="true" FilterControlWidth="90px"></telerik:GridBoundColumn>                                                           
                                                            <telerik:GridBoundColumn HeaderText="InvoiceNo." HeaderStyle-Width="110px" DataField="InvoiceNo" ReadOnly="true" FilterControlWidth="90px"></telerik:GridBoundColumn>
                                                           
                                                            <telerik:GridBoundColumn HeaderText="Amount" HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" DataField="Amount" ReadOnly="true" FilterControlWidth="45px"></telerik:GridBoundColumn>      
                                                            <telerik:GridBoundColumn HeaderText="MailId" HeaderStyle-Width="150px" DataField="MailId" ReadOnly="true" FilterControlWidth="90px"></telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderText="FileName" HeaderStyle-Width="300px" DataField="FileName" ReadOnly="true" FilterControlWidth="250px"></telerik:GridBoundColumn>      
<%--                                                <telerik:GridBoundColumn HeaderText="Date" HeaderStyle-Width="80px" DataField="Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ReadOnly="true" AllowFiltering="true" FilterControlWidth="65px" HeaderTooltip="Sorted by ascending order of date."></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderText="Txn.Cd" HeaderStyle-Width="80px" DataField="Group" ReadOnly="true" AllowFiltering="true" FilterControlWidth="65px" HeaderTooltip="This links to the transaction posting rules." ></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderText="Ref" HeaderStyle-Width="80px" DataField="Ref" AllowFiltering="true" FilterControlWidth="85px" HeaderTooltip="Digits 1 to 14 – Batch ref.no,  Last two digits – Sequence no. within the batch , Last but one digit – No.of txns in the batch."></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn HeaderText="Narration" HeaderStyle-Width="180px" DataField="Narration" ReadOnly="true" AllowFiltering="true" FilterControlWidth="135px" HeaderTooltip="Standard narration as set for the transaction code # additional remarks"></telerik:GridBoundColumn>
                                                <%--<telerik:GridBoundColumn HeaderText="DR/CR" HeaderStyle-Width="40px" DataField="Type" ReadOnly="true" AllowFiltering="true" FilterControlWidth="25px" HeaderTooltip=""></telerik:GridBoundColumn>--%>
<%--                                                <telerik:GridBoundColumn HeaderText="Amt." HeaderStyle-Width="50px" DataField="DR" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" ReadOnly="true" AllowFiltering="true" FilterControlWidth="55px" HeaderTooltip=""></telerik:GridBoundColumn>--%>
                                                <%--<telerik:GridBoundColumn HeaderText="Credit" HeaderStyle-Width="50px" DataField="CR" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" ReadOnly="true" AllowFiltering="true" FilterControlWidth="55px" HeaderTooltip=""></telerik:GridBoundColumn>--%>
                                              
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings>
                                            <Scrolling AllowScroll="True" SaveScrollPosition="True"></Scrolling>
                                            <Selecting AllowRowSelect="true"></Selecting>
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                  
                                </td>
                            </tr>
                        </table>

                     </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
         </div>
</asp:Content>

