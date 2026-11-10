# Tasks · Rechazo y nueva vuelta

- [ ] T1 (RF-05) Escribir test en rojo para motivo nulo/vacío/espacios con mensaje exacto y luego implementar validación mínima en `Rechazar` para que pase. · 25 min.
- [ ] T2 (RF-01) Escribir test en rojo para rechazo en turno que pasa a `Devuelto` y registra evidencia (firmante, motivo, versión, instante); luego crear/ajustar modelo de rechazo e implementación mínima. · 30 min.
- [ ] T3 (RF-06) Escribir test en rojo para rechazo con estado distinto de `EnCircuito` (incluye `Devuelto`) y luego implementar guardas de estado inválido. · 20 min.
- [ ] T4 (RF-07) Escribir test en rojo para rechazo fuera de turno y luego implementar validación con el mismo criterio de `Aprobar`. · 20 min.
- [ ] T5 (RF-02) Escribir test en rojo para reenvío válido por autor desde `Devuelto` (versión +1, `EnCircuito`, turno al primer firmante) y luego implementar `Reenviar`. · 25 min.
- [ ] T6 (RF-02) Escribir tests en rojo para reenvío inválido (no autor y estado distinto de `Devuelto`) con mensajes exactos y luego ajustar validaciones/mensajes. · 25 min.
- [ ] T7 (RF-04) Escribir test en rojo para rechazo en versión 3 que deja estado terminal `Rechazado` y bloquea nuevos reenvíos; luego implementar transición terminal. · 25 min.
- [ ] T8 (RF-03) Escribir test en rojo para conservar firmas históricas sin contarlas en la versión vigente y luego ajustar cálculo de turno/aprobación por versión actual. · 30 min.
