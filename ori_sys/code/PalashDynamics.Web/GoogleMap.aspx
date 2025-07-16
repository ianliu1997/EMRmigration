<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GoogleMap.aspx.cs" Inherits="GoogleMapHoster.Web.GoogleMap" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <script src="http://maps.google.com/maps/api/js?sensor=false" type="text/javascript"></script>
    <script type="text/javascript">
        var map = null;

          
        function initialize() {
            var latlng = new google.maps.LatLng(22.75592, 78.79395);
            var myOptions = {
                zoom: 4,
                center: latlng,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            }

            map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
        }

        //        function showVehicle(lat, lon, desc, vehiclename) {
        function showVehicle() {
            //init();
            initialize();
            var lat = document.getElementById("Latitude").value;
            var lon = document.getElementById("Langitude").value;
            var myLatlng = new google.maps.LatLng(lat, lon);
            //Get the marker object
            var marker = new google.maps.Marker({
                position: myLatlng,
                map: map,
                title: "Hello World!"
            });
            map.setCenter(myLatlng, 16);
            map.setZoom(14);
        }
        //function showMaxZoom(latlng) {   G_SATELLITE_MAP.getMaxZoomAtLatLng(latlng, function(response) {     if (response && response['status'] == G_GEO_SUCCESS) {       alert("The maximum zoom level where satellite imagery is available at this location is: " + response['zoom']);     }   });
    </script>
</head>
<body onload="showVehicle()" >
   <form id="form1" runat="server" >
     <table width="100%" cellpadding="1,1,1,1">
    <tr>
        <td >
           <asp:Label ID="lblAddress" Text = "Address :" runat="server" ></asp:Label>
        </td>
        <td>
            <asp:HiddenField ID = "Latitude" runat="server" />
             <asp:HiddenField ID = "Langitude" runat="server" />
         </td>
    <%--    <input id="Latitude" type="hidden" />
        <input id="Hidden1" type="hidden" />--%>
     
        <%--<td>
            <asp:TextBox ID="TextBox1" runat="server" Width="200"  ></asp:TextBox>
        </td>
        <td>
            <asp:Button
        </td>--%>
    </tr>
    </table>
       <div id="map_canvas" style="width:900px; height:500px">
        
    </div>
    </form>
   <%-- <script language ="javascript" type="text/javascript">
        showVehicle();
    </script>--%>
</body>
</html>
