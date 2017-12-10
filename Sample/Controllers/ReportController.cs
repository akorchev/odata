using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace CodewareDb.Controllers
{
    public partial class ReportController : Controller
    {
        [HttpGet("/report")]
        public async Task Get(string url)
        {
            using (var httpClient = new HttpClient())
            {
                var proxyRequest = new HttpRequestMessage(new HttpMethod(Request.Method), url);
                foreach (var header in Request.Headers)
                {
                    proxyRequest.Headers.Add(header.Key, new string[] { header.Value });
                }

                var responseMessage = await httpClient.SendAsync(proxyRequest);

                Response.StatusCode = (int)responseMessage.StatusCode;
                foreach (var header in responseMessage.Headers)
                {
                    Response.Headers[header.Key] = header.Value.ToArray();
                }

                foreach (var header in responseMessage.Content.Headers)
                {
                    Response.Headers[header.Key] = header.Value.ToArray();
                }

                // SendAsync removes chunking from the response. This removes the header so it doesn't expect a chunked response.
                Response.Headers.Remove("transfer-encoding");
                Response.Headers.Remove("Content-Length");

                var result = await responseMessage.Content.ReadAsStringAsync();

                await Response.WriteAsync(result .Replace("/ReportServer/", "http://localhost:5000/proxy/ReportServer/")
                .Replace("./ReportViewer.aspx", "http://localhost:5000/proxy/ReportServer/Pages/ReportViewer.aspx"));
            }
        }


        [Route("/proxy/{*url}")]
        public async Task Proxy()
        {
            var originalUrl = Request.GetDisplayUrl().Replace("http://localhost:5000/proxy/", "http://localhost/");

            using (var httpClient = new HttpClient())
            {
                var proxyRequest = new HttpRequestMessage(new HttpMethod(Request.Method), originalUrl);

                foreach (var header in Request.Headers)
                {
                    if (Request.Method == "POST" && (header.Key == "Content-Type" || header.Key == "Content-Length"))
                    {
                        // Do not set Content-Type and Content-Length for POST
                    }
                    else
                    {
                        proxyRequest.Headers.Add(header.Key, new string[] { header.Value });
                    }
                }

                if (Request.Method == "POST")
                {
                    using (var stream = new MemoryStream())
                    {
                        HttpContext.Request.Body.CopyTo(stream);
                        stream.Position = 0;

                        string body = new StreamReader(stream).ReadToEnd();
                        proxyRequest.Content = new StringContent(body);
                    }
                }

                var responseMessage = await httpClient.SendAsync(proxyRequest);

                Response.StatusCode = (int)responseMessage.StatusCode;
                foreach (var header in responseMessage.Headers)
                {
                    Response.Headers[header.Key] = header.Value.ToArray();
                }

                foreach (var header in responseMessage.Content.Headers)
                {
                    Response.Headers[header.Key] = header.Value.ToArray();
                }

                // SendAsync removes chunking from the response. This removes the header so it doesn't expect a chunked response.
                Response.Headers.Remove("transfer-encoding");

                using (var responseStream = await responseMessage.Content.ReadAsStreamAsync())
                {
                    await responseStream.CopyToAsync(Response.Body, 81920, HttpContext.RequestAborted);
                }
            }
        }
    }
}
