using COMPI_PY1.Clase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPI_PY1.Analizador
{
    class Lexico
    {
        public List<Token> listaT { get; set; }
        public List<TokenError> listaE { get; set; }
        
        public Lexico(string texto)
        {
            Analizar(texto);
        }

        public void Analizar(string texto) {
            int token = 0;
            int error = 0;
            int fila = 1;
            int columna = 1;
            int puntero = 0;
            int estado = 0;
            char caracter = ' ';
            char[] linea = texto.ToCharArray(0, texto.Length);
            
            while (puntero < linea.Length) {
                caracter = linea[puntero];
                switch (estado)
                {
                    case 0:
                        if (caracter.Equals('\t')) {
                            puntero++;
                        }
                        else if (caracter.Equals('\n'))
                        {

                        }
                        break;


                    default:
                        break;
                }

            }
            columna = 1;
            fila++;

            }

        }

    }
}
