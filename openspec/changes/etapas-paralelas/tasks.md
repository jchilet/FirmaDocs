# Tasks

## 1. Configuración de etapas y validaciones estructurales

- [ ] 1.1 (RF-10) Crear tests en rojo para etapa vacía e implementar validación mínima para rechazarla; verificar en verde con `dotnet test --filter "RF-10"`. (20-30 min)
- [ ] 1.2 (RF-11) Crear tests en rojo para límites (>5 etapas o >5 firmantes) e implementar validación de límite; verificar en verde con `dotnet test --filter "RF-11"`. (20-30 min)
- [ ] 1.3 (RF-12) Crear tests en rojo para firmante repetido entre etapas e implementar validación de duplicidad; verificar en verde con `dotnet test --filter "RF-12"`. (20-30 min)

## 2. Navegación por etapas y compatibilidad

- [ ] 2.1 (RF-08) Crear tests en rojo para configuración por etapas (`Legal` ? `Finanzas`/`Riesgos` ? `Gerencia`) e implementar habilitación por dependencias estrictas; verificar en verde con `dotnet test --filter "RF-08"`. (20-30 min)
- [ ] 2.2 (RF-08) Crear/ajustar tests para convivencia del modo secuencial existente con el modo por etapas e implementar la compatibilidad sin romper firmas públicas; verificar en verde con `dotnet test --filter "RF-08"`. (20-30 min)

## 3. Reglas de operación en etapa paralela

- [ ] 3.1 (RF-09) Crear tests en rojo para aprobación parcial (Sofía aprueba antes que Luis) e implementar cierre de etapa solo cuando ambos aprueban, incluyendo `FirmanteEnTurno`/`FirmantesPendientes`; verificar en verde con `dotnet test --filter "RF-09"`. (20-30 min)
- [ ] 3.2 (RF-14) Crear tests en rojo para doble aprobación del mismo firmante e implementar bloqueo de segunda aprobación en la misma vuelta; verificar en verde con `dotnet test --filter "RF-14"`. (20-30 min)
- [ ] 3.3 (RF-13) Crear tests en rojo para rechazo en etapa paralela tras aprobación previa e implementar transición de rechazo conservando evidencia; verificar en verde con `dotnet test --filter "RF-13"`. (20-30 min)

## 4. Regresión y validación final

- [ ] 4.1 (RF-08) Ejecutar regresión de la sesión 1 para confirmar que los 5 tests originales siguen pasando (`dotnet test --filter "Aprobar_|Constructor_"`) y luego validar suite completa de RF-08..RF-14 con `dotnet test`. (20-30 min)

## Workflow follow-up

- Archivar el cambio cuando implementación y revisión estén completas.
- Verificar el resultado archivado con validación OpenSpec del proyecto.
