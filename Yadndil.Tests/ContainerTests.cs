using NUnit.Framework;
using System;

namespace Yadndil.Tests
{
    public class ContainerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Container_Register_ExceptionWhenFirstParameterIsNotInterface()
        {
            var container = new Yadndil.Container();
            Assert.Throws<ArgumentException>(() =>
            {
                container.Register<TypesForTests.ContainerTestTypes.ClassForContainerTestsWithEmptyConstructor, TypesForTests.ContainerTestTypes.ClassForContainerTestsWithEmptyConstructor>();
            });
        }


        [Test]
        public void Container_Register_ThrowsWhenInterfaceHasAlreadyBeenAdded()
        {
            var container = new Yadndil.Container();
            Assert.Throws<Exception>(() =>
            {
                container.Register<TypesForTests.ContainerTestTypes.IInterfaceForContainerTestsWithEmptyConstructor, TypesForTests.ContainerTestTypes.ClassForContainerTestsWithEmptyConstructor>();
                container.Register<TypesForTests.ContainerTestTypes.IInterfaceForContainerTestsWithEmptyConstructor, TypesForTests.ContainerTestTypes.ClassForContainerTestsWithEmptyConstructor>();
            });
        }


        [Test]
        public void Container_Register_AddsMappingToList()
        {
            var container = new Yadndil.Container();
            container.Register<TypesForTests.ContainerTestTypes.IInterfaceForContainerTestsWithEmptyConstructor, TypesForTests.ContainerTestTypes.ClassForContainerTestsWithEmptyConstructor>();
            Assert.AreEqual(1, container._mapping.Count);
        }

        [Test]
        public void Container_Get_ExceptionWhenFirstParameterIsNotInterface()
        {
            var container = new Yadndil.Container();
            Assert.Throws<ArgumentException>(() =>
            {
                container.Get<TypesForTests.ContainerTestTypes.ClassForContainerTestsWithEmptyConstructor>();
            });
        }

        [Test]
        public void Container_Get_ExceptionWhenNotMapped()
        {
            var container = new Yadndil.Container();
            Assert.Throws<Exception>(() =>
            {
                container.Get<TypesForTests.ContainerTestTypes.IInterfaceForContainerTestsWithEmptyConstructor>();
            });
        }

        [Test]
        public void Container_Get_ReturnsImplementationWhenConstructorIsEmpty()
        {
            var container = new Yadndil.Container();
            container.Register<TypesForTests.ContainerTestTypes.IInterfaceForContainerTestsWithEmptyConstructor, TypesForTests.ContainerTestTypes.ClassForContainerTestsWithEmptyConstructor>();
            var result = container.Get<TypesForTests.ContainerTestTypes.IInterfaceForContainerTestsWithEmptyConstructor>();
            Assert.IsInstanceOf<TypesForTests.ContainerTestTypes.ClassForContainerTestsWithEmptyConstructor>(result);
        }

        [Test]
        public void Container_Get_CanCallInterfaceMethod()
        {
            var container = new Yadndil.Container();
            container.Register<TypesForTests.ContainerTestTypes.IInterfaceForContainerTestsWithEmptyConstructor, TypesForTests.ContainerTestTypes.ClassForContainerTestsWithEmptyConstructor>();
            var result = container.Get<TypesForTests.ContainerTestTypes.IInterfaceForContainerTestsWithEmptyConstructor>();
            Assert.AreEqual("Hi!", result.PrintSomething());
        }

        [Test]
        public void Container_Get_ReturnsImplementationWhenConstructorIsNotEmpty()
        {
            var container = new Yadndil.Container();
            container.Register<TypesForTests.ContainerTestTypes.IInterfaceForContainerTestsWithEmptyConstructor, TypesForTests.ContainerTestTypes.ClassForContainerTestsWithEmptyConstructor>();
            container.Register<TypesForTests.ContainerTestTypes.IInterfaceForContainerTestsWithNonEmptyConstructor, TypesForTests.ContainerTestTypes.ClassForContainerTestsWithNonEmptyConstructor>();
            var result = container.Get<TypesForTests.ContainerTestTypes.IInterfaceForContainerTestsWithNonEmptyConstructor>();
            Assert.IsInstanceOf<TypesForTests.ContainerTestTypes.ClassForContainerTestsWithNonEmptyConstructor>(result);
        }

        [Test]
        public void Container_Get_CanCallInterfaceMethodWhenConstructorIsNotEmpty()
        {
            var container = new Yadndil.Container();
            container.Register<TypesForTests.ContainerTestTypes.IInterfaceForContainerTestsWithEmptyConstructor, TypesForTests.ContainerTestTypes.ClassForContainerTestsWithEmptyConstructor>();
            container.Register<TypesForTests.ContainerTestTypes.IInterfaceForContainerTestsWithNonEmptyConstructor, TypesForTests.ContainerTestTypes.ClassForContainerTestsWithNonEmptyConstructor>();
            var result = container.Get<TypesForTests.ContainerTestTypes.IInterfaceForContainerTestsWithNonEmptyConstructor>();
            Assert.AreEqual("Child: Hi!", result.PrintSomething());
        }

        [Test]
        public void Container_FinishRegistering_CircularReferenceCausesStackOverflow()
        {
            var container = new Yadndil.Container();
            Assert.Throws<Exception>(() =>
            {
                container.Register<TypesForTests.ContainerTestTypes.IInterafce1ForStackOverflow, TypesForTests.ContainerTestTypes.Class1ForStackOverflow>();
                container.Register<TypesForTests.ContainerTestTypes.IInterafce2ForStackOverflow, TypesForTests.ContainerTestTypes.Class2ForStackOverflow>();
                container.Get<TypesForTests.ContainerTestTypes.IInterfaceForContainerTestsWithNonEmptyConstructor>();
            });
        }

    }
}