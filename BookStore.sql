/*
FILE        : BookStore.sql
PROJECT     : A3 | SENG2031 - Relational Database - Assignment #3
PROGRAMMER  : Zumhliansang Lung Ler | Sungmin Leem | Nick Turco
VERSION     : 2025-12-02
DESCRIPTION : This file is based on the bookstore ERD, it contain SQL DDL statements to create the database schema.    
			The file contain all necessary primary keys, foreign keys, and integrity constraints for bookstore database.  
*/
DROP DATABASE bookstore;
-- =============================
-- Create database
-- =============================
CREATE DATABASE IF NOT EXISTS bookstore;
USE bookstore;

-- =============================
-- Customer table
-- =============================
-- Purpose: To store customer information
-- Relationships: Primary entity
CREATE TABLE customer (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL,
    address VARCHAR(255) NOT NULL,
    phoneNumber VARCHAR(50) NOT NULL
);

-- =============================
-- Publisher table
-- =============================
-- Purpose: To store publisher information
-- Relationships: Referenced by Book table
CREATE TABLE publisher (
    id INT AUTO_INCREMENT PRIMARY KEY,
    publisherName VARCHAR(255) NOT NULL
);

-- =============================
-- Category table
-- =============================
-- Purpose: To store book category information
-- Relationships: Referenced by Book table
CREATE TABLE category (
    id INT AUTO_INCREMENT PRIMARY KEY,
    categoryName VARCHAR(255) NOT NULL
);

-- =============================
-- Book table
-- =============================
-- Purpose: To store book information
-- Relationships: References Publisher and Category tables
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

-- =============================
-- Order table
-- =============================
-- Purpose: To store order information
-- Relationships: References Customer table
CREATE TABLE `Order` (
    id INT AUTO_INCREMENT PRIMARY KEY,
    customerID INT NOT NULL,
    orderDate DATE NOT NULL DEFAULT (CURRENT_DATE()),
    totalAmount DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    CONSTRAINT FOREIGN KEY (customerID) REFERENCES customer(id)
);

-- =============================
-- OrderDetail table
-- =============================
-- Purpose: To store order details information
-- Relationships: References Order and Book tables
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


-- =============================
-- Sample datas and CRUD
-- =============================

-- Insert sample customers
INSERT INTO customer (name, email, address, phoneNumber) VALUES
('Alice Johnson', 'alice@example.com', '123 Maple Street', '555-1111'),
('Bob Smith', 'bob@example.com', '456 Oak Avenue', '555-2222');

-- Insert sample publishers
INSERT INTO publisher (publisherName) VALUES
('Pearson'),
('O''Reilly Media');

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


-- =============================
-- Customer CRUD 
-- =============================

-- Creat
INSERT INTO customer (name, email, address, phoneNumber)
VALUES ('Charlie Brown', 'charlie@example.com', '789 Pine Road', '555-3333');

-- Read
SELECT * FROM customer;
SELECT * FROM customer WHERE id = 1; -- by ID

-- Update
UPDATE customer
SET address = '321 Elm Street'
WHERE id = 2;

-- Delete
DELETE FROM customer
WHERE id = 2; -- Only works if no orders exist


-- =============================
-- BOOK CRUD
-- =============================

-- Create
INSERT INTO book (publisherID, categoryID, title, isbn, price, stock)
VALUES (1, 1, '1984', '9780451524935', 12.99, 8);

-- Read
SELECT * FROM book;
SELECT * FROM book WHERE categoryID = 2;

-- Update
UPDATE book
SET stock = stock + 5
WHERE id = 2;

-- Delete
DELETE FROM book
WHERE id = 3;

-- =============================
-- Orders CRUD
-- =============================

-- Creat
INSERT INTO `Order` (customerID, orderDate, totalAmount)
VALUES (1, '2025-12-03', 28.50);

-- Read
SELECT o.id, c.name, o.orderDate, o.totalAmount
FROM `Order` o
JOIN customer c ON o.customerID = c.id;

-- Update
UPDATE `Order`
SET totalAmount = 35.00
WHERE id = 1;

-- Delete
DELETE FROM `Order`
WHERE id = 1; -- order details are automatically deleted

-- =============================
-- OrderDetails CRUD
-- =============================

-- Create
INSERT INTO OrderDetail (orderID, bookID, quantity, unitPrice)
VALUES (1, 2, 1, 45.50);

-- Read
SELECT od.id, o.id AS orderID, b.title, od.quantity, od.unitPrice
FROM OrderDetail od
JOIN `Order` o ON od.orderID = o.id
JOIN book b ON od.bookID = b.id;


-- Update
UPDATE OrderDetail
SET quantity = 3
WHERE id = 1;

-- Delete
DELETE FROM OrderDetail
WHERE id = 2;




