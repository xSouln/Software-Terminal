using xLibV100.Common;
using xLibV100.Components;
using xLibV100.Controls;
using xLibV100.Ports;
using xLibV100.Transceiver;

namespace Terminal
{
    public partial class Control
    {
        public override unsafe void PacketReceiver(PortBase port, ReceivedPacketHandlerArg arg)
        {
            RxPacketManager manager = null;

            //string convert = "";
            //string temp = "";

            //casting data to package structure
            PacketT* packet = (PacketT*)arg.DataPtr;
            PacketT* identified_packet = null;

            //whether the package is a transaction
            if (arg.PacketSize >= sizeof(PacketIdentificator)
                && (packet->Header.Identificator & (uint)PacketIdentificator.Mask) == (uint)PacketIdentificator.Default)
            {
                //size check for minimum transaction length
                if (arg.PacketSize < sizeof(PacketT))
                {
                    return;
                }

                int content_size = arg.PacketSize - sizeof(PacketT);

                //checking if the package content size matches the actual size, if the size is short
                if (content_size < packet->Info.ContentSize)
                {
                    return;
                }

                //reset size when content exceeds size specified in packet.Info
                if (content_size > packet->Info.ContentSize)
                {
                    xTracer.Message("transaction content size error");
                    goto end;
                }

                identified_packet = packet;
            }

            manager = new RxPacketManager
            {
                Port = port,
                ReceivedPacket = arg,
                Response = new DataBuffer(),
                Packet = identified_packet
            };

            xContent content = new xContent
            {
                Data = arg.DataPtr,
                DataSize = arg.PacketSize
            };

            foreach (TerminalObject device in port.Listeners)
            {
                if (device.ResponseIdentification(manager, content))
                {
                    goto end;
                }
            }

            //convert = xConverter.ToStrHex(data, data_size);
            var convertedData = xConverter.GetString(arg.DataPtr,
                arg.PacketSize,
                new byte[] { (byte)'\n' });

            if (convertedData.Length > 0)
            {
                xTracer.Message("unidentified from " + port.Name, convertedData);
            }

            port.ToBridgePorts(arg);

        end:
            port.ClearRxBuffer();
            if (manager != null && manager.Response.DataSize > 0)
            {
                port.Send(manager.Response.Data);
                manager.Response.Clear();
            }
        }
    }
}
