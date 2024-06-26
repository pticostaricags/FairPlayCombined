﻿using BrowserInterop.Extensions;
using FairPlayCombined.Common.CustomExceptions;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Models.Common.GeoLocation;
using Microsoft.JSInterop;

namespace FairPlayDating.Services
{
    public class BlazorGeoLocationService(IJSRuntime jsRuntime) : IGeoLocationService
    {
        public async Task<GeoCoordinates> GetCurrentPositionAsync()
        {
            var window = await jsRuntime.Window();
            var navigator = await window.Navigator();
            var currentPosition = await navigator.Geolocation.GetCurrentPosition();
            if (currentPosition.Error != null)
            {
                string message = "Unable to retrieve location, please make sure to give location access to the app";
                throw new RuleException(message);
            }
            return new GeoCoordinates()
            {
                Latitude = currentPosition.Location.Coords.Latitude,
                Longitude = currentPosition.Location.Coords.Longitude
            };
        }
    }
}
