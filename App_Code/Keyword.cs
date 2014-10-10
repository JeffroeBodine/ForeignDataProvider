// ----------------------------------------------------------------------------------------
// Assembly version:          
// Create Date:               2/23/2012 16:15
// Solution Name:             CompassForeignDataProvider
// Project Item Name:         Keyword
// <copyright file="Keyword.cs" company="Northwoods">
//      Copyright © 2011, Northwoods Consulting Partners, Inc., All Rights Reserved     
// </copyright>
// Notes:							 
// ----------------------------------------------------------------------------------------

using System;
using System.Xml.Serialization;

[Serializable]
public class Keyword 
{
    [XmlAttribute()]
    public string Name { get; set; }

    [XmlAttribute()]
    public string Value { get; set; }

	public Keyword()
	{
    }
        
    public Keyword(string name, string value)
    {
        Name = name;
        Value = value;
    }
}