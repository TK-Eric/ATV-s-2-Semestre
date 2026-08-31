using BCrypt.Net;

namespace EventPlus.WebAPI.Utils
{

    /// <summary>
    /// Utilitário estático responsável pelas operações de criptografia e hashing de senhas na API
    /// </summary>
    public static class Criptografia
    {

        public static string GerarHash(string senha)
        {
            //retorna a senha criptografada
            return BCrypt.Net.BCrypt.HashPassword(senha);
        }


        public static bool CompararHash(string senhaInformada, string senhaBanco)
        {
            if (string.IsNullOrEmpty(senhaInformada) || string.IsNullOrEmpty(senhaBanco))
            {
                return false;
            }

            try
            {
                //retorna o resultado(bool) da verificação da senha informada com a senha do banco
                return BCrypt.Net.BCrypt.Verify(senhaInformada, senhaBanco);
            }
            catch (BCrypt.Net.SaltParseException)
            {
                return false;
            }
        }
    }
}