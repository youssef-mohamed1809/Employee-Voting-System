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
                //Get Department ID
                foreach(var employee in dbContext.Employee)
                {
                    if(employee.EmpId ==  v.votedID) {
                        votingDB.DepId = employee.DepId;
                        break;
                    }
                }
                bool exists = false;
                foreach(var voting in dbContext.Votings)
                {
                    if(voting.EmpId == v.voterID && voting.Year == DateTime.Now.Year)
                    {
                        exists = true;
                        voting.VotedEmpId = v.votedID;
                    }
                }

                if (!exists)
                {
                    dbContext.Votings.Add(votingDB);
                }
                dbContext.SaveChanges();
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
                if(votingYear.Year == DateTime.Now.Year && (votingYear.StartDate < now && votingYear.EndDate > now) && (bool)votingYear.status)
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

        [HttpGet("/rankingsThisYear")]
        public async Task<ActionResult<APIEmployeeVote>> ranking(int depID, int? year)
        {
            if(year == null)
            {
                year = DateTime.Now.Year;
            }
            List<Rank> ranks = new List<Rank>();
            using (var dbContext = new BankdbContext())
            {
                ranks = dbContext.Rank.FromSql($"select votedEmpID, count(votedEmpID) as votes from votings where depID = {depID} and year = {year} group by votedEmpID order by votes desc").ToList();
                return Ok(ranks);
            }
        }


        [HttpGet("/winnerOfYear")]
        public async Task<ActionResult<APIEmployeeVote>> winner(int depID, int year)
        {
            List<Rank> rank = new List<Rank>();
            using (var dbContext = new BankdbContext())
            {
                rank = dbContext.Rank.FromSql($"select top 1 votedEmpID, count(votedEmpID) as votes from votings where depID = {depID} and year = {year} group by votedEmpID order by votes desc").ToList();
            }
            return Ok(rank);
        }
    }
}
