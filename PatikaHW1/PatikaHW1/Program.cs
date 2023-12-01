using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

// Interface for printing properties of different classes.
interface IPrintable
{
    void printProperties();
}

// Generic literature class contains the properties of a usual literature piece.
abstract class Literature
{
    public string Name;
    public string Author;
    public int PublicationYear;
}

// Book class inherited from Literature class with addition of ID property, and also
// inherited from IPrintable interface for the printProperties method.
class Book : Literature, IPrintable
{
    static public int BookCount = 0; // Total book count in space, to make ID's of all books unique.
    public int ID { get; }
    public bool IsBorrowed;
    public int DaysOfBorrow;

    public Book(string _name, string _author, int _publicationYear)
    {
        Book.BookCount++;
        this.Name = _name;
        this.Author = _author;
        this.PublicationYear = _publicationYear;
        this.ID = Book.BookCount;
        this.IsBorrowed = false;
        this.DaysOfBorrow = 0;
    }

    public void printProperties()
    {
        Console.WriteLine("Information about Book #" + this.ID + ":");
        Console.WriteLine("Name: " + this.Name);
        Console.WriteLine("Author: " + this.Author);
        Console.WriteLine("Publication Year: " + this.PublicationYear);
    }
}

// Generic member class that holds the usual properties of a library member, inherited from
// IPrintable interface for the printProperties method.
class Member : IPrintable
{
    static public int MemberCount = 0; // Total member count in space, to make ID's of all members unique.
    public string Name { get; }
    public string Surname { get; }
    public int ID { get; }
    public List<Book> BorrowedBooks;

    public Member(string _name, string _surname)
    {
        Member.MemberCount++;
        this.Name = _name;
        this.Surname = _surname;
        this.ID = Member.MemberCount;
        BorrowedBooks = new List<Book>();
    }

    public void printProperties()
    {
        Console.WriteLine("Information about Member #" + this.ID + ":");
        Console.WriteLine("Name: " + this.Name);
        Console.WriteLine("Surname: " + this.Surname);
        Console.WriteLine("Borrowed Books: ");
        for(int i = 0; i < BorrowedBooks.Count; i++)
        {
            Console.WriteLine(BorrowedBooks[i]);
        }
    }
}

// Library class holds Book and Member objects and performs borrow, return, BookInventory and RegisteredMembers
// operations, inherited from the IPrintable interface to print it's BookInventory and RegisteredMembers
class Library : IPrintable
{
    public List<Book> BookInventory;
    public List<Member> RegisteredMembers;

    public Library()
    {
        BookInventory = new List<Book>();
        RegisteredMembers = new List<Member>();
    }

    // Method do add a book to the library's inventory
    public void addBookToInventory(Book _book)
    {
        if ((BookInventory.Find(Book => Book.ID == _book.ID)) == null)
        {
            BookInventory.Add(_book);
            Console.WriteLine("Added the book " + _book.Name + " to the inventory.");
        }
        else
        {
            Console.WriteLine("This book aldready exists in the inventory of the library!");
        }
    }

    // Method do remove a book to the library's inventory
    public void removeBookFromInventory(Book _book)
    {
        if ((BookInventory.Find(Book => Book.ID == _book.ID)) != null)
        {
            BookInventory.Remove(_book);
            Console.WriteLine("Removed the book " + _book.Name + " from the inventory.");
        }
        else
        {
            Console.WriteLine("This book doesn't exist in the inventory of the library, you cannot remove it!");
        }
    }


    public void registerMemberToLibrary(Member _member)
    {
        if ((RegisteredMembers.Find(Member => Member.ID == _member.ID)) == null)
        {
            RegisteredMembers.Add(_member);
            Console.WriteLine("Registered the member " + _member.Name + " " + _member.Surname + " to the library.");
        }
        else
        {
            Console.WriteLine("This member is already registered to the library!");
        }
    }

    public void removeMemberFromLibrary(Member _member)
    {
        if ((RegisteredMembers.Find(Member => Member.ID == _member.ID)) != null)
        {
            RegisteredMembers.Remove(_member);
            Console.WriteLine("Removed the member " + _member.Name + " " + _member.Surname + " from the library.");
        }
        else
        {
            Console.WriteLine("This member is not registered to the library, you cannot remove him/her!");
        }
    }

    public void printProperties()
    {
        Console.WriteLine("Member count of this library: " + RegisteredMembers.Count);
        Console.WriteLine("Book count of this library: " + BookInventory.Count);
    }

    // If book to be borrowed exists in the library inventory and is not borrowed by another member, and the given member with
    // the _memberID is registered to library, let member with the specified ID borrow the book by it's name
    public void borrowBook(string _bookName, int _memberID)
    {
        int bookToBorrowIndex = BookInventory.FindIndex(Book => Book.Name == _bookName);
        bool memberCheck = false;

        // If given _memberID is registered to the library, set memberCheck to true
        if (RegisteredMembers.Find(Member => Member.ID == _memberID) != null)
        {
            memberCheck = true;
        }

        // If all conditions met in the function description, let the borrow operation be
        if (BookInventory[bookToBorrowIndex] != null && BookInventory[bookToBorrowIndex].IsBorrowed != true && memberCheck == true)
        {
            RegisteredMembers.Find(Member => Member.ID == _memberID).BorrowedBooks.Add(BookInventory[bookToBorrowIndex]);
            BookInventory[bookToBorrowIndex].IsBorrowed = true;
            Console.WriteLine("Book " + BookInventory[bookToBorrowIndex].Name + "successfully borrowed by member #" + _memberID);
        }

        // If the library inventory doesn't include the book, throw an error message
        else if (BookInventory[bookToBorrowIndex] == null)
        {
            Console.WriteLine("The library doesn't have this book in it's inventory!");
        }

        // If the member is not registered to the library, throw an error message
        else if (memberCheck == false)
        {
            Console.WriteLine("This member is not registered to the library!");
        }

        // If the book is borrowed by another member, throw an error message based on it's days of borrow
        else if (BookInventory[bookToBorrowIndex].IsBorrowed)
        {
            if (BookInventory[bookToBorrowIndex].DaysOfBorrow == 0)
            {
                Console.WriteLine("This book is borrowed by another member indefinitely, don't expect it to be back soon...");
            }
            else
            {
                Console.WriteLine("This book is borrowed by another member for " + BookInventory[bookToBorrowIndex].DaysOfBorrow + " days.");
            }
        }
    }

    // Same with borrowBook, but instead of borrowing the book by it's name, borrow it by bookID
    public void borrowBook(int _bookID, int _memberID)
    {
        int bookToBorrowIndex = BookInventory.FindIndex(Book => Book.ID == _bookID);
        bool memberCheck = false;

        if (RegisteredMembers.Find(Member => Member.ID == _memberID) != null)
        {
            memberCheck = true;
        }

        if (bookToBorrowIndex != -1 && BookInventory[bookToBorrowIndex].IsBorrowed != true && memberCheck == true)
        {
            RegisteredMembers.Find(Member => Member.ID == _memberID).BorrowedBooks.Add(BookInventory[bookToBorrowIndex]);
            BookInventory[bookToBorrowIndex].IsBorrowed = true;
            Console.WriteLine("Book #" + BookInventory[bookToBorrowIndex].ID + "successfully borrowed by member #" + _memberID);
        }

        else if (bookToBorrowIndex == -1)
        {
            Console.WriteLine("The library doesn't have this book in it's inventory!");
        }

        else if (memberCheck == false)
        {
            Console.WriteLine("This member is not registered to the library!");
        }
    }

    // Same with borrowBook, but instead of borrowing the book indefinitely, borrow it for x amount of days
    public void borrowBook(string _bookName, int _memberID, int _daysToBorrow)
    {
        int bookToBorrowIndex = BookInventory.FindIndex(Book => Book.Name == _bookName);
        bool memberCheck = false;

        if (RegisteredMembers.Find(Member => Member.ID == _memberID) != null)
        {
            memberCheck = true;
        }

        if (bookToBorrowIndex != -1 && BookInventory[bookToBorrowIndex].IsBorrowed != true && memberCheck == true)
        {
            RegisteredMembers.Find(Member => Member.ID == _memberID).BorrowedBooks.Add(BookInventory[bookToBorrowIndex]);
            BookInventory[bookToBorrowIndex].IsBorrowed = true;
            BookInventory[bookToBorrowIndex].DaysOfBorrow = _daysToBorrow;
            Console.WriteLine("Book #" + BookInventory[bookToBorrowIndex].ID + "successfully borrowed by member #" + _memberID);
            
        }

        else if (bookToBorrowIndex == -1)
        {
            Console.WriteLine("The library doesn't have this book in it's inventory!");
        }

        else if (memberCheck == false)
        {
            Console.WriteLine("This member is not registered to the library!");
        }
    }

    // If book to be borrowed exists in the library inventory and is already borrowed by the given member and the given member with
    // the _memberID is registered to library, let member with the specified ID borrow the book by it's name
    public void returnBook(string _bookName, int _memberID)
    {
        int bookToReturnIndex = BookInventory.FindIndex(Book => Book.Name == _bookName);
        int returningMemberIndex = RegisteredMembers.FindIndex(Member => Member.ID == _memberID);
        bool memberCheck = false;

        // If given _memberID is registered to the library, set memberCheck to true
        if (returningMemberIndex != -1)
        {
            memberCheck = true;
        }

        // If all conditions met in the function description, let the return operation be
        if (bookToReturnIndex != -1 && (RegisteredMembers[returningMemberIndex].BorrowedBooks.FindIndex(Book => Book.ID == BookInventory[bookToReturnIndex].ID) != null) && BookInventory[bookToReturnIndex].IsBorrowed == true && memberCheck == true)
        {
            RegisteredMembers.Find(Member => Member.ID == _memberID).BorrowedBooks.Remove(BookInventory[bookToReturnIndex]);
            BookInventory[bookToReturnIndex].IsBorrowed = false;
            BookInventory[bookToReturnIndex].DaysOfBorrow = 0;
            Console.WriteLine("Book #" + BookInventory[bookToReturnIndex].ID + "successfully returned by member #" + _memberID);
        }

        // If the library inventory doesn't include the book, throw an error message
        else if (bookToReturnIndex == -1)
        {
            Console.WriteLine("The library doesn't have this book in it's inventory!");
        }

        // If the member is not registered to the library, throw an error message
        else if (memberCheck == false)
        {
            Console.WriteLine("This member is not registered to the library!");
        }

        // If the member didn't borrow the book, throw an error message
        else if (RegisteredMembers[returningMemberIndex].BorrowedBooks.Find(Book => Book.ID == BookInventory[bookToReturnIndex].ID) == null)
        {
            Console.WriteLine("This member didn't borrow the given book, so he/she cannot return it!");
        }
    }

}

class Program
{
    static void Main(string[] args)
    {
        Library library = new Library();
        library.printProperties();

        Member member1 = new Member("Ali", "Cikmaz");
        member1.printProperties();
        Member member2 = new Member("Baris", "Cikmaz");
        member2.printProperties();

        Book book1 = new Book("1984", "George Orwell", 2019);
        book1.printProperties();
        Book book2 = new Book("Simyaci", "Paulo Coelho", 2021);
        book2.printProperties();

        library.addBookToInventory(book1);
        library.printProperties();
        library.addBookToInventory(book2);
        library.printProperties();
        library.removeBookFromInventory(book2);
        library.printProperties();

        library.borrowBook(book1.ID, member1.ID);

        library.registerMemberToLibrary(member1);
        library.borrowBook(book1.ID, member1.ID);

        library.registerMemberToLibrary(member2);
        library.borrowBook(book1.ID, member2.ID);

        library.borrowBook(book2.Name, member2.ID, 13);
        library.borrowBook(book2.ID, member1.ID);

        library.returnBook(book2.Name, member2.ID);
        library.returnBook(book2.Name, member1.ID);
    }
}
