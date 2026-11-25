
CREATE DATABASE DineBookDB;
GO

USE DineBookDB;
GO

-- 1. Users: العملاء والأدمن
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL,
    Phone NVARCHAR(20),
    Role NVARCHAR(20) NOT NULL DEFAULT 'User',
    CONSTRAINT CK_Users_Role
        CHECK (Role IN ('User', 'Admin'))
);
GO

-- 2. Services: المطاعم
CREATE TABLE Services (
    ServiceID INT IDENTITY(1,1) PRIMARY KEY,
    ServiceName NVARCHAR(150) NOT NULL,
    Description NVARCHAR(1000),
    Duration INT NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    ServicePhoto NVARCHAR(500)
);
GO

-- 3. Availability: أوقات توفر المطاعم
CREATE TABLE Availability (
    AvailabilityID INT IDENTITY(1,1) PRIMARY KEY,
    ServiceID INT NOT NULL,
    AvailableDate DATE NOT NULL,
    AvailableTime TIME NOT NULL,

    CONSTRAINT FK_Availability_Services
        FOREIGN KEY (ServiceID)
        REFERENCES Services(ServiceID),

    CONSTRAINT UQ_Availability_Slot
        UNIQUE (ServiceID, AvailableDate, AvailableTime)
);
GO

-- 4. Appointments: حجوزات الطاولات
CREATE TABLE Appointments (
    AppointmentID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    ServiceID INT NOT NULL,
    AppointmentDate DATE NOT NULL,
    AppointmentTime TIME NOT NULL,

    -- حقول إضافية لتفاصيل حجز المطعم
    NumberOfGuests INT NOT NULL DEFAULT 2,
    SpecialRequest NVARCHAR(500),
    Status NVARCHAR(30) NOT NULL DEFAULT 'Confirmed',

    CONSTRAINT FK_Appointments_Users
        FOREIGN KEY (UserID) REFERENCES Users(UserID),

    CONSTRAINT FK_Appointments_Services
        FOREIGN KEY (ServiceID) REFERENCES Services(ServiceID),

    CONSTRAINT CK_Appointments_Guests
        CHECK (NumberOfGuests > 0),

    CONSTRAINT CK_Appointments_Status
        CHECK (Status IN ('Confirmed', 'Cancelled', 'Completed'))
);
GO

-- فهارس لتحسين البحث عن الحجوزات
CREATE INDEX IX_Appointments_UserID
ON Appointments(UserID);

CREATE INDEX IX_Appointments_ServiceDate
ON Appointments(ServiceID, AppointmentDate);
GO


INSERT INTO Users (Name, Email, Password, Phone, Role)
VALUES (
    'DineBook Admin',
    'admin@dinebook.com',
    'oTbt5aFQuofUfpMgsVGi3Q==:raJ7NHuA9Y5m4VesjxrowWzcGiLU5hj+lCJ5UyJyg0c=',
    NULL,
    'Admin'
);


-- Prevent duplicate active reservations
-- for the same restaurant, date, and time.

CREATE UNIQUE INDEX UX_Appointments_ActiveSlot
ON Appointments (ServiceID, AppointmentDate, AppointmentTime)
WHERE Status <> 'Cancelled';


-- Sample restaurants
INSERT INTO Services
(ServiceName, Description, Duration, Price, ServicePhoto)
VALUES
(
    'The Garden Restaurant',
    'A relaxing restaurant offering a variety of international dishes.',
    120, 150, NULL
),
(
    'Bella Italia',
    'Enjoy authentic Italian food, pasta, and delicious pizza.',
    120, 180, NULL
),
(
    'Ocean View',
    'A seafood restaurant with a beautiful dining atmosphere.',
    120, 220, NULL
);

-- Sample available reservation slots
INSERT INTO Availability
(ServiceID, AvailableDate, AvailableTime)
VALUES
(1, '2026-12-01', '18:00'),
(1, '2026-12-01', '20:00'),
(1, '2026-12-02', '19:00'),

(2, '2026-12-01', '18:00'),
(2, '2026-12-01', '20:00'),
(2, '2026-12-02', '19:00'),

(3, '2026-12-01', '18:00'),
(3, '2026-12-01', '20:00'),
(3, '2026-12-02', '19:00');

