using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.RequestDTO.TransactionRequestModel
{
    public class RequestTransactionReportModel
    {
        public TypeOfTransactionEnum? Type { get; set; } = null;
        public DateTime? FromDate { get; set; } = null;
        public DateTime? EndDate { get; set; } = null;
        public PaginationParameter PaginationParameter { get; set; }
	}
}
