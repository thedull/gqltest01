using GraphQL;
using GraphQL.Types;

namespace Orders.Schema
{
    public class OrdersSchema : GraphQL.Types.Schema
    {
        public OrdersSchema(OrdersQuery query, IDependencyResolver resolver)
        {
            Query = query;
            DependencyResolver = resolver;
        }
    }
}
