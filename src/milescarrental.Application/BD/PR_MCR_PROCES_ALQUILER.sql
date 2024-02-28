create or replace PROCEDURE "PR_MCR_PROCES_ALQUILER" (
    id                  TMCR_ALQUILER.ALQID%TYPE,
    idCliente           TMCR_ALQUILER.ALQIDCLIENTE%TYPE,
    idVehiculo          TMCR_ALQUILER.ALQIDVEHICULO%TYPE,
    fechaInicio         TMCR_ALQUILER.ALQFECHAINICIO%TYPE,
    fechaFin            TMCR_ALQUILER.ALQFECHAFIN%TYPE,
    kilometrajeInicial  TMCR_ALQUILER.ALQKILOMETRAJEINICIAL%TYPE,
    kilometrajeFinal    TMCR_ALQUILER.ALQKILOMETRAJEFINAL%TYPE,
    locLatitudVehiculo  TMCR_ALQUILER.ALQLOCLATITUDVEHICULO%TYPE,
    locLongitudVehiculo TMCR_ALQUILER.ALQLOCLONGITUDVEHICULO%TYPE,
    locLatitudCliente   TMCR_ALQUILER.ALQLOCLATITUDCLIENTE%TYPE,
    locLongitudCliente  TMCR_ALQUILER.ALQLOCLONGITUDCLIENTE%TYPE,
    costoAlquiler        TMCR_ALQUILER.ALQCOSTO%TYPE,
    estado              TMCR_ALQUILER.ALQESTADO%TYPE,
    observaciones       TMCR_ALQUILER.ALQUSUARIOCREACION%TYPE,
    usuario             TMCR_ALQUILER.ALQUSUARIOCREACION%TYPE,
    proceso IN number  --> Busqueda Exacta(1), Insertar(2), Actualizar(3), Borrar(4) Registro, Listar todos(5), Busqueda Generica(6)
) AS

    -- Parametros de control:
    p_numRegistros NUMBER:=0;
    p_mensaje VARCHAR2(255 BYTE):='EN EJECUCION'; -- Se coloca el mensaje de respuesta de la transacion
	exc_NoData EXCEPTION;

    maxid TMCR_ALQUILER.ALQID%TYPE; -- Maximo ID, extraer el consecutivo para la insercion

    rf_cursor_leer SYS_REFCURSOR; -- Cursor que se va a retornar
    
BEGIN
    -- Determinar si el registro existe:
        SELECT
            count(*) INTO p_numRegistros
        FROM  TMCR_ALQUILER
        WHERE
            ALQID = id;
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
        FROM  TMCR_ALQUILER
        WHERE
            ALQID = id;
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
                        ALQID,
                        ALQIDCLIENTE,
                        ALQIDVEHICULO,
                        ALQFECHAINICIO,
                        ALQFECHAFIN, 
                        ALQKILOMETRAJEINICIAL,
                        ALQKILOMETRAJEFINAL,
                        ALQLOCLATITUDVEHICULO,
                        ALQLOCLONGITUDVEHICULO,
                        ALQLOCLATITUDCLIENTE,
                        ALQLOCLONGITUDCLIENTE,
                        ALQCOSTO,
                        ALQESTADO,
                        ALQOBSERVACIONES,
                        ALQFECHAREGISTRO,
                        ALQUSUARIOCREACION,
                        proceso AS PROCESO,
                        p_mensaje AS MENSAJE
                    FROM  TMCR_ALQUILER
                    WHERE
                        ALQID = id;
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
        
        -- Regla para la primer insercion:
            IF(p_numRegistros = 0) THEN
                SELECT
                    COUNT(*) + 1
                    INTO maxid
                FROM TMCR_ALQUILER;
            ELSE
                SELECT
                    MAX(nvl(ALQID, 0)) + 1
                    INTO maxid
                FROM TMCR_ALQUILER;
            END IF;            

            INSERT INTO TMCR_ALQUILER(
                        ALQID,
                        ALQIDCLIENTE,
                        ALQIDVEHICULO,
                        ALQFECHAINICIO,
                        ALQFECHAFIN, 
                        ALQKILOMETRAJEINICIAL,
                        ALQKILOMETRAJEFINAL,
                        ALQLOCLATITUDVEHICULO,
                        ALQLOCLONGITUDVEHICULO,
                        ALQLOCLATITUDCLIENTE,
                        ALQLOCLONGITUDCLIENTE,
                        ALQCOSTO,
                        ALQESTADO,
                        ALQOBSERVACIONES,
                        ALQFECHAREGISTRO,
                        ALQUSUARIOCREACION)
            VALUES(
                        maxid, 
                        idCliente, 
                        idVehiculo, 
                        fechaInicio, 
                        fechaFin, 
                        kilometrajeInicial, 
                        kilometrajeFinal, 
                        locLatitudVehiculo, 
                        locLongitudVehiculo, 
                        locLatitudCliente, 
                        locLongitudCliente, 
                        costoAlquiler, 
                        estado, 
                        observaciones, 
                        SYSDATE(), 
                        usuario);
            COMMIT;            
            
            -- p_mensaje := 'INSERT - REGISTRO INSERTADO';
            SELECT
                mviestado ||': ' || mvimensaje
                INTO p_mensaje
            FROM TMCR_MENSAJESVISUALIZACION
                WHERE MVIPROCESO = proceso AND MVIESTADO = 'Exitoso';

            OPEN rf_cursor_leer FOR
                SELECT
                        ALQID,
                        ALQIDCLIENTE,
                        ALQIDVEHICULO,
                        ALQFECHAINICIO,
                        ALQFECHAFIN, 
                        ALQKILOMETRAJEINICIAL,
                        ALQKILOMETRAJEFINAL,
                        ALQLOCLATITUDVEHICULO,
                        ALQLOCLONGITUDVEHICULO,
                        ALQLOCLATITUDCLIENTE,
                        ALQLOCLONGITUDCLIENTE,
                        ALQCOSTO,
                        ALQESTADO,
                        ALQOBSERVACIONES,
                        ALQFECHAREGISTRO,
                        ALQUSUARIOCREACION,
                        proceso AS PROCESO,
                        p_mensaje AS MENSAJE
                    FROM  TMCR_ALQUILER
                    WHERE
                        ALQID = maxid;
            dbms_sql.return_result(rf_cursor_leer);

    END IF;

	-------------
	-- ACTUALIZAR:
	-------------
    IF(proceso = 3) THEN

		-- Cerrar el cursor si sigue abierto:
        IF ( rf_cursor_leer%isopen ) THEN
            CLOSE rf_cursor_leer;
        END IF;

        UPDATE TMCR_ALQUILER
            SET
                ALQIDCLIENTE = idCliente,
                ALQIDVEHICULO = idVehiculo,
                ALQFECHAINICIO = fechaInicio,
                ALQFECHAFIN = fechaFin, 
                ALQKILOMETRAJEINICIAL = kilometrajeInicial,
                ALQKILOMETRAJEFINAL = kilometrajeFinal,
                ALQLOCLATITUDVEHICULO = locLatitudVehiculo,
                ALQLOCLONGITUDVEHICULO = locLongitudVehiculo,
                ALQLOCLATITUDCLIENTE = locLatitudCliente,
                ALQLOCLONGITUDCLIENTE = locLongitudCliente,
                ALQCOSTO = costoAlquiler,
                ALQESTADO = estado,
                ALQUSUARIOCREACION = usuario               
            WHERE
				ALQID = id;
            COMMIT;

            --p_mensaje := 'UPDATE - REGISTRO ACTUALIZADO';
            SELECT
                mviestado ||': ' || mvimensaje
                INTO p_mensaje
            FROM TMCR_MENSAJESVISUALIZACION
                WHERE MVIPROCESO = proceso AND MVIESTADO = 'Exitoso';
           
            OPEN rf_cursor_leer FOR
                SELECT
                        ALQID,
                        ALQIDCLIENTE,
                        ALQIDVEHICULO,
                        ALQFECHAINICIO,
                        ALQFECHAFIN, 
                        ALQKILOMETRAJEINICIAL,
                        ALQKILOMETRAJEFINAL,
                        ALQLOCLATITUDVEHICULO,
                        ALQLOCLONGITUDVEHICULO,
                        ALQLOCLATITUDCLIENTE,
                        ALQLOCLONGITUDCLIENTE,
                        ALQCOSTO,
                        ALQESTADO,
                        ALQOBSERVACIONES,
                        ALQFECHAREGISTRO,
                        ALQUSUARIOCREACION,
                        proceso AS PROCESO,
                        p_mensaje AS MENSAJE
                    FROM  TMCR_ALQUILER
                    WHERE
                        ALQID = id;
            dbms_sql.return_result(rf_cursor_leer);

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
        FROM  TMCR_ALQUILER
        WHERE
            ALQID = id;

        --  SI HAY UN REGISTRADO EJECUTAR ACTUALIZACION DEL REGISTRO
        IF(p_numRegistros > 0) THEN

            UPDATE TMCR_ALQUILER
            SET
                ALQESTADO = 'INACTIVO'
            WHERE
				ALQID = id;
            COMMIT;

            -- p_mensaje := 'DELETE - REGISTRO INACTIVADO';
            SELECT
                mviestado ||': ' || mvimensaje
                INTO p_mensaje
            FROM TMCR_MENSAJESVISUALIZACION
                WHERE MVIPROCESO = proceso AND MVIESTADO = 'Exitoso' AND MVIABREVIATURA = 'INACTIVO';
            
            OPEN rf_cursor_leer FOR
                SELECT
                        ALQID,
                        ALQIDCLIENTE,
                        ALQIDVEHICULO,
                        ALQFECHAINICIO,
                        ALQFECHAFIN, 
                        ALQKILOMETRAJEINICIAL,
                        ALQKILOMETRAJEFINAL,
                        ALQLOCLATITUDVEHICULO,
                        ALQLOCLONGITUDVEHICULO,
                        ALQLOCLATITUDCLIENTE,
                        ALQLOCLONGITUDCLIENTE,
                        ALQCOSTO,
                        ALQESTADO,
                        ALQOBSERVACIONES,
                        ALQFECHAREGISTRO,
                        ALQUSUARIOCREACION,
                        proceso AS PROCESO,
                        p_mensaje AS MENSAJE
                    FROM  TMCR_ALQUILER
                    WHERE
                        ALQID = id;
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
                        ALQID,
                        ALQIDCLIENTE,
                        ALQIDVEHICULO,
                        ALQFECHAINICIO,
                        ALQFECHAFIN, 
                        ALQKILOMETRAJEINICIAL,
                        ALQKILOMETRAJEFINAL,
                        ALQLOCLATITUDVEHICULO,
                        ALQLOCLONGITUDVEHICULO,
                        ALQLOCLATITUDCLIENTE,
                        ALQLOCLONGITUDCLIENTE,
                        ALQCOSTO,
                        ALQESTADO,
                        ALQOBSERVACIONES,
                        ALQFECHAREGISTRO,
                        ALQUSUARIOCREACION,
                        proceso AS PROCESO,
                        p_mensaje AS MENSAJE
                    FROM  TMCR_ALQUILER;
        dbms_sql.return_result(rf_cursor_leer);

    END IF;

    ---------------------
	-- BUSQUEDA GENERICA:
	---------------------
    IF(proceso = 6) THEN

        SELECT
            count(*) INTO p_numRegistros
        FROM  TMCR_ALQUILER
        WHERE
            ALQIDCLIENTE = idCliente 
			OR ALQIDVEHICULO = idVehiculo;

        --  SI HAY UN REGISTRADO EN LA BUSQUEDA
        IF(p_numRegistros > 0) THEN

            p_mensaje := 'BUSCAR GENERICO - REGISTRO ENCONTRADOS';

            OPEN rf_cursor_leer FOR
                SELECT
                    ALQID,
                    ALQIDCLIENTE,
                    ALQIDVEHICULO,
                    ALQFECHAINICIO,
                    ALQFECHAFIN, 
                    ALQKILOMETRAJEINICIAL,
                    ALQKILOMETRAJEFINAL,
                    ALQLOCLATITUDVEHICULO,
                    ALQLOCLONGITUDVEHICULO,
                    ALQLOCLATITUDCLIENTE,
                    ALQLOCLONGITUDCLIENTE,
                    ALQCOSTO,
                    ALQESTADO,
                    ALQOBSERVACIONES,
                    ALQFECHAREGISTRO,
                    ALQUSUARIOCREACION,
                    proceso AS PROCESO,
                    p_mensaje AS MENSAJE
                FROM  TMCR_ALQUILER
                WHERE
                    ALQIDCLIENTE = idCliente 
                    OR ALQIDVEHICULO = idVehiculo;

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
                    0 AS ALQID,
                    0 AS ALQIDCLIENTE,
                    0 AS ALQIDVEHICULO,
                    '' AS ALQFECHAINICIO,
                    '' AS ALQFECHAFIN, 
                    0 AS ALQKILOMETRAJEINICIAL,
                    0 AS ALQKILOMETRAJEFINAL,
                    '' AS ALQLOCLATITUDVEHICULO,
                    '' AS ALQLOCLONGITUDVEHICULO,
                    '' AS ALQLOCLATITUDCLIENTE,
                    '' AS ALQLOCLONGITUDCLIENTE,
                    0 AS ALQCOSTO,
                    '' AS ALQESTADO,
                    '' AS ALQOBSERVACIONES,
                    '' AS ALQFECHAREGISTRO,
                    '' AS ALQUSUARIOCREACION,
                    proceso AS PROCESO,
                    p_mensaje AS MENSAJE
                FROM  DUAL;
            dbms_sql.return_result(rf_cursor_leer);

			IF ( rf_cursor_leer%isopen ) THEN
				CLOSE rf_cursor_leer;
            END IF;
END;