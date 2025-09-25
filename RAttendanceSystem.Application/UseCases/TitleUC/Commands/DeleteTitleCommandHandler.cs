using System;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.UseCases.TitleUC.Commands
{
    public class DeleteTitleCommandHandler
    {
        private readonly ITitleRepository _titleRepository;
        private readonly ILogger<DeleteTitleCommandHandler> _logger;

        public DeleteTitleCommandHandler(ITitleRepository titleRepository, ILogger<DeleteTitleCommandHandler> logger)
        {
            _titleRepository = titleRepository;
            _logger = logger;
        }

        public async Task HandleAsync(DeleteTitleCommand command)
        {
            try
            {
                var title = await _titleRepository.GetRecordAsync(command.Id);
                if (title == null)
                {
                    _logger.LogWarning("Title with ID {TitleId} not found.", command.Id);
                    throw new RecordNotFoundException($"Title with ID {command.Id} not found.");
                }
                _titleRepository.Delete(title);
                await _titleRepository.SaveChangesAsync();
                _logger.LogInformation("Title with ID {TitleId} deleted successfully.", command.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting Title with ID {TitleId}.", command.Id);
                throw;
            }
        }
    }
}
