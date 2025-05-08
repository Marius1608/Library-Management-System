using System;

namespace LibraryManagement.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public int TotalQuantity { get; set; }
        public int AvailableQuantity { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Publisher { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }

        public Book()
        {
            Id = Guid.NewGuid();
        }

        public Book(string title, string author, string isbn, int quantity, DateTime publicationDate,
                   string publisher, string genre, string description = "")
        {
            Id = Guid.NewGuid();
            Title = title;
            Author = author;
            ISBN = isbn;
            TotalQuantity = quantity;
            AvailableQuantity = quantity;
            PublicationDate = publicationDate;
            Publisher = publisher;
            Genre = genre;
            Description = description;
        }
    }
}