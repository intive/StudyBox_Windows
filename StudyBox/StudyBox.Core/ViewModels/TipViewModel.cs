﻿using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBox.Core.ViewModels
{
    public class TipViewModel : ViewModelBase
    {
        private string _id;
        private string _prompt;
        private readonly int _maxPromptCharacters = 500;

        public string ID
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    RaisePropertyChanged("ID");
                }
            }
        }

        public string Prompt
        {
            get
            {
                return _prompt;
            }
            set
            {
                if (_prompt != value)
                {
                    _prompt = value;
                    RaisePropertyChanged("Prompt");
                    RaisePropertyChanged("CurrentPromptCharactersNumber");
                }
            }
        }

        public int CurrentPromptCharactersNumber
        {
            get
            {
                return Prompt.Length > _maxPromptCharacters ? _maxPromptCharacters : Prompt.Length;
            }
        }

        public int MaxPromptCharacters
        {
            get
            {
                return _maxPromptCharacters;
            }
        }

        public TipViewModel() { }

        public TipViewModel(string id, string prompt)
        {
            this.ID = id;
            this.Prompt = prompt;
        }
    }
}
