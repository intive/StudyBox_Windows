using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace StudyBox.Core.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private INavigationService _navigationService;
        private ResourceLoader _stringResources;
        private RelayCommand _registerAction;
        private RelayCommand _cancelAction;
        private string _generalErrorMessage;
        private bool _isEmailNotValid;
        private bool _isPasswordNotValid;
        private bool _isRepeatPasswordNotValid;
        private bool _isGeneralError;
        private string _email;
        private string _password;
        private string _repeatPassword;
        

  
        public RegisterViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            _stringResources = new ResourceLoader();          
        }

        public RelayCommand RegisterAction
        {
            get
            {
                return _registerAction ?? (_registerAction = new RelayCommand(Register));
            }
        }

        public RelayCommand CancelAction
        {
            get
            {
                return _cancelAction ?? (_cancelAction = new RelayCommand(Cancel));
            }
        }

        public string GeneralErrorMessage
        {
            get { return _generalErrorMessage; }
            set
            {
                if (_generalErrorMessage != value)
                {
                    _generalErrorMessage = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string RepeatPassword
        {
            get { return _repeatPassword; }
            set
            {
                if (_repeatPassword != value)
                {
                    _repeatPassword = value;
                    RaisePropertyChanged();
                }
            }
        }



        public bool IsEmailNotValid
        {
            get { return _isEmailNotValid; }
            set
            {
                if (_isEmailNotValid != value)
                {
                    _isEmailNotValid = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsPasswordNotValid
        {
            get { return _isPasswordNotValid; }
            set
            {
                if (_isPasswordNotValid != value)
                {
                    _isPasswordNotValid = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsRepeatPasswordNotValid
        {
            get { return _isRepeatPasswordNotValid; }
            set
            {
                if (_isRepeatPasswordNotValid != value)
                {
                    _isRepeatPasswordNotValid = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsGeneralError
        {
            get { return _isGeneralError; }
            set
            {
                if (_isGeneralError != value)
                {
                    _isGeneralError = value;
                    RaisePropertyChanged();
                }
            }
        }

        public void Register()
        {
            IsEmailNotValid = string.IsNullOrEmpty(Email);
            IsPasswordNotValid = string.IsNullOrEmpty(Password);
            IsRepeatPasswordNotValid = string.IsNullOrEmpty(RepeatPassword);
            GeneralErrorMessage = _stringResources.GetString("NoInternetConnection");
            IsGeneralError = true;
        }

        public void Cancel()
        {
            _navigationService.NavigateTo("DecksListView");
        }
    }
}
