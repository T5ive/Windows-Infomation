using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TFive_Windows_Information.Properties;

namespace TFive_Windows_Information
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            var cv = new CursorConverter();
            _curTarget = (Cursor)cv.ConvertFrom(Resources.curTarget);
        }

        #region Load/Save

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadLocation();
            LoadSetting();
            _bitmapFind = Resources.bmpFind;
            _bitmapFind2 = Resources.bmpFinda;
            _newCursor = _curTarget;
            dataGridView1.Rows.Add("Position", "");
            dataGridView1.Rows.Add("Color Hex", "");
            dataGridView1.Rows.Add("Color RGB", "");
            dataGridView1.Rows.Add("Result", "");
            dataGridView1.Rows.Add("Size", "");
        }

        private void LoadLocation()
        {
            if (Settings.Default.Location == new System.Drawing.Point(0, 0))
            {
                CenterToScreen();
            }
            else
            {
                Location = Settings.Default.Location;
            }
        }

        private void frm_main_FormClosed(object sender, FormClosedEventArgs e)
        {
            UpdateSetting(4);
        }

        private void LoadSetting()
        {
            alwaysTopToolStripMenuItem.Checked = Settings.Default.alway_top;
            magnifyToolStripMenuItem.Checked = Settings.Default.magni;
            showColorToolStripMenuItem.Checked = Settings.Default.show_color;
            Mode = Settings.Default.crood_mode;
            CoordMode(Mode);
        }

        private void frm_main_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
        private void UpdateSetting(int mode)
        {
            switch (mode)
            {
                case 0: // Top
                    Settings.Default.alway_top = alwaysTopToolStripMenuItem.Checked;
                    break;

                case 1: // Mode
                    Settings.Default.crood_mode = Mode;
                    break;

                case 2: // Magnify
                    Settings.Default.magni = magnifyToolStripMenuItem.Checked;
                    break;

                case 3: // Show Color
                    Settings.Default.show_color = showColorToolStripMenuItem.Checked;
                    break;

                case 4: // All
                    Settings.Default.alway_top = alwaysTopToolStripMenuItem.Checked;
                    Settings.Default.crood_mode = Mode;
                    Settings.Default.magni = magnifyToolStripMenuItem.Checked;
                    Settings.Default.show_color = showColorToolStripMenuItem.Checked;
                    Settings.Default.Location = Location;
                    break;
            }
            Settings.Default.Save();
        }

        #endregion Load/Save

        #region Cusor

        private readonly Cursor _curTarget;

        private Bitmap _bitmapFind;
        private Bitmap _bitmapFind2;
        private Cursor _newCursor;

        #endregion Cusor

        #region Var

        private const uint GaRoot = 2;

        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public int X;
            public int Y;
        }

        #endregion Var

        #region Dll Import

        [DllImport("user32.dll")]
        private static extern IntPtr WindowFromPoint(int xPoint, int yPoint);

        [DllImport("user32.dll", ExactSpelling = true)]
        private static extern IntPtr GetAncestor(IntPtr hWnd, uint gaFlags);

        [DllImport("user32.dll")]
        private static extern bool ScreenToClient(IntPtr hWnd, ref Point lpPoint);

        #endregion Dll Import

        #region Options
        private  void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/T5ive");
        }
        private  void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private  void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"Get Color Window Information" + Environment.NewLine + @"TFive - เขียนโปรแกรมยามว่าง", @"About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void screenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CoordMode(0);
        }

        private void windowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CoordMode(1);
        }

        private void alwaysTopToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            TopMost = alwaysTopToolStripMenuItem.Checked;
            UpdateSetting(0);
        }

        private void showColorToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            panel_color.Visible = showColorToolStripMenuItem.Checked;
            UpdateSetting(3);
        }

        private int Mode = 1;

        private void CoordMode(int mode)
        {
            switch (mode)
            {
                case 0:
                    screenToolStripMenuItem.Checked = true;
                    windowsToolStripMenuItem.Checked = false;
                    Mode = 0;
                    break;

                case 1:
                    windowsToolStripMenuItem.Checked = true;
                    screenToolStripMenuItem.Checked = false;
                    Mode = 1;
                    break;
            }
            UpdateSetting(1);
        }

        private bool _magnifyState = true;

        private void magnifyToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            _magnifyState = magnifyToolStripMenuItem.Checked;
            UpdateSetting(2);
        }

        #endregion Options

        #region Get Color

        private void picTarget_MouseDown(object sender, MouseEventArgs e)
        {
            picTarget.Image = _bitmapFind2;
            picTarget.Cursor = _newCursor;
            if (_magnifyState)
            {
                _magnify.Show();
            }

            tm_getColor.Start();
            tm_mouseMove.Start();
        }

        private void tm_getColor_Tick(object sender, EventArgs e)
        {
            Get_Posix_Color();
        }

        private readonly frmMagnify _magnify = new frmMagnify();
        public static IntPtr IntPtr;
        private int _checkX;
        private int _checkY;
        private void Get_Posix_Color()
        {
            try
            {
                var pt = Cursor.Position;
                var wnd = WindowFromPoint(pt.X, pt.Y);
                var mainWnd = GetAncestor(wnd, GaRoot);
                Point point;
                if (Mode == 0)
                {
                    point.X = Cursor.Position.X;
                    point.Y = Cursor.Position.Y;
                    txtTitle.Clear();
                    //GetAppName.App = null;
                    TFive.App = null;
                    txtClassName.Clear();
                    //GetAppName.Class = null;
                    TFive.Class = null;
                }
                else
                {
                    point.X = pt.X;
                    point.Y = pt.Y;
                    ScreenToClient(mainWnd, ref point);
                    //txt_title.Text = Win32.GetWindowText(mainWnd);
                    txtTitle.Text = TFive.GetWindowsTitle(mainWnd);
                    //GetAppName.App = txtTitle.Text;
                    TFive.App = txtTitle.Text;
                    //txt_class.Text = Win32.GetClassName(mainWnd);
                    txtClassName.Text = TFive.GetWindowsClassName(mainWnd);
                    //GetAppName.Class = txtClassName.Text;
                    TFive.Class = txtClassName.Text;
                    //dataGridView1[1, 4].Value = $"{GetColor_.GetControlSize(mainWnd).Width}, {GetColor_.GetControlSize(mainWnd).Height}";
                    dataGridView1[1, 4].Value = $"{TFive.GetControlSize(mainWnd).Width}, {TFive.GetControlSize(mainWnd).Height}";
                }
                //GetAppName.GetWindow();
                TFive.GetAppName();
                //IntPtr = GetAppName.AppName;
                IntPtr = TFive.AppName;
                dataGridView1[1, 0].Value = $"{point.X}, {point.Y}";
                //dataGridView1[1, 1].Value = GetColor_.GetColorString(int.Parse(point.X.ToString()), int.Parse(point.Y.ToString()));
                dataGridView1[1, 1].Value = TFive.GetHexColor(int.Parse(point.X.ToString()), int.Parse(point.Y.ToString()));
                dataGridView1[1, 2].Value = GenerateRgba();
               
                _checkX = point.X;
                _checkY = point.Y;
               
                panel_color.BackColor = _magnify.magnifyingGlass1.PixelColor;
                LocationMagnify();
                
            }
            catch
            {
                // ignored
            }
        }

        private string CheckResult(int posX, int posY)
        {
            //var color = GetColor_.StringColor(dataGridView1[1, 1].Value.ToString());
            var color = TFive.StringColor(dataGridView1[1, 1].Value.ToString());
            var x = posX;
            var y = posY;
            //var status = GetColor_.GetColorFast(IntPtr, x, y, color, 4).ToString();
            var status = TFive.GetColor(IntPtr, x, y, color).ToString();
            return status;
        }

        private void tm_checkColor_Tick(object sender, EventArgs e)
        {
            try
            {
                dataGridView1[1, 3].Value = CheckResult(_checkX, _checkY);
            }
            catch
            {
                // ignored
            }
        }
        private void LocationMagnify()
        {
            var pt = Cursor.Position;
            pt.X = Cursor.Position.X;
            pt.Y = Cursor.Position.Y;
            var width = Screen.PrimaryScreen.Bounds.Width;
            var height = Screen.PrimaryScreen.Bounds.Height;
            var locationX = 30;
            var locationY = 30;
            if (pt.X > width - 167)
            {
                locationX -= 30 + 167;
            }
            if (pt.Y > height - 167)
            {
                locationY -= 30 + 167;
            }

            _magnify.Location = new System.Drawing.Point(pt.X + locationX, pt.Y + locationY);
        }

        public string GenerateRgba()
        {
            int r = _magnify.magnifyingGlass1.PixelColor.R;
            int g = _magnify.magnifyingGlass1.PixelColor.G;
            int b = _magnify.magnifyingGlass1.PixelColor.B;
            return $"{r}, {g}, {b}";
        }

        private void picTarget_MouseUp(object sender, MouseEventArgs e)
        {
            picTarget.Cursor = Cursors.Default;
            picTarget.Image = _bitmapFind;
            _magnify.Hide();
            tm_getColor.Stop();
            tm_mouseMove.Stop();
        }

        #endregion Get Color

        #region Control Mouse

        [DllImport("user32.dll")] public static extern short GetAsyncKeyState(Keys vKey);

        private void tm_mouseMove_Tick(object sender, EventArgs e)
        {
            if (GetAsyncKeyState(Keys.Up) != 0)
            {
                MouseMove(-1, 0);
            }
            if (GetAsyncKeyState(Keys.Down) != 0)
            {
                MouseMove(1, 0);
            }
            if (GetAsyncKeyState(Keys.Left) != 0)
            {
                MouseMove(0, -1);
            }
            if (GetAsyncKeyState(Keys.Right) != 0)
            {
                MouseMove(0, 1);
            }
        }

        private new void MouseMove(int y, int x)
        {
            Cursor.Position = new System.Drawing.Point(Cursor.Position.X + x, Cursor.Position.Y + y);
        }

        #endregion Control Mouse

        #region  datagridView

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            var m = new ContextMenu();
            m.MenuItems.Add(new MenuItem("Copy"));
            m.Show(dataGridView1, new System.Drawing.Point(e.X, e.Y));
            if (dataGridView1.GetCellCount(DataGridViewElementStates.Selected) > 0)
            {
                CopyText();
            }
        }

        private void CopyText()
        {
            try
            {
                string text;

                var columnIndex = dataGridView1.CurrentCell.ColumnIndex;
                switch (columnIndex)
                {
                    case 0:
                        text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                        break;
                    case 2:
                        text = dataGridView1.SelectedRows[1].Cells[1].Value.ToString();
                        break;
                    case 4:
                        text = dataGridView1.SelectedRows[2].Cells[1].Value.ToString();
                        break;
                    default:
                        text = dataGridView1.CurrentCell.Value.ToString();
                        break;
                }

                if (string.IsNullOrWhiteSpace(text))
                {
                    MessageBox.Show(@"The Clipboard could not be accessed. Please try again.");
                    return;
                }
                Clipboard.SetText(text);
            }
            catch (ExternalException)
            {
                MessageBox.Show(@"The Clipboard could not be accessed. Please try again.");
            }
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            CopyText();
        }



        #endregion

        
    }
}