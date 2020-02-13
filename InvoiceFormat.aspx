<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InvoiceFormat.aspx.cs" Inherits="InvoiceFormat" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            height: 16px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table style='width: 100%; font-family: Verdana; font-size: 10px; margin-left: 15px;'>
                <tr>
                    <td align='center' style='font-size: 10px;'><b>INVOICE </b></td>
                </tr>
                <tr>
                    <td style="width: 100%; vertical-align: top;">
                        <table border="1" style="table-layout:auto; width: 100%; border-style: dotted; border-width: thin; border-collapse: collapse;">
                            <tr>
                                <td style="width: 10%; vertical-align: top; table-layout:fixed;" rowspan="4">
                                    
                                </td>
                                <td style="width: 40%; vertical-align: top;" rowspan="4">
                                    <b>Covai Senior Citizens Services Pvt Ltd</b>
                                    <br />
                                    Reg. Office 13/4 Covaicare Tower<br />
                                </td>
                            </tr>
                            <tr>
                                <td>Invoice No.
                                    <br />
                                    <b>M/926</b>
                                </td>
                                <td>Dated<br />
                                    <b>04-Dec-2018</b>
                                </td>
                            </tr>
                            <tr>
                                <td>Delivery Note
                                    <br />
                                    <b>-</b>
                                </td>
                                <td>Mode/Terms of Payment<br />
                                    <b>-</b>
                                </td>
                            </tr>
                            <tr>
                                <td>Buyer's Order No.
                                    <br />
                                    <b>-</b>
                                </td>
                                <td>Dated<br />
                                    <b>-</b>
                                </td>
                            </tr>

                            <tr>
                                <td colspan="2" rowspan="4">
                                    <b>Buyer</b>
                                    <br />
                                    SDM 02<br />
                                    <br />
                                    <br />
                                    <br />
                                    <br />
                                    <br />
                                    <br />
                                    <br />

                                </td>
                            </tr>
                            <tr>
                                <td>Despatch Document No.
                                    <br />
                                    <b>-</b>
                                </td>
                                <td>Delivery Note Date<br />
                                    <b>-</b>
                                </td>
                            </tr>
                            <tr>
                                <td>Despatched through
                                    <br />
                                    <b>-</b>
                                </td>
                                <td>Destination<br />
                                    <b>-</b>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                    <br />
                                </td>
                                <td>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table border="1" style="width: 100%; border-style: solid; border-width: thin; border-collapse: collapse;">
                            <tr>
                                <th style="width: 5%;">#</th>
                                <th style="width: 45%;">Description of Goods and Services</th>
                                <th style="width: 25%;">HSN/SAC</th>
                                <th style="width: 35%;">Amount</th>
                            </tr>
                            <tr>
                                <td>1</td>
                                <td>Maintenance Charges</td>
                                <td>99322</td>
                                <td align="right">5600.00</td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>Total</td>
                                <td></td>
                                <td align="right"><b>5600.00</b></td>
                            </tr>
                            <tr>
                                <td colspan="3">Amount Chargeable (in words)
                                    <br />
                                    <b>INR Five Thousand Six Hundred only</b>
                                </td>
                                <td style="vertical-align: top; text-align: right;">E. & O.E
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table border="1" style="width: 100%; border-style: solid; border-width: thin; border-collapse: collapse;">
                            <tr>
                                <th style="width: 20%;" rowspan="2">HSN/SAC</th>
                                <th style="width: 20%;" rowspan="2">Taxable Value</th>
                                <th style="width: 20%;" colspan="2">Central Tax</th>
                                <th style="width: 20%;" colspan="2">State Tax</th>
                                <th style="width: 20%;" rowspan="2">Total Tax Amount</th>
                            </tr>
                            <tr>
                                <td align="right">Rate</td>
                                <td align="right">Amount</td>
                                <td align="right">Rate</td>
                                <td align="right">Amount</td>
                            </tr>
                            <tr>
                                <td>999322</td>
                                <td align="right">5655</td>
                                <td align="right">9%</td>
                                <td align="right">509.00</td>
                                <td align="right">9%</td>
                                <td align="right">509.00</td>
                                <td align="right">1018.00</td>
                            </tr>
                            <tr>
                                <td colspan="7">Tax Amount (in words)&nbsp;:&nbsp;<b>INR Five Thousand Six Hundred only</b><br />
                                    <br />
                                </td>
                            </tr>

                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%;">
                        <table style="width: 100%;">
                            <tr>
                                <td style="width: 50%;">Remarks: <br />
                                    Being Maintenance charges for the period of 26-10-2018 to 25-11-2018 - Invoice o. 926/27.11.2018 <br /><br />
                                    Company's PAN : <b>AAFCC6261P</b> <br /><br />
                                    <u>Declaration:</u><br />
                                    We declare that this invoice shows the actual price of the goods described and that all particulars are true and correct.
                                </td>
                                <td style="width: 50%;vertical-align:top;">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td colspan="2">
                                                Company's Bank Details
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style1">
                                                Bank Name :
                                            </td>
                                            <td class="auto-style1">
                                                Indian Overseas Bank
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                A/C No. :
                                            </td>
                                            <td>
                                                193702000000317
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Branch & IFSC Code :
                                            </td>
                                            <td>
                                                Madampatti Branch & IOBA00019431
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="right">
                                                <i>for</i> <b>Covai Senior Citizens Pvt Ltd</b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="right">
                                                Authorised Signatory
                                            </td>
                                        </tr>
                                    </table>                                                                                                            

                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align='center' colspan='2' style='font-size: 10px;'>This is a Computer Generated Invoice</td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
