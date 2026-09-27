using Notification.Application.Features.NotificationJobs.Dtos;
using Notification.Application.Providers;
using Notification.Domain.Entities;
using Microsoft.Extensions.Logging;
using Notification.Domain.Constants;
using Notification.Application.Abstractions;

namespace Notification.Application.Features.NotificationJobs.Commands;

public sealed record CreateNotificationJobsCommand(
    IReadOnlyCollection<CreateNotificationJobDto> Dtos)
    : ICommand;

public sealed class CreateNotificationJobsCommandValidator
    : AbstractValidator<CreateNotificationJobsCommand>
{
    public CreateNotificationJobsCommandValidator()
    {
        RuleFor(x => x.Dtos)
            .NotNull()
            .WithMessage(NotificationErrorCode.BadRequest);

        RuleForEach(x => x.Dtos)
            .ChildRules(dto =>
            {
                dto.RuleFor(x => x.EventId)
                    .NotEmpty()
                    .WithMessage(NotificationErrorCode.EventIdIsRequired);

                dto.RuleFor(x => x.TemplateKey)
                    .NotEmpty()
                    .WithMessage(NotificationErrorCode.TemplateKeyIsRequired);

                dto.RuleFor(x => x.Recipient)
                    .NotEmpty()
                    .WithMessage(NotificationErrorCode.AtLeastOneRecipientIsRequired);

                dto.RuleFor(x => x.MaxAttempts)
                    .GreaterThan(0)
                    .WithMessage(NotificationErrorCode.MaxAttemptsMustBeGreaterThanZero);
            });
    }
}

public sealed class CreateNotificationJobCommandHandler(
    IUnitOfWork unitOfWork,
    ITemplateProvider templateProvider,
    ILogger<CreateNotificationJobCommandHandler> logger)
    : ICommandHandler<CreateNotificationJobsCommand>
{
    public async Task Handle(CreateNotificationJobsCommand command,
        CancellationToken ct)
    {

        foreach (var dto in command.Dtos)
        {
            var template = await unitOfWork.NotificationTemplates
                .GetTemplateAsync(
                    dto.TemplateKey,
                    dto.ChannelType,
                    ct);

            if (template is null)
                throw new NotFoundException(NotificationErrorCode.TemplateNotFound);

            var variables = dto.TemplateVariables ?? [];

            var job = NotificationJobEntity.Create(
                dto.ChannelType,
                dto.UserId,
                dto.Recipient,
                templateProvider.Render(template.Subject!, variables),
                template.IsHtml,
                templateProvider.Render(template.Body!, variables),
                dto.Priority,
                dto.EventId,
                dto.MaxAttempts,
                dto.TargetUrl);

            unitOfWork.NotificationJobs.Add(job);
        }

        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation(
            "All notification jobs created. EventId={EventId}", command.Dtos.First().EventId);
    }
}

