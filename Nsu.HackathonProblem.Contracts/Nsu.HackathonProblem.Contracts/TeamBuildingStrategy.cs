namespace Nsu.HackathonProblem.Contracts
{
    public class TeamBuildingStrategy : ITeamBuildingStrategy
    {
        public IEnumerable<Team> BuildTeams(IEnumerable<Employee> teamLeads, IEnumerable<Employee> juniors,
            IEnumerable<Wishlist> teamLeadsWishlists, IEnumerable<Wishlist> juniorsWishlists)
        {
            var teamLeadsDict = teamLeads.ToDictionary(tl => tl.Id);
            var juniorsDict = juniors.ToDictionary(j => j.Id);

            var teamLeadsWishlistsDict = teamLeadsWishlists.ToDictionary(wl => wl.EmployeeId);
            var juniorsWishlistsDict = juniorsWishlists.ToDictionary(wl => wl.EmployeeId);

            var teams = new List<Team>();

            var usedLeads = new HashSet<int>();
            var usedJuniors = new HashSet<int>();

            foreach (var lead in teamLeads)
            {
                if (teamLeadsWishlistsDict.TryGetValue(lead.Id, out var leadWishlist))
                {
                    foreach (var desiredJuniorId in leadWishlist.DesiredEmployees)
                    {
                        if (juniorsDict.ContainsKey(desiredJuniorId) && !usedJuniors.Contains(desiredJuniorId))
                        {
                            var junior = juniorsDict[desiredJuniorId];
                            teams.Add(new Team(lead, junior));

                            usedLeads.Add(lead.Id);
                            usedJuniors.Add(desiredJuniorId);

                            break;
                        }
                    }
                }
            }

            foreach (var lead in teamLeads)
            {
                if (!usedLeads.Contains(lead.Id))
                {
                    foreach (var junior in juniors)
                    {
                        if (!usedJuniors.Contains(junior.Id))
                        {
                            teams.Add(new Team(lead, junior));
                            usedLeads.Add(lead.Id);
                            usedJuniors.Add(junior.Id);
                            break;
                        }
                    }
                }
            }

            return teams;
        }
    }
}
