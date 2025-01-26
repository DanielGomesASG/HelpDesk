using DanielASG.HelpDesk.Auditing;
using DanielASG.HelpDesk.Test.Base;
using Shouldly;
using Xunit;

namespace DanielASG.HelpDesk.Tests.Auditing
{
    // ReSharper disable once InconsistentNaming
    public class NamespaceStripper_Tests: AppTestBase
    {
        private readonly INamespaceStripper _namespaceStripper;

        public NamespaceStripper_Tests()
        {
            _namespaceStripper = Resolve<INamespaceStripper>();
        }

        [Fact]
        public void Should_Stripe_Namespace()
        {
            var controllerName = _namespaceStripper.StripNameSpace("DanielASG.HelpDesk.Web.Controllers.HomeController");
            controllerName.ShouldBe("HomeController");
        }

        [Theory]
        [InlineData("DanielASG.HelpDesk.Auditing.GenericEntityService`1[[DanielASG.HelpDesk.Storage.BinaryObject, DanielASG.HelpDesk.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null]]", "GenericEntityService<BinaryObject>")]
        [InlineData("CompanyName.ProductName.Services.Base.EntityService`6[[CompanyName.ProductName.Entity.Book, CompanyName.ProductName.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null],[CompanyName.ProductName.Services.Dto.Book.CreateInput, N...", "EntityService<Book, CreateInput>")]
        [InlineData("DanielASG.HelpDesk.Auditing.XEntityService`1[DanielASG.HelpDesk.Auditing.AService`5[[DanielASG.HelpDesk.Storage.BinaryObject, DanielASG.HelpDesk.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null],[DanielASG.HelpDesk.Storage.TestObject, DanielASG.HelpDesk.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null],]]", "XEntityService<AService<BinaryObject, TestObject>>")]
        public void Should_Stripe_Generic_Namespace(string serviceName, string result)
        {
            var genericServiceName = _namespaceStripper.StripNameSpace(serviceName);
            genericServiceName.ShouldBe(result);
        }
    }
}
