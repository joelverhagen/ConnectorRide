﻿using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Knapcode.ConnectorRide.Core.Tests.TestSupport;
using Xunit;

namespace Knapcode.ConnectorRide.Core.Tests
{
    public class GtfsConverterTests
    {
        [Fact]
        public async Task GtfsFeedSerializer_HasExpectedFileNamesWithGroupingAmPm()
        {
            await VerifyExpectedFileNames(true);
        }

        [Fact]
        public async Task GtfsFeedSerializer_HasExpectedFileNamesWithoutGroupingAmPm()
        {
            await VerifyExpectedFileNames(false);
        }

        private static async Task VerifyExpectedFileNames(bool groupAmPm)
        {
            // Arrange
            var feed = await TestData.GetGtfsFeedAsync(groupAmPm);
            var csvSerializer = new GtfsCsvSerializer();
            var target = new GtfsFeedSerializer(csvSerializer);

            using (var stream = new MemoryStream())
            {
                // Act
                await target.SerializeAsync(stream, feed);

                // Assert
                stream.Seek(0, SeekOrigin.Begin);
                using (var zipArchive = new ZipArchive(stream, ZipArchiveMode.Read))
                {
                    var fullNames = zipArchive.Entries.Select(e => e.FullName).OrderBy(e => e).ToList();
                    Assert.Equal(
                        new[] { "agency.txt", "calendar.txt", "routes.txt", "shapes.txt", "stop_times.txt", "stops.txt", "trips.txt" },
                        fullNames);
                }
            }
        }
    }
}
