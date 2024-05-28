using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace IDS.Web.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();

            if (exc != null)
            {
                // Handle HTTP errors
                if (exc.GetType() == typeof(HttpException))
                {
                    // The Complete Error Handling Example generates
                    // some errors using URLs with "NoCatch" in them;
                    // ignore these here to simulate what would happen
                    // if a global.asax handler were not implemented.
                    if (exc.Message.Contains("NoCatch") || exc.Message.Contains("maxUrlLength"))
                        return;

                    //Redirect HTTP errors to HttpError page
                    //Server.Transfer("HttpErrorPage.aspx");
                }

                // For other kinds of errors give the user some information
                // but stay on the default page
                Response.Write("<h2>Global Page Error</h2>\n");
                Response.Write(
                    "<p>" + exc.Message + "</p>\n");
                Response.Write("Return to the <a href='Default.aspx'>" +
                    "Default Page</a>\n");

                // Log the exception and notify system operators
                ExceptionUtility.LogException(exc, "DefaultPage");
                ExceptionUtility.NotifySystemOps(exc);
            }

            // Clear the error from the server
            Server.ClearError();
        }

        //Application_Init: Fired when an application initializes or is first called.It's invoked for all HttpApplication object instances.

        //Application_Disposed: Fired just before an application is destroyed.This is the ideal location for cleaning up previously used resources.

        //Application_Error: Fired when an unhandled exception is encountered within the application.

        //Application_Start: Fired when the first instance of the HttpApplication class is created.It allows you to create objects that are accessible by all HttpApplication instances.

        //Application_End: Fired when the last instance of an HttpApplication class is destroyed.It's fired only once during an application's lifetime.

        //Application_BeginRequest: Fired when an application request is received.It's the first event fired for a request, which is often a page request (URL) that a user enters.


        //Application_EndRequest: The last event fired for an application request.

        //Application_PreRequestHandlerExecute: Fired before the ASP.NET page framework begins executing an event handler like a page or Web service.

        //Application_PostRequestHandlerExecute: Fired when the ASP.NET page framework is finished executing an event handler.

        //Applcation_PreSendRequestHeaders: Fired before the ASP.NET page framework sends HTTP headers to a requesting client (browser).

        //Application_PreSendContent: Fired before the ASP.NET page framework sends content to a requesting client (browser).

        //Application_AcquireRequestState: Fired when the ASP.NET page framework gets the current state (Session state) related to the current request.

        //Application_ReleaseRequestState: Fired when the ASP.NET page framework completes execution of all event handlers.This results in all state modules to save their current state data.

        //Application_ResolveRequestCache: Fired when the ASP.NET page framework completes an authorization request. It allows caching modules to serve the request from the cache, thus bypassing handler execution.


        //Application_UpdateRequestCache: Fired when the ASP.NET page framework completes handler execution to allow caching modules to store responses to be used to handle subsequent requests.


        //Application_AuthenticateRequest: Fired when the security module has established the current user's identity as valid. At this point, the user's credentials have been validated.

        //Application_AuthorizeRequest: Fired when the security module has verified that a user can access resources.

        //Session_Start: Fired when a new user visits the application Web site.

        //Session_End: Fired when a user's session times out, ends, or they leave the application Web site.
    }
}