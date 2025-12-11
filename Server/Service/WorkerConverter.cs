using System.IO;
using System.Text;
using Core;
using ExcelDataReader;
namespace Server.Service
{
    public class WorkerConverter
    {
        
        public static List<ProjectHour> Convert(Stream stream)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            
            IExcelDataReader reader =  ExcelReaderFactory.CreateOpenXmlReader(stream);
            

            var ds = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = false
                }
            });

            List<ProjectHour> result = new();
            int row_no = 1;
            while (row_no < ds.Tables[0].Rows.Count)
            {
                var row = ds.Tables[0].Rows[row_no];
                ProjectHour p = new ProjectHour();
                p.Medarbejder = row[0].ToString();
                p.Dato = DateTime.Parse(row[1].ToString());
                p.Stoptid = DateTime.Parse(row[2].ToString());
                p.Timer = Decimal.Parse(row[5].ToString());
                p.Type = row[6].ToString();
                p.Kostpris = Decimal.Parse(row[8].ToString());
                
                // other fields here...

                result.Add(p);
                Console.WriteLine($"dato= {p.Dato}, stop tid = {p.Stoptid} Timer = {p.Timer}, Type = {p.Type}  kostpris = {p.Kostpris}");
                row_no++;

            }
            return result;

        }
    }
}
