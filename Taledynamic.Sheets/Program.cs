using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace SheetsQuickstart
{
    class Program
    {
        static string[] Scopes = { SheetsService.Scope.Spreadsheets};
        static string ApplicationName = "TaleDynamic";

        static void Main(string[] args)
        {
            UserCredential credential;

            using (var stream =
                new FileStream("../../credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });


            //String spreadsheetId = "13WUMGvFSi2iOeudxSsAULafgQRegoHy40hSp3dLTkn8"; ///
            String spreadsheetId = "1PbrrUAO-TTPs4fFyx590Io7VO-Erlntbomg0rEqq6G4";
            //String spreadsheetId = "1_4yQUfmsUeJHZOjFYERoZe1wZtWeqIOpijbZ6lULKJI";

            String rangeRead = "Лист1!A1:E5";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, rangeRead);


            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                int n = 0;
                foreach(var row in values) { if (row.Count > n) {n = row.Count; } }
                for (int i = 0; i < values.Count; i++)
                {
                    Console.Write("| ");
                    for (int j = 0; j < n; j++)
                    {
                        if (j < values[i].Count && values[i][j] != "")
                            Console.Write("{0} | ", values[i][j]);
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
    }
}