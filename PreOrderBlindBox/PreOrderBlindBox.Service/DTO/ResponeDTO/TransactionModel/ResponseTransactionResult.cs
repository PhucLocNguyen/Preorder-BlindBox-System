using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.ResponeDTO.TransactionModel
{
    public class ResponseTransactionResult
    {
        public int TransactionId { get; set; }
        public string Description { get; set; }

        public string Status { get; set; }

        public string Type { get; set; }

        public decimal Money { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
