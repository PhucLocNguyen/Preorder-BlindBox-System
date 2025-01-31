﻿using PreOrderBlindBox.Services.DTO.RequestDTO.MailModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.IServices
{
	public interface IMailService
	{
		public Task sendEmailAsync(MailRequest mailRequest);
	}
}
