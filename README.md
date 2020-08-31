Workshop Context 
----------------

In this exercise we’re going to perform a lightweight modernization on a fictitious full framework ASP.NET MVC application who's front-end consumes a WebAPI backend. The application is presumed to run on IIS today using standard virtual or physical infrastructure and our task is to migrate the application to Cloud Foundry. Our modernization efforts will involve building the necessary artifacts to get the application to run at all in the cloud and to allow the application to scale once it gets there.  

All the while we’ll try to use a lightweight approach and make as few changes to application code as possible.  

Current State 
----------------

Our sample application for this working is a single solution containing two projects. The first project is a WebAPI project exposing a single endpoint. The second project is an ASP.NET MVC front-end that consumes the WebAPI endpoint from JavaScript. 

The application is designed with an IIS topology in mind with both applications sharing a common hostname, the frontend responding to the root, and the backend living in a virtual directory located at the path “/api”. 

The frontend exposes an endpoint that tracks session-level view counts and is configured to use “InProc” session state. 

Desired State: 
----------------

Our goals are  

    Get this application operational on Cloud Foundry  

    Ensure the application is scalable  

    Use solutions that are minimally invasive to the code base 

Exercise 1: Push the application and try to scale (WIP @Chris)
----------------
 
Create a manifest for the backend in the root of its directory.

```
---
applications:
- stack: windows
  instances: 1
  buildpacks: 
    - hwc_buildpack
  routes:
    - route: <host-name.domain-name>/api
```    

Push the service to cloud foundry

```
cf push <api-app-name>
```

Create a manifest for the web frontend in the root of its directory.

```
---
applications:
- stack: windows
  instances: 1
  buildpacks: 
    - hwc_buildpack
```

Push the web app to cloud foundry

```
cf push <web-app-name>
```

See the app work

```
cf scale -i 3 <app-name> 
```

See the app fail
Buildpacks:  @Brian 
----------------
A `buildpack` is an artifact that configures or provides your app's hosting environment.  For example, the `HWC_BUILDPACK` provides the Hostable Web Core for IIS app support.

Buildpacks come in two flavors:
1. A `supply builpack` helps bootstrap your app's dependencies for startup.
1. A `final buildpack` is responsible for launching your app

Buildpacks provide a series of scripts for the platform to invoke during buildpack detection and execution phases.  These scripts are referred to as `hooks`.

#### Lifecycle

A `buildpack` is run by the platform prior to app instance startup if:
1. The buildpack's `detect` hook returns true (implicit)
2. The buildpack is specified in the manifest (explicit)

The platform invokes hooks provided by the buildpack:
1. `supply`
    * Dependency injection, environment setup.
2. `finalize`
    * Provide app startup concerns (webserver, etc.)
3. `release`
    * Provide metadata to control app startup - out of scope for today.

The platform invokes buildpack hooks depending on buildpack type:
1. A `supply builpack` must provide a `supply` hook.
1. A `final buildpack` must provide a `finalize` hook.

#### Composability
Multi-buildpack allows us to compose an app startup pipeline where `supply buildpacks` are invoked in the order provided prior to the `final buildpack`, which starts the app.
* Analogous to Middleware concept in WebAPI

Multi-buildpack can be specified in yml:

```yml
---
applications:
- stack: windows
  instances: 1
  buildpacks:
    - first_supply_buildpack
    - second_supply_buildpack
    - final_buildpack
```

Or, via command line:

```CLI
cf push APP-NAME -b FIRST-BUILDPACK -b SECOND-BUILDPACK -b FINAL-BUILDPACK
```

Exercise 2 (WIP @Chris)
----------

Create the shared redis service to store session state 

```
cf create-service p-redis shared-vm session 
```

Bind the application to redis

```
---
applications:
- stack: windows
  instances: 3
  buildpacks:
    - hwc_buildpack
  services:
    - session
```

Add the redis session buildpack

```
---
applications:
- stack: windows
  instances: 3
  buildpacks:
    - https://github.com/cloudfoundry-community/redis-session-aspnet-buildpack/releases/download/v1.0.5/Pivotal.Redis.Aspnet.Session.Buildpack-win-x64-1.0.5.zip 
    - hwc_buildpack
  services:
    - session
```

Push the web app to cloud foundry

```
cf push <web-app-name>
```
