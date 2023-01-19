using System.Runtime.Serialization;

[Serializable]
internal class RandomException : Exception
{
    public RandomException()
    {
    }

    public RandomException(string? message) : base(message)
    {
    }

    public RandomException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected RandomException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}