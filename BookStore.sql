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
    FOREIGN KEY (publisherID) REFERENCES publisher(id),
    FOREIGN KEY (categoryID) REFERENCES category(id)
);

###############################
## Order table
###############################
CREATE TABLE `Order` (
    id INT AUTO_INCREMENT PRIMARY KEY,
    customerID INT NOT NULL,
    orderDate DATE NOT NULL DEFAULT (CURRENT_DATE()),
    totalAmount DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    FOREIGN KEY (customerID) REFERENCES customer(id)
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








