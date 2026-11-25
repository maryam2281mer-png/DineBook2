<%@ Page Title="Admin Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="DineBook.Admin.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <section class="page-section">
        <h1>Admin Dashboard</h1>

        <h2>Welcome, <asp:Label ID="lblAdminName" runat="server" Text="Admin" />!</h2>

        <p>Manage your DineBook platform from one place.</p>

        <div class="dashboard-grid">

            <div class="dashboard-card">
                <h3>Manage Users</h3>
                <p>View, add, update, and delete user accounts.</p>
                <a href="ManageUsers.aspx" class="btn-primary">Manage Users</a>
            </div>

            <div class="dashboard-card">
                <h3>Manage Restaurants</h3>
                <p>Add, edit, and remove restaurants.</p>
                <a href="ManageRestaurants.aspx" class="btn-primary">Manage Restaurants</a>
            </div>

            <div class="dashboard-card">
                <h3>Manage Reservations</h3>
                <p>View and manage all customer reservations.</p>
                <a href="ManageReservations.aspx" class="btn-primary">Manage Reservations</a>
            </div>

            <div class="dashboard-card">
                <h3>Manage Availability</h3>
                <p>Control available restaurant dates and times.</p>
                <a href="ManageAvailability.aspx" class="btn-primary">Manage Availability</a>
            </div>

        </div>
    </section>
</asp:Content>
