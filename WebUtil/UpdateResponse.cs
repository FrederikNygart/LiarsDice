using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebUtil
{
    public class UpdateResponse
    {
        public HttpStatusCode Status { get; set; }
        public string UserMessage { get; set; }
        public string Exception { get; set; }
    }
}
