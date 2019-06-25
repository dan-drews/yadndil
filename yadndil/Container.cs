using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("Yadndil.Tests")]
namespace Yadndil
{
    public class Container
    {
        // All mappings
        internal Dictionary<Type, Type> _mapping = new Dictionary<Type, Type>();

        // Mappings that do not have concrete implementations constructed yet.
        internal Dictionary<Type, Type> _mappingsWithoutImplementations = new Dictionary<Type, Type>();

        // Singleton implementations
        private Dictionary<Type, object> _implementations = new Dictionary<Type, object>();

        // Used to detect recursion.
        private HashSet<Type> _typesBeingChecked = new HashSet<Type>();

        // Tracks whether registering is complete since it's a recursive process.
        private bool _hasFinishedRegistering = false;

        /// <summary>
        /// Register a mapping from interface to concrete type.
        /// </summary>
        /// <typeparam name="TInterface">Interface being registered</typeparam>
        /// <typeparam name="TImplementation">Type for the mapping</typeparam>
        public void Register<TInterface, TImplementation>() where TImplementation : TInterface
        {
            if (!typeof(TInterface).IsInterface)
                throw new ArgumentException($"{nameof(TInterface)} must be an interface.");

            if (_mapping.ContainsKey(typeof(TInterface)))
                throw new Exception("That interface is already mapped to a class");

            // If there is an empty constructor on the implementation, I will go ahead and just construct it now.
            if (HasEmptyConstructor(typeof(TImplementation)))
            {
                var implementation = System.Activator.CreateInstance<TImplementation>();
                _implementations.Add(typeof(TInterface), implementation);
            }
            else // No empty constructor, so I'll add it to the list of implementations that need to be added.
            {
                _mappingsWithoutImplementations.Add(typeof(TInterface), typeof(TImplementation));
            }

            _mapping.Add(typeof(TInterface), typeof(TImplementation));

        }

        public TInterface Get<TInterface>() where TInterface : class // Get the implementation
        {
            if (!typeof(TInterface).IsInterface)
                throw new ArgumentException($"{nameof(TInterface)} must be an interface.");

            if (!_mapping.ContainsKey(typeof(TInterface)))
                throw new Exception("That type is not registered");

            if (!_hasFinishedRegistering) // Not done registering, so go ahead and complete this process and generate implementations
                FinishRegistering();

            return (TInterface)_implementations[typeof(TInterface)]; // Get the singleton from the lookup
        }

        private void FinishRegistering()
        {
            while (_mappingsWithoutImplementations.Count != 0)
            {
                RecurseConstructorDependencies(_mappingsWithoutImplementations.Keys.First()); // Begin recursing through types
            }
            _hasFinishedRegistering = true;
        }

        private object RecurseConstructorDependencies(Type @interface)
        {
            // Prevent circular reference which will crash app and instead throw exception (which will crash app)
            if (_typesBeingChecked.Contains(@interface))
                throw new Exception("There seems to be a circular reference in your dependency injection. Whoops.");

            _typesBeingChecked.Add(@interface);

            // We have an implementation, so let's just return that one.
            if (_implementations.ContainsKey(@interface))
            {
                RemoveInterfaceFromMappingsWithoutImplementations(@interface);
                return _implementations[@interface];
            }

            // Get the type from the dictionary of mappings.
            var implementation = _mapping[@interface];

            // If it has an empty constructor, go ahead and add the implementation to the implementation dictionary and construct it empty.
            if (HasEmptyConstructor(implementation))
            {
                RemoveInterfaceFromMappingsWithoutImplementations(@interface);
                var result = System.Activator.CreateInstance(implementation);
                _implementations.Add(@interface, result);
                RemoveInterfaceFromMappingsWithoutImplementations(@interface);
                return result;
            }

            // Grab the first constructor. If there's more than one, too bad, so sad.
            var constructor = implementation.GetConstructors().FirstOrDefault();
            if (constructor != null)
            {
                // Get a list of parameters to the constructor
                var paramList = constructor.GetParameters();

                var paramArray = new List<Object>();

                if (paramList.Any())
                {
                    foreach (var param in paramList)
                    {
                        // Recursively construct dependencies in the constructor for this type.
                        paramArray.Add(RecurseConstructorDependencies(param.ParameterType));
                    }
                }
                var result = constructor.Invoke(paramArray.ToArray()); // Generate this type
                _implementations.Add(@interface, result); 
                RemoveInterfaceFromMappingsWithoutImplementations(@interface);
                return result;
            }
            else
            {
                throw new Exception("Somehow, this does not have any public constructors.");
            }
        }

        // Remove this from the list of what needs to be constructed still
        private void RemoveInterfaceFromMappingsWithoutImplementations(Type @interface)
        {
            if (_mappingsWithoutImplementations.ContainsKey(@interface))
            {
                _mappingsWithoutImplementations.Remove(@interface);
            }
            _typesBeingChecked.Remove(@interface);
        }

        private static bool HasEmptyConstructor(Type t)
        {
            return GetEmptyConstructor(t) != null;
        }

        private static System.Reflection.ConstructorInfo GetEmptyConstructor(Type t)
        {
            return t.GetConstructor(Type.EmptyTypes);
        }
    }
}
