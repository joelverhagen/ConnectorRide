﻿using System;

namespace Knapcode.ConnectorRide.Web
{
    public interface ISettings
    {
        string ScrapeResultPathFormat { get; }
        TimeSpan SchedulesMaximumScrapeFrequency { get; }
        string StorageContainer { get; }
        string StorageConnectionString { get; }
        string GtfsFeedArchivePathFormat { get; }
    }

    public class Settings : ISettings
    {
        private readonly SettingsProvider _provider;

        public Settings(SettingsProvider provider)
        {
            _provider = provider;
        }

        public string ScrapeResultPathFormat => _provider.GetValue("ConnectorRide:ScrapeResult:PathFormat") ?? "schedules/{0}.json";

        public string GtfsFeedArchivePathFormat => _provider.GetValue("ConnectorRide:GtfsFeedArchive:PathFormat") ?? "gtfs/{0}.zip";

        public TimeSpan SchedulesMaximumScrapeFrequency
        {
            get
            {
                var value = _provider.GetValue("ConnectorRide:ScrapeResult:MaximumScrapeFrequency");
                if (value == null)
                {
                    return TimeSpan.FromHours(1);
                }

                return TimeSpan.Parse(value);
            }
        }

        public string StorageContainer => _provider.GetValue("ConnectorRide:Storage:Container") ?? "scrape";

        public string StorageConnectionString => _provider.GetValue("ConnectorRide:Storage:ConnectionString");
    }
}