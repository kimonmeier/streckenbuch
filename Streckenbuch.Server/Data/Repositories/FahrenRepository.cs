using Microsoft.EntityFrameworkCore;
using MudBlazor;
using Streckenbuch.Server.Data.Entities.Betriebspunkte;
using Streckenbuch.Server.Data.Entities.Linien;
using Streckenbuch.Server.Data.Entities.Signale;
using Streckenbuch.Server.Data.Entities.Strecken;
using Streckenbuch.Server.Models;
using Streckenbuch.Shared.Models;
using System.Runtime.CompilerServices;

namespace Streckenbuch.Server.Data.Repositories;

public class FahrenRepository
{
    private readonly ApplicationDbContext _dbContext;

    public FahrenRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<FahrenTransferEntry> ListEntriesByLinie(Guid linieId)
    {        
        List<FahrenTransferEntry> entries = new();

        var streckenKonfigurationEntries = _dbContext
            .Set<LinienStreckenKonfigurationen>().Where(x => x.LinieId.Equals(linieId))
            .OrderBy(x => x.Order)
            .Select(x => new { x.StreckenKonfigurationId, x.VonBetriebspunktId, x.BisBetriebspunktId });

        foreach (var konfigurationEntry in streckenKonfigurationEntries)
        {
            List<FahrenTransferEntry> list = ListEntriesByStrecke(konfigurationEntry.StreckenKonfigurationId);
            /* Maybe obsolete
            while (list[0].Betriebspunkt == null || list[0].Betriebspunkt?.Id != konfigurationEntry.VonBetriebspunktId)
            {
                list.RemoveAt(0);
            }

            while (list[^1].Betriebspunkt == null || list[^1].Betriebspunkt?.Id != konfigurationEntry.VonBetriebspunktId)
            {
                list.RemoveAt(list.Count - 1);
            }
            */
            entries.AddRange(list);
        }

        return entries;
    }
    
    public List<FahrenTransferEntry> ListEntriesByStrecke(Guid streckenKonfigurationId)
    {
        List<FahrenTransferEntry> entries = new List<FahrenTransferEntry>();

        var betriebspunkte = _dbContext
            .Set<BetriebspunktStreckenZuordnung>()
            .AsNoTracking()
            .Include(x => x.Betriebspunkt)
            .OrderBy(x => x.SortNummer)
            .Where(x => x.StreckenKonfigurationId == streckenKonfigurationId).ToList();

        foreach (var betriebspunktZuordnung in betriebspunkte)
        {
            var signale = _dbContext.Set<SignalStreckenZuordnung>()
                .AsNoTracking()
                .Include(x => x.Signal)
                .Where(x => x.StreckeId == betriebspunktZuordnung.Id)
                .Where(x => x.Signal.BetriebspunktId.Equals(betriebspunktZuordnung.Betriebspunkt.Id)).ToList();

            var einfahrtSignal = signale.SingleOrDefault(x => x.Signal.Seite == SignalSeite.Einfahrt);
            var abschnitEinfahrtSignale = signale.Where(x => x.Signal.Seite == SignalSeite.Abschnitt_Einfahrt);

            var abschnittAusfahrtSignale = signale.Where(x => x.Signal.Seite == SignalSeite.Abschnitt_Ausfahrt);
            var ausfahrtSignal = signale.SingleOrDefault(x => x.Signal.Seite == SignalSeite.Ausfahrt);

            if (einfahrtSignal is not null)
            {
                entries.Add(new FahrenTransferEntry()
                {
                    SignalZuordnung = einfahrtSignal
                });
            }

            entries.AddRange(abschnitEinfahrtSignale.Select(x => new FahrenTransferEntry()
            {
                SignalZuordnung = x
            }));


            entries.Add(new FahrenTransferEntry()
            {
                Betriebspunkt = betriebspunktZuordnung.Betriebspunkt
            });


            entries.AddRange(abschnittAusfahrtSignale.Select(x => new FahrenTransferEntry()
            {
                SignalZuordnung = x
            }));

            if (ausfahrtSignal is not null)
            {
                entries.Add(new FahrenTransferEntry()
                {
                    SignalZuordnung = ausfahrtSignal
                });
            }
        }

        return entries;
    }
}
