using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.AspNetCore.Health;
using App.Metrics.Formatters;
using App.Metrics.Formatters.Prometheus;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DepCheck.AppMetrik
{
    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder UseAppMetrics(this IWebHostBuilder hostBuilder)
        {
            var metrics = AppMetrics.CreateDefaultBuilder()
                                .OutputMetrics.AsPrometheusPlainText()
                                .Build();

            return hostBuilder
                .UseHealth()
                .ConfigureMetrics(metrics)
                .UseMetrics(o =>
                {
                    o.EndpointOptions = 
                        endpointsOptions => endpointsOptions
                                                .MetricsTextEndpointOutputFormatter = metrics
                                                                                        .OutputMetricsFormatters
                                                                                        .GetType<MetricsPrometheusTextOutputFormatter>();
                });
        }
    }
}
