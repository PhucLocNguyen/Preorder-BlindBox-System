using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.RequestDTO.NotificationRequestModel
{
    public class RequestCreateNotification
    {
        public int? ReceiverId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public RequestCreateNotification NotificationForCustomer(int customerID)
        {
            return new RequestCreateNotification()
            {
                ReceiverId = customerID,
                Title = "Placing Order",
                Description = "You have successfully placed your order.",
            };
        }

        public RequestCreateNotification NotificationForStaff(string customerName, int staffID)
        {
            return new RequestCreateNotification()
            {
                ReceiverId = staffID,
                Title = "Successfully pre-ordered",
                Description = $"Customer {customerName} has successfully placed an order.",
            };
        }
    }
}
