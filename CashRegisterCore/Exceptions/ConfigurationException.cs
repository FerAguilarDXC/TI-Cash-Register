namespace CashRegisterCore.Exceptions
{
    //
    // Summary:
    //     Represents errors caused by incorrect user data.
    [Serializable]
    public class ConfigurationException : Exception
    {
        //
        // Summary:
        //     Initializes a new instance of the ConfigurationException class.
        public ConfigurationException()
        {
        }
        //
        // Summary:
        //     Initializes a new instance of the ConfigurationException class with a specified error message.
        //
        // Parameters:
        //   message:
        //     Error Message.
        public ConfigurationException(string? message) : base(message)
        {
        }
    }
}