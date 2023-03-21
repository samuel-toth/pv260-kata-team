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
            result += string.Format("  <h1>Statement for {0}</h1>\n", invoice.Customer);
            result += "  <table>\n";
            result += "    <tr><th>play</th><th>seats</th><th>cost</th></tr>\n";
            CultureInfo cultureInfo = new CultureInfo("en-US");

            CalculatePerformanceCost(invoice, plays, ref totalAmount, ref volumeCredits);
            foreach (var perf in invoice.Performances)
            {
                result += string.Format(cultureInfo, "    <tr><td>{0}</td><td>{2}</td><td>{1:C}</td></tr>\n", plays[perf.PlayID].Name,
                    Convert.ToDecimal(perf.Amount / 100), perf.Audience);
            }
            result += "  </table>\n";
            result += string.Format(cultureInfo, "  <p>Amount owed is <em>{0:C}</em></p>\n", Convert.ToDecimal(totalAmount / 100));
            result += string.Format("  <p>You earned <em>{0}</em> credits</p>\n", volumeCredits);
            result += "</html>";
            
            return result;
        }


        public string Print(Invoice invoice, Dictionary<string, Play> plays)
        {
            var totalAmount = 0;
            var volumeCredits = 0;
            var result = string.Format("Statement for {0}\n", invoice.Customer);
            CultureInfo cultureInfo = new CultureInfo("en-US");

            CalculatePerformanceCost(invoice, plays, ref totalAmount, ref volumeCredits);
            foreach (var perf in invoice.Performances)
            {
                result += string.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", plays[perf.PlayID].Name,
                    Convert.ToDecimal(perf.Amount / 100), perf.Audience);
            }

            result += string.Format(cultureInfo, "Amount owed is {0:C}\n", Convert.ToDecimal(totalAmount / 100));
            result += string.Format("You earned {0} credits\n", volumeCredits);
            return result;
        }

        private static void CalculatePerformanceCost(Invoice invoice, Dictionary<string, Play> plays,
            ref int totalAmount, ref int volumeCredits)
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