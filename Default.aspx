<%@ Page Language="C#" Inherits="Zip_Code_Finder.Default" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Default</title>
    <style>
            .pageStyle{
                background-color:#003057;
                color:white;
                font-family:'Open Sans','Helvetica','Verdana',sans-serif;
            }
            
            .zipCodeTB{
                height:30px;
                font-size:15px;
                background-color:white;
                color:black;
            }
            
            .cityInfoBox{
                color:black;
                background-color:white !important;
                border-color:black !important;
                text-align:left;
                border:solid;
                border-radius:7px;
                padding:5px;
                width:fit-content;
            }

            .submit{
                background-color:#007fa3;
                color:white;
                border-radius:4px;
                border-color:#007fa3;
                font-size:15px;
                font-weight:bold;
                padding-right:15px;
                padding-left:15px;
                padding-top:7px;
                padding-bottom:7px;
            }

            .add{
                background-color:#ffb81c;
                color:white;
                border-radius:4px;
                border-color:#ffb81c;
                font-size:15px;
                font-weight:bold;
                padding-right:15px;
                padding-left:15px;
                padding-top:7px;
                padding-bottom:7px;
            }

            .warning {
                color:red;
                font-size:24px;
            }
        </style>
</head>
<body class="pageStyle">
    <div style="text-align:center;">
        <u><h1>Chris Chi's Zip Code Locator</h1></u>
    </div>
           
    <div style="width:100%;display:flex;">
        <div style="text-align:left;width:50%;">
            <div style="text-align:center;">
                <u><h4>Enter a 5-digit U.S Zip Code below: </h4></u>
            </div>

            <form id="form1" runat="server" style="text-align:center;">
                <asp:TextBox id="tb_enterZip" runat="server" CssClass="zipCodeTB" placeholder="e.g. 12345" MaxLength=5/>
                <br/><br/>
                <asp:Button id="btn_submit_zip" runat="server" CssClass="submit" Text="Submit" OnClick="btn_Submit_ZipCode" />
                <asp:Button id="btn_add_to_list" runat="server" CssClass="add" Text="Add" OnClick="btn_Add_To_List_Click"/>
            </form>

            <br/>

                <div style="text-align:center;">
                <asp:Label id="lbl_all_alerts" runat="server" Visible="false" CssClass="warning"/> 
                <br/>
            </div>
                
            <br/>
                
            <div id="div_city_info" runat="server" class="cityInfoBox" style="margin-left:auto;margin-right:auto;padding:20px;">
                <strong><span style="font-size:22px;">Results for <asp:Label id="lbl_zip" runat="server"/></span></strong>:
                <br/>
                <asp:Label id="lbl_city" runat="server" Style="color:orange !important;font-size:19px;font-weight:bold;line-height:35px;"/>
                <br/>
                <asp:Label id="lbl_long_and_lat" Style="line-height:24px;" runat="server"/>
            </div>
        </div>
            
        <div style="width:50%;text-align:center;border:solid;border-radius:5px;margin-left:5px;">
            <u><h4>Saved Zip Code List:</h4></u>
            <strong><asp:Label id="lbl_city_list" runat="server"/></strong>
        </div> 
    </div>
</body>
</html>
