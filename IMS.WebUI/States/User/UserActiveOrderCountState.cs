﻿using IMS.Application.Service.Orders.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace IMS.WebUI.States.User
{
    public class UserActiveOrderCountState(IServiceScopeFactory serviceProvider)
    {
        public int ProcessingCount { get; set; }
        public int DeliveringCount { get; set; }
        public int DeliveredCount { get; set; }
        public int CanceledCount { get; set; }

        public event Action? StateChanged;

        public async Task GetActiveOrdersCount(string userId)
        {
            using var scope = serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>(); 
            var response = await mediator.Send(new GetGenericOrdersCountQuery(userId,false));
            ProcessingCount = response.Processing;
            DeliveringCount = response.Delivering;
            DeliveredCount = response.Delivered;
            CanceledCount = response.Canceled;

            StateChanged?.Invoke();
        }
    }
}
