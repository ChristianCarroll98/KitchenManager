namespace KitchenManager.KMAPI.Shared
{
    public class ResponseBase
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "No Error";
    }
}