using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book
{
    public class Contact
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Number { get; set; }

        public Contact() { }
        public Contact(string fName, string address, string pNumber)
        {
            Name = fName;
            Address = address;
            Number = pNumber;
        }

        public override string ToString()
        {
            return $"Name:{Name}\nAddress:{Address}\nPhoneNumber:{Number}\n";
        }
    }
}
