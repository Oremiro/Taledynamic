namespace Taledynamic.DAL.Models.Internal
{
    public class ValidateState
    {
        public bool Status { get; set; }
        public string Message { get; set; }

        public ValidateState(bool status, string message)
        {
            Status = status;
            Message = message;
        }
        
    }
}