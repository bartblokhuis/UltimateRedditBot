using AutoMapper;
using Database;

namespace Core.Features.Common
{
    public abstract class BaseCommandHandler
    {
        #region Fields

        protected readonly UltimateRedditBotDbContext _context;
        protected readonly IMapper _mapper;

        #endregion

        #region Constructor

        public BaseCommandHandler(UltimateRedditBotDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #endregion
    }
}
