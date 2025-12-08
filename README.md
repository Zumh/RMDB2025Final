# RMDB2025Final

* make sure to branch off from dev branch

\## A Bookstore managment UI:

### Folder structures

* Entities/
 + Customer.cs
 * Book.cs
 * Order.cs
 * OrderDetail.cs
* DataAccess/
 * DbManager.cs
 * CustomerRepository.cs
 * BookRepository.cs
 * OrderRepository.cs

## Project Phases:

* \[ ] Phase 1: Project Idea \& Use Case (5 Marks)

  * \[ ]  Choose a real-world scenario where a relational database is essential (e.g., a
    library management system, hospital management system, e-commerce site).
  * \[ ]  Create a GitHub repository and add all members to it
  * \[ ]  Make sure all members have access before you submit Phase 1 document
  * \[ ]  Follow the date and timeline for submission on eConestoga

* \[ ] Phase 2: Data Modeling \& ER Diagram (20 Marks)

  * \[ ]  Identify the key entities, attributes, and relationships in your use case.
  * \[ ]  Develop an Entity-Relationship Diagram (ERD) using appropriate diagramming
    tools.
  * \[ ]  The ERD must include all entities, their attributes, primary keys, and relationships
    between them.  
    Deliverable: ERD in PDF format.

* \[ ] Phase 3: Normalization (15 Marks)

  * \[ ]  Normalize your database to 3rd Normal Form (3NF).
  * \[ ]  Submit documentation showing the process of normalizing the database from
    1NF to 3NF.
  * \[ ]  Clearly explain how you handled redundant data and ensured that data
    dependencies are logically organized.
  * Deliverable: Normalization report.

* \[ ] Phase 4: DDL Statements (20 Marks)

  * \[ ]  Based on the ERD, write SQL DDL statements to create the database schema.
  * \[ ]  Ensure that all necessary primary keys, foreign keys, and integrity constraints are
    included.
  * Deliverable: SQL script with DDL statements to create tables.

* \[ ] Phase 5: Programmatic Access \& CRUD Operations (30 Marks)

  * \[ ]  Develop C# programs that connect to the MySQL database using the ADO.NET
  * \[ ]  Perform CRUD operations (Create, Read, Update, Delete) for the entities defined
    in your ERD.
  * \[ ]  Ensure proper error handling and transaction control.
  * \[ ]  Use the ADO.NET MySQL library to establish the connection and perform
    operations.
  * Deliverables:  C# programs with CRUD functionality.

* \[ ] Phase 6: Final Presentation \& Code Walkthrough (10 Marks)   Present
  your database project to the class. The presentation should:

  1. Explain the project use case.
  2. Walk through the ER diagram.
  3. Demonstrate CRUD operations using the C# programs.
  4. Discuss any challenges and how your team overcame them.
