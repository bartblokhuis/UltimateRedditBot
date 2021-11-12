
using AutoMapper;
using Core.Features.Subscriptions.Commands;
using Domain.Entities;

namespace Core.Mappings;
public class SubscriptionProfile : Profile
{
    public SubscriptionProfile()
    {
        CreateMap<Subscription, SubscribeCommand>().ReverseMap();
    }
}
