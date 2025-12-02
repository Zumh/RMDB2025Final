/*
FILE        : BookStore.sql
PROJECT     : A3 | SENG2031 - Relational Database - Assignment #3
PROGRAMMER  : Zumhliansang Lung Ler | Sungmin Leem | Nick Turco
VERSION     : 12/02/2025
DESCRIPTION : This file is based on the bookstore ERD, it contain SQL DDL statements to create the database schema.    
			The file contain all necessary primary keys, foreign keys, and integrity constraints for bookstore database.  
*/

CREATE DATABASE bookstore;

Use bookstore;

-- Table for customer data
CREATE TABLE customer (
	id INTEGER AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL,
    address VARCHAR(255) NOT NULL,
    phoneNumber VARCHAR(255) NOT NULL
);

-- table for publisher info
CREATE TABLE publisher (
    id INT AUTO_INCREMENT PRIMARY KEY,
    publisherName VARCHAR(255) NOT NULL
);

-- table for category info
CREATE TABLE category (
    id INT AUTO_INCREMENT PRIMARY KEY,
    categoryName VARCHAR(255) NOT NULL
);

-- table for book info
CREATE TABLE book (
    id INT AUTO_INCREMENT PRIMARY KEY,
    publisherID INT NOT NULL,
    categoryID INT NOT NULL,
    title VARCHAR(255) NOT NULL,
    isbn VARCHAR(255) NOT NULL,
    price DOUBLE,
    stock INT,
    
    FOREIGN KEY (publisherID) REFERENCES publisher(id),
	FOREIGN KEY (categoryID) REFERENCES category(id)
);



-- table for order details
CREATE TABLE `Order` (
	id INTEGER AUTO_INCREMENT PRIMARY KEY,
    customerID INT NOT NULL,
    orderDate DATE,
    totalAmount DOUBLE DEFAULT 0.00,
    FOREIGN KEY (customerID) REFERENCES customer(id)
);

-- table for orderdetails
CREATE TABLE OrderDetail (
    id INTEGER AUTO_INCREMENT,        
    orderID INTEGER NOT NULL,         
    bookID INTEGER NOT NULL,          
    quantity INT NOT NULL,
    unitPrice DOUBLE NOT NULL,
    PRIMARY KEY(id),                   
    CONSTRAINT fk_order FOREIGN KEY (orderID) REFERENCES `order`(id),
    CONSTRAINT fk_book FOREIGN KEY (bookID) REFERENCES book(id)
);

-- default data for testing purposes

-- adding customers
INSERT INTO customer VALUES (
1,
"John Doe",
"JohnDoe@email.com",
"123 round St, ON",
"905-456-6789"
);
INSERT INTO customer VALUES (
2,
"Peter Parker",
"parkerP@gmail.com",
"132 square St, ON",
"905-876-6349"
);

INSERT INTO customer VALUES (
3,
"Jen James",
"Jamesss@email.com",
"456 circle St, ON",
"225-234-1783"
);

-- adding publishers
INSERT INTO publisher VALUE (
1,
"Big Books"
);
INSERT INTO publisher VALUE (
2,
"World Wide Books"
);

INSERT INTO publisher VALUE (
3,
"Perdue Books"
);

INSERT INTO publisher VALUE (
4,
"AudioBook"
);

-- adding categories
INSERT INTO category VALUE (
1,
"Non fiction"
);
INSERT INTO category VALUE (
2,
"Science fiction"
);
INSERT INTO category VALUE (
3,
"Romance"
);
INSERT INTO category VALUE (
4,
"Business"
);
INSERT INTO category VALUE (
5,
"Self-help"
);

INSERT INTO category VALUE (
6,
"Science"
);
-- adding books

INSERT INTO book VALUE (
1, 
2,
4,
"How to start a business",
"978-3-16-148410-0",
19.99,
15
);

INSERT INTO book VALUE (
2, 
2,
2,
"Space Odyssey",
"658-3-16-675410-2",
29.99,
40
);

INSERT INTO book VALUE (
3, 
1,
1,
"MySQL for Dummies",
"375-7-26-148445-8",
14.99,
23
);

INSERT INTO book VALUE (
4, 
4,
6,
"Physics 101",
"456-3-17-172310-9",
39.99,
24
);

INSERT INTO book VALUE (
5, 
3,
5,
"How to help yourself",
"980-1-76-925110-2",
15.00,
33
);

-- adding orders
INSERT INTO `order` VALUES (
1,
1,
'2026-01-01',
14.99
);

INSERT INTO `order` VALUES (
2,
2,
'2026-02-13',
19.99
);

INSERT INTO `order` VALUES (
3,
3,
'2025-12-13',
29.99
);

-- adding order details
INSERT INTO orderDetail VALUE (
1, 
1,
3,
1,
14.99
);

INSERT INTO orderDetail VALUE (
2, 
2,
1,
1,
19.99
);
--
-- how do we save multiple books and prices in one order.!!!!!
--
-- INSERT INTO orderDetail VALUE (
-- 3, 
-- 3,
-- [5, 3], -- array??
-- 2,
-- [15.00, 14.99] -- array??
-- );

-- testing different search conditions

-- search each table
SELECT * FROM customer;
SELECT * FROM publisher;
SELECT * FROM book;
SELECT * FROM category;
SELECT * FROM `order`;
SELECT * FROM orderdetail;

-- search with conditions
SELECT * FROM book WHERE publisherID = 2;

SELECT * FROM book WHERE categoryID = 1;

SELECT title FROM book WHERE id = 1;

SELECT `name`, email 
FROM customer 
INNER JOIN `order` 
WHERE customer.id = `order`.customerID;