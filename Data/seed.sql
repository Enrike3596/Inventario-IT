-- ============================================================
-- SCRIPT DE INSERCIÓN - Inventario TI (HelpDesk.API)
-- Motor: PostgreSQL
-- Fecha: 2026-06-14
-- ============================================================
-- Nota: Las contraseñas están hasheadas con PBKDF2.
-- Hash de ejemplo para "Admin123":
--   salt = 16 bytes, hash = 32 bytes, 100000 iteraciones, SHA256
-- ============================================================

BEGIN;

-- ============================================================
-- 1. ROLES
-- ============================================================
INSERT INTO "Roles" ("IdRol", "Nombre", "Tipo", "Estado") VALUES
(1, 'Administrador',     'Admin',    'Activo'),
(2, 'Técnico',           'Tecnico',  'Activo'),
(3, 'Usuario Final',     'Usuario',  'Activo'),
(4, 'Auditor',           'Auditor',  'Activo');

SELECT setval(pg_get_serial_sequence('"Roles"', 'IdRol'), 4);

-- ============================================================
-- 2. SEDES
-- ============================================================
INSERT INTO "Sedes" ("IdSede", "Nombre", "Direccion", "Ciudad", "Estado") VALUES
(1, 'Sede Principal',     'Cra 50 #12-34',          'Bogotá',     'Activo'),
(2, 'Sede Norte',         'Av. 68 #45-67',          'Bogotá',     'Activo'),
(3, 'Sede Medellín',      'Calle 30 #20-10',        'Medellín',   'Activo'),
(4, 'Sede Cali',          'Av. 3N #5-20',           'Cali',       'Activo'),
(5, 'Sede Barranquilla',  'Cra 55 #80-90',          'Barranquilla', 'Activo');

SELECT setval(pg_get_serial_sequence('"Sedes"', 'IdSede'), 5);

-- ============================================================
-- 3. USUARIOS
--    Contraseña hasheada de "Admin123" (PBKDF2 SHA256)
--    ════════════════════════════════════════════════════════
--    CREDENCIALES DE ACCESO (todos usan: Admin123)
--    ════════════════════════════════════════════════════════
--    carlos.martinez@empresa.com   → Admin123  (Rol: Administrador)
--    maria.lopez@empresa.com       → Admin123  (Rol: Técnico)
--    juan.rodriguez@empresa.com    → Admin123  (Rol: Técnico)
--    ana.gomez@empresa.com         → Admin123  (Rol: Usuario Final)
--    pedro.hernandez@empresa.com   → Admin123  (Rol: Usuario Final)
--    laura.diaz@empresa.com        → Admin123  (Rol: Usuario Final)
--    diego.ramirez@empresa.com     → Admin123  (Rol: Usuario Final)
--    sofia.castillo@empresa.com    → Admin123  (Rol: Auditor)
--    andres.moreno@empresa.com     → Admin123  (Rol: Usuario Final)
--    carolina.torres@empresa.com   → Admin123  (Rol: Usuario Final)
-- ============================================================
DO $$
DECLARE
    pwd_hash TEXT := 'CE6gA+q8I0FMG3VXB3RX/A==.9vqPLhFr0x9SxpDjfgKS6v5Qf40GF+pAF9e7KScVP0Y=';
BEGIN
INSERT INTO "Usuarios" ("IdUsuario", "IdRol", "IdSede", "Nombre", "Correo", "Telefono", "Cargo", "Contraseña", "EstadoUsuario", "FechaCreacion") VALUES
(1,  1, 1, 'Carlos Andrés Martínez',    'carlos.martinez@empresa.com',    '3001234567', 'Administrador de TI',        pwd_hash, 'Activo',  '2025-01-15 08:00:00'),
(2,  2, 1, 'María Fernanda López',       'maria.lopez@empresa.com',        '3001234568', 'Técnico de Soporte N1',      pwd_hash, 'Activo',  '2025-01-20 08:00:00'),
(3,  2, 2, 'Juan David Rodríguez',       'juan.rodriguez@empresa.com',     '3001234569', 'Técnico de Soporte N2',      pwd_hash, 'Activo',  '2025-02-01 08:00:00'),
(4,  3, 1, 'Ana María Gómez',           'ana.gomez@empresa.com',          '3001234570', 'Coordinadora Administrativa', pwd_hash, 'Activo',  '2025-02-10 08:00:00'),
(5,  3, 1, 'Pedro José Hernández',      'pedro.hernandez@empresa.com',    '3001234571', 'Analista Financiero',         pwd_hash, 'Activo',  '2025-03-01 08:00:00'),
(6,  3, 2, 'Laura Patricia Díaz',       'laura.diaz@empresa.com',         '3001234572', 'Gerente de Ventas',           pwd_hash, 'Activo',  '2025-03-05 08:00:00'),
(7,  3, 3, 'Diego Alejandro Ramírez',   'diego.ramirez@empresa.com',      '3001234573', 'Desarrollador Senior',        pwd_hash, 'Activo',  '2025-03-10 08:00:00'),
(8,  4, 1, 'Sofía Elena Castillo',      'sofia.castillo@empresa.com',     '3001234574', 'Auditor Interno',             pwd_hash, 'Activo',  '2025-04-01 08:00:00'),
(9,  3, 4, 'Andrés Felipe Moreno',      'andres.moreno@empresa.com',      '3001234575', 'Jefe de Operaciones',         pwd_hash, 'Activo',  '2025-04-15 08:00:00'),
(10, 3, 5, 'Carolina Isabel Torres',    'carolina.torres@empresa.com',    '3001234576', 'Coordinadora de RH',          pwd_hash, 'Activo',  '2025-05-01 08:00:00');
END $$;

SELECT setval(pg_get_serial_sequence('"Usuarios"', 'IdUsuario'), 10);

-- ============================================================
-- 4. CATEGORÍAS DE ACTIVOS
-- ============================================================
INSERT INTO "CategoriasActivo" ("IdCategoria", "Nombre", "Estado") VALUES
(1, 'Laptop',        'Activo'),
(2, 'Desktop',       'Activo'),
(3, 'Monitor',       'Activo'),
(4, 'Impresora',     'Activo'),
(5, 'Switch / Router', 'Activo'),
(6, 'Periférico',    'Activo'),
(7, 'Servidor',      'Activo'),
(8, 'UPS',           'Activo'),
(9, 'Tablet',        'Activo'),
(10, 'Accesorio',    'Activo');

SELECT setval(pg_get_serial_sequence('"CategoriasActivo"', 'IdCategoria'), 10);

-- ============================================================
-- 5. ÓRDENES DE COMPRA
-- ============================================================
INSERT INTO "OrdenesCompra" ("IdOrden", "NumeroOC", "Proveedor", "Total", "Observaciones", "FechaCompra") VALUES
(1, 'OC-2025-001',  'Dell Technologies Colombia SAS',    85000000.00, 'Compra anual laptops Dell Latitude',           '2025-01-10 10:00:00'),
(2, 'OC-2025-002',  'HP Inc Sucursal Colombia',         32000000.00, 'Monitores HP para sede principal',             '2025-01-20 11:00:00'),
(3, 'OC-2025-003',  'Cisco Systems Colombia',           45000000.00, 'Switches y routers para red corporativa',      '2025-02-05 09:00:00'),
(4, 'OC-2025-004',  'Lenovo Colombia SAS',              28000000.00, 'Tablets Lenovo para área de ventas',           '2025-02-15 14:00:00'),
(5, 'OC-2025-005',  'Epson Colombia Ltda',              8500000.00,  'Impresoras multifuncionales',                  '2025-03-01 08:30:00'),
(6, 'OC-2025-006',  'APC by Schneider Electric',        12000000.00, 'UPS para sala de servidores',                  '2025-03-10 10:00:00'),
(7, 'OC-2025-007',  'Dell Technologies Colombia SAS',    1200000.00,  'Teclados y mouse inalámbricos Dell',           '2025-03-20 15:00:00'),
(8, 'OC-2025-008',  'Microsoft Colombia',               15000000.00, 'Licencias Office 365 y Windows',               '2025-04-01 09:00:00'),
(9, 'OC-2025-009',  'HP Inc Sucursal Colombia',         45000000.00, 'Servidores HP ProLiant para datacenter',        '2025-04-10 11:00:00'),
(10, 'OC-2025-010', 'Logitech Colombia SAS',             2400000.00,  'Cámaras web y headsets para home office',      '2025-05-05 10:00:00');

SELECT setval(pg_get_serial_sequence('"OrdenesCompra"', 'IdOrden'), 10);

-- ============================================================
-- 6. ACTIVOS
--    EstadoActivo: Disponible, Asignado, EnMantenimiento, DadoDeBaja
-- ============================================================
INSERT INTO "Activos" ("IdActivo", "IdCategoria", "IdOrden", "CodigoActivo", "Serial", "Marca", "Modelo", "Referencia", "EstadoActivo", "FechaAdquisicion", "FechaBaja", "Observaciones") VALUES
-- Laptops Dell (OC-001)
(1,  1, 1, 'ACT-001', 'DL-LAT-5420-001', 'Dell',    'Latitude 5420',       'i5-1135G7/16GB/512GB',   'Disponible',    '2025-01-15', NULL, NULL),
(2,  1, 1, 'ACT-002', 'DL-LAT-5420-002', 'Dell',    'Latitude 5420',       'i5-1135G7/16GB/512GB',   'Asignado',      '2025-01-15', NULL, 'Asignado a Ana Gómez'),
(3,  1, 1, 'ACT-003', 'DL-LAT-5420-003', 'Dell',    'Latitude 5420',       'i5-1135G7/16GB/512GB',   'Asignado',      '2025-01-15', NULL, 'Asignado a Pedro Hernández'),
(4,  1, 1, 'ACT-004', 'DL-LAT-5420-004', 'Dell',    'Latitude 5420',       'i5-1135G7/16GB/512GB',   'Asignado',      '2025-01-15', NULL, 'Asignado a Laura Díaz'),
(5,  1, 1, 'ACT-005', 'DL-LAT-5420-005', 'Dell',    'Latitude 5420',       'i5-1135G7/16GB/512GB',   'Disponible',    '2025-01-15', NULL, NULL),
-- Laptops Dell adicionales (OC-001)
(6,  1, 1, 'ACT-006', 'DL-LAT-5420-006', 'Dell',    'Latitude 5420',       'i5-1135G7/16GB/512GB',   'EnMantenimiento','2025-01-15', NULL, 'Pantalla dañada - en reparación'),
(7,  1, 1, 'ACT-007', 'DL-LAT-5420-007', 'Dell',    'Latitude 5420',       'i5-1135G7/16GB/512GB',   'Disponible',    '2025-01-15', NULL, NULL),
(8,  1, 1, 'ACT-008', 'DL-LAT-5420-008', 'Dell',    'Latitude 5420',       'i5-1135G7/16GB/512GB',   'Disponible',    '2025-01-15', NULL, NULL),
-- Monitores HP (OC-002)
(9,  3, 2, 'ACT-009', 'HP-MON-24-001',   'HP',      'Monitor E24 G5',      '24" FHD IPS',            'Disponible',    '2025-01-25', NULL, NULL),
(10, 3, 2, 'ACT-010', 'HP-MON-24-002',   'HP',      'Monitor E24 G5',      '24" FHD IPS',            'Disponible',    '2025-01-25', NULL, NULL),
(11, 3, 2, 'ACT-011', 'HP-MON-24-003',   'HP',      'Monitor E24 G5',      '24" FHD IPS',            'Asignado',      '2025-01-25', NULL, 'Entregado con laptop a Ana Gómez'),
(12, 3, 2, 'ACT-012', 'HP-MON-24-004',   'HP',      'Monitor E24 G5',      '24" FHD IPS',            'Disponible',    '2025-01-25', NULL, NULL),
-- Switch Cisco (OC-003)
(13, 5, 3, 'ACT-013', 'CS-CAT-2960-001', 'Cisco',   'Catalyst 2960-X',     '48 Puertos Gigabit',     'Disponible',    '2025-02-10', NULL, NULL),
(14, 5, 3, 'ACT-014', 'CS-CAT-2960-002', 'Cisco',   'Catalyst 2960-X',     '48 Puertos Gigabit',     'Disponible',    '2025-02-10', NULL, NULL),
(15, 5, 3, 'ACT-015', 'CS-ISR-1100-001', 'Cisco',   'ISR 1100',            'Router Gigabit',         'Disponible',    '2025-02-10', NULL, NULL),
-- Tablets Lenovo (OC-004)
(16, 9, 4, 'ACT-016', 'LEN-TAB-P11-001', 'Lenovo',  'Tab P11',             '128GB/WiFi',             'Asignado',      '2025-02-20', NULL, 'Asignado a Diego Ramírez'),
(17, 9, 4, 'ACT-017', 'LEN-TAB-P11-002', 'Lenovo',  'Tab P11',             '128GB/WiFi',             'Disponible',    '2025-02-20', NULL, NULL),
(18, 9, 4, 'ACT-018', 'LEN-TAB-P11-003', 'Lenovo',  'Tab P11',             '128GB/WiFi',             'Disponible',    '2025-02-20', NULL, NULL),
-- Impresoras Epson (OC-005)
(19, 4, 5, 'ACT-019', 'EPS-WF-7840-001', 'Epson',   'WorkForce WF-7840',   'Multifuncional A3',      'Disponible',    '2025-03-05', NULL, NULL),
(20, 4, 5, 'ACT-020', 'EPS-WF-7840-002', 'Epson',   'WorkForce WF-7840',   'Multifuncional A3',      'Disponible',    '2025-03-05', NULL, NULL),
-- UPS APC (OC-006)
(21, 8, 6, 'ACT-021', 'APC-SMT-1500-001','APC',     'Smart-UPS SMT1500',   '1500VA/900W',            'Disponible',    '2025-03-15', NULL, 'UPS para rack servidores'),
(22, 8, 6, 'ACT-022', 'APC-SMT-1500-002','APC',     'Smart-UPS SMT1500',   '1500VA/900W',            'Disponible',    '2025-03-15', NULL, 'UPS para rack servidores'),
-- Servidores HP (OC-009)
(23, 7, 9, 'ACT-023', 'HP-DL-380-001',   'HP',      'ProLiant DL380 Gen10','Xeon 16C/128GB/2TB',     'Disponible',    '2025-04-15', NULL, 'Servidor principal - Datacenter'),
(24, 7, 9, 'ACT-024', 'HP-DL-380-002',   'HP',      'ProLiant DL380 Gen10','Xeon 16C/128GB/2TB',     'Disponible',    '2025-04-15', NULL, 'Servidor respaldo - Datacenter'),
-- Desktop HP
(25, 2, 8, 'ACT-025', 'HP-DT-ELITE-001', 'HP',      'EliteDesk 800 G6',    'i7-10700/16GB/512GB',    'Disponible',    '2025-04-20', NULL, NULL),
(26, 2, 8, 'ACT-026', 'HP-DT-ELITE-002', 'HP',      'EliteDesk 800 G6',    'i7-10700/16GB/512GB',    'DadoDeBaja',    '2023-11-15', '2025-05-01', 'Dañado por descarga eléctrica'),
-- Periféricos (OC-007 y OC-010)
(27, 6, 7, 'ACT-027', 'DL-KB-MOUSE-001', 'Dell',    'Teclado + Mouse',     'KB216/MS116',            'Disponible',    '2025-03-25', NULL, NULL),
(28, 6, 7, 'ACT-028', 'DL-KB-MOUSE-002', 'Dell',    'Teclado + Mouse',     'KB216/MS116',            'Disponible',    '2025-03-25', NULL, NULL),
(29, 10,10,'ACT-029', 'LOG-C920-001',    'Logitech','C920 HD Pro',         'Cámara Web 1080p',       'Disponible',    '2025-05-10', NULL, NULL),
(30, 10,10,'ACT-030', 'LOG-H800-001',    'Logitech','H800 Wireless',       'Headset Inalámbrico',    'Disponible',    '2025-05-10', NULL, NULL);

SELECT setval(pg_get_serial_sequence('"Activos"', 'IdActivo'), 30);

-- ============================================================
-- 7. PARQUEADEROS
-- ============================================================
INSERT INTO "Parqueaderos" ("IdParqueadero", "IdSede", "Nombre", "Ubicacion", "Estado") VALUES
(1, 1, 'Parqueadero Principal',  'Sótano 1 - Puesto 1-20', 'Activo'),
(2, 1, 'Parqueadero Visitas',    'Planta Baja - Puesto 21-30', 'Activo'),
(3, 2, 'Parqueadero Norte',      'Sótano 1 - Puesto 1-15', 'Activo'),
(4, 3, 'Parqueadero Medellín',   'Planta Baja - Puesto 1-10', 'Activo');

SELECT setval(pg_get_serial_sequence('"Parqueaderos"', 'IdParqueadero'), 4);

-- ============================================================
-- 8. CANALES DE SOLICITUD
-- ============================================================
INSERT INTO "Canales" ("IdCanal", "Nombre", "FechaSolicitud") VALUES
(1, 'Correo Electrónico', '2025-01-01 00:00:00'),
(2, 'Sistema de Tickets', '2025-01-01 00:00:00'),
(3, 'Teléfono',           '2025-01-01 00:00:00'),
(4, 'Presencial',         '2025-01-01 00:00:00'),
(5, 'WhatsApp',           '2025-06-01 00:00:00');

SELECT setval(pg_get_serial_sequence('"Canales"', 'IdCanal'), 5);

-- ============================================================
-- 9. SALIDAS
--    CodigoUnico formato: SAL-YYYYMMDD-NNNNNN
-- ============================================================
INSERT INTO "Salidas" ("IdSalida", "IdCanal", "CodigoUnico", "NumeroTicket", "IdUsuarioDestino", "IdParqueaderoDestino", "IdUsuarioEntrega", "FechaSalida", "RegistroSalida", "Observaciones") VALUES
(1, 2, 'SAL-20250120-000001', 'TK-2025-001', 4,  NULL, 2, '2025-01-20 09:00:00', 'Entrega laptop + monitor a Ana Gómez',           'Asignación inicial sede principal'),
(2, 2, 'SAL-20250120-000002', 'TK-2025-002', 5,  NULL, 2, '2025-01-20 09:30:00', 'Entrega laptop a Pedro Hernández',               'Asignación inicial sede principal'),
(3, 2, 'SAL-20250201-000003', 'TK-2025-015', 6,  NULL, 3, '2025-02-01 10:00:00', 'Entrega laptop a Laura Díaz',                    'Asignación sede norte'),
(4, 2, 'SAL-20250220-000004', 'TK-2025-030', 7,  NULL, 2, '2025-02-20 11:00:00', 'Entrega tablet a Diego Ramírez',                 'Asignación sede Medellín'),
(5, 1, 'SAL-20250310-000005', NULL,          NULL, 1,   2, '2025-03-10 14:00:00', 'Envío switch al parqueadero principal',          'Instalación red en parqueadero'),
(6, 2, 'SAL-20250401-000006', 'TK-2025-050', NULL, 2,   2, '2025-04-01 08:00:00', 'Monitor adicional al parqueadero visitas',       'Para sala de juntas');

SELECT setval(pg_get_serial_sequence('"Salidas"', 'IdSalida'), 6);

-- ============================================================
-- 10. DETALLES DE SALIDA
-- ============================================================
INSERT INTO "DetallesSalida" ("IdDetalleSalida", "IdSalida", "IdActivo", "Cantidad") VALUES
-- Salida 1: Laptop + Monitor para Ana Gómez
(1, 1, 2, 1),   -- Laptop Dell Latitude
(2, 1, 11, 1),  -- Monitor HP
-- Salida 2: Laptop para Pedro Hernández
(3, 2, 3, 1),   -- Laptop Dell Latitude
-- Salida 3: Laptop para Laura Díaz
(4, 3, 4, 1),   -- Laptop Dell Latitude
-- Salida 4: Tablet para Diego Ramírez
(5, 4, 16, 1),  -- Tablet Lenovo
-- Salida 5: Switch al parqueadero
(6, 5, 13, 1),  -- Switch Cisco Catalyst
-- Salida 6: Monitor al parqueadero visitas
(7, 6, 10, 1);  -- Monitor HP

SELECT setval(pg_get_serial_sequence('"DetallesSalida"', 'IdDetalleSalida'), 7);

-- ============================================================
-- 11. ASIGNACIONES DE USUARIO
--    EstadoAsignacion: Activa, Finalizada
-- ============================================================
INSERT INTO "AsignacionesUsuario" ("IdAsignacion", "IdActivo", "IdUsuarioDestino", "IdParqueadero", "FechaAsignacion", "EstadoAsignacion") VALUES
(1, 2,  4, NULL, '2025-01-20 09:00:00', 'Activa'),     -- Ana Gómez - Laptop
(2, 3,  5, NULL, '2025-01-20 09:30:00', 'Activa'),     -- Pedro Hernández - Laptop
(3, 4,  6, NULL, '2025-02-01 10:00:00', 'Activa'),     -- Laura Díaz - Laptop
(4, 16, 7, NULL, '2025-02-20 11:00:00', 'Activa'),     -- Diego Ramírez - Tablet
(5, 11, 4, NULL, '2025-01-20 09:00:00', 'Activa'),     -- Ana Gómez - Monitor
(6, 26, 5, NULL, '2023-11-15 08:00:00', 'Finalizada'); -- Desktop dañado (finalizada)

SELECT setval(pg_get_serial_sequence('"AsignacionesUsuario"', 'IdAsignacion'), 6);

-- ============================================================
-- 12. HISTORIAL DE ACTIVOS
--    TipoMovimiento: Entrada, Salida, Asignacion, Devolucion
-- ============================================================
INSERT INTO "HistorialActivos" ("IdHistorial", "IdActivo", "IdSalida", "TipoMovimiento", "FechaMovimiento", "IdUsuarioEntrega") VALUES
-- Salida 1: Laptop + Monitor para Ana Gómez
(1, 2,  1, 'Salida',     '2025-01-20 09:00:00', 2),
(2, 11, 1, 'Salida',     '2025-01-20 09:00:00', 2),
-- Salida 2: Laptop para Pedro Hernández
(3, 3,  2, 'Salida',     '2025-01-20 09:30:00', 2),
-- Salida 3: Laptop para Laura Díaz
(4, 4,  3, 'Salida',     '2025-02-01 10:00:00', 3),
-- Salida 4: Tablet para Diego Ramírez
(5, 16, 4, 'Salida',     '2025-02-20 11:00:00', 2),
-- Salida 5: Switch al parqueadero principal
(6, 13, 5, 'Salida',     '2025-03-10 14:00:00', 2),
-- Salida 6: Monitor al parqueadero visitas
(7, 10, 6, 'Salida',     '2025-04-01 08:00:00', 2),
-- Devolución: Desktop dañado devuelto por Pedro Hernández (salió en Salida 2)
(8, 26, 2, 'Devolucion', '2025-05-01 15:00:00', 2);

SELECT setval(pg_get_serial_sequence('"HistorialActivos"', 'IdHistorial'), 8);

-- ============================================================
-- VERIFICACIÓN DE DATOS INSERTADOS
-- ============================================================
SELECT 'Roles' AS "Tabla", COUNT(*) AS "Registros" FROM "Roles"
UNION ALL
SELECT 'Sedes', COUNT(*) FROM "Sedes"
UNION ALL
SELECT 'Usuarios', COUNT(*) FROM "Usuarios"
UNION ALL
SELECT 'CategoriasActivo', COUNT(*) FROM "CategoriasActivo"
UNION ALL
SELECT 'OrdenesCompra', COUNT(*) FROM "OrdenesCompra"
UNION ALL
SELECT 'Activos', COUNT(*) FROM "Activos"
UNION ALL
SELECT 'Parqueaderos', COUNT(*) FROM "Parqueaderos"
UNION ALL
SELECT 'Canales', COUNT(*) FROM "Canales"
UNION ALL
SELECT 'Salidas', COUNT(*) FROM "Salidas"
UNION ALL
SELECT 'DetallesSalida', COUNT(*) FROM "DetallesSalida"
UNION ALL
SELECT 'AsignacionesUsuario', COUNT(*) FROM "AsignacionesUsuario"
UNION ALL
SELECT 'HistorialActivos', COUNT(*) FROM "HistorialActivos"
ORDER BY "Tabla";

COMMIT;
