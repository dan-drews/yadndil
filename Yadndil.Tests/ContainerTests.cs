using NUnit.Framework;
using System;
using Yadndil;
using Yadndil.Tests.TypesForTests.ContainerTestTypes;

namespace Tests
{
    [TestFixture]
    public class ContainerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Register_ExceptionWhenFirstParameterIsNotInterface()
        {
            var container = new Container();
            Assert.Throws<ArgumentException>(() =>
            {
                container.Register<ClassForContainerTestsWithEmptyConstructor, ClassForContainerTestsWithEmptyConstructor>();
            });
        }


        [Test]
        public void Register_ThrowsWhenInterfaceHasAlreadyBeenAdded()
        {
            var container = new Container();
            container.Register<IInterfaceForContainerTestsWithEmptyConstructor, ClassForContainerTestsWithEmptyConstructor>();
            Assert.Throws<Exception>(() =>
            {
                container.Register<IInterfaceForContainerTestsWithEmptyConstructor, ClassForContainerTestsWithEmptyConstructor>();
            });
        }


        [Test]
        public void Register_AddsMappingToList()
        {
            var container = new Container();
            container.Register<IInterfaceForContainerTestsWithEmptyConstructor, ClassForContainerTestsWithEmptyConstructor>();
            Assert.AreEqual(1, container._mapping.Count);
        }

        [Test]
        public void Get_ExceptionWhenFirstParameterIsNotInterface()
        {
            var container = new Container();
            Assert.Throws<ArgumentException>(() =>
            {
                container.Get<ClassForContainerTestsWithEmptyConstructor>();
            });
        }

        [Test]
        public void Get_ExceptionWhenNotMapped()
        {
            var container = new Container();
            Assert.Throws<Exception>(() =>
            {
                container.Get<IInterfaceForContainerTestsWithEmptyConstructor>();
            });
        }

        [Test]
        public void Get_ReturnsImplementationWhenConstructorIsEmpty()
        {
            var container = new Container();
            container.Register<IInterfaceForContainerTestsWithEmptyConstructor, ClassForContainerTestsWithEmptyConstructor>();
            var result = container.Get<IInterfaceForContainerTestsWithEmptyConstructor>();
            Assert.IsInstanceOf<ClassForContainerTestsWithEmptyConstructor>(result);
        }

        [Test]
        public void Get_CanCallInterfaceMethod()
        {
            var container = new Container();
            container.Register<IInterfaceForContainerTestsWithEmptyConstructor, ClassForContainerTestsWithEmptyConstructor>();
            var result = container.Get<IInterfaceForContainerTestsWithEmptyConstructor>();
            Assert.AreEqual("Something", result.PrintSomething());
        }

        [Test]
        public void Get_ReturnsImplementationWhenConstructorIsNotEmpty()
        {
            var container = new Container();
            container.Register<IInterfaceForContainerTestsWithEmptyConstructor, ClassForContainerTestsWithEmptyConstructor>();
            container.Register<IInterfaceForContainerTestsWithNonEmptyConstructor, ClassForContainerTestsWithNonEmptyConstructor>();
            var result = container.Get<IInterfaceForContainerTestsWithNonEmptyConstructor>();
            Assert.IsInstanceOf<ClassForContainerTestsWithNonEmptyConstructor>(result);
        }

        [Test]
        public void Get_CanCallInterfaceMethodWhenConstructorIsNotEmpty()
        {
            var container = new Container();
            container.Register<IInterfaceForContainerTestsWithEmptyConstructor, ClassForContainerTestsWithEmptyConstructor>();
            container.Register<IInterfaceForContainerTestsWithNonEmptyConstructor, ClassForContainerTestsWithNonEmptyConstructor>();
            var result = container.Get<IInterfaceForContainerTestsWithNonEmptyConstructor>();
            Assert.AreEqual("Child: Something", result.PrintSomething());
        }

        [Test]
        public void FinishRegistering_CircularReferenceCausesStackOverflow()
        {
            var container = new Container();
            container.Register<IInterafce1ForStackOverflow, Class1ForStackOverflow>();
            container.Register<IInterafce2ForStackOverflow, Class2ForStackOverflow>();
            Assert.Throws<Exception>(() =>
            {
                container.Get<IInterfaceForContainerTestsWithNonEmptyConstructor>();
            });
        }

    }
}