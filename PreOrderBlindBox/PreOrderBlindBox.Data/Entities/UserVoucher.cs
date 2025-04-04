﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace PreOrderBlindBox.Data.Entities;

public partial class UserVoucher
{
    public int UserVoucherId { get; set; }

    public int? UserId { get; set; }

    public int? VoucherCampaignId { get; set; }

    public int Quantity { get; set; }

    public int UsedQuantity { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<TempCampaignBulkOrder> TempCampaignBulkOrders { get; set; } = new List<TempCampaignBulkOrder>();

    public virtual User User { get; set; }

    public virtual VoucherCampaign VoucherCampaign { get; set; }
}