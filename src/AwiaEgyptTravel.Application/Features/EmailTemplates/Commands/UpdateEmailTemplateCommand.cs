using AutoMapper;
using AwiaEgyptTravel.Application.Common.DTOs;
using AwiaEgyptTravel.Application.Common.Models;
using AwiaEgyptTravel.Core.Entities;
using AwiaEgyptTravel.Core.Interfaces;
using MediatR;

namespace AwiaEgyptTravel.Application.Features.EmailTemplates.Commands
{
    public class UpdateEmailTemplateCommand : IRequest<BaseResponse<EmailTemplateDto>>
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsDefault { get; set; }
    }

    public class UpdateEmailTemplateCommandHandler : IRequestHandler<UpdateEmailTemplateCommand, BaseResponse<EmailTemplateDto>>
    {
        private readonly IEmailTemplateRepository _emailTemplateRepository;
        private readonly IMapper _mapper;

        public UpdateEmailTemplateCommandHandler(IEmailTemplateRepository emailTemplateRepository, IMapper mapper)
        {
            _emailTemplateRepository = emailTemplateRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<EmailTemplateDto>> Handle(UpdateEmailTemplateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var template = await _emailTemplateRepository.GetByIdAsync(request.Id);
                if (template == null)
                {
                    return BaseResponse<EmailTemplateDto>.FailureResponse("Email template not found");
                }

                template.Subject = request.Subject;
                template.Body = request.Body;
                template.IsDefault = request.IsDefault;

                await _emailTemplateRepository.UpdateAsync(template);

                var templateDto = _mapper.Map<EmailTemplateDto>(template);
                return BaseResponse<EmailTemplateDto>.SuccessResponse(templateDto, "Email template updated successfully");
            }
            catch (Exception ex)
            {
                return BaseResponse<EmailTemplateDto>.FailureResponse(ex.Message);
            }
        }
    }
}
