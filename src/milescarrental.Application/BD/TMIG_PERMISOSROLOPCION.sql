  CREATE TABLE "TMIG_PERMISOSROLOPCION" ("PROIDOPCION" NUMBER(4,0), "PROIDROL" NUMBER(4,0)) ;

   COMMENT ON COLUMN "TMIG_PERMISOSROLOPCION"."PROIDOPCION" IS 'CÃ³digo identificador de llave forÃ¡nea con las opciones del menÃº';
   COMMENT ON COLUMN "TMIG_PERMISOSROLOPCION"."PROIDROL" IS 'CÃ³digo identificador de llave forÃ¡nea con los roles definidos en la aplicaciÃ³n';
   COMMENT ON TABLE "TMIG_PERMISOSROLOPCION"  IS 'Entidad que se encarga de almacenar los permisos que cada rol tiene asignado en la aplicaciÃ³n';
   
Insert into TMIG_PERMISOSROLOPCION (PROIDOPCION,PROIDROL) values ('1','1');
Insert into TMIG_PERMISOSROLOPCION (PROIDOPCION,PROIDROL) values ('2','1');
Insert into TMIG_PERMISOSROLOPCION (PROIDOPCION,PROIDROL) values ('3','1');
Insert into TMIG_PERMISOSROLOPCION (PROIDOPCION,PROIDROL) values ('4','1');
Insert into TMIG_PERMISOSROLOPCION (PROIDOPCION,PROIDROL) values ('1','2');
Insert into TMIG_PERMISOSROLOPCION (PROIDOPCION,PROIDROL) values ('3','2');
Insert into TMIG_PERMISOSROLOPCION (PROIDOPCION,PROIDROL) values ('2','3');
Insert into TMIG_PERMISOSROLOPCION (PROIDOPCION,PROIDROL) values ('4','3');

commit;
   