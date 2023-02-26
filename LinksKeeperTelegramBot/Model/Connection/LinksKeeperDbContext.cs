using System;
using System.Collections.Generic;
using LinksKeeperTelegramBot.Model.Entities;
using LinksKeeperTelegramBot.Util;
using Microsoft.EntityFrameworkCore;

namespace LinksKeeperTelegramBot.Model.Connection;

public partial class LinksKeeperDbContext : DbContext
{
    public LinksKeeperDbContext()
    {
    }

    public LinksKeeperDbContext(DbContextOptions<LinksKeeperDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Link> Links { get; set; }

    public virtual DbSet<LinkCategory> LinksCategories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql($"Host={SystemStringsStorage.DbHost}:5432;Database={SystemStringsStorage.DbDatabasename};Username={SystemStringsStorage.DbUsername};Password={SystemStringsStorage.DbPassword}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Link>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("links_pk");

            entity.ToTable("links");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.ChatId).HasColumnName("chat_id");
            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(1024)
                .HasColumnName("description");
            entity.Property(e => e.Url)
                .IsRequired()
                .HasMaxLength(2048)
                .HasColumnName("url");

            entity.HasOne(d => d.Category).WithMany(p => p.Links)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("links_links_categories_id_fk");
        });

        modelBuilder.Entity<LinkCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("links_categories_pk");

            entity.ToTable("links_categories");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ChatId).HasColumnName("chat_id");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(64)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
