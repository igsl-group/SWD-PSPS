﻿using Psps.Core.Configuration;
using Psps.Core.Infrastructure.DependencyManagement;
using System;

namespace Psps.Core.Infrastructure
{
    /// <summary>
    /// Classes implementing this interface can serve as a portal for the
    /// various services composing the Nop engine. Edit functionality, modules
    /// and implementations access most Nop functionality through this
    /// interface.
    /// </summary>
    public interface IEngine
    {
        /// <summary>
        /// Container manager
        /// </summary>
        ContainerManager ContainerManager { get; }

        /// <summary>
        /// Initialize components and plugins in the Psps environment.
        /// </summary>
        /// <param name="config">Config</param>
        void Initialize(PspsConfig config);

        /// <summary>
        /// Resolve dependency
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns></returns>
        T Resolve<T>() where T : class;

        /// <summary>
        ///  Resolve dependency
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns></returns>
        object Resolve(Type type);

        /// <summary>
        /// Resolve dependencies
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns></returns>
        T[] ResolveAll<T>();
    }
}