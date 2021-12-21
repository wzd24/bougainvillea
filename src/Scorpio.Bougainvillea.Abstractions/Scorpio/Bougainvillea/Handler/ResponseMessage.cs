using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.Bougainvillea.Middleware;

namespace Scorpio.Bougainvillea.Handler
{
    internal class ResponseMessage : IResponseMessage,ISuccessd
    {
        public static ResponseMessage Sucess { get; } = new ResponseMessage(0);
        public int Code { get; }
        public string Message { get; }
        public object Data { get; }

        public ResponseMessage(int code, string message=null, object data=null)
        {
            Code = code;
            Message = message;
            Data = data;
        }

        public bool IsSuccessed() => Code == 0;
    }
}
