using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ServerLibrary.Model.Entities
{
    public class Error
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public string Details { get; set; }

        public string StackTrace { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
