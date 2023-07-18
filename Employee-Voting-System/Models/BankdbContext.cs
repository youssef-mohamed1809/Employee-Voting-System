﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Employee_Voting_System.Models;

public partial class BankdbContext : DbContext
{
    public BankdbContext()
    {
    }

    public BankdbContext(DbContextOptions<BankdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<VotingYear> VotingYears { get; set; }
    public virtual DbSet<Voting> Votings { get; set; }
    public virtual DbSet<Employee> Employee { get; set; }
    public virtual DbSet<Manager> Manager { get; set; }   
    public virtual DbSet<Department> Department { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VotingYear>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("VotingYear");

            entity.HasIndex(e => e.Year, "UQ__VotingYe__809A238BE541661A").IsUnique();

            entity.Property(e => e.EndDate)
                .HasColumnType("date")
                .HasColumnName("endDate");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.StartDate)
                .HasColumnType("date")
                .HasColumnName("startDate");
            entity.Property(e => e.Year).HasColumnName("year");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
