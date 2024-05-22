using PhoneNumberDetectionWeb.Models;
using PhoneNumberDetectionWeb.Services;
using System.Collections.Generic;
using Xunit;

namespace PhoneNumberDetectionWeb.Tests
{
    public class PhoneNumberDetectorTests
    {
        [Theory]
        [InlineData("123-456-7890", "Simple 10-digit")]
        [InlineData("+1-123-456-7890", "International or Local")]
        [InlineData("(123) 456-7890", "With Parentheses for Area Codes")]
        [InlineData("123 456 7890", "Simple 10-digit")]
        [InlineData("1234567890", "Simple 10-digit")]
        [InlineData("ONE TWO THREE FOUR FIVE SIX SEVEN EIGHT NINE ZERO", "Words and Hindi Digits")]
        [InlineData("?? ?? ??? ??? ???? ?? ??? ?? ?? ?????", "Words and Hindi Digits")]
        public void TestValidPhoneNumbers(string input, string expectedFormat)
        {
            var detectedNumbers = PhoneNumberDetector.DetectPhoneNumbers(input);

            Assert.Single(detectedNumbers);
            Assert.Equal(input.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", ""), detectedNumbers[0].Number.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", ""));
            Assert.Equal(expectedFormat, detectedNumbers[0].Format);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("This is a text without phone numbers.")]
        [InlineData("!!!@@@###$$$%%%")]
        public void TestInvalidPhoneNumbers(string input)
        {
            var detectedNumbers = PhoneNumberDetector.DetectPhoneNumbers(input);

            Assert.Empty(detectedNumbers);
        }

        [Fact]
        public void TestMultiplePhoneNumbers()
        {
            string input = "123-456-7890 and +1-123-456-7890";
            var detectedNumbers = PhoneNumberDetector.DetectPhoneNumbers(input);

            Assert.Equal(2, detectedNumbers.Count);
        }

        [Fact]
        public void TestNormalizedPhoneNumbers()
        {
            string input = "123   456-7890";
            var detectedNumbers = PhoneNumberDetector.DetectPhoneNumbers(input);

            Assert.Single(detectedNumbers);
            Assert.Equal("123 456 7890", detectedNumbers[0].Number);
        }
    }
}
