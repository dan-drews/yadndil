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
            Assert.Throws<ArgumentException>(() => {
                container.Register<TypesForTests.ContainerTestTypes.ClassForContainerTestsWithEmptyConstructor, TypesForTests.ContainerTestTypes.ClassForContainerTestsWithEmptyConstructor>();
            });
        }

        [Test]
        public void Container_Register_AddsMappingToList()
        {
            var container = new Yadndil.Container();
            container.Register<TypesForTests.ContainerTestTypes.IInterfaceForContainerTests, TypesForTests.ContainerTestTypes.ClassForContainerTestsWithEmptyConstructor>();
            Assert.AreEqual(1, container._mapping.Count);
        }

        [Test]
        public void Container_Get_ExceptionWhenFirstParameterIsNotInterface()
        {
            var container = new Yadndil.Container();
            Assert.Throws<ArgumentException>(() => {
                container.Get<TypesForTests.ContainerTestTypes.ClassForContainerTestsWithEmptyConstructor>();
            });
        }

        [Test]
        public void Container_Get_ExceptionWhenNotMapped()
        {
            var container = new Yadndil.Container();
            Assert.Throws<Exception>(() => {
                container.Get<TypesForTests.ContainerTestTypes.IInterfaceForContainerTests>();
            });
        }

        [Test]
        public void Container_Get_ThrowsExceptionIfFinishRegistereingHasNotBeenCalled()
        {
            var container = new Yadndil.Container();
            container.Register<TypesForTests.ContainerTestTypes.IInterfaceForContainerTests, TypesForTests.ContainerTestTypes.ClassForContainerTestsWithEmptyConstructor>();
            Assert.Throws<Exception>(() => {
                container.Get<TypesForTests.ContainerTestTypes.IInterfaceForContainerTests>();
            });
        }

        [Test]
        public void Container_Get_ReturnsImplementationWhenConstructorIsEmpty()
        {
            var container = new Yadndil.Container();
            container.Register<TypesForTests.ContainerTestTypes.IInterfaceForContainerTests, TypesForTests.ContainerTestTypes.ClassForContainerTestsWithEmptyConstructor>();
            container.FinishRegistering();
        }
    }
}