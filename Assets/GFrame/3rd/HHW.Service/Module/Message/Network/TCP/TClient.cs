﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace HHW.Service
{
    public class TClient : AClient
    {
        private Socket socket;
        private SocketAsyncEventArgs innArgs = new SocketAsyncEventArgs();
        private SocketAsyncEventArgs outArgs = new SocketAsyncEventArgs();

        private readonly CircularBuffer recvBuffer = new CircularBuffer();
        private readonly CircularBuffer sendBuffer = new CircularBuffer();

        private bool isSending;
        private bool isConnected;

        private readonly PacketParser parser;

        public TClient(IPEndPoint ipEndPoint, TServer server) 
            : base(server, ClientType.Connect)
        {
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.socket.NoDelay = true;
            this.parser = new PacketParser(this.recvBuffer);
            this.innArgs.Completed += this.OnComplete;
            this.outArgs.Completed += this.OnComplete;

            this.RemoteAddress = ipEndPoint;
            this.isConnected = false;
            this.isSending = false;
        }
        public TClient(Socket socket, TServer server)
            : base(server, ClientType.Accept)
        {
            this.socket = socket;
            this.socket.NoDelay = true;
            this.parser = new PacketParser(this.recvBuffer);
            this.innArgs.Completed += this.OnComplete;
            this.outArgs.Completed += this.OnComplete;

            this.RemoteAddress = (IPEndPoint)socket.RemoteEndPoint;
            this.isConnected = true;
            this.isSending = false;
        }

        public override void Dispose()
        {
            if(this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            this.socket.Close();
            this.innArgs.Dispose();
            this.outArgs.Dispose();
            this.innArgs = null;
            this.outArgs = null;
            this.socket = null;
        }

        public override void Start()
        {
            if(!this.isConnected)
            {
                this.ConnectAsync(this.RemoteAddress);
                return;
            }

            this.StartRecv();
            this.StartSend();
        }

        public override void Send(byte[] buffer, int index, int length)
        {
            if (this.IsDisposed)
            {
                throw new Exception("TClient已经被Dispose, 不能发送消息");
            }

            byte[] size = BitConverter.GetBytes((ushort)buffer.Length);
            this.sendBuffer.Write(size, 0, size.Length);
            this.sendBuffer.Write(buffer, index, length);
            if (!this.isSending)
            {
                this.StartSend();
            }
        }

        public override void Send(List<byte[]> buffers)
        {
            if (this.IsDisposed)
            {
                throw new Exception("TClient已经被Dispose, 不能发送消息");
            }

            ushort size = (ushort)buffers.Select(c => c.Length).Sum();
            byte[] sizeBuffer = BitConverter.GetBytes(size);
            this.sendBuffer.Write(sizeBuffer, 0, sizeBuffer.Length);
            foreach (byte[] buffer in buffers)
            {
                this.sendBuffer.Write(buffer, 0, buffer.Length);
            }
            if (!this.isSending)
            {
                this.StartSend();
            }
        }

        private void ConnectAsync(IPEndPoint ipEndPoint)
        {
            this.outArgs.RemoteEndPoint = ipEndPoint;
            if(this.socket.ConnectAsync(this.outArgs))
            {
                return;
            }
            OnConnectComplete(this.outArgs);
        }

        private void OnComplete(object sender, SocketAsyncEventArgs e)
        {
            switch(e.LastOperation)
            {
                case SocketAsyncOperation.Connect:
                    OnConnectComplete(e);
                    break;
                case SocketAsyncOperation.Receive:
                    OnRecvComplete(e);
                    break;
                case SocketAsyncOperation.Send:
                    OnSendComplete(e);
                    break;
                case SocketAsyncOperation.Disconnect:
                    break;
                default:
                    throw new Exception($"socket error: {e.LastOperation}");
            }
        }

        private void OnConnectComplete(object o)
        {
            if(this.socket == null)
            {
                return;
            }

            SocketAsyncEventArgs e = (SocketAsyncEventArgs)o;
            if(e.SocketError != SocketError.Success)
            {
                this.OnError(e.SocketError);
                return;
            }

            e.RemoteEndPoint = null;
            this.isConnected = true;

            this.StartRecv();
            this.StartSend();
        }

        private void OnRecvComplete(object o)
        {
            if(this.socket == null)
            {
                return;
            }
            SocketAsyncEventArgs e = (SocketAsyncEventArgs)o;
            if(e.SocketError != SocketError.Success)
            {
                this.OnError(e.SocketError);
                return;
            }
            if(e.BytesTransferred == 0)
            {
                this.OnError(e.SocketError);
                return;
            }

            this.recvBuffer.LastIndex += e.BytesTransferred;
            if(this.recvBuffer.LastIndex == this.recvBuffer.ChunkSize)
            {
                this.recvBuffer.AddLast();
                this.recvBuffer.LastIndex = 0;
            }

            while(true)
            {
                if(!this.parser.Parse())
                {
                    break;
                }

                Packet pack = this.parser.GetPacket();
                try
                {
                    this.OnRead(pack);
                }
                catch(Exception ex)
                {
                    Log.Error(ex);
                }
            }

            if(this.socket == null)
            {
                return;
            }

            this.StartRecv();
        }

        private void StartRecv()
        {
            int size = this.recvBuffer.ChunkSize - this.recvBuffer.LastIndex;
            this.RecvAsync(this.recvBuffer.Last, this.recvBuffer.LastIndex, size);
        }

        private void RecvAsync(byte[] buffer, int offset, int count)
        {
            try
            {
                this.innArgs.SetBuffer(buffer, offset, count);
            }
            catch (Exception ex)
            {
                throw new Exception($"socket set buffer error: {buffer.Length}, {offset}, {count}", ex);
            }

            if (this.socket.ReceiveAsync(this.innArgs))
            {
                return;
            }

            OnRecvComplete(this.innArgs);
        }

        private void OnSendComplete(object o)
        {
            if(this.socket == null)
            {
                return;
            }

            SocketAsyncEventArgs e = (SocketAsyncEventArgs)o;
            if(e.SocketError != SocketError.Success)
            {
                this.OnError(e.SocketError);
                return;
            }

            this.sendBuffer.FirstIndex += e.BytesTransferred;
            if(this.sendBuffer.FirstIndex == this.sendBuffer.ChunkSize)
            {
                this.sendBuffer.FirstIndex = 0;
                this.sendBuffer.RemoveFirst();
            }
            if(this.sendBuffer.Length == 0)
            {
                this.isSending = false;
                return;
            }

            this.StartSend();
        }

        private void StartSend()
        {
            if(!this.isConnected)
            {
                return;
            }
            this.isSending = true;
            int sendSize = this.sendBuffer.ChunkSize - this.sendBuffer.FirstIndex;
            if(sendSize > this.sendBuffer.Length)
            {
                sendSize = (int)this.sendBuffer.Length;
            }

            this.SendAsync(this.sendBuffer.First, this.sendBuffer.FirstIndex, sendSize);
        }

        private void SendAsync(byte[] buffer, int offset, int count)
        {
            try
            {
                this.outArgs.SetBuffer(buffer, offset, count);
            }
            catch(Exception ex)
            {
                throw new Exception($"socket set buffer error: {buffer.Length}, {offset}, {count}", ex);
            }
            if(this.socket.SendAsync(this.outArgs))
            {
                return;
            }
            OnSendComplete(this.outArgs);
        }

        private void OnDisconnectComplete(object o)
        {
            SocketAsyncEventArgs e = (SocketAsyncEventArgs)o;
            this.OnError(e.SocketError);
        }
    }
}
