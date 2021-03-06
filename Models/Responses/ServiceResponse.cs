using System;
using System.Collections.Generic;
using System.Text;

<<<<<<< HEAD
namespace Models.Responses
=======
namespace Models
>>>>>>> 69142915af0cedae9b642d72a42af8d86afd3ec1
{
    public class ServiceResponse<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = null;
    }
}
