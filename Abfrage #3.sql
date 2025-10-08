SELECT id_sdm, id_mu, value_numerical, value_alphanumerical
FROM iounit_data_currently
INNER JOIN
iounit_data_masterdata ON iounit_data_masterdata.id = iounit_data_currently.id_sdm
WHERE direction_stamp_a IS NOT NULL
ORDER BY direction_stamp_a ASC
LIMIT 1




SELECT * FROM iounit_configuration

SELECT * FROM iounit_data_masterdata

SELECT * FROM iounit_data_currently



CREATE TABLE measuring_units(
id SERIAL PRIMARY KEY,
long_name VARCHAR(40),
short_name VARCHAR(25))


SELECT * FROM measuring_units


ALTER TABLE iounit_data_chronology
RENAME COLUMN id_sc TO id_sdm;