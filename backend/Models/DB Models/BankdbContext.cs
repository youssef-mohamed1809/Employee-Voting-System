using System;
using System.Collections.Generic;
using backend.Models.API_Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Models;

public partial class BankdbContext : DbContext
{
    public BankdbContext()
    {
    }

    public BankdbContext(DbContextOptions<BankdbContext> options)
        : base(options)
    {
        
    }

    public virtual DbSet<Department> Department { get; set; }
    public virtual DbSet<Employee> Employee { get; set; }
    public virtual DbSet<Manager> Manager { get; set; }
    public virtual DbSet<Votings> Votings { get; set; }
    public virtual DbSet<VotingYear> VotingYear { get; set; }
    public virtual DbSet<Rank> Rank { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer("Server=localhost;Database=bankdb;Trusted_Connection=True;TrustServerCertificate=true");
}
