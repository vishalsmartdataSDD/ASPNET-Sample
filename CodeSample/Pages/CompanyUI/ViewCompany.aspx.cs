using BusinessLayer.Services.CompanyService;
using BussinessModels.Models.CompanyModel;
using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.UI;

namespace SampleCode.Pages.CompanyUI
{
    public partial class ViewCompany : System.Web.UI.Page
    {
        // Property to store the CompanyId retrieved from the query string
        public string CompanyId = string.Empty;

        // Page Load Event
        protected void Page_Load(object sender, EventArgs e)
        {
            // Retrieve CompanyId from the query string
            CompanyId = Request.QueryString["id"];

            // Check if the user is authenticated, if not, redirect to the login page
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("/Pages/AuthUI/Login.aspx");
            }
            else
            {
                // Add breadcrumbs for navigation
                AddBreadcrumbs("You are here:", new[]
                {
                    ("Contacts", "CompanyList.aspx"),
                    ("Companies", "CompanyList.aspx"),
                    ("View Company", "ViewCompany.aspx?id=" + CompanyId)
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

            // Create breadcrumb trail using anchor tags
            foreach (var (text, url) in items)
            {
                breadcrumbTrail.Add($"<a href=\"{url}\">{text}</a>");
            }

            // Add the breadcrumb trail to the page
            breadcrumbs.Controls.Add(new LiteralControl($"{prefix} {string.Join(" / ", breadcrumbTrail)}"));
        }

        /// <summary>
        /// WebMethod to get company details asynchronously.
        /// </summary>
        /// <param name="companyId">The ID of the company to fetch details for.</param>
        /// <returns>The company details as a finalCompanyDTO object.</returns>
        [WebMethod]
        public static finalCompanyDTO GetCompanyDetails(int companyId)
        {
            CompanyBL obj = new CompanyBL();
            return obj.GetCompanyDetailsById(companyId);
        }

        /// <summary>
        /// Button click event handler to navigate back to the CompanyList page.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/CompanyUI/CompanyList.aspx");
        }

        /// <summary>
        /// LinkButton click event handler to navigate to the EditCompany page.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("EditCompany.aspx?id=" + CompanyId);
        }

        /// <summary>
        /// WebMethod to delete a company asynchronously.
        /// </summary>
        /// <param name="companyId">The ID of the company to delete.</param>
        /// <returns>True if the company is successfully deleted; otherwise, false.</returns>
        [WebMethod]
        public static bool DeleteComapnyById(int companyId)
        {
            CompanyBL companyBL = new CompanyBL();
            return companyBL.DeleteCompany(Convert.ToInt32(companyId));
        }
    }
}
