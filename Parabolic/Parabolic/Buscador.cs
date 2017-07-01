using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Parabolic
{
    public class Buscador
    {
        const int DistanciaACubrir = 1151000; // distancia a Breda en metres

        private Calculador calculador = new Calculador();
        private ILog logger;

        public Buscador()
        {
            this.logger = LogManager.GetLogger(this.GetType());

        }

        public void Executar()
        {
            var resultat = RefinarAngle();

            logger.Info(resultat.ToString());
        }

        public Resultat RefinarAngle()
        {
            var angles = new SegmentDangles(0, 90, 10);

            Resultat millorResultat = null;

            millorResultat = BuscarAngles(angles);
            angles = angles.ZoomEnRadians(millorResultat.Angle);

            return millorResultat;
        }

        private Resultat BuscarAngles(SegmentDangles angles)
        {
            Resultat millorResultatObtingut = null;

            foreach(var angleAProbar in angles.EnRadians() )
            {
                var resultatEnProva = BuscarVelocitatPerUnAngle(angleAProbar);

                if (millorResultatObtingut == null || MilloremElResultat(millorResultatObtingut, resultatEnProva))
                {
                    millorResultatObtingut = resultatEnProva;
                    logger.Info(millorResultatObtingut.ToString());
                }
            }

            return millorResultatObtingut;
        }

        private Resultat BuscarVelocitatPerUnAngle(float angleEnProva)
        {
            var incrementDeVelocitat = 1000F;
            var velocitatEnProva = 0F;
            var millorResultatObtingut = new Resultat(angleEnProva, velocitatEnProva);

            while ((millorResultatObtingut.Distancia > 0) && (Math.Abs(incrementDeVelocitat) > 0.001))
            {
                velocitatEnProva += incrementDeVelocitat;

                var resultatEnProva = new Resultat(angleEnProva, velocitatEnProva);

                MostraResultat(millorResultatObtingut, resultatEnProva, incrementDeVelocitat);

                if (MilloremElResultat(millorResultatObtingut, resultatEnProva))
                {
                    millorResultatObtingut = resultatEnProva;
                }
                else
                {
                    incrementDeVelocitat = AfinarElIncrement(millorResultatObtingut, incrementDeVelocitat);
                    velocitatEnProva = millorResultatObtingut.VelocitatInicial;
                }
            }

            return millorResultatObtingut;
        }

        private void MostraResultat(Resultat millorResultatObtingut, Resultat resultatEnProva, float i)
        {
            var esMillor = MilloremElResultat(millorResultatObtingut, resultatEnProva) ? '*' : ' ';
            var angleEnGraus = Constants.RadianToDegreeCoeficient * resultatEnProva.Angle;
            var n = resultatEnProva.Sentit > 0 ? -resultatEnProva.Distancia : resultatEnProva.Distancia;
            logger.Debug($"{esMillor} A={angleEnGraus,3}º, V={resultatEnProva.VelocitatInicial,8:0.000} m/s, D={n,13:n} m., I={i,10:0.00000}");
        }

        private bool MilloremElResultat(Resultat guardat, Resultat nou)
        {
            return (guardat.CompareTo(nou) > 0);
        }

        private float AfinarElIncrement(Resultat millorResultat, float incrementAnterior)
        {
            return Math.Abs(incrementAnterior) * 0.5F * millorResultat.Sentit;
        }

        private float CalculaNovaVelocitat(float velocitat, float increment)
        {
            return (float)Math.Round(velocitat + increment, 3);
        }
    }
}
