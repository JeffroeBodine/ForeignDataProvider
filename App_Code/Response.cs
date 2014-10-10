// ----------------------------------------------------------------------------------------
// Assembly version:          
// Create Date:               2/23/2012 12:13
// Solution Name:             CompassForeignDataProvider
// Project Item Name:         Response
// <copyright file="Response.cs" company="Northwoods">
//      Copyright © 2011, Northwoods Consulting Partners, Inc., All Rights Reserved     
// </copyright>
// Notes:							 
// ----------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

[Serializable()]
public class Response
{
    public ResponseHeader Header { get; set; }
    public Collection<Member> Members { get; set; }
        
    public Response()
    {
        Header = new ResponseHeader();
        Members = new Collection<Member>();
    }

    public Response(ResponseHeader header, Collection<Member> members)
    {
        Header = header;
        Members = members;
    }
}