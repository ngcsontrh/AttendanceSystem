using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.UseCases.TitleUC.Queries
{
    public class GetListTitleQueryHandler
    {
        private readonly ITitleRepository _titleRepository;
        private readonly ILogger<GetListTitleQueryHandler> _logger;

        public GetListTitleQueryHandler(
            ITitleRepository titleRepository,
            ILogger<GetListTitleQueryHandler> logger)
        {
            _titleRepository = titleRepository;
            _logger = logger;
        }

        public async Task<GetListTitleResponse> Handle()
        {
            var titles = await _titleRepository.GetListAsync(x => true);
            var response = MapToResponse(titles);

            _logger.LogInformation("Retrieved {Count} titles", titles.Count);
            return response;
        }

        private GetListTitleResponse MapToResponse(IReadOnlyList<Title> titles)
        {
            var data = titles.Select(x => new GetListTitleResponseData(
                x.Id,
                x.Name,
                x.Description)).ToList();
            return new GetListTitleResponse(data);
        }
    }
}
