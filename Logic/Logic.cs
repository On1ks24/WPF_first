using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Windows.Input;
using Model;
using GalaSoft.MvvmLight.Command;
using System.IO;
using System.Xml.Linq;

namespace Logic
{
    public class Logic: INotifyPropertyChanged
    {
        public Logic(RadioButton[] r)
        {

            Disciplines = new ObservableCollection<Discipline>();
            ViewDisciplines = new ObservableCollection<Discipline>();
            Load();
            AddCommandAdd = new RelayCommand(Add);
            AddCommandFilter = new RelayCommand(Filter);
            AddCommandSave = new RelayCommand(Sawing);
            AddCommandChange = new RelayCommand(ChangeStatus);
            AddCommandDelete = new RelayCommand(Delete);

            radioButton = r;

            NewDiscipline = new Discipline();
        }
        public RelayCommand AddCommandAdd { get; set; }
        public RelayCommand AddCommandFilter { get; set; }
        public RelayCommand AddCommandSave { get; set; }
        public RelayCommand AddCommandChange { get; set; }
        public RelayCommand AddCommandDelete { get; set; }
        //ПЕРЕМЕННЫЕ/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        RadioButton[] radioButton;
        private Discipline newdiscipline { get; set; }
        public Discipline NewDiscipline
        {
            get
            {
                return newdiscipline;
            }
            set
            {
                newdiscipline = value;
                OnPropertyChanged("Newdiscipline");
            }
        }

        private Discipline selecteddiscipline { get; set; }
        public Discipline Selecteddiscipline
        {
            get
            {
                return selecteddiscipline;
            }
            set
            {
                selecteddiscipline = value;
                OnPropertyChanged("Selecteddiscipline");
            }
        }
        private ObservableCollection<Discipline> disciplines { set; get; }
        public ObservableCollection<Discipline> Disciplines
        {
            get
            {
                return disciplines;
            }
            set
            {
                disciplines = value;
                OnPropertyChanged("Disciplines");
            }
        }
        private ObservableCollection<Discipline> viewDisciplines { set; get; }
        public ObservableCollection<Discipline> ViewDisciplines
        {
            get
            {
                return viewDisciplines;
            }
            set
            {
                viewDisciplines = value;
                OnPropertyChanged("ViewDisciplines");
            }
        }
        private List<bool> statuses = new List<bool>
        {
            true,
            false
        };

        public List<bool> Statuses
        {
            get { return statuses; }
            set
            {
                OnPropertyChanged("Statuses");
            }
        }

        //МЕТОДЫ//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void Delete()
        {
            int i=-1;
            foreach(Discipline d in Disciplines)
            {
                i++;
                if(d.Name==Selecteddiscipline.Name)
                {
                    goto link1;
                }
            }
            link1:
                Disciplines.Remove(Disciplines[i]);
                ViewDisciplines.Remove(Selecteddiscipline);
        }
        private void Add()
        {
            ViewDisciplines.Add(new Discipline()
            {
                Name = NewDiscipline.Name,
                Id = NewDiscipline.Id
            });
            Disciplines.Add(new Discipline()
            {
                Name = NewDiscipline.Name,
                Id = NewDiscipline.Id
            });
        }
        private async void Load()
        {
            using (StreamReader reader = new StreamReader("C:/Users/sarae/source/repos/nado.txt"))
            {
                string text = await reader.ReadToEndAsync();
                text = text.Replace("\n", "p");
                string[] words = text.Split(new char[] { 'p' });
                foreach (string w in words)
                {
                    string[] wordss = w.Split(new char[] { ',' });
                    if (wordss.Length>1)
                    {
                        Disciplines.Add(new Discipline() { Name = wordss[0], Id = Convert.ToBoolean(wordss[1]) });
                    }
                }
            }
        }
        private async Task SaveToFile()
        {
            using (StreamWriter writerrr = new StreamWriter("C:/Users/sarae/source/repos/nado.txt", false))
            {
                string text2 = string.Empty;
                foreach (var d in Disciplines)
                {
                    text2 += d.Name + "," + d.Id + "\n";
                }
                writerrr.WriteLine(text2);
            }
        }
        private void ChangeStatus()
        {
            foreach (var k in ViewDisciplines)
            {
                if(Selecteddiscipline.Name==k.Name)
                {
                    if(k.Id==true)
                    {
                        k.Id = false;
                        foreach(Discipline w in Disciplines)
                        {
                            if(w.Name==k.Name)
                            {
                                w.Id = false;
                            }
                        }
                    }
                    else
                    {
                        k.Id=true;
                        foreach (Discipline w in Disciplines)
                        {
                            if (w.Name == k.Name)
                            {
                                w.Id = true;
                            }
                        }
                    }
                }
            }
        }
        private void Filter()
        {
            ViewDisciplines.Clear();
            foreach (var discipline in Disciplines)
            {
                foreach (var choice in radioButton)
                {
                    if ((choice.IsChecked == true) && (choice.Name == "All"))
                    {
                        ViewDisciplines.Add(new Discipline() 
                        {
                            Name = discipline.Name, Id = discipline.Id 
                        });
                    }
                    if ((choice.IsChecked == true) && (choice.Name == "Completed"))
                    {
                        if (discipline.Id)
                        {
                            ViewDisciplines.Add(new Discipline() 
                            { 
                                Name = discipline.Name, Id = discipline.Id 
                            });
                        }
                    }
                    if ((choice.IsChecked == true) && (choice.Name == "Incompleted"))
                    {
                        if (!discipline.Id)
                        {
                            ViewDisciplines.Add(new Discipline() 
                            { 
                                Name = discipline.Name, Id = discipline.Id 
                            });
                        }
                    }
                }
            }
        }
        private async void Sawing()
        {
            await SaveToFile();
        }











   



























        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
