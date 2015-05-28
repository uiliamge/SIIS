using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SIIS.Helpers
{
    public static class StringExtensions
    {
        public static string FormatoDataHoraOuVazio(this DateTime? dateTime)
        {
            return dateTime == null ? "" : dateTime.Value.ToString("dd/MM/yyyy HH:mm:ss");
        }

        public static string FormatoDataHora(this DateTime dateTime)
        {
            return dateTime.ToString("dd/MM/yyyy HH:mm:ss");
        }

        public static string FormatoDataOuVazio(this DateTime? dateTime)
        {
            return dateTime == null ? "" : dateTime.Value.ToString("dd/MM/yyyy");
        }

        public static string FormatoData(this DateTime dateTime)
        {
            return dateTime.ToString("dd/MM/yyyy");
        }

        public static string FormatoHoraOuVazio(this DateTime? dateTime)
        {
            return dateTime == null ? "" : dateTime.Value.ToString("HH:mm");
        }

        public static string FormatoHoraOuVazio(this TimeSpan? time)
        {
            return time == null ? "" : time.Value.ToString("hh':'mm");
        }

        public static string FormatoHora(this TimeSpan time)
        {
            return time.ToString("hh':'mm");
        }

        public static string FormatoMoedaOuVazio(this int? valor)
        {
            return valor == null ? "" : valor.Value.ToString("N");
        }

        public static string FormatoMoedaOuVazio(this decimal? valor)
        {
            return valor == null ? "" : valor.Value.ToString("N");
        }

        public static string FormataMoeda(this decimal valor)
        {
            return valor.ToString("N");
        }

        public static string FormataPorcentagem(this decimal valor)
        {
            return valor.ToString("F5");
        }

        public static string FormataPorcentagemOuVazio(this decimal? valor)
        {
            return valor == null ? "" : valor.Value.ToString("F5");
        }

        public static string FormataPorcentagem2Casas(this decimal valor)
        {
            return valor.ToString("F2");
        }

        public static string FormataTextoMaximo(this string valor, int tamanhoMaximo)
        {
            return (!String.IsNullOrEmpty(valor) && valor.Length > tamanhoMaximo) ? valor.Remove(tamanhoMaximo) + "..." : valor;
        }

        public static string FormataCnpj(this string valor)
        {
            if (valor != String.Empty)
                return Convert.ToUInt64(valor).ToString(@"00\.000\.000\/0000\-00");

            return valor;
        }

        public static string FormataCpf(this string valor)
        {
            if (valor != String.Empty)
                return Convert.ToUInt64(valor).ToString(@"000\.000\.000\-00");

            return valor;
        }

        public static string FormataCpfouCnpj(this string texto)
        {
            if (!string.IsNullOrEmpty(texto))
            {
                texto = texto.Replace("/", "").Replace(".", "").Replace("-", "");
                if (texto.Length == 11)
                    return Convert.ToUInt64(texto).ToString(@"000\.000\.000\-00");
                else
                    return Convert.ToUInt64(texto).ToString(@"00\.000\.000\/0000\-00");
            }

            return string.Empty;

        }

        /// <summary>
        /// Utilizado para formatação de CEP
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static string FormataCep(this string valor)
        {
            string retorno = string.Empty;

            if (!string.IsNullOrEmpty(valor))
                retorno = Convert.ToUInt64(valor).ToString(@"00000\-000");

            return retorno;
        }

        /// <summary>
        /// Utilizado para formatação de telefone nacional (brasileiro) com 11 ou 12 dígitos incluindo DDD de dois dígitos.
        /// </summary>
        /// <param name="texto">texto</param>
        /// <returns></returns>
        public static string FormataTelefoneNacional(this string texto)
        {
            if (texto != null)
            {
                //Remove tudo que não for número da string.
                texto = Regex.Replace(texto, @"[^\d]", string.Empty);

                if (texto.Length == 10)
                    return Convert.ToInt64(texto).ToString("(00) 0000-0000");
                else
                    return Convert.ToInt64(texto).ToString("(00) 0000-00000");
            }

            return string.Empty;
        }

        /// <summary>
        /// Utilizado para formatação de telefone nacional (brasileiro) com 11 ou 12 dígitos incluindo DDD de dois dígitos.
        /// </summary>
        /// <returns></returns>
        public static string FormataTelefoneNacional(this string[] texto, string separador)
        {
            string fones = string.Empty;

            if (texto != null)
                foreach (string fone in texto)
                {
                    if (!String.IsNullOrEmpty(fone))
                        fones += fone.FormataTelefoneNacional() + separador;
                }


            return fones;
        }

        public static string FormatoNumericoPadrao(this decimal? numero)
        {
            return numero == null ? string.Empty : string.Format(CultureInfo.InvariantCulture, "{0:0.0000}", numero).Replace(".", ",");
        }

        public static string RemoverMascara(this string str)
        {
            return str != null ? Regex.Replace(str, @"[^\d]", string.Empty) : str;
        }
    }
}
