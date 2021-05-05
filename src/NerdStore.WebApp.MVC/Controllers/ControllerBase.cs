using MediatR;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Core.Communication.Mediatr;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NerdStore.WebApp.MVC.Controllers
{
    public abstract class ControllerBase : Controller
    {
        private readonly DomainNotificationHandler _notifications;
        private readonly IMediatrHandler _mediatrHandler;
        
        protected Guid CustomerId = Guid.Parse("4885e451-b0e4-4490-b959-04fabc806d32");

        protected ControllerBase(INotificationHandler<DomainNotification> notifications,
                                 IMediatrHandler mediatrHandler)
        {
            _mediatrHandler = mediatrHandler;
            _notifications = (DomainNotificationHandler) notifications;
        }

        protected bool IsOperationValid()
        {
            return !_notifications.HasNotification();
        }

        protected IEnumerable<string> GetErrorMessages()
        {
            return _notifications.GetNotifications().Select(x => x.Value).ToList();
        }

        protected void PublishError(string code, string message)
        {
            _mediatrHandler.PublishNotification(new DomainNotification(code, message));
        }
    }
}
