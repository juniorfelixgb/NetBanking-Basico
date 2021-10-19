using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace NetBanking.DATA.Modelo
{
    public partial class netbankingContext : DbContext
    {
        public netbankingContext()
        {
        }

        public netbankingContext(DbContextOptions<netbankingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cuenta> Cuentas { get; set; }
        public virtual DbSet<Deposito> Depositos { get; set; }
        public virtual DbSet<EstadoTrd> EstadoTrds { get; set; }
        public virtual DbSet<Moneda> Monedas { get; set; }
        public virtual DbSet<MonedaCambio> MonedaCambios { get; set; }
        public virtual DbSet<Retiro> Retiros { get; set; }
        public virtual DbSet<Transaccione> Transacciones { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;Database=netbanking;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Cuenta>(entity =>
            {
                entity.HasIndex(e => e.NumeroCuenta, "UQ_NumeroCuenta")
                    .IsUnique();

                entity.Property(e => e.CuentaId).HasColumnName("CuentaID");

                entity.Property(e => e.AliasCuenta)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MonedaId).HasColumnName("MonedaID");

                entity.Property(e => e.MontoDisponible).HasColumnType("numeric(38, 2)");

                entity.Property(e => e.MontoTrancito).HasColumnType("numeric(38, 2)");

                entity.Property(e => e.NumeroCuenta)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

                entity.HasOne(d => d.Moneda)
                    .WithMany(p => p.Cuenta)
                    .HasForeignKey(d => d.MonedaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cuentas_MonedaID");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Cuenta)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cuentas_UsuarioID");
            });

            modelBuilder.Entity<Deposito>(entity =>
            {
                entity.Property(e => e.DepositoId).HasColumnName("DepositoID");

                entity.Property(e => e.CuentaId).HasColumnName("CuentaID");

                entity.Property(e => e.Detalles).IsUnicode(false);

                entity.Property(e => e.EstadoId).HasColumnName("EstadoID");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Monto).HasColumnType("numeric(35, 2)");

                entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

                entity.HasOne(d => d.Cuenta)
                    .WithMany(p => p.Depositos)
                    .HasForeignKey(d => d.CuentaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Depositos_CuentaID");

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.Depositos)
                    .HasForeignKey(d => d.EstadoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Depositos_EstadoID");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Depositos)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Depositos_UsuarioID");
            });

            modelBuilder.Entity<EstadoTrd>(entity =>
            {
                entity.HasKey(e => e.EstadoId);

                entity.ToTable("EstadoTRD");

                entity.HasIndex(e => e.Estado, "UQ__EstadoTR__36DF552FF6F60B02")
                    .IsUnique();

                entity.Property(e => e.EstadoId).HasColumnName("EstadoID");

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Moneda>(entity =>
            {
                entity.HasIndex(e => e.MonedaNombre, "UQ__Monedas__0312CC3C515BE83C")
                    .IsUnique();

                entity.HasIndex(e => e.Abreviado, "UQ__Monedas__C294908F0FDEB920")
                    .IsUnique();

                entity.Property(e => e.MonedaId).HasColumnName("MonedaID");

                entity.Property(e => e.Abreviado)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.MonedaNombre)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Simbolo)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MonedaCambio>(entity =>
            {
                entity.HasIndex(e => new { e.MonedaIddesde, e.MonedaIdhacia }, "UQ_MonedaIDdesde_MonedaIDhacia")
                    .IsUnique();

                entity.Property(e => e.MonedaCambioId).HasColumnName("MonedaCambioID");

                entity.Property(e => e.Detalles).IsUnicode(false);

                entity.Property(e => e.MonedaIddesde).HasColumnName("MonedaIDdesde");

                entity.Property(e => e.MonedaIdhacia).HasColumnName("MonedaIDhacia");

                entity.Property(e => e.Valor).HasColumnType("numeric(18, 15)");

                entity.HasOne(d => d.MonedaIddesdeNavigation)
                    .WithMany(p => p.MonedaCambioMonedaIddesdeNavigations)
                    .HasForeignKey(d => d.MonedaIddesde)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MonedaCambios_MonedaIDdesde");

                entity.HasOne(d => d.MonedaIdhaciaNavigation)
                    .WithMany(p => p.MonedaCambioMonedaIdhaciaNavigations)
                    .HasForeignKey(d => d.MonedaIdhacia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MonedaCambios_MonedaIDhasta");
            });

            modelBuilder.Entity<Retiro>(entity =>
            {
                entity.Property(e => e.RetiroId).HasColumnName("RetiroID");

                entity.Property(e => e.CuentaId).HasColumnName("CuentaID");

                entity.Property(e => e.Detalles).IsUnicode(false);

                entity.Property(e => e.EstadoId).HasColumnName("EstadoID");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Monto).HasColumnType("numeric(35, 2)");

                entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

                entity.HasOne(d => d.Cuenta)
                    .WithMany(p => p.Retiros)
                    .HasForeignKey(d => d.CuentaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Retiros_CuentaID");

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.Retiros)
                    .HasForeignKey(d => d.EstadoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Retiros_EstadoID");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Retiros)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Retiros_UsuarioID");
            });

            modelBuilder.Entity<Transaccione>(entity =>
            {
                entity.HasKey(e => e.TransaccionId);

                entity.Property(e => e.TransaccionId).HasColumnName("TransaccionID");

                entity.Property(e => e.CostoTransferencia).HasColumnType("numeric(18, 15)");

                entity.Property(e => e.DepositoId).HasColumnName("DepositoID");

                entity.Property(e => e.Detalles).IsUnicode(false);

                entity.Property(e => e.EstadoId).HasColumnName("EstadoID");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MonedaCambioId).HasColumnName("MonedaCambioID");

                entity.Property(e => e.RetiroId).HasColumnName("RetiroID");

                entity.HasOne(d => d.Deposito)
                    .WithMany(p => p.Transacciones)
                    .HasForeignKey(d => d.DepositoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transacciones_DepositoID");

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.Transacciones)
                    .HasForeignKey(d => d.EstadoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transacciones_EstadoID");

                entity.HasOne(d => d.MonedaCambio)
                    .WithMany(p => p.Transacciones)
                    .HasForeignKey(d => d.MonedaCambioId)
                    .HasConstraintName("FK_Transacciones_MonedaCambioID");

                entity.HasOne(d => d.Retiro)
                    .WithMany(p => p.Transacciones)
                    .HasForeignKey(d => d.RetiroId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transacciones_RetiroID");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasIndex(e => e.NombreUsuario, "UQ__Usuarios__6B0F5AE04DD6A5ED")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "UQ__Usuarios__A9D105349CC3908B")
                    .IsUnique();

                entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

                entity.Property(e => e.Apellidos)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Cedula)
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.Contrasena)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.FechaNacimiento).HasColumnType("date");

                entity.Property(e => e.FechaRegitro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NombreUsuario)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nombres)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
