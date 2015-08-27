namespace Aggregator.Objects.Configuration
{
    public class MarkupHotel
    {
        public int Id { get; set; }
        public int SuperId { get; set; }
        public decimal Markup { get; set; }
        public bool IsPercentageMarkup { get; set; }
    }
}