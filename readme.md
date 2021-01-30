# OAuth

Welcome! This project is designed to be a simple demonstration of how to secure webpages and APIs using the following technologies:
- ASP.Net Identity
- IdentityServer4
- OAuth

## SampleAPI
The `SampleAPI` project is located at the root level of the project and is a basic web API that is used by the other demo projects. For the other projects to work correctly this should be run locally on port 5003.

## ClientCredentials

Inside the `ClientCredentials` subfolder there is a `SampleAPI.OAuth.ClientCredentials` project that demonstrates how to set up a basic identity server using IdentityServer4 which authenticates access to the `SampleAPI` project. The `SampleAPI.OAuth.ClientCredentials` project should be run locally on port 5001.

There are two command line projects that demonstrate making authenticated calls to the API. The first - `SampleClient.ClientCredentials` - shows how to do this directly by reading request headers, and the second - `IS4.ClientCredentials` - shows how to do this using the `IdentityModel` package.

## UI 'in-memory' Server
Inside the `OAuth.UI.InMemory` subfolder there are two projects.

`SampleWebApp` is a simple web application that requires authorization. This application also makes calls to the `SampleAPI` project.

`OAuth.UI.InMemory` is a basic identity server using IdentityServer4 with a UI component that allows users to log in and authenticate their access to the `SampleWebApp` and `SampleAPI` projects.

The projects should be run locally on the following ports:
- `OAuth.UI.InMemory` - 5001
- `SampleWebApp` - 5002
- `SampleAPI` - 5003
