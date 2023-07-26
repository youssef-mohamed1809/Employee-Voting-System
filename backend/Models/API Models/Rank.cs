using System.ComponentModel.DataAnnotations;

namespace backend.Models.API_Models
{
    public class Rank
    {
        [Key]
        public int votedEmpID { get; set; }
        public int votes { get; set; }
    }
}
