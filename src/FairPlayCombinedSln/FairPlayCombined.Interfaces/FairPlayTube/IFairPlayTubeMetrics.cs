using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Interfaces.FairPlayTube
{
    public interface IFairPlayTubeMetrics
    {
        public const string SESSION_METER_NAME = $"{nameof(FairPlayTube)}.Videos";
        void Initialize();
    }
}
