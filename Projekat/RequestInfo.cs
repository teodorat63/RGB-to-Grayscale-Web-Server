using System;
using System.Diagnostics;

public class RequestInfo
{
    static private int numberOfRequests = 0;

    public RequestInfo()
    {
        numberOfRequests++;
        myNumber = numberOfRequests;
        stopwatch = new Stopwatch();
    }

    public int myNumber { get; set; }

    private string _request;
    public string request
    {
        get { return _request; }
        set
        {
            _request = value;
            if (stopwatch.IsRunning == false)
                stopwatch.Start();
        }
    }

    private string _details;
    public string details
    {
        get { return _details; }
        set
        {
            _details = value;
            if (stopwatch.IsRunning)
            {
                stopwatch.Stop();
                time = stopwatch.Elapsed;
            }
        }
    }

    public TimeSpan time { get; private set; }
    private Stopwatch stopwatch;

    public override string ToString()
    {
        string response = myNumber + ". Request\n " + request + " " + details + "\n" + "Time: " + time + "\n";
        return response;
    }
}
