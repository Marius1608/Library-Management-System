using Project1.Models;
using Project1.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project1.Services
{
    public class LoanService
    {
        private readonly IRepository<Loan> _loanRepository;
        private readonly BookService _bookService;

        public LoanService(IRepository<Loan> loanRepository, BookService bookService)
        {
            _loanRepository = loanRepository;
            _bookService = bookService;
        }

        public async Task<IEnumerable<Loan>> GetAllLoansAsync()
        {
            return await _loanRepository.GetAllAsync();
        }

        public async Task<Loan> GetLoanByIdAsync(Guid id)
        {
            return await _loanRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Loan>> GetActiveLoansAsync()
        {
            return await _loanRepository.FindAsync(l => !l.IsReturned);
        }

        public async Task<IEnumerable<Loan>> GetLoansByBookIdAsync(Guid bookId)
        {
            return await _loanRepository.FindAsync(l => l.BookId == bookId);
        }

        public async Task<IEnumerable<Loan>> GetLoansByBorrowerNameAsync(string borrowerName)
        {
            if (string.IsNullOrWhiteSpace(borrowerName))
            {
                return await GetAllLoansAsync();
            }

            borrowerName = borrowerName.ToLower();
            return await _loanRepository.FindAsync(l => l.BorrowerName.ToLower().Contains(borrowerName));
        }

        public async Task<(bool Success, string Message, Loan Loan)> BorrowBookAsync(Guid bookId, string borrowerName, string borrowerContact)
        {
            
            var book = await _bookService.GetBookByIdAsync(bookId);
            if (book == null)
            {
                return (false, "Book not found.", null);
            }

            if (book.AvailableQuantity <= 0)
            {
                return (false, "No copies of this book are currently available.", null);
            }

            var loan = new Loan(bookId, borrowerName, borrowerContact);

            await _bookService.DecrementBookQuantityAsync(bookId);

            await _loanRepository.AddAsync(loan);

            return (true, "Book borrowed successfully.", loan);
        }

        public async Task<(bool Success, string Message)> ReturnBookAsync(Guid loanId)
        {
            var loan = await _loanRepository.GetByIdAsync(loanId);
            if (loan == null)
            {
                return (false, "Loan record not found.");
            }

            if (loan.IsReturned)
            {
                return (false, "This book has already been returned.");
            }

            loan.ReturnBook();
            await _loanRepository.UpdateAsync(loan);

            await _bookService.IncrementBookQuantityAsync(loan.BookId);

            return (true, "Book returned successfully.");
        }

        public async Task<IEnumerable<Loan>> GetOverdueLoansAsync(int daysOverdue = 14)
        {
            var cutoffDate = DateTime.Now.AddDays(-daysOverdue);
            return await _loanRepository.FindAsync(l => !l.IsReturned && l.BorrowDate < cutoffDate);
        }
    }
}