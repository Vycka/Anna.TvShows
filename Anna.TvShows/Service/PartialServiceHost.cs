using System;
using System.ServiceProcess;
using log4net;

namespace Anna.TvShows.Service
{
    [System.ComponentModel.DesignerCategory("Code")]
    abstract class PartialServiceHost : ServiceBase
    {
        protected readonly ILog Log;
        protected PartialServiceHost(string serviceName)
        {
            Log = LogManager.GetLogger(GetType());
            ServiceName = serviceName;
        }

        public void Run(string[] args)
        {
            Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            if (Environment.UserInteractive)
            {
                Log.Info("Starting UserInteractive action..");
                OnStart(args);
                HandleUserInteraction();
                OnStop();
                Log.Info("Stoped UserInteractive action..");
            }
            else
            {
                Log.Info("Starting host..");
                Run(this);
                Log.Info("Host service stoped!");
            }
        }

        protected sealed override void OnStart(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            ServiceStarting(args);

            base.OnStart(args);
        }

        protected abstract void ServiceStarting(string[] args);

        protected sealed override void OnStop()
        {
            Log.Info("Stopping service");
            try
            {
                ServiceStopping();

                base.OnStop();

                Log.Info("Service stopped");
            }
            catch (Exception ex)
            {
                Log.Error("Service stop crashed.", ex);
                throw;
            }
        }

        protected abstract void ServiceStopping();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                AppDomain.CurrentDomain.UnhandledException -= OnUnhandledException;
            }
            base.Dispose(disposing);
        }

        private void HandleUserInteraction()
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        protected virtual void OnUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            var exception = args.ExceptionObject as Exception;

            var exc = new ApplicationException(string.Format("Unhandled excaption caught. IsTerminating={0}", args.IsTerminating), exception);

            Log.Error("Unhandled exception", exc);
        }
    }
}