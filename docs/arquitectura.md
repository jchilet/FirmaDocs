# Arquitectura — FirmaDocs
- Capas: consola → dominio → repositorios.
- Actualmente no existe persistencia.
- Cada capacidad tiene su propia carpeta.
- La capacidad de circuitos está en FirmaDocs/Circuito/.
- Program.cs no debe contener lógica de negocio.
- Los errores de dominio utilizan CircuitoFirmaException.
- El instante se inyecta como DateTimeOffset.