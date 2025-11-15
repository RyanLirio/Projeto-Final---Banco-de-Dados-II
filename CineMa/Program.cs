using Cine_Ma.Data;
using Cine_Ma.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// NECESSÁRIO PARA A SESSÃO FUNCIONAR
builder.Services.AddDistributedMemoryCache();   // <-- ADICIONE ISTO
builder.Services.AddSession();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<CineContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Defaultconnection"))
);

// seus repositórios
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IChairRepository, ChairRepository>();
builder.Services.AddScoped<ICinemaRepository, CinemaRepository>();
builder.Services.AddScoped<ICinemaRoomRepository, CinemaRoomRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<ILanguageRepository, LanguageRepository>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductOrderRepository, ProductOrderRepository>();
builder.Services.AddScoped<ISessionRepository, SessionRepository>();
builder.Services.AddScoped<ISexRepository, SexRepository>();
builder.Services.AddScoped<ISexMovieRepository, SexMovieRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// ORDEM CORRETA DO PIPELINE ↓↓↓↓
app.UseStaticFiles();
app.UseRouting();
app.UseSession();          // <-- AQUI (DEPOIS DO ROUTING)
app.UseAuthorization();    // <-- AQUI (DEPOIS DO SESSION)
// ORDEM CORRETA ↑↑↑↑

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

CreateDbIfNotExists(app);

app.Run();

static void CreateDbIfNotExists(IHost host)
{
    using (var scope = host.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<CineContext>();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred creating the DB.");
        }
    }
}
