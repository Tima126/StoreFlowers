using BusinessLogic.Authorization;
using BusinessLogic.Helpers;
using Domain.interfaces;
using Microsoft.Extensions.Options;

namespace webApi.Authorization
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSetings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSetinga)
        {
            _next = next;
            _appSetings = appSetinga.Value;
        }


        public async Task Invoke(HttpContext context, IRepositoryWrapper wrapper, IJwtUtils jwtUtils)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var account = jwtUtils.ValidateJwtToken(token);
            if(account != null)
            {
                context.Items["User"] = (await wrapper.User.GetByIdWithToken(account.Value));

            }
            await _next(context);
        }



    }
}
