using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Net.Http;

namespace BS.to_Tools
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadConfig();
            SetConfig();
        }
        private void btn_x_Click(object sender, RoutedEventArgs e)
        {
            // Schliessen
            Close();
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Fenster verschieben
            this.DragMove();
        }

        public static readonly HttpClient httpClient = new HttpClient();
        public string[] config = new string[255];
        public List<string> bookmarks_scan = new List<string>();
        public int homepagelinks = 20;
        public int index = 0;
        public string link = "";
        public string site = "";
        public string cbtext = "";

        public void LoadStandardConfig()
        {
            config[0] = "filter1=Anime";
            config[1] = "filter2=";
            config[2] = "filter3=";
            config[3] = "und/oder/nicht=oder";
            config[4] = "mehrfach/einzel=mehrfach";
            config[5] = "immer_erste_staffel=false";
        }

        public void LoadConfig()
        {
            if (File.Exists("config.txt"))
            {
                config = File.ReadAllLines("config.txt", Encoding.UTF8);
            }
            else
            {
                LoadStandardConfig();
                File.WriteAllLines("config.txt", config, Encoding.UTF8);
                config = File.ReadAllLines("config.txt", Encoding.UTF8);
            }

            if (File.Exists("bookmarks_scan.txt"))
            {
                var bookmarks_tmp = File.ReadAllLines("bookmarks_scan.txt", Encoding.UTF8);

                foreach (var bookmark in bookmarks_tmp)
                {
                    if (Regex.IsMatch(bookmark, @"bs\.to/serie/"))
                    {
                        bookmarks_scan.Add(bookmark);
                    }
                }

                if (bookmarks_scan.Count > 0)
                {
                    txt_know.Text = $"{bookmarks_scan.Count} Bookmarks ausgelesen..";
                    txt_know.Visibility = Visibility.Visible;
                    tb_know.Visibility = Visibility.Visible;
                }
                else
                {
                    txt_know.Visibility = Visibility.Collapsed;
                    tb_know.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                File.WriteAllLines("bookmarks_scan.txt", bookmarks_scan, Encoding.UTF8);
            }
        }

        public void SetConfig()
        {
            string[] conf = new string[config.Length];
            string[] items = new string[cb_1.Items.Count];
            ComboBoxItem[] cb_items = new ComboBoxItem[items.Length];

            for (int i = 0; i < conf.Length; i++)
            {
                conf[i] = Regex.Replace(config[i], @".*=([^\n]+)?", "$1");
            }

            for (int i = 0; i < cb_1.Items.Count; i++)
            {
                var item = cb_1.Items.GetItemAt(i) as ComboBoxItem;
                if (item != null)
                {
                    items[i] = item.Content?.ToString() ?? "";
                }
                else
                {
                    items[i] = "";
                }
            }

            cb_2.Items.Clear();
            cb_3.Items.Clear();
            foreach (var item in items)
            {
                cb_2.Items.Add(item);
                cb_3.Items.Add(item);
            }

            for (int i = 0; i < cb_items.Length; i++)
            {
                if (conf[0] == items[i])
                {
                    cb_1.SelectedIndex = i;
                }
                else if (conf[1] == items[i])
                {
                    cb_2.SelectedIndex = i;
                }
                else if (conf[2] == items[i])
                {
                    cb_3.SelectedIndex = i;
                }
            }

            if (conf[3] == "und")
            {
                rb_and.IsChecked = true;
            }
            else if (conf[3] == "nicht")
            {
                rb_not.IsChecked = true;
            }

            if (conf[4] == "einzel")
            {
                rb_single.IsChecked = true;
            }

            if (conf[5] == "true")
            {
                chb_first.IsChecked = true;
            }
        }

        public void SaveConfig()
        {
            string filter1 = cb_1.Text;
            string filter2 = cb_2.Text;
            string filter3 = cb_3.Text;

            config[0] = "filter1=" + filter1;

            if (filter2 != null)
            {
                config[1] = "filter2=" + filter2;
            }
            else
            {
                config[1] = "filter2=";
            }

            if (filter3 != null)
            {
                config[2] = "filter3=" + filter3;
            }
            else
            {
                config[2] = "filter3=";
            }

            if (rb_and.IsChecked == true)
            {
                config[3] = "und/oder/nicht=und";
            }
            else if (rb_or.IsChecked == true)
            {
                config[3] = "und/oder/nicht=oder";
            }
            else
            {
                config[3] = "und/oder/nicht=nicht";
            }

            if (rb_all.IsChecked == true)
            {
                config[4] = "mehrfach/einzel=mehrfach";
            }
            else
            {
                config[4] = "mehrfach/einzel=einzel";
            }

            if (chb_first.IsChecked == true)
            {
                config[5] = "immer_erste_staffel=true";
            }
            else
            {
                config[5] = "immer_erste_staffel=false";
            }

            if (File.Exists("config.txt"))
            {
                bool checknew = false;

                for (int i = 0; i < config.Length; i++)
                {
                    if (config[i] != File.ReadAllLines("config.txt", Encoding.UTF8)[i])
                    {
                        checknew = true;
                    }
                }
                if (checknew)
                {
                    File.WriteAllLines("config.txt", config, Encoding.UTF8);
                }
            }
        }

        private void btn_tab1_click(object sender, RoutedEventArgs e)
        {
            tab2.Visibility = Visibility.Collapsed;
            tab3.Visibility = Visibility.Collapsed;
            tab1.Visibility = Visibility.Visible;
            btn_tab2.Foreground = Brushes.Gray;
            //btn_tab3.Foreground = Brushes.Gray;
            btn_tab1.Foreground = Brushes.Black;
        }
        private void btn_tab2_click(object sender, RoutedEventArgs e)
        {
            tab1.Visibility = Visibility.Collapsed;
            tab3.Visibility = Visibility.Collapsed;
            tab2.Visibility = Visibility.Visible;
            btn_tab1.Foreground = Brushes.Gray;
            //btn_tab3.Foreground = Brushes.Gray;
            btn_tab2.Foreground = Brushes.Black;
        }
        private void btn_tab3_click(object sender, RoutedEventArgs e)
        {
            tab1.Visibility = Visibility.Collapsed;
            tab2.Visibility = Visibility.Collapsed;
            tab3.Visibility = Visibility.Visible;
            btn_tab1.Foreground = Brushes.Gray;
            btn_tab2.Foreground = Brushes.Gray;
            //btn_tab3.Foreground = Brushes.Black;
        }

        // Neue Folgen:
        public async Task<string[]> RegexExtractAsync()
        {
            string pattern1 = @".*(\t\t\t\t\t\t<a href=serie[^\n]+class=[^\n]+[\n][^\n]+[\n][^\n]+[\n][^\n]+(\s\s[^\n]+)[\n][^\<]+<a href=serie[^\n]+class=[^\n]+[\n][^\n]+[\n][^\n]+[\n][^\n]+[\n][^\<]+<a href=serie[^\n]+class=[^\n]+[\n][^\n]+[\n][^\n]+[\n][^\n]+[\n][^\<]+<a href=serie[^\n]+class=[^\n]+[\n][^\n]+[\n][^\n]+[\n][^\n]+[\n][^\<]+<a href=serie[^\n]+class=[^\n]+[\n][^\n]+[\n][^\n]+[\n][^\n]+[\n][^\<]+<a href=serie[^\n]+class=[^\n]+[\n][^\n]+[\n][^\n]+[\n][^\n]+[\n][^\<]+<a href=serie[^\n]+class=[^\n]+[\n][^\n]+[\n][^\n]+[\n][^\n]+[\n][^\<]+<a href=serie[^\n]+class=[^\n]+[\n][^\n]+[\n][^\n]+[\n][^\n]+[\n][^\<]+<a href=serie[^\n]+class=[^\n]+[\n][^\n]+[\n][^\n]+[\n][^\n]+[\n][^\<]+<a href=serie[^\n]+class=[^\n]+[\n][^\n]+[\n][^\n]+[\n][^\n]+[\n][^\<]+<a href=serie[^\n]+class=[^\n]+[\n][^\n]+[\n][^\n]+[\n][^\n]+[\n][^\<]+<a href=serie[^\n]+class=[^\n]+[\n][^\n]+[\n][^\n]+[\n][^\n]+[\n][^\<]+<a href=serie[^\n]+class=[^\n]+[\n][^\n]+[\n][^\n]+[\n][^\n]+[\n][^\<]+<a href=serie[^\n]+class=[^\n]+[\n][^\n]+[\n][^\n]+[\n][^\n]+[\n][^\<]+<a href=serie[^\n]+class=[^\n]+[\n][^\n]+[\n][^\n]+[\n][^\n]+[\n][^\<]+<a href=serie[^\n]+class=[^\n]+[\n][^\n]+[\n][^\n]+[\n][^\n]+[\n][^\<]+<a href=serie[^\n]+class=[^\n]+[\n][^\n]+[\n][^\n]+[\n][^\n]+[\n][^\<]+<a href=serie[^\n]+class=[^\n]+[\n][^\n]+[\n][^\n]+[\n][^\n]+[\n][^\<]+<a href=serie[^\n]+class=[^\n]+[\n][^\n]+[\n][^\n]+[\n][^\n]+[\n][^\<]+<a href=serie[^\n]+class=[^\n]+[\n][^\n]+[\n][^\n]+[\n][^\<]+)[^\n]+.*";
            string pattern2 = @"\t\t\t\t\t\t<a href=serie(/[^/]+/\d+/)[^\n]+[\n][^\n]+info>([^<]+)[^\n]+title=([^>]+)[^\n]+[\n][^\n]+[\n][^\n]+";
            string[] extracts = new string[255];
            string[] urls = new string[255];
            string[] webdata = new string[255];
            string input = "404";
            string cb_1_3 = "(" + cb_1.Text;

            try
            {
                input = await httpClient.GetStringAsync("https://bs.to");
            }
            catch (Exception)
            {
                input = "404";
            }

            // Vorbearbeitung
            input = Regex.Replace(input, "\"", "", RegexOptions.Singleline);

            // Relevanten Text rausziehen
            input = Regex.Replace(input, pattern1, "$1$2", RegexOptions.Singleline);

            // Relevanten Text -> Aufrufbare URL's; (ohne LineBrake)
            string extractURL = Regex.Replace(input, pattern2, @"https://bs.to/serie$1de;", RegexOptions.Singleline);
            extractURL = Regex.Replace(extractURL, @"\n+", "", RegexOptions.Singleline);

            // Relevanten Text -> URL;Staffel Episode;Sprache; (ohne LineBrake)
            string extract = Regex.Replace(input, pattern2, @"https://bs.to/serie$1de;$2;$3;", RegexOptions.Singleline);
            extract = Regex.Replace(extract, @"\n+", "", RegexOptions.Singleline);

            if (rb_or.IsChecked == true)
            {
                if (cb_2.Text != "" && cb_3.Text != "")
                {
                    cb_1_3 += "|" + cb_2.Text + "|" + cb_3.Text + ")";
                }
                else if (cb_2.Text != "")
                {
                    cb_1_3 += "|" + cb_2.Text + ")";
                }
                else if (cb_3.Text != "")
                {
                    cb_1_3 += "|" + cb_3.Text + ")";
                }
                else
                {
                    cb_1_3 += ")";
                }

                for (int i = 0; i < homepagelinks; i++)
                {
                    // Oberste URL aus String(extractURLs) in Array[i](urls) laden
                    urls[i] = Regex.Replace(extractURL, @"(https://bs.to/serie/[^;]+);.*", "$1", RegexOptions.Singleline);

                    // Überprüfe Webseite von urls[i] auf String(cb_1_3)
                    try
                    {
                        webdata[i] = await httpClient.GetStringAsync(urls[i]);
                    }
                    catch (Exception)
                    {
                        webdata[i] = "404";
                    }
                    if (Regex.IsMatch(webdata[i], cb_1_3, RegexOptions.Singleline))
                    {
                        // Oberste URL aus String(extract) in Array[i](extracts) laden
                        extracts[i] = Regex.Replace(extract, @"(https://bs.to/serie/[^;]+;[^;]+;[^;]+;).*", "$1", RegexOptions.Singleline);
                    }
                    else
                    {
                        extracts[i] = "";
                    }

                    // Erste URL aus String(extractURLs) entfernen
                    extractURL = extractURL.Substring(Regex.Match(extractURL, @"https://bs.to/serie/[^;]+;").Index + Regex.Match(extractURL, @"https://bs.to/serie/[^;]+;").Length);

                    // Erste URL aus String(extract) entfernen
                    extract = extract.Substring(Regex.Match(extract, @"https://bs.to/serie/[^;]+;[^;]+;[^;]+;").Index + Regex.Match(extract, @"https://bs.to/serie/[^;]+;[^;]+;[^;]+;").Length);
                }
            }
            else if (rb_not.IsChecked == true)
            {
                if (cb_2.Text != "" && cb_3.Text != "")
                {
                    cb_1_3 += "|" + cb_2.Text + "|" + cb_3.Text + ")";
                }
                else if (cb_2.Text != "")
                {
                    cb_1_3 += "|" + cb_2.Text + ")";
                }
                else if (cb_3.Text != "")
                {
                    cb_1_3 += "|" + cb_3.Text + ")";
                }
                else
                {
                    cb_1_3 += ")";
                }

                for (int i = 0; i < homepagelinks; i++)
                {
                    // Oberste URL aus String(extractURLs) in Array[i](urls) laden
                    urls[i] = Regex.Replace(extractURL, @"(https://bs.to/serie/[^;]+);.*", "$1", RegexOptions.Singleline);

                    // Überprüfe Webseite von urls[i] auf String(cb_1_3)
                    try
                    {
                        webdata[i] = await httpClient.GetStringAsync(urls[i]);
                    }
                    catch (Exception)
                    {
                        webdata[i] = "404";
                    }
                    if (Regex.IsMatch(webdata[i], cb_1_3, RegexOptions.Singleline))
                    {
                        extracts[i] = "";
                    }
                    else
                    {
                        // Oberste URL aus String(extract) in Array[i](extracts) laden
                        extracts[i] = Regex.Replace(extract, @"(https://bs.to/serie/[^;]+;[^;]+;[^;]+;).*", "$1", RegexOptions.Singleline);
                    }

                    // Erste URL aus String(extractURLs) entfernen
                    extractURL = extractURL.Substring(Regex.Match(extractURL, @"https://bs.to/serie/[^;]+;").Index + Regex.Match(extractURL, @"https://bs.to/serie/[^;]+;").Length);

                    // Erste URL aus String(extract) entfernen
                    extract = extract.Substring(Regex.Match(extract, @"https://bs.to/serie/[^;]+;[^;]+;[^;]+;").Index + Regex.Match(extract, @"https://bs.to/serie/[^;]+;[^;]+;[^;]+;").Length);
                }
            }
            else if (rb_and.IsChecked == true)
            {
                for (int i = 0; i < homepagelinks; i++)
                {
                    // Oberste URL aus String(extractURLs) in Array[i](urls) laden
                    urls[i] = Regex.Replace(extractURL, @"(https://bs.to/serie/[^;]+);.*", "$1", RegexOptions.Singleline);

                    // Überprüfe Webseite von urls[i] auf String(cb_1_3)
                    webdata[i] = await httpClient.GetStringAsync(urls[i]);

                    if (cb_2.Text != "" && cb_3.Text != "")
                    {
                        if (Regex.IsMatch(webdata[i], cb_1.Text, RegexOptions.Singleline) && Regex.IsMatch(webdata[i], cb_2.Text, RegexOptions.Singleline) && Regex.IsMatch(webdata[i], cb_3.Text, RegexOptions.Singleline))
                        {
                            // Oberste URL aus String(extract) in Array[i](extracts) laden
                            extracts[i] = Regex.Replace(extract, @"(https://bs.to/serie/[^;]+;[^;]+;[^;]+;).*", "$1", RegexOptions.Singleline);
                        }
                        else
                        {
                            extracts[i] = "";
                        }
                    }
                    else if (cb_2.Text != "")
                    {
                        if (Regex.IsMatch(webdata[i], cb_1.Text, RegexOptions.Singleline) && Regex.IsMatch(webdata[i], cb_2.Text, RegexOptions.Singleline))
                        {
                            // Oberste URL aus String(extract) in Array[i](extracts) laden
                            extracts[i] = Regex.Replace(extract, @"(https://bs.to/serie/[^;]+;[^;]+;[^;]+;).*", "$1", RegexOptions.Singleline);
                        }
                        else
                        {
                            extracts[i] = "";
                        }
                    }
                    else if (cb_3.Text != "")
                    {
                        if (Regex.IsMatch(webdata[i], cb_1.Text, RegexOptions.Singleline) && Regex.IsMatch(webdata[i], cb_3.Text, RegexOptions.Singleline))
                        {
                            // Oberste URL aus String(extract) in Array[i](extracts) laden
                            extracts[i] = Regex.Replace(extract, @"(https://bs.to/serie/[^;]+;[^;]+;[^;]+;).*", "$1", RegexOptions.Singleline);
                        }
                        else
                        {
                            extracts[i] = "";
                        }
                    }
                    else
                    {
                        cb_1_3 = cb_1.Text;

                        if (Regex.IsMatch(webdata[i], cb_1_3, RegexOptions.Singleline))
                        {
                            // Oberste URL aus String(extract) in Array[i](extracts) laden
                            extracts[i] = Regex.Replace(extract, @"(https://bs.to/serie/[^;]+;[^;]+;[^;]+;).*", "$1", RegexOptions.Singleline);
                        }
                        else
                        {
                            extracts[i] = "";
                        }
                    }

                    // Erste URL aus String(extractURLs) entfernen
                    extractURL = extractURL.Substring(Regex.Match(extractURL, @"https://bs.to/serie/[^;]+;").Index + Regex.Match(extractURL, @"https://bs.to/serie/[^;]+;").Length);

                    // Erste URL aus String(extract) entfernen
                    extract = extract.Substring(Regex.Match(extract, @"https://bs.to/serie/[^;]+;[^;]+;[^;]+;").Index + Regex.Match(extract, @"https://bs.to/serie/[^;]+;[^;]+;[^;]+;").Length);
                }
            }

            // Array(extracts) übergeben
            return extracts;
        }

        private async void btn_list_Click(object sender, RoutedEventArgs e)
        {
            string[] extracts = new string[255];
            bool[] know = new bool[255];
            int n = 1;

            try
            {
                extracts = await RegexExtractAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ein Fehler ist aufgetreten: {ex.Message}");
            }

            txt_nr.Clear();
            txt_data.Clear();
            txt_know.Clear();
            cb_meny.Items.Clear();

            for (int i = 0; i < homepagelinks; i++)
            {
                if (extracts[i] != "")
                {
                    txt_nr.Text += n.ToString() + "\n\n\n";
                    txt_data.Text += Regex.Replace(extracts[i], @"https://bs.to/serie/([^/]+)[^;]+;S?(\d+)?\s?E?(\d+);([^;]+);", "$1\nStaffel: $2 - Episode: $3 | $4") + "\n\n";

                    foreach (string bookmark in bookmarks_scan)
                    {
                        if (Regex.Replace(extracts[i], @"https://(bs.to/serie/[^/]+).*", "$1") == Regex.Replace(bookmark, @"https://(bs.to/serie/[^/]+).*", "$1"))
                        {
                            know[i] = true;
                        }
                    }
                    if (know[i])
                    {
                        txt_know.Text += "Ja" + "\n\n\n";
                    }
                    else
                    {
                        txt_know.Text += "Nein" + "\n\n\n";
                    }

                    cb_meny.Items.Add(n);
                    n++;
                }
                pb_listing.Value++;
            }
            pb_listing.Value = 0;

            if (cb_meny.SelectedItem == null)
            {
                cb_meny.SelectedIndex = n - 2;
            }
            SaveConfig();
        }

        private async void btn_open_all_Click(object sender, RoutedEventArgs e)
        {
            string[] sites = new string[255];
            int[] nr = new int[255];
            int meny = 0;
            int n = 1;

            try
            {
                sites = await RegexExtractAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ein Fehler ist aufgetreten: {ex.Message}");
            }

            if (cb_meny.Text.Length > 0)
            {
                meny = Convert.ToInt32(cb_meny.Text);
            }
            for (int i = 0; i < homepagelinks; i++)
            {
                if (sites[i] != "")
                {
                    if (meny > 0 && rb_all.IsChecked == true)
                    {
                        if (chb_first.IsChecked == true)
                        {
                            System.Diagnostics.Process.Start(@"explorer.exe", Regex.Replace(sites[i], @"(https://bs.to/serie/[^/]+)/\d+([^;]+);.*", @"$1/1$2")).WaitForExit(5000);
                        }
                        else
                        {
                            System.Diagnostics.Process.Start(@"explorer.exe", Regex.Replace(sites[i], @"(https://bs.to/serie/[^;]+);.*", "$1")).WaitForExit(5000);
                        }
                        meny--;
                    }
                    nr[n] = i;
                    n++;
                }
                pb_opening.Value++;
            }
            pb_opening.Value = 0;
            if (meny > 0 && rb_all.IsChecked == false)
            {
                if (chb_first.IsChecked == true)
                {
                    System.Diagnostics.Process.Start(@"explorer.exe", Regex.Replace(sites[nr[meny]], @"(https://bs.to/serie/[^/]+)/\d+([^;]+);.*", @"$1/1$2")).WaitForExit(5000);
                }
                else
                {
                    System.Diagnostics.Process.Start(@"explorer.exe", Regex.Replace(sites[nr[meny]], @"(https://bs.to/serie/[^;]+);.*", "$1")).WaitForExit(5000);
                }
            }
            SaveConfig();
        }

        private void cb_1_DropDownOpened(object sender, EventArgs e)
        {
            int n1 = cb_1.Items.Count;
            string[] items = new string[n1];
            ComboBoxItem[] cb_items = new ComboBoxItem[n1];

            for (int i = 0; i < n1; i++)
            {
                cb_items[i] = (ComboBoxItem)cb_1.Items.GetItemAt(i);
                items[i] = cb_items[i].Content.ToString();
            }

            for (int i = 0; i < n1; i++)
            {
                if (items[i] != cb_2.Text && items[i] != cb_3.Text)
                {
                    cb_items[i].Visibility = Visibility.Visible;
                }
                else
                {
                    cb_items[i].Visibility = Visibility.Collapsed;
                }
            }
        }

        private void cb_2_DropDownOpened(object sender, EventArgs e)
        {
            string[] cb1_items = new string[cb_1.Items.Count];

            for (int i = 0; i < cb1_items.Length; i++)
            {
                ComboBoxItem typeItem = (ComboBoxItem)cb_1.Items.GetItemAt(i);
                cb1_items[i] = typeItem.Content.ToString();
            }

            cb_2.Items.Clear();
            cb_2.Items.Add("");
            foreach (var item in cb1_items)
            {
                if (item != cb_1.Text && item != cb_3.Text)
                {
                    cb_2.Items.Add(item);
                }
            }
        }

        private void cb_3_DropDownOpened(object sender, EventArgs e)
        {
            string[] cb1_items = new string[cb_1.Items.Count];

            for (int i = 0; i < cb1_items.Length; i++)
            {
                ComboBoxItem typeItem = (ComboBoxItem)cb_1.Items.GetItemAt(i);
                cb1_items[i] = typeItem.Content.ToString();
            }

            cb_3.Items.Clear();
            cb_3.Items.Add("");
            foreach (var item in cb1_items)
            {
                if (item != cb_1.Text && item != cb_2.Text)
                {
                    cb_3.Items.Add(item);
                }
            }
        }
        // -------------------------------------------------------------------------------------------------------------------------------------------------------------

        // Link Creator:
        public string[] RegexExtractLinks()
        {
            string quelldata = "";
            quelldata += site;
            string pattern = @"(serie/[^/]+/[\d+]+/[^/]+/[^/]+/\b(?!unwatch:all|watch:all|\w+(>|\s))\w+)";
            int n = 0;

            while (Regex.IsMatch(quelldata, pattern))
            {
                quelldata = quelldata.Substring(Regex.Match(quelldata, pattern).Index + Regex.Match(quelldata, pattern).Length);
                n++;
            }

            string[] links = new string[n];
            quelldata = site;

            for (int i = 0; i < n; i++)
            {
                links[i] = Regex.Match(quelldata, pattern).ToString();
                quelldata = quelldata.Substring(Regex.Match(quelldata, links[i]).Index + Regex.Match(quelldata, links[i]).Length);
            }

            return links;
        }

        public string[] RegexExtractHoster()
        {
            string[] hosters = RegexExtractLinks();
            string allhoster = "";
            int n = 0;

            for (int i = 0; i < hosters.Length; i++)
            {
                hosters[i] = Regex.Replace(hosters[i], @".*/([^/]+)", "$1");
            }

            foreach (string host in hosters)
            {
                if (Regex.IsMatch(allhoster, host) != true)
                {
                    allhoster += host + "\n";
                    n++;
                }
            }

            string[] hoster = new string[n];
            allhoster = "";
            int ii = 0;

            foreach (string host in hosters)
            {
                if (Regex.IsMatch(allhoster, host) != true)
                {
                    allhoster += host + "\n";
                    hoster[ii] = host;
                    ii++;
                }
            }

            return hoster;
        }

        public string ExtractSite()
        {
            WebClient client = new WebClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            if (Regex.IsMatch(link, @"https?://bs\.to/serie/.*"))
            {
                string extract = client.DownloadString(link);
                return extract;
            }
            else
            {
                return "";
            }
        }

        private void btn_links_ausgeben(object sender, RoutedEventArgs e)
        {
            string output = "";
            int index = 0;

            cbtext = cb_hoster.Text;
            tb_output.Clear();
            cb_hoster.Items.Clear();
            link = tb_link.Text;
            site = ExtractSite();

            foreach (var host in RegexExtractHoster())
            {
                cb_hoster.Items.Add(host);

                if (host == cbtext)
                {
                    cb_hoster.SelectedIndex = index;
                }
                index++;
            }

            foreach (string sublink in RegexExtractLinks())
            {
                if (Regex.IsMatch(sublink, cb_hoster.Text))
                {
                    output += "https://bs.to/" + sublink + "\n";
                }
            }

            tb_output.Text = output;
        }

        private void tb_link_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            int index = 0;

            cbtext = cb_hoster.Text;
            cb_hoster.Items.Clear();
            ExtractSite();

            foreach (var host in RegexExtractHoster())
            {
                cb_hoster.Items.Add(host);

                if (host == cbtext)
                {
                    cb_hoster.SelectedIndex = index;
                }
                index++;
            }
        }
        // -------------------------------------------------------------------------------------------------------------------------------------------------------------

        // Meine Liste:



        // -------------------------------------------------------------------------------------------------------------------------------------------------------------
    }
}
