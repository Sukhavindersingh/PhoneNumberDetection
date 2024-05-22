using System.Collections.Generic;
using System.Text.RegularExpressions;
using PhoneNumberDetectionWeb.Models;
using System.Threading.Tasks;
using System.Linq;

namespace PhoneNumberDetectionWeb.Services
{
    public class PhoneNumberDetector
    {
        // Compile regular expressions once for performance
        private static readonly List<(Regex Pattern, string Format)> patterns = new List<(Regex, string)>
        {
            (new Regex(@"\+?\d{1,3}[- ]?\(?\d{1,4}\)?[- ]?\d{1,4}[- ]?\d{1,4}[- ]?\d{1,9}", RegexOptions.Compiled), "International or Local"),
            (new Regex(@"\(?\d{1,4}\)?[- ]?\d{1,4}[- ]?\d{1,4}[- ]?\d{1,9}", RegexOptions.Compiled), "With Parentheses for Area Codes"),
            (new Regex(@"\d{10}", RegexOptions.Compiled), "Simple 10-digit"),
            (new Regex(@"\b(?:ONE|TWO|THREE|FOUR|FIVE|SIX|SEVEN|EIGHT|NINE|ZERO|एक|दो|तीन|चार|पांच|छह|सात|आठ|नौ|शून्य)\b[- ]?", RegexOptions.Compiled), "Words and Hindi Digits"),
            (new Regex(@"(?:\+?\d{1,3})?[- ]?\d{1,4}[- ]?\d{1,4}[- ]?\d{1,9}", RegexOptions.Compiled), "General with Optional Country Code")
        };

        public static List<PhoneNumberInfo> DetectPhoneNumbers(string input)
        {
            var phoneNumbers = new List<PhoneNumberInfo>();

            // Use parallel processing for large inputs
            Parallel.ForEach(patterns, (pattern) =>
            {
                var matches = pattern.Pattern.Matches(input);
                foreach (Match match in matches)
                {
                    string number = NormalizePhoneNumber(match.Value);
                    lock (phoneNumbers)
                    {
                        phoneNumbers.Add(new PhoneNumberInfo { Number = number, Format = pattern.Format });
                    }
                }
            });

            // Remove duplicates and return
            return phoneNumbers.Distinct().ToList();
        }

        private static string NormalizePhoneNumber(string number)
        {
            // Normalize the phone number (e.g., trim spaces, remove redundant characters)
            return Regex.Replace(number, @"\s+", " ").Trim();
        }


    }
}


