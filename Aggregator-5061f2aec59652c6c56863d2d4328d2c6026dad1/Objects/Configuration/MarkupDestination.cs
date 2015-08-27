using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Aggregator.Objects.Configuration
{
    public class MarkupDestination
    {
        public int Id { get; set; }
        public string Iata { get; set; }
        public decimal MinPriceTakesEffect { get; set; }
        public decimal Markup { get; set; }
        public bool IsPercentageMarkup;
    }
}