﻿using System.Threading;

namespace HHW.Service
{
    public static class IdGenerater
    {
        public static long AppId { private get; set; }
        private static int value;

        public static long GenerateId()
        {
            long time = TimeHelper.ClientNowSeconds();

            return (AppId << 48) + (time << 16) + ((ushort)Interlocked.Increment(ref value));
        }
    }
}
