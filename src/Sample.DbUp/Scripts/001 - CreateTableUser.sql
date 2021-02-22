CREATE TABLE "User"
(
    user_id varchar(200) NOT NULL,
    user_name varchar(200),
    first_name varchar(200),
    last_name varchar(200),
    dob date,
    gender varchar(200),
    password varchar(1000),
    CONSTRAINT "_user_pkey" PRIMARY KEY (user_id)
);

CREATE TABLE UserAsset
(
    user_id varchar(200) NOT NULL,
    asset_id varchar(200) NOT NULL,
    CONSTRAINT user_fkid FOREIGN KEY (user_id)
        REFERENCES "User" (user_id) 
        ON UPDATE CASCADE
        ON DELETE CASCADE
)