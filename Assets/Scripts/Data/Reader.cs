using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper.Configuration;

namespace Smug.Data
{
    public static class Reader
    {
        public static List<PlanetData> ReadPlanets(string path)
        {
            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ",",
                MissingFieldFound = null
            };

            using var reader = new StreamReader(path);
            using var csv = new CsvHelper.CsvReader(reader, configuration);
            return csv.GetRecords<PlanetData>().ToList();
        }

        public static List<SerializablePlanetData> ToSerializable(this List<PlanetData> list)
        {
            return list.Select(planetData => planetData.ToSerializable()).ToList();
        }
    }
}