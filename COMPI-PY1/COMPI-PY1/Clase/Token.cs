using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPI_PY1.Clase
{
    class Token
    {
        public int noToken { get; set; }
        public int idToken { get; set; }
        public string lexema { get; set; }
        public int fila { get; set; }
        public int columna { get; set; }
        public string tipo { get; set; }

        public Token(int noToken, int idToken, string tipo, string lexema, int fila, int columna)
        {
            this.noToken = noToken;
            this.idToken = idToken;
            this.tipo = tipo;
            this.lexema = lexema;
            this.fila = fila;
            this.columna = columna;
        }
        
    }
}
