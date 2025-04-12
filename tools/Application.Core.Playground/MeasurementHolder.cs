using System.Collections.Concurrent;

namespace Application.Core.Playground;

public class MeasurementHolder
{
    public static MeasurementHolder Instance
    {
        get
        {
            if (_instance is null)
            {
                _instance = new MeasurementHolder();
            }

            return _instance;
        }
    }

    private static MeasurementHolder _instance = null!;

    public IReadOnlyDictionary<string, double[]> OperationMeasurements => _operationMeasurements;

    private readonly ConcurrentDictionary<string, double[]> _operationMeasurements = [];

    public void AddMeasurement(string operationName, double measurementInMs)
    {
        if (!OperationMeasurements.TryGetValue(operationName, out var measurements))
        {
            _operationMeasurements.TryAdd(operationName, [measurementInMs]);
            return;
        }
        var updatedMeasurements = new List<double>(measurements) { measurementInMs };
        _operationMeasurements.TryUpdate(operationName, updatedMeasurements.ToArray(), measurements);
    }
    public void PrintResults()
    {
        foreach (var (operationName,measurementInMs) in OperationMeasurements)
        {
            Console.WriteLine("Operation: " + operationName);
            for (var i = 0; i < measurementInMs.Length; i++)
            {
                Console.WriteLine($"{i} => {measurementInMs[i]} ms");
            }
            Console.WriteLine("Overall statistics => " + operationName);
            Console.WriteLine("Count => "+ measurementInMs.Length);
            Console.WriteLine("Avg => "+ measurementInMs.Average());
            Console.WriteLine("Min => "+ measurementInMs.Min());
            Console.WriteLine("Max =>  "+ measurementInMs.Max());
            Console.WriteLine("--------------");
        }
    }
}