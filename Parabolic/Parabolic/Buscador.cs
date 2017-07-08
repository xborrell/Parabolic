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
            var millorResultat = RefinarAngle();

            logger.Info("-".PadRight( 79, '-' ));
            logger.Info(millorResultat.ToString());
        }

        public Resultat RefinarAngle()
        {
            var millorResultat = new Resultat(0, 0);
            var angles = SegmentDangles.CreaEnGraus(0, 90);

            while( true )
            {
                var resultatObtingut = BuscarAngles(angles);

                if (MilloremElResultat(millorResultat, resultatObtingut))
                {
                    millorResultat = resultatObtingut;
                }

                if (angles.CanZoom())
                {
                    angles = angles.Zoom(millorResultat.Angle);
                } else
                {
                    return millorResultat;
                }
            }
        }

        private Resultat BuscarAngles(SegmentDangles angles)
        {
            logger.Info($"Comprobant el segment [{angles.AngleInicial * Constants.RadianToDegreeCoeficient}, {angles.AngleFinal * Constants.RadianToDegreeCoeficient}]");
            var millorResultatPerAquestSegment = new Resultat(angles.AngleInicial, 0);

            foreach (var angleAProbar in angles.EnRadians() )
            {
                var resultatObtingut = BuscarVelocitatPerUnAngle(angleAProbar);

                if( MilloremElResultat(millorResultatPerAquestSegment, resultatObtingut) )
                {
                    millorResultatPerAquestSegment = resultatObtingut;
                }
            }

            logger.Info($"Millor resultat pel segment [{angles.AngleInicial * Constants.RadianToDegreeCoeficient}, {angles.AngleFinal * Constants.RadianToDegreeCoeficient}] = {millorResultatPerAquestSegment.ToString()}");

            return millorResultatPerAquestSegment;
        }

        private Resultat BuscarVelocitatPerUnAngle(float angleEnProva)
        {
            logger.Debug($"Comprobant angle {angleEnProva * Constants.RadianToDegreeCoeficient}");

            var incrementDeVelocitat = 1000F;
            var velocitatEnProva = 0F;
            var millorResultatPerAquestAngle = new Resultat(angleEnProva, 0);

            while (Math.Abs(incrementDeVelocitat) > 0.001)
            {
                velocitatEnProva += incrementDeVelocitat;

                var resultatEnProva = new Resultat(angleEnProva, velocitatEnProva);

                MostraResultat(millorResultatPerAquestAngle, resultatEnProva, incrementDeVelocitat);

                if (MilloremElResultat(millorResultatPerAquestAngle, resultatEnProva))
                {
                    millorResultatPerAquestAngle = resultatEnProva;
                }
                else
                {
                    incrementDeVelocitat = AfinarElIncrement(millorResultatPerAquestAngle, incrementDeVelocitat);
                    velocitatEnProva = millorResultatPerAquestAngle.VelocitatInicial;
                }

                if(millorResultatPerAquestAngle.Distancia == 0 )
                {
                    break;
                }
            }

            logger.Debug(millorResultatPerAquestAngle.ToString());

            return millorResultatPerAquestAngle;
        }

        private void MostraResultat(Resultat millorResultatPerAquestAngle, Resultat resultatEnProva, float i)
        {
            var esMillor = MilloremElResultat(millorResultatPerAquestAngle, resultatEnProva) ? '*' : ' ';
            var angleEnGraus = Constants.RadianToDegreeCoeficient * resultatEnProva.Angle;
            var n = resultatEnProva.Sentit > 0 ? -resultatEnProva.Distancia : resultatEnProva.Distancia;
            logger.Debug($"{esMillor} A={angleEnGraus,3}º, V={resultatEnProva.VelocitatInicial,8:0.000} m/s, D={n,13:n} m., I={i,10:0.00000}");
        }

        private bool MilloremElResultat(Resultat millorResultatPerAquestAngle, Resultat nou)
        {
            return (millorResultatPerAquestAngle.CompareTo(nou) > 0);
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
