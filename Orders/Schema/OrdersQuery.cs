using System.Linq;
using GraphQL.Types;
using Orders.Services;

namespace Orders.Schema
{
    public class OrdersQuery : ObjectGraphType<object>
    {
        public OrdersQuery(IOrderService orders)
        {
            Name = "Query";
            Field<ListGraphType<OrderType>>(
                "orders",
                resolve: context => orders.GetOrdersAsync()
            );       
        }
    }
}