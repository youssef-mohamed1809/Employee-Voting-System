using System;
using System.Collections.Generic;

namespace Employee_Voting_System.Models;

public partial class Manager
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? DepId { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }
}
