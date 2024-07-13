using FairPlayCombined.Models.Common.VisitorTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Interfaces.Common
{
    public interface IVisitorTrackingService
    {
        Task<long?> TrackVisitAsync(VisitorTrackingModel visitorTrackingModel, CancellationToken cancellationToken);
        Task UpdateVisitTimeElapsedAsync(long visitorTrackingId, CancellationToken cancellationToken);
    }
}
