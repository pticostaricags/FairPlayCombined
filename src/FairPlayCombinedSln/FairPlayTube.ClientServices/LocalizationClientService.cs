using FairPlayCombined.Models.Common.Localization;
using FairPlayCombined.Models.Common.Resource;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayTube.ClientServices
{
    public class LocalizationClientService
    {
        private ResourceModel[]? AllResources { get; set; }

        public async Task LoadDataAsync()
        {
            await Task.Yield();
            throw new NotImplementedException();
        }

        public async Task<CultureModel[]> GetSupportedCulturesAsync()
        {
            await Task.Yield();
            throw new NotImplementedException();
        }

        public IEnumerable<LocalizedString> GetAllStrings()
        {
            return this.AllResources!.Select(p => new LocalizedString(p.Key!, p.Value!));
        }
        public string GetString(string typeName, string key)
        {
            return this.AllResources!.SingleOrDefault(p => p.Type == typeName &&
            p.Key == key)?.Value!;
        }
    }
}
