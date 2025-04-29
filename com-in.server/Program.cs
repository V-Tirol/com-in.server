using com_in.server.Hasher;
using com_in.server.Models;
using com_in.server.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ForumContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddAutoMapper(typeof(Program));

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowReactApp",
//        policy =>
//        {
//            policy.WithOrigins("http://localhost:5173/") // Replace with your frontend URL
//                  .AllowAnyHeader()
//                  .AllowAnyMethod();
//        });
//});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()  // <-- temporarily allow any origin
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<ForumContext>();
//    //db.Database.EnsureCreated();
//    await db.Database.MigrateAsync();
//    await DbSeeder.SeedAsync(db);
//}

//app.UseCors("AllowReactApp");
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
