var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;

// Add Carter for routing mapping
builder.Services.AddCarter(new DependencyContextAssemblyCatalog([typeof(Program).Assembly]));

// Add MediatR for CQRS pattern
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
});
// Add DbContext via EntityFramework
builder.Services.AddDbContext<AppDbContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration.GetConnectionString("Database")!);
});

// Add Identity that uses AppDbContext as store
builder.Services.AddIdentity<AppUser, AppRole>()
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

// Add custom exception handling
//builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapCarter();

//app.UseExceptionHandler(opts => { });

app.Run();