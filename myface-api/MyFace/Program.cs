using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyFace.Data;
using MyFace.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyFace
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();

            CreateDbIfNotExists(host);

            host.Run();
        }

        private static void CreateDbIfNotExists(IHost host)
        {
            using IServiceScope scope = host.Services.CreateScope();
            IServiceProvider services = scope.ServiceProvider;

            MyFaceDbContext context = services.GetRequiredService<MyFaceDbContext>();
            context.Database.EnsureCreated();

            if (!context.Users.Any())
            {
                IEnumerable<User> users = SampleUsers.GetUsers();
                context.Users.AddRange(users);
                context.SaveChanges();

                IEnumerable<Post> posts = SamplePosts.GetPosts();
                context.Posts.AddRange(posts);
                context.SaveChanges();

                IEnumerable<Interaction> interactions = SampleInteractions.GetInteractions();
                context.Interactions.AddRange(interactions);
                context.SaveChanges();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
.ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}