using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2MP {
    public class ChosenSettings {
        public string ip;
        public Role role;
    }

    public enum Role {
        HOST,
        JOIN
    }
}
