﻿using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.DBContext;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.Enum;
using PreOrderBlindBox.Data.GenericRepository;
using PreOrderBlindBox.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Data.Repositories
{
    public class PreorderCampaignRepository : GenericRepository<PreorderCampaign>, IPreorderCampaignRepository
    {
        public PreorderCampaignRepository(Preorder_BlindBoxContext context) : base(context)
        {
        }

        public async Task<List<PreorderCampaign>> GetAllValidPreorderCampaign(PaginationParameter paginationParameter, string? type)
        {
            Expression<Func<PreorderCampaign, bool>> filter = pc =>
                !pc.IsDeleted && (type == null || pc.Type == type);
            var items = await GetAll(paginationParameter, filter, null, includes: x => x.BlindBox);
            return items;
        }

        public async Task<List<PreorderCampaign>> GetAllActivePreorderCampaign(PaginationParameter paginationParameter, string? type)
        {
            Expression<Func<PreorderCampaign, bool>> filter = pc =>
                !pc.IsDeleted && (type == null || pc.Type == type) && pc.Status == "Active";
            var items = await GetAll(paginationParameter, filter, null, includes: x => x.BlindBox);
            return items;
        }

        public async Task<List<PreorderCampaign?>> GetAllCompleteBulkPreorderCampaign(PaginationParameter paginationParameter)
        {
            Expression<Func<PreorderCampaign, bool>> filter = pc =>
                !pc.IsDeleted && pc.Status == PreorderCampaignStatus.Completed.ToString() && pc.BlindBox != null
                && pc.Type == PreorderCampaignType.BulkOrder.ToString();

            Expression<Func<PreorderCampaign, object>>[] includes = new Expression<Func<PreorderCampaign, object>>[]
            {
                pc => pc.BlindBox,
                pc => pc.PreorderMilestones,
                pc => pc.OrderDetails
            };

            var items = await GetAll(paginationParameter, filter, null, includes);
            return items;
        }

        public async Task<PreorderCampaign?> GetDetailPreorderCampaignById(int id)
        {
            //return await _context.PreorderCampaigns.Include(x => x.BlindBox).FirstOrDefaultAsync(x => x.PreorderCampaignId == id);
            return await _context.PreorderCampaigns
                .Include(pc => pc.BlindBox)
                .ThenInclude(bb => bb.Images) // Lấy toàn bộ hình ảnh trong cùng một truy vấn
                .FirstOrDefaultAsync(pc => pc.PreorderCampaignId == id);
        }

        public async Task<PreorderCampaign?> GetPreorderCampaignBySlugAsync(string? slug)
        {
            if (string.IsNullOrEmpty(slug))
                return null;

            return await _context.PreorderCampaigns.Include(pc => pc.BlindBox).ThenInclude(pc => pc.Images).FirstOrDefaultAsync(c => c.Slug == slug);
        }

        public async Task<List<PreorderCampaign>> GetAllPreorderCampaign()
        {
            return await _context.PreorderCampaigns./*Where(x => x.IsDeleted == false).*/ToListAsync();
        }

        public async Task UpdateRangeAsync(IEnumerable<PreorderCampaign> preorderCampaigns)
        {
            _context.PreorderCampaigns.UpdateRange(preorderCampaigns);
        }

        public async Task<List<PreorderCampaign>> SearchPreorderCampaign(
            string blindBoxName,
            string sortOrder,
            PaginationParameter paginationParameter)
        {
            // Điều kiện chung: campaign không bị xóa, có BlindBox và trạng thái Active
            Expression<Func<PreorderCampaign, bool>> filter = pc =>
                !pc.IsDeleted &&
                pc.Status == "Active" &&
                pc.BlindBox != null &&
                (string.IsNullOrEmpty(blindBoxName) || pc.BlindBox.Name.Contains(blindBoxName));

            // Include các quan hệ cần thiết (không include hình ảnh)
            Expression<Func<PreorderCampaign, object>>[] includes = new Expression<Func<PreorderCampaign, object>>[]
            {
                pc => pc.BlindBox,
                pc => pc.PreorderMilestones,
                pc => pc.OrderDetails
            };

            // Xây dựng hàm sắp xếp dựa trên sortOrder (ví dụ như cũ)
            Func<IQueryable<PreorderCampaign>, IOrderedQueryable<PreorderCampaign>> orderBy;
            switch (sortOrder)
            {
                case "AZ":
                    orderBy = q => q.OrderBy(pc => pc.BlindBox.Name);
                    break;
                case "ZA":
                    orderBy = q => q.OrderByDescending(pc => pc.BlindBox.Name);
                    break;
                case "Newest":
                    orderBy = q => q.OrderByDescending(pc => pc.StartDate);
                    break;
                case "BestSelling":
                    orderBy = q => q.OrderByDescending(pc => pc.OrderDetails.Sum(od => od.Quantity));
                    break;
                case "PriceAscending":
                    orderBy = q => q.OrderBy(pc => pc.PreorderMilestones.Max(m => (decimal?)m.Price) ?? 0);
                    break;
                case "PriceDescending":
                    orderBy = q => q.OrderByDescending(pc => pc.PreorderMilestones.Max(m => (decimal?)m.Price) ?? 0);
                    break;
                default:
                    orderBy = q => q.OrderByDescending(pc => pc.CreatedDate);
                    break;
            }

            // Gọi hàm GetAll từ GenericRepository với filter, include, orderBy và phân trang
            return await GetAll(paginationParameter, filter, orderBy, includes);
        }

        public async Task<List<PreorderCampaign>> FilterPreorderCampaignsAsync(string? type,
            bool isEndingSoon, bool isNewlyLaunched, bool isTrending, PaginationParameter? pagination)
        {
            var now = DateTime.UtcNow;

            Expression<Func<PreorderCampaign, bool>> filter = pc => !pc.IsDeleted &&
                pc.Status == "Active" &&
                pc.BlindBox != null &&
                (type == null || pc.Type == type);

            Expression<Func<PreorderCampaign, object>>[] includes = new Expression<Func<PreorderCampaign, object>>[]
            {
                pc => pc.BlindBox,
                pc => pc.PreorderMilestones,
                pc => pc.OrderDetails
            };

            Func<IQueryable<PreorderCampaign>, IOrderedQueryable<PreorderCampaign>>? orderBy = null;

            if (isTrending)
            {
                orderBy = q => q.OrderByDescending(pc => pc.PlacedOrderCount);
            }
            if (isNewlyLaunched)
            {
                orderBy = q => q.OrderByDescending(pc => pc.StartDate);
            }
            if (isEndingSoon)
            {
                orderBy = q => q.OrderBy(pc => pc.EndDate);
            }

            return await GetAll(pagination, filter, orderBy, includes);
        }

        public async Task<List<PreorderCampaign>> GetSimilarPreorderCampaign(int id)
        {
            // Lấy thông tin bản ghi hiện tại
            var currentCampaign = await GetDetailPreorderCampaignById(id);
            if (currentCampaign == null)
            {
                return new List<PreorderCampaign>();
            }

            var currentType = currentCampaign.Type;
            Expression<Func<PreorderCampaign, bool>> filter = pc =>
                !pc.IsDeleted &&
                pc.Status == "Active" &&
                pc.BlindBox != null &&
                pc.Type == currentType &&
                pc.PreorderCampaignId != id;

            // Include các quan hệ cần thiết (không include hình ảnh)
            Expression<Func<PreorderCampaign, object>>[] includes = new Expression<Func<PreorderCampaign, object>>[]
            {
                pc => pc.BlindBox,
                pc => pc.PreorderMilestones,
                pc => pc.OrderDetails
            };

            return await GetAll(null, filter, null, includes);
        }

    }
}
