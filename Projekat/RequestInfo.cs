using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat
{
    internal class RequestInfo
    {
        static private int numberOfRequests = 0;

        public RequestInfo() {
            numberOfRequests++;
            myNumber = numberOfRequests;

        }

        public int myNumber { get; set; }
        public string request { get; set; }

        public string details { get; set; }

        public TimeSpan time { get; set; }

        public override string ToString() {

            string response = myNumber + ". Request\n " + request  + details  + "\n" + "Time: " +time + "\n";
            return response;
        }

    }
}
