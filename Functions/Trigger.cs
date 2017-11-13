﻿using System;
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
