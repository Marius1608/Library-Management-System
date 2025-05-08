using System;

namespace Project1.Models
{
    public class Loan
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public string BorrowerName { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsReturned { get; set; }
        public string BorrowerContact { get; set; }

        public Loan()
        {
            Id = Guid.NewGuid();
            BorrowDate = DateTime.Now;
            IsReturned = false;
        }

        public Loan(Guid bookId, string borrowerName, string borrowerContact)
        {
            Id = Guid.NewGuid();
            BookId = bookId;
            BorrowerName = borrowerName;
            BorrowDate = DateTime.Now;
            IsReturned = false;
            BorrowerContact = borrowerContact;
        }

        public void ReturnBook()
        {
            if (!IsReturned)
            {
                IsReturned = true;
                ReturnDate = DateTime.Now;
            }
        }
    }
}