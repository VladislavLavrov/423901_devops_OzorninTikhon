using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspredeleniyeDutyaFormulas
{
    public class PredvaritFormulas
    {
        [Formula("Теплоемкость кислорода при температуре дутья, кДж/(м3*к)")]
        public static double TeploemkKislorod(InitialData data)
            => 1.4851 + 0.000118 * data.TDut;

        [Formula("Теплоемкость азота при температуре дутья, кДж/(м3*К)")]
        public static double TeploemkAzot(InitialData data)
            => 1.3902 + 0.000133 * data.TDut;

        [Formula("Теплоемкость двухатомных газов при температуре дутья, кДж/(м3*К)")]
        public static double TeploemkDvuhatom(InitialData data)
            => 1.2897 + 0.000121 * data.TDut;

        [Formula("Теплоемкость паров воды при температуре дутья, кДж/(м3*к)")]
        public static double TeploemkParVoda(InitialData data)
            => 1.456 + 0.000282 * data.TDut;

        [Formula("Расход дутья на 1 кг углерода, м3/кг С")]
        public static double DutRashodPerC(InitialData data)
            => 0.9333 / (0.01 * data.SodKislorod + 0.00124 * data.VlazDut);

        [Formula("Расход дутья для сжигания 1 м3 газа, м3/м3")]
        public static double DutRashodPerGaz(InitialData data)
            => 0.5 / (0.01 * data.SodKislorod + 0.00124 * data.VlazDut);

        [Formula("Выход горнового газа при сжигании 1 кг углерода, м3/кг С")]
        public static double GornGazPerC(InitialData data)
            => 1.8667 + DutRashodPerC(data) * (1 - 0.01 * data.SodKislorod + 0.00124 * data.VlazDut);

        [Formula("Выход горнового газа при сжигании 1 м3 газа, м3/м3")]
        public static double GornGazPerGaz(InitialData data)
            => 3 + DutRashodPerGaz(data) * (1 - 0.01 * data.SodKislorod + 0.00124 * data.VlazDut);

        [Formula("Теплосодержание дутья, кДж/м3")]
        public static double TeplosodDut(InitialData data)
            => TeploemkDvuhatom(data) * data.TDut - 0.00124 * data.VlazDut * (10800 - TeploemkParVoda(data) * data.TDut);

        [Formula("Теплосодержание кокса, приходящего к фурмам, кДж/кг")]
        public static double TeplosodKoks(InitialData data)
            => 1.65 * 1500;

        [Formula("Теплоемкость дутья, кДж/(м3*К)")]
        public static double TeploemkDut(InitialData data)
            => 0.01 * data.SodKislorod * TeploemkKislorod(data) + (1 - 0.01 * data.SodKislorod) * TeploemkAzot(data);
    }
}
