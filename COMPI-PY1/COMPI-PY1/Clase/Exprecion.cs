using System;
using System.Collections.Generic;
using System.IO;
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
        public List<Transicion> transiciones { get; set; }
        int estado = 0;

        public Exprecion()
        {
            this.tokens = new List<Token>();
            this.transiciones = new List<Transicion>();
            this.grafo = new Grafo();
        }

        public void crearPrimera()
        {
            Transicion temp = new Transicion("S" + estado);
            rellenarCabecera(temp);
            transiciones.Add(temp);
            estado++;
            buscarPrimera(temp, grafo.primero);
            temp.lista = temp.lista.Distinct().ToList();
            temp.lista.Add(0);
            temp.lista.Sort();
            graficar();
        }
        //nuevo.lista.Find(x => x == i); nuevo.Sort()
        public void rellenarCabecera(Transicion temp)
        {
            for (int i = 0; i < grafo.transiciones.Count; i++)
            {
                Transicion nuevo = new Transicion(grafo.transiciones[i].lexema);
                nuevo.tipo = grafo.transiciones[i].idToken;
                temp.siguiente = nuevo;
                temp = nuevo;
            }
        }
        
        public void buscarPrimera(Transicion temp, NodoG raiz) {
            while (raiz != null)
            {
                if (raiz.p.Equals("ε"))
                {
                    temp.lista.Add(raiz.primero.id);
                    buscarPrimera(temp, raiz.primero);
                }

                if (raiz.s.Equals("ε"))
                {
                    temp.lista.Add(raiz.segundo.id);
                    buscarPrimera(temp, raiz.segundo);
                }
                raiz = null;
            }
        }

        public void graficar()
        {
            StreamWriter escribir = new StreamWriter("Reportes\\" + nombre.lexema + "Tabla.txt");
            //escribir.WriteLine("digraph D{\nrankdir=LR;");

            escribir.WriteLine("digraph grafico{\ngraph [pad=\"0.5\", nodesep=\"0.5\", ranksep=\"2\"];\nnode [shape=plain]\nrankdir=LR;\n");
            escribir.Write("Foo [label=<\n<table border=\"0\" cellborder=\"1\" cellspacing=\"0\">\n<tr><td><i><b>Estado</b></i></td>");

            for (int i = 0; i < grafo.transiciones.Count; i++)
            {
                Token nodot = grafo.transiciones[i];
                escribir.Write("<td><i><b>"+ nodot.lexema +"</b></i></td>");
            }

            escribir.Write("</tr>\n");
            
            for (int i = 0; i < transiciones.Count; i++)
            {
                Transicion tempcol = transiciones[i];

                if (tempcol.lista.Count == 0)
                {
                    escribir.Write("<tr><td><b>" + tempcol.nombre + "[]</b></td>");
                }
                else
                {
                    //string l = "[";

                    //for (int j = 0; j < tempcol.lista.Count; j++)
                    //{
                    //    int num = tempcol.lista[j];
                    //    if (j == 0 && tempcol.lista.Count == 1)
                    //    {
                    //        l += num + "]";
                    //    }
                    //    else
                    //    {
                    //        if (j == 0)
                    //        {
                    //            l += num;
                    //        }
                    //        else if (j + 1 == tempcol.lista.Count)
                    //        {
                    //            l += "," + num + "]";
                    //        }
                    //        else
                    //        {
                    //            l += "," + num;
                    //        }
                    //    }
                    //}
                    
                    escribir.Write("<tr><td><b>" + tempcol.nombre + "[" + String.Join(", ", tempcol.lista) + "]</b></td>");
                }
            
                Transicion tempfila = tempcol.siguiente;
                while (tempfila != null)
                {
                    if (tempfila.lista.Count == 0)
                    {
                        escribir.Write("<td>[]</td>");
                    }
                    else
                    {
                        //string l = "[";

                        //for (int j = 0; j < tempcol.lista.Count; j++)
                        //{
                        //    int num = tempcol.lista[j];
                        //    if (j == 0 && tempcol.lista.Count == 1)
                        //    {
                        //        l += num + "]";
                        //    }
                        //    else
                        //    {
                        //        if (j == 0)
                        //        {
                        //            l += num;
                        //        }
                        //        else if (j + 1 == tempcol.lista.Count)
                        //        {
                        //            l += "," + num + "]";
                        //        }
                        //        else
                        //        {
                        //            l += "," + num;
                        //        }
                        //    }
                        //}

                        escribir.Write("<td>" + "[" + String.Join(", ", tempcol.lista) + "]</td>");
                    }
                    tempfila = tempfila.siguiente;
                }

                escribir.Write("</tr>\n");
            }

            escribir.WriteLine("</table>>];\n}");
            
            escribir.Close();

            string texto = "/K dot -Tpng Reportes\\" + nombre.lexema + "Tabla.txt -o Reportes\\" + nombre.lexema + "Tabla.jpg";

            //System.Diagnostics.Process.Start("CMD.exe", "/K dot -Tpng Ella.txt -o UML.png");

            System.Diagnostics.Process.Start("CMD.exe", texto).Close();
            //System.Diagnostics.Process.Start("Reportes\\"+nombre.lexema+"Grafo.jpg");
        }

        
    }
}
