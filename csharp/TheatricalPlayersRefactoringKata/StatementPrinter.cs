using System;
using System.Collections.Generic;
using System.Globalization;

namespace TheatricalPlayersRefactoringKata
{
    public class StatementPrinter
    {


        public string PrintAsHtml(Invoice invoice, Dictionary<string, Play> plays)
        {
            var totalAmount = 0;
            var volumeCredits = 0;
            var result = "<html>\n";
            result += string.Format("<h1>Statement for {0}</h1>\n", invoice.Customer);
            CultureInfo cultureInfo = new CultureInfo("en-US");

            CalculatePerformanceCost(invoice, plays, ref totalAmount, ref volumeCredits, ref result, cultureInfo);
            foreach (var perf in invoice.Performances)
            {
                result += string.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", plays[perf.PlayID].Name, Convert.ToDecimal(perf.Amount / 100), perf.Audience);
            }
            result += string.Format(cultureInfo, "Amount owed is {0:C}\n", Convert.ToDecimal(totalAmount / 100));
            result += string.Format("You earned {0} credits\n", volumeCredits);
            return result;
        }


        public string Print(Invoice invoice, Dictionary<string, Play> plays)
        {
            var totalAmount = 0;
            var volumeCredits = 0;
            var result = string.Format("Statement for {0}\n", invoice.Customer);
            CultureInfo cultureInfo = new CultureInfo("en-US");

            CalculatePerformanceCost(invoice, plays, ref totalAmount, ref volumeCredits, ref result, cultureInfo);
            foreach (var perf in invoice.Performances)
            {
                result += string.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", plays[perf.PlayID].Name, Convert.ToDecimal(perf.Amount / 100), perf.Audience);
            }
            result += string.Format(cultureInfo, "Amount owed is {0:C}\n", Convert.ToDecimal(totalAmount / 100));
            result += string.Format("You earned {0} credits\n", volumeCredits);
            return result;
        }

        private static void CalculatePerformanceCost(Invoice invoice, Dictionary<string, Play> plays, ref int totalAmount, ref int volumeCredits, ref string result, CultureInfo cultureInfo)
        {
            foreach (var perf in invoice.Performances)
            {
                var play = plays[perf.PlayID];
                switch (play.Type)
                {
                    case "tragedy":
                        perf.Amount = 40000;
                        if (perf.Audience > 30)
                        {
                            perf.Amount += 1000 * (perf.Audience - 30);
                        }
                        break;
                    case "comedy":
                        perf.Amount = 30000;
                        if (perf.Audience > 20)
                        {
                            perf.Amount += 10000 + 500 * (perf.Audience - 20);
                        }
                        perf.Amount += 300 * perf.Audience;
                        break;
                    default:
                        throw new Exception("unknown type: " + play.Type);
                }
                // add volume credits
                volumeCredits += Math.Max(perf.Audience - 30, 0);
                // add extra credit for every ten comedy attendees
                if ("comedy" == play.Type) volumeCredits += (int)Math.Floor((decimal)perf.Audience / 5);

                // print line for this order
                totalAmount += perf.Amount;
            }
        }

    }
}
