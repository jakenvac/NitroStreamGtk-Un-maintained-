using System;
using Gtk;

namespace NitroStreamGtk
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			new MainClass ();
		}

		public MainClass()
		{
			Gtk.Application.Init ();
			new ViewModel ();
			Gtk.Application.Run ();
		}
	}
}
