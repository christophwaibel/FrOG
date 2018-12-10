using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrOG.Solvers
{
    public class Sampling
    {
        /// <summary>
        /// Lower bound for each variable.
        /// </summary>
        public double[] lb { get; private set; }
        /// <summary>
        /// Upper bound for each variable.
        /// </summary>
        public double[] ub { get; private set; }
        /// <summary>
        /// Maximum iterations. Should be smaller equal the input sequence file.
        /// </summary>
        public int itermax { get; private set; }
        /// <summary>
        /// Evaluation function.
        /// </summary>
        public Func<double[], double> evalfnc { get; private set; }
        /// <summary>
        /// Variable vector of best solution of the sequence.
        /// </summary>
        public double[] xopt { get; private set; }
        /// <summary>
        /// Cost of best solution of sequence.
        /// </summary>
        public double fxopt { get; private set; }
        /// <summary>
        /// Path of the input sequence. Should be .csv file, with each input vector in one line. Decision variables per line need to be comma-separated.
        /// </summary>
        public string inputpath { get; private set; }

        /// <summary>
        /// complete input sequence. Variable vector per line. Comma-separated variables per line.
        /// </summary>
        private List<double[]> inputsequence;


        /// <summary>
        /// Initialize sampling, using a pre-defined, comma-separated input sequence.
        /// </summary>
        /// <param name="lb">Lower bound for each variable.</param>
        /// <param name="ub">Upper bound for each variable.</param>
        /// <param name="itermax">Maximum iterations. Should be smaller equal the input sequence.</param>
        /// <param name="evalfnc">Evaluation function.</param>
        public Sampling(double[] lb, double[] ub, int itermax, Func<double[], double> evalfnc, string inputpath)
        {
            this.lb = lb;
            this.ub = ub;
            this.itermax = itermax;
            this.evalfnc = evalfnc;
            this.inputpath = inputpath;

            this.inputsequence = readSequence(inputpath);
        }

        private List<double []> readSequence(string path)
        {
            int n = this.lb.Length;
            //load input sequence for sampling
            List<double[]> x = new List<double[]>();
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            string line;
            string[] text = new string[] { };
            int linecount = 0;
            while ((line = file.ReadLine()) != null)
            {
                x.Add(new double[n]);
                text = line.Split(',');
                int counter = 0;
                foreach (string t in text)
                {
                    x[linecount][counter] = Convert.ToDouble(t);
                    counter++;
                }
                linecount++;
            }
            file.Close();



            return x;
        }


        /// <summary>
        /// Goes through the input sequence
        /// </summary>
        public void solve()
        {
            int n = lb.Length;
            double[] x = new double[n];
            double[] stdev = new double[n];


            x = this.inputsequence[0];
            double fx = evalfnc(x);

            for (int t = 1; t < itermax; t++)
            {
                double[] xtest = this.inputsequence[t];
                double fxtest = evalfnc(xtest);

                if (double.IsNaN(fxtest)) return;

                if (fxtest < fx)
                {
                    xtest.CopyTo(x, 0);
                    fx = fxtest;

                    xopt = new double[n];
                    x.CopyTo(xopt, 0);
                    fxopt = fx;
                }
            }
        }

        /// <summary>
        /// Get the variable vector of the best solution of the sequence.
        /// </summary>
        /// <returns>Variable vector.</returns>
        public double[] get_Xoptimum()
        {
            return this.xopt;
        }

        /// <summary>
        /// Get the cost value of the best solution of the sequence.
        /// </summary>
        /// <returns>Cost value.</returns>
        public double get_fxoptimum()
        {
            return this.fxopt;
        }
    }


}
