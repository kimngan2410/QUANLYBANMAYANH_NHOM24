using Microsoft.EntityFrameworkCore;
using QUANLYBANMAYANH_NHOM24.Models;
using QUANLYBANMAYANH_NHOM24.Utilities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ViewRenderHelper>();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Thêm dịch vụ để đọc các cấu hình từ appsettings.json và appsettings.Local.json
builder.Configuration
       .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

builder.Services.AddDbContext<QuanLyBanMayAnhContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian session tồn tại
    options.Cookie.HttpOnly = true; // Bảo mật cookie
    options.Cookie.IsEssential = true; // Cần thiết cho ứng dụng
});

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Cấu hình middleware
app.UseSession(); // Kích hoạt session

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Admin}/{action=QuanLySanPham}/{id?}");

app.Run();
