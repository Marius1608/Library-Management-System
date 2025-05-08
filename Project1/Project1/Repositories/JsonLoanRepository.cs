using Project1.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Project1.Repositories
{
    public class JsonLoanRepository : IRepository<Loan>
    {
        private readonly string _filePath;
        private List<Loan> _loans;

        public JsonLoanRepository(string filePath)
        {
            _filePath = filePath;
            LoadData().Wait();
        }

        private async Task LoadData()
        {
            if (File.Exists(_filePath))
            {
                string jsonData = await Task.Run(() => File.ReadAllText(_filePath));
                _loans = JsonSerializer.Deserialize<List<Loan>>(jsonData) ?? new List<Loan>();
            }
            else
            {
                _loans = new List<Loan>();
                await SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Loan>> GetAllAsync()
        {
            return _loans;
        }

        public async Task<Loan> GetByIdAsync(Guid id)
        {
            return _loans.FirstOrDefault(l => l.Id == id);
        }

        public async Task<IEnumerable<Loan>> FindAsync(Func<Loan, bool> predicate)
        {
            return _loans.Where(predicate);
        }

        public async Task AddAsync(Loan entity)
        {
            _loans.Add(entity);
            await SaveChangesAsync();
        }

        public async Task UpdateAsync(Loan entity)
        {
            var existingLoan = _loans.FirstOrDefault(l => l.Id == entity.Id);
            if (existingLoan != null)
            {
                int index = _loans.IndexOf(existingLoan);
                _loans[index] = entity;
                await SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var loan = _loans.FirstOrDefault(l => l.Id == id);
            if (loan != null)
            {
                _loans.Remove(loan);
                await SaveChangesAsync();
            }
        }

        public async Task SaveChangesAsync()
        {
            string jsonData = JsonSerializer.Serialize(_loans, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            await Task.Run(() => File.WriteAllText(_filePath, jsonData));
        }
    }
}