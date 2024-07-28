using CsvHelper;
using CsvHelper.Configuration;
using InvestorsApi.Models;
using InvestorsApi.Repositories;
using System.Globalization;

namespace InvestorsApi.Data
{
    public static class DbInitializer
    {
        public static void Initialize(InvestorsContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (context.Investors.Any())
            {
                return;
            }

            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null
            };

            using var reader = new StreamReader("data.csv");
            using var csv = new CsvReader(reader, csvConfig);
            var records = csv.GetRecords<InvestorRecord>().ToList();
            var investors = new Dictionary<string, Investor>();

            foreach (var record in records)
            {
                if (!investors.ContainsKey(record.InvestorName))
                {
                    investors[record.InvestorName] = new Investor
                    {
                        Name = record.InvestorName,
                        InvestorType = record.InvestorType,
                        Country = record.InvestorCountry,
                        DateAdded = DateTime.UtcNow,
                        LastUpdated = DateTime.UtcNow,
                    };
                }

                investors[record.InvestorName].Commitments.Add(new Commitment
                {
                    AssetClass = record.CommitmentAssetClass,
                    Amount = record.CommitmentAmount,
                    Currency = record.CommitmentCurrency
                });
            }

            context.Investors.AddRange(investors.Values);
            context.SaveChanges();
        }
    }

    public class InvestorRecord
    {
        public string InvestorName { get; set; }
        public string InvestorType { get; set; }
        public string InvestorCountry { get; set; }
        public string CommitmentAssetClass { get; set; }
        public decimal CommitmentAmount { get; set; }
        public string CommitmentCurrency { get; set; }
    }
}
