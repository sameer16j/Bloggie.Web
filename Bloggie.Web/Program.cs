using Bloggie.Web.Data;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//Injecting DbContext in services of our app
builder.Services.AddDbContext<BloggieDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("BloggieDbConnectionString")));

//Injecting AuthDbContext in services of our app
builder.Services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("BloggieAuthDbConnectionString")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AuthDbContext>();

//to configure password settings
builder.Services.Configure<IdentityOptions>(options =>
{
    //default settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

});

//Injecting ITagRepository in services to use it in a controller
builder.Services.AddScoped<ITagRepository, TagRepository>();

//Injecting IBlogPostRepository in services
builder.Services.AddScoped<IBlogPostRepository, BlogPostRepository>();

//Injecting IImageRepository in services
builder.Services.AddScoped<IImageRepository, CloudinaryImageRepository>();

//Injecting IBlogPostLikeRepository in services
builder.Services.AddScoped<IBlogPostLikeRepository, BlogPostLikeRepository>();

//Injecting IBlogPostCommentRepository in services
builder.Services.AddScoped<IBlogPostCommentRepository, BlogPostCommentRepository>();

//Injecting IUserRepository in services
builder.Services.AddScoped<IUserRepository,UserRepository>();

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

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
