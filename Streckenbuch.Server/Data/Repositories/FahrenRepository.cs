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
            .Select(x => new
            {
                x.StreckenKonfigurationId, x.VonBetriebspunktId, x.BisBetriebspunktId
            });

        foreach (var konfigurationEntry in streckenKonfigurationEntries)
        {
            List<FahrenTransferEntry> list = ListEntriesByStrecke(konfigurationEntry.StreckenKonfigurationId);
            entries.AddRange(list);
        }

        return entries;
    }

    public List<FahrenTransferEntry> ListEntriesByStrecke(Guid streckenKonfigurationId)
    {
        List<FahrenTransferEntry> entries = new List<FahrenTransferEntry>();

        var signalSorting = _dbContext
            .Set<SignalStreckenZuordnungSortingSignal>()
            .Where(x => x.SignalStreckenZuordnungSortingBetriebspunkt.SignalStreckenZuordnungSortingStrecke.StreckenKonfigurationId == streckenKonfigurationId)
            .Where(x => x.SignalStreckenZuordnungSortingBetriebspunkt.SignalStreckenZuordnungSortingStrecke.GueltigVon <= DateOnly.FromDateTime(DateTime.Today))
            .AsQueryable();

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
                .Where(x => x.Signal.BetriebspunktId.Equals(betriebspunktZuordnung.Betriebspunkt.Id))
                .ToList();

            var signaleSorting = signalSorting
                .Where(x => x.SignalStreckenZuordnungSortingBetriebspunkt.BetriebspunktId.Equals(betriebspunktZuordnung.Betriebspunkt.Id))
                .Where(x => x.SignalStreckenZuordnungSortingBetriebspunkt.SignalStreckenZuordnungSortingStrecke.GueltigVon <= DateOnly.FromDateTime(DateTime.Today) && 
                           (x.SignalStreckenZuordnungSortingBetriebspunkt.SignalStreckenZuordnungSortingStrecke.GueltigBis == null || 
                            x.SignalStreckenZuordnungSortingBetriebspunkt.SignalStreckenZuordnungSortingStrecke.GueltigBis >= DateOnly.FromDateTime(DateTime.Today)))
                .OrderBy(x => x.SortingOrder)
                .ToList();

            var einfahrSignale = signale.Where(x => x.Signal.Seite == SignalSeite.Einfahrt);
            var ausfahrSignale = signale.Where(x => x.Signal.Seite == SignalSeite.Ausfahrt);

            int index = 0;
            foreach (var einfahrSignal in einfahrSignale.Join(signaleSorting, x => x.SignalId, x => x.SignalId, (x, y) => new
                     {
                         Signal = x, Sorting = y
                     }).OrderBy(x => x.Sorting.SortingOrder))
            {
                entries.Add(new FahrenTransferEntry()
                {
                    SignalZuordnung = einfahrSignal.Signal, DisplaySeite = index == 0 ? DisplaySeite.Einfahrt : DisplaySeite.Einfahrt_Abschnitt
                });

                if (einfahrSignal.Signal.Signal.Typ is SignalTyp.Hauptsignal or SignalTyp.Kombiniert)
                {
                    index++;
                }
            }

            entries.Add(new FahrenTransferEntry()
            {
                Betriebspunkt = betriebspunktZuordnung.Betriebspunkt
            });

            foreach (var ausfahrSignal in ausfahrSignale.Join(signaleSorting, x => x.SignalId, x => x.SignalId, (x, y) => new
                     {
                         Signal = x, Sorting = y
                     }).OrderBy(x => x.Sorting.SortingOrder))
            {
                entries.Add(new FahrenTransferEntry()
                {
                    SignalZuordnung = ausfahrSignal.Signal, DisplaySeite = DisplaySeite.Ausfahrt_Abschnitt
                });
            }

            var lastSignal = entries.LastOrDefault(x =>
                x.SignalZuordnung is not null &&
                x.SignalZuordnung.Signal.BetriebspunktId == betriebspunktZuordnung.Betriebspunkt.Id &&
                x.SignalZuordnung.Signal.Typ is SignalTyp.Hauptsignal or SignalTyp.Kombiniert or SignalTyp.Streckengeschwindigkeit &&
                x.DisplaySeite == DisplaySeite.Ausfahrt_Abschnitt);

            if (lastSignal is not null)
            {
                lastSignal.DisplaySeite = DisplaySeite.Ausfahrt;
            }
        }

        return entries;
    }
}