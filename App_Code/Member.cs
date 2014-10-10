// ----------------------------------------------------------------------------------------
// Assembly version:          
// Create Date:               2/23/2012 12:12
// Solution Name:             CompassForeignDataProvider
// Project Item Name:         Member
// <copyright file="Member.cs" company="Northwoods">
//      Copyright © 2011, Northwoods Consulting Partners, Inc., All Rights Reserved     
// </copyright>
// Notes:							 
// ----------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

[Serializable]
public class Member
{
    public Collection<Keyword> Keywords { get; set; }
    
    public Member()
    {
        Keywords = new Collection<Keyword>();
    }

    public Member(Collection<Keyword> keywords)
    {
        Keywords = keywords;
    }
}