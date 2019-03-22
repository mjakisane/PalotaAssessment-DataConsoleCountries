using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace PalotaInterviewCS
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private const string countriesEndpoint = "https://restcountries.eu/rest/v2/all";

        static void Main(string[] args)
        {
            Country[] countries = GetCountries(countriesEndpoint).GetAwaiter().GetResult();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Palota Interview: Country Facts");
            Console.WriteLine();
            Console.ResetColor();

            Random rnd = new Random(); // random to populate fake answer - you can remove this once you use real values

            //TODO use data operations and data structures to optimally find the correct value (N.B. be aware of null values)

            /*
             * HINT: Sort the list in descending order to find South Africa's place in terms of gini coefficients
             * `Country.Gini` is the relevant field to use here           
             */

            var southAfricanGiniPlace = FindCountryGini(countries, "south africa");
            Console.WriteLine($"1. South Africa's Gini coefficient is the {GetOrdinal(southAfricanGiniPlace)} highest");

            /*
             * HINT: Sort the list in ascending order or just find the Country with the minimum gini coeficient          
             * use `Country.Gini` for the ordering then return the relevant country's name `Country.Name`
             */

            //string lowestGiniCountry = "ExampleCountry"; // Use correct value
            string lowestGiniCountry = GetLowestGiniCountry(countries).Name; // Use correct value
            Console.WriteLine($"2. {lowestGiniCountry} has the lowest Gini Coefficient");

            /*
             * HINT: Group by regions (`Country.Region`), then count the number of unique timezones that the countries in each region span
             * Once you have done the grouping, find the group `Region` with the most timezones and return it's name and the number of unique timezones found          
             */


            var countryDictionary = GetRegionWithMostTimezones(countries);

            string regionWithMostTimezones = countryDictionary.Key;
            int amountOfTimezonesInRegion = countryDictionary.Value;
            Console.WriteLine($"3. {regionWithMostTimezones} is the region that spans most timezones at {amountOfTimezonesInRegion} timezones");

            /*
             * HINT: Count occurances of each currency in all countries (check `Country.Currencies`)
             * Find the name of the currency with most occurances and return it's name (`Currency.Name`) also return the number of occurances found for that currency          
             */


            //string mostPopularCurrency = "ExampleCurrency"; // Use correct value
            //int numCountriesUsedByCurrency = rnd.Next(1, 10); // Use correct value
            var popularCurrency = GetPopularCurrency(countries).FirstOrDefault();
            string mostPopularCurrency = popularCurrency.Value; // Use correct value
            int numCountriesUsedByCurrency = popularCurrency.Key; // Use correct value
            Console.WriteLine($"4. {mostPopularCurrency} is the most popular currency and is used in {numCountriesUsedByCurrency} countries");

            /*
             * HINT: Count the number of occurances of each language (`Country.Languages`) and sort then in descending occurances count (i.e. most populat first)
             * Once done return the names of the top three languages (`Language.Name`)
             */

            string[] mostPopularLanguages = GetPopularLanguages(countries, 3);
            //var aa = retults.
            //string[] mostPopularLanguages = { "ExampleLanguage1", "ExampleLanguage2", "ExampleLanguage3" }; // Use correct values
            Console.WriteLine($"5. The top three popular languages are {mostPopularLanguages[0]}, {mostPopularLanguages[1]} and {mostPopularLanguages[2]}");

            /*
             * HINT: Each country has an array of Bordering countries `Country.Borders`, The array has a list of alpha3 codes of each bordering country `Country.alpha3Code`
             * Sum up the population of each country (`Country.Population`) along with all of its bordering countries's population. Sort this list according to the combined population descending
             * Find the country with the highest combined (with bordering countries) population the return that country's name (`Country.Name`), the number of it's Bordering countries (`Country.Borders.length`) and the combined population
             * Be wary of null values           
             */

            var countryWithHighestPopulation = GetCountryWithHeighestPopulation(countries);
            string countryWithBorderingCountries = countryWithHighestPopulation.Name; // Use correct value
            int numberOfBorderingCountries = countryWithHighestPopulation.Borders; // Use correct value
            long combinedPopulation = countryWithHighestPopulation.HighestTotalPopulation; // Use correct value
            Console.WriteLine($"6. {countryWithBorderingCountries} and it's {numberOfBorderingCountries} bordering countries has the highest combined population of {combinedPopulation}");

            /*
             * HINT: Population density is calculated as (population size)/area, i.e. `Country.Population/Country.Area`
             * Calculate the population density of each country and sort by that value to find the lowest density
             * Return the name of that country (`Country.Name`) and its calculated density.
             * Be wary of null values when doing calculations           
             */
            var lowPopDensityCountry = GethighestOrLowestDensityCountry(countries, "low");

            string lowPopDensityName = lowPopDensityCountry.Key; // Use correct value
            double lowPopDensity = lowPopDensityCountry.Value; // Use correct value
            Console.WriteLine($"7. {lowPopDensityName} has the lowest population density of {lowPopDensity}");

            /*
             * HINT: Population density is calculated as (population size)/area, i.e. `Country.Population/Country.Area`
             * Calculate the population density of each country and sort by that value to find the highest density
             * Return the name of that country (`Country.Name`) and its calculated density.
             * Be wary of any null values when doing calculations. Consider reusing work from above related question           
             */
            var highPopDensityCountry = GethighestOrLowestDensityCountry(countries, "high");
            string highPopDensityName = highPopDensityCountry.Key; // Use correct value
            double highPopDensity = highPopDensityCountry.Value; // Use correct value
            Console.WriteLine($"8. {highPopDensityName} has the highest population density of {highPopDensity}");

            /*
             * HINT: Group by subregion `Country.Subregion` and sum up the area (`Country.Area`) of all countries per subregion
             * Sort the subregions by the combined area to find the maximum (or just find the maximum)
             * Return the name of the subregion
             * Be wary of any null values when summing up the area           
             */

            string largestAreaSubregion = GetLargestAreaSubregion(countries); // Use correct value
            Console.WriteLine($"9. {largestAreaSubregion} is the subregion that covers the most area");

            /*
             * HINT: Group by regional blocks (`Country.RegionalBlocks`). For each regional block, average out the gini coefficient (`Country.Gini`) of all member countries
             * Sort the regional blocks by the average country gini coefficient to find the lowest (or find the lowest without sorting)
             * Return the name of the regional block (`RegionalBloc.Name`) along with the calculated average gini coefficient
             */

            var equalRegionalBlock = countries.Where(oo => oo.Gini != null).GroupBy(o => o.RegionalBlocs).Select(x => new { x.Key, AverageGiniCoefficient = x.Average(z => z.Gini) }).OrderBy(y => y.AverageGiniCoefficient).FirstOrDefault();

            string mostEqualRegionalBlock = equalRegionalBlock.Key.FirstOrDefault().Name; // Use correct value
            double lowestRegionalBlockGini = equalRegionalBlock.AverageGiniCoefficient.Value; // Use correct value
            Console.WriteLine($"10. {mostEqualRegionalBlock} is the regional block with the lowest average Gini coefficient of {lowestRegionalBlockGini}");
        }

        /// <summary>
        /// Gets the countries from a specified endpiny
        /// </summary>
        /// <returns>The countries.</returns>
        /// <param name="path">Path endpoint for the API.</param>
        static async Task<Country[]> GetCountries(string path)
        {
            Country[] countries = null;
            //TODO get data from endpoint and convert it to a typed array using Country.FromJson
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var _countries = await response.Content.ReadAsStringAsync();
                countries = JsonConvert.DeserializeObject<Country[]>(_countries);

            }

            return countries;
        }


        /// <summary>
        /// Gets the ordinal value of a number (e.g. 1 to 1st)
        /// </summary>
        /// <returns>The ordinal.</returns>
        /// <param name="num">Number.</param>
        public static string GetOrdinal(int num)
        {
            if (num <= 0) return num.ToString();

            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return num + "th";
            }

            switch (num % 10)
            {
                case 1:
                    return num + "st";
                case 2:
                    return num + "nd";
                case 3:
                    return num + "rd";
                default:
                    return num + "th";
            }

        }
        public static int FindCountryGini(Country[] countries, string countryToFind)
        {
            return countries.OrderByDescending(o => o.Gini).ToList().IndexOf(countries.FirstOrDefault(o => o.Name.ToLower() == countryToFind.ToLower())) + 1;
        }
        public static Country GetLowestGiniCountry(Country[] countries)
        {
            var lowestGiniCountry = countries.Where(o => o.Gini != null).OrderBy(x => x.Gini).Select(z => z).ToList().FirstOrDefault();
            return lowestGiniCountry;
        }
        public static KeyValuePair<string, int> GetRegionWithMostTimezones(Country[] countries)
        {
            var countryDictionary = new Dictionary<string, int>();
            var countriesGrooupByRegion = countries.GroupBy(o => o.Region);
            foreach (var item in countriesGrooupByRegion)
            {
                countryDictionary.Add(item.Key.ToString(), item.Count());
            }
            return countryDictionary.OrderByDescending(o => o.Value).FirstOrDefault();
        }

        public static Dictionary<int, string> GetPopularCurrency(Country[] countries)
        {
            int hightesOccurances = 0;
            string currencyCode = string.Empty;

            var currencyDictionary = new Dictionary<int, string>();

            var myCurrencies = new List<Currency>();
            foreach (var item in countries)
            {
                myCurrencies.AddRange(item.Currencies);
            }
            var groupedMyCurrencies = myCurrencies.GroupBy(x => x.Code);
            foreach (var item in groupedMyCurrencies)
            {
                if (hightesOccurances < item.Count())
                {
                    hightesOccurances = item.Count();
                    currencyCode = item.Key.ToString();
                }
            }
            currencyDictionary.Add(hightesOccurances, currencyCode);
            return currencyDictionary;
        }
        public static string[] GetPopularLanguages(Country[] countries, int count)
        {
            var langList = new List<Language>();
            foreach (var country in countries)
            {
                langList.AddRange(country.Languages);
            }
            var retults = langList.GroupBy(o => o.Name).Select(x => new { x.Key, Count = x.Count() }).OrderByDescending(o => o.Count).Take(3).ToList();
            return retults.Select(o => o.Key).ToArray();
        }
        public static HighestTotalPopulationCountry GetCountryWithHeighestPopulation(Country[] countries)
        {
            var countriesPoulation = (from i in countries select new { Population = i.Population, Alpha3Code = i.Alpha3Code });

            long HighestTotalPop = 0;
            int borderingCountriesTotal = 0;
            string countryWith = string.Empty;

            foreach (var country in countries)
            {
                long totalPop = 0;
                foreach (var border in country.Borders)
                {
                    var borderPop = countriesPoulation.FirstOrDefault(o => o.Alpha3Code == border);
                    if (borderPop != null)
                    {
                        totalPop = totalPop + borderPop.Population;
                    }
                }
                totalPop = totalPop + country.Population;
                if (totalPop > HighestTotalPop)
                {
                    HighestTotalPop = totalPop;
                    borderingCountriesTotal = country.Borders.Count();
                    countryWith = country.Name;
                }
            }
            return new HighestTotalPopulationCountry { HighestTotalPopulation = HighestTotalPop, Name = countryWith, Borders = borderingCountriesTotal };
        }

        public static KeyValuePair<string, double> GethighestOrLowestDensityCountry(Country[] countries, string highOrlow)
        {
            var densityCountryDictionary = new Dictionary<string, double>();

            double highOrLowestCoefficient = highOrlow == "low" ? 999999999999999999 : 0;
            var countryName = string.Empty;
            var filteredCountries = countries.Where(o => o.Area != null && o.Population != 0);// I excluded countries with zero population

            foreach (var country in filteredCountries)
            {
                var coefficient = GetCoefficient(country.Population, country.Area.Value);

                if (highOrlow == "low")
                {
                    if (highOrLowestCoefficient > coefficient)
                    {
                        highOrLowestCoefficient = coefficient;
                        countryName = country.Name;
                    }

                }
                else
                {
                    if (highOrLowestCoefficient < coefficient)
                    {
                        highOrLowestCoefficient = coefficient;
                        countryName = country.Name;
                    }
                }
            }

            densityCountryDictionary.Add(countryName, highOrLowestCoefficient);
            return densityCountryDictionary.FirstOrDefault();
        }
        private static double GetCoefficient(long numerator, double denominator)
        {
            return numerator / denominator;
        }
        public static string GetLargestAreaSubregion(Country[] countries)
        {
            var results = countries.GroupBy(o => o.Subregion).Select(x => new { x.Key, SubregionSum = x.Sum(o => o.Area) }).OrderByDescending(i => i.SubregionSum).FirstOrDefault();
            return results.Key;
        }
    }
}
class HighestTotalPopulationCountry
{
    public long HighestTotalPopulation { get; set; }
    public string Name { get; set; }
    public int Borders { get; set; }
}
