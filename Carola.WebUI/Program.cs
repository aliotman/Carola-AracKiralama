using Carola.BusinessLayer.Abstract;
using Carola.BusinessLayer.Concrete;
using Carola.BusinessLayer.Mapping;
using Carola.BusinessLayer.ValidationRules;
using Carola.DataAccessLayer.Abstract;
using Carola.DataAccessLayer.Concrete;
using Carola.DataAccessLayer.EntityFramework;
using Carola.DtoLayer.Dtos.CustomerDtos;
using Carola.DtoLayer.Dtos.ReservationDtos;
using Carola.EntityLayer.Entities;
using Carola.WebUI.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CarolaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CarolaConnection")));

builder.Services.AddScoped<IBrandService, BrandManager>();
builder.Services.AddScoped<IBrandDal, EFBrandDal>();

builder.Services.AddScoped<ICarService, CarManager>();
builder.Services.AddScoped<ICarDal,EFCarDal>();

builder.Services.AddScoped<ICategoryService,CategoryManager>();
builder.Services.AddScoped<ICategoryDal,EFCategoryDal>();

builder.Services.AddScoped<ILocationService, LocationManager>();
builder.Services.AddScoped<ILocationDal,EFLocationDal>();

builder.Services.AddScoped<ICustomerService, CustomerManager>();
builder.Services.AddScoped<ICustomerDal, EFCustomerDal>();

builder.Services.AddScoped<ISliderDal,EFSliderDal>();
builder.Services.AddScoped<ISliderService, SliderManager>();

builder.Services.AddScoped<IReservationService, ReservationManager>();
builder.Services.AddScoped<IReservationDal, EFReservationDal>();

builder.Services.AddScoped<IValidator<Brand>, BrandValidator>();
builder.Services.AddScoped<IValidator<Car>, CarValidator>();
builder.Services.AddScoped<IValidator<Category>, CategoryValidator>();
builder.Services.AddScoped<IValidator<Location>, LocationValidator>();
builder.Services.AddScoped<IValidator<Slider>, SliderValidator>();
builder.Services.AddScoped<IValidator<Reservation>, ReservationValidator>();
builder.Services.AddScoped<IValidator<CreateCustomerDto>, CreateCustomerValidator>();
builder.Services.AddScoped<IValidator<UpdateCustomerDto>, UpdateCustomerValidator>();
builder.Services.AddScoped<IValidator<CreateReservationDto>, CreateReservationValidator>();

builder.Services.AddAutoMapper(typeof(GeneralMapping));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Admin/Login/Index";
        options.LogoutPath = "/Admin/Login/Logout";
        options.AccessDeniedPath = "/Admin/Login/Index";
        options.ExpireTimeSpan = TimeSpan.FromHours(2);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.Name = "CarolaAdminAuth";
    });

builder.Services.AddScoped<MailService>();

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 5 * 1024 * 1024;
});
// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
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

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
