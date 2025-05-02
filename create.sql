CREATE TABLE Users (
    IdUser INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    IdStatus INT NOT NULL,
    IdUserRole INT NOT NULL
);

CREATE TABLE Categories (
    IdCategory INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL
);

CREATE TABLE Products (
    IdProduct INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX),
    Price DECIMAL(18, 2) NOT NULL,
    Quantity INT NOT NULL,
    IdCategory INT NOT NULL,
    IdStatus INT NOT NULL
    CONSTRAINT FK_Products_Categories FOREIGN KEY (IdCategory) REFERENCES Categories(IdCategory)
);

CREATE TABLE Notifications (
    IdNotification INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    IdAddresse INT NOT NULL,
    IdStatus INT NOT NULL
    CONSTRAINT FK_Notifications_Users FOREIGN KEY (IdAddresse) REFERENCES Users(IdUser)
);