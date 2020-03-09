using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPI_PY1.Clase
{
    class Exprecion
    {
        public Token nombre { get; set; }
        public Grafo grafo { get; set;}
        public List<Token> tokens { get; set; }

        public Exprecion()
        {
            this.tokens = new List<Token>();
            this.grafo = new Grafo();
        }
        
    }
}
