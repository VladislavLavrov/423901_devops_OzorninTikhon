namespace RaspredeleniyeDutyaFormulas
{
    /// <summary>
    /// Входные данные
    /// </summary>
    public class InitialData
    {
        /// <summary>
        /// Число работающих воздушных фурм, шт
        /// </summary>
        [Description("Число работающих воздушных фурм, шт")]
        public int NRabFurm { get; set; } = 20;

        /// <summary>
        /// Диаметр фурм, мм
        /// </summary>
        [Description("Диаметр фурм, мм")]
        public double DiamFurm { get; set; } = 142;

        /// <summary>
        /// Высота фурм, мм
        /// </summary>
        [Description("Высота фурм, мм")]
        public double HeightFurm { get; set; } = 350;

        /// <summary>
        /// Расход дутья, м3/мин
        /// </summary>
        [Description("Расход дутья, м3/мин")]
        public double RashDut { get; set; } = 2417;

        /// <summary>
        /// Давление дутья, ати
        /// </summary>
        [Description("Давление дутья, ати")]
        public double DavlDut { get; set; } = 2.5;

        /// <summary>
        /// Температура дутья, оС
        /// </summary>
        [Description("Температура дутья, оС")]
        public double TDut { get; set; } = 1182;

        /// <summary>
        /// Влажность дутья, г/м3
        /// </summary>
        [Description("Влажность дутья, г/м3")]
        public double VlazDut { get; set; } = 16.4;

        /// <summary>
        /// Содержание кислорода в дутье, %
        /// </summary>
        [Description("Содержание кислорода в дутье, %")]
        public double SodKislorod { get; set; } = 25.12;

        /// <summary>
        /// Ввод требуемого значения теоретической температуры горения, оС
        /// </summary>
        [Description("Ввод требуемого значения теоретической температуры горения, оС")]
        public double TrebZnTeor { get; set; } = 2150;

        /// <summary>
        /// Диаметр горна доменной печи, м
        /// </summary>
        [Description("Диаметр горна доменной печи, м")]
        public double GornDiam { get; set; } = 10.0;

        /// <summary>
        /// Коэффициент сжатия фурменного очага (в плане)
        /// </summary>
        [Description("Коэффициент сжатия фурменного очага (в плане)")]
        public double KoefSzhatOchag { get; set; } = 0.75;

        /// <summary>
        /// Открытие фурмы
        /// </summary>
        [Description("Открытие фурмы")]
        public List<bool> FurmPodachaDutya { get; set; } = [
            true, true, true, true, true,
            false, true, true, true, true,
            true, true, true, true, false,
            true, true, true, true, true,
            ];

        /// <summary>
        /// Расход газа на фурму, м3/ч
        /// </summary>
        [Description("Расход газа на фурму, м3/ч")]
        public List<double> RashGazNaF { get; set; } = [
            908.5220, 908.5220, 917.6240, 920.8330, 921.3780,
            22.2600, 925.3420, 926.8010, 921.7890, 918.6920,
            920.7320, 918.4270, 853.9550, 914.1990, 0.0000,
            904.4820, 910.4900, 905.3360, 921.6320, 910.5900,
            ];

        /// <summary>
        /// Расход воды на фурму, м3/ч
        /// </summary>
        [Description("Расход воды на фурму, м3/ч")]
        public List<double> RashVodiNaF { get; set; } = [
            11.4120, 12.1080, 12.8280, 12.6240, 12.2570,
            11.5870, 12.5030, 12.3260, 11.8070, 11.4000,
            13.4110, 12.7060, 13.2530, 14.4460, 13.6160,
            11.3130, 13.6310, 12.5670, 11.6830, 11.5220,
            ];

        /// <summary>
        /// Температурный перепад воды на фурме, оС
        /// </summary>
        [Description("Температурный перепад воды на фурме, оС")]
        public List<double> TPerepad { get; set; } = [
            11.2500, 9.4400, 8.1700, 8.9600, 9.6000,
            0.5300, 10.5900, 10.4000, 9.5600, 11.1000,
            10.3300, 10.4800, 11.0900, 9.8800, 1.4700,
            12.2100, 8.5800, 10.3400, 9.7500, 10.9700,
            ];
    }
}
