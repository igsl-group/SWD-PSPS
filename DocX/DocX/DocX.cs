﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace Novacode
{
    /// <summary>
    /// Represents a document.
    /// </summary>
    public class DocX : Container, IDisposable
    {
        #region Namespaces

        static internal XNamespace w = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";
        static internal XNamespace rel = "http://schemas.openxmlformats.org/package/2006/relationships";

        static internal XNamespace r = "http://schemas.openxmlformats.org/officeDocument/2006/relationships";
        static internal XNamespace m = "http://schemas.openxmlformats.org/officeDocument/2006/math";
        static internal XNamespace customPropertiesSchema = "http://schemas.openxmlformats.org/officeDocument/2006/custom-properties";
        static internal XNamespace customVTypesSchema = "http://schemas.openxmlformats.org/officeDocument/2006/docPropsVTypes";

        static internal XNamespace wp = "http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing";
        static internal XNamespace a = "http://schemas.openxmlformats.org/drawingml/2006/main";
        static internal XNamespace c = "http://schemas.openxmlformats.org/drawingml/2006/chart";

        static internal XNamespace v = "urn:schemas-microsoft-com:vml";

        internal static XNamespace n = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/numbering";

        #endregion Namespaces

        // Get the word\document.xml part
        internal PackagePart mainPart;

        // Get the word\settings.xml part
        internal PackagePart settingsPart;

        internal PackagePart endnotesPart;

        internal PackagePart footnotesPart;

        internal PackagePart stylesPart;

        internal PackagePart stylesWithEffectsPart;

        internal PackagePart numberingPart;

        internal PackagePart fontTablePart;

        private Headers headers;

        private Footers footers;

        internal DocX(DocX document, XElement xml)
            : base(document, xml)
        {
        }

        public float MarginTop
        {
            get
            {
                return getMarginAttribute(XName.Get("top", DocX.w.NamespaceName));
            }

            set
            {
                setMarginAttribute(XName.Get("top", DocX.w.NamespaceName), value);
            }
        }

        public float MarginBottom
        {
            get
            {
                return getMarginAttribute(XName.Get("bottom", DocX.w.NamespaceName));
            }

            set
            {
                setMarginAttribute(XName.Get("bottom", DocX.w.NamespaceName), value);
            }
        }

        public float MarginLeft
        {
            get
            {
                return getMarginAttribute(XName.Get("left", DocX.w.NamespaceName));
            }

            set
            {
                setMarginAttribute(XName.Get("left", DocX.w.NamespaceName), value);
            }
        }

        public float MarginRight
        {
            get
            {
                return getMarginAttribute(XName.Get("right", DocX.w.NamespaceName));
            }

            set
            {
                setMarginAttribute(XName.Get("right", DocX.w.NamespaceName), value);
            }
        }

        public float PageWidth
        {
            get
            {
                XElement body = mainDoc.Root.Element(XName.Get("body", DocX.w.NamespaceName));
                XElement sectPr = body.Element(XName.Get("sectPr", DocX.w.NamespaceName));
                if (sectPr != null)
                {
                    XElement pgSz = sectPr.Element(XName.Get("pgSz", DocX.w.NamespaceName));

                    if (pgSz != null)
                    {
                        XAttribute w = pgSz.Attribute(XName.Get("w", DocX.w.NamespaceName));
                        if (w != null)
                        {
                            float f;
                            if (float.TryParse(w.Value, out f))
                                return (int)(f / 15.0f);
                        }
                    }
                }

                return (int)(11906.0f / 15.0f);
            }

            set
            {
                XElement body = mainDoc.Root.Element(XName.Get("body", DocX.w.NamespaceName));

                if (body != null)
                {
                    XElement sectPr = body.Element(XName.Get("sectPr", DocX.w.NamespaceName));

                    if (sectPr != null)
                    {
                        XElement pgSz = sectPr.Element(XName.Get("pgSz", DocX.w.NamespaceName));

                        if (pgSz != null)
                        {
                            pgSz.SetAttributeValue(XName.Get("w", DocX.w.NamespaceName), value * 15);
                        }
                    }
                }
            }
        }

        public float PageHeight
        {
            get
            {
                XElement body = mainDoc.Root.Element(XName.Get("body", DocX.w.NamespaceName));
                XElement sectPr = body.Element(XName.Get("sectPr", DocX.w.NamespaceName));
                if (sectPr != null)
                {
                    XElement pgSz = sectPr.Element(XName.Get("pgSz", DocX.w.NamespaceName));

                    if (pgSz != null)
                    {
                        XAttribute w = pgSz.Attribute(XName.Get("h", DocX.w.NamespaceName));
                        if (w != null)
                        {
                            float f;
                            if (float.TryParse(w.Value, out f))
                                return (int)(f / 15.0f);
                        }
                    }
                }

                return (int)(16838.0f / 15.0f);
            }

            set
            {
                XElement body = mainDoc.Root.Element(XName.Get("body", DocX.w.NamespaceName));

                if (body != null)
                {
                    XElement sectPr = body.Element(XName.Get("sectPr", DocX.w.NamespaceName));

                    if (sectPr != null)
                    {
                        XElement pgSz = sectPr.Element(XName.Get("pgSz", DocX.w.NamespaceName));

                        if (pgSz != null)
                        {
                            pgSz.SetAttributeValue(XName.Get("h", DocX.w.NamespaceName), value * 15);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns true if any editing restrictions are imposed on this document.
        /// </summary>
        /// <example>
        /// <code>
        /// // Create a new document.
        /// using (DocX document = DocX.Create(@"Test.docx"))
        /// {
        ///     if(document.isProtected)
        ///         Console.WriteLine("Protected");
        ///     else
        ///         Console.WriteLine("Not protected");
        ///
        ///     // Save the document.
        ///     document.Save();
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="AddProtection"/>
        /// <seealso cref="RemoveProtection"/>
        /// <seealso cref="GetProtectionType"/>
        public bool isProtected
        {
            get
            {
                return settings.Descendants(XName.Get("documentProtection", DocX.w.NamespaceName)).Count() > 0;
            }
        }

        public PageLayout PageLayout
        {
            get
            {
                XElement sectPr = Xml.Element(XName.Get("sectPr", DocX.w.NamespaceName));
                if (sectPr == null)
                {
                    Xml.SetElementValue(XName.Get("sectPr", DocX.w.NamespaceName), string.Empty);
                    sectPr = Xml.Element(XName.Get("sectPr", DocX.w.NamespaceName));
                }

                return new PageLayout(this, sectPr);
            }
        }

        /// <summary>
        /// Returns a collection of Headers in this Document.
        /// A document typically contains three Headers.
        /// A default one (odd), one for the first page and one for even pages.
        /// </summary>
        /// <example>
        /// <code>
        /// // Create a document.
        /// using (DocX document = DocX.Create(@"Test.docx"))
        /// {
        ///    // Add header support to this document.
        ///    document.AddHeaders();
        ///
        ///    // Get a collection of all headers in this document.
        ///    Headers headers = document.Headers;
        ///
        ///    // The header used for the first page of this document.
        ///    Header first = headers.first;
        ///
        ///    // The header used for odd pages of this document.
        ///    Header odd = headers.odd;
        ///
        ///    // The header used for even pages of this document.
        ///    Header even = headers.even;
        /// }
        /// </code>
        /// </example>
        public Headers Headers
        {
            get
            {
                return headers;
            }
        }

        /// <summary>
        /// Returns a collection of Footers in this Document.
        /// A document typically contains three Footers.
        /// A default one (odd), one for the first page and one for even pages.
        /// </summary>
        /// <example>
        /// <code>
        /// // Create a document.
        /// using (DocX document = DocX.Create(@"Test.docx"))
        /// {
        ///    // Add footer support to this document.
        ///    document.AddFooters();
        ///
        ///    // Get a collection of all footers in this document.
        ///    Footers footers = document.Footers;
        ///
        ///    // The footer used for the first page of this document.
        ///    Footer first = footers.first;
        ///
        ///    // The footer used for odd pages of this document.
        ///    Footer odd = footers.odd;
        ///
        ///    // The footer used for even pages of this document.
        ///    Footer even = footers.even;
        /// }
        /// </code>
        /// </example>
        public Footers Footers
        {
            get
            {
                return footers;
            }
        }

        /// <summary>
        /// Should the Document use different Headers and Footers for odd and even pages?
        /// </summary>
        /// // Create a document.
        /// using (DocX document = DocX.Create(@"Test.docx"))
        /// {
        ///     // Add header support to this document.
        ///     document.AddHeaders();
        ///
        ///     // Get a collection of all headers in this document.
        ///     Headers headers = document.Headers;
        ///
        ///     // The header used for odd pages of this document.
        ///     Header odd = headers.odd;
        ///
        ///     // The header used for even pages of this document.
        ///     Header even = headers.even;
        ///
        ///     // Force the document to use a different header for odd and even pages.
        ///     document.DifferentOddAndEvenPages = true;
        ///
        ///     // Content can be added to the Headers in the same manor that it would be added to the main document.
        ///     Paragraph p1 = odd.InsertParagraph();
        ///     p1.Append("This is the odd pages header.");
        ///
        ///     Paragraph p2 = even.InsertParagraph();
        ///     p2.Append("This is the even pages header.");
        ///
        ///     // Save all changes to this document.
        ///     document.Save();
        /// }// Release this document from memory.
        /// </example>
        public bool DifferentOddAndEvenPages
        {
            get
            {
                XDocument settings;
                using (TextReader tr = new StreamReader(settingsPart.GetStream()))
                    settings = XDocument.Load(tr);

                XElement evenAndOddHeaders = settings.Root.Element(w + "evenAndOddHeaders");

                return evenAndOddHeaders != null;
            }

            set
            {
                XDocument settings;
                using (TextReader tr = new StreamReader(settingsPart.GetStream()))
                    settings = XDocument.Load(tr);

                XElement evenAndOddHeaders = settings.Root.Element(w + "evenAndOddHeaders");
                if (evenAndOddHeaders == null)
                {
                    if (value)
                        settings.Root.AddFirst(new XElement(w + "evenAndOddHeaders"));
                }
                else
                {
                    if (!value)
                        evenAndOddHeaders.Remove();
                }

                using (TextWriter tw = new StreamWriter(settingsPart.GetStream()))
                    settings.Save(tw);
            }
        }

        /// <summary>
        /// Should the Document use an independent Header and Footer for the first page?
        /// </summary>
        /// <example>
        /// // Create a document.
        /// using (DocX document = DocX.Create(@"Test.docx"))
        /// {
        ///     // Add header support to this document.
        ///     document.AddHeaders();
        ///
        ///     // The header used for the first page of this document.
        ///     Header first = document.Headers.first;
        ///
        ///     // Force the document to use a different header for first page.
        ///     document.DifferentFirstPage = true;
        ///
        ///     // Content can be added to the Headers in the same manor that it would be added to the main document.
        ///     Paragraph p = first.InsertParagraph();
        ///     p.Append("This is the first pages header.");
        ///
        ///     // Save all changes to this document.
        ///     document.Save();
        /// }// Release this document from memory.
        /// </example>
        public bool DifferentFirstPage
        {
            get
            {
                XElement body = mainDoc.Root.Element(w + "body");
                XElement sectPr = body.Element(w + "sectPr");

                if (sectPr != null)
                {
                    XElement titlePg = sectPr.Element(w + "titlePg");
                    if (titlePg != null)
                        return true;
                }

                return false;
            }

            set
            {
                XElement body = mainDoc.Root.Element(w + "body");
                XElement sectPr = null;
                XElement titlePg = null;

                if (sectPr == null)
                    body.Add(new XElement(w + "sectPr", string.Empty));

                sectPr = body.Element(w + "sectPr");

                titlePg = sectPr.Element(w + "titlePg");
                if (titlePg == null)
                {
                    if (value)
                        sectPr.Add(new XElement(w + "titlePg", string.Empty));
                }
                else
                {
                    if (!value)
                        titlePg.Remove();
                }
            }
        }

        /// <summary>
        /// Returns a list of Images in this document.
        /// </summary>
        /// <example>
        /// Get the unique Id of every Image in this document.
        /// <code>
        /// // Load a document.
        /// DocX document = DocX.Load(@"C:\Example\Test.docx");
        ///
        /// // Loop through each Image in this document.
        /// foreach (Novacode.Image i in document.Images)
        /// {
        ///     // Get the unique Id which identifies this Image.
        ///     string uniqueId = i.Id;
        /// }
        ///
        /// </code>
        /// </example>
        /// <seealso cref="AddImage(string)"/>
        /// <seealso cref="AddImage(Stream)"/>
        /// <seealso cref="Paragraph.Pictures"/>
        /// <seealso cref="Paragraph.InsertPicture"/>
        public List<Image> Images
        {
            get
            {
                PackageRelationshipCollection imageRelationships = mainPart.GetRelationshipsByType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/image");
                if (imageRelationships.Count() > 0)
                {
                    return
                    (
                        from i in imageRelationships
                        select new Image(this, i)
                    ).ToList();
                }

                return new List<Image>();
            }
        }

        /// <summary>
        /// Returns a list of custom properties in this document.
        /// </summary>
        /// <example>
        /// Method 1: Get the name, type and value of each CustomProperty in this document.
        /// <code>
        /// // Load Example.docx
        /// DocX document = DocX.Load(@"C:\Example\Test.docx");
        ///
        /// /*
        ///  * No two custom properties can have the same name,
        ///  * so a Dictionary is the perfect data structure to store them in.
        ///  * Each custom property can be accessed using its name.
        ///  */
        /// foreach (string name in document.CustomProperties.Keys)
        /// {
        ///     // Grab a custom property using its name.
        ///     CustomProperty cp = document.CustomProperties[name];
        ///
        ///     // Write this custom properties details to Console.
        ///     Console.WriteLine(string.Format("Name: '{0}', Value: {1}", cp.Name, cp.Value));
        /// }
        ///
        /// Console.WriteLine("Press any key...");
        ///
        /// // Wait for the user to press a key before closing the Console.
        /// Console.ReadKey();
        /// </code>
        /// </example>
        /// <example>
        /// Method 2: Get the name, type and value of each CustomProperty in this document.
        /// <code>
        /// // Load Example.docx
        /// DocX document = DocX.Load(@"C:\Example\Test.docx");
        ///
        /// /*
        ///  * No two custom properties can have the same name,
        ///  * so a Dictionary is the perfect data structure to store them in.
        ///  * The values of this Dictionary are CustomProperties.
        ///  */
        /// foreach (CustomProperty cp in document.CustomProperties.Values)
        /// {
        ///     // Write this custom properties details to Console.
        ///     Console.WriteLine(string.Format("Name: '{0}', Value: {1}", cp.Name, cp.Value));
        /// }
        ///
        /// Console.WriteLine("Press any key...");
        ///
        /// // Wait for the user to press a key before closing the Console.
        /// Console.ReadKey();
        /// </code>
        /// </example>
        /// <seealso cref="AddCustomProperty"/>
        public Dictionary<string, CustomProperty> CustomProperties
        {
            get
            {
                if (package.PartExists(new Uri("/docProps/custom.xml", UriKind.Relative)))
                {
                    PackagePart docProps_custom = package.GetPart(new Uri("/docProps/custom.xml", UriKind.Relative));
                    XDocument customPropDoc;
                    using (TextReader tr = new StreamReader(docProps_custom.GetStream(FileMode.Open, FileAccess.Read)))
                        customPropDoc = XDocument.Load(tr, LoadOptions.PreserveWhitespace);

                    // Get all of the custom properties in this document
                    return
                    (
                        from p in customPropDoc.Descendants(XName.Get("property", customPropertiesSchema.NamespaceName))
                        let Name = p.Attribute(XName.Get("name")).Value
                        let Type = p.Descendants().Single().Name.LocalName
                        let Value = p.Descendants().Single().Value
                        select new CustomProperty(Name, Type, Value)
                    ).ToDictionary(p => p.Name, StringComparer.CurrentCultureIgnoreCase);
                }

                return new Dictionary<string, CustomProperty>();
            }
        }

        ///<summary>
        /// Returns the list of document core properties with corresponding values.
        ///</summary>
        public Dictionary<string, string> CoreProperties
        {
            get
            {
                if (package.PartExists(new Uri("/docProps/core.xml", UriKind.Relative)))
                {
                    PackagePart docProps_Core = package.GetPart(new Uri("/docProps/core.xml", UriKind.Relative));
                    XDocument corePropDoc;
                    using (TextReader tr = new StreamReader(docProps_Core.GetStream(FileMode.Open, FileAccess.Read)))
                        corePropDoc = XDocument.Load(tr, LoadOptions.PreserveWhitespace);

                    // Get all of the core properties in this document
                    return (from docProperty in corePropDoc.Root.Elements()
                            select
                              new KeyValuePair<string, string>(
                              string.Format(
                                "{0}:{1}",
                                corePropDoc.Root.GetPrefixOfNamespace(docProperty.Name.Namespace),
                                docProperty.Name.LocalName),
                              docProperty.Value)).ToDictionary(p => p.Key, v => v.Value);
                }

                return new Dictionary<string, string>();
            }
        }

        /// <summary>
        /// Get the Text of this document.
        /// </summary>
        /// <example>
        /// Write to Console the Text from this document.
        /// <code>
        /// // Load a document
        /// DocX document = DocX.Load(@"C:\Example\Test.docx");
        ///
        /// // Get the text of this document.
        /// string text = document.Text;
        ///
        /// // Write the text of this document to Console.
        /// Console.Write(text);
        ///
        /// // Wait for the user to press a key before closing the console window.
        /// Console.ReadKey();
        /// </code>
        /// </example>
        public string Text
        {
            get
            {
                return HelperFunctions.GetText(Xml);
            }
        }

        /// <summary>
        /// Get the text of each footnote from this document
        /// </summary>
        public IEnumerable<string> FootnotesText
        {
            get
            {
                foreach (XElement footnote in footnotes.Root.Elements(w + "footnote"))
                {
                    yield return HelperFunctions.GetText(footnote);
                }
            }
        }

        /// <summary>
        /// Get the text of each endnote from this document
        /// </summary>
        public IEnumerable<string> EndnotesText
        {
            get
            {
                foreach (XElement endnote in endnotes.Root.Elements(w + "endnote"))
                {
                    yield return HelperFunctions.GetText(endnote);
                }
            }
        }

        public override ReadOnlyCollection<Paragraph> Paragraphs
        {
            get
            {
                ReadOnlyCollection<Paragraph> l = base.Paragraphs;
                foreach (var paragraph in l)
                {
                    paragraph.PackagePart = mainPart;
                }
                return l;
            }
        }

        public override List<List> Lists
        {
            get
            {
                List<List> l = base.Lists;
                l.ForEach(x => x.Items.ForEach(i => i.PackagePart = mainPart));
                return l;
            }
        }

        public override List<Table> Tables
        {
            get
            {
                List<Table> l = base.Tables;
                l.ForEach(x => x.mainPart = mainPart);
                return l;
            }
        }

        /// <summary>
        /// Creates a document using a Stream.
        /// </summary>
        /// <param name="stream">The Stream to create the document from.</param>
        /// <returns>Returns a DocX object which represents the document.</returns>
        /// <example>
        /// Creating a document from a FileStream.
        /// <code>
        /// // Use a FileStream fs to create a new document.
        /// using(FileStream fs = new FileStream(@"C:\Example\Test.docx", FileMode.Create))
        /// {
        ///     // Load the document using fs
        ///     using (DocX document = DocX.Create(fs))
        ///     {
        ///         // Do something with the document here.
        ///
        ///         // Save all changes made to this document.
        ///         document.Save();
        ///     }// Release this document from memory.
        /// }
        /// </code>
        /// </example>
        /// <example>
        /// Creating a document in a SharePoint site.
        /// <code>
        /// using(SPSite mySite = new SPSite("http://server/sites/site"))
        /// {
        ///     // Open a connection to the SharePoint site
        ///     using(SPWeb myWeb = mySite.OpenWeb())
        ///     {
        ///         // Create a MemoryStream ms.
        ///         using (MemoryStream ms = new MemoryStream())
        ///         {
        ///             // Create a document using ms.
        ///             using (DocX document = DocX.Create(ms))
        ///             {
        ///                 // Do something with the document here.
        ///
        ///                 // Save all changes made to this document.
        ///                 document.Save();
        ///             }// Release this document from memory
        ///
        ///             // Add the document to the SharePoint site
        ///             web.Files.Add("filename", ms.ToArray(), true);
        ///         }
        ///     }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="DocX.Create(string)"/>
        /// <seealso cref="DocX.Load(System.IO.Stream)"/>
        /// <seealso cref="DocX.Load(string)"/>
        /// <seealso cref="DocX.Save()"/>
        public static DocX Create(Stream stream, DocumentTypes documentType = DocumentTypes.Document)
        {
            // Store this document in memory
            MemoryStream ms = new MemoryStream();

            // Create the docx package
            Package package = Package.Open(ms, FileMode.Create, FileAccess.ReadWrite);

            PostCreation(package, documentType);
            DocX document = DocX.Load(ms);
            document.stream = stream;
            return document;
        }

        /// <summary>
        /// Creates a document using a fully qualified or relative filename.
        /// </summary>
        /// <param name="filename">The fully qualified or relative filename.</param>
        /// <returns>Returns a DocX object which represents the document.</returns>
        /// <example>
        /// <code>
        /// // Create a document using a relative filename.
        /// using (DocX document = DocX.Create(@"..\Test.docx"))
        /// {
        ///     // Do something with the document here.
        ///
        ///     // Save all changes made to this document.
        ///     document.Save();
        /// }// Release this document from memory
        /// </code>
        /// <code>
        /// // Create a document using a relative filename.
        /// using (DocX document = DocX.Create(@"..\Test.docx"))
        /// {
        ///     // Do something with the document here.
        ///
        ///     // Save all changes made to this document.
        ///     document.Save();
        /// }// Release this document from memory
        /// </code>
        /// <seealso cref="DocX.Create(System.IO.Stream)"/>
        /// <seealso cref="DocX.Load(System.IO.Stream)"/>
        /// <seealso cref="DocX.Load(string)"/>
        /// <seealso cref="DocX.Save()"/>
        /// </example>
        public static DocX Create(string filename, DocumentTypes documentType = DocumentTypes.Document)
        {
            // Store this document in memory
            MemoryStream ms = new MemoryStream();

            // Create the docx package
            //WordprocessingDocument wdDoc = WordprocessingDocument.Create(ms, DocumentFormat.OpenXml.WordprocessingDocumentType.Document);
            Package package = Package.Open(ms, FileMode.Create, FileAccess.ReadWrite);

            PostCreation(package, documentType);
            DocX document = DocX.Load(ms);
            document.filename = filename;
            return document;
        }

        /// <summary>
        /// Loads a document into a DocX object using a Stream.
        /// </summary>
        /// <param name="stream">The Stream to load the document from.</param>
        /// <returns>
        /// Returns a DocX object which represents the document.
        /// </returns>
        /// <example>
        /// Loading a document from a FileStream.
        /// <code>
        /// // Open a FileStream fs to a document.
        /// using (FileStream fs = new FileStream(@"C:\Example\Test.docx", FileMode.Open))
        /// {
        ///     // Load the document using fs.
        ///     using (DocX document = DocX.Load(fs))
        ///     {
        ///         // Do something with the document here.
        ///
        ///         // Save all changes made to the document.
        ///         document.Save();
        ///     }// Release this document from memory.
        /// }
        /// </code>
        /// </example>
        /// <example>
        /// Loading a document from a SharePoint site.
        /// <code>
        /// // Get the SharePoint site that you want to access.
        /// using (SPSite mySite = new SPSite("http://server/sites/site"))
        /// {
        ///     // Open a connection to the SharePoint site
        ///     using (SPWeb myWeb = mySite.OpenWeb())
        ///     {
        ///         // Grab a document stored on this site.
        ///         SPFile file = web.GetFile("Source_Folder_Name/Source_File");
        ///
        ///         // DocX.Load requires a Stream, so open a Stream to this document.
        ///         Stream str = new MemoryStream(file.OpenBinary());
        ///
        ///         // Load the file using the Stream str.
        ///         using (DocX document = DocX.Load(str))
        ///         {
        ///             // Do something with the document here.
        ///
        ///             // Save all changes made to the document.
        ///             document.Save();
        ///         }// Release this document from memory.
        ///     }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="DocX.Load(string)"/>
        /// <seealso cref="DocX.Create(System.IO.Stream)"/>
        /// <seealso cref="DocX.Create(string)"/>
        /// <seealso cref="DocX.Save()"/>
        public static DocX Load(Stream stream)
        {
            MemoryStream ms = new MemoryStream();

            stream.Position = 0;
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, (int)stream.Length);
            ms.Write(data, 0, (int)stream.Length);

            // Open the docx package
            Package package = Package.Open(ms, FileMode.Open, FileAccess.ReadWrite);

            DocX document = PostLoad(ref package);
            document.package = package;
            document.memoryStream = ms;
            document.stream = stream;
            return document;
        }

        /// <summary>
        /// Loads a document into a DocX object using a fully qualified or relative filename.
        /// </summary>
        /// <param name="filename">The fully qualified or relative filename.</param>
        /// <returns>
        /// Returns a DocX object which represents the document.
        /// </returns>
        /// <example>
        /// <code>
        /// // Load a document using its fully qualified filename
        /// using (DocX document = DocX.Load(@"C:\Example\Test.docx"))
        /// {
        ///     // Do something with the document here
        ///
        ///     // Save all changes made to document.
        ///     document.Save();
        /// }// Release this document from memory.
        /// </code>
        /// <code>
        /// // Load a document using its relative filename.
        /// using(DocX document = DocX.Load(@"..\..\Test.docx"))
        /// {
        ///     // Do something with the document here.
        ///
        ///     // Save all changes made to document.
        ///     document.Save();
        /// }// Release this document from memory.
        /// </code>
        /// <seealso cref="DocX.Load(System.IO.Stream)"/>
        /// <seealso cref="DocX.Create(System.IO.Stream)"/>
        /// <seealso cref="DocX.Create(string)"/>
        /// <seealso cref="DocX.Save()"/>
        /// </example>
        public static DocX Load(string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException(string.Format("File could not be found {0}", filename));

            MemoryStream ms = new MemoryStream();

            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, (int)fs.Length);
                ms.Write(data, 0, (int)fs.Length);
            }

            // Open the docx package
            Package package = Package.Open(ms, FileMode.Open, FileAccess.ReadWrite);

            DocX document = PostLoad(ref package);
            document.package = package;
            document.filename = filename;
            document.memoryStream = ms;

            return document;
        }

        /// <summary>
        /// Returns the type of editing protection imposed on this document.
        /// </summary>
        /// <returns>The type of editing protection imposed on this document.</returns>
        /// <example>
        /// <code>
        /// Create a new document.
        /// using (DocX document = DocX.Create(@"Test.docx"))
        /// {
        ///     // Make sure the document is protected before checking the protection type.
        ///     if (document.isProtected)
        ///     {
        ///         EditRestrictions protection = document.GetProtectionType();
        ///         Console.WriteLine("Document is protected using " + protection.ToString());
        ///     }
        ///
        ///     else
        ///         Console.WriteLine("Document is not protected.");
        ///
        ///     // Save the document.
        ///     document.Save();
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="AddProtection"/>
        /// <seealso cref="RemoveProtection"/>
        /// <seealso cref="isProtected"/>
        public EditRestrictions GetProtectionType()
        {
            if (isProtected)
            {
                XElement documentProtection = settings.Descendants(XName.Get("documentProtection", DocX.w.NamespaceName)).FirstOrDefault();
                string edit_type = documentProtection.Attribute(XName.Get("edit", DocX.w.NamespaceName)).Value;
                return (EditRestrictions)Enum.Parse(typeof(EditRestrictions), edit_type);
            }

            return EditRestrictions.none;
        }

        /// <summary>
        /// Add editing protection to this document.
        /// </summary>
        /// <param name="er">The type of protection to add to this document.</param>
        /// <example>
        /// <code>
        /// // Create a new document.
        /// using (DocX document = DocX.Create(@"Test.docx"))
        /// {
        ///     // Allow no editing, only the adding of comment.
        ///     document.AddProtection(EditRestrictions.comments);
        ///
        ///     // Save the document.
        ///     document.Save();
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="RemoveProtection"/>
        /// <seealso cref="GetProtectionType"/>
        /// <seealso cref="isProtected"/>
        public void AddProtection(EditRestrictions er)
        {
            // Call remove protection before adding a new protection element.
            RemoveProtection();

            if (er == EditRestrictions.none)
                return;

            XElement documentProtection = new XElement(XName.Get("documentProtection", DocX.w.NamespaceName));
            documentProtection.Add(new XAttribute(XName.Get("edit", DocX.w.NamespaceName), er.ToString()));
            documentProtection.Add(new XAttribute(XName.Get("enforcement", DocX.w.NamespaceName), "1"));

            settings.Root.AddFirst(documentProtection);
        }

        /// <summary>
        /// Remove editing protection from this document.
        /// </summary>
        /// <example>
        /// <code>
        /// // Create a new document.
        /// using (DocX document = DocX.Create(@"Test.docx"))
        /// {
        ///     // Remove any editing restrictions that are imposed on this document.
        ///     document.RemoveProtection();
        ///
        ///     // Save the document.
        ///     document.Save();
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="AddProtection"/>
        /// <seealso cref="GetProtectionType"/>
        /// <seealso cref="isProtected"/>
        public void RemoveProtection()
        {
            // Remove every node of type documentProtection.
            settings.Descendants(XName.Get("documentProtection", DocX.w.NamespaceName)).Remove();
        }

        public List<Section> GetSections()
        {
            var allParas = Paragraphs;

            var parasInASection = new List<Paragraph>();
            var sections = new List<Section>();

            foreach (var para in allParas)
            {
                var sectionInPara = para.Xml.Descendants().FirstOrDefault(s => s.Name.LocalName == "sectPr");

                if (sectionInPara == null)
                {
                    parasInASection.Add(para);
                }
                else
                {
                    parasInASection.Add(para);
                    var section = new Section(Document, sectionInPara) { SectionParagraphs = parasInASection };
                    sections.Add(section);
                    parasInASection = new List<Paragraph>();
                }
            }

            XElement body = mainDoc.Root.Element(XName.Get("body", DocX.w.NamespaceName));
            XElement baseSectionXml = body.Element(XName.Get("sectPr", DocX.w.NamespaceName));
            var baseSection = new Section(Document, baseSectionXml) { SectionParagraphs = parasInASection };
            sections.Add(baseSection);

            return sections;
        }

        /// <summary>
        /// Insert the contents of another document at the end of this document.
        /// </summary>
        /// <param name="remote_document">The document to insert at the end of this document.</param>
        /// <example>
        /// Create a new document and insert an old document into it.
        /// <code>
        /// // Create a new document.
        /// using (DocX newDocument = DocX.Create(@"NewDocument.docx"))
        /// {
        ///     // Load an old document.
        ///     using (DocX oldDocument = DocX.Load(@"OldDocument.docx"))
        ///     {
        ///         // Insert the old document into the new document.
        ///         newDocument.InsertDocument(oldDocument);
        ///
        ///         // Save the new document.
        ///         newDocument.Save();
        ///     }// Release the old document from memory.
        /// }// Release the new document from memory.
        /// </code>
        /// <remarks>
        /// If the document being inserted contains Images, CustomProperties and or custom styles, these will be correctly inserted into the new document. In the case of Images, new ID's are generated for the Images being inserted to avoid ID conflicts. CustomProperties with the same name will be ignored not replaced.
        /// </remarks>
        /// </example>
        public void InsertDocument(DocX remote_document)
        {
            XElement body = mainDoc.Root.Element(w + "body");
            XElement sectPr = body.Descendants(w + "sectPr").LastOrDefault();
            XElement para = body.Descendants(w + "p").LastOrDefault();

            if (sectPr != null && para != null)
            {
                XElement pPr = para.Descendants(w + "pPr").FirstOrDefault();

                if (pPr != null)
                {
                    pPr.Add(sectPr);
                    sectPr.Remove();
                }
                else {
                    //Add a dummy Pr in order to add the section inside it for section break!
                    pPr = new XElement(w + "pPr");
                    para.Add(pPr);
                    pPr.Add(sectPr);
                    sectPr.Remove();
                }
            }

            // We don't want to effect the origional XDocument, so create a new one from the old one.
            XDocument remote_mainDoc = new XDocument(remote_document.mainDoc);

            XDocument remote_footnotes = null;
            if (remote_document.footnotes != null)
                remote_footnotes = new XDocument(remote_document.footnotes);

            XDocument remote_endnotes = null;
            if (remote_document.endnotes != null)
                remote_endnotes = new XDocument(remote_document.endnotes);

            // Remove all header and footer references.
            //remote_mainDoc.Descendants(XName.Get("headerReference", DocX.w.NamespaceName)).Remove();
            remote_mainDoc.Descendants(XName.Get("footerReference", DocX.w.NamespaceName)).Remove();

            sectPr = remote_mainDoc.Descendants(XName.Get("sectPr", DocX.w.NamespaceName)).FirstOrDefault();
            if (sectPr != null)
            {
                sectPr.Descendants(XName.Get("type", DocX.w.NamespaceName)).Where(x => x.Attribute(XName.Get("val", DocX.w.NamespaceName)).Value == "continuous").Remove();
            }

            // Get the body of the remote document.
            XElement remote_body = remote_mainDoc.Root.Element(XName.Get("body", DocX.w.NamespaceName));

            // Every file that is missing from the local document will have to be copied, every file that already exists will have to be merged.
            PackagePartCollection ppc = remote_document.package.GetParts();

            List<String> ignoreContentTypes = new List<string>
            {
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml",
                //"application/vnd.openxmlformats-officedocument.wordprocessingml.header+xml",
                "application/vnd.openxmlformats-officedocument.wordprocessingml.footer+xml",
                "application/vnd.openxmlformats-package.core-properties+xml",
                "application/vnd.openxmlformats-officedocument.extended-properties+xml",
                "application/vnd.openxmlformats-package.relationships+xml",
            };

            List<String> imageContentTypes = new List<string>
            {
                "image/jpeg",
                "image/jpg",
                "image/png",
                "image/bmp",
                "image/gif",
                "image/tiff",
                "image/icon",
                "image/pcx",
                "image/emf",
                "image/wmf"
            };
            // Check if each PackagePart pp exists in this document.
            foreach (PackagePart remote_pp in ppc)
            {
                if (ignoreContentTypes.Contains(remote_pp.ContentType) || imageContentTypes.Contains(remote_pp.ContentType))
                    continue;

                // If this external PackagePart already exits then we must merge them.
                if (package.PartExists(remote_pp.Uri))
                {
                    PackagePart local_pp = package.GetPart(remote_pp.Uri);

                    switch (remote_pp.ContentType)
                    {
                        case "application/vnd.openxmlformats-officedocument.wordprocessingml.header+xml":
                            //header1.xml.rels
                            string filename = Path.GetFileName(remote_pp.Uri.ToString().Replace("/", "\\"));
                            List<PackagePart> relsParts = ppc.Where(x => x.ContentType == "application/vnd.openxmlformats-package.relationships+xml" && x.Uri.ToString().Contains(String.Format("{0}.rels", filename))).ToList();
                            string oldRelationshipId = remote_document.mainPart.GetRelationships().Where(x => x.TargetUri == remote_pp.Uri || x.TargetUri.ToString() == filename).Select(x => x.Id).FirstOrDefault();
                            string newRelationshipId = merge_headers(remote_pp, relsParts, remote_mainDoc, oldRelationshipId);

                            remote_body.Descendants(XName.Get("headerReference", DocX.w.NamespaceName)).Where(x => x.Attribute(XName.Get("id", DocX.r.NamespaceName)).Value == oldRelationshipId).ToList().ForEach(x => x.Attribute(XName.Get("id", DocX.r.NamespaceName)).Value = newRelationshipId);
                            break;

                        case "application/vnd.openxmlformats-officedocument.custom-properties+xml":
                            merge_customs(remote_pp, local_pp, remote_mainDoc);
                            break;

                        // Merge footnotes (and endnotes) before merging styles, then set the remote_footnotes to the just updated footnotes
                        case "application/vnd.openxmlformats-officedocument.wordprocessingml.footnotes+xml":
                            merge_footnotes(remote_pp, local_pp, remote_mainDoc, remote_document, remote_footnotes);
                            remote_footnotes = footnotes;
                            break;

                        case "application/vnd.openxmlformats-officedocument.wordprocessingml.endnotes+xml":
                            merge_endnotes(remote_pp, local_pp, remote_mainDoc, remote_document, remote_endnotes);
                            remote_endnotes = endnotes;
                            break;

                        case "application/vnd.openxmlformats-officedocument.wordprocessingml.styles+xml":
                            merge_styles(remote_pp, local_pp, remote_mainDoc, remote_document, remote_footnotes, remote_endnotes);
                            break;

                        // Merge styles after merging the footnotes, so the changes will be applied to the correct document/footnotes
                        case "application/vnd.ms-word.stylesWithEffects+xml":
                            merge_styles(remote_pp, local_pp, remote_mainDoc, remote_document, remote_footnotes, remote_endnotes);
                            break;

                        case "application/vnd.openxmlformats-officedocument.wordprocessingml.fontTable+xml":
                            merge_fonts(remote_pp, local_pp, remote_mainDoc, remote_document);
                            break;

                        case "application/vnd.openxmlformats-officedocument.wordprocessingml.numbering+xml":
                            merge_numbering(remote_pp, local_pp, remote_mainDoc, remote_document);
                            break;

                        default:
                            break;
                    }
                }

                // If this external PackagePart does not exits in the internal document then we can simply copy it.
                else
                {
                    var packagePart = clonePackagePart(remote_pp);
                    switch (remote_pp.ContentType)
                    {
                        case "application/vnd.openxmlformats-officedocument.wordprocessingml.endnotes+xml":
                            endnotesPart = packagePart;
                            endnotes = remote_endnotes;
                            break;

                        case "application/vnd.openxmlformats-officedocument.wordprocessingml.footnotes+xml":
                            footnotesPart = packagePart;
                            footnotes = remote_footnotes;
                            break;

                        case "application/vnd.openxmlformats-officedocument.custom-properties+xml":
                            break;

                        case "application/vnd.openxmlformats-officedocument.wordprocessingml.styles+xml":
                            stylesPart = packagePart;
                            using (TextReader tr = new StreamReader(stylesPart.GetStream()))
                                styles = XDocument.Load(tr);
                            break;

                        case "application/vnd.ms-word.stylesWithEffects+xml":
                            stylesWithEffectsPart = packagePart;
                            using (TextReader tr = new StreamReader(stylesWithEffectsPart.GetStream()))
                                stylesWithEffects = XDocument.Load(tr);
                            break;

                        case "application/vnd.openxmlformats-officedocument.wordprocessingml.fontTable+xml":
                            fontTablePart = packagePart;
                            using (TextReader tr = new StreamReader(fontTablePart.GetStream()))
                                fontTable = XDocument.Load(tr);
                            break;

                        case "application/vnd.openxmlformats-officedocument.wordprocessingml.numbering+xml":
                            numberingPart = packagePart;
                            using (TextReader tr = new StreamReader(numberingPart.GetStream()))
                                numbering = XDocument.Load(tr);
                            break;
                    }

                    clonePackageRelationship(remote_document, remote_pp, remote_mainDoc);
                }
            }

            foreach (var hyperlink_rel in remote_document.mainPart.GetRelationshipsByType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/hyperlink"))
            {
                var old_rel_Id = hyperlink_rel.Id;
                var new_rel_Id = mainPart.CreateRelationship(hyperlink_rel.TargetUri, hyperlink_rel.TargetMode, hyperlink_rel.RelationshipType).Id;
                var hyperlink_refs = remote_mainDoc.Descendants(XName.Get("hyperlink", DocX.w.NamespaceName));
                foreach (var hyperlink_ref in hyperlink_refs)
                {
                    XAttribute a0 = hyperlink_ref.Attribute(XName.Get("id", DocX.r.NamespaceName));
                    if (a0 != null && a0.Value == old_rel_Id)
                    {
                        a0.SetValue(new_rel_Id);
                    }
                }
            }

            ////ole object links
            foreach (var oleObject_rel in remote_document.mainPart.GetRelationshipsByType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/oleObject"))
            {
                var old_rel_Id = oleObject_rel.Id;
                var new_rel_Id = mainPart.CreateRelationship(oleObject_rel.TargetUri, oleObject_rel.TargetMode, oleObject_rel.RelationshipType).Id;
                var oleObject_refs = remote_mainDoc.Descendants(XName.Get("OLEObject", "urn:schemas-microsoft-com:office:office"));
                foreach (var oleObject_ref in oleObject_refs)
                {
                    XAttribute a0 = oleObject_ref.Attribute(XName.Get("id", DocX.r.NamespaceName));
                    if (a0 != null && a0.Value == old_rel_Id)
                    {
                        a0.SetValue(new_rel_Id);
                    }
                }
            }

            foreach (PackagePart remote_pp in ppc)
            {
                if (imageContentTypes.Contains(remote_pp.ContentType))
                {
                    merge_images(remote_pp, remote_document, remote_mainDoc, remote_pp.ContentType);
                }
            }

            int id = 0;
            var local_docPrs = mainDoc.Root.Descendants(XName.Get("docPr", DocX.wp.NamespaceName));
            foreach (var local_docPr in local_docPrs)
            {
                XAttribute a_id = local_docPr.Attribute(XName.Get("id"));
                int a_id_value;
                if (a_id != null && int.TryParse(a_id.Value, out a_id_value))
                    if (a_id_value > id)
                        id = a_id_value;
            }
            id++;

            // docPr must be sequential
            var docPrs = remote_body.Descendants(XName.Get("docPr", DocX.wp.NamespaceName));
            foreach (var docPr in docPrs)
            {
                docPr.SetAttributeValue(XName.Get("id"), id);
                id++;
            }

            foreach (var header in Headers.headers)
            {
                var docHeaderPrs = header.Xml.Descendants(XName.Get("docPr", DocX.wp.NamespaceName));
                foreach (var docPr in docHeaderPrs)
                {
                    docPr.SetAttributeValue(XName.Get("id"), id);
                    id++;
                }
            }

            //Compare the headerreference in remote_body with last section with headerreference
            sectPr = mainDoc.Root.Element(XName.Get("body", DocX.w.NamespaceName)).Descendants(XName.Get("sectPr", DocX.w.NamespaceName)).LastOrDefault(x => x.Descendants(XName.Get("headerReference", DocX.w.NamespaceName)).Any());
            if (sectPr != null)
            {
                bool remove = true;

                XElement remote_SectPr = remote_mainDoc.Descendants(XName.Get("sectPr", DocX.w.NamespaceName)).FirstOrDefault();
                if (remote_SectPr != null)
                {
                    foreach (XElement remote_headerReference in remote_SectPr.Descendants(XName.Get("headerReference", DocX.w.NamespaceName)))
                    {
                        XElement local_headerReference = sectPr.Descendants(XName.Get("headerReference", DocX.w.NamespaceName)).FirstOrDefault(x => x.Attribute(XName.Get("type", DocX.w.NamespaceName)).Value == remote_headerReference.Attribute(XName.Get("type", DocX.w.NamespaceName)).Value);
                        XElement a = sectPr.Descendants(XName.Get("headerReference", DocX.w.NamespaceName)).FirstOrDefault(x => x.Attribute(XName.Get("type", DocX.w.NamespaceName)).Value == "testing");
                        if (local_headerReference == null || local_headerReference.Attribute(XName.Get("id", DocX.r.NamespaceName)).Value != remote_headerReference.Attribute(XName.Get("id", DocX.r.NamespaceName)).Value)
                        {
                            remove = false;
                            break;
                        }
                    }

                    //headers are identical to last section, re-use the last section one.
                    if (remove)
                        remote_body.Descendants(XName.Get("headerReference", DocX.w.NamespaceName)).Remove();
                }
            }

            // Add the remote documents contents to this document.
            XElement local_body = mainDoc.Root.Element(XName.Get("body", DocX.w.NamespaceName));
            local_body.Add(remote_body.Elements());

            // Copy any missing root attributes to the local document.
            foreach (XAttribute a in remote_mainDoc.Root.Attributes())
            {
                if (mainDoc.Root.Attribute(a.Name) == null)
                {
                    mainDoc.Root.SetAttributeValue(a.Name, a.Value);
                }
            }
        }

        /// <summary>
        /// Insert a new Table at the end of this document.
        /// </summary>
        /// <param name="columnCount">The number of columns to create.</param>
        /// <param name="rowCount">The number of rows to create.</param>
        /// <returns>A new Table.</returns>
        /// <example>
        /// Insert a new Table with 2 columns and 3 rows, at the end of a document.
        /// <code>
        /// // Create a document.
        /// using (DocX document = DocX.Create(@"C:\Example\Test.docx"))
        /// {
        ///     // Create a new Table with 2 columns and 3 rows.
        ///     Table newTable = document.InsertTable(2, 3);
        ///
        ///     // Set the design of this Table.
        ///     newTable.Design = TableDesign.LightShadingAccent2;
        ///
        ///     // Set the column names.
        ///     newTable.Rows[0].Cells[0].Paragraph.InsertText("Ice Cream", false);
        ///     newTable.Rows[0].Cells[1].Paragraph.InsertText("Price", false);
        ///
        ///     // Fill row 1
        ///     newTable.Rows[1].Cells[0].Paragraph.InsertText("Chocolate", false);
        ///     newTable.Rows[1].Cells[1].Paragraph.InsertText("€3:50", false);
        ///
        ///     // Fill row 2
        ///     newTable.Rows[2].Cells[0].Paragraph.InsertText("Vanilla", false);
        ///     newTable.Rows[2].Cells[1].Paragraph.InsertText("€3:00", false);
        ///
        ///     // Save all changes made to document b.
        ///     document.Save();
        /// }// Release this document from memory.
        /// </code>
        /// </example>
        public new Table InsertTable(int rowCount, int columnCount)
        {
            if (rowCount < 1 || columnCount < 1)
                throw new ArgumentOutOfRangeException("Row and Column count must be greater than zero.");

            Table t = base.InsertTable(rowCount, columnCount);
            t.mainPart = mainPart;
            return t;
        }

        public Table AddTable(int rowCount, int columnCount)
        {
            if (rowCount < 1 || columnCount < 1)
                throw new ArgumentOutOfRangeException("Row and Column count must be greater than zero.");

            Table t = new Table(this, HelperFunctions.CreateTable(rowCount, columnCount));
            t.mainPart = mainPart;
            return t;
        }

        /// <summary>
        /// Create a new list with a list item.
        /// </summary>
        /// <param name="listText">The text of the first element in the created list.</param>
        /// <param name="level">The indentation level of the element in the list.</param>
        /// <param name="listType">The type of list to be created: Bulleted or Numbered.</param>
        /// <param name="startNumber">The number start number for the list. </param>
        /// <param name="trackChanges">Enable change tracking</param>
        /// <param name="continueNumbering">Set to true if you want to continue numbering from the previous numbered list</param>
        /// <returns>
        /// The created List. Call AddListItem(...) to add more elements to the list.
        /// Write the list to the Document with InsertList(...) once the list has all the desired
        /// elements, otherwise the list will not be included in the working Document.
        /// </returns>
        public List AddList(string listText = null, int level = 0, ListItemType listType = ListItemType.Numbered, int? startNumber = null, bool trackChanges = false, bool continueNumbering = false)
        {
            return AddListItem(new List(this, null), listText, level, listType, startNumber, trackChanges, continueNumbering);
        }

        /// <summary>
        /// Add a list item to an already existing list.
        /// </summary>
        /// <param name="list">The list to add the new list item to.</param>
        /// <param name="listText">The run text that should be in the new list item.</param>
        /// <param name="level">The indentation level of the new list element.</param>
        /// <param name="startNumber">The number start number for the list. </param>
        /// <param name="trackChanges">Enable change tracking</param>
        /// <param name="listType">Numbered or Bulleted list type. </param>
        /// /// <param name="continueNumbering">Set to true if you want to continue numbering from the previous numbered list</param>
        /// <returns>
        /// The created List. Call AddListItem(...) to add more elements to the list.
        /// Write the list to the Document with InsertList(...) once the list has all the desired
        /// elements, otherwise the list will not be included in the working Document.
        /// </returns>
        public List AddListItem(List list, string listText, int level = 0, ListItemType listType = ListItemType.Numbered, int? startNumber = null, bool trackChanges = false, bool continueNumbering = false)
        {
            if (startNumber.HasValue && continueNumbering) throw new InvalidOperationException("Cannot specify a start number and at the same time continue numbering from another list");
            var listToReturn = HelperFunctions.CreateItemInList(list, listText, level, listType, startNumber, trackChanges, continueNumbering);
            var lastItem = listToReturn.Items.LastOrDefault();
            if (lastItem != null)
            {
                lastItem.PackagePart = mainPart;
            }
            return listToReturn;
        }

        /// <summary>
        /// Insert list into the document.
        /// </summary>
        /// <param name="list">The list to insert into the document.</param>
        /// <returns>The list that was inserted into the document.</returns>
        public new List InsertList(List list)
        {
            base.InsertList(list);
            return list;
        }

        public new List InsertList(List list, System.Drawing.FontFamily fontFamily, double fontSize)
        {
            base.InsertList(list, fontFamily, fontSize);
            return list;
        }

        public new List InsertList(List list, double fontSize)
        {
            base.InsertList(list, fontSize);
            return list;
        }

        /// <summary>
        /// Insert a list at an index location in the document.
        /// </summary>
        /// <param name="index">Index in document to insert the list.</param>
        /// <param name="list">The list that was inserted into the document.</param>
        /// <returns></returns>
        public new List InsertList(int index, List list)
        {
            base.InsertList(index, list);
            return list;
        }

        /// <summary>
        /// Insert a Table into this document. The Table's source can be a completely different document.
        /// </summary>
        /// <param name="t">The Table to insert.</param>
        /// <param name="index">The index to insert this Table at.</param>
        /// <returns>The Table now associated with this document.</returns>
        /// <example>
        /// Extract a Table from document a and insert it into document b, at index 10.
        /// <code>
        /// // Place holder for a Table.
        /// Table t;
        ///
        /// // Load document a.
        /// using (DocX documentA = DocX.Load(@"C:\Example\a.docx"))
        /// {
        ///     // Get the first Table from this document.
        ///     t = documentA.Tables[0];
        /// }
        ///
        /// // Load document b.
        /// using (DocX documentB = DocX.Load(@"C:\Example\b.docx"))
        /// {
        ///     /*
        ///      * Insert the Table that was extracted from document a, into document b.
        ///      * This creates a new Table that is now associated with document b.
        ///      */
        ///     Table newTable = documentB.InsertTable(10, t);
        ///
        ///     // Save all changes made to document b.
        ///     documentB.Save();
        /// }// Release this document from memory.
        /// </code>
        /// </example>
        public new Table InsertTable(int index, Table t)
        {
            Table t2 = base.InsertTable(index, t);
            t2.mainPart = mainPart;
            return t2;
        }

        /// <summary>
        /// Insert a Table into this document. The Table's source can be a completely different document.
        /// </summary>
        /// <param name="t">The Table to insert.</param>
        /// <returns>The Table now associated with this document.</returns>
        /// <example>
        /// Extract a Table from document a and insert it at the end of document b.
        /// <code>
        /// // Place holder for a Table.
        /// Table t;
        ///
        /// // Load document a.
        /// using (DocX documentA = DocX.Load(@"C:\Example\a.docx"))
        /// {
        ///     // Get the first Table from this document.
        ///     t = documentA.Tables[0];
        /// }
        ///
        /// // Load document b.
        /// using (DocX documentB = DocX.Load(@"C:\Example\b.docx"))
        /// {
        ///     /*
        ///      * Insert the Table that was extracted from document a, into document b.
        ///      * This creates a new Table that is now associated with document b.
        ///      */
        ///     Table newTable = documentB.InsertTable(t);
        ///
        ///     // Save all changes made to document b.
        ///     documentB.Save();
        /// }// Release this document from memory.
        /// </code>
        /// </example>
        public new Table InsertTable(Table t)
        {
            t = base.InsertTable(t);
            t.mainPart = mainPart;
            return t;
        }

        /// <summary>
        /// Insert a new Table at the end of this document.
        /// </summary>
        /// <param name="columnCount">The number of columns to create.</param>
        /// <param name="rowCount">The number of rows to create.</param>
        /// <param name="index">The index to insert this Table at.</param>
        /// <returns>A new Table.</returns>
        /// <example>
        /// Insert a new Table with 2 columns and 3 rows, at index 37 in this document.
        /// <code>
        /// // Create a document.
        /// using (DocX document = DocX.Load(@"C:\Example\Test.docx"))
        /// {
        ///     // Create a new Table with 3 rows and 2 columns. Insert this Table at index 37.
        ///     Table newTable = document.InsertTable(37, 3, 2);
        ///
        ///     // Set the design of this Table.
        ///     newTable.Design = TableDesign.LightShadingAccent3;
        ///
        ///     // Set the column names.
        ///     newTable.Rows[0].Cells[0].Paragraph.InsertText("Ice Cream", false);
        ///     newTable.Rows[0].Cells[1].Paragraph.InsertText("Price", false);
        ///
        ///     // Fill row 1
        ///     newTable.Rows[1].Cells[0].Paragraph.InsertText("Chocolate", false);
        ///     newTable.Rows[1].Cells[1].Paragraph.InsertText("€3:50", false);
        ///
        ///     // Fill row 2
        ///     newTable.Rows[2].Cells[0].Paragraph.InsertText("Vanilla", false);
        ///     newTable.Rows[2].Cells[1].Paragraph.InsertText("€3:00", false);
        ///
        ///     // Save all changes made to document b.
        ///     document.Save();
        /// }// Release this document from memory.
        /// </code>
        /// </example>
        public new Table InsertTable(int index, int rowCount, int columnCount)
        {
            if (rowCount < 1 || columnCount < 1)
                throw new ArgumentOutOfRangeException("Row and Column count must be greater than zero.");

            Table t = base.InsertTable(index, rowCount, columnCount);
            t.mainPart = mainPart;
            return t;
        }

        ///<summary>
        /// Applies document template to the document. Document template may include styles, headers, footers, properties, etc. as well as text content.
        ///</summary>
        ///<param name="templateFilePath">The path to the document template file.</param>
        ///<exception cref="FileNotFoundException">The document template file not found.</exception>
        public void ApplyTemplate(string templateFilePath)
        {
            ApplyTemplate(templateFilePath, true);
        }

        ///<summary>
        /// Applies document template to the document. Document template may include styles, headers, footers, properties, etc. as well as text content.
        ///</summary>
        ///<param name="templateFilePath">The path to the document template file.</param>
        ///<param name="includeContent">Whether to copy the document template text content to document.</param>
        ///<exception cref="FileNotFoundException">The document template file not found.</exception>
        public void ApplyTemplate(string templateFilePath, bool includeContent)
        {
            if (!File.Exists(templateFilePath))
            {
                throw new FileNotFoundException(string.Format("File could not be found {0}", templateFilePath));
            }
            using (FileStream packageStream = new FileStream(templateFilePath, FileMode.Open, FileAccess.Read))
            {
                ApplyTemplate(packageStream, includeContent);
            }
        }

        ///<summary>
        /// Applies document template to the document. Document template may include styles, headers, footers, properties, etc. as well as text content.
        ///</summary>
        ///<param name="templateStream">The stream of the document template file.</param>
        public void ApplyTemplate(Stream templateStream)
        {
            ApplyTemplate(templateStream, true);
        }

        ///<summary>
        /// Applies document template to the document. Document template may include styles, headers, footers, properties, etc. as well as text content.
        ///</summary>
        ///<param name="templateStream">The stream of the document template file.</param>
        ///<param name="includeContent">Whether to copy the document template text content to document.</param>
        public void ApplyTemplate(Stream templateStream, bool includeContent)
        {
            Package templatePackage = Package.Open(templateStream);
            try
            {
                PackagePart documentPart = null;
                XDocument documentDoc = null;
                foreach (PackagePart packagePart in templatePackage.GetParts())
                {
                    switch (packagePart.Uri.ToString())
                    {
                        case "/word/document.xml":
                            documentPart = packagePart;
                            using (XmlReader xr = XmlReader.Create(packagePart.GetStream(FileMode.Open, FileAccess.Read)))
                            {
                                documentDoc = XDocument.Load(xr);
                            }
                            break;

                        case "/_rels/.rels":
                            if (!this.package.PartExists(packagePart.Uri))
                            {
                                this.package.CreatePart(packagePart.Uri, packagePart.ContentType, packagePart.CompressionOption);
                            }
                            PackagePart globalRelsPart = this.package.GetPart(packagePart.Uri);
                            using (
                              StreamReader tr = new StreamReader(
                                packagePart.GetStream(FileMode.Open, FileAccess.Read), Encoding.UTF8))
                            {
                                using (
                                  StreamWriter tw = new StreamWriter(
                                    globalRelsPart.GetStream(FileMode.Create, FileAccess.Write), Encoding.UTF8))
                                {
                                    tw.Write(tr.ReadToEnd());
                                }
                            }
                            break;

                        case "/word/_rels/document.xml.rels":
                            break;

                        default:
                            if (!this.package.PartExists(packagePart.Uri))
                            {
                                this.package.CreatePart(packagePart.Uri, packagePart.ContentType, packagePart.CompressionOption);
                            }
                            Encoding packagePartEncoding = Encoding.Default;
                            if (packagePart.Uri.ToString().EndsWith(".xml") || packagePart.Uri.ToString().EndsWith(".rels"))
                            {
                                packagePartEncoding = Encoding.UTF8;
                            }
                            PackagePart nativePart = this.package.GetPart(packagePart.Uri);
                            using (
                              StreamReader tr = new StreamReader(
                                packagePart.GetStream(FileMode.Open, FileAccess.Read), packagePartEncoding))
                            {
                                using (
                                  StreamWriter tw = new StreamWriter(
                                    nativePart.GetStream(FileMode.Create, FileAccess.Write), tr.CurrentEncoding))
                                {
                                    tw.Write(tr.ReadToEnd());
                                }
                            }
                            break;
                    }
                }
                if (documentPart != null)
                {
                    string mainContentType = documentPart.ContentType.Replace("template.main", "document.main");
                    if (this.package.PartExists(documentPart.Uri))
                    {
                        this.package.DeletePart(documentPart.Uri);
                    }
                    PackagePart documentNewPart = this.package.CreatePart(
                      documentPart.Uri, mainContentType, documentPart.CompressionOption);
                    using (XmlWriter xw = XmlWriter.Create(documentNewPart.GetStream(FileMode.Create, FileAccess.Write)))
                    {
                        documentDoc.WriteTo(xw);
                    }
                    foreach (PackageRelationship documentPartRel in documentPart.GetRelationships())
                    {
                        documentNewPart.CreateRelationship(
                          documentPartRel.TargetUri,
                          documentPartRel.TargetMode,
                          documentPartRel.RelationshipType,
                          documentPartRel.Id);
                    }
                    this.mainPart = documentNewPart;
                    this.mainDoc = documentDoc;
                    PopulateDocument(this, templatePackage);

                    // DragonFire: I added next line and recovered ApplyTemplate method.
                    // I do it, becouse  PopulateDocument(...) writes into field "settingsPart" the part of Template's package
                    //  and after line "templatePackage.Close();" in finally, field "settingsPart" becomes not available and method "Save" throw an exception...
                    // That's why I recreated settingsParts and unlinked it from Template's package =)
                    settingsPart = HelperFunctions.CreateOrGetSettingsPart(package);
                }
                if (!includeContent)
                {
                    foreach (Paragraph paragraph in this.Paragraphs)
                    {
                        paragraph.Remove(false);
                    }
                }
            }
            finally
            {
                this.package.Flush();
                var documentRelsPart = this.package.GetPart(new Uri("/word/_rels/document.xml.rels", UriKind.Relative));
                using (TextReader tr = new StreamReader(documentRelsPart.GetStream(FileMode.Open, FileAccess.Read)))
                {
                    tr.Read();
                }
                templatePackage.Close();
                PopulateDocument(Document, package);
            }
        }

        /// <summary>
        /// Add an Image into this document from a fully qualified or relative filename.
        /// </summary>
        /// <param name="filename">The fully qualified or relative filename.</param>
        /// <returns>An Image file.</returns>
        /// <example>
        /// Add an Image into this document from a fully qualified filename.
        /// <code>
        /// // Load a document.
        /// using (DocX document = DocX.Load(@"C:\Example\Test.docx"))
        /// {
        ///     // Add an Image from a file.
        ///     document.AddImage(@"C:\Example\Image.png");
        ///
        ///     // Save all changes made to this document.
        ///     document.Save();
        /// }// Release this document from memory.
        /// </code>
        /// </example>
        /// <seealso cref="AddImage(System.IO.Stream)"/>
        /// <seealso cref="Paragraph.InsertPicture"/>
        public Image AddImage(string filename)
        {
            string contentType = "";

            // The extension this file has will be taken to be its format.
            switch (Path.GetExtension(filename))
            {
                case ".tiff": contentType = "image/tif"; break;
                case ".tif": contentType = "image/tif"; break;
                case ".png": contentType = "image/png"; break;
                case ".bmp": contentType = "image/png"; break;
                case ".gif": contentType = "image/gif"; break;
                case ".jpg": contentType = "image/jpg"; break;
                case ".jpeg": contentType = "image/jpeg"; break;
                default: contentType = "image/jpg"; break;
            }

            return AddImage(filename as object, contentType);
        }

        /// <summary>
        /// Add an Image into this document from a Stream.
        /// </summary>
        /// <param name="stream">A Stream stream.</param>
        /// <returns>An Image file.</returns>
        /// <example>
        /// Add an Image into a document using a Stream.
        /// <code>
        /// // Open a FileStream fs to an Image.
        /// using (FileStream fs = new FileStream(@"C:\Example\Image.jpg", FileMode.Open))
        /// {
        ///     // Load a document.
        ///     using (DocX document = DocX.Load(@"C:\Example\Test.docx"))
        ///     {
        ///         // Add an Image from a filestream fs.
        ///         document.AddImage(fs);
        ///
        ///         // Save all changes made to this document.
        ///         document.Save();
        ///     }// Release this document from memory.
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="AddImage(string)"/>
        /// <seealso cref="Paragraph.InsertPicture"/>
        public Image AddImage(Stream stream)
        {
            return AddImage(stream as object);
        }

        /// <summary>
        /// Adds a hyperlink to a document and creates a Paragraph which uses it.
        /// </summary>
        /// <param name="text">The text as displayed by the hyperlink.</param>
        /// <param name="uri">The hyperlink itself.</param>
        /// <returns>Returns a hyperlink that can be inserted into a Paragraph.</returns>
        /// <example>
        /// Adds a hyperlink to a document and creates a Paragraph which uses it.
        /// <code>
        /// // Create a document.
        /// using (DocX document = DocX.Create(@"Test.docx"))
        /// {
        ///    // Add a hyperlink to this document.
        ///    Hyperlink h = document.AddHyperlink("Google", new Uri("http://www.google.com"));
        ///
        ///    // Add a new Paragraph to this document.
        ///    Paragraph p = document.InsertParagraph();
        ///    p.Append("My favourite search engine is ");
        ///    p.AppendHyperlink(h);
        ///    p.Append(", I think it's great.");
        ///
        ///    // Save all changes made to this document.
        ///    document.Save();
        /// }
        /// </code>
        /// </example>
        public Hyperlink AddHyperlink(string text, Uri uri)
        {
            XElement i = new XElement
            (
                XName.Get("hyperlink", DocX.w.NamespaceName),
                new XAttribute(r + "id", string.Empty),
                new XAttribute(w + "history", "1"),
                new XElement(XName.Get("r", DocX.w.NamespaceName),
                new XElement(XName.Get("rPr", DocX.w.NamespaceName),
                new XElement(XName.Get("rStyle", DocX.w.NamespaceName),
                new XAttribute(w + "val", "Hyperlink"))),
                new XElement(XName.Get("t", DocX.w.NamespaceName), text))
            );

            Hyperlink h = new Hyperlink(this, mainPart, i);

            h.text = text;
            h.uri = uri;

            AddHyperlinkStyleIfNotPresent();

            return h;
        }

        /// <summary>
        /// Adds three new Headers to this document. One for the first page, one for odd pages and one for even pages.
        /// </summary>
        /// <example>
        /// // Create a document.
        /// using (DocX document = DocX.Create(@"Test.docx"))
        /// {
        ///     // Add header support to this document.
        ///     document.AddHeaders();
        ///
        ///     // Get a collection of all headers in this document.
        ///     Headers headers = document.Headers;
        ///
        ///     // The header used for the first page of this document.
        ///     Header first = headers.first;
        ///
        ///     // The header used for odd pages of this document.
        ///     Header odd = headers.odd;
        ///
        ///     // The header used for even pages of this document.
        ///     Header even = headers.even;
        ///
        ///     // Force the document to use a different header for first, odd and even pages.
        ///     document.DifferentFirstPage = true;
        ///     document.DifferentOddAndEvenPages = true;
        ///
        ///     // Content can be added to the Headers in the same manor that it would be added to the main document.
        ///     Paragraph p = first.InsertParagraph();
        ///     p.Append("This is the first pages header.");
        ///
        ///     // Save all changes to this document.
        ///     document.Save();
        /// }// Release this document from memory.
        /// </example>
        public void AddHeaders()
        {
            AddHeadersOrFooters(true);

            headers.odd = Document.GetHeaderByType("default");
            headers.even = Document.GetHeaderByType("even");
            headers.first = Document.GetHeaderByType("first");
        }

        /// <summary>
        /// Adds three new Footers to this document. One for the first page, one for odd pages and one for even pages.
        /// </summary>
        /// <example>
        /// // Create a document.
        /// using (DocX document = DocX.Create(@"Test.docx"))
        /// {
        ///     // Add footer support to this document.
        ///     document.AddFooters();
        ///
        ///     // Get a collection of all footers in this document.
        ///     Footers footers = document.Footers;
        ///
        ///     // The footer used for the first page of this document.
        ///     Footer first = footers.first;
        ///
        ///     // The footer used for odd pages of this document.
        ///     Footer odd = footers.odd;
        ///
        ///     // The footer used for even pages of this document.
        ///     Footer even = footers.even;
        ///
        ///     // Force the document to use a different footer for first, odd and even pages.
        ///     document.DifferentFirstPage = true;
        ///     document.DifferentOddAndEvenPages = true;
        ///
        ///     // Content can be added to the Footers in the same manor that it would be added to the main document.
        ///     Paragraph p = first.InsertParagraph();
        ///     p.Append("This is the first pages footer.");
        ///
        ///     // Save all changes to this document.
        ///     document.Save();
        /// }// Release this document from memory.
        /// </example>
        public void AddFooters()
        {
            AddHeadersOrFooters(false);

            footers.odd = Document.GetFooterByType("default");
            footers.even = Document.GetFooterByType("even");
            footers.first = Document.GetFooterByType("first");
        }

        public virtual void InsertSectionWithHeader()
        {
            var newParagraphSection = new XElement
            (
                XName.Get("p", DocX.w.NamespaceName), new XElement(XName.Get("pPr", DocX.w.NamespaceName), new XElement(XName.Get("sectPr", DocX.w.NamespaceName), new XElement(XName.Get("type", DocX.w.NamespaceName), new XAttribute(DocX.w + "val", "continuous"))))
            );

            string element = "hdr";
            string reference = "header";

            XElement sectPr = newParagraphSection.Descendants(w + "sectPr").FirstOrDefault();

            string header_uri = string.Format("/word/{0}{1}.xml", reference, Headers.headers.Count() + 4);

            PackagePart headerPart = package.CreatePart(new Uri(header_uri, UriKind.Relative), string.Format("application/vnd.openxmlformats-officedocument.wordprocessingml.{0}+xml", reference));
            PackageRelationship headerRelationship = mainPart.CreateRelationship(headerPart.Uri, TargetMode.Internal, string.Format("http://schemas.openxmlformats.org/officeDocument/2006/relationships/{0}", reference));

            XDocument doc;
            // Load the document part into a XDocument object
            using (TextReader tr = new StreamReader(headerPart.GetStream(FileMode.Create, FileAccess.ReadWrite)))
            {
                doc = XDocument.Parse
                (string.Format(@"<?xml version=""1.0"" encoding=""utf-16"" standalone=""yes""?>
                       <w:{0} xmlns:ve=""http://schemas.openxmlformats.org/markup-compatibility/2006"" xmlns:o=""urn:schemas-microsoft-com:office:office"" xmlns:r=""http://schemas.openxmlformats.org/officeDocument/2006/relationships"" xmlns:m=""http://schemas.openxmlformats.org/officeDocument/2006/math"" xmlns:v=""urn:schemas-microsoft-com:vml"" xmlns:wp=""http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing"" xmlns:w10=""urn:schemas-microsoft-com:office:word"" xmlns:w=""http://schemas.openxmlformats.org/wordprocessingml/2006/main"" xmlns:wne=""http://schemas.microsoft.com/office/word/2006/wordml"">
                         <w:p w:rsidR=""009D472B"" w:rsidRDefault=""009D472B"">
                           <w:pPr>
                             <w:pStyle w:val=""{1}"" />
                           </w:pPr>
                         </w:p>
                       </w:{0}>", element, reference)
                );
            }

            Header header = new Header(this, doc.Element(w + "hdr"), headerPart);

            Paragraph p0 = header.InsertParagraph();
            p0.Append("Hello First Header1.").Bold();

            // Save the main document
            using (TextWriter tw = new StreamWriter(headerPart.GetStream(FileMode.Create, FileAccess.Write)))
                doc.Save(tw, SaveOptions.None);
            string type;
            type = "default";

            sectPr.Add
                (
                    new XElement
                    (
                        w + string.Format("{0}Reference", reference),
                        new XAttribute(w + "type", type),
                        new XAttribute(r + "id", headerRelationship.Id)
                    )
                );

            Xml.Add(newParagraphSection);
        }

        /// <summary>
        /// Save this document back to the location it was loaded from.
        /// </summary>
        /// <example>
        /// <code>
        /// // Load a document.
        /// using (DocX document = DocX.Load(@"C:\Example\Test.docx"))
        /// {
        ///     // Add an Image from a file.
        ///     document.AddImage(@"C:\Example\Image.jpg");
        ///
        ///     // Save all changes made to this document.
        ///     document.Save();
        /// }// Release this document from memory.
        /// </code>
        /// </example>
        /// <seealso cref="DocX.SaveAs(string)"/>
        /// <seealso cref="DocX.Create(System.IO.Stream)"/>
        /// <seealso cref="DocX.Create(string)"/>
        /// <seealso cref="DocX.Load(System.IO.Stream)"/>
        /// <seealso cref="DocX.Load(string)"/>
        /// <!--
        /// Bug found and fixed by krugs525 on August 12 2009.
        /// Use TFS compare to see exact code change.
        /// -->
        public void Save()
        {
            Headers headers = Headers;

            // Save the main document
            using (TextWriter tw = new StreamWriter(mainPart.GetStream(FileMode.Create, FileAccess.Write)))
                mainDoc.Save(tw, SaveOptions.None);

            using (TextReader tr = new StreamReader(settingsPart.GetStream()))
                settings = XDocument.Load(tr);

            XElement body = mainDoc.Root.Element(w + "body");
            XElement sectPr = body.Descendants(w + "sectPr").FirstOrDefault();

            if (sectPr != null)
            {
                var evenHeaderRef =
                (
                    from e in mainDoc.Descendants(w + "headerReference")
                    let type = e.Attribute(w + "type")
                    where type != null && type.Value.Equals("even", StringComparison.CurrentCultureIgnoreCase)
                    select e.Attribute(r + "id").Value
                 ).LastOrDefault();

                if (evenHeaderRef != null)
                {
                    XElement even = headers.even.Xml;

                    Uri target = PackUriHelper.ResolvePartUri
                    (
                        mainPart.Uri,
                        mainPart.GetRelationship(evenHeaderRef).TargetUri
                    );

                    using (TextWriter tw = new StreamWriter(package.GetPart(target).GetStream(FileMode.Create, FileAccess.Write)))
                    {
                        new XDocument
                        (
                            new XDeclaration("1.0", "UTF-8", "yes"),
                            even
                        ).Save(tw, SaveOptions.None);
                    }
                }

                var oddHeaderRef =
                (
                    from e in mainDoc.Descendants(w + "headerReference")
                    let type = e.Attribute(w + "type")
                    where type != null && type.Value.Equals("default", StringComparison.CurrentCultureIgnoreCase)
                    select e.Attribute(r + "id").Value
                 ).FirstOrDefault();

                if (oddHeaderRef != null)
                {
                    XElement odd = headers.odd.Xml;

                    Uri target = PackUriHelper.ResolvePartUri
                    (
                        mainPart.Uri,
                        mainPart.GetRelationship(oddHeaderRef).TargetUri
                    );

                    // Save header1
                    using (TextWriter tw = new StreamWriter(package.GetPart(target).GetStream(FileMode.Create, FileAccess.Write)))
                    {
                        new XDocument
                        (
                            new XDeclaration("1.0", "UTF-8", "yes"),
                            odd
                        ).Save(tw, SaveOptions.None);
                    }
                }

                var firstHeaderRef =
                (
                    from e in mainDoc.Descendants(w + "headerReference")
                    let type = e.Attribute(w + "type")
                    where type != null && type.Value.Equals("first", StringComparison.CurrentCultureIgnoreCase)
                    select e.Attribute(r + "id").Value
                 ).LastOrDefault();

                if (firstHeaderRef != null)
                {
                    XElement first = headers.first.Xml;
                    Uri target = PackUriHelper.ResolvePartUri
                    (
                        mainPart.Uri,
                        mainPart.GetRelationship(firstHeaderRef).TargetUri
                    );

                    // Save header3
                    using (TextWriter tw = new StreamWriter(package.GetPart(target).GetStream(FileMode.Create, FileAccess.Write)))
                    {
                        new XDocument
                        (
                            new XDeclaration("1.0", "UTF-8", "yes"),
                            first
                        ).Save(tw, SaveOptions.None);
                    }
                }

                var oddFooterRef =
                (
                    from e in mainDoc.Descendants(w + "footerReference")
                    let type = e.Attribute(w + "type")
                    where type != null && type.Value.Equals("default", StringComparison.CurrentCultureIgnoreCase)
                    select e.Attribute(r + "id").Value
                 ).LastOrDefault();

                if (oddFooterRef != null)
                {
                    XElement odd = footers.odd.Xml;
                    Uri target = PackUriHelper.ResolvePartUri
                    (
                        mainPart.Uri,
                        mainPart.GetRelationship(oddFooterRef).TargetUri
                    );

                    // Save header1
                    using (TextWriter tw = new StreamWriter(package.GetPart(target).GetStream(FileMode.Create, FileAccess.Write)))
                    {
                        new XDocument
                        (
                            new XDeclaration("1.0", "UTF-8", "yes"),
                            odd
                        ).Save(tw, SaveOptions.None);
                    }
                }

                var evenFooterRef =
                (
                    from e in mainDoc.Descendants(w + "footerReference")
                    let type = e.Attribute(w + "type")
                    where type != null && type.Value.Equals("even", StringComparison.CurrentCultureIgnoreCase)
                    select e.Attribute(r + "id").Value
                 ).LastOrDefault();

                if (evenFooterRef != null)
                {
                    XElement even = footers.even.Xml;
                    Uri target = PackUriHelper.ResolvePartUri
                    (
                        mainPart.Uri,
                        mainPart.GetRelationship(evenFooterRef).TargetUri
                    );

                    // Save header2
                    using (TextWriter tw = new StreamWriter(package.GetPart(target).GetStream(FileMode.Create, FileAccess.Write)))
                    {
                        new XDocument
                        (
                            new XDeclaration("1.0", "UTF-8", "yes"),
                            even
                        ).Save(tw, SaveOptions.None);
                    }
                }

                var firstFooterRef =
                (
                     from e in mainDoc.Descendants(w + "footerReference")
                     let type = e.Attribute(w + "type")
                     where type != null && type.Value.Equals("first", StringComparison.CurrentCultureIgnoreCase)
                     select e.Attribute(r + "id").Value
                ).LastOrDefault();

                if (firstFooterRef != null)
                {
                    XElement first = footers.first.Xml;
                    Uri target = PackUriHelper.ResolvePartUri
                    (
                        mainPart.Uri,
                        mainPart.GetRelationship(firstFooterRef).TargetUri
                    );

                    // Save header3
                    using (TextWriter tw = new StreamWriter(package.GetPart(target).GetStream(FileMode.Create, FileAccess.Write)))
                    {
                        new XDocument
                        (
                            new XDeclaration("1.0", "UTF-8", "yes"),
                            first
                        ).Save(tw, SaveOptions.None);
                    }
                }

                if (headers.headers.Count() > 0)
                {
                    foreach (Header header in headers.headers)
                    {
                        using (TextWriter tw = new StreamWriter(header.mainPart.GetStream(FileMode.Create, FileAccess.Write)))
                            header.Xml.Save(tw, SaveOptions.None);
                    }
                }

                // Save the settings document.
                using (TextWriter tw = new StreamWriter(settingsPart.GetStream(FileMode.Create, FileAccess.Write)))
                    settings.Save(tw, SaveOptions.None);

                if (endnotesPart != null)
                {
                    using (TextWriter tw = new StreamWriter(endnotesPart.GetStream(FileMode.Create, FileAccess.Write)))
                        endnotes.Save(tw, SaveOptions.None);
                }

                if (footnotesPart != null)
                {
                    using (TextWriter tw = new StreamWriter(footnotesPart.GetStream(FileMode.Create, FileAccess.Write)))
                        footnotes.Save(tw, SaveOptions.None);
                }

                if (stylesPart != null)
                {
                    using (TextWriter tw = new StreamWriter(stylesPart.GetStream(FileMode.Create, FileAccess.Write)))
                        styles.Save(tw, SaveOptions.None);
                }

                if (stylesWithEffectsPart != null)
                {
                    using (TextWriter tw = new StreamWriter(stylesWithEffectsPart.GetStream(FileMode.Create, FileAccess.Write)))
                        stylesWithEffects.Save(tw, SaveOptions.None);
                }

                if (numberingPart != null)
                {
                    using (TextWriter tw = new StreamWriter(numberingPart.GetStream(FileMode.Create, FileAccess.Write)))
                        numbering.Save(tw, SaveOptions.None);
                }

                if (fontTablePart != null)
                {
                    using (TextWriter tw = new StreamWriter(fontTablePart.GetStream(FileMode.Create, FileAccess.Write)))
                        fontTable.Save(tw, SaveOptions.None);
                }
            }

            // Close the document so that it can be saved.
            package.Flush();

            #region Save this document back to a file or stream, that was specified by the user at save time.

            if (filename != null)
            {
                using (FileStream fs = new FileStream(filename, FileMode.Create))
                {
                    fs.Write(memoryStream.ToArray(), 0, (int)memoryStream.Length);
                }
            }
            else
            {
                if (stream.CanSeek) // 2013-05-25: Check if stream can be seeked to support System.Web.HttpResponseStream
                {
                    // Set the length of this stream to 0
                    stream.SetLength(0);

                    // Write to the beginning of the stream
                    stream.Position = 0;
                }

                memoryStream.WriteTo(stream);
                memoryStream.Flush();
            }

            #endregion Save this document back to a file or stream, that was specified by the user at save time.
        }

        /// <summary>
        /// Save this document to a file.
        /// </summary>
        /// <param name="filename">The filename to save this document as.</param>
        /// <example>
        /// Load a document from one file and save it to another.
        /// <code>
        /// // Load a document using its fully qualified filename.
        /// DocX document = DocX.Load(@"C:\Example\Test1.docx");
        ///
        /// // Insert a new Paragraph
        /// document.InsertParagraph("Hello world!", false);
        ///
        /// // Save the document to a new location.
        /// document.SaveAs(@"C:\Example\Test2.docx");
        /// </code>
        /// </example>
        /// <example>
        /// Load a document from a Stream and save it to a file.
        /// <code>
        /// DocX document;
        /// using (FileStream fs1 = new FileStream(@"C:\Example\Test1.docx", FileMode.Open))
        /// {
        ///     // Load a document using a stream.
        ///     document = DocX.Load(fs1);
        ///
        ///     // Insert a new Paragraph
        ///     document.InsertParagraph("Hello world again!", false);
        /// }
        ///
        /// // Save the document to a new location.
        /// document.SaveAs(@"C:\Example\Test2.docx");
        /// </code>
        /// </example>
        /// <seealso cref="DocX.Save()"/>
        /// <seealso cref="DocX.Create(System.IO.Stream)"/>
        /// <seealso cref="DocX.Create(string)"/>
        /// <seealso cref="DocX.Load(System.IO.Stream)"/>
        /// <seealso cref="DocX.Load(string)"/>
        public void SaveAs(string filename)
        {
            this.filename = filename;
            this.stream = null;
            Save();
        }

        /// <summary>
        /// Save this document to a Stream.
        /// </summary>
        /// <param name="stream">The Stream to save this document to.</param>
        /// <example>
        /// Load a document from a file and save it to a Stream.
        /// <code>
        /// // Place holder for a document.
        /// DocX document;
        ///
        /// using (FileStream fs1 = new FileStream(@"C:\Example\Test1.docx", FileMode.Open))
        /// {
        ///     // Load a document using a stream.
        ///     document = DocX.Load(fs1);
        ///
        ///     // Insert a new Paragraph
        ///     document.InsertParagraph("Hello world again!", false);
        /// }
        ///
        /// using (FileStream fs2 = new FileStream(@"C:\Example\Test2.docx", FileMode.Create))
        /// {
        ///     // Save the document to a different stream.
        ///     document.SaveAs(fs2);
        /// }
        ///
        /// // Release this document from memory.
        /// document.Dispose();
        /// </code>
        /// </example>
        /// <example>
        /// Load a document from one Stream and save it to another.
        /// <code>
        /// DocX document;
        /// using (FileStream fs1 = new FileStream(@"C:\Example\Test1.docx", FileMode.Open))
        /// {
        ///     // Load a document using a stream.
        ///     document = DocX.Load(fs1);
        ///
        ///     // Insert a new Paragraph
        ///     document.InsertParagraph("Hello world again!", false);
        /// }
        ///
        /// using (FileStream fs2 = new FileStream(@"C:\Example\Test2.docx", FileMode.Create))
        /// {
        ///     // Save the document to a different stream.
        ///     document.SaveAs(fs2);
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="DocX.Save()"/>
        /// <seealso cref="DocX.Create(System.IO.Stream)"/>
        /// <seealso cref="DocX.Create(string)"/>
        /// <seealso cref="DocX.Load(System.IO.Stream)"/>
        /// <seealso cref="DocX.Load(string)"/>
        public void SaveAs(Stream stream)
        {
            this.filename = null;
            this.stream = stream;
            Save();
        }

        /// <summary>
        /// Add a core property to this document. If a core property already exists with the same name it will be replaced. Core property names are case insensitive.
        /// </summary>
        ///<param name="propertyName">The property name.</param>
        ///<param name="propertyValue">The property value.</param>
        ///<example>
        /// Add a core properties of each type to a document.
        /// <code>
        /// // Load Example.docx
        /// using (DocX document = DocX.Load(@"C:\Example\Test.docx"))
        /// {
        ///     // If this document does not contain a core property called 'forename', create one.
        ///     if (!document.CoreProperties.ContainsKey("forename"))
        ///     {
        ///         // Create a new core property called 'forename' and set its value.
        ///         document.AddCoreProperty("forename", "Cathal");
        ///     }
        ///
        ///     // Get this documents core property called 'forename'.
        ///     string forenameValue = document.CoreProperties["forename"];
        ///
        ///     // Print all of the information about this core property to Console.
        ///     Console.WriteLine(string.Format("Name: '{0}', Value: '{1}'\nPress any key...", "forename", forenameValue));
        ///
        ///     // Save all changes made to this document.
        ///     document.Save();
        /// } // Release this document from memory.
        ///
        /// // Wait for the user to press a key before exiting.
        /// Console.ReadKey();
        /// </code>
        /// </example>
        /// <seealso cref="CoreProperties"/>
        /// <seealso cref="CustomProperty"/>
        /// <seealso cref="CustomProperties"/>
        public void AddCoreProperty(string propertyName, string propertyValue)
        {
            string propertyNamespacePrefix = propertyName.Contains(":") ? propertyName.Split(new[] { ':' })[0] : "cp";
            string propertyLocalName = propertyName.Contains(":") ? propertyName.Split(new[] { ':' })[1] : propertyName;

            // If this document does not contain a coreFilePropertyPart create one.)
            if (!package.PartExists(new Uri("/docProps/core.xml", UriKind.Relative)))
                throw new Exception("Core properties part doesn't exist.");

            XDocument corePropDoc;
            PackagePart corePropPart = package.GetPart(new Uri("/docProps/core.xml", UriKind.Relative));
            using (TextReader tr = new StreamReader(corePropPart.GetStream(FileMode.Open, FileAccess.Read)))
            {
                corePropDoc = XDocument.Load(tr);
            }

            XElement corePropElement =
              (from propElement in corePropDoc.Root.Elements()
               where (propElement.Name.LocalName.Equals(propertyLocalName))
               select propElement).SingleOrDefault();
            if (corePropElement != null)
            {
                corePropElement.SetValue(propertyValue);
            }
            else
            {
                var propertyNamespace = corePropDoc.Root.GetNamespaceOfPrefix(propertyNamespacePrefix);
                corePropDoc.Root.Add(new XElement(XName.Get(propertyLocalName, propertyNamespace.NamespaceName), propertyValue));
            }

            using (TextWriter tw = new StreamWriter(corePropPart.GetStream(FileMode.Create, FileAccess.Write)))
            {
                corePropDoc.Save(tw);
            }
            UpdateCorePropertyValue(this, propertyLocalName, propertyValue);
        }

        /// <summary>
        /// Add a custom property to this document. If a custom property already exists with the same name it will be replace. CustomProperty names are case insensitive.
        /// </summary>
        /// <param name="cp">The CustomProperty to add to this document.</param>
        /// <example>
        /// Add a custom properties of each type to a document.
        /// <code>
        /// // Load Example.docx
        /// using (DocX document = DocX.Load(@"C:\Example\Test.docx"))
        /// {
        ///     // A CustomProperty called forename which stores a string.
        ///     CustomProperty forename;
        ///
        ///     // If this document does not contain a custom property called 'forename', create one.
        ///     if (!document.CustomProperties.ContainsKey("forename"))
        ///     {
        ///         // Create a new custom property called 'forename' and set its value.
        ///         document.AddCustomProperty(new CustomProperty("forename", "Cathal"));
        ///     }
        ///
        ///     // Get this documents custom property called 'forename'.
        ///     forename = document.CustomProperties["forename"];
        ///
        ///     // Print all of the information about this CustomProperty to Console.
        ///     Console.WriteLine(string.Format("Name: '{0}', Value: '{1}'\nPress any key...", forename.Name, forename.Value));
        ///
        ///     // Save all changes made to this document.
        ///     document.Save();
        /// } // Release this document from memory.
        ///
        /// // Wait for the user to press a key before exiting.
        /// Console.ReadKey();
        /// </code>
        /// </example>
        /// <seealso cref="CustomProperty"/>
        /// <seealso cref="CustomProperties"/>
        public void AddCustomProperty(CustomProperty cp)
        {
            // If this document does not contain a customFilePropertyPart create one.
            if (!package.PartExists(new Uri("/docProps/custom.xml", UriKind.Relative)))
                HelperFunctions.CreateCustomPropertiesPart(this);

            XDocument customPropDoc;
            PackagePart customPropPart = package.GetPart(new Uri("/docProps/custom.xml", UriKind.Relative));
            using (TextReader tr = new StreamReader(customPropPart.GetStream(FileMode.Open, FileAccess.Read)))
                customPropDoc = XDocument.Load(tr, LoadOptions.PreserveWhitespace);

            // Each custom property has a PID, get the highest PID in this document.
            IEnumerable<int> pids =
            (
                from d in customPropDoc.Descendants()
                where d.Name.LocalName == "property"
                select int.Parse(d.Attribute(XName.Get("pid")).Value)
            );

            int pid = 1;
            if (pids.Count() > 0)
                pid = pids.Max();

            // Check if a custom property already exists with this name
            // 2013-05-25: IgnoreCase while searching for custom property as it would produce a currupted docx.
            var customProperty =
            (
                from d in customPropDoc.Descendants()
                where (d.Name.LocalName == "property") && (d.Attribute(XName.Get("name")).Value.Equals(cp.Name, StringComparison.InvariantCultureIgnoreCase))
                select d
            ).SingleOrDefault();

            // If a custom property with this name already exists remove it.
            if (customProperty != null)
                customProperty.Remove();

            XElement propertiesElement = customPropDoc.Element(XName.Get("Properties", customPropertiesSchema.NamespaceName));
            propertiesElement.Add
            (
                new XElement
                (
                    XName.Get("property", customPropertiesSchema.NamespaceName),
                    new XAttribute("fmtid", "{D5CDD505-2E9C-101B-9397-08002B2CF9AE}"),
                    new XAttribute("pid", pid + 1),
                    new XAttribute("name", cp.Name),
                        new XElement(customVTypesSchema + cp.Type, cp.Value ?? "")
                )
            );

            // Save the custom properties
            using (TextWriter tw = new StreamWriter(customPropPart.GetStream(FileMode.Create, FileAccess.Write)))
                customPropDoc.Save(tw, SaveOptions.None);

            // Refresh all fields in this document which display this custom property.
            UpdateCustomPropertyValue(this, cp.Name, (cp.Value ?? "").ToString());
        }

        public override Paragraph InsertParagraph()
        {
            Paragraph p = base.InsertParagraph();
            p.PackagePart = mainPart;
            return p;
        }

        public override Paragraph InsertParagraph(int index, string text, bool trackChanges)
        {
            Paragraph p = base.InsertParagraph(index, text, trackChanges);
            p.PackagePart = mainPart;
            return p;
        }

        public override Paragraph InsertParagraph(Paragraph p)
        {
            p.PackagePart = mainPart;
            return base.InsertParagraph(p);
        }

        public override Paragraph InsertParagraph(int index, Paragraph p)
        {
            p.PackagePart = mainPart;
            return base.InsertParagraph(index, p);
        }

        public override Paragraph InsertParagraph(int index, string text, bool trackChanges, Formatting formatting)
        {
            Paragraph p = base.InsertParagraph(index, text, trackChanges, formatting);
            p.PackagePart = mainPart;
            return p;
        }

        public override Paragraph InsertParagraph(string text)
        {
            Paragraph p = base.InsertParagraph(text);
            p.PackagePart = mainPart;
            return p;
        }

        public override Paragraph InsertParagraph(string text, bool trackChanges)
        {
            Paragraph p = base.InsertParagraph(text, trackChanges);
            p.PackagePart = mainPart;
            return p;
        }

        public override Paragraph InsertParagraph(string text, bool trackChanges, Formatting formatting)
        {
            Paragraph p = base.InsertParagraph(text, trackChanges, formatting);
            p.PackagePart = mainPart;

            return p;
        }

        public Paragraph[] InsertParagraphs(string text)
        {
            String[] textArray = text.Split('\n');
            List<Paragraph> paragraphs = new List<Paragraph>();
            foreach (var textForParagraph in textArray)
            {
                Paragraph p = base.InsertParagraph(text);
                p.PackagePart = mainPart;
                paragraphs.Add(p);
            }
            return paragraphs.ToArray();
        }

        /// <summary>
        /// Create an equation and insert it in the new paragraph
        /// </summary>
        public override Paragraph InsertEquation(String equation)
        {
            Paragraph p = base.InsertEquation(equation);
            p.PackagePart = mainPart;
            return p;
        }

        /// <summary>
        /// Insert a chart in document
        /// </summary>
        public void InsertChart(Chart chart)
        {
            // Create a new chart part uri.
            String chartPartUriPath = String.Empty;
            Int32 chartIndex = 1;
            do
            {
                chartPartUriPath = String.Format
                (
                    "/word/charts/chart{0}.xml",
                    chartIndex
                );
                chartIndex++;
            } while (package.PartExists(new Uri(chartPartUriPath, UriKind.Relative)));

            // Create chart part.
            PackagePart chartPackagePart = package.CreatePart(new Uri(chartPartUriPath, UriKind.Relative), "application/vnd.openxmlformats-officedocument.drawingml.chart+xml");

            // Create a new chart relationship
            String relID = GetNextFreeRelationshipID();
            PackageRelationship rel = mainPart.CreateRelationship(chartPackagePart.Uri, TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/chart", relID);

            // Save a chart info the chartPackagePart
            using (TextWriter tw = new StreamWriter(chartPackagePart.GetStream(FileMode.Create, FileAccess.Write)))
                chart.Xml.Save(tw);

            // Insert a new chart into a paragraph.
            Paragraph p = InsertParagraph();
            XElement chartElement = new XElement(
                XName.Get("r", DocX.w.NamespaceName),
                new XElement(
                    XName.Get("drawing", DocX.w.NamespaceName),
                    new XElement(
                        XName.Get("inline", DocX.wp.NamespaceName),
                        new XElement(XName.Get("extent", DocX.wp.NamespaceName), new XAttribute("cx", "5486400"), new XAttribute("cy", "3200400")),
                        new XElement(XName.Get("effectExtent", DocX.wp.NamespaceName), new XAttribute("l", "0"), new XAttribute("t", "0"), new XAttribute("r", "19050"), new XAttribute("b", "19050")),
                        new XElement(XName.Get("docPr", DocX.wp.NamespaceName), new XAttribute("id", "1"), new XAttribute("name", "chart")),
                        new XElement(
                            XName.Get("graphic", DocX.a.NamespaceName),
                            new XElement(
                                XName.Get("graphicData", DocX.a.NamespaceName),
                                new XAttribute("uri", DocX.c.NamespaceName),
                                new XElement(
                                    XName.Get("chart", DocX.c.NamespaceName),
                                    new XAttribute(XName.Get("id", DocX.r.NamespaceName), relID)
                                )
                            )
                        )
                    )
               ));
            p.Xml.Add(chartElement);
        }

        /// <summary>
        /// Inserts a default TOC into the current document.
        /// Title: Table of contents
        /// Swithces will be: TOC \h \o '1-3' \u \z
        /// </summary>
        /// <returns>The inserted TableOfContents</returns>
        public TableOfContents InsertDefaultTableOfContents()
        {
            return InsertTableOfContents("Table of contents", TableOfContentsSwitches.O | TableOfContentsSwitches.H | TableOfContentsSwitches.Z | TableOfContentsSwitches.U);
        }

        /// <summary>
        /// Inserts a TOC into the current document.
        /// </summary>
        /// <param name="title">The title of the TOC</param>
        /// <param name="switches">Switches to be applied, see: http://officeopenxml.com/WPtableOfContents.php </param>
        /// <param name="headerStyle">Lets you set the style name of the TOC header</param>
        /// <param name="maxIncludeLevel">Lets you specify how many header levels should be included - default is 1-3</param>
        /// <param name="rightTabPos">Lets you override the right tab position - this is not common</param>
        /// <returns>The inserted TableOfContents</returns>
        public TableOfContents InsertTableOfContents(string title, TableOfContentsSwitches switches, string headerStyle = null, int maxIncludeLevel = 3, int? rightTabPos = null)
        {
            var toc = TableOfContents.CreateTableOfContents(this, title, switches, headerStyle, maxIncludeLevel, rightTabPos);
            Xml.Add(toc.Xml);
            return toc;
        }

        /// <summary>
        /// Inserts at TOC into the current document before the provided <see cref="reference"/>
        /// </summary>
        /// <param name="reference">The paragraph to use as reference</param>
        /// <param name="title">The title of the TOC</param>
        /// <param name="switches">Switches to be applied, see: http://officeopenxml.com/WPtableOfContents.php </param>
        /// <param name="headerStyle">Lets you set the style name of the TOC header</param>
        /// <param name="maxIncludeLevel">Lets you specify how many header levels should be included - default is 1-3</param>
        /// <param name="rightTabPos">Lets you override the right tab position - this is not common</param>
        /// <returns>The inserted TableOfContents</returns>
        public TableOfContents InsertTableOfContents(Paragraph reference, string title, TableOfContentsSwitches switches, string headerStyle = null, int maxIncludeLevel = 3, int? rightTabPos = null)
        {
            var toc = TableOfContents.CreateTableOfContents(this, title, switches, headerStyle, maxIncludeLevel, rightTabPos);
            reference.Xml.AddBeforeSelf(toc.Xml);
            return toc;
        }

        internal static void PostCreation(Package package, DocumentTypes documentType = DocumentTypes.Document)
        {
            XDocument mainDoc, stylesDoc, numberingDoc;

            #region MainDocumentPart

            // Create the main document part for this package
            PackagePart mainDocumentPart;
            if (documentType == DocumentTypes.Document)
            {
                mainDocumentPart = package.CreatePart(new Uri("/word/document.xml", UriKind.Relative), "application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml");
            }
            else
            {
                mainDocumentPart = package.CreatePart(new Uri("/word/document.xml", UriKind.Relative), "application/vnd.openxmlformats-officedocument.wordprocessingml.template.main+xml");
            }
            package.CreateRelationship(mainDocumentPart.Uri, TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument");

            // Load the document part into a XDocument object
            using (TextReader tr = new StreamReader(mainDocumentPart.GetStream(FileMode.Create, FileAccess.ReadWrite)))
            {
                mainDoc = XDocument.Parse
                (@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
                   <w:document xmlns:ve=""http://schemas.openxmlformats.org/markup-compatibility/2006"" xmlns:o=""urn:schemas-microsoft-com:office:office"" xmlns:r=""http://schemas.openxmlformats.org/officeDocument/2006/relationships"" xmlns:m=""http://schemas.openxmlformats.org/officeDocument/2006/math"" xmlns:v=""urn:schemas-microsoft-com:vml"" xmlns:wp=""http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing"" xmlns:w10=""urn:schemas-microsoft-com:office:word"" xmlns:w=""http://schemas.openxmlformats.org/wordprocessingml/2006/main"" xmlns:wne=""http://schemas.microsoft.com/office/word/2006/wordml"" xmlns:a=""http://schemas.openxmlformats.org/drawingml/2006/main"" xmlns:c=""http://schemas.openxmlformats.org/drawingml/2006/chart"">
                   <w:body>
                    <w:sectPr w:rsidR=""003E25F4"" w:rsidSect=""00FC3028"">
                        <w:pgSz w:w=""11906"" w:h=""16838""/>
                        <w:pgMar w:top=""1440"" w:right=""1440"" w:bottom=""1440"" w:left=""1440"" w:header=""708"" w:footer=""708"" w:gutter=""0""/>
                        <w:cols w:space=""708""/>
                        <w:docGrid w:linePitch=""360""/>
                    </w:sectPr>
                   </w:body>
                   </w:document>"
                );
            }

            // Save the main document
            using (TextWriter tw = new StreamWriter(mainDocumentPart.GetStream(FileMode.Create, FileAccess.Write)))
                mainDoc.Save(tw, SaveOptions.None);

            #endregion MainDocumentPart

            #region StylePart

            stylesDoc = HelperFunctions.AddDefaultStylesXml(package);

            #endregion StylePart

            #region NumberingPart

            numberingDoc = HelperFunctions.AddDefaultNumberingXml(package);

            #endregion NumberingPart

            package.Close();
        }

        internal static DocX PostLoad(ref Package package)
        {
            DocX document = new DocX(null, null);
            document.package = package;
            document.Document = document;

            #region MainDocumentPart

            document.mainPart = package.GetParts().Where
            (
                     p => p.ContentType.Equals(HelperFunctions.DOCUMENT_DOCUMENTTYPE, StringComparison.CurrentCultureIgnoreCase) ||
                     p.ContentType.Equals(HelperFunctions.TEMPLATE_DOCUMENTTYPE, StringComparison.CurrentCultureIgnoreCase)
            ).Single();

            using (TextReader tr = new StreamReader(document.mainPart.GetStream(FileMode.Open, FileAccess.Read)))
                document.mainDoc = XDocument.Load(tr, LoadOptions.PreserveWhitespace);

            #endregion MainDocumentPart

            PopulateDocument(document, package);

            using (TextReader tr = new StreamReader(document.settingsPart.GetStream()))
                document.settings = XDocument.Load(tr);

            document.paragraphLookup.Clear();
            foreach (var paragraph in document.Paragraphs)
            {
                if (!document.paragraphLookup.ContainsKey(paragraph.endIndex))
                    document.paragraphLookup.Add(paragraph.endIndex, paragraph);
            }

            return document;
        }

        internal static void UpdateCorePropertyValue(DocX document, string corePropertyName, string corePropertyValue)
        {
            string matchPattern = string.Format(@"(DOCPROPERTY)?{0}\\\*MERGEFORMAT", corePropertyName).ToLower();
            foreach (XElement e in document.mainDoc.Descendants(XName.Get("fldSimple", w.NamespaceName)))
            {
                string attr_value = e.Attribute(XName.Get("instr", w.NamespaceName)).Value.Replace(" ", string.Empty).Trim().ToLower();

                if (Regex.IsMatch(attr_value, matchPattern))
                {
                    XElement firstRun = e.Element(w + "r");
                    XElement firstText = firstRun.Element(w + "t");
                    XElement rPr = firstText.Element(w + "rPr");

                    // Delete everything and insert updated text value
                    e.RemoveNodes();

                    XElement t = new XElement(w + "t", rPr, corePropertyValue);
                    Novacode.Text.PreserveSpace(t);
                    e.Add(new XElement(firstRun.Name, firstRun.Attributes(), firstRun.Element(XName.Get("rPr", w.NamespaceName)), t));
                }
            }

            #region Headers

            IEnumerable<PackagePart> headerParts = from headerPart in document.package.GetParts()
                                                   where (Regex.IsMatch(headerPart.Uri.ToString(), @"/word/header\d?.xml"))
                                                   select headerPart;
            foreach (PackagePart pp in headerParts)
            {
                XDocument header = XDocument.Load(new StreamReader(pp.GetStream()));

                foreach (XElement e in header.Descendants(XName.Get("fldSimple", w.NamespaceName)))
                {
                    string attr_value = e.Attribute(XName.Get("instr", w.NamespaceName)).Value.Replace(" ", string.Empty).Trim().ToLower();
                    if (Regex.IsMatch(attr_value, matchPattern))
                    {
                        XElement firstRun = e.Element(w + "r");

                        // Delete everything and insert updated text value
                        e.RemoveNodes();

                        XElement t = new XElement(w + "t", corePropertyValue);
                        Novacode.Text.PreserveSpace(t);
                        e.Add(new XElement(firstRun.Name, firstRun.Attributes(), firstRun.Element(XName.Get("rPr", w.NamespaceName)), t));
                    }
                }

                using (TextWriter tw = new StreamWriter(pp.GetStream(FileMode.Create, FileAccess.Write)))
                    header.Save(tw);
            }

            #endregion Headers

            #region Footers

            IEnumerable<PackagePart> footerParts = from footerPart in document.package.GetParts()
                                                   where (Regex.IsMatch(footerPart.Uri.ToString(), @"/word/footer\d?.xml"))
                                                   select footerPart;
            foreach (PackagePart pp in footerParts)
            {
                XDocument footer = XDocument.Load(new StreamReader(pp.GetStream()));

                foreach (XElement e in footer.Descendants(XName.Get("fldSimple", w.NamespaceName)))
                {
                    string attr_value = e.Attribute(XName.Get("instr", w.NamespaceName)).Value.Replace(" ", string.Empty).Trim().ToLower();
                    if (Regex.IsMatch(attr_value, matchPattern))
                    {
                        XElement firstRun = e.Element(w + "r");

                        // Delete everything and insert updated text value
                        e.RemoveNodes();

                        XElement t = new XElement(w + "t", corePropertyValue);
                        Novacode.Text.PreserveSpace(t);
                        e.Add(new XElement(firstRun.Name, firstRun.Attributes(), firstRun.Element(XName.Get("rPr", w.NamespaceName)), t));
                    }
                }

                using (TextWriter tw = new StreamWriter(pp.GetStream(FileMode.Create, FileAccess.Write)))
                    footer.Save(tw);
            }

            #endregion Footers

            PopulateDocument(document, document.package);
        }

        /// <summary>
        /// Update the custom properties inside the document
        /// </summary>
        /// <param name="document">The DocX document</param>
        /// <param name="customPropertyName">The property used inside the document</param>
        /// <param name="customPropertyValue">The new value for the property</param>
        /// <remarks>Different version of Word create different Document XML.</remarks>
        internal static void UpdateCustomPropertyValue(DocX document, string customPropertyName, string customPropertyValue)
        {
            // A list of documents, which will contain, The Main Document and if they exist: header1, header2, header3, footer1, footer2, footer3.
            List<XElement> documents = new List<XElement> { document.mainDoc.Root };

            // Check if each header exists and add if if so.

            #region Headers

            Headers headers = document.Headers;
            if (headers.first != null)
                documents.Add(headers.first.Xml);
            if (headers.odd != null)
                documents.Add(headers.odd.Xml);
            if (headers.even != null)
                documents.Add(headers.even.Xml);

            #endregion Headers

            // Check if each footer exists and add if if so.

            #region Footers

            Footers footers = document.Footers;
            if (footers.first != null)
                documents.Add(footers.first.Xml);
            if (footers.odd != null)
                documents.Add(footers.odd.Xml);
            if (footers.even != null)
                documents.Add(footers.even.Xml);

            #endregion Footers

            // Process each document in the list.
            foreach (XElement doc in documents)
            {
                #region Word 2010+

                foreach (XElement e in doc.Descendants(XName.Get("instrText", w.NamespaceName)))
                {
                    string attr_value = e.Value.Replace(" ", string.Empty).Trim();
                    string match_value = string.Format(@"DOCPROPERTY  {0}  \* MERGEFORMAT", customPropertyName).Replace(" ", string.Empty);

                    if (attr_value.Equals(match_value, StringComparison.CurrentCultureIgnoreCase))
                    {
                        XNode node = e.Parent.NextNode;
                        bool found = false;
                        while (true)
                        {
                            if (node.NodeType == XmlNodeType.Element)
                            {
                                var ele = node as XElement;
                                var match = ele.Descendants(XName.Get("t", w.NamespaceName));
                                if (match.Count() > 0)
                                {
                                    if (!found)
                                    {
                                        match.First().Value = customPropertyValue;
                                        found = true;
                                    }
                                    else
                                    {
                                        ele.RemoveNodes();
                                    }
                                }
                                else
                                {
                                    match = ele.Descendants(XName.Get("fldChar", w.NamespaceName));
                                    if (match.Count() > 0)
                                    {
                                        var endMatch = match.First().Attribute(XName.Get("fldCharType", w.NamespaceName));
                                        if (endMatch != null && endMatch.Value == "end")
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                            node = node.NextNode;
                        }
                    }
                }

                #endregion Word 2010+

                #region < Word 2010

                foreach (XElement e in doc.Descendants(XName.Get("fldSimple", w.NamespaceName)))
                {
                    string attr_value = e.Attribute(XName.Get("instr", w.NamespaceName)).Value.Replace(" ", string.Empty).Trim();
                    string match_value = string.Format(@"DOCPROPERTY  {0}  \* MERGEFORMAT", customPropertyName).Replace(" ", string.Empty);

                    if (attr_value.Equals(match_value, StringComparison.CurrentCultureIgnoreCase))
                    {
                        XElement firstRun = e.Element(w + "r");
                        XElement firstText = firstRun.Element(w + "t");
                        XElement rPr = firstText.Element(w + "rPr");

                        // Delete everything and insert updated text value
                        e.RemoveNodes();

                        XElement t = new XElement(w + "t", rPr, customPropertyValue);
                        Novacode.Text.PreserveSpace(t);
                        e.Add(new XElement(firstRun.Name, firstRun.Attributes(), firstRun.Element(XName.Get("rPr", w.NamespaceName)), t));
                    }
                }

                #endregion < Word 2010
            }
        }

        internal float getMarginAttribute(XName name)
        {
            XElement body = mainDoc.Root.Element(XName.Get("body", DocX.w.NamespaceName));
            XElement sectPr = body.Element(XName.Get("sectPr", DocX.w.NamespaceName));
            if (sectPr != null)
            {
                XElement pgMar = sectPr.Element(XName.Get("pgMar", DocX.w.NamespaceName));
                if (pgMar != null)
                {
                    XAttribute top = pgMar.Attribute(name);
                    if (top != null)
                    {
                        float f;
                        if (float.TryParse(top.Value, out f))
                            return (int)(f / 15.0f);
                    }
                }
            }

            return 0;
        }

        internal void setMarginAttribute(XName xName, float value)
        {
            XElement body = mainDoc.Root.Element(XName.Get("body", DocX.w.NamespaceName));
            XElement sectPr = body.Element(XName.Get("sectPr", DocX.w.NamespaceName));
            if (sectPr != null)
            {
                XElement pgMar = sectPr.Element(XName.Get("pgMar", DocX.w.NamespaceName));
                if (pgMar != null)
                {
                    XAttribute top = pgMar.Attribute(xName);
                    if (top != null)
                    {
                        top.SetValue(value * 15);
                    }
                }
            }
        }

        internal string GetCollectiveText(List<PackagePart> list)
        {
            string text = string.Empty;

            foreach (var hp in list)
            {
                using (TextReader tr = new StreamReader(hp.GetStream()))
                {
                    XDocument d = XDocument.Load(tr);

                    StringBuilder sb = new StringBuilder();

                    // Loop through each text item in this run
                    foreach (XElement descendant in d.Descendants())
                    {
                        switch (descendant.Name.LocalName)
                        {
                            case "tab":
                                sb.Append("\t");
                                break;

                            case "br":
                                sb.Append("\n");
                                break;

                            case "t":
                                goto case "delText";
                            case "delText":
                                sb.Append(descendant.Value);
                                break;

                            default: break;
                        }
                    }

                    text += "\n" + sb.ToString();
                }
            }

            return text;
        }

        internal XDocument AddStylesForList()
        {
            var wordStylesUri = new Uri("/word/styles.xml", UriKind.Relative);

            // If the internal document contains no /word/styles.xml create one.
            if (!package.PartExists(wordStylesUri))
                HelperFunctions.AddDefaultStylesXml(package);

            // Load the styles.xml into memory.
            XDocument wordStyles;
            using (TextReader tr = new StreamReader(package.GetPart(wordStylesUri).GetStream()))
                wordStyles = XDocument.Load(tr);

            bool listStyleExists =
            (
              from s in wordStyles.Element(w + "styles").Elements()
              let styleId = s.Attribute(XName.Get("styleId", w.NamespaceName))
              where (styleId != null && styleId.Value == "ListParagraph")
              select s
            ).Any();

            if (!listStyleExists)
            {
                var style = new XElement
                (
                    w + "style",
                    new XAttribute(w + "type", "paragraph"),
                    new XAttribute(w + "styleId", "ListParagraph"),
                        new XElement(w + "name", new XAttribute(w + "val", "List Paragraph")),
                        new XElement(w + "basedOn", new XAttribute(w + "val", "Normal")),
                        new XElement(w + "uiPriority", new XAttribute(w + "val", "34")),
                        new XElement(w + "qformat"),
                        new XElement(w + "rsid", new XAttribute(w + "val", "00832EE1")),
                        new XElement
                        (
                            w + "rPr",
                            new XElement(w + "ind", new XAttribute(w + "left", "720")),
                            new XElement
                            (
                                w + "contextualSpacing"
                            )
                        )
                );
                wordStyles.Element(w + "styles").Add(style);

                // Save the styles document.
                using (TextWriter tw = new StreamWriter(package.GetPart(wordStylesUri).GetStream()))
                    wordStyles.Save(tw);
            }

            return wordStyles;
        }

        internal void AddHyperlinkStyleIfNotPresent()
        {
            Uri word_styles_Uri = new Uri("/word/styles.xml", UriKind.Relative);

            // If the internal document contains no /word/styles.xml create one.
            if (!package.PartExists(word_styles_Uri))
                HelperFunctions.AddDefaultStylesXml(package);

            // Load the styles.xml into memory.
            XDocument word_styles;
            using (TextReader tr = new StreamReader(package.GetPart(word_styles_Uri).GetStream()))
                word_styles = XDocument.Load(tr);

            bool hyperlinkStyleExists =
            (
                from s in word_styles.Element(w + "styles").Elements()
                let styleId = s.Attribute(XName.Get("styleId", w.NamespaceName))
                where (styleId != null && styleId.Value == "Hyperlink")
                select s
            ).Count() > 0;

            if (!hyperlinkStyleExists)
            {
                XElement style = new XElement
                (
                    w + "style",
                    new XAttribute(w + "type", "character"),
                    new XAttribute(w + "styleId", "Hyperlink"),
                        new XElement(w + "name", new XAttribute(w + "val", "Hyperlink")),
                        new XElement(w + "basedOn", new XAttribute(w + "val", "DefaultParagraphFont")),
                        new XElement(w + "uiPriority", new XAttribute(w + "val", "99")),
                        new XElement(w + "unhideWhenUsed"),
                        new XElement(w + "rsid", new XAttribute(w + "val", "0005416C")),
                        new XElement
                        (
                            w + "rPr",
                            new XElement(w + "color", new XAttribute(w + "val", "0000FF"), new XAttribute(w + "themeColor", "hyperlink")),
                            new XElement
                            (
                                w + "u",
                                new XAttribute(w + "val", "single")
                            )
                        )
                );
                word_styles.Element(w + "styles").Add(style);

                // Save the styles document.
                using (TextWriter tw = new StreamWriter(package.GetPart(word_styles_Uri).GetStream()))
                    word_styles.Save(tw);
            }
        }

        /// <summary>
        /// Adds a Header to a document.
        /// If the document already contains a Header it will be replaced.
        /// </summary>
        /// <returns>The Header that was added to the document.</returns>
        internal void AddHeadersOrFooters(bool b)
        {
            string element = "ftr";
            string reference = "footer";
            if (b)
            {
                element = "hdr";
                reference = "header";
            }

            DeleteHeadersOrFooters(b);

            XElement sectPr = mainDoc.Root.Element(w + "body").Element(w + "sectPr");

            for (int i = 1; i < 4; i++)
            {
                string header_uri = string.Format("/word/{0}{1}.xml", reference, i);

                PackagePart headerPart = package.CreatePart(new Uri(header_uri, UriKind.Relative), string.Format("application/vnd.openxmlformats-officedocument.wordprocessingml.{0}+xml", reference));
                PackageRelationship headerRelationship = mainPart.CreateRelationship(headerPart.Uri, TargetMode.Internal, string.Format("http://schemas.openxmlformats.org/officeDocument/2006/relationships/{0}", reference));

                XDocument header;

                // Load the document part into a XDocument object
                using (TextReader tr = new StreamReader(headerPart.GetStream(FileMode.Create, FileAccess.ReadWrite)))
                {
                    header = XDocument.Parse
                    (string.Format(@"<?xml version=""1.0"" encoding=""utf-16"" standalone=""yes""?>
                       <w:{0} xmlns:ve=""http://schemas.openxmlformats.org/markup-compatibility/2006"" xmlns:o=""urn:schemas-microsoft-com:office:office"" xmlns:r=""http://schemas.openxmlformats.org/officeDocument/2006/relationships"" xmlns:m=""http://schemas.openxmlformats.org/officeDocument/2006/math"" xmlns:v=""urn:schemas-microsoft-com:vml"" xmlns:wp=""http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing"" xmlns:w10=""urn:schemas-microsoft-com:office:word"" xmlns:w=""http://schemas.openxmlformats.org/wordprocessingml/2006/main"" xmlns:wne=""http://schemas.microsoft.com/office/word/2006/wordml"">
                         <w:p w:rsidR=""009D472B"" w:rsidRDefault=""009D472B"">
                           <w:pPr>
                             <w:pStyle w:val=""{1}"" />
                           </w:pPr>
                         </w:p>
                       </w:{0}>", element, reference)
                    );
                }

                // Save the main document
                using (TextWriter tw = new StreamWriter(headerPart.GetStream(FileMode.Create, FileAccess.Write)))
                    header.Save(tw, SaveOptions.None);

                string type;
                switch (i)
                {
                    case 1: type = "default"; break;
                    case 2: type = "even"; break;
                    case 3: type = "first"; break;
                    default: throw new ArgumentOutOfRangeException();
                }

                sectPr.Add
                (
                    new XElement
                    (
                        w + string.Format("{0}Reference", reference),
                        new XAttribute(w + "type", type),
                        new XAttribute(r + "id", headerRelationship.Id)
                    )
                );
            }
        }

        internal void DeleteHeadersOrFooters(bool b)
        {
            string reference = "footer";
            if (b)
                reference = "header";

            // Get all header Relationships in this document.
            var header_relationships = mainPart.GetRelationshipsByType(string.Format("http://schemas.openxmlformats.org/officeDocument/2006/relationships/{0}", reference));

            foreach (PackageRelationship header_relationship in header_relationships)
            {
                // Get the TargetUri for this Part.
                Uri header_uri = header_relationship.TargetUri;

                // Check to see if the document actually contains the Part.
                if (!header_uri.OriginalString.StartsWith("/word/"))
                    header_uri = new Uri("/word/" + header_uri.OriginalString, UriKind.Relative);

                if (package.PartExists(header_uri))
                {
                    // Delete the Part
                    package.DeletePart(header_uri);

                    // Get all references to this Relationship in the document.
                    var query =
                    (
                        from e in mainDoc.Descendants(XName.Get("body", DocX.w.NamespaceName)).Descendants()
                        where (e.Name.LocalName == string.Format("{0}Reference", reference)) && (e.Attribute(r + "id").Value == header_relationship.Id)
                        select e
                    );

                    // Remove all references to this Relationship in the document.
                    for (int i = 0; i < query.Count(); i++)
                        query.ElementAt(i).Remove();

                    // Delete the Relationship.
                    package.DeleteRelationship(header_relationship.Id);
                }
            }
        }

        internal Image AddImage(object o, string contentType = "image/jpeg")
        {
            // Open a Stream to the new image being added.
            Stream newImageStream;
            if (o is string)
                newImageStream = new FileStream(o as string, FileMode.Open, FileAccess.Read);
            else
                newImageStream = o as Stream;

            // Get all image parts in word\document.xml

            PackageRelationshipCollection relationshipsByImages = mainPart.GetRelationshipsByType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/image");
            List<PackagePart> imageParts = relationshipsByImages.Select(ir => package.GetParts().FirstOrDefault(p => p.Uri.ToString().EndsWith(ir.TargetUri.ToString()))).Where(e => e != null).ToList();

            foreach (PackagePart relsPart in package.GetParts().Where(part => part.Uri.ToString().Contains("/word/")).Where(part => part.ContentType.Equals("application/vnd.openxmlformats-package.relationships+xml")))
            {
                XDocument relsPartContent;
                using (TextReader tr = new StreamReader(relsPart.GetStream(FileMode.Open, FileAccess.Read)))
                    relsPartContent = XDocument.Load(tr);

                IEnumerable<XElement> imageRelationships =
                relsPartContent.Root.Elements().Where
                (
                    imageRel =>
                    imageRel.Attribute(XName.Get("Type")).Value.Equals("http://schemas.openxmlformats.org/officeDocument/2006/relationships/image")
                );

                foreach (XElement imageRelationship in imageRelationships)
                {
                    if (imageRelationship.Attribute(XName.Get("Target")) != null)
                    {
                        string targetMode = string.Empty;

                        XAttribute targetModeAttibute = imageRelationship.Attribute(XName.Get("TargetMode"));
                        if (targetModeAttibute != null)
                        {
                            targetMode = targetModeAttibute.Value;
                        }

                        if (!targetMode.Equals("External"))
                        {
                            string imagePartUri = Path.Combine(Path.GetDirectoryName(relsPart.Uri.ToString()), imageRelationship.Attribute(XName.Get("Target")).Value);
                            imagePartUri = Path.GetFullPath(imagePartUri.Replace("\\_rels", string.Empty));
                            imagePartUri = imagePartUri.Replace(Path.GetFullPath("\\"), string.Empty).Replace("\\", "/");

                            if (!imagePartUri.StartsWith("/"))
                                imagePartUri = "/" + imagePartUri;

                            PackagePart imagePart = package.GetPart(new Uri(imagePartUri, UriKind.Relative));
                            imageParts.Add(imagePart);
                        }
                    }
                }
            }

            // Loop through each image part in this document.
            foreach (PackagePart pp in imageParts)
            {
                // Open a tempory Stream to this image part.
                using (Stream tempStream = pp.GetStream(FileMode.Open, FileAccess.Read))
                {
                    // Compare this image to the new image being added.
                    if (HelperFunctions.IsSameFile(tempStream, newImageStream))
                    {
                        // Get the image object for this image part
                        string id = mainPart.GetRelationshipsByType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/image")
                        .Where(r => r.TargetUri == pp.Uri)
                        .Select(r => r.Id).First();

                        // Return the Image object
                        return Images.Where(i => i.Id == id).First();
                    }
                }
            }

            string imgPartUriPath = string.Empty;
            string extension = contentType.Substring(contentType.LastIndexOf("/") + 1);
            do
            {
                // Create a new image part.
                imgPartUriPath = string.Format
                (
                    "/word/media/{0}.{1}",
                    Guid.NewGuid().ToString(), // The unique part.
                    extension
                );
            } while (package.PartExists(new Uri(imgPartUriPath, UriKind.Relative)));

            // We are now guareenteed that imgPartUriPath is unique.
            PackagePart img = package.CreatePart(new Uri(imgPartUriPath, UriKind.Relative), contentType);

            // Create a new image relationship
            PackageRelationship rel = mainPart.CreateRelationship(img.Uri, TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image");

            // Open a Stream to the newly created Image part.
            using (Stream stream = img.GetStream(FileMode.Create, FileAccess.Write))
            {
                // Using the Stream to the real image, copy this streams data into the newly create Image part.
                using (newImageStream)
                {
                    byte[] bytes = new byte[newImageStream.Length];
                    newImageStream.Read(bytes, 0, (int)newImageStream.Length);
                    stream.Write(bytes, 0, (int)newImageStream.Length);
                }// Close the Stream to the new image.
            }// Close the Stream to the new image part.

            return new Image(this, rel);
        }

        protected void clonePackageRelationship(DocX remote_document, PackagePart pp, XDocument remote_mainDoc)
        {
            string url = pp.Uri.OriginalString.Replace("/", "");
            var remote_rels = remote_document.mainPart.GetRelationships();
            foreach (var remote_rel in remote_rels)
            {
                if (url.Equals("word" + remote_rel.TargetUri.OriginalString.Replace("/", "")))
                {
                    String remote_Id = remote_rel.Id;
                    String local_Id = mainPart.CreateRelationship(remote_rel.TargetUri, remote_rel.TargetMode, remote_rel.RelationshipType).Id;

                    // Replace all instances of remote_Id in the local document with local_Id
                    var elems = remote_mainDoc.Descendants(XName.Get("blip", DocX.a.NamespaceName));
                    foreach (var elem in elems)
                    {
                        XAttribute embed = elem.Attribute(XName.Get("embed", DocX.r.NamespaceName));
                        if (embed != null && embed.Value == remote_Id)
                        {
                            embed.SetValue(local_Id);
                        }
                    }

                    // Replace all instances of remote_Id in the local document with local_Id (for shapes as well)
                    var v_elems = remote_mainDoc.Descendants(XName.Get("imagedata", DocX.v.NamespaceName));
                    foreach (var elem in v_elems)
                    {
                        XAttribute id = elem.Attribute(XName.Get("id", DocX.r.NamespaceName));
                        if (id != null && id.Value == remote_Id)
                        {
                            id.SetValue(local_Id);
                        }
                    }
                    break;
                }
            }
        }

        protected PackagePart clonePackagePart(PackagePart pp)
        {
            PackagePart new_pp = package.CreatePart(pp.Uri, pp.ContentType);

            using (Stream s_read = pp.GetStream())
            {
                using (Stream s_write = new_pp.GetStream(FileMode.Create))
                {
                    byte[] buffer = new byte[32768];
                    int read;
                    while ((read = s_read.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        s_write.Write(buffer, 0, read);
                    }
                }
            }

            return new_pp;
        }

        protected string GetMD5HashFromStream(Stream stream)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(stream);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }

        private static void PopulateDocument(DocX document, Package package)
        {
            Headers headers = new Headers();
            headers.odd = document.GetHeaderByType("default");
            headers.even = document.GetHeaderByType("even");
            headers.first = document.GetHeaderByType("first");

            Footers footers = new Footers();
            footers.odd = document.GetFooterByType("default");
            footers.even = document.GetFooterByType("even");
            footers.first = document.GetFooterByType("first");

            //// Get the sectPr for this document.
            //XElement sect = document.mainDoc.Descendants(XName.Get("sectPr", DocX.w.NamespaceName)).Single();

            //if (sectPr != null)
            //{
            //    // Extract the even header reference
            //    var header_even_ref = sectPr.Elements().SingleOrDefault(x => x.Name.LocalName == "headerReference" && x.Attribute(XName.Get("type", DocX.w.NamespaceName)) != null && x.Attribute(XName.Get("type", DocX.w.NamespaceName)).Value == "even");
            //    string id = header_even_ref.Attribute(XName.Get("id", DocX.r.NamespaceName)).Value;
            //    var res = document.mainPart.GetRelationship(id);
            //    string ans = res.SourceUri.OriginalString;
            //    headers.even.xml_filename = ans;

            //    // Extract the odd header reference
            //    var header_odd_ref = sectPr.Elements().SingleOrDefault(x => x.Name.LocalName == "headerReference" && x.Attribute(XName.Get("type", DocX.w.NamespaceName)) != null && x.Attribute(XName.Get("type", DocX.w.NamespaceName)).Value == "default");
            //    string id2 = header_odd_ref.Attribute(XName.Get("id", DocX.r.NamespaceName)).Value;
            //    var res2 = document.mainPart.GetRelationship(id2);
            //    string ans2 = res2.SourceUri.OriginalString;
            //    headers.odd.xml_filename = ans2;

            //    // Extract the first header reference
            //    var header_first_ref = sectPr.Elements().SingleOrDefault(x => x.Name.LocalName == "h
            //eaderReference" && x.Attribute(XName.Get("type", DocX.w.NamespaceName)) != null && x.Attribute(XName.Get("type", DocX.w.NamespaceName)).Value == "first");
            //    string id3 = header_first_ref.Attribute(XName.Get("id", DocX.r.NamespaceName)).Value;
            //    var res3 = document.mainPart.GetRelationship(id3);
            //    string ans3 = res3.SourceUri.OriginalString;
            //    headers.first.xml_filename = ans3;

            //    // Extract the even footer reference
            //    var footer_even_ref = sectPr.Elements().SingleOrDefault(x => x.Name.LocalName == "footerReference" && x.Attribute(XName.Get("type", DocX.w.NamespaceName)) != null && x.Attribute(XName.Get("type", DocX.w.NamespaceName)).Value == "even");
            //    string id4 = footer_even_ref.Attribute(XName.Get("id", DocX.r.NamespaceName)).Value;
            //    var res4 = document.mainPart.GetRelationship(id4);
            //    string ans4 = res4.SourceUri.OriginalString;
            //    footers.even.xml_filename = ans4;

            //    // Extract the odd footer reference
            //    var footer_odd_ref = sectPr.Elements().SingleOrDefault(x => x.Name.LocalName == "footerReference" && x.Attribute(XName.Get("type", DocX.w.NamespaceName)) != null && x.Attribute(XName.Get("type", DocX.w.NamespaceName)).Value == "default");
            //    string id5 = footer_odd_ref.Attribute(XName.Get("id", DocX.r.NamespaceName)).Value;
            //    var res5 = document.mainPart.GetRelationship(id5);
            //    string ans5 = res5.SourceUri.OriginalString;
            //    footers.odd.xml_filename = ans5;

            //    // Extract the first footer reference
            //    var footer_first_ref = sectPr.Elements().SingleOrDefault(x => x.Name.LocalName == "footerReference" && x.Attribute(XName.Get("type", DocX.w.NamespaceName)) != null && x.Attribute(XName.Get("type", DocX.w.NamespaceName)).Value == "first");
            //    string id6 = footer_first_ref.Attribute(XName.Get("id", DocX.r.NamespaceName)).Value;
            //    var res6 = document.mainPart.GetRelationship(id6);
            //    string ans6 = res6.SourceUri.OriginalString;
            //    footers.first.xml_filename = ans6;

            //}

            document.Xml = document.mainDoc.Root.Element(w + "body");
            document.headers = headers;
            document.footers = footers;
            document.settingsPart = HelperFunctions.CreateOrGetSettingsPart(package);

            var ps = package.GetParts();

            //document.endnotesPart = HelperFunctions.GetPart();

            foreach (var rel in document.mainPart.GetRelationships())
            {
                switch (rel.RelationshipType)
                {
                    case "http://schemas.openxmlformats.org/officeDocument/2006/relationships/endnotes":
                        document.endnotesPart = package.GetPart(new Uri("/word/" + rel.TargetUri.OriginalString.Replace("/word/", ""), UriKind.Relative));
                        using (TextReader tr = new StreamReader(document.endnotesPart.GetStream()))
                            document.endnotes = XDocument.Load(tr);
                        break;

                    case "http://schemas.openxmlformats.org/officeDocument/2006/relationships/footnotes":
                        document.footnotesPart = package.GetPart(new Uri("/word/" + rel.TargetUri.OriginalString.Replace("/word/", ""), UriKind.Relative));
                        using (TextReader tr = new StreamReader(document.footnotesPart.GetStream()))
                            document.footnotes = XDocument.Load(tr);
                        break;

                    case "http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles":
                        document.stylesPart = package.GetPart(new Uri("/word/" + rel.TargetUri.OriginalString.Replace("/word/", ""), UriKind.Relative));
                        using (TextReader tr = new StreamReader(document.stylesPart.GetStream()))
                            document.styles = XDocument.Load(tr);
                        break;

                    case "http://schemas.microsoft.com/office/2007/relationships/stylesWithEffects":
                        document.stylesWithEffectsPart = package.GetPart(new Uri("/word/" + rel.TargetUri.OriginalString.Replace("/word/", ""), UriKind.Relative));
                        using (TextReader tr = new StreamReader(document.stylesWithEffectsPart.GetStream()))
                            document.stylesWithEffects = XDocument.Load(tr);
                        break;

                    case "http://schemas.openxmlformats.org/officeDocument/2006/relationships/fontTable":
                        document.fontTablePart = package.GetPart(new Uri("/word/" + rel.TargetUri.OriginalString.Replace("/word/", ""), UriKind.Relative));
                        using (TextReader tr = new StreamReader(document.fontTablePart.GetStream()))
                            document.fontTable = XDocument.Load(tr);
                        break;

                    case "http://schemas.openxmlformats.org/officeDocument/2006/relationships/numbering":
                        document.numberingPart = package.GetPart(new Uri("/word/" + rel.TargetUri.OriginalString.Replace("/word/", ""), UriKind.Relative));
                        using (TextReader tr = new StreamReader(document.numberingPart.GetStream()))
                            document.numbering = XDocument.Load(tr);
                        break;

                    default:
                        break;
                }
            }
        }

        private Header GetHeaderByType(string type)
        {
            return (Header)GetHeaderOrFooterByType(type, true);
        }

        private Footer GetFooterByType(string type)
        {
            return (Footer)GetHeaderOrFooterByType(type, false);
        }

        private object GetHeaderOrFooterByType(string type, bool isHeader)
        {
            // Switch which handles either case Header\Footer, this just cuts down on code duplication.
            string reference = "footerReference";
            if (isHeader)
                reference = "headerReference";

            // Get the Id of the [default, even or first] [Header or Footer]
            string Id =
            (
                from e in mainDoc.Descendants(XName.Get("body", DocX.w.NamespaceName)).Descendants()
                where (e.Name.LocalName == reference) && (e.Attribute(w + "type").Value == type)
                select e.Attribute(r + "id").Value
            ).LastOrDefault();

            if (Id != null)
            {
                // Get the Xml file for this Header or Footer.
                Uri partUri = mainPart.GetRelationship(Id).TargetUri;

                // Weird problem with PackaePart API.
                if (!partUri.OriginalString.StartsWith("/word/"))
                    partUri = new Uri("/word/" + partUri.OriginalString, UriKind.Relative);

                // Get the Part and open a stream to get the Xml file.
                PackagePart part = package.GetPart(partUri);

                XDocument doc;
                using (TextReader tr = new StreamReader(part.GetStream()))
                {
                    doc = XDocument.Load(tr);

                    // Header and Footer extend Container.
                    Container c;
                    if (isHeader)
                        c = new Header(this, doc.Element(w + "hdr"), part);
                    else
                        c = new Footer(this, doc.Element(w + "ftr"), part);

                    return c;
                }
            }

            // If we got this far something went wrong.
            return null;
        }

        #region Internal variables defined foreach DocX object

        // Object representation of the .docx
        internal Package package;

        // The mainDocument is loaded into a XDocument object for easy querying and editing
        internal XDocument mainDoc;

        internal XDocument settings;
        internal XDocument endnotes;
        internal XDocument footnotes;
        internal XDocument styles;
        internal XDocument stylesWithEffects;
        internal XDocument numbering;
        internal XDocument fontTable;
        internal XDocument header1;
        internal XDocument header2;
        internal XDocument header3;

        // A lookup for the Paragraphs in this document.
        internal Dictionary<int, Paragraph> paragraphLookup = new Dictionary<int, Paragraph>();

        // Every document is stored in a MemoryStream, all edits made to a document are done in memory.
        internal MemoryStream memoryStream;

        // The filename that this document was loaded from
        internal string filename;

        // The stream that this document was loaded from
        internal Stream stream;

        #endregion Internal variables defined foreach DocX object

        private void merge_images(PackagePart remote_pp, DocX remote_document, XDocument remote_mainDoc, String contentType)
        {
            // Before doing any other work, check to see if this image is actually referenced in the document.
            // In my testing I have found cases of Images inside documents that are not referenced
            var remote_rel = remote_document.mainPart.GetRelationships().Where(r => r.TargetUri.OriginalString.Equals(remote_pp.Uri.OriginalString.Replace("/word/", ""))).FirstOrDefault();
            if (remote_rel == null)
            {
                remote_rel = remote_document.mainPart.GetRelationships().Where(r => r.TargetUri.OriginalString.Equals(remote_pp.Uri.OriginalString)).FirstOrDefault();
                if (remote_rel == null)
                    return;
            }
            String remote_Id = remote_rel.Id;

            String remote_hash = ComputeMD5HashString(remote_pp.GetStream());
            var image_parts = package.GetParts().Where(pp => pp.ContentType.Equals(contentType));

            bool found = false;
            foreach (var part in image_parts)
            {
                String local_hash = ComputeMD5HashString(part.GetStream());
                if (local_hash.Equals(remote_hash))
                {
                    // This image already exists in this document.
                    found = true;

                    var local_rel = mainPart.GetRelationships().Where(r => r.TargetUri.OriginalString.Equals(part.Uri.OriginalString.Replace("/word/", ""))).FirstOrDefault();
                    if (local_rel == null)
                    {
                        local_rel = mainPart.GetRelationships().Where(r => r.TargetUri.OriginalString.Equals(part.Uri.OriginalString)).FirstOrDefault();
                    }
                    if (local_rel != null)
                    {
                        String new_Id = local_rel.Id;

                        // Replace all instances of remote_Id in the local document with local_Id
                        var elems = remote_mainDoc.Descendants(XName.Get("blip", DocX.a.NamespaceName));
                        foreach (var elem in elems)
                        {
                            XAttribute embed = elem.Attribute(XName.Get("embed", DocX.r.NamespaceName));
                            if (embed != null && embed.Value == remote_Id)
                            {
                                embed.SetValue(new_Id);
                            }
                        }

                        // Replace all instances of remote_Id in the local document with local_Id (for shapes as well)
                        var v_elems = remote_mainDoc.Descendants(XName.Get("imagedata", DocX.v.NamespaceName));
                        foreach (var elem in v_elems)
                        {
                            XAttribute id = elem.Attribute(XName.Get("id", DocX.r.NamespaceName));
                            if (id != null && id.Value == remote_Id)
                            {
                                id.SetValue(new_Id);
                            }
                        }
                    }

                    break;
                }
            }

            // This image does not exist in this document.
            if (!found)
            {
                String new_uri = remote_pp.Uri.OriginalString;
                new_uri = new_uri.Remove(new_uri.LastIndexOf("/"));
                //new_uri = new_uri.Replace("word/", "");
                new_uri += "/" + Guid.NewGuid().ToString() + contentType.Replace("image/", ".");
                if (!new_uri.StartsWith("/"))
                    new_uri = "/" + new_uri;

                PackagePart new_pp = package.CreatePart(new Uri(new_uri, UriKind.Relative), remote_pp.ContentType);

                using (Stream s_read = remote_pp.GetStream())
                {
                    using (Stream s_write = new_pp.GetStream(FileMode.Create))
                    {
                        byte[] buffer = new byte[32768];
                        int read;
                        while ((read = s_read.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            s_write.Write(buffer, 0, read);
                        }
                    }
                }

                PackageRelationship pr = mainPart.CreateRelationship(new Uri(new_uri, UriKind.Relative), TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image");

                String new_Id = pr.Id;

                //Check if the remote relationship id is a default rId from Word
                Match defRelId = Regex.Match(remote_Id, @"rId\d+", RegexOptions.IgnoreCase);

                // Replace all instances of remote_Id in the local document with local_Id
                var elems = remote_mainDoc.Descendants(XName.Get("blip", DocX.a.NamespaceName));
                foreach (var elem in elems)
                {
                    XAttribute embed = elem.Attribute(XName.Get("embed", DocX.r.NamespaceName));
                    if (embed != null && embed.Value == remote_Id)
                    {
                        embed.SetValue(new_Id);
                    }
                }

                if (!defRelId.Success)
                {
                    // Replace all instances of remote_Id in the local document with local_Id
                    var elems_local = mainDoc.Descendants(XName.Get("blip", DocX.a.NamespaceName));
                    foreach (var elem in elems_local)
                    {
                        XAttribute embed = elem.Attribute(XName.Get("embed", DocX.r.NamespaceName));
                        if (embed != null && embed.Value == remote_Id)
                        {
                            embed.SetValue(new_Id);
                        }
                    }

                    // Replace all instances of remote_Id in the local document with local_Id
                    var v_elems_local = mainDoc.Descendants(XName.Get("imagedata", DocX.v.NamespaceName));
                    foreach (var elem in v_elems_local)
                    {
                        XAttribute id = elem.Attribute(XName.Get("id", DocX.r.NamespaceName));
                        if (id != null && id.Value == remote_Id)
                        {
                            id.SetValue(new_Id);
                        }
                    }
                }

                // Replace all instances of remote_Id in the local document with local_Id (for shapes as well)
                var v_elems = remote_mainDoc.Descendants(XName.Get("imagedata", DocX.v.NamespaceName));
                foreach (var elem in v_elems)
                {
                    XAttribute id = elem.Attribute(XName.Get("id", DocX.r.NamespaceName));
                    if (id != null && id.Value == remote_Id)
                    {
                        id.SetValue(new_Id);
                    }
                }
            }
        }

        private string ComputeMD5HashString(Stream stream)
        {
            //MD5 md5 = MD5.Create();
            byte[] hash = SHA1.Create().ComputeHash(stream);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
                sb.Append(hash[i].ToString("X2"));
            return sb.ToString();
        }

        private void merge_endnotes(PackagePart remote_pp, PackagePart local_pp, XDocument remote_mainDoc, DocX remote, XDocument remote_endnotes)
        {
            IEnumerable<int> ids =
            (
                from d in endnotes.Root.Descendants()
                where d.Name.LocalName == "endnote"
                select int.Parse(d.Attribute(XName.Get("id", DocX.w.NamespaceName)).Value)
            );

            int max_id = ids.Max() + 1;
            var endnoteReferences = remote_mainDoc.Descendants(XName.Get("endnoteReference", DocX.w.NamespaceName));

            foreach (var endnote in remote_endnotes.Root.Elements().OrderBy(fr => fr.Attribute(XName.Get("id", DocX.r.NamespaceName))).Reverse())
            {
                XAttribute id = endnote.Attribute(XName.Get("id", DocX.w.NamespaceName));
                int i;
                if (id != null && int.TryParse(id.Value, out i))
                {
                    if (i > 0)
                    {
                        foreach (var endnoteRef in endnoteReferences)
                        {
                            XAttribute a = endnoteRef.Attribute(XName.Get("id", DocX.w.NamespaceName));
                            if (a != null && int.Parse(a.Value).Equals(i))
                            {
                                a.SetValue(max_id);
                            }
                        }

                        // We care about copying this footnote.
                        endnote.SetAttributeValue(XName.Get("id", DocX.w.NamespaceName), max_id);
                        endnotes.Root.Add(endnote);
                        max_id++;
                    }
                }
            }
        }

        private void merge_footnotes(PackagePart remote_pp, PackagePart local_pp, XDocument remote_mainDoc, DocX remote, XDocument remote_footnotes)
        {
            IEnumerable<int> ids =
            (
                from d in footnotes.Root.Descendants()
                where d.Name.LocalName == "footnote"
                select int.Parse(d.Attribute(XName.Get("id", DocX.w.NamespaceName)).Value)
            );

            int max_id = ids.Max() + 1;
            var footnoteReferences = remote_mainDoc.Descendants(XName.Get("footnoteReference", DocX.w.NamespaceName));

            foreach (var footnote in remote_footnotes.Root.Elements().OrderBy(fr => fr.Attribute(XName.Get("id", DocX.r.NamespaceName))).Reverse())
            {
                XAttribute id = footnote.Attribute(XName.Get("id", DocX.w.NamespaceName));
                int i;
                if (id != null && int.TryParse(id.Value, out i))
                {
                    if (i > 0)
                    {
                        foreach (var footnoteRef in footnoteReferences)
                        {
                            XAttribute a = footnoteRef.Attribute(XName.Get("id", DocX.w.NamespaceName));
                            if (a != null && int.Parse(a.Value).Equals(i))
                            {
                                a.SetValue(max_id);
                            }
                        }

                        // We care about copying this footnote.
                        footnote.SetAttributeValue(XName.Get("id", DocX.w.NamespaceName), max_id);
                        footnotes.Root.Add(footnote);
                        max_id++;
                    }
                }
            }
        }

        private string merge_headers(PackagePart remote_pp, List<PackagePart> relsParts, XDocument remote_mainDoc, String id)
        {
            // Get the remote documents custom.xml file.
            XDocument remote_header_document;
            using (TextReader tr = new StreamReader(remote_pp.GetStream()))
                remote_header_document = XDocument.Load(tr);

            //string oldRelationshipId = remote_document.mainPart.GetRelationships().Where(x => x.TargetUri == remote_pp.Uri || x.TargetUri.ToString() == filename).Select(x => x.Id).FirstOrDefault();

            //Compare the header content with existing headers
            string type = remote_mainDoc.Descendants(XName.Get("headerReference", DocX.w.NamespaceName)).Where(x => x.Attribute(XName.Get("id", DocX.r.NamespaceName)).Value == id).Select(x => x.Attribute(XName.Get("type", DocX.w.NamespaceName)).Value).FirstOrDefault();

            if (headers.odd != null && type == "odd" && headers.odd.Xml.ToString() == remote_header_document.ToString())
            {
                string filename = Path.GetFileName(headers.odd.mainPart.Uri.ToString().Replace("/", "\\"));
                return mainPart.GetRelationships().First(x => x.TargetUri == headers.odd.mainPart.Uri || x.TargetUri.ToString() == filename).Id;
            }
            else if (headers.first != null && type == "first" && headers.first.Xml.ToString() == remote_header_document.ToString())
            {
                string filename = Path.GetFileName(headers.first.mainPart.Uri.ToString().Replace("/", "\\"));
                return mainPart.GetRelationships().First(x => x.TargetUri == headers.first.mainPart.Uri || x.TargetUri.ToString() == filename).Id;
            }
            else if (headers.even != null && type == "even" && headers.even.Xml.ToString() == remote_header_document.ToString())
            {
                string filename = Path.GetFileName(headers.even.mainPart.Uri.ToString().Replace("/", "\\"));
                return mainPart.GetRelationships().First(x => x.TargetUri == headers.even.mainPart.Uri || x.TargetUri.ToString() == filename).Id;
            }
            else if (headers.headers.Count() > 0)
            {
                foreach (Header header in headers.headers)
                {
                    if (header.type == type && header.Xml.ToString() == remote_header_document.ToString())
                    {
                        string filename = Path.GetFileName(header.mainPart.Uri.ToString().Replace("/", "\\"));
                        return mainPart.GetRelationships().First(x => x.TargetUri == header.mainPart.Uri || x.TargetUri.ToString() == filename).Id;
                    }
                }
            }

            //The header is new to document
            string maxId = String.Format("rId{0}", mainPart.GetRelationships().Where(x => x.Id.Substring(0, 3) == "rId").Max(x => Convert.ToInt32(x.Id.Replace("rId", ""))) + 1);
            string header_uri = string.Format("/word/header{0}.xml", Headers.Count() + 1);

            PackagePart headerPart = package.CreatePart(new Uri(header_uri, UriKind.Relative), string.Format("application/vnd.openxmlformats-officedocument.wordprocessingml.header+xml"));
            PackageRelationship headerRelationship = mainPart.CreateRelationship(headerPart.Uri, TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/header", maxId);

            foreach (PackagePart rels in relsParts)
            {
                XDocument remote_rels_document;

                using (TextReader tr = new StreamReader(rels.GetStream()))
                    remote_rels_document = XDocument.Load(tr);

                string rels_uri = string.Format("/word/_rels/header{0}.xml.rels", Headers.Count() + 1);
                PackagePart relsPart = package.CreatePart(new Uri(rels_uri, UriKind.Relative), string.Format("application/vnd.openxmlformats-package.relationships+xml"));

                // Save the modified local custom styles.xml file.
                using (TextWriter tw = new StreamWriter(relsPart.GetStream(FileMode.Create, FileAccess.Write)))
                    remote_rels_document.Save(tw, SaveOptions.None);
            }

            // Save the modified local custom styles.xml file.
            using (TextWriter tw = new StreamWriter(headerPart.GetStream(FileMode.Create, FileAccess.Write)))
                remote_header_document.Save(tw, SaveOptions.None);

            Header newHeader = new Header(this, remote_header_document.Element(w + "hdr"), headerPart);
            newHeader.type = type;
            Headers.headers.Add(newHeader);

            return maxId;
        }

        private void merge_customs(PackagePart remote_pp, PackagePart local_pp, XDocument remote_mainDoc)
        {
            // Get the remote documents custom.xml file.
            XDocument remote_custom_document;
            using (TextReader tr = new StreamReader(remote_pp.GetStream()))
                remote_custom_document = XDocument.Load(tr);

            // Get the local documents custom.xml file.
            XDocument local_custom_document;
            using (TextReader tr = new StreamReader(local_pp.GetStream()))
                local_custom_document = XDocument.Load(tr);

            IEnumerable<int> pids =
            (
                from d in remote_custom_document.Root.Descendants()
                where d.Name.LocalName == "property"
                select int.Parse(d.Attribute(XName.Get("pid")).Value)
            );

            int pid = pids.Max() + 1;

            foreach (XElement remote_property in remote_custom_document.Root.Elements())
            {
                bool found = false;
                foreach (XElement local_property in local_custom_document.Root.Elements())
                {
                    XAttribute remote_property_name = remote_property.Attribute(XName.Get("name"));
                    XAttribute local_property_name = local_property.Attribute(XName.Get("name"));

                    if (remote_property != null && local_property_name != null && remote_property_name.Value.Equals(local_property_name.Value))
                        found = true;
                }

                if (!found)
                {
                    remote_property.SetAttributeValue(XName.Get("pid"), pid);
                    local_custom_document.Root.Add(remote_property);

                    pid++;
                }
            }

            // Save the modified local custom styles.xml file.
            using (TextWriter tw = new StreamWriter(local_pp.GetStream(FileMode.Create, FileAccess.Write)))
                local_custom_document.Save(tw, SaveOptions.None);
        }

        private void merge_numbering(PackagePart remote_pp, PackagePart local_pp, XDocument remote_mainDoc, DocX remote)
        {
            // Add each remote numbering to this document.
            IEnumerable<XElement> remote_abstractNums = remote.numbering.Root.Elements(XName.Get("abstractNum", DocX.w.NamespaceName));
            int guidd = 0;
            foreach (var an in remote_abstractNums)
            {
                XAttribute a = an.Attribute(XName.Get("abstractNumId", DocX.w.NamespaceName));
                if (a != null)
                {
                    int i;
                    if (int.TryParse(a.Value, out i))
                    {
                        if (i > guidd)
                            guidd = i;
                    }
                }
            }
            guidd++;

            IEnumerable<XElement> remote_nums = remote.numbering.Root.Elements(XName.Get("num", DocX.w.NamespaceName));
            int guidd2 = 0;
            foreach (var an in remote_nums)
            {
                XAttribute a = an.Attribute(XName.Get("numId", DocX.w.NamespaceName));
                if (a != null)
                {
                    int i;
                    if (int.TryParse(a.Value, out i))
                    {
                        if (i > guidd2)
                            guidd2 = i;
                    }
                }
            }
            guidd2++;

            foreach (XElement remote_abstractNum in remote_abstractNums)
            {
                XAttribute abstractNumId = remote_abstractNum.Attribute(XName.Get("abstractNumId", DocX.w.NamespaceName));
                if (abstractNumId != null)
                {
                    String abstractNumIdValue = abstractNumId.Value;
                    abstractNumId.SetValue(guidd);

                    foreach (XElement remote_num in remote_nums)
                    {
                        var numIds = remote_mainDoc.Descendants(XName.Get("numId", DocX.w.NamespaceName));
                        foreach (var numId in numIds)
                        {
                            XAttribute attr = numId.Attribute(XName.Get("val", DocX.w.NamespaceName));
                            if (attr != null && attr.Value.Equals(remote_num.Attribute(XName.Get("numId", DocX.w.NamespaceName)).Value))
                            {
                                attr.SetValue(guidd2);
                            }
                        }
                        remote_num.SetAttributeValue(XName.Get("numId", DocX.w.NamespaceName), guidd2);

                        XElement e = remote_num.Element(XName.Get("abstractNumId", DocX.w.NamespaceName));
                        if (e != null)
                        {
                            XAttribute a2 = e.Attribute(XName.Get("val", DocX.w.NamespaceName));
                            if (a2 != null && a2.Value.Equals(abstractNumIdValue))
                                a2.SetValue(guidd);
                        }

                        guidd2++;
                    }
                }

                guidd++;
            }

            // Checking whether there were more than 0 elements, helped me get rid of exceptions thrown while using InsertDocument
            if (numbering.Root.Elements(XName.Get("abstractNum", DocX.w.NamespaceName)).Count() > 0)
                numbering.Root.Elements(XName.Get("abstractNum", DocX.w.NamespaceName)).Last().AddAfterSelf(remote_abstractNums);

            if (numbering.Root.Elements(XName.Get("num", DocX.w.NamespaceName)).Count() > 0)
                numbering.Root.Elements(XName.Get("num", DocX.w.NamespaceName)).Last().AddAfterSelf(remote_nums);
        }

        private void merge_fonts(PackagePart remote_pp, PackagePart local_pp, XDocument remote_mainDoc, DocX remote)
        {
            // Add each remote font to this document.
            IEnumerable<XElement> remote_fonts = remote.fontTable.Root.Elements(XName.Get("font", DocX.w.NamespaceName));
            IEnumerable<XElement> local_fonts = fontTable.Root.Elements(XName.Get("font", DocX.w.NamespaceName));

            foreach (XElement remote_font in remote_fonts)
            {
                bool flag_addFont = true;
                foreach (XElement local_font in local_fonts)
                {
                    if (local_font.Attribute(XName.Get("name", DocX.w.NamespaceName)).Value == remote_font.Attribute(XName.Get("name", DocX.w.NamespaceName)).Value)
                    {
                        flag_addFont = false;
                        break;
                    }
                }

                if (flag_addFont)
                {
                    fontTable.Root.Add(remote_font);
                }
            }
        }

        private void merge_styles(PackagePart remote_pp, PackagePart local_pp, XDocument remote_mainDoc, DocX remote, XDocument remote_footnotes, XDocument remote_endnotes)
        {
            Dictionary<String, String> local_styles = new Dictionary<string, string>();
            foreach (XElement local_style in styles.Root.Elements(XName.Get("style", DocX.w.NamespaceName)))
            {
                XElement temp = new XElement(local_style);
                XAttribute styleId = temp.Attribute(XName.Get("styleId", DocX.w.NamespaceName));
                String value = styleId.Value;
                styleId.Remove();
                String key = Regex.Replace(temp.ToString(), @"\s+", "");
                if (!local_styles.ContainsKey(key)) local_styles.Add(key, value);
            }

            // Add each remote style to this document.
            IEnumerable<XElement> remote_styles = remote.styles.Root.Elements(XName.Get("style", DocX.w.NamespaceName));
            foreach (XElement remote_style in remote_styles)
            {
                XElement temp = new XElement(remote_style);
                XAttribute styleId = temp.Attribute(XName.Get("styleId", DocX.w.NamespaceName));
                String value = styleId.Value;
                styleId.Remove();
                String key = Regex.Replace(temp.ToString(), @"\s+", "");
                String guuid;

                // Check to see if the local document already contains the remote style.
                if (local_styles.ContainsKey(key))
                {
                    String local_value;
                    local_styles.TryGetValue(key, out local_value);

                    // If the styleIds are the same then nothing needs to be done.
                    if (local_value == value)
                        continue;

                    // All we need to do is update the styleId.
                    else
                    {
                        guuid = local_value;
                    }
                }
                else
                {
                    guuid = Guid.NewGuid().ToString();
                    // Set the styleId in the remote_style to this new Guid
                    // [Fixed the issue that my document referred to a new Guid while my styles still had the old value ("Titel")]
                    remote_style.SetAttributeValue(XName.Get("styleId", DocX.w.NamespaceName), guuid);
                }

                foreach (XElement e in remote_mainDoc.Root.Descendants(XName.Get("pStyle", DocX.w.NamespaceName)))
                {
                    XAttribute e_styleId = e.Attribute(XName.Get("val", DocX.w.NamespaceName));
                    if (e_styleId != null && e_styleId.Value.Equals(styleId.Value))
                    {
                        e_styleId.SetValue(guuid);
                    }
                }

                foreach (XElement e in remote_mainDoc.Root.Descendants(XName.Get("rStyle", DocX.w.NamespaceName)))
                {
                    XAttribute e_styleId = e.Attribute(XName.Get("val", DocX.w.NamespaceName));
                    if (e_styleId != null && e_styleId.Value.Equals(styleId.Value))
                    {
                        e_styleId.SetValue(guuid);
                    }
                }

                foreach (XElement e in remote_mainDoc.Root.Descendants(XName.Get("tblStyle", DocX.w.NamespaceName)))
                {
                    XAttribute e_styleId = e.Attribute(XName.Get("val", DocX.w.NamespaceName));
                    if (e_styleId != null && e_styleId.Value.Equals(styleId.Value))
                    {
                        e_styleId.SetValue(guuid);
                    }
                }

                if (remote_endnotes != null)
                {
                    foreach (XElement e in remote_endnotes.Root.Descendants(XName.Get("rStyle", DocX.w.NamespaceName)))
                    {
                        XAttribute e_styleId = e.Attribute(XName.Get("val", DocX.w.NamespaceName));
                        if (e_styleId != null && e_styleId.Value.Equals(styleId.Value))
                        {
                            e_styleId.SetValue(guuid);
                        }
                    }

                    foreach (XElement e in remote_endnotes.Root.Descendants(XName.Get("pStyle", DocX.w.NamespaceName)))
                    {
                        XAttribute e_styleId = e.Attribute(XName.Get("val", DocX.w.NamespaceName));
                        if (e_styleId != null && e_styleId.Value.Equals(styleId.Value))
                        {
                            e_styleId.SetValue(guuid);
                        }
                    }
                }

                if (remote_footnotes != null)
                {
                    foreach (XElement e in remote_footnotes.Root.Descendants(XName.Get("rStyle", DocX.w.NamespaceName)))
                    {
                        XAttribute e_styleId = e.Attribute(XName.Get("val", DocX.w.NamespaceName));
                        if (e_styleId != null && e_styleId.Value.Equals(styleId.Value))
                        {
                            e_styleId.SetValue(guuid);
                        }
                    }

                    foreach (XElement e in remote_footnotes.Root.Descendants(XName.Get("pStyle", DocX.w.NamespaceName)))
                    {
                        XAttribute e_styleId = e.Attribute(XName.Get("val", DocX.w.NamespaceName));
                        if (e_styleId != null && e_styleId.Value.Equals(styleId.Value))
                        {
                            e_styleId.SetValue(guuid);
                        }
                    }
                }

                // Make sure they don't clash by using a uuid.
                styleId.SetValue(guuid);
                styles.Root.Add(remote_style);
            }
        }

        private string GetNextFreeRelationshipID()
        {
            int id = (
                 from r in mainPart.GetRelationships()
                 where r.Id.Substring(0, 3).Equals("rId")
                 select int.Parse(r.Id.Substring(3))
             ).DefaultIfEmpty().Max();

            // The conventiom for ids is rid01, rid02, etc
            string newId = id.ToString();
            int result;
            if (int.TryParse(newId, out result))
                return ("rId" + (result + 1));
            else
            {
                String guid = String.Empty;
                do
                {
                    guid = Guid.NewGuid().ToString();
                } while (Char.IsDigit(guid[0]));
                return guid;
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Releases all resources used by this document.
        /// </summary>
        /// <example>
        /// If you take advantage of the using keyword, Dispose() is automatically called for you.
        /// <code>
        /// // Load document.
        /// using (DocX document = DocX.Load(@"C:\Example\Test.docx"))
        /// {
        ///      // The document is only in memory while in this scope.
        ///
        /// }// Dispose() is automatically called at this point.
        /// </code>
        /// </example>
        /// <example>
        /// This example is equilivant to the one above example.
        /// <code>
        /// // Load document.
        /// DocX document = DocX.Load(@"C:\Example\Test.docx");
        ///
        /// // Do something with the document here.
        ///
        /// // Dispose of the document.
        /// document.Dispose();
        /// </code>
        /// </example>
        public void Dispose()
        {
            package.Close();
        }

        #endregion IDisposable Members
    }
}