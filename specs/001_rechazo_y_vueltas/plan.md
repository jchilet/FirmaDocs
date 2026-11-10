# Plan de implementación · Rechazo y nueva vuelta

## 1) Archivos a crear o modificar

**Crear**
- `FirmaDocs/Circuito/Rechazo.cs`  
  Record de dominio para registrar evidencia de rechazo (`firmante`, `motivo`, `version`, `instante`).

**Modificar**
- `FirmaDocs/Circuito/EstadoDocumento.cs`  
  Incorporar estados requeridos por la spec (`Devuelto`, `Rechazado`) y ajustar XML docs.
- `FirmaDocs/Circuito/CircuitoFirma.cs`  
  Implementar reglas de rechazo/reenvío, preservación de historial y cálculo de turno por versión vigente.
- `FirmaDocs.Tests/Circuito/CircuitoFirmaTests.cs`  
  Agregar tests RF-01 a RF-07 sin tocar los tests existentes.

**No se modifica**
- `Program.cs` (por constitución).
- Constructor, `Enviar()` y `Aprobar(...)` como firmas públicas existentes (por reglas del repo).

---

## 2) Tipos nuevos y miembros públicos nuevos

### Tipos nuevos
- `record Rechazo(Firmante Firmante, string Motivo, int Version, DateTimeOffset Instante)`

### Miembros públicos nuevos en `CircuitoFirma`
- `IReadOnlyList<Rechazo> Rechazos`  
  Exposición de evidencia de rechazos registrados (sin mutación externa).
- `void Rechazar(Firmante firmante, string motivo, DateTimeOffset instante)`  
  Rechazo en turno con motivo obligatorio.
- `void Reenviar(string autorId)`  
  Reenvío solo por autor cuando el estado es `Devuelto`.

> Se mantienen intactas las firmas públicas existentes: constructor, `Enviar()`, `Aprobar(...)`.

---

## 3) Cálculo del turno sin contar firmas de versiones anteriores

Regla:
- El turno se calcula con la cantidad de firmas cuya `Version == Version` vigente.

En términos de comportamiento:
1. Se conserva todo el historial de firmas de todas las versiones.
2. Para determinar `FirmanteEnTurno`, se cuenta solo el subconjunto de firmas de la versión actual.
3. El índice de turno = `firmasVigentes.Count`.
4. La aprobación completa ocurre cuando `firmasVigentes.Count == cantidadFirmantes`.

Efecto:
- Firmas de versiones previas permanecen como evidencia, pero no adelantan turno ni cierran aprobación de la versión actual.

---

## 4) Alternativas de diseño descartadas

### Alternativa A: crear un `CircuitoFirma` nuevo por cada vuelta
**Descartada** porque se pierde el historial unificado de firmas y rechazos, además de complicar el conteo de vueltas y la trazabilidad del documento.

### Alternativa B: vaciar las firmas al reenviar
**Descartada** porque viola el principio 6 de la constitución: una firma registrada nunca se borra ni se modifica. También incumple RF-03, que exige conservar las firmas de versiones anteriores.

---

## 5) Riesgos y mitigaciones

1. **Riesgo: regresión en tests existentes de aprobación.**  
   **Mitigación:** no cambiar contratos existentes; ejecutar suite completa y validar los 5 tests actuales en verde.

2. **Riesgo: errores de estado en transiciones (`EnCircuito`/`Devuelto`/`Rechazado`).**  
   **Mitigación:** tests por RF y por caso límite de estado inválido.

3. **Riesgo: mensajes de error no coinciden exactamente.**  
   **Mitigación:** assertions por texto exacto para mensajes obligatorios definidos en spec.

4. **Riesgo: contar firmas históricas para la versión vigente por error.**  
   **Mitigación:** encapsular cálculo por versión vigente y cubrir con tests RF-03.

5. **Riesgo: reenvío indebido tras rechazo en versión 3.**  
   **Mitigación:** validar estado terminal `Rechazado` y test específico RF-04.

---

## 6) Orden de implementación

1. **Tests primero (TDD):** agregar tests RF-01 a RF-07 en rojo.
2. **Modelo de estado:** actualizar `EstadoDocumento` con `Devuelto` y `Rechazado`.
3. **Modelo de rechazo:** crear `Rechazo`.
4. **Lógica de dominio en `CircuitoFirma`:**
   - rechazo con motivo,
   - registro de evidencia,
   - reenvío por autor,
   - transición terminal en versión 3,
   - cálculo de turno/aprobación por firmas de versión vigente.
5. **Ajuste de mensajes exactos y XML docs en español.**
6. **Validación final:** ejecutar `dotnet test` y confirmar verde total.
