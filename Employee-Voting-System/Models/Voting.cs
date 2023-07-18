using System;
using System.Collections.Generic;

namespace Employee_Voting_System.Models;

public partial class Voting
{
    public int Id { get; set; }

    public int? Year { get; set; }

    public int? VotedEmpId { get; set; }

    public int? EmpId { get; set; }

    public int? DepId { get; set; }
}
