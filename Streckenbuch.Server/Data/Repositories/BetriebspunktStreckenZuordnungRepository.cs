using Microsoft.EntityFrameworkCore;
using Streckenbuch.Server.Data.Entities.Betriebspunkte;
using Streckenbuch.Shared.Data.Repositories;

namespace Streckenbuch.Server.Data.Repositories;

public class BetriebspunktStreckenZuordnungRepository : GenericRepository<BetriebspunktStreckenZuordnung>
{
    public BetriebspunktStreckenZuordnungRepository(DbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Guid> FindZuordnungByKonfigurationAndBetriebspunkt(Guid streckenKonfigurationId, Guid betriebspunktId)
    {
        return (await Entities.SingleAsync(x => x.StreckenKonfigurationId == streckenKonfigurationId && x.BetriebspunktId == betriebspunktId)).Id;
    }

    public Task<List<BetriebspunktStreckenZuordnung>> ListByStreckenKonfigurationId(Guid streckenKonfigurationId)
    {
        return Entities.Where(x => x.StreckenKonfigurationId == streckenKonfigurationId).Include(x => x.Betriebspunkt).ToListAsync();
    }

    public Task<List<BetriebspunktStreckenZuordnung>> ListByBetriebspunktAsync(Guid betriebspunktId)
    {
        return Entities.Where(x => x.BetriebspunktId == betriebspunktId).Include(x => x.StreckenKonfiguration).ThenInclude(x => x.Strecke).ToListAsync();
    }

    public Task RecalculateSortNumbersAsync(Guid streckenKonfigurationId)
    {
        return RearangeSortNumbersAsync(streckenKonfigurationId, false);
    }

    public async Task AddZuordnungAsync(BetriebspunktStreckenZuordnung zuordnung)
    {
        await AddAsync(zuordnung);

        var oldZuordnung = Entities.Where(x => x.StreckenKonfigurationId == zuordnung.StreckenKonfigurationId).SingleOrDefault(x => x.SortNummer == zuordnung.SortNummer);
        if (oldZuordnung is null)
        {
            return;
        }

        await MoveZuordnungAsync(oldZuordnung.Id, oldZuordnung.SortNummer, oldZuordnung.SortNummer + 1);
    }

    public async Task ChangeZuordnungAsync(Guid zuordnungId, int oldIndex, int newIndex)
    {
        await MoveZuordnungAsync(zuordnungId, oldIndex, newIndex);
    }

    private async Task MoveZuordnungAsync(Guid zuordnungId, int oldIndex, int newIndex)
    {
        BetriebspunktStreckenZuordnung? streckenZuordnung = await Entities.SingleAsync(x => x.Id == zuordnungId);
        var currentIndexHolder = Entities.Local.Where(x => x.StreckenKonfigurationId == streckenZuordnung.StreckenKonfigurationId).SingleOrDefault(x => x.SortNummer == newIndex);

        streckenZuordnung.SortNummer = newIndex;


        if (currentIndexHolder is null)
        {
            currentIndexHolder = Entities.Where(x => x.StreckenKonfigurationId == streckenZuordnung.StreckenKonfigurationId).SingleOrDefault(x => x.SortNummer == newIndex);
            if (currentIndexHolder is null || HasChanges(currentIndexHolder))
            {
                return;
            }
        }

        if (oldIndex > newIndex)
        {
            await MoveZuordnungAsync(currentIndexHolder.Id, oldIndex, currentIndexHolder.SortNummer + 1);
        }
        else
        {
            await MoveZuordnungAsync(currentIndexHolder.Id, oldIndex, currentIndexHolder.SortNummer - 1);
        }
    }

    private async Task RearangeSortNumbersAsync(Guid streckenKonfigurationId, bool replaceExistingNumbers)
    {
        List<BetriebspunktStreckenZuordnung> databaseStreckenZuordnungen = await Entities.Where(x => x.StreckenKonfigurationId == streckenKonfigurationId).ToListAsync();
        List<BetriebspunktStreckenZuordnung> streckenZuordnungen = Entities.Local.Where(x => x.StreckenKonfigurationId == streckenKonfigurationId).OrderBy(x => x.SortNummer).ToList();

        foreach (var entity in databaseStreckenZuordnungen)
        {
            if (streckenZuordnungen.Any(x => x.Id == entity.Id))
            {
                continue;
            }

            streckenZuordnungen.Add(entity);
        }

        int previousNummer = int.MinValue;
        foreach (var item in streckenZuordnungen)
        {
            if (await IsDeletedAsync(item.Id))
            {
                continue;
            }

            if (previousNummer == int.MinValue)
            {
                if (item.SortNummer != 1)
                {
                    item.SortNummer = 1;
                }
                previousNummer = item.SortNummer;
                continue;
            }

            if (previousNummer + 1 == item.SortNummer)
            {
                previousNummer = item.SortNummer;
                continue;
            }

            if (previousNummer >= item.SortNummer && !replaceExistingNumbers)
            {
                throw new InvalidDataException("Die Sortnummern sind zwei mal vorhanden");
            }

            item.SortNummer = previousNummer + 1;
            previousNummer = item.SortNummer;
        }
    }
}
