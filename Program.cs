using Microsoft.EntityFrameworkCore;
using novasoft_technical_test.Data;

var builder = WebApplication.CreateBuilder(args);

string accountsUrl = builder.Configuration["ApiSettings:AccountsUrl"];
string loginUrl = builder.Configuration["ApiSettings:LoginUrl"];

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpClient(); // Registro de HttpClient

// Register DbContext with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts(); // HTTP Strict Transport Security
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    Console.WriteLine($"?? Conectando a: {dbContext.Database.GetConnectionString()}");
    if (dbContext.Database.CanConnect())
        Console.WriteLine("? Conexión exitosa a SQL Server");
    else
        Console.WriteLine("? No se pudo conectar a SQL Server");
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

