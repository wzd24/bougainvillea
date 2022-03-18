using CSRedis.Internal.IO;
using System;
using System.Globalization;

namespace CSRedis.Internal.Commands
{
    internal class RedisFloat : RedisCommand<double>
    {
        public RedisFloat(string command, params object[] args)
            : base(command, args)
        { }

        public override double Parse(RedisReader reader)
        {
            return FromString(reader.ReadBulkString());
        }

        private static double FromString(string input)
        {
            return double.Parse(input, NumberStyles.Any);
        }

        public class Nullable : RedisCommand<double?>
        {
            public Nullable(string command, params object[] args)
                : base(command, args)
            { }

            public override double? Parse(RedisReader reader)
            {
                var result = reader.ReadBulkString();
                if (string.IsNullOrEmpty(result))
                    return null;
                return FromString(result);
            }
        }
    }
}
