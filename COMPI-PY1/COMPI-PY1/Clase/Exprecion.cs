using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace COMPI_PY1.Clase
{
    class Exprecion
    {
        public Token nombre { get; set; }
        public Grafo grafo { get; set;}
        public List<Token> tokens { get; set; }
        public List<Transicion> transiciones { get; set; }
        public List<Conjunto> conjuntos { get; set; }
        public List<LexemaError> listaE { get; set; }
        public List<LexemaToken> listaT { get; set; }
        int estado = 0;
        public Exprecion(Token nombre)
        {
            this.nombre = nombre;
            this.tokens = new List<Token>();
            this.transiciones = new List<Transicion>();
            this.grafo = new Grafo();
            this.listaE = new List<LexemaError>();
            this.listaT = new List<LexemaToken>();
        }

        public void comienzo()
        {
            Transicion temp = new Transicion("S" + estado);
            temp.anterior.Add(0);
            temp.lista.Add(0);
            transiciones.Add(temp);
            //llena la cabecera con los siguientes de las posibles transiciones
            siguientesCabecera(temp);
            //llena la cabecera con los numeros que tienen salto ε
            llenarCabecera(temp);

            temp.lista = temp.lista.Distinct().ToList();
            temp.lista.Sort();
            estado++;
            
            for (int i = 0; i < transiciones.Count; i++)
            {
                Transicion nodocol = transiciones[i];

                Transicion nodofila = nodocol.siguiente;
                while (nodofila != null)
                {
                    llenarNodos(nodocol.lista, grafo.nodos, nodofila);
                    nodofila.lista = nodofila.lista.Distinct().ToList();
                    nodofila.lista.Sort();

                    if (nodofila.lista.Count != 0)
                    {
                        nodofila.estado = crearCabecera(nodofila);
                    }
                    else
                    {
                        nodofila.estado = null;
                    }
                    
                    nodofila = nodofila.siguiente;
                }
            }
            
            

            int v = temp.lista.Find(x => x == grafo.nodos.Count-1);
            if (v == grafo.nodos.Count - 1)
            {
                temp.aceptacion = true;
            }
            graficarTabla();
            graficarTrans();
        }
        //nuevo.lista.Find(x => x == i); nuevo.Sort()
        public void siguientesCabecera(Transicion temp)
        {
            Transicion t = temp;
            for (int i = 0; i < grafo.transiciones.Count; i++)
            {
                Transicion nuevo = new Transicion(grafo.transiciones[i].lexema);
                nuevo.tipo = grafo.transiciones[i].idToken;
                t.siguiente = nuevo;
                t = nuevo;
            }
        }

        public void llenarCabecera(Transicion temp)
        {
            for (int i = 0; i < temp.anterior.Count; i++)
            {
                temp.lista.Add(temp.anterior[i]);
                buscarVacios(temp, grafo.nodos[temp.anterior[i]]);
            }
            temp.lista = temp.lista.Distinct().ToList();
            temp.lista.Sort();

            int v = temp.lista.Find(x => x == grafo.nodos.Count - 1);
            if (v == grafo.nodos.Count - 1)
            {
                temp.aceptacion = true;
            }
        }

        public void buscarVacios(Transicion temp, NodoG raiz)
        {
            if (raiz.p.Equals("ε"))
            {
                int ver = temp.lista.Find(x => x == raiz.primero.id);
                if (ver != raiz.primero.id)
                {
                    temp.lista.Add(raiz.primero.id);
                    buscarVacios(temp, raiz.primero);
                }
               
            }

            if (raiz.s.Equals("ε"))
            {
                int ver = temp.lista.Find(x => x == raiz.segundo.id);
                if (ver != raiz.segundo.id)
                {
                    temp.lista.Add(raiz.segundo.id);
                    buscarVacios(temp, raiz.segundo);
                }
            }
                
        }

        public void llenarNodos(List<int> lista, List<NodoG> nodos, Transicion temp)
        {
            for (int i = 0; i < lista.Count; i++)
            {
                NodoG nt = nodos[lista[i]];
                if (nt.primero != null && nt.p.Equals(temp.nombre) && nt.idp == temp.tipo)
                {
                    temp.lista.Add(nt.primero.id);
                }
                //buscarCoincidencias(temp, nt);
            }
        }

        public void buscarCoincidencias(Transicion temp, NodoG raiz)
        {
            if (raiz.primero != null && raiz.p.Equals(temp.nombre) && raiz.idp == temp.tipo)
            {
                int ver = temp.lista.Find(x => x == raiz.primero.id);
                if (ver != raiz.primero.id)
                {
                    temp.lista.Add(raiz.primero.id);
                    buscarCoincidencias(temp, raiz.primero);
                }
            }

            if (raiz.p.Equals("ε"))
            {
                int ver = temp.lista.Find(x => x == raiz.primero.id);
                if (ver != raiz.primero.id)
                {
                    temp.lista.Add(raiz.primero.id);
                    buscarCoincidencias(temp, raiz.primero);
                }

            }

            if (raiz.s.Equals("ε"))
            {
                int ver = temp.lista.Find(x => x == raiz.segundo.id);
                if (ver != raiz.segundo.id)
                {
                    temp.lista.Add(raiz.segundo.id);
                    buscarCoincidencias(temp, raiz.segundo);
                }
            }

        }

        public Transicion crearCabecera(Transicion temp)
        {
            //bool cumple = false;
            
            for (int i = 0; i < transiciones.Count; i++)
            {
                Transicion t = transiciones[i];


                List<int> ups = t.anterior.Intersect(temp.lista).ToList();

                if (ups.Count == temp.lista.Count && t.anterior.Count == ups.Count)
                {
                    return t;
                }

                //if (transiciones[i].anterior.Count == temp.lista.Count)
                //{

                //    for (int j = 0; j < temp.lista.Count; j++)
                //    {
                //        if (transiciones[i].anterior[j] == temp.lista[j])
                //        {
                //            cumple = true;
                //        }
                //        else
                //        {
                //            break;
                //        }
                //    }

                //}

                //if (cumple)
                //{
                //    return transiciones[i];
                //}

            }
            
            

            //if (!cumple)
            //{
                Transicion nuevo = new Transicion("S" + estado);
                nuevo.anterior = temp.lista;
                transiciones.Add(nuevo);
                siguientesCabecera(nuevo);
                llenarCabecera(nuevo);
                estado++;

                return nuevo;
            //}

            //return null;
        }

        public void graficarTabla()
        {
            StreamWriter escribir = new StreamWriter("Reportes\\" + nombre.lexema + "-Tabla.txt");
            //escribir.WriteLine("digraph D{\nrankdir=LR;");

            escribir.WriteLine("digraph grafico{\ngraph [pad=\"0.5\", nodesep=\"0.5\", ranksep=\"2\"];\nnode [shape=plain]\nrankdir=LR;\n");
            escribir.Write("Foo [label=<\n<table border=\"0\" cellborder=\"1\" cellspacing=\"0\">\n<tr><td><i><b>Estado</b></i></td>");

            for (int i = 0; i < grafo.transiciones.Count; i++)
            {
                Token nodot = grafo.transiciones[i];
                if (nodot.lexema.Equals(">"))
                {
                    escribir.Write("<td><i><b>Signo mayor que</b></i></td>");
                }else if (nodot.lexema.Equals("<"))
                {
                    escribir.Write("<td><i><b>Signo menor que</b></i></td>");
                }
                else if (nodot.lexema.Length == 1 && nodot.lexema[0] == 10)
                {
                    escribir.Write("<td><i><b>-Salto de linea-</b></i></td>");
                }
                else if (nodot.lexema.Length == 1 && nodot.lexema[0] == 9)
                {
                    escribir.Write("<td><i><b>-Tabulacion-</b></i></td>");
                }
                else
                {
                    escribir.Write("<td><i><b>" + nodot.lexema + "</b></i></td>");
                }
            }

            escribir.Write("</tr>\n");
            
            for (int i = 0; i < transiciones.Count; i++)
            {
                Transicion tempcol = transiciones[i];

                if (tempcol.aceptacion)
                {
                    escribir.Write("<tr><td><b>*" + tempcol.nombre + " = [" + String.Join(", ", tempcol.lista) + "]</b></td>");
                }
                else
                {
                    escribir.Write("<tr><td><b>" + tempcol.nombre + " = [" + String.Join(", ", tempcol.lista) + "]</b></td>");
                }
                
            
                Transicion tempfila = tempcol.siguiente;
                while (tempfila != null)
                {
                    if (tempfila.estado != null)
                    {
                        escribir.Write("<td>[" + String.Join(", ", tempfila.lista) + "] = " + tempfila.estado.nombre + "</td>");
                    }
                    else
                    {
                        escribir.Write("<td>[" + String.Join(", ", tempfila.lista) + "]</td>");

                    }

                    tempfila = tempfila.siguiente;
                }

                escribir.Write("</tr>\n");
            }

            escribir.WriteLine("</table>>];\n}");
            
            escribir.Close();

            string texto = "/K dot -Tpng Reportes\\" + nombre.lexema + "-Tabla.txt -o Reportes\\" + nombre.lexema + "-Tabla.jpg";

            //System.Diagnostics.Process.Start("CMD.exe", "/K dot -Tpng Ella.txt -o UML.png");

            System.Diagnostics.Process.Start("CMD.exe", texto).Close();
            //System.Diagnostics.Process.Start("Reportes\\"+nombre.lexema+"Grafo.jpg");
        }

        public void graficarTrans()
        {
            StreamWriter escribir = new StreamWriter("Reportes\\" + nombre.lexema + "-AFD.txt");
            escribir.WriteLine("digraph D{\nrankdir=LR;node[];\n");

            Transicion temp = null;
            for (int j = 0; j < transiciones.Count; j++)
            {
                temp = transiciones[j];

                Transicion n = temp.siguiente;
                if (temp.aceptacion)
                {
                    escribir.WriteLine(temp.nombre + "[label=\"" + temp.nombre + "\", shape=doublecircle];");
                }
                else
                {
                    escribir.WriteLine(temp.nombre + "[label=\"" + temp.nombre + "\", shape=circle];");
                }
                
                while (n != null)
                {
                    if (n.estado != null)
                    {
                        if (n.nombre.Length == 1 && n.nombre[0] == 10)
                        {
                            escribir.WriteLine(temp.nombre + "->" + n.estado.nombre + "[label=\"-Salto de linea-\"];");
                        }
                        else if (n.nombre.Length == 1 && n.nombre[0] == 9)
                        {
                            escribir.WriteLine(temp.nombre + "->" + n.estado.nombre + "[label=\"-Tabulacion-\"];");
                        }
                        else
                        {
                            escribir.WriteLine(temp.nombre + "->" + n.estado.nombre + "[label=\"" + n.nombre + "\"];");
                        }
                    }
                    n = n.siguiente;
                }

            }
            escribir.WriteLine("}");

            escribir.Close();

            string texto = "/K dot -Tpng Reportes\\" + nombre.lexema + "-AFD.txt -o Reportes\\" + nombre.lexema + "-AFD.jpg";

            //System.Diagnostics.Process.Start("CMD.exe", "/K dot -Tpng Ella.txt -o UML.png");

            System.Diagnostics.Process.Start("CMD.exe", texto).Close();
            //System.Diagnostics.Process.Start("Reportes\\"+nombre.lexema+"Grafo.jpg");
        }

        public bool validar(string texto, int id)
        {
            listaE = new List<LexemaError>();
            listaT = new List<LexemaToken>();

            Transicion columna = transiciones[0];
            Transicion fila = columna.siguiente;
            int i = 0;
            bool validado = true;

            while (fila != null)
            {
                if (columna.aceptacion && texto.Length == i)
                {
                    break;
                }
                else if (fila.estado != null)
                {
                    if (fila.tipo == 15)
                    {
                        //recorre conjuntos
                        for (int j = 0; j < conjuntos.Count; j++)
                        {
                            //validar si conjunto es igual a estado
                            if (conjuntos[j].nombre.lexema == fila.nombre)
                            {
                                //recorrer lista de caracteres del conjuto
                                for (int k = 0; k < conjuntos[j].caracteres.Count; k++)
                                {
                                    //validar si es de tamanio 1
                                    if (conjuntos[j].caracteres[k].Length == 1 && texto[i] == conjuntos[j].caracteres[k][0])
                                    {
                                        listaT.Add(new LexemaToken(i, texto[i].ToString(), "Identificador", fila.nombre));
                                        i++;
                                        columna = fila.estado;
                                        fila = columna;
                                        break;
                                    }
                                    else if (conjuntos[j].caracteres[k].Length > 1)
                                    {
                                        int tempi = i;
                                        bool malo = false;
                                        for (int l = 0; l < conjuntos[j].caracteres[k].Length; l++)
                                        {
                                            if (tempi < texto.Length && texto[tempi] == conjuntos[j].caracteres[k][l])
                                            {
                                                tempi++;
                                            }
                                            else
                                            {
                                                malo = true;
                                                break;
                                            }
                                        }

                                        if (!malo)
                                        {
                                            listaT.Add(new LexemaToken(i, fila.nombre, "Identificador", fila.nombre));
                                            i = tempi;
                                            columna = fila.estado;
                                            fila = columna;
                                            break;
                                        }
                                    }
                                }

                                break;
                            }
                        }
                    }
                    else if (fila.tipo == 19)
                    {
                        int tempi = i;
                        bool malo = false;
                        //recorre el texto 
                        for (int j = 0; j < fila.nombre.Length; j++)
                        {
                            if (tempi < texto.Length && texto[tempi] == fila.nombre[j])
                            {
                                tempi++;
                            }
                            else
                            {
                                malo = true;
                                break;
                            }
                        }

                        if (!malo)
                        {
                            listaT.Add(new LexemaToken(i, fila.nombre, "Cadena", fila.nombre));
                            i = tempi;
                            columna = fila.estado;
                            fila = columna;
                        }
                    }

                }
                
                fila = fila.siguiente;

                if (fila == null && texto.Length > i)
                {
                    validado = false;
                    fila = columna.siguiente;
                    LexemaError nuevo = new LexemaError(texto[i].ToString(), i);
                    while (fila != null)
                    {
                        if (fila.estado != null)
                        {
                            nuevo.esperado.Add(fila.nombre);
                        }
                        fila = fila.siguiente;
                    }
                    fila = columna.siguiente;
                    listaE.Add(nuevo);
                    i++;
                }

            }

            if (validado)
            {
                ReporteTokenXML(id);
            }
            else
            {
                ReporteTokenXML(id);
                ReporteErrorXML(id);
            }

            return validado;
        }
        
        public void ReporteTokenXML(int id)
        {
            XmlDocument documento = new XmlDocument();
            XmlElement raiz = documento.CreateElement("ListaTokens" + "-" + nombre.lexema + "-" + id);
            documento.AppendChild(raiz);

            for (int i = 0; i < listaT.Count; i++)
            {
                XmlElement token = documento.CreateElement("Token");
                raiz.AppendChild(token);

                XmlElement nombre = documento.CreateElement("Posicion");
                nombre.AppendChild(documento.CreateTextNode(listaT[i].columna.ToString()));
                token.AppendChild(nombre);

                XmlElement valor = documento.CreateElement("Lexema");
                valor.AppendChild(documento.CreateTextNode(listaT[i].lexema));
                token.AppendChild(valor);

                XmlElement fila = documento.CreateElement("TipoToken");
                fila.AppendChild(documento.CreateTextNode(listaT[i].nombretoken.ToString()));
                token.AppendChild(fila);

                XmlElement columna = documento.CreateElement("NombreToken");
                columna.AppendChild(documento.CreateTextNode(listaT[i].tipo));
                token.AppendChild(columna);
            }

            documento.Save("Reportes\\ListaTokens-" + nombre.lexema + "-" + id + ".xml");
            
            //Process.Start("Reportes\\TablaTokens" + nombre.lexema + id + ".xml");
        }

        public void ReporteErrorXML(int id)
        {
            XmlDocument documento = new XmlDocument();
            XmlElement raiz = documento.CreateElement("ListaErrores-" + nombre.lexema + "-" + id);
            documento.AppendChild(raiz);

            for (int i = 0; i < listaE.Count; i++)
            {
                XmlElement token = documento.CreateElement("Token");
                raiz.AppendChild(token);

                XmlElement valor = documento.CreateElement("Caracter");
                valor.AppendChild(documento.CreateTextNode(listaE[i].caracter));
                token.AppendChild(valor);

                XmlElement fila = documento.CreateElement("Posicion");
                fila.AppendChild(documento.CreateTextNode(listaE[i].posicion.ToString()));
                token.AppendChild(fila);

                XmlElement columna = documento.CreateElement("Esperabo");
                columna.AppendChild(documento.CreateTextNode(String.Join(", ", listaE[i].esperado).ToString()));
                token.AppendChild(columna);
            }

            documento.Save("Reportes\\ListaErrores-" + nombre.lexema + "-" + id + ".xml");
            
            //Process.Start("Reportes\\TablaErrores" + nombre.lexema + id + ".xml");
        }

    }
}
