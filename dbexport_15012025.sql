-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server-Version:               PostgreSQL 17.2 (Debian 17.2-1.pgdg120+1) on x86_64-pc-linux-gnu, compiled by gcc (Debian 12.2.0-14) 12.2.0, 64-bit
-- Server-Betriebssystem:        
-- HeidiSQL Version:             12.8.0.6908
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES  */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

-- Exportiere Daten aus Tabelle public.iounit_configuration: 0 rows
/*!40000 ALTER TABLE "iounit_configuration" DISABLE KEYS */;
/*!40000 ALTER TABLE "iounit_configuration" ENABLE KEYS */;

-- Exportiere Daten aus Tabelle public.iounit_data_chronology: 3 rows
/*!40000 ALTER TABLE "iounit_data_chronology" DISABLE KEYS */;
INSERT IGNORE INTO "iounit_data_chronology" ("id", "datetime", "value_numerical", "value_alphanumerical", "id_mu", "id_sdm") VALUES
	(1, '2024-12-29 10:04:20.382702', 24.2, NULL, NULL, 1),
	(2, '2024-12-29 10:04:38.58103', 28.2, NULL, NULL, 1),
	(3, '2024-12-29 10:04:52.391005', 33.4, NULL, NULL, 1);
/*!40000 ALTER TABLE "iounit_data_chronology" ENABLE KEYS */;

-- Exportiere Daten aus Tabelle public.iounit_data_currently: 2 rows
/*!40000 ALTER TABLE "iounit_data_currently" DISABLE KEYS */;
INSERT IGNORE INTO "iounit_data_currently" ("id_sdm", "value_numerical", "value_alphanumerical", "write_direction", "direction_stamp_a", "direction_stamp_b", "last_processing") VALUES
	(1, 288, NULL, 'false', '2025-01-01 00:00:00', '2025-01-01 00:00:00', '2025-01-01 00:00:00'),
	(2, 299, NULL, 'false', '2025-01-01 00:00:00', '2025-01-01 00:00:00', '2025-01-01 00:00:00');
/*!40000 ALTER TABLE "iounit_data_currently" ENABLE KEYS */;

-- Exportiere Daten aus Tabelle public.iounit_data_masterdata: 1 rows
/*!40000 ALTER TABLE "iounit_data_masterdata" DISABLE KEYS */;
INSERT IGNORE INTO "iounit_data_masterdata" ("id", "long_name", "short_name", "id_sc", "id_mu") VALUES
	(1, 'Temperature Sensor', 'Temp Sensor', NULL, NULL);
/*!40000 ALTER TABLE "iounit_data_masterdata" ENABLE KEYS */;

-- Exportiere Daten aus Tabelle public.iounit_measuring_units: -1 rows
/*!40000 ALTER TABLE "iounit_measuring_units" DISABLE KEYS */;
/*!40000 ALTER TABLE "iounit_measuring_units" ENABLE KEYS */;

-- Exportiere Daten aus Tabelle public.iounit_type: -1 rows
/*!40000 ALTER TABLE "iounit_type" DISABLE KEYS */;
/*!40000 ALTER TABLE "iounit_type" ENABLE KEYS */;

-- Exportiere Daten aus Tabelle public.iounit_users: -1 rows
/*!40000 ALTER TABLE "iounit_users" DISABLE KEYS */;
INSERT IGNORE INTO "iounit_users" ("id", "username", "pw_hash", "id_ur", "last_modify") VALUES
	(1, 'testy', 'ecd71870d1963316a97e3ac3408c9835ad8cf0f3c1bc703527c30265534f75ae', 2, '2025-01-03 10:58:12');
/*!40000 ALTER TABLE "iounit_users" ENABLE KEYS */;

-- Exportiere Daten aus Tabelle public.iounit_user_roles: -1 rows
/*!40000 ALTER TABLE "iounit_user_roles" DISABLE KEYS */;
INSERT IGNORE INTO "iounit_user_roles" ("id", "rolename") VALUES
	(1, 'Default'),
	(2, 'Admin');
/*!40000 ALTER TABLE "iounit_user_roles" ENABLE KEYS */;

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
