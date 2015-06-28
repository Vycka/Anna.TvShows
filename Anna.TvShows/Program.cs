using System.Threading;
using Anna.TvShows.Service;

namespace Anna.TvShows
{
    class Program
    {
        private const string ServiceName = "Anna.TvShows";

        static void Main(string[] args)
        {
            if (args.Length != 0 && args[0] == "--install")
            {
                new InstallService(ServiceName).Install();
                Thread.Sleep(3000);
            }
            else
            {
                PartialServiceHost service = new TvShowsService(ServiceName);
                service.Run(args);
            }
        }
    }
}
