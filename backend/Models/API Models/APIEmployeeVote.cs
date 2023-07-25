namespace backend.Models.API_Models
{
    public class APIEmployeeVote : APIEmployee
    {
        public int id { get; set;  }
        public string name { get; set; }
        public int year { get; set; }
    }
}
