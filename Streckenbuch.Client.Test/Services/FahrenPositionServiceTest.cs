using MediatR;
using Microsoft.JSInterop;
using Moq;
using Newtonsoft.Json;
using Streckenbuch.Client.Components.Fahren;
using Streckenbuch.Client.Models.Fahren;
using Streckenbuch.Client.Models.Fahren.Betriebspunkt;
using Streckenbuch.Client.Models.Fahren.Signal;
using Streckenbuch.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streckenbuch.Client.Test.Services;

public class FahrenPositionServiceTest
{
    public interface TestInterface
    {
        void DoSomething();
    }

    private GeolocationPosition[] Positions => JsonConvert.DeserializeObject<GeolocationPosition[]>(PositionData)!;


    [Fact]
    public void SouldNotMove()
    {
        Mock<TestInterface> mock = new Mock<TestInterface>();
        Mock<ISender> sender = new Mock<ISender>();

        FahrenPositionService positionService = new FahrenPositionService(sender.Object);
        positionService.Initialize(TimeLineEntries, (action) =>
        {
            mock.Object.DoSomething();
            action();
        });

        mock.Verify(s => s.DoSomething(), Times.Never());

        var firstPosition = Positions.First();

        positionService.UpdatePosition(firstPosition);
        positionService.UpdatePosition(firstPosition);
        positionService.UpdatePosition(firstPosition);
        positionService.UpdatePosition(firstPosition);

        mock.Verify(s => s.DoSomething(), Times.Never());
    }
    
    [Fact]
    public async Task ShouldMoveOnePositionWithMultipleUpdatesInOneCycle()
    {
        Mock<TestInterface> mock = new Mock<TestInterface>();
        Mock<ISender> sender = new Mock<ISender>();

        FahrenPositionService positionService = new FahrenPositionService(sender.Object);
        positionService.Initialize(TimeLineEntries, (action) =>
        {
            _ = Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(2.1));
                mock.Object.DoSomething();
                action();
            }).ConfigureAwait(false);
        });

        mock.Verify(s => s.DoSomething(), Times.Never());

        IEnumerable<GeolocationPosition> positions = Positions.Take(10);

        foreach(GeolocationPosition pos in positions)
        {
            await Task.Delay(TimeSpan.FromSeconds(0.5));
            await positionService.UpdatePosition(pos);
        }
        
        await Task.Delay(TimeSpan.FromSeconds(5));

        mock.Verify(s => s.DoSomething(), Times.Exactly(2));
    }
    
    [Fact]
    public async Task ShouldMoveOnlyOnePositionWhenSpammedWithUpdates()
    {
        Mock<TestInterface> mock = new Mock<TestInterface>();
        Mock<ISender> sender = new Mock<ISender>();

        FahrenPositionService positionService = new FahrenPositionService(sender.Object);
        positionService.Initialize(TimeLineEntries, (action) =>
        {
            mock.Object.DoSomething();
            action();
        });

        mock.Verify(s => s.DoSomething(), Times.Never());

        IEnumerable<GeolocationPosition> positions = Positions.Take(10);

        foreach(GeolocationPosition pos in positions)
        {
            _ = positionService.UpdatePosition(pos);
        }
        
        await Task.Delay(TimeSpan.FromSeconds(5));

        mock.Verify(s => s.DoSomething(), Times.Exactly(2));
    }

    [Fact]
    public async Task ShouldMoveOnePosition()
    {
        Mock<TestInterface> mock = new Mock<TestInterface>();
        Mock<ISender> sender = new Mock<ISender>();

        FahrenPositionService positionService = new FahrenPositionService(sender.Object);
        positionService.Initialize(TimeLineEntries, (action) =>
        {
            mock.Object.DoSomething();
            action();
        });

        mock.Verify(s => s.DoSomething(), Times.Never());

        IEnumerable<GeolocationPosition> positions = Positions.Take(10);

        foreach(GeolocationPosition pos in positions)
        {
            await positionService.UpdatePosition(pos);
        }

        mock.Verify(s => s.DoSomething(), Times.Exactly(2));
    }

    [Fact]
    public void ShouldNotMoveAwayFromLastTimelineEntry()
    {
        var mock = new Mock<TestInterface>();
        Mock<ISender> sender = new Mock<ISender>();

        FahrenPositionService positionService = new FahrenPositionService(sender.Object);
        positionService.Initialize(TimeLineEntries, (action) =>
        {
            mock.Object.DoSomething();
            action();
        });

        mock.Verify(s => s.DoSomething(), Times.Never());

        // Get last two timeline entries
        var lastEntry = TimeLineEntries[^1]; // Rupperswil
        var secondLastEntry = TimeLineEntries[^2]; // Abschnitt_Ausfahrt Signal

        // Calculate the direction vector from second-last to last
        var directionLatitude = lastEntry.Location.Y - secondLastEntry.Location.Y;
        var directionLongitude = lastEntry.Location.X - secondLastEntry.Location.X;
        
        List<GeolocationPosition> positions = new List<GeolocationPosition>();

        // 1. First position exactly at the last entry
        positions.Add(new GeolocationPosition
        {
            Coords = new GeolocationCoordinates
            {
                Latitude = lastEntry.Location.X,
                Longitude = lastEntry.Location.Y,
                Accuracy = 5.0
            },
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        });
        
        // 2. Next positions slowly moving *away* from lastEntry
        for (int i = 1; i <= 100; i++)
        {            
            Console.WriteLine("{{coords:{{accuracy:1,latitude:{0},longitude:{1}}}}},", lastEntry.Location.X + i * directionLatitude, lastEntry.Location.Y + i * directionLongitude);

            positions.Add(new GeolocationPosition
            {
                Coords = new GeolocationCoordinates
                {
                    Latitude = lastEntry.Location.X + i * directionLatitude, // Every step a bit farther
                    Longitude = lastEntry.Location.Y + i * directionLongitude,
                    Accuracy = 5.0
                },
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            });
        }

        foreach (var pos in positions)
        {
            positionService.UpdatePosition(pos);
        }

        // The service should NOT trigger DoSomething, because movement is away from the timeline
        mock.Verify(s => s.DoSomething(), Times.Never());
    }

    #region Data

    #region PositionData
    public const string PositionData = @"[

  {
    timestamp: 1744367741326,
    coords: {
      accuracy: 60.6150016784668,
      latitude: 47.4014565,
      longitude: 8.1173157,
      altitude: 423.5,
      altitudeAccuracy: null,
      heading: 80.00009155273438,
      speed: 38.49980926513672
    }
  },
  {
    timestamp: 1744367741326,
    coords: {
      accuracy: 60.6150016784668,
      latitude: 47.4022565,
      longitude: 8.1233157,
      altitude: 423.5,
      altitudeAccuracy: null,
      heading: 80.00009155273438,
      speed: 38.49980926513672
    }
  },
{""timestamp"":1744367741326,""coords"":{""accuracy"":60.6150016784668,""latitude"":47.4034565,""longitude"":8.1273157,""altitude"":423.5,""altitudeAccuracy"":null,""heading"":80.00009155273438,""speed"":38.49980926513672}},
{""timestamp"":1744367742350,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4035725,""longitude"":8.1275989,""altitude"":423.5,""altitudeAccuracy"":null,""heading"":74.00704193115234,""speed"":38.628726959228516}},
{""timestamp"":1744367742851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4036517,""longitude"":8.127981,""altitude"":423.5,""altitudeAccuracy"":null,""heading"":74.00426483154297,""speed"":38.60675048828125}},
{""timestamp"":1744367743849,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4037533,""longitude"":8.1285327,""altitude"":423.5,""altitudeAccuracy"":null,""heading"":74.44450378417969,""speed"":38.68349075317383}},
{""timestamp"":1744367744850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4038375,""longitude"":8.1290575,""altitude"":423.5,""altitudeAccuracy"":null,""heading"":76.94271087646484,""speed"":39.435550689697266}},
{""timestamp"":1744367745851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4039124,""longitude"":8.1295685,""altitude"":423.5,""altitudeAccuracy"":null,""heading"":76.99565887451172,""speed"":38.50142288208008}},
{""timestamp"":1744367746851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.403984,""longitude"":8.1300718,""altitude"":425.6064515337357,""altitudeAccuracy"":null,""heading"":77.9981689453125,""speed"":38.709712982177734}},
{""timestamp"":1744367747850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4040501,""longitude"":8.1305775,""altitude"":425.7547779399279,""altitudeAccuracy"":null,""heading"":78.99873352050781,""speed"":38.71002197265625}},
{""timestamp"":1744367748851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4041105,""longitude"":8.1310841,""altitude"":426.1031030115412,""altitudeAccuracy"":null,""heading"":79.99857330322266,""speed"":38.710025787353516}},
{""timestamp"":1744367749850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4041654,""longitude"":8.1315907,""altitude"":426.28157132284514,""altitudeAccuracy"":null,""heading"":80.9986343383789,""speed"":38.690040588378906}},
{""timestamp"":1744367750851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4042119,""longitude"":8.1320986,""altitude"":426.6191660888127,""altitudeAccuracy"":null,""heading"":82.99776458740234,""speed"":38.65005111694336}},
{""timestamp"":1744367751850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4042474,""longitude"":8.1326069,""altitude"":427.00838414465665,""altitudeAccuracy"":null,""heading"":83.99886322021484,""speed"":38.63003158569336}},
{""timestamp"":1744367752851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4042737,""longitude"":8.1331173,""altitude"":427.50944768710684,""altitudeAccuracy"":null,""heading"":85.99527740478516,""speed"":38.699867248535156}},
{""timestamp"":1744367753851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.404288,""longitude"":8.1336287,""altitude"":428.01914313020603,""altitudeAccuracy"":null,""heading"":86.91123962402344,""speed"":38.64474105834961}},
{""timestamp"":1744367754851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4042906,""longitude"":8.1341397,""altitude"":428.8988319971351,""altitudeAccuracy"":null,""heading"":89.45792388916016,""speed"":38.596378326416016}},
{""timestamp"":1744367755851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.404279,""longitude"":8.134651,""altitude"":429.12682923289964,""altitudeAccuracy"":null,""heading"":91.64122009277344,""speed"":38.59235763549805}},
{""timestamp"":1744367756851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4042544,""longitude"":8.1351612,""altitude"":429.296740408855,""altitudeAccuracy"":null,""heading"":93.68952178955078,""speed"":38.62394714355469}},
{""timestamp"":1744367757850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4042164,""longitude"":8.1356695,""altitude"":429.6559781859023,""altitudeAccuracy"":null,""heading"":95.69186401367188,""speed"":38.67980194091797}},
{""timestamp"":1744367758851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4041654,""longitude"":8.1361764,""altitude"":430.07975102814277,""altitudeAccuracy"":null,""heading"":97.67577362060547,""speed"":38.71866226196289}},
{""timestamp"":1744367759851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4041011,""longitude"":8.1366809,""altitude"":430.2905696616751,""altitudeAccuracy"":null,""heading"":99.65467071533203,""speed"":38.76266860961914}},
{""timestamp"":1744367760851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4040245,""longitude"":8.1371824,""altitude"":430.6369159194056,""altitudeAccuracy"":null,""heading"":102.45955657958984,""speed"":38.83316421508789}},
{""timestamp"":1744367761850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4039355,""longitude"":8.1376794,""altitude"":431.09086702939356,""altitudeAccuracy"":null,""heading"":104.56224822998047,""speed"":38.85637283325195}},
{""timestamp"":1744367762850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.403835,""longitude"":8.1381708,""altitude"":431.40928561677555,""altitudeAccuracy"":null,""heading"":106.55525207519531,""speed"":38.86725997924805}},
{""timestamp"":1744367763850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4037219,""longitude"":8.1386566,""altitude"":431.62228841498,""altitudeAccuracy"":null,""heading"":108.5296630859375,""speed"":38.87506866455078}},
{""timestamp"":1744367764851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4035956,""longitude"":8.1391347,""altitude"":431.8998454317425,""altitudeAccuracy"":null,""heading"":110.5031509399414,""speed"":38.89940643310547}},
{""timestamp"":1744367765851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4034588,""longitude"":8.1396062,""altitude"":432.13451318348075,""altitudeAccuracy"":null,""heading"":112.46842193603516,""speed"":38.9017448425293}},
{""timestamp"":1744367766851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4033083,""longitude"":8.1400692,""altitude"":432.46588564726414,""altitudeAccuracy"":null,""heading"":115.1960678100586,""speed"":38.93430709838867}},
{""timestamp"":1744367767851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4031441,""longitude"":8.1405218,""altitude"":433.0359858766767,""altitudeAccuracy"":null,""heading"":117.32832336425781,""speed"":38.90495681762695}},
{""timestamp"":1744367768851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4029684,""longitude"":8.1409664,""altitude"":433.4814474178247,""altitudeAccuracy"":null,""heading"":119.33854675292969,""speed"":38.89164733886719}},
{""timestamp"":1744367769850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.402783,""longitude"":8.1414011,""altitude"":433.7332402350462,""altitudeAccuracy"":null,""heading"":121.31838989257812,""speed"":38.8709716796875}},
{""timestamp"":1744367770850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4025878,""longitude"":8.1418255,""altitude"":434.0431492075723,""altitudeAccuracy"":null,""heading"":123.2934341430664,""speed"":38.84727478027344}},
{""timestamp"":1744367771850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4023815,""longitude"":8.1422413,""altitude"":434.69098111785337,""altitudeAccuracy"":null,""heading"":125.27153015136719,""speed"":38.82892990112305}},
{""timestamp"":1744367772851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4021663,""longitude"":8.1426461,""altitude"":435.0138314973552,""altitudeAccuracy"":null,""heading"":127.96788787841797,""speed"":38.8076286315918}},
{""timestamp"":1744367773851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4019387,""longitude"":8.143038,""altitude"":435.51752191062513,""altitudeAccuracy"":null,""heading"":130.15138244628906,""speed"":38.790958404541016}},
{""timestamp"":1744367774850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4017013,""longitude"":8.1434157,""altitude"":436.01047077134814,""altitudeAccuracy"":null,""heading"":132.19664001464844,""speed"":38.74137878417969}},
{""timestamp"":1744367775850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4014555,""longitude"":8.1437798,""altitude"":436.3871974638151,""altitudeAccuracy"":null,""heading"":134.20843505859375,""speed"":38.704341888427734}},
{""timestamp"":1744367776850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.401203,""longitude"":8.1441314,""altitude"":436.73164773905904,""altitudeAccuracy"":null,""heading"":136.21737670898438,""speed"":38.68526077270508}},
{""timestamp"":1744367777851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4009441,""longitude"":8.1444736,""altitude"":436.80699807583,""altitudeAccuracy"":null,""heading"":137.50714111328125,""speed"":38.71833419799805}},
{""timestamp"":1744367778851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4006811,""longitude"":8.14481,""altitude"":437.1794537229728,""altitudeAccuracy"":null,""heading"":138.5947723388672,""speed"":38.727745056152344}},
{""timestamp"":1744367779851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4004173,""longitude"":8.1451471,""altitude"":437.77584693586084,""altitudeAccuracy"":null,""heading"":138.8899383544922,""speed"":38.78273010253906}},
{""timestamp"":1744367780851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4001532,""longitude"":8.1454871,""altitude"":438.1720299106897,""altitudeAccuracy"":null,""heading"":138.2434844970703,""speed"":38.78742980957031}},
{""timestamp"":1744367781851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3998928,""longitude"":8.1458337,""altitude"":438.53808475053796,""altitudeAccuracy"":null,""heading"":137.3455352783203,""speed"":38.78697967529297}},
{""timestamp"":1744367782850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3996417,""longitude"":8.146189,""altitude"":439.0699447571346,""altitudeAccuracy"":null,""heading"":135.65528869628906,""speed"":38.792625427246094}},
{""timestamp"":1744367783850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3993992,""longitude"":8.1465608,""altitude"":439.38437705854994,""altitudeAccuracy"":null,""heading"":133.7467803955078,""speed"":38.783748626708984}},
{""timestamp"":1744367784851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3991645,""longitude"":8.1469432,""altitude"":439.7246401868302,""altitudeAccuracy"":null,""heading"":131.76918029785156,""speed"":38.778564453125}},
{""timestamp"":1744367785851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.398939,""longitude"":8.1473338,""altitude"":440.2199554771864,""altitudeAccuracy"":null,""heading"":129.7675323486328,""speed"":38.76107406616211}},
{""timestamp"":1744367786851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3987247,""longitude"":8.1477402,""altitude"":440.5473573002176,""altitudeAccuracy"":null,""heading"":127.0171890258789,""speed"":38.69776153564453}},
{""timestamp"":1744367787850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3985209,""longitude"":8.1481597,""altitude"":441.0104395071349,""altitudeAccuracy"":null,""heading"":124.79954528808594,""speed"":38.708248138427734}},
{""timestamp"":1744367788850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3983278,""longitude"":8.1485908,""altitude"":441.24091296845245,""altitudeAccuracy"":null,""heading"":122.71915435791016,""speed"":38.700740814208984}},
{""timestamp"":1744367789851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3981455,""longitude"":8.1490311,""altitude"":441.75573113765796,""altitudeAccuracy"":null,""heading"":120.67389678955078,""speed"":38.691436767578125}},
{""timestamp"":1744367790850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3979741,""longitude"":8.1494818,""altitude"":442.1951819051802,""altitudeAccuracy"":null,""heading"":118.63556671142578,""speed"":38.67559814453125}},
{""timestamp"":1744367791851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3978157,""longitude"":8.1499392,""altitude"":442.56140803238776,""altitudeAccuracy"":null,""heading"":116.59407043457031,""speed"":38.652198791503906}},
{""timestamp"":1744367792851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3976651,""longitude"":8.1504011,""altitude"":443.04398862615034,""altitudeAccuracy"":null,""heading"":115.35431671142578,""speed"":38.70886993408203}},
{""timestamp"":1744367793851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3975184,""longitude"":8.1508677,""altitude"":443.5567601799143,""altitudeAccuracy"":null,""heading"":114.2811050415039,""speed"":38.7027587890625}},
{""timestamp"":1744367794850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3973721,""longitude"":8.1513288,""altitude"":443.8713326506715,""altitudeAccuracy"":null,""heading"":114.05484008789062,""speed"":38.748046875}},
{""timestamp"":1744367795851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3972224,""longitude"":8.1517987,""altitude"":444.26348704894616,""altitudeAccuracy"":null,""heading"":114.01507568359375,""speed"":38.79137420654297}},
{""timestamp"":1744367796850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3970744,""longitude"":8.1522652,""altitude"":444.802189826524,""altitudeAccuracy"":null,""heading"":114.0036849975586,""speed"":38.789031982421875}},
{""timestamp"":1744367797852,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3969292,""longitude"":8.1527293,""altitude"":444.9529527127969,""altitudeAccuracy"":null,""heading"":113.99874877929688,""speed"":38.75864028930664}},
{""timestamp"":1744367798850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3967876,""longitude"":8.1531937,""altitude"":445.4874685234427,""altitudeAccuracy"":null,""heading"":114.00151062011719,""speed"":38.76580810546875}},
{""timestamp"":1744367799852,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3966427,""longitude"":8.1536621,""altitude"":445.6490988159682,""altitudeAccuracy"":null,""heading"":114.82278442382812,""speed"":38.75406265258789}},
{""timestamp"":1744367800851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3964953,""longitude"":8.154127,""altitude"":446.17926749881286,""altitudeAccuracy"":null,""heading"":114.9591293334961,""speed"":38.6616325378418}},
{""timestamp"":1744367801851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3963496,""longitude"":8.1545901,""altitude"":446.40556896252644,""altitudeAccuracy"":null,""heading"":114.97872161865234,""speed"":38.53681182861328}},
{""timestamp"":1744367802849,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3962015,""longitude"":8.155042,""altitude"":446.66851719023333,""altitudeAccuracy"":null,""heading"":114.98429107666016,""speed"":38.41226577758789}},
{""timestamp"":1744367803852,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3960556,""longitude"":8.1555026,""altitude"":447.30652335789745,""altitudeAccuracy"":null,""heading"":114.98286437988281,""speed"":38.28617858886719}},
{""timestamp"":1744367804851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3959089,""longitude"":8.1559554,""altitude"":447.4703433890901,""altitudeAccuracy"":null,""heading"":114.97795867919922,""speed"":38.11227798461914}},
{""timestamp"":1744367805851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3957617,""longitude"":8.1564099,""altitude"":447.88421842105856,""altitudeAccuracy"":null,""heading"":114.98831939697266,""speed"":38.04674530029297}},
{""timestamp"":1744367806851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.395615,""longitude"":8.1568654,""altitude"":448.2743392962425,""altitudeAccuracy"":null,""heading"":114.9830551147461,""speed"":37.908931732177734}},
{""timestamp"":1744367807852,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3954736,""longitude"":8.1573219,""altitude"":448.57621022994033,""altitudeAccuracy"":null,""heading"":114.97767639160156,""speed"":37.74029541015625}},
{""timestamp"":1744367808850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3953302,""longitude"":8.157776,""altitude"":448.98582663113865,""altitudeAccuracy"":null,""heading"":114.97440338134766,""speed"":37.557518005371094}},
{""timestamp"":1744367809851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3951876,""longitude"":8.1582241,""altitude"":449.39977460398194,""altitudeAccuracy"":null,""heading"":114.9832534790039,""speed"":37.44956588745117}},
{""timestamp"":1744367810851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3950466,""longitude"":8.1586716,""altitude"":449.7603105248038,""altitudeAccuracy"":null,""heading"":114.15034484863281,""speed"":37.308128356933594}},
{""timestamp"":1744367811850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3949084,""longitude"":8.1591175,""altitude"":450.16566923450057,""altitudeAccuracy"":null,""heading"":114.0146713256836,""speed"":37.20786666870117}},
{""timestamp"":1744367812851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3947661,""longitude"":8.1595619,""altitude"":450.59429868486893,""altitudeAccuracy"":null,""heading"":113.99016571044922,""speed"":37.089778900146484}},
{""timestamp"":1744367813850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3946275,""longitude"":8.1600067,""altitude"":450.9613490689297,""altitudeAccuracy"":null,""heading"":114.81293487548828,""speed"":36.992557525634766}},
{""timestamp"":1744367814850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.39449,""longitude"":8.1604509,""altitude"":451.4594950536444,""altitudeAccuracy"":null,""heading"":114.94860076904297,""speed"":36.84379196166992}},
{""timestamp"":1744367815850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3943499,""longitude"":8.1608923,""altitude"":451.61463346359375,""altitudeAccuracy"":null,""heading"":114.97722625732422,""speed"":36.72459030151367}},
{""timestamp"":1744367816852,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3942093,""longitude"":8.16133,""altitude"":451.87343022893043,""altitudeAccuracy"":null,""heading"":114.98479461669922,""speed"":36.6258430480957}},
{""timestamp"":1744367817850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3940719,""longitude"":8.161767,""altitude"":452.35234837285043,""altitudeAccuracy"":null,""heading"":114.14132690429688,""speed"":36.421730041503906}},
{""timestamp"":1744367818852,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3939345,""longitude"":8.1622022,""altitude"":452.80959417498343,""altitudeAccuracy"":null,""heading"":114.84288024902344,""speed"":36.34636306762695}},
{""timestamp"":1744367819851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3937983,""longitude"":8.1626369,""altitude"":453.1784280783589,""altitudeAccuracy"":null,""heading"":114.95785522460938,""speed"":36.2300910949707}},
{""timestamp"":1744367820850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.393659,""longitude"":8.1630699,""altitude"":453.6292465542573,""altitudeAccuracy"":null,""heading"":114.9764633178711,""speed"":36.090904235839844}},
{""timestamp"":1744367821851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.393522,""longitude"":8.1635009,""altitude"":453.9765438875089,""altitudeAccuracy"":null,""heading"":114.97804260253906,""speed"":35.95371627807617}},
{""timestamp"":1744367822851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3933884,""longitude"":8.1639338,""altitude"":454.3087544608582,""altitudeAccuracy"":null,""heading"":114.14192962646484,""speed"":35.760555267333984}},
{""timestamp"":1744367823851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3932603,""longitude"":8.164369,""altitude"":454.6690232882011,""altitudeAccuracy"":null,""heading"":113.1730728149414,""speed"":35.6078987121582}},
{""timestamp"":1744367824850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3931343,""longitude"":8.164805,""altitude"":455.1673840887516,""altitudeAccuracy"":null,""heading"":113.01897430419922,""speed"":35.53099060058594}},
{""timestamp"":1744367825849,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3930128,""longitude"":8.1652424,""altitude"":455.68950733290853,""altitudeAccuracy"":null,""heading"":112.14447784423828,""speed"":35.3768424987793}},
{""timestamp"":1744367826851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3928933,""longitude"":8.1656761,""altitude"":455.9246889008856,""altitudeAccuracy"":null,""heading"":112.00996398925781,""speed"":35.272132873535156}},
{""timestamp"":1744367827850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3927751,""longitude"":8.1661087,""altitude"":456.1205000869271,""altitudeAccuracy"":null,""heading"":111.98745727539062,""speed"":35.16065216064453}},
{""timestamp"":1744367828851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3926566,""longitude"":8.1665392,""altitude"":456.5413573846579,""altitudeAccuracy"":null,""heading"":111.98011779785156,""speed"":35.0223388671875}},
{""timestamp"":1744367829852,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3925373,""longitude"":8.1669637,""altitude"":456.9469418938717,""altitudeAccuracy"":null,""heading"":111.97313690185547,""speed"":34.839599609375}},
{""timestamp"":1744367830852,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3924216,""longitude"":8.1673899,""altitude"":457.30957634950244,""altitudeAccuracy"":null,""heading"":111.97672271728516,""speed"":34.71485900878906}},
{""timestamp"":1744367831850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3923022,""longitude"":8.1678136,""altitude"":457.46927092920834,""altitudeAccuracy"":null,""heading"":111.98829650878906,""speed"":34.64568328857422}},
{""timestamp"":1744367832851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.392185,""longitude"":8.1682366,""altitude"":457.62239587324564,""altitudeAccuracy"":null,""heading"":111.9849853515625,""speed"":34.53883361816406}},
{""timestamp"":1744367833850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3920675,""longitude"":8.1686592,""altitude"":457.6786048019543,""altitudeAccuracy"":null,""heading"":111.98255157470703,""speed"":34.42367935180664}},
{""timestamp"":1744367834851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3919505,""longitude"":8.1690837,""altitude"":457.72608329642014,""altitudeAccuracy"":null,""heading"":111.98486328125,""speed"":34.3353385925293}},
{""timestamp"":1744367835851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3918313,""longitude"":8.1695012,""altitude"":457.86852035254617,""altitudeAccuracy"":null,""heading"":112.82405853271484,""speed"":34.28581619262695}},
{""timestamp"":1744367836850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3917053,""longitude"":8.1699126,""altitude"":457.9591633438459,""altitudeAccuracy"":null,""heading"":113.78318786621094,""speed"":34.18872833251953}},
{""timestamp"":1744367837851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3915705,""longitude"":8.1703214,""altitude"":458.0346998997294,""altitudeAccuracy"":null,""heading"":115.59095764160156,""speed"":34.167232513427734}},
{""timestamp"":1744367838851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3914276,""longitude"":8.1707196,""altitude"":458.1210282045805,""altitudeAccuracy"":null,""heading"":116.74189758300781,""speed"":34.224998474121094}},
{""timestamp"":1744367839851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3912804,""longitude"":8.1711163,""altitude"":458.39512631614497,""altitudeAccuracy"":null,""heading"":117.75204467773438,""speed"":34.206302642822266}},
{""timestamp"":1744367840851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3911308,""longitude"":8.1715117,""altitude"":458.610957748564,""altitudeAccuracy"":null,""heading"":118.7748794555664,""speed"":34.339847564697266}},
{""timestamp"":1744367841851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3909816,""longitude"":8.171903,""altitude"":458.66275809835327,""altitudeAccuracy"":null,""heading"":118.00279235839844,""speed"":34.378997802734375}},
{""timestamp"":1744367842851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3908409,""longitude"":8.1723056,""altitude"":458.66707480825283,""altitudeAccuracy"":null,""heading"":117.00352478027344,""speed"":34.428680419921875}},
{""timestamp"":1744367843850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.390707,""longitude"":8.1727189,""altitude"":458.6368578844584,""altitudeAccuracy"":null,""heading"":115.0066146850586,""speed"":34.437679290771484}},
{""timestamp"":1744367844851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3905859,""longitude"":8.1731408,""altitude"":458.53109948725574,""altitudeAccuracy"":null,""heading"":112.00935363769531,""speed"":34.486000061035156}},
{""timestamp"":1744367845852,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3904785,""longitude"":8.1735707,""altitude"":458.5051996698674,""altitudeAccuracy"":null,""heading"":109.0081558227539,""speed"":34.4964485168457}},
{""timestamp"":1744367846852,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3903881,""longitude"":8.1740075,""altitude"":458.5095163006829,""altitudeAccuracy"":null,""heading"":106.00736236572266,""speed"":34.54634094238281}},
{""timestamp"":1744367847851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3903179,""longitude"":8.1744525,""altitude"":458.6260661516885,""altitudeAccuracy"":null,""heading"":103.00623321533203,""speed"":34.517364501953125}},
{""timestamp"":1744367848851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3902605,""longitude"":8.1749033,""altitude"":458.8095274504955,""altitudeAccuracy"":null,""heading"":100.00616455078125,""speed"":34.66609573364258}},
{""timestamp"":1744367849851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3902205,""longitude"":8.1753549,""altitude"":459.13760917740933,""altitudeAccuracy"":null,""heading"":96.00684356689453,""speed"":34.48891830444336}},
{""timestamp"":1744367850850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3901922,""longitude"":8.1758093,""altitude"":459.54341178551624,""altitudeAccuracy"":null,""heading"":93.0046615600586,""speed"":34.578182220458984}},
{""timestamp"":1744367851850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3901793,""longitude"":8.1762656,""altitude"":459.8563036307978,""altitudeAccuracy"":null,""heading"":90.00444030761719,""speed"":34.58946990966797}},
{""timestamp"":1744367852850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3901882,""longitude"":8.1767252,""altitude"":460.16283673089487,""altitudeAccuracy"":null,""heading"":87.00447845458984,""speed"":34.590248107910156}},
{""timestamp"":1744367853851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3902104,""longitude"":8.177182,""altitude"":460.40893459143103,""altitudeAccuracy"":null,""heading"":84.00479888916016,""speed"":34.57106018066406}},
{""timestamp"":1744367854851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.390247,""longitude"":8.1776352,""altitude"":460.860132293773,""altitudeAccuracy"":null,""heading"":81.00526428222656,""speed"":34.591331481933594}},
{""timestamp"":1744367855852,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3902913,""longitude"":8.178086,""altitude"":461.0760247349819,""altitudeAccuracy"":null,""heading"":79.00384521484375,""speed"":34.60121154785156}},
{""timestamp"":1744367856850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3903535,""longitude"":8.1785348,""altitude"":461.48582041063617,""altitudeAccuracy"":null,""heading"":77.00407409667969,""speed"":34.5915412902832}},
{""timestamp"":1744367857852,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3904203,""longitude"":8.17898,""altitude"":461.68918882230764,""altitudeAccuracy"":null,""heading"":76.0019302368164,""speed"":34.62056350708008}},
{""timestamp"":1744367858850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3904894,""longitude"":8.1794288,""altitude"":462.2548930545936,""altitudeAccuracy"":null,""heading"":76.00008392333984,""speed"":34.59022903442383}},
{""timestamp"":1744367859851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3905597,""longitude"":8.1798724,""altitude"":462.5939015899623,""altitudeAccuracy"":null,""heading"":76,""speed"":34.58999252319336}},
{""timestamp"":1744367860850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3906314,""longitude"":8.1803183,""altitude"":463.00850344172716,""altitudeAccuracy"":null,""heading"":76.00013732910156,""speed"":34.53041458129883}},
{""timestamp"":1744367861852,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3907016,""longitude"":8.1807636,""altitude"":463.5224647332235,""altitudeAccuracy"":null,""heading"":76.9982681274414,""speed"":34.568878173828125}},
{""timestamp"":1744367862850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3907707,""longitude"":8.1812082,""altitude"":464.1984353799842,""altitudeAccuracy"":null,""heading"":77.0001220703125,""speed"":34.51039505004883}},
{""timestamp"":1744367863849,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.390841,""longitude"":8.181656,""altitude"":464.6692706088892,""altitudeAccuracy"":null,""heading"":76.99989318847656,""speed"":34.559654235839844}},
{""timestamp"":1744367864852,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3909116,""longitude"":8.1821048,""altitude"":465.0126942122813,""altitudeAccuracy"":null,""heading"":77.0000228881836,""speed"":34.550071716308594}},
{""timestamp"":1744367865851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3909799,""longitude"":8.1825504,""altitude"":465.38637195097846,""altitudeAccuracy"":null,""heading"":77,""speed"":34.549991607666016}},
{""timestamp"":1744367866852,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3910496,""longitude"":8.1829991,""altitude"":465.6239789575313,""altitudeAccuracy"":null,""heading"":76.99987030029297,""speed"":34.60957717895508}},
{""timestamp"":1744367867851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3911201,""longitude"":8.183447,""altitude"":465.9825619601149,""altitudeAccuracy"":null,""heading"":76.9999771118164,""speed"":34.61992645263672}},
{""timestamp"":1744367868851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3911907,""longitude"":8.1838943,""altitude"":466.60903802309406,""altitudeAccuracy"":null,""heading"":76.99988555908203,""speed"":34.669639587402344}},
{""timestamp"":1744367869851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3912596,""longitude"":8.1843409,""altitude"":466.950378670525,""altitudeAccuracy"":null,""heading"":77.00012969970703,""speed"":34.61042404174805}},
{""timestamp"":1744367870850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3913283,""longitude"":8.1847919,""altitude"":467.0627215936736,""altitudeAccuracy"":null,""heading"":76.99996185302734,""speed"":34.62987518310547}},
{""timestamp"":1744367871852,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3913974,""longitude"":8.1852437,""altitude"":467.4926628738075,""altitudeAccuracy"":null,""heading"":76.99931335449219,""speed"":34.89771270751953}},
{""timestamp"":1744367872850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3914638,""longitude"":8.1856928,""altitude"":467.7951470065575,""altitudeAccuracy"":null,""heading"":77.00056457519531,""speed"":34.63184356689453}},
{""timestamp"":1744367873850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3915331,""longitude"":8.1861379,""altitude"":468.2640184389594,""altitudeAccuracy"":null,""heading"":77.00004577636719,""speed"":34.61014175415039}},
{""timestamp"":1744367874851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3916033,""longitude"":8.186586,""altitude"":468.61190731154437,""altitudeAccuracy"":null,""heading"":76.9999771118164,""speed"":34.61992645263672}},
{""timestamp"":1744367875851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3916757,""longitude"":8.1870356,""altitude"":469.0527308171745,""altitudeAccuracy"":null,""heading"":77.00004577636719,""speed"":34.60014343261719}},
{""timestamp"":1744367876851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3917424,""longitude"":8.1874799,""altitude"":469.6448528492037,""altitudeAccuracy"":null,""heading"":77.00001525878906,""speed"":34.590057373046875}},
{""timestamp"":1744367877850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.391809,""longitude"":8.1879245,""altitude"":469.9258009206044,""altitudeAccuracy"":null,""heading"":77.00003814697266,""speed"":34.570125579833984}},
{""timestamp"":1744367878851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3918788,""longitude"":8.1883719,""altitude"":470.32346618786426,""altitudeAccuracy"":null,""heading"":76.99990844726562,""speed"":34.609718322753906}},
{""timestamp"":1744367879852,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3919465,""longitude"":8.1888198,""altitude"":470.693052025497,""altitudeAccuracy"":null,""heading"":77.0000228881836,""speed"":34.60006332397461}},
{""timestamp"":1744367880851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3920146,""longitude"":8.1892647,""altitude"":471.19018083358714,""altitudeAccuracy"":null,""heading"":76.99984741210938,""speed"":34.669498443603516}},
{""timestamp"":1744367881851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3920855,""longitude"":8.1897118,""altitude"":471.4646947080973,""altitudeAccuracy"":null,""heading"":77.00008392333984,""speed"":34.630279541015625}},
{""timestamp"":1744367882852,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3921575,""longitude"":8.19016,""altitude"":471.752187135639,""altitudeAccuracy"":null,""heading"":77.00006103515625,""speed"":34.6002082824707}},
{""timestamp"":1744367883851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3922272,""longitude"":8.1906105,""altitude"":472.1480537623485,""altitudeAccuracy"":null,""heading"":76.99996185302734,""speed"":34.61986541748047}},
{""timestamp"":1744367884851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3922972,""longitude"":8.1910561,""altitude"":472.45690847454387,""altitudeAccuracy"":null,""heading"":76.99996948242188,""speed"":34.62991714477539}},
{""timestamp"":1744367885851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3923653,""longitude"":8.1915055,""altitude"":472.76389104039487,""altitudeAccuracy"":null,""heading"":77,""speed"":34.6300048828125}},
{""timestamp"":1744367886851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3924346,""longitude"":8.1919493,""altitude"":473.016835614071,""altitudeAccuracy"":null,""heading"":76.99951934814453,""speed"":34.055519104003906}},
{""timestamp"":1744367887851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3924999,""longitude"":8.1923954,""altitude"":473.49896425183476,""altitudeAccuracy"":null,""heading"":76.99872589111328,""speed"":34.5858039855957}},
{""timestamp"":1744367888851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3925683,""longitude"":8.1928435,""altitude"":473.92274088952297,""altitudeAccuracy"":null,""heading"":76.99974822998047,""speed"":34.69919204711914}},
{""timestamp"":1744367889850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3926367,""longitude"":8.1932902,""altitude"":474.1065280661416,""altitudeAccuracy"":null,""heading"":77.00012969970703,""speed"":34.64041519165039}},
{""timestamp"":1744367890851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3927064,""longitude"":8.1937362,""altitude"":474.41789412882275,""altitudeAccuracy"":null,""heading"":77.00003814697266,""speed"":34.62013244628906}},
{""timestamp"":1744367891851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3927733,""longitude"":8.1941835,""altitude"":474.9692991898413,""altitudeAccuracy"":null,""heading"":76.99986267089844,""speed"":34.6795654296875}},
{""timestamp"":1744367892850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3928447,""longitude"":8.1946316,""altitude"":475.20717111185957,""altitudeAccuracy"":null,""heading"":76.99990844726562,""speed"":34.7197151184082}},
{""timestamp"":1744367893852,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3929137,""longitude"":8.1950811,""altitude"":475.8321568774077,""altitudeAccuracy"":null,""heading"":77.00015258789062,""speed"":34.65048599243164}},
{""timestamp"":1744367894852,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3929839,""longitude"":8.1955294,""altitude"":476.1133071542014,""altitudeAccuracy"":null,""heading"":76.99987030029297,""speed"":34.709571838378906}},
{""timestamp"":1744367895851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3930561,""longitude"":8.1959819,""altitude"":476.3079550379376,""altitudeAccuracy"":null,""heading"":77.00006866455078,""speed"":34.68022155761719}},
{""timestamp"":1744367896852,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3931257,""longitude"":8.1964322,""altitude"":476.5026073273342,""altitudeAccuracy"":null,""heading"":77.0006332397461,""speed"":34.422027587890625}},
{""timestamp"":1744367897852,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3931912,""longitude"":8.1968773,""altitude"":476.90490268732935,""altitudeAccuracy"":null,""heading"":76.99958038330078,""speed"":34.61862564086914}},
{""timestamp"":1744367898850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3932616,""longitude"":8.1973241,""altitude"":477.24881525383734,""altitudeAccuracy"":null,""heading"":76.99995422363281,""speed"":34.63984298706055}},
{""timestamp"":1744367899852,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.393333,""longitude"":8.1977713,""altitude"":477.6835923322986,""altitudeAccuracy"":null,""heading"":77.00001525878906,""speed"":34.63006591796875}},
{""timestamp"":1744367900851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3934016,""longitude"":8.198215,""altitude"":478.0967590785902,""altitudeAccuracy"":null,""heading"":76.99999237060547,""speed"":34.629981994628906}},
{""timestamp"":1744367901850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3934751,""longitude"":8.1986606,""altitude"":478.2373700900379,""altitudeAccuracy"":null,""heading"":76.0017318725586,""speed"":34.6407585144043}},
{""timestamp"":1744367902851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3935515,""longitude"":8.1991082,""altitude"":478.79551031712145,""altitudeAccuracy"":null,""heading"":75.00186157226562,""speed"":34.65079879760742}},
{""timestamp"":1744367903850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3936336,""longitude"":8.1995464,""altitude"":479.1827684264561,""altitudeAccuracy"":null,""heading"":73.004150390625,""speed"":34.75114440917969}},
{""timestamp"":1744367904851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3937344,""longitude"":8.1999818,""altitude"":479.4467197009693,""altitudeAccuracy"":null,""heading"":70.00775146484375,""speed"":34.72292709350586}},
{""timestamp"":1744367905851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3938465,""longitude"":8.200412,""altitude"":479.77991566432,""altitudeAccuracy"":null,""heading"":68.00587463378906,""speed"":34.712039947509766}},
{""timestamp"":1744367906850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3939726,""longitude"":8.2008302,""altitude"":480.2229248229634,""altitudeAccuracy"":null,""heading"":65.00970458984375,""speed"":34.752498626708984}},
{""timestamp"":1744367907852,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3941098,""longitude"":8.201244,""altitude"":480.5323436106057,""altitudeAccuracy"":null,""heading"":62.01134490966797,""speed"":34.72287368774414}},
{""timestamp"":1744367908852,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3942616,""longitude"":8.201649,""altitude"":480.70817249514715,""altitudeAccuracy"":null,""heading"":59.01227951049805,""speed"":34.82188034057617}},
{""timestamp"":1744367909852,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3944274,""longitude"":8.2020374,""altitude"":480.9656787386054,""altitudeAccuracy"":null,""heading"":57.00857925415039,""speed"":34.76173400878906}},
{""timestamp"":1744367910851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3946083,""longitude"":8.2024144,""altitude"":481.9036285512258,""altitudeAccuracy"":null,""heading"":54.01359176635742,""speed"":34.78150939941406}},
{""timestamp"":1744367911851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3948015,""longitude"":8.2027791,""altitude"":481.7798314065593,""altitudeAccuracy"":null,""heading"":51.014469146728516,""speed"":34.80097579956055}},
{""timestamp"":1744367912852,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3950065,""longitude"":8.2031302,""altitude"":481.26370528688983,""altitudeAccuracy"":null,""heading"":48.014339447021484,""speed"":34.85016632080078}},
{""timestamp"":1744367913849,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3952196,""longitude"":8.2034691,""altitude"":481.168489600127,""altitudeAccuracy"":null,""heading"":46.00990295410156,""speed"":34.830142974853516}},
{""timestamp"":1744367914851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.395445,""longitude"":8.2037936,""altitude"":481.0653337507452,""altitudeAccuracy"":null,""heading"":43.01498031616211,""speed"":34.79945755004883}},
{""timestamp"":1744367915851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3956796,""longitude"":8.2041022,""altitude"":480.53951569803417,""altitudeAccuracy"":null,""heading"":40.01451110839844,""speed"":34.73896789550781}},
{""timestamp"":1744367916851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.39592,""longitude"":8.2043948,""altitude"":479.81487365408566,""altitudeAccuracy"":null,""heading"":38.00952911376953,""speed"":34.70902633666992}},
{""timestamp"":1744367917850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3961731,""longitude"":8.2046672,""altitude"":479.4687012552838,""altitudeAccuracy"":null,""heading"":35.013729095458984,""speed"":34.717628479003906}},
{""timestamp"":1744367918850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.396433,""longitude"":8.2049279,""altitude"":479.32661745260583,""altitudeAccuracy"":null,""heading"":32.012874603271484,""speed"":34.71721649169922}},
{""timestamp"":1744367919851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3967036,""longitude"":8.2051623,""altitude"":478.8565749063393,""altitudeAccuracy"":null,""heading"":29.011695861816406,""speed"":34.66718673706055}},
{""timestamp"":1744367920851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.396979,""longitude"":8.2053734,""altitude"":478.4394044655355,""altitudeAccuracy"":null,""heading"":27.007436752319336,""speed"":34.68775939941406}},
{""timestamp"":1744367921851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3972587,""longitude"":8.2055667,""altitude"":477.9079865171336,""altitudeAccuracy"":null,""heading"":24.010009765625,""speed"":34.726402282714844}},
{""timestamp"":1744367922851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3975461,""longitude"":8.2057434,""altitude"":477.497008532657,""altitudeAccuracy"":null,""heading"":21.008811950683594,""speed"":34.63710403442383}},
{""timestamp"":1744367923850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3978388,""longitude"":8.2059044,""altitude"":477.1153475818248,""altitudeAccuracy"":null,""heading"":19.005882263183594,""speed"":34.74695587158203}},
{""timestamp"":1744367924852,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3981372,""longitude"":8.206044,""altitude"":476.6719620506738,""altitudeAccuracy"":null,""heading"":17.004913330078125,""speed"":34.77775573730469}},
{""timestamp"":1744367925850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3984369,""longitude"":8.2061777,""altitude"":476.47906949065316,""altitudeAccuracy"":null,""heading"":16.002166748046875,""speed"":34.749244689941406}},
{""timestamp"":1744367926849,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3987366,""longitude"":8.2063082,""altitude"":476.09627878424817,""altitudeAccuracy"":null,""heading"":16.000083923339844,""speed"":34.779781341552734}},
{""timestamp"":1744367927850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3990351,""longitude"":8.2064397,""altitude"":475.6574069524362,""altitudeAccuracy"":null,""heading"":15.9999361038208,""speed"":34.750186920166016}},
{""timestamp"":1744367928851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3993315,""longitude"":8.2065762,""altitude"":474.4628443885502,""altitudeAccuracy"":null,""heading"":16.99776840209961,""speed"":34.6912956237793}},
{""timestamp"":1744367929850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3996286,""longitude"":8.2067146,""altitude"":473.7968879063143,""altitudeAccuracy"":null,""heading"":16.99994659423828,""speed"":34.670135498046875}},
{""timestamp"":1744367930851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.3999254,""longitude"":8.2068594,""altitude"":473.37221337081706,""altitudeAccuracy"":null,""heading"":17.997785568237305,""speed"":34.651065826416016}},
{""timestamp"":1744367931850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4002198,""longitude"":8.2070084,""altitude"":473.65291638877767,""altitudeAccuracy"":null,""heading"":18.00010871887207,""speed"":34.689727783203125}},
{""timestamp"":1744367932850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.400517,""longitude"":8.2071581,""altitude"":472.85322476876013,""altitudeAccuracy"":null,""heading"":18.99751853942871,""speed"":34.62141799926758}},
{""timestamp"":1744367933851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4008141,""longitude"":8.2073085,""altitude"":472.5238455723806,""altitudeAccuracy"":null,""heading"":19.000385284423828,""speed"":34.76905822753906}},
{""timestamp"":1744367934851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4011081,""longitude"":8.2074609,""altitude"":472.08642866012417,""altitudeAccuracy"":null,""heading"":18.99967384338379,""speed"":34.650787353515625}},
{""timestamp"":1744367935851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4014028,""longitude"":8.2076172,""altitude"":471.8417731100537,""altitudeAccuracy"":null,""heading"":19.997554779052734,""speed"":34.641056060791016}},
{""timestamp"":1744367936851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4016969,""longitude"":8.2077799,""altitude"":471.73862794618816,""altitudeAccuracy"":null,""heading"":20.000102996826172,""speed"":34.67975616455078}},
{""timestamp"":1744367937850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4019881,""longitude"":8.2079429,""altitude"":471.2299154575939,""altitudeAccuracy"":null,""heading"":20.997228622436523,""speed"":34.58158493041992}},
{""timestamp"":1744367938852,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4022769,""longitude"":8.208109,""altitude"":471.31820527100854,""altitudeAccuracy"":null,""heading"":20.999940872192383,""speed"":34.56012725830078}},
{""timestamp"":1744367939850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4025656,""longitude"":8.2082822,""altitude"":471.7871711888397,""altitudeAccuracy"":null,""heading"":21.99728012084961,""speed"":34.551082611083984}},
{""timestamp"":1744367940851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4028512,""longitude"":8.208461,""altitude"":471.639078208357,""altitudeAccuracy"":null,""heading"":21.99994468688965,""speed"":34.530120849609375}},
{""timestamp"":1744367941851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4031336,""longitude"":8.2086445,""altitude"":471.41891624293294,""altitudeAccuracy"":null,""heading"":23.99378776550293,""speed"":34.46240234375}},
{""timestamp"":1744367942851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.403409,""longitude"":8.2088484,""altitude"":471.7814423755774,""altitudeAccuracy"":null,""heading"":26.989627838134766,""speed"":34.39310836791992}},
{""timestamp"":1744367943851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4036788,""longitude"":8.2090626,""altitude"":471.5983942346467,""altitudeAccuracy"":null,""heading"":28.992630004882812,""speed"":34.46139907836914}},
{""timestamp"":1744367944851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4039431,""longitude"":8.2093003,""altitude"":471.96142375255044,""altitudeAccuracy"":null,""heading"":31.987354278564453,""speed"":34.41257095336914}},
{""timestamp"":1744367945851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4041994,""longitude"":8.2095618,""altitude"":471.6622127484453,""altitudeAccuracy"":null,""heading"":34.98676681518555,""speed"":34.38193893432617}},
{""timestamp"":1744367946850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4044476,""longitude"":8.2098427,""altitude"":471.6492435124163,""altitudeAccuracy"":null,""heading"":37.985904693603516,""speed"":34.36137008666992}},
{""timestamp"":1744367947851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4046834,""longitude"":8.210138,""altitude"":471.84347420408284,""altitudeAccuracy"":null,""heading"":39.99061965942383,""speed"":34.430259704589844}},
{""timestamp"":1744367948850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4049123,""longitude"":8.2104425,""altitude"":471.8261818850321,""altitudeAccuracy"":null,""heading"":42.985382080078125,""speed"":34.320735931396484}},
{""timestamp"":1744367949852,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4051363,""longitude"":8.2107623,""altitude"":472.0665553800048,""altitudeAccuracy"":null,""heading"":44.990211486816406,""speed"":34.30995559692383}},
{""timestamp"":1744367950850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4053505,""longitude"":8.2110857,""altitude"":471.91487974485983,""altitudeAccuracy"":null,""heading"":46.990657806396484,""speed"":34.26980209350586}},
{""timestamp"":1744367951851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4055609,""longitude"":8.2114204,""altitude"":471.8457114248313,""altitudeAccuracy"":null,""heading"":46.99995803833008,""speed"":34.27000045776367}},
{""timestamp"":1744367952851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4057684,""longitude"":8.2117527,""altitude"":471.74117957788434,""altitudeAccuracy"":null,""heading"":46.999996185302734,""speed"":34.28990936279297}},
{""timestamp"":1744367953850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4059736,""longitude"":8.2120879,""altitude"":471.6784971033625,""altitudeAccuracy"":null,""heading"":47.00002670288086,""speed"":34.260128021240234}},
{""timestamp"":1744367954852,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4061825,""longitude"":8.2124201,""altitude"":471.5393265375871,""altitudeAccuracy"":null,""heading"":47.000003814697266,""speed"":34.25004196166992}},
{""timestamp"":1744367955851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4063909,""longitude"":8.2127543,""altitude"":471.19662005218953,""altitudeAccuracy"":null,""heading"":47.00002670288086,""speed"":34.200218200683594}},
{""timestamp"":1744367956850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4065972,""longitude"":8.2130857,""altitude"":471.43757119697756,""altitudeAccuracy"":null,""heading"":47.00001525878906,""speed"":34.180084228515625}},
{""timestamp"":1744367957851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4068055,""longitude"":8.2134201,""altitude"":471.5110610408696,""altitudeAccuracy"":null,""heading"":47.00000762939453,""speed"":34.18000030517578}},
{""timestamp"":1744367958850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4070155,""longitude"":8.2137528,""altitude"":471.35478830087976,""altitudeAccuracy"":null,""heading"":47.00004196166992,""speed"":34.07048034667969}},
{""timestamp"":1744367959850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4072262,""longitude"":8.214085,""altitude"":471.2731452555962,""altitudeAccuracy"":null,""heading"":46.99998474121094,""speed"":34.09987258911133}},
{""timestamp"":1744367960850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4074357,""longitude"":8.214421,""altitude"":471.2925987355923,""altitudeAccuracy"":null,""heading"":46.999996185302734,""speed"":34.12987518310547}},
{""timestamp"":1744367961851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4076413,""longitude"":8.2147516,""altitude"":471.70485225450483,""altitudeAccuracy"":null,""heading"":47.000038146972656,""speed"":34.05034255981445}},
{""timestamp"":1744367962851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4078473,""longitude"":8.2150805,""altitude"":471.62762895252735,""altitudeAccuracy"":null,""heading"":47.0000114440918,""speed"":34.03007888793945}},
{""timestamp"":1744367963851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4080518,""longitude"":8.2154131,""altitude"":471.92115080531795,""altitudeAccuracy"":null,""heading"":47.00001525878906,""speed"":34.020042419433594}},
{""timestamp"":1744367964851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.408257,""longitude"":8.2157492,""altitude"":471.8098377774014,""altitudeAccuracy"":null,""heading"":47.00005340576172,""speed"":33.93040084838867}},
{""timestamp"":1744367965851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4084603,""longitude"":8.216083,""altitude"":471.7579610940682,""altitudeAccuracy"":null,""heading"":47.99570083618164,""speed"":33.90985870361328}},
{""timestamp"":1744367966851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.408664,""longitude"":8.2164186,""altitude"":471.6999856770659,""altitudeAccuracy"":null,""heading"":47.999820709228516,""speed"":34.02898025512695}},
{""timestamp"":1744367967851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4088666,""longitude"":8.2167605,""altitude"":471.80373987821974,""altitudeAccuracy"":null,""heading"":48.0001106262207,""speed"":33.89064025878906}},
{""timestamp"":1744367968850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.409066,""longitude"":8.2171033,""altitude"":472.0898564205557,""altitudeAccuracy"":null,""heading"":48.995399475097656,""speed"":33.82003402709961}},
{""timestamp"":1744367969850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4092584,""longitude"":8.2174527,""altitude"":472.08769483639446,""altitudeAccuracy"":null,""heading"":50.9908332824707,""speed"":33.769290924072266}},
{""timestamp"":1744367970851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4094414,""longitude"":8.2178087,""altitude"":472.26862290116543,""altitudeAccuracy"":null,""heading"":52.9912109375,""speed"":33.719058990478516}},
{""timestamp"":1744367971851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4096112,""longitude"":8.2181735,""altitude"":472.0820445701735,""altitudeAccuracy"":null,""heading"":54.99119186401367,""speed"":33.698631286621094}},
{""timestamp"":1744367972851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4097753,""longitude"":8.2185505,""altitude"":472.0322772682512,""altitudeAccuracy"":null,""heading"":56.99187088012695,""speed"":33.65861129760742}},
{""timestamp"":1744367973851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4099305,""longitude"":8.2189327,""altitude"":472.28883060886733,""altitudeAccuracy"":null,""heading"":59.98823928833008,""speed"":33.61725997924805}},
{""timestamp"":1744367974852,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4100756,""longitude"":8.219324,""altitude"":472.1208139664846,""altitudeAccuracy"":null,""heading"":61.99257278442383,""speed"":33.568214416503906}},
{""timestamp"":1744367975849,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.410208,""longitude"":8.2197234,""altitude"":472.33776668459626,""altitudeAccuracy"":null,""heading"":63.99317932128906,""speed"":33.51820755004883}},
{""timestamp"":1744367976851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.410333,""longitude"":8.2201303,""altitude"":472.26267874196793,""altitudeAccuracy"":null,""heading"":65.99361419677734,""speed"":33.5079345703125}},
{""timestamp"":1744367977851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.410451,""longitude"":8.2205343,""altitude"":472.19426727810156,""altitudeAccuracy"":null,""heading"":66.99713134765625,""speed"":33.47916030883789}},
{""timestamp"":1744367978851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4105702,""longitude"":8.2209414,""altitude"":472.2050752994135,""altitudeAccuracy"":null,""heading"":67.0001220703125,""speed"":33.4302864074707}},
{""timestamp"":1744367979850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4106873,""longitude"":8.221345,""altitude"":471.973013161959,""altitudeAccuracy"":null,""heading"":66.99993133544922,""speed"":33.449859619140625}},
{""timestamp"":1744367980851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4108024,""longitude"":8.2217517,""altitude"":471.68733838022683,""altitudeAccuracy"":null,""heading"":67.00013732910156,""speed"":33.40028762817383}},
{""timestamp"":1744367981849,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4109193,""longitude"":8.2221637,""altitude"":471.6916616153745,""altitudeAccuracy"":null,""heading"":67.0000991821289,""speed"":33.37019348144531}},
{""timestamp"":1744367982851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4110335,""longitude"":8.2225707,""altitude"":471.65923740473966,""altitudeAccuracy"":null,""heading"":67.00055694580078,""speed"":33.18114471435547}},
{""timestamp"":1744367983851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4111496,""longitude"":8.2229751,""altitude"":471.6051973253438,""altitudeAccuracy"":null,""heading"":66.99951171875,""speed"":33.3489875793457}},
{""timestamp"":1744367984851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4112642,""longitude"":8.2233862,""altitude"":471.5446728395593,""altitudeAccuracy"":null,""heading"":67.00018310546875,""speed"":33.29037094116211}},
{""timestamp"":1744367985851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4113784,""longitude"":8.2237935,""altitude"":471.57277344070155,""altitudeAccuracy"":null,""heading"":67,""speed"":33.290000915527344}},
{""timestamp"":1744367986851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4114974,""longitude"":8.2242018,""altitude"":471.5641270921091,""altitudeAccuracy"":null,""heading"":67.00003051757812,""speed"":33.280059814453125}},
{""timestamp"":1744367987850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4116143,""longitude"":8.2246054,""altitude"":471.67681745931174,""altitudeAccuracy"":null,""heading"":67.00007629394531,""speed"":33.25016784667969}},
{""timestamp"":1744367988850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4117311,""longitude"":8.2250137,""altitude"":471.480116366049,""altitudeAccuracy"":null,""heading"":67.00003051757812,""speed"":33.24006652832031}},
{""timestamp"":1744367989851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4118453,""longitude"":8.2254204,""altitude"":471.4895685286114,""altitudeAccuracy"":null,""heading"":66.99994659423828,""speed"":33.25988006591797}},
{""timestamp"":1744367990851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4119627,""longitude"":8.2258263,""altitude"":471.2913892843668,""altitudeAccuracy"":null,""heading"":67.00008392333984,""speed"":33.23017120361328}},
{""timestamp"":1744367991852,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4120805,""longitude"":8.2262309,""altitude"":471.44238467904665,""altitudeAccuracy"":null,""heading"":67.00008392333984,""speed"":33.20017623901367}},
{""timestamp"":1744367992851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.412198,""longitude"":8.2266376,""altitude"":470.8507084665797,""altitudeAccuracy"":null,""heading"":66.99996948242188,""speed"":33.20994186401367}},
{""timestamp"":1744367993850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4123128,""longitude"":8.2270409,""altitude"":470.80515764518634,""altitudeAccuracy"":null,""heading"":67.00005340576172,""speed"":33.19010925292969}},
{""timestamp"":1744367994851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4124279,""longitude"":8.2274465,""altitude"":470.38051446813125,""altitudeAccuracy"":null,""heading"":66.99990844726562,""speed"":33.219818115234375}},
{""timestamp"":1744367995851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.412544,""longitude"":8.2278531,""altitude"":470.6151485731171,""altitudeAccuracy"":null,""heading"":67.00003051757812,""speed"":33.21005630493164}},
{""timestamp"":1744367996850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4126594,""longitude"":8.2282538,""altitude"":470.4211767613006,""altitudeAccuracy"":null,""heading"":66.00275421142578,""speed"":33.23081970214844}},
{""timestamp"":1744367997850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4127747,""longitude"":8.2286599,""altitude"":470.2937881832701,""altitudeAccuracy"":null,""heading"":66.0000228881836,""speed"":33.230010986328125}},
{""timestamp"":1744367998851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4128935,""longitude"":8.2290662,""altitude"":470.17134089611886,""altitudeAccuracy"":null,""heading"":66.99732971191406,""speed"":33.21906280517578}},
{""timestamp"":1744367999850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4130085,""longitude"":8.2294721,""altitude"":470.3190482537716,""altitudeAccuracy"":null,""heading"":66.9999008178711,""speed"":33.24980926513672}},
{""timestamp"":1744368000851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4131242,""longitude"":8.2298816,""altitude"":470.0156635210406,""altitudeAccuracy"":null,""heading"":67.00003051757812,""speed"":33.24006652832031}},
{""timestamp"":1744368001851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4132403,""longitude"":8.2302866,""altitude"":469.72889396771217,""altitudeAccuracy"":null,""heading"":67.00011444091797,""speed"":33.200233459472656}},
{""timestamp"":1744368002850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4133555,""longitude"":8.2306892,""altitude"":469.4492608417146,""altitudeAccuracy"":null,""heading"":66.9998550415039,""speed"":33.249698638916016}},
{""timestamp"":1744368003851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4134741,""longitude"":8.231094,""altitude"":469.2977404930446,""altitudeAccuracy"":null,""heading"":66.0028305053711,""speed"":33.26089859008789}},
{""timestamp"":1744368004852,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4135922,""longitude"":8.2314952,""altitude"":468.9209135772386,""altitudeAccuracy"":null,""heading"":66.00000762939453,""speed"":33.2599983215332}},
{""timestamp"":1744368005850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4137142,""longitude"":8.231895,""altitude"":468.83231623986126,""altitudeAccuracy"":null,""heading"":65.00299072265625,""speed"":33.26093292236328}},
{""timestamp"":1744368006852,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4138397,""longitude"":8.2322948,""altitude"":468.5902988971301,""altitudeAccuracy"":null,""heading"":64.9999771118164,""speed"":33.26994705200195}},
{""timestamp"":1744368007850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4139674,""longitude"":8.2326918,""altitude"":468.5661663258189,""altitudeAccuracy"":null,""heading"":65.00007629394531,""speed"":33.24016189575195}},
{""timestamp"":1744368008850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4140918,""longitude"":8.2330909,""altitude"":468.3742177390857,""altitudeAccuracy"":null,""heading"":65.00016784667969,""speed"":33.18033981323242}},
{""timestamp"":1744368009851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4142163,""longitude"":8.2334911,""altitude"":468.36341382369767,""altitudeAccuracy"":null,""heading"":64.99962615966797,""speed"":33.30924987792969}},
{""timestamp"":1744368010851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4143451,""longitude"":8.2338936,""altitude"":468.30291214835086,""altitudeAccuracy"":null,""heading"":65.00019836425781,""speed"":33.24040603637695}},
{""timestamp"":1744368011850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4144735,""longitude"":8.2342928,""altitude"":468.2575361711608,""altitudeAccuracy"":null,""heading"":64.0031967163086,""speed"":33.21111297607422}},
{""timestamp"":1744368012851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4146009,""longitude"":8.234693,""altitude"":468.2640184389594,""altitudeAccuracy"":null,""heading"":63.999969482421875,""speed"":33.22990417480469}},
{""timestamp"":1744368013850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4147275,""longitude"":8.2350916,""altitude"":468.36725481839403,""altitudeAccuracy"":null,""heading"":64.00009155273438,""speed"":33.200172424316406}},
{""timestamp"":1744368014851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4148539,""longitude"":8.2354889,""altitude"":468.3564510143016,""altitudeAccuracy"":null,""heading"":64.99707794189453,""speed"":33.179107666015625}},
{""timestamp"":1744368015850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4149817,""longitude"":8.2358873,""altitude"":468.556199644723,""altitudeAccuracy"":null,""heading"":65.00021362304688,""speed"":33.140438079833984}},
{""timestamp"":1744368016852,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4151081,""longitude"":8.236286,""altitude"":468.5367528071279,""altitudeAccuracy"":null,""heading"":64.99982452392578,""speed"":33.199649810791016}},
{""timestamp"":1744368017851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4152399,""longitude"":8.2366802,""altitude"":468.4719303327372,""altitudeAccuracy"":null,""heading"":64.00304412841797,""speed"":33.26057434082031}},
{""timestamp"":1744368018850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4153708,""longitude"":8.2370746,""altitude"":468.40848777403835,""altitudeAccuracy"":null,""heading"":63.00367736816406,""speed"":33.131656646728516}},
{""timestamp"":1744368019852,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4155049,""longitude"":8.2374661,""altitude"":468.42908572502733,""altitudeAccuracy"":null,""heading"":62.00358200073242,""speed"":33.12099838256836}},
{""timestamp"":1744368020850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4156434,""longitude"":8.2378529,""altitude"":468.4410117600868,""altitudeAccuracy"":null,""heading"":60.0074577331543,""speed"":33.09178924560547}},
{""timestamp"":1744368021851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.415789,""longitude"":8.2382338,""altitude"":468.2403697421097,""altitudeAccuracy"":null,""heading"":59.00400924682617,""speed"":33.04109191894531}},
{""timestamp"":1744368022851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4159394,""longitude"":8.2386109,""altitude"":467.89662998771837,""altitudeAccuracy"":null,""heading"":58.00387954711914,""speed"":33.05070114135742}},
{""timestamp"":1744368023851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4160988,""longitude"":8.2389853,""altitude"":467.6621308327475,""altitudeAccuracy"":null,""heading"":56.00813293457031,""speed"":33.08113479614258}},
{""timestamp"":1744368024850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4162649,""longitude"":8.2393489,""altitude"":467.5322785922091,""altitudeAccuracy"":null,""heading"":54.00871276855469,""speed"":33.10100173950195}},
{""timestamp"":1744368025850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4164405,""longitude"":8.2397074,""altitude"":467.42511229677143,""altitudeAccuracy"":null,""heading"":53.004371643066406,""speed"":33.10050964355469}},
{""timestamp"":1744368026851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4166177,""longitude"":8.2400612,""altitude"":467.05747736174476,""altitudeAccuracy"":null,""heading"":52.00440216064453,""speed"":33.130306243896484}},
{""timestamp"":1744368027851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4168014,""longitude"":8.2404004,""altitude"":467.1181011526139,""altitudeAccuracy"":null,""heading"":51.00440979003906,""speed"":33.12041091918945}},
{""timestamp"":1744368028851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4169893,""longitude"":8.2407417,""altitude"":466.5000601536067,""altitudeAccuracy"":null,""heading"":50.004798889160156,""speed"":33.14023971557617}},
{""timestamp"":1744368029851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4171839,""longitude"":8.241073,""altitude"":466.19763520856395,""altitudeAccuracy"":null,""heading"":48.0092658996582,""speed"":33.18013381958008}},
{""timestamp"":1744368030851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4173862,""longitude"":8.241396,""altitude"":465.81086457405127,""altitudeAccuracy"":null,""heading"":47.00455856323242,""speed"":33.1900634765625}},
{""timestamp"":1744368031850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4175912,""longitude"":8.2417202,""altitude"":465.47390310093755,""altitudeAccuracy"":null,""heading"":46.004608154296875,""speed"":33.26968002319336}},
{""timestamp"":1744368032851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4178014,""longitude"":8.2420375,""altitude"":465.45918900008445,""altitudeAccuracy"":null,""heading"":45.0045166015625,""speed"":33.289886474609375}},
{""timestamp"":1744368033851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4180139,""longitude"":8.2423492,""altitude"":464.6923012750573,""altitudeAccuracy"":null,""heading"":44.00459671020508,""speed"":33.29985809326172}},
{""timestamp"":1744368034851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4182325,""longitude"":8.2426545,""altitude"":464.42845733935894,""altitudeAccuracy"":null,""heading"":42.00947570800781,""speed"":33.35918426513672}},
{""timestamp"":1744368035851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4184584,""longitude"":8.2429464,""altitude"":463.90367262314487,""altitudeAccuracy"":null,""heading"":40.0091438293457,""speed"":33.25967788696289}},
{""timestamp"":1744368036851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4186878,""longitude"":8.2432289,""altitude"":463.5834410017578,""altitudeAccuracy"":null,""heading"":38.017311096191406,""speed"":33.228050231933594}},
{""timestamp"":1744368037851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.418926,""longitude"":8.2434948,""altitude"":463.14557904834504,""altitudeAccuracy"":null,""heading"":36.009037017822266,""speed"":33.27846908569336}},
{""timestamp"":1744368038851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4191693,""longitude"":8.2437472,""altitude"":462.77237626920333,""altitudeAccuracy"":null,""heading"":34.008731842041016,""speed"":33.32823181152344}},
{""timestamp"":1744368039851,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.4194175,""longitude"":8.2439968,""altitude"":462.399299605668,""altitudeAccuracy"":null,""heading"":33.00420379638672,""speed"":33.30931091308594}},
{""timestamp"":1744368040850,""coords"":{""accuracy"":3.7899999618530273,""latitude"":47.419669,""longitude"":8.2442332,""altitude"":461.84032753158067,""altitudeAccuracy"":null,""heading"":32.00387954711914,""speed"":33.29926300048828}},
{}]";

    #endregion

    #region TimeLineEntries

    public static List<IBaseEntry> TimeLineEntries => new List<IBaseEntry>()
    {
        { new BahnhofEntry() { Location = new NetTopologySuite.Geometries.Coordinate(47.3916525103458, 8.1696260844281), Name = "Lenzburg" } },
        { new KombiniertSignalEntry() { Location = new NetTopologySuite.Geometries.Coordinate(47.3951799713142, 8.15822796604049), SignalSeite = Shared.Models.DisplaySeite.Einfahrt } },
        { new KombiniertSignalEntry() { Location = new NetTopologySuite.Geometries.Coordinate(47.4042555578771, 8.13428282621334), SignalSeite = Shared.Models.DisplaySeite.Ausfahrt } },
        { new KombiniertSignalEntry() { Location = new NetTopologySuite.Geometries.Coordinate(47.4035723847273, 8.12776686256773), SignalSeite = Shared.Models.DisplaySeite.Ausfahrt_Abschnitt } },
        { new BahnhofEntry() { Location = new NetTopologySuite.Geometries.Coordinate(47.4032760934054, 8.12615590038544), Name = "Rupperswil" } }
    };

    #endregion

    #endregion
}
