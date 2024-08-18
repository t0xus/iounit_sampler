CREATE TABLE iounit_data_currently (
    id_sdm SERIAL PRIMARY KEY,
	value_numerical NUMERIC,
	value_alphanumerical VARCHAR(25),
	write_direction BOOLEAN,
	direction_stamp_a: TIMESTAMP,
	direction_stamp_b: TIMESTAMP,
	last_processing: TIMESTAMP
);