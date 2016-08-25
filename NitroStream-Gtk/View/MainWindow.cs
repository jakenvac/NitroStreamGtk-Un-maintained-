using System;
using Gtk;

namespace NitroStreamGtk.View
{
	public partial class MainWindow: Gtk.Window
	{
		// Hacky way of getting an Mvvm like datacontext
		public object DataContext {get;set;}

        public bool UpdateEnabled
        {
            set { btnUpdate.Sensitive = value; }
        }
        public string UpdateText
        {
            set { btnUpdate.Label = value; }
        }
        public string logText
        {
            set { txtLog.Buffer.Text = value; }
        }

		public MainWindow(object dc) : base (Gtk.WindowType.Toplevel)
		{
			DataContext = dc;
//            Menu.
			Build();
		}



		protected void OnDeleteEvent (object sender, DeleteEventArgs a)
		{
			Application.Quit ();
			a.RetVal = true;
		}

		protected void CheckUpdate ()
		{
			ViewModel vm = DataContext as ViewModel;
			if (vm != null && vm.Updater != null)
			{
				btnUpdate.Sensitive = vm.Updater.UpdateAvailable;
			}
		}

		protected void onDonateClick (object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=ASKURE99X999W");
		}

		public void Populate ()
		{
			ViewModel vm = DataContext as ViewModel;
			if (vm != null && vm.ViewSettings != null)
			{
				libNitroStream.ViewSettings vs = vm.ViewSettings;
				txtIP.Text = vs.IPAddress;
				txtTopScale.Text = vs.TopScale.ToString ();
				txtBottomScale.Text = vs.BottomScale.ToString ();
				cmbViewmode.Active = (vs.ViewMode == libNitroStream.Orientations.Vertical ? 0 : 1);
				spinQuality.Value = vs.PictureQuality;
				txtPriority.Text = vs.PriorityFactor.ToString ();
				txtQoS.Text = vs.QosValue.ToString ();
				cmbScreen.Active = vs.PriorityMode ? 1 : 0;
			}
		}

		protected void ConnectButtonPressed (object sender, EventArgs e)
		{
			ViewModel vm = this.DataContext as ViewModel;
			if (vm != null) {
				libNitroStream.ViewSettings vs = vm.ViewSettings;
				vs.IPAddress = txtIP.Text;
				vs.ViewMode = cmbViewmode.Active == 0 ? libNitroStream.Orientations.Vertical : libNitroStream.Orientations.Horizontal;
				vs.PriorityMode = cmbScreen.Active == 1;

				double ts, bs;
				uint pf, qos, pq;

				if (double.TryParse (txtTopScale.Text, out ts)
					&& double.TryParse (txtBottomScale.Text, out bs)
					&& uint.TryParse (txtPriority.Text, out pf)
					&& uint.TryParse (txtQoS.Text, out qos)
					&& uint.TryParse (spinQuality.Value.ToString (), out pq))
				{
					vs.TopScale = ts;
					vs.BottomScale = bs;
					vs.PriorityFactor = pf;
					vs.QosValue = qos;
					vs.PictureQuality = pq;
				}
				else
				{
					throw new Exception ("A field has an incorrect value.");
				}

				vm.ViewSettings = vs;
				vm.SaveViewSettings();
                vm.InitiateConnection(libNitroStream.ClientManager.ConnectionIntents.RemotePlay);
			}
		}

		protected void DefineViewrPath (object sender, EventArgs e)
		{
			ViewModel vm = this.DataContext as ViewModel;
			if (vm != null)
			{
				using (FileChooserDialog f = new FileChooserDialog ("Locate NTRViewer.exe", this, FileChooserAction.Open,
					   "Cancel", ResponseType.Cancel, "Select", ResponseType.Accept))
				{
					if (f.Run() == (int)ResponseType.Accept)
					{
						vm.ViewSettings.ViewerPath = f.Filename;
						vm.SaveViewSettings();
					}
					f.Destroy();
				}
			}
		}

		protected override void OnShown ()
		{
			CheckUpdate();
			base.OnShown ();
		}

		protected void onWindowRealized (object sender, EventArgs e)
		{
			CheckUpdate();
		}

		protected void onWindowShown (object sender, EventArgs e)
		{
			CheckUpdate();
		}

        protected void onUpdateButtonPress(object sender, EventArgs e)
        {
            ViewModel vm = DataContext as ViewModel;
            if (vm != null)
            {
                vm.Updater.GetUpdate();
            }
        }

        protected void onSendMemoryPatch(object sender, EventArgs e)
        {
            ViewModel vm = DataContext as ViewModel;
            if (vm != null)
            {           
                vm.InitiateConnection(libNitroStream.ClientManager.ConnectionIntents.MemoryPatch);
            }
        }

        protected void onShowLogToggled(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

	}

}
