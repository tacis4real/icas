using System;
using System.Linq;

namespace ICAS.Areas.Helpers
{
    public class IDGenerator
    {

        internal static string RandCodeGen(int length)
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}