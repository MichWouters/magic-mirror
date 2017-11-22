using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.WiFi;
using Windows.Networking.Connectivity;
using Windows.Security.Credentials;
using Windows.UI.Xaml.Media.Imaging;

namespace MagicMirror.UniversalApp.ViewModels
{
    public enum WiFiSettingsStatus { NotReady, Initializing, Ready, AccessDenied, Unavailable, Error }

    public class WiFiSettingsViewModel : ViewModelBase
    {
        private WiFiSettingsStatus _status;
        private WiFiAdapter _wifiAdapter;
        private WiFiNetworkViewModel _selectedWiFiNetwork;
        public ObservableCollection<WiFiNetworkViewModel> WiFiNetworks { get; private set; }

        public event EventHandler OnReady;

        public event EventHandler OnConnecting;

        public event EventHandler OnConnected;

        public event EventHandler OnDisconnected;

        public event EventHandler OnSelect;

        public event EventHandler<Exception> OnError;

        public WiFiNetworkViewModel SelectedWiFiNetwork
        {
            get { return _selectedWiFiNetwork; }
            set { _selectedWiFiNetwork = value; NotifyPropertyChanged(); }
        }

        public WiFiSettingsStatus Status
        {
            get { return _status; }
            set { _status = value; NotifyPropertyChanged(); }
        }

        public WiFiSettingsViewModel()
        {
        }

        public async void InitializeAsync()
        {
            Status = WiFiSettingsStatus.Initializing;
            WiFiNetworks = new ObservableCollection<WiFiNetworkViewModel>();
            var access = await WiFiAdapter.RequestAccessAsync();
            if (access != WiFiAccessStatus.Allowed) // Manifest aanpassen!
            {
                Status = WiFiSettingsStatus.AccessDenied;
                OnError?.Invoke(this, new UnauthorizedAccessException("WiFi access not permitted"));
                return;
            }
            var result = await DeviceInformation.FindAllAsync(WiFiAdapter.GetDeviceSelector());
            if (result.Count <= 0)// Werkt alleen met RPI >= 3
            {
                Status = WiFiSettingsStatus.Unavailable;
                OnError?.Invoke(this, new NotSupportedException("No WiFi adapters detected on this device"));
                return;
            }
            _wifiAdapter = await WiFiAdapter.FromIdAsync(result[0].Id);
            OnReady?.Invoke(this, EventArgs.Empty);
        }

        public async Task ScanNetworksAsync()
        {
            try
            {
                await _wifiAdapter.ScanAsync();
            }
            catch (Exception ex)
            {
                Status = WiFiSettingsStatus.Error;
                Debug.WriteLine(ex.Message);
            }
            await InitializeNetworksAsync(_wifiAdapter.NetworkReport);
        }

        private async Task InitializeNetworksAsync(WiFiNetworkReport networkReport)
        {
            WiFiNetworks.Clear();
            foreach (var nw in networkReport.AvailableNetworks)
            {
                var item = new WiFiNetworkViewModel(nw, _wifiAdapter);
                await item.UpdateAsync();
                if (IsConnected(nw))
                {
                    WiFiNetworks.Insert(0, item);
                    SelectedWiFiNetwork = WiFiNetworks[0];
                    OnSelect?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    WiFiNetworks.Add(item);
                }
            }
        }

        public async Task ConnectAsync()
        {
            if (SelectedWiFiNetwork == null)
            {
                OnError?.Invoke(this, new ArgumentException("Network not selected"));
                return;
            }
            WiFiReconnectionKind reconnectionKind = WiFiReconnectionKind.Manual;
            if (SelectedWiFiNetwork.ConnectAutomatically)
            {
                reconnectionKind = WiFiReconnectionKind.Automatic;
            }
            Task<WiFiConnectionResult> didConnect = null;
            WiFiConnectionResult result = null;
            if (SelectedWiFiNetwork.IsEapAvailable)
            {
                if (SelectedWiFiNetwork.UsePassword)
                {
                    var credential = new PasswordCredential();
                    if (!String.IsNullOrEmpty(SelectedWiFiNetwork.Domain))
                    {
                        credential.Resource = SelectedWiFiNetwork.Domain;
                    }
                    credential.UserName = SelectedWiFiNetwork.UserName ?? "";
                    credential.Password = SelectedWiFiNetwork.Password ?? "";

                    didConnect = _wifiAdapter.ConnectAsync(SelectedWiFiNetwork.AvailableNetwork, reconnectionKind, credential).AsTask();
                }
                else
                {
                    didConnect = _wifiAdapter.ConnectAsync(SelectedWiFiNetwork.AvailableNetwork, reconnectionKind).AsTask();
                }
            }
            else if (SelectedWiFiNetwork.AvailableNetwork.SecuritySettings.NetworkAuthenticationType == NetworkAuthenticationType.Open80211 &&
                    SelectedWiFiNetwork.AvailableNetwork.SecuritySettings.NetworkEncryptionType == NetworkEncryptionType.None)
            {
                didConnect = _wifiAdapter.ConnectAsync(SelectedWiFiNetwork.AvailableNetwork, reconnectionKind).AsTask();
            }
            else
            {
                // Only the password potion of the credential need to be supplied
                if (String.IsNullOrEmpty(SelectedWiFiNetwork.Password))
                {
                    didConnect = _wifiAdapter.ConnectAsync(SelectedWiFiNetwork.AvailableNetwork, reconnectionKind).AsTask();
                }
                else
                {
                    var credential = new PasswordCredential();
                    credential.Password = SelectedWiFiNetwork.Password ?? "";

                    didConnect = _wifiAdapter.ConnectAsync(SelectedWiFiNetwork.AvailableNetwork, reconnectionKind, credential).AsTask();
                }
            }

            OnConnecting?.Invoke(this, EventArgs.Empty);

            if (didConnect != null)
            {
                result = await didConnect;
            }

            if (result != null && result.ConnectionStatus == WiFiConnectionStatus.Success)
            {
                WiFiNetworks.Remove(SelectedWiFiNetwork);
                WiFiNetworks.Insert(0, SelectedWiFiNetwork);
                OnSelect?.Invoke(this, EventArgs.Empty);
                OnConnected?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                OnError?.Invoke(this, new Exception("Could not connect to network"));
                OnDisconnected?.Invoke(this, EventArgs.Empty);
            }

            // Since a connection attempt was made, update the connectivity level displayed for each
            foreach (var network in WiFiNetworks)
            {
                await network.UpdateConnectivityLevel();
            }
        }

        public void Disconnect()
        {
            if (SelectedWiFiNetwork == null)
            {
                OnError?.Invoke(this, new ArgumentException("Network not selected"));
                return;
            }
            SelectedWiFiNetwork.Disconnect();
        }

        public bool IsConnected(WiFiAvailableNetwork network)
        {
            if (network == null)
            {
                return false;
            }
            string profileName = GetCurrentWiFiNetwork();
            if (!String.IsNullOrEmpty(network.Ssid) &&
                !String.IsNullOrEmpty(profileName) &&
                (network.Ssid == profileName))
            {
                return true;
            }

            return false;
        }

        private string GetCurrentWiFiNetwork()
        {
            var connectionProfiles = NetworkInformation.GetConnectionProfiles();
            if (connectionProfiles.Count < 1)
            {
                return null;
            }
            var validProfiles = connectionProfiles.Where(profile =>
            {
                return (profile.IsWlanConnectionProfile && profile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.None);
            });
            if (validProfiles.Count() < 1)
            {
                return null;
            }
            ConnectionProfile firstProfile = validProfiles.First();
            return firstProfile.ProfileName;
        }
    }

    public class WiFiNetworkViewModel : ViewModelBase
    {
        private WiFiAdapter _wifiAdapter;

        public WiFiNetworkViewModel(WiFiAvailableNetwork availableNetwork, WiFiAdapter wifiAdapter)
        {
            _wifiAdapter = wifiAdapter;
            AvailableNetwork = availableNetwork;
        }

        public async Task UpdateAsync()
        {
            UpdateWiFiImage();
            UpdateNetworkKeyVisibility();
            await UpdateConnectivityLevel();
        }

        private void UpdateNetworkKeyVisibility()
        {
            // Only show the password box if needed
            if ((AvailableNetwork.SecuritySettings.NetworkAuthenticationType == NetworkAuthenticationType.Open80211 &&
                 AvailableNetwork.SecuritySettings.NetworkEncryptionType == NetworkEncryptionType.None) ||
                 IsEapAvailable)
            {
                NetworkKeyInfoVisibility = false;
            }
            else
            {
                NetworkKeyInfoVisibility = true;
            }
        }

        private void UpdateWiFiImage()
        {
            string imageFileNamePrefix = "secure";
            if (AvailableNetwork.SecuritySettings.NetworkAuthenticationType == NetworkAuthenticationType.Open80211)
            {
                imageFileNamePrefix = "open";
            }

            string imageFileName = string.Format("ms-appx:/Assets/{0}_{1}bar.png", imageFileNamePrefix, AvailableNetwork.SignalBars);

            WiFiImage = new BitmapImage(new Uri(imageFileName));

            NotifyPropertyChanged("WiFiImage");
        }

        public async Task UpdateConnectivityLevel()
        {
            string connectivityLevel = "Not Connected";
            string connectedSsid = null;

            var connectedProfile = await _wifiAdapter.NetworkAdapter.GetConnectedProfileAsync();
            if (connectedProfile != null &&
                connectedProfile.IsWlanConnectionProfile &&
                connectedProfile.WlanConnectionProfileDetails != null)
            {
                connectedSsid = connectedProfile.WlanConnectionProfileDetails.GetConnectedSsid();
            }

            if (!string.IsNullOrEmpty(connectedSsid))
            {
                if (connectedSsid.Equals(AvailableNetwork.Ssid))
                {
                    connectivityLevel = connectedProfile.GetNetworkConnectivityLevel().ToString();
                }
            }

            ConnectivityLevel = connectivityLevel;
            NotifyPropertyChanged("ConnectivityLevel");
        }

        public void Disconnect()
        {
            _wifiAdapter.Disconnect();
        }

        public bool NetworkKeyInfoVisibility { get; set; }

        private bool usePassword = false;

        public bool UsePassword
        {
            get
            {
                return usePassword;
            }
            set
            {
                usePassword = value;
                NotifyPropertyChanged("UsePassword");
            }
        }

        private bool connectAutomatically = false;

        public bool ConnectAutomatically
        {
            get
            {
                return connectAutomatically;
            }
            set
            {
                connectAutomatically = value;
                NotifyPropertyChanged("ConnectAutomatically");
            }
        }

        public String Ssid
        {
            get
            {
                return availableNetwork.Ssid;
            }
        }

        public String Bssid
        {
            get
            {
                return availableNetwork.Bssid;
            }
        }

        public String ChannelCenterFrequency
        {
            get
            {
                return string.Format("{0}kHz", availableNetwork.ChannelCenterFrequencyInKilohertz);
            }
        }

        public String Rssi
        {
            get
            {
                return string.Format("{0}dBm", availableNetwork.NetworkRssiInDecibelMilliwatts);
            }
        }

        public String SecuritySettings
        {
            get
            {
                return string.Format("Authentication: {0}; Encryption: {1}", availableNetwork.SecuritySettings.NetworkAuthenticationType, availableNetwork.SecuritySettings.NetworkEncryptionType);
            }
        }

        public String ConnectivityLevel
        {
            get;
            private set;
        }

        public BitmapImage WiFiImage
        {
            get;
            private set;
        }

        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; NotifyPropertyChanged("UserName"); }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; NotifyPropertyChanged("Password"); }
        }

        private string domain;

        public string Domain
        {
            get { return domain; }
            set { domain = value; NotifyPropertyChanged("Domain"); }
        }

        public bool IsEapAvailable
        {
            get
            {
                return ((availableNetwork.SecuritySettings.NetworkAuthenticationType == NetworkAuthenticationType.Rsna) ||
                    (availableNetwork.SecuritySettings.NetworkAuthenticationType == NetworkAuthenticationType.Wpa));
            }
        }

        private WiFiAvailableNetwork availableNetwork;

        public WiFiAvailableNetwork AvailableNetwork
        {
            get
            {
                return availableNetwork;
            }

            private set
            {
                availableNetwork = value;
            }
        }
    }
}