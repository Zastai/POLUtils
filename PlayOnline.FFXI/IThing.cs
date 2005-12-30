// $Id$

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace PlayOnline.FFXI {

  public interface IThing {

    /// <summary>Gets a name for this type of IThing.</summary>
    string TypeName { get; }

    /// <summary>
    /// Returns the full list of fields that can be supported by this IThing, with the guarantee that
    /// no instance of this class will ever contain a field that is not in this list.
    /// </summary>
    /// <returns>A list of field names.</returns>
    List<string> GetAllFields();

    /// <summary>
    /// Returns the list of fields supported by this IThing instance.
    /// </summary>
    /// <returns>A list of field names.</returns>
    List<string> GetFields();

    /// <summary>
    /// Checks whether or not a field with the given name is available.
    /// </summary>
    /// <param name="Field">The name of the field to check.</param>
    /// <returns>true if the field is available, false otherwise.</returns>
    bool HasField(string Field);

    /// <summary>
    /// Returns the value of the given field in text form.
    /// </summary>
    /// <param name="Field">The name of the field.</param>
    /// <returns>The field's value in text form.</returns>
    string GetFieldText(string Field);

    /// <summary>
    /// Returns the value of the given field.
    /// </summary>
    /// <param name="Field">The name of the field.</param>
    /// <returns>The field's value.</returns>
    object GetFieldValue(string Field);

    /// <summary>
    /// Clears all fields of this IThing.
    /// </summary>
    void Clear();

    /// <summary>
    /// Returns an image that can serve as an icon for this IThing.
    /// </summary>
    /// <returns>The icon image, or null if no such image is available.</returns>
    Image GetIcon();

    /// <summary>
    /// Returns a list of "property pages" for this IThing; can go from generic to very specialized.
    /// </summary>
    /// <returns>A list of tab pages that can be seen as "property pages" for this IThing.</returns>
    List<TabPage> GetPropertyPages();

    /// <summary>
    /// Fills this IThing based on the given XML representation.
    /// </summary>
    /// <param name="Node">The XML representation to load.</param>
    void Load(XmlElement Node);

    /// <summary>
    /// Saves this IThing in XML form (suitable for later Load()).
    /// </summary>
    /// <param name="Document">The XML document to use as context for the created XML representation.</param>
    /// <returns>The created XML representation of this IThing.</returns>
    XmlElement Save(XmlDocument Document);

  }

}
