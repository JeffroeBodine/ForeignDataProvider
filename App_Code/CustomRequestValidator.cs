// ----------------------------------------------------------------------------------------
// Assembly version:          
// Create Date:               2/23/2012 15:06
// Solution Name:             CompassForeignDataProvider
// Project Item Name:         CustomRequestValidator
// <copyright file="CustomRequestValidator.cs" company="Northwoods">
//      Copyright © 2011, Northwoods Consulting Partners, Inc., All Rights Reserved     
// </copyright>
// Notes: This is required in .NET 4.0 to allow for XML input through a string variable.							 
// ----------------------------------------------------------------------------------------

using System.Web;
using System.Web.Util;
public class CustomRequestValidator : RequestValidator
{
    protected override bool IsValidRequestString(HttpContext context, string value, RequestValidationSource requestValidationSource, string collectionKey, out int validationFailureIndex)
    {
        validationFailureIndex = -1;

        return true;
  }
}
