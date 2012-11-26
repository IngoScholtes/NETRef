/*  Copyright (C) 2003-2011 JabRef contributors.
    This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along
    with this program; if not, write to the Free Software Foundation, Inc.,
    51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
*/
using System.Collections.Generic;
using System.IO;
namespace net.sf.jabref.imports {


public class ParserResult {

    private BibtexDatabase database;
    private Dictionary<string, string> metaData;
    private Dictionary<string, BibtexEntryType> entryTypes;


    private Stream file = null;
    private List<string> _warnings = new List<string>();
    private List<string> duplicateKeys = new List<string>();

    private string errorMessage = null;
    private string encoding = null; // Which encoding was used?

    private bool postponedAutosaveFound = false;
    private bool invalid = false;

    private string jabrefVersion = null; // Which JabRef version wrote the file, if any?
    private int jabrefMajorVersion = 0;
    private int jabrefMinorVersion = 0;
    private int jabrefMinor2Version = 0; // Numeric version representation
    private bool _toOpenTab = false;
    
    public ParserResult(BibtexDatabase database, Dictionary<string, string> metaData, Dictionary<string, BibtexEntryType> entryTypes) {
		this.database = database;
		this.metaData = metaData;
		this.entryTypes = entryTypes;
    }

    /**
     * Check if this database is marked to be added to the currently open tab. Default is false.
     * @return
     */
    public bool toOpenTab() {
        return _toOpenTab;
    }

    public void setToOpenTab(bool toOpenTab) {
        this._toOpenTab = toOpenTab;
    }


    /**
     * Find which version of JabRef, if any, produced this bib file.
     * @return The version number string, or null if no JabRef signature could be read.
     */
    public string getJabrefVersion() {
        return jabrefVersion;
    }

    /**
     * Set the JabRef version number string for this parser result.
     * @param jabrefVersion The version number string.                                         
     */
    public void setJabrefVersion(string jabrefVersion) {
        this.jabrefVersion = jabrefVersion;
    }


    public int getJabrefMajorVersion() {
        return jabrefMajorVersion;
    }

    public void setJabrefMajorVersion(int jabrefMajorVersion) {
        this.jabrefMajorVersion = jabrefMajorVersion;
    }

    public int getJabrefMinorVersion() {
        return jabrefMinorVersion;
    }

    public void setJabrefMinorVersion(int jabrefMinorVersion) {
        this.jabrefMinorVersion = jabrefMinorVersion;
    }

    public int getJabrefMinor2Version() {
        return jabrefMinor2Version;
    }

    public void setJabrefMinor2Version(int jabrefMinor2Version) {
        this.jabrefMinor2Version = jabrefMinor2Version;
    }
    
    public BibtexDatabase getDatabase() {
    	return database;
    }

    public Dictionary<string, string> getMetaData() {
	return metaData;
    }

    public Dictionary<string, BibtexEntryType> getEntryTypes() {
    	return entryTypes;
    }

    public Stream getFile() {
      return file;
    }

    public void setFile(Stream f) {
      file = f;
    }

    /**
     * Sets the variable indicating which encoding was used during parsing.
     *
     * @param enc string the name of the encoding.
     */
    public void setEncoding(string enc) {
      encoding = enc;
    }

    /**
     * Returns the name of the encoding used during parsing, or null if not specified
     * (indicates that prefs.get("defaultEncoding") was used).
     */
    public string getEncoding() {
      return encoding;
    }

    /**
     * Add a parser warning.
     *
     * @param s string Warning text. Must be pretranslated. Only added if there isn't already a dupe.
     */
    public void addWarning(string s) {
        if (!_warnings.Contains(s))
            _warnings.Add(s);
    }

    public bool hasWarnings() {
        return (_warnings.Count > 0);
    }

    public string[] warnings() {
      string[] s = new string[_warnings.Count];
      for (int i=0; i<_warnings.Count; i++)
        s[i] = _warnings[i];
      return s;
    }

    /**
     * Add a key to the list of duplicated BibTeX keys found in the database.
     * @param key The duplicated key
     */
    public void addDuplicateKey(string key) {
        if (!duplicateKeys.Contains(key))
            duplicateKeys.Add(key);
    }

    /**
     * Query whether any duplicated BibTeX keys have been found in the database.
     * @return true if there is at least one duplicate key.
     */
    public bool hasDuplicateKeys() {
        return duplicateKeys.Count > 0;
    }

    /**
     * Get all duplicated keys found in the database.
     * @return An array containing the duplicated keys.
     */
    public string[] getDuplicateKeys() {
        return duplicateKeys.ToArray();
    }
    

    public bool isPostponedAutosaveFound() {
        return postponedAutosaveFound;
    }

    public void setPostponedAutosaveFound(bool postponedAutosaveFound) {
        this.postponedAutosaveFound = postponedAutosaveFound;
    }

    public bool isInvalid() {
        return invalid;
    }

    public void setInvalid(bool invalid) {
        this.invalid = invalid;
    }

    public string getErrorMessage() {
        return errorMessage;
    }

    public void setErrorMessage(string errorMessage) {
        this.errorMessage = errorMessage;
    }
}
}
