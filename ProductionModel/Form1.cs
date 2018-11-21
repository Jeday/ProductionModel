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
        private List<Fact> selected_userfacts= new List<Fact>(); // user choosen facts
        private List<Fact> all_userfacts = new List<Fact>(); //all facts that users can change
        private List<TerminalFact> terminals = new List<TerminalFact>(); // terminal facts
        private List<Rule> Rules = new List<Rule>();
        private HashSet<Fact> work_area = new HashSet<Fact>(); // proven facts
        private HashSet<Fact> support_area = new HashSet<Fact>();  // non terminal 
        public Form1()
        {
            InitializeComponent();
            ForwardReasoningButton.Checked = true;
        }

        private void reset_controls() {
            foreach (Control c in flowLayoutPanel1.Controls)
                c.Dispose();
            flowLayoutPanel1.Controls.Clear();
            foreach (Fact f in all_userfacts) {
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
            List<string> lines = System.IO.File.ReadLines(filename, Encoding.UTF8).ToList();
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
            selected_userfacts.Clear();
            all_userfacts.Clear();
            terminals.Clear();
            Rules.Clear();
            work_area.Clear();
            support_area.Clear();
            foreach (Fact f in facts.Values)
            {
                all_userfacts.Add(f);
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
            //checkBox1.Checked = true;
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
            work_area = new HashSet<Fact>(selected_userfacts.Select(x => new Fact(x)));
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

        private List<TerminalFact> confidence_forward() {
            double THRESHOLD = 0.05;
            selected_userfacts = all_userfacts.Select(f => {
                Fact r = new Fact(f)
                {
                    weight = (double)f.cntrl.FactValueControl.Value / 100
                }; return r;
            }).ToList();


            Dictionary<Rule, double> applyable = Rules.ToDictionary(r => r, r => 0.0);
            HashSet<Rule> used = new HashSet<Rule>();
            List<TerminalFact> res = new List<TerminalFact>();
            HashSet<Fact> all_facts = new HashSet<Fact>(support_area.Union(terminals).Select(f => new Fact(f) { weight = 0 }));
            all_facts.UnionWith(selected_userfacts);



            double RuleConditionWeight(Rule r) {
                var new_cond = all_facts.Where(ft =>  r.condition.Select(f => f.id).Contains(ft.id));
                double res_weight = 0;
                if (new_cond.Count() > 0) {
                    res_weight = r.weight * new_cond.Min(f => f.weight);
                }
                return res_weight;
            }



            applyable = applyable.ToDictionary(item => item.Key, item => RuleConditionWeight(item.Key));
            while (applyable.Count(item => item.Value>=THRESHOLD) >0)
            {


                if (applyable.Count <=0) {
                    break;
                }

                applyable = applyable.ToDictionary(item => item.Key, item => RuleConditionWeight(item.Key));

                Rule rule_toapply = applyable.Keys.OrderByDescending(key => applyable[key]).ThenByDescending(key => key.weight).First();
                if (applyable[rule_toapply] < THRESHOLD)
                    break;
                var new_result = all_facts.Where(ft => rule_toapply.result.Select(f => f.id).Contains(ft.id));
                double res_conf = applyable[rule_toapply];
                // 
                var panel = panel_factory();
                panel.Controls.Add(label_factory("Applied Rule:"));
                panel.Controls.Add(label_factory(String.Format("{0} confidence: {1}",rule_toapply,res_conf)));
                ThoughtLinePanel.Controls.Add(panel);
                //
                panel = panel_factory();
                panel.Controls.Add(label_factory("New Facts:"));
                foreach (Fact f in new_result) {

                    f.weight = f.weight + res_conf - res_conf * f.weight;
                    string text = f.text + "conf: " + f.weight;
                    Color c = Color.Black;
                    TerminalFact old_term = terminals.Where(t => t.id == f.id).FirstOrDefault();
                    if (old_term != null) {
                        TerminalFact tf =new TerminalFact(old_term) { weight = f.weight };                      
                        res.Add(tf);
                        c = Color.Green;
                    }

                    panel.Controls.Add(label_factory(text,c));



                }
                ThoughtLinePanel.Controls.Add(panel);

                applyable.Remove(rule_toapply);
                used.Add(rule_toapply);
                
            }

            return res;


        }



        private void resolve(Node n)
        {
            if (n.flag)
                return;
            if (n is AndNode) 
                n.flag = n.children.All(c => c.flag == true);

            if(n is OrNode)
                n.flag = n.children.Any(c => c.flag == true);

            if (n.flag)
            {
                foreach (Node p in n.parents)
                    resolve(p);
            }
        }

        private Dictionary<TerminalFact, Tuple<int, double>> backward()
        {
            ThoughtLinePanel.Controls.Clear();
            Dictionary<TerminalFact, Tuple<int, double>> res = new Dictionary<TerminalFact, Tuple<int,double>>();
            foreach (TerminalFact term in terminals)
            {
                Dictionary<Rule, AndNode> and_dict = new Dictionary<Rule, AndNode>();
                Dictionary<Fact, OrNode> or_dict = new Dictionary<Fact, OrNode>();
                OrNode root = new OrNode(term);
                or_dict.Add(term, root);

                Stack<Node> tree = new Stack<Node>();
                tree.Push(root);
                while (tree.Count != 0)
                {
                    Node cur = tree.Pop();
                    if (cur is AndNode)
                    {
                        AndNode n = cur as AndNode;
                        foreach (Fact f in n.rule.condition)
                            if (or_dict.ContainsKey(f))
                            {
                                cur.children.Add(or_dict[f]);
                                or_dict[f].parents.Add(cur);
                            }
                            else
                            {
                                or_dict.Add(f, new OrNode(f));
                                n.children.Add(or_dict[f]);
                                or_dict[f].parents.Add(n);
                                tree.Push(or_dict[f]);
                            }
                    }
                    if(cur is OrNode)
                    {
                        OrNode n = cur as OrNode;
                        foreach (Rule rl in Rules.Where(r => r.result.Contains(n.fact)))
                            if (and_dict.ContainsKey(rl))
                            {
                                cur.children.Add(and_dict[rl]);
                                and_dict[rl].parents.Add(cur);
                            }
                            else
                            {
                                and_dict.Add(rl, new AndNode(rl));
                                n.children.Add(and_dict[rl]);
                                and_dict[rl].parents.Add(n);
                                tree.Push(and_dict[rl]);
                            }
                    }
                }

                int cnt = 0;
                foreach (var f in or_dict)
                {
                    if (selected_userfacts.Contains(f.Key))
                        ++cnt;
                }

                var person = panel_factory();
                person.Controls.Add(label_factory(root.fact.text + ": ", Color.Green));
                foreach (var val in or_dict)
                    if (selected_userfacts.Contains(val.Key))
                    {
                        val.Value.flag = true;
                        foreach (Node p in val.Value.parents)
                            resolve(p);
                        if (root.flag == true)
                        {
                            person.Controls.Add(label_factory(val.Key.text, Color.Red));
                            res.Add(root.fact as TerminalFact, Tuple.Create(cnt,1.0));
                            break;
                        }
                        else {
                            person.Controls.Add(label_factory(val.Key.text));
                        }
                    }
                if (root.flag)
                {
                    ThoughtLinePanel.Controls.Add(person);
                }
                else {
                    int resolved=0;
                    int all = 0;
                    foreach (Node n in root.children) {
                        all += n.children.Count();
                        resolved += n.children.Count(r => r.flag);
                    }
                    res.Add(root.fact as TerminalFact, Tuple.Create(cnt, (double)resolved / all));

                }
            
         }

            return res;
        }

        private void Run_Clicked(object sender, EventArgs e)
        {

            if (ForwardReasoningButton.Checked)
            {
                ThoughtLinePanel.Controls.Clear();
                selected_userfacts = all_userfacts.Where(f => f.cntrl.FactValueControl.Value >= 50).ToList();
                List<TerminalFact> res = forward();
                if (res.Count != 0)
                {
                    TerminalFact best = res.First();
                    label1.Text = best.text;
                    pictureBox1.ImageLocation = best.img;
                }
                else
                {

                    List<Tuple<TerminalFact, double>> guesses = new List<Tuple<TerminalFact, double>>();
                    foreach (TerminalFact t in terminals)
                    {
                        double val;
                        var r = Rules.Where(cr => cr.result.Contains(t) && cr.condition.Any(cf => work_area.Contains(cf)))
                                     .Select(cr => (double)cr.condition.Intersect(work_area).Count() / cr.condition.Count());
                        if (r.Count() == 0)
                            val = 0;
                        else
                            val = r.Max();
                        guesses.Add(new Tuple<TerminalFact, double>(t, val));
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
                        foreach (var g in guesses)
                        {
                            p.Controls.Add(label_factory(g.Item1.ToString() + " prb:%" + (g.Item2 * 100).ToString("N1")));
                        }
                        ThoughtLinePanel.Controls.Add(p);
                    }

                }
            }
            else if (FCbutton.Checked)
            {
                ThoughtLinePanel.Controls.Clear();
                //selected_userfacts = all_userfacts.Where(f => f.cntrl.FactValueControl.Value == 1).ToList();
                List<TerminalFact> res = confidence_forward();
                if (res.Count != 0)
                {
                    res = res.OrderByDescending(f => f.weight).ToList();
                    TerminalFact best = res.First();
                    label1.Text = best.text+ "%" + (best.weight * 100).ToString("N1");
                    pictureBox1.ImageLocation = best.img;
                    var panel = panel_factory();
                    panel.Controls.Add(label_factory("Guesses:"));
                    foreach (var f in res) {
                        panel.Controls.Add(label_factory(f.text + "%" + (f.weight * 100).ToString("N1")));
                    }
                    ThoughtLinePanel.Controls.Add(panel);
                }
                else
                {
                    
                    label1.Text = "UNKNOWN";
                    pictureBox1.ImageLocation = "https://cdn-images-1.medium.com/max/800/1*Km98PgzRp9yRYfVZeSzwzQ.png";
                }
            }
            else if (BackwardReasoningButton.Checked)
            {
                selected_userfacts = all_userfacts.Where(f => f.cntrl.FactValueControl.Value >= 50).ToList();
                Dictionary<TerminalFact, Tuple<int, double>> res = backward();
                if (res.Count != 0)
                {
                    TerminalFact best_guess = null;
                    TerminalFact best_result = null;
                    var guess = panel_factory();
                    guess.Controls.Add(label_factory("Guesses: ",Color.Red));
                    var guessed_terminals = res.Where(r => r.Value.Item2 != 1.0 && r.Value.Item2 >0.0).OrderByDescending(r => r.Value.Item2).ThenByDescending(r => r.Value.Item1).ToList();
                    
                    foreach (var g in guessed_terminals) {
                        guess.Controls.Add(label_factory(g.Key.text + " prb:%" + (g.Value.Item2*100).ToString("N1")));
                    }
                    if (guessed_terminals.Count > 0) {
                        ThoughtLinePanel.Controls.Add(guess);
                        best_guess = guessed_terminals.First().Key as TerminalFact; 
                    }

                    var result = panel_factory();
                    result.Controls.Add(label_factory("Results: ", Color.Green));
                    var resolved = res.Where(r => r.Value.Item2 == 1.0).OrderByDescending(r => r.Value.Item1).ToList();
                    foreach (var r in resolved)
                    {
                        result.Controls.Add(label_factory(r.Key.text));
                    }

                    if (resolved.Count > 0)
                    {
                        ThoughtLinePanel.Controls.Add(result);
                        best_result = resolved.First().Key as TerminalFact;
                    }

                    if (best_result != null)
                    {
                        pictureBox1.ImageLocation = best_result.img;
                        label1.Text = best_result.text;

                    }
                    else if (best_guess != null)
                    {
                        pictureBox1.ImageLocation = best_guess.img;
                        label1.Text = best_guess.text;
                    }
                    else {
                        label1.Text = "UNKNOWN";
                        pictureBox1.ImageLocation = "https://cdn-images-1.medium.com/max/800/1*Km98PgzRp9yRYfVZeSzwzQ.png";
                    }


                    
                }
                else
                {
                    label1.Text = "UNKNOWN";
                    pictureBox1.ImageLocation = "https://cdn-images-1.medium.com/max/800/1*Km98PgzRp9yRYfVZeSzwzQ.png";

                }
            }
            else
            {
                return;
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

        public Fact(string _id, string _text, double w = 1.0)
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
        public double weight = 1;
        public HashSet<Fact> condition;
        public HashSet<Fact> result;

        public Rule() { }

        public Rule(string _id, Fact[] cond, Fact[] res, double w = 1.0)
        {
            id = _id.Trim();
            weight = w;
            condition = new HashSet<Fact>();
            foreach (var c in cond)
                condition.Add(c);
            result = new HashSet<Fact>();
            foreach (var r in res)
                result.Add(r);
        }

        public override string ToString()
        {
            return id + ": " + string.Join(",", condition.Select(f => f.id)) + " -> " + string.Join(",", result.Select(f => f.id));
        }
            

    }

    class Node
    {
        public List<Node> parents = new List<Node>();
        public List<Node> children = new List<Node>();
        public bool flag = false;

        public Node() { }
    }

    class AndNode : Node
    {
        public Rule rule = new Rule();

        public AndNode()
        { }      
        
        public AndNode(Rule v)
        {
            rule = v;
        }
    }

    class OrNode : Node
    {
        public Fact fact = new Fact();

        public OrNode()
        { }

        public OrNode(Fact v)
        {
            fact = v;
        }
    }
}
