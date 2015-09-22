using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Text;

namespace TBD2PROYECTO2.Managers
{
    public static class Parser
    {
        public static string  Parse(string hex, Types elementTypes)
        {
            switch (elementTypes)
            {
                case Types.Char:
                    return HexToChar(hex);
                case Types.VarChar:
                    return HexToString(hex);
                case Types.DateTime:
                    return HexToDate(hex).ToString(CultureInfo.InvariantCulture);
                case Types.SmallDateTime:
                    return HexToSDate(hex).ToString(CultureInfo.InvariantCulture);
                case Types.Int:
                    return "" + HexToInt(hex);
                case Types.BigInt:
                    return "" + HexToBInt(hex);
                case Types.TinyInt:
                    return "" + HexToTinyInt(hex);
                case Types.Decimal:
                    return "" + HexToDecimal(hex);
                case Types.Money:
                    return "" + HexToMoney(hex);
                case Types.Float:
                    return "" + HexToFloat(hex);
                case Types.Real:
                    return "" + HexToReal(hex);
                case Types.Numeric:
                    return "" + HexToNumeric(hex);
                case Types.Bit:
                    return "" + BitIntVal(hex);
                case Types.Binary:
                    return "" + HexBinaryString(hex);
                default:
                    return null;
            }
        }

        public static string HexToChar(string hex)
        {
            string result = "";
            for (int i = 0; i < hex.Length; i += 2)
            {
                result += (char)Int16.Parse(hex.Substring(i, 2), NumberStyles.AllowHexSpecifier);
            }
            result = result.Trim();
            return result;
        }

        public static double HexToMoney(string hex)
        {
            var stringlist = new List<string>();
            for (int i = 0; i < hex.Length; i += 2)
            {
                stringlist.Add(hex.Substring(i, 2));
            }
            stringlist.Reverse();
            stringlist.RemoveAt(stringlist.Count - 1);
            string bigEndian = String.Join("", stringlist);
            return Convert.ToInt64(bigEndian, 16);
        }

        public static string HexToString(string hex)
        {
            return HexToChar(hex);
        }

        public static DateTime HexToDate(string hex)
        {
            string sDate = string.Empty;
            for (int i = 0; i < hex.Length - 1; i += 2)
            {
                string ss = hex.Substring(i, 2);
                int nn = int.Parse(ss, NumberStyles.AllowHexSpecifier);
                string c = Char.ConvertFromUtf32(nn);
                sDate += c;
            }
            CultureInfo provider = CultureInfo.InvariantCulture;
            return DateTime.ParseExact(sDate, "yyyyMMddHHmmss", provider);

        }

        public static DateTime HexToSDate(string hex)
        {
            UInt32 secondsAfterEpoch = (uint)HexToInt(hex);
            DateTime epoch = new DateTime(1900, 1, 1);
            DateTime myDateTime = epoch.AddMinutes(secondsAfterEpoch);
            return myDateTime;
        }

        public static Int32 HexToInt(string hex)
        {
            var stringlist = new List<string>();
            for (int i = 0; i < 8; i += 2){
                stringlist.Add(hex.Substring(i, 2));
            }
            stringlist.Reverse();
            string bigEndian = String.Join("", stringlist);
            
            return Int32.Parse(bigEndian, NumberStyles.HexNumber); ;
        }



        public static Int16 HexToSInt(string hex)
        {
            var stringlist = new List<string>();
            for (int i = 0; i < 4; i += 2) {
                stringlist.Add(hex.Substring(i, 2));
            }
            stringlist.Reverse();
            string bigEndian = String.Join("", stringlist);

            return Int16.Parse(bigEndian, NumberStyles.HexNumber); ;
        }

        public static Int64 HexToBInt(string hex)
        {
            var stringlist = new List<string>();
            for (int i = 0; i < 16; i += 2){
                stringlist.Add(hex.Substring(i, 2));
            }
            stringlist.Reverse();
            string bigEndian = String.Join("", stringlist);
            return Int64.Parse(bigEndian, NumberStyles.HexNumber);
        }

        public static byte HexToTinyInt(string hex)
        {
            return Byte.Parse(hex, NumberStyles.HexNumber);
        }

        public static decimal HexToDecimal(string hex)
        {

            var stringlist = new List<string>();
            for (int i = 0; i < hex.Length; i += 2){
                stringlist.Add(hex.Substring(i, 2));
            }
            stringlist.Reverse();
            stringlist.RemoveAt(stringlist.Count - 1);
            string bigEndian = String.Join("", stringlist);
            Console.WriteLine(bigEndian);
            return Convert.ToInt64(bigEndian, 16);
        }

        public static double HexToFloat(string hex)
        {
            return BitConverter.Int64BitsToDouble(HexToBInt(hex));
        }

        public static Single HexToReal(string hex) 
        {
            var stringlist = new List<string>();
            for (int i = 0; i < hex.Length; i += 2){
                stringlist.Add(hex.Substring(i, 2));
            }
            stringlist.Reverse();
            var bytes = new byte[hex.Length / 2];
            for (int i = 0; i < stringlist.Count; i++){
                bytes[i] = HexToTinyInt(stringlist.ElementAt(i));
            }
            return BitConverter.ToSingle(bytes, 0);
        }

        public static decimal HexToNumeric(string hex)
        {
            return HexToDecimal(hex);
        }

        private static int BitIntVal(string hex)
        {
            return (ConvertToBit(hex) ? 1 : 0);
        }

        public static  bool ConvertToBit(string hex)
        {
            return hex == "01";
        }

        public static string HexBinaryString(string hex)
        {
            var bytes = HexToBinary(hex);
            //var sstringbytes = "";

            //foreach (var b   in bytes)
            //{
            //    var s = Convert.ToString(b, 2);
            //    sstringbytes += s + " ";
            //}

            string res = BitConverter.ToString(bytes);
            return "0x" +  res.Replace("-", "");

            //return res;
        }
        public static byte[] HexToBinary(string hex)
        {
            var bytesList = new List<byte>();
            for (int i = 0; i < hex.Length; i += 2)
            {
                bytesList.Add(HexToTinyInt(hex.Substring(i, 2)));
            }
            bytesList.Reverse();
            int j = 0;
            while (bytesList.ElementAt(j) == 0) {
                bytesList.RemoveAt(j);
                j++;
            }
            bytesList.Reverse();
            return bytesList.ToArray();
        }



        public static string FromDateToHex(DateTime myDate)
        {
            string isoDate = myDate.ToString("yyyyMMddHHmmss");

            string resultString = string.Empty;

            for (int i = 0; i < isoDate.Length; i++) 
            {
                int n = char.ConvertToUtf32(isoDate, i);
                string hs = n.ToString("x");
                resultString += hs;

            }
            return resultString;
        }
    }
}
