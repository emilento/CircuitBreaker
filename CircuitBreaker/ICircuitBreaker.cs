namespace CircuitBreaker
{
    public interface ICircuitBreaker
    {
        CircuitState CurrentState { get; }

        int Threshold { get; }

        TimeSpan Timeout { get;}

        void Call(Action action);

        bool TryCall(Action action);
    }
}
