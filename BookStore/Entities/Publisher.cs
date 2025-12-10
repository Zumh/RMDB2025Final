//FILE : Publisher.cs
//PROJECT : PROG2111 Final Project
//PROGRAMMER : Zumhliansang Lung Ler | Sungmin Leem | Nick Turco
//FIRST VERSION : 03/12/2025
/*DESCRIPTION: 
This class represents the company that publishes the books.
It stores the publisher's name and other details.
*/



namespace BookStore.Entities
{
    /*
    * Class: Publisher
    * Purpose: The Publisher class has been created to represent a book publisher within
    *  the bookstore system. It contains members to track the publisher’s unique
    *  ID, name, and any additional descriptive content. This class serves as a
    *  simple data model for managing and referencing publishers in relation
    *  to books and orders.
    */

    internal class Publisher
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Content { get; set; }
    }
}
