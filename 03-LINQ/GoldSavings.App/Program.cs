using GoldSavings.App.Model;
using GoldSavings.App.Client;
using GoldSavings.App.Services;
namespace GoldSavings.App;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, Gold Saver!");

        GoldClient goldClient = new GoldClient();

        GoldPrice currentPrice = goldClient.GetCurrentGoldPrice().GetAwaiter().GetResult();
        Console.WriteLine($"The price for today is {currentPrice.Price}");

        List<GoldPrice> thisMonthPrices = goldClient.GetGoldPrices(new DateTime(2024, 03, 01), new DateTime(2024, 03, 11)).GetAwaiter().GetResult();
        foreach(var goldPrice in thisMonthPrices)
        {
            Console.WriteLine($"The price for {goldPrice.Date} is {goldPrice.Price}");
        }


//------------------- a. (method and query syntax) What are the TOP 3 highest and TOP 3 lowest prices of gold within the last year?---------------------------------
        DateTime oneYearAgo = new DateTime(2024, 01, 01);
        DateTime endoneYearAgo = new DateTime(2024, 12, 31);

        List<GoldPrice> lastYearPrices = goldClient.GetGoldPrices(oneYearAgo, endoneYearAgo).GetAwaiter().GetResult();

        var top3HighestPrices = lastYearPrices
            .OrderByDescending(p => p.Price)
            .Take(3);

        var top3LowestPrices = lastYearPrices
            .OrderBy(p => p.Price)
            .Take(3);
        Console.WriteLine("------------------------- Question 2 -----------------------");
        // -------------------------------------------------------------------- Question .a --------------------------------------------------------------------
        Console.WriteLine("----------------- Question .a -----------------");
        GoldResultPrinter.PrintPrices(top3HighestPrices.ToList(), "Top 3 Highest Prices in the Last Year");
        GoldResultPrinter.PrintPrices(top3LowestPrices.ToList(), "Top 3 Lowest Prices in the Last Year");


           // b.If one had bought gold in January 2020, is it possible that they would have earned more than 5%? On which days?

           // -------------------------------------------------------------------- Question .b --------------------------------------------------------------------
            DateTime janStart2020 = new DateTime(2020, 01, 01);
            DateTime janEnd2020 = new DateTime(2020, 01, 31);
            List<GoldPrice> january2020Prices = goldClient.GetGoldPrices(janStart2020, janEnd2020).GetAwaiter().GetResult();

            DateTime currentStart = new DateTime(2020, 02, 01);
            DateTime maxEnd = new DateTime(2024, 12, 31);
            List<GoldPrice> laterPrices = new List<GoldPrice>();

            while (currentStart <= maxEnd)
            {
                DateTime currentEnd = currentStart.AddDays(366);
                if (currentEnd > maxEnd) currentEnd = maxEnd;

                var price = goldClient.GetGoldPrices(currentStart, currentEnd).GetAwaiter().GetResult();
                laterPrices.AddRange(price);

                currentStart = currentEnd.AddDays(1); 
            }

            var profitableDays = new List<(DateTime, DateTime, double, double)>(); 

            foreach (var buyDay in january2020Prices)
            {
                var profitableSellDay = laterPrices
                    .Where(p => p.Price >= buyDay.Price * 1.05)
                    .OrderBy(p => p.Date) 
                    .FirstOrDefault();

                if (profitableSellDay != null)
                {
                    profitableDays.Add((buyDay.Date, profitableSellDay.Date, buyDay.Price, profitableSellDay.Price));
                }
            }

            Console.WriteLine("----------------- Question .b -----------------");
            if (profitableDays.Any())
            {
                Console.WriteLine("Les jours où un achat en janvier 2020 aurait généré plus de 5% de profit :");
                foreach (var day in profitableDays)
                {
                    Console.WriteLine($"Achat le {day.Item1.ToShortDateString()} à {day.Item3} -> Vente le {day.Item2.ToShortDateString()} à {day.Item4}");
                }
            }
            else
            {
                Console.WriteLine("Aucun jour en janvier 2020 n'aurait permis un gain de plus de 5%.");
            }


//------------------------------------------------------------------- Question .c --------------------------------------------------------------------------------------


        DateTime Y2019S = new DateTime(2019, 01, 01);
        DateTime Y2019E = new DateTime(2019, 12, 31);
        
        DateTime Y2020S = new DateTime(2020, 01, 01);
        DateTime Y2020E = new DateTime(2020, 12, 31);

        DateTime Y2021S = new DateTime(2021, 01, 01);
        DateTime Y2021E = new DateTime(2021, 12, 31);

        DateTime Y2022S = new DateTime(2022, 01, 01);
        DateTime Y2022E = new DateTime(2022, 12, 31);




        List<GoldPrice> TOP13_19 = goldClient.GetGoldPrices(Y2019S, Y2019E).GetAwaiter().GetResult();

        var top13_19 = TOP13_19
            .OrderByDescending(p => p.Price)
            .Take(13);


        List<GoldPrice> TOP13_20 = goldClient.GetGoldPrices(Y2019S, Y2019E).GetAwaiter().GetResult();

        var top13_20 = TOP13_20
            .OrderByDescending(p => p.Price)
            .Take(13);

        List<GoldPrice> TOP13_21 = goldClient.GetGoldPrices(Y2021S, Y2021E).GetAwaiter().GetResult();

        var top13_21 = TOP13_21
            .OrderByDescending(p => p.Price)
            .Take(13);

        List<GoldPrice> TOP13_22 = goldClient.GetGoldPrices(Y2022S, Y2022E).GetAwaiter().GetResult();

        var top13_22 = TOP13_22
            .OrderByDescending(p => p.Price)
            .Take(13);

        var combinedTop = top13_19.Concat(top13_20).Concat(top13_21).Concat(top13_22)
            .OrderByDescending(p => p.Price)
            .Skip(10)
            .Take(3)
            .ToList();

        // Afficher les 13 meilleurs prix après avoir fusionné et trié
        // GoldResultPrinter.PrintPrices("Question C");
        Console.WriteLine("----------------- Question .c -----------------");

        GoldResultPrinter.PrintPrices(combinedTop, "Top 11,12 & 13 Combined");


//------------------------------------------------------------------------- Question .d -----------------------------------------------------------------------------



        DateTime start2020 = new DateTime(2020, 01, 01);
        DateTime end2020 = new DateTime(2020, 12, 31);

        DateTime start2023 = new DateTime(2023, 01, 01);
        DateTime end2023 = new DateTime(2023, 12, 31);

        DateTime start2024 = new DateTime(2024, 01, 01);
        DateTime end2024 = new DateTime(2024, 12, 31);

        List<GoldPrice> prices2020 = goldClient.GetGoldPrices(start2020, end2020).GetAwaiter().GetResult() ?? new List<GoldPrice>();
        List<GoldPrice> prices2023 = goldClient.GetGoldPrices(start2023, end2023).GetAwaiter().GetResult() ?? new List<GoldPrice>();
        List<GoldPrice> prices2024 = goldClient.GetGoldPrices(start2024, end2024).GetAwaiter().GetResult() ?? new List<GoldPrice>();

        double average2020 = prices2020.Any() ? prices2020.Average(p => p.Price) : 0;
        double average2023 = prices2023.Any() ? prices2023.Average(p => p.Price) : 0;
        double average2024 = prices2024.Any() ? prices2024.Average(p => p.Price) : 0;

        Console.WriteLine("----------------- Question .d -----------------");
        GoldResultPrinter.PrintSingleValue(average2020, "Average Gold Price in 2020");
        GoldResultPrinter.PrintSingleValue(average2023, "Average Gold Price in 2023");
        GoldResultPrinter.PrintSingleValue(average2024, "Average Gold Price in 2024");

//-------------------------------------------------------------------- Question .e -------------------------------------------------------------------------------------


        DateTime A2020S = new DateTime(2020, 01, 01);
        DateTime A2020E = new DateTime(2020, 12, 31);

        DateTime A2021S = new DateTime(2021, 01, 01);
        DateTime A2021E = new DateTime(2021, 12, 31);

        DateTime A2022S = new DateTime(2022, 01, 01);
        DateTime A2022E = new DateTime(2022, 12, 31);

        DateTime A2023S = new DateTime(2023, 01, 01);
        DateTime A2023E = new DateTime(2023, 12, 31);

        DateTime A2024S = new DateTime(2024, 01, 01);
        DateTime A2024E = new DateTime(2024, 12, 31);


        List<GoldPrice> TOP_20 = goldClient.GetGoldPrices(A2020S, A2020E).GetAwaiter().GetResult();

        var MAX_20 = TOP13_20
            .OrderByDescending(p => p.Price)
            .Take(1);

        var MIN_20 = TOP13_20
            .OrderBy(p => p.Price)
            .Take(1);


        List<GoldPrice> TOP_21 = goldClient.GetGoldPrices(A2021S, A2021E).GetAwaiter().GetResult();

        var MAX_21 = TOP13_21
            .OrderByDescending(p => p.Price)
            .Take(1);

        var MIN_21 = TOP13_21
            .OrderBy(p => p.Price)
            .Take(1);


        List<GoldPrice> TOP_22 = goldClient.GetGoldPrices(A2022S, A2022E).GetAwaiter().GetResult();

        var MAX_22 = TOP_22
            .OrderByDescending(p => p.Price)
            .Take(1);

        var MIN_22 = TOP_22
            .OrderBy(p => p.Price)
            .Take(1);

        List<GoldPrice> TOP_23 = goldClient.GetGoldPrices(A2023S, A2023E).GetAwaiter().GetResult();

        var MAX_23 = TOP_23
        .OrderByDescending(p => p.Price)
        .Take(1);

        var MIN_23 = TOP_23
        .OrderBy(p => p.Price)
        .Take(1);

        List<GoldPrice> TOP_24 = goldClient.GetGoldPrices(A2024S, A2024E).GetAwaiter().GetResult();

        var MAX_24 = TOP_24
        .OrderByDescending(p => p.Price)
        .Take(1);

        var MIN_24 = TOP_24
        .OrderBy(p => p.Price)
        .Take(1);


        var BestSell = MAX_20.Concat(MAX_21).Concat(MAX_22).Concat(MAX_23).Concat(MAX_24)
            .OrderByDescending(p => p.Price)
            .Take(1)
            .ToList();

        var BestBought = MIN_20.Concat(MIN_21).Concat(MIN_22).Concat(MIN_23).Concat(MIN_24)
            .OrderBy(p => p.Price)
            .Take(1)
            .ToList();

        
        Console.WriteLine("----------------- Question .e -----------------");
        GoldResultPrinter.PrintPrices(BestBought , "the best moment to bought gold");
        GoldResultPrinter.PrintPrices(BestSell, "The best moment to sell gold");


        Console.WriteLine("-----------------------------------------------------------------");

    
    //-------------------------------------------------------------------- Question 3 -------------------------------------------------------------------------------------


        List<GoldPrice> goldPrices = new List<GoldPrice>
        {
            new GoldPrice { Date = new DateTime(2024, 3, 1), Price = 1890.45 },
            new GoldPrice { Date = new DateTime(2024, 3, 2), Price = 1905.30 }
        };

        Console.WriteLine("------------------------ Question 3 ------------------------------");
        GoldPriceSerializer.SaveToXml(goldPrices, "gold_prices.xml");


    //-------------------------------------------------------------------- Question 4 -------------------------------------------------------------------------------------


        Console.WriteLine("------------------------ Question 4 ------------------------------");
        var prices = GoldPriceSerializer.LoadFromXml("gold_prices.xml");
        Console.WriteLine($"Loaded {prices.Count} prices.");





    }
}