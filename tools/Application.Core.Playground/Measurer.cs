using System.Diagnostics;

namespace Application.Core.Playground;

public class Measurer(string operationName) : IDisposable
{
    private readonly long _startingTimestamp = Stopwatch.GetTimestamp();

    public void Dispose()
    {
        var endingTimestamp = Stopwatch.GetTimestamp();
        var elapsedInMs = Stopwatch.GetElapsedTime(_startingTimestamp, endingTimestamp).TotalMilliseconds;
        MeasurementHolder.Instance.AddMeasurement(operationName, elapsedInMs);
    }
}