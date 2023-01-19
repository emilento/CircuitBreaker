namespace CircuitBreaker
{
    public class CircuitBreaker : ICircuitBreaker
    {
        public CircuitBreaker(int threshold, TimeSpan timeout)
        {
            Threshold = threshold;
            Timeout = timeout;
        }

        public int Threshold { get; private set; }
        
        public TimeSpan Timeout { get; private set; }

        public event Action<CircuitState>? OnStateChange;

        public CircuitState CurrentState
        {
            get => _state; 
            set { _state = value; OnStateChange?.Invoke(_state); }
        }

        private CircuitState _state;

        private DateTime? _openDateTime;

        private int _failuresCount;

        public void Call(Action action)
        {
            if (CurrentState == CircuitState.Open)
            {
                if (_openDateTime != null && DateTime.UtcNow >= _openDateTime + Timeout)
                {
                    CurrentState = CircuitState.HalfOpen;
                    _openDateTime = null;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }

            try
            {
                action();

                if (CurrentState == CircuitState.HalfOpen)
                {
                    CurrentState = CircuitState.Closed;
                    _failuresCount = 0;
                }
            }
            catch
            {
                _failuresCount++;
                if (_failuresCount >= Threshold)
                {
                    CurrentState = CircuitState.Open;
                    _openDateTime = DateTime.UtcNow;
                 } 
            }
        } 

        public bool TryCall(Action action)
        {
            try
            {
                Call(action);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
