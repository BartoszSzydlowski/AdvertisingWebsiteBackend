using System.Collections.Generic;

namespace API.Wrappers
{
    public class Response
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }

        public Response()
        {
        }

        public Response(bool succeeded, string message)
        {
            Succeeded = succeeded;
            Message = message;
        }
    }

    public class Response<T> : Response
    {
        public T Data { get; set; }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public Response()
        {
        }

        public Response(T data)
        {
            Data = data;
            Succeeded = true;
        }
    }
}