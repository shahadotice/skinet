
using API.Helpers;
using AutoMapper;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IProductRepository,ProductRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>),(typeof(GenericRepository<>)));
builder.Services.AddAutoMapper(typeof(MappingProfile));
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<StoreContext>(x=>x.UseSqlite(connectionString));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c=>
{
    c.SwaggerDoc("v1",new Microsoft.OpenApi.Models.OpenApiInfo{Title="API",Version="v1"});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c=>c.SwaggerEndpoint("/swagger/v1/swagger.json","API v1"));
}

app.UseHttpsRedirection();
app.UseRouting();

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
