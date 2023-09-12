﻿global using Booking.ApplicationCore.DTO;
global using Booking.ApplicationCore.Exceptions;
global using Booking.ApplicationCore.Interfaces.IBackgroundJobs;
global using Booking.ApplicationCore.Interfaces.IGrpcService;
global using Booking.ApplicationCore.Interfaces.IService;
global using Booking.ApplicationCore.Services;
global using Booking.Domain.Abstractions.IRepository;
global using Booking.Domain.Entities;
global using MentorPlatform.Shared.MassTransitEvents;
global using MentorPlatform.Tests.UnitTests.Booking.API.BogusData;
global using MentorPlatform.Tests.UnitTests.Booking.API.Helpers.Bookings;
global using MentorPlatform.Tests.UnitTests.Booking.API.Helpers.Students;
global using Booking.Infrastructure.BackGroundJobs;
global using MentorPlatform.Shared.MessageBus;
global using Booking.API.Controllers;
global using Booking.ApplicationCore.Validators;