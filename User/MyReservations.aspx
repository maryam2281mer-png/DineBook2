<%@ Page Title="My Reservations" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyReservations.aspx.cs" Inherits="DineBook.User.MyReservations" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <section class="page-section">
        <h1>My Reservations</h1>
        <p>View your upcoming and previous restaurant reservations.</p>

        <asp:Label ID="lblMessage" runat="server" />

        <div class="table-responsive">
            <asp:GridView ID="gvReservations" runat="server"
                AutoGenerateColumns="False"
                CssClass="reservations-table"
                EmptyDataText="You have no reservations yet."
                GridLines="None">
                <Columns>
                    <asp:BoundField DataField="ServiceName" HeaderText="Restaurant" />
                    <asp:BoundField DataField="AppointmentDate" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField DataField="AppointmentTime" HeaderText="Time" />
                    <asp:BoundField DataField="NumberOfGuests" HeaderText="Guests" />
                    <asp:BoundField DataField="SpecialRequest" HeaderText="Special Request" />
                    <asp:BoundField DataField="Status" HeaderText="Status" />
                </Columns>
            </asp:GridView>
        </div>
    </section>
</asp:Content>
