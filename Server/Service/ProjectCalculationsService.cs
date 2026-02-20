using Core;
using Server.Repositories.HourRepositories;
using Server.Repositories.MaterialRepositories;
using Server.Repositories.ProjectRepositories;
using System.Globalization;

namespace Server.Service;

public class ProjectCalculationsService
{
    // Vi bruger Dependency Injection til at få adgang til vores 3 repositories.
    // Dette gør, at servicen ikke selv skal vide, hvordan man snakker SQL, 
    // men blot beder om data.
    private readonly IProjectRepository _projectRepo;
    private readonly IHourRepository _hourRepo;
    private readonly IMaterialRepository _materialRepo;

    public ProjectCalculationsService(
        IProjectRepository projectRepo,
        IHourRepository hourRepo,
        IMaterialRepository materialRepo)
    {
        _projectRepo = projectRepo;
        _hourRepo = hourRepo;
        _materialRepo = materialRepo;
    }

    // Hovedmetoden: Tager et projekt-ID og returnerer et færdigt 'Calculation' objekt
    // med alle tal lagt sammen og sorteret.
    public Calculation? CalculateProject(int projectId)
    {
        // Vi henter stamdata, timer og materialer hver for sig.
        var project = _projectRepo.GetById(projectId);
        if (project == null) return null; // Stop hvis projektet ikke findes

        
        var hours = _hourRepo.GetByProjectId(projectId);
        var materials = _materialRepo.GetByProjectId(projectId);
        
        // Vi starter vores "Data Transfer Object", som skal sendes til Client.
        var dto = new Calculation
        {
            ProjectId = project.ProjectId,
            Project = project,
            Hours = hours, // Rå liste
            Materials = materials // Rå liste
        };

        // Vi løber igennem alle materialelinjer for at finde total kostpris (indkøb) 
        // og total salgspris.
        foreach (var m in materials)
        {
            dto.TotalKostPrisMaterialer += (m.Kostpris * m.Antal);
            dto.TotalPrisMaterialer += m.Total; // "Total her kommer fra excel filen"
        }

        // Vi grupperer timerne per medarbejder, da en medarbejder kan have flere timeregistreringer.
        var employeeGroups = hours.GroupBy(x => x.Medarbejder);
        foreach (var group in employeeGroups)
        {
            // Find medarbejderens "Basistype" (Svend, Lærling, osv.)
            // Vi kigger på alle deres timer og ser bort fra "overtid" for at finde grundtypen.
            // Hvis intet er fundet, antager vi det er en "svend".
            var normalType = group
                .FirstOrDefault(h => h.Type != null && !h.Type.ToLower().Contains("overtid"))?
                .Type?.ToLower() ?? "svend";

            // Find den timepris, der er aftalt på selve projektet fra Project-tabellen
            decimal grundSats = 0;
            if (normalType.Contains("lærling")) grundSats = project.LærlingTimePris;
            else if (normalType.Contains("konsulent")) grundSats = project.KonsulentTimePris;
            else if (normalType.Contains("arbejdsmand")) grundSats = project.ArbejdsmandTimePris;
            else grundSats = project.SvendTimePris;
            
            // Nu beregner vi prisen for hver time-registrering
            foreach (var h in group)
            {
                decimal faktor = 1.0m;
                string typeLower = h.Type?.ToLower() ?? "";

                // Håndter overtidstillæg (f.eks. 50% eller 100% ekstra)
                if (typeLower.Contains("overtid 1")) faktor = 1.5m;
                else if (typeLower.Contains("overtid 2")) faktor = 2.0m;

                // Beregn salgspris: Antal timer * Timepris fra projektet * Overtidsfaktor
                decimal salgsPris = h.Timer * grundSats * faktor;

                dto.TotalPrisTimer += salgsPris;
                dto.TotalKostPrisTimer += h.Kostpris;
            }
        }
        // Simpel summering af antal timer totalt
        dto.TotalTimer = dto.Hours.Sum(h => h.Timer);

       
  

        // A. Gruppering af Timer (Svend, Lærling osv.)
        dto.GroupedHours = hours // liste med timer 
            .GroupBy(h => { // grupperer timer 
                var t = h.Type?.ToLower() ?? ""; // Henter typer og gøre det til lowercase  
                if (t.Contains("overtid 1")) return "Overtid 1";
                if (t.Contains("overtid 2")) return "Overtid 2";
                if (t.Contains("lærling")) return "Lærling";
                if (t.Contains("konsulent")) return "Konsulent";
                if (t.Contains("arbejdsmand")) return "Arbejdsmand";
                return "Svend";
            })
            .Select(g => new HourGroupDto { Type = g.Key, Total = g.Sum(x => x.Timer) }) //gruppens navn og total timer 
            .OrderByDescending(x => x.Total) //sorterer efter flest timer 
            .ToList();

        // B. Gruppering af Materialer (Kundevisning - Kategorier)
        var categories = new Dictionary<string, string[]>
        {
            { "Belysning", new[] { 
                "spot", "lampe", "led", "lys", "armatur", "pendel", "driver", "dæmper", "skinne", 
                "pære", "lyskilde", "halogen", "projektør", "sensor", "pir", "downlight", "uplight", 
                "lysrør", "fatning", "trafo", "transformator", "plafond", "bevægelsessensor",
                "lampeudtag", "corepro", "strømforsyning", "ceiling", "track", "anker", "dekorativ", 
                "spreaderlight", "stipo", "lighstick", "anker&co", "anker & co",
                "philips", "sg", "nordlux", "louis poulsen", "fabbian", "iguzzini", "zumtobel", "vossloh", "osram", "batten", "glimtænder"
            } },
            { "Alarm", new[] {
                "røgdetektor", "abdl", "brandalarm", "detektor", "alarm", "nøgleskab", "cylinder", "almsten sikring aps",
                "kamera", "itv", "overvågning", "hikvision", "axis", "pax", "adgangskontrol", "magnetkontakt", 
                "siréne", "passiv", "infra", "almsten"
            } },
            { "KNX", new[] {
                "ihc", "knx", "wiser", "dali", "devisnow", "relæmodul", "smart", "home", "saas", 
                "programmering", "controller", "astrour", "timer",
                "gateway", "aktuator", "schneider", "abb", "zennio", "buskabel", "fortrådning", "logikmodul"
            } },
            { "Sikringer & Tavler", new[] { 
                "tavle", "sikring", "hpfi", "rce", "automatsikring", "kombiafbryder", "rcbo", 
                "gruppeafbryder", "hovedafbryder", "måler", "målertavle", "smeltesikring", "neozed", 
                "diazed", "din-skinne", "samleskinne", "klemrække", "overspænding", "gruppetavle", 
                "jordspyd", "byggepladstavle", "abs", "din", "byggepladscentral", "pfi", "sikkerhedsafb", "hovedtavle",
                "tavlemateriel", "kontaktor", "transistor", "strømtransformer", "skinnesystem", "effektafbryder"
            } },
            { "Lyd & AV-udstyr", new[] {
                "lydsystem", "højtaler", "westsound", "iport", "connect pro", "av", "lyd", "sonos", "højtalere",
                "hdmi", "skærm", "projektor", "tv", "lydstyring", "forstærker", "beosound", "bang & olufsen", "displayport"
            } },
            { "Netværk & Data", new[] {
                "datakabel", "netværkskabel", "cat5", "cat6", "cat7", "coax", "antennekabel", 
                "utp", "kat", "patchkabel", "patchpanel", "rack", "rj45", "wifi", "unifi", "surf", 
                "vægbøjle", "19\"",
                "switch", "router", "fiber", "sfp", "keystone", "pdu", "server", "accesspoint", "ubiquiti"
            } },
            { "Kabler & Rør", new[] { 
                "kabel", "ledning", "rør", "nkt", "pvi", "flex", "5x1,5", "3x1,5", "5x2,5", "3x2,5", 
                "5x6", "7x1,5", "installationskabel", "gummikabel", "tomrør", "pn", "noflik", "noiklx", 
                "flexrør", "plastrør", "kabelrør", "afumex", "pklj", "pvl", "signalkabel", "forlængerk", 
                "forlængerkabel", "tilslutningstråd", "kobbertråd", "noikal", "caddy", "qaddy", "tromle",
                "jordledning", "brandkabel", "pknm", "pvikly", "halogenfri", "kabelbakke", "gitterbakke", "stålør", "pariser"
            } },
            { "Installation (alt fra køkken til bil lader & solceller)", new[] { 
                "stikkontakt", "afbryder", "underlag", "dåse", "fuga", "opus", "ramme", "tangent", 
                "wago", "muffe", "samlemuffe", "forfradåse", "indmuringsdåse", "loftdåse", "udtag", 
                "roset", "stikprop", "schuko", "blinddæksel", "korrespondance", "krydsning", 
                "tryk", "clips", "dæksel", "lk", "pressemuffe", "stikk", "polykonmuffe", "jung", 
                "blænddæksel", "designramme", "krympemuffe", "jord", "membrandåse", "forgreningsdåse", 
                "afd", "cee", "mat", "boks", "box", "afdækning", "klemme", "hybridstikprop", "modul",
                "cover", "kasse", "låg", "skrue", "spånskrue", "plugs", "kabelkanal", "ledningskanal", "strips", "kabelbinder", 
                "kabelbøjle", "strip", "bøjlebånd", "tape", "dampspærretape", "fzb", "galv", "metal", 
                "krympeflex", "krympeslange", "hulsav", "bor", "fladbor", "spiralbor", "klinge", "savklinge", "lenox", "dymotape", 
                "fuge", "fugemasse", "batteri", "handske", "nitril", "affald", "støvpose", "qaddy", 
                "tromle", "loddekolbe", "rengøring", "støvsugning",
                "vinkel", "bolt", "møtrik", "gevind", "unbrakoskrue", "ankerskrue", "isolering", "brandlukning", "gaggenau", "miele", "quooker", "liebherr", "køleskab", "vinskab", "ovn", "fryseskab", 
                "opvaskemaskine", "kogeplade", "emhætte", "hanstholm", "køkken", "vask", "armatur", "ladestander", 
                "ladeboks", "easee", "zaptec", "ladekabel", "solcelle", "inverter", "hybrid", "batteri", 
                "varmepumpe", "elbil", "montagesystem", "ventilation", "ølund", "udsugning", "kanal", "aggregat", 
                "emhætteaftræk", "solceller", "solcelle", "inverter", "batteri", "S750", "hybrid", "sol", "energi", 
                "varmepumpe", "ladestander", "ladeboks", "easee", "zaptec"
            } }
        };


        dto.GroupedMaterialsClientView = materials 
            .GroupBy(m => { 
                // ÆNDRING: Nu slår vi Beskrivelse og Leverandør sammen til én sætning
                string desc = $"{m.Beskrivelse} {m.Leverandør}".ToLower();
        
                // Find første kategori der matcher
                foreach (var category in categories)
                {
                    if (category.Value.Any(keyword => desc.Contains(keyword))) return category.Key; 
                }
                return "Øvrige materialer"; 
            })
            .Select(g => new ProjectMaterial 
            {
                Beskrivelse = g.Key, 
                Total = g.Sum(x => x.Total) 
            })
            .OrderByDescending(m => m.Total) 
            .ToList();

        // C. Gruppering af Materialer (Intern visning - Leverandør/Navn)
        // Kendte leverandører

        dto.GroupedMaterialsInternView = materials 
            .GroupBy(m => { 
                string desc = $"{m.Beskrivelse} {m.Leverandør}".ToLower();
                foreach (var category in categories)
                {
                    if (category.Value.Any(keyword => desc.Contains(keyword))) return category.Key; 
                }
                return "Øvrige materialer"; 
            })
            .Select(g => new ProjectMaterial 
            {
                Beskrivelse = g.Key, 
                // HER ER ÆNDRINGEN: Brug Kostpris * Antal i stedet for x.Total
                Total = g.Sum(x => x.Kostpris * x.Antal) 
            })
            .OrderByDescending(m => m.Total) 
            .ToList();

        return dto; // Returnerer dto (timer + materialer) 
    }
}