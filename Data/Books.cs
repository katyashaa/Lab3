using Book.Interfaces;

namespace Book.Data
{
    // Класс книги, реализующий интерфейс ISearchable
    public class Books : ISearchable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string Year { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }

        // Конструктор без параметров
        public Books() { }

        // Конструктор с параметрами
        public Books(string title, string author, string isbn, string year, string keywords, string description)
        {
            Title = title;
            Author = author;
            ISBN = isbn;
            Year = year;
            Description = description;
            Keywords = keywords;
        }

        // Переопределение метода ToString для удобного отображения информации о книге
        public override string ToString()
        {
            return $"Title: {Title}\nAuthor: {Author}\nISBN: {ISBN}\nYear: {Year}\nKeywords: {Keywords}\nDescription: {Description}\n";
        }
        
    }
}