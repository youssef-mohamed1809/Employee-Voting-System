using backend.Models;
using backend.Models.API_Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        [HttpPost("submitVote/")] 
        public async Task<ActionResult<APIVote>> Vote(APIVote v)
        {
            

            Votings votingDB = new Votings();
            votingDB.Year = DateTime.Now.Year;
            votingDB.EmpId = v.voterID;
            votingDB.VotedEmpId = v.votedID;

            using(var dbContext = new BankdbContext())
            {
                dbContext.Votings.Add(votingDB);
            }
            
            return Ok(true);
        }





        [HttpGet("votingIsOpen/")]
        public async Task<ActionResult<bool>> votingIsOpen()
        {
            int year = DateTime.Now.Year;
            string start;
            string end;
            List<VotingYear> votingYears;
            using(var dbContext = new BankdbContext())
            {
                votingYears = dbContext.VotingYear.ToList();
            }
            DateTime now = DateTime.Now;
            foreach(var votingYear in votingYears)
            {
                if(votingYear.Year == DateTime.Now.Year && (votingYear.StartDate < now && votingYear.EndDate > now))
                {
                    return Ok(true);
                }
            }
            return Ok(false);
        }


        [HttpGet("hasEmployeeVoted/")]
        public async Task<ActionResult<bool>> employeeVoted(int empID)
        {
            int year = DateTime.Now.Year;
            List<Votings> list = new List<Votings>();
            using(var dbContext = new BankdbContext())
            {
                list = dbContext.Votings.ToList();
            }
            bool found = false;
            foreach(var vote in list) { 
                if(year == vote.Year && empID == vote.EmpId){
                    return Ok(true);
                }
            }
            return Ok(false);
        }

        [HttpGet("employeeVotingHistory/")]
        public async Task<ActionResult<APIEmployeeVote>> employeeVotesHistory(int empID)
        {
            List<APIEmployeeVote> voteHistory = new List<APIEmployeeVote>();
            List<Votings> votings;
            using(var dbContext = new BankdbContext())
            {
                votings = dbContext.Votings.FromSql($"select * from votings where empID = {empID} order by year desc").ToList();

            }
            using (var dbContext = new BankdbContext())
            {
                foreach (var vote in votings)
                {
                    APIEmployeeVote voter = new APIEmployeeVote();
                    voter.year = (int)vote.Year;
                    voter.id = (int)vote.VotedEmpId;
                    var temp = dbContext.Employee.ToList();
                    for (int i = 0; i < temp.Count; i++)
                    {
                        if (temp[i].EmpId == vote.VotedEmpId)
                        {
                            voter.name = temp[i].Name;
                        }
                    }
                    voteHistory.Add(voter);
                }
            }


            return Ok(voteHistory);
        }

        [HttpGet("/winnerByYear")]
        public async Task<ActionResult<APIEmployeeVote>> winner(int depID, int year)
        {
            List<Votings> votings;
            using (var dbContext = new BankdbContext())
            {
                votings = dbContext.Votings.FromSql($"select * from votings where depID = {depID} and year = {year}").ToList();
            }
            int max = -1;
            int empID = -1;
            for(int i = 0; i < votings.Count; i++)
            {
                int count = 0;
                for(int j = 0; j < votings.Count; j++) {
                    if (votings[i].VotedEmpId == votings[j].VotedEmpId) {
                        count++;
                    }
                }
                if(max < count)
                {
                    max = count;
                    empID = (int)votings[i].VotedEmpId;
                }
            }

            return Ok(empID);
        }
    }
}
