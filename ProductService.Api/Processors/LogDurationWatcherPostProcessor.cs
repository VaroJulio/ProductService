using FastEndpoints;

namespace ProductService.Api.Processors
{
    public class LogDurationWatcherPostProcessor : IGlobalPostProcessor
    {
        public Task PostProcessAsync(IPostProcessorContext context, CancellationToken ct)
        {
            var durationWatcher = (DurationWatch?) (context.HttpContext.Items.FirstOrDefault(x => (string)x.Key == "DurationWatcher")).Value;

            context.HttpContext.Resolve<ILogger<LogDurationWatcherPostProcessor>>()
            .LogInformation($"request to: {context.HttpContext.GetEndpoint()?.DisplayName} with identifier: {context.HttpContext.TraceIdentifier} took {durationWatcher?.duration} ms.");

            return Task.CompletedTask;
        }
    }
}
