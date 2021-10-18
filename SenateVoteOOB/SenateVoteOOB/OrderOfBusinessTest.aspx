<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="OrderOfBusinessTest.aspx.vb" Inherits="SenateVoteOOB.OrderOfBusinessTest" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

	<head id="Head1" runat="server">
		<title>Alabama Senate Voting System</title>
        <link rel="stylesheet" type="text/css" href="Styles/styleIE.css" />
	</head>

<body class ="Body">
<form id="Form1" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
		<div class="header">
             <div class="headgraphic" >
         
             </div> <!-- end div headgraphic -->
        </div> <!-- end div header-->

<!-- Begin Insertion Point -->

   


        <div class="order_of_business" runat ="server" id ="divOOB">

            <div class="order_of_business_heading" runat="server" id="divOOBHeader" >
                <p style="margin-top:20px;">               
                    <asp:Label ID="lblOrderOBusiness" runat="server" Text=""></asp:Label>
                </p>
            </div>
        
        </div>                     
        
	    <div class="page">                  
                
            <div class="content" >
                <div class="content_bill" runat="server" id="divBillPage">
                    <asp:Label ID="lblBillCalendarPage" runat="server" Text=""></asp:Label>
                </div>
				
                <div class="content_subject" runat="server" id ="divSenatorSubject">
                    <asp:Label ID="lblSenatorSubject" runat="server" Text=""></asp:Label>				
				</div>

                <div class="content_text" id="divText" runat ="server">
               
              <%--  Dial Motion to Adopt
                <br>
                Sanders First Substitute Offered
                <br>
                Dial Motion to Table--%>
                 <asp:Label ID="lblWorkArea" runat="server" Text=""></asp:Label>
               
                    <asp:Label ID="lblWelcome" runat="server" CssClass="content_text_center" 
                        Font-Bold="True" Height="100%" 
                        Text="<br>Welcome<br><br>to the<br><br>Alabama Senate" 
                        Visible="False" Width="100%"></asp:Label>

                     <%--     <asp:Label id="lblOOB" runat="server" CssClass="content_text_center"   Font-Bold="True" Height="100%" 
                        Text="Introduction of Bills<br>House Messages<br>Committee Reports<br>Motions and Resolutions<br>Local Bills<br>Bills on Third Reading" 
                        Visible="False" Width="100%"></asp:Label>--%>
                           
                    <asp:Label ID="lblOOBText" runat="server"   Font-Bold="True" Height="100%" 
                        Text="<br><br>Introduction of Bills<br>House Messages<br>Committee Reports<br>Motions and Resolutions<br>Local Bills<br>Bills on Third Reading" 
                        Visible="False" Width="100%"></asp:Label>

                     <%--     <asp:Label ID="Label1" runat="server"   Font-Bold="True" Height="100%" 
                        Text="Introduction of Bills<br>House Messages<br>Committee Reports<br>Motions and Resolutions<br>Local Bills<br>Bills on Third Reading" 
                        Visible="False" Width="100%">--%>
                </div>


                </div>

            </div>      <!-- end div content -->

                    <br class="clearfix" />
		</div> <!-- </div> end div page --> 

    <!-- End Insertion Point-->    
       <asp:Timer ID="CheckDataTimer" runat="server" Enabled="true" Interval="2000">
</asp:Timer>
    </ContentTemplate>
   </asp:UpdatePanel>

   </form>
	</body>
</html>
