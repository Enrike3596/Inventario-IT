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
INSERT INTO "Roles" ("IdRol", "Nombre", "Tipo", "Estado", "FechaCreacion", "FechaModificacion", "CreadoPor", "ModificadoPor") VALUES
(1, 'Administrador',     'super_admin',    'Activo',    '2025-01-01 00:00:00', NULL, NULL, NULL),
(2, 'Técnico',           'agente_soporte',  'Activo',   '2025-01-01 00:00:00', NULL, NULL, NULL),
(3, 'Usuario Final',     'coordinador',     'Activo',   '2025-01-01 00:00:00', NULL, NULL, NULL),
(4, 'Auditor',           'super_admin',     'Activo',   '2025-01-01 00:00:00', NULL, NULL, NULL);

SELECT setval(pg_get_serial_sequence('"Roles"', 'IdRol'), 4);

-- ============================================================
-- 2. SEDES
-- ============================================================
INSERT INTO "Sedes" ("IdSede", "Nombre", "Direccion", "Ciudad", "Estado", "FechaCreacion", "FechaModificacion", "CreadoPor", "ModificadoPor") VALUES
(1, 'Sede Principal',     'Cra 50 #12-34',          'Bogotá',      'Activo', '2025-01-01 00:00:00', NULL, NULL, NULL),
(2, 'Sede Norte',         'Av. 68 #45-67',          'Bogotá',      'Activo', '2025-01-01 00:00:00', NULL, NULL, NULL),
(3, 'Sede Medellín',      'Calle 30 #20-10',        'Medellín',    'Activo', '2025-01-01 00:00:00', NULL, NULL, NULL),
(4, 'Sede Cali',          'Av. 3N #5-20',           'Cali',        'Activo', '2025-01-01 00:00:00', NULL, NULL, NULL),
(5, 'Sede Barranquilla',  'Cra 55 #80-90',          'Barranquilla','Activo', '2025-01-01 00:00:00', NULL, NULL, NULL);

SELECT setval(pg_get_serial_sequence('"Sedes"', 'IdSede'), 5);

-- ============================================================
-- 3. USUARIOS
--    Contraseña hasheada de "Admin123" (PBKDF2 SHA256)
-- ============================================================
DO $$
DECLARE
    pwd_hash TEXT := 'hQqszOD3lK2CO0RfY+inSg==.g/m0E2xkIJxdHq5X0axUWnGb+soD9SlIQmpo4ZahSGE=';
BEGIN
INSERT INTO "Usuarios" ("IdUsuario", "IdRol", "IdSede", "Nombre", "Correo", "Telefono", "Cargo", "Contraseña", "EstadoUsuario", "FechaCreacion", "FechaModificacion", "CreadoPor", "ModificadoPor") VALUES
(1,  1, 1, 'Carlos Andrés Martínez',    'carlos.martinez@empresa.com',    '3001234567', 'Administrador de TI',        pwd_hash, 'Activo',  '2025-01-15 08:00:00', NULL, NULL, NULL),
(2,  2, 1, 'María Fernanda López',       'maria.lopez@empresa.com',        '3001234568', 'Técnico de Soporte N1',      pwd_hash, 'Activo',  '2025-01-20 08:00:00', NULL, NULL, NULL),
(3,  2, 2, 'Juan David Rodríguez',       'juan.rodriguez@empresa.com',     '3001234569', 'Técnico de Soporte N2',      pwd_hash, 'Activo',  '2025-02-01 08:00:00', NULL, NULL, NULL),
(4,  3, 1, 'Ana María Gómez',           'ana.gomez@empresa.com',          '3001234570', 'Coordinadora Administrativa', pwd_hash, 'Activo',  '2025-02-10 08:00:00', NULL, NULL, NULL),
(5,  3, 1, 'Pedro José Hernández',      'pedro.hernandez@empresa.com',    '3001234571', 'Analista Financiero',         pwd_hash, 'Activo',  '2025-03-01 08:00:00', NULL, NULL, NULL),
(6,  3, 2, 'Laura Patricia Díaz',       'laura.diaz@empresa.com',         '3001234572', 'Gerente de Ventas',           pwd_hash, 'Activo',  '2025-03-05 08:00:00', NULL, NULL, NULL),
(7,  3, 3, 'Diego Alejandro Ramírez',   'diego.ramirez@empresa.com',      '3001234573', 'Desarrollador Senior',        pwd_hash, 'Activo',  '2025-03-10 08:00:00', NULL, NULL, NULL),
(8,  4, 1, 'Sofía Elena Castillo',      'sofia.castillo@empresa.com',     '3001234574', 'Auditor Interno',             pwd_hash, 'Activo',  '2025-04-01 08:00:00', NULL, NULL, NULL),
(9,  3, 4, 'Andrés Felipe Moreno',      'andres.moreno@empresa.com',      '3001234575', 'Jefe de Operaciones',         pwd_hash, 'Activo',  '2025-04-15 08:00:00', NULL, NULL, NULL),
(10, 3, 5, 'Carolina Isabel Torres',    'carolina.torres@empresa.com',    '3001234576', 'Coordinadora de RH',          pwd_hash, 'Activo',  '2025-05-01 08:00:00', NULL, NULL, NULL);
END $$;

SELECT setval(pg_get_serial_sequence('"Usuarios"', 'IdUsuario'), 10);

-- ============================================================
-- 4. CATEGORÍAS DE ACTIVOS
-- ============================================================
INSERT INTO "CategoriasActivo" ("IdCategoria", "Nombre", "Estado", "FechaCreacion", "FechaModificacion", "CreadoPor", "ModificadoPor") VALUES
(1, 'Laptop',        'Activo', '2025-01-01 00:00:00', NULL, NULL, NULL),
(2, 'Desktop',       'Activo', '2025-01-01 00:00:00', NULL, NULL, NULL),
(3, 'Monitor',       'Activo', '2025-01-01 00:00:00', NULL, NULL, NULL),
(4, 'Impresora',     'Activo', '2025-01-01 00:00:00', NULL, NULL, NULL),
(5, 'Switch / Router', 'Activo', '2025-01-01 00:00:00', NULL, NULL, NULL),
(6, 'Periférico',    'Activo', '2025-01-01 00:00:00', NULL, NULL, NULL),
(7, 'Servidor',      'Activo', '2025-01-01 00:00:00', NULL, NULL, NULL),
(8, 'UPS',           'Activo', '2025-01-01 00:00:00', NULL, NULL, NULL),
(9, 'Tablet',        'Activo', '2025-01-01 00:00:00', NULL, NULL, NULL),
(10, 'Accesorio',    'Activo', '2025-01-01 00:00:00', NULL, NULL, NULL);

SELECT setval(pg_get_serial_sequence('"CategoriasActivo"', 'IdCategoria'), 10);

-- ============================================================
-- 5. ÓRDENES DE COMPRA
-- ============================================================
INSERT INTO "OrdenesCompra" ("IdOrden", "NumeroOC", "Proveedor", "Total", "Observaciones", "FechaCompra", "FechaCreacion", "FechaModificacion", "CreadoPor", "ModificadoPor") VALUES
(1, 'OC-2025-001',  'Dell Technologies Colombia SAS',    85000000.00, 'Compra anual laptops Dell Latitude',           '2025-01-10 10:00:00', '2025-01-10 10:00:00', NULL, NULL, NULL),
(2, 'OC-2025-002',  'HP Inc Sucursal Colombia',         32000000.00, 'Monitores HP para sede principal',             '2025-01-20 11:00:00', '2025-01-20 11:00:00', NULL, NULL, NULL),
(3, 'OC-2025-003',  'Cisco Systems Colombia',           45000000.00, 'Switches y routers para red corporativa',      '2025-02-05 09:00:00', '2025-02-05 09:00:00', NULL, NULL, NULL),
(4, 'OC-2025-004',  'Lenovo Colombia SAS',              28000000.00, 'Tablets Lenovo para área de ventas',           '2025-02-15 14:00:00', '2025-02-15 14:00:00', NULL, NULL, NULL),
(5, 'OC-2025-005',  'Epson Colombia Ltda',              8500000.00,  'Impresoras multifuncionales',                  '2025-03-01 08:30:00', '2025-03-01 08:30:00', NULL, NULL, NULL),
(6, 'OC-2025-006',  'APC by Schneider Electric',        12000000.00, 'UPS para sala de servidores',                  '2025-03-10 10:00:00', '2025-03-10 10:00:00', NULL, NULL, NULL),
(7, 'OC-2025-007',  'Dell Technologies Colombia SAS',    1200000.00,  'Teclados y mouse inalámbricos Dell',           '2025-03-20 15:00:00', '2025-03-20 15:00:00', NULL, NULL, NULL),
(8, 'OC-2025-008',  'Microsoft Colombia',               15000000.00, 'Licencias Office 365 y Windows',               '2025-04-01 09:00:00', '2025-04-01 09:00:00', NULL, NULL, NULL),
(9, 'OC-2025-009',  'HP Inc Sucursal Colombia',         45000000.00, 'Servidores HP ProLiant para datacenter',        '2025-04-10 11:00:00', '2025-04-10 11:00:00', NULL, NULL, NULL),
(10, 'OC-2025-010', 'Logitech Colombia SAS',             2400000.00,  'Cámaras web y headsets para home office',      '2025-05-05 10:00:00', '2025-05-05 10:00:00', NULL, NULL, NULL);

SELECT setval(pg_get_serial_sequence('"OrdenesCompra"', 'IdOrden'), 10);

-- ============================================================
-- 6. ITEMS DE ORDEN DE COMPRA
-- ============================================================
INSERT INTO "ItemsOC" ("IdItemOC", "IdOrden", "IdCategoria", "NombreProducto", "Marca", "Modelo", "Referencia", "Observaciones", "CantidadEsperada", "FechaCreacion", "FechaModificacion", "CreadoPor", "ModificadoPor") VALUES
(1,  1, 1, 'Laptop Dell Latitude 5420',          'Dell',     'Latitude 5420',       'i5-1135G7/16GB/512GB',   NULL, 8,  '2025-01-10 10:00:00', NULL, NULL, NULL),
(2,  2, 3, 'Monitor HP E24 G5',                   'HP',       'Monitor E24 G5',      '24" FHD IPS',            NULL, 4,  '2025-01-20 11:00:00', NULL, NULL, NULL),
(3,  3, 5, 'Switch Cisco Catalyst 2960-X',        'Cisco',    'Catalyst 2960-X',     '48 Puertos Gigabit',     NULL, 2,  '2025-02-05 09:00:00', NULL, NULL, NULL),
(4,  3, 5, 'Router Cisco ISR 1100',               'Cisco',    'ISR 1100',            'Router Gigabit',         NULL, 1,  '2025-02-05 09:00:00', NULL, NULL, NULL),
(5,  4, 9, 'Tablet Lenovo Tab P11',               'Lenovo',   'Tab P11',             '128GB/WiFi',             NULL, 3,  '2025-02-15 14:00:00', NULL, NULL, NULL),
(6,  5, 4, 'Impresora Epson WorkForce WF-7840',   'Epson',    'WorkForce WF-7840',   'Multifuncional A3',      NULL, 2,  '2025-03-01 08:30:00', NULL, NULL, NULL),
(7,  6, 8, 'UPS APC Smart-UPS SMT1500',           'APC',      'Smart-UPS SMT1500',   '1500VA/900W',            NULL, 2,  '2025-03-10 10:00:00', NULL, NULL, NULL),
(8,  7, 6, 'Kit Teclado y Mouse Dell',            'Dell',     'Teclado + Mouse',     'KB216/MS116',            NULL, 2,  '2025-03-20 15:00:00', NULL, NULL, NULL),
(9,  8, 2, 'Desktop HP EliteDesk 800 G6',         'HP',       'EliteDesk 800 G6',    'i7-10700/16GB/512GB',    NULL, 2,  '2025-04-01 09:00:00', NULL, NULL, NULL),
(10, 9, 7, 'Servidor HP ProLiant DL380 Gen10',    'HP',       'ProLiant DL380 Gen10','Xeon 16C/128GB/2TB',     NULL, 2,  '2025-04-10 11:00:00', NULL, NULL, NULL),
(11, 10,10,'Cámara Web Logitech C920 HD Pro',     'Logitech', 'C920 HD Pro',         'Cámara Web 1080p',       NULL, 1,  '2025-05-05 10:00:00', NULL, NULL, NULL),
(12, 10,10,'Headset Logitech H800 Wireless',      'Logitech', 'H800 Wireless',       'Headset Inalámbrico',    NULL, 1,  '2025-05-05 10:00:00', NULL, NULL, NULL);

SELECT setval(pg_get_serial_sequence('"ItemsOC"', 'IdItemOC'), 12);

-- ============================================================
-- 7. ACTIVOS (IdDetalleItemOC se actualiza en paso 8)
--    EstadoActivo: Disponible, Asignado, EnMantenimiento, DadoDeBaja
-- ============================================================
INSERT INTO "Activos" ("IdActivo", "IdCategoria", "IdOrden", "IdItemOC", "IdDetalleItemOC", "CodigoActivo", "Serial", "Marca", "Modelo", "Referencia", "EstadoActivo", "FechaAdquisicion", "FechaBaja", "Observaciones", "FechaCreacion", "FechaModificacion", "CreadoPor", "ModificadoPor") VALUES
-- Laptops Dell (OC-001 / ItemOC 1)
(1,  1, 1, 1, NULL, 'ACT-001', 'DL-LAT-5420-001', 'Dell',    'Latitude 5420',       'i5-1135G7/16GB/512GB',   'Disponible',    '2025-01-15', NULL, NULL,     '2025-01-15', NULL, NULL, NULL),
(2,  1, 1, 1, NULL, 'ACT-002', 'DL-LAT-5420-002', 'Dell',    'Latitude 5420',       'i5-1135G7/16GB/512GB',   'Asignado',      '2025-01-15', NULL, 'Asignado a Ana Gómez',              '2025-01-15', NULL, NULL, NULL),
(3,  1, 1, 1, NULL, 'ACT-003', 'DL-LAT-5420-003', 'Dell',    'Latitude 5420',       'i5-1135G7/16GB/512GB',   'Asignado',      '2025-01-15', NULL, 'Asignado a Pedro Hernández',          '2025-01-15', NULL, NULL, NULL),
(4,  1, 1, 1, NULL, 'ACT-004', 'DL-LAT-5420-004', 'Dell',    'Latitude 5420',       'i5-1135G7/16GB/512GB',   'Asignado',      '2025-01-15', NULL, 'Asignado a Laura Díaz',               '2025-01-15', NULL, NULL, NULL),
(5,  1, 1, 1, NULL, 'ACT-005', 'DL-LAT-5420-005', 'Dell',    'Latitude 5420',       'i5-1135G7/16GB/512GB',   'Disponible',    '2025-01-15', NULL, NULL,     '2025-01-15', NULL, NULL, NULL),
(6,  1, 1, 1, NULL, 'ACT-006', 'DL-LAT-5420-006', 'Dell',    'Latitude 5420',       'i5-1135G7/16GB/512GB',   'EnMantenimiento','2025-01-15', NULL, 'Pantalla dañada - en reparación',     '2025-01-15', NULL, NULL, NULL),
(7,  1, 1, 1, NULL, 'ACT-007', 'DL-LAT-5420-007', 'Dell',    'Latitude 5420',       'i5-1135G7/16GB/512GB',   'Disponible',    '2025-01-15', NULL, NULL,     '2025-01-15', NULL, NULL, NULL),
(8,  1, 1, 1, NULL, 'ACT-008', 'DL-LAT-5420-008', 'Dell',    'Latitude 5420',       'i5-1135G7/16GB/512GB',   'Disponible',    '2025-01-15', NULL, NULL,     '2025-01-15', NULL, NULL, NULL),
-- Monitores HP (OC-002 / ItemOC 2)
(9,  3, 2, 2, NULL, 'ACT-009', 'HP-MON-24-001',   'HP',      'Monitor E24 G5',      '24" FHD IPS',            'Disponible',    '2025-01-25', NULL, NULL,     '2025-01-25', NULL, NULL, NULL),
(10, 3, 2, 2, NULL, 'ACT-010', 'HP-MON-24-002',   'HP',      'Monitor E24 G5',      '24" FHD IPS',            'Disponible',    '2025-01-25', NULL, NULL,     '2025-01-25', NULL, NULL, NULL),
(11, 3, 2, 2, NULL, 'ACT-011', 'HP-MON-24-003',   'HP',      'Monitor E24 G5',      '24" FHD IPS',            'Asignado',      '2025-01-25', NULL, 'Entregado con laptop a Ana Gómez',    '2025-01-25', NULL, NULL, NULL),
(12, 3, 2, 2, NULL, 'ACT-012', 'HP-MON-24-004',   'HP',      'Monitor E24 G5',      '24" FHD IPS',            'Disponible',    '2025-01-25', NULL, NULL,     '2025-01-25', NULL, NULL, NULL),
-- Switch / Router Cisco (OC-003 / ItemOC 3 y 4)
(13, 5, 3, 3, NULL, 'ACT-013', 'CS-CAT-2960-001', 'Cisco',   'Catalyst 2960-X',     '48 Puertos Gigabit',     'Disponible',    '2025-02-10', NULL, NULL,     '2025-02-10', NULL, NULL, NULL),
(14, 5, 3, 3, NULL, 'ACT-014', 'CS-CAT-2960-002', 'Cisco',   'Catalyst 2960-X',     '48 Puertos Gigabit',     'Disponible',    '2025-02-10', NULL, NULL,     '2025-02-10', NULL, NULL, NULL),
(15, 5, 3, 4, NULL, 'ACT-015', 'CS-ISR-1100-001', 'Cisco',   'ISR 1100',            'Router Gigabit',         'Disponible',    '2025-02-10', NULL, NULL,     '2025-02-10', NULL, NULL, NULL),
-- Tablets Lenovo (OC-004 / ItemOC 5)
(16, 9, 4, 5, NULL, 'ACT-016', 'LEN-TAB-P11-001', 'Lenovo',  'Tab P11',             '128GB/WiFi',             'Asignado',      '2025-02-20', NULL, 'Asignado a Diego Ramírez',           '2025-02-20', NULL, NULL, NULL),
(17, 9, 4, 5, NULL, 'ACT-017', 'LEN-TAB-P11-002', 'Lenovo',  'Tab P11',             '128GB/WiFi',             'Disponible',    '2025-02-20', NULL, NULL,     '2025-02-20', NULL, NULL, NULL),
(18, 9, 4, 5, NULL, 'ACT-018', 'LEN-TAB-P11-003', 'Lenovo',  'Tab P11',             '128GB/WiFi',             'Disponible',    '2025-02-20', NULL, NULL,     '2025-02-20', NULL, NULL, NULL),
-- Impresoras Epson (OC-005 / ItemOC 6)
(19, 4, 5, 6, NULL, 'ACT-019', 'EPS-WF-7840-001', 'Epson',   'WorkForce WF-7840',   'Multifuncional A3',      'Disponible',    '2025-03-05', NULL, NULL,     '2025-03-05', NULL, NULL, NULL),
(20, 4, 5, 6, NULL, 'ACT-020', 'EPS-WF-7840-002', 'Epson',   'WorkForce WF-7840',   'Multifuncional A3',      'Disponible',    '2025-03-05', NULL, NULL,     '2025-03-05', NULL, NULL, NULL),
-- UPS APC (OC-006 / ItemOC 7)
(21, 8, 6, 7, NULL, 'ACT-021', 'APC-SMT-1500-001','APC',     'Smart-UPS SMT1500',   '1500VA/900W',            'Disponible',    '2025-03-15', NULL, 'UPS para rack servidores',            '2025-03-15', NULL, NULL, NULL),
(22, 8, 6, 7, NULL, 'ACT-022', 'APC-SMT-1500-002','APC',     'Smart-UPS SMT1500',   '1500VA/900W',            'Disponible',    '2025-03-15', NULL, 'UPS para rack servidores',            '2025-03-15', NULL, NULL, NULL),
-- Servidores HP (OC-009 / ItemOC 10)
(23, 7, 9, 10, NULL, 'ACT-023', 'HP-DL-380-001',   'HP',      'ProLiant DL380 Gen10','Xeon 16C/128GB/2TB',     'Disponible',    '2025-04-15', NULL, 'Servidor principal - Datacenter',     '2025-04-15', NULL, NULL, NULL),
(24, 7, 9, 10, NULL, 'ACT-024', 'HP-DL-380-002',   'HP',      'ProLiant DL380 Gen10','Xeon 16C/128GB/2TB',     'Disponible',    '2025-04-15', NULL, 'Servidor respaldo - Datacenter',      '2025-04-15', NULL, NULL, NULL),
-- Desktop HP (OC-008 / ItemOC 9)
(25, 2, 8, 9, NULL, 'ACT-025', 'HP-DT-ELITE-001', 'HP',      'EliteDesk 800 G6',    'i7-10700/16GB/512GB',    'Disponible',    '2025-04-20', NULL, NULL,     '2025-04-20', NULL, NULL, NULL),
(26, 2, 8, 9, NULL, 'ACT-026', 'HP-DT-ELITE-002', 'HP',      'EliteDesk 800 G6',    'i7-10700/16GB/512GB',    'DadoDeBaja',    '2023-11-15', '2025-05-01', 'Dañado por descarga eléctrica',        '2023-11-15', NULL, NULL, NULL),
-- Periféricos (OC-007 / ItemOC 8)
(27, 6, 7, 8, NULL, 'ACT-027', 'DL-KB-MOUSE-001', 'Dell',    'Teclado + Mouse',     'KB216/MS116',            'Disponible',    '2025-03-25', NULL, NULL,     '2025-03-25', NULL, NULL, NULL),
(28, 6, 7, 8, NULL, 'ACT-028', 'DL-KB-MOUSE-002', 'Dell',    'Teclado + Mouse',     'KB216/MS116',            'Disponible',    '2025-03-25', NULL, NULL,     '2025-03-25', NULL, NULL, NULL),
-- Periféricos (OC-010 / ItemOC 11 y 12)
(29, 10,10,11, NULL, 'ACT-029', 'LOG-C920-001',    'Logitech','C920 HD Pro',         'Cámara Web 1080p',       'Disponible',    '2025-05-10', NULL, NULL,     '2025-05-10', NULL, NULL, NULL),
(30, 10,10,12, NULL, 'ACT-030', 'LOG-H800-001',    'Logitech','H800 Wireless',       'Headset Inalámbrico',    'Disponible',    '2025-05-10', NULL, NULL,     '2025-05-10', NULL, NULL, NULL);

SELECT setval(pg_get_serial_sequence('"Activos"', 'IdActivo'), 30);

-- ============================================================
-- 8. DETALLES DE ITEM OC
-- ============================================================
INSERT INTO "DetallesItemOC" ("IdDetalleItemOC", "IdItemOC", "Serial", "Procesado", "IdActivo", "Observaciones", "FechaCreacion", "FechaModificacion", "CreadoPor", "ModificadoPor") VALUES
-- ItemOC 1 - Laptops Dell (8 activos)
(1,  1, 'DL-LAT-5420-001', true, 1,  NULL, '2025-01-15', NULL, NULL, NULL),
(2,  1, 'DL-LAT-5420-002', true, 2,  NULL, '2025-01-15', NULL, NULL, NULL),
(3,  1, 'DL-LAT-5420-003', true, 3,  NULL, '2025-01-15', NULL, NULL, NULL),
(4,  1, 'DL-LAT-5420-004', true, 4,  NULL, '2025-01-15', NULL, NULL, NULL),
(5,  1, 'DL-LAT-5420-005', true, 5,  NULL, '2025-01-15', NULL, NULL, NULL),
(6,  1, 'DL-LAT-5420-006', true, 6,  NULL, '2025-01-15', NULL, NULL, NULL),
(7,  1, 'DL-LAT-5420-007', true, 7,  NULL, '2025-01-15', NULL, NULL, NULL),
(8,  1, 'DL-LAT-5420-008', true, 8,  NULL, '2025-01-15', NULL, NULL, NULL),
-- ItemOC 2 - Monitores HP (4 activos)
(9,  2, 'HP-MON-24-001',   true, 9,  NULL, '2025-01-25', NULL, NULL, NULL),
(10, 2, 'HP-MON-24-002',   true, 10, NULL, '2025-01-25', NULL, NULL, NULL),
(11, 2, 'HP-MON-24-003',   true, 11, NULL, '2025-01-25', NULL, NULL, NULL),
(12, 2, 'HP-MON-24-004',   true, 12, NULL, '2025-01-25', NULL, NULL, NULL),
-- ItemOC 3 - Switch Cisco (2 activos)
(13, 3, 'CS-CAT-2960-001', true, 13, NULL, '2025-02-10', NULL, NULL, NULL),
(14, 3, 'CS-CAT-2960-002', true, 14, NULL, '2025-02-10', NULL, NULL, NULL),
-- ItemOC 4 - Router Cisco (1 activo)
(15, 4, 'CS-ISR-1100-001', true, 15, NULL, '2025-02-10', NULL, NULL, NULL),
-- ItemOC 5 - Tablets Lenovo (3 activos)
(16, 5, 'LEN-TAB-P11-001', true, 16, NULL, '2025-02-20', NULL, NULL, NULL),
(17, 5, 'LEN-TAB-P11-002', true, 17, NULL, '2025-02-20', NULL, NULL, NULL),
(18, 5, 'LEN-TAB-P11-003', true, 18, NULL, '2025-02-20', NULL, NULL, NULL),
-- ItemOC 6 - Impresoras Epson (2 activos)
(19, 6, 'EPS-WF-7840-001', true, 19, NULL, '2025-03-05', NULL, NULL, NULL),
(20, 6, 'EPS-WF-7840-002', true, 20, NULL, '2025-03-05', NULL, NULL, NULL),
-- ItemOC 7 - UPS APC (2 activos)
(21, 7, 'APC-SMT-1500-001', true, 21, NULL, '2025-03-15', NULL, NULL, NULL),
(22, 7, 'APC-SMT-1500-002', true, 22, NULL, '2025-03-15', NULL, NULL, NULL),
-- ItemOC 10 - Servidores HP (2 activos)
(23, 10,'HP-DL-380-001',   true, 23, NULL, '2025-04-15', NULL, NULL, NULL),
(24, 10,'HP-DL-380-002',   true, 24, NULL, '2025-04-15', NULL, NULL, NULL),
-- ItemOC 9 - Desktop HP (2 activos)
(25, 9, 'HP-DT-ELITE-001', true, 25, NULL, '2025-04-20', NULL, NULL, NULL),
(26, 9, 'HP-DT-ELITE-002', true, 26, NULL, '2023-11-15', NULL, NULL, NULL),
-- ItemOC 8 - Kit Teclado+Mouse Dell (2 activos)
(27, 8, 'DL-KB-MOUSE-001', true, 27, NULL, '2025-03-25', NULL, NULL, NULL),
(28, 8, 'DL-KB-MOUSE-002', true, 28, NULL, '2025-03-25', NULL, NULL, NULL),
-- ItemOC 11 - Cámara Logitech (1 activo)
(29, 11,'LOG-C920-001',    true, 29, NULL, '2025-05-10', NULL, NULL, NULL),
-- ItemOC 12 - Headset Logitech (1 activo)
(30, 12,'LOG-H800-001',    true, 30, NULL, '2025-05-10', NULL, NULL, NULL);

SELECT setval(pg_get_serial_sequence('"DetallesItemOC"', 'IdDetalleItemOC'), 30);

-- Actualizar Activos con IdDetalleItemOC
UPDATE "Activos" SET "IdDetalleItemOC" = 1 WHERE "IdActivo" = 1;
UPDATE "Activos" SET "IdDetalleItemOC" = 2 WHERE "IdActivo" = 2;
UPDATE "Activos" SET "IdDetalleItemOC" = 3 WHERE "IdActivo" = 3;
UPDATE "Activos" SET "IdDetalleItemOC" = 4 WHERE "IdActivo" = 4;
UPDATE "Activos" SET "IdDetalleItemOC" = 5 WHERE "IdActivo" = 5;
UPDATE "Activos" SET "IdDetalleItemOC" = 6 WHERE "IdActivo" = 6;
UPDATE "Activos" SET "IdDetalleItemOC" = 7 WHERE "IdActivo" = 7;
UPDATE "Activos" SET "IdDetalleItemOC" = 8 WHERE "IdActivo" = 8;
UPDATE "Activos" SET "IdDetalleItemOC" = 9 WHERE "IdActivo" = 9;
UPDATE "Activos" SET "IdDetalleItemOC" = 10 WHERE "IdActivo" = 10;
UPDATE "Activos" SET "IdDetalleItemOC" = 11 WHERE "IdActivo" = 11;
UPDATE "Activos" SET "IdDetalleItemOC" = 12 WHERE "IdActivo" = 12;
UPDATE "Activos" SET "IdDetalleItemOC" = 13 WHERE "IdActivo" = 13;
UPDATE "Activos" SET "IdDetalleItemOC" = 14 WHERE "IdActivo" = 14;
UPDATE "Activos" SET "IdDetalleItemOC" = 15 WHERE "IdActivo" = 15;
UPDATE "Activos" SET "IdDetalleItemOC" = 16 WHERE "IdActivo" = 16;
UPDATE "Activos" SET "IdDetalleItemOC" = 17 WHERE "IdActivo" = 17;
UPDATE "Activos" SET "IdDetalleItemOC" = 18 WHERE "IdActivo" = 18;
UPDATE "Activos" SET "IdDetalleItemOC" = 19 WHERE "IdActivo" = 19;
UPDATE "Activos" SET "IdDetalleItemOC" = 20 WHERE "IdActivo" = 20;
UPDATE "Activos" SET "IdDetalleItemOC" = 21 WHERE "IdActivo" = 21;
UPDATE "Activos" SET "IdDetalleItemOC" = 22 WHERE "IdActivo" = 22;
UPDATE "Activos" SET "IdDetalleItemOC" = 23 WHERE "IdActivo" = 23;
UPDATE "Activos" SET "IdDetalleItemOC" = 24 WHERE "IdActivo" = 24;
UPDATE "Activos" SET "IdDetalleItemOC" = 25 WHERE "IdActivo" = 25;
UPDATE "Activos" SET "IdDetalleItemOC" = 26 WHERE "IdActivo" = 26;
UPDATE "Activos" SET "IdDetalleItemOC" = 27 WHERE "IdActivo" = 27;
UPDATE "Activos" SET "IdDetalleItemOC" = 28 WHERE "IdActivo" = 28;
UPDATE "Activos" SET "IdDetalleItemOC" = 29 WHERE "IdActivo" = 29;
UPDATE "Activos" SET "IdDetalleItemOC" = 30 WHERE "IdActivo" = 30;

-- ============================================================
-- 9. PARQUEADEROS
-- ============================================================
INSERT INTO "Parqueaderos" ("IdParqueadero", "IdSede", "Nombre", "Ubicacion", "Estado", "FechaCreacion", "FechaModificacion", "CreadoPor", "ModificadoPor") VALUES
(1, 1, 'Parqueadero Principal',  'Sótano 1 - Puesto 1-20', 'Activo', '2025-01-01 00:00:00', NULL, NULL, NULL),
(2, 1, 'Parqueadero Visitas',    'Planta Baja - Puesto 21-30', 'Activo', '2025-01-01 00:00:00', NULL, NULL, NULL),
(3, 2, 'Parqueadero Norte',      'Sótano 1 - Puesto 1-15', 'Activo', '2025-01-01 00:00:00', NULL, NULL, NULL),
(4, 3, 'Parqueadero Medellín',   'Planta Baja - Puesto 1-10', 'Activo', '2025-01-01 00:00:00', NULL, NULL, NULL);

SELECT setval(pg_get_serial_sequence('"Parqueaderos"', 'IdParqueadero'), 4);

-- ============================================================
-- 10. CANALES DE SOLICITUD
-- ============================================================
INSERT INTO "Canales" ("IdCanal", "Nombre", "FechaSolicitud", "FechaCreacion", "FechaModificacion", "CreadoPor", "ModificadoPor") VALUES
(1, 'Correo Electrónico', '2025-01-01 00:00:00', '2025-01-01 00:00:00', NULL, NULL, NULL),
(2, 'Sistema de Tickets', '2025-01-01 00:00:00', '2025-01-01 00:00:00', NULL, NULL, NULL),
(3, 'Teléfono',           '2025-01-01 00:00:00', '2025-01-01 00:00:00', NULL, NULL, NULL),
(4, 'Presencial',         '2025-01-01 00:00:00', '2025-01-01 00:00:00', NULL, NULL, NULL),
(5, 'WhatsApp',           '2025-06-01 00:00:00', '2025-06-01 00:00:00', NULL, NULL, NULL);

SELECT setval(pg_get_serial_sequence('"Canales"', 'IdCanal'), 5);

-- ============================================================
-- 11. SALIDAS
-- ============================================================
INSERT INTO "Salidas" ("IdSalida", "CodigoUnico", "EstadoActivo", "FechaSalida", "Observaciones", "FechaCreacion", "FechaModificacion", "CreadoPor", "ModificadoPor") VALUES
(1, 'SAL-20250120-000001', 'EnMantenimiento', '2025-01-20 09:00:00', 'Laptop con pantalla dañada - en reparación',  '2025-01-20 09:00:00', NULL, NULL, NULL),
(2, 'SAL-20250120-000002', 'Vendido',         '2025-01-20 09:30:00', 'Venta de equipo excedente',                   '2025-01-20 09:30:00', NULL, NULL, NULL),
(3, 'SAL-20250201-000003', 'DadoDeBaja',      '2025-02-01 10:00:00', 'Laptop obsoleta - dada de baja',              '2025-02-01 10:00:00', NULL, NULL, NULL),
(4, 'SAL-20250220-000004', 'Vendido',         '2025-02-20 11:00:00', 'Venta de tablet en desuso',                   '2025-02-20 11:00:00', NULL, NULL, NULL),
(5, 'SAL-20250310-000005', 'EnMantenimiento', '2025-03-10 14:00:00', 'Switch en configuración para red parqueadero','2025-03-10 14:00:00', NULL, NULL, NULL),
(6, 'SAL-20250401-000006', 'Vendido',         '2025-04-01 08:00:00', 'Monitor vendido a personal externo',          '2025-04-01 08:00:00', NULL, NULL, NULL);

SELECT setval(pg_get_serial_sequence('"Salidas"', 'IdSalida'), 6);

-- ============================================================
-- 12. DETALLES DE SALIDA
-- ============================================================
INSERT INTO "DetallesSalida" ("IdDetalleSalida", "IdSalida", "IdActivo", "Cantidad", "FechaCreacion", "FechaModificacion", "CreadoPor", "ModificadoPor") VALUES
(1, 1, 2, 1,  '2025-01-20 09:00:00', NULL, NULL, NULL),
(2, 1, 11, 1, '2025-01-20 09:00:00', NULL, NULL, NULL),
(3, 2, 3, 1,  '2025-01-20 09:30:00', NULL, NULL, NULL),
(4, 3, 4, 1,  '2025-02-01 10:00:00', NULL, NULL, NULL),
(5, 4, 16, 1, '2025-02-20 11:00:00', NULL, NULL, NULL),
(6, 5, 13, 1, '2025-03-10 14:00:00', NULL, NULL, NULL),
(7, 6, 10, 1, '2025-04-01 08:00:00', NULL, NULL, NULL);

SELECT setval(pg_get_serial_sequence('"DetallesSalida"', 'IdDetalleSalida'), 7);

-- ============================================================
-- 13. ASIGNACIONES DE USUARIO
-- ============================================================
INSERT INTO "AsignacionesUsuario" ("IdAsignacion", "IdActivo", "IdUsuarioDestino", "IdParqueadero", "IdCanal", "IdUsuarioEntrega", "RegistroSalida", "NumeroTicket", "FechaAsignacion", "EstadoAsignacion", "FechaCreacion", "FechaModificacion", "CreadoPor", "ModificadoPor") VALUES
(1, 2,  4, NULL, 2, 2, 'Entrega laptop + monitor a Ana Gómez',      'TK-2025-001', '2025-01-20 09:00:00', 'Activa',     '2025-01-20 09:00:00', NULL, NULL, NULL),
(2, 3,  5, NULL, 2, 2, 'Entrega laptop a Pedro Hernández',          'TK-2025-002', '2025-01-20 09:30:00', 'Activa',     '2025-01-20 09:30:00', NULL, NULL, NULL),
(3, 4,  6, NULL, 2, 3, 'Entrega laptop a Laura Díaz',               'TK-2025-015', '2025-02-01 10:00:00', 'Activa',     '2025-02-01 10:00:00', NULL, NULL, NULL),
(4, 16, 7, NULL, 2, 2, 'Entrega tablet a Diego Ramírez',            'TK-2025-030', '2025-02-20 11:00:00', 'Activa',     '2025-02-20 11:00:00', NULL, NULL, NULL),
(5, 11, 4, NULL, 2, 2, 'Entrega monitor a Ana Gómez',               'TK-2025-001', '2025-01-20 09:00:00', 'Activa',     '2025-01-20 09:00:00', NULL, NULL, NULL),
(6, 26, 5, NULL, 2, 2, 'Devolución desktop dañado',                 'TK-2025-002', '2023-11-15 08:00:00', 'Finalizada', '2023-11-15 08:00:00', NULL, NULL, NULL);

SELECT setval(pg_get_serial_sequence('"AsignacionesUsuario"', 'IdAsignacion'), 6);

-- ============================================================
-- 14. HISTORIAL DE ACTIVOS
-- ============================================================
INSERT INTO "HistorialActivos" ("IdHistorial", "IdActivo", "IdSalida", "TipoMovimiento", "FechaMovimiento", "IdUsuarioEntrega", "FechaCreacion", "FechaModificacion", "CreadoPor", "ModificadoPor") VALUES
(1, 2,  1, 'Salida',     '2025-01-20 09:00:00', 2, '2025-01-20 09:00:00', NULL, NULL, NULL),
(2, 11, 1, 'Salida',     '2025-01-20 09:00:00', 2, '2025-01-20 09:00:00', NULL, NULL, NULL),
(3, 3,  2, 'Salida',     '2025-01-20 09:30:00', 2, '2025-01-20 09:30:00', NULL, NULL, NULL),
(4, 4,  3, 'Salida',     '2025-02-01 10:00:00', 3, '2025-02-01 10:00:00', NULL, NULL, NULL),
(5, 16, 4, 'Salida',     '2025-02-20 11:00:00', 2, '2025-02-20 11:00:00', NULL, NULL, NULL),
(6, 13, 5, 'Salida',     '2025-03-10 14:00:00', 2, '2025-03-10 14:00:00', NULL, NULL, NULL),
(7, 10, 6, 'Salida',     '2025-04-01 08:00:00', 2, '2025-04-01 08:00:00', NULL, NULL, NULL),
(8, 26, 2, 'Devolucion', '2025-05-01 15:00:00', 2, '2025-05-01 15:00:00', NULL, NULL, NULL);

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
SELECT 'ItemsOC', COUNT(*) FROM "ItemsOC"
UNION ALL
SELECT 'Activos', COUNT(*) FROM "Activos"
UNION ALL
SELECT 'DetallesItemOC', COUNT(*) FROM "DetallesItemOC"
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
