using Microsoft.EntityFrameworkCore;
using AutocompleteDemo.Data;
using AutocompleteDemo.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Додаємо контекст бази даних
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Додаємо сідування даних
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    
    // Переконуємося що база даних створена
    context.Database.EnsureCreated();
    
    // Перевіряємо чи є дані
    if (!context.Items.Any())
    {
        // Додаємо тестові дані
        var items = new List<Item>
        {
            new Item { Name = "Item 57384" },
            new Item { Name = "Product 12345" },
            new Item { Name = "Test 57384 Item" },
            new Item { Name = "Sample 98765" },
            new Item { Name = "57384 Demo" },
            new Item { Name = "JavaScript" },
            new Item { Name = "Python" },
            new Item { Name = "Java" },
            new Item { Name = "C#" },
            new Item { Name = "PHP" }
        };
        
        context.Items.AddRange(items);
        context.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();