<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MailFormat.aspx.cs" Inherits="MailFormat" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
            height: 23px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table width="100%">
                <tr>
                    <td style="width: 100%; text-align: left;">
                        <span style="font-family: Verdana; font-size: 12px; color: darkblue;">Preview Statistics for <b><span style="color:darkolivegreen;">Today</span></b> and <b><span style="color:coral;">Yesterday</span></b>  as of <b>dd-mon-yyyy hh:mn</b>:</span>
                        <br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%; text-align: left;">
                        <span style="font-family: Verdana; font-size: 12px; color: darkblue; font-weight:600;"><u>Today:</u></span>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%; text-align: left;">
                        <table style="border: 1PX SOLID #000000; font-size: 12px; border-collapse: COLLAPSE; padding: 3PX; margin-left: 0PX; margin-top: 10PX; font-family: Verdana;">
                            <tr style="background-color: darkolivegreen;">
                                <th style="border: 1PX SOLID #000000; border-collapse: COLLAPSE; padding: 5PX;" align="left">Status</th>
                                <th style="border: 1PX SOLID #000000; border-collapse: COLLAPSE; padding: 5PX;" align="center">Count</th>                                
                            </tr>
                            <tr>
                                <td style="border: 1PX SOLID #000000; border-collapse: COLLAPSE; padding: 5PX;" align="left">Submitted</td>
                                <td style="border: 1PX SOLID #000000; border-collapse: COLLAPSE; padding: 5PX;" align="center">507</td>
                            </tr>
                        </table>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td style="text-align: left;">
                                    <span style="font-family: Verdana; font-size: 12px; color: darkblue; font-weight: 500;">Earliest still in Submitted state:</span>
                                </td>
                                <td style="text-align: left;">
                                    <span style="font-family: Verdana; font-size: 12px; color: darkblue; font-weight: 600;">100</span>
                                </td>
                            </tr>
                        </table>
                    </td>
                    
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td style="text-align: left;">
                                    <span style="font-family: Verdana; font-size: 12px; color: darkblue; font-weight: 500;">Last Submitted  and still in Submitted State:</span>
                                </td>
                                <td style="text-align: left;">
                                    <span style="font-family: Verdana; font-size: 12px; color: darkblue; font-weight: 600;">100</span>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                 <tr>
                    <td style="text-align: left;">
                        <span style="font-family: Verdana; font-size: 12px; color: darkblue; font-weight:600;"><u>Yesterday:</u></span>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%; text-align: left;">
                       <table style="border: 1PX SOLID #000000; font-size: 12px; border-collapse: COLLAPSE; padding: 3PX; margin-left: 0PX; margin-top: 10PX; font-family: Verdana;">
                            <tr style="background-color: coral;">
                                <th style="border: 1PX SOLID #000000; border-collapse: COLLAPSE; padding: 5PX;" align="left">Status</th>
                                <th style="border: 1PX SOLID #000000; border-collapse: COLLAPSE; padding: 5PX;" align="center">Count</th>                                
                            </tr>
                            <tr>
                                <td style="border: 1PX SOLID #000000; border-collapse: COLLAPSE; padding: 5PX;" align="left">Submitted</td>
                                <td style="border: 1PX SOLID #000000; border-collapse: COLLAPSE; padding: 5PX;" align="center">507</td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <br/><span style="font-family:Verdana;font-size:10px;">Innovatus Systems Special Support Team for Akzo Nobel Dulux Smart System.</span><br/>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
