using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DotnetAwsCognito.Controllers
{
    public class AuthenticationController : ControllerBase
    {
        private readonly string _clientId;
        private readonly string _userPoolId;
        private readonly RegionEndpoint _region = RegionEndpoint.USEast2;

        public AuthenticationController(IConfiguration configuration)
        {
            _clientId = configuration["AWS:CLIENT_ID"];
            _userPoolId = configuration["AWS:USERPOOL_ID"];
        }

        [HttpPost]
        [Route("api/register")]
        public async Task<ActionResult<string>> Register([FromBody] User user)
        {
            var cognito = new AmazonCognitoIdentityProviderClient(_region);

            var request = new SignUpRequest
            {
                ClientId = _clientId,
                Password = user.Password,
                Username = user.Username,
                UserAttributes = new List<AttributeType>
                {
                    new AttributeType {Name = "family_name", Value = user.FamilyName},
                    new AttributeType {Name = "gender", Value = user.Gender},
                    new AttributeType {Name = "given_name", Value = user.GivenName},
                    new AttributeType {Name = "name", Value = user.Name},
                    new AttributeType {Name = "phone_number", Value = user.PhoneNumber},
                    new AttributeType {Name = "email", Value = user.Email}
                }
            };

            try
            {
                await cognito.SignUpAsync(request);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpPost]
        [Route("api/signin")]
        public async Task<ActionResult<string>> SignIn([FromBody] User user)
        {
            var cognito = new AmazonCognitoIdentityProviderClient(_region);

            var request = new AdminInitiateAuthRequest
            {
                UserPoolId = _userPoolId,
                ClientId = _clientId,
                AuthFlow = AuthFlowType.ADMIN_NO_SRP_AUTH
            };

            request.AuthParameters.Add("USERNAME", user.Username);
            request.AuthParameters.Add("PASSWORD", user.Password);

            var response = await cognito.AdminInitiateAuthAsync(request);

            return Ok(response.AuthenticationResult.IdToken);
        }
    }
}