using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Threading;
using Terminal.UI;
using xLibV100.Common;
using xLibV100.Controls;
using xLibV100.Net;
using xLibV100.Net.MQTT;
using xLibV100.Ports;
using xLibV100.UI;

namespace Terminal
{
    public partial class Control : TerminalBase
    {
        public List<ViewModelBase> ViewModels { get; set; }

        public Control(object parent) : base()
        {
            Name = nameof(Terminal);

            PortAdded += PortAddedHandler;
            PortRemoved += PortRemovedHandler;

            if (parent is DispatcherObject dispatcher)
            {
                xSupport.Context = dispatcher;
            }

            List<PortSerializer> ports;
            List<PortBase> servers = new List<PortBase>();
            List<PortBase> clients = new List<PortBase>();

            string subpath = "Components\\Terminal\\Saves";
            Json.Open(Environment.CurrentDirectory + "\\" + subpath + "\\ports.json", out ports);

            if (ports != null)
            {
                foreach (PortBase port in ports)
                {
                    PortBase createdPort;
                    if (port.Type == typeof(SerialPort).ToString())
                    {
                        if (port.Options != null)
                        {
                            var options = JsonSerializer.Deserialize<SerialPortOptions>((JsonElement)port.Options, new JsonSerializerOptions());
                            port.Options = options;
                        }
                        createdPort = new SerialPort(port);
                    }
                    else if (port.Type == typeof(TCPClient).ToString())
                    {
                        if (port.Options != null)
                        {
                            var options = JsonSerializer.Deserialize<TCPClientOptions>((JsonElement)port.Options, new JsonSerializerOptions());
                            port.Options = options;
                        }
                        createdPort = new TCPClient(port);
                    }
                    else if (port.Type == typeof(TCPServer).ToString())
                    {
                        if (port.Options != null)
                        {
                            var options = JsonSerializer.Deserialize<TCPServerOptions>((JsonElement)port.Options, new JsonSerializerOptions());
                            port.Options = options;
                        }
                        createdPort = TCPServer.Create(port);
                    }
                    else if (port.Type == typeof(MqttBroker).ToString())
                    {
                        if (port.Options != null)
                        {
                            var options = JsonSerializer.Deserialize<MqttBrokerOptions>((JsonElement)port.Options, new JsonSerializerOptions());
                            port.Options = options;
                        }
                        createdPort = new MqttBrokerViewModel(port);

                        string fileName = "subports_" + port.Name + "_" + port.Id + ".json";
                        List<PortSerializer> subPorts = null;
                        Json.Open(Environment.CurrentDirectory + "\\" + subpath + "\\" + fileName, out subPorts);

                        if (subPorts != null)
                        {
                            foreach (var element in subPorts)
                            {
                                if (element.Type == typeof(MqttBrokerTopic).ToString())
                                {
                                    var subPort = new MqttBrokerTopic(element);
                                    if (element.Options != null)
                                    {
                                        var options = JsonSerializer.Deserialize<MqttBrokerTopicOptions>((JsonElement)element.Options, new JsonSerializerOptions());
                                        subPort.Options = options;
                                    }
                                    createdPort.AddSubPort(subPort);
                                }
                            }
                        }
                    }
                    else if (port.Type == typeof(MqttClient).ToString())
                    {
                        if (port.Options != null)
                        {
                            var options = JsonSerializer.Deserialize<MqttClientOptions>((JsonElement)port.Options, new JsonSerializerOptions());
                            port.Options = options;
                        }

                        createdPort = new MqttClient(port);

                        string fileName = "subports_" + port.Name + "_" + port.Id + ".json";
                        List<PortSerializer> subPorts = null;
                        Json.Open(Environment.CurrentDirectory + "\\" + subpath + "\\" + fileName, out subPorts);

                        if (subPorts != null)
                        {
                            foreach (var element in subPorts)
                            {
                                if (element.Type == typeof(MqttTopic).ToString())
                                {
                                    var subPort = new MqttTopic(element);

                                    if (element.Options != null)
                                    {
                                        var options = JsonSerializer.Deserialize<MqttTopicOptions>((JsonElement)element.Options, new JsonSerializerOptions());
                                        subPort.Options = options;
                                    }
                                    createdPort.AddSubPort(subPort);
                                }
                            }
                        }
                    }
                    else
                    {
                        continue;
                    }

                    if (createdPort != null)
                    {
                        AddPort(createdPort);

                        switch (port.State)
                        {
                            case States.Connected:
                                clients.Add(createdPort);
                                break;

                            case States.Started:
                                servers.Add(createdPort);
                                break;
                        }
                    }
                }
            }

            Task.Run(async () =>
            {
                foreach (var port in servers)
                {
                    await port.StartAsync();
                }

                foreach (var port in clients)
                {
                    await port.ConnectAsync();
                }
            });

            ViewModels = new List<ViewModelBase>();

            SerialPort.CoreStart();
        }

        private void PortAddedHandler(TerminalBase terminal, PortBase port)
        {
            //port.SubPortAdded += SubPortAddedHandler;
        }

        private void PortRemovedHandler(TerminalBase terminal, PortBase port)
        {
            //port.SubPortRemoved += SubPortRemovedHandler;
        }

        private void SubPortRemovedHandler(PortBase port, PortBase subPort)
        {
            subPort.PacketReceiver -= PacketReceiver;
        }

        private void SubPortAddedHandler(PortBase port, PortBase subPort)
        {
            subPort.PacketReceiver += PacketReceiver;
        }

        public override void Dispose()
        {
            base.Dispose();

            foreach (var element in ViewModels)
            {
                element.Dispose();
            }

            SerialPort.CoreStop();

            string subpath = "Components\\Terminal\\Saves";
            Json.CreateFolder(Environment.CurrentDirectory, subpath);

            if (AvailablePorts.Count > 0)
            {
                PortBase[] ports = new PortBase[AvailablePorts.Count];

                int i = 0;
                foreach (var port in AvailablePorts)
                {
                    ports[i] = port;
                    i++;
                }

                Json.Save(Environment.CurrentDirectory + "\\" + subpath + "\\ports" + ".json", ports);

                foreach (var port in ports)
                {
                    if (port.SubPorts != null && port.SubPorts.Count > 0)
                    {
                        Json.Save(Environment.CurrentDirectory + "\\" + subpath + "\\subports_" + port.Name + "_" + port.Id + ".json", port.SubPorts);
                    }
                }

                foreach (PortBase port in AvailablePorts)
                {
                    port.Dispose();
                }
            }
        }
    }
}
