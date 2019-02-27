using Moq;
using NUnit.Framework;
using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.Purchases;
using WizardsCastle.Logic.Situations;
using WizardsCastle.Logic.Tests.Helpers;

namespace WizardsCastle.Logic.Tests.Situations
{
    [TestFixture]
    public class PurchaseSituationTests
    {
        private ISituation _situation;
        private MockGameTools _tools;
        private GameData _data;
        private Mock<IPurchaseChoice> _purchaseChoice;
        private IPurchaseChoice[] _availableOptions;
        private ISituation _next;

        [SetUp]
        public void Setup()
        {
            _next = Mock.Of<ISituation>();
            _availableOptions = new[] {Mock.Of<IPurchaseChoice>(), Mock.Of<IPurchaseChoice>()};
            _situation = new SituationBuilder().Purchase(_availableOptions, _next);

            _tools = new MockGameTools();
            _data = Any.GameData();

            _purchaseChoice = new Mock<IPurchaseChoice>();
        }

        [Test]
        public void ReturnNextSituationIfPlayerMakesNoPurchase()
        {
            IPurchaseChoice empty = null;
            _tools.PurchaseUIMock.Setup(p => p.OfferPurchaseOptions(_availableOptions, out empty)).Returns(false);

            var actual = _situation.PlayThrough(_data, _tools);

            Assert.That(actual, Is.SameAs(_next));
        }

        [Test]
        public void NotifiesInsufficientFundsIfPlayerChoosesExpensiveOption()
        {
            IPurchaseChoice chosen = _purchaseChoice.Object;
            _tools.PurchaseUIMock.Setup(p => p.OfferPurchaseOptions(_availableOptions, out chosen)).Returns(true);
            _purchaseChoice.Setup(c => c.Cost).Returns(_data.Player.GoldPieces + Any.Number());

            var actual = _situation.PlayThrough(_data, _tools);

            _tools.PurchaseUIMock.Verify(ui=>ui.NotifyInsufficientFunds(), Times.Once());
            Assert.That(actual, Is.SameAs(_situation));
        }

        [Test]
        public void ExecutesChosenPurchase()
        {
            IPurchaseChoice chosen = _purchaseChoice.Object;
            _tools.PurchaseUIMock.Setup(p => p.OfferPurchaseOptions(_availableOptions, out chosen)).Returns(true);
            _purchaseChoice.Setup(c => c.Cost).Returns(_data.Player.GoldPieces);

            var actual = _situation.PlayThrough(_data, _tools);

            _tools.PurchaseUIMock.Verify(ui=>ui.NotifyInsufficientFunds(), Times.Never);
            _purchaseChoice.Verify(c=>c.Apply(_data.Player), Times.Once);
            Assert.That(actual, Is.SameAs(_situation));
            Assert.That(_data.Player.GoldPieces, Is.EqualTo(0));
        }
    }
}