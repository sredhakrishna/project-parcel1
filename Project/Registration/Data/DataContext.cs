
using Microsoft.EntityFrameworkCore;

using Registration.Entities;

namespace Registration.Data;

public sealed class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
}
