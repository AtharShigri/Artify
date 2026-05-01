using AutoMapper;
using artifi.Api.Data;
using artifi.Api.Models;
using artifi.Api.Hubs;
using artifi.Api.Repositories.Interfaces;
using artifi.Api.Repositories.Implementations;
using artifi.Api.Services.Interfaces;
using artifi.Api.Services.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// 1. SERVICES REGISTRATION (Must be before builder.Build())
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "artifi API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT token}"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// CORS Configuration (MOVED UP HERE)
var allowedOrigins = builder.Environment.IsDevelopment() 
    ? new[] { "http://localhost:5173", "https://localhost:7294" } 
    : new[] { "https://artifi.art", "https://www.artifi.art" };

builder.Services.AddCors(options => {
    options.AddPolicy("ArtifyPolicy", policy => {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Often needed for SignalR
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options => {
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        RoleClaimType = ClaimTypes.Role,
        NameClaimType = ClaimTypes.NameIdentifier
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) &&
                (path.StartsWithSegments("/chathub") || path.StartsWithSegments("/notificationhub")))
            {
                context.Token = accessToken;
            }
            return System.Threading.Tasks.Task.CompletedTask;
        }
    };
});

builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Repositories
builder.Services.AddScoped<IArtistRepository, ArtistRepository>();
builder.Services.AddScoped<IArtServiceRepository, ArtServiceRepository>();
builder.Services.AddScoped<IArtworkRepository, ArtworkRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProtectionRepository, ProtectionRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IBuyerRepository, BuyerRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IHiringRepository, HiringRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IEscrowRepository, EscrowRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IAdminArtworkRepository, AdminArtworkRepository>();
builder.Services.AddScoped<IAdminReportRepository, AdminReportRepository>();
builder.Services.AddScoped<IAdminTransactionRepository, AdminTransactionRepository>();
builder.Services.AddScoped<IAdminUserRepository, AdminUserRepository>();

// Services
builder.Services.AddScoped<IArtistDashboardService, ArtistDashboardService>();
builder.Services.AddScoped<IArtistProfileService, ArtistProfileService>();
builder.Services.AddScoped<IArtServiceListingService, ArtServiceListingService>();
builder.Services.AddScoped<IArtworkService, ArtworkService>();
builder.Services.AddScoped<IProtectionService, ProtectionService>();
builder.Services.AddScoped<IBuyerService, BuyerService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IHiringService, HiringService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IMarketplaceService, MarketplaceService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IAdminArtworkService, AdminArtworkService>();
builder.Services.AddScoped<IAdminReportService, AdminReportService>();
builder.Services.AddScoped<IAdminTransactionService, AdminTransactionService>();
builder.Services.AddScoped<IAdminUserService, AdminUserService>();
builder.Services.AddScoped<IAdminDashboardService, AdminDashboardService>();
builder.Services.AddScoped<IPlagiarismService, PlagiarismService>();

builder.Services.AddSignalR();

// 2. BUILD THE APP
var app = builder.Build();

// 3. MIDDLEWARE PIPELINE
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Use the CORS policy registered above
app.UseCors("ArtifyPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<ChatHub>("/chathub");
app.MapHub<NotificationHub>("/notificationhub");
app.MapControllers();
app.MapFallbackToFile("index.html");

// 4. DATABASE MIGRATIONS & SEEDING
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try 
    {
        var db = services.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate(); 

        var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        string[] roles = { "Admin", "Artist", "Buyer", "Agency" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
        }

        await DbSeeder.SeedAdminUser(services);
        await DbSeeder.SeedCategories(db);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during migration or seeding.");
    }
}

app.Run();