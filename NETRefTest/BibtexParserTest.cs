﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using net.sf.jabref;
using net.sf.jabref.imports;
using System.IO;
using System.Diagnostics;

namespace NETRefTest
{
    [TestClass]
    public class BibtexParserTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var parser = new BibtexParser(new StreamReader(@"../../bib/cvgip.bib"));
            var result = parser.Parse();
            var db = result.Database;

            foreach (var entry in db.getEntries())
            {
                Debug.WriteLine(entry.ToString());
            }
        }
    }
}
