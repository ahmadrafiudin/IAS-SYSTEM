<%@ Page Language="VB" MasterPageFile="~/pagesetting/MasterIntern.master" AutoEventWireup="false" CodeFile="xm_masteraddress_xls_s.aspx.vb" Inherits="xm_masteraddress_xls_s"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mpCONTENT" Runat="Server">
<link href="../script/calendar.css" rel="stylesheet" type="text/css" media="all" />
<script type="text/javascript" src="../script/calendar.js"></script>

<script type="text/javascript" language="Javascript">
<!-- hide script from older browsers
function openwindow(url,nama,width,height)
{
OpenWin = this.open(url, nama);
if (OpenWin != null)
{
  if (OpenWin.opener == null)
  OpenWin.opener=self;
}
OpenWin.focus();
} 
// End hiding script-->
</script>

<form id="mpFORM" runat="server">
<ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true" EnableScriptLocalization="true" ID="ToolkitScriptManager1" />

<asp:Table id="mlTABLE1" BorderWidth ="0" CellPadding ="0" CellSpacing="0" Width="100%" runat="server">
<asp:TableRow>   
<asp:TableCell> 
<asp:Panel ID="pnTOOLBAR" runat="server">  
    <table border="0" cellpadding="3" cellspacing="3">
        <tr>
            <td><asp:ImageButton id="btNewRecord" ToolTip="NewRecord" ImageUrl="~/images/toolbar/new.jpg" runat="server" /></td>
            <td><asp:ImageButton id="btSaveRecord" ToolTip="SaveRecord" ImageUrl="~/images/toolbar/save.jpg" runat="server" OnClientClick="return confirm('Save Record ?');" /></td>
            <td><asp:ImageButton id="btSearchRecord" ToolTip="SearchRecord" ImageUrl="~/images/toolbar/find.jpg" runat="server" /></td>
            <td><asp:ImageButton id="btCancelOperation" ToolTip="CancelOperation" ImageUrl="~/images/toolbar/cancel.jpg" runat="server" /></td>            
        </tr>               
    </table>
    <hr />
</asp:Panel>
</asp:TableCell>    
</asp:TableRow>

<asp:TableRow><asp:TableCell><p class="header1"><b><asp:Label id="mlTITLE" runat="server"></asp:Label></b></p></asp:TableCell></asp:TableRow>
<asp:TableRow><asp:TableCell><p><asp:Label ID="mlMESSAGE" runat="server" ForeColor="Purple" Font-Italic="true"></asp:Label></p></asp:TableCell></asp:TableRow>
<asp:TableRow><asp:TableCell><asp:HiddenField ID="mlSYSCODE" runat="server"/></asp:TableCell></asp:TableRow>
<asp:TableRow><asp:TableCell><p><asp:HyperLink ID="mlLINKDOC" runat="server"></asp:HyperLink></p></asp:TableCell></asp:TableRow>


<asp:TableRow>
<asp:TableCell>
<asp:Panel ID="pnFILL" runat="server">
    <table width="100%" cellpadding="0" cellspacing="0" border="0">                 
        
        <tr>
            <td><p><asp:Label ID="lbITEM" Text="Upload Excel File" runat="server"></asp:Label></p></td>
            <td valign="top">:</td>
            <td valign="top">                
                <asp:FileUpload ID="fpUPLOAD1" runat="server" />
            </td>  
        </tr>
        
        <tr>
            <td><p><asp:Label ID="lbSHEET" Text="Worksheet Name" runat="server"></asp:Label></p></td>
            <td valign="top">:</td>
            <td valign="top">                
                <asp:TextBox ID="txSHEET" runat="server" Width="250" ></asp:TextBox>
                <asp:ImageButton ID="btSUBMIT" ToolTip="" ImageUrl="~/images/toolbar/autocomplete.jpg" runat="server" />
            </td>  
        </tr>
    </table>
      
    <hr /><br />
</asp:Panel>
</asp:TableCell>
</asp:TableRow>




<asp:TableRow>
<asp:TableCell>
<asp:Panel ID="pnGRID" runat="server">    
    
    <asp:DataGrid runat="server" ID="mlDATAGRID" 
    OnItemCommand="mlDATAGRID_ItemCommand"
    autogeneratecolumns="true"
    CssClass="Grid"
    >	    
    
    
    
    <HeaderStyle CssClass="GridHeader"></HeaderStyle>
    <ItemStyle CssClass="GridItem"></ItemStyle>
    <EditItemStyle  CssClass="GridItem" />
    <PagerStyle  CssClass="GridItem" />
    <AlternatingItemStyle CssClass="GridAltItem"></AlternatingItemStyle>
    <Columns>  
    
        
        
    </Columns>
 </asp:DataGrid>  
</asp:Panel>

</asp:TableCell>
</asp:TableRow>

</asp:Table>
</form>
<br /><br /><br /><br />

</asp:Content>

