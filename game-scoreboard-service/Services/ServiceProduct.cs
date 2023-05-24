using game_scoreboard_service.Models.Enum;
using game_scoreboard_service.Services.Interfaces;

namespace game_scoreboard_service.Services
{
    /// <summary>
    /// Wrapper around the result of a service.
    /// 
    /// Contains if the service call was successfully processed or 
    /// rejected, and a reason for rejection.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServiceProduct<T> : ServiceProduct
    {
        /// <summary>
        /// Create a typed serviceProduct from an untyped serviceproduct
        /// 
        /// Usefull for rejecting typed service calls;
        /// use <code>return new(Reject(Code, Reason));</code>
        /// 
        /// </summary>
        /// <param name="source"></param>
        public ServiceProduct(IServiceProduct source)
        {
            RejectionCode = source.RejectionCode;
            RejectionReason = source.RejectionReason;
            Success = source.Success;
        }
        public ServiceProduct()
        {
        }

        /// <summary>
        /// Resultdata of the service call
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Implicit operator, so that the result can automatically be wrapped on return call from service.
        /// </summary>
        /// <param name="result"></param>
        public static implicit operator ServiceProduct<T>(T result) => new() { Data = result, Success = true };

        /// <summary>
        /// Implicit operator for automatic dewrapping of the service return call.
        /// </summary>
        /// <param name="source"></param>
        public static implicit operator T?(ServiceProduct<T> source) => source.Data;
    }
    /// <inheritdoc />
    public class ServiceProduct : IServiceProduct
    {
        /// <inheritdoc />
        public bool Success { get; set; }

        /// <inheritdoc />
        public RejectionCode? RejectionCode { get; set; }

        /// <inheritdoc />
        public string? RejectionReason { get; set; }
    }
}
