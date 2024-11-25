using Microsoft.EntityFrameworkCore;
using Presentes.Entities;

namespace Presentes.Infra;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}
    
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<AmigoSecreto> AmigosSecretos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AmigoSecreto>()
            .HasOne(a => a.Usuario)
            .WithMany()
            .HasForeignKey(a => a.UsuarioId);

        modelBuilder.Entity<AmigoSecreto>()
            .HasOne(a => a.UsuarioSorteado)
            .WithMany()
            .HasForeignKey(a => a.UsuarioSorteadoId);
    }
}