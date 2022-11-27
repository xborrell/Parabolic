using log4net;

namespace Parabolic.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            var buscador = new Buscador();

            //buscador.Executar();

            var logger = LogManager.GetLogger("koko");

            var angles = SegmentDangles.CreaEnGraus(0, 90);
            logger.Info(angles);
            angles = angles.Zoom(40 * Constants.DegreeToRadianCoeficient);
            logger.Info(angles);

            foreach( var angle in angles.EnRadians())
            {
                logger.Info($"    {angle * Constants.RadianToDegreeCoeficient}");
                var r = buscador.BuscarVelocitatPerUnAngle(angle);
                logger.Info($"    {r}");
            }
        }
    }
}
