using System.Diagnostics;

namespace ProductService.Api.Processors
{
    public class DurationWatch
    {
        private readonly Stopwatch sw = new();
        public long duration => sw.ElapsedMilliseconds;
        public DurationWatch() => sw.Start();
    }
}
