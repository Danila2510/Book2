using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.IO;

namespace Book
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<ViewModel> contacts = new ObservableCollection<ViewModel>();
        string Path = "Contact.txt";
        private string fullName;
        private string address;
        private string phoneNumber;
        public string FullName
        {
            get { return fullName; }
            set
            {
                fullName = value;
                OnPropertyChanged(nameof(FullName));
            }
        }
        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                OnPropertyChanged(nameof(Address));
            }
        }
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }
        public ObservableCollection<ViewModel> Contacts
        {
            get { return contacts; }
            set 
            {
                contacts = value;
                OnPropertyChanged(nameof(Contacts));
            }
        }
        private Command addPersone;
        public ICommand ADDPersone
        {
            get
            {
                if (addPersone==null)
                    addPersone = new Command(Add,CanAdd);
                return addPersone;
            }
        }
        private Command loadContacts;
        public ICommand LoadContactsCommand
        {
            get
            {
                if (loadContacts == null)
                    loadContacts = new Command(LoadContacts,CanAdd);
                return loadContacts;
            }
        }
        private Command removeCommand;
        public ICommand RemoveCommand
        {
            get
            {
                if (removeCommand == null)
                    removeCommand = new Command(Remove, CanRemove);
                return removeCommand;
            }
        }
        private Command updateCommand;
        public ICommand UpdateCommand
        {
            get
            {
                if (updateCommand == null)
                    updateCommand = new Command(Update_Сontact, CanUpdate);
                return updateCommand;
            }
        }

        private void LoadContacts(object parameter)
        {
            LoadContactsFromFile(Path);
        }

        private void Add(object parametr)
        {
            Contacts.Add(new ViewModel() { FullName = FullName, Address = Address, PhoneNumber = PhoneNumber });
            SaveContactsToFile(Path);
            FullName = string.Empty;
            Address = string.Empty;
            PhoneNumber = string.Empty;
        }
        private bool CanAdd(object paramater)
        {
            if (FullName==""||Address==""||PhoneNumber=="")
                return false;
            return true;
        }
        private bool CanRemove(object parameter)
        {
            return parameter is ViewModel;
        }
        private bool CanUpdate(object parameter)
        {
            return parameter is ViewModel existingContact &&
                   !string.IsNullOrEmpty(existingContact.FullName) &&
                   !string.IsNullOrEmpty(existingContact.Address) &&
                   !string.IsNullOrEmpty(existingContact.PhoneNumber);
        }

        public void SaveContactsToFile(string filePath)
        {
            try
            {
                List<string> lines = new List<string>();
                foreach (var contact in Contacts)
                {
                    string line = $"{contact.FullName} {contact.Address} {contact.PhoneNumber}";
                    lines.Add(line);
                }
                File.WriteAllLines(filePath, lines);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении данных в файл: " + ex.Message);
            }
        }
        public void LoadContactsFromFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    List<string> lines = File.ReadAllLines(filePath).ToList();
                    Contacts.Clear();

                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(' ');
                        if (parts.Length == 3)
                        {
                            Contacts.Add(new ViewModel
                            {
                                FullName = parts[0],
                                Address = parts[1],
                                PhoneNumber = parts[2]
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных из файла: " + ex.Message);
            }
        }
        private void Remove(object parameter)
        {
            if (parameter is ViewModel contactToRemove)
                Contacts.Remove(contactToRemove);
        }
        private void Update_Сontact(object parametr)
        {
            if (parametr is ViewModel existingContact)
            {
                ViewModel newContact = new ViewModel
                {
                    FullName = fullName,
                    Address = address,
                    PhoneNumber = phoneNumber 
                };
                Update(existingContact, newContact);
                FullName = string.Empty;
                Address = string.Empty;
                PhoneNumber = string.Empty;
            }
        }
        private void Update(ViewModel existingContact, ViewModel newContact)
        {
            existingContact.FullName = newContact.FullName;
            existingContact.Address = newContact.Address;
            existingContact.PhoneNumber = newContact.PhoneNumber;
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
