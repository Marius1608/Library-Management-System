using LibraryManagement.Models;
using Project1.Models;
using Project1.Repositories;
using Project1.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Project1
{
    public class Program
    {
        private static BookService _bookService;
        private static LoanService _loanService;
        private static RecommendationService _recommendationService;
        private static string _dataDirectory;

        public static async Task Main(string[] args)
        {
            SetupApplication();
            await DisplayMenu();
        }

        private static void SetupApplication()
        {
            Console.WriteLine("Library Management System");
            Console.WriteLine("-------------------------");

            _dataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            if (!Directory.Exists(_dataDirectory))
            {
                Directory.CreateDirectory(_dataDirectory);
            }

            var bookRepository = new JsonBookRepository(Path.Combine(_dataDirectory, "books.json"));
            var loanRepository = new JsonLoanRepository(Path.Combine(_dataDirectory, "loans.json"));

            _bookService = new BookService(bookRepository);
            _bookService = new BookService(bookRepository);
            _loanService = new LoanService(loanRepository, _bookService);
            _recommendationService = new RecommendationService(bookRepository, loanRepository);

            Console.WriteLine("System initialized successfully!");
            Console.WriteLine();
        }

        private static async Task DisplayMenu()
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\nMain Menu:");
                Console.WriteLine("1. Book Management");
                Console.WriteLine("2. Loan Management");
                Console.WriteLine("3. Book Recommendations");
                Console.WriteLine("0. Exit");
                Console.Write("\nEnter your choice: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine();

                    switch (choice)
                    {
                        case 1:
                            await BookManagementMenu();
                            break;
                        case 2:
                            await LoanManagementMenu();
                            break;
                        case 3:
                            await RecommendationMenu();
                            break;
                        case 0:
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }

            Console.WriteLine("Thank you for using the Library Management System. Goodbye!");
        }

        private static async Task BookManagementMenu()
        {
            bool back = false;

            while (!back)
            {
                Console.WriteLine("\nBook Management Menu:");
                Console.WriteLine("1. View All Books");
                Console.WriteLine("2. Search Books");
                Console.WriteLine("3. Add New Book");
                Console.WriteLine("4. Update Book");
                Console.WriteLine("5. Delete Book");
                Console.WriteLine("0. Back to Main Menu");
                Console.Write("\nEnter your choice: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine();

                    switch (choice)
                    {
                        case 1:
                            await ViewAllBooks();
                            break;
                        case 2:
                            await SearchBooks();
                            break;
                        case 3:
                            await AddNewBook();
                            break;
                        case 4:
                            await UpdateBook();
                            break;
                        case 5:
                            await DeleteBook();
                            break;
                        case 0:
                            back = true;
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }
        }

        private static async Task LoanManagementMenu()
        {
            bool back = false;

            while (!back)
            {
                Console.WriteLine("\nLoan Management Menu:");
                Console.WriteLine("1. View All Loans");
                Console.WriteLine("2. View Active Loans");
                Console.WriteLine("3. Search Loans by Borrower");
                Console.WriteLine("4. Borrow Book");
                Console.WriteLine("5. Return Book");
                Console.WriteLine("6. View Overdue Loans");
                Console.WriteLine("0. Back to Main Menu");
                Console.Write("\nEnter your choice: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine();

                    switch (choice)
                    {
                        case 1:
                            await ViewAllLoans();
                            break;
                        case 2:
                            await ViewActiveLoans();
                            break;
                        case 3:
                            await SearchLoansByBorrower();
                            break;
                        case 4:
                            await BorrowBook();
                            break;
                        case 5:
                            await ReturnBook();
                            break;
                        case 6:
                            await ViewOverdueLoans();
                            break;
                        case 0:
                            back = true;
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }
        }

        private static async Task RecommendationMenu()
        {
            bool back = false;

            while (!back)
            {
                Console.WriteLine("\nBook Recommendation Menu:");
                Console.WriteLine("1. Get Popular Books");
                Console.WriteLine("2. Get Personalized Recommendations");
                Console.WriteLine("3. Get Similar Books");
                Console.WriteLine("0. Back to Main Menu");
                Console.Write("\nEnter your choice: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine();

                    switch (choice)
                    {
                        case 1:
                            await GetPopularBooks();
                            break;
                        case 2:
                            await GetPersonalizedRecommendations();
                            break;
                        case 3:
                            await GetSimilarBooks();
                            break;
                        case 0:
                            back = true;
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }
        }

        #region Book Management Methods
        private static async Task ViewAllBooks()
        {
            var books = await _bookService.GetAllBooksAsync();
            DisplayBooks(books);
        }

        private static async Task SearchBooks()
        {
            Console.WriteLine("Search Books:");
            Console.WriteLine("1. Search by Title");
            Console.WriteLine("2. Search by Author");
            Console.WriteLine("3. Search by Genre");
            Console.WriteLine("4. Search All Fields");
            Console.Write("\nEnter your choice: ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.Write("\nEnter search term: ");
                string searchTerm = Console.ReadLine();
                Console.WriteLine();

                IEnumerable<Book> books;
                switch (choice)
                {
                    case 1:
                        books = await _bookService.SearchBooksByTitleAsync(searchTerm);
                        break;
                    case 2:
                        books = await _bookService.SearchBooksByAuthorAsync(searchTerm);
                        break;
                    case 3:
                        books = await _bookService.SearchBooksByGenreAsync(searchTerm);
                        break;
                    case 4:
                        books = await _bookService.SearchBooksAsync(searchTerm);
                        break;
                    default:
                        books = await _bookService.GetAllBooksAsync();
                        break;
                }

                DisplayBooks(books);
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number.");
            }
        }

        private static async Task AddNewBook()
        {
            Console.WriteLine("Add New Book:");
            Console.Write("Title: ");
            string title = Console.ReadLine();

            Console.Write("Author: ");
            string author = Console.ReadLine();

            Console.Write("ISBN: ");
            string isbn = Console.ReadLine();

            Console.Write("Quantity: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity < 0)
            {
                Console.WriteLine("Invalid quantity. Book not added.");
                return;
            }

            Console.Write("Publication Date (yyyy-MM-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime publicationDate))
            {
                Console.WriteLine("Invalid date format. Book not added.");
                return;
            }

            Console.Write("Publisher: ");
            string publisher = Console.ReadLine();

            Console.Write("Genre: ");
            string genre = Console.ReadLine();

            Console.Write("Description (optional): ");
            string description = Console.ReadLine();

            var book = new Book(title, author, isbn, quantity, publicationDate, publisher, genre, description);
            await _bookService.AddBookAsync(book);

            Console.WriteLine("\nBook added successfully!");
        }

        private static async Task UpdateBook()
        {
            Console.Write("Enter the ID of the book to update: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                Console.WriteLine("Invalid ID format.");
                return;
            }

            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                Console.WriteLine("Book not found.");
                return;
            }

            DisplayBook(book);
            Console.WriteLine("\nEnter new details (leave blank to keep current value):");

            Console.Write($"Title [{book.Title}]: ");
            string title = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(title))
            {
                book.Title = title;
            }

            Console.Write($"Author [{book.Author}]: ");
            string author = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(author))
            {
                book.Author = author;
            }

            Console.Write($"ISBN [{book.ISBN}]: ");
            string isbn = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(isbn))
            {
                book.ISBN = isbn;
            }

            Console.Write($"Total Quantity [{book.TotalQuantity}]: ");
            string quantityInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(quantityInput) && int.TryParse(quantityInput, out int quantity) && quantity >= 0)
            {
                int diff = quantity - book.TotalQuantity;
                book.TotalQuantity = quantity;
                book.AvailableQuantity = Math.Max(0, book.AvailableQuantity + diff);
            }

            Console.Write($"Publication Date [{book.PublicationDate:yyyy-MM-dd}]: ");
            string dateInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(dateInput) && DateTime.TryParse(dateInput, out DateTime publicationDate))
            {
                book.PublicationDate = publicationDate;
            }

            Console.Write($"Publisher [{book.Publisher}]: ");
            string publisher = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(publisher))
            {
                book.Publisher = publisher;
            }

            Console.Write($"Genre [{book.Genre}]: ");
            string genre = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(genre))
            {
                book.Genre = genre;
            }

            Console.Write($"Description [{book.Description}]: ");
            string description = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(description))
            {
                book.Description = description;
            }

            await _bookService.UpdateBookAsync(book);
            Console.WriteLine("\nBook updated successfully!");
        }

        private static async Task DeleteBook()
        {
            Console.Write("Enter the ID of the book to delete: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                Console.WriteLine("Invalid ID format.");
                return;
            }

            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                Console.WriteLine("Book not found.");
                return;
            }

            DisplayBook(book);
            Console.Write("\nAre you sure you want to delete this book? (y/n): ");
            string confirmation = Console.ReadLine()?.ToLower();

            if (confirmation == "y" || confirmation == "yes")
            {
                await _bookService.DeleteBookAsync(id);
                Console.WriteLine("Book deleted successfully!");
            }
            else
            {
                Console.WriteLine("Deletion cancelled.");
            }
        }
        #endregion

        #region Loan Management Methods
        private static async Task ViewAllLoans()
        {
            var loans = await _loanService.GetAllLoansAsync();
            await DisplayLoans(loans);
        }

        private static async Task ViewActiveLoans()
        {
            var loans = await _loanService.GetActiveLoansAsync();
            await DisplayLoans(loans);
        }

        private static async Task SearchLoansByBorrower()
        {
            Console.Write("Enter borrower name: ");
            string borrowerName = Console.ReadLine();

            var loans = await _loanService.GetLoansByBorrowerNameAsync(borrowerName);
            await DisplayLoans(loans);
        }

        private static async Task BorrowBook()
        {
            Console.WriteLine("Borrow Book:");

            Console.Write("Search for a book (title/author/ISBN): ");
            string searchTerm = Console.ReadLine();
            var books = await _bookService.SearchBooksAsync(searchTerm);

            if (!books.Any())
            {
                Console.WriteLine("No books found matching your search.");
                return;
            }

            DisplayBooks(books);

            Console.Write("\nEnter the ID of the book to borrow: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid bookId))
            {
                Console.WriteLine("Invalid ID format.");
                return;
            }

            var book = await _bookService.GetBookByIdAsync(bookId);
            if (book == null)
            {
                Console.WriteLine("Book not found.");
                return;
            }

            if (book.AvailableQuantity <= 0)
            {
                Console.WriteLine("Sorry, this book is currently not available for borrowing.");
                return;
            }

            Console.Write("Borrower Name: ");
            string borrowerName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(borrowerName))
            {
                Console.WriteLine("Borrower name is required.");
                return;
            }

            Console.Write("Borrower Contact (Phone/Email): ");
            string borrowerContact = Console.ReadLine();

            var result = await _loanService.BorrowBookAsync(bookId, borrowerName, borrowerContact);
            Console.WriteLine(result.Message);

            if (result.Success)
            {
                Console.WriteLine($"Loan ID: {result.Loan.Id}");
                Console.WriteLine($"Borrow Date: {result.Loan.BorrowDate:yyyy-MM-dd HH:mm}");
            }
        }

        private static async Task ReturnBook()
        {
            Console.WriteLine("Return Book:");

            var activeLoans = await _loanService.GetActiveLoansAsync();
            if (!activeLoans.Any())
            {
                Console.WriteLine("There are no active loans to return.");
                return;
            }

            await DisplayLoans(activeLoans);

            Console.Write("\nEnter the ID of the loan to return: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid loanId))
            {
                Console.WriteLine("Invalid ID format.");
                return;
            }

            var result = await _loanService.ReturnBookAsync(loanId);
            Console.WriteLine(result.Message);
        }

        private static async Task ViewOverdueLoans()
        {
            Console.Write("Enter the number of days to consider as overdue (default 14): ");
            string input = Console.ReadLine();

            int days = 14;
            if (!string.IsNullOrWhiteSpace(input) && int.TryParse(input, out int inputDays))
            {
                days = inputDays;
            }

            var overdueLoans = await _loanService.GetOverdueLoansAsync(days);
            Console.WriteLine($"\nOverdue Loans (more than {days} days):");
            await DisplayLoans(overdueLoans);
        }
        
        private static async Task GetPopularBooks()
        {
            Console.Write("Enter the number of popular books to show (default 5): ");
            string input = Console.ReadLine();

            int count = 5;
            if (!string.IsNullOrWhiteSpace(input) && int.TryParse(input, out int inputCount))
            {
                count = inputCount;
            }

            var popularBooks = await _recommendationService.GetPopularBooksAsync(count);

            Console.WriteLine($"\nTop {count} Popular Books:");
            DisplayBooks(popularBooks);
        }

        private static async Task GetPersonalizedRecommendations()
        {
            Console.Write("Enter borrower name for personalized recommendations: ");
            string borrowerName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(borrowerName))
            {
                Console.WriteLine("Borrower name is required.");
                return;
            }

            Console.Write("Enter the number of recommendations to show (default 5): ");
            string input = Console.ReadLine();

            int count = 5;
            if (!string.IsNullOrWhiteSpace(input) && int.TryParse(input, out int inputCount))
            {
                count = inputCount;
            }

            var recommendations = await _recommendationService.GetPersonalizedRecommendationsAsync(borrowerName, count);

            Console.WriteLine($"\nPersonalized Book Recommendations for {borrowerName}:");
            DisplayBooks(recommendations);
        }

        private static async Task GetSimilarBooks()
        {
            Console.Write("Search for a book to find similar ones (title/author/ISBN): ");
            string searchTerm = Console.ReadLine();
            var books = await _bookService.SearchBooksAsync(searchTerm);

            if (!books.Any())
            {
                Console.WriteLine("No books found matching your search.");
                return;
            }

            DisplayBooks(books);

            Console.Write("\nEnter the ID of the book to find similar ones: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid bookId))
            {
                Console.WriteLine("Invalid ID format.");
                return;
            }

            Console.Write("Enter the number of similar books to show (default 3): ");
            string input = Console.ReadLine();

            int count = 3;
            if (!string.IsNullOrWhiteSpace(input) && int.TryParse(input, out int inputCount))
            {
                count = inputCount;
            }

            var similarBooks = await _recommendationService.GetSimilarBooksAsync(bookId, count);

            var originalBook = await _bookService.GetBookByIdAsync(bookId);
            if (originalBook != null)
            {
                Console.WriteLine($"\nBooks Similar to '{originalBook.Title}' by {originalBook.Author}:");
                DisplayBooks(similarBooks);
            }
            else
            {
                Console.WriteLine("Original book not found.");
            }
        }
        
        private static void DisplayBooks(IEnumerable<Book> books)
        {
            if (!books.Any())
            {
                Console.WriteLine("No books found.");
                return;
            }

            Console.WriteLine("Books:");
            Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine(string.Format("{0,-36} | {1,-30} | {2,-20} | {3,-15} | {4,-10}", "ID", "Title", "Author", "Available/Total", "Genre"));
            Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------");

            foreach (var book in books)
            {
                Console.WriteLine(string.Format("{0,-36} | {1,-30} | {2,-20} | {3,-15} | {4,-10}",
                    book.Id,
                    book.Title.Length > 30 ? book.Title.Substring(0, 27) + "..." : book.Title,
                    book.Author.Length > 20 ? book.Author.Substring(0, 17) + "..." : book.Author,
                    $"{book.AvailableQuantity}/{book.TotalQuantity}",
                    book.Genre));
            }
            Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------");
        }

        private static void DisplayBook(Book book)
        {
            Console.WriteLine($"Book Details:");
            Console.WriteLine($"ID: {book.Id}");
            Console.WriteLine($"Title: {book.Title}");
            Console.WriteLine($"Author: {book.Author}");
            Console.WriteLine($"ISBN: {book.ISBN}");
            Console.WriteLine($"Quantity: {book.AvailableQuantity}/{book.TotalQuantity}");
            Console.WriteLine($"Publication Date: {book.PublicationDate:yyyy-MM-dd}");
            Console.WriteLine($"Publisher: {book.Publisher}");
            Console.WriteLine($"Genre: {book.Genre}");
            Console.WriteLine($"Description: {book.Description}");
        }

        private static async Task DisplayLoans(IEnumerable<Loan> loans)
        {
            if (!loans.Any())
            {
                Console.WriteLine("No loans found.");
                return;
            }

            Console.WriteLine("Loans:");
            Console.WriteLine("----------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine(string.Format("{0,-36} | {1,-25} | {2,-30} | {3,-20} | {4,-10}",
                "ID", "Book Title", "Borrower", "Borrow Date", "Status"));
            Console.WriteLine("----------------------------------------------------------------------------------------------------------------------------------");

            foreach (var loan in loans)
            {
                var book = await _bookService.GetBookByIdAsync(loan.BookId);
                string bookTitle = book != null ? book.Title : "Unknown Book";

                Console.WriteLine(string.Format("{0,-36} | {1,-25} | {2,-30} | {3,-20} | {4,-10}",
                    loan.Id,
                    bookTitle.Length > 25 ? bookTitle.Substring(0, 22) + "..." : bookTitle,
                    loan.BorrowerName.Length > 30 ? loan.BorrowerName.Substring(0, 27) + "..." : loan.BorrowerName,
                    loan.BorrowDate.ToString("yyyy-MM-dd HH:mm"),
                    loan.IsReturned ? $"Returned on {loan.ReturnDate?.ToString("yyyy-MM-dd")}" : "Active"));
            }
            Console.WriteLine("----------------------------------------------------------------------------------------------------------------------------------");
        }
        #endregion
    }
}