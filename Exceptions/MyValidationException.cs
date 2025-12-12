namespace ToDo_list.Exceptions
{
    public class MyValidationException : Exception
    {
        public object Details { get; set; }

        public MyValidationException() : base("Validation failed.") { }

        public MyValidationException(string message) : base(message) { }

        public MyValidationException(string message, object details) : base(message)
        {
            Details = details;
        }
    }


}
