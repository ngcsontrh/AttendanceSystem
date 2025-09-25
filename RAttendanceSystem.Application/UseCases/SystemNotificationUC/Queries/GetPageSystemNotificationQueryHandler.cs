using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.UseCases.SystemNotificationUC.Queries
{
    public class GetPageSystemNotificationQueryHandler
    {
        private readonly ISystemNotificationRepository _systemNotificationRepository;
        private readonly ILogger<GetPageSystemNotificationQueryHandler> _logger;

        public GetPageSystemNotificationQueryHandler(
            ISystemNotificationRepository systemNotificationRepository,
            ILogger<GetPageSystemNotificationQueryHandler> logger)
        {
            _systemNotificationRepository = systemNotificationRepository;
            _logger = logger;
        }

        public async Task<GetPageSystemNotificationResponse> HandleAsync(GetPageSystemNotificationQuery model)
        {
            try
            {
                int skip = (model.PageNumber - 1) * model.PageSize;
                int take = model.PageSize;
                var spec = new SystemNotificationFilterSpecificationBuilder()
                    .WithReceiverId(model.ReceiverId)
                    .WithOrderByDescending(x => x.CreatedAt)
                    .WithPaging(skip, take)
                    .Build();

                var result = await _systemNotificationRepository.GetPageAsync(spec);

                return MapToResponse(result.Item1, result.Item2);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting paged system notifications.");
                throw;
            }
        }

        private GetPageSystemNotificationResponse MapToResponse(IReadOnlyList<SystemNotification> notifications, int total)
        {
            var data = notifications.Select(x => new GetPageSystemNotificationResponseData(
                x.Id,
                x.Title,
                x.Content,
                x.CreatedAt)).ToList();
            return new GetPageSystemNotificationResponse(data, total);
        }
    }
}
