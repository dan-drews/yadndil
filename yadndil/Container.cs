using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("Yadndil.Tests")]
namespace Yadndil
{
    public class Container
    {

        internal Dictionary<Type, Type> _mapping = new Dictionary<Type, Type>();

        private bool _hasFinishedRegistering = false;

        public void Register<TInterface, TImplementation>() where TImplementation : TInterface
        {
            if (!typeof(TInterface).IsInterface)
                throw new ArgumentException($"{nameof(TInterface)} must be an interface.");

            _mapping.Add(typeof(TInterface), typeof(TImplementation));
        }

        public TInterface Get<TInterface>() where TInterface : class
        {
            if (!typeof(TInterface).IsInterface)
                throw new ArgumentException($"{nameof(TInterface)} must be an interface.");

            if (!_mapping.ContainsKey(typeof(TInterface)))
                throw new Exception("That type is not registered");

            if (!_hasFinishedRegistering)
                throw new Exception($"You must first call {nameof(FinishRegistering)}() before getting a type");

            return null;
        }

        public void FinishRegistering()
        {
            _hasFinishedRegistering = true;
        }
    }
}
