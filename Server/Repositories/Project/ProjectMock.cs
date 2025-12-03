using Core;
using Server.Repositories;

public class ProjectRepositoryMock : IProjectRepo
{
    private List<Project> mProjects = new();

    public ProjectRepositoryMock()
    {
        mProjects.Add(new Project
        {
            ProjectId = 1,
            Name = "Elinstallation i villa",
            DateCreated = DateTime.Now.AddDays(-7),
            SvendTimePris = 500,
            LærlingTimePris = 200,
            KonsulentTimePris = 800,
            ArbejdsmandTimePris = 250,
            Houses = new List<House>
            {
                new House
                {
                    Name = "Hus 1",
                    CostCustomer = 20000,
                    CostCompany = 14000,
                    Hours = 80,
                    InstallationCosts = new Dictionary<string, decimal>
                    {
                        { "Stikkontakter", 1000 },
                        { "Belysning", 2000 }
                    },
                    MaterialsUsed = new Dictionary<string, int>
                    {
                        { "Ledninger", 50 },
                        { "Stik", 20 }
                    }
                }
            },
            TotalCostCustomer = 50000,
            TotalCostCompany = 35000,
            TimeEntries = new List<TimeEntry>
            {
                new TimeEntry { Date = DateTime.Today.AddDays(-5), Hours = 5 },
                new TimeEntry { Date = DateTime.Today.AddDays(-4), Hours = 6 }
            }
        });
        mProjects.Add(new Project
        {
            ProjectId = 2,
            Name = " JumboLombo villa",
            DateCreated = DateTime.Now.AddDays(-7),
            SvendTimePris = 500,
            LærlingTimePris = 200,
            KonsulentTimePris = 800,
            ArbejdsmandTimePris = 250,
            Houses = new List<House>
            {
                new House
                {
                    Name = "Hus 2",
                    CostCustomer = 20000,
                    CostCompany = 14000,
                    Hours = 80,
                    InstallationCosts = new Dictionary<string, decimal>
                    {
                        { "Stikkontakter", 1000 },
                        { "Belysning", 2000 }
                    },
                    MaterialsUsed = new Dictionary<string, int>
                    {
                        { "Ledninger", 50 },
                        { "Stik", 20 }
                    }
                }
            },
            TotalCostCustomer = 50000,
            TotalCostCompany = 35000,
            TimeEntries = new List<TimeEntry>
            {
                new TimeEntry { Date = DateTime.Today.AddDays(-5), Hours = 5 },
                new TimeEntry { Date = DateTime.Today.AddDays(-4), Hours = 6 }
            }
        });
    }

    public List<Project> GetAll() => mProjects;

    public void Add(Project p) => mProjects.Add(p);

    public void DeleteById(int id) => mProjects.RemoveAll(p => p.ProjectId == id);
}
