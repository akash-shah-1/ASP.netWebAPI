using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Data.SqlTypes;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Reflection.Metadata;

namespace api.Data
{
    public class ApplicationDBContext : DbContext
    //a DbContext is the primary class that interacts with the database.It manages the entity objects during runtime,
    //which includes retrieving them from the database, keeping track of changes, and persisting data back to the database.
    {
        //Constructor
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        //DbContextOptions is a class provided by Entity Framework Core.
        //It encapsulates configuration information needed to set up the DbContext.This includes details
        //like the database provider (e.g., SQL Server, SQLite), connection strings, and other options.

        //The : base(dbContextOptions) syntax is a call to the base class (DbContext) constructor.
        //It passes the dbContextOptions parameter to the base class constructor, ensuring that the DbContext is properly initialized with the provided options.
        {
        }
        public DbSet<Stock> Stock {get;set;}
        public DbSet<Comment> Comment { get;set;}

        //By defining a DbSet<T>, you are telling Entity Framework Core that you want to be able to query and save instances of Stock and Comment.


    }
}
