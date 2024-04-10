using BusinessLayer.Services.CompanyService;
using BussinessModels.Models.Company;
using BussinessModels.Models.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SampleCode.Pages.CompanyUI
{
    public partial class CompanyList : System.Web.UI.Page
    {
        // Default page number for pagination
        public int ActivePageNo = CommonConstants.DefaultPageNumber;

        protected void Page_Load(object sender, EventArgs e)
        {
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
                });

                // Bind the grid view only if it's not a postback
                if (!IsPostBack)
                {
                    BindGridView(CommonConstants.DefaultPageNumber, CommonConstants.DefaultPageSize);
                }
            }
        }

        /// <summary>
        /// Adds breadcrumbs to the page.
        /// </summary>
        /// <param name="prefix">The prefix text to be displayed before the breadcrumbs.</param>
        /// <param name="items">A collection of tuples containing the text and URL for each breadcrumb item.</param>
        private void AddBreadcrumbs(string prefix, IEnumerable<(string Text, string Url)> items)
        {
            var breadcrumbTrail = items.Select(item => $"<a href=\"{item.Url}\">{item.Text}</a>");
            breadcrumbs.Controls.Add(new LiteralControl($"{prefix} {string.Join(" / ", breadcrumbTrail)}"));
        }

        /// <summary>
        /// Event handler for the "Add" button click. Redirects the user to the "AddCompany.aspx" page.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddCompany.aspx");
        }

        /// <summary>
        /// Event handler for grid view row commands (Edit, View, Delete).
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        protected void grdView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int companyId = Convert.ToInt32(e.CommandArgument.ToString());

            // Redirect based on the command name
            switch (e.CommandName)
            {
                case "Edit":
                    Response.Redirect($"EditCompany.aspx?id={companyId}");
                    break;
                case "View":
                    Response.Redirect($"ViewCompany.aspx?id={companyId}");
                    break;
                case "Delete":
                    CompanyBL companyBL = new CompanyBL();
                    companyBL.DeleteCompany(companyId);
                    Response.Redirect("CompanyList.aspx");
                    break;
            }
        }

        /// <summary>
        /// Event handler for grid view page index changing.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments containing the new page index.</param>
        protected void grdView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdView.PageIndex = e.NewPageIndex;
            BindGridView(e.NewPageIndex, CommonConstants.DefaultPageSize);
        }

        /// <summary>
        /// Event handler for grid view sorting.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments containing the sort expression.</param>
        protected void grdView_Sorting(object sender, GridViewSortEventArgs e)
        {
            // Sorting logic
            string sortExpression = ViewState["SortExpression"] as string;
            string sortDirection = ViewState["SortDirection"] as string;

            if (e.SortExpression == sortExpression)
            {
                sortDirection = (sortDirection == "ASC") ? "DESC" : "ASC";
            }
            else
            {
                sortDirection = "ASC";
            }

            ViewState["SortExpression"] = e.SortExpression;
            ViewState["SortDirection"] = sortDirection;

            // Fetch and sort the data
            CompanyBL obj = new CompanyBL();
            List<CompanyDTO> data = obj.GetAllCompanies();
            DataTable dt = ConvertListToDataTable(data);

            if (dt != null)
            {
                dt.DefaultView.Sort = $"{ViewState["SortExpression"]} {ViewState["SortDirection"]}";
                grdView.DataSource = dt;
                grdView.DataBind();
            }
        }

        /// <summary>
        /// Converts a list of objects to a DataTable.
        /// </summary>
        /// <typeparam name="T">The type of objects in the list.</typeparam>
        /// <param name="list">The list of objects to be converted.</param>
        /// <returns>A DataTable containing the data from the list.</returns>
        private DataTable ConvertListToDataTable<T>(List<T> list)
        {
            DataTable table = new DataTable();

            foreach (var prop in typeof(T).GetProperties())
            {
                table.Columns.Add(prop.Name, prop.PropertyType);
            }

            foreach (var item in list)
            {
                DataRow row = table.NewRow();

                foreach (var prop in typeof(T).GetProperties())
                {
                    row[prop.Name] = prop.GetValue(item);
                }

                table.Rows.Add(row);
            }

            return table;
        }

        /// <summary>
        /// Event handler for smart search text changed.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        protected void txtSmartSearch_TextChanged(object sender, EventArgs e)
        {
            // Filter data based on the smart search text
            string searchQuery = txtSmartSearch.Text.ToLower();

            if (string.IsNullOrEmpty(searchQuery))
            {
                BindGridView(CommonConstants.DefaultPageNumber, CommonConstants.DefaultPageSize);
            }
            else
            {
                BindGridViewWithFilter(searchQuery);
            }
        }

        /// <summary>
        /// Event handler for search button click.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Filter data based on the search text
            string searchQuery = txtSmartSearch.Text.ToLower();

            if (string.IsNullOrEmpty(searchQuery))
            {
                BindGridView(CommonConstants.DefaultPageNumber, CommonConstants.DefaultPageSize);
            }
            else
            {
                BindGridViewWithFilter(searchQuery);
            }
        }

        /// <summary>
        /// Filters the grid view data based on the search query and binds the grid view with the filtered data.
        /// </summary>
        /// <param name="searchQuery">The search query used to filter the data.</param>
        private void BindGridViewWithFilter(string searchQuery)
        {
            CompanyBL obj = new CompanyBL();
            List<CompanyDTO> data = obj.GetAllCompanies();

            List<CompanyDTO> filteredData = data
                .Where(item =>
                    item.CustomerCode.ToLower().Contains(searchQuery) ||
                    item.CompanyName.ToLower().Contains(searchQuery) ||
                    item.SharedWithName.ToLower().Contains(searchQuery) ||
                    item.CreatedBy.ToLower().Contains(searchQuery) ||
                    item.CreatedDate.ToLower().Contains(searchQuery) ||
                    item.ModifiedDate.ToLower().Contains(searchQuery))
                .ToList();

            grdView.DataSource = filteredData;
            grdView.DataBind();
        }

        /// <summary>
        /// Event handler for the "Back" button click. Redirects the user to the secure page.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/AuthUI/SecurePage.aspx");
        }

        /// <summary>
        /// Binds the grid view with data and handles pagination.
        /// </summary>
        /// <param name="pageNumber">The current page number.</param>
        /// <param name="pageSize">The number of items per page.</param>
        private void BindGridView(int pageNumber, int pageSize)
        {
            CompanyBL obj = new CompanyBL();
            List<CompanyDTO> data = obj.GetAllCompanies(pageNumber, pageSize);

            // Get total rows for pagination
            int totalRows = data.FirstOrDefault()?.TotalRows ?? 0;

            grdView.DataSource = data;
            grdView.DataBind();
            PopulatePager(totalRows, pageSize);
        }

        /// <summary>
        /// Event handler for page changed in the pager.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        protected void Page_Changed(object sender, EventArgs e)
        {
            txtSmartSearch.Text = string.Empty;
            int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
            ActivePageNo = pageIndex;
            BindGridView(pageIndex, CommonConstants.DefaultPageSize);
        }

        /// <summary>
        /// Populates the pager control based on the total record count and current page.
        /// </summary>
        /// <param name="recordCount">The total number of records.</param>
        /// <param name="currentPage">The current page number.</param>
        private void PopulatePager(int recordCount, int currentPage)
        {
            double dblPageCount = recordCount;
            int pageCount = (int)Math.Ceiling(dblPageCount / currentPage);

            List<ListItem> pages = new List<ListItem> { new ListItem("", "1", currentPage > 1) };

            for (int i = 1; i <= pageCount; i++)
            {
                pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
            }

            pages.Add(new ListItem("", pageCount.ToString(), currentPage < pageCount));

            rptPager.DataSource = pages;
            rptPager.DataBind();
        }

        /// <summary>
        /// Event handler for item data bound in the pager.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        protected void rptPager_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int activePage = ActivePageNo;
                var lnkPage = e.Item.FindControl("lnkPage") as LinkButton;

                if (lnkPage != null)
                {
                    int pageNumber = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "Value"));

                    if (pageNumber == activePage)
                    {
                        lnkPage.CssClass += " active";
                    }
                }
            }
        }
    }
}
