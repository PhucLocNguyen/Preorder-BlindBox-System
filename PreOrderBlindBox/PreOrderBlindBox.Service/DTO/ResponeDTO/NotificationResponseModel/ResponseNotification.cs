using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.ResponeDTO.NotificationResponseModel
{
    public class ResponseNotification
    {
		public int NotificationId { get; set; }
		public string Title { get; set; }
        public string Description { get; set; }
        public bool IsRead { get; set; }
		public DateTime CreatedDate { get; set; }
	}
}
