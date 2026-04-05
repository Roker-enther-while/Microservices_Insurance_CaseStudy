using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using jsreport.Client;
using jsreport.Types;
using MediatR;
using Microsoft.Extensions.Logging;
using PolicyService.Api.Events;

namespace DocumentService.Api.Listeners;

public class PolicyCreatedHandler : INotificationHandler<PolicyCreated>
{
    private readonly IReportingService renderService;
    private readonly ILogger<PolicyCreatedHandler> logger;

    public PolicyCreatedHandler(IReportingService renderService, ILogger<PolicyCreatedHandler> logger)
    {
        this.renderService = renderService;
        this.logger = logger;
    }

    public async Task Handle(PolicyCreated notification, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Generating certificate for policy {PolicyNumber}", notification.PolicyNumber);

            var report = await renderService.RenderAsync(new RenderRequest
            {
                Template = new Template
                {
                    Content = "<h1>Insurance Policy Certificate</h1>" +
                              "<p>Policy Number: {{PolicyNumber}}</p>" +
                              "<p>Product: {{ProductCode}}</p>" +
                              "<p>Holder: {{PolicyHolder.FirstName}} {{PolicyHolder.LastName}}</p>" +
                              "<p>Premium: {{TotalPremium}}</p>" +
                              "<p>From: {{PolicyFrom}} To: {{PolicyTo}}</p>",
                    Recipe = Recipe.ChromePdf,
                    Engine = Engine.Handlebars
                },
                Data = notification
            });

            var fileName = $"Policy_{notification.PolicyNumber}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Certs", fileName);

            using (var fileStream = File.Create(filePath))
            {
                await report.Content.CopyToAsync(fileStream);
            }

            logger.LogInformation("Certificate generated and saved to {FilePath}", filePath);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error generating certificate for policy {PolicyNumber}", notification.PolicyNumber);
        }
    }
}
