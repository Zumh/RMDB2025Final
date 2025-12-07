/*
FILE        : BookStore.sql
PROJECT     : A3 | SENG2031 - Relational Database - Assignment #3
PROGRAMMER  : Zumhliansang Lung Ler | Sungmin Leem | Nick Turco
VERSION     : 12/02/2025
DESCRIPTION : This file is based on the bookstore ERD, it contain SQL DDL statements to create the database schema.    
			The file contain all necessary primary keys, foreign keys, and integrity constraints for bookstore database.  
*/
DROP DATABASE bookstore;
## Create database
CREATE DATABASE IF NOT EXISTS bookstore;
USE bookstore;

###############################
## Customer table
###############################
CREATE TABLE customer (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL,
    address VARCHAR(255) NOT NULL,
    phoneNumber VARCHAR(50) NOT NULL
);

###############################
## Publisher table
###############################
CREATE TABLE publisher (
    id INT AUTO_INCREMENT PRIMARY KEY,
    publisherName VARCHAR(255) NOT NULL
);

###############################
## Category table
###############################
CREATE TABLE category (
    id INT AUTO_INCREMENT PRIMARY KEY,
    categoryName VARCHAR(255) NOT NULL
);

###############################
## Book table
###############################
CREATE TABLE book (
    id INT AUTO_INCREMENT PRIMARY KEY,
    publisherID INT NOT NULL,
    categoryID INT NOT NULL,
    title VARCHAR(255) NOT NULL,
    isbn VARCHAR(13) NOT NULL,
    price DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    stock INT NOT NULL DEFAULT 0,
    CONSTRAINT FOREIGN KEY (publisherID) REFERENCES publisher(id),
   CONSTRAINT  FOREIGN KEY (categoryID) REFERENCES category(id)
);

###############################
## Order table
###############################
CREATE TABLE `Order` (
    id INT AUTO_INCREMENT PRIMARY KEY,
    customerID INT NOT NULL,
    orderDate DATE NOT NULL DEFAULT (CURRENT_DATE()),
    totalAmount DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    CONSTRAINT FOREIGN KEY (customerID) REFERENCES customer(id)
);

###############################
## OrderDetail table
###############################
CREATE TABLE OrderDetail (
    id INT AUTO_INCREMENT PRIMARY KEY,
    orderID INT NOT NULL,
    bookID INT NOT NULL,
    quantity INT NOT NULL DEFAULT 1,
    unitPrice DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    UNIQUE KEY unique_order_book (orderID, bookID),
    CONSTRAINT fk_order FOREIGN KEY (orderID) REFERENCES `Order`(id) ON DELETE CASCADE,
    CONSTRAINT fk_book FOREIGN KEY (bookID) REFERENCES book(id)
);

################### Sample datas and CRUD ################### 
-- Insert sample customers
INSERT INTO customer (name, email, address, phoneNumber) VALUES
('Alice Johnson', 'alice@example.com', '123 Maple Street', '555-1111'),
('Bob Smith', 'bob@example.com', '456 Oak Avenue', '555-2222'),
('Ron Swanson', 'ron@example.com', '763 Circle Avenue', '555-4444'),
('Sam Lamb', 'sam@example.com', '4126 Square Crest', '532-2254');

-- Insert sample publishers
INSERT INTO publisher (publisherName) VALUES
('Pearson'),
('O\'Reilly Media');

-- Insert sample categories
INSERT INTO category (categoryName) VALUES
('Fiction'),
('Technology');

-- Insert sample books
INSERT INTO book (publisherID, categoryID, title, isbn, price, stock) VALUES
(1, 1, 'The Great Gatsby', '9780141182636', 15.99, 10),
(2, 2, 'Learning Python', '9781449355739', 45.50, 5);

-- Insert sample orders
INSERT INTO `Order` (customerID, orderDate, totalAmount) VALUES
(1, '2025-12-01', 31.98),
(2, '2025-12-02', 45.50);

-- Insert sample order details
INSERT INTO OrderDetail (orderID, bookID, quantity, unitPrice) VALUES
(1, 1, 2, 15.99),
(2, 2, 1, 45.50);

################### Customer CRUD ################### 

## Creat
INSERT INTO customer (name, email, address, phoneNumber)
VALUES ('Charlie Brown', 'charlie@example.com', '789 Pine Road', '555-3333');

## Read
SELECT * FROM customer;
SELECT * FROM customer WHERE id = 1; -- by ID

## Update
UPDATE customer
SET address = '321 Elm Street'
WHERE id = 2;

## Delete

DELETE FROM customer
WHERE id = 2; -- Only works if no orders exist

# BOOK CRUD

## Creat
INSERT INTO book (publisherID, categoryID, title, isbn, price, stock)
VALUES (1, 1, '1984', '9780451524935', 12.99, 8);

INSERT INTO book (publisherID, categoryID, title, isbn, price, stock) VALUE (
2,
2,
"Space Odyssey",
"6583166754102",
29.99,
40
);

INSERT INTO book (publisherID, categoryID, title, isbn, price, stock) VALUE (
1,
1,
"MySQL for Dummies",
"3757261484458",
14.99,
23
);

INSERT INTO book (publisherID, categoryID, title, isbn, price, stock) VALUE (
4,
6,
"Physics 101",
"4563171723109",
39.99,
24
);

INSERT INTO book (publisherID, categoryID, title, isbn, price, stock) VALUE (
3,
5,
"How to help yourself",
"9801769251102",
15.00,
33
);

INSERT INTO book (publisherID, categoryID, title, isbn, price, stock) VALUE (
2,
4,
"How to start a business",
"9783161484100",
19.99,
15
);
## Read
SELECT * FROM book;
SELECT * FROM book WHERE categoryID = 2;

## Update
UPDATE book
SET stock = stock + 5
WHERE id = 2;

## Delete
DELETE FROM book
WHERE id = 3;

################### Orders ################### 
## Creat
INSERT INTO `Order` (customerID, orderDate, totalAmount)
VALUES (1, '2025-12-03', 28.50);

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

## Read
SELECT o.id, c.name, o.orderDate, o.totalAmount
FROM `Order` o
JOIN customer c ON o.customerID = c.id;

## Update
UPDATE `Order`
SET totalAmount = 35.00
WHERE id = 1;

## Delete
DELETE FROM `Order`
WHERE id = 1; -- order details are automatically deleted

################### OrderDetails ################### 

## Create
INSERT INTO OrderDetail (orderID, bookID, quantity, unitPrice)
VALUES (1, 2, 1, 45.50);

INSERT INTO orderDetail (orderID, bookID, quantity, unitPrice) VALUE ( 
1,
3,
1,
14.99
);

INSERT INTO orderDetail (orderID, bookID, quantity, unitPrice) VALUE (
2,
1,
1,
19.99
);

## Read
SELECT od.id, o.id AS orderID, b.title, od.quantity, od.unitPrice
FROM OrderDetail od
JOIN `Order` o ON od.orderID = o.id
JOIN book b ON od.bookID = b.id;

## Update
UPDATE OrderDetail
SET quantity = 3
WHERE id = 1;

## Delete
DELETE FROM OrderDetail
WHERE id = 2;

-- adding publishers
INSERT INTO publisher (publishername) VALUE (
"Big Books"
);
INSERT INTO publisher (publishername) VALUE (
"World Wide Books"
);

INSERT INTO publisher (publishername) VALUE (
"Perdue Books"
);

INSERT INTO publisher (publishername) VALUE (
"AudioBook"
);

-- adding categories
INSERT INTO category (categoryname) VALUE (
"Non fiction"
);
INSERT INTO category (categoryname) VALUE (
"Science fiction"
);
INSERT INTO category (categoryname) VALUE (
"Romance"
);
INSERT INTO category (categoryname) VALUE (
"Business"
);
INSERT INTO category (categoryname) VALUE (
"Self-help"
);

INSERT INTO category VALUE (
"Science"
);

SELECT * FROM category;
SELECT * FROM customer;

SELECT c.`name`, c.email 
FROM customer c
INNER JOIN `order` o
WHERE c.id = o.customerID;