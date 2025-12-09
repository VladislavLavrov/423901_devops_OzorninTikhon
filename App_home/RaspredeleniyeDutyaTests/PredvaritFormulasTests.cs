using RaspredeleniyeDutyaFormulas;

namespace RaspredeleniyeDutyaTests
{
    [TestClass]
    public class PredvaritFormulasTests
    {
        InitialData data;

        [TestInitialize]
        public void Init() => data = new InitialData();

        [TestMethod]
        public void TeploemkKislorodTest()
            => Assert.AreEqual(1.6246, PredvaritFormulas.TeploemkKislorod(data), 0.01);

        [TestMethod]
        public void TeploemkAzotTest()
            => Assert.AreEqual(1.5474, PredvaritFormulas.TeploemkAzot(data), 0.01);

        [TestMethod]
        public void TeploemkDvuhatomTest()
            => Assert.AreEqual(1.4327, PredvaritFormulas.TeploemkDvuhatom(data), 0.01);

        [TestMethod]
        public void TeploemkParVodaTest()
            => Assert.AreEqual(1.7893, PredvaritFormulas.TeploemkParVoda(data), 0.01);

        [TestMethod]
        public void DutRashodPerCTest()
            => Assert.AreEqual(3.4371, PredvaritFormulas.DutRashodPerC(data), 0.01);

        [TestMethod]
        public void DutRashodPerGazTest()
            => Assert.AreEqual(1.8414, PredvaritFormulas.DutRashodPerGaz(data), 0.01);

        [TestMethod]
        public void GornGazPerCTest()
            => Assert.AreEqual(4.5103, PredvaritFormulas.GornGazPerC(data), 0.01);

        [TestMethod]
        public void GornGazPerGazTest()
            => Assert.AreEqual(4.4163, PredvaritFormulas.GornGazPerGaz(data), 0.01);

        [TestMethod]
        public void TeplosodDutTest()
            => Assert.AreEqual(1516.8589, PredvaritFormulas.TeplosodDut(data), 0.01);

        [TestMethod]
        public void TeplosodKoksTest()
            => Assert.AreEqual(2475.0, PredvaritFormulas.TeplosodKoks(data), 0.01);

        [TestMethod]
        public void TeploemkDutTest()
            => Assert.AreEqual(1.5668, PredvaritFormulas.TeploemkDut(data), 0.01);
    }
}