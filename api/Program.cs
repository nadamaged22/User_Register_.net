using api.Data;
using api.Interfaces;
using api.Models;
using api.Services;
using api.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c=>{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

    // Define the Security Definition for JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token with Bearer prefix (e.g. Bearer <token>)",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Add a Security Requirement to use the token for protected endpoints
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

}
);



builder.Services.AddDbContext<ApplicationDBContext>(Options=>{
    Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<AppUser,IdentityRole>(Options=>{
    //put some restrictions in password to be more secure
    Options.Password.RequireLowercase=true;
    Options.Password.RequireUppercase=true;
    Options.Password.RequireDigit=true;
    Options.Password.RequireNonAlphanumeric=true;
    Options.Password.RequiredLength=12;


}).AddEntityFrameworkStores<ApplicationDBContext>();
builder.Services.AddAuthentication(Options=>{
    Options.DefaultAuthenticateScheme=
    Options.DefaultSignInScheme=
    Options.DefaultForbidScheme=
    Options.DefaultChallengeScheme=
    Options.DefaultScheme=
    Options.DefaultSignOutScheme=JwtBearerDefaults.AuthenticationScheme;



}).AddJwtBearer(
    Options=>{
        Options.TokenValidationParameters=new TokenValidationParameters{
            ValidateIssuer=true,
            ValidIssuer=builder.Configuration["JWT:Issuer"],
            ValidateAudience=true,
            ValidAudience=builder.Configuration["JWT:Audience"],
            ValidateIssuerSigningKey=true,
            IssuerSigningKey=new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
            )

        };
    }
);

builder.Services.AddAuthorization();
builder.Services.AddScoped<ITokenService,TokenService>();
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddScoped<ICloudinaryService,CloudinaryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();


