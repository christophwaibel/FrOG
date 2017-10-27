﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Opt_ES.cs
 * Copyright 2017 Christoph Waibel <chwaibel@student.ethz.ch>
 * 
 * This work is licensed under the GNU GPL license version 3 or later.
*/

namespace FrOG
{
    public class Opt_ES : ISolver
    {
        /// <summary>
        /// Variable vector of final solution.
        /// </summary>
        public double[] Xopt { get; private set; }
        /// <summary>
        /// Cost of final solution.
        /// </summary>
        public double Fxopt { get; private set; }

        //public Dictionary<string, string> settings = new Dictionary<string, string>();

        private readonly Dictionary<string, Dictionary<string, double>> _presets = new Dictionary<string, Dictionary<string, double>>();

        public Opt_ES()
        {
            //Prepare settings
            var ES_Settings = new Dictionary<string, double>{
                {"popsize", 20},
                { "itermax", 10000},
                { "seed", 1},
                {"lambda", 20},         //offspring
                {"roh", 2},             //mixing nr.
                {"s", 0.5},             //stepsize s
                {"s0", 1.0},            //initial stepsize s0
                {"tauc", 1.0},            //learning rate tau
                {"pmut_int", 0.1},      //mutation probability, only for integer
                {"x0samplingmode", 0}   // 0 = uniform, 1 = gaussian
            };

            _presets.Add("ES", ES_Settings);
        }

        public bool RunSolver(List<Variable> variables, Func<IList<decimal>, double> evaluate, string preset, string expertsettings, string installFolder, string documentPath)
        {
            var settings = _presets[preset];

            //System.Windows.Forms.MessageBox.Show(expertsettings);     //use expertsettings to input custom solver parameters

            int? seedin = null;
            string[] expsets = expertsettings.Split(';');
            foreach (string strexp in expsets)
            {
                string[] stre = strexp.Split('=');
                if (string.Equals(stre[0], "seed"))
                {
                    seedin = Convert.ToInt16(stre[1]);
                }
            }

            var dvar = variables.Count;
            var lb = new double[dvar];
            var ub = new double[dvar];
            var integer = new bool[dvar];

            for (var i = 0; i < dvar; i++)
            {
                lb[i] = Convert.ToDouble(variables[i].LowerB);
                ub[i] = Convert.ToDouble(variables[i].UpperB);
                integer[i] = variables[i].Integer;
            }

            Func<double[], double> eval = x =>
            {
                var decis = x.Select(Convert.ToDecimal).ToList();
                return evaluate(decis);
            };

            try
            {
                if (preset.Equals("ES"))
                {
                    Dictionary<string, object> ESsettings = new Dictionary<string, object>();
                    ESsettings.Add("popsize", (int)settings["popsize"]);
                    ESsettings.Add("lambda", (int)settings["lambda"]);
                    ESsettings.Add("roh", (int)settings["roh"]);
                    ESsettings.Add("s", settings["s"]);
                    ESsettings.Add("s0", settings["s0"]);
                    ESsettings.Add("tauc", settings["tauc"]);
                    ESsettings.Add("pmut_int", settings["pmut_int"]);
                    ESsettings.Add("x0samplingmode", (int)settings["x0samplingmode"]);
                    int seed;
                    if (seedin != null)
                    {
                        seed = Convert.ToInt16(seedin);
                    }
                    else
                    {
                        seed = (int)settings["seed"];
                    }
                    int itermax = (int)settings["itermax"];

                    var es = new MetaheuristicsLibrary.SolversSO.SimpleES(lb, ub, integer, itermax, eval, seed, ESsettings);
                    es.solve();
                    Xopt = es.get_Xoptimum();
                    Fxopt = es.get_fxoptimum();
                }
                return true;
            }                
            catch
            {
                return false;
            }

        }

        public string GetErrorMessage()
        {
            return "";
        }

        /// <summary>
        /// Get the variable vector of the final solution.
        /// </summary>
        /// <returns>Variable vector.</returns>
        public double[] get_Xoptimum()
        {
            return Xopt;
        }

        public IEnumerable<string> GetPresetNames()
        {
            return _presets.Keys;
        }
    }
}
