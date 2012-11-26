/*
Copyright (C) 2003 David Weitzman, Morten O. Alver

All programs in this directory and
subdirectories are published under the GNU General Public License as
described below.

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2 of the License, or (at
your option) any later version.

This program is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307
USA

Further information about the GNU GPL is available at:
http://www.gnu.org/copyleft/gpl.ja.html

Note:
Modified for use in JabRef.

*/

using System;
using System.Collections.Generic;
namespace net.sf.jabref {


public class BibtexEntry
{
    public readonly static string ID_FIELD = "id";
    private string _id;
    private BibtexEntryType _type;
    private Dictionary<string, string> _fields = new Dictionary<string, string>();

    // Search and grouping status is stored in bool fields for quick reference:
    private bool groupHit;

    public BibtexEntry()
        : this(Util.createNeutralId())
    {
    }

    public BibtexEntry(string id)
        : this(id, BibtexEntryType.OTHER.Instance)
    {
    }

    public BibtexEntry(string id, BibtexEntryType type)
    {
        if (id == null)
        {
            throw new NullReferenceException("Every BibtexEntry must have an ID");
        }

        _id = id;
        setType(type);
    }

    /**
     * Returns an array describing the optional fields for this entry.
     */
    public string[] getOptionalFields()
    {
        return _type.getOptionalFields();
    }

    /**
     * Returns an array describing the required fields for this entry.
     */
    public string[] getRequiredFields()
    {
        return _type.getRequiredFields();
    }

    /**
     * Returns an set containing the names of all fields that are
     * set for this particular entry.
     */
    public Dictionary<string, bool> getAllFields() {
        var d = new Dictionary<string, bool>();
        foreach (var k in _fields.Keys)
        {
            d.Add(k, true);
        }
        return d;
    }

    /**
     * Returns a string describing the required fields for this entry.
     */
    public string describeRequiredFields()
    {
        return _type.describeRequiredFields();
    }

    /**
     * Returns true if this entry contains the fields it needs to be
     * complete.
     */
    public bool hasAllRequiredFields(BibtexDatabase database)
    {
        return _type.hasAllRequiredFields(this, database);
    }

    /**
     * Returns this entry's type.
     */
    public BibtexEntryType getType()
    {
        return _type;
    }

    /**
     * Sets this entry's type.
     */
    public void setType(BibtexEntryType type)
    {
        if (type == null)
        {
            throw new NullReferenceException(
                "Every BibtexEntry must have a type.  Instead of null, use type OTHER");
        }
        
        _type = type;
    }

    /**
     * Prompts the entry to call BibtexEntryType.getType(string) with
     * its current type name as argument, and sets its type according
     * to what is returned. This method is called when a user changes
     * the type customization, to make sure all entries are set with
     * current types.
     * @return true if the entry could find a type, false if not (in
     * this case the type will have been set to
     * BibtexEntryType.TYPELESS).
     */
    public bool updateType() {
        BibtexEntryType newType = BibtexEntryType.getType(_type.getName());
        if (newType != null) {
            _type = newType;
            return true;
        }
        _type = BibtexEntryType.TYPELESS.Instance;
        return false;
    }

    /**
     * Sets this entry's ID, provided the database containing it
     * doesn't veto the change.
     */
    public void setId(string id) {

        if (id == null) {
            throw new
                NullReferenceException("Every BibtexEntry must have an ID");
        }

        _id = id;
    }

    /**
     * Returns this entry's ID.
     */
    public string getId()
    {
        return _id;
    }

    /**
     * Returns the contents of the given field, or null if it is not set.
     */
    public string getField(string name) {
        return _fields.ContainsKey(name) ? _fields[name] : null;
    }

    public string getCiteKey() {
        return (_fields.ContainsKey(Globals.KEY_FIELD) ?
                _fields[(Globals.KEY_FIELD)] : null);
    }

    /**
     * Sets a number of fields simultaneously. The given Dictionary contains field
     * names as keys, each mapped to the value to set.
     * WARNING: this method does not notify change listeners, so it should *NOT*
     * be used for entries that are being displayed in the GUI. Furthermore, it
     * does not check values for content, so e.g. empty strings will be set as such.
     */
    public void setField(Dictionary<string, string> fields){
        foreach (var k in fields.Keys)
        {
            _fields.Add(k, fields[k]);
        }
    }

    /**
     * Set a field, and notify listeners about the change.
     *
     * @param name The field to set.
     * @param value The value to set.
     */
    public void setField(string name, string value) {

        if (ID_FIELD.Equals(name)) {
            throw new ArgumentException("The field name '" + name +
                                               "' is reserved");
        }

        _fields.Add(name, value);
    }

    /**
     * Remove the mapping for the field name, and notify listeners about
     * the change.
     *
     * @param name The field to clear.
     */
    public void clearField(string name) {

      if (ID_FIELD.Equals(name)) {
           throw new ArgumentException("The field name '" + name +
                                              "' is reserved");
       }
      
       _fields.Remove(name);
    }

    /**
     * Determines whether this entry has all the given fields present. If a non-null
     * database argument is given, this method will try to look up missing fields in
     * entries linked by the "crossref" field, if any.
     *
     * @param fields An array of field names to be checked.
     * @param database The database in which to look up crossref'd entries, if any. This
     *  argument can be null, meaning that no attempt will be made to follow crossrefs.
     * @return true if all fields are set or could be resolved, false otherwise.
     */
    public bool allFieldsPresent(string[] fields, BibtexDatabase database) {
        for (int i = 0; i < fields.Length; i++) {
            if (BibtexDatabase.getResolvedField(fields[i], this, database) == null) {
                return false;
            }
        }

        return true;
    }
    
    /**
     * Returns a clone of this entry. Useful for copying.
     */
    public object clone() {
        BibtexEntry clone = new BibtexEntry(_id, _type);
        clone._fields = new Dictionary<string, string>(_fields); 
        return clone;
    }

    public string toString() {
        return getType().getName()+":"+getField(Globals.KEY_FIELD);
    }
    
    public bool isGroupHit() {
        return groupHit;
    }

    public void setGroupHit(bool groupHit) {
        this.groupHit = groupHit;
    }

    /**
     * @param maxCharacters The maximum number of characters (additional
     * characters are replaced with "..."). Set to 0 to disable truncation.
     * @return A short textual description of the entry in the format:
     * Author1, Author2: Title (Year)
     */
    public string getAuthorTitleYear(int maxCharacters) {
        string[] s = new string[] {
                getField("author"),
                getField("title"),
                getField("year")};
        for (int i = 0; i < s.Length; ++i)
            if (s[i] == null)
                s[i] = "N/A";
        string text = s[0] + ": \"" + s[1] + "\" (" + s[2] + ")";
        if (maxCharacters <= 0 || text.Length <= maxCharacters)
            return text;
        return text.Substring(0, maxCharacters + 1) + "...";
    }
    
}
}
