using BancaEnLinea.Services;
// Si en tu .csproj NO tienes ImplicitUsings=enable, descomenta esta línea:
// using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<InMemoryStore>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AccountsService>();
builder.Services.AddScoped<BeneficiariosService>();
builder.Services.AddScoped<TransferenciasService>();
builder.Services.AddScoped<PagosService>();
builder.Services.AddScoped<HistorialService>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();

// Gate de login: permite Account/Login, Account/Logout y archivos estáticos; pide sesión en lo demás.
app.Use(async (ctx, next) =>
{
    var path = (ctx.Request.Path.Value ?? string.Empty).ToLowerInvariant();
    bool isPublic =
        path.StartsWith("/account/login") ||
        path.StartsWith("/account/logout") ||
        path.StartsWith("/css") ||
        path.StartsWith("/js") ||
        path.StartsWith("/lib") ||
        path == "/";

    if (!isPublic && string.IsNullOrEmpty(ctx.Session.GetString("user")))
    {
        ctx.Response.Redirect("/Account/Login");
        return;
    }

    await next();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();
