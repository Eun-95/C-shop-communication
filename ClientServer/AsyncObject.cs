﻿using System;
using System.Net.Sockets;

namespace ClientServer
{
    // 비동기 작업에서 사용하는 소켓과 해당 작업에 대한 데이터 버퍼를 저장하는 클래스
    public class AsyncObject
    {
        public string id;
        public Socket WorkingSocket;
        public readonly int BufferSize;
        public byte[] Buffer;
        public AsyncObject(int bufferSize)
        {
            BufferSize = bufferSize;
            Buffer = new byte[BufferSize];
        }

        public void ClearBuffer()
        {
            Array.Clear(Buffer, 0, BufferSize);
        }
    }
}