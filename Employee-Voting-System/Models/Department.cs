using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Employee_Voting_System.Models;

public partial class Department
{
    [Key]
    public int DepId { get; set; }

    public string? DepName { get; set; }
}
