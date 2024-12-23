using Book.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Book.Data
{
    public class DatabaseConnection : IDataRepository
    {
        private readonly BookContext _context;
        
        public DatabaseConnection(BookContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Метод для сохранения книги в базе данных
        public async Task SaveBooksAsync(Books books)
        {
            if (books == null) throw new ArgumentNullException(nameof(books));

            try
            {
                // Проверяем, существует ли книга с таким же ISBN
                if (!await _context.Books.AnyAsync(b => b.ISBN == books.ISBN))
                {
                    await _context.Books.AddAsync(books);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving book: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                throw;
            }
        }
        
        public async Task<List<Books>> SearchBooksAsync(Func<IQueryable<Books>, IQueryable<Books>> filter)
        {
            // Выполняем запрос с переданным фильтром и возвращаем полный список книг
            return await filter(_context.Set<Books>()).ToListAsync();
        }


        // Поиск книг по названию
        public async Task<List<Books>> SearchBooksByTitleAsync(string title)
        {
            return await SearchBooksAsync(query => query.Where(b => EF.Functions.ILike(b.Title, $"%{title.Replace("%", "\\%").Replace("_", "\\_")}%")));

        }

        // Поиск книг по автору
        public async Task<List<Books>> SearchBooksByAuthorAsync(string author)
        {
            return await SearchBooksAsync(query => query.Where(b => EF.Functions.ILike(b.Author, $"%{author}%")));
        }

        // Поиск книг по ISBN
        public async Task<List<Books>> SearchBooksByISBNAsync(string isbn)
        {
            return await SearchBooksAsync(query => query.Where(b => EF.Functions.ILike(b.ISBN, isbn)));
        }

        // Поиск книг по ключевым словам
        public async Task<List<Books>> SearchBooksByKeywordAsync(string keyword)
        {
            return await SearchBooksAsync(query => query.Where(b => EF.Functions.ILike(b.Keywords, $"%{keyword}%")));
        }

        
        public async Task DeleteBookByIsbnAsync(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                throw new ArgumentException("ISBN не может быть пустым.", nameof(isbn));

            // Начало транзакции
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Поиск книги по ISBN
                var book = await _context.Books.FirstOrDefaultAsync(b => b.ISBN == isbn);

                if (book != null)
                {
                    _context.Books.Remove(book);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                else
                {
                    throw new KeyNotFoundException("Книга с указанным ISBN не найдена.");
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Error deleting book: {ex.Message}");
                
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                
                throw;
            }
        }

    }
}
