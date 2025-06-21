using Streckenbuch.Miku.Models.Fahrten;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Streckenbuch.Miku.Mocked;
public class Data
{
    private List<FahrtenRoot> Fahrten { get; set; }
    
    public void LoadData()
    {
        Fahrten = JsonSerializer.Deserialize<FahrtenRoot[]>(JSONDATA)!.ToList();
    }

    public FahrtenRoot? GetNextData()
    {
        if (Fahrten.Count == 0)
        {
            return null;
        }
        var fahrt = Fahrten[0];
        Fahrten.RemoveAt(0);

        return fahrt;
    }
    
    
    #region JSONDATA
    
    private const string JSONDATA = @"[
  {
    ""serverZeit"": ""2025-06-21T13:52:23.019Z"",
    ""betriebspunkt"": {
      ""bpUic"": 8500023,
      ""bezOff"": ""Liestal"",
      ""abk"": ""LST"",
      ""qosBitfeld"": 9183865
    },
    ""fahrten"": [
      {
        ""fahrtId"": {
          ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
          ""betriebstag"": ""2025-06-21""
        },
        ""fahrtInfo"": {
          ""zugnummer"": 17355,
          ""vmGruppe"": ""REGIONALVERKEHR"",
          ""vmArt"": ""S3"",
          ""tu"": ""11""
        },
        ""zuglaeufe"": [
          {
            ""fahrtId"": {
              ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
              ""betriebstag"": ""2025-06-21""
            },
            ""fahrtInfo"": {
              ""zugnummer"": 17355,
              ""vmGruppe"": ""REGIONALVERKEHR"",
              ""vmArt"": ""S3"",
              ""tu"": ""11""
            },
            ""haltestellen"": [
              {
                ""bpUic"": 8500010,
                ""abk"": ""BS"",
                ""bezeichnung"": ""Basel SBB"",
                ""flags"": [],
                ""qosBitfeld"": 533497,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:31:00Z"",
                  ""erw"": ""2025-06-21T13:37:00Z"",
                  ""ist"": ""2025-06-21T13:37:12Z""
                },
                ""ankunftsgleise"": {},
                ""abfahrtsgleise"": {
                  ""kb"": ""18"",
                  ""ist"": ""16"",
                  ""kbStatus"": ""AUFGEHOBEN"",
                  ""istStatus"": ""ERSATZ"",
                  ""abPerron"": ""16""
                },
                ""abfahrtsformation"": ""@A,F,F,[(12#NF$H,2#BHP;VH;KW;NF$H@B,2#NF$H,2)#NF$H]@C,F,F@D,F,F@E,F,F@F,F,F"",
                ""fahrzeugziele"": [],
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500020,
                ""abk"": ""MU"",
                ""bezeichnung"": ""Muttenz"",
                ""flags"": [],
                ""qosBitfeld"": 8921713,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:37:00Z"",
                  ""erw"": ""2025-06-21T13:41:53Z"",
                  ""ist"": ""2025-06-21T13:42:35Z""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:37:00Z"",
                  ""erw"": ""2025-06-21T13:41:24Z"",
                  ""ist"": ""2025-06-21T13:42:06Z""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""4"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""L"",
                  ""anPerron"": ""4""
                },
                ""abfahrtsformation"": ""@D,F,[(12#NF$H,2#BHP;VH;KW;NF$H@C,2#NF$H,2)#NF$H]@B,F,F@A,F,F,F"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500022,
                ""abk"": ""FRE"",
                ""bezeichnung"": ""Frenkendorf-Füllinsdorf"",
                ""flags"": [],
                ""qosBitfeld"": 8921713,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:44:00Z"",
                  ""erw"": ""2025-06-21T13:49:52Z"",
                  ""ist"": ""2025-06-21T13:50:26Z""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:44:00Z"",
                  ""erw"": ""2025-06-21T13:48:48Z"",
                  ""ist"": ""2025-06-21T13:49:29Z""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""1"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""L"",
                  ""anPerron"": ""1""
                },
                ""abfahrtsformation"": ""[(12#NF$H,2#BHP;VH;KW;NF$H,2#NF$H,2)#NF$H]"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500023,
                ""abk"": ""LST"",
                ""bezeichnung"": ""Liestal"",
                ""flags"": [],
                ""qosBitfeld"": 9183865,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:48:00Z"",
                  ""erw"": ""2025-06-21T13:52:26Z""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:47:00Z"",
                  ""erw"": ""2025-06-21T13:51:44Z"",
                  ""ist"": ""2025-06-21T13:51:56Z""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""1"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""L"",
                  ""anPerron"": ""1""
                },
                ""abfahrtsformation"": ""@A,F,F@B,F,F@C,F,F@D,F,[(12#NF$H@E,2#BHP;VH;KW;NF$H,2#NF$H,2)#NF$H]@F,F,F@G,F,F@H,F,F,F@J,F,F"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500024,
                ""abk"": ""LSN"",
                ""bezeichnung"": ""Lausen"",
                ""flags"": [],
                ""qosBitfeld"": 8921713,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:51:00Z"",
                  ""erw"": ""2025-06-21T13:54:58Z""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:51:00Z"",
                  ""erw"": ""2025-06-21T13:54:27Z""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""1"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""L"",
                  ""anPerron"": ""1""
                },
                ""abfahrtsformation"": ""[(12#NF$H,2#BHP;VH;KW;NF$H,2#NF$H,2)#NF$H]"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500025,
                ""abk"": ""IT"",
                ""bezeichnung"": ""Itingen"",
                ""flags"": [],
                ""qosBitfeld"": 533089,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:53:00Z"",
                  ""erw"": ""2025-06-21T13:57:08Z""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:53:00Z"",
                  ""erw"": ""2025-06-21T13:56:42Z""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""1"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""L"",
                  ""anPerron"": ""1""
                },
                ""abfahrtsformation"": ""[(12#NF$H,2#BHP;VH;KW;NF$H,2#NF$H,2)#NF$H]"",
                ""fahrzeugziele"": [],
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500026,
                ""abk"": ""SIS"",
                ""bezeichnung"": ""Sissach"",
                ""flags"": [],
                ""qosBitfeld"": 9183857,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:56:00Z"",
                  ""erw"": ""2025-06-21T13:59:24Z""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:56:00Z"",
                  ""erw"": ""2025-06-21T13:58:51Z""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""1"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""L"",
                  ""anPerron"": ""1""
                },
                ""abfahrtsformation"": ""@D,F,F@C,F,F,[(12#NF$M@B,2#BHP;VH;KW;NF$H,2#NF$M,2)#NF$M],F@A,F,F,F,F,F"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500027,
                ""abk"": ""GKD"",
                ""bezeichnung"": ""Gelterkinden"",
                ""flags"": [],
                ""qosBitfeld"": 9183857,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:59:00Z"",
                  ""erw"": ""2025-06-21T14:02:14Z""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:59:00Z"",
                  ""erw"": ""2025-06-21T14:01:43Z""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""4"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""R"",
                  ""anPerron"": ""2""
                },
                ""abfahrtsformation"": ""@D,F,F@C,[(12#NF$M,2#BHP;VH;KW;NF$H,2#NF$M,2)#NF$M],F@B,F,F,F@A,F,F,F,F"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500028,
                ""abk"": ""TK"",
                ""bezeichnung"": ""Tecknau"",
                ""flags"": [],
                ""qosBitfeld"": 8921713,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T14:02:00Z"",
                  ""erw"": ""2025-06-21T14:05:37Z""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T14:02:00Z"",
                  ""erw"": ""2025-06-21T14:05:01Z""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""3"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""R"",
                  ""anPerron"": ""3/4""
                },
                ""abfahrtsformation"": ""[(12#NF$M,2#BHP;VH;KW;NF$M,2#NF$M,2)#NF$M]"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500218,
                ""abk"": ""OL"",
                ""bezeichnung"": ""Olten"",
                ""flags"": [],
                ""qosBitfeld"": 8922105,
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T14:10:00Z"",
                  ""erw"": ""2025-06-21T14:11:54Z""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""11"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""R"",
                  ""anPerron"": ""10/11""
                },
                ""abfahrtsgleise"": {},
                ""ankunftsformation"": ""@D,F,F,F@C,F,F,F,[(12#NF,2#BHP;VH;KW;NF,2#NF@B,2)#NF],F,F@A,F,F,F,F,F,F,F"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              }
            ]
          }
        ]
      }
    ]
  },
  {
    ""serverZeit"": ""2025-06-21T13:52:23.019Z"",
    ""betriebspunkt"": {
      ""bpUic"": 8500023,
      ""bezOff"": ""Liestal"",
      ""abk"": ""LST"",
      ""qosBitfeld"": 9183865
    },
    ""fahrten"": [
      {
        ""fahrtId"": {
          ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
          ""betriebstag"": ""2025-06-21""
        },
        ""fahrtInfo"": {
          ""zugnummer"": 17355,
          ""vmGruppe"": ""REGIONALVERKEHR"",
          ""vmArt"": ""S3"",
          ""tu"": ""11""
        },
        ""zuglaeufe"": [
          {
            ""fahrtId"": {
              ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
              ""betriebstag"": ""2025-06-21""
            },
            ""fahrtInfo"": {
              ""zugnummer"": 17355,
              ""vmGruppe"": ""REGIONALVERKEHR"",
              ""vmArt"": ""S3"",
              ""tu"": ""11""
            },
            ""haltestellen"": [
              {
                ""bpUic"": 8500010,
                ""abk"": ""BS"",
                ""bezeichnung"": ""Basel SBB"",
                ""flags"": [],
                ""qosBitfeld"": 533497,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:31:00Z"",
                  ""erw"": ""2025-06-21T13:37:00Z"",
                  ""ist"": ""2025-06-21T13:37:12Z"",
                  ""verspaetung"": 1,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {},
                ""abfahrtsgleise"": {
                  ""kb"": ""18"",
                  ""ist"": ""16"",
                  ""kbStatus"": ""AUFGEHOBEN"",
                  ""istStatus"": ""ERSATZ"",
                  ""abPerron"": ""16""
                },
                ""abfahrtsformation"": ""@A,F,F,[(12#NF$H,2#BHP;VH;KW;NF$H@B,2#NF$H,2)#NF$H]@C,F,F@D,F,F@E,F,F@F,F,F"",
                ""fahrzeugziele"": [],
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500020,
                ""abk"": ""MU"",
                ""bezeichnung"": ""Muttenz"",
                ""flags"": [],
                ""qosBitfeld"": 8921713,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:37:00Z"",
                  ""erw"": ""2025-06-21T13:41:53Z"",
                  ""ist"": ""2025-06-21T13:42:35Z"",
                  ""verspaetung"": 2,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:37:00Z"",
                  ""erw"": ""2025-06-21T13:41:24Z"",
                  ""ist"": ""2025-06-21T13:42:06Z"",
                  ""verspaetung"": 1,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""4"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""L"",
                  ""anPerron"": ""4""
                },
                ""abfahrtsformation"": ""@D,F,[(12#NF$H,2#BHP;VH;KW;NF$H@C,2#NF$H,2)#NF$H]@B,F,F@A,F,F,F"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500022,
                ""abk"": ""FRE"",
                ""bezeichnung"": ""Frenkendorf-Füllinsdorf"",
                ""flags"": [],
                ""qosBitfeld"": 8921713,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:44:00Z"",
                  ""erw"": ""2025-06-21T13:49:52Z"",
                  ""ist"": ""2025-06-21T13:50:26Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:44:00Z"",
                  ""erw"": ""2025-06-21T13:48:48Z"",
                  ""ist"": ""2025-06-21T13:49:29Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""1"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""L"",
                  ""anPerron"": ""1""
                },
                ""abfahrtsformation"": ""[(12#NF$H,2#BHP;VH;KW;NF$H,2#NF$H,2)#NF$H]"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500023,
                ""abk"": ""LST"",
                ""bezeichnung"": ""Liestal"",
                ""flags"": [],
                ""qosBitfeld"": 9183865,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:48:00Z"",
                  ""erw"": ""2025-06-21T13:52:26Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:47:00Z"",
                  ""erw"": ""2025-06-21T13:51:44Z"",
                  ""ist"": ""2025-06-21T13:51:56Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""1"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""L"",
                  ""anPerron"": ""1""
                },
                ""abfahrtsformation"": ""@A,F,F@B,F,F@C,F,F@D,F,[(12#NF$H@E,2#BHP;VH;KW;NF$H,2#NF$H,2)#NF$H]@F,F,F@G,F,F@H,F,F,F@J,F,F"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500024,
                ""abk"": ""LSN"",
                ""bezeichnung"": ""Lausen"",
                ""flags"": [],
                ""qosBitfeld"": 8921713,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:51:00Z"",
                  ""erw"": ""2025-06-21T13:54:58Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:51:00Z"",
                  ""erw"": ""2025-06-21T13:54:27Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""1"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""L"",
                  ""anPerron"": ""1""
                },
                ""abfahrtsformation"": ""[(12#NF$H,2#BHP;VH;KW;NF$H,2#NF$H,2)#NF$H]"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500025,
                ""abk"": ""IT"",
                ""bezeichnung"": ""Itingen"",
                ""flags"": [],
                ""qosBitfeld"": 533089,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:53:00Z"",
                  ""erw"": ""2025-06-21T13:57:08Z"",
                  ""verspaetung"": 2,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:53:00Z"",
                  ""erw"": ""2025-06-21T13:56:42Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""1"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""L"",
                  ""anPerron"": ""1""
                },
                ""abfahrtsformation"": ""[(12#NF$H,2#BHP;VH;KW;NF$H,2#NF$H,2)#NF$H]"",
                ""fahrzeugziele"": [],
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500026,
                ""abk"": ""SIS"",
                ""bezeichnung"": ""Sissach"",
                ""flags"": [],
                ""qosBitfeld"": 9183857,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:56:00Z"",
                  ""erw"": ""2025-06-21T13:59:24Z"",
                  ""verspaetung"": 2,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:56:00Z"",
                  ""erw"": ""2025-06-21T13:58:51Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""1"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""L"",
                  ""anPerron"": ""1""
                },
                ""abfahrtsformation"": ""@D,F,F@C,F,F,[(12#NF$M@B,2#BHP;VH;KW;NF$H,2#NF$M,2)#NF$M],F@A,F,F,F,F,F"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500027,
                ""abk"": ""GKD"",
                ""bezeichnung"": ""Gelterkinden"",
                ""flags"": [],
                ""qosBitfeld"": 9183857,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:59:00Z"",
                  ""erw"": ""2025-06-21T14:02:14Z"",
                  ""verspaetung"": 2,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:59:00Z"",
                  ""erw"": ""2025-06-21T14:01:43Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""4"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""R"",
                  ""anPerron"": ""2""
                },
                ""abfahrtsformation"": ""@D,F,F@C,[(12#NF$M,2#BHP;VH;KW;NF$H,2#NF$M,2)#NF$M],F@B,F,F,F@A,F,F,F,F"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500028,
                ""abk"": ""TK"",
                ""bezeichnung"": ""Tecknau"",
                ""flags"": [],
                ""qosBitfeld"": 8921713,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T14:02:00Z"",
                  ""erw"": ""2025-06-21T14:05:37Z"",
                  ""verspaetung"": 1,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T14:02:00Z"",
                  ""erw"": ""2025-06-21T14:05:01Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""3"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""R"",
                  ""anPerron"": ""3/4""
                },
                ""abfahrtsformation"": ""[(12#NF$M,2#BHP;VH;KW;NF$M,2#NF$M,2)#NF$M]"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500218,
                ""abk"": ""OL"",
                ""bezeichnung"": ""Olten"",
                ""flags"": [],
                ""qosBitfeld"": 8922105,
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T14:10:00Z"",
                  ""erw"": ""2025-06-21T14:11:54Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""11"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""R"",
                  ""anPerron"": ""10/11""
                },
                ""abfahrtsgleise"": {},
                ""ankunftsformation"": ""@D,F,F,F@C,F,F,F,[(12#NF,2#BHP;VH;KW;NF,2#NF@B,2)#NF],F,F@A,F,F,F,F,F,F,F"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              }
            ]
          }
        ]
      }
    ]
  },
  {
    ""serverZeit"": ""2025-06-21T13:52:23.019Z"",
    ""betriebspunkt"": {
      ""bpUic"": 8500023,
      ""bezOff"": ""Liestal"",
      ""abk"": ""LST"",
      ""qosBitfeld"": 9183865
    },
    ""fahrten"": [
      {
        ""fahrtId"": {
          ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
          ""betriebstag"": ""2025-06-21""
        },
        ""fahrtInfo"": {
          ""zugnummer"": 17355,
          ""vmGruppe"": ""REGIONALVERKEHR"",
          ""vmArt"": ""S3"",
          ""tu"": ""11""
        },
        ""zuglaeufe"": [
          {
            ""fahrtId"": {
              ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
              ""betriebstag"": ""2025-06-21""
            },
            ""fahrtInfo"": {
              ""zugnummer"": 17355,
              ""vmGruppe"": ""REGIONALVERKEHR"",
              ""vmArt"": ""S3"",
              ""tu"": ""11""
            },
            ""haltestellen"": [
              {
                ""bpUic"": 8500010,
                ""abk"": ""BS"",
                ""bezeichnung"": ""Basel SBB"",
                ""flags"": [],
                ""qosBitfeld"": 533497,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:31:00Z"",
                  ""erw"": ""2025-06-21T13:37:00Z"",
                  ""ist"": ""2025-06-21T13:37:12Z"",
                  ""verspaetung"": 6,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {},
                ""abfahrtsgleise"": {
                  ""kb"": ""18"",
                  ""ist"": ""16"",
                  ""kbStatus"": ""AUFGEHOBEN"",
                  ""istStatus"": ""ERSATZ"",
                  ""abPerron"": ""16""
                },
                ""abfahrtsformation"": ""@A,F,F,[(12#NF$H,2#BHP;VH;KW;NF$H@B,2#NF$H,2)#NF$H]@C,F,F@D,F,F@E,F,F@F,F,F"",
                ""fahrzeugziele"": [],
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500020,
                ""abk"": ""MU"",
                ""bezeichnung"": ""Muttenz"",
                ""flags"": [],
                ""qosBitfeld"": 8921713,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:37:00Z"",
                  ""erw"": ""2025-06-21T13:41:53Z"",
                  ""ist"": ""2025-06-21T13:42:35Z"",
                  ""verspaetung"": 5,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:37:00Z"",
                  ""erw"": ""2025-06-21T13:41:24Z"",
                  ""ist"": ""2025-06-21T13:42:06Z"",
                  ""verspaetung"": 4,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""4"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""L"",
                  ""anPerron"": ""4""
                },
                ""abfahrtsformation"": ""@D,F,[(12#NF$H,2#BHP;VH;KW;NF$H@C,2#NF$H,2)#NF$H]@B,F,F@A,F,F,F"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500022,
                ""abk"": ""FRE"",
                ""bezeichnung"": ""Frenkendorf-Füllinsdorf"",
                ""flags"": [],
                ""qosBitfeld"": 8921713,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:44:00Z"",
                  ""erw"": ""2025-06-21T13:49:52Z"",
                  ""ist"": ""2025-06-21T13:50:26Z"",
                  ""verspaetung"": 6,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:44:00Z"",
                  ""erw"": ""2025-06-21T13:48:48Z"",
                  ""ist"": ""2025-06-21T13:49:29Z"",
                  ""verspaetung"": 5,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""1"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""L"",
                  ""anPerron"": ""1""
                },
                ""abfahrtsformation"": ""[(12#NF$H,2#BHP;VH;KW;NF$H,2#NF$H,2)#NF$H]"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500023,
                ""abk"": ""LST"",
                ""bezeichnung"": ""Liestal"",
                ""flags"": [],
                ""qosBitfeld"": 9183865,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:48:00Z"",
                  ""erw"": ""2025-06-21T13:52:26Z"",
                  ""verspaetung"": 4,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:47:00Z"",
                  ""erw"": ""2025-06-21T13:51:44Z"",
                  ""ist"": ""2025-06-21T13:51:56Z"",
                  ""verspaetung"": 5,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""1"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""L"",
                  ""anPerron"": ""1""
                },
                ""abfahrtsformation"": ""@A,F,F@B,F,F@C,F,F@D,F,[(12#NF$H@E,2#BHP;VH;KW;NF$H,2#NF$H,2)#NF$H]@F,F,F@G,F,F@H,F,F,F@J,F,F"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500024,
                ""abk"": ""LSN"",
                ""bezeichnung"": ""Lausen"",
                ""flags"": [],
                ""qosBitfeld"": 8921713,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:51:00Z"",
                  ""erw"": ""2025-06-21T13:54:58Z"",
                  ""verspaetung"": 4,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:51:00Z"",
                  ""erw"": ""2025-06-21T13:54:27Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""1"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""L"",
                  ""anPerron"": ""1""
                },
                ""abfahrtsformation"": ""[(12#NF$H,2#BHP;VH;KW;NF$H,2#NF$H,2)#NF$H]"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500025,
                ""abk"": ""IT"",
                ""bezeichnung"": ""Itingen"",
                ""flags"": [],
                ""qosBitfeld"": 533089,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:53:00Z"",
                  ""erw"": ""2025-06-21T13:57:08Z"",
                  ""verspaetung"": 4,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:53:00Z"",
                  ""erw"": ""2025-06-21T13:56:42Z"",
                  ""verspaetung"": 4,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""1"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""L"",
                  ""anPerron"": ""1""
                },
                ""abfahrtsformation"": ""[(12#NF$H,2#BHP;VH;KW;NF$H,2#NF$H,2)#NF$H]"",
                ""fahrzeugziele"": [],
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500026,
                ""abk"": ""SIS"",
                ""bezeichnung"": ""Sissach"",
                ""flags"": [],
                ""qosBitfeld"": 9183857,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:56:00Z"",
                  ""erw"": ""2025-06-21T13:59:24Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:56:00Z"",
                  ""erw"": ""2025-06-21T13:58:51Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""1"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""L"",
                  ""anPerron"": ""1""
                },
                ""abfahrtsformation"": ""@D,F,F@C,F,F,[(12#NF$M@B,2#BHP;VH;KW;NF$H,2#NF$M,2)#NF$M],F@A,F,F,F,F,F"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500027,
                ""abk"": ""GKD"",
                ""bezeichnung"": ""Gelterkinden"",
                ""flags"": [],
                ""qosBitfeld"": 9183857,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:59:00Z"",
                  ""erw"": ""2025-06-21T14:02:14Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:59:00Z"",
                  ""erw"": ""2025-06-21T14:01:43Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""4"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""R"",
                  ""anPerron"": ""2""
                },
                ""abfahrtsformation"": ""@D,F,F@C,[(12#NF$M,2#BHP;VH;KW;NF$H,2#NF$M,2)#NF$M],F@B,F,F,F@A,F,F,F,F"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500028,
                ""abk"": ""TK"",
                ""bezeichnung"": ""Tecknau"",
                ""flags"": [],
                ""qosBitfeld"": 8921713,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T14:02:00Z"",
                  ""erw"": ""2025-06-21T14:05:37Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T14:02:00Z"",
                  ""erw"": ""2025-06-21T14:05:01Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""3"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""R"",
                  ""anPerron"": ""3/4""
                },
                ""abfahrtsformation"": ""[(12#NF$M,2#BHP;VH;KW;NF$M,2#NF$M,2)#NF$M]"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500218,
                ""abk"": ""OL"",
                ""bezeichnung"": ""Olten"",
                ""flags"": [],
                ""qosBitfeld"": 8922105,
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T14:10:00Z"",
                  ""erw"": ""2025-06-21T14:11:54Z"",
                  ""verspaetung"": 2,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""11"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""R"",
                  ""anPerron"": ""10/11""
                },
                ""abfahrtsgleise"": {},
                ""ankunftsformation"": ""@D,F,F,F@C,F,F,F,[(12#NF,2#BHP;VH;KW;NF,2#NF@B,2)#NF],F,F@A,F,F,F,F,F,F,F"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              }
            ]
          }
        ]
      }
    ]
  },
  {
    ""serverZeit"": ""2025-06-21T13:52:23.019Z"",
    ""betriebspunkt"": {
      ""bpUic"": 8500023,
      ""bezOff"": ""Liestal"",
      ""abk"": ""LST"",
      ""qosBitfeld"": 9183865
    },
    ""fahrten"": [
      {
        ""fahrtId"": {
          ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
          ""betriebstag"": ""2025-06-21""
        },
        ""fahrtInfo"": {
          ""zugnummer"": 17355,
          ""vmGruppe"": ""REGIONALVERKEHR"",
          ""vmArt"": ""S3"",
          ""tu"": ""11""
        },
        ""zuglaeufe"": [
          {
            ""fahrtId"": {
              ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
              ""betriebstag"": ""2025-06-21""
            },
            ""fahrtInfo"": {
              ""zugnummer"": 17355,
              ""vmGruppe"": ""REGIONALVERKEHR"",
              ""vmArt"": ""S3"",
              ""tu"": ""11""
            },
            ""haltestellen"": [
              {
                ""bpUic"": 8500010,
                ""abk"": ""BS"",
                ""bezeichnung"": ""Basel SBB"",
                ""flags"": [],
                ""verspaetungsgrund"": {
                  ""id"": ""EG_121"",
                  ""de"": ""Personaldisposition"",
                  ""fr"": ""Disposition personnel"",
                  ""it"": ""Disposizione personale""
                },
                ""qosBitfeld"": 533497,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:31:00Z"",
                  ""erw"": ""2025-06-21T13:37:00Z"",
                  ""ist"": ""2025-06-21T13:37:12Z"",
                  ""verspaetung"": 6,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {},
                ""abfahrtsgleise"": {
                  ""kb"": ""18"",
                  ""ist"": ""16"",
                  ""kbStatus"": ""AUFGEHOBEN"",
                  ""istStatus"": ""ERSATZ"",
                  ""abPerron"": ""16""
                },
                ""abfahrtsformation"": ""@A,F,F,[(12#NF$H,2#BHP;VH;KW;NF$H@B,2#NF$H,2)#NF$H]@C,F,F@D,F,F@E,F,F@F,F,F"",
                ""fahrzeugziele"": [],
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500020,
                ""abk"": ""MU"",
                ""bezeichnung"": ""Muttenz"",
                ""flags"": [],
                ""verspaetungsgrund"": {
                  ""id"": ""EG_121"",
                  ""de"": ""Personaldisposition"",
                  ""fr"": ""Disposition personnel"",
                  ""it"": ""Disposizione personale""
                },
                ""qosBitfeld"": 8921713,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:37:00Z"",
                  ""erw"": ""2025-06-21T13:41:53Z"",
                  ""ist"": ""2025-06-21T13:42:35Z"",
                  ""verspaetung"": 5,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:37:00Z"",
                  ""erw"": ""2025-06-21T13:41:24Z"",
                  ""ist"": ""2025-06-21T13:42:06Z"",
                  ""verspaetung"": 4,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""4"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""L"",
                  ""anPerron"": ""4""
                },
                ""abfahrtsformation"": ""@D,F,[(12#NF$H,2#BHP;VH;KW;NF$H@C,2#NF$H,2)#NF$H]@B,F,F@A,F,F,F"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500022,
                ""abk"": ""FRE"",
                ""bezeichnung"": ""Frenkendorf-Füllinsdorf"",
                ""flags"": [],
                ""verspaetungsgrund"": {
                  ""id"": ""EG_121"",
                  ""de"": ""Personaldisposition"",
                  ""fr"": ""Disposition personnel"",
                  ""it"": ""Disposizione personale""
                },
                ""qosBitfeld"": 8921713,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:44:00Z"",
                  ""erw"": ""2025-06-21T13:49:52Z"",
                  ""ist"": ""2025-06-21T13:50:26Z"",
                  ""verspaetung"": 6,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:44:00Z"",
                  ""erw"": ""2025-06-21T13:48:48Z"",
                  ""ist"": ""2025-06-21T13:49:29Z"",
                  ""verspaetung"": 5,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""1"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""L"",
                  ""anPerron"": ""1""
                },
                ""abfahrtsformation"": ""[(12#NF$H,2#BHP;VH;KW;NF$H,2#NF$H,2)#NF$H]"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500023,
                ""abk"": ""LST"",
                ""bezeichnung"": ""Liestal"",
                ""flags"": [],
                ""verspaetungsgrund"": {
                  ""id"": ""EG_121"",
                  ""de"": ""Personaldisposition"",
                  ""fr"": ""Disposition personnel"",
                  ""it"": ""Disposizione personale""
                },
                ""qosBitfeld"": 9183865,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:48:00Z"",
                  ""erw"": ""2025-06-21T13:52:26Z"",
                  ""verspaetung"": 4,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:47:00Z"",
                  ""erw"": ""2025-06-21T13:51:44Z"",
                  ""ist"": ""2025-06-21T13:51:56Z"",
                  ""verspaetung"": 5,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""1"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""L"",
                  ""anPerron"": ""1""
                },
                ""abfahrtsformation"": ""@A,F,F@B,F,F@C,F,F@D,F,[(12#NF$H@E,2#BHP;VH;KW;NF$H,2#NF$H,2)#NF$H]@F,F,F@G,F,F@H,F,F,F@J,F,F"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500024,
                ""abk"": ""LSN"",
                ""bezeichnung"": ""Lausen"",
                ""flags"": [],
                ""verspaetungsgrund"": {
                  ""id"": ""EG_121"",
                  ""de"": ""Personaldisposition"",
                  ""fr"": ""Disposition personnel"",
                  ""it"": ""Disposizione personale""
                },
                ""qosBitfeld"": 8921713,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:51:00Z"",
                  ""erw"": ""2025-06-21T13:54:58Z"",
                  ""verspaetung"": 4,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:51:00Z"",
                  ""erw"": ""2025-06-21T13:54:27Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""1"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""L"",
                  ""anPerron"": ""1""
                },
                ""abfahrtsformation"": ""[(12#NF$H,2#BHP;VH;KW;NF$H,2#NF$H,2)#NF$H]"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500025,
                ""abk"": ""IT"",
                ""bezeichnung"": ""Itingen"",
                ""flags"": [],
                ""verspaetungsgrund"": {
                  ""id"": ""EG_121"",
                  ""de"": ""Personaldisposition"",
                  ""fr"": ""Disposition personnel"",
                  ""it"": ""Disposizione personale""
                },
                ""qosBitfeld"": 533089,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:53:00Z"",
                  ""erw"": ""2025-06-21T13:57:08Z"",
                  ""verspaetung"": 4,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:53:00Z"",
                  ""erw"": ""2025-06-21T13:56:42Z"",
                  ""verspaetung"": 4,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""1"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""L"",
                  ""anPerron"": ""1""
                },
                ""abfahrtsformation"": ""[(12#NF$H,2#BHP;VH;KW;NF$H,2#NF$H,2)#NF$H]"",
                ""fahrzeugziele"": [],
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500026,
                ""abk"": ""SIS"",
                ""bezeichnung"": ""Sissach"",
                ""flags"": [],
                ""verspaetungsgrund"": {
                  ""id"": ""EG_121"",
                  ""de"": ""Personaldisposition"",
                  ""fr"": ""Disposition personnel"",
                  ""it"": ""Disposizione personale""
                },
                ""qosBitfeld"": 9183857,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:56:00Z"",
                  ""erw"": ""2025-06-21T13:59:24Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:56:00Z"",
                  ""erw"": ""2025-06-21T13:58:51Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""1"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""L"",
                  ""anPerron"": ""1""
                },
                ""abfahrtsformation"": ""@D,F,F@C,F,F,[(12#NF$M@B,2#BHP;VH;KW;NF$H,2#NF$M,2)#NF$M],F@A,F,F,F,F,F"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500027,
                ""abk"": ""GKD"",
                ""bezeichnung"": ""Gelterkinden"",
                ""flags"": [],
                ""verspaetungsgrund"": {
                  ""id"": ""EG_121"",
                  ""de"": ""Personaldisposition"",
                  ""fr"": ""Disposition personnel"",
                  ""it"": ""Disposizione personale""
                },
                ""qosBitfeld"": 9183857,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:59:00Z"",
                  ""erw"": ""2025-06-21T14:02:14Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:59:00Z"",
                  ""erw"": ""2025-06-21T14:01:43Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""4"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""R"",
                  ""anPerron"": ""2""
                },
                ""abfahrtsformation"": ""@D,F,F@C,[(12#NF$M,2#BHP;VH;KW;NF$H,2#NF$M,2)#NF$M],F@B,F,F,F@A,F,F,F,F"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500028,
                ""abk"": ""TK"",
                ""bezeichnung"": ""Tecknau"",
                ""flags"": [],
                ""verspaetungsgrund"": {
                  ""id"": ""EG_121"",
                  ""de"": ""Personaldisposition"",
                  ""fr"": ""Disposition personnel"",
                  ""it"": ""Disposizione personale""
                },
                ""qosBitfeld"": 8921713,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T14:02:00Z"",
                  ""erw"": ""2025-06-21T14:05:37Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T14:02:00Z"",
                  ""erw"": ""2025-06-21T14:05:01Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""3"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""R"",
                  ""anPerron"": ""3/4""
                },
                ""abfahrtsformation"": ""[(12#NF$M,2#BHP;VH;KW;NF$M,2#NF$M,2)#NF$M]"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500218,
                ""abk"": ""OL"",
                ""bezeichnung"": ""Olten"",
                ""flags"": [],
                ""qosBitfeld"": 8922105,
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T14:10:00Z"",
                  ""erw"": ""2025-06-21T14:11:54Z"",
                  ""verspaetung"": 2,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""11"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""R"",
                  ""anPerron"": ""10/11""
                },
                ""abfahrtsgleise"": {},
                ""ankunftsformation"": ""@D,F,F,F@C,F,F,F,[(12#NF,2#BHP;VH;KW;NF,2#NF@B,2)#NF],F,F@A,F,F,F,F,F,F,F"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              }
            ]
          }
        ]
      }
    ]
  },
  {
    ""serverZeit"": ""2025-06-21T13:52:23.019Z"",
    ""betriebspunkt"": {
      ""bpUic"": 8500023,
      ""bezOff"": ""Liestal"",
      ""abk"": ""LST"",
      ""qosBitfeld"": 9183865
    },
    ""fahrten"": [
      {
        ""fahrtId"": {
          ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
          ""betriebstag"": ""2025-06-21""
        },
        ""fahrtInfo"": {
          ""zugnummer"": 17355,
          ""vmGruppe"": ""REGIONALVERKEHR"",
          ""vmArt"": ""S3"",
          ""tu"": ""11""
        },
        ""zuglaeufe"": [
          {
            ""fahrtId"": {
              ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
              ""betriebstag"": ""2025-06-21""
            },
            ""fahrtInfo"": {
              ""zugnummer"": 17355,
              ""vmGruppe"": ""REGIONALVERKEHR"",
              ""vmArt"": ""S3"",
              ""tu"": ""11""
            },
            ""haltestellen"": [
              {
                ""bpUic"": 8500010,
                ""abk"": ""BS"",
                ""bezeichnung"": ""Basel SBB"",
                ""flags"": [],
                ""qosBitfeld"": 533497,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:31:00Z"",
                  ""erw"": ""2025-06-21T13:37:00Z"",
                  ""ist"": ""2025-06-21T13:37:12Z"",
                  ""verspaetung"": 1,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {},
                ""abfahrtsgleise"": {
                  ""kb"": ""18"",
                  ""ist"": ""16"",
                  ""kbStatus"": ""AUFGEHOBEN"",
                  ""istStatus"": ""ERSATZ"",
                  ""abPerron"": ""16""
                },
                ""abfahrtsformation"": ""@A,F,F,[(12#NF$H,2#BHP;VH;KW;NF$H@B,2#NF$H,2)#NF$H]@C,F,F@D,F,F@E,F,F@F,F,F"",
                ""fahrzeugziele"": [],
                ""halteCode"": ""HALT""
              },
              {
                ""ausfallgrund"": {
                  ""id"": ""EG_004"",
                  ""de"": ""Bauarbeiten"",
                  ""fr"": ""Travaux"",
                  ""it"": ""Lavori di costruzione""
                },
                ""bpUic"": 8500020,
                ""abk"": ""MU"",
                ""bezeichnung"": ""Muttenz"",
                ""flags"": [
                  ""AUSFALL""
                ],
                ""qosBitfeld"": 8921713,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:37:00Z"",
                  ""erw"": ""2025-06-21T13:41:53Z"",
                  ""ist"": ""2025-06-21T13:42:35Z"",
                  ""status"": ""AUSFALL""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:37:00Z"",
                  ""erw"": ""2025-06-21T13:41:24Z"",
                  ""ist"": ""2025-06-21T13:42:06Z"",
                  ""status"": ""AUSFALL""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""4"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""L"",
                  ""anPerron"": ""4""
                },
                ""abfahrtsformation"": ""@D,F,[(12#NF$H,2#BHP;VH;KW;NF$H@C,2#NF$H,2)#NF$H]@B,F,F@A,F,F,F"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500022,
                ""abk"": ""FRE"",
                ""bezeichnung"": ""Frenkendorf-Füllinsdorf"",
                ""flags"": [],
                ""qosBitfeld"": 8921713,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:44:00Z"",
                  ""erw"": ""2025-06-21T13:49:52Z"",
                  ""ist"": ""2025-06-21T13:50:26Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:44:00Z"",
                  ""erw"": ""2025-06-21T13:48:48Z"",
                  ""ist"": ""2025-06-21T13:49:29Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""1"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""L"",
                  ""anPerron"": ""1""
                },
                ""abfahrtsformation"": ""[(12#NF$H,2#BHP;VH;KW;NF$H,2#NF$H,2)#NF$H]"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500023,
                ""abk"": ""LST"",
                ""bezeichnung"": ""Liestal"",
                ""flags"": [],
                ""qosBitfeld"": 9183865,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:48:00Z"",
                  ""erw"": ""2025-06-21T13:52:26Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:47:00Z"",
                  ""erw"": ""2025-06-21T13:51:44Z"",
                  ""ist"": ""2025-06-21T13:51:56Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""1"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""L"",
                  ""anPerron"": ""1""
                },
                ""abfahrtsformation"": ""@A,F,F@B,F,F@C,F,F@D,F,[(12#NF$H@E,2#BHP;VH;KW;NF$H,2#NF$H,2)#NF$H]@F,F,F@G,F,F@H,F,F,F@J,F,F"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500024,
                ""abk"": ""LSN"",
                ""bezeichnung"": ""Lausen"",
                ""flags"": [],
                ""qosBitfeld"": 8921713,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:51:00Z"",
                  ""erw"": ""2025-06-21T13:54:58Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:51:00Z"",
                  ""erw"": ""2025-06-21T13:54:27Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""1"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""L"",
                  ""anPerron"": ""1""
                },
                ""abfahrtsformation"": ""[(12#NF$H,2#BHP;VH;KW;NF$H,2#NF$H,2)#NF$H]"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500025,
                ""abk"": ""IT"",
                ""bezeichnung"": ""Itingen"",
                ""flags"": [],
                ""qosBitfeld"": 533089,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:53:00Z"",
                  ""erw"": ""2025-06-21T13:57:08Z"",
                  ""verspaetung"": 2,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:53:00Z"",
                  ""erw"": ""2025-06-21T13:56:42Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""1"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""L"",
                  ""anPerron"": ""1""
                },
                ""abfahrtsformation"": ""[(12#NF$H,2#BHP;VH;KW;NF$H,2#NF$H,2)#NF$H]"",
                ""fahrzeugziele"": [],
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500026,
                ""abk"": ""SIS"",
                ""bezeichnung"": ""Sissach"",
                ""flags"": [],
                ""qosBitfeld"": 9183857,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:56:00Z"",
                  ""erw"": ""2025-06-21T13:59:24Z"",
                  ""verspaetung"": 2,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:56:00Z"",
                  ""erw"": ""2025-06-21T13:58:51Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""1"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""L"",
                  ""anPerron"": ""1""
                },
                ""abfahrtsformation"": ""@D,F,F@C,F,F,[(12#NF$M@B,2#BHP;VH;KW;NF$H,2#NF$M,2)#NF$M],F@A,F,F,F,F,F"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500027,
                ""abk"": ""GKD"",
                ""bezeichnung"": ""Gelterkinden"",
                ""flags"": [],
                ""qosBitfeld"": 9183857,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:59:00Z"",
                  ""erw"": ""2025-06-21T14:02:14Z"",
                  ""verspaetung"": 2,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:59:00Z"",
                  ""erw"": ""2025-06-21T14:01:43Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""4"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""R"",
                  ""anPerron"": ""2""
                },
                ""abfahrtsformation"": ""@D,F,F@C,[(12#NF$M,2#BHP;VH;KW;NF$H,2#NF$M,2)#NF$M],F@B,F,F,F@A,F,F,F,F"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500028,
                ""abk"": ""TK"",
                ""bezeichnung"": ""Tecknau"",
                ""flags"": [],
                ""qosBitfeld"": 8921713,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T14:02:00Z"",
                  ""erw"": ""2025-06-21T14:05:37Z"",
                  ""verspaetung"": 1,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T14:02:00Z"",
                  ""erw"": ""2025-06-21T14:05:01Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""3"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""R"",
                  ""anPerron"": ""3/4""
                },
                ""abfahrtsformation"": ""[(12#NF$M,2#BHP;VH;KW;NF$M,2#NF$M,2)#NF$M]"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500218,
                ""abk"": ""OL"",
                ""bezeichnung"": ""Olten"",
                ""flags"": [],
                ""qosBitfeld"": 8922105,
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T14:10:00Z"",
                  ""erw"": ""2025-06-21T14:11:54Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""11"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""R"",
                  ""anPerron"": ""10/11""
                },
                ""abfahrtsgleise"": {},
                ""ankunftsformation"": ""@D,F,F,F@C,F,F,F,[(12#NF,2#BHP;VH;KW;NF,2#NF@B,2)#NF],F,F@A,F,F,F,F,F,F,F"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              }
            ]
          }
        ]
      }
    ]
  },
  {
    ""serverZeit"": ""2025-06-21T13:52:23.019Z"",
    ""betriebspunkt"": {
      ""bpUic"": 8500023,
      ""bezOff"": ""Liestal"",
      ""abk"": ""LST"",
      ""qosBitfeld"": 9183865
    },
    ""fahrten"": [
      {
        ""fahrtId"": {
          ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
          ""betriebstag"": ""2025-06-21""
        },
        ""fahrtInfo"": {
          ""zugnummer"": 17355,
          ""vmGruppe"": ""REGIONALVERKEHR"",
          ""vmArt"": ""S3"",
          ""tu"": ""11""
        },
        ""zuglaeufe"": [
          {
            ""fahrtId"": {
              ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
              ""betriebstag"": ""2025-06-21""
            },
            ""fahrtInfo"": {
              ""zugnummer"": 17355,
              ""vmGruppe"": ""REGIONALVERKEHR"",
              ""vmArt"": ""S3"",
              ""tu"": ""11""
            },
            ""haltestellen"": [
              {
                ""bpUic"": 8500010,
                ""abk"": ""BS"",
                ""bezeichnung"": ""Basel SBB"",
                ""flags"": [],
                ""qosBitfeld"": 533497,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:31:00Z"",
                  ""erw"": ""2025-06-21T13:37:00Z"",
                  ""ist"": ""2025-06-21T13:37:12Z"",
                  ""verspaetung"": 1,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {},
                ""abfahrtsgleise"": {
                  ""kb"": ""18"",
                  ""ist"": ""16"",
                  ""kbStatus"": ""AUFGEHOBEN"",
                  ""istStatus"": ""ERSATZ"",
                  ""abPerron"": ""16""
                },
                ""abfahrtsformation"": ""@A,F,F,[(12#NF$H,2#BHP;VH;KW;NF$H@B,2#NF$H,2)#NF$H]@C,F,F@D,F,F@E,F,F@F,F,F"",
                ""fahrzeugziele"": [],
                ""halteCode"": ""HALT""
              },
              {
                ""ausfallgrund"": {
                  ""id"": ""EG_004"",
                  ""de"": ""Bauarbeiten"",
                  ""fr"": ""Travaux"",
                  ""it"": ""Lavori di costruzione""
                },
                ""bpUic"": 8500020,
                ""abk"": ""MU"",
                ""bezeichnung"": ""Muttenz"",
                ""flags"": [
                  ""AUSFALL""
                ],
                ""qosBitfeld"": 8921713,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:37:00Z"",
                  ""erw"": ""2025-06-21T13:41:53Z"",
                  ""ist"": ""2025-06-21T13:42:35Z"",
                  ""status"": ""AUSFALL""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:37:00Z"",
                  ""erw"": ""2025-06-21T13:41:24Z"",
                  ""ist"": ""2025-06-21T13:42:06Z"",
                  ""status"": ""AUSFALL""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""4"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""L"",
                  ""anPerron"": ""4""
                },
                ""abfahrtsformation"": ""@D,F,[(12#NF$H,2#BHP;VH;KW;NF$H@C,2#NF$H,2)#NF$H]@B,F,F@A,F,F,F"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500021,
                ""abk"": ""PR"",
                ""bezeichnung"": ""Pratteln"",
                ""flags"": [
                  ""AO_HALT""
                ],
                ""qosBitfeld"": 8921713,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:41:00Z"",
                  ""erw"": ""2025-06-21T13:45:35Z"",
                  ""ist"": ""2025-06-21T13:46:39Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:41:00Z"",
                  ""erw"": ""2025-06-21T13:45:02Z"",
                  ""ist"": ""2025-06-21T13:45:33Z"",
                  ""verspaetung"": 2,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""2"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""L"",
                  ""anPerron"": ""2/4""
                },
                ""abfahrtsformation"": ""[(12#NF$H,2#BHP;VH;KW;NF$H,2#NF$H,2)#NF$H]"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500022,
                ""abk"": ""FRE"",
                ""bezeichnung"": ""Frenkendorf-Füllinsdorf"",
                ""flags"": [],
                ""qosBitfeld"": 8921713,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:44:00Z"",
                  ""erw"": ""2025-06-21T13:49:52Z"",
                  ""ist"": ""2025-06-21T13:50:26Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:44:00Z"",
                  ""erw"": ""2025-06-21T13:48:48Z"",
                  ""ist"": ""2025-06-21T13:49:29Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""1"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""L"",
                  ""anPerron"": ""1""
                },
                ""abfahrtsformation"": ""[(12#NF$H,2#BHP;VH;KW;NF$H,2#NF$H,2)#NF$H]"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500023,
                ""abk"": ""LST"",
                ""bezeichnung"": ""Liestal"",
                ""flags"": [],
                ""qosBitfeld"": 9183865,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:48:00Z"",
                  ""erw"": ""2025-06-21T13:52:26Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:47:00Z"",
                  ""erw"": ""2025-06-21T13:51:44Z"",
                  ""ist"": ""2025-06-21T13:51:56Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""1"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""L"",
                  ""anPerron"": ""1""
                },
                ""abfahrtsformation"": ""@A,F,F@B,F,F@C,F,F@D,F,[(12#NF$H@E,2#BHP;VH;KW;NF$H,2#NF$H,2)#NF$H]@F,F,F@G,F,F@H,F,F,F@J,F,F"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500024,
                ""abk"": ""LSN"",
                ""bezeichnung"": ""Lausen"",
                ""flags"": [],
                ""qosBitfeld"": 8921713,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:51:00Z"",
                  ""erw"": ""2025-06-21T13:54:58Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:51:00Z"",
                  ""erw"": ""2025-06-21T13:54:27Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""1"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""L"",
                  ""anPerron"": ""1""
                },
                ""abfahrtsformation"": ""[(12#NF$H,2#BHP;VH;KW;NF$H,2#NF$H,2)#NF$H]"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500025,
                ""abk"": ""IT"",
                ""bezeichnung"": ""Itingen"",
                ""flags"": [],
                ""qosBitfeld"": 533089,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:53:00Z"",
                  ""erw"": ""2025-06-21T13:57:08Z"",
                  ""verspaetung"": 2,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:53:00Z"",
                  ""erw"": ""2025-06-21T13:56:42Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""1"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""L"",
                  ""anPerron"": ""1""
                },
                ""abfahrtsformation"": ""[(12#NF$H,2#BHP;VH;KW;NF$H,2#NF$H,2)#NF$H]"",
                ""fahrzeugziele"": [],
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500026,
                ""abk"": ""SIS"",
                ""bezeichnung"": ""Sissach"",
                ""flags"": [],
                ""qosBitfeld"": 9183857,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:56:00Z"",
                  ""erw"": ""2025-06-21T13:59:24Z"",
                  ""verspaetung"": 2,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:56:00Z"",
                  ""erw"": ""2025-06-21T13:58:51Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""1"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""L"",
                  ""anPerron"": ""1""
                },
                ""abfahrtsformation"": ""@D,F,F@C,F,F,[(12#NF$M@B,2#BHP;VH;KW;NF$H,2#NF$M,2)#NF$M],F@A,F,F,F,F,F"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500027,
                ""abk"": ""GKD"",
                ""bezeichnung"": ""Gelterkinden"",
                ""flags"": [],
                ""qosBitfeld"": 9183857,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T13:59:00Z"",
                  ""erw"": ""2025-06-21T14:02:14Z"",
                  ""verspaetung"": 2,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T13:59:00Z"",
                  ""erw"": ""2025-06-21T14:01:43Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""4"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""R"",
                  ""anPerron"": ""2""
                },
                ""abfahrtsformation"": ""@D,F,F@C,[(12#NF$M,2#BHP;VH;KW;NF$H,2#NF$M,2)#NF$M],F@B,F,F,F@A,F,F,F,F"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500028,
                ""abk"": ""TK"",
                ""bezeichnung"": ""Tecknau"",
                ""flags"": [],
                ""qosBitfeld"": 8921713,
                ""abfahrtszeiten"": {
                  ""kb"": ""2025-06-21T14:02:00Z"",
                  ""erw"": ""2025-06-21T14:05:37Z"",
                  ""verspaetung"": 1,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T14:02:00Z"",
                  ""erw"": ""2025-06-21T14:05:01Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""3"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""R"",
                  ""anPerron"": ""3/4""
                },
                ""abfahrtsformation"": ""[(12#NF$M,2#BHP;VH;KW;NF$M,2#NF$M,2)#NF$M]"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              },
              {
                ""bpUic"": 8500218,
                ""abk"": ""OL"",
                ""bezeichnung"": ""Olten"",
                ""flags"": [],
                ""qosBitfeld"": 8922105,
                ""ankunftszeiten"": {
                  ""kb"": ""2025-06-21T14:10:00Z"",
                  ""erw"": ""2025-06-21T14:11:54Z"",
                  ""verspaetung"": 3,
                  ""verspaetungPrefix"": ""+""
                },
                ""ankunftsgleise"": {
                  ""ist"": ""11"",
                  ""kbStatus"": ""BESTAETIGT"",
                  ""aussteigeseite"": ""R"",
                  ""anPerron"": ""10/11""
                },
                ""abfahrtsgleise"": {},
                ""ankunftsformation"": ""@D,F,F,F@C,F,F,F,[(12#NF,2#BHP;VH;KW;NF,2#NF@B,2)#NF],F,F@A,F,F,F,F,F,F,F"",
                ""fahrzeugziele"": [],
                ""anschlussInfo"": {
                  ""fahrtId"": {
                    ""fahrtBezeichner"": ""ch:1:sjyid:100001:17355-002"",
                    ""betriebstag"": ""2025-06-21""
                  },
                  ""tu"": ""11"",
                  ""fahrtNummer"": 17355
                },
                ""halteCode"": ""HALT""
              }
            ]
          }
        ]
      }
    ]
  }
]";
    
    
    #endregion
}