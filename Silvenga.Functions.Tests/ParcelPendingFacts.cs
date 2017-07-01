using System.IO;

using FluentAssertions;

using Xunit;

namespace Silvenga.Functions.Tests
{
    public class ParcelPendingFacts
    {
        private string _text;

        public ParcelPendingFacts()
        {
            const string samplePath = @"Assets\sample-pp.html";
            var text = File.ReadAllText(samplePath);

            _text = text;
        }

        [Fact]
        public void Can_parse_AccessCode()
        {
            // Act
            var result = ParcelPending.Parse(_text);

            // Assert
            result.AccessCode.Should().Be("123 - 456");
        }

        [Fact]
        public void Can_parse_LockerLocation()
        {
            // Act
            var result = ParcelPending.Parse(_text);

            // Assert
            result.LockerLocation.Should().Be("KIOSK B");
        }

        [Fact]
        public void Can_parse_LockerNumber()
        {
            // Act
            var result = ParcelPending.Parse(_text);

            // Assert
            result.LockerNumber.Should().Be("1");
        }

        [Fact]
        public void Can_parse_PackageBarcode()
        {
            // Act
            var result = ParcelPending.Parse(_text);

            // Assert
            result.PackageBarcode.Should().Be("N/A");
        }

        [Fact]
        public void Can_parse_PackageCarrier()
        {
            // Act
            var result = ParcelPending.Parse(_text);

            // Assert
            result.PackageCarrier.Should().Be("Amazon");
        }

        [Fact]
        public void Can_parse_PickupDate()
        {
            // Act
            var result = ParcelPending.Parse(_text);

            // Assert
            result.PickupDate.Should().Be("2017-05-25");
        }

        [Fact]
        public void Can_parse_RecipientLocation()
        {
            // Act
            var result = ParcelPending.Parse(_text);

            // Assert
            result.RecipientLocation.Should().Be("A & A, 200");
        }

        [Fact]
        public void Can_parse_RecipientName()
        {
            // Act
            var result = ParcelPending.Parse(_text);

            // Assert
            result.RecipientName.Should().Be("Mark");
        }

        [Fact]
        public void Can_parse_when_no_barcode_exists()
        {
            const string samplePath = @"Assets\sample-pp-no-barcode.html";
            var text = File.ReadAllText(samplePath);

            // Act
            var result = ParcelPending.Parse(text);

            // Assert
            result.PackageBarcode.Should().Be("");
        }

        [Fact]
        public void Can_parse_when_no_pickup_date_exists()
        {
            const string samplePath = @"Assets\sample-pp-no-barcode.html";
            var text = File.ReadAllText(samplePath);

            // Act
            var result = ParcelPending.Parse(text);

            // Assert
            result.PickupDate.Should().Be("");
        }

        [Fact]
        public void Can_parse_when_access_code_has_colon()
        {
            const string samplePath = @"Assets\sample-pp-no-pickup-date.html";
            var text = File.ReadAllText(samplePath);

            // Act
            var result = ParcelPending.Parse(text);

            // Assert
            result.PickupDate.Should().Be("");
        }
    }
}