using Ordering.Application.Orders.Queries.GetOrdersByCustomer;
using Ordering.Application.Orders.Queries.GetOrdersByName;

namespace Ordering.API.Endpoints
{
    //public record GetOrdersByCustomerRequest(string Name);
    public record GetOrdersByCustomerResponse(IEnumerable<OrderDto> Orders);
    public class GetOrdersByCustomer : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/orders/customer/{customerId}", async (Guid customerId, ISender sender) =>
            {
                 var result = await sender.Send(new GetOrdersByCustomerQuery(customerId));

                var response = result.Adapt<GetOrdersByCustomerResponse>();

                return Results.Ok(response);
            })
                .WithName("GetOrdersByCustomer")
                .WithSummary("Get orders by Customer")
                .Produces<GetOrdersByCustomerResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithDescription("Retrieves a list of orders by the specified Customer Id.");
        }
    }
}
