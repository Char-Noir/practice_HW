CREATE TABLE Groups (
    gr_id INT IDENTITY(1,1) PRIMARY KEY,
    gr_name VARCHAR(255) NOT NULL,
    gr_temp VARCHAR(50) NOT NULL
);

CREATE TABLE Analysis (
    an_id INT IDENTITY(1,1) PRIMARY KEY,
    an_name VARCHAR(255) NOT NULL,
    an_cost DECIMAL(10,2) NOT NULL,
    an_price DECIMAL(10,2) NOT NULL,
    an_group INT NOT NULL,
    FOREIGN KEY (an_group) REFERENCES Groups(gr_id)
);

CREATE TABLE Orders (
    ord_id INT IDENTITY(1,1) PRIMARY KEY,
    ord_datetime DATETIME NOT NULL,
    ord_an INT NOT NULL,
    FOREIGN KEY (ord_an) REFERENCES Analysis(an_id)
);