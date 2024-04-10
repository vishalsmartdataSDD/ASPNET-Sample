using BussinessModels.Models.Company;
using BussinessModels.Models.CompanyModel;
using DataAccessLayer.Repositories.CompanyRepository;
using System.Collections.Generic;

namespace BusinessLayer.Services.CompanyService
{
    public class CompanyBL
    {
        CompanyRepo _context = new CompanyRepo();
       
	    /// <summary>
		/// Adds or edits a company with the provided data.
		/// </summary>
		/// <param name="data">The company data to be added or edited.</param>
        public CommonResponse AddEditCompany(CompanyModel data)
        {
            try
            {
                return _context.AddEditCompany(data);
            }
            catch
            {
                throw;
            }
        }
		
		
		/// <summary>
		/// Retrieves a list of all companies.
		/// </summary>
		/// <returns>A list of CompanyDTO objects representing all companies.</returns>
        public List<CompanyDTO> GetAllCompanies()
        {
            try
            {
               
                return _context.GetAllCompanies();
            }
            catch
            {
                throw;
            }
        }
		
		/// <summary>
		/// Retrieves a paginated list of companies.
		/// </summary>
		/// <param name="pageNumber">The page number of the results.</param>
		/// <param name="pageSize">The number of companies per page.</param>
		/// <returns>A list of CompanyDTO objects representing the companies on the specified page.</returns>
        public List<CompanyDTO> GetAllCompanies(int pageNumber, int pageSize)
        {
            try
            {
                return _context.GetAllCompanies(pageNumber, pageSize);
            }
            catch
            {
                throw;
            }
        }
		
		/// <summary>
		/// Retrieves the details of a company based on the provided company ID.
		/// </summary>
		/// <param name="companyId">The ID of the company to retrieve details for.</param>
		/// <returns>A finalCompanyDTO object representing the details of the company, or null if not found or an error occurs.</returns>
        public finalCompanyDTO GetCompanyDetailsById(int companyId)
        {
            try
            {
               
                return _context.GetCompanyDetailsById(companyId);
            }
            catch
            {
                return null;
            }
        }
		
		/// <summary>
		/// Retrieves a list of payment terms.
		/// </summary>
		/// <returns>A list of PaymentTermsDTO objects representing the payment terms, or null if an error occurs.</returns>
        public List<PaymentTermsDTO> GetPaymentTerms()
        {
            try
            {
               
                return _context.GetPaymentTerms();
            }
            catch
            {
                return null;
            }
        }
		
		/// <summary>
		/// Checks if a customer code already exists.
		/// </summary>
		/// <param name="customerCode">The customer code to check for existence.</param>
		/// <returns>True if the customer code exists, otherwise false. Returns false if an error occurs.</returns>
        public bool IsCustomerCodeExist(string customerCode)
        {
            try
            {
                
                return _context.IsCustomerCodeExist(customerCode);
            }
            catch
            {
                return false;
            }
        }
		
		/// <summary>
		/// Retrieves a list of company members associated with the specified branch.
		/// </summary>
		/// <param name="branchId">The ID of the branch to retrieve company members for.</param>
		/// <returns>A list of CompanyMembersDto objects representing the company members, or null if an error occurs.</returns>
        public List<CompanyMembersDto> GetCompanyMembers(int branchId)
        {
            try
            {
               
                return _context.GetCompanyMembers(branchId);
            }
            catch
            {
                return null;
            }
        }
		
		/// <summary>
		/// Deletes a person with the specified ID.
		/// </summary>
		/// <param name="personId">The ID of the person to delete.</param>
		/// <returns>True if the person is successfully deleted, otherwise false. Returns false if an error occurs.</returns>
        public bool DeletePerson(int personId)
        {
            try
            {

                return _context.DeletePerson(personId);
            }
            catch
            {
                return false;
            }
        }
		
		/// <summary>
		/// Deletes a company with the specified ID.
		/// </summary>
		/// <param name="companyId">The ID of the company to delete.</param>
		/// <returns>True if the company is successfully deleted, otherwise false. Returns false if an error occurs.</returns>
        public bool DeleteCompany(int companyId)
        {
            try
            {

                return _context.DeleteCompany(companyId);
            }
            catch
            {
                return false;
            }
        }
    }
}
