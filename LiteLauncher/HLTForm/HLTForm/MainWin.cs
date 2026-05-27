using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HLTStudio.Commons;
using HLTStudio.Dialogs;

namespace HLTStudio
{
	public partial class MainWin : Form
	{
		#region WndProc

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			const int WM_SYSCOMMAND = 0x112;
			const long SC_CLOSE = 0xF060L;
			const long WP_MASK = 0xFFF0L;

			if (m.Msg == WM_SYSCOMMAND && (m.WParam.ToInt64() & WP_MASK) == SC_CLOSE)
				return;

			base.WndProc(ref m);
		}

		#endregion

		public MainWin()
		{
			InitializeComponent();
		}

		private void MainWin_Load(object sender, EventArgs e)
		{
			this.MinimumSize = this.Size;
		}

		private void MainWin_Shown(object sender, EventArgs e)
		{
			new Thread(() =>
			{
				try
				{
					Thread closeDelayTh = new Thread(() =>
					{
						Thread.Sleep(500);
					});

					closeDelayTh.Start();
					try
					{
						new LaunchSequence().Run();
					}
					finally
					{
						closeDelayTh.Join();
					}

					this.BeginInvoke((MethodInvoker)delegate
					{
						this.Close();
					});
				}
				catch (Exception ex)
				{
					this.BeginInvoke((MethodInvoker)delegate
					{
						MessageDlg.Run(
							MessageDlg.Kind_e.Warning,
							"エラーか何か",
							ex.Message,
							ex,
							new string[] { "OK" }
							);

						this.Close();
					});
				}
			})
			.Start();
		}
	}
}
