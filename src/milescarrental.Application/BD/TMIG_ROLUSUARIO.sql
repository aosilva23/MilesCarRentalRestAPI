 CREATE TABLE "TMIG_ROLUSUARIO" ("RUSIDROL" NUMBER(4,0), "RUSNOMBREUSUARIO" VARCHAR2(100 CHAR)) ;

   COMMENT ON COLUMN "TMIG_ROLUSUARIO"."RUSIDROL" IS 'CÃ³digo identificador de llave forÃ¡nea con la entidad rol';
   COMMENT ON COLUMN "TMIG_ROLUSUARIO"."RUSNOMBREUSUARIO" IS 'CÃ³digo identificador de llave forÃ¡nea con el usuario';
   COMMENT ON TABLE "TMIG_ROLUSUARIO"  IS 'Entidad que se encarga de almacenar los usuarios y los roles que posee';
   
Insert into TMIG_ROLUSUARIO (RUSIDROL,RUSNOMBREUSUARIO) values ('1','ADMIN');

COMMIT;
