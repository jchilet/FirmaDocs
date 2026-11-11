# Design

## Context

Estado actual: `CircuitoFirma` opera en secuencia lineal de firmantes y resuelve un único `FirmanteEnTurno`.  
La propuesta (`proposal.md`) introduce etapas con dependencias estrictas: `Legal` (secuencial) → `Finanzas` y `Riesgos` (paralela) → `Gerencia` (secuencial), sin romper compatibilidad con circuitos secuenciales existentes.

Restricciones relevantes:
- Mantener firmas públicas existentes.
- Conservar reglas de rechazo/vueltas ya vigentes.
- Preservar evidencia histórica.
- Limitar complejidad de configuración (máximo de etapas/firmantes según spec).
- Los 5 tests de la sesión 1 deben seguir pasando sin cambios.

## Goals / Non-Goals

**Goals:**
- Soportar configuración por etapas secuenciales y paralelas con bloqueos explícitos.
- Mantener comportamiento actual cuando el circuito se define como lista plana.
- Incorporar validaciones de dominio para etapas vacías, duplicidad de firmante y límites de tamaño.
- Mantener trazabilidad de aprobaciones/rechazos en etapas paralelas (incluyendo aprobación parcial y rechazo posterior).

**Non-Goals:**
- Persistencia o migraciones de base de datos.
- Cambios de interfaz de usuario o API externa.
- Refactor no funcional fuera del alcance del cambio.

## Decisions

1. **Modelo de configuración dual (compatibilidad):**
   - Se mantiene la entrada secuencial actual como caso válido.
   - Se agrega un modelo explícito de etapas para el nuevo flujo paralelo.
   - Rationale: evita ruptura de contratos y habilita adopción gradual.

   **Convivencia de construcción (sin romper firmas públicas existentes):**
   - Se conserva el constructor público actual `CircuitoFirma(string documentoId, string autorId, IReadOnlyList<Firmante> firmantes)` sin cambios.
   - Se incorpora un nuevo constructor público adicional `CircuitoFirma(string documentoId, string autorId, IReadOnlyList<Etapa> etapas)` para el modo por etapas.
   - Ambos caminos inicializan un único modelo interno normalizado de ejecución del circuito.

   **Alternativa descartada:** reemplazar por completo el modelo secuencial por etapas obligatorias.  
   **Por qué se descarta:** rompe compatibilidad y fuerza migración inmediata de consumidores.

2. **Estado de ejecución por “etapa activa” + conjunto de pendientes en paralelo:**
   - La resolución de turno pasa de “un índice lineal” a “etapa activa + firmantes pendientes de la etapa”.
   - En etapas paralelas, la etapa cierra solo cuando aprueban todos los firmantes requeridos.
   - `FirmanteEnTurno` mantiene compatibilidad: en etapa secuencial devuelve el único firmante esperado; en etapa paralela devuelve `null` por no existir un único turno.
   - Se agrega `FirmantesPendientes` para exponer los firmantes pendientes de la etapa activa (1 elemento en secuencial, N en paralelo).
   - Rationale: representa directamente los requisitos RF-08 y RF-09.

   **Alternativa descartada:** expandir etapas paralelas a una secuencia artificial (topología lineal).  
   **Por qué se descarta:** pierde semántica de paralelismo y complica reglas de cierre parcial.

3. **Validación de configuración al crear/validar circuito:**
   - Rechazar etapa vacía.
   - Rechazar firmantes repetidos entre etapas.
   - Rechazar configuraciones que excedan límites de etapas/firmantes.
   - Rationale: fallar temprano y mantener invariantes de dominio.

   **Alternativa descartada:** validar en diferido durante ejecución.  
   **Por qué se descarta:** genera fallos tardíos y estados intermedios difíciles de explicar.

4. **Reglas de rechazo sin bifurcar historial:**
   - Un rechazo en etapa paralela tras una aprobación previa aplica la regla de rechazo vigente y conserva evidencia completa.
   - Rationale: continuidad con reglas de negocio actuales y trazabilidad auditables.

   **Alternativa descartada:** revertir aprobaciones previas de la misma etapa al rechazar.  
   **Por qué se descarta:** contradice principio de evidencia histórica y complica auditoría.

5. **Prevención de doble aprobación por firmante y vuelta en etapa paralela:**
   - Se impide segunda aprobación del mismo firmante en la misma vuelta.
   - Rationale: asegura unicidad de decisión por firmante y evita cierres incorrectos.

   **Alternativa descartada:** aceptar múltiples aprobaciones y usar la última.  
   **Por qué se descarta:** introduce ambigüedad de evidencia y riesgo de fraude/errores operativos.

### Tipos nuevos y módulos afectados

**Tipos nuevos (dominio):**
- `ModoEtapa` (enum, `FirmaDocs/Circuito/ModoEtapa.cs`): `Secuencial` o `Paralela`.
- `Etapa` (record, `FirmaDocs/Circuito/Etapa.cs`): nombre de etapa, firmantes de la etapa y su `ModoEtapa`.
- `EstadoEtapa` (enum o value object): estado operativo de etapa (`Bloqueada`, `Activa`, `Completada`).

**Módulos afectados:**
- `FirmaDocs/Circuito/CircuitoFirma.cs` (orquestación de turnos y transiciones por etapa).
- `FirmaDocs/Circuito/EstadoDocumento.cs` (sin cambios de estados globales previstos por este diseño; se reutilizan estados del documento).
- `FirmaDocs/Circuito/Firma.cs` y `FirmaDocs/Circuito/Rechazo.cs` (uso de evidencia existente, con posible enriquecimiento contextual de etapa en caso necesario).
- `FirmaDocs.Tests/Circuito/` (nuevos tests de escenarios paralelos y validaciones de configuración).

## Risks / Trade-offs

- **[Riesgo] Complejidad mayor en cálculo de turno** → **Mitigación:** encapsular cálculo en funciones de dominio específicas para etapa activa y pendientes.
- **[Riesgo] Regresión en comportamiento secuencial existente** → **Mitigación:** mantener camino secuencial explícito y pruebas de no regresión.
- **[Riesgo] Confusión entre “etapa paralela abierta” y “documento en circuito”** → **Mitigación:** tests de transición por etapa y criterios de cierre claros.
- **[Riesgo] Mensajes de error inconsistentes** → **Mitigación:** catálogo de mensajes de dominio y asserts exactos en pruebas.

## Migration Plan

1. Introducir tipos de configuración por etapas e invariantes sin reemplazar el flujo secuencial.
2. Integrar resolución de etapa activa en `CircuitoFirma` manteniendo compatibilidad con lista plana.
3. Agregar reglas de validación estructural y de ejecución paralela.
4. Cubrir escenarios RF-08+ con pruebas y validar no regresión de secuencial.
5. Habilitar uso gradual: circuitos existentes continúan operando sin cambios de contrato.

## Open Questions

- Ninguna por el momento.
