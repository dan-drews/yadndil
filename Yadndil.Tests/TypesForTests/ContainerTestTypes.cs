using System;
using System.Collections.Generic;
using System.Text;

namespace Yadndil.Tests.TypesForTests
{
    namespace ContainerTestTypes
    {
        public interface IInterfaceForContainerTestsWithEmptyConstructor
        {
            string PrintSomething();
        }

        public class ClassForContainerTestsWithEmptyConstructor : IInterfaceForContainerTestsWithEmptyConstructor
        {
            public ClassForContainerTestsWithEmptyConstructor()
            {

            }

            public string PrintSomething()
            {
                return "Something";
            }
        }

        public interface IInterfaceForContainerTestsWithNonEmptyConstructor
        {
            string PrintSomething();
        }

        public class ClassForContainerTestsWithNonEmptyConstructor : IInterfaceForContainerTestsWithNonEmptyConstructor
        {
            private IInterfaceForContainerTestsWithEmptyConstructor _withEmptyConstructor;
            public ClassForContainerTestsWithNonEmptyConstructor(IInterfaceForContainerTestsWithEmptyConstructor withEmptyConstructor)
            {
                _withEmptyConstructor = withEmptyConstructor;
            }

            public string PrintSomething()
            {
                return $"Child: {_withEmptyConstructor.PrintSomething()}";
            }
        }

        public interface IInterafce1ForStackOverflow
        {

        }

        public interface IInterafce2ForStackOverflow
        {

        }

        public class Class1ForStackOverflow : IInterafce1ForStackOverflow
        {

            private IInterafce2ForStackOverflow _interafce2ForStackOverflow;

            public Class1ForStackOverflow(IInterafce2ForStackOverflow interafce2ForStackOverflow)
            {

            }
        }

        public class Class2ForStackOverflow : IInterafce2ForStackOverflow
        {

            private IInterafce1ForStackOverflow _interafce2ForStackOverflow;

            public Class2ForStackOverflow(IInterafce1ForStackOverflow interafce1ForStackOverflow)
            {

            }
        }
    }
}
