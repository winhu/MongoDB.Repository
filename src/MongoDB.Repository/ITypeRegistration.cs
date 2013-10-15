using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB.Repository
{
    public interface ITypeRegistration
    {
        /// <summary>
        /// register database
        /// </summary>
        /// <param name="dbName">database name</param>
        /// <returns></returns>
        ITypeRegistration RegisterDatabase(string dbName);

        /// <summary>
        /// register collection type
        /// </summary>
        /// <typeparam name="T">collection type</typeparam>
        /// <returns></returns>
        ITypeRegistration RegisterType<T>();
        /// <summary>
        /// register collection type
        /// </summary>
        /// <param name="entityType">collection type</param>
        /// <returns></returns>
        ITypeRegistration RegisterType(Type entityType);

        /// <summary>
        /// unregister collection type
        /// </summary>
        /// <typeparam name="T">collection type</typeparam>
        /// <returns></returns>
        ITypeRegistration UnRegisterType<T>();
        /// <summary>
        /// unregister collection type
        /// </summary>
        /// <param name="entityType">collection type</param>
        /// <returns></returns>
        ITypeRegistration UnRegisterType(Type entityType);

        /// <summary>
        /// is collection type registered
        /// </summary>
        /// <typeparam name="T">collection type</typeparam>
        /// <returns></returns>
        bool IsRegisterType<T>();
        /// <summary>
        /// is collection type registered
        /// </summary>
        /// <param name="entityType">collection type</param>
        /// <returns></returns>
        bool IsRegisterType(Type entityType);

        /// <summary>
        /// get all registered collection types
        /// </summary>
        /// <returns></returns>
        List<Type> GetRegisterTypes();
    }
}
