using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using Xunit;

namespace SocketPoll
{
    public class TestSuite
    {
        [Fact]
        public void ImmediateConnectionCheckWithPlainSocket()
        {
            const int connectionCheckTimeout = 3005000;
            var stopwatch = new Stopwatch();
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                socket.Bind(new IPEndPoint(IPAddress.Loopback, 0));

                // socket.Poll(timeout, selectMode)
                //
                // SelectMode.SelectRead
                // true if Listen(Int32) has been called and a connection is pending
                // true if data is available for reading
                // true if the connection has been closed, reset, or terminated
                // false otherwise
                //
                // SelectMode.SelectWrite
                // true if processing a Connect(EndPoint) and the connection has succeeded
                // true if data can be sent
                // false otherwise
                //
                // The Poll method will check the state of the Socket:
                //  - SelectMode.SelectRead to determine if the Socket is readable
                //  - SelectMode.SelectWrite to determine if the Socket is writable
                //  - SelectError to detect an error condition
                // Poll will block execution until the specified time period, measured in microseconds, elapses
                // This method cannot detect certain kinds of connection problems, such as a broken network cable,
                // or that the remote host was shut down ungracefully
                // You must attempt to send or receive data to detect these kinds of errors
                stopwatch.Restart();
                var pollResult = socket.Poll(connectionCheckTimeout, SelectMode.SelectRead);
                stopwatch.Stop();

                Assert.False(pollResult);
                Assert.Equal(0, stopwatch.ElapsedMilliseconds / 1000);
            }
        }
    }
}