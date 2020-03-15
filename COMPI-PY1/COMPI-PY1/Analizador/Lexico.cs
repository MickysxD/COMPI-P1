using COMPI_PY1.Clase;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace COMPI_PY1.Analizador
{
    class Lexico
    {
        public List<Token> listaT { get; set; }
        public List<TokenError> listaE { get; set; }
        public string texto { get; set; }
        public RichTextBox salida { get; set; }
        public ComboBox seleccion { get; set; }
        public List<Exprecion> expreciones { get; set; }

        public Lexico(string texto, RichTextBox t, ComboBox seleccion)
        {
            this.texto = texto;
            this.salida = t;
            this.seleccion = seleccion;
            this.listaT = new List<Token>();
            this.listaE = new List<TokenError>();
            this.expreciones = new List<Exprecion>();
        }
     
        public void Analizar() {
            int token = 0;
            int error = 0;
            int fila = 1;
            int columna = 1;
            int puntero = 0;
            int estado = 0;
            char caracter = ' ';
            char[] linea = texto.ToCharArray(0, texto.Length);
            string lexema = "";

            while (puntero < linea.Length) {
                caracter = linea[puntero];
                //salida.AppendText(caracter+"");
                switch (estado)
                {
                    case 0:
                        if (caracter.Equals('\t') || caracter.Equals(' ')) {
                            puntero++;
                        }
                        else if (caracter.Equals('\n'))
                        {
                            puntero++;
                            fila++;
                            columna = 1;
                        }
                        else if (caracter.Equals('.'))
                        {
                            listaT.Add(new Token(token, 1, "Concatenacion", ".", fila, columna));
                            token++;
                            columna++;
                            puntero++;
                        }
                        else if (caracter.Equals('|'))
                        {
                            listaT.Add(new Token(token, 2, "Disyuncion", "|", fila, columna));
                            token++;
                            columna++;
                            puntero++;
                        }
                        else if (caracter.Equals('?'))
                        {
                            listaT.Add(new Token(token, 3, "Cerradura ?", "?", fila, columna));
                            token++;
                            columna++;
                            puntero++;
                        }
                        else if (caracter.Equals('*'))
                        {
                            listaT.Add(new Token(token, 4, "Cerradura de Kleen", "*", fila, columna));
                            token++;
                            columna++;
                            puntero++;
                        }
                        else if (caracter.Equals('+'))
                        {
                            listaT.Add(new Token(token, 5, "Cerradura +", "+", fila, columna));
                            token++;
                            columna++;
                            puntero++;
                        }
                        else if (caracter.Equals('{'))
                        {
                            listaT.Add(new Token(token, 6, "Llave que abre", "{", fila, columna));
                            token++;
                            columna++;
                            puntero++;
                        }
                        else if (caracter.Equals('}'))
                        {
                            listaT.Add(new Token(token, 7, "Llave que cierra", "}", fila, columna));
                            token++;
                            columna++;
                            puntero++;
                        }
                        else if (caracter.Equals(':'))
                        {
                            listaT.Add(new Token(token, 8, "Dos puntos", ":", fila, columna));
                            token++;
                            columna++;
                            puntero++;
                        }
                        else if (caracter.Equals(';'))
                        {
                            listaT.Add(new Token(token, 9, "Punto y coma", ";", fila, columna));
                            token++;
                            columna++;
                            puntero++;
                        }
                        else if (caracter.Equals('~'))
                        {
                            listaT.Add(new Token(token, 10, "Conjunto", "~", fila, columna));
                            token++;
                            columna++;
                            puntero++;
                        }
                        else if (caracter.Equals(','))
                        {
                            listaT.Add(new Token(token, 11, "Coma", ",", fila, columna));
                            token++;
                            columna++;
                            puntero++;
                        }
                        else if (caracter.Equals('/'))
                        {
                            lexema += caracter;
                            puntero++;
                            estado = 1;
                        }
                        else if (caracter.Equals('<'))
                        {
                            lexema += caracter;
                            puntero++;
                            estado = 3;
                        }
                        else if ((char.IsLetterOrDigit(caracter) && caracter < 123))
                        {
                            lexema += caracter;
                            puntero++;
                            estado = 6;
                        }
                        else if (char.IsDigit(caracter))
                        {
                            lexema += caracter;
                            puntero++;
                            estado = 7;
                        }
                        else if (caracter.Equals('\"'))
                        {
                            puntero++;
                            estado = 8;
                        }
                        else if (caracter.Equals('-'))
                        {
                            puntero++;
                            lexema += caracter;
                            estado = 10;
                        }
                        else
                        {
                            estado = 100;
                        }
                        break;

                    case 1:
                        if (caracter.Equals('/')) {
                            lexema = "";
                            puntero++;
                            estado = 2;
                        }
                        else
                        {
                            lexema = "";
                            puntero--;
                            estado = 100;
                        }

                        break;

                    case 2:
                        if (!caracter.Equals('\n')) {
                            lexema += caracter;
                            puntero++;
                        }
                        else
                        {
                            listaT.Add(new Token(token, 13, "Comentario de una linea", lexema, fila, columna));
                            token++;
                            columna++;
                            lexema = "";
                            estado = 0;
                        }
                        break;

                    case 3:
                        if (caracter.Equals('!'))
                        {
                            lexema = "";
                            puntero++;
                            estado = 4;
                        }
                        else
                        {
                            puntero--;
                            lexema = "";
                            estado = 100;
                        }
                        break;

                    case 4:
                        if (!caracter.Equals('!'))
                        {
                            lexema += caracter;
                            puntero++;
                        }
                        else
                        {
                            puntero++;
                            estado = 5;
                        }
                        break;

                    case 5:
                        if (caracter.Equals('>'))
                        {
                            listaT.Add(new Token(token, 13, "Comentario multilinea", lexema, fila, columna));
                            token++;
                            columna++;
                            puntero++;
                            lexema = "";
                            estado = 0;
                        }
                        else
                        {
                            lexema += caracter;
                            puntero++;
                            estado = 4;
                        }
                        break;

                    case 6:
                        if ((char.IsLetterOrDigit(caracter) && caracter < 123) || caracter.Equals('_'))
                        {
                            lexema += caracter;
                            puntero++;
                        }
                        else
                        {
                            if (lexema.ToLower().Equals("conj"))
                            {
                                listaT.Add(new Token(token, 14, "Palabra reservada", lexema, fila, columna));
                            }
                            else
                            { 
                                listaT.Add(new Token(token, 15, "Identificador", lexema, fila, columna));
                            }
                            token++;
                            columna++;
                            lexema = "";
                            estado = 0;
                        }
                        break;
                        

                    case 7:
                        if (char.IsDigit(caracter))
                        {
                            lexema += caracter;
                            puntero++;
                        }
                        else
                        {
                            if (lexema.Length == 1)
                            {
                                listaT.Add(new Token(token, 16, "Digito", lexema, fila, columna));
                            }
                            else
                            {
                                listaT.Add(new Token(token, 17, "Numero", lexema, fila, columna));
                            }
                            token++;
                            columna++;
                            lexema = "";
                            estado = 0;
                        }
                        break;

                    case 8:
                        if (caracter.Equals('\\'))
                        {
                            lexema += caracter;
                            puntero++;
                            estado = 9;
                        }
                        else if (!caracter.Equals('\"'))
                        {
                            lexema += caracter;
                            puntero++;
                        }
                        else
                        {
                            listaT.Add(new Token(token, 19, "Texto", lexema, fila, columna));
                            token++;
                            columna++;
                            lexema = "";
                            puntero++;
                            estado = 0;
                        }
                        break;

                    case 9:
                        lexema += caracter;
                        puntero++;
                        estado = 8;
                        break;

                    case 10:
                        if (caracter.Equals('>'))
                        {
                            listaT.Add(new Token(token, 20, "Asignacion", lexema+caracter, fila, columna));
                            token++;
                            columna++;
                            lexema = "";
                            puntero++;
                            estado = 0;
                        }
                        else
                        {
                            lexema = "";
                            puntero--;
                            estado = 100;
                        }
                        break;

                    case 100:
                        if (!char.IsLetterOrDigit(caracter) && 32 < caracter && caracter < 126)
                        {
                            listaT.Add(new Token(token, 18, "Signo", caracter+"", fila, columna));
                            token++;
                            columna++;
                            lexema = "";
                            estado = 0;
                            puntero++;
                        }
                        else
                        {
                            listaE.Add(new TokenError(error, caracter+"", fila, columna));
                            error++;
                            columna++;
                            estado = 0;
                            puntero++;
                        }
                        
                        break;

                    default:
                        break;
                }

            }

            if (listaE.Count > 0)
            {
                ReporteToken();
                ReporteError();
            }
            else
            {
                ReporteToken();
                Expreciones();
            }


        }

        public void Expreciones() {
            //epsilon ε

            Token temp = null;
            Exprecion nuevo = new Exprecion();
            int estado = 0;
            int i = 0;

            while (i < listaT.Count)
            {
                temp = listaT[i];
                int no = temp.idToken;
                switch (estado)
                {
                    case 0:
                        if (no == 14) {
                            i++;
                            estado = 1;
                        }
                        else if (no == 15)
                        {
                            i++;
                            estado = 2;
                            nuevo = new Exprecion();
                            nuevo.nombre = temp;
                        }
                        else
                        {
                            i++;
                        }
                        break;

                    case 1:
                        if (no == 9)
                        {
                            i++;
                            estado = 0;
                        }
                        else
                        {
                            i++;
                        }
                        break;

                    case 2:
                        if (no == 20)
                        {
                            i++;
                            estado = 3;
                        }
                        else
                        {
                            estado = 1;
                        }
                        break;

                    case 3:
                        if (no == 1 || no == 2 || no == 3 || no == 4 || no == 5 || no == 19)
                        {
                            nuevo.tokens.Add(temp);
                            i++;
                            estado = 3;
                        }
                        else if (no == 6)
                        {
                            i++;
                            estado = 4;
                        }
                        else if (no == 9)
                        {
                            expreciones.Add(nuevo);
                            nuevo.grafo.comienzo(nuevo.tokens, nuevo.nombre);
                            nuevo.grafo.graficar();
                            nuevo.crearPrimera();
                            seleccion.Items.Add(nuevo.nombre.lexema+"Grafo");
                            seleccion.Items.Add(nuevo.nombre.lexema + "Transicion");
                            i++;
                            estado = 0;
                        }
                        else
                        {
                            estado = 1;
                        }
                        break;

                    case 4:
                        if (no == 15)
                        {
                            nuevo.tokens.Add(temp);
                            i++;
                            estado = 4;
                        }
                        else if (no == 7)
                        {
                            i++;
                            estado = 3;
                        }
                        else
                        {
                            estado = 1;
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        public void ReporteToken()
        {
            XmlDocument documento = new XmlDocument();
            XmlElement raiz = documento.CreateElement("ListaTokens");
            documento.AppendChild(raiz);

            for (int i = 0; i < listaT.Count; i++)
            {
                XmlElement token = documento.CreateElement("Token");
                raiz.AppendChild(token);

                XmlElement nombre = documento.CreateElement("Nombre");
                nombre.AppendChild(documento.CreateTextNode(listaT[i].tipo));
                token.AppendChild(nombre);

                XmlElement valor = documento.CreateElement("Valor");
                valor.AppendChild(documento.CreateTextNode(listaT[i].lexema));
                token.AppendChild(valor);

                XmlElement fila = documento.CreateElement("Fila");
                fila.AppendChild(documento.CreateTextNode(listaT[i].fila.ToString()));
                token.AppendChild(fila);

                XmlElement columna = documento.CreateElement("Columna");
                columna.AppendChild(documento.CreateTextNode(listaT[i].columna.ToString()));
                token.AppendChild(columna);
            }

            documento.Save("Reportes\\TablaTokens.xml");
            
            //StreamWriter escribir = new StreamWriter("Reportes\\TablaTokens.html");
            //escribir.WriteLine("{0}", "<!DOCTYPE html>");
            //escribir.WriteLine("{0}", "<html>");
            //escribir.WriteLine("{0}", "<head>");
            //escribir.WriteLine("{0}", "<title>");
            //escribir.WriteLine("{0}", "Tabla de Tokens");
            //escribir.WriteLine("{0}", "</title>");
            //escribir.WriteLine("{0}", "</head>");
            //escribir.WriteLine("{0}", "<body>");
            //escribir.WriteLine("{0}", "<table>");
            //escribir.WriteLine("{0}", "<tr>");
            //escribir.WriteLine("{0}{1}{2}{3}{4}", "<td>", "<strong>", "No", "</strong>", "</td>");
            //escribir.WriteLine("{0}{1}{2}{3}{4}", "<td>", "<strong>", "Id Token", "</strong>", "</td>");
            //escribir.WriteLine("{0}{1}{2}{3}{4}", "<td>", "<strong>", "Tipo", "</strong>", "</td>");
            //escribir.WriteLine("{0}{1}{2}{3}{4}", "<td>", "<strong>", "Lexema", "</strong>", "</td>");
            //escribir.WriteLine("{0}{1}{2}{3}{4}", "<td>", "<strong>", "Fila", "</strong>", "</td>");
            //escribir.WriteLine("{0}{1}{2}{3}{4}", "<td>", "<strong>", "Columna", "</strong>", "</td>");
            //escribir.WriteLine("{0}", "</tr>");


            //for (int i = 0; i < listaT.Count; i++)
            //{
            //    escribir.WriteLine("{0}", "<tr>");
            //    escribir.WriteLine("{0}{1}{2}", "<td>", listaT[i].noToken, "</td>");
            //    escribir.WriteLine("{0}{1}{2}", "<td>", listaT[i].idToken, "</td>");
            //    escribir.WriteLine("{0}{1}{2}", "<td>", listaT[i].tipo, "</td>");
            //    escribir.WriteLine("{0}{1}{2}", "<td>", listaT[i].lexema, "</td>");
            //    escribir.WriteLine("{0}{1}{2}", "<td>", listaT[i].fila, "</td>");
            //    escribir.WriteLine("{0}{1}{2}", "<td>", listaT[i].columna, "</td>");
            //    escribir.WriteLine("{0}", "</tr>");

            //}

            //escribir.WriteLine("{0}", "</table>");
            //escribir.WriteLine("{0}", "</body>");
            //escribir.WriteLine("{0}", "</html>");
            //escribir.Close();
            
            Process.Start("Reportes\\TablaTokens.xml");
        }

        public void ReporteError()
        {
            XmlDocument documento = new XmlDocument();
            XmlElement raiz = documento.CreateElement("ListaErrores");
            documento.AppendChild(raiz);
            
            for (int i = 0; i < listaE.Count; i++)
            {
                XmlElement token = documento.CreateElement("Token");
                raiz.AppendChild(token);
                
                XmlElement valor = documento.CreateElement("Valor");
                valor.AppendChild(documento.CreateTextNode(listaE[i].lexema));
                token.AppendChild(valor);

                XmlElement fila = documento.CreateElement("Fila");
                fila.AppendChild(documento.CreateTextNode(listaE[i].fila.ToString()));
                token.AppendChild(fila);

                XmlElement columna = documento.CreateElement("Columna");
                columna.AppendChild(documento.CreateTextNode(listaE[i].columna.ToString()));
                token.AppendChild(columna);
            }

            documento.Save("Reportes\\TablaErrores.xml");

            //StreamWriter escribir = new StreamWriter("Reportes\\TablaErrores.html");
            //escribir.WriteLine("{0}", "<!DOCTYPE html>");
            //escribir.WriteLine("{0}", "<html>");
            //escribir.WriteLine("{0}", "<head>");
            //escribir.WriteLine("{0}", "<title>");
            //escribir.WriteLine("{0}", "Tabla de errores");
            //escribir.WriteLine("{0}", "</title>");
            //escribir.WriteLine("{0}", "</head>");
            //escribir.WriteLine("{0}", "<body>");
            //escribir.WriteLine("{0}", "<table>");
            //escribir.WriteLine("{0}", "<tr>");
            //escribir.WriteLine("{0}{1}{2}{3}{4}", "<td>", "<strong>", "No", "</strong>", "</td>");
            //escribir.WriteLine("{0}{1}{2}{3}{4}", "<td>", "<strong>", "Lexema", "</strong>", "</td>");
            //escribir.WriteLine("{0}{1}{2}{3}{4}", "<td>", "<strong>", "Fila", "</strong>", "</td>");
            //escribir.WriteLine("{0}{1}{2}{3}{4}", "<td>", "<strong>", "Columna", "</strong>", "</td>");
            //escribir.WriteLine("{0}", "</tr>");


            //for (int i = 0; i < listaE.Count; i++)
            //{
            //    escribir.WriteLine("{0}", "<tr>");
            //    escribir.WriteLine("{0}{1}{2}", "<td>", listaE[i].noToken, "</td>");
            //    escribir.WriteLine("{0}{1}{2}", "<td>", listaE[i].lexema, "</td>");
            //    escribir.WriteLine("{0}{1}{2}", "<td>", listaE[i].fila, "</td>");
            //    escribir.WriteLine("{0}{1}{2}", "<td>", listaE[i].columna, "</td>");
            //    escribir.WriteLine("{0}", "</tr>");

            //}

            //escribir.WriteLine("{0}", "</table>");
            //escribir.WriteLine("{0}", "</body>");
            //escribir.WriteLine("{0}", "</html>");
            //escribir.Close();

            Process.Start("Reportes\\TablaErrores.xml");
        }

    }
}
