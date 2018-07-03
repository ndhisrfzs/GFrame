namespace GFrame
{
    internal class CalcBytes
    {
        public static ulong[] ConvertToUlongs(byte[] data)
        {
            ulong[] result = new ulong[6];
            int indexR = 0;
            byte pV = 0;
            byte pVLast = 125;
            ulong vL = 0;
            for (int i = 0, len = data.Length; i < len; i++)
            {
                pV = data[i];
                result[indexR] += (uint)((pV << pVLast) + pV) + vL;
                vL = result[indexR];
                indexR++;
                if (indexR > 5) { indexR = 0; }

                pVLast = pV;
            }

            return result;
        }


        public static ulong[] ConvertToUlongs2(byte[] data)
        {
            ulong[] result = new ulong[6];
            int indexR = 0;
            byte pV = 0;
            byte pVLast = 36;
            ulong vL = 0;
            for (int i = 0, len = data.Length; i < len; i++)
            {
                pV = data[i];
                result[indexR] += (uint)((pV >> pVLast) + pV) - vL;
                vL = result[indexR];
                indexR++;
                if (indexR > 5) { indexR = 0; }

                pVLast = pV;
            }

            return result;
        }
    }
}