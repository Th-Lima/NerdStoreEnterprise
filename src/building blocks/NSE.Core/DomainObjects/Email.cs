using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace NSE.Core.DomainObjects
{
    public class Email
    {
        public const int EmailAddressMaxLength = 254;
        public const int EmailAddressMinLength = 5;

        public string AddressEmail { get; set; }

        //CONSTRUCTOR EF 
        public Email() { }

        public Email(string email)
        {
            if(!Validate(email)) 
                throw new DomainException("E-mail inválido");

            AddressEmail = email;
        }

        private static bool Validate(string email)
        {
            var regexEmail = new Regex(@"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");
            return regexEmail.IsMatch(email);
        }
    }
}
