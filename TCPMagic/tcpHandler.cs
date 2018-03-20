/*
 *  https://msdn.microsoft.com/de-de/library/system.net.sockets.tcplistener(v=vs.110).aspx
 *
 *
 *  Note_#1:  You could also use server.AcceptSocket().
 *  Note_#2:  The [Read() != 0] keeps the stream open, till the connection from the client would be closed.
 */

using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SwissKnife;
using SwissKnife.WinForms;
using TCPMagic;
using Gui = TCPMagic.TCPMagic;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ps4TCP {
    /// <summary>
    /// Receiving data Structure.
    /// </summary>
    public struct RecvBytes {
        /// <summary>
        /// The Data it self as byte representation.
        /// </summary>
        public byte[] Data;

        /// <summary>
        /// The size of the received data.
        /// </summary>
        public int Size;
    }

    public class PS4TCP {
        #region Vars
        /// <summary>
        /// A Counter to display in the console for user wait.
        /// </summary>
        private int COUNT = 0;

        /// <summary>
        /// The Port to communicate over.
        /// </summary>
        private Int32 PORT;

        /// <summary>
        /// Define to use formatting for the output or not.
        /// </summary>
        public bool formatting = true;

        /// <summary>
        /// Used to tell the server which encoding he shall use for the client.
        /// </summary>
        private Encoding encode;

        /// <summary>
        /// Determine from the Gui instance if we shall exit the loop
        /// </summary>
        private bool exit = false;

        /// <summary>
        /// Flag to jump into some specific port dumb routines.
        /// </summary>
        public bool binData = false;

        /// <summary>
        /// Determine if this connection is a browser debug session so the server won't ask for reconnection and also won't print out information on each new dis and re connect.
        /// </summary>
        public bool noConnectMsg = false;

        /// <summary>
        /// Determine if we shall use a log or not.
        /// </summary>
        public bool log = false;

        /// <summary>
        /// Determine if this process shall be a server or a client.
        /// </summary>
        private bool useServer = false;
        
        /// <summary>
        /// Variable used to store the custom color for the fore ground.
        /// </summary>
        public string foreCol;
        
        /// <summary>
        /// Variable used to store the custom color for the back ground.
        /// </summary>
        public string backCol;
        
        /// <summary>
        /// Variable used to store the custom color for the error.
        /// </summary>
        public string errCol;

        /// <summary>
        /// Variable used to store the custom color for the socket string.
        /// </summary>
        public string sockCol;

        /// <summary>
        /// Variable used to store the custom color for the PC string.
        /// </summary>
        public string pcCol;

        /// <summary>
        /// A Variable for string comparison which holds a integer flag for the options.
        /// The options is: Invariant Cultur, Ignore Case. (InvariantCultureIgnoreCase)
        /// </summary>
        private StringComparison icic = StringComparison.InvariantCultureIgnoreCase;

        /// <summary>
        /// Our IP wher we will run the server on our local host.
        /// </summary>
        private IPAddress localHost;

        /// <summary>
        /// The TCP listener server.
        /// </summary>
        private TcpListener server;

        /// <summary>
        /// The TCP Client.
        /// </summary>
        private TcpClient client;

        /// <summary>
        /// A Network Stream to pipe the client/server stream into.
        /// </summary>
        private NetworkStream stream;

        /// <summary>
        /// A thread which we use to count so we can break user input.
        /// </summary>
        private Thread counter;

        /// <summary>
        /// The RichTextBox to write out.
        /// </summary>
        private RichTextBox rtb;

        /// <summary>
        /// The control to use and wrie into.
        /// </summary>
        private Control control;

        /// <summary>
        /// Representing the position of the cursor within the text field.
        /// </summary>
        private int cursorLeft, cursorTop;

        /// <summary>
        /// The current directory, to resolve files and more.
        /// </summary>
        public static string currDir = Directory.GetCurrentDirectory();

        /// <summary>
        /// Set dir where to store reveived files.
        /// </summary>
        public static string recvDir = currDir + @"\received\";

        /// <summary>
        /// A string for our binary TCP dump file.
        /// </summary>
        private string binFile = recvDir + "portdump.bin";

        /// <summary>
        /// An output buffer. We use that to store some strings to the beginn of the initialization process till we can write into the log. That we we can write into the console-
        /// and the the log file to same time with one function.
        /// </summary>
        private string outbuff = string.Empty;

        /// <summary>
        /// Resolve the applications run time path and generate a string, based on that path, for the logger with included and trimmed date and time.
        /// </summary>
        public string useLog = string.Empty;

        /// <summary>
        /// String variable for the foreground string.
        /// </summary>
        private const string fg = "foreground=";

        /// <summary>
        /// String variable for the background string.
        /// </summary>
        private const string bg = "background=";

        /// <summary>
        /// String variable for the error string.
        /// </summary>
        private const string er = "error=";

        /// <summary>
        /// String variable for the encoding ascii string.
        /// </summary>
        private const string _ascii = "ascii";

        /// <summary>
        /// String variable for the encoding utf8 string.
        /// </summary>
        private const string _utf8 = "utf8";

        /// <summary>
        /// String variable for the encoding utf16 string.
        /// </summary>
        private const string _utf16 = "utf16";

        /// <summary>
        /// String variable for the encoding unicode string.
        /// </summary>
        private const string _unicode = "unicode";

        /// <summary>
        /// String variable for the encoding utf7 string.
        /// </summary>
        private const string _utf7 = "utf7";

        /// <summary>
        /// String variable for the encoding utf32 string.
        /// </summary>
        private const string _utf32 = "utf32";

        /// <summary>
        /// Add a string to our output, make it nice.
        /// </summary>
        private string PC = "[PC]:";

        /// <summary>
        /// Add a string to our output, make it nice.
        /// </summary>
        private string PCE = "[PC][ERROR]:";

        /// <summary>
        /// A test string which we will encode with the client's format encoding.
        /// </summary>
        private const string test = "cfwprphtcfwprphtThis_Is_A_Test_Xcfwprphtcfwprpht";

        /// <summary>
        /// Flag which is used to tell us to clear the screen.
        /// </summary>
        private const string CLS = "CLEARSCREEN";

        /// <summary>
        /// Flag to trigger a line, string or a single char deletetion.
        /// </summary>
        private const string CLL = "CLEARLINE";

        /// <summary>
        /// Flag to request line cursor.
        /// </summary>
        private const string GCRS = "GETCURSOR";

        /// <summary>
        /// Flag to trigger a line altere.
        /// </summary>
        private const string AL = "ALTERELINE";

        /// <summary>
        /// With that one the client can tell the server that he shall use this and that custom color for fore and back ground.
        /// </summary>
        private const string CCOL = "CUSTOM_COLOR";

        /// <summary>
        /// This flag is used to tell us (or the server :P ) that the application on the ps4 is requesting a file from the computer.
        /// </summary>
        private const string FGREQ = "host://GETFILE";

        /// <summary>
        /// This flag is used to tell us (or the server :P ) that the application on the ps4 wants to send a file to this computer.
        /// </summary>
        private const string FSREQ = "host://PUTFILE";

        /// <summary>
        /// String to display when we ask for a new connection.
        /// </summary>
        private const string askReCon = "{0}Connection closed.\n{0}Do you want to wait for a new connection ?\n{0}Will automaticly re-listen in {1}\n";

        /// <summary>
        /// String to display when the counter hase runned trough and the server will start to relisten for a new client.
        /// </summary>
        private const string noInput = "no-input\n[PC][INF][+]:  Time is up. No input, will re-connect.";

        /// <summary>
        /// Title of the Question MessageBox.
        /// </summary>
        private const string tit = "File Request";

        /// <summary>
        /// The massage to display to the user when the client requested a file.
        /// </summary>
        private const string ask = "The Client requested a File from this Computer !\nDo you allow to send this file ?:\n{0}";

        /// <summary>
        /// Define a Socket Name to use.
        /// </summary>
        public string sockName;

        /// <summary>
        /// Temporary store the ip string.
        /// </summary>
        private string tempIP = string.Empty;

        /// <summary>
        /// Byte represnentation of "STOP".
        /// </summary>
        private byte[] stop = new byte[4];

        /// <summary>
        /// Byte representation of "START".
        /// </summary>
        private byte[] start = new byte[5];
        #endregion Vars

        /// <summary>
        /// Instance initializer.
        /// </summary>
        public PS4TCP(RichTextBox rtb, Control control) {
            this.rtb = rtb;
            this.control = control;
            rtb.SetControl(control);
        }

        /// <summary>
        /// Exit from the Gui instance.
        /// </summary>
        public void Exit() {
            if (Gui.connected) {
                exit = true;
                if (counter.IsAlive) {
                    counter.Abort();
                    rtb.SetSelectedText("n");
                }
            } else {
                server.Stop();               
            }
        }

        /// <summary>
        /// The Count Thread. Will count a second then subtract 1 from the counter and alter the console output.
        /// </summary>
        private void Count() {
            int toAlter;
            toAlter = rtb.CursorTop() - 2;
            int cursorLeft;                                                              // Set up two integers to store the cursor position for backup reasons.
            cursorLeft = cursorTop = 0;                                                             // Initialize them to 0.
            for (int i = 0; i < COUNT; i++) {                                                       // Loop now for 10 seconds.
                Thread.Sleep(1000);                                                                 // Thread wait for a second.
                COUNT--;                                                                            // Count 1 down.
                cursorLeft = rtb.CursorLeft();
                rtb.SetSelectionStart(rtb.GetFirstCharIndexFromLin(toAlter) + 45);             // Set Cursor to the position to alter.
                Write(COUNT.ToString());                                                    // Write out the new value.
                rtb.SetSelectionStart(cursorLeft);                                                    // Reset previous backed up Cursor Position and Line.
            }
            if (log) Logger.WriteLine(useLog, false, noInput);                                      // Write information into the logger.
            rtb.WriteLine("y");
        }        
        
        /// <summary>
        /// Create a test file in the folder '\test\' of the server's run time root directory.
        /// </summary>
        private bool CreateFile(string path) {
            try {
                if (path == currDir + @"\test\test.bin") {                                             // Determine wether we shall create the test file or simple a other one.
                    if (File.Exists(path)) File.Delete(path);                                          // If the test file exists, delete it.
                    File.Create(path).Close();                                                         // Create a new clean file.
                    WriteLine("{0}File successfull created!");
                    WriteLine("{0}{1}", PC, path);

                    byte[] _test = test.Encode(encode);                                                 // Set up the buffer and encode test string.
                    WriteLine("{0}Test string encoded!", PC);

                    _test.ToFile(path);                                                                 // Write to file.
                    WriteLine("{0}Bytes written into file!", PC);
                } else if (path == currDir + @"\received\portdump.bin") {                               // Else if we shall dump the whole port traffic.
                    string test = path;                                                                // first we overload the path var into new one.
                    if (File.Exists(test)) {                                                           // Then we check if the file exists. -1
                        for (int i = 0; i < 1000; i++) {                                               // If the file exists, we go trough an integer value up to 1000.
                            test = currDir + @"\received\portdump" + i.ToString() + ".bin";            // Add it to the file string and reset it.
                            if (!File.Exists(test)) break;                                             // And if the file do not exist, we break and have a new file to create in the test var.
                        }
                    }
                    File.Create(test).Close();                                                         // -1 If not, we create it and else, we find trough the loop out which file to create.
                    WriteLine("{0}Port dump file successfull created!", PC);
                    WriteLine("{0}Will save to file={1}", PC, test);
                } else {                                                                               // Else for any other type of file.
                    if (File.Exists(path)) {                                                           // If the file already exists.
                        WriteLine("{0}File already exists!", PCE);
                        WriteLine("{0}Do you want me to replace the File?", PC);
                        while (true) {                                                                 // User Input loop.
                            string input;
                            input = rtb.ReadLine();
                            if (input == "yes" || input == "y") {
                                File.Delete(path);
                                WriteLine("{0}File Deleted!", PC);
                            }
                            if (input == "no" || input == "n") {
                                WriteLine("{0}Will abort file creation!", PC);
                                WriteLine("{0}Will not save incomming data to desired file!", PC);
                                return false;
                            }
                        }
                    }
                    File.Create(path).Close();                                                         // Create the File.
                    WriteLine("{0}File successfull created!", PC);
                    WriteLine("{0}{1}", PC, path);
                }
            } catch (IOException e) {                                                                  // Catch IO exception, if any.
                WriteLine("{0}There was a IO Exception catched!", PCE);
                WriteLine("{0}Exception: {1}", PCE, e);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Check if the directorys we need are there. If not create them.
        /// </summary>
        private bool CheckDirs() {
            string[] paths = new string[3];                                                                              // Set up some strings.
            string[] strings = new string[4];
            paths[0] = currDir + @"\received\";
            paths[1] = currDir + @"\test\";
            paths[2] = currDir + @"\logs\";
            strings[0] = "Ok! test->";
            strings[1] = "Ok! logs->";
            strings[2] = strings[3] = "Ok!";
            outbuff = PC + "Checking for directorys: received->";                                                 // Write into the output buffer till we can write into the log too.

            int i;
            for (i = 0; i < paths.Length; i++) {                                                                   // Go through all path's.
                try {
                    if (!Directory.Exists(paths[i])) Directory.CreateDirectory(paths[i]);                          // Creating directorys if they don't exist.
                    outbuff += strings[i];                                                                         // Text into buffer.
                } catch (Exception e) {
                    strings[3] = e.ToString();
                    goto error;
                }
            }
            outbuff += "\n";                                                                                       // Close the line.
            return true;

            error: {
                rtb.WriteColor("Error!", Color.FromName(errCol));
                WriteLine("{0}Please run me from a location to where i have access to my self.", PCE);
                WriteLine("{0}Exception: {1}", PCE, strings[3]);
                throw new Exception(strings[3]);
            }
        }

        /// <summary>
        /// Checks the format and Triggers the depending Cast to make the output nice.
        /// </summary>
        /// <param name="format">The format to check.</param>
        /// <returns>The formatted string, if any.</returns>
        private string CheckFormat(string format) {
            if (!formatting) format = format.Replace("[PS4]:", "").Replace("[PC]:", "");
            else {
                if (format.Contains("[PC]:")) {
                    WritePCString();
                    format = format.Replace("[PC]:", "");
                } else if (format.Contains("[PC][ERROR]:")) {
                    WriteErrorString();
                    rtb.WriteColor(format.Replace("[PC][ERROR]:", ""), Color.FromName(errCol));
                } else WriteSockString();
            }
            return format;
        }

        /// <summary>
        /// Piped Function to write a char or a string into the console and the logger to same time.
        /// </summary>
        /// <param name="format">The String to write.</param>
        /// <param name="args">The argument(s) which shall be swapped with the place holder(s).</param>
        private void Write(string format, [Optional] params object[] args) {
            if (args != null) format = format.FormatString(args);
            format = CheckFormat(format);
            rtb.WriteColor(format, Color.FromName(foreCol));

            if (log) {
                if (args != null) Logger.Write(useLog, format, args);
                else Logger.Write(useLog, format);
            }
        }

        /// <summary>
        /// Piped Function to write a char or a string with a line termination into the console and the logger to same time.
        /// </summary>
        /// <param name="format">The char or format to print out.</param>
        /// <param name="args">The argument(s) which shall be swapped with the place holder(s).</param>
        private void WriteLine(string format, [Optional] params object[] args) {
            if (args != null) format = format.FormatString(args);
            format = CheckFormat(format);
            rtb.WriteColor(format + "\n", Color.FromName(foreCol));

            if (log) {
                if (args != null) Logger.WriteLine(useLog, false, format, args);
                else Logger.WriteLine(useLog, false, format);
            }
        }

        /// <summary>
        /// Write Colorized Output from the client/server.
        /// </summary>
        /// <param name="data">The data as string to use.</param>
        private void WriteColor(string data) {
            if (formatting) WriteSockString();
            rtb.SetSelectedText(ResolveColorForInlineWrite(data));
        }

        /// <summary>
        /// A wrapper for the message and to write bytes out.
        /// </summary>
        /// <param name="data">The received string data.</param>
        /// <param name="bytes">The received byte[] data.</param>
        /// <param name="count">Length of the received data.</param>
        private void ConnectionWrite(string data, byte[] bytes, int count) {
            if (binData) bytes.ToFile(binFile, 0, count);              // Write data to bin file.
            WriteLine(data);                                             // Write string data out.
        }

        /// <summary>
        /// Write the Socket string nice.
        /// </summary>
        private void WriteSockString() {
            rtb.WriteColor("[", Color.FromName(foreCol));
            rtb.WriteColor(sockName, Color.FromName(sockCol));
            rtb.WriteColor("]: ", Color.FromName(foreCol));
        }

        /// <summary>
        /// Write the Pc string nice.
        /// </summary>
        private void WritePCString() {
            rtb.WriteColor("[", Color.FromName(foreCol));
            rtb.WriteColor("PC", Color.FromName(pcCol));
            rtb.WriteColor("]: ", Color.FromName(foreCol));
        }

        /// <summary>
        /// Write the error string nice.
        /// </summary>
        private void WriteErrorString() {
            rtb.WriteColor("[", Color.FromName(foreCol));
            rtb.WriteColor("PC", Color.FromName(pcCol));
            rtb.WriteColor("][", Color.FromName(foreCol));
            rtb.WriteColor("ERROR", Color.FromName(errCol));
            rtb.WriteColor("]: ", Color.FromName(foreCol));
        }

        /// <summary>
        /// Clear's a line, a string or only a char from the console.
        /// </summary>
        /// <param name="position">The position within the line. (from left to right)</param>
        /// <param name="count">The count to clear.</param>
        private void ClearLine(int position, int count) {
            int index = rtb.CursorLeft();
            rtb.SetSelectionStart(position);
            rtb.SetSelectedText(new string(' ', count));
            rtb.SetSelectionStart(index);
        }

        /// <summary>
        /// Change a line.
        /// </summary>
        /// <param name="data">The string data to use.</param>
        private void AlterLine(string data) {
            string[] splitted = data.Split(' ');                                  // Split on spaces.

            if (splitted.Length < 2) return;                                      // If splitted array is shorten then 2, error.

            string left = splitted[0].Replace("Left::", "");                      // Replace trigger on first string.
            string top = splitted[1].Replace("Top::", "");                        // Replace trigger           

            if (int.TryParse(left, out cursorLeft)) {                             // Try to parse the left cursor.
                if (int.TryParse(top, out cursorTop)) {                           // Try to parse the top cursor.
                    string output = data.Replace("Left::" +                       // Format the true output.
                                                 left +
                                                 " ",
                                                 "").Replace("Top::" +
                                                             top +
                                                             " ",
                                                             "");
                    int backupLeft;

                    backupLeft = rtb.GetSelectionStart();
                    rtb.SetSelectionStart(cursorLeft);
                    Write(output);
                    rtb.SetSelectionStart(backupLeft);
                } else WriteLine("{0}Can't parse the line to use !", PCE);
            } else WriteLine("{0}Can't parse the position within the line to use !", PCE);
        }

        /// <summary>
        /// Clear a Line.
        /// </summary>
        /// <param name="data">The string data to use.</param>
        private void ClearLineTrigger(string data) {
            int converted;                                                                          // Stored the converted value.
            if (int.TryParse(data.Replace("CLEARLINE ", ""), out converted)) {                      // Remove trigger and try to convet number.
                int backupLeft;
                backupLeft = rtb.CursorLeft();                                                     // Backup the cursor possition.
                rtb.SetSelectionStart(rtb.GetFirstCharIndexFromLin(converted));                    // Set now the line to the one to clean.
                rtb.SetSelectionLength(rtb.GetLine(converted).Length);                             // And select all text within this line.
                rtb.SetSelectedText(string.Empty);                                                 // Clear the selected text aka the whole line.
                rtb.SetSelectionStart(backupLeft);                                                 // Reset the cursor to his last position.
            } else WriteLine("{0}[ClearLineCMD] Couldn't convert the line indexer string to a integer.", PCE); // Else write a error.
        }

        /// <summary>
        /// Return the cursor position to the client/server.
        /// </summary>
        /// <param name="stream">The stream to which we shall write the respong.</param>
        private void GetCursor(NetworkStream stream) { Respond(stream, "Left::" + rtb.CursorLeft().ToString() + " Top::" + rtb.CursorTop().ToString()); }

        /// <summary>
        /// Read Line Trigger.
        /// </summary>
        private void ReadLineTrigger() { Respond(stream, rtb.ReadLine()); }     

        /// <summary>
        /// Ordinary Welcome Screen.
        /// </summary>
        private void ShowWelcome() {
            string format;
            format = "\n";
            format += "      *******************************************************\n";
            format += "      **<<<<<<<<<<<<<<<<<<<<<#>>>>>>>>>>>>>>>>>>>>>**\n";
            format += "      **<<                                                                                             >>**\n";
            format += "      **<<                                                                                             >>**\n";
            format += "      **<<                                          '''''''''                                             >>**\n";
            format += "      **<<                              ''''''''''''PS4 TCP''''''''''''                                >>**\n";
            format += "      **<<              ''A simple PS4 Debugging Server''               >>**\n";
            format += "      **<<                     '''''Featuring Ultimate IO'''''                        >>**\n";
            format += "      **<<                                      '''''''''''''''''''''''                                        >>**\n";
            format += "      **<<                                                                                              >>**\n";
            format += "      **<<                                                                                              >>**\n";
            format += "      **<<<<<<<<<<<<<<<<<<<<<#>>>>>>>>>>>>>>>>>>>>>>**\n";
            format += "      ********************************************************\n\n";
            format += "\n     Simple TCP Debugging Server. Adjusted for the PS4.\n";
            format += "     Can also receive Files and save them to their names.\n";
            format += "     ^^ Can also send Files to the PS4.\n";
            format += "     Can clear the screen too.\n";
            format += "     Can receive and dynamical resolve arguments sended from a -\n";
            format += "     application running on the PS4. No hardcoded library needed.\n";
            format += "     Will also generate a Log.txt.\n";
            format += "\n     For more information please refer to the README.\n";
            format += "\n     TCP Listener Class by Microsoft.\n";
            format += "     Code written for PS4 Debugging by (c) cfwprophet.\n\n";

            Font backup = rtb.GetSelectionFont();
            rtb.SetSelectionFont(new Font("Century Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0));
            rtb.WriteColor(format, Color.FromKnownColor(KnownColor.ForestGreen));
            rtb.SetSelectionFont(backup);
            rtb.SetSelectionColor(Color.FromName(foreCol));

            int index = rtb.CursorLeft();                                                                    // Store actual position within the rtb.
            int time;
            for (time = 3; time > -1; time--) {                                                              // Give user time to read, then count one down. Do that 3 times.
                rtb.SetSelect(index, rtb.CursorLeft() - index);
                rtb.SetSelectedText("..." + time);
                if (time == 0) {
                    Thread.Sleep(500);                                                                       // At last a half second to read the 0.
                    break;
                }
                Thread.Sleep(1000);                                                                          // We sleep a second.               
            }
            rtb.SetSelectedText("\n\n");                                                                     // Line ending and go on.
        }

        /// <summary>
        /// Initialize the logger, generate a new log file.
        /// </summary>
        /// <returns>True if we could generate a new file, else false.</returns>
        private bool InitializeResources(string[] args) {
            try {
                CheckDirs();                                                                                    // Check if directorys we need exists.

                string temp = (@"\logs\log_" +
                               DateTime.Now.ToLongDateString() + "_" +
                               DateTime.Now.ToLongTimeString() +
                               ".txt").Replace(":", "_").Replace(",", "").Replace(". ", "_").Replace(" ", "_"); // Set up the log file specific. We add date and time, then format it.

                useLog = currDir + temp;                                                                        // Concat current directory string with our log file.
                File.Create(useLog).Close();                                                                    // Create log file. We do that manually cause our function would print something.

                outbuff = "Log File successful created!";
                Logger.WriteLine(useLog, false, "{0}Log File successful created!", PC);

                ShowWelcome();                                                                                  // Since we need to buff the first few lines and since i wanted to print the welcome-
                WriteLine(PC + outbuff);                                                                        // before to return the string, do not work here.

                if (!ParseArgs(args)) return false;                                                             // Checking the arguments input.
                if (!File.Exists(currDir + @"\test\test.bin")) { CreateFile(currDir + @"\test\test.bin"); }     // Creating the test file.
                if (binData) CreateFile(binFile);                                                               // Create a dump file to store the sniffed data from the tcp port.
                return true;
            } catch (IOException e) {
                WriteLine("{0}An Exception occured:\n{1}", PCE, e);
                throw new Exception(PCE + "An Exception occured:\n", e);
            }
        }

        /// <summary>
        /// Resolve the encoding for the client.
        /// </summary>
        /// <param name="encoding">The string that holds the param.</param>
        private void ResolveEncoding(string encoding) {
            if (encoding.Equals(_ascii, icic)) encode = Encoding.ASCII;                                                          // Check if the user input equals a encoding.
            else if (encoding.Equals(_utf8, icic)) encode = Encoding.UTF8;                                                       // and doing this we 
            else if (encoding.Equals(_utf7, icic)) encode = Encoding.UTF7;                                                       // ignore the culture and
            else if (encoding.Equals(_utf32, icic)) encode = Encoding.UTF32;                                                     // we ignore also the case of the string.
            else if (encoding.Equals(_utf16, icic) || encoding.Equals(_unicode, icic)) encode = Encoding.Unicode;
            else {
                MessagBox.Error("Wrong Encoding defined !");
                Exit();
            }
            start = "START".Encode(encode);
            stop = "STOP".Encode(encode);
        }

        /// <summary>
        /// Set the guis fore and back ground as well as the error colors to the custom overloaded one.
        /// </summary>
        /// <param name="data">A string array containing data from the TCP port which we will read and set the colors too.</param>
        private void SetGuiColor(string[] data) {
            KnownColor foreGround, backGround, error;
            backCol = foreCol = errCol = "nothing";
            foreGround = backGround = error = 0;

            foreach (string str in data) {
                if (str.Contains(fg)) Enum.TryParse(str.Replace(fg, ""), true, out foreGround);
                if (str.Contains(bg)) Enum.TryParse(str.Replace(bg, ""), true, out backGround);
                if (str.Contains(er)) Enum.TryParse(str.Replace(er, ""), true, out error);
            }

            if (foreGround != 0 || backGround != 0 || error != 0) {
                foreach (string name in Enum.GetNames(typeof(KnownColor))) {
                    KnownColor toCompare = (KnownColor)Enum.Parse(typeof(KnownColor), name);
                    if (foreGround != 0 && toCompare == foreGround) { foreCol = name; rtb.SetForeColor(Color.FromKnownColor(foreGround)); }
                    if (backGround != 0 && toCompare == backGround) { backCol = name; rtb.SetBackColor(Color.FromKnownColor(backGround)); }
                    if (error != 0 && toCompare == error) errCol = name;
                }
            } else {
                Write("error!\n");
                WriteLine("{0}An parsing error occured!\n{0}Could not resolve a single custom Color. :(", PCE);
                WriteLine("{0}Could not resolve a single custom Color. :(", PCE);
            }
        }

        /// <summary>
        /// Resolve the color to use for the output if we received the 'COLOR' trigger.
        /// </summary>
        /// <param name="data">The received string data to get file name.</param>
        private string ResolveColorForInlineWrite(string data) {
            string[] splitted = data.Split(' ');
            string replace = splitted[0] + " ";
            if (Enum.TryParse(splitted[0].Replace("COLOR ", ""), out KnownColor parse)) rtb.SetSelectionColor(Color.FromKnownColor(parse));
            return data.Replace(replace, "");
        }

        /// <summary>
        /// Change the Color on received Trigger from the socket.
        /// </summary>
        /// <param name="data">The colors as string.</param>
        private void ChangeColorTrigger(string data) {
            string[] trim = data.Replace(CCOL + " ", "").Split(' ');                            // Trim the trigger and splitt the rest into a array.
            SetGuiColor(trim);
        }

        /// <summary>
        /// Throw a format exception.
        /// </summary>
        /// <param name="badboy">The badboy, causing the format exception.</param>
        private void FormatException(string badboy) {
            WriteLine("{0}{1}: Bad Format!\n{0}Wrong Format for Port entered!", PCE, badboy);
            throw new Exception(PCE + badboy.ToString() + ": Bad Format!\n" + PCE + "Wrong Format for Port entered!");
        }

        /// <summary>
        /// Throw a overflow exception.
        /// </summary>
        /// <param name="badboy">The badboy, causing the overflow exception.</param>
        private void Overflow(string badboy) {
            WriteLine("{0}{1}: Overflow!\n{0}  Value entered is not in a valid range!", PCE, badboy);
            throw new Exception(PCE + badboy.ToString() + ": Overflow!\n" + PCE + "Value entered is not in a valid range!");
        }

        /// <summary>
        /// Sending out a Respond to the Client. (in our case the PS4, but will also work with any other client, respecting our rules.
        /// </summary>
        /// <param name="stream">The stream to which we shall write the respond.</param>
        /// <param name="respond">The Respond to sent.</param>
        private void Respond(NetworkStream stream, string respond) {
            byte[] msg = respond.Encode(encode);                                        // Byte array to convert our message into.
            stream.Write(msg, 0, msg.Length);                                          // Send back a response.
        }

        /// <summary>
        /// Receive Bytes from a stream.
        /// </summary>
        /// <param name="stream">The stream from where to read the incomming data.</param>
        /// <returns>The received structure containing the data and the readed size.</returns>
        private RecvBytes ReceiveData(Stream stream) {
            RecvBytes incomming = new RecvBytes();                                        // Init a new received strucutre.
            incomming.Data = new byte[15000000];                                          // Set byte buffer to 15 MB.
            incomming.Size = 0;                                                           // Initi the size counter.
            bool started = false;                                                         // Flag to indicate byte income hase started.
            int readed = 0;                                                               // Counter for the read loop.

            while ((readed = stream.Read(incomming.Data, incomming.Size, 4096)) > 0) {    // Read from the socket as long connection is open.
                if (incomming.Data.Encode(5) == "START") started = true;             // Do we have the str "START" trigger ?
                if (started) {                                                            // If start trigger was received we write bytes into buffer and count up.
                    if (incomming.Data.Encode(4) == "STOP") break;                   // Do we have the str "STOP" trigger ? Then break the loop.
                    incomming.Size += readed;                                             // Else we count the readed bytes up.
                }
            }
            Array.Resize(ref incomming.Data, incomming.Size);                             // When we done, we resize the array before returning it, based on the readed size of the byte data.
            return incomming;                                                             // And back to the caller.
        }
        
        /// <summary>
        /// Send bytes to a stream.
        /// </summary>
        /// <param name="data">The received string data to get file name.</param>
        private void SendFileToClient(string data) {
            Write("{0}Sending Data...", PC);                                                   // Inform that we sending data.
            string fileName = data.Replace("host://GETFILE ", "");                             // Trim
            if (File.Exists(currDir + @"/share/" + fileName)) {                                // check if file exists
                if (MessagBox.Question(Buttons.YesNoCancel,                                    // Ask user for permission
                                       "File Requesd",
                                       "The Client on the other side requesded this file:\n" +
                                       fileName +
                                       "\nDo you allow to send it ?") == DialogResult.Yes) {
                    byte[] fileDat = (currDir + @"/share/" + fileName).FromFile();                                          // Clear the file info variable.

                    stream.Write(start, 0, start.Length);                                      // Sending the start trigger.
                    stream.Write(fileDat, 0, fileDat.Length);                                  // Now the Data it self.
                    stream.Write(stop, 0, stop.Length);                                        // And tell the client that we are done.

                    Write("Done !\n");                                                         // Inform user.
                    WriteLine("{0}Sended " + fileDat.Length.ToString() + " bytes out to the client.", PC);
                } else {
                    stream.Write(start, 0, start.Length);                                      // Sending the start trigger.
                    stream.Write(new byte[0], 0, 1);                                           // Now a dummy aka single byte.
                    stream.Write(stop, 0, stop.Length);                                        // And tell the client that we are done.
                }
            }
        }

        /// <summary>
        /// Send bytes to a Stream from a Drag&Drop Event.
        /// </summary>
        /// <param name="data">The received string data to get file name.</param>
        public void SendFileFromDrop(string data) {
            Write("{0}Sending Data...", PC);                                                   // Inform that we sending data.
            byte[] fileDat = data.FromFile();

            stream.Write(start, 0, start.Length);                                              // Sending the start trigger.
            stream.Write(fileDat, 0, fileDat.Length);                                          // Now the Data it self.
            stream.Write(stop, 0, stop.Length);                                                // And tell the client that we are done.

            Write("Done !\n");
            WriteLine("{0}Sended " + fileDat.Length.ToString() + " bytes out to the client.", PC); // Inform user.
            Gui.sended = true;                                                                 // Tell the gui that we have sended the file.
        }

        /// <summary>
        /// Receive a File from a client and save it to the disk. Uses the string trigger "START" and "STOP" to indicate the state of the transfare.
        /// </summary>
        /// <param name="data">The received string data to get file name.</param>
        private void ReceiveFileFromClient(string data) {
            Write("{0}Getting Data...", PC);                                                          // Inform that we getting data.
            string fileName = data.Replace("host://PUTFILE ", "");                                    // store trimmed file name.
            RecvBytes received = new RecvBytes();
            received = ReceiveData(stream);
            if (received.Size > 0) {
                File.Create((recvDir + fileName).TestFileName()).Close();
                received.Data.ToFile(recvDir + fileName, 0, received.Size);
                Write("Done!\n");      // Inform that we are done.
                WriteLine("{0}Got " + received.Size.ToString() + "bytes of sweatness.", PC);
            }
        }
        
        /// <summary>
        /// Ask user for reconnect.
        /// </summary>
        /// <returns>True if shall not reconnect, else false.</returns>
        private bool CheckReconnect() {
            COUNT = 10;                                                                                       // Set the counter.
            WriteLine(askReCon, PC, COUNT);                                                                   // Ask user for reconnection.
            counter.Start();                                                                                  // Start the counter now.
            Write("{0}Enter key: (y/n) ", PC);                                                                // Write the information for the user to input.
            
            int indexer = rtb.CursorLeft();

            bool reconnect = true;                                                                            // Set reconnect bool to true by default.
            while (true) {                                                                                    // Aslong the counter is still running.
                string userInput = string.Empty;                                                              // Buffer to read the user input.

                userInput = rtb.ReadLine().Replace("[PC]:  Enter key: (y/n) ", "");

                if (userInput.Equals("y", icic) || userInput.Equals("yes", icic)) {                           // If input was yes.
                    if (log) Logger.WriteLine(useLog, false, userInput);                                      // Log it.
                    break;                                                                                    // Stop the loop.
                } else if (userInput.Equals("n", icic) || userInput.Equals("no", icic)) {                     // If input was no.
                    reconnect = false;                                                                        // Set no flag to true.
                    if (log) Logger.WriteLine(useLog, false, userInput);                                      // Log it.
                    break;                                                                                    // Stop the loop.
                }
                ClearLine(indexer, rtb.GetTextLength() - indexer);
            }
            return reconnect;
        }

        /// <summary>
        /// Reset the color of the output.
        /// </summary>
        private void ResetColor() {
            backCol = "Black";
            foreCol = "White";
            errCol = "Red";
        }

        /// <summary>
        /// Parse the arguments entered by the user and validate them.
        /// </summary>
        /// <param name="args">The arguments to parse.</param>
        /// <returns>true if all is fine, else false.</returns>
        private bool ParseArgs(string[] args) {
            string tempPort, tempEnc;                                                                                // Temp strings for IP & PORT + encoding, so we only need to set..
            tempPort = tempEnc = string.Empty;                                                                       // this up one time and not for every diffrent check a own one.
            
            if (args[0].Contain("-i")) tempIP = args[0].Replace("-i", "");                                          // Resolve.
            else if (args[1].Contain("-i")) tempIP = args[1].Replace("-i", "");

            if (args[0].Contain("-p")) tempPort = args[0].Replace("-p", "");
            else if (args[1].Contain("-p")) tempPort = args[1].Replace("-p", "");

            localHost = IPAddress.Parse(tempIP);
            PORT = Port.Parse(tempPort);

            tempEnc = args[2];
            if (args[3].Equals("server", icic)) useServer = true;
            else useServer = false;
            return true;
        }

        /// <summary>
        /// The main entry of the application.
        /// </summary>
        /// <param name="args">The arguments that can be used with this application.</param>
        public void Ps4Tcp(string[] args) {
            if (!InitializeResources(args)) Exit();                                                                 // Try to init resources and to parse the args.

            try {
                counter = new Thread(Count) { IsBackground = true };                                                // Set up a Counting Thread for time limited user input, which runs in background.
                counter.SetApartmentState(ApartmentState.STA);                                                      // Set the Thread to be a Single Threaded Apartment.
                if (useServer) {
                    server = new TcpListener(localHost, PORT);                                                      // Initialize the TCP Listener on given IP and PORT.
                    server.Start();                                                                                 // Start listening for client requests.
                }

                while (true) {                                                                                      // Enter the listening loop.
                    if (!noConnectMsg) Write("{0}Waiting for a connection... ", PC);                                // If this is not a Browser Debug session.

                    if (useServer) client = server.AcceptTcpClient();                                               // Perform a blocking call to accept requests.
                    else client = new TcpClient(tempIP, PORT);

                    if (!noConnectMsg) rtb.SetSelectedText("Connected!");                                         // Again if that's a normal session, write out.
                    Gui.connected = true;                                                                           // If this is a GUI instance, set the connected var.

                    int i;
                    byte[] bytes = new byte[client.ReceiveBufferSize];                                              // We set our byte buffer to the same size of the client's buffer.
                    string data = string.Empty;                                                                     // The string data buffer.                    
                    cursorLeft = cursorTop = 0;                                                                     // Set to 0 to full fill the compiler.

                    stream = client.GetStream();                                                                    // Get a stream object for reading and writing.
                    while ((i = stream.Read(bytes, 0, client.ReceiveBufferSize)) != 0) {                            // Loop to receive all the data sent by the client.  -Magic0 Note#2
                        data = bytes.Encode(i, encode);                                                             // Encode Bytes to a string, based on the clients encoding.

                        if (!string.IsNullOrEmpty(data)) {                                                          // Check if reall data would be received.
                            if (data.Contain(FSREQ)) ReceiveFileFromClient(data);                                   // Put a file onto this host.
                            else if (data.Contain(FGREQ)) SendFileToClient(data);                                   // Send a file to the connected computer.
                            else if (data.Contain(CLS)) rtb.SetText(string.Empty);
                            else if (data.Contain("RESETCOLOR")) ResetColor();                                      // Reset the consoles color.
                            else if (data.Contain(CCOL)) ChangeColorTrigger(data);                                  // Change Colors for this instance.
                            else if (data.Contain(CLL)) ClearLineTrigger(data);                                     // Clear the Line based on the trigger from the socket.
                            else if (data.Contain(GCRS)) GetCursor(stream);                                         // Get Cursor and return.
                            else if (data.Contain("READLINE")) ReadLineTrigger();                                   // Read the Line based on the trigger from the socket. 
                            else if (data.Contain(AL)) AlterLine(data.Replace(AL, ""));                             // Alter a Line.
                            else if (data.Contain("COLOR")) WriteColor(data);                                       // Write Colorized output.
                            else if (data.Contain("FORMATOFF")) formatting = false;                                 // Turn formatting off.
                            else if (data.Contain("FORMATON")) formatting = true;                                   // Turn formatting on.
                            else ConnectionWrite(data, bytes, i);                                                        // Else write out text (and bin).
                        }
                        if (exit) break;                                                                            // Gui requested to kill the process ?
                    }
                    stream.Close();                                                                                 // Close Stream.
                    client.Close();                                                                                 // Shutdown and end connection
                    if (exit) break;                                                                                // Gui requested to kill the process ?
                    if (!noConnectMsg && !CheckReconnect()) break;                                                  // If this is not a debug loop and user choosed to close, break the loop.
                }
            } catch (SocketException e) { WriteLine("{0}SocketException: {1}", PCE, e); }                           // There was a socket exception catched.
            finally {
                if (useServer) server.Stop();                                                                       // Stop listening for new clients.
                Gui.connected = false;                                                                              // If this is a GUI instance, clear the connected var.
            }

            exit = false;                                                                                           // Reset exit.
            WriteLine("\n{0}By by, have a nice day. :)", PC);
        }
    }
}

