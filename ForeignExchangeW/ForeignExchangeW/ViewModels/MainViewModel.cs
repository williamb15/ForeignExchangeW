

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

        #endregion

        #region Properties

        public string Amount { get; set; }

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
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter a value in amount",
                    "Accept");
                return;
            }
            decimal amount = 0;
            if(!decimal.TryParse (Amount, out amount))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter a numeric value in amount",
                    "Accept");
                return;
            }
            if (SourceRate == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must select a source rate",
                    "Accept");
                return;
            }
            if (TargetRate == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must select a target rate",
                    "Accept");
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
            LoadRates();
           }

         #endregion

        #region Methods

        async void LoadRates ()
        {
            IsRunning = true;
            Result = "Loading Rates..";

            try
            {
                var client = new HttpClient();
                client.BaseAddress = new 
                    Uri("http://apiexchangerates.azurewebsites.net");
                var controller = "/api/Rates";
                var response = await client.GetAsync(controller);
                var result = await response.Content.ReadAsStringAsync();
                if(!response.IsSuccessStatusCode)
                {
                    IsRunning = false;
                    Result = result;
                }
                var rates = JsonConvert.DeserializeObject<List<Rates>>(result);
                Rates = new ObservableCollection<Models.Rates>(rates);
                IsRunning = false;
                IsEnabled = true;
                Result = "Ready to convet!";
            }
            catch (Exception ex)
            {
                IsRunning = false;
                Result = ex.Message; 
                throw;
            }

        }

     #endregion
    }

}
