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
    class GSheets
    {
        //public static string spreadsheetId = null;

        GSheets()
        {

        }

        private static SheetsService InvokeAPI()
        {
            string[] Scopes = { SheetsService.Scope.Spreadsheets };
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
                ApplicationName = "Taledynamic",
            });

            return service;
        }

        public static String CreateSpreadsheet(SheetsService service, string spreadsheetName)
        {
            Spreadsheet requestBody = new Spreadsheet();
            requestBody.Properties = new SpreadsheetProperties();
            requestBody.Properties.Title = spreadsheetName;
            SpreadsheetsResource.CreateRequest request = service.Spreadsheets.Create(requestBody);
            Spreadsheet response = request.Execute(); // await request.ExecuteAsync();

            return response.SpreadsheetId; // TODO: Сохранить в БД для линковки с рабочим простарством
        }

        public static void DeleteSpreadsheet(SheetsService service, String spreadsheetId)
        {
            // TODO: Из БД достать spreadsheetId 
        }

        public static String GetCells(SheetsService service, String spreadsheetId, string tableName, string cellRange="")
        {
            String range;
            if (cellRange == "")
                range = tableName;
            else
                range = tableName + "!" + cellRange;
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);
            ValueRange response = request.Execute();

            String values = JsonConvert.SerializeObject(response.Values);
            
            return values;
        }

        public static void UpdateCell(SheetsService service, String spreadsheetId, string tableName, string cellRange, string Value)
        {
            String range = tableName+"!"+cellRange;

            var cellData = new List<object>() { Value };
            ValueRange valueRange = new ValueRange();
            valueRange.MajorDimension = "ROWS";
            valueRange.Values = new List<IList<object>> { cellData };

            SpreadsheetsResource.ValuesResource.UpdateRequest update =
                service.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);

            update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW; //USER_ENTERED
            //UpdateValuesResponse response = update.Execute();
            update.Execute();
        }

        public static void UpdateCells(SheetsService service, String spreadsheetId, string tableName, String[,] Value)
        {
            for (int i=0; i < Value.GetLength(0); i++)
            {
                for (int j=0; j < Value.GetLength(1); j++)
                {
                    String range = tableName + "!R" + (i+1) + "C" + (j+1);

                    var cellData = new List<object>() { Value[i, j] };
                    ValueRange valueRange = new ValueRange();
                    valueRange.MajorDimension = "ROWS";//"COLUMNS"
                    valueRange.Values = new List<IList<object>> { cellData };

                    SpreadsheetsResource.ValuesResource.UpdateRequest update =
                        service.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);

                    update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
                    //UpdateValuesResponse response = update.Execute();
                    update.Execute();
                }
            }
            //Console.WriteLine(response.UpdatedCells);
        }

        public static void AppendCell(SheetsService service, String spreadsheetId, string tableName, string cellRange, string Value)
        {
            String range = tableName+"!"+cellRange;

            var cellData = new List<object>() { Value };
            ValueRange valueRange = new ValueRange();
            valueRange.MajorDimension = "ROWS";
            valueRange.Values = new List<IList<object>> { cellData };

            SpreadsheetsResource.ValuesResource.AppendRequest append =
                service.Spreadsheets.Values.Append(valueRange, spreadsheetId, range);

            append.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.RAW; //USER_ENTERED
            append.InsertDataOption = SpreadsheetsResource.ValuesResource.AppendRequest.InsertDataOptionEnum.INSERTROWS;
            //AppendValuesResponse response = append.Execute();
            //Console.WriteLine(JsonConvert.SerializeObject(response));
            append.Execute();
        }

        public static void ClearCells(SheetsService service, String spreadsheetId, string tableName, string cellRange="")
        {
            String range;
            if (cellRange == "")
                range = tableName;
            else
                range = tableName+"!"+cellRange;
            ClearValuesRequest requestBody = new ClearValuesRequest();
            SpreadsheetsResource.ValuesResource.ClearRequest request = 
                service.Spreadsheets.Values.Clear(requestBody, spreadsheetId, range);
            ClearValuesResponse response = request.Execute();
        }


        static void justPrint(List<IList<Object>> values)
        {
            Console.WriteLine("-------------");
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
            Console.WriteLine("-------------");
        }

        static void Main(string[] args)
        {
            SheetsService service = InvokeAPI();

            // jude // tale // natan // projes
            String spreadsheetId = "13WUMGvFSi2iOeudxSsAULafgQRegoHy40hSp3dLTkn8";
            //String spreadsheetId = "1PbrrUAO-TTPs4fFyx590Io7VO-Erlntbomg0rEqq6G4"; 
            //String spreadsheetId = "1_4yQUfmsUeJHZOjFYERoZe1wZtWeqIOpijbZ6lULKJI"; 
            //String spreadsheetId = "14JxSgVtHnJwfoXBzU7m3hCdeSU-4D_TyWDY86-h5lvc"; 

            String[,] table = new string[,]
            {
                {"████", "████", "████", "████", "████", "████" },
                {"Адам", "████", "[L.O.L]", "████", "████", "████" },
                {"[Данные удалены]", "[Данные удалены]", "███", "[Данные удалены]", "████", "████" }
            };
            //UpdateCells(service, spreadsheetId, "Лист2", table);

            UpdateCell(service, spreadsheetId, "Лист3", "C3", "|i want ");
            UpdateCell(service, spreadsheetId, "Лист3", "D3", "want new");
            UpdateCell(service, spreadsheetId, "Лист3", "E3", "Лист Блять");

            String response = GetCells(service, spreadsheetId, "Лист2", "C3");
            var values = JsonConvert.DeserializeObject<List<IList<Object>>>(response);
            justPrint(values);

        }
    }
}