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
    }
}
