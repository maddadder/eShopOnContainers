﻿var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddSignalR(builder.Configuration);

builder.Services.AddTransient<IIntegrationEventHandler<OrderStatusChangedToAwaitingValidationIntegrationEvent>, OrderStatusChangedToAwaitingValidationIntegrationEventHandler>();
builder.Services.AddTransient<IIntegrationEventHandler<OrderStatusChangedToCancelledIntegrationEvent>, OrderStatusChangedToCancelledIntegrationEventHandler>();
builder.Services.AddTransient<IIntegrationEventHandler<OrderStatusChangedToPaidIntegrationEvent>, OrderStatusChangedToPaidIntegrationEventHandler>();
builder.Services.AddTransient<IIntegrationEventHandler<OrderStatusChangedToShippedIntegrationEvent>, OrderStatusChangedToShippedIntegrationEventHandler>();
builder.Services.AddTransient<IIntegrationEventHandler<OrderStatusChangedToStockConfirmedIntegrationEvent>, OrderStatusChangedToStockConfirmedIntegrationEventHandler>();
builder.Services.AddTransient<IIntegrationEventHandler<OrderStatusChangedToSubmittedIntegrationEvent>, OrderStatusChangedToSubmittedIntegrationEventHandler>();

var app = builder.Build();

app.UseServiceDefaults();

app.MapHub<NotificationsHub>("/hub/notificationhub");

var eventBus = app.Services.GetRequiredService<IEventBus>();

eventBus.Subscribe<OrderStatusChangedToAwaitingValidationIntegrationEvent, IIntegrationEventHandler<OrderStatusChangedToAwaitingValidationIntegrationEvent>>();
eventBus.Subscribe<OrderStatusChangedToPaidIntegrationEvent, IIntegrationEventHandler<OrderStatusChangedToPaidIntegrationEvent>>();
eventBus.Subscribe<OrderStatusChangedToStockConfirmedIntegrationEvent, IIntegrationEventHandler<OrderStatusChangedToStockConfirmedIntegrationEvent>>();
eventBus.Subscribe<OrderStatusChangedToShippedIntegrationEvent, IIntegrationEventHandler<OrderStatusChangedToShippedIntegrationEvent>>();
eventBus.Subscribe<OrderStatusChangedToCancelledIntegrationEvent, IIntegrationEventHandler<OrderStatusChangedToCancelledIntegrationEvent>>();
eventBus.Subscribe<OrderStatusChangedToSubmittedIntegrationEvent, IIntegrationEventHandler<OrderStatusChangedToSubmittedIntegrationEvent>>();

await app.RunAsync();
