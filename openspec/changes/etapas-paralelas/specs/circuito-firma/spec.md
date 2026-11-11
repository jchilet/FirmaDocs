# Spec Delta

## Purpose

Definir el comportamiento observable de un circuito de firma por etapas con combinación secuencial/paralela, preservando la evidencia de decisiones, las reglas de rechazo y la compatibilidad con la funcionalidad existente.

## ADDED Requirements

### Requirement: RF-08 Orden de etapas y firmantes por etapa
CUANDO se configure el circuito por etapas, EL SISTEMA DEBERÁ ejecutar el orden `Legal` (Ana) → `Finanzas` y `Riesgos` en paralelo (Luis y Sofía) → `Gerencia` (Raúl).

#### Scenario: Avance por etapas con dependencia estricta
- **GIVEN** un documento en circuito con etapas `Legal`, `Finanzas`/`Riesgos` y `Gerencia`
- **WHEN** Ana completa `Legal`
- **THEN** se habilitan en paralelo Luis y Sofía, y Raúl permanece bloqueado hasta cerrar ambas ramas

### Requirement: RF-09 Aprobación parcial en etapa paralela
SI Sofía aprueba antes que Luis en la etapa paralela, ENTONCES EL SISTEMA DEBERÁ mantener la etapa abierta hasta que Luis también apruebe.

#### Scenario: Sofía aprueba primero y la etapa no cierra
- **GIVEN** `Legal` completada y etapa paralela activa con Luis y Sofía pendientes
- **WHEN** Sofía registra su aprobación antes que Luis
- **THEN** la etapa continúa abierta y no habilita `Gerencia` hasta la aprobación de Luis

### Requirement: RF-10 Validación de etapa vacía
SI una etapa se define sin firmantes, ENTONCES EL SISTEMA DEBERÁ rechazar la configuración del circuito por ser inválida.

#### Scenario: Etapa sin firmantes
- **GIVEN** una definición de circuito que incluye una etapa vacía
- **WHEN** se intenta crear o validar el circuito
- **THEN** el sistema informa error de dominio y no habilita el circuito

### Requirement: RF-11 Límite de complejidad del circuito
SI la configuración contiene más de 5 etapas o más de 5 firmantes totales, ENTONCES EL SISTEMA DEBERÁ rechazar la configuración por exceder el límite permitido.

#### Scenario: Configuración excede límites
- **GIVEN** una propuesta de circuito con 6 etapas
- **WHEN** se intenta crear o validar el circuito
- **THEN** el sistema rechaza la configuración por exceder el máximo permitido

### Requirement: RF-12 Firmante repetido entre etapas
SI un firmante aparece en más de una etapa del mismo circuito, ENTONCES EL SISTEMA DEBERÁ rechazar la configuración por firmante repetido.

#### Scenario: Luis repetido en dos etapas
- **GIVEN** una configuración donde Luis aparece en `Finanzas` y nuevamente en `Gerencia`
- **WHEN** se intenta crear o validar el circuito
- **THEN** el sistema rechaza la configuración por duplicidad de firmante entre etapas

### Requirement: RF-13 Rechazo en etapa paralela con aprobación previa
CUANDO un firmante rechace en etapa paralela después de que otro firmante de la misma etapa ya aprobó, EL SISTEMA DEBERÁ aplicar la regla de rechazo del documento y conservar la evidencia de ambas decisiones.

#### Scenario: Rechazo de Luis luego de aprobación de Sofía
- **GIVEN** etapa paralela activa con aprobación registrada de Sofía
- **WHEN** Luis registra un rechazo de la misma vuelta
- **THEN** el documento entra al flujo de rechazo vigente y mantiene evidencia de la aprobación previa y del rechazo

### Requirement: RF-14 Prohibición de doble aprobación en etapa paralela
SI el mismo firmante intenta aprobar dos veces dentro de la misma etapa paralela y en la misma vuelta, ENTONCES EL SISTEMA DEBERÁ rechazar la segunda aprobación.

#### Scenario: Segunda aprobación de Sofía en la misma vuelta
- **GIVEN** Sofía ya aprobó en la etapa paralela de la vuelta vigente
- **WHEN** Sofía intenta aprobar nuevamente antes de cambiar de vuelta
- **THEN** el sistema rechaza la segunda aprobación y conserva una única aprobación válida de Sofía en esa vuelta
