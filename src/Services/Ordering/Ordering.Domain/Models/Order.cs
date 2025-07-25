﻿namespace Ordering.Domain.Models
{
    public class Order : Aggregate<OrderId>
    {
        private readonly List<OrderItem> _orderItems = new();
        public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();
        public CustomerId CustomerId { get; private set; } = default!;
        public OrderName OrderName { get; private set; } = default!;
        public Address ShippingAddress { get; private set; } = default!;
        public Address BillingAddress { get; private set; } = default!;
        public Payment Payment { get; private set; } = default!;
        public OrderStatus Status { get; private set; } = OrderStatus.Pending;
        public decimal TotalPrice
        {
            get => OrderItems.Sum(item => item.Price * item.Quantity);
            private set { }
        }

        public static Order Create(OrderId id, CustomerId customerId, OrderName orderName, Address shippingAddress, Address billingAddress, Payment payment)
        {
            var order = new Order
            {
                Id = id,
                CustomerId = customerId,
                OrderName = orderName,
                ShippingAddress = shippingAddress,
                BillingAddress = billingAddress,
                Payment = payment
            };
            order.AddDomainEvent(new OrderCreatedEvent(order));
            return order;
        }

        public void Update(OrderName orderName, Address shippingAddress, Address billingAddress, Payment payment, OrderStatus status)
        {
            OrderName = orderName;
            ShippingAddress = shippingAddress;
            BillingAddress = billingAddress;
            Payment = payment;
            Status = status;
            AddDomainEvent(new OrderUpdatedEvent(this));
        }

        public void Add(ProductId productId, decimal price, int quantity)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
            var orderItem = new OrderItem(Id, productId, price, quantity);
            _orderItems.Add(orderItem);
            //AddDomainEvent(new OrderItemAddedEvent(this, orderItem));
        }

        public void Remove(ProductId productId)
        {
            var orderItem = _orderItems.FirstOrDefault(item => item.ProductId == productId);
            if (orderItem != null)
            {
                _orderItems.Remove(orderItem);
                //AddDomainEvent(new OrderItemRemovedEvent(this, orderItem));
            }
        }
    }
}
