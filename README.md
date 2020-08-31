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

Exercise 1: Push the application and try to scale 
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
 
Exercise 2
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
