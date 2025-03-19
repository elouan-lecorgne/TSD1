using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace GoldSavings.App.Model
{
    public class GoldPrice
    {
        [JsonProperty("Data")]
        public DateTime Date { get; set; }

        [JsonProperty("Cena")]
        public double Price { get; set; }
    }

    public class GoldPriceSerializer
    {
        public static void SaveToXml(List<GoldPrice> prices, string filePath)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<GoldPrice>));

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    serializer.Serialize(writer, prices);
                }

                Console.WriteLine($"Gold prices saved to {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to XML: {ex.Message}");
            }
        }

        public static List<GoldPrice> LoadFromXml(string filePath) => (List<GoldPrice>)new XmlSerializer(typeof(List<GoldPrice>)).Deserialize(new StreamReader(filePath));

    }
}
