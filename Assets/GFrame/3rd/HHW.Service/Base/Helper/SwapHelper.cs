﻿namespace HHW.Service
{
    public static class SwapHelper
    {
        public static void Swap<T>(ref T t1, ref T t2)
        {
            T t3 = t1;
            t1 = t2;
            t2 = t3;
        }
    }
}
