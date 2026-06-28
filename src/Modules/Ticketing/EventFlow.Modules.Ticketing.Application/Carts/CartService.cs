using EventFlow.Common.Application.Caching;

namespace EventFlow.Modules.Ticketing.Application.Carts;

public sealed class CartService(ICacheService cacheService)
{
    // Keep shopping carts in the cache for a limited time to avoid
    // storing abandoned carts indefinitely.
    private static readonly TimeSpan DefaultExpiration = TimeSpan.FromMinutes(20);

    public async Task<Cart> GetAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        string cacheKey = CreateCacheKey(customerId);

        // Return the cached cart if it exists; otherwise create a new
        // empty cart so callers never have to handle a null value.
        Cart cart = await cacheService.GetAsync<Cart>(cacheKey, cancellationToken) ??
                    Cart.CreateDefault(customerId);

        return cart;
    }

    public async Task ClearAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        string cacheKey = CreateCacheKey(customerId);

        // Replace the existing cart with a new empty instance instead
        // of removing the cache entry entirely.
        var cart = Cart.CreateDefault(customerId);

        await cacheService.SetAsync(cacheKey, cart, DefaultExpiration, cancellationToken);
    }

    public async Task AddItemAsync(Guid customerId, CartItem cartItem, CancellationToken cancellationToken = default)
    {
        string cacheKey = CreateCacheKey(customerId);

        Cart cart = await GetAsync(customerId, cancellationToken);

        CartItem? existingCartItem = cart.Items.Find(c => c.TicketTypeId == cartItem.TicketTypeId);

        if (existingCartItem is null)
        {
            // Add the item if it does not already exist in the cart.
            cart.Items.Add(cartItem);
        }
        else
        {
            // Merge duplicate items by increasing the quantity instead
            // of storing multiple entries for the same ticket type.
            existingCartItem.Quantity += cartItem.Quantity;
        }

        // Persist the updated cart back to the cache and refresh its expiration.
        await cacheService.SetAsync(cacheKey, cart, DefaultExpiration, cancellationToken);
    }

    public async Task RemoveItemAsync(Guid customerId, Guid ticketTypeId, CancellationToken cancellationToken = default)
    {
        string cacheKey = CreateCacheKey(customerId);

        Cart cart = await GetAsync(customerId, cancellationToken);

        CartItem? cartItem = cart.Items.Find(c => c.TicketTypeId == ticketTypeId);

        // Nothing to remove if the requested item is not in the cart.
        if (cartItem is null)
        {
            return;
        }

        cart.Items.Remove(cartItem);

        // Persist the updated cart after removing the item.
        await cacheService.SetAsync(cacheKey, cart, DefaultExpiration, cancellationToken);
    }

    // Use a predictable cache key to ensure each customer has an isolated cart.
    private static string CreateCacheKey(Guid customerId) => $"carts:{customerId}";
}
