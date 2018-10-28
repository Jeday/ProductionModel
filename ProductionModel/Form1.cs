using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductionModel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void parse_file(string filename)
        {
            Dictionary<string, Fact> facts = new Dictionary<string, Fact>();
            Dictionary<string, TerminalFact> termfacts = new Dictionary<string, TerminalFact>();
            List<Rule> rules = new List<Rule>();
            List<string> lines = System.IO.File.ReadLines(filename).ToList();
            var st = lines[0].Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            int countKnowledge = Int32.Parse(st[0]);
            for(int i = 1; i < countKnowledge + 1; ++i)
            {
                st = lines[i].Split(new char[] { ':', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                facts.Add(st[0], new Fact(st[0], Int32.Parse(st[1]), st[2]));
            }

            st = lines[countKnowledge + 1].Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            int countTfact = Int32.Parse(st[0]);
            int ind = countKnowledge + countTfact + 2;
            for(int i = countKnowledge + 2; i < ind; ++i)
            {
                st = lines[i].Split(new char[] { ':', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                termfacts.Add(st[0], new TerminalFact(st[0], st[1], st[2]));
            }

            st = lines[ind].Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            int countRules = Int32.Parse(st[0]);

            for(int i = ind + 1; i < ind + countRules; ++i)
            {
                st = lines[i].Split(new char[] { ':', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                string id = st[0];
                var fct = st[1].Split(new char[] { ',', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                Fact[] list_facts = new Fact[] { };
                for(int j = 0; j < fct.Count(); ++j)
                {
                    string index = fct[j];
                    if (facts.ContainsKey(id))
                        list_facts[j] = facts[id];
                    else if (termfacts.ContainsKey(id))
                        list_facts[j] = termfacts[id];
                    // кинуть экспешн, если не найдено вообще

                }

                fct = st[2].Split(new char[] { ',', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                Fact[] list_facts2 = new Fact[] { };
                for(int k = 0; k < fct.Count(); ++k)
                {
                    string index = fct[k];
                    if (facts.ContainsKey(id))
                        list_facts2[k] = facts[id];
                    else if (termfacts.ContainsKey(id))
                        list_facts2[k] = termfacts[id];
                    // кинуть экспешн, если не найдено вообще

                }
                rules.Add(new Rule(id, list_facts, list_facts2));
            }


        }
    }

    public class Fact
    {
        public string id;
        public double weight;
        public string text;

        public Fact() { }

        public Fact(string _id, double _weight, string _text)
        {
            id = _id;
            weight = _weight;
            text = _text;
        }
    }

    public class TerminalFact : Fact
    {
        string img;

        public TerminalFact() { }

        public TerminalFact(string _id, string _text, string _img)
        {
            id = _id;
            text = _text;
            img = _img;
        }
    }

    class Rule
    {
        public string id;
        public HashSet<Fact> condition;
        public HashSet<Fact> result;

        public Rule() { }

        public Rule(string _id, Fact[] cond, Fact[] res)
        {
            id = _id;
            condition = new HashSet<Fact>();
            foreach (var c in cond)
                condition.Add(c);
            result = new HashSet<Fact>();
            foreach (var r in res)
                result.Add(r);
        }
    }
}
