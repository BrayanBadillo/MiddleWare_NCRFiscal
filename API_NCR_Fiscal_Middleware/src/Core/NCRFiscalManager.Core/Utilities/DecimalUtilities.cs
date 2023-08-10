using System.Globalization;
using System.Runtime.ConstrainedExecution;

namespace NCRFiscalManager.Core.Utilities
{
    public class DecimalUtilities
    {
        public static float FormaterDecimal(float NumberIn)
        {
            //string number = "";
            //string[] decimales = null;
            //string ParteEntera = "";
            //string Cifra1String = "";
            //string Cifra2String = "";
            //string SeparadorDecimalEntradaErrado = ",";
            //string SeparadorDecimalCorrecto = ".";

            //if (NumberIn == 0)
            //{
            //    ParteEntera = "0";
            //    Cifra1String = "0";
            //}
            //else
            //{
            //    NumberIn = (float)(Math.Truncate(NumberIn * 100) / 100);

            //    var decimalErradado = (NumberIn.ToString()).Contains(SeparadorDecimalEntradaErrado);
            //    var decimalBueno = (NumberIn.ToString()).Contains(SeparadorDecimalCorrecto);

            //    if (!decimalErradado && !decimalBueno)
            //    {
            //        number = NumberIn.ToString() + SeparadorDecimalCorrecto + "0000";
            //    }
            //    else
            //    {
            //        number = NumberIn.ToString() + "0000";
            //    }

            //    number = number.Replace(SeparadorDecimalEntradaErrado, SeparadorDecimalCorrecto);
            //    decimales = number.Split(SeparadorDecimalCorrecto);

            //    ParteEntera = decimales[0];
            //    Cifra1String = decimales[1].Substring(0, 1);
            //    Cifra2String = decimales[1].Substring(1, 1);


            //    if (Int32.Parse(Cifra2String) >= 0 && Int32.Parse(Cifra2String) <= 4)
            //    {
            //        Cifra1String = Cifra1String;
            //        Cifra2String = "";
            //    }
            //    else if (Int32.Parse(Cifra2String) >= 6 && Int32.Parse(Cifra2String) <= 9)
            //    {
            //        Cifra1String = (Int32.Parse(Cifra1String) + 1).ToString();
            //        Cifra2String = "";

            //        if (Cifra1String == "10")
            //        {
            //            ParteEntera = (Int32.Parse(ParteEntera) + 1).ToString();
            //            Cifra1String = "0";
            //        }
            //    }
            //    else if (Int32.Parse(Cifra1String) == 5 && Int32.Parse(Cifra2String) == 0 || (Int32.Parse(Cifra2String) % 2) == 0)
            //    {
            //        Cifra1String = Cifra1String;
            //        Cifra2String = "";
            //    }
            //    else if (Int32.Parse(Cifra1String) == 5 && (Int32.Parse(Cifra2String) % 2) != 0)
            //    {
            //        Cifra1String = (Int32.Parse(Cifra1String) + 1).ToString();
            //        Cifra2String = "";

            //        if (Cifra1String == "10")
            //        {
            //            ParteEntera = (Int32.Parse(ParteEntera) + 1).ToString();
            //            Cifra1String = "0";
            //        }
            //    }
            //    else
            //    {
            //        if (Int32.Parse(Cifra2String) > 5)
            //        {
            //            Cifra1String = (Int32.Parse(Cifra1String) + 1).ToString();
            //            Cifra2String = "";

            //            if (Cifra1String == "10")
            //            {
            //                ParteEntera = (Int32.Parse(ParteEntera) + 1).ToString();
            //                Cifra1String = "0";
            //            }
            //        }
            //        else
            //        {
            //            Cifra2String = "";
            //        }
            //    }
            //}
            //string result = ParteEntera + SeparadorDecimalCorrecto + Cifra1String + Cifra2String;
            //float Final = float.Parse(result, CultureInfo.InvariantCulture.NumberFormat);

            //return (float)(Math.Truncate(Final * 100) / 100);


            //float Final = (float)Math.Truncate(NumberIn * 100) / 100;
            //return Final;


            //return NumberIn;

            var xx = Math.Round(Math.Truncate(NumberIn * 100) / 100, 2, MidpointRounding.ToEven);

            return (float)xx;
        }
    }

}
