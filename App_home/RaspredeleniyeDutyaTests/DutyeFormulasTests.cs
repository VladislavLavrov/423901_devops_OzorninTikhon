using RaspredeleniyeDutyaFormulas;

namespace RaspredeleniyeDutyaTests
{
    [TestClass]
    public class DutyeFormulasTests
    {
        InitialData data;

        [TestInitialize]
        public void Init() => data = new InitialData();

        [TestMethod]
        public void SrZnachTest()
            => Assert.AreEqual(912.7414, DutyeFormulas.SrZnach(data, (data, furm) => data.RashGazNaF[furm]), 0.01);

        [TestMethod]
        public void KolTeplaTest()
            => Assert.AreEqual(149.0693, DutyeFormulas.KolTepla(data, 0), 0.01);

        [TestMethod]
        public void FactRashDutTest()
            => Assert.AreEqual(123.3227, DutyeFormulas.FactRashDut(data, 0), 0.01);

        [TestMethod]
        public void RasOtnosTest()
            => Assert.AreEqual(1.0205, DutyeFormulas.RasOtnos(data, 0), 0.01);

        [TestMethod]
        public void KolUglerTest()
            => Assert.AreEqual(33.1915, DutyeFormulas.KolUgler(data, 0), 0.01);

        [TestMethod]
        public void RashPGNaKGTest()
            => Assert.AreEqual(0.4562, DutyeFormulas.RashPGNaKG(data, 0), 0.01);

        [TestMethod]
        public void TeplosodGornTest()
            => Assert.AreEqual(2986.6882, DutyeFormulas.TeplosodGorn(data, 0), 0.01);

        [TestMethod]
        public void TeorTGorTest()
            => Assert.AreEqual(1990.7625, DutyeFormulas.TeorTGor(data, 0), 0.01);

        [TestMethod]
        public void VIstDutTest()
            => Assert.AreEqual(221.8943, DutyeFormulas.VIstDut(data, 0), 0.01);

        [TestMethod]
        public void KinetWTest()
            => Assert.AreEqual(7124.6608, DutyeFormulas.KinetW(data, 0), 0.01);

        [TestMethod]
        public void ProtZoniCirkTest()
            => Assert.AreEqual(1.1348, DutyeFormulas.ProtZoniCirk(data, 0), 0.01);

        [TestMethod]
        public void ProtZoniOkislTest()
            => Assert.AreEqual(1.4248, DutyeFormulas.ProtZoniOkisl(data, 0), 0.01);

        [TestMethod]
        public void OtnoshVPGDTest()
            => Assert.AreEqual(12.2784, DutyeFormulas.OtnoshVPGD(data, 0), 0.01);

        [TestMethod]
        public void TeplosodPriZadZnTest()
            => Assert.AreEqual(3247.1781, DutyeFormulas.TeplosodPriZadZn(data, 0), 0.01);

        [TestMethod]
        public void RashPodderzTest()
            => Assert.AreEqual(0.2855, DutyeFormulas.RashPodderz(data, 0), 0.01);

        [TestMethod]
        public void TrebRashGazTest()
            => Assert.AreEqual(568.5767, DutyeFormulas.TrebRashGaz(data, 0), 0.01);

        [TestMethod]
        public void SFurmOchagTest()
            => Assert.AreEqual(1.0108, DutyeFormulas.SFurmOchag(data, 0), 0.01);

        [TestMethod]
        public void DlinaSrOkrTest()
            => Assert.AreEqual(24.7283, DutyeFormulas.DlinaSrOkr(data, 0), 0.01);
        
        [TestMethod]
        public void SumDlinaMalOsTest()
            => Assert.AreEqual(21.3713, DutyeFormulas.SumDlinaMalOs(data, 0), 0.01);
        
        [TestMethod]
        public void PerekrRazobSmezhOchagTest()
            => Assert.AreEqual(-0.1678, DutyeFormulas.PerekrRazobSmezhOchag(data, 0), 0.01);
    }
}
