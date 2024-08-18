--Daten prüfen


SELECT * FROM iounit_data_chronology



SELECT * FROM iounit_data_currently


--Testcase



UPDATE iounit_data_currently SET direction_stamp_a = CURRENT_TIMESTAMP WHERE id_sdm = 1