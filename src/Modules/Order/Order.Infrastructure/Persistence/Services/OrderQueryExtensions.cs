using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Order.Application.Features.Order.Dtos;
using Order.Domain.Entities;

namespace Order.Infrastructure.Persistence.Services;

public static class OrderQueryExtensions
{
    public static IQueryable<OrderDto> ToOrderDto(this IQueryable<OrderEntity> query)
    {
        return query.Select(o => new OrderDto
        {
            Id = o.Id,
            Customer = new CustomerDto
            {
                Name = o.Customer.Name,
                PhoneNumber = o.Customer.PhoneNumber,
                Email = o.Customer.Email,
                Address = o.Customer.Address
            },
            OrderNo = o.OrderNo.Value,
            TotalPrice = o.TotalPrice,
            OrderStatus = o.OrderStatus,
            PaymentMethod = o.PaymentMethod,
            Notes = o.Notes,
            CreatedAt = o.CreatedAt,
            OrderItems = o.OrderItems.Select(i => new OrderItemDto
            {
                ProductId = i.Product.Id,
                ProductName = i.Product.Name,
                ThumbnailUrl = i.Product.ThumbnailUrl,
                Quantity = i.Quantity,
                Price = i.Product.Price,
            }).ToList()
        });
    }

    public static Expression<Func<OrderEntity, OrderSummaryDto>> ToSummaryDto()
    {
        return o => new OrderSummaryDto
        {
            Id = o.Id,
            OrderNo = o.OrderNo.Value,
            CustomerName = o.Customer.Name,
            TotalPrice = o.TotalPrice,
            OrderStatus = o.OrderStatus,
            PaymentMethod = o.PaymentMethod,
            CreatedAt = o.CreatedAt,
            TotalItems = o.OrderItems.Count
        };
    }

    public static IQueryable<OrderEntity> ApplyOrderFilter(
        this IQueryable<OrderEntity> query,
        OrderFilterBase filter)
    {
        if (filter.OrderStatus.HasValue)
        {
            query = query.Where(o => o.OrderStatus == filter.OrderStatus.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.Keyword))
        {
            var search = filter.Keyword.Trim();

            query = query.Where(o =>
                EF.Functions.Like(o.OrderNo.Value, $"%{search}%"));
        }

        if (filter.FromDate.HasValue)
        {
            query = query.Where(o =>o.OrderDate >= filter.FromDate.Value);
        }

        if (filter.ToDate.HasValue)
        {
            query = query.Where(x => x.OrderDate <= filter.ToDate.Value);
        }

        return query;
    }
}