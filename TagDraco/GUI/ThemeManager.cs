using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TagDraco.GUI
{
    public class ThemeManager
    {
        public List<Control> lightControls;
        public List<Control> darkControls;
        public List<Control> whiteOrBlack;

        //Dark Theme Colors
        public static Color DarkBlay     { get; } = Color.FromArgb(20, 20, 24);
        public static Color Blay         { get; } = Color.FromArgb(47, 49, 60);
        public static Color LightBlay    { get; } = Color.FromArgb(57, 59, 70);
        public static Color LighterBlay  { get; } = Color.FromArgb(67, 69, 80);


        //Light Theme Colors
        public static Color Blite        { get; } = Color.FromArgb(216, 230, 229);
        public static Color LighterBlite { get; } = Color.FromArgb(240, 255, 254);
        public static Color DarkBlite    { get; } = Color.FromArgb(168, 178, 177);
        public static Color DarkerBlite  { get; } = Color.FromArgb(120, 128, 128);

        public enum Theme
        {
            Dark,
            Light
        }

        MainGUI mainGUI;

        public Theme ActiveTheme { get; private set; } = Theme.Dark;

        public ThemeManager(MainGUI mainGUI)
        {
            lightControls = new List<Control>();
            darkControls = new List<Control>();
            whiteOrBlack = new List<Control>();
            this.mainGUI = mainGUI;
            foreach(Control control in mainGUI.Controls)
            {
                CheckAndAddControl(control);
                IterateControls(control);
            }
        }

        void IterateControls(Control ctrl)
        {
            foreach (Control c in ctrl.Controls)
            {
                IterateControls(c);
                CheckAndAddControl(c);
            }
        }

        void IterateAndRefreshControls(Control ctrl)
        {
            foreach (Control c in ctrl.Controls)
            {
                IterateControls(c);
                c.Invalidate();
            }
        }

        void CheckAndAddControl(Control control)
        {
            if (control.Equals(null))
                return;
            if (control is TextBox || control is Button 
                || control is MenuStrip || control.Name.Equals("mainPanel")
                || control is PictureBox)
            {
                darkControls.Add(control);
            }
            else if (control is Label || control is GroupBox)
            {
                whiteOrBlack.Add(control);
            }
            else
            {
                lightControls.Add(control);
            }
        }

        public void ChangeTheme(Theme theme)
        {
            ActiveTheme = theme;
            if (theme.Equals(Theme.Dark))
            {
                foreach(Control control in lightControls)
                {
                    control.BackColor = Blay;
                    control.ForeColor = Color.White;
                    mainGUI.BackColor = Blay;
                }
                foreach(Control control in darkControls)
                {
                    control.BackColor = DarkBlay;
                    control.ForeColor = Color.White;
                }
                foreach(Control control in whiteOrBlack)
                {
                    control.ForeColor = Color.White;
                    if (control is GroupBox)
                        ((GroupBox)control).Update();
                }
            }
            else
            {
                foreach (Control control in lightControls)
                {
                    control.BackColor = Blite;
                    control.ForeColor = Color.Black;
                    mainGUI.BackColor = Blite;
                }
                foreach (Control control in darkControls)
                {
                    control.BackColor = DarkBlite;
                    control.ForeColor = Color.Black;
                }
                foreach (Control control in whiteOrBlack)
                {
                    control.ForeColor = Color.Black;
                    if (control is GroupBox)
                        ((GroupBox)control).Update();
                }
            }
            mainGUI.Refresh();
            mainGUI.Invalidate();
            IterateAndRefreshControls(mainGUI);
        }


        

    }
}
