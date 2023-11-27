using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Common.CustomExceptions
{
    public class AzureVideoIndexerException(string? message) : Exception(message)
    {
    }
}
