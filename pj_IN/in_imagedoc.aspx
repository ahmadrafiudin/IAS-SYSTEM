<%@ Page Language="VB" MasterPageFile="~/pagesetting/MasterPrint.master" AutoEventWireup="false" CodeFile="in_imagedoc.aspx.vb" Inherits="in_imagedoc" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mpCONTENT" Runat="Server">

<script type="text/javascript" Language="Javascript">
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


<form id="form1" runat="server">
<p><asp:Label ID="mlMESSAGE" runat="server" ForeColor="Purple" Font-Italic="true"></asp:Label></p>
<asp:HiddenField ID="mlSYSCODE" runat="server"/>
    
    <table width="100%" cellpadding="0" cellspacing="0" border="1" bordercolor="gray">
    <tr>
        <td align="Center">       
            <table width="100%" cellpadding="2" cellspacing="2" border="0" >           
            <tr>
            <td align="Right"><img src="../images/company/logo_100bw.png" /></td>
            
            <td align="center">
                <p class="header2"><asp:Label ID="mlCOMPANYNAME" runat="server"></asp:Label></p>
                <asp:Label ID="mlCOMPANYADDRESS" runat="server"></asp:Label><br />                                 
            </td>
            
            <td></td>
            
            </tr>
            </table>            
        </td>
    </tr>
    
    <tr>
    <td>        
        <table width="100%" cellpadding="0" cellspacing="0"  border="0">
        
            <tr><td colspan="3"><br /><br /></td></tr>
            
            <tr>
                <td align="center" colspan="3">
                    <p class="header2"><b><asp:Label ID="lbTITLE" runat="server"></asp:Label></b></p>
                    
                </td>
            </tr>       
            
            <tr><td colspan="3"><br /><br /></td></tr>
         </table>
                     
        </td>
    </tr>
    
    <tr>
    <td>        
        <table width="100%" cellpadding="0" cellspacing="0"  border="0">
        
            <tr><td colspan="3"><br /></td></tr>
            
            <tr>
                <td><p>Kode Produk</p></td>
                <td><p>:</p></td>
                <td><p><asp:Label ID="lbITEMKEY" runat="server"></asp:Label></p></td>
            </tr>
            
            <tr>
                <td><p>Nama Produk</p></td>
                <td><p>:</p></td>
                <td><p><asp:Label ID="lbITEMDESC" runat="server"></asp:Label></p></td>                
            </tr>            
            
            
            <tr><td colspan="3" align="center">
                <br />
                <asp:Image ID="mpIMAGE"  runat="server" width="200" Height="200" AlternateText=""/>
            </td></tr>
            
            <tr><td colspan="3"><br /></td></tr>
            </table>            
        </td>
    </tr>
    
    </table>
    
    
    <br /><br />        
    <input type="button" value="Close" onclick="window.close();return false;" />
    
    </form>                        

<br /><br /><br /><br />
</asp:Content>
