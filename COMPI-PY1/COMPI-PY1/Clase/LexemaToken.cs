using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPI_PY1.Clase
{
    class LexemaToken
    {

        public int columna { get; set; }
        public string lexema { get; set; }
        public string nombretoken { get; set; }
        public string tipo { get; set; }

        public LexemaToken(int columna, string lexema, string nombretoken, string tipo)
        {
            this.nombretoken = nombretoken;
            this.tipo = tipo;
            this.lexema = lexema;
            this.columna = columna;
        }

    }
}
