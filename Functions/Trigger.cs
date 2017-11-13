using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace AzureFunctions
{
    public static class CheckTemperatureAndHumidity
    {
        [FunctionName("CheckTemperatureAndHumidity")]
        public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"Trigger executed at: {DateTime.Now}");

            log.Info("Hello from log info!");
        }
    }

    public static class Whatever
    {
        [FunctionName("Whatever")]
        public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"Trigger executed at: {DateTime.Now}");

            log.Info("Hello from log info!");
        }
    }

    public static class TestHTTPEndpoint
    {
        [FunctionName("TestHTTPEndPoint")]
        public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
        {
            log.Info($"C# HTTP trigger function processed a request. RequestUri={req.RequestUri}");

            // parse query parameter
            string name = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
                .Value;

            // Get request body
            dynamic data = await req.Content.ReadAsAsync<object>();

            // Set name to query string or body data
            name = name ?? data?.name;

            return name == null
                ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
                : req.CreateResponse(HttpStatusCode.OK, "Hello " + name);
        }
    }

    public class TraceWriterSink : ILogEventSink
    {
        private readonly TraceWriter writer;

        public TraceWriterSink(TraceWriter writer)
        {
            this.writer = writer;
        }

        public void Emit(LogEvent logEvent)
        {
            switch (logEvent.Level)
            {
                case LogEventLevel.Verbose:
                case LogEventLevel.Debug:
                    writer.Verbose(logEvent.RenderMessage());
                    break;
                case LogEventLevel.Information:
                    writer.Info(logEvent.RenderMessage());
                    break;
                case LogEventLevel.Warning:
                    writer.Warning(logEvent.RenderMessage());
                    break;
                default:
                    writer.Error(logEvent.RenderMessage());
                    break;
            }
        }
    }
}
