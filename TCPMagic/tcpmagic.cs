using ps4TCP;
using SwissKnife;
using SwissKnife.WinForms;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using TCPMagic.Properties;
using katana = SwissKnife.SwissKnife;

namespace TCPMagic {
    /// <summary>
    /// A TCP Class which can act as server or as client.
    /// </summary>
    public partial class TCPMagic : Form {
        #region vars
        /// <summary>
        /// A own PS4TCP instance for this GUI.
        /// </summary>
        private PS4TCP ps4TCP;

        /// <summary>
        /// A new Settings instance.
        /// </summary>
        private Settings settings;

        /// <summary>
        /// A thread for the drop aka payloader mode.
        /// </summary>
        private Thread socketThread;

        /// <summary>
        /// A ClipboardWatcehr Instance to check for DataChanged.
        /// </summary>
        private ClipboardWatcher clipboard;

        /// <summary>
        /// The Profile to Add to the list.
        /// </summary>
        private Profile toAdd;

        /// <summary>
        /// EventHandler Array for the RichTextBox ContextMenu.
        /// </summary>
        private EventHandler[] actions;

        /// <summary>
        /// Represents the RichTextBox Background Color.
        /// </summary>
        public static Color backCol = Color.Black;

        /// <summary>
        /// Represents the RichTextBox Foreground Color.
        /// </summary>
        public static Color foreCol = Color.Yellow;

        /// <summary>
        /// Represents the RichTextBox Error Color.
        /// </summary>
        public static Color errCol;

        /// <summary>
        /// Represents the RichTextBox Error Color.
        /// </summary>
        public static Color sockCol;

        /// <summary>
        /// Represents the RichTextBox Error Color.
        /// </summary>
        public static Color pcCol;

        /// <summary>
        /// Define a new Size for the input Box.
        /// </summary>
        private Size size = new Size(288, 148);

        /// <summary>
        /// Determine if a conenction was established.
        /// </summary>
        public static bool connected = false;

        /// <summary>
        /// Determine that a file from a drop was sended.
        /// </summary>
        public static bool sended = false;

        /// <summary>
        /// Determine if the Profile Selector instance is a load or a save one.
        /// </summary>
        public static bool load = false;

        /// <summary>
        /// A integer which represents the selected profile.
        /// </summary>
        public int profileSelected = 0;

        /// <summary>
        /// String array which is used to overload the arguments to the Ps4Tcp cast.
        /// </summary>
        private static string[] args;

        /// <summary>
        /// Ask before closing if a connection is open.
        /// </summary>
        private const string closeAsk = "There is currently a connection opened !\nAre you sure that you want to exit the program now ?";

        /// <summary>
        /// Define a Socket Name to use.
        /// </summary>
        private static string socketName = "PS4";

        /// <summary>
        /// About Message.
        /// </summary>
        private const string aboutMes = "TCPMagic is a simple but powerful Application.\n\nIt can be used as a simple TCP Sniffer and store data to a binary.\n\n" +
                                        "It is input able and just need a simple string trigger from the other site of the connection to read the users input from " +
                                        "the cml window.\n\nIt can use colores, clear the screen or a specific line or alter it, save and receive files and even\n" +
                                        "store them to names, define a encoding to use, drag and drop files to send, act as a clinet or a server, only connect to\n" +
                                        "send a file aka payloader mode, not tighten to any system it self.\n\nIcon by sandungas";

        /// <summary>
        /// Help Message.
        /// </summary>
        private const string help = "Log = Will store all messages into a log file.\n\n" +
                                    "Save2Bin = Stores the whole session to a .bin file.\n\n" +
                                    "No Connection Message = Will not print out a message that a connection could be established.\n\n" +
                                    "Socket Name = Define a Name to display for the connection to this computer.\n\n" +
                                    "Drop = Is the payloader Mode.\nWill run a connection as client and only send a file to the server, then close the connection after.\n" +
                                    "You can either define a file before you run the connection or you define it when a connection could be established.\n" +
                                    "On run time you can Drop your file to send into the Gui or you use the button and the application will do the rest.";

        /// <summary>
        /// InputBox message for a new profile.
        /// </summary>
        private const string mesName = "Enter a name for your Profile to save...";

        /// <summary>
        /// InputBox message for rename a profile.
        /// </summary>
        private const string mesNew = "Enter a new Profile name...";

        /// <summary>
        /// Label Text for the Input Box.
        /// </summary>
        private const string name = "Enter Name:";

        /// <summary>
        /// The Name of hte Form for hte InputBox.
        /// </summary>
        private const string form = "Profile Name";

        /// <summary>
        /// Profile Exists error message.
        /// </summary>
        private const string profExists = "The same Profile already exists !\nNo need to save a new one.";

        /// <summary>
        /// Profile Name Exists error message.
        /// </summary>
        private const string nameExists = "A Profile with the same Name already Exists !\nPlese choose a other Name.";

        /// <summary>
        /// Buffer to store the path to the file which shall be sendet to the other site. Aka payloader mode.
        /// </summary>
        private static string dropFile = string.Empty;
        #endregion Vars

        /// <summary>
        /// Instance Initializer.
        /// </summary>
        public TCPMagic() { InitializeComponent(); }

        #region TCPMagic Events
        /// <summary>
        /// On Load of this Form do.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void TCPMagic_Load(object sender, EventArgs e) {
            // Set MessagBox stuff.
            MessagBox.ListClicks = new EventHandler[] { ButtonAdd_Click, ButtonRemove_Click, ButtonRename_Click, ButtonShow_Click, ButtonUTP_Click, ListBox_SelectedIndexChanged, ListBox_DoubleClick, ProfileSelector_Load };
            MessagBox.TextBack = backCol;
            MessagBox.TextFore = foreCol;
            MessagBox.ListIcon = MessagBox.ProfileIcon;
            MessagBox.Size = size;

            // Set some default options.
            logMenuItem.Checked = formatMenuItem.Checked = true;

            // Set encoding to display within the combo box.
            comboEncoding.Items.Add("ASCII");
            comboEncoding.Items.Add("UTF8");
            comboEncoding.Items.Add("UTF7");
            comboEncoding.Items.Add("UTF32");
            comboEncoding.Items.Add("Unicode");
            comboEncoding.SelectedItem = "UTF8";

            // Initialize the PS4TCP handler instance.
            ps4TCP = new PS4TCP(rtb, this);

            //Initialize a new instance of the ClipboardWatcher class. By default it is set to watch for text.
            clipboard = new ClipboardWatcher(DataFormats.Text, false);
            clipboard.ContentPresent += Clipboard_ContentPresent;

            // Initialize the settings instance and laod up.
            settings = new Settings();
            LoadSettings();
            LoadColors();

            // Set RichTextBox depending variables.
            actions = new EventHandler[] { ClearContext_Click, CopyContext_Click, CutContext_Click, PasteContext_Click, SaveContext_Click, Save2BinContext_Click, SelectAllContext_Click, KillContext_Click };
            rtb.InitContextMenu(actions);
            rtb.ContextMenu.MenuItems[0].Enabled = rtb.ContextMenu.MenuItems[6].Enabled = rtb.ContextMenu.MenuItems[7].Enabled = killMenuItem.Enabled = false;
            rtb.ContextMenu.MenuItems[1].Enabled = rtb.ContextMenu.MenuItems[2].Enabled = rtb.ContextMenu.MenuItems[4].Enabled = rtb.ContextMenu.MenuItems[5].Enabled = false;
            rtb.SelectionChanged += Rtb_SelectionChanged;
            checkServer.Checked = true;
            rtb.ReadOnly = false;
        }
        
        /// <summary>
        /// On Button Click.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void ButtonRun_Click(object sender, EventArgs e) {
            // IP and Port are valid ?
            if (!CheckIP() || !CheckPort()) return;

            // Ensure that output and input as well all other events are relfected while we are not done here.
            Application.DoEvents();

            // Disable certain Gui functions while there is a process running.
            DisableGui();
            
            // Only Client Drop aka Payloader mode ?
            if (!checkServer.Checked && checkDrop.Checked) {
                try {
                    // Set args, initialize a clean new instance and run the client in a thread.
                    SetArgs();
                    args[3] = "client";
                    ps4TCP = new PS4TCP(rtb, this, logMenuItem.Checked, save2BinMenuItem.Checked, noCMsgMenuItem.Checked, formatMenuItem.Checked, errCol.GetName(), sockCol.GetName(),
                                        pcCol.GetName(), socketName, backCol.GetName(), foreCol.GetName());

                    // Run The socket in a Thread and activate certain Menu Items.
                    RunSocketThread();
                    rtb.ContextMenu.MenuItems[7].Enabled = killMenuItem.Enabled = true;

                    while (!connected) { /* Nothing to do here, just wait for the client to be connected. */ }

                    // Do we have a drop file defined ?
                    if (dropFile != string.Empty) {
                        ps4TCP.SendFileFromDrop(dropFile);
                        dropFile = string.Empty;
                    }

                    while (!sended) { /* Nothing to do here, just waiting for the file to be sended, if not already. */ }
                    ps4TCP.Exit();
                    EnableGui();
                    return;
                } catch (Exception ex) { MessagBox.Error("An Error Ocured !\n\n" + ex.ToString()); return; }
            } else {
                // Run Server or Client.
                try {
                    SetArgs();
                    if (checkServer.Checked) args[3] = "server";
                    else args[3] = "client";
                    ps4TCP = new PS4TCP(rtb, this, logMenuItem.Checked, save2BinMenuItem.Checked, noCMsgMenuItem.Checked, formatMenuItem.Checked, errCol.GetName(), sockCol.GetName(),
                                        pcCol.GetName(), socketName, backCol.GetName(), foreCol.GetName());

                    RunSocketThread();
                    rtb.ContextMenu.MenuItems[7].Enabled = killMenuItem.Enabled = true;
                }
                catch (Exception ex) { MessagBox.Error("An Error Ocured !\n\n" + ex.ToString()); }
            }
        }

        /// <summary>
        /// On Button Select File click.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void ButtonSelFile_Click(object sender, EventArgs e) {
            string initDir = string.Empty;
            if (settings.LastPath != string.Empty) initDir = settings.LastPath;
            else initDir = Directory.GetCurrentDirectory();
            string result = MessagBox.ShowOpenFile("Select File to send", "All Files (*.*)|*.*;", "some file...", initDir);
            if (result != string.Empty) {
                if (connected) ps4TCP.SendFileFromDrop(result);
                else dropFile = result;
            }
        }

        /// <summary>
        /// On Button Click.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void TCPMagic_DragEnter(object sender, DragEventArgs e) {
            DragDropEffects result = DragDropEffects.None;
            if (connected) {
                if (e.Data.GetDataPresent(DataFormats.FileDrop, false)) {
                    string[] file = (string[])e.Data.GetData(DataFormats.FileDrop, false);
                    if (file.Length == 1) e.Effect = DragDropEffects.Copy;
                }
            }
            e.Effect = result;
        }

        /// <summary>
        /// On Button Click.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void TCPMagic_DragDrop(object sender, DragEventArgs e) {
            string[] file = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (MessagBox.Question("Send File", "Are you sure to send this file ?\n{0}", file[0].GetName()) == DialogResult.OK) ps4TCP.SendFileFromDrop(file[0]);
        }

        /// <summary>
        /// On Clipboard ContentChanged do.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        public void Clipboard_ContentPresent(object sender, AppEventArgs e) {
            if (e.DataPresent) rtb.ContextMenu.MenuItems[3].Enabled = true;
            else rtb.ContextMenu.MenuItems[3].Enabled = false;
        }

        /// <summary>
        /// On RichTextBox Key pressed down do.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void Rtb_KeyDown(object sender, KeyEventArgs e) {
            if (RichTextBoxExtension.readKey && e.KeyCode == Keys.Enter) {
                rtb.SelectionStart = RichTextBoxExtension.indexer;
                rtb.SelectionLength = rtb.TextLength - RichTextBoxExtension.indexer;
                RichTextBoxExtension.readLine = rtb.SelectedText;
                RichTextBoxExtension.keyReaded = true;
            }
        }

        /// <summary>
        /// On RichTextBox SelectionChanged do.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void Rtb_SelectionChanged(object sender, EventArgs e) {
            if (!rtb.ReadOnly) {
                if (rtb.SelectionLength > 0) {
                    rtb.ContextMenu.MenuItems[1].Enabled = rtb.ContextMenu.MenuItems[2].Enabled = rtb.ContextMenu.MenuItems[4].Enabled = rtb.ContextMenu.MenuItems[5].Enabled = true;
                    return;
                }
            }
            rtb.ContextMenu.MenuItems[1].Enabled = rtb.ContextMenu.MenuItems[2].Enabled = rtb.ContextMenu.MenuItems[4].Enabled = rtb.ContextMenu.MenuItems[5].Enabled = false;
        }

        /// <summary>
        /// On RichTextBox TextChanged do.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void Rtb_TextChanged(object sender, EventArgs e) {
            if (rtb.Text.Length > 0) {
                if (!rtb.ReadOnly) rtb.ContextMenu.MenuItems[6].Enabled = true;
                else rtb.ContextMenu.MenuItems[6].Enabled = false;
                rtb.ContextMenu.MenuItems[0].Enabled = true;
                rtb.SelectionStart = rtb.Text.Length;
                rtb.ScrollToCaret();
            } else rtb.ContextMenu.MenuItems[0].Enabled = rtb.ContextMenu.MenuItems[6].Enabled = false;
        }

        /// <summary>
        /// On RichtTextBox ReadOnly changed do.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void Rtb_ReadOnlyChanged(object sender, EventArgs e) {
            if (rtb.ReadOnly) clipboard.StopWatching();
            else clipboard.StartWatching();
        }

        /// <summary>
        /// On click of checkbox serv er do.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void CheckServer_CheckedChanged(object sender, EventArgs e) {
            if (checkServer.Checked) checkDrop.Enabled = buttonSelFile.Enabled = false;
            else checkDrop.Enabled = true;
            LoadSettings();
        }

        /// <summary>
        /// On ComboBox IP SelectedText changed do.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void ComboIP_TextChanged(object sender, EventArgs e) {
            if (comboPort.Text.Length <= 5 && comboIP.Text.Length <= 15) {
                if (comboIP.Text != string.Empty && comboPort.Text != string.Empty) {
                    if (checkDrop.Checked && !checkServer.Checked) buttonSelFile.Enabled = true;
                }
            } else buttonSelFile.Enabled = false;
        }

        /// <summary>
        /// On ComboBox Port SelectedText changed do.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void ComboPort_TextChanged(object sender, EventArgs e) {
            if (comboPort.Text.Length <= 5 && comboIP.Text.Length <= 15) {
                if (comboIP.Text != string.Empty && comboPort.Text != string.Empty) {
                    if (checkDrop.Checked && !checkServer.Checked) buttonSelFile.Enabled = true;
                }
            } else buttonSelFile.Enabled = false;
        }

        /// <summary>
        /// On CheckBox Drop CheckedChanged do.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void CheckDrop_CheckedChanged(object sender, EventArgs e) {
            if (comboPort.Text.Length <= 5 && comboIP.Text.Length <= 15) {
                if (comboIP.Text != string.Empty && comboPort.Text != string.Empty) {
                    if (checkDrop.Checked && !checkServer.Checked) buttonSelFile.Enabled = true;
                }
            } else buttonSelFile.Enabled = false;
        }

        /// <summary>
        /// On button Select Profile click.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void ButtonSelectProfile_Click(object sender, EventArgs e) {
            load = true;
            if (MessagBox.List("Use this Profile", "Profile Selector") == DialogResult.OK) LoadProfile(profileSelected);
        }

        /// <summary>
        /// On button Save Profile click.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void ButtonSaveProfile_Click(object sender, EventArgs e) {
            if (!CheckIP() || !CheckPort()) return;
            load = false;
            
            toAdd = new Profile();
            toAdd.ProfileName = string.Empty;
            toAdd.IP = comboIP.Text;
            toAdd.Port = comboPort.Text;
            toAdd.Encoding = comboEncoding.Text;
            toAdd.Server = checkServer.Checked;
            toAdd.Drop = checkDrop.Checked;
            toAdd.Log = logMenuItem.Checked;
            toAdd.BinDmp = save2BinMenuItem.Checked;
            toAdd.format = formatMenuItem.Checked;
            toAdd.nCMdg = noCMsgMenuItem.Checked;
            toAdd.sockName = socketName;
            toAdd.font = rtb.Font.ToString() + "Style=" + rtb.Font.Style.ToString();
            toAdd.backCol = backCol.GetName();
            toAdd.foreCol = foreCol.GetName();
            toAdd.errCol = errCol.GetName();
            toAdd.sockCol = sockCol.GetName();
            toAdd.pcCol = pcCol.GetName();
            
            if (MessagBox.List("Use this Profile", "Profile Selector") == DialogResult.OK) LoadProfile(profileSelected);
        }

        /// <summary>
        /// On Button Click.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void ExitMenuItem_Click(object sender, EventArgs e) { Close(); }

        /// <summary>
        /// On Reset Menu Item click do.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void ResetMenuItem_Click(object sender, EventArgs e) {
            if (MessagBox.Question("Reset local Variables", "This will delete all stored IPs, Ports and Profiles as well.\nAre you sure ?") == DialogResult.OK) {
                settings.Reset();
                settings.DoIt();
            }
        }

        /// <summary>
        /// On ContextMenu Kill click do.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void KillMenuItem_Click(object sender, EventArgs e) { rtb.ContextMenu.MenuItems[7].PerformClick(); }

        /// <summary>
        /// On Form closing do.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void TCPMagic_FormClosing(object sender, FormClosingEventArgs e) {
            if (connected) {
                if (MessagBox.Question("Close Connection", closeAsk) == DialogResult.OK) ps4TCP.Exit();
                if (socketThread != null && socketThread.IsAlive) socketThread.Abort();
            }
        }

        /// <summary>
        /// On Menu Item Option Log Click.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void LogMenuItem_Click(object sender, EventArgs e) {
            if (logMenuItem.Checked) logMenuItem.Checked = false;
            else logMenuItem.Checked = true;
        }

        /// <summary>
        /// On Menu Item Option Save2Bin Click.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void Save2BinMenuItem_Click(object sender, EventArgs e) {
            if (save2BinMenuItem.Checked) save2BinMenuItem.Checked = false;
            else save2BinMenuItem.Checked = true;
        }

        /// <summary>
        /// On Menu Item Option Format Click.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void FormatMenuItem_Click(object sender, EventArgs e) {
            if (formatMenuItem.Checked) formatMenuItem.Checked = false;
            else formatMenuItem.Checked = true;
        }

        /// <summary>
        /// On Menu Item Option CheckedChanged.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void FormatMenuItem_CheckedChanged(object sender, EventArgs e) {
            if (formatMenuItem.Checked) formatMenuItem.Text = "Format On";
            else formatMenuItem.Text = "Format Off";
        }

        /// <summary>
        /// On Menu Item No Connect Message click do.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void NoCMsgMenuItem_Click(object sender, EventArgs e) {
            if (noCMsgMenuItem.Checked) noCMsgMenuItem.Checked = false;
            else noCMsgMenuItem.Checked = true;
        }

        /// <summary>
        /// On Menu Item Socket Name click do.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void SocketNameMenuItem_Click(object sender, EventArgs e) { if (MessagBox.Input(socketName, "Enter new Name:", "Socket Name") == DialogResult.OK) socketName = MessagBox.UsrInput; }

        /// <summary>
        /// On Menu Item Color click do.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void ColorsMenuItem_Click(object sender, EventArgs e) {
            Color[] result = MessagBox.ColorDialog(new string[] { "Back Color", "Fore Color", "Error Color", "Sock Color", "PC Color" },
                                                   new Color[] { backCol, foreCol, errCol, sockCol, pcCol },
                                                   new bool[] { true, false, false, false, false,});
            if (result.Length > 0) {
                settings.BackCol = result[0];
                if (result.Length >= 2) settings.ForeCol = result[1];
                if (result.Length >= 3) settings.ErrCol = result[2];
                if (result.Length >= 4) settings.SockCol = result[3];
                if (result.Length >= 5) settings.PcCol = result[4];
                settings.DoIt();
                LoadColors();
            }
        }

        /// <summary>
        /// On MenuItem Font click do.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void FontMenuItem_Click(object sender, EventArgs e) {
            FontDialog font = new FontDialog() { Font = rtb.Font };
            if (font.ShowDialog() == DialogResult.OK) rtb.Font = font.Font;
        }

        /// <summary>
        /// On Button Click.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void HelpMenuItem_Click(object sender, EventArgs e) { MessagBox.Show("Help", help); }

        /// <summary>
        /// On Button Click.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void AboutMenuItem_Click(object sender, EventArgs e) { MessagBox.Info("About", aboutMes); }

        /// <summary>
        /// On menu item register click do.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void RegisterMenuItem_Click(object sender, EventArgs e) { MessagBox.Warning("LOL XD\nNot really mate,...not with me. ^^"); }

        /// <summary>
        /// On RichTextBox ContextMenu Clear click do.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void ClearContext_Click(object sender, EventArgs e) { rtb.ClearInvoke(); }

        /// <summary>
        /// On RichTextBox ContextMenu Copy click do.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void CopyContext_Click(object sender, EventArgs e) { rtb.CopyInvoke(); }

        /// <summary>
        /// On RichTextBox ContextMenu Cut click do.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void CutContext_Click(object sender, EventArgs e) { rtb.CutInvoke(); }

        /// <summary>
        /// On RichTextBox ContextMenu Paste click do.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void PasteContext_Click(object sender, EventArgs e) { rtb.PasteInvoke(); }

        /// <summary>
        /// On RichTextBox ContextMenu Save click do.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void SaveContext_Click(object sender, EventArgs e) { rtb.Save(); }

        /// <summary>
        /// On RichTextBox ContextMenu Save2Bin click do.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void Save2BinContext_Click(object sender, EventArgs e) { rtb.SaveBin(); }

        /// <summary>
        /// On RichTextBox ContextMenu SelectAll click do.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void SelectAllContext_Click(object sender, EventArgs e) { rtb.SelectAllInvoke(); }

        /// <summary>
        /// On RichTextBox ContextMenu Kill click do.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void KillContext_Click(object sender, EventArgs e) {
            ps4TCP.Exit();
            if (socketThread != null && socketThread.IsAlive) socketThread.Abort();

            buttonRun.Text = "Run";
            PS4TCP.FireOnWriteLineEvent(new AppEventArgs(""));
            PS4TCP.FireOnWriteLineEvent(new AppEventArgs("[PC]: Process Killed."));
            EnableGui();
            rtb.ContextMenu.MenuItems[7].Enabled = killMenuItem.Enabled = false;
        }
        #endregion TCPMagic Events

        #region ProfileEvents
        /// <summary>
        /// On Load of Form do.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void ProfileSelector_Load(object sender, EventArgs e) {
            MessagBox.buttonOKList.Enabled = MessagBox.buttonRemove.Enabled = MessagBox.buttonRename.Enabled = MessagBox.buttonShow.Enabled = false;     // Disable the Button Use this Profile.

            if (settings.Profiles.Count == 0) {
                Profile none = new Profile() { ProfileName = "None" };
                settings.Profiles.Add(none);
                settings.DoIt();
            }
            foreach (Profile prof in settings.Profiles) MessagBox.listBox.Items.Add(prof.ProfileName);

            MessagBox.listBox.BackColor = backCol;                                        // Set Background Color of the listBox.
            MessagBox.listBox.ForeColor = foreCol;                                        // Set Foreground Color of the listBox.
            if (!load) MessagBox.buttonAdd.Enabled = true;                                // Is this a Save Instance ?
            else MessagBox.buttonAdd.Enabled = false;
        }

        /// <summary>
        /// On Button Use This Profile Clicked do.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void ButtonUTP_Click(object sender, EventArgs e) {
            profileSelected = MessagBox.listBox.SelectedIndex;
            MessagBox.list.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// On listBox Index Double Clicked do.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void ListBox_DoubleClick(object sender, EventArgs e) {
            if (MessagBox.listBox.SelectedIndex > 0) MessagBox.buttonOKList.PerformClick();
            else Close();
        }

        /// <summary>
        /// On listBox Selected Index Changed do.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void ListBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (MessagBox.listBox.SelectedIndex > 0) MessagBox.buttonOKList.Enabled = MessagBox.buttonRemove.Enabled = MessagBox.buttonRename.Enabled = MessagBox.buttonShow.Enabled = true;
            else MessagBox.buttonOKList.Enabled = MessagBox.buttonRemove.Enabled = MessagBox.buttonRename.Enabled = MessagBox.buttonShow.Enabled = false;
        }

        /// <summary>
        /// Add a new Profile to the list.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void ButtonAdd_Click(object sender, EventArgs e) {
            bool profOk = false;
            while (true) {
                if (MessagBox.Input(mesName, name, form) == DialogResult.OK) {
                    toAdd.ProfileName = MessagBox.UsrInput;

                    if (!profOk && !CheckProfile(toAdd)) { MessagBox.Error(profExists); MessagBox.buttonAdd.Enabled = false; return; }
                    else if (!CheckProfileName(toAdd.ProfileName)) { MessagBox.Error(nameExists); profOk = true; }
                    else {
                        settings.Profiles.Add(toAdd);
                        settings.DoIt();
                        LoadListBox();
                        MessagBox.buttonAdd.Enabled = false;
                        break;
                    }
                } else break;
            }
        }

        /// <summary>
        /// Remove a Profile from the list.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void ButtonRemove_Click(object sender, EventArgs e) {
            if (MessagBox.Question("Remove Profile", "Are you sure to remove the selected Profile from the list ?") == DialogResult.OK) {
                settings.Profiles.RemoveAt(MessagBox.listBox.SelectedIndex);
                settings.DoIt();
                LoadListBox();
            }
        }

        /// <summary>
        /// Rename a Profile from the list.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void ButtonRename_Click(object sender, EventArgs e) {
            if (MessagBox.Input(name, mesNew, "Rename") == DialogResult.OK) {
                settings.Profiles[MessagBox.listBox.SelectedIndex].ProfileName = MessagBox.UsrInput;
                settings.DoIt();
                LoadListBox();
            }
        }

        /// <summary>
        /// Show Profile informations.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private void ButtonShow_Click(object sender, EventArgs e) {
            int index = MessagBox.listBox.SelectedIndex;
            string info = string.Empty;
            info += "Name  ==  " + settings.Profiles[index].ProfileName;
            info += "\nIP  ==  " + settings.Profiles[index].IP;
            info += "\nPort  ==  " + settings.Profiles[index].Port;
            info += "\nEncoding  ==  " + settings.Profiles[index].Encoding;
            info += "\nServer  ==  " + settings.Profiles[index].Server.ToString();
            info += "\nDrop  ==  " + settings.Profiles[index].Drop.ToString();
            info += "\nLog  ==  " + settings.Profiles[index].Log.ToString();
            info += "\nSave 2 Bin  ==  " + settings.Profiles[index].BinDmp.ToString();
            info += "\nFormatting  ==  " + settings.Profiles[index].format.ToString();
            info += "\nConnect Msg  ==  " + settings.Profiles[index].nCMdg.ToString();
            info += "\nSocket Name  ==  " + settings.Profiles[index].sockName;
            info += "\nFont  ==  " + settings.Profiles[index].font.FontName();
            info += "\nTxt Back Col  ==  " + settings.Profiles[index].backCol;
            info += "\nTxt Fore Col  ==  " + settings.Profiles[index].foreCol;
            info += "\nTxt Error Col  ==  " + settings.Profiles[index].errCol;
            info += "\nSock Name Col  ==  " + settings.Profiles[index].sockCol;
            info += "\nPC Name Col  ==  " + settings.Profiles[index].pcCol;
            MessagBox.Info("Profile Info", info);
        }

        /// <summary>
        /// Clear the ListBox and reload it.
        /// </summary>
        private void LoadListBox() {
            MessagBox.listBox.Items.Clear();
            foreach (Profile prof in settings.Profiles) MessagBox.listBox.Items.Add(prof.ProfileName);
        }

        /// <summary>
        /// Check if the same profile already exists.
        /// </summary>
        /// <param name="newProf">The new profile which shall be added and which we shall check for existens.</param>
        /// <returns>True if the Profile do not already exists in the List of stored Profiles, else false.</returns>
        private bool CheckProfile(Profile newProf) {
            foreach (Profile prof in settings.Profiles) if (prof.Equals(newProf)) return false;
            return true;
        }

        /// <summary>
        /// Check if a profile with the same name already exists.
        /// </summary>
        /// <param name="newProfName">The Name of the new Profile to add and to check for existens.</param>
        /// <returns>Ture if the name do not already exists, else false.</returns>
        private bool CheckProfileName(string newProfName) {
            foreach (Profile prof in settings.Profiles) if (prof.ProfileName.Equals(newProfName)) return false;
            return true;
        }
        #endregion ProfileEvents

        #region TCPMagic Casts
        /// <summary>
        /// Run the Socket Thread.
        /// </summary>
        private void RunSocketThread() {
            socketThread = new Thread(RunSock) { IsBackground = true };
            socketThread.SetApartmentState(ApartmentState.STA);
            socketThread.Start();
        }

        /// <summary>
        /// Set the arguments for the tcp server/client loop.
        /// </summary>
        private void SetArgs() {
            args = new string[4];
            args[0] = "-i" + comboIP.SelectedItem.ToString();
            if (comboPort.SelectedItem.ToString() != string.Empty) args[1] = "-p" + comboPort.SelectedItem.ToString();
            else args[1] = "-p" + comboPort.SelectedText;
            args[2] = comboEncoding.SelectedItem.ToString();
        }

        /// <summary>
        /// Get Local IPs.
        /// </summary>
        private void GetIP() {
            string[] ips = katana.GetLocalIPAddress();
            comboIP.Items.AddRange(ips);
            comboIP.SelectedIndex = 0;
        }

        /// <summary>
        /// Checks IP format.
        /// </summary>
        /// <returns>True if string is in IP format and in a valid range, else false.</returns>
        private bool CheckIP() {
            bool res = false;
            if (comboIP.Text.Length > 0) {
                IPAddress result;
                if (!IPAddress.TryParse(comboIP.Text, out result)) MessagBox.Error("This is not a valid IP Address !");
                else {
                    if (!checkServer.Checked && !settings.ClientIPs.Contains(comboIP.Text)) {
                        settings.ClientIPs.Add(comboIP.Text);
                        settings.DoIt();
                    }
                    res = true;
                }
                
            } else MessagBox.Error("IP is Empty !");
            return res;
        }

        /// <summary>
        /// Checks Port format.
        /// </summary>
        /// <returns>True if string is in Port format and in a valid range, else false.</returns>
        private bool CheckPort() {
            bool res = false;
            ushort result;
            if (comboPort.Text.Length > 0) {
                if (!Port.TryParse(comboPort.Text, out result)) MessagBox.Error("This is not a valid Port Number !");
                else {
                    if (!settings.Ports.Contains(comboPort.Text)) {
                        settings.Ports.Add(comboPort.Text);
                        settings.DoIt();
                    }
                    res = true;
                }
            } else MessagBox.Error("Port is Empty !");
            return res;
        }

        /// <summary>
        /// Thread which is used for the drop aka payloader mode.
        /// </summary>
        private void RunSock() { ps4TCP.Ps4Tcp(args); }

        /// <summary>
        /// Disable certain Gui functions while there is a connection open.
        /// </summary>
        private void DisableGui() {
            comboIP.Enabled = comboPort.Enabled = comboEncoding.Enabled = checkServer.Enabled = buttonRun.Enabled = buttonSaveProfile.Enabled = buttonSelectProfile.Enabled = false;
            optionsMenuItem.Enabled = false;
            if (!checkServer.Checked) checkDrop.Enabled = false;
        }

        /// <summary>
        /// Enable certain Gui functions after a connection was closed.
        /// </summary>
        private void EnableGui() {
            comboIP.Enabled = comboPort.Enabled = comboEncoding.Enabled = checkServer.Enabled = buttonRun.Enabled = buttonSaveProfile.Enabled = buttonSelectProfile.Enabled = true;
            optionsMenuItem.Enabled = true;
            if (!checkServer.Checked) checkDrop.Enabled = true;
        }        
        
        /// <summary>
        /// Clear Combobox IP.
        /// </summary>
        private void ClearIP() { comboIP.Items.Clear(); comboIP.ResetText(); }

        /// <summary>
        /// Clear Combobox Port.
        /// </summary>
        private void ClearPort() { comboPort.Items.Clear(); comboPort.ResetText(); }

        /// <summary>
        /// Load a Profile into the Gui.
        /// </summary>
        /// <param name="index">The index of the Profile to use within the Profile list.</param>
        private void LoadProfile(int index) {
            Profile load = settings.Profiles[index];
            comboIP.SelectedItem = load.IP;
            comboPort.SelectedItem = load.Port;
            comboEncoding.SelectedItem = load.Encoding;
            checkServer.Checked = load.Server;
            checkDrop.Checked = load.Drop;
            logMenuItem.Checked = load.Log;
            save2BinMenuItem.Checked = load.BinDmp;
            formatMenuItem.Checked = load.format;
            noCMsgMenuItem.Checked = load.nCMdg;
            socketName = load.sockName;
            rtb.Font = new Font(load.font.FontName(), load.font.FontSize(), load.font.FontStyleFont(), load.font.FontUnits(), load.font.FontCharSet(), load.font.FontVertical());
            settings.BackCol = Color.FromName(load.backCol);
            settings.ForeCol = Color.FromName(load.foreCol);
            settings.ErrCol = Color.FromName(load.errCol);
            settings.SockCol = Color.FromName(load.sockCol);
            settings.PcCol = Color.FromName(load.pcCol);
            settings.DoIt();
            LoadColors();
        }

        /// <summary>
        /// Load stored IPs and Ports.
        /// </summary>
        private void LoadSettings() {
            // Add stored ports to the combo box.
            ClearPort();
            if (settings.Ports.Count > 0) {
                string[] toAdd = new string[settings.Ports.Count];
                settings.Ports.CopyTo(toAdd, 0);
                comboPort.Items.AddRange(toAdd);
                comboPort.SelectedItem = comboPort.Items[0];
            }

            // If we are in client mode.
            ClearIP();
            if (!checkServer.Checked) {
                // Add stored IPs to the combo box.
                if (settings.ClientIPs.Count > 0) {
                    string[] toAdd = new string[settings.ClientIPs.Count];
                    settings.ClientIPs.CopyTo(toAdd, 0);
                    comboIP.Items.AddRange(toAdd);
                    comboIP.SelectedItem = comboIP.Items[0];
                } // Else when server mode, get local ips.
            } else GetIP();
        }

        /// <summary>
        /// Load Colors from the settings and set them.
        /// </summary>
        private void LoadColors() {
            backCol = settings.BackCol;
            foreCol = settings.ForeCol;
            errCol = settings.ErrCol;
            sockCol = settings.SockCol;
            pcCol = settings.PcCol;
            MessagBox.TextBack = backCol;
            MessagBox.TextFore = foreCol;
        }
        #endregion TCPMagic Casts
    }
}
