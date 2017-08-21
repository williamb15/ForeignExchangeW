

namespace ForeignExchangeW.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Models;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using System;


    public class MainViewModel
    {

    #region Properties

    public string Amount { get; set; }

    public ObservableCollection<Rates> Rates { get; set; }

    public Rates SourceRate { get; set; }

    public Rates TargetRate { get; set; }

    public bool IsRunning { get; set; }

    public bool IsEnabled { get; set; }

    public string Result { get; set; }

    #endregion

   

    #region Commands
    public ICommand ConvertCommand
    {
        get
        {
            return new RelayCommand(Convert);
        }
        
    }

    void Convert()
    {
        throw new NotImplementedException();
    }

        #endregion

    }

}
