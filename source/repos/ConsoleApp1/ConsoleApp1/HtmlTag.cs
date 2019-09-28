// MP1: HTML Validator
// You should not modify this file!
// You should read the code here to understand how the HtmlTag class is implemented.

using System;
using System.Collections.Generic;
using System.Text;

namespace HtmlValidator
{
    public class HtmlTag
    {
        private readonly string Element;
        private readonly bool IsOpenTag;

        /// <summary>
        /// Constructs an HTML "opening" tag with the given element
        /// (e.g. "table").
        /// effects: creates a new HtmlTag object for this tag. 
        /// </summary>
        /// <param name="element">element that represents an opening
        /// HTML tag such as &lt;p&gt; or &lt;table&gt;
        /// </param>
        /// <exception cref="NullReferenceException">Throws if element is null
        /// </exception>
        public HtmlTag(string element) : this(element, true) {  }

        /// <summary>
        /// Constructs an HTML tag with the given element (e.g. 
        /// "table") and type.
        /// Self-closing tags like &lt;br /&gt; are considered 
        /// to be "opening" tags, and return true from the 
        /// isOpenTag method.
        /// effects: creates an HtmlTag object for the provided tag
        /// </summary>
        /// <param name="element">element represents an HTML tag 
        /// such as &lt;p&gt; or &lt;/p&gt;
        /// </param>
        /// <param name="isOpenTag">isOpenTag indicates if the 
        /// tag in an opening tag (true) or closing tag(false)
        /// </param>
        /// <exception cref="NullReferenceException">Throws if element is null
        /// </exception>
        public HtmlTag(string element, bool isOpenTag)
        {
            this.Element = element.ToLower();
            this.IsOpenTag = isOpenTag;
        }

        /// <summary>
        /// Clone this HtmlTag and obtain a reference to a new
        /// HtmlTag object. This method is useful if one wants
        /// to make deep copies of HtmlTags. Use with
        /// care because cloning imposes performance and memory
        /// overheads.
        /// </summary>
        /// <returns>a reference to an HtmlTag object that is a 
        /// clone of this object.
        /// </returns>
        public HtmlTag Clone()
        {
            return new HtmlTag(string.Copy(Element), IsOpenTag);
        }

        /// <summary>
        /// Returns true if this tag has the same element and 
        /// type as the given other tag. 
        /// </summary>
        /// <param name="o">o is the object to compare this HtmlTag 
        /// to</param>
        /// <returns>true if this HtmlTag is equal to o and false
        /// otherwise</returns>
        public bool IsEqual(Object o)
        {
            if (o is HtmlTag other)
            {
                return Element.Equals(other.Element) 
                    && IsOpenTag == other.IsOpenTag;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns this HTML tag's element, such as "table" or "p". 
        /// </summary>
        /// <returns>a string representation of the tag represented
        /// by this HtmlTag without the angle brackets
        /// </returns>
        public string GetElement()
        {
            return Element;
        }

        /// <summary>
        /// Returns true if this HTML tag is an "opening" (starting)
        /// tag and false if it is a closing tag.
        /// Self-closing tags like &lt;br /&gt; are considered to 
        /// be "opening" tags.
        /// </summary>
        /// <returns>true if this HtmlTag is an opening tag or a 
        /// self-closing tag, and false otherwise
        /// </returns>
        public bool GetIsOpenTag()
        {
            return IsOpenTag;
        }

        /// <summary>
        /// A match occurs when one tage is an opening tag and 
        /// the other is a closing tag.
        /// Returns true if the given other tag is non-null 
        /// and matches this tag; that is, if they have the 
        /// same element but opposite types, such as &lt;body&gt; 
        /// and &lt;/body&gt;.
        /// </summary>
        /// <param name="other">other is an HtmlTag to compare 
        /// this HtmlTag with
        /// </param>
        /// <returns>true if this HtmlTag "matches" the other 
        /// HtmlTag and false otherwise.
        /// </returns>
        public bool Matches(HtmlTag other)
        {
            return other != null && string.Equals(Element, other.Element,
                StringComparison.OrdinalIgnoreCase) && IsOpenTag != other.IsOpenTag;
        }

        /// <summary>
        /// Returns true if this tag does not requires a matching 
        /// closing tag, which is the case for certain elements 
        /// such as br and img.
        /// </summary>
        /// <returns>true if this HtmlTag is self-closing(e.g. 
        /// &lt;br /&gt;) and false otherwise
        /// </returns>
        public bool IsSelfClosing()
        {
            return SELF_CLOSING_TAGS.Contains(Element);
        }

        /// <summary>
        /// Returns a string representation of this HTML tag, 
        /// such as "&lt;/table&gt;".
        /// </summary>
        /// <returns>a string representation of this HtmlTag
        /// including the angle brackets
        /// </returns>
        public override string ToString()
        {
            return "<" + (IsOpenTag ? "" : "/")
                    + (Element.Equals("!--") ? "!-- --" : Element) + ">";
        }

        // a set of tags that don't need to be matched (self-closing)
        private static readonly HashSet<string> SELF_CLOSING_TAGS = new HashSet<string>(
                new[]{"!doctype", "!--", "?xml", "xml", "area", "base",
                      "basefont", "br", "col", "frame", "hr", "img",
                      "input", "link", "meta", "param" });

        // all whitespace characters; used in text parsing
        private static readonly string WHITESPACE = " \f\n\r\t";

        /// <summary>
        /// Reads a string such as "&lt;table&gt;" or "&lt;/p&gt;"
        /// and converts it into an HtmlTag, which is returned.
        /// </summary>
        /// <param name="tagText">tagText is a string that 
        /// represents an HtmlTag including angle brackets
        /// </param>
        /// <returns>an HtmlTag that represents the tag given
        /// as a String
        /// </returns>
        /// <exception cref="NullReferenceException">Throws if tagText is null
        /// </exception>
        public static HtmlTag Parse(string tagText)
        {
            tagText = tagText.Trim();
            bool isOpenTag = !tagText.Contains("</");
            string element = tagText.Replace("[^a-zA-Z!-?]+", "");
            if (element.Contains("!--"))
            {
                element = "!--";  // HTML comments
            }
            return new HtmlTag(element, isOpenTag);
        }

        // You don't need to call this method in your MP code
        /// <summary>
        /// Takes a string and converts it into a list of HTML 
        /// tokens represented using a list of HtmlTag objects.
        /// The input string represents the HTML text.
        /// </summary>
        /// <param name="text">text is the input HTML, and 
        /// text != null
        /// </param>
        /// <returns>a list of HtmlTag objects obtained 
        /// from the input HTML
        /// </returns>
        public static LinkedList<HtmlTag> Tokenize(string text)
        {
            StringBuilder buf = new StringBuilder(text);
            LinkedList<HtmlTag> queue = new LinkedList<HtmlTag>();

            while (true)
            {
                HtmlTag nextTag = NextTag(buf);
                if (nextTag == null)
                {
                    break;
                }
                else
                {
                    queue.AddLast(nextTag);
                }
            }

            return queue;
        }

        /// <summary>
        /// This method grabs the next HTML tag from a string 
        /// buffer and returns an HtmlTag object for the tag found
        /// in the buffer
        /// It advances to next tag in input; probably not a perfect
        /// HTML tag tokenizer, but it will do for this MP
        /// </summary>
        /// <param name="buf">buf represents HTML text that needs 
        /// to be processed; buf is modified in this method when 
        /// a tag is found and the tag and associated text are 
        /// removed from buf.
        /// This effectively advances the processing point.</param>
        /// <returns>an HtmlTag object for the next HTML tag in
        /// buf</returns>
        private static HtmlTag NextTag(StringBuilder buf)
        {
            int index1 = buf.ToString().IndexOf("<");
            int index2 = buf.ToString().IndexOf(">");

            if (index1 >= 0 && index2 > index1)
            {
                // check for HTML comments: <!-- -->
                if (index1 + 4 <= buf.Length && buf.ToString().Substring(index1 + 1, 3).Equals("!--"))
                {
                    // a comment; look for closing comment tag -->
                    index2 = buf.ToString().IndexOf("-->", index1 + 4);
                    if (index2 < 0)
                    {
                        return null;
                    }
                    else
                    {
                        buf.Insert(index1 + 4, " ");    // fixes things like <!--hi-->
                        index2 += 3;    // advance to the closing >
                    }
                }

                string element = buf.ToString().Substring(index1 + 1, index2 - index1 - 1).Trim();

                // remove attributes
                for (int i = 0; i < WHITESPACE.Length; i++)
                {
                    int index3 = element.IndexOf(WHITESPACE[i]);
                    if (index3 >= 0)
                    {
                        element = element.Substring(0, index3);
                    }
                }

                // determine whether opening or closing tag
                bool isOpenTag = true;
                if (element.IndexOf("/") == 0)
                {
                    isOpenTag = false;
                    element = element.Substring(1);
                }
                element = element.Replace("[^a-zA-Z0-9!-]+", "");

                buf.Remove(0, index2 + 1);
                return new HtmlTag(element, isOpenTag);
            }
            else
            {
                return null;
            }
        }
    }
}
