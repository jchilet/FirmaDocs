using System;

namespace FirmaDocs;

/// <summary>Rechazo registrado por un firmante sobre una versión del documento.</summary>
/// <param name="Firmante">Quién rechazó.</param>
/// <param name="Motivo">Motivo del rechazo.</param>
/// <param name="Version">Versión del documento rechazada.</param>
/// <param name="Instante">Momento del rechazo.</param>
public record Rechazo(Firmante Firmante, string Motivo, int Version, DateTimeOffset Instante);
