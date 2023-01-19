namespace CircuitBreaker;

internal class Program
{
    private static readonly Random Rand = new(100);

    protected Program()
    {
    }

    private static void Main()
    {
        var circuitBraker = new CircuitBreaker(5, TimeSpan.FromSeconds(5));
        circuitBraker.OnStateChange += CircuitBraker_OnStateChange;
        Console.WriteLine("Circuit Braker:");
        Console.WriteLine($" - {nameof(circuitBraker.Threshold)}: {circuitBraker.Threshold}");
        Console.WriteLine($" - {nameof(circuitBraker.Timeout)}: {circuitBraker.Timeout}");
        Console.WriteLine($" - {nameof(circuitBraker.CurrentState)}: {circuitBraker.CurrentState}");

        for (int i = 0; i < 15; i++)
        {
            var isSucceeded = circuitBraker.TryCall(() =>
            {
                Console.WriteLine("Executing...");

                ThrowExceptionRandomly();
            });

            if (!isSucceeded)
            {
                Console.WriteLine($"Executing failed. {nameof(circuitBraker.CurrentState)}: {circuitBraker.CurrentState}");
            }

            Thread.Sleep(TimeSpan.FromSeconds(1));
        }

        circuitBraker.OnStateChange -= CircuitBraker_OnStateChange;

        void ThrowExceptionRandomly()
        {
            if (Rand.Next() % 2 == 0)
            {
                throw new RandomException();
            }
        }

        void CircuitBraker_OnStateChange(CircuitState state)
        {
            Console.WriteLine($"State has changed to: {state}");
        }
    }
}
