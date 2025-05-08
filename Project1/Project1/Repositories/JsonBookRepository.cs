using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Project1.Repositories
{
    public class JsonBookRepository : IRepository<Book>
    {
        private readonly string _filePath;
        private List<Book> _books;

        public JsonBookRepository(string filePath)
        {
            _filePath = filePath;
            LoadData().Wait();
        }

        private async Task LoadData()
        {
            
            if (File.Exists(_filePath))
            {
                string jsonData = await Task.Run(() => File.ReadAllText(_filePath));
                _books = JsonSerializer.Deserialize<List<Book>>(jsonData) ?? new List<Book>();
            }
            else
            {
                _books = new List<Book>();
                await SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return _books;
        }

        public async Task<Book> GetByIdAsync(Guid id)
        {
            return _books.FirstOrDefault(b => b.Id == id);
        }

        public async Task<IEnumerable<Book>> FindAsync(Func<Book, bool> predicate)
        {
            return _books.Where(predicate);
        }

        public async Task AddAsync(Book entity)
        {
            _books.Add(entity);
            await SaveChangesAsync();
        }

        public async Task UpdateAsync(Book entity)
        {
            var existingBook = _books.FirstOrDefault(b => b.Id == entity.Id);
            if (existingBook != null)
            {
                int index = _books.IndexOf(existingBook);
                _books[index] = entity;
                await SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                _books.Remove(book);
                await SaveChangesAsync();
            }
        }

        public async Task SaveChangesAsync()
        {
            string jsonData = JsonSerializer.Serialize(_books, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            await Task.Run(() => File.WriteAllText(_filePath, jsonData));
        }
    }
}