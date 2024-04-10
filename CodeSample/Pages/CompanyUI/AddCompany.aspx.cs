using BusinessLayer.Services.CommanService;
using BusinessLayer.Services.CompanyService;
using BussinessModels.Models.CompanyModel;
using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.UI;

namespace SampleCode.Pages.CompanyUI
{
    [System.Web.Script.Services.ScriptService]
    public partial class AddCompany : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Redirect to login page if not authenticated
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("/Pages/AuthUI/Login.aspx");
            else
            {
                // Add breadcrumbs for navigation
                AddBreadcrumbs("You are here:", new[]
                {
                    ("Contacts", "CompanyList.aspx"),
                    ("Companies", "CompanyList.aspx"),
                    ("New Company", "AddCompany.aspx")
                });
            }
        }

        /// <summary>
        /// Adds breadcrumbs to the page.
        /// </summary>
        /// <param name="prefix">The prefix text to be displayed before the breadcrumbs.</param>
        /// <param name="items">A collection of tuples containing the text and URL for each breadcrumb item.</param>
        private void AddBreadcrumbs(string prefix, IEnumerable<(string Text, string Url)> items)
        {
            var breadcrumbTrail = new List<string>();

            foreach (var (text, url) in items)
            {
                breadcrumbTrail.Add($"<a href=\"{url}\">{text}</a>");
            }
            breadcrumbs.Controls.Add(new LiteralControl($"{prefix} {string.Join(" / ", breadcrumbTrail)}"));
        }

        // <summary>
        /// Web method to retrieve autocomplete data for registered users' emails based on the provided search text.
        /// </summary>
        /// <param name="searchText">The text used for searching registered users' emails.</param>
        [WebMethod]
        public static List<RegisteredUsersDetailsDTO> GetAutoCompleteData(string searchText)
        {
            CommanBL commonService = new CommanBL();
            return commonService.GetRegisteredUsersEmails(searchText);
        }

        /// <summary>
        /// Web method to retrieve the list of tags.
        /// </summary>
        /// <returns>A list of TagsDTO objects representing the tags data.</returns>
        [WebMethod]
        public static List<TagsDTO> GetTagList()
        {
            CommanBL commonService = new CommanBL();
            return commonService.GetTagsData();
        }

        /// <summary>
        /// Web method to retrieve the list of countries.
        /// </summary>
        /// <returns>A list of CountryMasterDTO objects representing the countries data.</returns>
        [WebMethod]
        public static List<CountryMasterDTO> GetCountryList()
        {
            CommanBL commonService = new CommanBL();
            return commonService.GetCountryList();
        }

        /// <summary>
        /// Web method to retrieve payment terms.
        /// </summary>
        /// <returns>A list of PaymentTermsDTO objects representing the payment terms data.</returns>
        [WebMethod]
        public static List<PaymentTermsDTO> GetPaymentTerms()
        {
            CompanyBL companyService = new CompanyBL();
            return companyService.GetPaymentTerms();
        }

        /// <summary>
        /// Web method to save company data.
        /// </summary>
        /// <param name="data">The company data to be saved.</param>
        /// <returns>A CommonResponse object indicating the result of the operation.</returns>
        [WebMethod]
        public static CommonResponse SaveCompanyData(CompanyModel data)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                CompanyBL companyService = new CompanyBL();
                response = companyService.AddEditCompany(data);
            }
            catch (Exception)
            {
                // Handle exceptions
                response.message = "Error";
                response.Id = 0;
            }
            return response;
        }

        /// <summary>
        /// Web method to check if a customer code exists.
        /// </summary>
        /// <param name="customerCode">The customer code to check for existence.</param>
        /// <returns>True if the customer code exists, otherwise false.</returns>
        [WebMethod]
        public static bool IsCustomerCodeExist(string customerCode)
        {
            CompanyBL companyService = new CompanyBL();
            return companyService.IsCustomerCodeExist(customerCode);
        }

        /// <summary>
        /// Redirects the user to the company list page.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/CompanyUI/CompanyList.aspx");
        }

        /// <summary>
        /// Web method to retrieve company members associated with a specific branch.
        /// </summary>
        /// <param name="branchId">The ID of the branch to retrieve company members for.</param>
        /// <returns>A list of CompanyMembersDto objects representing the company members data.</returns>
        [WebMethod]
        public static List<CompanyMembersDto> GetCompanyMembers(int branchId)
        {
            CompanyBL companyService = new CompanyBL();
            return companyService.GetCompanyMembers(branchId);
        }

        /// <summary>
        /// Web method to delete a person.
        /// </summary>
        /// <param name="personId">The ID of the person to delete.</param>
        /// <returns>True if the person is successfully deleted, otherwise false.</returns>
        [WebMethod]
        public static bool DeletePerson(int personId)
        {
            CompanyBL companyService = new CompanyBL();
            return companyService.DeletePerson(personId);
        }
    }
}
