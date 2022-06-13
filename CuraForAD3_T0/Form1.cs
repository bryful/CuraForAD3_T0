using BRY;
using Codeplex.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
/// <summary>
/// 基本となるアプリのスケルトン
/// </summary>
namespace CureForAD3_T0
{
	public partial class Form1 : Form
	{
		//-------------------------------------------------------------
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Form1()
		{
			InitializeComponent();
		}
		/// <summary>
		/// コントロールの初期化はこっちでやる
		/// </summary>
		protected override void InitLayout()
		{
			base.InitLayout();
		}
		//-------------------------------------------------------------
		/// <summary>
		/// フォーム作成時に呼ばれる
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form1_Load(object sender, EventArgs e)
		{
			if (ModifierKeys == 0)
			{
				//設定ファイルの読み込み
				JsonPref pref = new JsonPref();
				if (pref.Load())
				{
					bool ok = false;
					Size sz = pref.GetSize("Size", out ok);
					if (ok) this.Size = sz;
					Point p = pref.GetPoint("Point", out ok);
					if (ok) this.Location = p;
				}
			}
			this.Text = Path.GetFileNameWithoutExtension(Application.ExecutablePath);
		}
		//-------------------------------------------------------------
		/// <summary>
		/// フォームが閉じられた時
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
			//設定ファイルの保存
			JsonPref pref = new JsonPref();
			pref.SetSize("Size", this.Size);
			pref.SetPoint("Point", this.Location);

			pref.SetIntArray("IntArray", new int[] { 8, 9, 7 });
			pref.Save();

		}
		//-------------------------------------------------------------
		/// <summary>
		/// ドラッグ＆ドロップの準備
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form1_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.All;
			}
			else
			{
				e.Effect = DragDropEffects.None;
			}
		}
		/// <summary>
		/// ドラッグ＆ドロップの本体
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form1_DragDrop(object sender, DragEventArgs e)
		{
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
			//ここでは単純にファイルをリストアップするだけ
			GetCommand(files);
		}
		//-------------------------------------------------------------
		/// <summary>
		/// ダミー関数
		/// </summary>
		/// <param name="cmd"></param>
		public void GetCommand(string[] cmd)
		{
			if (cmd.Length > 0)
			{
				foreach (string s in cmd)
				{
					if (ChnageT0(s)==false)
					{
						textBox1.Text += "Error : " + s + "\r\n";
					}
					else
					{
						textBox1.Text += "Success! : " + s + "\r\n";
					}
				}
			}
		}
		/// <summary>
		/// メニューの終了
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		//-------------------------------------------------------------
		private void quitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AppInfoDialog.ShowAppInfoDialog();
		}
		//-------------------------------------------------------------
		private string BakFileName(string p)
		{
			if (File.Exists(p)==false)
			{
				return p;
			}
			string p2 = Path.GetFileNameWithoutExtension(p);
			int inx = 0;
			string pp = p2 + string.Format(".{0:3}",inx);
			while(File.Exists(pp)==true)
			{
				inx++;
				pp = p2 + string.Format(".{0:3}",inx);
			}
			return pp;
		}

		//-------------------------------------------------------------
		public bool ChnageT0(string p)
		{
			bool ret = false;
			if (File.Exists(p) == false) return ret;

			string[] lines = new string[0]; try
			{
				lines = System.IO.File.ReadAllLines(p, Encoding.GetEncoding("shift_jis"));
			}
			catch
			{
				return ret;
			}
			if (lines.Length <= 0) return ret;

			for (int i=0; i< lines.Length; i++)
			{
				string[] line = lines[i].Trim().Split(' ');
				switch(line.Length)
				{
					case 0:
					case 1:
						break;
					default:
						if  (line[0] == "M140")
						{
							lines[i] += " T0";

						}else if (line[0] == "M104")
						{
							if ((line.Length==2)&&(line[1]=="S0"))
							{
								lines[i] += " T1";
							}
							else
							{
								lines[i] += " T0";
							}
						}
						break;
				}
			}

			try
			{
				string gfile = Path.ChangeExtension(p, ".g");

				if (File.Exists(gfile)==true)
				{
					string bak = BakFileName(Path.ChangeExtension(gfile, ".000"));
					File.Move(gfile, bak);
				}
				System.IO.File.WriteAllLines(gfile, lines, Encoding.GetEncoding("shift_jis"));
				ret = true;
			}
			catch
			{
				ret = false;
			}
			return ret;
		}


	}
}
