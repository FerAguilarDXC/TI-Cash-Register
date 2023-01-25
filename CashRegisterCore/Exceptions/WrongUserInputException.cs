namespace CashRegisterCore.Exceptions
{
    //
    // Summary:
    //     Represents errors caused by incorrect user data.
    [Serializable]
    public class WrongUserInputException : Exception
    {
        //
        // Summary:
        //     Initializes a new instance of the WrongUserInputException class.
        public WrongUserInputException()
        {
        }
        //
        // Summary:
        //     Initializes a new instance of the WrongUserInputException class with a specified error message.
        //
        // Parameters:
        //   message:
        //     Error Message.
        public WrongUserInputException(string? message) : base(message)
        {
        }
    }
}