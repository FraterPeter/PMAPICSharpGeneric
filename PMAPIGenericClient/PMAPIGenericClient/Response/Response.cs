using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuT.PMAPI.Generic
{
    public class Response
    {
        public int Count { get; set; }
        public string Status { get; set; }
        public string Next { get; set; }
        public List<Dictionary<string, object>> Data { get; set; }
        public bool IsError;
        public ResponseError Error;

        public void setError(string message, string code, string subcode)
        {
            IsError = true;
            Error = new ResponseError(message, code, subcode);
        }
    }
}
