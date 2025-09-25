using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.UseCases.TitleUC.Commands
{
    public class CreateTitleCommandHandler
    {
        private readonly ITitleRepository _titleRepository;
        private readonly ILogger<CreateTitleCommandHandler> _logger;

        public CreateTitleCommandHandler(ITitleRepository titleRepository, ILogger<CreateTitleCommandHandler> logger)
        {
            _titleRepository = titleRepository;
            _logger = logger;
        }

        public async Task<CreateTitleResponse> HandleAsync(CreateTitleCommand command)
        {
            try
            {
                var newTitle = new Title
                {
                    Id = Guid.CreateVersion7(),
                    Name = command.Name,
                    Description = command.Description,
                    CreatedAt = DateTime.UtcNow
                };
                _titleRepository.Add(newTitle);
                await _titleRepository.SaveChangesAsync();

                _logger.LogInformation("Created new title with ID: {TitleId}", newTitle.Id);
                return new CreateTitleResponse(newTitle.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new title.");
                throw;
            }            
        }
    }
}
