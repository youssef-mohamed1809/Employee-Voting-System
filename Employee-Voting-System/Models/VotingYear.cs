using System;
using System.Collections.Generic;

namespace Employee_Voting_System.Models;

public partial class VotingYear
{
    public int Id { get; set; }

    public int? Year { get; set; }

    public string? Name { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}
