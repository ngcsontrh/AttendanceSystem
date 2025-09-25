using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.UseCases.TitleUC.Commands
{
    public class UpdateTitleCommandHandler
    {
        private readonly ITitleRepository _titleRepository;
        private readonly ILogger<UpdateTitleCommandHandler> _logger;

        public UpdateTitleCommandHandler(
            ITitleRepository titleRepository,
            ILogger<UpdateTitleCommandHandler> logger)
        {
            _titleRepository = titleRepository;
            _logger = logger;
        }

        public async Task HandleAsync(UpdateTitleCommand command)
        {
            try
            {
                var title = await _titleRepository.GetRecordAsync(command.Id);
                if (title == null)
                {
                    _logger.LogWarning("Title with ID {TitleId} not found.", command.Id);
                    throw new RecordNotFoundException($"Title with ID {command.Id} not found.");
                }

                MapToEntity(title, command);
                await _titleRepository.SaveChangesAsync();

                _logger.LogInformation("Title with ID {TitleId} updated successfully.", command.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating Title with ID {TitleId}.", command.Id);
                throw;
            }            
        }

        void MapToEntity(Title entity, UpdateTitleCommand command)
        {
            entity.Name = command.Name;
            entity.Description = command.Description;
        }
    }
}
