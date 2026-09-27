using Notification.Application.Models;
using Microsoft.Extensions.Logging;
using Notification.Application.Strategy;
using Notification.Domain.Constants;
using Notification.Application.Abstractions;

namespace Notification.Application.Features.NotificationJobs.Commands;

public sealed record ProcessNotificationJobCommand(Guid NotificationJobId) : ICommand<bool>;

public sealed class ProcessNotificationJobCommandValidator : AbstractValidator<ProcessNotificationJobCommand>
{
    public ProcessNotificationJobCommandValidator()
    {
        RuleFor(x => x.NotificationJobId)
            .NotEmpty()
            .WithMessage(NotificationErrorCode.IdIsRequired);
    }
}

public sealed class ProcessNotificationJobCommandHandler(
    IUnitOfWork unitOfWork,
    INotificationSenderResolver resolver,
    ILogger<ProcessNotificationJobCommandHandler> logger)
    : ICommandHandler<ProcessNotificationJobCommand, bool>
{
    public async Task<bool> Handle(ProcessNotificationJobCommand command, CancellationToken ct)
    {
        var job = await unitOfWork.NotificationJobs.GetJobByIdAsync(command.NotificationJobId, ct);

        if (job == null)
            throw new NotFoundException(NotificationErrorCode.NotificationJobNotFound);

        try
        {
            var now = DateTimeOffset.UtcNow;

            var ctx = new NotificationContext
            {
                Recipient = job.Payload.Recipient,
                Subject = job.Payload.Subject,
                Body = job.Payload.Body,
                IsHtml = job.Payload.IsHtml,
                UserId = job.Payload.UserId,
                TargetUrl = job.Payload.TargetUrl
            };

            var sender = resolver.Resolve(job.Payload.Channel);

            var result = await sender.SendAsync(ctx, ct);

            if (result.IsSuccess)
            {
                job.MarkSent();

                logger.LogInformation(
                    "NotificationJobId={NotificationJobId} sent successfully", job.Id);
            }
            else
            {
                job.Fail(result.ErrorMessage!, now);

                logger.LogWarning(
                    "NotificationJobId={NotificationJobId} failed: {Error}", job.Id, result.ErrorMessage);
            }

            await unitOfWork.SaveChangesAsync(ct);

            return result.IsSuccess;
        }
        catch (Exception ex)
        {
            var now = DateTimeOffset.UtcNow;

            job.Fail(ex.Message, now);

            await unitOfWork.SaveChangesAsync(ct);

            logger.LogError(
                ex,
                "Unhandled error occurred while processing NotificationJobId={NotificationJobId}",
                job.Id);

            return false;
        }
    }
}

