create or replace PROCEDURE "PR_MCR_PROCES_CLIENTE" (
    id              TMCR_CLIENTE.CLIID%TYPE,
    tipoDocumento   TMCR_CLIENTE.CLITIPODOCUMENTO%TYPE,
    nroDocumento    TMCR_CLIENTE.CLINRODOCUMENTO%TYPE,
    nombreCompleto  TMCR_CLIENTE.CLINOMBRECOMPLETO%TYPE,
    telefono        TMCR_CLIENTE.CLITELEFONO%TYPE,
    direccion       TMCR_CLIENTE.CLIDIRECCION%TYPE,
    email           TMCR_CLIENTE.CLIEMAIL%TYPE,
    observaciones   TMCR_CLIENTE.CLIOBSERVACIONES%TYPE,
    estado          TMCR_CLIENTE.CLIESTADO%TYPE,
    usuario         TMCR_CLIENTE.CLISUARIOCREACION%TYPE,
    proceso IN number  --> Busqueda Exacta(1), Insertar(2), Actualizar(3), Borrar(4) Registro, Listar todos(5), Busqueda Generica(6)
) AS
    -- Parametros de control:
    p_numRegistros NUMBER:=0;
    p_mensaje VARCHAR2(255 BYTE):='EN EJECUCION'; -- Se coloca el mensaje de respuesta de la transacion
	exc_NoData EXCEPTION;

    maxid TMCR_CLIENTE.CLIID%TYPE; -- Maximo ID, extraer el consecutivo para la insercion

    rf_cursor_leer SYS_REFCURSOR; -- Cursor que se va a retornar
    
BEGIN
    -- Determinar si el registro existe:
        SELECT
            count(*) INTO p_numRegistros
        FROM  TMCR_CLIENTE
        WHERE
            UPPER(CLITIPODOCUMENTO) = UPPER(tipoDocumento)
            AND UPPER(CLINRODOCUMENTO) = UPPER(nroDocumento);
    dbms_output.put_line(' NUMERO REGISTROS --> ' || p_numRegistros);

    -------------------
	-- BUSQUEDA EXACTA:
	-------------------
    IF(proceso = 1) THEN

		-- Cerrar el cursor si sigue abierto:
        IF ( rf_cursor_leer%isopen ) THEN
            CLOSE rf_cursor_leer;
        END IF;

        -- Determinar si el registro existe:
        SELECT
            count(*) INTO p_numRegistros
        FROM  TMCR_CLIENTE
        WHERE
            UPPER(CLITIPODOCUMENTO) = UPPER(tipoDocumento)
            AND UPPER(CLINRODOCUMENTO) = UPPER(nroDocumento);
        dbms_output.put_line(' NUMERO REGISTROS --> ' || p_numRegistros);

        --  SI SE ENCONTRARON REGISTROS, SE PUEDE MOSTRAR DAT0S
        IF(p_numRegistros > 0) THEN

            --p_mensaje := 'BUSQUEDA EXACTA - RESULTADO';
            SELECT
                mviestado ||': ' || mvimensaje
                INTO p_mensaje
            FROM TMCR_MENSAJESVISUALIZACION
                WHERE MVIPROCESO = proceso AND MVIESTADO = 'Exitoso';

            OPEN rf_cursor_leer FOR
                    SELECT
                        CLIID,
                        CLITIPODOCUMENTO,
                        CLINRODOCUMENTO,
                        CLINOMBRECOMPLETO,
                        CLITELEFONO, 
                        CLIDIRECCION,
                        CLIEMAIL,
                        CLIOBSERVACIONES,
                        CLIESTADO,
                        CLIFECHAREGISTRO,    
                        CLISUARIOCREACION,
                        proceso AS PROCESO,
                        p_mensaje AS MENSAJE
                    FROM  TMCR_CLIENTE
                    WHERE
                        UPPER(CLITIPODOCUMENTO) = UPPER(tipoDocumento)
                        AND UPPER(CLINRODOCUMENTO) = UPPER(nroDocumento);
            dbms_sql.return_result(rf_cursor_leer);
            
        -- NO EXISTE UN REGISTRO CON ESAS KEYS, NO SE PUEDE MOSTRAR DATOS
        ELSE
            RAISE exc_NoData;
        END IF;

    END IF;

	------------
	-- INSERTAR:
	------------
    IF(proceso = 2) THEN

		-- Cerrar el cursor si sigue abierto:
        IF ( rf_cursor_leer%isopen ) THEN
            CLOSE rf_cursor_leer;
        END IF;

		-- Determinar si el registro existe:
        SELECT
            count(*) INTO p_numRegistros
        FROM  TMCR_CLIENTE
        WHERE
            UPPER(CLITIPODOCUMENTO) = UPPER(tipoDocumento)
            AND UPPER(CLINRODOCUMENTO) = UPPER(nroDocumento);

        --  YA HAY UN REGISTRADO CON ESA KEY, NO PUEDE INSERTAR ESE REGISTRO
        IF(p_numRegistros > 0) THEN

            -- p_mensaje := 'INSERT - REGISTRO YA EXISTE';
            SELECT
                mviestado ||': ' || mvimensaje
                INTO p_mensaje
            FROM TMCR_MENSAJESVISUALIZACION
                WHERE MVIPROCESO = proceso AND MVIESTADO = 'Fallo';

            OPEN rf_cursor_leer FOR
                SELECT
                        CLIID,
                        CLITIPODOCUMENTO,
                        CLINRODOCUMENTO,
                        CLINOMBRECOMPLETO,
                        CLITELEFONO, 
                        CLIDIRECCION,
                        CLIEMAIL,
                        CLIOBSERVACIONES,
                        CLIESTADO,
                        CLIFECHAREGISTRO,    
                        CLISUARIOCREACION,
                        proceso AS PROCESO,
                        p_mensaje AS MENSAJE
                    FROM  TMCR_CLIENTE
                    WHERE
                        UPPER(CLITIPODOCUMENTO) = UPPER(tipoDocumento)
                        AND UPPER(CLINRODOCUMENTO) = UPPER(nroDocumento);
            dbms_sql.return_result(rf_cursor_leer);

        -- NO EXISTE UN REGISTRO CON ESA KEY, PUEDE PROCEDER A INSERTAR
        ELSE
        
            -- Regla para la primer insercion:
            IF(p_numRegistros = 0) THEN
                SELECT
                    COUNT(*) + 1
                    INTO maxid
                FROM TMCR_CLIENTE;
            ELSE
                SELECT
                    MAX(nvl(CLIID, 0)) + 1
                    INTO maxid
                FROM TMCR_CLIENTE;
            END IF;            

            INSERT INTO TMCR_CLIENTE(CLIID, CLITIPODOCUMENTO, CLINRODOCUMENTO, CLINOMBRECOMPLETO, CLITELEFONO, CLIDIRECCION, CLIEMAIL, CLIOBSERVACIONES, CLIESTADO, CLIFECHAREGISTRO, CLISUARIOCREACION)
            VALUES(maxid, tipoDocumento, nroDocumento, nombreCompleto, telefono, direccion, email, observaciones, estado, SYSDATE(), usuario);
            COMMIT;            
            
            -- p_mensaje := 'INSERT - REGISTRO INSERTADO';
            SELECT
                mviestado ||': ' || mvimensaje
                INTO p_mensaje
            FROM TMCR_MENSAJESVISUALIZACION
                WHERE MVIPROCESO = proceso AND MVIESTADO = 'Exitoso';

            OPEN rf_cursor_leer FOR
                SELECT
                        CLIID,
                        CLITIPODOCUMENTO,
                        CLINRODOCUMENTO,
                        CLINOMBRECOMPLETO,
                        CLITELEFONO, 
                        CLIDIRECCION,
                        CLIEMAIL,
                        CLIOBSERVACIONES,
                        CLIESTADO,
                        CLIFECHAREGISTRO,    
                        CLISUARIOCREACION,
                        proceso AS PROCESO,
                        p_mensaje AS MENSAJE
                    FROM  TMCR_CLIENTE
                    WHERE
                        CLIID = maxid;
            dbms_sql.return_result(rf_cursor_leer);

        END IF;

    END IF;

	-------------
	-- ACTUALIZAR:
	-------------
    IF(proceso = 3) THEN

		-- Cerrar el cursor si sigue abierto:
        IF ( rf_cursor_leer%isopen ) THEN
            CLOSE rf_cursor_leer;
        END IF;

        -- Determinar si el registro existe:
        SELECT
            count(*) INTO p_numRegistros
        FROM  TMCR_CLIENTE
        WHERE
            UPPER(CLITIPODOCUMENTO) = UPPER(tipoDocumento)
            AND UPPER(CLINRODOCUMENTO) = UPPER(nroDocumento);
        dbms_output.put_line(' NUMERO REGISTROS --> ' || p_numRegistros);

        --  SI HAY UN REGISTRADO EJECUTAR ACTUALIZACION DEL REGISTRO
        IF(p_numRegistros > 0) THEN

            UPDATE TMCR_CLIENTE
            SET
                CLITIPODOCUMENTO = tipoDocumento,
				CLINRODOCUMENTO = nroDocumento,
                CLINOMBRECOMPLETO = nombreCompleto,
                CLITELEFONO = telefono,
                CLIDIRECCION = direccion,
                CLIEMAIL = email,
                CLIOBSERVACIONES = observaciones,
                CLIESTADO = estado                
            WHERE
				CLIID = id;
            COMMIT;

            --p_mensaje := 'UPDATE - REGISTRO ACTUALIZADO';
            SELECT
                mviestado ||': ' || mvimensaje
                INTO p_mensaje
            FROM TMCR_MENSAJESVISUALIZACION
                WHERE MVIPROCESO = proceso AND MVIESTADO = 'Exitoso';
           
            OPEN rf_cursor_leer FOR
                SELECT
                        CLIID,
                        CLITIPODOCUMENTO,
                        CLINRODOCUMENTO,
                        CLINOMBRECOMPLETO,
                        CLITELEFONO, 
                        CLIDIRECCION,
                        CLIEMAIL,
                        CLIOBSERVACIONES,
                        CLIESTADO,
                        CLIFECHAREGISTRO,    
                        CLISUARIOCREACION,
                        proceso AS PROCESO,
                        p_mensaje AS MENSAJE
                    FROM  TMCR_CLIENTE
                    WHERE
                        CLIID = id;
            dbms_sql.return_result(rf_cursor_leer);

        -- NO EXISTE UN REGISTRO CON ESA KEY, NO PUEDE PROCEDER A ACTUALIZAR
        ELSE
            RAISE exc_NoData;
        END IF;

    END IF;

	------------
	-- ELIMINAR:
	------------
    IF(proceso = 4) THEN

		-- Cerrar el cursor si sigue abierto:
        IF ( rf_cursor_leer%isopen ) THEN
            CLOSE rf_cursor_leer;
        END IF;

        -- Determinar si el registro existe:
        SELECT
            count(*) INTO p_numRegistros
        FROM  TMCR_CLIENTE
        WHERE
            CLIID = id;

        --  SI HAY UN REGISTRADO EJECUTAR ACTUALIZACION DEL REGISTRO
        IF(p_numRegistros > 0) THEN

            UPDATE TMCR_VEHICULO
            SET
                VEHESTADO = 'INACTIVO'
            WHERE
				VEHID = id;
            COMMIT;

            -- p_mensaje := 'DELETE - REGISTRO INACTIVADO';
            SELECT
                mviestado ||': ' || mvimensaje
                INTO p_mensaje
            FROM TMCR_MENSAJESVISUALIZACION
                WHERE MVIPROCESO = proceso AND MVIESTADO = 'Exitoso' AND MVIABREVIATURA = 'INACTIVO';
            
            OPEN rf_cursor_leer FOR
                SELECT
                        CLIID,
                        CLITIPODOCUMENTO,
                        CLINRODOCUMENTO,
                        CLINOMBRECOMPLETO,
                        CLITELEFONO, 
                        CLIDIRECCION,
                        CLIEMAIL,
                        CLIOBSERVACIONES,
                        CLIESTADO,
                        CLIFECHAREGISTRO,    
                        CLISUARIOCREACION,
                        proceso AS PROCESO,
                        p_mensaje AS MENSAJE
                    FROM  TMCR_CLIENTE
                    WHERE
                        CLIID = id;
            dbms_sql.return_result(rf_cursor_leer);

        -- NO EXISTE UN REGISTRO CON ESA KEY, NO SE PUEDE PROCEDER A INACTIVAR
        ELSE
            RAISE exc_NoData;
        END IF;

    END IF;

    -------------------
	-- BUSQUEDA TODOS:
	-------------------
    IF(proceso = 5) THEN

        --p_mensaje := 'BUSQUEDA - LISTAR TODOS';
        SELECT
                mviestado ||': ' || mvimensaje
                INTO p_mensaje
            FROM TMCR_MENSAJESVISUALIZACION
                WHERE MVIPROCESO = proceso AND MVIESTADO = 'Exitoso';

        OPEN rf_cursor_leer FOR
                SELECT
                        CLIID,
                        CLITIPODOCUMENTO,
                        CLINRODOCUMENTO,
                        CLINOMBRECOMPLETO,
                        CLITELEFONO, 
                        CLIDIRECCION,
                        CLIEMAIL,
                        CLIOBSERVACIONES,
                        CLIESTADO,
                        CLIFECHAREGISTRO,    
                        CLISUARIOCREACION,
                        proceso AS PROCESO,
                        p_mensaje AS MENSAJE
                    FROM  TMCR_CLIENTE;
        dbms_sql.return_result(rf_cursor_leer);

    END IF;

    ---------------------
	-- BUSQUEDA GENERICA:
	---------------------
    IF(proceso = 6) THEN

        SELECT
            count(*) INTO p_numRegistros
        FROM  TMCR_CLIENTE
        WHERE
            UPPER(CLITIPODOCUMENTO) LIKE '%' || UPPER(tipoDocumento) || '%'
			OR UPPER(CLINRODOCUMENTO) LIKE '%' || UPPER(nroDocumento) || '%'
            OR UPPER(CLINOMBRECOMPLETO) LIKE '%' || UPPER(nombreCompleto) || '%'
            OR UPPER(CLITELEFONO) LIKE '%' || UPPER(telefono) || '%'
            OR UPPER(CLIDIRECCION) LIKE '%' || UPPER(direccion) || '%'
            OR UPPER(CLIEMAIL) LIKE '%' || UPPER(email) || '%'
            OR UPPER(CLIOBSERVACIONES) LIKE '%' || UPPER(observaciones) || '%'
            OR UPPER(CLIESTADO) LIKE '%' || UPPER(estado) || '%';

        --  SI HAY UN REGISTRADO EN LA BUSQUEDA
        IF(p_numRegistros > 0) THEN

            p_mensaje := 'BUSCAR GENERICO - REGISTRO ENCONTRADOS';

            OPEN rf_cursor_leer FOR
                SELECT
                        CLIID,
                        CLITIPODOCUMENTO,
                        CLINRODOCUMENTO,
                        CLINOMBRECOMPLETO,
                        CLITELEFONO, 
                        CLIDIRECCION,
                        CLIEMAIL,
                        CLIOBSERVACIONES,
                        CLIESTADO,
                        CLIFECHAREGISTRO,    
                        CLISUARIOCREACION,
                        proceso AS PROCESO,
                        p_mensaje AS MENSAJE
                    FROM  TMCR_CLIENTE
                    WHERE
                        UPPER(CLITIPODOCUMENTO) LIKE '%' || UPPER(tipoDocumento) || '%'
                        OR UPPER(CLINRODOCUMENTO) LIKE '%' || UPPER(nroDocumento) || '%'
                        OR UPPER(CLINOMBRECOMPLETO) LIKE '%' || UPPER(nombreCompleto) || '%'
                        OR UPPER(CLITELEFONO) LIKE '%' || UPPER(telefono) || '%'
                        OR UPPER(CLIDIRECCION) LIKE '%' || UPPER(direccion) || '%'
                        OR UPPER(CLIEMAIL) LIKE '%' || UPPER(email) || '%'
                        OR UPPER(CLIOBSERVACIONES) LIKE '%' || UPPER(observaciones) || '%'
                        OR UPPER(CLIESTADO) LIKE '%' || UPPER(estado) || '%';
                        dbms_sql.return_result(rf_cursor_leer);

        -- NO EXISTE REGISTROS EN LA BUSQIEDA
        ELSE
            RAISE exc_NoData;
        END IF;

    END IF;

    ---------------
	-- EXCEPCIONES:
	---------------
    EXCEPTION
		WHEN exc_NoData THEN

			IF ( rf_cursor_leer%isopen ) THEN
				CLOSE rf_cursor_leer;
			END IF;

            SELECT
                mviestado ||': ' || mvimensaje
                INTO p_mensaje
            FROM TMCR_MENSAJESVISUALIZACION
                WHERE MVIPROCESO = 6 AND MVIESTADO = 'Fallo';

			OPEN rf_cursor_leer FOR
                SELECT
                    0 AS      CLIID,
                    '' AS     CLITIPODOCUMENTO,
                    '' AS     CLINRODOCUMENTO,
                    '' AS     CLINOMBRECOMPLETO,
                    '' AS     CLITELEFONO, 
                    '' AS     CLIDIRECCION,
                    '' AS     CLIEMAIL,
                    '' AS     CLIOBSERVACIONES,
                    '' AS     CLIESTADO,
                    '' AS     CLIFECHAREGISTRO,    
                    '' AS     CLISUARIOCREACION,
                    proceso AS PROCESO,
                    p_mensaje AS MENSAJE
                FROM  DUAL;
            dbms_sql.return_result(rf_cursor_leer);

			IF ( rf_cursor_leer%isopen ) THEN
				CLOSE rf_cursor_leer;
            END IF;
END;