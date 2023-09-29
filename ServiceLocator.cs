using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracks
{
    // Singleton implementation from here: https://csharpindepth.com/articles/singleton
    internal class ServiceLocator
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private static readonly ServiceLocator _instance = new ServiceLocator();

        static ServiceLocator()
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
        }

        public static ServiceLocator Instance
        {
            get
            {
                return _instance;
            }
        }

        private Dictionary<string, object> Services { get; set; } = new Dictionary<string, object>();

        private ServiceLocator()
        {
            // Singleton
        }

        public void ProvideService<T>(object service) where T : class
        {
            string typeName = typeof(T).Name;
            ProvideService(typeName, service);
        }

        public void ProvideService<T>(T service) where T : class
        {
            string typeName = typeof(T).Name;
            ProvideService(typeName, service);
        }

        public void ProvideService(string key, object service)
        {
            if (Services.ContainsKey(key))
            {
                Logger.Info($"Replacing existing service for key: '{key}'");
            }
            else
            {
                Logger.Info($"Adding service for key: '{key}'");
            }

            Services[key] = service;
        }

        public T GetService<T>() where T : class
        {
            string typeName = typeof(T).Name;
            return GetService<T>(typeName);
        }

        public T GetService<T>(string key) where T : class
        {
            if (!Services.TryGetValue(key, out object service))
            {
                throw new Exception($"Unable to locate service for '{key}'");
            }

            return (T)service;
        }
    }
}
