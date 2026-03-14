namespace FieldReservation.Domain.Exceptions
{
    public class EmailSendingException : Exception
    {
        public EmailSendingException(string message) : base(message) { }
    }
}