// ----------------------------------------------------------------------------------------
// Assembly version:          
// Create Date:               2/23/2012 12:13
// Solution Name:             CompassForeignDataProvider
// Project Item Name:         Request
// <copyright file="Request.cs" company="Northwoods">
//      Copyright © 2011, Northwoods Consulting Partners, Inc., All Rights Reserved     
// </copyright>
// Notes:							 
// ----------------------------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;

[Serializable()]
public class Request
{
    public string UserName { get; set; }
    public string System { get; set; }
    public Collection<Keyword> Keywords { get; set; }

	public Request()
	{
        UserName = string.Empty;
        System = string.Empty;
        Keywords = new Collection<Keyword>();
    }

    public Request(string userName, string system, Collection<Keyword> keywords)
    {
        UserName = userName;
        System = system;
        Keywords = keywords;
    }
}