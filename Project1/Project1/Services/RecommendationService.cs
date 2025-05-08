using LibraryManagement.Models;
using Project1.Models;
using Project1.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.Services
{
    public class RecommendationService
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<Loan> _loanRepository;

        public RecommendationService(IRepository<Book> bookRepository, IRepository<Loan> loanRepository)
        {
            _bookRepository = bookRepository;
            _loanRepository = loanRepository;
        }

        public async Task<IEnumerable<Book>> GetPersonalizedRecommendationsAsync(string borrowerName, int maxRecommendations = 5)
        {
            if (string.IsNullOrWhiteSpace(borrowerName))
            {
                return new List<Book>();
            }

     
            var userLoans = (await _loanRepository.FindAsync(l =>
                l.BorrowerName.Equals(borrowerName, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            if (!userLoans.Any())
            {
                return await GetPopularBooksAsync(maxRecommendations);
            }

            var borrowedBookIds = userLoans.Select(l => l.BookId).Distinct().ToList();

            var borrowedBooks = new List<Book>();
            foreach (var id in borrowedBookIds)
            {
                var book = await _bookRepository.GetByIdAsync(id);
                if (book != null)
                {
                    borrowedBooks.Add(book);
                }
            }

            var preferredGenres = borrowedBooks
                .Select(b => b.Genre)
                .GroupBy(g => g)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .Take(3)
                .ToList();

            var allBooks = await _bookRepository.GetAllAsync();
            var recommendations = allBooks
                .Where(b => preferredGenres.Contains(b.Genre) && !borrowedBookIds.Contains(b.Id) && b.AvailableQuantity > 0)
                .OrderByDescending(b => preferredGenres.IndexOf(b.Genre))
                .Take(maxRecommendations)
                .ToList();

            if (recommendations.Count < maxRecommendations)
            {
                var popularBooks = (await GetPopularBooksAsync(maxRecommendations * 2))
                    .Where(b => !borrowedBookIds.Contains(b.Id) && !recommendations.Any(r => r.Id == b.Id))
                    .Take(maxRecommendations - recommendations.Count);

                recommendations.AddRange(popularBooks);
            }

            return recommendations;
        }
        public async Task<IEnumerable<Book>> GetPopularBooksAsync(int count = 5)
        {
            var allLoans = await _loanRepository.GetAllAsync();

            var bookPopularity = allLoans
                .GroupBy(l => l.BookId)
                .Select(g => new { BookId = g.Key, BorrowCount = g.Count() })
                .OrderByDescending(x => x.BorrowCount)
                .Take(count)
                .ToList();

            var popularBooks = new List<Book>();
            foreach (var item in bookPopularity)
            {
                var book = await _bookRepository.GetByIdAsync(item.BookId);
                if (book != null && book.AvailableQuantity > 0)
                {
                    popularBooks.Add(book);
                }
            }

            return popularBooks;
        }
        public async Task<IEnumerable<Book>> GetSimilarBooksAsync(Guid bookId, int count = 3)
        {
            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null)
            {
                return new List<Book>();
            }

            var allBooks = await _bookRepository.GetAllAsync();

            var similarBooks = allBooks
                .Where(b => b.Id != bookId && b.AvailableQuantity > 0 &&
                           (b.Genre.Equals(book.Genre, StringComparison.OrdinalIgnoreCase) ||
                            b.Author.Equals(book.Author, StringComparison.OrdinalIgnoreCase)))
                .OrderByDescending(b =>
                    (b.Genre.Equals(book.Genre, StringComparison.OrdinalIgnoreCase) ? 1 : 0) +
                    (b.Author.Equals(book.Author, StringComparison.OrdinalIgnoreCase) ? 2 : 0))
                .Take(count)
                .ToList();

            return similarBooks;
        }
    }
}