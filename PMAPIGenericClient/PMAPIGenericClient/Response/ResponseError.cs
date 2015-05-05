using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuT.PMAPI.Generic
{
    public class ResponseError
    {
        public string Message;
        public string Code;
        public string Subcode;

        public ResponseError(string message, string code, string subcode)
        {
            Message = message;
            Code = code;
            Subcode = subcode;
        }
    }
}
