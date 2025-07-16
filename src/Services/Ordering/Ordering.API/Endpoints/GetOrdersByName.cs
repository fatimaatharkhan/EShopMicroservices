﻿using Ordering.Application.Orders.Queries.GetOrdersByName;

namespace Ordering.API.Endpoints
{
    //public record GetOrdersByNameRequest(string Name);
    public record GetOrdersByNameResponse(IEnumerable<OrderDto> Orders);
    public class GetOrdersByName : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/orders/{orderName}", async (string orderName, ISender sender) =>
            {
                var result = await sender.Send(new GetOrdersByNameQuery(orderName));

                var response = result.Adapt<GetOrdersByNameResponse>();

                return Results.Ok(response);
            })
                .WithName("GetOrdersByName")
                .WithSummary("Get orders by name")
                .Produces<GetOrdersByNameResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithDescription("Retrieves a list of orders by the specified order name.");
        }
    }
}
