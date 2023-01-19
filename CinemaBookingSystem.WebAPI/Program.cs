using AutoMapper;
using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Data.Repositories;
using CinemaBookingSystem.Data;
using CinemaBookingSystem.Service;
using CinemaBookingSystem.WebAPI.Infrastructure.Mappings;
using CinemaBookingSystem.WebAPI.Infrastructure.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMvc(option => option.EnableEndpointRouting = false)
    .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Latest)
    .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
//CORS
builder.Services.AddCors(options =>
{
    // this defines a CORS policy called "default"
    options.AddPolicy("default", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
//Dependency Injection

//UnitOfWork & DbFactory
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IDbFactory, DbFactory>();

//Context
builder.Services.AddTransient<CinemaBookingSystemDbContext>();

//Services
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IBookingDetailService, BookingDetailService>();
builder.Services.AddScoped<ISlideService, SlideService>();
builder.Services.AddScoped<ICinemaService, CinemaService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IErrorService, ErrorService>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IScreeningPositionService, ScreeningPositionService>();
builder.Services.AddScoped<IScreeningService, ScreeningService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<ITheatreService, TheatreService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IVisitorStatisticService, VisitorStatisticService>();

//Repository
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingDetailRepository, BookingDetailRepository>();
builder.Services.AddScoped<ISlideRepository, SlideRepository>();
builder.Services.AddScoped<ICinemaRepository, CinemaRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IErrorRepository, ErrorRepository>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IScreeningPositionRepository, ScreeningPositionRepository>();
builder.Services.AddScoped<IScreeningRepository, ScreeningRepository>();
builder.Services.AddScoped<ISupportOnlineRepository, ContactRepository>();
builder.Services.AddScoped<ITheatreRepository, TheatreRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IVisitorStatisticRepository, VisitorStatisticRepository>();

//Add AutoMapper Singleton
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new AutoMapperConfiguration());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("default");

app.UseMiddleware<ApiKeyMiddleware>();

app.MapControllers();

app.Run();
