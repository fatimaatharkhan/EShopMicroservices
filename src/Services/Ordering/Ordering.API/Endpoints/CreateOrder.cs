﻿using Ordering.Application.Orders.Commands.CreateOrder;

namespace Ordering.API.Endpoints
{
    public record  CreateOrderRequest(OrderDto Order);

    public record CreateOrderResponse(Guid Id);

    public class CreateOrder : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/orders", async (CreateOrderRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateOrderCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<CreateOrderResponse>();

                return Results.Created($"/orders/{response.Id}", response);
            })
                .WithName("CreateOrder")
                .WithSummary("Create a new order")
                .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest)
                .WithDescription("Creates a new order in the system.");
        }
    }
}
