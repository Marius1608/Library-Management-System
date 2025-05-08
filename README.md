Running Instructions - Library Management System
Setup and Configuration

Open the Solution:

Open the Project1.sln file in Visual Studio
Ensure the project loads successfully


Build the Solution:

Select Build > Build Solution from the menu
Verify that the build completes without errors


Run the Application:

Press F5 or click the Start button in Visual Studio
The application will open in a console window displaying the main menu



Application Structure
The solution follows a multi-layer architecture:

Models: Contains Book and Loan entities
Repositories: Implements data access with JSON storage
Services: Contains business logic for book and loan management
Program.cs: Implements the console interface

Data Storage
The application stores data in JSON files in a Data directory which is automatically created in the application's execution path:

books.json: Contains all book information
loans.json: Stores loan records


To test the basic requirements:

Book Management:

Add new books (Main Menu > 1 > 3)
View all books (Main Menu > 1 > 1)
Search books by title/author/genre (Main Menu > 1 > 2)
Update book information (Main Menu > 1 > 4)
Delete books (Main Menu > 1 > 5)


Loan Management:

Borrow books (Main Menu > 2 > 4)
Return books (Main Menu > 2 > 5)
View active loans (Main Menu > 2 > 2)
View overdue loans (Main Menu > 2 > 6)



Innovative Feature: Book Recommendation System
For requirement 7, I developed a comprehensive book recommendation system that enhances the library experience with three key capabilities:

Personalized Recommendations (Main Menu > 3 > 2):

The system analyzes a borrower's history to identify their preferred genres and authors
Based on this analysis, it suggests books they haven't yet read that match their preferences
If a borrower has no history, it falls back to recommending popular books


Popular Books Tracking (Main Menu > 3 > 1):

The system keeps track of borrowing frequency for all books
Generates a list of most frequently borrowed titles
Helps librarians understand collection usage patterns and reader preferences


Similar Books Discovery (Main Menu > 3 > 3):

When viewing a specific book, the system can find other titles with the same genre or author
Uses a weighted algorithm that prioritizes books that match both genre and author
Facilitates exploration of related content in the library collection



To test this innovative feature:

First add several books with varying genres and authors
Create multiple loans for different borrowers
Return some books to have a mixed history
Use the recommendation features in Menu 3 to see the recommendations generated

This recommendation system adds significant value beyond basic CRUD operations by creating a personalized discovery experience for patrons and improving collection utilization through data-driven insights.
Troubleshooting
If you encounter any issues:

If using .NET Framework version earlier than 4.7.2, the application uses Task.Run for file operations
