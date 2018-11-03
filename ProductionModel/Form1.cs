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
        private List<Fact> init_knowledge= new List<Fact>(); // user choosen facts
        private List<Fact> possible_knoweledge = new List<Fact>(); //all facts that users can change
        private List<TerminalFact> terminals = new List<TerminalFact>(); // terminal facts
        private List<Rule> Rules = new List<Rule>();
        private HashSet<Fact> work_area = new HashSet<Fact>(); // proven facts
        private HashSet<Fact> support_area = new HashSet<Fact>();  // non terminal 
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
                    else if (support_facts.ContainsKey(index))
                        list_facts.Add(support_facts[index]);
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
                    else if (support_facts.ContainsKey(index))
                        list_facts2.Add(support_facts[index]);
                    // кинуть экспешн, если не найдено вообще

                }
                rules.Add(new Rule(id, list_facts.ToArray(), list_facts2.ToArray()));
 
            }
            init_knowledge.Clear();
            possible_knoweledge.Clear();
            terminals.Clear();
            Rules.Clear();
            work_area.Clear();
            support_area.Clear();
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
            foreach(Fact f in support_facts.Values)
            {

                support_area.Add(f);
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

        private FlowLayoutPanel panel_factory() {
            FlowLayoutPanel panel = new FlowLayoutPanel();
            panel.FlowDirection = FlowDirection.TopDown;
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.AutoScroll = true;
            panel.AutoSize = true;
            panel.WrapContents = false;
            return panel;

        }

        private Label label_factory(string text, Color c = new Color()) {
            Label l = new Label();
            l.Text = text;
            l.ForeColor = c;
            l.AutoSize = true;
            return l;
        }


        private List<TerminalFact> forward() {
            ThoughtLinePanel.Controls.Clear();
            work_area = new HashSet<Fact>(init_knowledge.Select(x => new Fact(x)));
            List<Rule> applyable = Rules.Where(r=> r.condition.IsSubsetOf(work_area)).ToList();
            HashSet<Rule> used = new HashSet<Rule>() ;
            List<TerminalFact> res = new List<TerminalFact>();


            int time = 0;
            while (applyable.Count != 0)
            {
                time++;
                /// cool design
                FlowLayoutPanel used_facts_panel = panel_factory();
                FlowLayoutPanel used_rules_panel = panel_factory();
                FlowLayoutPanel new_facts_panel = panel_factory();



                /// collect all used facts and all newly proven ones
                HashSet<Fact> used_facts = new HashSet<Fact>();
                HashSet<Fact> new_facts = new HashSet<Fact>();




                ///get the BEST applyable rule and only apply it
                applyable = applyable
                    .OrderByDescending(r=>r.result.Count(f => f is TerminalFact)) // fist priority getting terminals
                    .ThenBy(r => r.result.Intersect(work_area).Count()) //  getting new facts
                    .ThenByDescending(r => (double)r.condition.Sum(f => f.time) / r.condition.Count()) // using newer facts
                    .ThenByDescending(r => r.condition.Count) // using  more specific condition                              
                    .ToList();
                Rule best_rule = applyable.First();


                    foreach (Fact f in best_rule.condition)
                        used_facts.Add(f);

                    foreach (Fact f in best_rule.result)
                        if (!work_area.Contains(f)){
                            new_facts.Add(f);
                            f.time = time;
                            work_area.Add(f);
                        }                       
                


                // add panel for used facts
                used_facts_panel.Controls.Add(label_factory("Applyable facts:"));
                foreach (Fact f in applyable.SelectMany(r => r.condition).Distinct())          
                    used_facts_panel.Controls.Add(label_factory("\t" + f.id + ": " + f.text, best_rule.condition.Contains(f) ? Color.Green : Color.Black));
                ThoughtLinePanel.Controls.Add(used_facts_panel);


                // add panel for used rules
                used_rules_panel.Controls.Add(label_factory("Active rules:"));
                foreach (Rule r in applyable)
                    used_rules_panel.Controls.Add(label_factory("\t" + r.ToString(),r == best_rule ? Color.Green : Color.Black ));

                ThoughtLinePanel.Controls.Add(used_rules_panel);

                // add panel for new facts
                new_facts_panel.Controls.Add(label_factory("New facts:"));
                foreach (Fact f in new_facts)
                {
                    Label txt = label_factory("\t" + f.id + ": " + f.text);
                    if(f is TerminalFact)
                    {
                        txt.ForeColor = System.Drawing.Color.Red;
                        res.Add(f as TerminalFact);
                    }
                    new_facts_panel.Controls.Add(txt);

                }
                ThoughtLinePanel.Controls.Add(new_facts_panel);


                // get used rules to all used
                used.Add(best_rule);
                // get new applyable rules and sort by the strongest ones
                applyable = Rules.Where((Rule r) => r.condition.IsSubsetOf(work_area) && !used.Contains(r)).ToList();

                ThoughtLinePanel.Refresh();
                if (res.Count > 0)
                    break;
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

                List<Tuple<TerminalFact, double>> guesses= new List<Tuple<TerminalFact, double>>();
                foreach (TerminalFact t in terminals) {
                    double val;
                    var r = Rules.Where(cr => cr.result.Contains(t) && cr.condition.Any(cf => work_area.Contains(cf)))
                                 .Select(cr => (double)cr.condition.Intersect(work_area).Count() / cr.condition.Count());
                    if (r.Count() == 0)
                        val = 0;
                    else
                        val = r.Max();
                    guesses.Add(new Tuple<TerminalFact, double> ( t, val));
                }
                guesses = guesses.OrderByDescending(t => t.Item2).ToList();
                

                if (guesses.Count() == 0 || guesses.First().Item2 == 0)
                {
                    label1.Text = "UNKNOWN";
                    pictureBox1.ImageLocation = "https://cdn-images-1.medium.com/max/800/1*Km98PgzRp9yRYfVZeSzwzQ.png";
                }
                else
                {
                    guesses = guesses.Where(g => g.Item2 != 0).ToList();
                    label1.Text = guesses.First().Item1.text + " with probability " + guesses.First().Item2.ToString("N2");
                    pictureBox1.ImageLocation = guesses.First().Item1.img;
                    var p = panel_factory();
                    foreach (var g in guesses) {
                        p.Controls.Add(label_factory(g.Item1.ToString()+" prb:%"+(g.Item2 * 100).ToString("N1")));
                    }
                    ThoughtLinePanel.Controls.Add(p);
                }

            }
            pictureBox1.Refresh();
            flowLayoutPanel1.Refresh();
        }

 
    }

    public class Fact
    {
        public string id;
        public double weight = 1;
        public string text;
        public FactControl cntrl;
        public int time = 0;

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
            id = _id.Trim();
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
            return id.GetHashCode();
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
            id = _id.Trim();
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
            id = _id.Trim();
            condition = new HashSet<Fact>();
            foreach (var c in cond)
                condition.Add(c);
            result = new HashSet<Fact>();
            foreach (var r in res)
                result.Add(r);
        }

        // override object.Equals
        public override bool Equals(object obj)
        {


            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }


            return id == (obj as Rule).id;  
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {

            return id.GetHashCode();
        }

        public override string ToString()
        {
            return id + ": " + string.Join(",", condition.Select(f => f.id)) + " -> " + string.Join(",", result.Select(f => f.id));
        }
            

    }
}
