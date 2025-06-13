namespace AwiaEgyptTravel.Application.Common.Models
{
    public class BaseResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public static BaseResponse<T> SuccessResponse(T data, string message = null)
        {
            return new BaseResponse<T>
            {
                Success = true,
                Message = message ?? "Operation completed successfully",
                Data = data
            };
        }

        public static BaseResponse<T> FailureResponse(string message)
        {
            return new BaseResponse<T>
            {
                Success = false,
                Message = message,
                Data = default
            };
        }
    }
}
