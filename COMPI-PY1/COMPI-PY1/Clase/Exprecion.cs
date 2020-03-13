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
        public List<Transicion> trans { get; set; }
        int estado = 0;

        public Exprecion()
        {
            this.tokens = new List<Token>();
            this.grafo = new Grafo();
        }

        public Transicion crearE()
        {
            Transicion temp = new Transicion("S" + estado);
            rellenar(temp);
            trans.Add(temp);
            estado++;
            return temp;
        }
        //nuevo.lista.Find(x => x == i); nuevo.Sort()
        public void rellenar(Transicion temp)
        {
            for (int i = 0; i < grafo.transiciones.Count; i++)
            {
                Transicion nuevo = new Transicion(grafo.transiciones[i].lexema);
                nuevo.tipo = grafo.transiciones[i].idToken;
                temp.siguiente = nuevo;
                temp = nuevo;
            }
        }
    }
}
