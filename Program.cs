using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ===================== CONTROLLERS =====================
builder.Services.AddControllers();

// ===================== OPEN API (optional) =====================
builder.Services.AddOpenApi();

// ===================== DATABASE =====================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ===================== CORS =====================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact",
        policy =>
        {
            policy.WithOrigins(
                    "http://localhost:3000",
                    "http://localhost:3002"
                )
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// ===================== AUTH (JWT SUPPORT - SAFE ADD) =====================
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

// ===================== DEVELOPMENT TOOLS =====================
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// ===================== MIDDLEWARE ORDER (VERY IMPORTANT) =====================

app.UseHttpsRedirection();

// 🔥 MUST be before auth & controllers
app.UseCors("AllowReact");

// 🔐 AUTH PIPELINE
app.UseAuthentication();
app.UseAuthorization();

// ===================== MAP CONTROLLERS =====================
app.MapControllers();

app.Run();