using FastEndpoints;

namespace ProductService.Api.Processors
{
    public class InitDurationWatcherPreprocessor : IGlobalPreProcessor
    {
        public Task PreProcessAsync(IPreProcessorContext context,   CancellationToken ct)
        {
            DurationWatch durationWatcher = new();
            context.HttpContext.Items.Add("DurationWatcher", durationWatcher);
            return Task.CompletedTask;
        }
    }
}
