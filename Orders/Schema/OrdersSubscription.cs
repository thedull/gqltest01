using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using GraphQL.Types;
using GraphQL.Subscription;
using GraphQL.Resolvers;
using Orders.Models;
using Orders.Services;

namespace Orders.Schema
{
    public class OrdersSubscription : ObjectGraphType<object>
    {
        private readonly IOrderEventService _events;

        public OrdersSubscription(IOrderEventService events)
        {
            _events = events;
            Name = "Subscription";
            AddField(new EventStreamFieldType
            {
                Name = "orderEvent",
                Arguments = new QueryArguments(new QueryArgument<ListGraphType<OrderStatusesEnum>>
                {
                    Name = "statuses"
                }),
                Type = typeof(OrderEventType),
                Resolver = new FuncFieldResolver<OrderEvent>(ResolveEvent),
                Subscriber = new EventStreamResolver<OrderEvent>(Subscribe)
            });
        }

        private IObservable<OrderEvent> Subscribe(ResolveEventStreamContext context)
        {
            var statusList = context.GetArgument<IList<OrderStatuses>>("statuses", new List<OrderStatuses>());

            if (statusList.Count > 0)
            {
                OrderStatuses statuses = 0;

                foreach (var status in statusList)
                {
                    statuses = statuses | status;    
                }
                return _events.EventStream().Where(e => (e.Status & statuses) == e.Status);
            }
            else
            {
                return _events.EventStream();
            }
        
        }

        private OrderEvent ResolveEvent(ResolveFieldContext context)
        {
            var orderEvent = context.Source as OrderEvent;
            return orderEvent;
        }
    }
}