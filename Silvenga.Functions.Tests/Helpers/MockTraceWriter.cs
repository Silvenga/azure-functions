using System.Diagnostics;

using Microsoft.Azure.WebJobs.Host;

namespace Silvenga.Functions.Tests.Helpers
{
    public abstract class MockTraceWriter : TraceWriter
    {
        public MockTraceWriter() : base(TraceLevel.Verbose)
        {
        }
    }
}