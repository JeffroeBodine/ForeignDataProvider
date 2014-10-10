// ----------------------------------------------------------------------------------------
// Assembly version:          
// Create Date:               2/23/2012 12:13
// Solution Name:             CompassForeignDataProvider
// Project Item Name:         ResponseHeader
// <copyright file="ResponseHeader.cs" company="Northwoods">
//      Copyright © 2011, Northwoods Consulting Partners, Inc., All Rights Reserved     
// </copyright>
// Notes:							 
// ----------------------------------------------------------------------------------------

using System;
using System.Xml.Serialization;

[Serializable]
public class ResponseHeader
{
    [XmlAttribute()]
    public string Code { get; set; }
    [XmlAttribute()]
    public string Value { get; set; }

	public ResponseHeader()
	{
        Code = string.Empty;
        Value = string.Empty;
    }

    public ResponseHeader(string code, string value)
    {
        Code = code;
        Value = value;
    }
}