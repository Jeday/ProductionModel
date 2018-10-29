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
        private List<Fact> init_knowledge= new List<Fact>();
        private List<Fact> possible_knoweledge = new List<Fact>();
        private List<TerminalFact> terminals = new List<TerminalFact>();
        private List<Rule> Rules = new List<Rule>();
        private HashSet<Fact> work_area = new HashSet<Fact>();

        public Form1()
        {
            InitializeComponent();
    
            
        }

        private void reset_controls() {
            foreach (Control c in flowLayoutPanel1.Controls)
                c.Dispose();
            flowLayoutPanel1.Controls.Clear();
            foreach (Fact f in possible_knoweledge) {
                FactControl fc = new FactControl();
                fc.FactText.Text = f.text;
                fc.father = f;
                f.cntrl = fc;
                flowLayoutPanel1.Controls.Add(fc);
            }
            flowLayoutPanel1.Refresh();
        }

        private void parse_file(string filename)
        {
            Dictionary<string, Fact> facts = new Dictionary<string, Fact>();
            Dictionary<string, TerminalFact> termfacts = new Dictionary<string, TerminalFact>();
            Dictionary<string, Fact> support_facts = new Dictionary<string, Fact>();
            List<Rule> rules = new List<Rule>();
            List<string> lines = System.IO.File.ReadLines(filename, Encoding.GetEncoding(1251)).ToList();
            var st = lines[0].Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            int countKnowledge = Int32.Parse(st[0]);

            for(int i = 1; i < countKnowledge + 1; ++i)
            {
                st = lines[i].Split(new char[] { ':', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                facts.Add(st[0].Trim(), new Fact(st[0], st[1]));
            }

            st = lines[countKnowledge + 1].Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            int countSfact = Int32.Parse(st[0]);
            int ind = countKnowledge + countSfact + 2;
            for(int i = countKnowledge + 2; i < ind; ++i)
            {
                st = lines[i].Split(new char[] { ':', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                support_facts.Add(st[0].Trim(), new Fact(st[0], st[1]));
            }

            st = lines[ind].Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            int countTfact = Int32.Parse(st[0]);
            ind += 1;
            for(int i = ind; i < ind + countTfact; ++i)
            {
                string img_link = lines[i].Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries).Last();
                st = lines[i].Split(new char[] { ':', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                termfacts.Add(st[0].Trim(), new TerminalFact(st[0].Trim(), st[1], img_link));
            }

            ind += countTfact;
            st = lines[ind].Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            int countRules = Int32.Parse(st[0]);

            for(int i = ind + 1; i < ind + countRules; ++i)
            {
                
                st = lines[i].Split(new char[] { ':', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                string id = st[0].Trim();
                var fct = st[1].Split(new char[] { ',', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                List<Fact> list_facts = new List<Fact>();
                for(int j = 0; j < fct.Count(); ++j)
                {
                    string index = fct[j].Trim();
                    if (facts.ContainsKey(index))
                        list_facts.Add(facts[index]);
                    else if (termfacts.ContainsKey(index))
                        list_facts.Add(termfacts[index]);
                    // кинуть экспешн, если не найдено вообще

                }

                fct = st[2].Split(new char[] { ',', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                List<Fact> list_facts2 = new List<Fact>();
                for (int k = 0; k < fct.Count(); ++k)
                {
                    string index = fct[k].Trim();
                    if (facts.ContainsKey(index))
                        list_facts2.Add(facts[index]);
                    else if (termfacts.ContainsKey(index))
                        list_facts2.Add(termfacts[index]);
                    // кинуть экспешн, если не найдено вообще

                }
                rules.Add(new Rule(id, list_facts.ToArray(), list_facts2.ToArray()));
 
            }
            init_knowledge.Clear();
            possible_knoweledge.Clear();
            terminals.Clear();
            Rules.Clear();
            work_area.Clear();
            foreach (Fact f in facts.Values)
            {
                possible_knoweledge.Add(f);
            }
            foreach (Rule r in rules)
            {
                Rules.Add(r);
            }
            foreach (TerminalFact f in termfacts.Values)
            {
                terminals.Add(f);
            }
            reset_controls();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            string filename = openFileDialog1.FileName;
            if (!System.IO.File.Exists(filename))
                return;
            parse_file(filename);
        }


        private List<TerminalFact> forward() {
            work_area = new HashSet<Fact>(init_knowledge.Select(x => new Fact(x)));
            List<Rule> applyable = Rules.Where((Rule r) => r.condition.All((Fact r_c) => work_area.Contains(r_c))).ToList();
            List<Rule> used = new List<Rule>() ;
            List<TerminalFact> res = new List<TerminalFact>();

            while (applyable.Count != 0)
            {
                foreach (Rule r in applyable)
                    foreach (Fact f in r.result)
                    {
                        if (f is TerminalFact)
                            res.Add(f as TerminalFact);
                        work_area.Add(f);
                    }
                used = used.Union(applyable).ToList();
                applyable = Rules.Where((Rule r) => r.condition.IsSubsetOf(work_area) && !used.Contains(r)).ToList();
            }
            return res;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            init_knowledge = possible_knoweledge.Where(f => f.cntrl.FactValueControl.Value == 1).ToList();
            List<TerminalFact> res = forward();
            if (res.Count != 0)
            {
                TerminalFact best = res.First();
                label1.Text = best.text;
                pictureBox1.ImageLocation = best.img;
            }
            else {
                label1.Text = "UNKNOWN";
                pictureBox1.ImageLocation = "https://cdn-images-1.medium.com/max/800/1*Km98PgzRp9yRYfVZeSzwzQ.png";

            }
            pictureBox1.Refresh();
        }
    }

    public class Fact
    {
        public string id;
        public double weight = 1;
        public string text;
        public FactControl cntrl;


        public Fact(Fact f) {
            id = f.id;
            weight = f.weight;
            text = f.text;
            cntrl = f.cntrl;
        }

        public override string ToString()
        {
            return text;
        }

        public Fact() { }

        public Fact(string _id, string _text)
        {
            id = _id;
            text = _text;
        }

        public override bool Equals(object obj)
        {
            var item = obj as Fact;

            if (item == null)
            {
                return false;
            }

            return id == item.id;
        }

        public override int GetHashCode()
        {
            return this.id.GetHashCode();
        }

    }

    public class TerminalFact : Fact
    {
        public string img;

        public TerminalFact() { }

        public TerminalFact(TerminalFact f) : base(f as Fact){
            img = f.img;

        }

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
