﻿global using FluentValidation;
global using FluentValidation.AspNetCore;
global using Mentors.API.Middlewares;
global using Mentors.ApplicationCore.DTO;
global using Mentors.ApplicationCore.Interfaces.IService;
global using Mentors.ApplicationCore.Mapper;
global using Mentors.ApplicationCore.Services;
global using Mentors.ApplicationCore.Validators;
global using Mentors.Domain.Abstractions.IRepository;
global using Mentors.Infrastructure.Repositories;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.IdentityModel.Protocols.OpenIdConnect;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Models;
global using Serilog;
global using System.Reflection;
global using System.Security.Cryptography.X509Certificates;
global using Mentors.API.Configuration;
global using Mentors.Infrastructure.Data;
global using Microsoft.EntityFrameworkCore;
global using Mentors.Domain.Entities;
global using Microsoft.AspNetCore.Mvc;
global using Mentors.ApplicationCore.Exceptions;
global using System.Net;
global using AutoMapper;
global using Grpc.Core;
global using Mentors.API.Protos;
global using Google.Protobuf.WellKnownTypes;
global using Mentors.API.Mapper;
global using Mentors.API.Services.GrpcService;
global using MassTransit;
global using MentorPlatform.Shared.MessageBus;
global using Mentors.Infrastructure.Consumer;
global using Mentors.Infrastructure.MessageBroker;
global using Microsoft.Extensions.Options;
global using Serilog.Exceptions;
global using Serilog.Sinks.Elasticsearch;
global using Mentors.ApplicationCore.Interfaces.IMongoService;
global using Mentors.ApplicationCore.Services.MongoServices;
global using Mentors.Domain.Abstractions.IRepository.IMongoRepository;
global using Mentors.Infrastructure.MongoDb;
global using Mentors.Infrastructure.Repositories.MongoRepository;
global using Mentors.Domain.Entities.MongoDb;