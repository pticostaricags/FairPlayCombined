﻿using FairPlayCombined.Common;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Models.Common.VisitorTracking;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using FairPlayCombined.DataAccess.Data;
using Microsoft.Extensions.Logging;

namespace FairPlayCombined.Services.Common
{
    public class VisitorTrackingService(
        IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
        IHttpContextAccessor httpContextAccessor,
        IpDataService ipDataService,
        ILogger<VisitorTrackingService> logger) : IVisitorTrackingService
    {
        public async Task<long?> TrackVisitAsync(VisitorTrackingModel visitorTrackingModel, CancellationToken cancellationToken)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            try
            {
                var httpContext = httpContextAccessor.HttpContext;
                string? referer=null;
                if (httpContext != null && httpContext.Request != null &&
                    httpContext.Request.Headers != null &&
                    httpContext.Request.Headers.Referer.Count > 0)
                {
                    referer = httpContext!.Request.Headers.Referer.ToString();
                }
                string? remoteIpAddress = null;
                if (httpContext != null && httpContext.Connection.RemoteIpAddress != null)
                {
                    remoteIpAddress = httpContext!.Connection.RemoteIpAddress!.ToString();
                }
                else
                {
                    remoteIpAddress = "Unknown";
                }
                if (remoteIpAddress == "::1")
                {
                    var ipAddresses = await IpAddressProvider.GetCurrentHostIPv4AddressesAsync();
                    remoteIpAddress = ipAddresses[0];
                }
                var parsedIpAddress = System.Net.IPAddress.Parse(remoteIpAddress);
                var ipGeoLocationInfo = await ipDataService.GetIpGeoLocationInfoAsync(ipAddress: parsedIpAddress, cancellationToken);
                string? country = ipGeoLocationInfo?.country_name;
                var host = httpContext!.Request!.Host.Value;
                var userAgent = httpContext.Request.Headers!.UserAgent.ToString();
                AspNetUsers? userEntity = null;
                if (!String.IsNullOrWhiteSpace(visitorTrackingModel.ApplicationUserId))
                    userEntity = await dbContext.AspNetUsers
                        .SingleOrDefaultAsync(p => p.Id == visitorTrackingModel.ApplicationUserId, cancellationToken);
                var visitedPage = new VisitorTracking()
                {
                    ApplicationUserId = userEntity?.Id,
                    Country = country,
                    Host = host,
                    RemoteIpAddress = remoteIpAddress,
                    UserAgent = userAgent,
                    VisitDateTime = DateTimeOffset.UtcNow,
                    VisitedUrl = visitorTrackingModel.VisitedUrl,
                    SessionId = visitorTrackingModel.SessionId,
                    Referer = referer
                };
                await dbContext.VisitorTracking.AddAsync(visitedPage, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
                var pageUri = new Uri(visitedPage.VisitedUrl!);
                var lastSegment = pageUri.Segments[^1].TrimEnd('/');
                if (!String.IsNullOrWhiteSpace(lastSegment))
                {
                    var videoInfoEntity = await dbContext.VideoInfo
                        .SingleOrDefaultAsync(p => p.VideoId == lastSegment, cancellationToken);
                    if (videoInfoEntity != null)
                    {
                        visitedPage.VideoInfoId = videoInfoEntity.VideoInfoId;
                        await dbContext.SaveChangesAsync(cancellationToken);
                    }
                }
                return visitedPage.VisitorTrackingId;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error: {ErrorMessage}", ex.Message);
                try
                {
                    await dbContext.ErrorLog.AddAsync(new ErrorLog()
                    {
                        FullException = ex.ToString(),
                        Message = ex.Message,
                        StackTrace = ex.StackTrace
                    }, cancellationToken);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
                catch (Exception ex2)
                {
                    logger.LogError(ex2, "Error: {ErrorMessage}", ex2.Message);
                }
            }

            return null;
        }

        public async Task UpdateVisitTimeElapsedAsync(long visitorTrackingId, CancellationToken cancellationToken)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var entity = await dbContext.VisitorTracking
               .SingleOrDefaultAsync(p => p.VisitorTrackingId == visitorTrackingId, cancellationToken);
            if (entity != null)
            {
                entity.LastTrackedDateTime = DateTimeOffset.UtcNow;
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
