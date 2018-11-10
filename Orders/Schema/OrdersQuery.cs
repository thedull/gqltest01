using System.Linq;
using GraphQL.Types;
using Orders.Services;

namespace Orders.Schema
{
    public class OrdersQuery : ObjectGraphType<object>
    {
        public OrdersQuery(IOrderService orders, ICustomerService customers)
        {
            Name = "Query";
            Field<ListGraphType<OrderType>>(
                "orders",
                resolve: context => orders.GetOrdersAsync()
            );       
            Field<CustomerType>(
                "customer",
                arguments: new QueryArguments(new QueryArgument<IntGraphType> {Name = "id"}),
                resolve: context => 
                {
                    var id = context.GetArgument<int>("id");
                    return customers.GetCustomerByIdAsync(id);
                }
            );
            Field<ListGraphType<CustomerType>>(
                "customers",
                resolve: context => customers.GetCustomersAsync()
            );
        }
    }
}