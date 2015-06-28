using System;
using Anna.TvShows.Service;

namespace Anna.TvShows
{
    class TvShowsService : PartialServiceHost
    {
        public TvShowsService(string serviceName) : base(serviceName)
        {
        }

        protected override void ServiceStarting(string[] args)
        {
            Console.Out.WriteLine("My cool server has started");
        }

        protected override void ServiceStopping()
        {
            Console.Out.WriteLine("My cool service has stopped");
        }
    }
}