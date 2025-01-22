using Book.Data;
using Book.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApplication1;
using Xunit;

namespace Test;

public class BooksControllerAddTests
{
    private readonly Mock<IDataRepository> _repositoryMock;
    private readonly BooksController _controller;

    public BooksControllerAddTests()
    {
        _repositoryMock = new Mock<IDataRepository>();
        _controller = new BooksController(_repositoryMock.Object);
    }

    [Fact]
    public async Task TestAddBook()
    {
        var newBook = new Books
        {
            Title = "Мастер и Маргарита",
            Author = "Михаил Булгаков",
            ISBN = "978-5-17-069829-5",
            Year = "1967",
            Description = "Мистический роман о любви, дьяволе и Москве",
            Keywords = "дьявол, любовь"
        };

        _repositoryMock.Setup(repo => repo.SaveBooksAsync(newBook))
            .Returns(Task.CompletedTask);
        
        var result = await _controller.AddBook(newBook);
        
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(_controller.GetBookByISBN), createdResult.ActionName);
        Assert.Equal(newBook.ISBN, ((Books)createdResult.Value).ISBN);
    }
}