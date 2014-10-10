// ----------------------------------------------------------------------------------------
// Assembly version:          
// Create Date:               2/23/2012 16:14
// Solution Name:             CompassForeignDataProvider
// Project Item Name:         MySerializer
// <copyright file="MySerializer.cs" company="Northwoods">
//      Copyright © 2011, Northwoods Consulting Partners, Inc., All Rights Reserved     
// </copyright>
// Notes:							 
// ----------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

public static class MySerializer
{
    public static object FromXml(string xml, System.Type objType)
    {
        object oRet = null;
        XmlSerializer ser = new XmlSerializer(objType);

        using (StringReader stringReader = new StringReader(xml))
        {
            XmlTextReader xmlReader = new XmlTextReader(stringReader);
            oRet = ser.Deserialize(xmlReader); 
        }
        	       
        return oRet;
    }

    public static string ToXml(object obj)
    {
        string sRet = string.Empty;

        if (obj != null)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            MemoryStream memStream = null;

            try
            {
                memStream = new MemoryStream();

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                settings.NewLineOnAttributes = false;
                settings.Indent = true;

                XmlWriter writer = XmlWriter.Create(memStream, settings);

                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add(string.Empty, string.Empty);

                serializer.Serialize(writer, obj, namespaces);

                sRet = Encoding.UTF8.GetString(memStream.GetBuffer());
                sRet = sRet.Substring(sRet.IndexOf(Convert.ToChar(60)));
                sRet = sRet.Substring(0, (sRet.LastIndexOf(Convert.ToChar(62)) + 1));
            }
            finally
            {
                if (memStream != null)
                {
                    memStream.Dispose();
                }
            }
        }
        
        return sRet;
    }
}