using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Pipes;

namespace CyberCortex
{
    public static class PipeServer
    {
        private static NamedPipeServerStream _server = new NamedPipeServerStream("MT4.Server");

        public static double StartServer(double DataToClient)
        {
            double DataFromClient = 0;
            try
            {
                _server.WaitForConnection();

                BinaryReader reader = new BinaryReader(_server);
                DataFromClient = reader.ReadDouble();

                BinaryWriter writer = new BinaryWriter(_server);
                writer.Write(DataToClient);
                writer.Flush();

                _server.Disconnect();
            }
            catch { }

            return DataFromClient;
        }

        public static void StopServer()
        {
            try
            {
                if (_server.IsConnected)
                {
                    _server.Disconnect();
                }
            }
            catch { }
        }
    }
}
