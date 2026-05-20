using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace JAPLearning.Business.Services.Internals.Shareds
{
    public class ClaimsTransformationService : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            throw new NotImplementedException();
        }
    }
}
