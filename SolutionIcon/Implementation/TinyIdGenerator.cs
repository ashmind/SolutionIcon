using System;
using System.Collections.Generic;
using System.Linq;
using AshMind.Extensions;

namespace SolutionIcon.Implementation {
    public class TinyIdGenerator {
        public string GetTinyId(string name) {
            Argument.NotNullOrEmpty("name", name);

            // this can all be done with one regexp, but I find this easier to read
            var lastPart = name.SubstringAfterLast("."); // e.g. in Company.Whatever.MyThing -- MyThing is the most important
            var idChar1 = lastPart[0]; // MyThing => M, myThing => m
            var idChar2 = lastPart.Skip(1).Where(Char.IsUpper).Cast<char?>().FirstOrDefault(); // MyThing => T, myThing => T
            if (idChar2 == null && !idChar1.IsUpper() && lastPart.Length > 1)
                idChar2 = lastPart[1]; // mything => y

            if (idChar2 == null)
                return new string(idChar1, 1);

            return new string(new[] { idChar1, idChar2.Value });
        }
    }
}
