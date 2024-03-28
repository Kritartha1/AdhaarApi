using Adhaar.API.Data;
using Adhaar.API.Helper;
using Adhaar.API.Mappings;
using Adhaar.API.Models.Domain;
using Adhaar.API.Repositories.Implementaion;
using Adhaar.API.Repositories.Interface;
using Adhaar.API.Services;
using Adhaar.API.SMS;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

[ExcludeFromCodeCoverage]
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        var logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("Logs/Adhaar.txt", rollingInterval: RollingInterval.Day)
            .MinimumLevel.Information()
            .CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(logger);

        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Adhaar API",
                Version = "v1",
                Contact=new OpenApiContact
                {
                    Name="IdentifyMe Contact",
                    Email="example@email.com",
                    Url=new Uri("https://example.com/Contact"),
                   /* smtp://live.smtp.mailtrap.io:587*/

                }

            }) ;
            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference=new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id=JwtBearerDefaults.AuthenticationScheme
                },
                Scheme="OAuth2",
                Name=JwtBearerDefaults.AuthenticationScheme,
                In=ParameterLocation.Header
            },
            new List<string>()
        }
    });
        });

      /*  string accountSid = Keys.SMSAccountIdentification;
        string authToken = Keys.SMSAccountPassword;

        TwilioClient.Init(accountSid, authToken);

        var mssg = MessageResource.CreateAsync(
            body: "This is the ship that made the Kessel Run in fourteen parsecs?",
            from: new Twilio.Types.PhoneNumber(Keys.SMSAccountFrom),
            to: new Twilio.Types.PhoneNumber("+917008196889")
        );*/

        builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

        ///////////////////////////////////////////////////////////////////////////////////////////
        builder.Services.Configure<IdentityOptions>(opts=>opts.SignIn.RequireConfirmedEmail = true);
        ///////////////////////////////////////////////////////////////////////////////////////////
        ///
        builder.Services.AddTransient<IMailService, MailService>();
        builder.Services.AddTransient<IIdentityMessageService, SmsService>();

        builder.Services.AddDbContext<AdhaarApiDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("AdhaarConnectionString"))
        );

        builder.Services.AddDbContext<AdhaarApiAuthDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("AdhaarConnectionString"))
        );

        builder.Services.AddScoped<IUserRepository, SQLUserRepository>();
        builder.Services.AddScoped<ITokenRepository, TokenRepository>();
        builder.Services.AddScoped<IImageRepository, ImageRepository>();
       


        builder.Services.AddAutoMapper(typeof(AutoMapperprofiles));

        builder.Services.AddIdentityCore<IdentityUser>().AddRoles<IdentityRole>()
            .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("Adhaar.API")
            .AddEntityFrameworkStores<AdhaarApiAuthDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;
        });

        builder.Services.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan=TimeSpan.FromMinutes(5) );
       

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
            options =>
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))

            }
            );





        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors(options =>
        {
            options.AllowAnyHeader();
            options.AllowAnyMethod();
            options.AllowAnyOrigin();
            //options.AllowCredentials();
        });

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();


    }
}

