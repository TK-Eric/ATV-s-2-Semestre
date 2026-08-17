namespace EventPlus.WebAPI.Utils
{
    /// <summary>
    /// Utilitario estatico responsavel pelas operações de criptografia e hsahinga nas APIs
    /// </summary>
    public static class Criptografia
    {
        public static string GerarHash(string senha) 
        {
            return BCrypt.Net.BCrypt.HashPassword(senha);
        }

        internal static bool CompararHash(string senhaInformada, string senhaBanco)
        {
            if(string.IsNullOrEmpty(senhaInformada) || string.IsNullOrEmpty(senhaBanco))
            {
                return false;
            }
            return BCrypt.Net.BCrypt.Verify(senhaInformada, senhaBanco);

            throw new NotImplementedException();
        }
    }
}
