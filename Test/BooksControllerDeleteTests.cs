using Book.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApplication1;

namespace Test;

public class BooksControllerDeleteTests
{
    private readonly Mock<IDataRepository> _repositoryMock;
    private readonly BooksController _controller;

    public BooksControllerDeleteTests()
    {
        _repositoryMock = new Mock<IDataRepository>();
        _controller = new BooksController(_repositoryMock.Object);
    }

    [Fact]
    public async Task DeleteBook_ShouldReturnNoContent_WhenBookExists()
    {
        var isbn = "12345";

        _repositoryMock.Setup(repo => repo.DeleteBookByIsbnAsync(isbn))
            .Returns(Task.CompletedTask);
        
        var result = await _controller.DeleteBook(isbn);
        
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteBook_DoesNotExist()
    {
        var isbn = "12345";

        _repositoryMock.Setup(repo => repo.DeleteBookByIsbnAsync(isbn))
            .ThrowsAsync(new KeyNotFoundException($"Книга с ISBN {isbn} не найдена."));
        
        var result = await _controller.DeleteBook(isbn);
        
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal($"Книга с ISBN {isbn} не найдена.", notFoundResult.Value);
    }
}