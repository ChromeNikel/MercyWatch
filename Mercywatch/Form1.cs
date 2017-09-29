using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Mercywatch
{
    public partial class Form1 : Form
    {                
        string playersLink;
        
        public Form1()
        {
           
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                WebClientEx wc = new WebClientEx();
                OwerParser pr = new OwerParser();
                playersLink = textBox1.Text;

                if (radioButton1.Checked == true)
                {
                    playersLink = "https://www.overbuff.com/players/pc/" + playersLink.Replace('#', '-') + "?mode=competitive";
                    IHtmlDocument doc = pr.Connect(wc, playersLink);
                    int rate = pr.GetCompetetiveRate("span.color-stat-rating", doc);
                    string name = pr.GetName("div.layout-header-primary-bio", doc);
                    float winRate = pr.GetWinRate("dl:nth-child(5)", doc);
                    string tag = pr.GetTag("div.layout-header-primary-bio", doc);
                    Dictionary<string, string> dic = pr.GetHeroRank("div.group.special .stat:nth-child(1) .value span", "div.group.normal div.name a.color-white", doc);
                    string path = @"nameNtags.txt";
                    using (FileStream fstream = new FileStream(path, FileMode.Append))
                    {
                        System.IO.StreamWriter textFile = new System.IO.StreamWriter(fstream);
                        textFile.WriteLine(name + tag);
                        textFile.Close();
                    }    
                }
                else if (radioButton2.Checked == true)
                {                    
                    Collection<int> rates = new Collection<int>();
                    var namesNtags = getNameNTags(playersLink);

                    for (int i = 0; i < namesNtags.Count(); i++)
                    {
                        if (namesNtags[i] == null)
                        {
                            break;
                        }

                        playersLink = "https://www.overbuff.com/players/pc/" + namesNtags[i].Replace('#', '-') + "?mode=competitive";
                        IHtmlDocument doc = pr.Connect(wc, playersLink);
                        int rate = pr.GetCompetetiveRate("span.color-stat-rating", doc);
                        rates.Add(rate);
                        string name = pr.GetName("div.layout-header-primary-bio", doc);
                        float winRate = pr.GetWinRate("dl:nth-child(5)", doc);
                        string tag = pr.GetTag("div.layout-header-primary-bio", doc);
                        Dictionary<string, string> dic = pr.GetHeroRank("div.group.special .stat:nth-child(1) .value span", "div.group.normal div.name a.color-white", doc);

                        dataGridView2.Columns.Add(name, name);
                        if (dataGridView2.Rows.Count < 3)
                        {
                            dataGridView2.Rows.Add();
                            dataGridView2.Rows.Add();
                            dataGridView2.Rows.Add();
                        }
                        var arrdic = dic.ToArray();
                        if (arrdic.Length > 2)
                        {
                            dataGridView2.Rows[0].Cells[i].Value = rate.ToString();
                            dataGridView2.Rows[1].Cells[i].Value = arrdic[0].Key + " " + arrdic[0].Value;
                            dataGridView2.Rows[2].Cells[i].Value = arrdic[1].Key + " " + arrdic[1].Value;
                            dataGridView2.Rows[3].Cells[i].Value = arrdic[2].Key + " " + arrdic[2].Value;
                        }
                        else if (arrdic.Length > 1)
                        {
                            dataGridView2.Rows[0].Cells[i].Value = rate.ToString();
                            dataGridView2.Rows[1].Cells[i].Value = arrdic[0].Key + " " + arrdic[0].Value;
                            dataGridView2.Rows[2].Cells[i].Value = arrdic[1].Key + " " + arrdic[1].Value;
                        }
                        else
                        {
                            dataGridView2.Rows[0].Cells[i].Value = rate.ToString();
                            dataGridView2.Rows[1].Cells[i].Value = arrdic[0].Key + " " + arrdic[0].Value;
                        }
                        Application.DoEvents();
                    }
                   linkLabel1.Text = "Средний рейтинг - " + getAverageRate(rates.ToArray()).ToString();
                }
            }
            textBox1.Text = string.Empty;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateMercyAsync();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateMercyAsync();
        }

        public void UpdateMercyAsync()
        {          
            OwerParser pr = new OwerParser();
            WebClientEx wc = new WebClientEx();
            string nameNtag;
            Collection<Player> pls = new Collection<Player>();
            using (FileStream fstream = new FileStream("nameNtags.txt", FileMode.Open))
            {
                StreamReader fr = new StreamReader(fstream);
                while (!fr.EndOfStream)
                {
                    nameNtag = fr.ReadLine();
                    string playersLink = "https://www.overbuff.com/players/pc/" + nameNtag.Replace('#', '-') + "?mode=competitive";
                    IHtmlDocument doc = pr.Connect(wc, playersLink);
                    Player player = new Player();
                    player.Name = nameNtag.Substring(0, nameNtag.IndexOf('#'));
                    player.Rate = pr.GetCompetetiveRate("div.layout-header-secondary dl dd span.color-stat-rating", doc).ToString();
                    player.WinRate = pr.GetWinRate("dl:nth-child(5)", doc).ToString();
                    Dictionary<string, string> dic = pr.GetHeroRank("div.group.special .stat:nth-child(1) .value span", "div.group.normal div.name a.color-white", doc);
                    Collection<Hero> hers = new Collection<Hero>();
                    foreach (var itemdic in dic)
                    {
                        Hero her = new Hero();
                        her.Name = itemdic.Key.ToString();
                        her.WinRate = itemdic.Value;
                        hers.Add(her);
                    }
                    player.Heroes = hers;
                    pls.Add(player);
                }                
            }
            
            var names = pls.Select(c => c.Name).ToArray();
            var rates = pls.Select(c => c.Rate).ToArray();
            for (int i = 0; i < names.Length; i++)
            {
                for (int j = 0; j < names.Length; j++)
                {
                    if (Convert.ToInt32(rates[i]) > Convert.ToInt32(rates[j]))
                    {
                        var a = rates[i];
                        var b = names[i];
                        rates[i] = rates[j];
                        names[i] = names[j];
                        rates[j] = a;
                        names[j] = b;
                    }
                }
            }
            dataGridView1.Columns.Add("Рейтинг", "Рейтинг");

            if (names.Length > dataGridView1.Columns.Count - 1)
            {
                for (int i = 0; i < names.Count(); i++)
                {
                    dataGridView1.Columns.Add(names[i], names[i]);
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].HeaderCell.Value = names[i];
                }
            }           
            for (int j = 0; j < rates.Length; j++)
            {
                dataGridView1.Rows[j].Cells[0].Value = rates[j];
            }
            for (int i = 0; i < rates.Length; i++)
            {
                for (int j = 1; j < rates.Length + 1; j++)
                {
                    dataGridView1.Rows[i].Cells[j].Value = Math.Abs(Convert.ToInt32(rates[i]) - Convert.ToInt32(rates[j - 1]));

                    if (Convert.ToInt32(dataGridView1.Rows[i].Cells[j].Value) < 500)
                    {
                        dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Green;
                    }
                    else
                    {
                        dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Red;
                    }
                    if (Convert.ToInt32(dataGridView1.Rows[i].Cells[j].Value) == 0)
                    {
                        dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Black;
                        dataGridView1.Rows[i].Cells[j].Style.ForeColor = Color.Black;
                    }
                }
            }           
        }       

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                timer1.Enabled = false;
                dataGridView1.Visible = false;
                panel1.Visible = true;
                buttonAddPlayer.Text = "Добавить команду";
                button1.Visible = false;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                buttonAddPlayer.Text = "Добавить нового игрока";
                button1.Visible = true;
                timer1.Enabled = true;
                dataGridView1.Visible = true;
                panel1.Visible = false;
            }
        }

        public string[] getNameNTags(string order)
        {

            string[] arrayNames = new string[100];
            int i = 0;
            string name = "";
            while (order.IndexOf('(') != -1)
            {
                name = order.Substring(order.IndexOf('(') + 1);
                arrayNames[i] = name.Substring(0, name.IndexOf(')'));
                arrayNames[i] = arrayNames[i].Trim();
                arrayNames[i] = Uri.EscapeDataString(arrayNames[i]);
                if (name.IndexOf('\n') != -1)
                {
                    order = name.Substring(name.IndexOf('\n'));
                }
                else
                {
                    break;
                }
                i++;
            }
           
            return arrayNames;
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            UpdateMercyAsync();
        }

        public double getAverageRate(int [] rates)
        {
            if (rates.Length > 0)
            {
                return rates.Average();
            }
            else
            {
                return 0;
            }             
        }
    }

}
