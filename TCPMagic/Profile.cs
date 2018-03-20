using System.Drawing;

namespace TCPMagic {
    /// <summary>
    /// A Profile structure to store user defined profiles.
    /// </summary>
    public class Profile {
        public string ProfileName;
        public string IP;
        public string Port;
        public string Encoding;
        public bool Server;
        public bool Drop;
        public bool Log;
        public bool BinDmp;
        public bool format;
        public bool nCMdg;
        public string font;
        public string sockName;
        public string backCol;
        public string foreCol;
        public string errCol;
        public string sockCol;
        public string pcCol;
    }
}
