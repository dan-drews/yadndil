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

        internal Dictionary<Type, Type> _mapping = new Dictionary<Type, Type>();
        internal Dictionary<Type, Type> _mappingsWithoutImplementations = new Dictionary<Type, Type>();
        private Dictionary<Type, object> _implementations = new Dictionary<Type, object>();
        private HashSet<Type> _typesBeingChecked = new HashSet<Type>();

        private bool _hasFinishedRegistering = false;

        public void Register<TInterface, TImplementation>() where TImplementation : TInterface
        {
            if (!typeof(TInterface).IsInterface)
                throw new ArgumentException($"{nameof(TInterface)} must be an interface.");

            if (_mapping.ContainsKey(typeof(TInterface)))
                throw new Exception("That interface is already mapped to a class");

            if (HasEmptyConstructor(typeof(TImplementation)))
            {
                var implementation = System.Activator.CreateInstance<TImplementation>();
                _implementations.Add(typeof(TInterface), implementation);
            }
            else
            {
                _mappingsWithoutImplementations.Add(typeof(TInterface), typeof(TImplementation));
            }

            _mapping.Add(typeof(TInterface), typeof(TImplementation));

        }

        public TInterface Get<TInterface>() where TInterface : class
        {
            if (!typeof(TInterface).IsInterface)
                throw new ArgumentException($"{nameof(TInterface)} must be an interface.");

            if (!_mapping.ContainsKey(typeof(TInterface)))
                throw new Exception("That type is not registered");

            if (!_hasFinishedRegistering)
                FinishRegistering();

            return (TInterface)_implementations[typeof(TInterface)];
        }

        private void FinishRegistering()
        {
            _hasFinishedRegistering = true;
            while(_mappingsWithoutImplementations.Count != 0)
            {
                RecurseConstructorDependencies(_mappingsWithoutImplementations.Keys.First());
            }
        }

        private object RecurseConstructorDependencies(Type @interface)
        {

            if(_typesBeingChecked.Contains(@interface))
                throw new Exception("There seems to be a circular reference in your dependency injection. Whoops.");

            _typesBeingChecked.Add(@interface);

            if (_implementations.ContainsKey(@interface))
            {
                RemoveInterfaceFromMappingsWithoutImplementations(@interface);
                return _implementations[@interface];
            }

            var implementation = _mapping[@interface];
            if (HasEmptyConstructor(implementation))
            {
                RemoveInterfaceFromMappingsWithoutImplementations(@interface);
                var result = System.Activator.CreateInstance(implementation);
                _implementations.Add(@interface, result);
                RemoveInterfaceFromMappingsWithoutImplementations(@interface);
                return result;
            }

            var constructor = implementation.GetConstructors().FirstOrDefault();
            if (constructor != null)
            {
                var paramList = constructor.GetParameters();

                var paramArray = new List<Object>();

                if (paramList.Any())
                {
                    foreach (var param in paramList)
                    {
                        paramArray.Add(RecurseConstructorDependencies(param.ParameterType));
                    }
                }
                var result = constructor.Invoke(paramArray.ToArray());
                _implementations.Add(@interface, result);
                RemoveInterfaceFromMappingsWithoutImplementations(@interface);
                return result;
            }
            else
            {
                throw new Exception("Somehow, this does not have any public constructors.");
            }
        }

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
