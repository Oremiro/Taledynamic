using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Newtonsoft.Json;

namespace Taledynamic.Sheets
{
    class Program
    {
        static string[] Scopes = {SheetsService.Scope.Spreadsheets};
        static string ApplicationName = "Taledynamic";

        public static string spreadsheetId = null;

        public static SheetsService InvokeAPI()
        {
            UserCredential credential;

            using (var stream = new FileStream("../credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            SheetsService service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            return service;
        }

        public static String CreateSpreadsheet(SheetsService service, string spreadsheetName="")
        {
            Spreadsheet requestBody = new Spreadsheet();
            requestBody.Properties = new SpreadsheetProperties();
            requestBody.Properties.Title = spreadsheetName;
            SpreadsheetsResource.CreateRequest request = service.Spreadsheets.Create(requestBody);
            Spreadsheet response = request.Execute(); // await request.ExecuteAsync();

            return response.SpreadsheetId;
        }

        public static String GetCells(SheetsService service, /*String spreadsheetId,*/ string tableNum, string range="A1:E10")
        {
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, tableNum+"!"+range);
            ValueRange response = request.Execute();

            String values = JsonConvert.SerializeObject(response.Values);
            
            return values;

            //IList<IList<Object>> values = response.Values;
            /*if (values != null && values.Count > 0)
            {
                int n = 0;
                foreach (var row in values) { if (row.Count > n) { n = row.Count; } }
                for (int i = 0; i < values.Count; i++)
                {
                    Console.Write("| ");
                    for (int j = 0; j < n; j++)
                    {

                        if (j < values[i].Count)
                        {
                            String val = values[i][j].ToString();
                            if (val != "")
                                Console.Write("{0} | ", values[i][j]);
                            else
                                Console.Write("- | ");
                        }
                        else
                            Console.Write("- | ");
                    }
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("No data found.");
            }*/
        }

        public static void UpdateCell(SheetsService service, String spreadsheetId, string tableNum, string range, string Value)
        {
            String rangeWrite = tableNum+"!"+range;

            var cellData = new List<object>() { Value };
            ValueRange valueRange = new ValueRange();
            valueRange.MajorDimension = "ROWS";
            valueRange.Values = new List<IList<object>> { cellData };

            SpreadsheetsResource.ValuesResource.UpdateRequest update =
                service.Spreadsheets.Values.Update(valueRange, spreadsheetId, rangeWrite);

            update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
            UpdateValuesResponse response = update.Execute();
        }

        public static void UpdateCells(SheetsService service, String spreadsheetId, string tableNum, String[,] Value)
        {

            for (int i=0; i < Value.GetLength(0); i++)
            {
                for (int j=0; j < Value.GetLength(1); j++)
                {
                    String rangeWrite = tableNum + "!R" + (i+1) + "C" + (j+1);

                    var cellData = new List<object>() { Value[i, j] };
                    ValueRange valueRange = new ValueRange();
                    valueRange.MajorDimension = "ROWS";//"COLUMNS"
                    valueRange.Values = new List<IList<object>> { cellData };

                    SpreadsheetsResource.ValuesResource.UpdateRequest update =
                        service.Spreadsheets.Values.Update(valueRange, spreadsheetId, rangeWrite);

                    update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
                    UpdateValuesResponse response = update.Execute();
                }
            }

            //Console.WriteLine(response.UpdatedCells);
        }

        public static void AppendCell(SheetsService service, String spreadsheetId, string tableNum, string range, string Value)
        {
            String rangeWrite = tableNum + "!" + range;

            var cellData = new List<object>() { Value };
            ValueRange valueRange = new ValueRange();
            valueRange.MajorDimension = "ROWS";
            valueRange.Values = new List<IList<object>> { cellData };

            SpreadsheetsResource.ValuesResource.AppendRequest append =
                service.Spreadsheets.Values.Append(valueRange, spreadsheetId, rangeWrite);

            append.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.RAW;
            append.InsertDataOption = SpreadsheetsResource.ValuesResource.AppendRequest.InsertDataOptionEnum.INSERTROWS;
            AppendValuesResponse response = append.Execute();

            //Console.WriteLine(JsonConvert.SerializeObject(response));
        }

        public static void ClearCell(SheetsService service, String spreadsheetId, string tableNum, string range = "A1:E10")
        {
            String rangeClear = tableNum + "!" + range;
            ClearValuesRequest requestBody = new ClearValuesRequest();
            SpreadsheetsResource.ValuesResource.ClearRequest request = service.Spreadsheets.Values.Clear(requestBody, spreadsheetId, rangeClear);
            ClearValuesResponse response = request.Execute();
        }

        static void Main(string[] args)
        {
            SheetsService service = InvokeAPI();

            // jude // tale // natan // projes
            String spreadsheetId = "13WUMGvFSi2iOeudxSsAULafgQRegoHy40hSp3dLTkn8";
            //String spreadsheetId = "1PbrrUAO-TTPs4fFyx590Io7VO-Erlntbomg0rEqq6G4"; 
            //String spreadsheetId = "1_4yQUfmsUeJHZOjFYERoZe1wZtWeqIOpijbZ6lULKJI"; 
            //String spreadsheetId = "14JxSgVtHnJwfoXBzU7m3hCdeSU-4D_TyWDY86-h5lvc"; 
            Program.spreadsheetId = spreadsheetId;

            /*String[,] table = new string[,]
            {
                {"Имя", "Номер", "код резервации", "время" },
                {"Адам", "679", "977", "15:16" },
                {"[Данные удалены]", "1001", "███", "████" }
            };
            UpdateCells(service, spreadsheetId, "Лист1", table);*/

            /*UpdateCell(service, spreadsheetId, "Лист1", "A1", "Name");
            UpdateCell(service, spreadsheetId, "Лист1", "B1", "Surname");
            UpdateCell(service, spreadsheetId, "Лист1", "C1", "Класс");*/

            String response = GetCells(service, "Лист1");
            var values = JsonConvert.DeserializeObject<List<IList<Object>>>(response);

            //List<IList<Object>> values = new List<IList<Object>>();

            if (values != null && values.Count > 0)
            {
                int n = 0;
                foreach (var row in values) { if (row.Count > n) { n = row.Count; } }
                for (int i = 0; i < values.Count; i++)
                {
                    Console.Write("| ");
                    for (int j = 0; j < n; j++)
                    {

                        if (j < values[i].Count)
                        {
                            String val = values[i][j].ToString();
                            if (val != "")
                                Console.Write("{0} | ", values[i][j]);
                            else
                                Console.Write("- | ");
                        }
                        else
                            Console.Write("- | ");
                    }
                    Console.WriteLine();
                }
            }





        }
    }
}