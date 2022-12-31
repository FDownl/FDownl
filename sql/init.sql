USE fdownl
CREATE TABLE CouponCodes (
    Id int NOT NULL AUTO_INCREMENT,
    Code longtext,
    LifetimeAdd int NOT NULL,
    LifetimeSet int NOT NULL,
    PRIMARY KEY (Id)
);
CREATE TABLE StorageServers (
    Id int NOT NULL AUTO_INCREMENT,
    Ip longtext,
    Hostname longtext,
    Location longtext,
    RowVersion timestamp(6) DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (Id)
);
CREATE TABLE UploadedFiles (
    Id int NOT NULL AUTO_INCREMENT,
    RandomId longtext,
    ServerName longtext,
    Hostname longtext,
    Filename longtext,
    UploadedAt datetime(6) NOT NULL,
    Lifetime int NOT NULL,
    Ip longtext,
    Size bigint NOT NULL,
    Coupon longtext,
    RowVersion timestamp(6) DEFAULT CURRENT_TIMESTAMP(6),
    IsEncrypted tinyint(1) NOT NULL DEFAULT 0,
    ZipContents longtext,
    IsPublic tinyint(1) NOT NULL DEFAULT 0,
    PRIMARY KEY (Id)
);
