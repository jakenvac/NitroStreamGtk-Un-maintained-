using System;
using Gtk;
using libNitroStream;
using System.Text;

namespace NitroStreamGtk
{
	public class ViewModel
	{  

        // Getting the assembly version doesn't seem to work correctly in mono
            // If anyone knows a better way that this please issue a pull request with a fix!!
		public static string Version { get { return "0.1.0.0"; } }

		public View.MainWindow MainWindow { get; set; }
		public ViewSettings ViewSettings { get; set; }
		public Updater Updater {get; private set; }

        private StringBuilder _LogText { get; set;}

		private ClientManager _ClientManager;
		private string _ConfigPath {get { return System.IO.Path.Combine (AppDomain.CurrentDomain.BaseDirectory, "config.xml"); } }

		public ViewModel ()
		{
			ViewSettings = ViewSettings.Load(_ConfigPath) ?? new ViewSettings();
			Updater = new Updater("NitroStreamGtk","JakeHL","NitroStreamGtk-", Version);
            _ClientManager = new ClientManager(ViewSettings);
            Logger.Logged += onLogRecieved;
            _LogText = new StringBuilder();
            AppDomain.CurrentDomain.UnhandledException += onUnhandledException;
			MainWindow = new View.MainWindow(this);
            Updater.UpdateFound += onUpdateFound;
            Updater.CheckUpdate();

			MainWindow.Populate();
			MainWindow.Show();
		}

        private void onUnhandledException (object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            _LogText.Append(ex.Message);
            MainWindow.logText = _LogText.ToString();
        }

        private void onLogRecieved(object sender, LogEventArgs e)
        {
            WriteToLog(e.Message);
        }

        public void WriteToLog(string msg)
        {
            _LogText.Append(msg + "\n");
            MainWindow.logText = _LogText.ToString();
        }

        public void onUpdateFound (object source, EventArgs e)
        {
            MainWindow.UpdateEnabled = true;
            MainWindow.UpdateText = "Update Available!";
        }

		public void SaveViewSettings()
		{
			ViewSettings.Save(_ConfigPath);
		}

		public void InitiateConnection(ClientManager.ConnectionIntents intent)
		{
            _ClientManager.Initiate(intent);
		}


	}
}

