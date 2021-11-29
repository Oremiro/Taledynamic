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

        public static void GetAllTable(SheetsService service, String spreadsheetId, string tableNum, string range="A1:E10")
        {
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, tableNum+"!"+range);
            //String[] ranges = {"Лист1!A1:E10", "Лист2!A1:E4"};
            //BatchGetValuesResponse response2 =
            //        service.Spreadsheets.Values.BatchGet(spreadsheetId).setRanges(ranges).Execute();

            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;
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
            else
            {
                Console.WriteLine("No data found.");
            }
        }

        public static void UpdateCell(SheetsService service, String spreadsheetId, string tableNum, string range, string Value="")
        {
            String rangeWrite = tableNum+"!"+range;

            var cellData = new List<object>() { Value };
            ValueRange valueRange = new ValueRange();
            valueRange.MajorDimension = "ROWS";//"COLUMNS"
            valueRange.Values = new List<IList<object>> { cellData };

            SpreadsheetsResource.ValuesResource.UpdateRequest update =
                service.Spreadsheets.Values.Update(valueRange, spreadsheetId, rangeWrite);

            update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
            UpdateValuesResponse response = update.Execute();

            Console.WriteLine(response.UpdatedCells);
        }

        public static void AppendCell(SheetsService service, String spreadsheetId, string tableNum, string range, string Value = "")
        {
            String rangeWrite = tableNum + "!" + range;

            var cellData = new List<object>() { Value };
            ValueRange valueRange = new ValueRange();
            valueRange.Values = new List<IList<object>> { cellData };

            SpreadsheetsResource.ValuesResource.AppendRequest append =
                service.Spreadsheets.Values.Append(valueRange, spreadsheetId, rangeWrite);

            append.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.RAW;
            append.InsertDataOption = SpreadsheetsResource.ValuesResource.AppendRequest.InsertDataOptionEnum.INSERTROWS;
            AppendValuesResponse response = append.Execute();

            Console.WriteLine(JsonConvert.SerializeObject(response));

        }

        public static void Clear(SheetsService service, String spreadsheetId, string tableNum, string range = "A1:E10")
        {
            String rangeClear = tableNum + "!" + range;
            ClearValuesRequest requestBody = new ClearValuesRequest();
            SpreadsheetsResource.ValuesResource.ClearRequest request = service.Spreadsheets.Values.Clear(requestBody, spreadsheetId, rangeClear);
            ClearValuesResponse response = request.Execute();

            Console.WriteLine(JsonConvert.SerializeObject(response));

        }

        static void Main(string[] args)
        {
            UserCredential credential;

            using (var stream = new FileStream("../../credentials.json", FileMode.Open, FileAccess.Read))
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


            //String spreadsheetId = "13WUMGvFSi2iOeudxSsAULafgQRegoHy40hSp3dLTkn8"; ///
            String spreadsheetId = "1PbrrUAO-TTPs4fFyx590Io7VO-Erlntbomg0rEqq6G4";
            //String spreadsheetId = "1_4yQUfmsUeJHZOjFYERoZe1wZtWeqIOpijbZ6lULKJI";
            //String spreadsheetId = "14JxSgVtHnJwfoXBzU7m3hCdeSU-4D_TyWDY86-h5lvc";

            //GetAllTable(service, spreadsheetId, "Лист1");

            UpdateCell(service, spreadsheetId, "Лист1", "B2", "Fuck you 2");




            /*String rangeWrite2 = "Лист1!B2";

            var cellsValues = new List<object>() { "Cell-Text" };
            ValueRange valueRange2 = new ValueRange().Values = new List<IList<object>> { cellsValues };
            SpreadsheetsResource.ValuesResource.AppendRequest append =
                service.Spreadsheets.Values.Append(valueRange2, spreadsheetId, rangeWrite2);

            append.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.RAW;
            AppendValuesResponse result2 = append.Execute();*/


        }
    }
}