// MP1: HTML Validator
// Instructor-provided code. You should not modify this file!
// This program tests your HTML validator object on any file or URL you want.
//
// When it prompts you for a file name, if you type a simple string such
// as "test1.html" (without the quotes) it will just look on your hard disk
// in the same directory as the executable file.
//
// If you type a string such as "http://www.google.ca", it will
// connect to that URL and download the HTML content from it.

// You should mainly test with simpler html files as few actual 
// URL would work well with our limited implementation.


using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace HtmlValidator
{
    public class ValidatorMain
    {
        public static void Main()
        {
            HtmlValidator validator = new HtmlValidator();
            String pageText = "";
            String choice = "s";

            while (true)
            {
                if (choice.StartsWith("s"))
                {
                    // prompt for page, then download it if it's a URL
                    Console.Write("File name or page URL (blank for empty): ");
                    string url = Console.ReadLine().Trim();
                    if (url.Length > 0)
                    {
                        if (IsURL(url))
                        {
                            Console.WriteLine("Downloading from " + url + " ...");
                        }

                        try
                        {
                            pageText = ReadCompleteFileOrURL(url);
                            Queue<HtmlTag> tags = new Queue<HtmlTag>(HtmlTag.Tokenize(pageText));

                            // create the HTML validator
                            validator = new HtmlValidator(tags);
                        }
                        catch (UriFormatException) 
                        {
                            Console.WriteLine("Badly formatted URL: " + url);
                        }
                        catch (FileNotFoundException) 
                        {
                            Console.WriteLine("Web page or file not found: " + url);
                        }
                        catch (IOException ioe)
                        {
                            Console.WriteLine("I/O error: " + ioe.Message);
                        }
                    }
                    else
                    {
                        pageText = "No page text (starting from empty queue)";
                        validator = new HtmlValidator();
                    }
                }
                else if (choice.StartsWith("a"))
                {
                    Console.Write("What tag (such as <table> or </p>)? ");
                    string tagText = Console.ReadLine().Trim();
                    bool isOpenTag = !tagText.Contains("</");
                    string element = tagText.Replace("[^a-zA-Z!-]+", "");
                    if (element.Contains("!--"))
                    {
                        element = "!--";  // HTML comments
                    }
                    HtmlTag tag = new HtmlTag(element, isOpenTag);
                    validator.AddTag(tag);
                }
                else if (choice.StartsWith("g"))
                {
                    Console.WriteLine(validator.GetTags());
                }
                else if (choice.StartsWith("p"))
                {
                    Console.WriteLine(pageText);
                }
                else if (choice.StartsWith("r"))
                {
                    Console.Write("Remove what element? ");
                    String element = Console.ReadLine().Trim();
                    validator.Remove(element);
                }
                else if (choice.StartsWith("v"))
                {
                    validator.Validate();
                    Console.WriteLine();
                }
                else if (choice.StartsWith("q"))
                {
                    break;
                }

                Console.WriteLine();
                Console.Write("(a)ddTag, (g)etTags, (r)emoveAll, (v)alidate, (s)et URL, (p)rint, (q)uit? ");
                choice = Console.ReadLine().Trim().ToLower();
            }
        }

        /// <summary>
        ///  Returns an input stream to read from the given address.
        ///  Works with URLs or normal file names.
        /// </summary>
        public static StreamReader GetInputStream(string address)  
        {
            if (IsURL(address))
            {   
                return new StreamReader(WebRequest.Create(address).GetResponse().GetResponseStream());
            }
            else
            {
                // local file
                return new StreamReader(address);
            }
        }

        /// <summary>
        /// Returns true if the given string represents a URL.
        /// </summary>
        public static bool IsURL(string address)
        {
            return address.StartsWith("http://") || address.StartsWith("https://") ||
                    address.StartsWith("www.") ||
                    address.EndsWith("/") ||
                    address.EndsWith(".com") || address.Contains(".com/") ||
                    address.EndsWith(".org") || address.Contains(".org/") ||
                    address.EndsWith(".edu") || address.Contains(".edu/") ||
                    address.EndsWith(".ca") || address.Contains(".ca/") ||
                    address.EndsWith(".gov") || address.Contains(".gov/");
        }

       /// <summary>
       /// Opens the given address for reading input, and reads it until the end 
       /// of the file, and returns the entire file contents as a big String.
       /// If address starts with http[s]://, assumes address is a URL and tries
       /// to download the data from the web.  Otherwise, assumes the address
       /// is a local file and tries to read it from the disk.
     /// </summary>
        public static string ReadCompleteFileOrURL(String address)  
        {
            StreamReader stream = GetInputStream(address);   // open file

            // read each letter into a buffer
            StringBuilder buffer = new StringBuilder();
            while (true)
            {
                int ch = stream.Read();
                if (ch < 0)
                {
                    break;
                }

                buffer.Append((char)ch);
            }

            return buffer.ToString();
        }
    }

}
