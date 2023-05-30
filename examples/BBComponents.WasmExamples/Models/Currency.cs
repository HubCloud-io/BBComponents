namespace BBComponents.WasmExamples.Models
{
    public class Currency
    {
        public int Id { get; set; }
        public string CodeNumeric { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }

        public static IEnumerable<Currency> SampleData()
        {
            var collection = new List<Currency>();

            collection.Add(new Currency { Id = 1, CodeNumeric = "978", Code = "EUR", Title = "Euro" });
            collection.Add(new Currency { Id = 2, CodeNumeric = "826", Code = "GBP", Title = "Pound Sterling" });
            collection.Add(new Currency { Id = 3, CodeNumeric = "840", Code = "USD", Title = "US Dollar" });

            return collection;

        }
    }
}
