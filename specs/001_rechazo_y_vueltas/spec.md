# Rechazo y nueva vuelta · spec v1

## 1) Contexto y alcance

`CircuitoFirma` debe permitir que el firmante en turno rechace un documento con un motivo, que el autor lo corrija y lo reenvíe, y que el historial quede como evidencia.

**Alcance de esta spec:**
- Flujo de rechazo y reenvío por vueltas (versión).
- Estados involucrados: `Borrador`, `EnCircuito`, `Devuelto`, `Aprobado` y `Rechazado`.
- Reglas de validación y mensajes de error de dominio.
- Conservación del historial de firmas y rechazos como evidencia.

**Fuera de alcance:**
- Cambios de UI/consola.
- Persistencia o infraestructura.

## 2) Requisitos funcionales (EARS)

- **RF-01** · **CUANDO** el firmante en turno rechace un documento con un motivo válido y un instante, **EL SISTEMA DEBERÁ** pasar el documento a `Devuelto` y registrar el rechazo con firmante, motivo, versión e instante.

- **RF-02** · **CUANDO** el autor reenvíe un documento en estado `Devuelto`, **EL SISTEMA DEBERÁ** incrementar la versión en 1, pasar el documento a `EnCircuito` y reiniciar el turno en el primer firmante.

- **RF-03** · **EL SISTEMA DEBERÁ** conservar las firmas de versiones anteriores en el historial y **NO DEBERÁ** contarlas para aprobar la versión vigente.

- **RF-04** · **SI** el documento es rechazado en la vuelta 3 (equivalente a la versión 3), **ENTONCES EL SISTEMA DEBERÁ** pasar el documento a `Rechazado`, estado terminal que no admite nuevos reenvíos.

- **RF-05** · **SI** el motivo de rechazo es nulo, está vacío o contiene únicamente espacios en blanco, **ENTONCES EL SISTEMA DEBERÁ** lanzar `CircuitoFirmaException` con el mensaje exacto: **"El motivo de rechazo es obligatorio."**

- **RF-06** · **SI** se intenta rechazar un documento cuyo estado no es `EnCircuito` (incluidos `Devuelto`, `Borrador`, `Aprobado` y `Rechazado`), **ENTONCES EL SISTEMA DEBERÁ** lanzar `CircuitoFirmaException` por estado inválido.

- **RF-07** · **SI** quien intenta rechazar no es exactamente el firmante en turno, **ENTONCES EL SISTEMA DEBERÁ** lanzar `CircuitoFirmaException` por turno inválido, aplicando el mismo criterio de validación que en `Aprobar`.

## 3) Casos límite y reglas de error

- Rechazo con motivo nulo, vacío o compuesto únicamente por espacios: error RF-05.
- Rechazo en estado `Devuelto`, `Borrador`, `Aprobado` o `Rechazado`: error de estado inválido (RF-06).
- Rechazo fuera de turno: error de turno inválido (RF-07).
- Intentar aprobar un documento en estado `Devuelto`: error de estado inválido.
- Reenvío por un usuario distinto del autor: `CircuitoFirmaException` con el mensaje exacto: **"Solo el autor puede reenviar el documento."**
- Reenvío en un estado distinto de `Devuelto`, incluido `Rechazado`: `CircuitoFirmaException` con el mensaje exacto: **"Solo se puede reenviar un documento devuelto."**
- Rechazo en la versión 3: el documento pasa a `Rechazado` y no admite más reenvíos (RF-04).
- Reenvío desde `Devuelto`: el turno vuelve siempre al primer firmante, independientemente de las firmas de versiones anteriores (RF-02).
- Las firmas anteriores se conservan como evidencia, pero no cuentan para aprobar la versión vigente (RF-03).

## 4) Criterios de finalización (verificables)

- Existe al menos un test por cada requisito funcional (RF-01 a RF-07) en `FirmaDocs.Tests`.
- Los tests nuevos siguen la convención del repositorio: `Metodo_Escenario_Resultado_RFxx`.
- Los cinco tests existentes de `CircuitoFirma` permanecen sin cambios y en verde.
- Se verifican los mensajes exactos obligatorios:
  - "El motivo de rechazo es obligatorio."
  - "Solo el autor puede reenviar el documento."
  - "Solo se puede reenviar un documento devuelto."
- Se verifica mediante tests que:
  - El historial de firmas no se borra al reenviar.
  - Las firmas de versiones anteriores no aprueban la versión vigente.
  - El rechazo queda registrado como evidencia con firmante, motivo, versión e instante.
  - El rechazo en la versión 3 deja el documento en estado terminal `Rechazado`.
  - No se puede aprobar un documento en estado `Devuelto`.
- Los miembros públicos nuevos o modificados del dominio tienen documentación XML en español.

## 5) Preguntas abiertas

- Ninguna por el momento.