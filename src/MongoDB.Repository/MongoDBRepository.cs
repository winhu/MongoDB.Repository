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
            Contexts = new List<IRegistrationContext>();
        }
        static readonly List<IRegistrationContext> Contexts;

        /// <summary>
        /// register IMongoDBContext if not exists
        /// </summary>
        /// <param name="dbContext"></param>
        public static void RegisterMongoDBContext(IMongoDBContext dbContext)
        {
            if (Contexts.Exists(c => c.Code == dbContext.GetType().FullName))
                return;

            IRegistrationContext context = new RegistrationContext();
            context.RegisterDBContext(dbContext);
            context.GetAllTypes().ForEach(delegate(Type t)
            {
                Contexts.ForEach(delegate(IRegistrationContext con)
                {
                    if (con.GetAllTypes().Exists(_t => _t.FullName == t.FullName))
                        throw new Exception(string.Format("Conflicted type {0} between {1} and {2}", t.FullName, con.Code, context.Code));
                });
            });
            Contexts.Add(context);
        }

        /// <summary>
        /// ensure mongo index by BsonIndexAttribute
        /// </summary>
        public static void RegisterMongoIndex()
        {
            foreach (var context in Contexts)
            {
                context.EnsureDBIndex();
            }
        }
        /// <summary>
        /// ensure mongo index by BsonIndexAttribute
        /// </summary>
        /// <param name="DBContextType">IMongoDBContext</param>
        /// <param name="entityType">collection type</param>
        public static void RegisterMongoIndex(Type DBContextType, Type entityType)
        {
            var context = Contexts.SingleOrDefault(c => c.Code == DBContextType.FullName);
            if (context == null) return;
            if (!context.IsRegisterType(entityType)) return;
            context.EnsureDBIndex(entityType);
        }

        /// <summary>
        /// get MongoUrl of type which first found
        /// </summary>
        /// <param name="type">collection type</param>
        /// <returns></returns>
        internal static MongoUrl GetConfig(Type type)
        {
            var context = Contexts.SingleOrDefault(c => c.IsRegisterType(type));
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
        /// <param name="dbContextType">IMongoDBContext</param>
        /// <param name="entityType">collection type</param>
        public static void RegisterType(Type dbContextType, Type entityType)
        {
            if (!Contexts.Exists(c => c.Code == dbContextType.FullName)) throw new MongoException("Unregisterd MongoDBContext");

            var context = Contexts.SingleOrDefault(c => c.Code == dbContextType.FullName);
            if (context == null) throw new MongoException("Unregisterd MongoDBContext");
            context.RegisterType(entityType);
            RegisterMongoIndex(dbContextType, entityType);
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
            return Contexts.Exists(c => c.IsRegisterType(type));
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
            Contexts.ForEach(delegate(IRegistrationContext context)
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
            Contexts.RemoveAll(registeration => registeration.Code == typeof(TDBContext).FullName);
        }

    }

}
