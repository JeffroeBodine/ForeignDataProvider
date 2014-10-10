// ----------------------------------------------------------------------------------------
// Assembly version:          
// Create Date:               2/23/2012 12:13
// Solution Name:             CompassForeignDataProvider
// Project Item Name:         Service
// <copyright file="Service.cs" company="Northwoods">
//      Copyright © 2011, Northwoods Consulting Partners, Inc., All Rights Reserved     
// </copyright>
// Notes:							 
// ----------------------------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Security.Principal;
using System.Threading;
using System.Web.Services;
using DataInterface;

[WebService(Namespace = "http://www.teamnorthwoods.com/ForeignDataProvider/", Description = "This web service is to be used as a bridge between Pilot and the database to get autofill information", Name = "Foreign Data Provider")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class Service : System.Web.Services.WebService
{
    // ----------------------------------------------------------------------------------------
    // Procedure Name:                GetSampleResponse
    // Procedure Kind Description:    Function
    // Function Return Type FullName: System.String
    // Purpose: To return sample data in the structure that an actual search would return. Used for administering the Autofill.
    // Parameters:
    // ----------------------------------------------------------------------------------------
    [WebMethod]
    public string GetSampleResponse()
    {
        string un = this.User.Identity.Name;

        Response r = new Response();
        r.Header = new ResponseHeader("1", "Records Return Successfully");

        Member m1 = new Member();
        r.Members.Add(m1);

        m1.Keywords.Add(new Keyword("FirstName", "Johnny"));
        m1.Keywords.Add(new Keyword("MiddleName", "Q"));
        m1.Keywords.Add(new Keyword("LastName", "Northwoods"));
        m1.Keywords.Add(new Keyword("NameSuffix", "Jr."));

        m1.Keywords.Add(new Keyword("SSN", "111111111"));
        m1.Keywords.Add(new Keyword("ExternalSystemID", "MN786545746"));
        m1.Keywords.Add(new Keyword("Sex", "M"));
        m1.Keywords.Add(new Keyword("BirthDate", "01/27/1963"));

        m1.Keywords.Add(new Keyword("PhysicalStreet1", "123 High St"));
        m1.Keywords.Add(new Keyword("PhysicalStreet2", "Apt 4"));
        m1.Keywords.Add(new Keyword("PhysicalStreet3", string.Empty));
        m1.Keywords.Add(new Keyword("PhysicalCity", "Dublin"));
        m1.Keywords.Add(new Keyword("PhysicalState", "OH"));
        m1.Keywords.Add(new Keyword("PhysicalZip", "43017"));
        m1.Keywords.Add(new Keyword("PhysicalZipPlus4", "1234"));

        m1.Keywords.Add(new Keyword("MailingStreet1", "456 Broad Rd."));
        m1.Keywords.Add(new Keyword("MailingStreet2", string.Empty));
        m1.Keywords.Add(new Keyword("MailingStreet3", string.Empty));
        m1.Keywords.Add(new Keyword("MailingCity", "Columbus"));
        m1.Keywords.Add(new Keyword("MailingState", "HO"));
        m1.Keywords.Add(new Keyword("MailingZip", "43016"));
        m1.Keywords.Add(new Keyword("MailingZipPlus4", "5678"));

        m1.Keywords.Add(new Keyword("HomePhone", "0123456"));
        m1.Keywords.Add(new Keyword("CellPhone", "9876543"));

        m1.Keywords.Add(new Keyword("StateCaseNumber", "020478922"));
        m1.Keywords.Add(new Keyword("LocalCaseNumber", "037450943"));
        m1.Keywords.Add(new Keyword("CaseManagerUserName", "Northwoods"));
        m1.Keywords.Add(new Keyword("CaseManagerName", "North Woods"));

        m1.Keywords.Add(new Keyword("CompassNumber", "OH1230000000001"));
        m1.Keywords.Add(new Keyword("UserName", "CompassServiceUser"));

        m1.Keywords.Add(new Keyword("xxxBatch #", "01234567890123456789"));
        m1.Keywords.Add(new Keyword("xxxCheck Consecutive #", "012345678"));

        return MySerializer.ToXml(r);
    }

    // ----------------------------------------------------------------------------------------
    // Procedure Name:                Search
    // Procedure Kind Description:    Function
    // Function Return Type FullName: System.String
    // Purpose: To purpose of this method is to get Autofill data based on search criteria.
    // Parameters: string 
    // ----------------------------------------------------------------------------------------
    [WebMethod]
    public string Search(string sRequest)
    {
        Response resp = null;
        Request request = (Request)MySerializer.FromXml(sRequest, typeof(Request));

        if (request != null)
        {
            string requestUserName = request.UserName;

            string firstName = string.Empty;
            string lastName = string.Empty;
            string sSN = string.Empty;
            DateTime? birthDate = null;
            string sISNumber = string.Empty;
            string stateIssuedNumber = string.Empty;
            string nineChar = string.Empty;
            string twentyChar = string.Empty;

            foreach (Keyword kw in request.Keywords)
            {
                if (kw.Name.Equals(Constants.FIRSTNAME, StringComparison.CurrentCultureIgnoreCase))
                {
                    firstName = kw.Value;
                }
                else if (kw.Name.Equals(Constants.LASTNAME, StringComparison.CurrentCultureIgnoreCase))
                {
                    lastName = kw.Value;
                }
                else if (kw.Name.Equals(Constants.SSN, StringComparison.CurrentCultureIgnoreCase))
                {
                    sSN = kw.Value.Replace("-", string.Empty);
                 }
                else if (kw.Name.Equals(Constants.BIRTHDATE, StringComparison.CurrentCultureIgnoreCase))
                {
                    birthDate = DateTime.Parse(kw.Value, CultureInfo.InvariantCulture);
                }
                else if (kw.Name.Equals(Constants.SISNUMBER, StringComparison.CurrentCultureIgnoreCase))
                {
                    sISNumber = kw.Value;
                }
                else if (kw.Name.Equals(Constants.STATEISSUEDNUMBER, StringComparison.CurrentCultureIgnoreCase))
                {
                    stateIssuedNumber = kw.Value;
                }
                else if (kw.Name.Equals(Constants.NINECHAR, StringComparison.CurrentCultureIgnoreCase))
                {
                    nineChar = kw.Value;
                }
                else if (kw.Name.Equals(Constants.TWENTYCHAR, StringComparison.CurrentCultureIgnoreCase))
                {
                    twentyChar = kw.Value;
                }
            }

            DataSet dsResult = MakeSQLCall(firstName, lastName, sSN, birthDate, sISNumber, stateIssuedNumber, requestUserName, nineChar, twentyChar);

            resp = new Response();
            int rowCount = 0;

            if (dsResult != null && dsResult.Tables.Count > 0)
            {
                rowCount = dsResult.Tables[0].Rows.Count;

                foreach (DataRow dr in dsResult.Tables[0].Rows)
                {
                    resp.Members.Add(GetMemberFromDataRow(dr));
                }
            }

            resp.Header = new ResponseHeader(rowCount.ToString(CultureInfo.InvariantCulture), "Records Return Successfully");
        }

        return MySerializer.ToXml(resp);
    }

    // ----------------------------------------------------------------------------------------
    // Procedure Name:                IsFeatureSupported
    // Procedure Kind Description:    Function
    // Function Return Type FullName: System.String
    // Purpose: This function allows the client to interrogate the service for the availability of a specific feature. Currently only needed for paging.
    // Parameters: string 
    // ----------------------------------------------------------------------------------------
    [WebMethod]
    public bool IsFeatureSupported(string feature)
    {
        bool bRet;

        switch (feature)
        {
            case Constants.SUPPORTEDFEATUREPAGING:
                bRet = false;
                break;
            default:
                bRet = false;
                break;
        }

        return bRet;
    }

    // ----------------------------------------------------------------------------------------
    // Procedure Name:                GetSearchableFields
    // Procedure Kind Description:    Function
    // Function Return Type FullName: Collection<System.String>
    // Purpose: Placeholder used for future development.
    // Parameters: 
    // ----------------------------------------------------------------------------------------
    [WebMethod]
    public Collection<string> GetSearchableFields()
    {
        return new Collection<string>();
    }
    
    #region Private Methods
    // This method is used to mimic the SMI Web Service call and return a list of results.
    private DataSet MakeSQLCall(string firstName, string lastName, string sSN, DateTime? birthDate, string sISNumber, string stateIssuedNumber, string wsUsername, string nineChar, string twentyChar)
    {
        SqlStoredProc spGetClientInfo = new SqlStoredProc("ud.GetPeopleAutofillRecords");
        bool bExecuteProc = false;

        if (!String.IsNullOrEmpty(firstName))
        {
            spGetClientInfo.AddInputParameter("FirstName", firstName);
            bExecuteProc = true;
        }

        if (!String.IsNullOrEmpty(lastName))
        {
            spGetClientInfo.AddInputParameter("LastName", lastName);
            bExecuteProc = true;
        }

        if (!String.IsNullOrEmpty(sSN))
        {
            spGetClientInfo.AddInputParameter("SSN", sSN);
            bExecuteProc = true;
        }

        if (birthDate != null)
        {
            spGetClientInfo.AddInputParameter("BirthDate", birthDate);
            bExecuteProc = true;
        }

        if (!String.IsNullOrEmpty(sISNumber))
        {
            spGetClientInfo.AddInputParameter("SISNumber", sISNumber);
            bExecuteProc = true;
        }

        if (!String.IsNullOrEmpty(stateIssuedNumber))
        {
            spGetClientInfo.AddInputParameter("StateIssuedNumber", stateIssuedNumber);
            bExecuteProc = true;
        }

        if (!String.IsNullOrEmpty(nineChar))
        {
            spGetClientInfo.AddInputParameter("NineChar", nineChar);
            bExecuteProc = true;
        }

        if (!String.IsNullOrEmpty(twentyChar))
        {
            spGetClientInfo.AddInputParameter("TwentyChar", twentyChar);
            bExecuteProc = true;
        }

        spGetClientInfo.AddInputParameter("WSUserName", wsUsername);

        DataSet dsRet = null;

        if (bExecuteProc)
        {
            dsRet = spGetClientInfo.GetDsResult();
        }

        return dsRet;
    }

    // This method is used to convert a result to an actual member object.
    private Member GetMemberFromDataRow(DataRow dr)
    {
        Member mRet = new Member();
        try
        {
            foreach (DataColumn dc in dr.Table.Columns)
            {
                mRet.Keywords.Add(new Keyword(dc.ColumnName, dr[dc.ColumnName].ToString()));
            }
        }
        catch (Exception)
        {
            throw;
        }

        return mRet;
    }

    #endregion
}