using API.Extensions;
using API.Helpers;
using API.MiddleWare;
using AutoMapper;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(MappingProfile));
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<StoreContext>(x=>x.UseSqlite(connectionString));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddApplicationServices();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddEndpointsApiExplorer();



var app = builder.Build();
app.UseMiddleware<ExceptionMiddleWare>();
// Configure the HTTP request pipeline.
app.UseSwaggerDocumentation();
app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
 {
    endpoints.MapControllers();
 }
);
using (var scope = app.Services.CreateScope())
{
    var services=scope.ServiceProvider;
    var loggerFacrory=services.GetRequiredService<ILoggerFactory>();
    try
    {
       var context = services.GetRequiredService<StoreContext>();
       await context.Database.MigrateAsync();
       await StoreContextSeed.SeedAsync(context,loggerFacrory);
    }
    catch(Exception ex)
    {
        var logger=loggerFacrory.CreateLogger<Program>();
        logger.LogError(ex,"An error occured during migration");
    }
    
}
// app.MapControllers();

app.Run();
