using BusinessLayer.Services.CompanyService;
using BussinessModels.Models.CompanyModel;
using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.UI;

namespace SampleCode.Pages.CompanyUI
{
    public partial class EditCompany : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Retrieve the company ID from the query string
            string idParameter = Request.QueryString["id"];

            // Check if the user is not authenticated, then redirect to the login page
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("/Pages/AuthUI/Login.aspx");
            }
            else
            {
                // User is authenticated, add breadcrumbs for navigation
                AddBreadcrumbs("You are here:", new[]
                {
                    ("Contacts", "CompanyList.aspx"),
                    ("Companies", "CompanyList.aspx"),
                    ("Edit Company", $"EditCompany.aspx?id={idParameter}")
                });
            }
        }

        // <summary>
        /// Adds breadcrumbs to the page.
        /// </summary>
        /// <param name="prefix">The prefix text to be displayed before the breadcrumb trail.</param>
        /// <param name="items">A collection of tuples containing the text and URL for each breadcrumb item.</param>
        private void AddBreadcrumbs(string prefix, IEnumerable<(string Text, string Url)> items)
        {
            var breadcrumbTrail = new List<string>();

            // Generate breadcrumb trail HTML
            foreach (var (text, url) in items)
            {
                breadcrumbTrail.Add($"<a href=\"{url}\">{text}</a>");
            }

            // Add breadcrumbs to the page
            breadcrumbs.Controls.Add(new LiteralControl($"{prefix} {string.Join(" / ", breadcrumbTrail)}"));
        }

        /// <summary>
        /// WebMethod to fetch company details asynchronously.
        /// </summary>
        /// <param name="companyId">The ID of the company to fetch details for.</param>
        /// <returns>The company details as a finalCompanyDTO object.</returns>
        [WebMethod]
        public static finalCompanyDTO GetCompanyDetails(int companyId)
        {
            // Create an instance of the Company Business Logic class
            CompanyBL obj = new CompanyBL();

            // Call the method to get company details by ID and return the result
            return obj.GetCompanyDetailsById(companyId);
        }

        /// <summary>
        /// Event handler for the "Back" button click.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            // Redirect to the CompanyList page when the "Back" button is clicked
            Response.Redirect("/Pages/CompanyUI/CompanyList.aspx");
        }
    }
}
