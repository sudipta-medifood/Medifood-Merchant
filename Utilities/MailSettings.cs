using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities
{
    public class MailSettings
    {
        public string SenderMail { get; set; }
        public string SenderUsername { get; set; }
        public string SenderName { get; set; }
        public string SenderMailPassword { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
    }
}
