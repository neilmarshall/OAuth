CREATE TABLE OAuthDemo.AspNetUsers (
    Id INT IDENTITY(1, 1) PRIMARY KEY,
    AccessFailedCount INT NOT NULL DEFAULT 0,
    ConcurrencyStamp NVARCHAR(MAX),
    Email NVARCHAR(255),
    EmailConfirmed BIT NOT NULL DEFAULT 0,
    LockoutEnabled BIT NOT NULL DEFAULT 0,
    LockoutEnd DATETIMEOFFSET,
    NormalizedEmail NVARCHAR(255),
    NormalizedUserName NVARCHAR(255),
    PasswordHash NVARCHAR(MAX),
    PhoneNumber NVARCHAR(MAX),
    PhoneNumberConfirmed BIT NOT NULL DEFAULT 0,
    SecurityStamp NVARCHAR(MAX),
    TwoFactorEnabled BIT NOT NULL DEFAULT 0,
    UserName NVARCHAR(255),
);

CREATE UNIQUE INDEX EmailIndex ON OAuthDemo.AspNetUsers (NormalizedEmail);
CREATE UNIQUE INDEX UserNameIndex ON OAuthDemo.AspNetUsers (NormalizedUserName);