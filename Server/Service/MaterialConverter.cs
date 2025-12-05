using System.IO;
using System.Text;
using Core;
using ExcelDataReader;
namespace Server.Service
{
    public class MaterialConverter
    {

        public static List<ProjectMaterial> Convert(Stream stream)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            IExcelDataReader reader = ExcelReaderFactory.CreateOpenXmlReader(stream);


            var ds = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = false
                }
            });

            List<ProjectMaterial> result = new();
            int row_no = 1;
            while (row_no < ds.Tables[0].Rows.Count)
            {
                var row = ds.Tables[0].Rows[row_no];
                ProjectMaterial p = new ProjectMaterial();
                p.Beskrivelse = row[1].ToString();
                p.Kostpris = Decimal.Parse(row[2].ToString());
                p.Antal = Decimal.Parse(row[4].ToString());
                p.Leverandør = row[8].ToString();
                p.Total = Decimal.Parse(row[17].ToString());
                p.Avance = Decimal.Parse(row[19].ToString());
                p.Dækningsgrad = string.IsNullOrEmpty(row[20].ToString()) ? 0 : decimal.Parse(row[20].ToString());

                
                result.Add(p);
                Console.WriteLine($"Beskrivelse= {p.Beskrivelse}, Kostpris = {p.Kostpris} Antal = {p.Antal}, Total = {p.Total}  Avance = {p.Avance}, dækningsgrad = {p.Dækningsgrad}");
                row_no++;

            }
            return result;

        }
    }
}
