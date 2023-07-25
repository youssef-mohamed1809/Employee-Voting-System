using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public partial class Votings
{
    [Key]
    public int Id { get; set; }

    public int? Year { get; set; }

    public int? VotedEmpId { get; set; }

    public int? EmpId { get; set; }

    public int? DepId { get; set; }
}
