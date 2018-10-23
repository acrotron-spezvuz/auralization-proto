using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auralization.API
{
    /// <summary>
    /// Default api response wrapper
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Data for client
        /// </summary>
        public T Data { get; set; }
        
        /// <summary>
        ///  default constructor
        /// </summary>
        public ApiResponse(){}

        /// <summary>
        /// Init reponse with data
        /// </summary>
        /// <param name="data"></param>
        public ApiResponse(T data) => Data = data;
    }
}
