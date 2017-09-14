# PhotoSchool

### The project use:
- **Asp.Net Core 2.0**
- **MongoDb** 
- **Angular 2**
- **CSS Grid** 

## =====================================================

# Asp.Net Core 2.0 Server

More info for [The .NET Core command-line interface](https://docs.microsoft.com/en-us/dotnet/core/tools/)

## Restore dependencies

If you’re `VS 2017`, it will auto-restore the dependencies once the file is saved, otherwise navigate to `Server` folder run `dotnet restore`.

## Development server

Navigate to `Server` folder. Run `dotnet watch run` for a dev server. Server listening on:  `http://localhost:55551`. The `server` will automatically reload if you change any of the source files. But `client` needs manual refresh.

## Build

Navigate to `Server` folder. Run `dotnet build` to the project and its dependencies into a set of binaries.

## Release

Navigate to `Server` folder. Run `dotnet run --configuration Release` to the project and its dependencies into a set of binaries. The build artifacts will be stored in the `wwwroot/` directory for `client` and in the `bin/Debug/` for `Server`.

## Hosting environment

Set Development `setx ASPNETCORE_ENVIRONMENT "Development"`
Set Production `setx ASPNETCORE_ENVIRONMENT "Production"`


## =====================================================


# Angular 2 client

This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 1.2.4.

## Restore dependencies

Navigate to `Client` folder. Use `npm install`.
If you have installed `yarn`, can use `yarn`.

## Development server

Navigate to `Client` folder. Run `npm start` for a dev server. Navigate to `http://localhost:4200`. The app will automatically reload if you change any of the source files.

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|module`.

## Build

Navigate to `Client` folder. Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory. Use the `-prod` flag for a production build.

## Running unit tests

Navigate to `Client` folder. Run `ng test` to execute the unit tests via [Karma](https://karma-runner.github.io).

## Running end-to-end tests

Navigate to `Client` folder. Run `ng e2e` to execute the end-to-end tests via [Protractor](http://www.protractortest.org/).
Before running the tests make sure you are serving the app via `ng serve`.


## =====================================================


## Further help

To get more help on the Angular CLI use `ng help` or go check out the [Angular CLI README](https://github.com/angular/angular-cli/blob/master/README.md).

How to build an Angular Application with ASP.NET Core in Visual Studio 2017 [visualized](https://medium.com/@levifuller/building-an-angular-application-with-asp-net-core-in-visual-studio-2017-visualized-f4b163830eaa).