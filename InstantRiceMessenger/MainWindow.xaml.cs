using System;
using System.ComponentModel;
using System.Configuration;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;

namespace InstantRiceMessenger
{
	public partial class MainWindow : Window
	{
		private Connection conn;
		public MainWindow()
		{
			InitializeComponent();

			useTrayIcon();
			inputMsg.Focus();

			conn = new Connection(
				ConfigurationManager.AppSettings["Host"] ?? "127.0.0.1",
				Int32.Parse(ConfigurationManager.AppSettings["Port"] ?? "6746"),
				msg => statusText.Text = msg
			);
			try
			{
				conn.Server.Start();
			}
			catch
			{
				statusText.Text = "Not Ready - Unable to listen for messages";
			}
			connInputs.ItemsSource = Util.Listo<Connection>(conn);
		}

		private void useTrayIcon()
		{
			var trayIcon = new System.Windows.Forms.NotifyIcon();
			trayIcon.Text = "Instant Rice Messenger";
			trayIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetEntryAssembly().Location);
			trayIcon.ContextMenu = new ContextMenu(new MenuItem[] { new MenuItem("E&xit", (sender, e) => {
				saveConfig();
				trayIcon.Visible = false;
				trayIcon.Dispose();
				trayIcon = null;
				Close();
				System.Windows.Application.Current.Shutdown();
			})});
			trayIcon.Visible = true;
			trayIcon.DoubleClick += (sender, e) =>
			{
				WindowState = WindowState.Normal;
				eventWindowChanged(sender, e);
			};

			StateChanged += eventWindowChanged;
			Closing += new System.ComponentModel.CancelEventHandler((sender, e) =>
			{
				e.Cancel = true;
				WindowState = WindowState.Minimized;
			});
		}

		private void saveConfig()
		{
			Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			config.AppSettings.Settings.Clear();
			config.AppSettings.Settings.Add("Host", conn.Host);
			config.AppSettings.Settings.Add("Port", conn.Port.ToString());
			config.Save(ConfigurationSaveMode.Full);
		}

		private void eventWindowChanged(object sender, EventArgs e)
		{
			if (WindowState == WindowState.Minimized)
			{
				ShowInTaskbar = false;
			}
			else
			{
				ShowInTaskbar = true;
				Activate();
			}
		}

		private void eventSend(object sender, EventArgs e)
		{
			if (inputMsg.Text == ""){ return; }
			try
			{
				conn.Client.Send(inputMsg.Text);
			}
			catch
			{
				statusText.Text = "Message undeliverable";
			}
		}
	}
}
