using AutoMapper;
using Core.Features.Common;
using Database;
using Domain.Dtos;
using Domain.Entities;
using Domain.Wrapper;
using MediatR;

namespace Core.Features.FeatureName.Commands;

internal class FeatureNameCommandHandler : BaseCommandHandler
{
    #region Constructor

    public FeatureNameCommandHandler(UltimateRedditBotDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    #endregion

    #region Methods

    #endregion
}