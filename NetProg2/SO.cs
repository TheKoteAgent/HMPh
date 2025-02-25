using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace NetProg2
{
    internal class SO
    {
        public Socket WorkSocket { get; set; }

        public string Message { get; set; } = String.Empty;

        public static readonly int BufferSize = 1024;

        public byte[] Buffer = new byte[BufferSize];
    }
}
