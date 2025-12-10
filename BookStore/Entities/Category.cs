

//FILE : Category.cs
//PROJECT : PROG2111 Final Project
//PROGRAMMER : Zumhliansang Lung Ler | Sungmin Leem | Nick Turco
//FIRST VERSION : 03/12/2025
/*DESCRIPTION: 
This class represents a book category.
It is used to group books into different types like Fiction or History.
*/

namespace BookStore
{
    /*
* Class: Category
* Purpose: The Category class has been created to represent a book category within
 *  the bookstore system. It contains members to track the category’s unique
 *  ID, name, and description. This class is used to organize books into
 *  meaningful groups, facilitating classification, filtering, and display
 *  of books based on their category.
 */

    public class Category
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
