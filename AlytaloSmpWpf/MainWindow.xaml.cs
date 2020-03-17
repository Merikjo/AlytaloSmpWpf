using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AlytaloSmpWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       using System;
using System.Speech.Synthesis;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Diagnostics;


namespace ÄlytaloWpfJM
    {
        /// <summary>
        /// Interaction logic for MainWindow.xaml
        /// </summary>
        public partial class MainWindow : Window
        {
            public Lights OloHuone = new Lights(); //olio l. instanssi
            public Lights Keittio = new Lights(); //olio l. instanssi
            public Sauna HouseSauna = new Sauna();
            public Thermostat HouseHeat = new Thermostat();
            public SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();

            public char Astemerkki;
            public DateTime date1 = new DateTime(2015, 11, 15, 17, 12, 0);

            private int j = 17;

            public DispatcherTimer asteTimer = new DispatcherTimer(); // Tick 
            public DispatcherTimer asteTimer2 = new DispatcherTimer();
            public DispatcherTimer asteTimer3 = new DispatcherTimer();

            public DispatcherTimer SaunaTimer = new DispatcherTimer();
            public DispatcherTimer SaunaSammutusTimer = new DispatcherTimer();

            DispatcherTimer dtClockTime = new DispatcherTimer();

            public MediaElement MediaElement = new MediaElement();

            List<HouseStatus> HouseState = new List<HouseStatus>();

            public MainWindow()
            {
                InitializeComponent();

                AsetaDgKentat();

                OloHuone.Dimmer = "0";
                OloHuone.Switched = false;
                txtOlohuoneValo.Text = "OFF";

                Keittio.Dimmer = "0";
                Keittio.Switched = false;
                txtKeittiöValo.Text = "OFF";

                HouseSauna.SaunaPäällä(0);
                txtSaunaTila.Text = "";

                Astemerkki = Convert.ToChar(176);
                txtLämpötila.Text = "20" + Astemerkki;
                txtTavoitelämpötila.Text = "";

                //HouseHeat = new Thermostat(txtLämpötila, txtTavoitelämpötila, txbNykyLampo);


                #region saunan lämpötila tikit
                asteTimer.Tick += Lämpenee_Tick;
                asteTimer.Interval = new TimeSpan(0, 0, 1); //Tikki käy sekunnin välein

                // Saunan Ajastin
                SaunaTimer.Tick += SaunanLampo_Tick;
                SaunaTimer.Interval = new TimeSpan(0, 0, 1); //Tikki käy sekunnin välein

                // Saunan ajastin sammutus
                SaunaSammutusTimer.Tick += SaunanSammutus_Tick;
                SaunaSammutusTimer.Interval = new TimeSpan(0, 0, 1); //Tikki käy sekunnin välein

                HouseHeat.SaunaTemperature = lblHouseThermo.Content.ToString();

                HouseSauna.Lampotila = HouseHeat.Temperature;
                lblSaunaHeat.Content = HouseHeat.Temperature.ToString();

                //asteTimer2.Tick += Lämpenee2_Tick;
                //asteTimer2.Interval = new TimeSpan(0, 0, 1);

                //asteTimer3.Tick += Lämpenee3_Tick;
                //asteTimer.Interval = new TimeSpan(0, 0, 1);
                #endregion

                #region kello
                //Kellon asetus
                dtClockTime.Interval = new TimeSpan(0, 0, 1); //in Hour, Minutes, Second.
                dtClockTime.Tick += dtClockTime_Tick;

                dtClockTime.Start();

                startclock();

                //Video
                //myMedia.Volume = 100;
                //myMedia.Play();
                #endregion
            }

            #region//Kellon asetukset
            private void startclock()
            {
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += tickevent;
                timer.Start();
            }

            private void tickevent(object sender, EventArgs e)
            {
                datelbl.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            }
            #endregion

            #region Lights

            private void btnOlohuonePois_Click(object sender, RoutedEventArgs e)
            {
                OloHuone.SwitchOff();
                txtOlohuoneValo.Text = OloHuone.Dimmer;
                speechSynthesizer.Speak("LIVING ROOM " + txtOlohuoneValo.Text);
            }

            private void btnOlohuoneHämärä_Click(object sender, RoutedEventArgs e)
            {
                OloHuone.SwitchOn(33);
                txtOlohuoneValo.Text = "HÄMÄRÄ " + OloHuone.Dimmer + " %";
                txtOlohuoneValo.Background = Brushes.Lavender;
                speechSynthesizer.Speak("LIVING ROOM " + txtOlohuoneValo.Text);
            }

            private void btnOlohuonePuolivalo_Click(object sender, RoutedEventArgs e)
            {
                OloHuone.SwitchOn(66);
                txtOlohuoneValo.Text = "PUOLIVALO " + OloHuone.Dimmer + " %";
                txtOlohuoneValo.Background = Brushes.LightBlue;
                speechSynthesizer.Speak("LIVING ROOM " + txtOlohuoneValo.Text);
            }

            private void btnOlohuoneKirkas_Click(object sender, RoutedEventArgs e)
            {
                OloHuone.SwitchOn(100);
                txtOlohuoneValo.Text = "KIRKAS " + OloHuone.Dimmer + " %";
                txtOlohuoneValo.Background = Brushes.Azure;
                speechSynthesizer.Speak("LIVING ROOM " + txtOlohuoneValo.Text);

            }

            private void btnKeittiöPois_Click(object sender, RoutedEventArgs e)
            {
                Keittio.SwitchOff();
                txtKeittiöValo.Text = Keittio.Dimmer;
                speechSynthesizer.Speak("KITCHEN " + txtKeittiöValo.Text);
            }

            private void btnKeittiöHämärä_Click(object sender, RoutedEventArgs e)
            {
                Keittio.SwitchOn(33);
                txtKeittiöValo.Text = "HÄMÄRÄ " + Keittio.Dimmer + " %";
                txtKeittiöValo.Background = Brushes.Lavender;
                speechSynthesizer.Speak("KITCHEN " + txtKeittiöValo.Text);
            }

            private void btnKeittiöPuolivalo_Click(object sender, RoutedEventArgs e)
            {
                Keittio.SwitchOn(66);
                txtKeittiöValo.Text = "PUOLIVALO " + Keittio.Dimmer + " %";
                txtKeittiöValo.Background = Brushes.LightBlue;
                speechSynthesizer.Speak("KITCHEN " + txtKeittiöValo.Text);
            }

            private void btnKeittiöKirkas_Click(object sender, RoutedEventArgs e)
            {
                Keittio.SwitchOn(100);
                txtKeittiöValo.Text = "KIRKAS " + Keittio.Dimmer + " %";
                txtKeittiöValo.Background = Brushes.Azure;
                speechSynthesizer.Speak("KITCHEN " + txtKeittiöValo.Text);
            }
            #endregion

            #region Sauna
            private void btnSaunaTila_Click(object sender, RoutedEventArgs e)
            {

                if (HouseSauna.Switched)
                {
                    HouseSauna.Lampotila = Convert.ToInt32(lblHouseThermo.Content);
                    HouseSauna.SaunaPäällä(0);
                    SaunaTimer.Stop();

                    if (HouseSauna.Lampotila > HouseHeat.Temperature)
                    {
                        SaunaSammutusTimer.Start();
                    }

                    txtSaunaTila.Text = "OFF";

                    lblSaunaHeat.Content = HouseHeat.Temperature.ToString() + Astemerkki;
                    txtSaunaTila.Background = Brushes.Silver;
                    txtSaunaTila.Foreground = Brushes.BlueViolet;
                    speechSynthesizer.Speak("SAUNA HEAT" + txtSaunaTila.Text);
                }
                else
                {

                    HouseSauna.SaunaPäällä(1);
                    SaunaTimer.Start();
                    txtSaunaTila.Text = "ON";
                    HouseSauna.Lampotila = Convert.ToInt32(lblHouseThermo.Content);
                    lblSaunaHeat.Content = HouseHeat.Temperature.ToString() + Astemerkki + 1;
                    txtSaunaTila.Background = Brushes.MistyRose;
                    txtSaunaTila.Foreground = Brushes.Red;
                    speechSynthesizer.Speak("SAUNA HEAT" + txtSaunaTila.Text);

                    //if (SaunaSammutusTimer.IsEnabled)
                    //{
                    //    SaunaSammutusTimer.Stop();
                    //}
                    //HouseSauna.SaunaOn();
                    //if (HouseSauna.Lampotila - 0.5 > HouseHeat.Temperature)
                    //{
                    //}
                    //else
                    //{
                    //    HouseSauna.Lampotila = HouseHeat.Temperature;
                    //}
                    //lblSaunaHeat.Content = HouseSauna.Lampotila;
                    //asteTimer.Start();
                    //txtSaunaTila.Text = "ON";
                    //txtSaunaTila.Background = Brushes.MistyRose;
                    //txtSaunaTila.Foreground = Brushes.Red;
                }
            }

            private void SaunanLampo_Tick(object sender, EventArgs e)
            {

                if (HouseSauna.SaunaTemperature > 0)
                {
                    lblSaunaHeat.Content = HouseSauna.Lampotila.ToString();
                    HouseHeat.SaunaTemperature = HouseSauna.Lampotila.ToString();
                    txtSaunaTila.Text = String.Empty;
                    txtSaunaTila.AppendText("SAUNAN LÄMMITYS");
                    txtSaunaTila.Background = Brushes.LightPink;
                    SaunaTimer.Start();
                }

                if (HouseSauna.SaunaTemperature > 30)
                {
                    lblSaunaHeat.Content = HouseSauna.Lampotila.ToString();
                    HouseHeat.SaunaTemperature = HouseSauna.Lampotila.ToString();
                    txtSaunaTila.Text = String.Empty;
                    txtSaunaTila.AppendText("SAUNA LÄMPENEE");
                    txtSaunaTila.Background = Brushes.LightPink;
                }
                if (HouseSauna.SaunaTemperature > 59)
                {
                    lblSaunaHeat.Content = HouseSauna.Lampotila.ToString();
                    HouseHeat.SaunaTemperature = HouseSauna.Lampotila.ToString();
                    txtSaunaTila.Text = String.Empty;
                    txtSaunaTila.AppendText("SAUNA LÄMMIN");
                    txtSaunaTila.Background = Brushes.IndianRed;
                    SaunaTimer.Stop();
                }
                HouseSauna.SaunaTemperature = HouseSauna.SaunaTemperature + 0.50;
                lblSaunaHeat.Content = HouseSauna.SaunaTemperature.ToString() + Astemerkki;
                sldSauna.Value = HouseSauna.SaunaTemperature;
            }

            private void SaunanSammutus_Tick(object sender, EventArgs e)
            {
                if (HouseSauna.SaunaTemperature <= 59)
                {
                    txtSaunaTila.Text = String.Empty;
                    txtSaunaTila.AppendText("SAUNA VIILENEE");
                    txtSaunaTila.Background = Brushes.CadetBlue;
                }
                if (HouseSauna.SaunaTemperature <= 29)
                {
                    txtSaunaTila.Text = String.Empty;
                    txtSaunaTila.AppendText("SAUNA VIILEÄ");
                    txtSaunaTila.Background = Brushes.LightBlue;
                }
                if (HouseSauna.SaunaTemperature <= 0.50)
                {
                    txtSaunaTila.Text = String.Empty;
                    txtSaunaTila.AppendText("SAUNA OFF");
                    txtSaunaTila.Background = Brushes.AliceBlue;
                    SaunaSammutusTimer.Stop();
                }
                lblSaunaHeat.Content = HouseSauna.Lampotila.ToString();
                HouseHeat.SaunaTemperature = HouseSauna.Lampotila.ToString();
                HouseSauna.SaunaTemperature = HouseSauna.SaunaTemperature - 0.50;

                sldSauna.Value = HouseSauna.SaunaTemperature;
            }

            private void Lämpenee_Tick(object sender, EventArgs e)
            {
                //Kello on määritetty tikkaamaan sekunnin välein, joka on muunnettu sleepillä, että tikki kestää 2.0sek. Myös lämpötila celsius ilmoitettu.
                HouseSauna.SaunaTemperature = HouseSauna.SaunaTemperature + 1;
                Thread.Sleep(1000);
                lblSaunaHeat.Content = HouseSauna.SaunaTemperature + Astemerkki;

                //Määrittää saunan lämpötilan pysähtymisen
                if (HouseSauna.SaunaTemperature > 59)
                {
                    txtSaunaTila.Text = String.Empty;
                    txtSaunaTila.AppendText("SAUNA LÄMMIN");
                    asteTimer.Stop();
                }
            }

            //private void Lämpenee3_Tick(object sender, EventArgs e)
            //{
            //    HouseSauna.Deg = HouseSauna.Deg + 1;
            //    Thread.Sleep(1000);
            //    lblSaunaHeat.Content = HouseSauna.Deg + Astemerkki;

            //    if (HouseSauna.Deg > 99)
            //    {
            //        txtSaunaTila.AppendText("SAUNA LÄMMIN");
            //        asteTimer3.Stop();
            //    }
            //}
            //private void Lämpenee2_Tick(object sender, EventArgs e)
            //{
            //    HouseSauna.Deg = HouseSauna.Deg + 1;
            //    Thread.Sleep(1000);
            //    lblSaunaHeat.Content = HouseSauna.Deg + Astemerkki;

            //    if (HouseSauna.Deg > 79)
            //    {
            //        txtSaunaTila.Text = String.Empty;
            //        txtSaunaTila.AppendText("SAUNA LÄMMIN");
            //        asteTimer2.Stop();
            //    }
            //}

            #endregion sauna

            #region Thermostat
            private void btnAsetaLämpötila_Click(object sender, RoutedEventArgs e)
            {
                int Tavoitelämpötila;

                try
                {
                    Tavoitelämpötila = Int32.Parse(txtTavoitelämpötila.Text); //parse = merkkijonon muuttaminen kokonaisluvuksi

                    //if ((Tavoitelämpötila >= 15) && (Tavoitelämpötila < 30))
                    //{
                    //    //virhetilanteen käsittely
                    //    if (Tavoitelämpötila == 28)
                    //    {
                    //        Tavoitelämpötila = 30;
                    //    }
                    //}

                    HouseHeat.SetTemp(Tavoitelämpötila);
                    txtLämpötila.Text = HouseHeat.Temperature.ToString() + Astemerkki;
                    txtTavoitelämpötila.Text = "";
                    lblHouseThermo.Content = HouseHeat.Temperature.ToString() + Astemerkki;
                    //speechSynthesizer.Speak("NEW TEMPERATURE " + txtLämpötila.Text);

                }
                catch
                {
                    txtTavoitelämpötila.Text = "ERROR " + "USE NUMBER VALUE";
                    //speechSynthesizer.Speak(txtTavoitelämpötila.Text);
                }
            }

            private void btnMinus_Click(object sender, RoutedEventArgs e)
            {
                j--;
                string s = j.ToString();
                txtTavoitelämpötila.Text = (s);
            }

            private void btnPlus_Click(object sender, RoutedEventArgs e)
            {
                j++;
                string s = j.ToString();
                txtTavoitelämpötila.Text = (s);
            }

            private void btnTyhjennäTeksti_Click(object sender, RoutedEventArgs e)
            {
                txtTavoitelämpötila.Clear();
            }
            #endregion

            private void dtClockTime_Tick(object sender, EventArgs e)
            {
                //lblClockTime.Content = DateTime.Now.ToLongTimeString();
                lblClockTime.Content = DateTime.Now.ToString("HH:mm:ss");
            }


            #region Video
            /*void mediaPlay (object sender, EventArgs e)
            {
                saaTanaan.Play();
            }*/
            //void mediaPlay(Object sender, EventArgs e)
            //{
            //    myMedia.Play();
            //}

            //void mediaPause(Object sender, EventArgs e)
            //{
            //    myMedia.Pause();
            //}

            //void mediaMute(Object sender, EventArgs e)
            //{

            //    if (myMedia.Volume == 100)
            //    {
            //        myMedia.Volume = 0;
            //        muteButt.Content = "Listen";
            //    }
            //    else
            //    {
            //        myMedia.Volume = 100;
            //        muteButt.Content = "Mute";
            //    }
            //}

            #endregion

            private void sldOlohuone_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
            {
                if (sender == sldOlohuone && OloHuone.Switched == true)
                {
                    OloHuone.Lumen = sldOlohuone.Value;
                    txtOlohuoneValo.Text = OloHuone.Lumen.ToString();
                    if (OloHuone.Lumen == 50)
                    {
                        txtOlohuoneValo.Background = Brushes.Gold;
                    }
                    else if (OloHuone.Lumen < 50)
                    {
                        txtOlohuoneValo.Background = Brushes.BlanchedAlmond;
                    }
                    else if (OloHuone.Lumen > 50)
                    {
                        txtOlohuoneValo.Background = Brushes.Yellow;
                    }
                }
                else if (sender == sldKeittio && Keittio.Switched == true)
                {
                    Keittio.Lumen = sldKeittio.Value;
                    txtKeittiöValo.Text = Keittio.Lumen.ToString();
                    if (Keittio.Lumen == 50)
                    {
                        txtKeittiöValo.Background = Brushes.Gold;
                    }
                    else if (Keittio.Lumen < 50)
                    {
                        txtKeittiöValo.Background = Brushes.BlanchedAlmond;
                    }
                    else if (Keittio.Lumen > 50)
                    {
                        txtKeittiöValo.Background = Brushes.Yellow;
                    }
                }
            }

            private void sldKeittio_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
            {
                if (sender == sldKeittio && Keittio.Switched == true)
                {
                    Keittio.Lumen = sldKeittio.Value;
                    txtKeittiöValo.Text = Keittio.Lumen.ToString();
                    if (Keittio.Lumen == 50)
                    {
                        txtKeittiöValo.Background = Brushes.Gold;
                    }
                    else if (Keittio.Lumen < 50)
                    {
                        txtKeittiöValo.Background = Brushes.BlanchedAlmond;
                    }
                    else if (Keittio.Lumen > 50)
                    {
                        txtKeittiöValo.Background = Brushes.Yellow;
                    }
                }
            }

            private void sldOlohuone2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
            {

            }

            private void btnToPageOne_Click(object sender, RoutedEventArgs e)
            {
                frmPages.NavigationService.Navigate(new Page1());
            }

            private void sldSauna_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
            {

            }

            private void btnHouseStatus_Click(object sender, RoutedEventArgs e)
            {
                HouseStatus tila = new HouseStatus();
                tila.lampotila = txtLämpötila.Text;

                dgHouseStatus.Items.Clear();

                if (OloHuone.Switched == true)
                {
                    tila.OlohuoneValot = "Päällä";
                    if (txtOlohuoneValo.Text == "ON")
                    {
                        tila.KirkkausOlohuone = "50";
                    }
                    else
                    {
                        tila.KirkkausOlohuone = txtOlohuoneValo.Text;
                    }
                }
                else
                {
                    tila.OlohuoneValot = "Pois";
                    tila.KirkkausOlohuone = " ";
                }
                if (Keittio.Switched == true)
                {
                    tila.KeittioValot = "Päällä";
                    if (txtKeittiöValo.Text == "ON")
                    {
                        tila.KirkkausKeittio = "50";
                    }
                    else
                    {
                        tila.KirkkausKeittio = txtKeittiöValo.Text;
                    }
                }
                else
                {
                    tila.KeittioValot = "Pois";
                    tila.KirkkausKeittio = " ";
                }

                tila.saunaAsetettuLampotila = HouseSauna.Lampotila.ToString();
                if (HouseSauna.Switched == true)
                {
                    tila.sauna = "Päällä";
                    tila.saunaLampotila = HouseSauna.saunaCurrentValue.ToString();

                }
                else
                {
                    tila.sauna = "Pois";
                }

                if (HouseSauna.Switched == false)
                {
                    tila.currentSaunaLampotila = HouseSauna.saunaRecedingValue.ToString();
                }
                else
                {
                    tila.currentSaunaLampotila = tila.saunaLampotila;
                }

                HouseState.Add(tila);
                dgHouseStatus.Items.Add(tila);

            }

            //Talon loki
            public void AsetaDgKentat()
            {
                DataGridTextColumn textColLampotila = new DataGridTextColumn();
                DataGridTextColumn textColOlohuone = new DataGridTextColumn();
                DataGridTextColumn textColOlohuoneKirkkaus = new DataGridTextColumn();
                DataGridTextColumn textColKeittio = new DataGridTextColumn();
                DataGridTextColumn textColKeittioKirkkaus = new DataGridTextColumn();
                DataGridTextColumn textColSauna = new DataGridTextColumn();
                DataGridTextColumn textColSaunaSet = new DataGridTextColumn();
                DataGridTextColumn textColSaunaTemp = new DataGridTextColumn();

                textColLampotila.Binding = new System.Windows.Data.Binding("lampotila");
                textColOlohuone.Binding = new System.Windows.Data.Binding("OlohuoneValot");
                textColOlohuoneKirkkaus.Binding = new System.Windows.Data.Binding("KirkkausOlohuone");
                textColKeittio.Binding = new System.Windows.Data.Binding("KeittioValot");
                textColKeittioKirkkaus.Binding = new System.Windows.Data.Binding("KirkkausKeittio");
                textColSauna.Binding = new System.Windows.Data.Binding("sauna");
                textColSaunaSet.Binding = new System.Windows.Data.Binding("saunaAsetettuLampotila");
                textColSaunaTemp.Binding = new System.Windows.Data.Binding("currentSaunaLampotila");

                textColLampotila.Header = "Lämpötila";
                textColOlohuone.Header = "Olohuone valo";
                textColOlohuoneKirkkaus.Header = "Kirkkaus";
                textColKeittio.Header = "Keittiö valo";
                textColKeittioKirkkaus.Header = "Kirkkaus";
                textColSauna.Header = "Sauna";
                textColSaunaSet.Header = "Saunan asetettu lämpötila";
                textColSaunaTemp.Header = "Saunan lämpötila";

                dgHouseStatus.Columns.Add(textColLampotila);
                dgHouseStatus.Columns.Add(textColOlohuone);
                dgHouseStatus.Columns.Add(textColOlohuoneKirkkaus);
                dgHouseStatus.Columns.Add(textColKeittio);
                dgHouseStatus.Columns.Add(textColKeittioKirkkaus);
                dgHouseStatus.Columns.Add(textColSauna);
                dgHouseStatus.Columns.Add(textColSaunaSet);
                dgHouseStatus.Columns.Add(textColSaunaTemp);
            }

        }
    }














