using System;
using System.Collections.Generic;
using System.Linq;
using TBD2PROYECTO2.DataObjects;

namespace TBD2PROYECTO2.Managers
{
    public class Converter
    {

        private static Tuple<int, short, string> rowVals(ref string log)
        {
            var offset = (Parser.HexToSInt(log.Substring(4, 4)) * 2) - 2;
            log = log.Substring(8);
            var varLength = Parser.HexToSInt(log.Substring(offset, 4));
            var varColumns = log.Substring(offset + 4);

            return Tuple.Create(offset, varLength, varColumns);
        }

        private static void withVarying(ref List<string> result, string log, List<ColumnEntity> datos)
        {
            var info = rowVals(ref log);
            var cont = 0;
            for (var i = 0; i < datos.Count - info.Item2; i++)
            {
                result.Add(Parser.Parse(log.Substring(cont, datos[i].Length * 2), datos[i].Types));
                cont = cont + datos.ElementAt(i).Length * 2;
            }
            cont = 0;
            var del = new List<int> { (info.Item2 * 4) };
            for (var i = 0; i < info.Item2; i++)
            {
                del.Add(((Parser.HexToSInt(info.Item3.Substring(cont, 4)) * 2)) - (info.Item1 + 12));
                cont = cont + 4;
            }
            for (var i = 0; i < info.Item2; i++)
            {
                if (i != info.Item2 - 1)
                {

                    var newVal = Parser.Parse(info.Item3.Substring(del[i], del[i + 1] - (del[i])), datos.ElementAt(datos.Count - info.Item2 + i).Types);
                    result.Add(newVal);
                }
                else
                {
                    var newVal = Parser.Parse(info.Item3.Substring(del[i]), datos.ElementAt(datos.Count - 1).Types);
                    result.Add(newVal);
                }

            }
        }

        private static void withConstant(ref string log, ref List<string> result, List<ColumnEntity> datos)
        {
            log = log.Substring(8);

            for (int i = 0, n = 0; i < datos.Count; i++)
            {
                try
                {
                    result.Add(Parser.Parse(log.Substring(n, datos.ElementAt(i).Length * 2),
                        datos.ElementAt(i).Types));
                    n += datos.ElementAt(i).Length * 2;
                }
                catch (Exception ex)
                {
                    result.Add("NULL");
                }
            }
        }

        public static List<string> ParseLog(string log, List<ColumnEntity> datos)
        {
            var result = new List<string>();
            if (log.StartsWith("3"))
            {
                withVarying(ref result, log, datos);
            }
            else
            {
                withConstant(ref log, ref result, datos);
            }
            return result;
        }

        
    }
}
