using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Book.Data;

public class BookContext : DbContext
{
    public BookContext(DbContextOptions<BookContext> options) : base(options)
    {
    }

    public BookContext()
    {
    }

    // Набор данных для хранения информации о книгах
    public DbSet<Books> Books { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Конфигурация Books
        modelBuilder.Entity<Books>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Title).IsRequired().HasMaxLength(255);
            entity.Property(t => t.Author).IsRequired().HasMaxLength(255);
            entity.Property(t => t.Description).HasMaxLength(1000);
            entity.Property(t => t.ISBN).IsRequired().HasMaxLength(255);
            entity.Property(t => t.Year).IsRequired().HasMaxLength(5);
            entity.Property(b => b.Keywords).HasColumnType("text");
        });
    }
    
    public static BookContext Init()
    {
        try
        {
            // Устанавливаем путь к папке, где находится исполняемый файл
            var basePath = AppContext.BaseDirectory;

            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath) // Используем BaseDirectory
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);

            var configuration = builder.Build(); // Компилируем конфигурацию

            // Создаём коллекцию сервисов для настройки зависимостей
            var services = new ServiceCollection();

            // Добавляем DbContext
            services.AddDbContext<BookContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });

            // Создаём провайдер сервисов
            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider.GetRequiredService<BookContext>();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Ошибка при инициализации базы данных: " + ex.Message, ex);
        }
    }

}