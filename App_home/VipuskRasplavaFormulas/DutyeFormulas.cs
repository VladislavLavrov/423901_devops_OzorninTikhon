namespace RaspredeleniyeDutyaFormulas
{
    public class DutyeFormulas
    {
        public static double SrZnach(InitialData data, Func<InitialData, int, double> func)
        {
            double sum = 0.0;
            int count = 0;
            for (int i = 0; i < data.RashGazNaF.Count; i++)
            {
                if (data.FurmPodachaDutya[i])
                {
                    sum += func(data, i);
                    count++;
                }
            }
            double srznach = sum / count;
            return srznach;
        }

        [Formula("Количество тепла на нагрев воды, кВт")]
        public static double KolTepla(InitialData data, int furm)
            => data.RashVodiNaF[furm] * data.TPerepad[furm] * 1000 * 4.18 / 3600;

        [Formula("Фактический расход дутья через фурму, м3/мин")]
        public static double FactRashDut(InitialData data, int furm)
        {
            double srZnachTeplPotoka = data.RashDut * PredvaritFormulas.TeploemkDut(data) * data.TDut / data.NRabFurm
                + 2.22 * 50 * SrZnach(data, (data, furm) => data.RashGazNaF[furm]) / 60;
            double doltepl = SrZnach(data, KolTepla) / srZnachTeplPotoka;
            return (KolTepla(data, furm) - data.RashGazNaF[furm] * 2.22 * 50 * doltepl / 60)
                / (PredvaritFormulas.TeploemkDut(data) * data.TDut * doltepl);
        }

        [Formula("Расчетный относительный расход дутья через фурму")]
        public static double RasOtnos(InitialData data, int furm)
            => FactRashDut(data, furm) / SrZnach(data, FactRashDut);

        [Formula("Количество сожжённого углерода , кг/мин")]
        public static double KolUgler(InitialData data, int furm)
            => 1.07143 * FactRashDut(data, furm) * 0.01 * data.SodKislorod;

        [Formula("Расход природного газа на 1 кг углерода, м3/кгС")]
        public static double RashPGNaKG(InitialData data, int furm)
            => data.RashGazNaF[furm] / 60 / KolUgler(data, furm);

        [Formula("Теплосодержание горновых газов, кДж/м3")]
        public static double TeplosodGorn(InitialData data, int furm)
            => (9800 + PredvaritFormulas.DutRashodPerC(data) * PredvaritFormulas.TeplosodDut(data)
            + PredvaritFormulas.TeplosodKoks(data)
            + RashPGNaKG(data, furm) * (1590 + PredvaritFormulas.DutRashodPerGaz(data) * PredvaritFormulas.TeplosodDut(data)))
            / (PredvaritFormulas.GornGazPerC(data) + RashPGNaKG(data, furm) * PredvaritFormulas.GornGazPerGaz(data));

        [Formula("Теоретическая температура горения, оС")]
        public static double TeorTGor(InitialData data, int furm)
            => 0.6113 * TeplosodGorn(data, furm) + 165;

        [Formula("Скорость истечения дутья из воздушной фурмы, м/с")]
        public static double VIstDut(InitialData data, int furm)
            => (FactRashDut(data, furm) + data.RashGazNaF[furm] / 60) * (data.TDut + 273) * 77.73
            / (data.DiamFurm * data.DiamFurm * (1 + data.DavlDut));

        [Formula("Кинетическая энергия истечения дутья из фурмы, кгм/с")]
        public static double KinetW(InitialData data, int furm)
            => (FactRashDut(data, furm) * 1.293 + data.RashGazNaF[furm] * 0.717 / 60)/ 1177 * Math.Pow(VIstDut(data, furm), 2);

        [Formula("Протяженность зоны циркуляции (кислородная часть зоны горения), м")]
        public static double ProtZoniCirk(InitialData data, int furm)
            => 0.8039 + 0.00005 * KinetW(data, furm) - 0.0000000005 * Math.Pow(KinetW(data, furm), 2);

        [Formula("Протяженность окислительной зоны (по 2% CO2), м")]
        public static double ProtZoniOkisl(InitialData data, int furm)
            => ProtZoniCirk(data, furm) + 0.29;

        [Formula("Отношение расхода природного газа к расходу дутья, %")]
        public static double OtnoshVPGD(InitialData data, int furm)
            => data.RashGazNaF[furm] / 60 / FactRashDut(data, furm) * 100;

        [Formula("Теплосодержание горновых газов при заданной теоретической температуре горения, кДж/м3")]
        public static double TeplosodPriZadZn(InitialData data, int furm)
            => (data.TrebZnTeor - 165) / 0.6113;

        [Formula("Расход газа для поддержания теоретической температуры горения, м3/кг С")]
        public static double RashPodderz(InitialData data, int furm)
            => (
                9800 + PredvaritFormulas.DutRashodPerC(data) * PredvaritFormulas.TeplosodDut(data)
                + PredvaritFormulas.TeplosodKoks(data) - TeplosodPriZadZn(data, furm) * PredvaritFormulas.GornGazPerC(data)
            ) / (
                TeplosodPriZadZn(data, furm) * PredvaritFormulas.GornGazPerGaz(data)
                - (1590 + PredvaritFormulas.DutRashodPerGaz(data) * PredvaritFormulas.TeplosodDut(data))
            );

        [Formula("Требуемый расход природного газа для поддержания теоретической температуры горения, м3/ч")]
        public static double TrebRashGaz(InitialData data, int furm)
            => RashPodderz(data, furm) * KolUgler(data, furm) * 60;

        [Formula("Площадь фурменного очага, м2")]
        public static double SFurmOchag(InitialData data, int furm)
            => 3.14 * Math.Pow(ProtZoniCirk(data, furm), 2) / 4;

        [Formula("Относительная площадь фурменных очагов к площади горна, %")]
        public static double SFurmOchagToSGorn(InitialData data, int furm)
            => 0.9 * data.NRabFurm * Math.Pow(ProtZoniOkisl(data, furm) / data.GornDiam, 2) * 100;

        [Formula("Длина средней окружности по центрам фурменных очагов, м")]
        public static double DlinaSrOkr(InitialData data, int furm)
            => (data.GornDiam - (2 * data.HeightFurm / 1000 + ProtZoniOkisl(data, furm))) * 3.14;

        [Formula("Суммарная длина малых осей фурменных очагов, м")]
        public static double SumDlinaMalOs(InitialData data, int furm)
            => data.NRabFurm * ProtZoniOkisl(data, furm) * data.KoefSzhatOchag;

        [Formula("Перекрытие (+), разобщение (-) смежных фурменных очагов, м", allows_negative: true)]
        public static double PerekrRazobSmezhOchag(InitialData data, int furm)
            => (SumDlinaMalOs(data, furm) - DlinaSrOkr(data, furm)) / data.NRabFurm;
    }
}
