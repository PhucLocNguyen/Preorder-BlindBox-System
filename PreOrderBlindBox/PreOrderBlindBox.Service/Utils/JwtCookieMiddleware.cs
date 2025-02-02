using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.Utils
{
	public class JwtCookieMiddleware
	{
		private readonly RequestDelegate _next;

		public JwtCookieMiddleware(RequestDelegate next)
		{
			_next = next;
		}
		public async Task Invoke(HttpContext context)
		{
			if (!context.Request.Headers.ContainsKey("Authorization"))
			{
				var token = context.Request.Cookies["accessToken"];
				if (!string.IsNullOrEmpty(token))
				{
					context.Request.Headers.Append("Authorization", "Bearer " + token);
				}
			}
			await _next(context);
		}


	}
}
