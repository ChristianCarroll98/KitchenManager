namespace KitchenManager.API.SharedNS.ResponseNS
{
    public class Response<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "No Error";

        public T Data { get; set; } = default;
    }
}