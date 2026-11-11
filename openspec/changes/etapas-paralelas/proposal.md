# Proposal: etapas-paralelas

## Contexto
Se requiere planificar el trabajo del circuito completo de firma con ejecución por etapas paralelas, preservando las funcionalidades existentes y manteniendo trazabilidad con los RF vigentes. El plan debe contemplar desde el inicio tanto el flujo normal de rechazo y nueva vuelta como los estados terminales.

## Objetivo
Definir una secuencia de trabajo con dependencias estrictas entre tareas, organizada en etapas:
1. `Legal` (secuencial)
2. `Finanzas` y `Riesgos` (paralela)
3. `Gerencia` (secuencial)

## Alcance
- Incluye el circuito completo de firma en esta iniciativa.
- Incluye reglas para flujo normal (`EnCircuito` ? `Devuelto` ? nueva vuelta) y flujo terminal (`Rechazado`) desde la primera etapa.
- Incluye definición explícita de bloqueos entre etapas y criterios de entrada/salida de cada etapa.

## Dependencias estrictas y orden de etapas

### Etapa 1: Legal (secuencial)
**Propósito**
- Establecer base de reglas de dominio para rechazo y vuelta nueva sobre la que dependen las demás áreas.

**Bloquea a**
- Bloquea el inicio de `Finanzas` y `Riesgos`.

**Criterio de salida**
- Reglas base de rechazo/nueva vuelta y estados terminales definidas y verificadas en tests.

---

### Etapa 2: Finanzas y Riesgos (paralela)
**Propósito**
- Ejecutar en paralelo validaciones y escenarios de negocio sobre la base cerrada en `Legal`.

**Dependencia de entrada**
- Requiere Etapa 1 completada.

**Relación interna**
- `Finanzas` y `Riesgos` no se bloquean entre sí.
- Ambas deben respetar contratos y reglas establecidos por `Legal`.

**Bloquea a**
- `Gerencia` no puede iniciar hasta que finalicen **ambas** ramas: `Finanzas` y `Riesgos`.

**Criterio de salida**
- Consolidación de escenarios en paralelo sin romper compatibilidad con funcionalidades existentes.

---

### Etapa 3: Gerencia (secuencial)
**Propósito**
- Integrar y cerrar el flujo completo tras la convergencia de `Finanzas` y `Riesgos`.

**Dependencia de entrada**
- Requiere cierre completo de Etapa 2 en sus dos ramas.

**Criterio de salida**
- Flujo de circuito completo validado extremo a extremo, incluyendo casos normales y terminales.

## Criterios de éxito de la propuesta
- Orden de ejecución respetado: `Legal` ? (`Finanzas` || `Riesgos`) ? `Gerencia`.
- Dependencias estrictas documentadas y observables (qué bloquea a qué).
- Cobertura explícita de flujo normal y terminal desde la primera etapa.
- Sin degradar funcionalidades existentes del circuito.

## Fuera de alcance
- Persistencia.
- Interfaz de usuario.
- Refactor no funcional.
- Cambios en firmas públicas.

## Preguntas abiertas
- Ninguna por el momento.
