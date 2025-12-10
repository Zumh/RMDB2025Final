//FILE : CategoryRepository.cs
//PROJECT : PROG2111 Final Project
//PROGRAMMER : Zumhliansang Lung Ler | Sungmin Leem | Nick Turco
//FIRST VERSION : 03/12/2025
/*DESCRIPTION: 
This class manages category data in the database.
It is used to get all categories and create default ones if needed.
*/

using System;
using System.Collections.Generic;
using System.Data;

namespace BookStore.DataAccess
{
    /*
* Class: CategoryRepository
* Purpose: The CategoryRepository class has been created to manage all database
 *  operations related to book categories within the bookstore system. It
 *  provides methods to retrieve all categories, determine the correct
 *  column names dynamically, and seed default categories into the database
 *  if they are missing. This class ensures consistent access and management
 *  of category data while handling variations in database schema.
 */

    public class CategoryRepository
    {
        private DBManager db = new DBManager();

        //NAME: GetNameColumn
        //DESCRIPTION: Finds the correct name for the category column in the table.
        //PARAMETERS: DataTable dt
        //RETURN: "name"
        private string GetNameColumn(DataTable dt)
        {
            if (dt.Columns.Contains("name")) return "name";
            if (dt.Columns.Contains("CategoryName")) return "CategoryName";
            if (dt.Columns.Contains("categoryName")) return "categoryName";
            if (dt.Columns.Contains("Title")) return "Title";
            if (dt.Columns.Contains("title")) return "title";
            if (dt.Columns.Contains("Genre")) return "Genre";
            if (dt.Columns.Contains("genre")) return "genre";
            
            // If we can't find it, return the second column as a fallback if it exists (assuming ID is first)
            if (dt.Columns.Count >= 2) return dt.Columns[1].ColumnName;
            
            return "name"; // Default failure
        }

        //NAME: GetAll
        //DESCRIPTION: Gets all categories from the database.
        //PARAMETERS: None.
        //RETURN: list
        public List<Category> GetAll()
        {
            List<Category> list = new List<Category>();
            // Assuming table name is 'category' based on convention used in other repos
            DataTable data = db.DataBaseQuery("SELECT * FROM category");
            if (data != null && data.Rows.Count > 0)
            {
                string nameCol = GetNameColumn(data);
                
                foreach (DataRow row in data.Rows)
                {
                    list.Add(new Category
                    {
                        Id = Convert.ToInt32(row["id"]),
                        Name = row[nameCol]?.ToString() ?? "Unknown",
                        // Safe check for description if it exists
                        Description = row.Table.Columns.Contains("description") && row["description"] != DBNull.Value 
                                     ? row["description"]?.ToString() 
                                     : ""
                    });
                }
            }
            return list;
        }

        //NAME: SeedCategories
        //DESCRIPTION: Adds default categories to the database if they are missing.
        //PARAMETERS: None
        //RETURN: void
        public void SeedCategories()
        {
            // Check if any categories exist
            string countQuery = "SELECT * FROM category LIMIT 1";
            DataTable dt = db.DataBaseQuery(countQuery);
            
            if (dt == null) return;
            
            string nameCol = GetNameColumn(dt);

            // Seed data
            List<string> defaults = new List<string> 
            { 
                "Fiction", "Non-Fiction", "Sci-Fi", "Mystery", 
                "Fantasy", "Biography", "History", "Romance", 
                "Thriller", "Technology" 
            };

            foreach (var name in defaults)
            {
                // Check if this specific category exists
                string checkQuery = $"SELECT count(*) FROM category WHERE {nameCol} = '{name}'";
                DataTable checkDt = db.DataBaseQuery(checkQuery);
                if (checkDt.Rows.Count > 0 && Convert.ToInt32(checkDt.Rows[0][0]) > 0)
                {
                    continue; // Exist, skip
                }

                // We use dynamic column name
                string insertQuery = $"INSERT INTO category ({nameCol}, description) VALUES ('{name}', 'Default Category')";
                
                // If the column is not 'description' and we are assuming description exists, we might fail if description doesn't exist.
                // Let's check description too
                if (!dt.Columns.Contains("description"))
                {
                     insertQuery = $"INSERT INTO category ({nameCol}) VALUES ('{name}')";
                }
                
                db.ExecuteNonQuery(insertQuery);
            }
        }
    }
}
