namespace PulseBridge.Condo.Build.Tasks
{
    using System;
    using System.Net;
    using System.Net.Sockets;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    /// <summary>
    /// Represents a Microsoft Build task that gets information about a git repository.
    /// </summary>
    public class GetServerTime : Task
    {
        /// <summary>
        /// Gets an accurate server time from NIST represented in UTC.
        /// </summary>
        [Output]
        public DateTime UtcTime { get; private set; }

        /// <summary>
        /// Executes the <see cref="GetServerTime"/> task.
        /// </summary>
        /// <returns>
        /// A value indicating whether or not the task executed successfully.
        /// </returns>
        public override bool Execute()
        {
            // define the server and port used to connect to the time server
            var server = "time.nist.gov";
            var port = 123;

            try
            {
                // get the current address of the time server from DNS
                var addresses = Dns.GetHostEntryAsync(server).Result.AddressList;

                // create the endpoint using the first address in the response
                var endpoint = new IPEndPoint(addresses[0], port);

                // create a byte array to retain the request/response from the socket
                var data = new byte[48];
                data[0] = 0x1B;

                // create a new socket to connect to the time server
                using (var socket = new Socket(endpoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp))
                {
                    // connect to the endpoint
                    socket.Connect(endpoint);

                    // wait no more than 3 seconds for the time to be received
                    socket.ReceiveTimeout = 3000;

                    // send the request
                    socket.Send(data);

                    // get the response
                    socket.Receive(data);
                }

                // create the seconds and second fraction bits from the time server
                ulong seconds = (ulong)data[40] << 24 | (ulong)data[41] << 16 | (ulong)data[42] << 8 | (ulong)data[43];
                ulong fraction = (ulong)data[44] << 24 | (ulong)data[45] << 16 | (ulong)data[46] << 8 | (ulong)data[47];

                // calculate the total millisconds since 1900
                var ms = (seconds * 1000) + ((fraction * 1000) / 0x100000000L);

                // generate the utc time stamp from the response received via NTP
                this.UtcTime = (new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddMilliseconds(ms);

                // return the server time
                return true;
            }
            catch (Exception)
            {
                // log a warning
                this.Log.LogError("Unable to retrieve time from the time server {0} on port {1}, reverting to local time.", server, port);

                // use local time
                this.UtcTime = DateTime.UtcNow;

                // return true
                return true;
            }
        }
    }
}