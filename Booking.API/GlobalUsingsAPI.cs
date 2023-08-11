﻿global using Microsoft.AspNetCore.Mvc;
global using Booking.ApplicationCore.DTO;
global using Booking.ApplicationCore.Interfaces.IService;
global using Booking.Domain.Entities;
global using Booking.ApplicationCore.Interfaces.IGrpcService;
global using Booking.ApplicationCore.Mapper;
global using Booking.ApplicationCore.Services;
global using Booking.Domain.Abstractions.IRepository;
global using Booking.Infrastructure.Data;
global using Booking.Infrastructure.Repositories;
global using Microsoft.EntityFrameworkCore;
global using Serilog;
global using Booking.ApplicationCore.Exceptions;
global using System.Net;
global using Booking.API.Middlewares;
global using Booking.ApplicationCore.Validators;
global using FluentValidation;
global using FluentValidation.AspNetCore;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.IdentityModel.Protocols.OpenIdConnect;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Models;
global using System.Reflection;
global using System.Security.Cryptography.X509Certificates;
global using Booking.API.Configuration;
global using Booking.Infrastructure.Options;
global using Booking.Infrastracture.Protos;
global using Booking.Infrastructure.Services.GrpcServices;
global using Booking.Infrastructure.Mapper;
global using Booking.Infrastructure.Consumer;
global using Booking.Infrastructure.MessageBroker;
global using MassTransit;
global using MentorPlatform.Shared.MessageBus;
global using Microsoft.Extensions.Options;
global using Serilog.Exceptions;
global using Serilog.Sinks.Elasticsearch;
global using Hangfire;
global using HangfireBasicAuthenticationFilter;
global using Booking.ApplicationCore.Interfaces.IBackgroundJobs;
global using Booking.Infrastructure.BackGroundJobs;
global using Hangfire.SqlServer;