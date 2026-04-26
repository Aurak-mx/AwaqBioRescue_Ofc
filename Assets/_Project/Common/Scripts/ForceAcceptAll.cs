using UnityEngine;
using UnityEngine.Networking;

// Funcion para aceptar cualquier certificado SSL/TLS sin validarlo
public class ForceAcceptAll : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        return true;
    }
}
