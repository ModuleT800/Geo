using System;
using System.Collections.Generic;

namespace Geo
{
    // 1.
    class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }

        public Book(string title, string author, int year)
        {
            Title = title;
            Author = author;
            Year = year;
        }

        public override string ToString()
        {
            return $"{Title} - {Author} ({Year})";
        }
    }

    class BookList
    {
        private List<Book> books = new List<Book>();

        public int Count
        {
            get { return books.Count; }
        }

        public Book this[int index]
        {
            get { return books[index]; }
            set { books[index] = value; }
        }

        public void Add(Book book)
        {
            books.Add(book);
        }

        public bool Remove(string title)
        {
            Book book = Find(title);

            if (book != null)
            {
                books.Remove(book);
                return true;
            }

            return false;
        }

        public bool Contains(string title)
        {
            return Find(title) != null;
        }

        private Book Find(string title)
        {
            foreach (Book book in books)
            {
                if (book.Title.Equals(title,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return book;
                }
            }

            return null;
        }

        public void Show()
        {
            if (books.Count == 0)
            {
                Console.WriteLine("Booklist is empty.");
                return;
            }

            for (int i = 0; i < books.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {books[i]}");
            }
        }

        public static BookList operator +(BookList list, Book book)
        {
            list.Add(book);
            return list;
        }

        public static BookList operator -(BookList list, string title)
        {
            list.Remove(title);
            return list;
        }
    }

    // 2.
    class Fraction
    {
        private int numerator;
        private int denominator;

        public int Numerator
        {
            get { return numerator; }
            set { numerator = value; }
        }

        public int Denominator
        {
            get { return denominator; }
            set
            {
                if (value == 0)
                {
                    throw new ArgumentException("Denominator can't be 0.");
                }
                denominator = value;
            }
        }

        public Fraction(int numerator, int denominator)
        {
            Numerator = numerator;
            Denominator = denominator;
            Reduce();
        }

        private static int GCD(int a, int b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);

            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }

            return a == 0 ? 1 : a;
        }

        public void Reduce()
        {
            int gcd = GCD(numerator, denominator);


            numerator /= gcd;
            denominator /= gcd;

            if (denominator < 0)
            {
                numerator = -numerator;
                denominator = -denominator;
            }
        }

        public override string ToString()
        {
            return $"{numerator}/{denominator}";
        }

        public static Fraction operator +(Fraction a, Fraction b)
        {
            return new Fraction(a.numerator * b.denominator + b.numerator * a.denominator, a.denominator * b.denominator);
        }

        public static Fraction operator -(Fraction a, Fraction b)
        {
            return new Fraction(a.numerator * b.denominator - b.numerator * a.denominator, a.denominator * b.denominator);
        }

        public static Fraction operator *(Fraction a, Fraction b)
        {
            return new Fraction(a.numerator * b.numerator, a.denominator * b.denominator);
        }

        public static Fraction operator /(Fraction a, Fraction b)
        {
            if (b.numerator == 0)
            {
                throw new DivideByZeroException("Cannot divide by zero fraction.");
            }
            return new Fraction(a.numerator * b.denominator, a.denominator * b.numerator);
        }

        public static bool operator ==(Fraction a, Fraction b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }
            if (a is null || b is null)
            {
                return false;
            }
            return a.numerator * b.denominator == b.numerator * a.denominator;
        }

        public static bool operator !=(Fraction a, Fraction b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj is Fraction other)
            {
                return this == other;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(numerator, denominator);
        }
    }

    // 3.
    class USD
    {
        public decimal Amount { get; set; }

        public string CurrencyName { get; set; } = "USD";

        public decimal ExchangeRate { get; set; } = 1.00m;

        public USD(decimal amount)
        {
            Amount = amount;
        }

        public override string ToString()
        {
            return $"{Amount:F2} {CurrencyName}";
        }
    }

    class EUR
    {
        public decimal Amount { get; set; }
        public string CurrencyName { get; set; } = "EUR";
        public decimal ExchangeRate { get; set; } = 1.08m;

        public EUR(decimal amount)
        {
            Amount = amount;
        }

        public override string ToString()
        {
            return $"{Amount:F2} {CurrencyName}";
        }

        public static implicit operator USD(EUR euro)
        {
            return new USD(euro.Amount * euro.ExchangeRate);
        }

        public static implicit operator EUR(USD dollar)
        {
            return new EUR(dollar.Amount / 1.08m);
        }
    }

    class GBP
    {
        public decimal Amount { get; set; }
        public string CurrencyName { get; set; } = "GBP";
        public decimal ExchangeRate { get; set; } = 1.27m;
        public GBP(decimal amount)
        {
            Amount = amount;
        }

        public override string ToString()
        {
            return $"{Amount:F2} {CurrencyName}";
        }

        public static explicit operator USD(GBP pound)
        {
            return new USD(pound.Amount * pound.ExchangeRate);
        }

        public static explicit operator GBP(USD dollar)
        {
            return new GBP(dollar.Amount / 1.27m);
        }
    }

    class Program
    {
        static void Main()
        {
            // Task 1
            BookList list = new BookList();

            while (true)
            {
                Console.WriteLine("\n--- Booklist ---");
                Console.WriteLine("1. Add book");
                Console.WriteLine("2. Remove book");
                Console.WriteLine("3. Check a book");
                Console.WriteLine("4. Display booklist");
                Console.WriteLine("5. Get a book by index");
                Console.WriteLine("0. Exit");
                Console.Write("Choose(0-5): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Title: ");
                        string title = Console.ReadLine();

                        Console.Write("Author: ");
                        string author = Console.ReadLine();

                        Console.Write("Year: ");
                        int year = int.Parse(Console.ReadLine());

                        list = list + new Book(title, author, year);
                        Console.WriteLine("Book added.");
                        break;

                    case "2":
                        Console.Write("Book's title: ");
                        title = Console.ReadLine();

                        list = list - title;
                        Console.WriteLine("Book was removed.");
                        break;

                    case "3":
                        Console.Write("Book's removed: ");
                        title = Console.ReadLine();

                        if (list.Contains(title))
                            Console.WriteLine("Book in booklist.");
                        else
                            Console.WriteLine("Book isn't the booklist.");
                        break;

                    case "4":
                        list.Show();
                        break;

                    case "5":
                        Console.Write("Enter index(from 0): ");
                        int index = int.Parse(Console.ReadLine());

                        if (index >= 0 && index < list.Count)
                            Console.WriteLine(list[index]);
                        else
                            Console.WriteLine("Invalid index.");
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }

            // Task 2
            Fraction a = new Fraction(1, 2);
            Fraction b = new Fraction(3, 4);

            Console.WriteLine("First fraction: " + a);
            Console.WriteLine("Second fraction: " + b);

            Console.WriteLine("Addition: " + (a + b));
            Console.WriteLine("Subtraction: " + (a - b));
            Console.WriteLine("Multiplication: " + (a * b));
            Console.WriteLine("Division: " + (a / b));

            Fraction c = new Fraction(2, 4);
            Fraction d = new Fraction(1, 2);

            Console.WriteLine("Are fractions equal? " + (c == d));
            Console.WriteLine("Are fractions not equal? " + (c != d));

            // Task 3
            USD dollar = new USD(100);

            Console.WriteLine("Initial amount: " + dollar);

            EUR euro = dollar;
            Console.WriteLine("\nImplicit conversion: " + euro);
            USD dollar2 = euro;
            Console.WriteLine(dollar2);

            GBP pound = (GBP)dollar;
            Console.WriteLine("\nExplicit conversion: " + pound);
            USD dollar3 = (USD)pound;
            Console.WriteLine(dollar3);
        }
    }
}
