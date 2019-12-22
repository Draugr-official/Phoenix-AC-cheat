using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Phoenix___AssaultCube
{
    public partial class Form1 : Form
    {
        StormClient funcs = new StormClient("unhooked");
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
           int nLeftRect,
           int nTopRect,
           int nRightRect,
           int nBottomRect,
           int nWidthEllipse,
           int nHeightEllipse
        );
        public static int ClientID = 0x1E4;

        public static int pState = 0x50F4F4;
        public static int Health = 0xF8;
        public static int Speed = 0x80;
        public static int PrimaryAmmo = 0x150;
        public static int PrimaryReserve = 0x128;
        public static int PrimaryDelay = 0x178;
        public static int SecondaryAmmo = 0x13C;
        public static int AkimboAmmo = 0x15C;
        public static int Armor = 0xFC;
        public static int Grenade = 0x158;
        public static int GrenadeWaitingTime = 0x180;
        public static int Akimbo = 0x10C;
        public static int Recoil = 0x4EE444;
        public static int ShotsFired = 0x1A0;
        public static int delayTime = 0x50;
        public static int PlrArrayPtr = 0x10;
        public static int PlrEntityPtr = 0xC;
        public static int LocalName = 0x224;

        public static int PlrHeight = 0x5C;
        public static int PlrNormalHeight = 0x60;
        public static int PlrKills = 0x1FC;

        public static int m_Sound = 0x0104;
        public static int m_Reload = 0x0106;
        public static int m_ReloadTime = 0x0108;
        public static int m_AttackDelay = 0x010A;
        public static int m_Damage = 0x010C;
        public static int m_Piercing = 0x010E;
        public static int m_ProjSpeed = 0x0110;
        public static int m_Part = 0x0112;
        public static int m_Spread = 0x0114;
        public static int m_Recoil = 0x0116;
        public static int m_MagSize = 0x0118;
        public static int m_MdlKickRot = 0x011A;
        public static int m_MdlKickBack = 0x011C;
        public static int m_RecoilIncrease = 0x011E;
        public static int m_RecoilBase = 0x0120;
        public static int m_MaxRecoil = 0x0122;
        public static int m_RecoilBackFade = 0x0124;
        public static int m_PushFactor = 0x0126;
        public static int m_IsAuto = 0x0128;

        public static int test_CordinationPTR = 0x1E18D18;
        public static int test_Ord_Y = 0x000C;

        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }
        private void writecons(string input)
        {
            richTextBox2.Text = richTextBox2.Text + "\n" + input;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            hook();
            autohooker.Start();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            string[] lines = richTextBox1.Text.Split('\n');
            int editorlinelength = 0;
            linenum.Items.Clear();
            foreach (string line in lines)
            {
                linenum.Items.Add(editorlinelength);
                editorlinelength += 1;
            }
            string keywords = @"\b(Ammo|PrimaryGun|SecondaryGun|Enabled|DualWield|Armor|Health|WalkSpeed|Recoil|Grenade|GunSettings|Kills)\b";
            MatchCollection keywordMatches = Regex.Matches(this.richTextBox1.Text, keywords);

            string letters = @"\b(1|2|3|4|5|6|7|8|9|0)\b";
            MatchCollection lettersMatches = Regex.Matches(this.richTextBox1.Text, letters);

            string classes = @"\b(game|Players|LocalPlayer|Character|Humanoid|print|true|false|Stats)\b";
            MatchCollection classesmatches = Regex.Matches(this.richTextBox1.Text, classes);

            string types = @"\b(Console)\b";
            MatchCollection typeMatches = Regex.Matches(this.richTextBox1.Text, types);

            string comments = @"(\/\/.+?$|\/\*.+?\*\/)";
            MatchCollection commentMatches = Regex.Matches(this.richTextBox1.Text, comments, RegexOptions.Multiline);

            int originalIndex = this.richTextBox1.SelectionStart;
            int originalLength = this.richTextBox1.SelectionLength;
            Color originalColor = Color.White;

            this.label2.Focus();

            this.richTextBox1.SelectionStart = 0;
            this.richTextBox1.SelectionLength = this.richTextBox1.Text.Length;
            this.richTextBox1.SelectionColor = originalColor;

            foreach (Match m in keywordMatches)
            {
                this.richTextBox1.SelectionStart = m.Index;
                this.richTextBox1.SelectionLength = m.Length;
                this.richTextBox1.SelectionColor = Color.CornflowerBlue;
            }

            foreach (Match m in classesmatches)
            {
                this.richTextBox1.SelectionStart = m.Index;
                this.richTextBox1.SelectionLength = m.Length;
                this.richTextBox1.SelectionColor = Color.CadetBlue;
            }

            foreach (Match m in lettersMatches)
            {
                this.richTextBox1.SelectionStart = m.Index;
                this.richTextBox1.SelectionLength = m.Length;
                this.richTextBox1.SelectionColor = Color.LightPink;
            }

            foreach (Match m in typeMatches)
            {
                this.richTextBox1.SelectionStart = m.Index;
                this.richTextBox1.SelectionLength = m.Length;
                this.richTextBox1.SelectionColor = Color.DarkCyan;
            }

            foreach (Match m in commentMatches)
            {
                this.richTextBox1.SelectionStart = m.Index;
                this.richTextBox1.SelectionLength = m.Length;
                this.richTextBox1.SelectionColor = Color.Green;
            }

            string strings = "\".+?\"";
            MatchCollection stringMatches = Regex.Matches(this.richTextBox1.Text, strings);

            foreach (Match m in stringMatches)
            {
                this.richTextBox1.SelectionStart = m.Index;
                this.richTextBox1.SelectionLength = m.Length;
                this.richTextBox1.SelectionColor = Color.OrangeRed;
            }

            this.richTextBox1.SelectionStart = originalIndex;
            this.richTextBox1.SelectionLength = originalLength;
            this.richTextBox1.SelectionColor = originalColor;

            this.richTextBox1.Focus();
        }

        private string checkValue(string input)
        {
            if (input == "BigNum")
            {
                return "1000000";
            }
            return input;
        }

        private void runMethod(string methodName, int address, string[] prsed, int count, int linecount)
        {
            writecons("Parser: Found " + methodName);
            if (prsed[count] == "=")
            {
                try
                {
                    funcs.ChangeAddressValue(pState, address, Int32.Parse(checkValue(prsed[count + 1])));
                    writecons("Phoenix | new " + methodName + " is: " + prsed[count + 1]);
                }
                catch (Exception)
                {
                    if (prsed.Length > count + 1)
                        if (prsed[count + 1] == "")
                            writecons("Phoenix | Value is nil");
                        else
                            writecons("Phoenix | Value is not integer / line " + linecount);
                    else
                        writecons("Phoenix | Value is nil");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (funcs.processName == "unhooked")
            {
                writecons("Phoenix | Not hooked to process!");
            }
            else
            {
                string input = richTextBox1.Text;
                string[] parsed = input.Split('\n');
                int linecount = 0;
                foreach (string line in parsed)
                {
                    linecount++;
                    string[] prsed = line.Split(' ');
                    int count = 0;
                    foreach (string word in prsed)
                    {
                        count++;
                        if (word.Contains("game.Players.LocalPlayer."))
                        {
                            writecons("Parser: Found LocalPlayer");
                            string[] plr = word.Split('.');
                            if (plr.Length > 3)
                                if (plr[3] == "setY")
                                {
                                    runMethod(plr[3], test_CordinationPTR + test_Ord_Y, prsed, count, linecount);
                                }
                                else if (plr[3] == "Height")
                                {
                                    runMethod(plr[3], PlrHeight, prsed, count, linecount);
                                }
                                else if (plr[3] == "NHeight")
                                {
                                    runMethod(plr[3], PlrNormalHeight, prsed, count, linecount);
                                }
                            if (plr.Length > 4)
                                if (plr[3] + "." + plr[4] == "Stats.Kills")
                                {
                                    runMethod(plr[3] + "." + plr[4], PlrKills, prsed, count, linecount);
                                }
                            if (plr.Length > 5)
                                if (plr[3] + "." + plr[4] + "." + plr[5] == "Character.Humanoid.Health")
                                {
                                    runMethod(plr[3] + "." + plr[4] + "." + plr[5], Health, prsed, count, linecount);
                                }
                                else if (plr[3] + "." + plr[4] + "." + plr[5] == "Character.Humanoid.Armor")
                                {
                                    runMethod(plr[3] + "." + plr[4] + "." + plr[5], Armor, prsed, count, linecount);
                                }
                                else if (plr[3] + "." + plr[4] + "." + plr[5] == "Character.Humanoid.WalkSpeed")
                                {
                                    runMethod(plr[3] + "." + plr[4] + "." + plr[5], Speed, prsed, count, linecount);
                                }
                                else if (plr[3] + "." + plr[4] + "." + plr[5] == "BackPack.PrimaryGun.Ammo")
                                {
                                    runMethod(plr[3] + "." + plr[4] + "." + plr[5], PrimaryAmmo, prsed, count, linecount);
                                }
                                else if (plr[3] + "." + plr[4] + "." + plr[5] == "BackPack.PrimaryGun.ReserveAmmo")
                                {
                                    runMethod(plr[3] + "." + plr[4] + "." + plr[5], PrimaryReserve, prsed, count, linecount);
                                }
                                else if (plr[3] + "." + plr[4] + "." + plr[5] == "BackPack.PrimaryGun.Delay")
                                {
                                    runMethod(plr[3] + "." + plr[4] + "." + plr[5], PrimaryDelay, prsed, count, linecount);
                                }
                                else if (plr[3] + "." + plr[4] + "." + plr[5] == "BackPack.SecondaryGun.Ammo")
                                {
                                    runMethod(plr[3] + "." + plr[4] + "." + plr[5], SecondaryAmmo, prsed, count, linecount);
                                }
                                else if (plr[3] + "." + plr[4] + "." + plr[5] == "BackPack.DualWield.Ammo")
                                {
                                    runMethod(plr[3] + "." + plr[4] + "." + plr[5], AkimboAmmo, prsed, count, linecount);
                                }
                                else if (plr[3] + "." + plr[4] + "." + plr[5] == "BackPack.Grenade.Ammo")
                                {
                                    runMethod(plr[3] + "." + plr[4] + "." + plr[5], Grenade, prsed, count, linecount);
                                }
                                else if (plr[3] + "." + plr[4] + "." + plr[5] == "BackPack.Grenade.Delay")
                                {
                                    runMethod(plr[3] + "." + plr[4] + "." + plr[5], GrenadeWaitingTime, prsed, count, linecount);
                                }
                                else if (plr[3] + "." + plr[4] + "." + plr[5] == "BackPack.GunSettings.Recoil")
                                {
                                    runMethod(plr[3] + "." + plr[4] + "." + plr[5], Recoil, prsed, count, linecount);
                                }
                                else if (plr[3] + "." + plr[4] + "." + plr[5] == "BackPack.DualWield.Enabled")
                                {
                                    if (prsed[count] == "=")
                                    {
                                        try
                                        {
                                            int cnvd = 0;
                                            if (Convert.ToBoolean(prsed[count + 1]) == true)
                                            {
                                                cnvd = 1;
                                            }
                                            else if (Convert.ToBoolean(prsed[count + 1]) == false)
                                            {
                                                cnvd = 0;
                                            }
                                            funcs.ChangeAddressValue(pState, Akimbo, cnvd);
                                            writecons("Phoenix | new " + plr[1] + " is: " + prsed[count + 1]);
                                        }
                                        catch (Exception)
                                        {
                                            if (prsed.Length > count + 1)
                                                if (prsed[count + 1] == "")
                                                    writecons("Phoenix | Value is nil");
                                                else
                                                    writecons("Phoenix | Value is not boolean / line " + linecount);
                                            else
                                                writecons("Phoenix | Value is nil");
                                        }
                                    }
                                }
                        }
                        else if (word == "print")
                        {
                            string nextth = prsed[count];
                            string sls = nextth;
                            string playercontextname = "game.Players.LocalPlayer.";
                            if (nextth == playercontextname + "Character.Humanoid.Health")
                            {
                                sls = sls.Replace(playercontextname, "");
                                writecons(sls + ": " + Convert.ToString(funcs.TryGetAddressValue(pState, Health)));
                            }
                            else if (nextth == playercontextname + "Character.Humanoid.Armor")
                            {
                                sls = sls.Replace(playercontextname, "");
                                writecons(sls + ": " + Convert.ToString(funcs.TryGetAddressValue(pState, Armor)));
                            }
                            else if (nextth == playercontextname + "Character.Humanoid.WalkSpeed")
                            {
                                sls = sls.Replace(playercontextname, "");
                                writecons(sls + ": " + Convert.ToString(funcs.TryGetAddressValue(pState, Speed)));
                            }
                            else if (nextth == playercontextname + "BackPack.PrimaryGun.Ammo")
                            {
                                sls = sls.Replace(playercontextname, "");
                                writecons(sls + ": " + Convert.ToString(funcs.TryGetAddressValue(pState, PrimaryAmmo)));
                            }
                            else if (nextth == playercontextname + "BackPack.SecondaryGun.Ammo")
                            {
                                sls = sls.Replace(playercontextname, "");
                                writecons(sls + ": " + Convert.ToString(funcs.TryGetAddressValue(pState, SecondaryAmmo)));
                            }
                            else if (nextth == playercontextname + "BackPack.DualWield.Ammo")
                            {
                                sls = sls.Replace(playercontextname, "");
                                writecons(sls + ": " + Convert.ToString(funcs.TryGetAddressValue(pState, AkimboAmmo)));
                            }
                            else if (nextth == playercontextname + "BackPack.Grenade.Ammo")
                            {
                                sls = sls.Replace(playercontextname, "");
                                writecons(sls + ": " + Convert.ToString(funcs.TryGetAddressValue(pState, Grenade)));
                            }
                            else if (nextth == playercontextname + "BackPack.GunSettings.Recoil")
                            {
                                sls = sls.Replace(playercontextname, "");
                                writecons(sls + ": " + Convert.ToString(funcs.TryGetAddressValue(pState, Recoil)));
                            }
                            else if (nextth == playercontextname + "localinplayers")
                            {
                                sls = sls.Replace(playercontextname, "");
                                writecons(sls + ": " + Convert.ToString(funcs.TryGetAddressValue(pState, PlrEntityPtr + LocalName)));
                            }
                            else if (nextth == playercontextname + "BackPack.DualWield.Enabled")
                            {
                                sls = sls.Replace(playercontextname, "");
                                string clvn = "false";
                                if (funcs.TryGetAddressValue(pState, Akimbo) == 1)
                                {
                                    clvn = "true";
                                }
                                else
                                {
                                    clvn = "false";
                                }
                                writecons(sls + ": " + clvn);
                            }
                        }
                    }

                    count = 0;
                }
                linecount = 0;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            hook();
        }

        private void hook()
        {
            Process[] exists = Process.GetProcessesByName("ac_client");
            if (exists.Length > 0)
            {
                funcs.processName = "ac_client";
                label2.Text = "Phoenix | Hooked | " + funcs.GetAddressValue(pState, LocalName);
            }
            else
            {
                writecons("Phoenix | Process not found!");
                label2.Text = "Phoenix";
            }
        }
        private void anon_hook()
        {
            Process[] exists = Process.GetProcessesByName("ac_client");
            if (exists.Length > 0)
            {
                funcs.processName = "ac_client";
                label2.Text = "Phoenix | Hooked | " + funcs.GetAddressValue(pState, LocalName);
            }
            else
            {
                funcs.processName = "unhooked";
                label2.Text = "Phoenix";
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void autohooker_Tick(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                anon_hook();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
