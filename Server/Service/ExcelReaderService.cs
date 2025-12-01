using Core;
using ExcelDataReader;
using System.Text;

namespace Server.Service
{
    public class ExcelReaderService
    {
        public ExcelReaderService()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        public List<ProjectHour> ParseHours(Stream stream)
        {
            var list = new List<ProjectHour>();
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                reader.Read(); // Skip header

                while (reader.Read())
                {
                    if (reader.GetValue(0) == null) continue;

                    var h = new ProjectHour();
                    
                    try 
                    {
                        // 1. DATO (Kolonne B / Index 1)
                        var rawDato = reader.GetValue(1);
                        h.Dato = ParseDate(rawDato);

                        // 2. STOPTID (Kolonne C / Index 2)
                        var rawStop = reader.GetValue(2);
                        
                        h.Stoptid = ParseDate(rawStop);

                        // 3. TIMER (Kolonne F / Index 5)
                        var timerVal = reader.GetValue(5)?.ToString();
                        if (decimal.TryParse(timerVal, out var t)) h.Timer = t;

                        // 4. TYPE (Kolonne G / Index 6)
                        h.Type = reader.GetValue(6)?.ToString();

                        // 5. KOSTPRIS (Kolonne I / Index 8)
                        var kostVal = reader.GetValue(8)?.ToString();
                        if (decimal.TryParse(kostVal, out var k)) h.Kostpris = k;

                        h.RawRow = GetRawRowString(reader);
                        list.Add(h);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Fejl ved læsning af række: {ex.Message}");
                    }
                }
            }
            return list;
        }

        // Hjælpefunktion til at håndtere alle mulige dato-formater
        private DateTime? ParseDate(object value)
        {
            if (value == null) return null;

            // Hvis det allerede er en DateTime
            if (value is DateTime dt) return dt;

            // Hvis det er et tal (Excel format)
            if (value is double dbl) 
            {
                try { return DateTime.FromOADate(dbl); } catch {}
            }

            // Hvis det er tekst string
            string s = value.ToString();
            if (string.IsNullOrWhiteSpace(s)) return null;

            if (DateTime.TryParse(s, out var result)) return result;

            return null;
        }

        public List<ProjectMaterial> ParseMaterials(Stream stream)
{
     var list = new List<ProjectMaterial>();
     using (var reader = ExcelReaderFactory.CreateReader(stream))
     {
         reader.Read(); // Skipper header-rækken
         while (reader.Read())
         {
             if (reader.GetValue(1) == null && reader.GetValue(2) == null) continue;

             var m = new ProjectMaterial();
             
             try 
             {
                 m.Beskrivelse = reader.GetValue(1)?.ToString();
                 if (decimal.TryParse(reader.GetValue(2)?.ToString(), out var kost)) m.Kostpris = kost;
                 if (decimal.TryParse(reader.GetValue(4)?.ToString(), out var ant)) m.Antal = ant;
                 if (decimal.TryParse(reader.GetValue(17)?.ToString(), out var tot)) m.Total = tot;
                 if (decimal.TryParse(reader.GetValue(19)?.ToString(), out var av)) m.Avance = av;
                 if (decimal.TryParse(reader.GetValue(20)?.ToString(), out var dg)) m.Dækningsgrad = dg;
                 m.RawRow = GetRawRowString(reader);
                 
                 list.Add(m);
             }
             catch (Exception ex)
             {
                 Console.WriteLine($"Fejl ved læsning af materiale-række: {ex.Message}");
             }
         }
     }
     return list;
}

        private string GetRawRowString(IExcelDataReader reader)
        {
            var sb = new StringBuilder();
            for(int i = 0; i < reader.FieldCount; i++)
            {
                sb.Append(reader.GetValue(i)?.ToString() + ";");
            }
            return sb.ToString();
        }
    }
}