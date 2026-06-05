using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;


namespace WorkLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        string serverIp = "212.73.137.3";
        string serverPort = "22184";
        // ÚJ: Eltároljuk a szerver jelszavát
        string serverPassword = "nf2025dev";
        public MainWindow()
        {
            InitializeComponent();
            var a = Database.Connect();
            MessageBox.Show(a.ToString());
            //datagridACCs.ItemsSource = Database.GetAccounts();
        }

        // Az eseménykezelőt async-é tesszük, hogy megvárhassa a hálózati kérést

        public async Task QueryMtaServerAsync()
        {
            if (!int.TryParse(serverPort, out int port) || string.IsNullOrEmpty(serverIp))
            {
                MessageBox.Show("Érvénytelen IP cím vagy port!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            int queryPort = port + 123;

            using UdpClient udpClient = new UdpClient();
            try
            {
                udpClient.Connect(serverIp, queryPort);

                byte[] sendBytes = Encoding.ASCII.GetBytes("s");
                await udpClient.SendAsync(sendBytes, sendBytes.Length);

                using var cts = new CancellationTokenSource(2000);

                UdpReceiveResult receiveResult = await udpClient.ReceiveAsync(cts.Token);
                byte[] receiveBytes = receiveResult.Buffer;

                MTAclass.MtaServerData serverData = MTAclass.MtaAseParser.Parse(receiveBytes);

                Debug.WriteLine($"Sikeres lekérés: {serverData.ServerName}");
                Debug.WriteLine($"Játékosok: {serverData.PlayerCount}/{serverData.MaxPlayers}");

                this.Title = $"{serverData.ServerName} | {serverData.PlayerCount}/{serverData.MaxPlayers}";

                // A sikeres státuszlekérdezés után azonnal indítjuk a csatlakozást
                ConnectToMtaServer();
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("A szerver nem válaszolt időben. Offline lehet.", "Időtúllépés", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt a lekérdezés során: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ConnectToMtaServer()
        {
            try
            {
                // 1. Jelszó másolása a vágólapra
                if (!string.IsNullOrEmpty(serverPassword))
                {
                    Clipboard.SetText(serverPassword);
                    Debug.WriteLine("A szerver jelszava a vágólapra lett másolva!");
                }

                // 2. PROTOKOLL ALAPÚ INDÍTÁS
                // Nem az EXE-t hívjuk meg, hanem magát a bejegyzett mtasa:// sémát.
                // Így nincs szükség a hardkódolt mtaPath-ra, és az argumentum sem tud elveszni!
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = $"mtasa://{serverIp}:{serverPort}",
                    UseShellExecute = true // Ez kötelező, hogy a Windows kezelje a sémát
                };

                Debug.WriteLine($"Csatlakozás indítása a Windows sémán keresztül: mtasa://{serverIp}:{serverPort}");
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nem sikerült elindítani az MTA-t a protokollon keresztül: {ex.Message}",
                                "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void Connect_Click(object sender, RoutedEventArgs e)
        {
            await QueryMtaServerAsync();
            
        }
    }

}