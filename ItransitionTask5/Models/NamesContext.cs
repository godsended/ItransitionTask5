using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace ItransitionTask5.Models;

public class NamesContext : DbContext
{
    public DbSet<NameModel> Names { get; set; }

    public NamesContext(DbContextOptions<NamesContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public async void TryAddName(string name)
    {
        if (await Names.FirstOrDefaultAsync(n => n.Name == name) == null)
        {
            Names.Add(new NameModel() {Name = name});
            await SaveChangesAsync();
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=task5;Username=postgres;Password=174285396020");
    }
}