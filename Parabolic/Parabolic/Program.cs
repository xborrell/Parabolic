namespace Parabolic
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            var buscador = new Buscador();

            buscador.Executar();
        }
    }
}
