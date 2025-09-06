var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;

// Add Carter for routing mapping
builder.Services.AddCarter(new DependencyContextAssemblyCatalog([typeof(Program).Assembly]));

// Add MediatR for CQRS pattern
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});
// Add DbContext via EntityFramework
builder.Services.AddDbContext<AppDbContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration.GetConnectionString("Database")!);
});

// Add Identity that uses AppDbContext as store, with password requirements
builder.Services.AddIdentity<AppUser, AppRole>(opts =>
{
    opts.Password = new()
    {
        RequireDigit = true,
        RequiredLength = 6,
        RequireNonAlphanumeric = false,
        RequireLowercase = true,
        RequireUppercase = true
    };
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Add Authentication and Authorization
builder.Services.AddAuthentication(opts =>
{
    opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(opts =>
    {
        opts.TokenValidationParameters = new()
        {
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigninKey"] ?? "")),
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true
        };
    });
builder.Services.AddAuthorization();

// Add service to generate jwt tokens for authorizations/authentications
builder.Services.AddScoped<JwtTokenGenerator>();

// Add repository for Tasks
builder.Services.AddScoped<TasksRepository>();

// Add custom exception handling
builder.Services.AddExceptionHandler<CustomExceptionHandling>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

// map all endpoints across assembly
app.MapCarter();

// use global exception handling
app.UseExceptionHandler(opts => { });

app.Run();