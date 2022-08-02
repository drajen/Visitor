using System.Net;
using System.Linq;
using System.Net.Sockets;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace Visitor.Services {
    public class IpTools : IIpTools {
        IHttpContextAccessor _httpContextAccessor;
        IHostEnvironment _hostEnvironment;

        public IpTools(IHttpContextAccessor httpContextAccessor, IHostEnvironment hostEnvironment) {
            _httpContextAccessor = httpContextAccessor;
            _hostEnvironment = hostEnvironment;
        }
        public string GetIP() {
            string ip = _httpContextAccessor.HttpContext.Connection?.RemoteIpAddress.ToString();
            if (ip == "::1") { // IPV6 loopback
                ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork).ToString();

                //ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[2].ToString();
            }
            if (_hostEnvironment.IsDevelopment())
            {
                ip = "192.02.85.100";
            }
            return ip;
        }

        public int? GetBranchId() {
            var ip = _httpContextAccessor.HttpContext.Connection?.RemoteIpAddress;
            if (_hostEnvironment.IsDevelopment())
                ip = IPAddress.Parse("192.2.85.100");
            
            if (ip == null)
                return null;

            byte[] bytes = ip.GetAddressBytes();
            if (bytes.Length != 4)
                return null;

            return (bytes[1] * 100) + bytes[2];
        }
    }
}
