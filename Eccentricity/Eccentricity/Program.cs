using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace Eccentricity
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input Data
            double fbc, eccnew, alphanew, kesi, m, ft, fr, k1, fcc, rho, J2, r, rfbc;
            double a, b, c;
            double fc = 120;
            double ecc = 0.52;
            double alpha = 1;
            int SF = 0;
            const double tol9 = 0.000000001;

            // Calculate basic parameter
            fbc = 1.5 * Pow(fc, 0.925);
            rfbc = fbc /fc;
            kesi = -2.0 * fbc / Sqrt(3.0);
            J2 = 2.0 / 6.0 * Pow(-fbc, 2.0);
            rho = Sqrt(2.0 * J2);
            if (SF == 0)
            {
                ft = 0.9 * 0.32 * Pow(fc, 0.67);
            }
            else
            {
                ft = 0.9 * 0.62 * Pow(fc, 0.5);
            }
            first:;
            m = 3.0 * (fc * fc - ft * ft) / (fc * ft);
            m *= ecc / (ecc + 1.0);

            // Calculate fr
            a = 9.0;
            b = -(alpha * m * fc - 6.0 * Sqrt(3.0) * kesi);
            c = 3.0 * kesi * kesi - fc * fc + 2.0 * Sqrt(3.0) * kesi * m * (1.0 - alpha) * fc / 6.0;
            fr = -b - Sqrt(b * b - 4.0 * a * c);
            fr /= 2.0 * a;

            // Calculate fcc (Attard and Setunge - 1996)
            k1 = 1.25 * (1.0 + 0.062 * fr / fc) * Pow(fc, -0.21);
            fcc = fc * Pow(fr / ft + 1.0, k1);

            // Calculate AlphaNew
            alphanew = 1.0 - ((fc * fc + m * fr * fc - Pow(fcc - fr, 2.0)) / (m * fc / 3.0 * (fr - fcc)));

            if (Abs(alpha - alphanew) > tol9)
            {
                alpha = alphanew;
                goto first;
            }

            r = 1.0 - 1.5 * rho * rho / (fc * fc) - m * kesi / (Sqrt(3.0) * fc);
            r *= Sqrt(6.0) * fc / (rho * m * alphanew);
            eccnew = 1.0 / r;

            if (Abs(ecc - eccnew) > tol9)
            {
                ecc = eccnew;
                goto first;
            }

            Console.WriteLine("fc = " + fc + "MPa ecc = " + eccnew);
            Console.WriteLine("I1A = " + (fcc + 2.0 * fr) + "MPa I1B = " + (2.0 * fbc) + " MPa");
            Console.ReadKey();
        }
    }
}
