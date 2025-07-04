namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketRequest(ShoppingCart Cart);
    public record StoreBasketResponse(string UserName);
    public class StoreBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket", async (StoreBasketRequest request, ISender sender) =>
            {
                var command = request.Adapt<StoreBasketCommand>();

                var result = await sender.Send(command);

                var response = new StoreBasketResponse(result.UserName);

                return Results.Created($"/basket/{result.UserName}", response);
            })
                .WithName("StoreBasket")
                .WithSummary("Store a shopping cart in the basket")
                .WithDescription("Stores a shopping cart in the basket for a user.")
                .Produces<StoreBasketResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest);
        }
    }
}
