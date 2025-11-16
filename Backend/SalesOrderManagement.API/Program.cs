using Microsoft.EntityFrameworkCore;
using SalesOrderManagement.Application.Interfaces;
using SalesOrderManagement.Application.Services;
using SalesOrderManagement.Infrastructure.Data;
using SalesOrderManagement.Infrastructure.Repositories;
using SalesOrderManagement.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add repositories
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<ISalesOrderRepository, SalesOrderRepository>();

// Add services
builder.Services.AddScoped<ISalesOrderService, SalesOrderService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder => builder
            .WithOrigins("http://localhost:3000") // React development server
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowReactApp");

app.UseAuthorization();

app.MapControllers();

// Auto-migrate database on startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate(); // Apply any pending migrations
    
    // Seed data only if tables are empty
    SeedDataIfEmpty(context);
}

app.Run();

static void SeedDataIfEmpty(ApplicationDbContext context)
{
    // Only seed if no data exists to avoid duplicates
    if (!context.Clients.Any())
    {
        var clients = new[]
        {
            new Client { CustomerName = "John Smith", Address1 = "123 Main St", Address2 = "Suite 100", Suburb = "Downtown", State = "NY", PostCode = "10001" },
            new Client { CustomerName = "Sarah Johnson", Address1 = "456 Oak Ave", Suburb = "Westside", State = "CA", PostCode = "90210" },
            new Client { CustomerName = "Mike Davis", Address1 = "789 Pine Rd", Address2 = "Apt 5B", Suburb = "Easthill", State = "TX", PostCode = "75001" },
            new Client { CustomerName = "Emily Wilson", Address1 = "321 Elm St", Suburb = "Northtown", State = "FL", PostCode = "33101" },
            new Client { CustomerName = "David Brown", Address1 = "654 Maple Dr", Suburb = "Southpark", State = "WA", PostCode = "98101" }
        };
        context.Clients.AddRange(clients);
        context.SaveChanges();
    }

    if (!context.Items.Any())
    {
        var items = new[]
        {
            new Item { ItemCode = "LAP001", Description = "Dell Laptop 15-inch", Price = 899.99m },
            new Item { ItemCode = "MOU001", Description = "Wireless Mouse", Price = 29.99m },
            new Item { ItemCode = "KEY001", Description = "Mechanical Keyboard", Price = 89.99m },
            new Item { ItemCode = "MON001", Description = "27-inch Monitor", Price = 299.99m },
            new Item { ItemCode = "HDD001", Description = "External Hard Drive 1TB", Price = 79.99m },
            new Item { ItemCode = "SPK001", Description = "Bluetooth Speakers", Price = 59.99m },
            new Item { ItemCode = "CAM001", Description = "Webcam HD", Price = 49.99m },
            new Item { ItemCode = "CHR001", Description = "USB-C Charger", Price = 39.99m },
            new Item { ItemCode = "TAB001", Description = "Graphics Tablet", Price = 199.99m },
            new Item { ItemCode = "PRT001", Description = "Wireless Printer", Price = 149.99m }
        };
        context.Items.AddRange(items);
        context.SaveChanges();
    }
}