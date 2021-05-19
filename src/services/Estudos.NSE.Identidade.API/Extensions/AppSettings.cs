namespace Estudos.NSE.Identidade.API.Extensions
{
    public class AppSettings
    {
        /// <summary>
        /// CHAVE DE ENCRIPTAÇÃO
        /// </summary>
        public string Secret { get; set; }
        /// <summary>
        /// TEMPO DE DURAÇÃO DA VALIDADE DO TOKEN
        /// </summary>
        public int ExpiracaoHoras { get; set; }
        /// <summary>
        /// EMISSO DO TOKEN
        /// </summary>
        public string Emissor { get; set; }
        /// <summary>
        /// ONDE O TOKEN É VÁLIDO (AUDIÊNCIA)
        /// </summary>
        public string ValidoEm { get; set; }
    }
}