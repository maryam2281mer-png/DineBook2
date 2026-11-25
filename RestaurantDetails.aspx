
<%@ Page Title="Restaurant Details" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="RestaurantDetails.aspx.cs"
    Inherits="DineBook.RestaurantDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<section class="page-section">
    <div class="container">

        <div class="card">

            <asp:Image ID="imgRestaurant" runat="server"
                CssClass="restaurant-image" />

            <h1 class="section-title">
                <asp:Label ID="lblName" runat="server" />
            </h1>

            <p>
                <asp:Label ID="lblDescription" runat="server" />
            </p>

            <p>Price:
                <asp:Label ID="lblPrice" runat="server" />
            </p>

            <hr />

            <h2>Make a Reservation</h2>
            <p>Choose a date, time, and number of guests.</p>

            <asp:Label ID="lblMessage" runat="server" />

            <p>Reservation Date</p>
            <asp:TextBox ID="txtDate" runat="server"
                TextMode="Date" CssClass="form-control" />

            <asp:Button ID="btnLoadTimes" runat="server"
                Text="Show Available Times"
                CssClass="btn-primary"
                OnClick="btnLoadTimes_Click" />

            <p style="margin-top:20px;">Available Time</p>
            <asp:DropDownList ID="ddlTime" runat="server"
                CssClass="form-control">
                <asp:ListItem Text="Choose a time" Value="" />
            </asp:DropDownList>

            <p>Number of Guests</p>
            <asp:DropDownList ID="ddlGuests" runat="server"
                CssClass="form-control">
                <asp:ListItem Text="1 Person" Value="1" />
                <asp:ListItem Text="2 People" Value="2" />
                <asp:ListItem Text="3 People" Value="3" />
                <asp:ListItem Text="4 People" Value="4" />
                <asp:ListItem Text="5 People" Value="5" />
                <asp:ListItem Text="6 People" Value="6" />
                <asp:ListItem Text="7 People" Value="7" />
                <asp:ListItem Text="8 People" Value="8" />
            </asp:DropDownList>

            <p>Special Request (Optional)</p>
            <asp:TextBox ID="txtRequest" runat="server"
                TextMode="MultiLine" Rows="3"
                CssClass="form-control" />

            <asp:Button ID="btnBook" runat="server"
                Text="CONFIRM RESERVATION"
                CssClass="btn-primary"
                OnClick="btnBook_Click" />

        </div>
    </div>
</section>

</asp:Content>
