open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Authentication.Cookies
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Giraffe
open DotNetEnv
open Core
open Infrastructure.Core
open Http.Handlers

// ---------------------------------
// Web app
// ---------------------------------

let webApp =
    choose [
        subRoute "/api"
            (choose [
                POST >=> route "/login" >=> AuthHandlers.login
                POST >=> route "/logout" >=> AuthHandlers.logout
                subRoute "/users"
                    (choose [
                        GET  >=> route "" >=> UserHandlers.index
                        GET  >=> route "/me" >=> UserHandlers.show
                        POST >=> route "" >=> UserHandlers.store
                        PUT >=> route "/me" >=> UserHandlers.update
                        PATCH >=> route "/me/credentials" >=> UserHandlers.updateCredentials
                        DELETE >=> routef "/%i" UserHandlers.delete
                    ])
            ])
        setStatusCode 404 >=> negotiate {| message = "Not Found" |}
    ]

// ---------------------------------
// Config and Main
// ---------------------------------

let configureCors (builder : CorsPolicyBuilder) =
    builder
        .WithOrigins(
            "http://localhost:5000",
            "https://localhost:5001")
       .AllowAnyMethod()
       .AllowAnyHeader()
       |> ignore

let configureApp (app : IApplicationBuilder) =
    let env = app.ApplicationServices.GetService<IWebHostEnvironment>()

    app.UseAuthentication() |> ignore

    (match env.IsDevelopment() with
    | true  ->
        app.UseDeveloperExceptionPage()
    | false ->
        app .UseGiraffeErrorHandler(ErrorHandlers.mainHandler)
            .UseHttpsRedirection())
        .UseCors(configureCors)
        .UseGiraffe(webApp)

let configureServices (services : IServiceCollection) =
    services.AddCors()    |> ignore
    services.AddGiraffe() |> ignore
    services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(fun options ->
            options.Cookie.Name <- Configs.App.name
            options.Cookie.HttpOnly <- true
            options.Cookie.SameSite <- SameSiteMode.Strict
            options.ExpireTimeSpan <- TimeSpan.FromHours 1
            options.SlidingExpiration <- true
        ) |> ignore

let configureLogging (builder : ILoggingBuilder) =
    builder.AddConsole()
           .AddDebug() |> ignore

[<EntryPoint>]
let main args =
    Env.Load() |> ignore

    Database.configure() |> ignore

    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(
            fun webHostBuilder ->
                webHostBuilder
                    .Configure(Action<IApplicationBuilder> configureApp)
                    .ConfigureServices(configureServices)
                    .ConfigureLogging(configureLogging)
                    |> ignore)
        .Build()
        .Run()
    0

