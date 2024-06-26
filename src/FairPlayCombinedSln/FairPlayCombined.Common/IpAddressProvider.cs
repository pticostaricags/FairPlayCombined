﻿using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace FairPlayCombined.Common
{
    public static class IpAddressProvider
    {

        public static async Task<List<string>> GetCurrentHostIPv4AddressesAsync(bool getPublicIpAddress = true)
        {
            if (getPublicIpAddress)
            {
                var publicIpAddress = await GetPublicIpAsync();
                return [publicIpAddress.ToString()];
            }
            //Check https://stackoverflow.com/questions/50386546/net-core-2-x-how-to-get-the-current-active-local-network-ipv4-address
            // order interfaces by speed and filter out down and loopback
            // take first of the remaining
            var allUpInterfaces = NetworkInterface.GetAllNetworkInterfaces()
                .Where(c => c.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                c.OperationalStatus == OperationalStatus.Up)
                .OrderByDescending(c => c.Speed)
                .ToList();
            List<string> lstIps = [];
            if (allUpInterfaces.Count > 0)
            {
                foreach (var singleUpInterface in allUpInterfaces)
                {
                    var props = singleUpInterface.GetIPProperties();
                    // get first IPV4 address assigned to this interface
                    var allIpV4Address = props.UnicastAddresses
                        .Where(c => c.Address.AddressFamily == AddressFamily.InterNetwork)
                        .Select(c => c.Address)
                        .ToList();
                    allIpV4Address.ForEach((IpV4Address) =>
                    {
                        lstIps.Add(IpV4Address.ToString());
                    });
                }
            }

            return lstIps;
        }

        public static async Task<IPAddress> GetPublicIpAsync(string? serviceUrl = null)
        {
            if (String.IsNullOrWhiteSpace(serviceUrl))
                serviceUrl = Properties.Resources.IpInfoUrl;
            HttpClient httpClient = new();
            var ipAddress = await httpClient.GetStringAsync(serviceUrl);
            return IPAddress.Parse(ipAddress.Trim());
        }
    }
}
