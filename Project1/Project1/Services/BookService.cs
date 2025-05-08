using LibraryManagement.Models;
using Project1.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project1.Services
{
    public class BookService
    {
        private readonly IRepository<Book> _bookRepository;

        public BookService(IRepository<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _bookRepository.GetAllAsync();
        }

        public async Task<Book> GetBookByIdAsync(Guid id)
        {
            return await _bookRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllBooksAsync();
            }

            searchTerm = searchTerm.ToLower();
            return await _bookRepository.FindAsync(b =>
                b.Title.ToLower().Contains(searchTerm) ||
                b.Author.ToLower().Contains(searchTerm) ||
                b.ISBN.ToLower().Contains(searchTerm) ||
                b.Genre.ToLower().Contains(searchTerm) ||
                b.Publisher.ToLower().Contains(searchTerm));
        }

        public async Task<IEnumerable<Book>> SearchBooksByAuthorAsync(string author)
        {
            if (string.IsNullOrWhiteSpace(author))
            {
                return await GetAllBooksAsync();
            }

            author = author.ToLower();
            return await _bookRepository.FindAsync(b => b.Author.ToLower().Contains(author));
        }

        public async Task<IEnumerable<Book>> SearchBooksByTitleAsync(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return await GetAllBooksAsync();
            }

            title = title.ToLower();
            return await _bookRepository.FindAsync(b => b.Title.ToLower().Contains(title));
        }

        public async Task<IEnumerable<Book>> SearchBooksByGenreAsync(string genre)
        {
            if (string.IsNullOrWhiteSpace(genre))
            {
                return await GetAllBooksAsync();
            }

            genre = genre.ToLower();
            return await _bookRepository.FindAsync(b => b.Genre.ToLower().Contains(genre));
        }

        public async Task<Book> AddBookAsync(Book book)
        {
            await _bookRepository.AddAsync(book);
            return book;
        }

        public async Task UpdateBookAsync(Book book)
        {
            await _bookRepository.UpdateAsync(book);
        }

        public async Task DeleteBookAsync(Guid id)
        {
            await _bookRepository.DeleteAsync(id);
        }

        public async Task<bool> DecrementBookQuantityAsync(Guid bookId)
        {
            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null || book.AvailableQuantity <= 0)
            {
                return false;
            }

            book.AvailableQuantity--;
            await _bookRepository.UpdateAsync(book);
            return true;
        }

        public async Task<bool> IncrementBookQuantityAsync(Guid bookId)
        {
            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null || book.AvailableQuantity >= book.TotalQuantity)
            {
                return false;
            }

            book.AvailableQuantity++;
            await _bookRepository.UpdateAsync(book);
            return true;
        }
    }
}