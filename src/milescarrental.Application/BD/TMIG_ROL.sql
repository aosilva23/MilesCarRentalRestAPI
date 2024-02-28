  CREATE TABLE "TMIG_ROL" ("ROLID" NUMBER(4,0), "ROLNOMBRE" VARCHAR2(50 CHAR), "ROLACTIVO" CHAR(1 CHAR), "ROLUSUARIOCREACION" VARCHAR2(50 CHAR), "ROLFECHACREACION" DATE) ;

   COMMENT ON COLUMN "TMIG_ROL"."ROLID" IS 'CÃ³digo identificador de llave primaria';
   COMMENT ON COLUMN "TMIG_ROL"."ROLNOMBRE" IS 'Nombre del rol';
   COMMENT ON COLUMN "TMIG_ROL"."ROLACTIVO" IS 'Indica si el rol estÃ¡ activo en la aplicaciÃ³n';
   COMMENT ON COLUMN "TMIG_ROL"."ROLUSUARIOCREACION" IS 'Usuario de auditorÃ­a';
   COMMENT ON COLUMN "TMIG_ROL"."ROLFECHACREACION" IS 'Fecha de auditorÃ­a';
   COMMENT ON TABLE "TMIG_ROL"  IS 'Entidad que se encarga de almacenar todos los roles definidos en la aplicaciÃ³n';
   
Insert into TMIG_ROL (ROLID,ROLNOMBRE,ROLACTIVO,ROLUSUARIOCREACION,ROLFECHACREACION) values ('1','ADMINISTRADOR','S','NEO',to_date('20/09/2022 14:34:17','DD/MM/YYYY HH24:MI:SS'));
Insert into TMIG_ROL (ROLID,ROLNOMBRE,ROLACTIVO,ROLUSUARIOCREACION,ROLFECHACREACION) values ('2','CONSULTOR','S','NEO',to_date('20/09/2022 14:34:17','DD/MM/YYYY HH24:MI:SS'));
Insert into TMIG_ROL (ROLID,ROLNOMBRE,ROLACTIVO,ROLUSUARIOCREACION,ROLFECHACREACION) values ('3','VISITANTE','N','NEO',to_date('20/09/2022 14:34:17','DD/MM/YYYY HH24:MI:SS'));

COMMIT;