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
    - route: <hostname>/api
```    

Buildpacks:  @Brian 
----------------
 

 