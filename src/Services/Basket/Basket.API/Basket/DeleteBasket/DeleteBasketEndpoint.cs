
namespace Basket.API.Basket.DeleteBasket
{
    //public record class DeleteBasketRequest(string UserName);
    public record class DeleteBasketResponse(bool Success);
    public class DeleteBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/basket/{userName}", async (string userName, ISender sender) =>
            {
                var result = await sender.Send(new DeleteBasketCommand(userName));

                var response = result.Adapt<DeleteBasketResponse>();

                return Results.Ok(response);
            })
                .WithName("DeleteBasket")
                .WithSummary("Delete a shopping cart from the basket")
                .WithDescription("Deletes a shopping cart from the basket for a user.")
                .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest);
        }
    }
}
