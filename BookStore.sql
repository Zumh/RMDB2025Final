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

CREATE TABLE customer (
	id INTEGER AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL,
    address VARCHAR(255) NOT NULL,
    phoneNumber VARCHAR(255) NOT NULL
);

CREATE TABLE publisher (
    id INT AUTO_INCREMENT PRIMARY KEY,
    publisherName VARCHAR(255) NOT NULL
);

CREATE TABLE category (
    id INT AUTO_INCREMENT PRIMARY KEY,
    categoryName VARCHAR(255) NOT NULL
);

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


CREATE TABLE `Order` (
	id INTEGER AUTO_INCREMENT PRIMARY KEY,
    customerID INT NOT NULL,
    orderDate DATE,
    totalAmount DOUBLE,
    FOREIGN KEY (customerID) REFERENCES customer(id)
);

CREATE TABLE OrderDetail (
	id INTEGER AUTO_INCREMENT PRIMARY KEY,
	orderID INT NOT NULL, 
    bookID INT NOT NULL,
    quantity INT NOT NULL,
    unitPrice DOUBLE NOT NULL,
    FOREIGN KEY (orderID) REFERENCES `order`(id),
    FOREIGN KEY (bookID) REFERENCES book(id)
    
);



