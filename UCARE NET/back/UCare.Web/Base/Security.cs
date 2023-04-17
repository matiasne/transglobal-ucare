using GQ.Core.service;
using GQ.Core.utils;
using GQ.Security.Internal;
using GQ.Security.Jwt;
using System.Reflection;
using System.Xml.Serialization;
using UCare.Application.Users;
using UCare.Shared.Domain.Auth;

namespace UCare.Web.Base
{
    public static class Security
    {
        private static Dictionary<string, Type> StringToType = new Dictionary<string, Type>();

        private static void AddStringToType(Type item)
        {
            var controllerNameFull = ClassUtils.GetNameObject(item);
            StringToType.Add(controllerNameFull, item);

            var index = controllerNameFull.LastIndexOf('.') + 1;
            controllerNameFull = controllerNameFull.Substring(index, controllerNameFull.Length - index).Replace("Controller", "");
            StringToType.Add(controllerNameFull, item);
        }

        private static object TypeMethodSecurityDescriptionLock = new object();
        private static Dictionary<Type, Dictionary<string, SecurityDescriptionAttribute>> TypeMethodSecurityDescription = new Dictionary<Type, Dictionary<string, SecurityDescriptionAttribute>>();

        private static SecurityDescriptionAttribute GetTypeMethodSecurityDescription(Type type, string method)
        {
            lock (TypeMethodSecurityDescriptionLock)
            {
                if (!TypeMethodSecurityDescription.ContainsKey(type))
                {
                    TypeMethodSecurityDescription.Add(type, new Dictionary<string, SecurityDescriptionAttribute>());
                }
                if (!TypeMethodSecurityDescription[type].ContainsKey(method))
                {
                    SecurityDescriptionAttribute attribute = null;
                    var methodFind = ClassUtils.GetMethod(type, method);
                    if (methodFind.Length > 0)
                    {
                        attribute = (SecurityDescriptionAttribute)methodFind[0].GetCustomAttribute(typeof(SecurityDescriptionAttribute), true);
                    }
                    TypeMethodSecurityDescription[type].Add(method, attribute);
                }
            }
            return TypeMethodSecurityDescription[type][method];
        }

        private static Dictionary<Type, SecurityDescriptionAttribute> TypeSecurityDescription = new Dictionary<Type, SecurityDescriptionAttribute>();

        private static object TypeSecurityDescriptionLock = new object();

        private static SecurityDescriptionAttribute GetTypeSecurityDescription(Type type)
        {
            lock (TypeSecurityDescriptionLock)
            {
                if (!TypeSecurityDescription.ContainsKey(type))
                {
                    SecurityDescriptionAttribute attribute = (SecurityDescriptionAttribute)type.GetCustomAttribute(typeof(SecurityDescriptionAttribute), true);
                    TypeSecurityDescription.Add(type, attribute);
                }
            }
            return TypeSecurityDescription[type];
        }
        public static bool CheckSecurity(string controller, string action)
        {
            if (string.IsNullOrWhiteSpace(controller))
                return false;

            bool result = System.Web.HttpContext.Current.User?.Identity?.IsAuthenticated ?? false;

            Type objectType = StringToType[controller];

            if (CheckWebUniqueSession())
            {
                SecurityDescriptionAttribute attribute = GetTypeSecurityDescription(objectType);

                if (attribute != null && !string.IsNullOrWhiteSpace(attribute.SecurityName))
                {
                    result &= attribute.SecurityPerfiles.Any(x => SecurityJwt.Usuario<AuthUser>().Rol == x.ToString());
                }

                if (result && action != null)
                {
                    attribute = GetTypeMethodSecurityDescription(objectType, action);
                    if (attribute != null && !string.IsNullOrWhiteSpace(attribute.SecurityName))
                    {
                        result &= attribute.SecurityPerfiles.Any(x => SecurityJwt.Usuario<AuthUser>().Rol == x.ToString());
                    }
                }
                else
                {
                    result = false;
                }

                return result;
            }
            else
                return false;
        }

        private static IUniqueSession? uniqueSession = null;
        private static bool CheckWebUniqueSession()
        {
            if (uniqueSession == null)
            {
                using var scope = ServicesContainer.CreateScope();
                uniqueSession = scope.ServiceProvider.GetService<IUniqueSession>();
            }

            return uniqueSession!.CheckUser(SecurityJwt.Usuario<AuthUser>());
        }

        public static void CreateAccessSecurity(UsuarioManagerCrudApp crudApp, Assembly assembly)
        {
            crudApp.CreateUsuarioPropietario();

            var elementsType = (from iface in assembly.GetTypes()
                                where !iface.IsAbstract && iface.Namespace != null && iface.Namespace.Contains("Controller")
                                select iface);

            SecurityDescriptionAttribute attribute;

            foreach (Type item in elementsType)
            {
                attribute = (SecurityDescriptionAttribute)item.GetCustomAttribute(typeof(SecurityDescriptionAttribute), true);

                if (attribute != null)
                {
                    AddStringToType(item);
                    foreach (MethodInfo method in item.GetMethods())
                    {
                        attribute = (SecurityDescriptionAttribute)method.GetCustomAttribute(typeof(SecurityDescriptionAttribute), true);
                        if (attribute != null)
                        {
                        }
                    }
                }
            }
        }
    }
}
