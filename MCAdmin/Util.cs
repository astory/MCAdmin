using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiscUtil.Conversion;

namespace MCAdmin
{
    public static class Util
    {
        static Encoding enc = new UTF8Encoding(false);

        #region Networking ByteArray Encoders


        public static short AtoN(byte[] arr, int start)
        {
            return EndianBitConverter.Big.ToInt16(arr, start);
        }
        public static byte[] NtoA(short num)
        {
            return EndianBitConverter.Big.GetBytes(num); ;
        }
        public static void NinA(short num, byte[] arr, int start)
        {
            Util.NtoA(num).CopyTo(arr, start);
        }

        public static int AtoI(byte[] arr, int start)
        {
            return EndianBitConverter.Big.ToInt32(arr, start);
        }

        public static double AtoD(byte[] arr, int start)
        {
            return EndianBitConverter.Big.ToInt64(arr, start);
        }
        public static byte[] DtoA(double num)
        {
            return EndianBitConverter.Big.GetBytes(num);
        }
        public static void DinA(double num, byte[] arr, int start)
        {
            Util.DtoA(num).CopyTo(arr, start);
        }

        public static float AtoF(byte[] arr, int start)
        {
            return EndianBitConverter.Big.ToSingle(arr, start);
        }
        public static byte[] FtoA(float num)
        {
            return EndianBitConverter.Big.GetBytes(num);
        }
        public static void FinA(float num, byte[] arr, int start)
        {
            Util.FtoA(num).CopyTo(arr, start);
        }

        public static long AtoL(byte[] arr, int start)
        {
            return EndianBitConverter.Big.ToInt64(arr, start);
        }
        public static byte[] LtoA(long num)
        {
            return EndianBitConverter.Big.GetBytes(num);
        }
        public static void LinA(long num, byte[] arr, int start)
        {
            Util.LtoA(num).CopyTo(arr, start);
        }

        /*public static byte[] VtoA(Vector vec)
        {
            byte[] ret = new byte[6];
            Util.NinA(vec.X, ret, 0);
            Util.NinA(vec.Y, ret, 2);
            Util.NinA(vec.Z, ret, 4);
            return ret;
        }
        public static void VinA(Vector vec, byte[] arr, int start)
        {
            Util.NinA(vec.X, arr, start);
            Util.NinA(vec.Y, arr, start + 2);
            Util.NinA(vec.Z, arr, start + 4);
        }
        public static Vector AtoV(byte[] arr, int start)
        {
            return new Vector(Util.AtoN(arr, start), Util.AtoN(arr, start + 2), Util.AtoN(arr, start + 4));
        }*/

        public static byte[] StoA(string str)
        {
            byte[] strb = enc.GetBytes(str);
            if(strb.Length > short.MaxValue) return new byte[] { 0x00, 0x00 };
            byte[] ret = new byte[strb.Length + 2];
            strb.CopyTo(ret, 2);
            Util.NinA((short)strb.Length, ret, 0);
            return ret;
        }
        public static void SinA(string str, byte[] arr, int start)
        {
            Util.StoA(str).CopyTo(arr, start);
        }

        public static string AtoS(byte[] arr, int start)
        {
            return enc.GetString(arr, start + 2, Util.AtoN(arr, start));
        }
        #endregion

        public static bool ContainsInvalidChars(string str, bool isname)
        {
            foreach (char c in str)
            {
                if (c == ' ') return true;
                //if (c < 48 || c > 122 || (c > 57 && c < 65) || (c > 90 && c < 97)) return true;
            }
            return false;
        }
    }
}
