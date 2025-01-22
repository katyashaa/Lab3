using Book.Data;
using Book.Interfaces;
using Moq;
using WebApplication1;

namespace Test;

public class BooksUtilityTests 
{
    private readonly Mock<IDataRepository> _repositoryMock;
    private readonly BooksController _controller;

    public BooksUtilityTests()
    {
        _repositoryMock = new Mock<IDataRepository>();
        _controller = new BooksController(_repositoryMock.Object);
    }
    
    [Fact]
    public void Books_ShouldSplitKeywordsIntoList()
    {
        var book = new Books
        {
            Keywords = "test, example, book"
        };

        var expectedKeywords = new List<string> { "test", "example", "book" };
            
        var keywords = book.Keywords.Split(',').Select(k => k.Trim()).ToList();
            
        Assert.Equal(expectedKeywords, keywords);
    }

    [Fact]
    public void Books_ToString_ShouldReturnFormattedString()
    {
        // Arrange
        var book = new Books
        {
            Title = "Test Book", 
            Author = "Test Author",
            ISBN = "12345",
            Year = "2023",
            Description = "Test Description",
            Keywords = "test, example, book"
        };

        var expectedOutput =
            $"Title: Test Book\nAuthor: Test Author\nISBN: 12345\nYear: 2023\nKeywords: test, example, book\nDescription: Test Description\n";

        // Act
        var output = book.ToString();

        // Assert
        Assert.Equal(expectedOutput, output);
    }
}