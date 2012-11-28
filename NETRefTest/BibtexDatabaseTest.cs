using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using net.sf.jabref;
using System.IO;
using net.sf.jabref.imports;

namespace NETRefTest
{
    [TestClass]
    public class BibtexDatabaseTest
    {
        [TestMethod]
        public void TestCreateNew()
        {
            if (Globals.prefs == null)
            {
                Globals.prefs = JabRefPreferences.getInstance();
            }

            BibtexDatabase db = new BibtexDatabase();

            BibtexEntry entry = new BibtexEntry();
            entry.setType(BibtexEntryType.ARTICLE.Instance);

            db.insertEntry(entry);
            db.setCiteKeyForEntry(entry.getId(), "ZXY");

            foreach (var f in entry.getRequiredFields())
            {
                string value = BibtexFields.isNumeric(f) ? "1000" : "Test";
                entry.setField(f, value);
            }

            db.saveDatabase(File.Open(@"C:\Users\Peter\Desktop\bibtest.bib", FileMode.Create));
        }

        [TestMethod]
        public void TestAddNew()
        {
            if (Globals.prefs == null)
            {
                Globals.prefs = JabRefPreferences.getInstance();
            }

            ParserResult result;
            using (var parser = new BibtexParser(new StreamReader(@"C:\Users\Peter\Desktop\bibtest.bib")))
            {
                result = parser.Parse();
            }

            var db = result.Database;

            BibtexEntry entry = new BibtexEntry();
            entry.setType(BibtexEntryType.ARTICLE.Instance);

            db.insertEntry(entry);
            db.setCiteKeyForEntry(entry.getId(), "ABC");

            foreach (var f in entry.getRequiredFields())
            {
                string value = BibtexFields.isNumeric(f) ? "1300" : "Another Test";
                entry.setField(f, value);
            }

            db.saveDatabase(File.Open(@"C:\Users\Peter\Desktop\bibtest.bib", FileMode.Open));
        }
    }
}
