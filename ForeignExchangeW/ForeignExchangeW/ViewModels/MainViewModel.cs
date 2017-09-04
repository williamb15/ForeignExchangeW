

namespace ForeignExchangeW.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Models;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using System;
    using System.ComponentModel;
    using System.Net.Http;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using Xamarin.Forms;
    using Helpers;
    using Services;
    using System.Threading.Tasks;

    public class MainViewModel : INotifyPropertyChanged
    {

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Attributes

            bool _IsRunning;
            bool _IsEnabled;
            string _Results;
            ObservableCollection<Rates> _rates;
            Rates _sourRate;
            Rates _targetRates;
            string _status;
            List<Rates> rates;
            

            #endregion

        #region Properties

        public string Amount { get; set; }


        public String Status

        {
            get
            {
                return _status;

            }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Status)));
                }
            }
        }

        public ObservableCollection<Rates> Rates
        {
            get
            {
                return _rates;

            }
            set
            {
                if (_rates != value)
                {
                    _rates = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Rates)));
                }
            }
        }

        public Rates SourceRate
        {
            get
            {
                return _sourRate;

            }
            set
            {
                if (_sourRate != value)
                {
                    _sourRate = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(SourceRate)));
                }
            }
        }

        public Rates TargetRate
        { 
            get
            {
                return _targetRates;

            }
            set
            {
                if (_targetRates != value)
                {
                    _targetRates = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(TargetRate)));
                }
            }
        }
        public bool IsRunning
        {
            get
            {
                return _IsRunning;
              
            }
            set
            {
                if(_IsRunning != value)
                {
                    _IsRunning = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsRunning)));
                }
            }
        }

        public bool IsEnabled
        {
            get
            {
                return _IsEnabled;

            }
            set
            {
                if (_IsEnabled != value)
                {
                    _IsEnabled = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsEnabled)));
                }
            }
        }

        public string Result
        {
            get
            {
                return _Results;

            }
            set
            {
                if (_Results != value)
                {
                    _Results = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Result)));
                }
            }
        }

         #endregion

        #region Commands
        public ICommand ConvertCommand
        {
        get
        {
            return new RelayCommand(Convert);
        }
        
        }

         async void Convert()
         {
            if(string.IsNullOrEmpty(Amount))
            {
                await dialogService.ShowMessage(
                    Lenguages.Error,
                    Lenguages.AmountValidation);
                return;
            }
            decimal amount = 0;
            if(!decimal.TryParse (Amount, out amount))
            {
                await dialogService.ShowMessage(
                    Lenguages.Error,
                    Lenguages.AmountNumericValidation );
                return;
            }
            if (SourceRate == null)
            {
                await dialogService.ShowMessage(
                    Lenguages.Error,
                    Lenguages.SourceRateValidation);
                return;
            }
            if (TargetRate == null)
            {
                await dialogService.ShowMessage(
                    Lenguages.Error,
                    Lenguages.TargetRateValidation);
                return;
            }

            var amountConverted = amount / 
                                  (decimal)SourceRate.TaxRate * 
                                  (decimal)TargetRate.TaxRate;

            Result = string.Format("{0}{1:C2} = {2}{3:C2}",
                SourceRate.Code, 
                amount, 
                TargetRate.Code, 
                amountConverted);
        }

        public  ICommand SwitchCommand
        {
            get
            {
                return new RelayCommand(Switch);
            }
        }

        void Switch ()
        {
            var AUX = SourceRate;
            SourceRate = TargetRate;
            TargetRate = AUX;
            Convert();
        }

        #endregion

        #region Constructors

        public MainViewModel()
          {
            apiService = new ApiService();
            dataService = new DataService();
            dialogService = new DialogService();
            LoadRates();
           }

         #endregion

        #region Methods

        async void LoadRates ()
        {
            IsRunning = true;
            Result = Lenguages.Loading;

            var connection = await apiService.CheckConnection();

            if(!connection.IsSuccess)
            {
                LoadLocalData();
                Status = "Rates Loades from local data.";
            }
            else
            {
                await LoadDataFromAPI();
            }

            if(rates.Count == 0)
            {
                IsRunning = false;
                IsEnabled = false;
                Result = "There are not internet connection and  not " +
                    "load previously rates. Please try again with  " +
                    "internet connection.";
                Status = "No rates loaded";
                return;
            }

            Rates = new ObservableCollection<Rates>(rates);

            IsRunning = false;
            IsEnabled = true;
            Result = Lenguages.Ready;
            Status = "Rates Loades from Internet.";
        }

        private void LoadLocalData()
        {
            rates = dataService.Get<Rates>(false);
        }

        async Task LoadDataFromAPI()
        {
            var url = "http://apiexchangerates.azurewebsites.net"; // Application.Current.Resources["URLAPI"].ToString();

            var response = await apiService.GetList<Rates>(
                url,
                "api/Rates");

            if (!response.IsSuccess)
            {
                LoadLocalData();
                return;
            }

            //storage data local
            rates = (List<Rates>)response.Result;
            dataService.DeleteAll<Rates>();
            dataService.Save(rates);
        }

        #endregion

        #region Services

        ApiService apiService;
        DialogService dialogService;
        DataService dataService;


        #endregion
    }

}
