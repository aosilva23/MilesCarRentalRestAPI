CREATE TABLE "TMCR_VEHICULO" 
(	
	"VEHID" NUMBER(10,0) NOT NULL ENABLE,	
	"VEHPLACA" VARCHAR2(10 CHAR) NOT NULL ENABLE, 
	"VEHMODELO" VARCHAR2(50 CHAR) NOT NULL ENABLE, 
	"VEHKILOMETRAJE" NUMBER(8,0) NOT NULL ENABLE, 
	"VEHTIPO" VARCHAR2(50 CHAR) NOT NULL ENABLE, 
	"VEHNOMECLATURA" VARCHAR2(50 CHAR) NOT NULL ENABLE,
	"VEHLOCLATITUD" VARCHAR2(100 CHAR) NOT NULL ENABLE,
	"VEHLOCLONGITUD" VARCHAR2(100 CHAR) NOT NULL ENABLE,
	"VEHOBSERVACIONES" VARCHAR2(500 CHAR),	
	"VEHESTADO" VARCHAR2(20 CHAR) NOT NULL ENABLE,
	"VEHFECHAREGISTRO" DATE,
	"VEHSUARIOCREACION" VARCHAR2(20 CHAR),
	CONSTRAINT "TMCR_VEHICULO_PK" PRIMARY KEY ("VEHID")
)


Insert into TMCR_VEHICULO (VEHID,VEHPLACA,VEHMODELO,VEHKILOMETRAJE,VEHTIPO,VEHNOMECLATURA,VEHLOCLATITUD,VEHLOCLONGITUD,VEHOBSERVACIONES,VEHESTADO,VEHFECHAREGISTRO,VEHSUARIOCREACION) values ('2','TEST002','TESLA','10','DEPORTIVO','TEST0002','1.00001','2.00001','NA','ALQUILADO',to_date('19/02/2024 23:53:50','DD/MM/YYYY HH24:MI:SS'),'AOSILVA');
Insert into TMCR_VEHICULO (VEHID,VEHPLACA,VEHMODELO,VEHKILOMETRAJE,VEHTIPO,VEHNOMECLATURA,VEHLOCLATITUD,VEHLOCLONGITUD,VEHOBSERVACIONES,VEHESTADO,VEHFECHAREGISTRO,VEHSUARIOCREACION) values ('1','TEST001','TESLA','10','DEPORTIVO','TEST0001','1.00001','2.00001','NA','ALQUILADO',to_date('19/02/2024 23:41:26','DD/MM/YYYY HH24:MI:SS'),'AOSILVA');
