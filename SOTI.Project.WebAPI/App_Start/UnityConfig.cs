using System.Web.Http;
using Unity;
using Unity.WebApi;
using Xebia.Project.DataAccessLayer;

namespace SOTI.Project.WebAPI
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IProduct, ProductDetailsConnect>();
            container.RegisterType<ICustomer, CustomerDetails>();
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}