using CameraView.Data; // Add this
using CameraView.Models; // Ensure this namespace is included
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml; // Ensure this namespace is included

var builder = WebApplication.CreateBuilder(args);

// Fix for CS0200 and CS0246: Use the correct property and method to set the license context
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.AccessDeniedPath = "/AccessDenied";
    });

var app = builder.Build();
System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();                // ✅ Ensure session is enabled before auth
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var hasher = new PasswordHasher<User>();

    var users = db.Users.ToList();
    foreach (var user in users)
    {
        try
        {
            // Thử decode base64 → nếu fail, nghĩa là chưa hash
            Convert.FromBase64String(user.Password);
        }
        catch
        {
            // Mật khẩu chưa mã hóa → thực hiện mã hóa
            var hashed = hasher.HashPassword(user, user.Password);
            user.Password = hashed;
        }
    }

    db.SaveChanges();
}


app.Run();
