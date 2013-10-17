using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoDB.Repository
{
    /// <summary>
    /// MongoDBRepository
    /// </summary>
    public class MongoDBRepository
    {
        static MongoDBRepository()
        {
            contexts = new List<IRegistrationContext>();
        }
        static List<IRegistrationContext> contexts;

        /// <summary>
        /// register IMongoDBContext if not exists
        /// </summary>
        /// <param name="dbContext"></param>
        public static void RegisterMongoDBContext(IMongoDBContext dbContext)
        {
            if (contexts.Exists(c => c.Code == dbContext.GetType().FullName))
                return;

            IRegistrationContext context = new RegistrationContext();
            context.RegisterDBContext(dbContext);
            contexts.Add(context);
        }

        /// <summary>
        /// get MongoUrl of type which first found
        /// </summary>
        /// <param name="type">collection type</param>
        /// <returns></returns>
        internal static MongoUrl GetConfig(Type type)
        {
            var context = contexts.SingleOrDefault(c => c.IsRegisterType(type));
            if (context == null) return null;
            return context.GetMongoUrl();
        }

        /// <summary>
        /// get MongoUrl of type which first found
        /// </summary>
        /// <typeparam name="T">collection type</typeparam>
        /// <returns></returns>
        internal static MongoUrl GetConfig<T>()
        {
            return GetConfig(typeof(T));
        }
        /// <summary>
        /// register collection type for IMongoDBContext
        /// </summary>
        /// <param name="DBContextType">IMongoDBContext</param>
        /// <param name="EntityType">collection type</param>
        public static void RegisterType(Type DBContextType, Type EntityType)
        {
            if (!contexts.Exists(c => c.Code == DBContextType.FullName)) throw new MongoException("Unregisterd MongoDBContext");

            var context = contexts.SingleOrDefault(c => c.Code == DBContextType.FullName);
            if (context == null) throw new MongoException("Unregisterd MongoDBContext");
            context.RegisterType(EntityType);
        }
        /// <summary>
        /// register collection type for IMongoDBContext
        /// </summary>
        /// <typeparam name="DBContextType">IMongoDBContext</typeparam>
        /// <typeparam name="EntityType">collection type</typeparam>
        public static void RegisterType<DBContextType, EntityType>()
        {
            RegisterType(typeof(DBContextType), typeof(EntityType));
        }
        /// <summary>
        /// is register collection type
        /// </summary>
        /// <param name="type">collection type</param>
        /// <returns></returns>
        public static bool IsRegisterType(Type type)
        {
            return contexts.Exists(c => c.IsRegisterType(type));
        }
        /// <summary>
        /// is register collection type
        /// </summary>
        /// <typeparam name="T">collection type</typeparam>
        /// <returns></returns>
        public static bool IsRegisterType<T>()
        {
            return IsRegisterType(typeof(T));
        }
        /// <summary>
        /// unregister collection type
        /// </summary>
        /// <param name="type">collection type</param>
        public static void UnregisterType(Type type)
        {
            contexts.ForEach(delegate(IRegistrationContext context)
            {
                context.UnregisterType(type);
            });
        }
        /// <summary>
        /// unregister collection type
        /// </summary>
        /// <typeparam name="T">collection type</typeparam>
        public static void UnregisterType<T>()
        {
            UnregisterType(typeof(T));
        }

        public static void UnregisterDBContext<TDBContext>() where TDBContext : IMongoDBContext
        {
            contexts.RemoveAll(registeration => registeration.Code == typeof(TDBContext).FullName);
        }

    }

}
