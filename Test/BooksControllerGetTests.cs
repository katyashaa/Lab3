using Book.Data;
using Book.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApplication1;

namespace Test;

public class BooksControllerGetTests
{
    private readonly Mock<IDataRepository> _repositoryMock;
    private readonly BooksController _controller;

    public BooksControllerGetTests()
    {
        _repositoryMock = new Mock<IDataRepository>();
        _controller = new BooksController(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetBookByISBN_BookExists()
    {
        var isbn = "978-5-17-069829-5";
        var expectedBook = new Books
        {
            Title = "Мастер и Маргарита",
            Author = "Михаил Булгаков",
            ISBN = isbn,
            Year = "1967",
            Description = "Мистический роман о любви, дьяволе и Москве",
            Keywords = "дьявол, любовь"
        };

        _repositoryMock.Setup(repo => repo.SearchBooksByISBNAsync(isbn))
            .ReturnsAsync(new List<Books> { expectedBook });
        
        var result = await _controller.GetBookByISBN(isbn);
        
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var book = Assert.IsType<Books>(okResult.Value);
        Assert.Equal(isbn, book.ISBN);
    }

    [Fact]
    public async Task GetBookByISBN_BookDoesNotExist()
    {
        var isbn = "12345";

        _repositoryMock.Setup(repo => repo.SearchBooksByISBNAsync(isbn))
            .ReturnsAsync(new List<Books>());
        
        var result = await _controller.GetBookByISBN(isbn);
        
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal("Книга с указанным ISBN не найдена.", notFoundResult.Value);
    }
}