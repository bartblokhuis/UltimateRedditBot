using AutoMapper;
using Core.Features.Common;
using Database;
using Domain.Dtos;
using Domain.Entities;
using Domain.Wrapper;
using MediatR;

namespace Core.Features.FeatureName.Commands;

internal class FeatureNameQueryHandler : BaseQueryHandler
{
    #region Constructor

    public FeatureNameQueryHandler(UltimateRedditBotDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    #endregion

    #region Methods

    #endregion
}