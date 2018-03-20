using System.Configuration;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Drawing;
using SwissKnife;

namespace TCPMagic.Properties {
    // Diese Klasse ermöglicht die Behandlung bestimmter Ereignisse der Einstellungsklasse:
    internal sealed partial class Settings {
        /// <summary>
        /// Instance initializer.
        /// </summary>
        public Settings() {
            if (ClientIPs == null) ClientIPs = new StringCollection();
            if (Ports == null) Ports = new StringCollection();
            if (Profiles == null) Profiles = new List<Profile>();
            DoIt();
        }

        /// <summary>
        /// Save and Reload settings.
        /// </summary>
        public void DoIt() { Save(); Reload(); }

        /// <summary>
        /// Local Store for used Client IPs.
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("")]
        public StringCollection ClientIPs {
            get { return (StringCollection)this["ClientIPs"]; }
            set { this["ClientIPs"] = value; }
        }

        /// <summary>
        /// Local Store for Ports.
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("")]
        public StringCollection Ports {
            get { return (StringCollection)this["Ports"]; }
            set { this["Ports"] = value; }
        }

        /// <summary>
        /// Local Store the Last used Path for Select File.
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("EMPTY")]
        public string LastPath {
            get { return (string)this["LastPath"]; }
            set { this["LastPath"] = value; }
        }

        /// <summary>
        /// Local Store for user defined Profiles.
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("")]
        public List<Profile> Profiles {
            get { return (List<Profile>)this["Profiles"]; }
            set { this["Profiles"] = value; }
        }

        /// <summary>
        /// Local Store of the RichTextBox Back Color.
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("Black")]
        public Color BackCol {
            get { return (Color)this["BackCol"]; }
            set { this["BackCol"] = value; }
        }

        /// <summary>
        /// Local Store of the RichTextBox Fore Color.
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("Yellow")]
        public Color ForeCol {
            get { return (Color)this["ForeCol"]; }
            set { this["ForeCol"] = value; }
        }

        /// <summary>
        /// Local Store of the RichTextBox Error Color.
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("Red")]
        public Color ErrCol {
            get { return (Color)this["ErrCol"]; }
            set { this["ErrCol"] = value; }
        }

        /// <summary>
        /// Local Store of the RichTextBox Socket Color.
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("RoyalBlue")]
        public Color SockCol {
            get { return (Color)this["SockCol"]; }
            set { this["SockCol"] = value; }
        }

        /// <summary>
        /// Local Store of the RichTextBox PC Color.
        /// </summary>
        [UserScopedSetting]
        [DefaultSettingValue("Cyan")]
        public Color PcCol {
            get { return (Color)this["PcCol"]; }
            set { this["PcCol"] = value; }
        }
    }
}
