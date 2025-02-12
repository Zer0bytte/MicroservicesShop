using Carter;
using InstagramDMs.API;
using InstagramDMs.API.Data;
using InstagramDMs.API.Endpoints;
using InstagramDMs.API.Hubs.IGHubs;
using InstagramDMs.API.Services;
using InstagramDMs.API.Vms;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<CurrentUser>();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

JsonConvert.DefaultSettings = () => new JsonSerializerSettings
{
    NullValueHandling = NullValueHandling.Ignore,
    DefaultValueHandling = DefaultValueHandling.Ignore,
    ContractResolver = new CamelCasePropertyNamesContractResolver()
};
var assembly = typeof(Program).Assembly;

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
});

builder.Services.AddSignalR();
builder.Services.AddScoped<InstagramService>();
builder.Services.AddScoped<MediaSecurityService>();
builder.Services.AddHttpClient("Instagram", httpclient =>
{
    httpclient.BaseAddress = new Uri("https://graph.instagram.com/v21.0/");
});

builder.Services.AddCarter();

builder.Services.AddAntiforgery();
builder.Services.AddScoped<ClickPayService>();
builder.Services.AddScoped<InstagramWebhookService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.WithOrigins("http://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
        });
});


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{

    var key = Encoding.UTF8.GetBytes("282ce8d74008a99d25fd361eb1f0034a0b611852a254faed849e14bf926559e2");
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false, // on production make it true
        ValidateAudience = false, // on production make it true
        ValidateIssuerSigningKey = true,
        ValidIssuer = "https://info-sender.com",
        ValidAudience = "https://info-sender.com",
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero
    };

});

builder.Services.AddAuthorization();

var app = builder.Build();
app.UseCors("AllowAllOrigins");

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

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    DbInitializer.Initialize(context);
}

app.UseCors();

app.MapCarter();

app.MapMessagingEndpoints();
app.MapWebhookEndpoints();
//app.MapConversationEndpoints();
app.MapTemplateEndpoints();



app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(@"C:\Users\Zer0byte\source\repos\PaymentIntegration\uploads"),
    RequestPath = new PathString("/media")
});


app.MapHub<InstagramHub>("/chatHub");
app.Run();

public class SendTextMessageCommand
{
    public required string RecipientId { get; set; }
    public required string MessageText { get; set; }
}

public class SendMediaMessageCommand
{
    public required string RecipientId { get; set; }
    public IFormFile? Media { get; set; }
    public string? AttachmentId { get; set; }
    public string? MediaType { get; set; }
}

public class InstagramSendMessageRequest
{
    public Recipient Recipient { get; set; }
    public InstagramMessageRequest Message { get; set; }
}

public class InstagramSendMessageResponse
{
    [JsonProperty("recipient_id")]
    public string RecipientId { get; set; }
    [JsonProperty("message_id")]
    public string MessageId { get; set; }
}

public record ChatAssignmentRequest(int ConversationId, int TeamMemberId);

public class ConversationAssignedEvent
{
    public AssignedTeamMember AssignedTo { get; set; }
    public AssignedTeamMember AssignedBy { get; set; }
};

public class AssignedTeamMember
{
    public int Id { get; set; }
    public string Name { get; set; }
}