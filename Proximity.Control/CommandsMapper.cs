using System;
using System.Collections.Generic;

namespace Proximity.Control
{
    public static class CommandsMapper {
        public static void Map(string arg, IDictionary<string, Action> mapping, Action defaultAction = null, bool ignoreCase = false) {
            if (arg == null) {
                throw new ArgumentNullException(nameof(arg));
            }

            if (mapping == null) {
                throw new ArgumentNullException(nameof(mapping));
            }

            if (!mapping.ContainsKey(arg) && defaultAction != null) {
                defaultAction();
            }
            else {
                mapping[arg]();
            }
        }
    }
}
