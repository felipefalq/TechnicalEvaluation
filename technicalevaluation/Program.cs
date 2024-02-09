using Microsoft.EntityFrameworkCore;
using technicalevaluation.Data;
using technicalevaluation.Repos;
using technicalevaluation.Repos.Interfaces;

namespace technicalevaluation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<UsersContext>(
                options => options.UseNpgsql(builder.Configuration.GetConnectionString("Database"))
                                  //.UseLazyLoadingProxies()
            );

            builder.Services.AddScoped<IUserRepo, UserRepo>();
            builder.Services.AddScoped<ICollaboratorRepo, CollaboratorRepo>();
            builder.Services.AddScoped<IUnitRepo, UnitRepo>();
            builder.Services.AddLogging();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
