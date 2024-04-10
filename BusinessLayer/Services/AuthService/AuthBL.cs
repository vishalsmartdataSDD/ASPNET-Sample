using BussinessModels.Models.AuthModel;
using BussinessModels.Models.CommanResponse;
using BussinessModels.Models.Constants;
using DataAccessLayer.Repositories.AuthRepository;
using DataAccessLayer.Repositories.CommanRepository;

namespace BusinessLayer.Services.AuthService
{
    public class AuthBL
    {
        AuthRepo _context = new AuthRepo();
        CommanUtilities _utilities = new CommanUtilities();
		
		/// <summary>
		/// Checks if the provided email address is confirmed.
		/// </summary>
		/// <param name="isEmail">The email address to be checked.</param>
		/// <returns>A response return ture and false.</returns>		
        public CommonResponse<bool> IsEmailConfirmed(string isEmail)
        { 
            CommonResponse<bool> response = _context.IsEmailConfirmed(isEmail);
            return response;
        }

		/// <summary>
		/// Validates a user.
		/// </summary>
		/// <param name="request">The login request containing user credentials.</param>
		/// <returns>A response indicating whether the user is valid or not.</returns>	
        public CommonResponse<bool> IsUserValid(LoginDTO request)
        {
            try
            {
                CommonResponse<string> result = _context.IsUserValid(request);
                string decryptedResult = _utilities.Decrypt(result.Data);
                if (string.IsNullOrEmpty(decryptedResult))
                {
                    return new CommonResponse<bool>(true, CommonConstants.UserNotValidMessage, false);
                }
                else if (decryptedResult == request.Password)
                {
                    return new CommonResponse<bool>(true, CommonConstants.UserValidMessage, true);
                }
                else
                {
                    return new CommonResponse<bool>(true, CommonConstants.UserNotValidMessage, false);
                }
            }
            catch
            {
                return new CommonResponse<bool>(false, CommonConstants.ValidationError, false);
            }
        }

		/// <summary>
		/// Retrieves the user ID associated with the provided login credentials.
		/// </summary>
		/// <param name="request">The login request containing user credentials.</param>
		/// <returns>A response containing the user ID if found, along with a success message, or an error message if not found or an error occurred.</returns>
        public CommonResponse<int> GetUserId(LoginDTO request)
        {
            try
            {
                CommonResponse<int> response = _context.GetUserId(request);

                if (response.Data == 0)
                {
                    return new CommonResponse<int>(true, CommonConstants.UserNotFoundMessage, 0);
                }
                else
                {
                    return new CommonResponse<int>(true, CommonConstants.UserIdRetrievedMessage, response.Data);
                }
            }
            catch
            {
                return new CommonResponse<int>(false, CommonConstants.RetrievalError, 0);
            }
        }

		/// <summary>
		/// Registers a new user with the provided registration details.
		/// </summary>
		/// <param name="request">The registration request containing user details.</param>
		/// <returns>A response indicating whether the registration was successful or not.</returns>
        public CommonResponse<bool> RegisterUser(RegisterDTO request)
        {
            try
            {
                // Encrypt the text
                request.Password = _utilities.encrypt(request.Password);

                CommonResponse<bool> result = _context.RegisterUser(request);

                if (result.Success == true)
                {
                    return new CommonResponse<bool>(true, CommonConstants.RegistrationSuccessMessage, true);
                }
                else
                {
                    return new CommonResponse<bool>(true, CommonConstants.RegistrationErrorMessage, false);
                }
            }
            catch
            {

                return new CommonResponse<bool>(false, CommonConstants.RegistrationErrorMessage, false);
            }
        }


    }

}
