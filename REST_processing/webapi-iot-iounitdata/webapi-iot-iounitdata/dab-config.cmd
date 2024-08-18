@echo off
@echo This cmd file creates a Data API Builder configuration based on the chosen database objects.
@echo To run the cmd, create an .env file with the following contents:
@echo dab-connection-string=your connection string
@echo ** Make sure to exclude the .env file from source control **
@echo **
dotnet tool install -g Microsoft.DataApiBuilder
dab init -c dab-config.json --database-type postgresql --connection-string "@env('dab-connection-string')" --host-mode Development
@echo Adding tables
dab add "iounit_data_currently" --source "[public].[iounit_data_currently]" --fields.include "id_sdm,value_numerical,value_alphanumerical,write_direction,direction_stamp_a,direction_stamp_b,last_processing" --permissions "anonymous:*" 
dab add "iounit_data_chronology" --source "[public].[iounit_data_chronology]" --fields.include "id,datetime,value_numeric,value_alphanumeric,id_mu,id_sc" --permissions "anonymous:*" 
@echo Adding views and tables without primary key
@echo Adding relationships
@echo **
@echo ** run 'dab validate' to validate your configuration **
@echo ** run 'dab start' to start the development API host **
